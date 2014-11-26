using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dirac.GameServer;
using Dirac.GameServer.Network;

using Dirac.Math;

namespace Dirac.Extensions
{
    public static class DiracMathExtensions
    {
        public static Vector3 Parse(this Vector3 vector3, GameBitBuffer buffer)
        {
            vector3.x = buffer.ReadFloat32();
            vector3.y = buffer.ReadFloat32();
            vector3.z = buffer.ReadFloat32();
            return vector3;
        }

        public static void Encode(this Vector3 vector3, GameBitBuffer buffer)
        {
            buffer.WriteFloat32(vector3.x);
            buffer.WriteFloat32(vector3.y);
            buffer.WriteFloat32(vector3.z);
        }

        public static void AsText(this Vector3 vector3, StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Vector3:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("X: " + vector3.x.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Y: " + vector3.y.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Z: " + vector3.z.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        /*public static Vector3 RotatedCopyNorm(this Mogre.Vector3 vector3, Degree degree, Mogre.Vector3 Axis )
        {
            Logging.LogManager.DefaultLogger.Trace("Degree entered : " + degree.ValueDegrees.ToString());
            Quaternion q = new Quaternion(degree, Axis);
            Matrix3 rotM = q.ToRotationMatrix();
            Vector3 newDirector = (vector3 * rotM).NormalisedCopy;
            return newDirector;
        }*/

        //-----------------------------------------------------------------------------

        public static Quaternion Parse(this Quaternion quaternion, GameBitBuffer buffer)
        {
            quaternion.x = buffer.ReadFloat32();
            quaternion.y = buffer.ReadFloat32();
            quaternion.z = buffer.ReadFloat32();
            quaternion.w = buffer.ReadFloat32();
            return quaternion;
        }

        public static void Encode(this Quaternion quaternion, GameBitBuffer buffer)
        {
            buffer.WriteFloat32(quaternion.x);
            buffer.WriteFloat32(quaternion.y);
            buffer.WriteFloat32(quaternion.z);
            buffer.WriteFloat32(quaternion.w);
        }

        public static void AsText(this Quaternion quaternion, StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Quaternion:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("X: " + quaternion.x.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Y: " + quaternion.y.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("Z: " + quaternion.z.ToString("G"));
            b.Append(' ', pad);
            b.AppendLine("W: " + quaternion.w.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
