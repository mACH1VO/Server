// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Recast/Recast.h"

using namespace System;

namespace EulerNavMesh
{
public ref class rcPolyMeshDetailM
{
private:
	rcPolyMeshDetail *CLRObj;
public:
  rcPolyMeshDetailM()
  {
	  CLRObj = new rcPolyMeshDetail();
  }
  ~rcPolyMeshDetailM()
  {
	  delete CLRObj;
  }
};
}