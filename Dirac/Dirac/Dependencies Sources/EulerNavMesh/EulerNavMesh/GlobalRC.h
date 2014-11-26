// EulerNavMesh.h
#pragma managed

#using <mscorlib.dll>

#include "Recast/Recast.h"

using namespace System;

namespace EulerNavMesh
{
public ref class RC
{
public:
	RC(){}
	float rcSqrM(float p)
	{
		return rcSqr(p);
	}

};
}