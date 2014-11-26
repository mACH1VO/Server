// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Recast/Recast.h"

using namespace System;

namespace EulerNavMesh
{
public ref class rcPolyMeshM
{
private:
	rcPolyMesh *CLRObj;

public:
  rcPolyMeshM()
  {
	  CLRObj = new rcPolyMesh();
  }
  ~rcPolyMeshM()
  {
	  delete CLRObj;
  }
};
}