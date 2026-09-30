package com.mst.models;

import java.time.LocalDate;
import java.util.List;

public record SaleGdnReportFilter(LocalDate fromDate, LocalDate toDate, List<Integer> branchIds,
        int fromNo, int toNo, int orderNoFrom, int orderNoTo, int customerId,
        int parentCategoryId, int categoryId, int itemTypeId, int itemClassId,
        int itemId, int cropYearId, int jobLotId, int warehouseId) {}
