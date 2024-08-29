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
public class InvoiceTest {
    [Fact]
    public void PaidAll() {
        Invoice c = new Invoice("aReason", 100);
        c.Pay("payer", 100);
        Assert.Throws<InvalidOperationException>(() => c.Pay("payer", 1));
    }
    
    [Fact]
    public void InvoiceAll() {
        Invoice c = new Invoice("aReason", 100);
        c.Bill("party", 100);
        Assert.Throws<InvalidOperationException>(() => c.Bill("somebody",1));
        c.Pay("payer", 100);
        Assert.Throws<InvalidOperationException>(() => c.Pay("payer", 1));
    }
    
    [Fact]
    public void CreateWithInvoice() {
        Invoice c = new Invoice("aReason", 100, "party");
        Assert.Throws<InvalidOperationException>(() => c.Bill("somebody",1));
        c.Pay("payer", 100);
        Assert.Throws<InvalidOperationException>(() => c.Pay("payer", 1));
    }

    [Fact]
    public void PartialPayment() {
        Invoice originalC = new Invoice("aReason", 100);
        Invoice newC = originalC.Pay("payer", 25);
    }
}