/*
 * Copyright (c) 2024 by Fred George
 * May be used freely except for training; license required for training.
 * @author Fred George  fredgeorge@acm.org
 */

namespace Engine.Geometry;

// Understands a polygon with 4 sides at right angles
public class Rectangle {
    private readonly double _length;
    private readonly double _width;

    public Rectangle(double length, double width) {
        if (length <= 0.0 || width <= 0.0) throw new ArgumentException("Each dimension must be greater than zero");
        _length = length;
        _width = width;
    }

    public static Rectangle Square(double side) => new(side, side);

    public double Area() => _length * _width;

    public double Perimeter() => 2 * (_length + _width);
}