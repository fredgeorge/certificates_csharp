/*
 * Copyright (c) 2024 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

namespace Engine.Certificates;

// Understands a financial obligation
public class Certificate {
    private readonly object _reason;
    private readonly double _amountOwed;
    private readonly double _amountPaid = 0.0;
    private Object? _payer = null;
    private Object? _invoiceParty = null;
    private State _state = Initial.Instance;

    public Certificate(Object reason, double amountOwed) {
        _reason = reason;
        _amountOwed = amountOwed;
    }

    public void Pay(Object payer, double newAmount) {
        if (newAmount <= 0.0)
            throw new ArgumentException("Amount paid must be greater than zero.");
        _state.Pay(this, payer, _amountOwed, _amountPaid, newAmount);
    }

    public void Invoice(Object invoiceParty, double amount) {
        if (amount <= 0.0)
            throw new ArgumentException("Amount paid must be greater than zero.");
        _state.Invoice(this, invoiceParty, _amountOwed, _amountPaid, amount);
    }

    private State SplitOnPayment() => Replaced.Instance;

    private State SplitOnInvoice() => Replaced.Instance;

    private interface State {
        void Pay(Certificate c, object payer1, double amountOwed, double amountPaid, double newAmount);
        void Invoice(Certificate c, object invoiceParty1, double amountOwed, double amountPaid, double invoiceAmount);
    }

    private class Initial : State {
        internal static readonly Initial Instance = new();
        private Initial() { }

        public void Pay(Certificate c, object payer, double amountOwed, double amountPaid, double newAmount) {
            if (amountOwed < newAmount)
                throw new ArgumentException("Amount paid cannot be greater than amount owed.");
            amountPaid = newAmount;
            c._payer = payer;
            c._state = amountPaid == amountOwed ? Paid.Instance : c.SplitOnPayment();
        }

        public void Invoice(Certificate c,
            object invoiceParty,
            double amountOwed,
            double amountPaid,
            double invoiceAmount) {
            if (amountOwed < invoiceAmount)
                throw new ArgumentException("Amount paid cannot be greater than amount owed.");
            c._invoiceParty = invoiceParty;
            c._state = invoiceAmount == amountOwed ? Invoiced.Instance : c.SplitOnInvoice();
        }
    }

    private class Paid : State {
        internal static readonly Paid Instance = new();
        private Paid() { }

        public void Pay(Certificate c, object payer, double amountOwed, double amountPaid, double newAmount) {
            throw new InvalidOperationException("Certificate has already been paid.");
        }

        public void Invoice(Certificate c,
            object invoiceParty,
            double amountOwed,
            double amountPaid,
            double invoiceAmount) {
            throw new InvalidOperationException("Certificate has already been paid.");
        }
    }

    private class Invoiced : State {
        internal static readonly Invoiced Instance = new();
        private Invoiced() { }

        public void Pay(Certificate c, object payer, double amountOwed, double amountPaid, double newAmount) {
            if (amountOwed < newAmount)
                throw new ArgumentException("Amount paid cannot be greater than amount owed.");
            amountPaid = newAmount;
            c._payer = payer;
            c._state = amountPaid == amountOwed ? Paid.Instance : c.SplitOnPayment();
        }

        public void Invoice(Certificate c,
            object invoiceParty,
            double amountOwed,
            double amountPaid,
            double invoiceAmount) {
            throw new InvalidOperationException("Certificate has already been invoiced.");
        }
    }

    private class Replaced : State {
        internal static readonly Replaced Instance = new();
        private Replaced() { }

        public void Pay(Certificate c, object payer, double amountOwed, double amountPaid, double newAmount) {
            throw new InvalidOperationException("Certificate has been replaced with split.");
        }

        public void Invoice(Certificate c,
            object invoiceParty,
            double amountOwed,
            double amountPaid,
            double invoiceAmount) {
            throw new InvalidOperationException("Certificate has been replaced with split.");
        }
    }
}