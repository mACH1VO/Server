// EulerNavMesh.h
#pragma once

#include <stdio.h>      // standard input/output
#include <vector>       // stl vector header

#include "include\OgreRecast.h"
#include "include\OgreDetourTileCache.h"
#include "include\InputGeom.h"

//#include <SDL_opengl.h>

//#include "Logg.h"

using namespace System;
using namespace System::Runtime::InteropServices;
//using namespace Log;

namespace EulerNavMesh {

	public ref class inputData
	{
	public:
		array<float>^ bmin;// = gcnew array<float>(NativeOgreRecast->verticesDrawListOutBorder->size());
		array<float>^ bmax;// = gcnew array<float>(NativeOgreRecast->verticesDrawListOutBorder->size());
		array<float>^ normals;// = gcnew array<float>(NativeOgreRecast->verticesDrawListOutBorder->size());
		array<float>^ verts;// = gcnew array<float>(NativeOgreRecast->verticesDrawListOutBorder->size());
		array<int>^ tris;// = gcnew array<float>(NativeOgreRecast->verticesDrawListOutBorder->size());
		int nverts;
		int ntris;

		inputData(){}
		void process()
		{
		}
	};

	public ref class inputConfig
	{
	public:
		  property float cellSize; 
          property float cellHeight; 
          property float agentMaxSlope; 
          property float agentHeight; 
          property float agentMaxClimb; 
          property float agentRadius; 
          property float edgeMaxLen; 
          property float edgeMaxError; 
          property float regionMinSize; 
          property float regionMergeSize; 
          property float vertsPerPoly;    // (=6)
          property float detailSampleDist;
          property float detailSampleMaxError;
          bool keepInterResults;

		inputConfig(){}
	};

	public ref class EulerNavMeshWrapper
	{
	public:
		OgreRecast* NativeOgreRecast;
		OgreDetourTileCache* NativeOgreDetourTileCache;
		InputGeom* NativeInputGeom;

		bool Initialize(inputData^ data, inputConfig^ config)
		{
			//int GL_PROJECTION1 = GL_PROJECTION;
			//int GL_MODELVIEW1 = GL_MODELVIEW;

			initLog();
			OgreRecastConfigParams configParams;

			configParams.cellSize = config->cellSize;
			configParams.cellHeight = config->cellHeight;
			configParams.agentMaxSlope = config->agentMaxSlope;
			configParams.agentHeight = config->agentHeight;
			configParams.agentMaxClimb = config->agentMaxClimb;
			configParams.agentRadius = config->agentRadius;
			configParams.edgeMaxLen = config->edgeMaxLen;
			configParams.edgeMaxError = config->edgeMaxError;
			configParams.regionMinSize = config->regionMinSize;
			configParams.regionMergeSize = config->regionMergeSize;
			configParams.vertsPerPoly = config->vertsPerPoly;
			configParams.detailSampleDist = config->detailSampleDist;
			configParams.detailSampleMaxError = config->detailSampleMaxError;
			configParams.keepInterResults = config->keepInterResults;

			configParams.eval();

			NativeOgreRecast = new OgreRecast(configParams);
			NativeOgreDetourTileCache = new OgreDetourTileCache(NativeOgreRecast);
			NativeInputGeom = new InputGeom();
			processInputGeom(data);
			NativeInputGeom->buildChunkyTriMesh();

			NativeOgreDetourTileCache->TileCacheBuild(NativeInputGeom);
			NativeOgreDetourTileCache->drawNavMesh();

			/*float s[3];
			s[0] = 1;
			s[1] = 1;
			s[2] = 1;

			float d[3];
			d[0] = 2;
			d[1] = 2;
			d[2] = 2;

			int result = NativeOgreRecast->FindPath(s, d, 1, 1);
			if (result > 0)
			{

			}*/

			return true;
		}

		void destroyAll()
		{
			delete NativeOgreRecast;
			delete NativeOgreDetourTileCache;
			//delete NativeOgreDetourTileCache;
			delete NativeInputGeom;
		}
		void loadAll(System::String^ str)
		{
			array<float>^ returnResult = gcnew array<float>(NativeOgreRecast->verticesDrawListTiles->size());

			this->NativeOgreRecast->verticesDrawListOutBorder->clear();
			this->NativeOgreRecast->verticesDrawListTriangleNeighbour->clear();
			this->NativeOgreRecast->verticesDrawListTiles->clear();

			array<float>^ returnResult2 = gcnew array<float>(NativeOgreRecast->verticesDrawListTiles->size());

			//this->NativeOgreRecast->

			const char* str2 = (const char*)(void*)Marshal::StringToHGlobalAnsi(str);
			this->NativeOgreDetourTileCache->loadAll(str2);
			this->NativeOgreDetourTileCache->drawNavMesh();
		}

		void loadAll2(System::String^ str, inputConfig^ config) 
		{
		    initLog();
			OgreRecastConfigParams configParams;

			configParams.cellSize = config->cellSize;
			configParams.cellHeight = config->cellHeight;
			configParams.agentMaxSlope = config->agentMaxSlope;
			configParams.agentHeight = config->agentHeight;
			configParams.agentMaxClimb = config->agentMaxClimb;
			configParams.agentRadius = config->agentRadius;
			configParams.edgeMaxLen = config->edgeMaxLen;
			configParams.edgeMaxError = config->edgeMaxError;
			configParams.regionMinSize = config->regionMinSize;
			configParams.regionMergeSize = config->regionMergeSize;
			configParams.vertsPerPoly = config->vertsPerPoly;
			configParams.detailSampleDist = config->detailSampleDist;
			configParams.detailSampleMaxError = config->detailSampleMaxError;
			configParams.keepInterResults = config->keepInterResults;

			configParams.eval();

			NativeOgreRecast = new OgreRecast(configParams);
			NativeOgreDetourTileCache = new OgreDetourTileCache(NativeOgreRecast);
			const char* str2 = (const char*)(void*)Marshal::StringToHGlobalAnsi(str);
			this->NativeOgreDetourTileCache->loadAll(str2);
			this->NativeOgreDetourTileCache->drawNavMesh();
		}

		void saveAll(System::String^ str)
		{
			const char* str2 = (const char*)(void*)Marshal::StringToHGlobalAnsi(str);
			this->NativeOgreDetourTileCache->saveAll(str2, this->NativeOgreRecast->m_navMesh);
		}
		/*bool ReInitialize(inputData^ data, inputConfig^ config)
		{
			//delete NativeOgreRecast;
			//delete NativeOgreDetourTileCache;
			//delete NativeInputGeom;

			OgreRecastConfigParams configParams;

			configParams.cellSize = config->cellSize;
			configParams.cellHeight = config->cellHeight;
			configParams.agentMaxSlope = config->agentMaxSlope;
			configParams.agentHeight = config->agentHeight;
			configParams.agentMaxClimb = config->agentMaxClimb;
			configParams.agentRadius = config->agentRadius;
			configParams.edgeMaxLen = config->edgeMaxLen;
			configParams.edgeMaxError = config->edgeMaxError;
			configParams.regionMinSize = config->regionMinSize;
			configParams.regionMergeSize = config->regionMergeSize;
			configParams.vertsPerPoly = config->vertsPerPoly;
			configParams.detailSampleDist = config->detailSampleDist;
			configParams.detailSampleMaxError = config->detailSampleMaxError;
			configParams.keepInterResults = config->keepInterResults;
			configParams.eval();

			

			NativeOgreRecast = new OgreRecast(configParams);
			NativeOgreDetourTileCache = new OgreDetourTileCache(NativeOgreRecast);
			NativeInputGeom = new InputGeom();
			processInputGeom(data);
			NativeInputGeom->buildChunkyTriMesh();

			NativeOgreDetourTileCache->TileCacheBuild(NativeInputGeom);
			NativeOgreDetourTileCache->drawNavMesh();

			return true;
		}*/

		

		// TODO: Add your methods for this class here.
		int FindPath(array<float> ^s, array<float> ^d, int nPathSlot, int nTarget)
		{
			float s_unmanaged[3];
			s_unmanaged[0] = s[0];
			s_unmanaged[1] = s[1];
			s_unmanaged[2] = s[2];

			float d_unmanaged[3];
			d_unmanaged[0] = d[0];
			d_unmanaged[1] = d[1];
			d_unmanaged[2] = d[2];

			//float* s = pStartPos
			return NativeOgreRecast->FindPath(s_unmanaged, d_unmanaged, nPathSlot, nTarget);
			//return 1;
		}

		array<float>^ GetPath(int pathSlot)
		{
			std::vector<float> result;// = new std::vector<float>();
			if(pathSlot < 0 || pathSlot >= MAX_PATHSLOT || NativeOgreRecast->m_PathStore[pathSlot].MaxVertex <= 0)
				return gcnew array<float>(1);

			PATHDATA *path = &(NativeOgreRecast->m_PathStore[pathSlot]);
			result.reserve(path->MaxVertex);
			for(int i = 0; i < path->MaxVertex; i++) {
				result.push_back(path->PosX[i]);
				result.push_back(path->PosY[i]);
				result.push_back(path->PosZ[i]);
			}

			array<float>^ returnResult = gcnew array<float>(result.size());

			for(std::vector<float>::size_type i = 0; i != result.size(); i++) {
				 returnResult[i] = result[i]; 
			}

			return returnResult;
		}

		array<float>^ GetVerticesDrawListTiles()
		{
			array<float>^ returnResult = gcnew array<float>(NativeOgreRecast->verticesDrawListTiles->size());

			vector<float>::iterator it;

			int i = 0;
			for(it = NativeOgreRecast->verticesDrawListTiles->begin() ;
				it != NativeOgreRecast->verticesDrawListTiles->end() ;
				++it)
			{
				float temp = (*it);
				returnResult[i] = temp; 
				i++;
			}

			return returnResult;

		}

		array<float>^ GetVerticesDrawListTriangleNeighbour()
		{
			array<float>^ returnResult = gcnew array<float>(NativeOgreRecast->verticesDrawListTriangleNeighbour->size());

			vector<float>::iterator it;

			int i = 0;
			for(it = NativeOgreRecast->verticesDrawListTriangleNeighbour->begin() ;
				it != NativeOgreRecast->verticesDrawListTriangleNeighbour->end() ;
				++it)
			{
				float temp = (*it);
				returnResult[i] = temp; 
				i++;
			}

			return returnResult;
		}

		array<float>^ GetVerticesDrawListOutBorder()
		{
			array<float>^ returnResult = gcnew array<float>(NativeOgreRecast->verticesDrawListOutBorder->size());

			vector<float>::iterator it;

			int i = 0;
			for(it = NativeOgreRecast->verticesDrawListOutBorder->begin() ;
				it != NativeOgreRecast->verticesDrawListOutBorder->end() ;
				++it)
			{
				float temp = (*it);
				returnResult[i] = temp; 
				i++;
			}

			return returnResult;
		}

		private: 

		void processInputGeom(inputData^ data)
		{
			NativeInputGeom->bmin = new float[3];
			NativeInputGeom->bmax = new float[3];

			NativeInputGeom->bmin[0] = data->bmin[0];
			NativeInputGeom->bmin[1] = data->bmin[1];
			NativeInputGeom->bmin[2] = data->bmin[2];

			NativeInputGeom->bmax[0] = data->bmax[0];
			NativeInputGeom->bmax[1] = data->bmax[1];
			NativeInputGeom->bmax[2] = data->bmax[2];

			NativeInputGeom->nverts = data->nverts;
			NativeInputGeom->ntris = data->ntris;

			NativeInputGeom->verts = new float[data->nverts*3];
			for(int i = 0; i < (data->nverts*3); i++)
			{
				NativeInputGeom->verts[i] = data->verts[i];
			}

			NativeInputGeom->tris = new int[data->ntris*3];
			for(int i = 0; i < (data->ntris*3); i++)
			{
				NativeInputGeom->tris[i] =data->tris[i];
			}

			NativeInputGeom->normals = new float[data->ntris*3];
			for(int i = 0; i < (data->ntris*3); i++)
			{
				NativeInputGeom->normals[i] =data->normals[i];
			}
		}
	};

};
