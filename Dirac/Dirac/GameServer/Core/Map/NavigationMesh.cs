using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Dirac.Math;
using EulerNavMesh;
//using EulerNavMesh;

namespace Dirac.GameServer.Core
{
    public static class NavigationMesh
    {
        public static EulerNavMeshWrapper Wrapper;

        public static inputData CurrentData { get; set; }
        public static inputConfig CurrentConfig { get; set; }

        private static bool initialized;
        public static void Initialize()
        {
            if (!initialized)
            {
                DateTime dt = DateTime.Now;
                Wrapper = new EulerNavMeshWrapper();
                Wrapper.Initialize(CurrentData, CurrentConfig);
                initialized = true;
                TimeSpan ts = DateTime.Now - dt;
                Logging.Logger.Add("NavigationMesh Initialize Took: " + ts.ToString());
            }
            //Wrapper = new EulerNavMeshWrapper();
            //Wrapper.Initialize(CurrentData, CurrentConfig);
        }

        public static List<Vector3> FindPath(Vector3 source, Vector3 dest)
        {
            //use lock here.
            if (!initialized)
                return new List<Vector3>();

            try
            {
                List<Vector3> result = new List<Vector3>(); //avoid this, try static or singleton.

                float[] s = new float[3];
                s[0] = source.x;
                s[1] = source.y;
                s[2] = source.z;

                float[] d = new float[3];
                d[0] = dest.x;
                d[1] = dest.y;
                d[2] = dest.z;

                int ErrorCode = Wrapper.FindPath(s, d, 1, 1);
                /*
                    *  0   found path
                    *  -1  Couldn't find polygon nearest to start point
                    *  -2  Couldn't find polygon nearest to end point
                    *  -3  Couldn't create a path
                    *  -4  Couldn't find a path
                    *  -5  Couldn't create a straight path
                    *  -6  Couldn't find a straight path
                 */
                if (ErrorCode == 1)
                {
                    //Logging.Logger.Warn("Find 1 point path, meaning src == dest. Aborting Pathfind");
                    return new List<Vector3>();
                }

                if (ErrorCode == -3)
                {
                    Logging.Logger.Warn("Couldn't find a path");
                    return new List<Vector3>();
                }

                float[] path = Wrapper.GetPath(1);

                if (path.Length <= 1)
                    return new List<Vector3>();

                for (int i = 0; i < path.Length; i = i + 3)
                {
                    result.Add(new Vector3(path[i], path[i + 1], path[i + 2]));
                }

                return result;
            }
            catch (Exception ex)
            {
                Logging.Logger.Error(ex);
                return new List<Vector3>();
            }
        }

        private static inputData getsavedData()
        {
            StreamReader sr = new StreamReader("inputGeomDump.txt");
            EulerNavMesh.inputData returnData = new EulerNavMesh.inputData();
            String firstString = sr.ReadLine();
            if (firstString == "bmin: ")
            {
                returnData.bmin = new float[3];
                returnData.bmin[0] = float.Parse((sr.ReadLine().Replace(".", ",")));
                returnData.bmin[1] = float.Parse((sr.ReadLine().Replace(".", ",")));
                returnData.bmin[2] = float.Parse((sr.ReadLine().Replace(".", ",")));
            }
            if (sr.ReadLine() == "bmax: ")
            {
                returnData.bmax = new float[3];
                returnData.bmax[0] = float.Parse((sr.ReadLine().Replace(".", ",")));
                returnData.bmax[1] = float.Parse((sr.ReadLine().Replace(".", ",")));
                returnData.bmax[2] = float.Parse((sr.ReadLine().Replace(".", ",")));
            }
            if (sr.ReadLine() == "nverts: ")
            {
                returnData.nverts = int.Parse(sr.ReadLine());
            }
            if (sr.ReadLine() == "ntris: ")
            {
                returnData.ntris = int.Parse(sr.ReadLine());
            }
            if (sr.ReadLine() == "verts: ")
            {
                returnData.verts = new float[returnData.nverts * 3];
                for (int i = 0; i < returnData.verts.Length; i++)
                {
                    returnData.verts[i] = float.Parse((sr.ReadLine().Replace(".", ",")));
                }
            }
            if (sr.ReadLine() == "tris: ")
            {
                returnData.tris = new int[returnData.ntris * 3];
                for (int i = 0; i < returnData.tris.Length; i++)
                {
                    returnData.tris[i] = int.Parse((sr.ReadLine()));
                }
            }
            if (sr.ReadLine() == "normals: ")
            {
                returnData.normals = new float[returnData.ntris * 3];
                for (int i = 0; i < returnData.normals.Length; i++)
                {
                    returnData.normals[i] = float.Parse((sr.ReadLine().Replace(".", ",")));
                }
            }
            sr.Close();
            return returnData;
        }
    }
}
