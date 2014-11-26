// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Recast/Recast.h"

using namespace System;

namespace EulerNavMesh
{
public ref class rcConfigM
{
private:
	rcConfig *CLRObj;

public:
  rcConfigM()
  {
	  CLRObj = new rcConfig();
  }
  ~rcConfigM()
  {
	  delete CLRObj;
  }

  /// The width of the field along the x-axis. [Limit: >= 0] [Units: vx]
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

	/// The height of the field along the z-axis. [Limit: >= 0] [Units: vx]
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
	
	/// The width/height size of tile's on the xz-plane. [Limit: >= 0] [Units: vx]
	property int tileSize
    {
        int get()
        {
            return CLRObj->tileSize;
        }
        
        void set(int v)
        {
            CLRObj->tileSize = v;
        }
    }
	
	/// The size of the non-navigable border around the heightfield. [Limit: >=0] [Units: vx]
	property int borderSize
    {
        int get()
        {
            return CLRObj->borderSize;
        }
        
        void set(int v)
        {
            CLRObj->borderSize = v;
        }
    }

	/// The xz-plane cell size to use for fields. [Limit: > 0] [Units: wu] 
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

	/// The y-axis cell size to use for fields. [Limit: > 0] [Units: wu]
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

	/// The maximum slope that is considered walkable. [Limits: 0 <= value < 90] [Units: Degrees] 
	property float walkableSlopeAngle
    {
        float get()
        {
            return CLRObj->walkableSlopeAngle;
        }
        
        void set(float v)
        {
            CLRObj->walkableSlopeAngle = v;
        }
    }

	/// Minimum floor to 'ceiling' height that will still allow the floor area to 
	/// be considered walkable. [Limit: >= 3] [Units: vx] 
	property int walkableHeight
    {
        int get()
        {
            return CLRObj->walkableHeight;
        }
        
        void set(int v)
        {
            CLRObj->walkableHeight = v;
        }
    }
	
	/// Maximum ledge height that is considered to still be traversable. [Limit: >=0] [Units: vx] 
	property int walkableClimb
    {
        int get()
        {
            return CLRObj->walkableClimb;
        }
        
        void set(int v)
        {
            CLRObj->walkableClimb = v;
        }
    }
	
	/// The distance to erode/shrink the walkable area of the heightfield away from 
	/// obstructions.  [Limit: >=0] [Units: vx] 
	property int walkableRadius
    {
        int get()
        {
            return CLRObj->walkableRadius;
        }
        
        void set(int v)
        {
            CLRObj->walkableRadius = v;
        }
    }
	
	/// The maximum allowed length for contour edges along the border of the mesh. [Limit: >=0] [Units: vx] 
	property int maxEdgeLen
    {
        int get()
        {
            return CLRObj->maxEdgeLen;
        }
        
        void set(int v)
        {
            CLRObj->maxEdgeLen = v;
        }
    }
	
	/// The maximum distance a simplfied contour's border edges should deviate 
	/// the original raw contour. [Limit: >=0] [Units: wu]
	property int maxSimplificationError
    {
        int get()
        {
            return CLRObj->maxSimplificationError;
        }
        
        void set(int v)
        {
            CLRObj->maxSimplificationError = v;
        }
    }
	
	/// The minimum number of cells allowed to form isolated island areas. [Limit: >=0] [Units: vx] 
	property int minRegionArea
    {
        int get()
        {
            return CLRObj->minRegionArea;
        }
        
        void set(int v)
        {
            CLRObj->minRegionArea = v;
        }
    }
	
	/// Any regions with a span count smaller than this value will, if possible, 
	/// be merged with larger regions. [Limit: >=0] [Units: vx] 
	property int mergeRegionArea
    {
        int get()
        {
            return CLRObj->mergeRegionArea;
        }
        
        void set(int v)
        {
            CLRObj->mergeRegionArea = v;
        }
    }
	
	/// The maximum number of vertices allowed for polygons generated during the 
	/// contour to polygon conversion process. [Limit: >= 3] 
	property int maxVertsPerPoly
    {
        int get()
        {
            return CLRObj->maxVertsPerPoly;
        }
        
        void set(int v)
        {
            CLRObj->maxVertsPerPoly = v;
        }
    }
	
	/// Sets the sampling distance to use when generating the detail mesh.
	/// (For height detail only.) [Limits: 0 or >= 0.9] [Units: wu] 
	property float detailSampleDist
    {
        float get()
        {
            return CLRObj->detailSampleDist;
        }
        
        void set(float v)
        {
            CLRObj->detailSampleDist = v;
        }
    }
	
	/// The maximum distance the detail mesh surface should deviate from heightfield
	/// data. (For height detail only.) [Limit: >=0] [Units: wu] 
	property float detailSampleMaxError
    {
        float get()
        {
            return CLRObj->detailSampleMaxError;
        }
        
        void set(float v)
        {
            CLRObj->detailSampleMaxError = v;
        }
    }

};
}