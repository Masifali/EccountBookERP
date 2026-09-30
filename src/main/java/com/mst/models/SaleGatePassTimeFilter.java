package com.mst.models;

import java.time.LocalDate;
import java.util.List;

/** frmGatePassVehicleEntryAndExitTimeAnalysisReport's ReportsParameters inputs. */
public record SaleGatePassTimeFilter(LocalDate fromDate, LocalDate toDate, int fromNo, int toNo,
        int customerId, int documentTypeId, String status, List<Integer> branchIds) {}
