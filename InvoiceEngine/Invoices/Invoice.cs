/*
 * Copyright (c) 2024 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

using System.Collections;

namespace InvoiceEngine.Invoices;

// Understands a financial obligation
public class Invoice : IEnumerable<Invoice> {
    private readonly object _reason;
    private readonly double _amountOwed;
    private double _amountPaid = 0.0;
    private object? _payer = null;
    private object? _invoiceParty = null;
    private State _state = Initial.Instance;

    public Invoice(object reason, double amountOwed) {
        _reason = reason;
        _amountOwed = amountOwed;
    }

    public Invoice(object reason, double amountOwed, object invoiceParty) {
        _reason = reason;
        _amountOwed = amountOwed;
        _invoiceParty = invoiceParty;
        _state = Invoiced.Instance;
    }

    public void Pay(object payer, double newAmount) {
        if (newAmount <= 0.0)
            throw new ArgumentException("Amount paid must be greater than zero.");
        _state.Pay(this, payer, newAmount);
    }

    public void Bill(object invoiceParty, double amount) {
        if (amount <= 0.0)
            throw new ArgumentException("Amount paid must be greater than zero.");
        _state.Bill(this, invoiceParty, amount);
    }

    public IEnumerator<Invoice> GetEnumerator() {
        return _state is Split split 
            ? split.GetEnumerator() 
            : new List<Invoice> { this }.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    private State SplitOnPayment(object payer, double amount) => new Split(new List<Invoice>());

    private State SplitOnInvoice(object invoiceParty, double invoiceAmount) => new Split(new List<Invoice>());

    private interface State {
        void Pay(Invoice c, object payer1, double newAmount);
        void Bill(Invoice c, object invoiceParty, double invoiceAmount);
    }

    private class Initial : State {
        internal static readonly Initial Instance = new();
        private Initial() { }

        public void Pay(Invoice c, object payer, double newAmount) {
            if (c._amountOwed < newAmount)
                throw new ArgumentException("Amount paid cannot be greater than amount owed.");
            c._amountPaid = newAmount;
            c._payer = payer;
            c._state = c._amountPaid == c._amountOwed ? Paid.Instance : c.SplitOnPayment(payer, newAmount);
        }

        public void Bill(Invoice c,
            object invoiceParty,
            double invoiceAmount) {
            if (c._amountOwed < invoiceAmount)
                throw new ArgumentException("Amount paid cannot be greater than amount owed.");
            c._invoiceParty = invoiceParty;
            c._state = invoiceAmount == c._amountOwed
                ? Invoiced.Instance
                : c.SplitOnInvoice(invoiceParty, invoiceAmount);
        }
    }

    private class Paid : State {
        internal static readonly Paid Instance = new();
        private Paid() { }

        public void Pay(Invoice c, object payer, double newAmount) {
            throw new InvalidOperationException("Certificate has already been paid.");
        }

        public void Bill(Invoice c,
            object invoiceParty,
            double invoiceAmount) {
            throw new InvalidOperationException("Certificate has already been paid.");
        }
    }

    private class Invoiced : State {
        internal static readonly Invoiced Instance = new();
        private Invoiced() { }

        public void Pay(Invoice c, object payer, double newAmount) {
            if (c._amountOwed < newAmount)
                throw new ArgumentException("Amount paid cannot be greater than amount owed.");
            c._amountPaid = newAmount;
            c._payer = payer;
            c._state = c._amountPaid == c._amountOwed ? Paid.Instance : c.SplitOnPayment(payer, newAmount);
        }

        public void Bill(Invoice c,
            object invoiceParty,
            double invoiceAmount) {
            throw new InvalidOperationException("Certificate has already been invoiced.");
        }
    }

    private class Split : State {
        private readonly List<Invoice> _invoices = new();

        internal Split(List<Invoice> invoices) {
            _invoices = invoices;
        }

        public void Pay(Invoice c, object payer, double newAmount) {
            throw new InvalidOperationException("Certificate has been replaced with split.");
        }

        public void Bill(Invoice c, object invoiceParty, double invoiceAmount) {
            throw new InvalidOperationException("Certificate has been replaced with split.");
        }

        internal IEnumerator<Invoice> GetEnumerator() => _invoices.GetEnumerator();
    }
}