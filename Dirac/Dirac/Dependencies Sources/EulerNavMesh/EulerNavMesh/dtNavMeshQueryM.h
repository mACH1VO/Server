// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Detour/DetourNavMeshQuery.h"

using namespace System;

namespace EulerNavMesh
{
public ref class dtNavMeshQueryM
{
private:
	dtNavMeshQuery *CLRObj;
public:
  dtNavMeshQueryM()
  {
	  CLRObj = new dtNavMeshQuery();
  }
  ~dtNavMeshQueryM()
  {
	  delete CLRObj;
  }
};
}