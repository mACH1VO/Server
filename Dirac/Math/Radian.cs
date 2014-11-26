using Numeric = System.Single;

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Dirac.Math
{
    /// <summary>
    /// Wrapper class which indicates a given angle value is in Radians.
    /// </summary>
    /// <remarks>
    /// Radian values are interchangeable with Degree values, and conversions
    /// will be done automatically between them.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct Radian : ISerializable, IComparable<Radian>, IComparable<Degree>
    {
        private static readonly float _radiansToDegrees = 180.0f / Utility.PI;

        public static readonly Radian Zero = (Radian)0;

        private float _value;

        public Radian(float r)
        {
            _value = r;
        }

        public Radian(Radian r)
        {
            _value = r._value;
        }

        public Radian(Degree d)
        {
            _value = d.InRadians;
        }

        public Degree InDegrees { get { return _value * _radiansToDegrees; } }

        public static implicit operator Radian(float value)
        {
            Radian retVal;
            retVal._value = value;
            return retVal;
        }

        public static implicit operator Radian(Degree value)
        {
            Radian retVal;
            retVal._value = value;
            return retVal;
        }


        public static explicit operator Radian(int value)
        {
            Radian retVal;
            retVal._value = value;
            return retVal;
        }

        public static implicit operator float(Radian value)
        {
            return (float)value._value;
        }

        public static Radian operator +(Radian left, Radian right)
        {
            return left._value + right._value;
        }

        public static Radian operator +(Radian left, Degree right)
        {
            return left + right.InRadians;
        }

        public static Radian operator -(Radian r)
        {
            return -r._value;
        }

        public static Radian operator -(Radian left, Radian right)
        {
            return left._value - right._value;
        }

        public static Radian operator -(Radian left, Degree right)
        {
            return left - right.InRadians;
        }


        public static Radian operator *(Radian left, Radian right)
        {
            return left._value * right._value;
        }

        public static Radian operator *(Radian left, Degree right)
        {
            return left._value * right.InRadians;
        }


        public static bool operator <(Radian left, Radian right)
        {
            return left._value < right._value;
        }

        public static bool operator ==(Radian left, Radian right)
        {
            return left._value == right._value;
        }

        public static bool operator !=(Radian left, Radian right)
        {
            return left._value != right._value;
        }

        public static bool operator >(Radian left, Radian right)
        {
            return left._value > right._value;
        }

        public override bool Equals(object obj)
        {
            return (obj is Radian && this == (Radian)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

#if !( XBOX || XBOX360 )

        #region ISerializable Implementation

        private Radian(SerializationInfo info, StreamingContext context)
        {
            _value = (float)info.GetValue("value", typeof(float));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("value", _value);
        }

        #endregion ISerializable Implementation

#endif

        #region IComparable<T> Members

        public int CompareTo(Radian other)
        {
            return this._value.CompareTo(other._value);
        }

        public int CompareTo(Degree other)
        {
            return this._value.CompareTo(other.InRadians);
        }

        public int CompareTo(float other)
        {
            return this._value.CompareTo(other);
        }

        #endregion IComparable<T> Members
    }
}