/*
 * Copyright (c) 2024 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System;
using System.Linq;
using InvoiceEngine.Invoices;
using Xunit;

namespace InvoiceEngine.Tests.Unit;

// Ensures that Certificate works correctly
public class InvoiceTest {
    [Fact]
    public void PaidAll() {
        Invoice invoice = new Invoice("aService", 100);
        Assert.Single(invoice);
        invoice.Pay("payer", 100);
        Assert.Single(invoice);
        Assert.Throws<InvalidOperationException>(() => invoice.Pay("payer", 1));
    }
    
    [Fact]
    public void InvoiceAll() {
        Invoice invoice = new Invoice("aService", 100);
        Assert.Single(invoice);
        invoice.Bill("party", 100);
        Assert.Single(invoice);
        Assert.Throws<InvalidOperationException>(() => invoice.Bill("somebody",1));
        invoice.Pay("payer", 100);
        Assert.Single(invoice);
        Assert.Throws<InvalidOperationException>(() => invoice.Pay("payer", 1));
    }
    
    [Fact]
    public void CreateWithInvoice() {
        Invoice invoice = new Invoice("aService", 100, "party");
        Assert.Throws<InvalidOperationException>(() => invoice.Bill("somebody",1));
        invoice.Pay("payer", 100);
        Assert.Single(invoice);
        Assert.Throws<InvalidOperationException>(() => invoice.Pay("payer", 1));
    }

    [Fact]
    public void PartialPayment() {
        var invoice = new Invoice("aService", 100);
        Assert.Single(invoice);
        invoice.Pay("payer", 25);
        Assert.Equal(2, invoice.Count());
    }

    [Fact]
    public void PartialInvoice() {
        var invoice = new Invoice("aService", 100);
        Assert.Single(invoice);
        invoice.Bill("somebody", 25);
        Assert.Equal(2, invoice.Count());
    }
}