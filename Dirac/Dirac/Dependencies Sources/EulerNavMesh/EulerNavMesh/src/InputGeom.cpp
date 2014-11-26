#pragma once
#include "InputGeom.h"
#include "OgreRecast.h"
//#include "../DetourWrapper.h"
#include <float.h>
#include <cstdio>

// ChunkyTriMesh
struct BoundsItem
{
    float bmin[2];
    float bmax[2];
    int i;
};

static void calcExtends(const BoundsItem* items, const int /*nitems*/,
                        const int imin, const int imax,
                        float* bmin, float* bmax)
{
    bmin[0] = items[imin].bmin[0];
    bmin[1] = items[imin].bmin[1];

    bmax[0] = items[imin].bmax[0];
    bmax[1] = items[imin].bmax[1];

    for (int i = imin+1; i < imax; ++i)
    {
        const BoundsItem& it = items[i];
        if (it.bmin[0] < bmin[0]) bmin[0] = it.bmin[0];
        if (it.bmin[1] < bmin[1]) bmin[1] = it.bmin[1];

        if (it.bmax[0] > bmax[0]) bmax[0] = it.bmax[0];
        if (it.bmax[1] > bmax[1]) bmax[1] = it.bmax[1];
    }
}

static int compareItemX(const void* va, const void* vb)
{
    const BoundsItem* a = (const BoundsItem*)va;
    const BoundsItem* b = (const BoundsItem*)vb;
    if (a->bmin[0] < b->bmin[0])
        return -1;
    if (a->bmin[0] > b->bmin[0])
        return 1;
    return 0;
}

static int compareItemY(const void* va, const void* vb)
{
    const BoundsItem* a = (const BoundsItem*)va;
    const BoundsItem* b = (const BoundsItem*)vb;
    if (a->bmin[1] < b->bmin[1])
        return -1;
    if (a->bmin[1] > b->bmin[1])
        return 1;
    return 0;
}


inline int longestAxis(float x, float y)
{
    return y > x ? 1 : 0;
}

static void subdivide(BoundsItem* items, int nitems, int imin, int imax, int trisPerChunk,
                      int& curNode, rcChunkyTriMeshNode* nodes, const int maxNodes,
                      int& curTri, int* outTris, const int* inTris)
{
    int inum = imax - imin;
    int icur = curNode;

    if (curNode > maxNodes)
        return;

    rcChunkyTriMeshNode& node = nodes[curNode++];

    if (inum <= trisPerChunk)
    {
        // Leaf
        calcExtends(items, nitems, imin, imax, node.bmin, node.bmax);

        // Copy triangles.
        node.i = curTri;
        node.n = inum;

        for (int i = imin; i < imax; ++i)
        {
            const int* src = &inTris[items[i].i*3];
            int* dst = &outTris[curTri*3];
            curTri++;
            dst[0] = src[0];
            dst[1] = src[1];
            dst[2] = src[2];
        }
    }
    else
    {
        // Split
        calcExtends(items, nitems, imin, imax, node.bmin, node.bmax);

        int	axis = longestAxis(node.bmax[0] - node.bmin[0],
                                   node.bmax[1] - node.bmin[1]);

        if (axis == 0)
        {
            // Sort along x-axis
            qsort(items+imin, inum, sizeof(BoundsItem), compareItemX);
        }
        else if (axis == 1)
        {
            // Sort along y-axis
            qsort(items+imin, inum, sizeof(BoundsItem), compareItemY);
        }

        int isplit = imin+inum/2;

        // Left
        subdivide(items, nitems, imin, isplit, trisPerChunk, curNode, nodes, maxNodes, curTri, outTris, inTris);
        // Right
        subdivide(items, nitems, isplit, imax, trisPerChunk, curNode, nodes, maxNodes, curTri, outTris, inTris);

        int iescape = curNode - icur;
        // Negative index means escape.
        node.i = -iescape;
    }
}


InputGeom::InputGeom()
      : nverts(0),
      ntris(0),
      bmin(0),
      bmax(0),
      m_offMeshConCount(0),
      m_volumeCount(0),
      m_chunkyMesh(0),
      normals(0),
      verts(0),
      tris(0)
{

	/*this->bmin = new float[3];
    this->bmin[0] = -51 ;
	this->bmin[1] =   0 ;
	this->bmin[2] = -51 ;

	this->bmax = new float[3];
	this->bmax[0] =  51 ;
	this->bmax[1] =   0 ;
	this->bmax[2] =  51 ;

	this->ntris = 2;
	this->nverts = 4;

	this->tris = new int[ntris*3];
	this->tris[0] = 2;
	this->tris[1] = 0;

	this->verts = new float[nverts*3];
	this->verts[0] = 50;
	this->verts[1] =  0;
	this->verts[2] = -50;
	this->verts[3] = -50;
	this->verts[4] = 0;
	this->verts[5] = -50;
	this->verts[6] = 50;
	this->verts[7] = 0;
	this->verts[8] = 50;
	this->verts[9] = -50;
	this->verts[10] = 0;
	this->verts[11] = 50;

	this->normals = new float[ntris*3];
	this->normals[0] = 0;
	this->normals[1] = 1;
	this->normals[2] = 0;
	this->normals[3] = 0;
	this->normals[4] = 1;
	this->normals[5] = 0;


    //TODO You don't need to build this in single navmesh mode
    buildChunkyTriMesh();
	//this->dump();*/
}

InputGeom::~InputGeom()
{
	delete verts;
	delete tris;
	delete normals;
}


void InputGeom::buildChunkyTriMesh()
{
    this->m_chunkyMesh = new rcChunkyTriMesh();
    if (!this->m_chunkyMesh)
    {
        LogAdd("buildTiledNavigation: Out of memory 'm_chunkyMesh'.");
        return;
    }
	const float* verts = getVerts();
	const int* tris = getTris();
	int ntris = getTriCount();


    if (!rcCreateChunkyTriMesh(verts, tris, ntris, 256, this->m_chunkyMesh))
    {
        LogAdd("buildTiledNavigation: Failed to build chunky mesh.");
        return;
    }
}

bool rcCreateChunkyTriMesh(const float* verts, const int* tris, int ntris,
                           int trisPerChunk, rcChunkyTriMesh* cm)
{
    int nchunks = (ntris + trisPerChunk-1) / trisPerChunk;

    cm->nodes = new rcChunkyTriMeshNode[nchunks*4];
    if (!cm->nodes)
        return false;

    cm->tris = new int[ntris*3];
    if (!cm->tris)
        return false;

    cm->ntris = ntris;

    // Build tree
    BoundsItem* items = new BoundsItem[ntris];
    if (!items)
        return false;

    for (int i = 0; i < ntris; i++)
    {
        const int* t = &tris[i*3];
        BoundsItem& it = items[i];
        it.i = i;
        // Calc triangle XZ bounds.
        it.bmin[0] = it.bmax[0] = verts[t[0]*3+0];
        it.bmin[1] = it.bmax[1] = verts[t[0]*3+2];
        for (int j = 1; j < 3; ++j)
        {
            const float* v = &verts[t[j]*3];
            if (v[0] < it.bmin[0]) it.bmin[0] = v[0];
            if (v[2] < it.bmin[1]) it.bmin[1] = v[2];

            if (v[0] > it.bmax[0]) it.bmax[0] = v[0];
            if (v[2] > it.bmax[1]) it.bmax[1] = v[2];
        }
    }

    int curTri = 0;
    int curNode = 0;
    subdivide(items, ntris, 0, ntris, trisPerChunk, curNode, cm->nodes, nchunks*4, curTri, cm->tris, tris);

    delete [] items;

    cm->nnodes = curNode;

    // Calc max tris per node.
    cm->maxTrisPerChunk = 0;
    for (int i = 0; i < cm->nnodes; ++i)
    {
        rcChunkyTriMeshNode& node = cm->nodes[i];
        const bool isLeaf = node.i >= 0;
        if (!isLeaf) continue;
        if (node.n > cm->maxTrisPerChunk)
            cm->maxTrisPerChunk = node.n;
    }

    return true;
}


float* InputGeom::getMeshBoundsMax()
{
    return bmax;
}

float* InputGeom::getMeshBoundsMin()
{
    return bmin;
}

int InputGeom::getVertCount()
{
    return nverts;
}

int InputGeom::getTriCount()
{
    return ntris;
}

int* InputGeom::getTris()
{
    return tris;
}

float* InputGeom::getVerts()
{
    return verts;
}

bool InputGeom::isEmpty()
{
    return nverts <= 0 || ntris <= 0;
}

inline bool checkOverlapRect(const float amin[2], const float amax[2],
                             const float bmin[2], const float bmax[2])
{
    bool overlap = true;
    overlap = (amin[0] > bmax[0] || amax[0] < bmin[0]) ? false : overlap;
    overlap = (amin[1] > bmax[1] || amax[1] < bmin[1]) ? false : overlap;
    return overlap;
}

int rcGetChunksOverlappingRect(const rcChunkyTriMesh* cm,
                               float bmin[2], float bmax[2],
                               int* ids, const int maxIds)
{
    // Traverse tree
    int i = 0;
    int n = 0;
    while (i < cm->nnodes)
    {
        const rcChunkyTriMeshNode* node = &cm->nodes[i];
        const bool overlap = checkOverlapRect(bmin, bmax, node->bmin, node->bmax);
        const bool isLeafNode = node->i >= 0;

        if (isLeafNode && overlap)
        {
            if (n < maxIds)
            {
                ids[n] = i;
                n++;
            }
        }

        if (overlap || isLeafNode)
            i++;
        else
        {
            const int escapeIndex = -node->i;
            i += escapeIndex;
        }
    }

    return n;
}