package com.mst.models;

import java.time.LocalDate;
import java.util.List;

/** Selection indexes refer to the displayed report, with the Id checked again on the server. */
public record SaleOrderReportAction(SaleOrderReportFilter filter, String grid, String action,
        List<Selection> selections, String remarks, LocalDate expiryDate, boolean bulk) {
    public record Selection(int id, int rowIndex) { }
}
