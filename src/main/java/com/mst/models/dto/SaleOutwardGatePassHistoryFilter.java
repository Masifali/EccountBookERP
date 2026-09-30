package com.mst.models.dto;

import java.time.LocalDate;

/** OutwardGatePass.gridhistory: optional document/entry/modify dates and GP range. */
public record SaleOutwardGatePassHistoryFilter(LocalDate fromDate, LocalDate toDate,
        String dateType, int fromNo, int toNo, int customerId) {
    public SaleOutwardGatePassHistoryFilter {
        if (dateType == null || dateType.isBlank()) dateType = "document";
        if (!java.util.Set.of("document", "entry", "modify").contains(dateType))
            throw new IllegalArgumentException("Invalid history date type");
    }
}
