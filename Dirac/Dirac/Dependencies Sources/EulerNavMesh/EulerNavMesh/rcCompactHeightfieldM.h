// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Recast/Recast.h"

using namespace System;

namespace EulerNavMesh
{
public ref class rcCompactHeightfieldM
{
private:
	rcCompactHeightfield *CLRObj;
public:
  rcCompactHeightfieldM()
  {
	  CLRObj = new rcCompactHeightfield();
  }
  ~rcCompactHeightfieldM()
  {
	  delete CLRObj;
  }
};
}