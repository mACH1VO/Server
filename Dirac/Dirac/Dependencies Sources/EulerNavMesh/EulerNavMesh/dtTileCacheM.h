// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "DetourTileCache/DetourTileCache.h"

using namespace System;

namespace EulerNavMesh
{
public ref class dtTileCacheM
{
private:
	dtTileCache *CLRObj;

public:
  dtTileCacheM()
  {
	  CLRObj = new dtTileCache();
  }
  ~dtTileCacheM()
  {
	  delete CLRObj;
  }
};
}