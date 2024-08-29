/*
 * Copyright (c) 2024 by Fred George
 * @author Fred George  fredgeorge@acm.org
 * Licensed under the MIT License; see LICENSE file in root.
 */

namespace Engine.Certificates;

// Understands a financial obligation
public interface Invoice {
    Invoice Pay(object payer, double newAmount);
    Invoice Invoice(object invoiceParty, double amount);
}