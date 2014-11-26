// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Recast/Recast.h"

using namespace System;

namespace EulerNavMesh
{
public ref struct rcHeightfieldM
{
private:
	rcHeightfield *CLRObj;
public:
  rcHeightfieldM()
  {
	  CLRObj = new rcHeightfield();
  }
  ~rcHeightfieldM()
  {
	  delete CLRObj;
  }
};
}