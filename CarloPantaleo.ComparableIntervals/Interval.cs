using System;

namespace CarloPantaleo.ComparableIntervals {
    /// <summary>
    /// Represents an interval of type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the interval.</typeparam>
    public class Interval<T> where T : IComparable {
        /// <summary>
        /// The upper bound of the interval.
        /// </summary>
        public virtual Bound<T> UpperBound { get; }
        
        /// <summary>
        /// The lower bound of the interval.
        /// </summary>
        public virtual Bound<T> LowerBound { get; }

        #region Constructors

        /// <summary>
        /// Creates an interval given its bounds.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        private Interval(Bound<T> lowerBound, Bound<T> upperBound) {
            CheckBounds(lowerBound, upperBound);
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        /// <summary>
        /// Internal no-args constructor used to create an empty interval.
        /// </summary>
        internal Interval() {
        }

        /// <summary>
        /// Creates an open interval.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        public static Interval<T> Open(T lowerBound, T upperBound) {
            return FromBounds(Bound<T>.Open(lowerBound), Bound<T>.Open(upperBound));
        }

        /// <summary>
        /// Creates a closed interval.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        public static Interval<T> Closed(T lowerBound, T upperBound) {
            return FromBounds(Bound<T>.Closed(lowerBound), Bound<T>.Closed(upperBound));
        }

        /// <summary>
        /// Creates an interval with an open lower bound and a closed upper bound.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        public static Interval<T> OpenClosed(T lowerBound, T upperBound) {
            return FromBounds(Bound<T>.Open(lowerBound), Bound<T>.Closed(upperBound));
        }

        /// <summary>
        /// Creates an interval with a closed lower bound and an open upper bound.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        public static Interval<T> ClosedOpen(T lowerBound, T upperBound) {
            return FromBounds(Bound<T>.Closed(lowerBound), Bound<T>.Open(upperBound));
        }

        /// <summary>
        /// Creates an interval given its bounds.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The newly created interval.</returns>
        public static Interval<T> FromBounds(Bound<T> lowerBound, Bound<T> upperBound) {
            return lowerBound == upperBound && (lowerBound.IsOpen() || upperBound.IsOpen())
                ? new EmptyInterval<T>()
                : new Interval<T>(lowerBound, upperBound);
        }

        /// <summary>
        /// Creates an empty interval.
        /// </summary>
        /// <remarks>
        /// In order to check if an interval is empty, two methods may be used:
        /// <list type="number">
        /// <item>
        /// <description>Type checking: <code>interval is EmptyInterval&lt;T>;</code></description>
        /// </item>
        /// <item>
        /// <description><see cref="IsEmpty"/> method: <code>interval.IsEmpty();</code></description>
        /// </item>
        /// </list>
        /// Direct comparison is discouraged. This works: <code>interval == Interval&lt;T>.Empty();</code>
        /// but should be avoided as the solutions above are semantically better.
        /// </remarks>
        public static Interval<T> Empty() {
            return new EmptyInterval<T>();
        }

        private static void CheckBounds(Bound<T> lowerBound, Bound<T> upperBound) {
            if (lowerBound == null || upperBound == null) {
                throw new ArgumentException("Bounds cannot be null.");
            }

            if (lowerBound.IsPositiveInfinity()) {
                throw new ArgumentException("Lower bound cannot be positive infinity.");
            }

            if (upperBound.IsNegativeInfinity()) {
                throw new ArgumentException("Upper bound cannot be negative infinity.");
            }

            if (!lowerBound.IsNegativeInfinity() && (T) lowerBound > upperBound ||
                !upperBound.IsPositiveInfinity() && lowerBound > (T) upperBound) {
                throw new ArgumentException("Lower bound must be <= upper bound.");
            }
        }

        #endregion

        #region Equality Operators

        private bool Equals(Interval<T> other) {
            if (other is EmptyInterval<T>) {
                return this is EmptyInterval<T>;
            }

            return UpperBound.Equals(other.UpperBound) && LowerBound.Equals(other.LowerBound);
        }

        ///<inheritdoc/>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Interval<T>) obj);
        }

        ///<inheritdoc/>
        public override int GetHashCode() {
            unchecked {
                return (UpperBound.GetHashCode() * 397) ^ LowerBound.GetHashCode();
            }
        }

        /// <summary>
        /// Checks if two intervals are equal.
        /// </summary>
        public static bool operator ==(Interval<T> left, Interval<T> right) {
            return Equals(left, right);
        }

        /// <summary>
        /// Checks if two intervals are not equal.
        /// </summary>
        public static bool operator !=(Interval<T> left, Interval<T> right) {
            return !Equals(left, right);
        }

        #endregion

        /// <summary>
        /// Checks if an interval is empty.
        /// </summary>
        public virtual bool IsEmpty() => false;

        ///<inheritdoc/>
        public override string ToString() {
            return $"{(LowerBound.IsClosed() ? "[" : "(")}{GetVal(LowerBound)}, " +
                   $"{GetVal(UpperBound)}{(UpperBound.IsClosed() ? "]" : ")")}";

            string GetVal(Bound<T> bound) {
                if (bound.IsNegativeInfinity()) {
                    return "-∞";
                }

                if (bound.IsPositiveInfinity()) {
                    return "∞";
                }

                return ((T) bound).ToString();
            }
        }
    }


    /// <summary>
    /// Interval operations are implemented as extension methods, so they can be called as static or instance methods.
    /// </summary>
    public static class IntervalExtensions {
        /// <summary>
        /// Gets the intersection of two intervals.
        /// </summary>
        /// <param name="first">The first interval.</param>
        /// <param name="second">The second interval.</param>
        /// <typeparam name="T">The <see cref="IComparable"/> type of the interval.</typeparam>
        /// <returns>An interval representing the intersection.</returns>
        public static Interval<T> Intersection<T>(this Interval<T> first, Interval<T> second) where T : IComparable {
            if (first.IsEmpty() || second.IsEmpty()) {
                return Interval<T>.Empty();
            }

            return UncheckedIntersection(first, second);
        }

        /// <summary>
        /// Gets the union of two overlapping intervals
        /// </summary>
        /// <param name="first">The first interval.</param>
        /// <param name="second">The second interval.</param>
        /// <typeparam name="T">The <see cref="IComparable"/> type of the interval.</typeparam>
        /// <returns>An interval representing the union.</returns>
        /// <remarks>
        /// If the two intervals are not overlapping, an <see cref="EmptyInterval{T}"/> will be returned.
        /// </remarks>
        public static Interval<T> Union<T>(this Interval<T> first, Interval<T> second) where T : IComparable {
            if (first.IsEmpty()) {
                return second;
            }

            if (second.IsEmpty()) {
                return first;
            }

            if (UncheckedIntersection(first, second).IsEmpty()) {
                return Interval<T>.Empty();
            }
            
            var lowerBound = LowerMin(first.LowerBound, second.LowerBound);
            var upperBound = UpperMax(first.UpperBound, second.UpperBound);
            return Interval<T>.FromBounds(lowerBound, upperBound);
        }

        private static Interval<T> UncheckedIntersection<T>(Interval<T> first, Interval<T> second) where T : IComparable {
            var lowerBound = LowerMax(first.LowerBound, second.LowerBound);
            var upperBound = UpperMin(first.UpperBound, second.UpperBound);

            return lowerBound > upperBound
                ? Interval<T>.Empty()
                : Interval<T>.FromBounds(lowerBound, upperBound);
        }

        private static Bound<T> LowerMin<T>(Bound<T> left, Bound<T> right) where T : IComparable {
            if (left < right && left > right) {
                return left.IsClosed() ? left : right;
            }

            return left <= right ? left : right;
        }

        private static Bound<T> UpperMin<T>(Bound<T> left, Bound<T> right) where T : IComparable {
            if (left < right && left > right) {
                return left.IsOpen() ? left : right;
            }

            return left <= right ? left : right;
        }

        private static Bound<T> LowerMax<T>(Bound<T> left, Bound<T> right) where T : IComparable {
            if (left < right && left > right) {
                return left.IsOpen() ? left : right;
            }

            return left >= right ? left : right;
        }

        private static Bound<T> UpperMax<T>(Bound<T> left, Bound<T> right) where T : IComparable {
            if (left < right && left > right) {
                return left.IsClosed() ? left : right;
            }

            return left >= right ? left : right;
        }
    }

    /// <summary>
    /// Represents an empty interval.
    /// </summary>
    /// <typeparam name="T">The type of the empty interval.</typeparam>
    public class EmptyInterval<T> : Interval<T> where T : IComparable {
        /// <summary>
        /// The upper bound. Calling this property on an empty interval will always throw a
        /// <see cref="NullReferenceException"/>.
        /// </summary>
        public override Bound<T> UpperBound =>
            throw new NullReferenceException("Upper bound is undefined on empty interval.");

        /// <summary>
        /// The lower bound. Calling this property on an empty interval will always throw a
        /// <see cref="NullReferenceException"/>.
        /// </summary>
        public override Bound<T> LowerBound =>
            throw new NullReferenceException("Lower bound is undefined on empty interval.");

       /// <inheritdoc/>
        public override string ToString() {
            return "∅";
        }

       /// <inheritdoc/>
       public override bool IsEmpty() => true;
    }
}