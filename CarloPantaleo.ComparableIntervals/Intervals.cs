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
        /// <typeparam name="T">The <see cref="IComparable"/> type of the interval.</typeparam>
        /// <returns>The resulting flattened collection.</returns>
        public static List<Interval<T>> Flatten<T>(IEnumerable<Interval<T>> intervals) where T : IComparable {
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

        /// <summary>
        /// Creates a list of intervals which is the resulting union of the passed collections of intervals.
        /// </summary>
        /// <param name="collections">The collections of intervals.</param>
        /// <typeparam name="T">The <see cref="IComparable"/> type of the interval.</typeparam>
        /// <returns>The resulting union.</returns>
        public static List<Interval<T>> Union<T>(params IEnumerable<Interval<T>>[] collections)
            where T : IComparable {
            var joinedList = new List<Interval<T>>();
            foreach (var collection in collections) {
                joinedList.AddRange(collection);
            }

            return Flatten(joinedList);
        }

        private static ICollection<Interval<T>> RemoveEmptyIntervals<T>(IEnumerable<Interval<T>> intervals)
            where T : IComparable => intervals.Where(i => !i.IsEmpty()).ToList();

        private static void SortByLowerBound<T>(List<Interval<T>> intervals) where T : IComparable =>
            intervals.Sort((i, j) => i.LowerBound < j.LowerBound ? -1 : 1);
    }
}