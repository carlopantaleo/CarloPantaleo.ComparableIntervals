using System;
using System.Collections.Generic;
using System.Linq;

namespace CarloPantaleo.ComparableIntervals {
    /// <summary>
    /// Utility class which provides some static methods to handle collections of intervals.
    /// </summary>
    public static class Intervals {
        /// <summary>
        /// Creates a list of intervals without overlapping intervals by performing a union on the overlapping
        /// intervals.
        /// </summary>
        /// <param name="intervals">The collection of intervals to flatten.</param>
        /// <typeparam name="T">The <see cref="IComparable"/> type of the interval.</typeparam>
        /// <returns>The resulting flattened collection.</returns>
        public static List<Interval<T>> Flatten<T>(ICollection<Interval<T>> intervals) where T : IComparable {
            if (intervals == null || !intervals.Any()) {
                return new List<Interval<T>>();
            }

            var cleanIntervals = RemoveEmptyIntervals(intervals);
            var resultingIntervals = PerformFlatten(cleanIntervals);
            SortByLowerBound(resultingIntervals);

            return resultingIntervals;
        }

        private static List<Interval<T>> PerformFlatten<T>(ICollection<Interval<T>> intervals) where T : IComparable {
            var resultingIntervals = new List<Interval<T>>();
            var ignoredIntervals = new List<Interval<T>>(); // Intervals already unified with others.

            foreach (var inspectedInterval in intervals) {
                var resultingInterval = inspectedInterval;
                var intervalsToProcess = intervals
                    .SkipWhile(i =>
                        !ReferenceEquals(i, inspectedInterval)) // Skip intervals previous to the inspected one.
                    .Skip(1) // Skip one more interval (which is exactly the inspected one).
                    .Where(i =>
                        !ignoredIntervals.Any(ii =>
                            ReferenceEquals(ii, i))); // Exclude already ignored intervals references (see note below).
                foreach (var interval in intervalsToProcess) {
                    if (!resultingInterval.Intersection(interval).IsEmpty()) {
                        resultingInterval = resultingInterval.Union(interval);
                        ignoredIntervals.Add(interval);
                    }
                }

                /*
                 * Here checking if ignoredIntervals contains resultingInterval is not enough. We have to check if the
                 * reference of resultingInterval is contained in ignoredIntervals, because resultingInterval may be a
                 * union equal (by value) to a interval contained in ignoredInterval, but in that case it's a valid
                 * interval to be added to the result (consider the case [2, 3], [1, 4] => the resulting interval has
                 * to be [1, 4]).
                 */
                if (!ignoredIntervals.Any(ii => ReferenceEquals(ii, resultingInterval))) {
                    resultingIntervals.Add(resultingInterval);
                }
            }

            return resultingIntervals;
        }

        private static ICollection<Interval<T>> RemoveEmptyIntervals<T>(IEnumerable<Interval<T>> intervals)
            where T : IComparable => intervals.Where(i => !i.IsEmpty()).ToList();

        private static void SortByLowerBound<T>(List<Interval<T>> intervals) where T : IComparable =>
            intervals.Sort((i, j) => i.LowerBound < j.LowerBound ? -1 : 1);

        /// <summary>
        /// Creates a list of intervals which is the resulting union of the passed collections of intervals.
        /// </summary>
        /// <param name="collections">The collections of intervals.</param>
        /// <typeparam name="T">The <see cref="IComparable"/> type of the interval.</typeparam>
        /// <returns>The resulting union.</returns>
        public static List<Interval<T>> Union<T>(params IEnumerable<Interval<T>>[] collections)
            where T : IComparable {
            if (collections == null || collections.Length == 0) {
                return new List<Interval<T>>();
            }

            var joinedList = new List<Interval<T>>();
            foreach (var collection in collections) {
                joinedList.AddRange(collection);
            }

            return Flatten(joinedList);
        }

        /// <summary>
        /// Creates a list of intervals which is the intersection of the passed collections of intervals.
        /// </summary>
        /// <param name="collections">
        /// The collections of intervals to intersect. Since each collection should not have overlapping intervals, a
        /// flattening (see <see cref="Flatten{T}"/> will be performed on each collection before processing.
        /// </param>
        /// <typeparam name="T">The <see cref="IComparable"/> type of the interval.</typeparam>
        /// <returns>The resulting intersection.</returns>
        public static List<Interval<T>> Intersection<T>(params ICollection<Interval<T>>[] collections)
            where T : IComparable {
            if (collections == null || collections.Length == 0) {
                return new List<Interval<T>>();
            }

            if (collections.Length == 1) {
                return Flatten(collections[0]);
            }

            return collections.Aggregate(PerformIntersection) as List<Interval<T>>;
        }

        private static List<Interval<T>> PerformIntersection<T>(IEnumerable<Interval<T>> first,
                                                                ICollection<Interval<T>> second) where T : IComparable {
            var resultingIntervals = new List<Interval<T>>();

            // This brute force approach is not the most efficient, but I won't prematurely optimise it unless needed.
            foreach (var inspectedInterval in first) {
                foreach (var interval in second) {
                    var intersection = inspectedInterval.Intersection(interval);
                    if (!intersection.IsEmpty()) {
                        resultingIntervals.Add(intersection);
                    }
                }
            }

            return Flatten(resultingIntervals);
        }

        /// <summary>
        /// Creates a list of intervals which is the complement of the passed collection of intervals.
        /// </summary>
        /// <param name="intervals">
        /// The collection of intervals to complement. Since this collection should not have overlapping intervals, a
        /// flattening (see <see cref="Flatten{T}"/> will be performed before processing.
        /// </param>
        /// <typeparam name="T">The <see cref="IComparable"/> type of the interval.</typeparam>
        /// <returns>The resulting complement.</returns>
        public static List<Interval<T>> Complement<T>(ICollection<Interval<T>> intervals) where T : IComparable {
            var normalizedIntervals = Flatten(intervals);
            if (normalizedIntervals.Count == 0) {
                return SingleIntervalComplement(Interval<T>.Empty());
            }

            var complementedIntervals = normalizedIntervals
                .Select(interval => SingleIntervalComplement(interval) as ICollection<Interval<T>>)
                .ToArray();
            return Intersection(complementedIntervals);
        }

        private static List<Interval<T>> SingleIntervalComplement<T>(Interval<T> interval) where T : IComparable {
            if (interval.IsEmpty()) {
                return new List<Interval<T>> {
                    Interval<T>.FromBounds(Bound<T>.NegativeInfinity(), Bound<T>.PositiveInfinity())
                };
            }

            if (interval.LowerBound.IsNegativeInfinity()) {
                if (interval.UpperBound.IsPositiveInfinity()) {
                    return new List<Interval<T>> {Interval<T>.Empty()};
                }

                return new List<Interval<T>> {UpperUnbounded()};
            }

            if (interval.UpperBound.IsPositiveInfinity()) {
                return new List<Interval<T>> {LowerUnbounded()};
            }
            
            return new List<Interval<T>> {LowerUnbounded(), UpperUnbounded()};

            // Local functions
            
            Interval<T> UpperUnbounded() {
                return Interval<T>.FromBounds(interval.UpperBound.IsOpen()
                    ? Bound<T>.Closed(interval.UpperBound)
                    : Bound<T>.Open(interval.UpperBound), Bound<T>.PositiveInfinity());
            }

            Interval<T> LowerUnbounded() {
                return Interval<T>.FromBounds(Bound<T>.NegativeInfinity(), interval.LowerBound.IsOpen()
                    ? Bound<T>.Closed(interval.LowerBound)
                    : Bound<T>.Open(interval.LowerBound));
            }
        }
    }
}