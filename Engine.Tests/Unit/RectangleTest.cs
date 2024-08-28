/*
 * Copyright (c) 2024 by Fred George
 * May be used freely except for training; license required for training.
 * @author Fred George  fredgeorge@acm.org
 */

using System;
using Engine.Geometry;
using Xunit;

namespace Engine.Tests.Unit;

// Ensures that Rectangle works correctly
public class RectangleTest
{
    [Fact]
    public void Area()
    {
        Assert.Equal(24.0, new Rectangle(4.0, 6.0).Area());
        Assert.Equal(24.0, new Rectangle(4, 6).Area());
        Assert.Equal(36.0, Rectangle.Square(6.0).Area());
    }
    
    [Fact]
    public void Perimeter()
    {
        Assert.Equal(20.0, new Rectangle(4.0, 6.0).Perimeter());
        Assert.Equal(24.0, Rectangle.Square(6.0).Perimeter());
    }

    [Fact]
    public void ValidDimensions() {
        Assert.Throws<ArgumentException>(() => new Rectangle(0, 6.0));
        Assert.Throws<ArgumentException>(() => new Rectangle(4.00, 0));
        Assert.Throws<ArgumentException>(() => Rectangle.Square(0.0));
    }
}