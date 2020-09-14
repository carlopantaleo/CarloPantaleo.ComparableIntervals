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
        public void Union(List<Interval<int>> first, List<Interval<int>> second, List<Interval<int>> expectedOutput) {
            var output = Intervals.Union(first, second);
            // Comparison is done on the string representation of intervals in order to handle infinity cases which are
            // not comparable.
            Assert.Equal(expectedOutput.Select(i => i.ToString()), output.Select(i => i.ToString()));
        }

        [Theory]
        [MemberData(nameof(IntersectionData))]
        public void Intersection(List<Interval<int>> expected, params ICollection<Interval<int>>[] input) {
            var output = Intervals.Intersection(input);
            // Comparison is done on the string representation of intervals in order to handle infinity cases which are
            // not comparable.
            Assert.Equal(expected.Select(i => i.ToString()), output.Select(i => i.ToString()));
        }

        [Theory]
        [MemberData(nameof(ComplementData))]
        public void Complement(List<Interval<int>> input, List<Interval<int>> expected) {
            var output = Intervals.Complement(input);
            // Comparison is done on the string representation of intervals in order to handle infinity cases which are
            // not comparable.
            Assert.Equal(expected.Select(i => i.ToString()), output.Select(i => i.ToString()));
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

        public static IEnumerable<object[]> IntersectionData =>
            new List<object[]> {
                new object[] {new List<Interval<int>>(), null,},
                new object[] {new List<Interval<int>>(), new List<Interval<int>>(),},
                new object[] {new List<Interval<int>>(), new List<Interval<int>> {Interval<int>.Empty()},},
                new object[] {
                    new List<Interval<int>> {Interval<int>.Closed(2, 4)},
                    new List<Interval<int>> {Interval<int>.Closed(2, 3), Interval<int>.Closed(3, 4)},
                },
                new object[] {
                    new List<Interval<int>> {Interval<int>.Closed(2, 4)},
                    new List<Interval<int>> {Interval<int>.Closed(1, 4)},
                    new List<Interval<int>> {Interval<int>.Closed(2, 5)},
                },
                new object[] {
                    new List<Interval<int>> {Interval<int>.Closed(2, 4)},
                    new List<Interval<int>> {Interval<int>.Closed(1, 4)},
                    new List<Interval<int>> {Interval<int>.Closed(-5, -1), Interval<int>.Closed(2, 5)},
                },
                new object[] {
                    new List<Interval<int>> {Interval<int>.Closed(1, 2), Interval<int>.Closed(3, 4)},
                    new List<Interval<int>> {Interval<int>.Closed(1, 3), Interval<int>.Closed(2, 4)},
                    new List<Interval<int>> {Interval<int>.Closed(1, 2), Interval<int>.Closed(3, 4)},
                },
                new object[] {
                    new List<Interval<int>> {Interval<int>.OpenClosed(-5, 0), Interval<int>.ClosedOpen(10, 15)},
                    new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.Closed(0)),
                        Interval<int>.FromBounds(Bound<int>.Closed(10), Bound<int>.PositiveInfinity())
                    },
                    new List<Interval<int>> {Interval<int>.Open(-5, 15)},
                },
                new object[] {
                    new List<Interval<int>> {Interval<int>.Closed(5, 6)},
                    new List<Interval<int>> {Interval<int>.Closed(1, 10)},
                    new List<Interval<int>> {Interval<int>.ClosedOpen(5, 10), Interval<int>.Open(15, 20)},
                    new List<Interval<int>> {Interval<int>.Closed(0, 6)},
                },
            };

        public static IEnumerable<object[]> ComplementData =>
            new List<object[]> {
                new object[] {
                    null, new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity())
                    },
                },
                new object[] {
                    new List<Interval<int>>(), new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity())
                    },
                },
                new object[] {
                    new List<Interval<int>> {Interval<int>.Empty()}, new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.PositiveInfinity())
                    },
                },
                new object[] {
                    new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.Closed(0)),
                        Interval<int>.FromBounds(Bound<int>.Closed(10), Bound<int>.PositiveInfinity())
                    },
                    new List<Interval<int>> {
                        Interval<int>.Open(0, 10)
                    },
                },
                new object[] {
                    new List<Interval<int>> {Interval<int>.Closed(0, 10)},
                    new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.Open(0)),
                        Interval<int>.FromBounds(Bound<int>.Open(10), Bound<int>.PositiveInfinity()),
                    },
                },
                new object[] {
                    new List<Interval<int>> {Interval<int>.ClosedOpen(0, 10), Interval<int>.OpenClosed(5, 20)},
                    new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.Open(0)),
                        Interval<int>.FromBounds(Bound<int>.Open(20), Bound<int>.PositiveInfinity()),
                    },
                },
                new object[] {
                    new List<Interval<int>> {Interval<int>.Open(0, 10), Interval<int>.Closed(20, 30)},
                    new List<Interval<int>> {
                        Interval<int>.FromBounds(Bound<int>.NegativeInfinity(), Bound<int>.Closed(0)),
                        Interval<int>.ClosedOpen(10, 20),
                        Interval<int>.FromBounds(Bound<int>.Open(30), Bound<int>.PositiveInfinity()),
                    },
                },
            };
    }
}