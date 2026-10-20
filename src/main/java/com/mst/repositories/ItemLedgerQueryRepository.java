package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.Item;
import com.mst.projections.ItemLedgerRow;

/** usp_ItemLedgerFromStockEvaluations - item stock ledger (opening balance + running balances) from InventoryStockEvalautionDetail. */
@Repository
public interface ItemLedgerQueryRepository extends org.springframework.data.repository.Repository<Item, Integer> {

    /** usp_ItemLedgerFromStockEvaluations (single branch).
     *  // KEEP-EXEC: procedural @FromDate adjustment (InventoryStockAsOnDate, parent category 7 switch, @AvgUom 1/40),
     *  4 CTEs with UNION ALL, FORMAT(), ROW_NUMBER() and SUM() OVER running totals, V_UomScheduleAndUom view.
     *  All five parameters are mandatory (no defaults). Ordered by ItemId, RowNum; the first row is the opening balance (RowNum = -1). */
    @Query(value = "EXEC dbo.usp_ItemLedgerFromStockEvaluations @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@ItemId = :itemId, @FromDate = :fromDate, @ToDate = :toDate", nativeQuery = true)
    List<ItemLedgerRow> itemLedgerFromStockEvaluations(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("itemId") Integer itemId, @Param("fromDate") LocalDate fromDate, @Param("toDate") LocalDate toDate);
}
