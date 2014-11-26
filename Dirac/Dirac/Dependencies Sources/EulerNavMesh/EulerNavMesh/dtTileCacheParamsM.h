// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "DetourTileCache/DetourTileCache.h"

using namespace System;

namespace EulerNavMesh
{
public ref class dtTileCacheParamsM
{
private:
	dtTileCacheParams *CLRObj;

public:
  dtTileCacheParamsM()
  {
	  CLRObj = new dtTileCacheParams();
  }
  ~dtTileCacheParamsM()
  {
	  delete CLRObj;
  }

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

  property float cs
    {
        float get()
        {
            return CLRObj->cs;
        }
        
        void set(float v)
        {
            CLRObj->cs = v;
        }
    }

  property float ch
    {
        float get()
        {
            return CLRObj->ch;
        }
        
        void set(float v)
        {
            CLRObj->ch = v;
        }
    }

  property int width
    {
        int get()
        {
            return CLRObj->width;
        }
        
        void set(int v)
        {
            CLRObj->width = v;
        }
    }
  property int height
    {
        int get()
        {
            return CLRObj->height;
        }
        
        void set(int v)
        {
            CLRObj->height = v;
        }
    }

  property float walkableHeight
    {
        float get()
        {
            return CLRObj->walkableHeight;
        }
        
        void set(float v)
        {
            CLRObj->walkableHeight = v;
        }
    }

  property float walkableRadius
    {
        float get()
        {
            return CLRObj->walkableRadius;
        }
        
        void set(float v)
        {
            CLRObj->walkableRadius = v;
        }
    }

  property float walkableClimb
    {
        float get()
        {
            return CLRObj->walkableClimb;
        }
        
        void set(float v)
        {
            CLRObj->walkableClimb = v;
        }
    }

  property float maxSimplificationError
    {
        float get()
        {
            return CLRObj->maxSimplificationError;
        }
        
        void set(float v)
        {
            CLRObj->maxSimplificationError = v;
        }
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

	  property int maxObstacles
    {
        int get()
        {
            return CLRObj->maxObstacles;
        }
        
        void set(int v)
        {
            CLRObj->maxObstacles = v;
        }
    }
};

}