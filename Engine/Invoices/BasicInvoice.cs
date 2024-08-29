/*
 * Copyright (c) 2024 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

namespace Engine.Certificates;

// Understands a financial obligation
public class Invoice {
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

    public Invoice Pay(object payer, double newAmount) {
        if (newAmount <= 0.0)
            throw new ArgumentException("Amount paid must be greater than zero.");
        return _state.Pay(this, payer, newAmount);
    }
    
    public Invoice Bill(object invoiceParty, double amount) {
        if (amount <= 0.0)
            throw new ArgumentException("Amount paid must be greater than zero.");
        return _state.Invoice(this, invoiceParty, amount);
    }

    private State SplitOnPayment(object payer, double amount) => new Split();

    private State SplitOnInvoice(object invoiceParty, double invoiceAmount) => new Split();

    private interface State {
        Invoice Pay(Invoice c, object payer1, double newAmount);
        Invoice Invoice(Invoice c, object invoiceParty, double invoiceAmount);
    }

    private class Initial : State {
        internal static readonly Initial Instance = new();
        private Initial() { }

        public Invoice Pay(Invoice c, object payer, double newAmount) {
            if (c._amountOwed < newAmount)
                throw new ArgumentException("Amount paid cannot be greater than amount owed.");
            c._amountPaid = newAmount;
            c._payer = payer;
            c._state = c._amountPaid == c._amountOwed ? Paid.Instance : c.SplitOnPayment(payer, newAmount);
            return c;
        }

        public Invoice Invoice(Invoice c,
            object invoiceParty,
            double invoiceAmount) {
            if (c._amountOwed < invoiceAmount)
                throw new ArgumentException("Amount paid cannot be greater than amount owed.");
            c._invoiceParty = invoiceParty;
            c._state = invoiceAmount == c._amountOwed ? Invoiced.Instance : c.SplitOnInvoice(invoiceParty, invoiceAmount);
            return c;
        }
    }

    private class Paid : State {
        internal static readonly Paid Instance = new();
        private Paid() { }

        public Invoice Pay(Invoice c, object payer, double newAmount) {
            throw new InvalidOperationException("Certificate has already been paid.");
        }

        public Invoice Invoice(Invoice c,
            object invoiceParty,
            double invoiceAmount) {
            throw new InvalidOperationException("Certificate has already been paid.");
        }
    }

    private class Invoiced : State {
        internal static readonly Invoiced Instance = new();
        private Invoiced() { }

        public Invoice Pay(Invoice c, object payer, double newAmount) {
            if (c._amountOwed < newAmount)
                throw new ArgumentException("Amount paid cannot be greater than amount owed.");
            c._amountPaid = newAmount;
            c._payer = payer;
            c._state = c._amountPaid == c._amountOwed ? Paid.Instance : c.SplitOnPayment(payer, newAmount);
            return c;
        }

        public Invoice Invoice(Invoice c,
            object invoiceParty,
            double invoiceAmount) {
            throw new InvalidOperationException("Certificate has already been invoiced.");
        }
    }

    private class Split : State {
        internal Split() { }

        public Invoice Pay(Invoice c, object payer, double newAmount) {
            throw new InvalidOperationException("Certificate has been replaced with split.");
        }

        public Invoice Invoice(Invoice c,
            object invoiceParty,
            double invoiceAmount) {
            throw new InvalidOperationException("Certificate has been replaced with split.");
        }
    }
}