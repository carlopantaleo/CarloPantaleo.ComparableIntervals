using System;
using System.Collections.Generic;

namespace CarloPantaleo.ComparableIntervals {
    /// <summary>
    /// A bound to be used in <see cref="Interval{T}"/>s.
    /// </summary>
    /// <typeparam name="T">The type of the bound, which must be <see cref="IComparable"/>.</typeparam>
    /// <remarks>
    /// <see cref="Bound{T}"/> is not <see cref="IComparable"/>, however comparison operators are provided and
    /// comparison between bounds is defined as follows:
    /// <ul>
    ///     <li>Comparing two negative or positive infinity bounds always returns false.</li>
    ///     <li>A negative infinity bound is always less than a positive infinity one and any other finite value.</li>
    ///     <li>A positive infinity bound is always greater than a negative infinity one and any other finite value.</li>
    ///     <li>If the boundary value of the two bounds is the same and the two bounds are both open or closed, the two
    ///     bounds are considered equal.</li>
    ///     <li>If the boundary value of the two bounds is the same, one of the two bounds is open and the other one
    ///     is closed, the comparison is undefined (i.e., with <c>a</c> closed and <c>b</c> open, <c>a > b</c> and
    ///     <c>a &lt; b</c> are both true, while <c>a == b</c> is false).</li>
    /// </ul>
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
        /// <param name="value">The boundary value.</param>
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
        /// <param name="value">The boundary value.</param>
        /// <returns>The bound.</returns>
        /// <exception cref="ArgumentNullException">If <see cref="value"/> is null.</exception>
        public static Bound<T> Open(T value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            return new Bound<T>(BoundType.Open, value);
        }

        /// <summary>
        /// Creates a bound representing negative infinity.
        /// </summary>
        /// <returns>The bound.</returns>
        public static Bound<T> NegativeInfinity() {
            return new Bound<T>(BoundType.NegativeInfinity);
        }

        /// <summary>
        /// Creates a bound representing positive infinity.
        /// </summary>
        /// <returns>The bound.</returns>
        public static Bound<T> PositiveInfinity() {
            return new Bound<T>(BoundType.PositiveInfinity);
        }

        /// <summary>
        /// Gets the boundary value. 
        /// </summary>
        /// <param name="bound">The bound to get the boundary value of.</param>
        /// <exception cref="InvalidCastException">If the bound is not finite.</exception>
        public static implicit operator T(Bound<T> bound) {
            if (bound.Type == BoundType.NegativeInfinity || bound.Type == BoundType.PositiveInfinity) {
                throw new InvalidCastException("Cannot cast a bound which tends to infinity.");
            }

            return bound._value;
        }

        public static bool operator <(Bound<T> left, T right) => (Compare(right, left) ?? 1) > 0;

        public static bool operator <=(Bound<T> left, T right) => (Compare(right, left) ?? 1) >= 0;

        public static bool operator >(Bound<T> left, T right) => (Compare(right, left) ?? -1) < 0;

        public static bool operator >=(Bound<T> left, T right) => (Compare(right, left) ?? -1) <= 0;

        public static bool operator <(T left, Bound<T> right) => (Compare(left, right) ?? -1) < 0;

        public static bool operator <=(T left, Bound<T> right) => (Compare(left, right) ?? -1) <= 0;

        public static bool operator >(T left, Bound<T> right) => (Compare(left, right) ?? 1) > 0;

        public static bool operator >=(T left, Bound<T> right) => (Compare(left, right) ?? 1) >= 0;

        public static bool operator <(Bound<T> left, Bound<T> right) =>
            (Compare(left, right) ??
             (left.Type == BoundType.PositiveInfinity || left.Type == BoundType.NegativeInfinity ? 1 : -1)) < 0;

        public static bool operator <=(Bound<T> left, Bound<T> right) => 
            (Compare(left, right) ?? 
             (left.Type == BoundType.PositiveInfinity || left.Type == BoundType.NegativeInfinity ? 1 : -1)) <= 0;

        public static bool operator >(Bound<T> left, Bound<T> right) => 
            (Compare(left, right) ?? 
             (left.Type == BoundType.PositiveInfinity || left.Type == BoundType.NegativeInfinity ? -1 : 1)) > 0;

        public static bool operator >=(Bound<T> left, Bound<T> right) => 
            (Compare(left, right) ?? 
             (left.Type == BoundType.PositiveInfinity || left.Type == BoundType.NegativeInfinity ? -1 : 1)) >= 0;

        private static int? Compare(T left, Bound<T> right) {
            switch (right.Type) {
                case BoundType.NegativeInfinity:
                    return 1;
                case BoundType.PositiveInfinity:
                    return -1;
                case BoundType.Closed:
                    return left.CompareTo((T) right);
                default: {
                    int cmp = left.CompareTo((T) right);
                    return cmp == 0 ? (int?) null : cmp;
                }
            }
        }

        private static int? Compare(Bound<T> left, Bound<T> right) {
            if (left.Type == right.Type &&
                (left.Type == BoundType.NegativeInfinity || left.Type == BoundType.PositiveInfinity)) {
                return null;
            }

            if (left.Type == BoundType.NegativeInfinity || right.Type == BoundType.PositiveInfinity) {
                return -1;
            }

            if (left.Type == BoundType.PositiveInfinity || right.Type == BoundType.NegativeInfinity) {
                return 1;
            }

            if (EqualityComparer<T>.Default.Equals(left._value, right._value)) {
                return left.Type == right.Type ? (int?) 0 : null;
            }

            return left._value.CompareTo(right._value);
        }

        /// <summary>
        /// Two bounds are considered equal if their type (open, closed) and their values are equal. infinity bounds
        /// are always considered not equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Bound<T> other) {
            return Type != BoundType.PositiveInfinity &&
                   Type != BoundType.NegativeInfinity &&
                   EqualityComparer<T>.Default.Equals(_value, other._value) && Type == other.Type;
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

        public override string ToString() {
            switch (Type) {
                case BoundType.Closed:
                    return $"[{_value}]";
                case BoundType.Open:
                    return $"({_value})";
                case BoundType.NegativeInfinity:
                    return "-∞";
                case BoundType.PositiveInfinity:
                    return "∞";
                default:
                    return base.ToString();
            }
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
        /// A bound representing the negative infinity. It is always less than any other non-Negativeinfinity
        /// bound.
        /// </summary>
        NegativeInfinity,

        /// <summary>
        /// A bound representing the positive infinity. It is always greater than any other non-Positiveinfinity
        /// bound.
        /// </summary>
        PositiveInfinity
    }
}