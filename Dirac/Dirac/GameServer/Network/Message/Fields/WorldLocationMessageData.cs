using System;
using System.Text;
using Dirac.Extensions;
using Dirac.Math;
using Dirac.GameServer.Core;

namespace Dirac.GameServer.Network.Message
{
    [Serializable]
    public class WorldLocationMessageData
    {
        public float Scale;
        public Vector3 Position;
        public Vector3 lookAt;
        public Quaternion Orientation;
        public int WorldID;

        public void Parse(GameBitBuffer buffer)
        {
            Scale = buffer.ReadFloat32();
            Position = new Vector3();
            Position = Position.Parse(buffer);
            lookAt = new Vector3();
            lookAt = lookAt.Parse(buffer);
            Orientation = new Quaternion();
            Orientation = Orientation.Parse(buffer);
            WorldID = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Scale);
            Position.Encode(buffer);
            lookAt.Encode(buffer);
            Orientation.Encode(buffer);
            buffer.WriteInt(32, WorldID);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WorldLocationMessageData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Scale: " + Scale.ToString("G"));
            Position.AsText(b, pad);
            lookAt.AsText(b, pad);
            Orientation.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("WorldID: 0x" + WorldID.ToString("X8") + " (" + WorldID + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}
