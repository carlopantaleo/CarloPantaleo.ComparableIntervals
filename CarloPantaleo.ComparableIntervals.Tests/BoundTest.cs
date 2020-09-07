using System;
using System.Collections.Generic;
using Xunit;

namespace CarloPantaleo.ComparableIntervals.Tests {
    public class BoundTest {
        [Theory]
        [MemberData(nameof(IntBoundsData))]
        public void OpBoundsLessThan(Bound<int> left, Bound<int> right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, true},
                {0, false},
                {1, false},
                {2, true},
                {3, false}
            };

            Assert.Equal(left < right, outcomesMap[outcome]);
        }

        [Theory]
        [MemberData(nameof(IntBoundsData))]
        public void OpBoundsLessThanOrEqual(Bound<int> left, Bound<int> right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, true},
                {0, true},
                {1, false},
                {2, true},
                {3, false}
            };

            Assert.Equal(left <= right, outcomesMap[outcome]);
        }

        [Theory]
        [MemberData(nameof(IntBoundsData))]
        public void OpBoundsGreaterThan(Bound<int> left, Bound<int> right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, false},
                {0, false},
                {1, true},
                {2, true},
                {3, false}
            };

            Assert.Equal(left > right, outcomesMap[outcome]);
        }

        [Theory]
        [MemberData(nameof(IntBoundsData))]
        public void OpBoundsGreaterThanOrEqual(Bound<int> left, Bound<int> right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, false},
                {0, true},
                {1, true},
                {2, true},
                {3, false}
            };

            Assert.Equal(left >= right, outcomesMap[outcome]);
        }
        
        [Theory]
        [MemberData(nameof(IntData))]
        public void OpLessThan(object left, object right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, true},
                {0, false},
                {1, false},
                {2, true}
            };

            switch (left) {
                case Bound<int> bleft when right is int iright:
                    Assert.Equal(bleft < iright, outcomesMap[outcome]);
                    break;
                case int ileft when right is Bound<int> bright:
                    Assert.Equal(ileft < bright, outcomesMap[outcome]);
                    break;
                default:
                    throw new InvalidOperationException("Unexpected test data.");
            }
        }

        [Theory]
        [MemberData(nameof(IntData))]
        public void OpLessThanOrEqual(object left, object right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, true},
                {0, true},
                {1, false},
                {2, true}
            };

            switch (left) {
                case Bound<int> bleft when right is int iright:
                    Assert.Equal(bleft <= iright, outcomesMap[outcome]);
                    break;
                case int ileft when right is Bound<int> bright:
                    Assert.Equal(ileft <= bright, outcomesMap[outcome]);
                    break;
                default:
                    throw new InvalidOperationException("Unexpected test data.");
            }
        }

        [Theory]
        [MemberData(nameof(IntData))]
        public void OpGreaterThan(object left, object right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, false},
                {0, false},
                {1, true},
                {2, true}
            };

            switch (left) {
                case Bound<int> bleft when right is int iright:
                    Assert.Equal(bleft > iright, outcomesMap[outcome]);
                    break;
                case int ileft when right is Bound<int> bright:
                    Assert.Equal(ileft > bright, outcomesMap[outcome]);
                    break;
                default:
                    throw new InvalidOperationException("Unexpected test data.");
            }
        }

        [Theory]
        [MemberData(nameof(IntData))]
        public void OpGreaterThanOrEqual(object left, object right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, false},
                {0, true},
                {1, true},
                {2, true}
            };

            switch (left) {
                case Bound<int> bleft when right is int iright:
                    Assert.Equal(bleft >= iright, outcomesMap[outcome]);
                    break;
                case int ileft when right is Bound<int> bright:
                    Assert.Equal(ileft >= bright, outcomesMap[outcome]);
                    break;
                default:
                    throw new InvalidOperationException("Unexpected test data.");
            }
        }

        [Theory]
        [MemberData(nameof(IntBoundsData))]
        public void OpEqual(Bound<int> left, Bound<int> right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, false},
                {0, true},
                {1, false},
                {2, false},
                {3, false}
            };
            
            Assert.Equal(outcomesMap[outcome], left == right);
        }
        
        [Theory]
        [MemberData(nameof(IntBoundsData))]
        public void OpNotEqual(Bound<int> left, Bound<int> right, int outcome) {
            var outcomesMap = new Dictionary<int, bool> {
                {-1, true},
                {0, false},
                {1, true},
                {2, true},
                {3, true}
            };
            
            Assert.Equal(outcomesMap[outcome], left != right);
        }

        [Theory]
        [MemberData(nameof(IntBoundsData))]
        public void Min(Bound<int> left, Bound<int> right, int outcome) {
            switch (outcome) {
                case -1:
                    if (left.Type == BoundType.NegativeInfinite) {
                        Assert.Equal(BoundType.NegativeInfinite, Bound<int>.Min(left, right).Type);
                    } else {
                        Assert.Equal(left, Bound<int>.Min(left, right));
                    }
                    break;
                case 0:
                    Assert.Equal(left, Bound<int>.Min(left, right));
                    Assert.Equal(right, Bound<int>.Min(left, right));
                    break;
                case 1:
                    if (right.Type == BoundType.NegativeInfinite) {
                        Assert.Equal(BoundType.NegativeInfinite, Bound<int>.Min(left, right).Type);
                    } else {
                        Assert.Equal(right, Bound<int>.Min(left, right));
                    }
                    break;
                case 2 when left.Type == BoundType.Closed:
                    Assert.Equal(left, Bound<int>.Min(left, right));
                    break;
                case 2 when right.Type == BoundType.Closed:
                    Assert.Equal(right, Bound<int>.Min(left, right));
                    break;
                case 3:
                    Assert.Equal(left.Type, Bound<int>.Min(left, right).Type);
                    break;
            }
        }
        
        [Theory]
        [MemberData(nameof(IntBoundsData))]
        public void Max(Bound<int> left, Bound<int> right, int outcome) {
            switch (outcome) {
                case -1:
                    if (right.Type == BoundType.PositiveInfinite) {
                        Assert.Equal(BoundType.PositiveInfinite, Bound<int>.Max(left, right).Type);
                    } else {
                        Assert.Equal(right, Bound<int>.Max(left, right));
                    }
                    break;
                case 0:
                    Assert.Equal(left, Bound<int>.Max(left, right));
                    Assert.Equal(right, Bound<int>.Max(left, right));
                    break;
                case 1:
                    if (left.Type == BoundType.PositiveInfinite) {
                        Assert.Equal(BoundType.PositiveInfinite, Bound<int>.Max(left, right).Type);
                    } else {
                        Assert.Equal(left, Bound<int>.Max(left, right));
                    }
                    break;
                case 2 when left.Type == BoundType.Closed:
                    Assert.Equal(left, Bound<int>.Max(left, right));
                    break;
                case 2 when right.Type == BoundType.Closed:
                    Assert.Equal(right, Bound<int>.Max(left, right));
                    break;
                case 3:
                    Assert.Equal(left.Type, Bound<int>.Max(left, right).Type);
                    break;
            }
        }

        public static IEnumerable<object[]> IntBoundsData =>
            new List<object[]> {
                new object[] {Bound<int>.Open(1), Bound<int>.Open(2), -1},
                new object[] {Bound<int>.Open(2), Bound<int>.Open(1), 1},
                new object[] {Bound<int>.Closed(1), Bound<int>.Closed(2), -1},
                new object[] {Bound<int>.Closed(2), Bound<int>.Closed(1), 1},
                new object[] {Bound<int>.Open(1), Bound<int>.Closed(2), -1},
                new object[] {Bound<int>.Open(2), Bound<int>.Closed(1), 1},
                new object[] {Bound<int>.Closed(1), Bound<int>.Open(2), -1},
                new object[] {Bound<int>.Closed(2), Bound<int>.Open(1), 1},
                new object[] {Bound<int>.Closed(1), Bound<int>.Open(1), 2},
                new object[] {Bound<int>.Open(1), Bound<int>.Closed(1), 2},
                new object[] {Bound<int>.Open(0), Bound<int>.Open(0), 0},
                new object[] {Bound<int>.Closed(0), Bound<int>.Closed(0), 0},
                new object[] {Bound<int>.NegativeInfinite(), Bound<int>.NegativeInfinite(), 3},
                new object[] {Bound<int>.PositiveInfinite(), Bound<int>.PositiveInfinite(), 3},
                new object[] {Bound<int>.NegativeInfinite(), Bound<int>.PositiveInfinite(), -1},
                new object[] {Bound<int>.PositiveInfinite(), Bound<int>.NegativeInfinite(), 1},
                new object[] {Bound<int>.PositiveInfinite(), Bound<int>.Open(1), 1},
                new object[] {Bound<int>.NegativeInfinite(), Bound<int>.Open(1), -1},
                new object[] {Bound<int>.PositiveInfinite(), Bound<int>.Closed(1), 1},
                new object[] {Bound<int>.NegativeInfinite(), Bound<int>.Closed(1), -1},
                new object[] {Bound<int>.Open(1), Bound<int>.PositiveInfinite(), -1},
                new object[] {Bound<int>.Open(1), Bound<int>.NegativeInfinite(), 1},
                new object[] {Bound<int>.Closed(1), Bound<int>.PositiveInfinite(), -1},
                new object[] {Bound<int>.Closed(1), Bound<int>.NegativeInfinite(), 1},
            };

        public static IEnumerable<object[]> IntData =>
            new List<object[]> {
                new object[] {Bound<int>.Open(1), 2, -1},
                new object[] {Bound<int>.Open(2), 1, 1},
                new object[] {Bound<int>.Closed(1), 2, -1},
                new object[] {Bound<int>.Closed(2), 1, 1},
                new object[] {Bound<int>.Open(1), 1, 2},
                new object[] {Bound<int>.Closed(1), 1, 0},
                new object[] {Bound<int>.NegativeInfinite(), 1, -1},
                new object[] {Bound<int>.PositiveInfinite(), 1, 1},
                new object[] {2, Bound<int>.Open(1), 1},
                new object[] {1, Bound<int>.Open(2), -1},
                new object[] {2, Bound<int>.Closed(1), 1},
                new object[] {1, Bound<int>.Closed(2), -1},
                new object[] {1, Bound<int>.Open(1), 2},
                new object[] {1, Bound<int>.Closed(1), 0},
                new object[] {1, Bound<int>.NegativeInfinite(), 1},
                new object[] {1, Bound<int>.PositiveInfinite(), -1},
            };
    }
}