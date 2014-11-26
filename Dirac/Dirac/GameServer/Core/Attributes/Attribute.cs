using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Dirac.GameServer.Core
{
    public class Attribute
    {
        public const float Float16Min = -65536.0f;
        public const float Float16Max = 65536.0f;

        public int Id;
        public AttributeValue _defaultValue;
        public int U3;
        public int U4;
        public int U5;

        public string ScriptA;
        public string ScriptB;
        public string Name;

        public AttributeEncoding EncodingType;

        public byte U10;

        public AttributeValue Min;
        public AttributeValue Max;
        public int BitCount;


        public bool IsInteger { get { return EncodingType == AttributeEncoding.Int || EncodingType == AttributeEncoding.IntMinMax; } }

        public bool ScriptedAndSettable = false;

        public Attribute() {  }

        public Attribute(int id, int defaultValue, int u3, int u4, int u5, string scriptA, string scriptB, string name, AttributeEncoding encodingType, byte u10, int min, int max, int bitCount)
        {
            Id = id;
            _defaultValue.Value = defaultValue;
            U3 = u3;
            U4 = u4;
            U5 = u5;
            ScriptA = scriptA;
            ScriptB = scriptB;
            Name = name;
            EncodingType = encodingType;
            U10 = u10;

            Min = new AttributeValue(min);
            Max = new AttributeValue(max);
            BitCount = bitCount;
        }

        public Attribute(int id, float defaultValue, int u3, int u4, int u5, string scriptA, string scriptB, string name, AttributeEncoding encodingType, byte u10, float min, float max, int bitCount)
        {
            Id = id;
            _defaultValue.ValueF = defaultValue;
            U3 = u3;
            U4 = u4;
            U5 = u5;
            ScriptA = scriptA;
            ScriptB = scriptB;
            Name = name;
            EncodingType = encodingType;
            U10 = u10;

            Min = new AttributeValue(min);
            Max = new AttributeValue(max);

            BitCount = bitCount;
        }
    }

    public enum AttributeEncoding
    {
        Int,
        IntMinMax,
        Float16,
        Float16Or32,
        Float32,
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct AttributeValue
    {
        [FieldOffset(0)]
        public int Value;
        [FieldOffset(0)]
        public float ValueF;

        public AttributeValue(int value) { ValueF = 0f; Value = value; }
        public AttributeValue(float value) { Value = 0; ValueF = value; }
    }


    

    

    


}
