// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Detour/DetourNavMeshQuery.h"

using namespace System;

namespace EulerNavMesh
{
public ref class dtQueryFilterM
{

private:
	dtQueryFilter *CLRObj;

public:

  dtQueryFilterM()
  {
	  CLRObj = new dtQueryFilter();
  }
  ~dtQueryFilterM()
  {
	  delete CLRObj;
  }

  void setIncludeFlags(unsigned short flags)
  {
	  CLRObj->setIncludeFlags(flags);
  }

  void setExcludeFlags(unsigned short flags)
  {
	  CLRObj->setExcludeFlags(flags);
  }

  void setAreaCost(int i, float cost) 
  {
	  CLRObj->setAreaCost(i, cost) ;
  }

};
}