// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Recast/Recast.h"

using namespace System;

namespace EulerNavMesh
{
public ref class rcContourSetM
{
private:
	rcContourSet *CLRObj;

public:
  rcContourSetM()
  {
	  CLRObj = new rcContourSet();
  }
  ~rcContourSetM()
  {
	  delete CLRObj;
  }
};
}