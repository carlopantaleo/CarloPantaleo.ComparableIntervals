using System;
using System.Collections.Generic;

namespace CarloPantaleo.ComparableIntervals {
    /// <summary>
    /// A bound to be used in <see cref="Interval{T}"/>s.
    /// </summary>
    /// <typeparam name="T">The type of the bound, which must be <see cref="IComparable"/>.</typeparam>
    /// <remarks>
    /// <see cref="Bound{T}"/> is not <see cref="IComparable"/>, however comparison operators are provided in order to
    /// compare objects of type <see cref="T"/> to the bound itself. For open or closed intervals, this is the same as
    /// casting the bound to <see cref="T"/> and comparing the two objects of type <see cref="T"/>. Those operators
    /// are eventually handy when comparing an object against an infinite interval.
    /// </remarks>
    public readonly struct Bound<T> where T : IComparable {
        private readonly T _value;
        public BoundType Type { get; }

        private Bound(BoundType type, T value = default) {
            Type = type;
            _value = value;
        }

        /// <summary>
        /// Creates a closed bound.
        /// </summary>
        /// <param name="value">The value of the bound.</param>
        /// <returns>The bound.</returns>
        /// <exception cref="ArgumentNullException">If <see cref="value"/> is null.</exception>
        public static Bound<T> Closed(T value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            return new Bound<T>(BoundType.Closed, value);
        }

        /// <summary>
        /// Creates an open bound.
        /// </summary>
        /// <param name="value">The value of the bound.</param>
        /// <returns>The bound.</returns>
        /// <exception cref="ArgumentNullException">If <see cref="value"/> is null.</exception>
        public static Bound<T> Open(T value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            return new Bound<T>(BoundType.Open, value);
        }

        /// <summary>
        /// Creates a bound representing negative infinite.
        /// </summary>
        /// <returns>The bound.</returns>
        public static Bound<T> NegativeInfinite() {
            return new Bound<T>(BoundType.NegativeInfinite);
        }

        /// <summary>
        /// Creates a bound representing positive infinite.
        /// </summary>
        /// <returns>The bound.</returns>
        public static Bound<T> PositiveInfinite() {
            return new Bound<T>(BoundType.PositiveInfinite);
        }

        /// <summary>
        /// Gets the exact value of the bound (if it's closed), or the limit it approaches to (if it's open). 
        /// </summary>
        /// <param name="bound">The bound to get the value of.</param>
        /// <exception cref="InvalidCastException">If the bound is not finite.</exception>
        public static implicit operator T(Bound<T> bound) {
            if (bound.Type == BoundType.NegativeInfinite || bound.Type == BoundType.PositiveInfinite) {
                throw new InvalidCastException("Cannot cast a bound which tends to infinity.");
            }

            return bound._value;
        }

        public static bool operator <(Bound<T> left, T right) {
            switch (left.Type) {
                case BoundType.NegativeInfinite:
                    return true;
                case BoundType.PositiveInfinite:
                    return false;
                default:
                    return ((T) left).CompareTo(right) < 0;
            }
        }
        
        public static bool operator <=(Bound<T> left, T right) {
            switch (left.Type) {
                case BoundType.NegativeInfinite:
                    return true;
                case BoundType.PositiveInfinite:
                    return false;
                default:
                    return ((T) left).CompareTo(right) <= 0;
            }
        }

        public static bool operator >(Bound<T> left, T right) {
            switch (left.Type) {
                case BoundType.NegativeInfinite:
                    return false;
                case BoundType.PositiveInfinite:
                    return true;
                default:
                    return ((T) left).CompareTo(right) > 0;
            }
        }        
        
        public static bool operator >=(Bound<T> left, T right) {
            switch (left.Type) {
                case BoundType.NegativeInfinite:
                    return false;
                case BoundType.PositiveInfinite:
                    return true;
                default:
                    return ((T) left).CompareTo(right) >= 0;
            }
        }
        
        public static bool operator <(T left, Bound<T> right) {
            switch (right.Type) {
                case BoundType.NegativeInfinite:
                    return false;
                case BoundType.PositiveInfinite:
                    return true;
                default:
                    return left.CompareTo((T) right) < 0;
            }
        }
        
        public static bool operator <=(T left, Bound<T> right) {
            switch (right.Type) {
                case BoundType.NegativeInfinite:
                    return false;
                case BoundType.PositiveInfinite:
                    return true;
                default:
                    return left.CompareTo((T) right) <= 0;
            }
        }
        public static bool operator >(T left, Bound<T> right) {
            switch (right.Type) {
                case BoundType.NegativeInfinite:
                    return true;
                case BoundType.PositiveInfinite:
                    return false;
                default:
                    return left.CompareTo((T) right) > 0;
            }
        }
        
        public static bool operator >=(T left, Bound<T> right) {
            switch (right.Type) {
                case BoundType.NegativeInfinite:
                    return true;
                case BoundType.PositiveInfinite:
                    return false;
                default:
                    return left.CompareTo((T) right) >= 0;
            }
        }

        public bool Equals(Bound<T> other) {
            return EqualityComparer<T>.Default.Equals(_value, other._value) && Type == other.Type;
        }

        public override bool Equals(object obj) {
            return obj is Bound<T> other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return (EqualityComparer<T>.Default.GetHashCode(_value) * 397) ^ (int) Type;
            }
        }

        public static bool operator ==(Bound<T> left, Bound<T> right) {
            return Equals(left, right);
        }

        public static bool operator !=(Bound<T> left, Bound<T> right) {
            return !Equals(left, right);
        }
    }

    /// <summary>
    /// Represents the bound type.
    /// </summary>
    public enum BoundType {
        /// <summary>
        /// A closed bound.
        /// </summary>
        Closed,

        /// <summary>
        /// An open bound.
        /// </summary>
        Open,

        /// <summary>
        /// A bound representing the negative infinite. It is always less than any other non-NegativeInfinite
        /// bound.
        /// </summary>
        NegativeInfinite,

        /// <summary>
        /// A bound representing the positive infinite. It is always greater than any other non-PositiveInfinite
        /// bound.
        /// </summary>
        PositiveInfinite
    }
}