using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Gibbed.IO;

namespace Dirac.Store.FileFormats
{
    public class NavMeshDef
    {
        public int SquaresCountX { get; set; }
        public int SquaresCountY { get; set; }
        public float SquareSize { get; set; }
        public int NavMeshSquareCount { get; set; }
        public NavMeshSquare[,] WalkGrid;

        public NavMeshDef() { }

        public NavMeshDef(String FileName)
        {
            FileStream stream = new FileStream(FileName, FileMode.Open);
            SquaresCountX = stream.ReadInt32();
            SquaresCountY = stream.ReadInt32();
            WalkGrid = new NavMeshSquare[SquaresCountX, SquaresCountY];
            SquareSize = stream.ReadFloat32();
            NavMeshSquareCount = stream.ReadInt32();
            for (int i = 0; i < SquaresCountX; i++)
            {
                for (int j = 0; j < SquaresCountY; j++)
                {
                    WalkGrid[i,j] = new NavMeshSquare(stream);
                }
            }
            stream.Close();
        }

        public void Save(String FileName)
        {
            FileStream stream = new FileStream(FileName, FileMode.OpenOrCreate);
            stream.WriteInt32(SquaresCountX);
            stream.WriteInt32(SquaresCountY);
            stream.WriteFloat32(SquareSize);
            stream.WriteInt32(NavMeshSquareCount);
            for (int i = 0; i < SquaresCountX; i++)
            {
                for (int j = 0; j < SquaresCountY; j++)
                {
                    WalkGrid[i, j].Save(stream);
                }
            }
            stream.Flush();
            stream.Close();
        }
    }

    public class NavMeshSquare
    {

        public float Z { get; set; }
        public NavCellFlags Flags { get; set; }

        public NavMeshSquare() { }
        public NavMeshSquare(Stream stream)
        {
            Z = stream.ReadFloat32();
            Flags = (NavCellFlags)stream.ReadInt32();
        }
        public void Save(Stream stream)
        {
            stream.WriteFloat32(Z);
            stream.WriteInt32((int)Flags);
        }
    }

    [Flags]
    public enum NavCellFlags : int
    {
        AllowWalk = 0x1,
        AllowFlier = 0x2,
        AllowSpider = 0x4,
        LevelAreaBit0 = 0x8,
        LevelAreaBit1 = 0x10,
        NoNavMeshIntersected = 0x20,
        NoSpawn = 0x40,
        Special0 = 0x80,
        Special1 = 0x100,
        SymbolNotFound = 0x200,
        AllowProjectile = 0x400,
        AllowGhost = 0x800,
        RoundedCorner0 = 0x1000,
        RoundedCorner1 = 0x2000,
        RoundedCorner2 = 0x4000,
        RoundedCorner3 = 0x8000
    }


}
