package com.mst.models;

import java.time.LocalDate;
import java.util.List;

/** frmSaleOrderHistory: gridHisory / GridSummaryFill. Session identifiers are never accepted here. */
public record SaleOrderReportFilter(
        LocalDate fromDate, LocalDate toDate, int fromNo, int toNo,
        int customerId, int itemId, int parentCategoryId, int categoryId, int itemTypeId,
        int bookingPersonId, int cityId, int referencePartyId, int costCenterId,
        String status, String approval, boolean includeApprovedDo, List<Integer> branchIds,
        int packUom, String cropYear, int jobLotId, int packingTypeId, int districtId,
        String activity, boolean skipZero) { }
