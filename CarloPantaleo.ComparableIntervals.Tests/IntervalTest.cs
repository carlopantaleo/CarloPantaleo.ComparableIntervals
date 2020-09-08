using System.Collections.Generic;
using Xunit;

namespace CarloPantaleo.ComparableIntervals.Tests {
    public class IntervalTest {
        [Theory]
        [MemberData(nameof(IntersectionData))]
        public void Intersection(Interval<int> first, Interval<int> second, Interval<int> expected) {
            var actual = first.Intersection(second);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> IntersectionData =>
            new List<object[]> {
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(0,1), Interval<int>.Closed(1, 1)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(5,6), Interval<int>.Closed(5, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(4,7), Interval<int>.Closed(4, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(2,4), Interval<int>.Closed(2, 4)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(0,6), Interval<int>.Closed(1, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(6,7), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(0,1), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(5,6), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(4,7), Interval<int>.Open(4, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(2,4), Interval<int>.Open(2, 4)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(0,6), Interval<int>.Open(1, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(6,7), Interval<int>.Empty()},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(0,1), Interval<int>.Empty()},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(5,6), Interval<int>.Empty()},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(4,7), Interval<int>.OpenClosed(4, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(2,4), Interval<int>.Open(2, 4)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(0,6), Interval<int>.Closed(1, 5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(6,7), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(0,1), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(5,6), Interval<int>.Empty()},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(4,7), Interval<int>.ClosedOpen(4, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(2,4), Interval<int>.Closed(2, 4)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(0,6), Interval<int>.Open(1, 5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Closed(6,7), Interval<int>.Empty()},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Closed(1,5), Interval<int>.Closed(1,5)},
                new object[] {Interval<int>.Closed(1, 5), Interval<int>.Open(1,5), Interval<int>.Open(1,5)},
                new object[] {Interval<int>.Open(1, 5), Interval<int>.Open(1,5), Interval<int>.Open(1,5)},
            };
    }
}