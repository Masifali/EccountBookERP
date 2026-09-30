package com.mst.models;

import java.time.LocalDate;
import java.util.List;

/** frmGPOutward.gridHisory; GatepassType is its selected display text, not the lookup ID. */
public record SaleOutwardReportFilter(LocalDate fromDate, LocalDate toDate, int fromNo, int toNo,
        int customerId, String gatePassType, String status, List<Integer> branchIds, boolean onlyPending) { }
