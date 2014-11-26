// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Detour/DetourNavMesh.h"

using namespace System;

namespace EulerNavMesh
{
public ref class dtNavMeshM
{
private:
	dtNavMesh *CLRObj;

public:
  dtNavMeshM()
  {
	  CLRObj = new dtNavMesh();
  }
  ~dtNavMeshM()
  {
	  delete CLRObj;
  }
};

public ref class dtNavMeshParamsM
{
private:
	dtNavMeshParams *CLRObj;

public:
  dtNavMeshParamsM()
  {
	  CLRObj = new dtNavMeshParams();
  }
  ~dtNavMeshParamsM()
  {
	  delete CLRObj;
  }
  property int maxTiles
    {
        int get()
        {
            return CLRObj->maxTiles;
        }
        
        void set(int v)
        {
            CLRObj->maxTiles = v;
        }
    }
  property int maxPolys
    {
        int get()
        {
            return CLRObj->maxPolys;
        }
        
        void set(int v)
        {
            CLRObj->maxPolys = v;
        }
    }

  property float tileWidth
    {
        float get()
        {
            return CLRObj->tileWidth;
        }
        
        void set(float v)
        {
            CLRObj->tileWidth = v;
        }
    }
   property float tileHeight
    {
        float get()
        {
            return CLRObj->tileHeight;
        }
        
        void set(float v)
        {
            CLRObj->tileHeight = v;
        }
    }

  //float orig[3];					///< The world space origin of the navigation mesh's tile space. [(x, y, z)]
  
  property float orig_x
    {
        float get()
        {
            return CLRObj->orig[0];
        }
        
        void set(float v)
        {
            CLRObj->orig[0] = v;
        }
    }

  property float orig_y
    {
        float get()
        {
            return CLRObj->orig[1];
        }
        
        void set(float v)
        {
            CLRObj->orig[1] = v;
        }
    }

  property float orig_z
    {
        float get()
        {
            return CLRObj->orig[2];
        }
        
        void set(float v)
        {
            CLRObj->orig[2] = v;
        }
    }
};





}