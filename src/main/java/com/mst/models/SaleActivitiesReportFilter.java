package com.mst.models;

import java.time.LocalDate;
import java.util.List;

/** frmEvaulationDetailSalesReports.GridFill; session/user identifiers are supplied server-side. */
public record SaleActivitiesReportFilter(LocalDate fromDate, LocalDate toDate, int fromNo, int toNo,
        int parentCategoryId, int categoryId, int customerId, int itemId, int packUom, String cropYear,
        int jobLotId, int cityId, int warehouseId, int districtId, int itemClassId, int itemTypeId,
        int otherCategoryId, int saleAccountId, int stockAccountId, int cgsAccountId, int referencePartyId,
        String activity, List<Integer> branchIds, List<Integer> customGroupIds) { }
