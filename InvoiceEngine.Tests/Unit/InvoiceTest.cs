/*
 * Copyright (c) 2024 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System;
using InvoiceEngine.Invoices;
using Xunit;

namespace InvoiceEngine.Tests.Unit;

// Ensures that Certificate works correctly
public class InvoiceTest {
    [Fact]
    public void PaidAll() {
        Invoice invoice = new Invoice("aReason", 100);
        invoice.Pay("payer", 100);
        Assert.Throws<InvalidOperationException>(() => invoice.Pay("payer", 1));
    }
    
    [Fact]
    public void InvoiceAll() {
        Invoice invoice = new Invoice("aReason", 100);
        invoice.Bill("party", 100);
        Assert.Throws<InvalidOperationException>(() => invoice.Bill("somebody",1));
        invoice.Pay("payer", 100);
        Assert.Throws<InvalidOperationException>(() => invoice.Pay("payer", 1));
    }
    
    [Fact]
    public void CreateWithInvoice() {
        Invoice invoice = new Invoice("aReason", 100, "party");
        Assert.Throws<InvalidOperationException>(() => invoice.Bill("somebody",1));
        invoice.Pay("payer", 100);
        Assert.Throws<InvalidOperationException>(() => invoice.Pay("payer", 1));
    }

    [Fact]
    public void PartialPayment() {
        var invoice = new Invoice("aReason", 100);
        invoice.Pay("payer", 25);
    }
}