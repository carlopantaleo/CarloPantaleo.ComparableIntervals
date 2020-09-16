# ComparableIntervals

A library which helps doing common operations, such as _union_, _intersection_ or _complement_, on intervals of
`IComparable` types.

**Disclaimer**: I've created this library for a very specific application I'm developing in my company. I've tried to
make it more generic than I needed, but eventually it may be not as generic as it should be. If this library fits your
needs, you're welcome to use it, but if you need more functionalities, please don't ask for them. In any case, any
pull request is appreciated.


## Getting started

Every method and class in this library is documented inline, but here are a few hints to get started.

Create two `Interval`s of type `int`:
```c#
var interval1 = Interval<int>.Closed(1, 5);
var interval2 = Interval<int>.Open(3, 7);
```

Perform some operations on them:
```c#
var intersection = interval1.Intersection(interval2); // => (3, 5]
var Union = interval1.Union(interval2); // => [1, 7)
```

Perform operations on lists of intervals:
```c#
var intervals1 = new List<Interval<int>> {
    Interval<int>.ClosedOpen(-2, 2),
    Interval<int>.OpenClosed(4, 8)
};
var intervals2 = new List<Interval<int>> {
    Interval<int>.Closed(0, 6),
};

var intersection = Intervals.Intersection(intervals1, intervals2); // => [0, 2), (4, 6]
var union = Intervals.Union(intervals1, intervals2); // => [-2, 8]
var complement = Intervals.Complement(intervals1); // => (-∞, -2), [2, 4], (8, ∞)
```

## Library structure

This library library exposes a few objects to build and compute intervals.

`struct Bound<T>`: represents a bound of type `T`, where `T : IComparable`.
A bound may be open, closed, or may represent negative or positive infinity. Methods to create those types of bounds
are provided.

`class Interval<T>`: represents an interval of type `T`, where `T : IComparable`.
An interval is an entity with an upper bound and a lower bound. To create a closed, open, open-close, closed-open
interval, one of the provided methods may be used, otherwise it can be created providing its bounds.
Operations on an interval (such as _union_ and _intersection_) are provided as extension methods, so that they can be
called on an instance of an interval or in a static way.

`static class Intervals`: a class which provides static methods to operate on collections of intervals.