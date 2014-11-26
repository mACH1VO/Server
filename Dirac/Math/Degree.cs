using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Dirac.Math
{
    /// <summary>
    /// Wrapper class which indicates a given angle value is in Radian.
    /// </summary>
    /// <remarks>
    /// Degree values are interchangeable with Radian values, and conversions
    /// will be done automatically between them.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential), Serializable]
    public struct Degree : ISerializable, IComparable<Degree>, IComparable<Radian>, IComparable<float>
    {
        private static readonly float _degreesToRadians = Utility.PI / 180.0f;

        public static readonly Degree Zero = (Degree)0;

        private float _value;

        public Degree(float r)
        {
            _value = r;
        }

        public Degree(Degree d)
        {
            _value = d._value;
        }

        public Degree(Radian r)
        {
            _value = r.InDegrees;
        }

        public Radian InRadians { get { return _value * _degreesToRadians; } }

        public static implicit operator Degree(float value)
        {
            Degree retVal;
            retVal._value = value;
            return retVal;
        }

        public static implicit operator Degree(Radian value)
        {
            Degree retVal;
            retVal._value = value;
            return retVal;
        }

        public static explicit operator Degree(int value)
        {
            Degree retVal;
            retVal._value = value;
            return retVal;
        }

        public static implicit operator float(Degree value)
        {
            return (float)value._value;
        }

        public static Degree operator +(Degree left, float right)
        {
            return left._value + right;
        }

        public static Degree operator +(Degree left, Degree right)
        {
            return left._value + right._value;
        }

        public static Degree operator +(Degree left, Radian right)
        {
            return left + right.InDegrees;
        }

        public static Degree operator -(Degree r)
        {
            return -r._value;
        }

        public static Degree operator -(Degree left, Degree right)
        {
            return left._value - right._value;
        }

        public static Degree operator -(Degree left, Radian right)
        {
            return left - right.InDegrees;
        }

        public static Degree operator *(Degree left, Degree right)
        {
            return left._value * right._value;
        }

        public static Degree operator *(Degree left, Radian right)
        {
            return left._value * right.InDegrees;
        }

        public static Degree operator /(Degree left, float right)
        {
            return left._value / right;
        }

        public static bool operator <(Degree left, Degree right)
        {
            return left._value < right._value;
        }

        public static bool operator ==(Degree left, Degree right)
        {
            return left._value == right._value;
        }

        public static bool operator !=(Degree left, Degree right)
        {
            return left._value != right._value;
        }

        public static bool operator >(Degree left, Degree right)
        {
            return left._value > right._value;
        }

        public override bool Equals(object obj)
        {
            return (obj is Degree && this == (Degree)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

#if !( XBOX || XBOX360 )

        #region ISerializable Implementation

        private Degree(SerializationInfo info, StreamingContext context)
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

        public int CompareTo(Degree other)
        {
            return this._value.CompareTo(other);
        }

        public int CompareTo(Radian other)
        {
            return this._value.CompareTo(other.InDegrees);
        }

        public int CompareTo(float other)
        {
            return this._value.CompareTo(other);
        }

        #endregion IComparable<T> Members
    }
}