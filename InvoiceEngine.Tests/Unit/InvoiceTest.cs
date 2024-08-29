/*
 * Copyright (c) 2024 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceEngine.Invoices;
using Xunit;

namespace InvoiceEngine.Tests.Unit;

// Ensures that Certificate works correctly
public class InvoiceTest {
    [Fact]
    public void PaidAll() {
        Invoice invoice = new Invoice("aService", 100);
        Assert.Equal(1, invoice.Count());
        invoice.Pay("payer", 100);
        Assert.Equal(1, invoice.Count());
        Assert.Throws<InvalidOperationException>(() => invoice.Pay("payer", 1));
    }
    
    [Fact]
    public void InvoiceAll() {
        Invoice invoice = new Invoice("aService", 100);
        Assert.Equal(1, invoice.Count());
        invoice.Bill("party", 100);
        Assert.Equal(1, invoice.Count());
        Assert.Throws<InvalidOperationException>(() => invoice.Bill("somebody",1));
        invoice.Pay("payer", 100);
        Assert.Equal(1, invoice.Count());
        Assert.Throws<InvalidOperationException>(() => invoice.Pay("payer", 1));
    }
    
    [Fact]
    public void CreateWithInvoice() {
        Invoice invoice = new Invoice("aService", 100, "party");
        Assert.Throws<InvalidOperationException>(() => invoice.Bill("somebody",1));
        invoice.Pay("payer", 100);
        Assert.Equal(1, invoice.Count());
        Assert.Throws<InvalidOperationException>(() => invoice.Pay("payer", 1));
    }

    [Fact]
    public void PartialPayment() {
        var invoice = new Invoice("aService", 100);
        Assert.Equal(1, invoice.Count());
        invoice.Pay("payer", 25);
        Assert.Equal(2, invoice.Count());
    }

    [Fact]
    public void PartialInvoice() {
        var invoice = new Invoice("aService", 100);
        Assert.Equal(1, invoice.Count());
        invoice.Bill("somebody", 25);
        Assert.Equal(2, invoice.Count());
    }

    internal class BasicInvoices : InvoiceVisitor {
        internal readonly List<Invoice> Invoices = new();

        public BasicInvoices(Invoice invoice) {
            invoice.Accept(this);
        }

        public void PreVisit(Invoice invoice, object service, double amountOwed, double amountPaid, object? payer, object? party) {
            // Ignore
        }

        public void Visit(Invoice invoice, object service, double amountOwed, double amountPaid, object? payer, object? party) {
            Invoices.Add(invoice);
        }

        public void PostVisit(Invoice invoice, object service, double amountOwed, double amountPaid, object? payer, object? party) {
            // Ignore
        }
    }
}

internal static class TestExtensions {
    internal static int Count(this Invoice invoice) => 
        new InvoiceTest.BasicInvoices(invoice).Invoices.Count();
}