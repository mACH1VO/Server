#pragma once
#include <stdio.h>
#include <../Logg.h>



static FILE* logfp;
void initLog()
{
	logfp = fopen ( "recastLog.log" , "wb" );
}

void LogAdd(std::string szLog)
{
	char szBuffer[512]={0};

	sprintf(szBuffer,"%s",szLog.c_str());

	if (logfp != NULL)
	{
		fprintf(logfp, "%s\n", szBuffer);
		fflush(logfp);
	}

	
	//int GL_PROJECTION = GL_PROJECTION;
}

//static string temp;


/*string toString(float v)
{
	char[10] tempchars;
	temp.clear();
	sprintf(tempchars, "%f", 0);
	sprintf(tempchars, "%f", v);
	temp = tempchars;
	return temp;
}*/

/*string toString(int v);
string toString(unsigned int v);*/
