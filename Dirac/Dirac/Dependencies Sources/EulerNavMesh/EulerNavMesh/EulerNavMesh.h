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
}