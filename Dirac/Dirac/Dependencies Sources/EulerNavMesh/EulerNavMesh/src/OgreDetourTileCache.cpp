#pragma once
#include "OgreDetourTileCache.h"
#include "InputGeom.h"
#include "Recast/Recast.h"
#include "DetourTileCache/DetourTileCache.h"
//#include "../DetourWrapper.h"
#include <float.h>

using namespace std;

//esto de aca abajo es para el save.
static const int NAVMESHSET_MAGIC = 'M'<<24 | 'S'<<16 | 'E'<<8 | 'T'; //'MSET';
static const int NAVMESHSET_VERSION = 1;

struct NavMeshSetHeader
{
	int magic;
	int version;
	int numTiles;
	dtNavMeshParams params;
};

struct NavMeshTileHeader
{
	dtTileRef tileRef;
	int dataSize;
};


// Max number of layers a tile can have
const int OgreDetourTileCache::EXPECTED_LAYERS_PER_TILE = 4;

// Max number of (temp) obstacles that can be added to the tilecache
const int OgreDetourTileCache::MAX_OBSTACLES = 128;

// Extra padding added to the border size of tiles (together with agent radius)
const float OgreDetourTileCache::BORDER_PADDING = 3;

// Set to false to disable debug drawing. Improves performance.
bool OgreDetourTileCache::DEBUG_DRAW = true;

// Set to true to draw bounding boxes around rebuilt tiles.
bool OgreDetourTileCache::DEBUG_DRAW_REBUILT_BB = false;

OgreDetourTileCache::OgreDetourTileCache(OgreRecast *recast, int tileSize)
    : m_recast(recast),
      m_tileSize(tileSize - (tileSize%8)),  // Make sure tilesize is a multiple of 8
      m_keepInterResults(false),
      m_tileCache(0),
      m_cacheBuildTimeMs(0),
      m_cacheCompressedSize(0),
      m_cacheRawSize(0),
      m_cacheLayerCount(0),
      m_cacheBuildMemUsage(0),
      m_maxTiles(0),
      m_maxPolysPerTile(0),
      m_cellSize(0),
      m_tcomp(0),
      m_geom(0),
      m_th(0),
      m_tw(0),
      mChangedConvexVolumesCount(0)
{
    m_talloc = new LinearAllocator(32000);
    m_tcomp = new FastLZCompressor;
    m_tmproc = new MeshProcess;

    // Sanity check on tilesize
    if(m_tileSize < 16 || m_tileSize > 128)
        m_tileSize = 48;
}

OgreDetourTileCache::~OgreDetourTileCache()
{
	delete m_tileCache;
	//delete m_ctx;
	delete m_talloc;
	delete m_tcomp;
	delete m_tmproc;
	//delete m_recast;
}

bool OgreDetourTileCache::TileCacheBuild(InputGeom *inputGeom)
{
    // Init configuration for specified geometry
    configure(inputGeom);

    dtStatus status;

    // Preprocess tiles.
    // Prepares navmesh tiles in a 2D intermediary format that allows quick conversion to a 3D navmesh

//    ctx->resetTimers();

    m_cacheLayerCount = 0;
    m_cacheCompressedSize = 0;
    m_cacheRawSize = 0;

    for (int y = 0; y < m_th; ++y)
    {
        for (int x = 0; x < m_tw; ++x)
        {
            TileCacheData tiles[MAX_LAYERS];
            memset(tiles, 0, sizeof(tiles));
            int ntiles = rasterizeTileLayers(m_geom, x, y, m_cfg, tiles, MAX_LAYERS);  // This is where the tile is built

            for (int i = 0; i < ntiles; ++i)
            {
                TileCacheData* tile = &tiles[i];
                status = m_tileCache->addTile(tile->data, tile->dataSize, DT_COMPRESSEDTILE_FREE_DATA, 0);  // Add compressed tiles to tileCache
                if (dtStatusFailed(status))
                {
                    dtFree(tile->data);
                    tile->data = 0;
                    continue;
                }

                m_cacheLayerCount++;
                m_cacheCompressedSize += tile->dataSize;
                m_cacheRawSize += calcLayerBufferSize(m_tcparams.width, m_tcparams.height);
            }
        }
    }

    // Build initial meshes
    // Builds detour compatible navmesh from all tiles.
    // A tile will have to be rebuilt if something changes, eg. a temporary obstacle is placed on it.
//    ctx->startTimer(RC_TIMER_TOTAL);
    for (int y = 0; y < m_th; ++y)
        for (int x = 0; x < m_tw; ++x)
            m_tileCache->buildNavMeshTilesAt(x,y, m_recast->m_navMesh); // This immediately builds the tile, without the need of a dtTileCache::update()
//    ctx->stopTimer(RC_TIMER_TOTAL);

//    m_cacheBuildTimeMs = ctx->getAccumulatedTime(RC_TIMER_TOTAL)/1000.0f;
    m_cacheBuildMemUsage = m_talloc->high;


    // Count the total size of all generated tiles of the tiled navmesh
    const dtNavMesh* nav = m_recast->m_navMesh;
    int navmeshMemUsage = 0;
    for (int i = 0; i < nav->getMaxTiles(); ++i)
    {
        const dtMeshTile* tile = nav->getTile(i);
        if (tile->header)
            navmeshMemUsage += tile->dataSize;
    }



//    printf("navmeshMemUsage = %.1f kB\n", navmeshMemUsage/1024.0f);
    LogAdd("Navmesh Mem Usage = "/*+ Ogre::StringConverter::toString(navmeshMemUsage/1024.0f) +" kB"*/);
    LogAdd("Tilecache Mem Usage = "/* +Ogre::StringConverter::toString(m_cacheCompressedSize/1024.0f) +" kB"*/);


    return true;
}

bool OgreDetourTileCache::configure(InputGeom *inputGeom)
{
    m_geom = inputGeom;

    // Reuse OgreRecast context for tiled navmesh building
    m_ctx = m_recast->m_ctx;

    if (!m_geom || m_geom->isEmpty()) {
        LogAdd("ERROR: OgreDetourTileCache::configure: No vertices and triangles.");
        return false;
    }

    if (!m_geom->getChunkyMesh()) {
        LogAdd("ERROR: OgreDetourTileCache::configure: Input mesh has no chunkyTriMesh built.");
        return false;
    }

    m_tmproc->init(m_geom);


    // Init cache bounding box
    const float* bmin = m_geom->getMeshBoundsMin();
    const float* bmax = m_geom->getMeshBoundsMax();

    // Navmesh generation params.
    // Use config from recast module
    m_cfg = m_recast->m_cfg;

    // Most params are taken from OgreRecast::configure, except for these:
    m_cfg.tileSize = m_tileSize;
    m_cfg.borderSize = (int) (m_cfg.walkableRadius + BORDER_PADDING); // Reserve enough padding.
    m_cfg.width = m_cfg.tileSize + m_cfg.borderSize*2;
    m_cfg.height = m_cfg.tileSize + m_cfg.borderSize*2;

    // Set mesh bounds
    rcVcopy(m_cfg.bmin, bmin);
    rcVcopy(m_cfg.bmax, bmax);
    // Also define navmesh bounds in recast component
    rcVcopy(m_recast->m_cfg.bmin, bmin);
    rcVcopy(m_recast->m_cfg.bmax, bmax);

    // Cell size navmesh generation property is copied from OgreRecast config
    m_cellSize = m_cfg.cs;

    // Determine grid size (number of tiles) based on bounding box and grid cell size
    int gw = 0, gh = 0;
    rcCalcGridSize(bmin, bmax, m_cellSize, &gw, &gh);   // Calculates total size of voxel grid
    const int ts = m_tileSize;
    const int tw = (gw + ts-1) / ts;    // Tile width
    const int th = (gh + ts-1) / ts;    // Tile height
    m_tw = tw;
    m_th = th;
	LogAdd("aa");
    LogAdd("Total Voxels: %f, x %f " /*+ (gw) + (gh)*/);
    LogAdd("Tilesize: %d, Cellsize %d "/*, (m_tileSize), (m_cellSize)*/);
    LogAdd("Tiles: %f, x %f"/*, (m_tw), (m_th)*/);


    // Max tiles and max polys affect how the tile IDs are caculated.
    // There are 22 bits available for identifying a tile and a polygon.
    int tileBits = rcMin((int)dtIlog2(dtNextPow2(tw*th*EXPECTED_LAYERS_PER_TILE)), 14);
    if (tileBits > 14) tileBits = 14;
    int polyBits = 22 - tileBits;
    m_maxTiles = 1 << tileBits;
    m_maxPolysPerTile = 1 << polyBits;
    LogAdd("Max Tiles"/*, (m_maxTiles)*/);
    LogAdd("Max Polys: %d "/*, (m_maxPolysPerTile)*/);


    // Tile cache params.
    memset(&m_tcparams, 0, sizeof(m_tcparams));
    rcVcopy(m_tcparams.orig, bmin);
    m_tcparams.width = m_tileSize;
    m_tcparams.height = m_tileSize;
    m_tcparams.maxTiles = tw*th*EXPECTED_LAYERS_PER_TILE;
    m_tcparams.maxObstacles = MAX_OBSTACLES;    // Max number of temp obstacles that can be added to or removed from navmesh

    // Copy the rest of the parameters from OgreRecast config
    m_tcparams.cs = m_cfg.cs;
    m_tcparams.ch = m_cfg.ch;
    m_tcparams.walkableHeight = (float) m_cfg.walkableHeight;
    m_tcparams.walkableRadius = (float) m_cfg.walkableRadius;
    m_tcparams.walkableClimb = (float) m_cfg.walkableClimb;
    m_tcparams.maxSimplificationError = m_cfg.maxSimplificationError;

    return initTileCache();
}

bool OgreDetourTileCache::initTileCache()
{
    // BUILD TileCache
    dtFreeTileCache(m_tileCache);

    dtStatus status;

    m_tileCache = dtAllocTileCache();
    if (!m_tileCache)
    {
        LogAdd("ERROR: buildTiledNavigation: Could not allocate tile cache.");
        return false;
    }
    status = m_tileCache->init(&m_tcparams, m_talloc, m_tcomp, m_tmproc);
    if (dtStatusFailed(status))
    {
        LogAdd("ERROR: buildTiledNavigation: Could not init tile cache.");
        return false;
    }

    dtFreeNavMesh(m_recast->m_navMesh);

    m_recast->m_navMesh = dtAllocNavMesh();
    if (!m_recast->m_navMesh)
    {
        LogAdd("ERROR: buildTiledNavigation: Could not allocate navmesh.");
        return false;
    }


    // Init multi-tile navmesh parameters
    dtNavMeshParams params;
    memset(&params, 0, sizeof(params));
    rcVcopy(params.orig, m_tcparams.orig);   // Set world-space origin of tile grid
    params.tileWidth = m_tileSize*m_tcparams.cs;
    params.tileHeight = m_tileSize*m_tcparams.cs;
    params.maxTiles = m_maxTiles;
    params.maxPolys = m_maxPolysPerTile;

    status = m_recast->m_navMesh->init(&params);
    if (dtStatusFailed(status))
    {
        LogAdd("ERROR: buildTiledNavigation: Could not init navmesh.");
        return false;
    }

    // Init recast navmeshquery with created navmesh (in OgreRecast component)
    m_recast->m_navQuery = dtAllocNavMeshQuery();
    status = m_recast->m_navQuery->init(m_recast->m_navMesh, 2048);
    if (dtStatusFailed(status))
    {
        LogAdd("ERROR: buildTiledNavigation: Could not init Detour navmesh query");
        return false;
    }

    return true;
}

int OgreDetourTileCache::rasterizeTileLayers(InputGeom* geom, const int tx, const int ty, const rcConfig& cfg, TileCacheData* tiles, const int maxTiles)
{
    if (!geom || geom->isEmpty()) {
        LogAdd("ERROR: buildTile: Input mesh is not specified.");
        return 0;
    }

    if (!geom->getChunkyMesh()) {
        LogAdd("ERROR: buildTile: Input mesh has no chunkyTriMesh built.");
        return 0;
    }

//TODO make these member variables?
    FastLZCompressor comp;
    RasterizationContext rc;

    const float* verts = geom->getVerts();
    const int nverts = geom->getVertCount();

    // The chunky tri mesh in the inputgeom is a simple spatial subdivision structure that allows to
    // process the vertices in the geometry relevant to this part of the tile.
    // The chunky tri mesh is a grid of axis aligned boxes that store indices to the vertices in verts
    // that are positioned in that box.
    const rcChunkyTriMesh* chunkyMesh = geom->getChunkyMesh();

    // Tile bounds.
    float tcs = m_tileSize * m_cellSize;

    rcConfig tcfg;
    memcpy(&tcfg, &m_cfg, sizeof(tcfg));
    tcfg.bmin[0] = m_cfg.bmin[0] + tx*tcs;
    tcfg.bmin[1] = m_cfg.bmin[1];
    tcfg.bmin[2] = m_cfg.bmin[2] + ty*tcs;
    tcfg.bmax[0] = m_cfg.bmin[0] + (tx+1)*tcs;
    tcfg.bmax[1] = m_cfg.bmax[1];
    tcfg.bmax[2] = m_cfg.bmin[2] + (ty+1)*tcs;
    tcfg.bmin[0] -= tcfg.borderSize*tcfg.cs;
    tcfg.bmin[2] -= tcfg.borderSize*tcfg.cs;
    tcfg.bmax[0] += tcfg.borderSize*tcfg.cs;
    tcfg.bmax[2] += tcfg.borderSize*tcfg.cs;


    // This is part of the regular recast navmesh generation pipeline as in OgreRecast::NavMeshBuild()
    // but only up till step 4 and slightly modified.


    // Allocate voxel heightfield where we rasterize our input data to.
    rc.solid = rcAllocHeightfield();
    if (!rc.solid)
    {
       LogAdd("ERROR: buildNavigation: Out of memory 'solid'.");
        return 0;
    }
    if (!rcCreateHeightfield(m_ctx, *rc.solid, tcfg.width, tcfg.height, tcfg.bmin, tcfg.bmax, tcfg.cs, tcfg.ch))
    {
        LogAdd("ERROR: buildNavigation: Could not create solid heightfield.");
        return 0;
    }

    // Allocate array that can hold triangle flags.
    // If you have multiple meshes you need to process, allocate
    // an array which can hold the max number of triangles you need to process.
    rc.triareas = new unsigned char[chunkyMesh->maxTrisPerChunk];
    if (!rc.triareas)
    {
        LogAdd("ERROR: buildNavigation: Out of memory 'm_triareas' (  )");
        return 0;
    }

    float tbmin[2], tbmax[2];
    tbmin[0] = tcfg.bmin[0];
    tbmin[1] = tcfg.bmin[2];
    tbmax[0] = tcfg.bmax[0];
    tbmax[1] = tcfg.bmax[2];
    int cid[512];// TODO: Make grow when returning too many items.
    const int ncid = rcGetChunksOverlappingRect(chunkyMesh, tbmin, tbmax, cid, 512);
    if (!ncid)
    {
        return 0; // empty
    }

    for (int i = 0; i < ncid; ++i)
    {
        const rcChunkyTriMeshNode& node = chunkyMesh->nodes[cid[i]];
        const int* tris = &chunkyMesh->tris[node.i*3];
        const int ntris = node.n;

        memset(rc.triareas, 0, ntris*sizeof(unsigned char));
        rcMarkWalkableTriangles(m_ctx, tcfg.walkableSlopeAngle,
                                verts, nverts, tris, ntris, rc.triareas);

        rcRasterizeTriangles(m_ctx, verts, nverts, tris, rc.triareas, ntris, *rc.solid, tcfg.walkableClimb);
    }

    // Once all geometry is rasterized, we do initial pass of filtering to
    // remove unwanted overhangs caused by the conservative rasterization
    // as well as filter spans where the character cannot possibly stand.
    rcFilterLowHangingWalkableObstacles(m_ctx, tcfg.walkableClimb, *rc.solid);
    rcFilterLedgeSpans(m_ctx, tcfg.walkableHeight, tcfg.walkableClimb, *rc.solid);
    rcFilterWalkableLowHeightSpans(m_ctx, tcfg.walkableHeight, *rc.solid);


    rc.chf = rcAllocCompactHeightfield();
    if (!rc.chf)
    {
       LogAdd("ERROR: buildNavigation: Out of memory 'chf'.");
        return 0;
    }
    if (!rcBuildCompactHeightfield(m_ctx, tcfg.walkableHeight, tcfg.walkableClimb, *rc.solid, *rc.chf))
    {
        LogAdd("ERROR: buildNavigation: Could not build compact data.");
        return 0;
    }

    // Erode the walkable area by agent radius.
    if (!rcErodeWalkableArea(m_ctx, tcfg.walkableRadius, *rc.chf))
    {
        LogAdd("ERROR: buildNavigation: Could not erode.");
        return 0;
    }

    // Mark areas of dynamically added convex polygons
    /*const ConvexVolume* const* vols = geom->getConvexVolumes();
    for (int i  = 0; i < geom->getConvexVolumeCount(); ++i)
    {
        rcMarkConvexPolyArea(m_ctx, vols[i]->verts, vols[i]->nverts,
                             vols[i]->hmin, vols[i]->hmax,
                             (unsigned char)vols[i]->area, *rc.chf);
    }*/



    // Up till this part was more or less the same as OgreRecast::NavMeshBuild()
    // The following part is specific for creating a 2D intermediary navmesh tile.

    rc.lset = rcAllocHeightfieldLayerSet();
    if (!rc.lset)
    {
        LogAdd("ERROR: buildNavigation: Out of memory 'lset'.");
        return 0;
    }
    if (!rcBuildHeightfieldLayers(m_ctx, *rc.chf, tcfg.borderSize, tcfg.walkableHeight, *rc.lset))
    {
        LogAdd("ERROR: buildNavigation: Could not build heightfield layers.");
        return 0;
    }

    rc.ntiles = 0;
    for (int i = 0; i < rcMin(rc.lset->nlayers, MAX_LAYERS); ++i)
    {
        TileCacheData* tile = &rc.tiles[rc.ntiles++];
        const rcHeightfieldLayer* layer = &rc.lset->layers[i];

        // Store header
        dtTileCacheLayerHeader header;
        header.magic = DT_TILECACHE_MAGIC;
        header.version = DT_TILECACHE_VERSION;

        // Tile layer location in the navmesh.
        header.tx = tx;
        header.ty = ty;
        header.tlayer = i;
        dtVcopy(header.bmin, layer->bmin);
        dtVcopy(header.bmax, layer->bmax);

        // Tile info.
        header.width = (unsigned char)layer->width;
        header.height = (unsigned char)layer->height;
        header.minx = (unsigned char)layer->minx;
        header.maxx = (unsigned char)layer->maxx;
        header.miny = (unsigned char)layer->miny;
        header.maxy = (unsigned char)layer->maxy;
        header.hmin = (unsigned short)layer->hmin;
        header.hmax = (unsigned short)layer->hmax;

        dtStatus status = dtBuildTileCacheLayer(&comp, &header, layer->heights, layer->areas, layer->cons,
                                                &tile->data, &tile->dataSize);
        if (dtStatusFailed(status))
        {
            return 0;
        }
    }

    // Transfer ownsership of tile data from build context to the caller.
    int n = 0;
    for (int i = 0; i < rcMin(rc.ntiles, maxTiles); ++i)
    {
        tiles[n++] = rc.tiles[i];
        rc.tiles[i].data = 0;
        rc.tiles[i].dataSize = 0;
    }

    return n;
}

void OgreDetourTileCache::drawNavMesh()
{
    for (int y = 0; y < m_th; ++y)
    {
        for (int x = 0; x < m_tw; ++x)
        {
            drawDetail(x, y);
        }
    }
}

void OgreDetourTileCache::drawDetail(const int tx, const int ty)
{
    if (!DEBUG_DRAW)
        return ; // Don't debug draw for huge performance gain!

    struct TileCacheBuildContext
    {
        inline TileCacheBuildContext(struct dtTileCacheAlloc* a) : layer(0), lcset(0), lmesh(0), alloc(a) {}
        inline ~TileCacheBuildContext() { purge(); }
        void purge()
        {
            dtFreeTileCacheLayer(alloc, layer);
            layer = 0;
            dtFreeTileCacheContourSet(alloc, lcset);
            lcset = 0;
            dtFreeTileCachePolyMesh(alloc, lmesh);
            lmesh = 0;
        }
        struct dtTileCacheLayer* layer;
        struct dtTileCacheContourSet* lcset;
        struct dtTileCachePolyMesh* lmesh;
        struct dtTileCacheAlloc* alloc;
    };

    dtCompressedTileRef tiles[MAX_LAYERS];
    const int ntiles = m_tileCache->getTilesAt(tx,ty,tiles,MAX_LAYERS);

    dtTileCacheAlloc* talloc = m_tileCache->getAlloc();
    dtTileCacheCompressor* tcomp = m_tileCache->getCompressor();
    const dtTileCacheParams* params = m_tileCache->getParams();

    for (int i = 0; i < ntiles; ++i)
    {
        const dtCompressedTile* tile = m_tileCache->getTileByRef(tiles[i]);

        talloc->reset();

        TileCacheBuildContext bc(talloc);
        const int walkableClimbVx = (int)(params->walkableClimb / params->ch);
        dtStatus status;

        // Decompress tile layer data.
        status = dtDecompressTileCacheLayer(talloc, tcomp, tile->data, tile->dataSize, &bc.layer);
        if (dtStatusFailed(status))
            return ;

        // Build navmesh
        status = dtBuildTileCacheRegions(talloc, *bc.layer, walkableClimbVx);
        if (dtStatusFailed(status))
            return ;

//TODO this part is replicated from navmesh tile building in DetourTileCache. Maybe that can be reused. Also is it really necessary to do an extra navmesh rebuild from compressed tile just to draw it? Can't I just draw it somewhere where the navmesh is rebuilt?
        bc.lcset = dtAllocTileCacheContourSet(talloc);
        if (!bc.lcset)
            return ;
        status = dtBuildTileCacheContours(talloc, *bc.layer, walkableClimbVx,
                                          params->maxSimplificationError, *bc.lcset);
        if (dtStatusFailed(status))
            return ;

        bc.lmesh = dtAllocTileCachePolyMesh(talloc);
        if (!bc.lmesh)
            return ;
        status = dtBuildTileCachePolyMesh(talloc, *bc.lcset, *bc.lmesh);
        if (dtStatusFailed(status))
            return ;
        // Draw navmesh
        int tileName =(int)(tiles[i]);
//        Ogre::LogManager::getSingletonPtr()->logMessage("Drawing tile: "+tileName);
// TODO this is a dirty quickfix that should be gone as soon as there is a rebuildTile(tileref) method
        /*if(m_recast->m_pSceneMgr->hasManualObject("RecastMOWalk_"+tileName))
            return;*/
        drawPolyMesh(tileName, *bc.lmesh, tile->header->bmin, params->cs, params->ch, *bc.layer);

		/*dt
		dtFreeTileCacheContourSet(talloc, bc.lcset);
		dtFreeTileCachePolyMesh(talloc, bc.lmesh);*/
    }
}

void OgreDetourTileCache::drawPolyMesh(const int tileName, const struct dtTileCachePolyMesh &mesh, const float *orig, const float cs, const float ch, const struct dtTileCacheLayer &regionLayers, bool colorRegions)
{
            const int nvp = mesh.nvp;

            const unsigned short* verts = mesh.verts;
            const unsigned short* polys = mesh.polys;
            const unsigned char* areas = mesh.areas;
            const unsigned char *regs = regionLayers.regs;
            const int nverts = mesh.nverts;
            const int npolys = mesh.npolys;
            const int maxpolys = m_maxPolysPerTile;

            unsigned short *regions = new unsigned short[npolys];
            for (int i =0; i< npolys; i++) {
                regions[i] = (const unsigned short)regs[i];
            }

            m_recast->CreateRecastPolyMesh(tileName, verts, nverts, polys, npolys, areas, maxpolys, regions, nvp, cs, ch, orig, colorRegions);

            delete[] regions;
}

void OgreRecast::CreateRecastPolyMesh(int name, const unsigned short *verts, const int nverts, const unsigned short *polys, const int npolys, const unsigned char *areas, const int maxpolys, const unsigned short *regions, const int nvp, const float cs, const float ch, const float *orig, bool colorRegions)
{
   // When drawing regions choose different random colors for each region
// TODO maybe cache generated colors so when rebuilding tiles the same colors can be reused? If possible
   /*Ogre::ColourValue* regionColors = NULL;
   if(colorRegions) {
       regionColors = new Ogre::ColourValue[maxpolys];
       for (int i = 0; i < maxpolys; ++i) {
           regionColors[i] = Ogre::ColourValue(Ogre::Math::RangeRandom(0,1), Ogre::Math::RangeRandom(0,1), Ogre::Math::RangeRandom(0,1), 1);
       }
   }*/

	//EulerNavMesh::DrawData^ DrawReturnData = gcnew EulerNavMesh::DrawData();
	int debug;
	if (npolys > 5)
		debug = 1;

   int nIndex=0 ;


   if(npolys)
   {

      // start defining the manualObject with the navmesh planes
      //m_pRecastMOWalk = m_pSceneMgr->createManualObject("RecastMOWalk_"+name);
      //m_pRecastMOWalk->begin("recastdebug", Ogre::RenderOperation::OT_TRIANGLE_LIST) ;
      for (int i = 0; i < npolys; ++i) {    // go through all polygons
         if (areas[i] == SAMPLE_POLYAREA_GROUND || areas[i] == DT_TILECACHE_WALKABLE_AREA)
         {
            const unsigned short* p = &polys[i*nvp*2];

            unsigned short vi[3];
            for (int j = 2; j < nvp; ++j) // go through all verts in the polygon
            {
               if (p[j] == RC_MESH_NULL_IDX) break;
               vi[0] = p[0];
               vi[1] = p[j-1];
               vi[2] = p[j];
               for (int k = 0; k < 3; ++k) // create a 3-vert triangle for each 3 verts in the polygon.
               {
                  const unsigned short* v = &verts[vi[k]*3];
                  const float x = orig[0] + v[0]*cs;
                  const float y = orig[1] + (v[1]/*+1*/)*ch;
                  const float z = orig[2] + v[2]*cs;
				  
				  verticesDrawListTiles->push_back(x);
				  verticesDrawListTiles->push_back(y);
				  verticesDrawListTiles->push_back(z);
				  //save to structure

                  //m_pRecastMOWalk->position(x, y+m_navMeshOffsetFromGround, z);
                  if(colorRegions) {
                      //m_pRecastMOWalk->colour(regionColors[regions[i/*(int)Ogre::Math::RangeRandom(5,100)*/]]);  // Assign vertex color
                  } else {
                     /* if (areas[i] == SAMPLE_POLYAREA_GROUND)
                         m_pRecastMOWalk->colour(m_navmeshGroundPolygonCol);
                      else
                         m_pRecastMOWalk->colour(m_navmeshOtherPolygonCol);*/
                  }

               }
               //m_pRecastMOWalk->triangle(nIndex, nIndex+1, nIndex+2) ;
               nIndex+=3 ;
            }
         }
     }
      //m_pRecastMOWalk->end() ;



      // Define manualObject with the navmesh edges between neighbouring polygons
      //m_pRecastMONeighbour = m_pSceneMgr->createManualObject("RecastMONeighbour_"+name);
      //m_pRecastMONeighbour->begin("recastdebug", Ogre::RenderOperation::OT_LINE_LIST) ;

      for (int i = 0; i < npolys; ++i)
      {
         const unsigned short* p = &polys[i*nvp*2];
         for (int j = 0; j < nvp; ++j)
         {
            if (p[j] == RC_MESH_NULL_IDX) break;
            if (p[nvp+j] == RC_MESH_NULL_IDX) continue;
            int vi[2];
            vi[0] = p[j];
            if (j+1 >= nvp || p[j+1] == RC_MESH_NULL_IDX)
               vi[1] = p[0];
            else
               vi[1] = p[j+1];
            for (int k = 0; k < 2; ++k)
            {
               const unsigned short* v = &verts[vi[k]*3];
               const float x = orig[0] + v[0]*cs;
               const float y = orig[1] + (v[1]/*+1*/)*ch /*+ 0.1f*/;
               const float z = orig[2] + v[2]*cs;
               //dd->vertex(x, y, z, coln);
			   verticesDrawListTriangleNeighbour->push_back(x);
			   verticesDrawListTriangleNeighbour->push_back(y);
			   verticesDrawListTriangleNeighbour->push_back(z);

			   //save to structure

               //m_pRecastMONeighbour->position(x, y+m_navMeshEdgesOffsetFromGround, z) ;
               //m_pRecastMONeighbour->colour(m_navmeshNeighbourEdgeCol) ;
			   //ojo

            }
         }
      }

      //m_pRecastMONeighbour->end() ;
      

      // Define manualObject with navmesh outer edges (boundaries)
      //m_pRecastMOBoundary = m_pSceneMgr->createManualObject("RecastMOBoundary_"+name);
      //m_pRecastMOBoundary->begin("recastdebug", Ogre::RenderOperation::OT_LINE_LIST) ;

      for (int i = 0; i < npolys; ++i)
      {
         const unsigned short* p = &polys[i*nvp*2];
         for (int j = 0; j < nvp; ++j)
         {
            if (p[j] == RC_MESH_NULL_IDX) break;
            if (p[nvp+j] != RC_MESH_NULL_IDX) continue;
            int vi[2];
            vi[0] = p[j];
            if (j+1 >= nvp || p[j+1] == RC_MESH_NULL_IDX)
               vi[1] = p[0];
            else
               vi[1] = p[j+1];
            for (int k = 0; k < 2; ++k)
            {
               const unsigned short* v = &verts[vi[k]*3];
               const float x = orig[0] + v[0]*cs;
               const float y = orig[1] + (v[1]/*+1*/)*ch /*+ 0.1f*/;
               const float z = orig[2] + v[2]*cs;

			   verticesDrawListOutBorder->push_back(x);
			   verticesDrawListOutBorder->push_back(y);
			   verticesDrawListOutBorder->push_back(z);
               //dd->vertex(x, y, z, colb);
			   
			   //save to structure

               //m_pRecastMOBoundary->position(x, y+m_navMeshEdgesOffsetFromGround, z) ;
               //m_pRecastMOBoundary->colour(m_navmeshOuterEdgeCol);
            }
         }
      }

      //m_pRecastMOBoundary->end() ;

      

   }// end areacount
   //return void;
}

bool OgreDetourTileCache::loadAll(const char* path)
{
	FILE* fp = fopen(path, "rb");
	if (!fp) 
		return false;
	
	// Read header.
	NavMeshSetHeader header;
	fread(&header, sizeof(NavMeshSetHeader), 1, fp);
	if (header.magic != NAVMESHSET_MAGIC)
	{
		fclose(fp);
		return false;
	}
	if (header.version != NAVMESHSET_VERSION)
	{
		fclose(fp);
		return false;
	}
	
	dtNavMesh* mesh = dtAllocNavMesh();
	if (!mesh)
	{
		fclose(fp);
		return false;
	}
	dtStatus status = mesh->init(&header.params);
	if (dtStatusFailed(status))
	{
		fclose(fp);
		return false;
	}
		
	// Read tiles.
	for (int i = 0; i < header.numTiles; ++i)
	{
		NavMeshTileHeader tileHeader;
		fread(&tileHeader, sizeof(tileHeader), 1, fp);
		if (!tileHeader.tileRef || !tileHeader.dataSize)
			break;

		unsigned char* data = (unsigned char*)dtAlloc(tileHeader.dataSize, DT_ALLOC_PERM);
		if (!data) break;
		memset(data, 0, tileHeader.dataSize);
		fread(data, tileHeader.dataSize, 1, fp);
		
		mesh->addTile(data, tileHeader.dataSize, DT_TILE_FREE_DATA, tileHeader.tileRef, 0);
	}
	
	fclose(fp);
	this->getRecast()->m_navQuery->init(mesh, 2048);
	//m_navQuery->

	return true;
}

bool OgreDetourTileCache::saveAll(const char* path, const dtNavMesh* mesh)
{
	if (!mesh) return false;
	
	FILE* fp = fopen(path, "wb");
	if (!fp)
		return false;
	
	// Store header.
	NavMeshSetHeader header;
	header.magic = NAVMESHSET_MAGIC;
	header.version = NAVMESHSET_VERSION;
	header.numTiles = 0;
	for (int i = 0; i < mesh->getMaxTiles(); ++i)
	{
		const dtMeshTile* tile = mesh->getTile(i);
		if (!tile || !tile->header || !tile->dataSize) continue;
		header.numTiles++;
	}
	memcpy(&header.params, mesh->getParams(), sizeof(dtNavMeshParams));
	fwrite(&header, sizeof(NavMeshSetHeader), 1, fp);

	// Store tiles.
	for (int i = 0; i < mesh->getMaxTiles(); ++i)
	{
		const dtMeshTile* tile = mesh->getTile(i);
		if (!tile || !tile->header || !tile->dataSize) continue;

		NavMeshTileHeader tileHeader;
		tileHeader.tileRef = mesh->getTileRef(tile);
		tileHeader.dataSize = tile->dataSize;
		fwrite(&tileHeader, sizeof(tileHeader), 1, fp);

		fwrite(tile->data, tile->dataSize, 1, fp);
	}

	fclose(fp);

	return true;
}

void OgreDetourTileCache::drawNavMesh2(void)
{
	dtNavMesh* mesh = this->getRecast()->m_navMesh;

	for (int i = 0; i < mesh->getMaxTiles(); ++i)
	{
		const dtMeshTile* tile = mesh->getTile(i);
		if (!tile->header) 
			continue;
		//drawMeshTile(dd, mesh, q, tile, flags);
	}
}

//esta function de arriba intentaba emular a :
/*void duDebugDrawNavMeshWithClosedList(struct duDebugDraw* dd, const dtNavMesh& mesh, const dtNavMeshQuery& query, unsigned char flags)
{
	//mACH1VO
	if (!dd) return;

	const dtNavMeshQuery* q = (flags & DU_DRAWNAVMESH_CLOSEDLIST) ? &query : 0;
	
	for (int i = 0; i < mesh.getMaxTiles(); ++i)
	{
		const dtMeshTile* tile = mesh.getTile(i);
		if (!tile->header) continue;
		drawMeshTile(dd, mesh, q, tile, flags);
	}
}*/
//en Recast project, el del svn