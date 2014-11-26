#pragma once
#include "OgreRecast.h"
#include "DetourTileCache/DetourTileCacheBuilder.h"
#include "OgreRecastConfigParams.h"
#include "..\Vector3.h"



OgreRecast::OgreRecast(OgreRecastConfigParams configParams)
      : m_rebuildSg(false),
      mFilter(0),
      //mNavmeshPruner(0),
      m_ctx(0)
{
   // Init recast stuff in a safe state
   verticesDrawListTiles = new vector<float>();
   verticesDrawListTriangleNeighbour = new vector<float>();
   verticesDrawListOutBorder = new vector<float>();

   m_triareas=NULL;
   m_solid=NULL ;
   m_chf=NULL ;
   m_cset=NULL;
   m_pmesh=NULL;
   //m_cfg;   
   m_dmesh=NULL ;
   m_geom=NULL;
   m_navMesh=NULL;
   m_navQuery=NULL;
   //m_navMeshDrawFlags;
   m_ctx=NULL ;

   RecastCleanup() ; // TODO ?? don't know if I should do this prior to making any recast stuff, but the demo did.

   // Set default size of box around points to look for nav polygons
   mExtents[0] = 32.0f; mExtents[1] = 32.0f; mExtents[2] = 32.0f;

   // Setup the default query filter
   mFilter = new dtQueryFilter();
   mFilter->setIncludeFlags(0xFFFF);    // Include all
   mFilter->setExcludeFlags(0);         // Exclude none
   // Area flags for polys to consider in search, and their cost
   mFilter->setAreaCost(SAMPLE_POLYAREA_GROUND, 1.0f);       // TODO have a way of configuring the filter
   mFilter->setAreaCost(DT_TILECACHE_WALKABLE_AREA, 1.0f);


   // Init path store. MaxVertex 0 means empty path slot
   for(int i = 0; i < MAX_PATHSLOT; i++) {
       m_PathStore[i].MaxVertex = 0;
       m_PathStore[i].Target = 0;
   }


   // Set configuration
   configure(configParams);
}

OgreRecast::~OgreRecast()
{
	//delete m_triareas;
	this->RecastCleanup();
	delete m_solid;
	delete m_chf;
	delete m_cset;
	delete m_pmesh;
	delete m_dmesh;
	delete m_navMesh;
	delete m_navQuery;

	delete mFilter;

	delete m_ctx;
	delete m_geom;
	

	//delete verticesDrawListOutBorder;

	verticesDrawListTiles->clear();
	verticesDrawListTriangleNeighbour->clear();
	verticesDrawListOutBorder->clear();
	delete verticesDrawListTriangleNeighbour;
	delete verticesDrawListOutBorder;

	//delete verticesDrawListOutBorder;
	//delete mFilter;
}
/**
 * Cleanup recast stuff, not debug manualobjects.
**/
void OgreRecast::RecastCleanup()
{
   if(m_triareas) delete [] m_triareas;
   m_triareas = 0;

   rcFreeHeightField(m_solid);
   m_solid = 0;
   rcFreeCompactHeightfield(m_chf);
   m_chf = 0;
   rcFreeContourSet(m_cset);
   m_cset = 0;
   rcFreePolyMesh(m_pmesh);
   m_pmesh = 0;
   rcFreePolyMeshDetail(m_dmesh);
   m_dmesh = 0;
   dtFreeNavMesh(m_navMesh);
   m_navMesh = 0;

   dtFreeNavMeshQuery(m_navQuery);
   m_navQuery = 0 ;

   if(m_ctx){
       delete m_ctx;
       m_ctx = 0;
   }
}

void OgreRecast::configure(OgreRecastConfigParams params)
{
    // NOTE: this is one of the most important parts to get it right!!
    // Perhaps the most important part of the above is setting the agent size with m_agentHeight and m_agentRadius,
    // and the voxel cell size used, m_cellSize and m_cellHeight. In my project 1 units is a little less than 1 meter,
    // so I've set the agent to 2.5 units high, and the cell sizes to sub-meter size.
    // This is about the same as in the original cell sizes in the Recast/Detour demo.

    // Smaller cellsizes are the most accurate at finding all the places we could go, but are also slow to generate.
    // Might be suitable for pre-generated meshes. Though it also produces a lot more polygons.

    if(m_ctx) {
        delete m_ctx;
        m_ctx = 0;
    }
    m_ctx=new rcContext(true);

    m_cellSize = params.getCellSize();
    m_cellHeight = params.getCellHeight();
    //m_agentMaxSlope = params.getAgentMaxSlope(); //ojo
    m_agentHeight = params.getAgentHeight();
    m_agentMaxClimb = params.getAgentMaxClimb();
    m_agentRadius = params.getAgentRadius();
    m_edgeMaxLen = params.getEdgeMaxLen();
    m_edgeMaxError = params.getEdgeMaxError();
    m_regionMinSize = params.getRegionMinSize();
    m_regionMergeSize = params.getRegionMergeSize();
    m_vertsPerPoly = params.getVertsPerPoly();
    m_detailSampleDist = params.getDetailSampleDist();
    m_detailSampleMaxError = params.getDetailSampleMaxError();
    m_keepInterResults = params.getKeepInterResults();

    // Init cfg object
    memset(&m_cfg, 0, sizeof(m_cfg));
    m_cfg.cs = m_cellSize;
    m_cfg.ch = m_cellHeight;
    m_cfg.walkableSlopeAngle = m_agentMaxSlope;
    m_cfg.walkableHeight = params._getWalkableheight();
    m_cfg.walkableClimb = params._getWalkableClimb();
    m_cfg.walkableRadius = params._getWalkableRadius();
    m_cfg.maxEdgeLen = params._getMaxEdgeLen();
    m_cfg.maxSimplificationError = m_edgeMaxError;
    m_cfg.minRegionArea = params._getMinRegionArea();
    m_cfg.mergeRegionArea = params._getMergeRegionArea();
    m_cfg.maxVertsPerPoly = m_vertsPerPoly;
    m_cfg.detailSampleDist = (float) params._getDetailSampleDist();
    m_cfg.detailSampleMaxError = (float) params._getDetailSampleMaxError();


    // Demo specific parameters
    m_navMeshOffsetFromGround = m_cellHeight/5;         // Distance above ground for drawing navmesh polygons
    m_navMeshEdgesOffsetFromGround = m_cellHeight/3;    // Distance above ground for drawing edges of navmesh (should be slightly higher than navmesh polygons)
    m_pathOffsetFromGround = m_agentHeight+m_navMeshOffsetFromGround; // Distance above ground for drawing path debug lines relative to cellheight (should be higher than navmesh polygons)
}



#include <math.h>


/**
 * Now for the pathfinding code. 
 * This takes a start point and an end point and, if possible, generates a list of lines in a path. It might fail if the start or end points aren't near any navmesh polygons, or if the path is too long, or it can't make a path, or various other reasons. So far I've not had problems though.
 *
 * nTarget: The index number for the slot in which the found path is to be stored
 * nPathSlot: Number identifying the target the path leads to
 *
 * Return codes:
 *  0   found path
 *  -1  Couldn't find polygon nearest to start point
 *  -2  Couldn't find polygon nearest to end point
 *  -3  Couldn't create a path
 *  -4  Couldn't find a path
 *  -5  Couldn't create a straight path
 *  -6  Couldn't find a straight path
**/
int OgreRecast::FindPath(float* pStartPos, float* pEndPos, int nPathSlot, int nTarget)
{
   dtStatus status ;
   dtPolyRef StartPoly ;
   float StartNearest[3] ;
   dtPolyRef EndPoly ;
   float EndNearest[3] ;
   dtPolyRef PolyPath[MAX_PATHPOLY] ;
   int nPathCount=0 ;
   float StraightPath[MAX_PATHVERT*3] ;
   int nVertCount=0 ;


   // find the start polygon
   status=m_navQuery->findNearestPoly(pStartPos, mExtents, mFilter, &StartPoly, StartNearest) ;
   if((status&DT_FAILURE) || (status&DT_STATUS_DETAIL_MASK)) return -1 ; // couldn't find a polygon

   // find the end polygon
   status=m_navQuery->findNearestPoly(pEndPos, mExtents, mFilter, &EndPoly, EndNearest) ;
   if((status&DT_FAILURE) || (status&DT_STATUS_DETAIL_MASK)) return -2 ; // couldn't find a polygon

   status=m_navQuery->findPath(StartPoly, EndPoly, StartNearest, EndNearest, mFilter, PolyPath, &nPathCount, MAX_PATHPOLY) ;
   if((status&DT_FAILURE) || (status&DT_STATUS_DETAIL_MASK)) return -3 ; // couldn't create a path
   if(nPathCount==0) return -4 ; // couldn't find a path

   status=m_navQuery->findStraightPath(StartNearest, EndNearest, PolyPath, nPathCount, StraightPath, NULL, NULL, &nVertCount, MAX_PATHVERT) ;
   if((status&DT_FAILURE) || (status&DT_STATUS_DETAIL_MASK)) return -5 ; // couldn't create a path
   if(nVertCount==0) return -6 ; // couldn't find a path

   // At this point we have our path.  Copy it to the path store
   int nIndex=0 ;
   for(int nVert=0 ; nVert<nVertCount ; nVert++)
   {
      m_PathStore[nPathSlot].PosX[nVert]=StraightPath[nIndex++] ;
      m_PathStore[nPathSlot].PosY[nVert]=StraightPath[nIndex++] ;
      m_PathStore[nPathSlot].PosZ[nVert]=StraightPath[nIndex++] ;

      //sprintf(m_chBug, "Path Vert %i, %f %f %f", nVert, m_PathStore[nPathSlot].PosX[nVert], m_PathStore[nPathSlot].PosY[nVert], m_PathStore[nPathSlot].PosZ[nVert]) ;
      //m_pLog->logMessage(m_chBug);
   }
   m_PathStore[nPathSlot].MaxVertex=nVertCount ;
   m_PathStore[nPathSlot].Target=nTarget ;

   return nVertCount ;

}


void OgreRecast::OgreVect3ToFloatA(const Vector3 vect, float* result)
{
    result[0] = vect.x;
    result[1] = vect.y;
    result[2] = vect.z;
};

void OgreRecast::FloatAToOgreVect3(const float* vect, Vector3 &result)
{
    result.x = vect[0];
    result.y = vect[1];
    result.z = vect[2];
}

/*int OgreRecast::FindPath(Vector3 startPos, Vector3 endPos, int nPathSlot, int nTarget)
{
    float start[3];
    float end[3];
    OgreVect3ToFloatA(startPos, start);
    OgreVect3ToFloatA(endPos, end);

    return FindPath(start,end,nPathSlot,nTarget);
}*/

/*std::vector<Vector3> OgreRecast::getPath(int pathSlot)
{
    std::vector<Vector3> result;
    if(pathSlot < 0 || pathSlot >= MAX_PATHSLOT || m_PathStore[pathSlot].MaxVertex <= 0)
        return result;

    PATHDATA *path = &(m_PathStore[pathSlot]);
    result.reserve(path->MaxVertex);
    for(int i = 0; i < path->MaxVertex; i++) {
        result.push_back(Vector3(path->PosX[i], path->PosY[i], path->PosZ[i]));
    }

    return result;
}*/
