using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CarloPantaleo.ComparableIntervals.Tests {
    public class IntervalsTest {
        [Theory]
        [MemberData(nameof(FlattenData))]
        public void Flatten(List<Interval<int>> input, List<Interval<int>> expectedOutput) {
            var output = Intervals.Flatten(input);
            // Comparison is done on the string representation of intervals in order to handle infinity cases which are
            // not comparable.
            Assert.Equal(expectedOutput.Select(i => i.ToString()), output.Select(i => i.ToString()));
        }
        
        [Theory]
        [MemberData(nameof(UnionData))]
        public void Union(List<Interval<int>> first,List<Interval<int>> second, List<Interval<int>> expectedOutput) {
            var output = Intervals.Union(first, second);
            // Comparison is done on the string representation of intervals in order to handle infinity cases which are
            // not comparable.
            Assert.Equal(expectedOutput.Select(i => i.ToString()), output.Select(i => i.ToString()));
        }

        public static IEnumerable<object[]> FlattenData =>
            new List<object[]> {
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Closed(1, 3), Interval<int>.Open(4, 7), Interval<int>.Closed(5, 8)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(1, 3), Interval<int>.OpenClosed(4, 8)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Open(4, 7), Interval<int>.Closed(5, 8), Interval<int>.Closed(1, 3)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(1, 3), Interval<int>.OpenClosed(4, 8)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.OpenClosed(0, 3), Interval<int>.Open(1, 3), Interval<int>.Open(4, 7),
                        Interval<int>.Closed(5, 8)
                    },
                    new List<Interval<int>> {
                        Interval<int>.OpenClosed(0, 3), Interval<int>.OpenClosed(4, 8)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.OpenClosed(0, 3), Interval<int>.Open(1, 3), Interval<int>.Open(2, 4),
                        Interval<int>.Open(5, 7), Interval<int>.Closed(5, 9)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Open(0, 4), Interval<int>.Closed(5, 9)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Closed(5, 9), Interval<int>.OpenClosed(0, 3), Interval<int>.Open(1, 3),
                        Interval<int>.Open(2, 4), Interval<int>.Open(5, 7)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Open(0, 4), Interval<int>.Closed(5, 9)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Closed(5, 9), Interval<int>.OpenClosed(0, 3), Interval<int>.Open(1, 3),
                        Interval<int>.Open(2, 4), Interval<int>.Open(5, 7), Interval<int>.Empty()
                    },
                    new List<Interval<int>> {
                        Interval<int>.Open(0, 4), Interval<int>.Closed(5, 9)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Closed(0, 1), Interval<int>.OpenClosed(3, 4), Interval<int>.Open(6, 7)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(0, 1), Interval<int>.OpenClosed(3, 4), Interval<int>.Open(6, 7)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Closed(0, 1),
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity()),
                        Interval<int>.Open(6, 7)
                    },
                    new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity())
                    }
                },
            };

        public static IEnumerable<object[]> UnionData =>
            new List<object[]> {
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Closed(1, 3), Interval<int>.Open(4, 7)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(5, 8)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(1, 3), Interval<int>.OpenClosed(4, 8)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Open(4, 7), Interval<int>.Closed(5, 8)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(5, 8), Interval<int>.Closed(1, 3)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(1, 3), Interval<int>.OpenClosed(4, 8)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Open(1, 3), Interval<int>.Open(4, 7), Interval<int>.Closed(5, 8)
                    },
                    new List<Interval<int>> {
                        Interval<int>.OpenClosed(0, 3), Interval<int>.Open(1, 3), Interval<int>.Closed(5, 8)
                    },
                    new List<Interval<int>> {
                        Interval<int>.OpenClosed(0, 3), Interval<int>.OpenClosed(4, 8)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.OpenClosed(0, 3), Interval<int>.Open(1, 3), Interval<int>.Open(2, 4),
                        Interval<int>.Open(5, 7)
                    },
                    new List<Interval<int>> {
                        Interval<int>.OpenClosed(0, 3), Interval<int>.Open(2, 4),
                        Interval<int>.Open(5, 7), Interval<int>.Closed(5, 9)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Open(0, 4), Interval<int>.Closed(5, 9)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.OpenClosed(0, 3), Interval<int>.Open(1, 3),
                        Interval<int>.Open(2, 4), Interval<int>.Open(5, 7)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(5, 9)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Open(0, 4), Interval<int>.Closed(5, 9)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Closed(5, 9), Interval<int>.OpenClosed(0, 3), Interval<int>.Open(1, 3),
                        Interval<int>.Open(2, 4), Interval<int>.Open(5, 7), Interval<int>.Empty()
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(5, 9), Interval<int>.OpenClosed(0, 3), Interval<int>.Open(1, 3),
                        Interval<int>.Open(2, 4), Interval<int>.Open(5, 7), Interval<int>.Empty()
                    },
                    new List<Interval<int>> {
                        Interval<int>.Open(0, 4), Interval<int>.Closed(5, 9)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Closed(0, 1), Interval<int>.OpenClosed(3, 4),
                    },
                    new List<Interval<int>> {
                        Interval<int>.Open(6, 7)
                    },
                    new List<Interval<int>> {
                        Interval<int>.Closed(0, 1), Interval<int>.OpenClosed(3, 4), Interval<int>.Open(6, 7)
                    }
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.Closed(0, 1),
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity()),
                        Interval<int>.Open(6, 7)
                    },
                    new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity()),
                    },
                    new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity())
                    }
                },
            };
    }
}