/*
 * Copyright (c) 2024 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

namespace InvoiceEngine.Invoices;

// Understands sequence of an Invoice hierarchy
public interface InvoiceVisitor {
    void PreVisit(Invoice invoice, object service, double amountOwed, double amountPaid, object? payer, object? party);
    void Visit(Invoice invoice, object service, double amountOwed, double amountPaid, object? payer, object? party);
    void PostVisit(Invoice invoice, object service, double amountOwed, double amountPaid, object? payer, object? party);
}