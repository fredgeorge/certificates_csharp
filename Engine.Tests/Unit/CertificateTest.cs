/*
 * Copyright (c) 2024 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System;
using Engine.Certificates;
using Xunit;

namespace Engine.Tests.Unit;

// Ensures that Certificate works correctly
public class CertificateTest {
    [Fact]
    public void PaidAll() {
        var c = new Certificate("test", 100);
        c.Pay(100);
        Assert.Throws<InvalidOperationException>(() => c.Pay(1));
    }
    [Fact]
    public void InvoiceAll() {
        var c = new Certificate("test", 100);
        c.Invoice(100);
        Assert.Throws<InvalidOperationException>(() => c.Invoice(1));
        c.Pay(100);
        Assert.Throws<InvalidOperationException>(() => c.Pay(1));
    }
}