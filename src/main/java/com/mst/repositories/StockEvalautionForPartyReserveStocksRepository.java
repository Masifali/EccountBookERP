package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.StockEvalautionForPartyReserveStocks;

/**
 * Repository for dbo.StockEvalautionForPartyReserveStocks.
 * <p>
 * Source procedure: Sp_InventoryStockEvalautionDetail_Update, section {@code if @RefDocumentTypeId = 99} (SaleInvoiceDirect):
 * <pre>
 *   select @IsReserve = IsReserve from InvSaleInvoice where Id = @RefDocIdNo
 *   Delete StockEvalautionForPartyReserveStocks where RefDocumentTypeId=@RefDocumentTypeId and RefDocIdNo=@RefDocIdNo
 *                                                 and OrganizationId=@OrganizationId and CompanyId=@CompanyId
 *   ... INSERT INTO InventoryStockEvalautionDetail (stock OUT of the sale) ...
 *   IF @IsReserve = 1
 *       INSERT INTO StockEvalautionForPartyReserveStocks SELECT ... (QtyIn / BillWeightIn / StockWeightIn = invoice detail,
 *            WarehouseId = InvSaleInvoiceDetail.ReserveWareHouse, IsApproved = InvSaleInvoice.IsReserve,
 *            BranchesId = CASE WHEN pid.BranchId > 0 THEN pid.BranchId ELSE pih.BranchesId END)
 *       FROM InvSaleInvoice pih JOIN InvSaleInvoiceDetail pid JOIN SupplierCustomer JOIN Item
 *            JOIN V_UomScheduleAndUom uomItm LEFT JOIN V_UomScheduleAndUom RateUom
 * </pre>
 * The INSERT ... SELECT reads InvSaleInvoice / InvSaleInvoiceDetail / V_UomScheduleAndUom (no entities) and is part of
 * the KEEP-EXEC call in {@link InventoryStockEvalautionDetailRepository#spInventoryStockEvalautionDetailUpdate};
 * only the standalone DELETE is re-expressed here. {@code save()} works for simple cases.
 */
@Repository
public interface StockEvalautionForPartyReserveStocksRepository extends JpaRepository<StockEvalautionForPartyReserveStocks, Integer> {

    /**
     * Sp_InventoryStockEvalautionDetail_Update section RefDocumentTypeId = 99:
     * {@code Delete StockEvalautionForPartyReserveStocks where RefDocumentTypeId=@RefDocumentTypeId and RefDocIdNo=@RefDocIdNo
     * and OrganizationId=@OrganizationId and CompanyId=@CompanyId}. Returns the number of rows removed.
     */
    @Modifying
    @Transactional
    @Query("DELETE FROM StockEvalautionForPartyReserveStocks r "
         + "WHERE r.refDocumentTypeId = :refDocumentTypeId AND r.refDocIdNo = :refDocIdNo "
         + "AND r.organizationId = :organizationId AND r.companyId = :companyId")
    int deleteByRefDocument(@Param("refDocumentTypeId") Integer refDocumentTypeId,
                            @Param("refDocIdNo") Integer refDocIdNo,
                            @Param("organizationId") Integer organizationId,
                            @Param("companyId") Integer companyId);

    /** Reserve-stock rows of one sale invoice (RefDocumentTypeId = 99). Not a proc branch; verification helper. */
    List<StockEvalautionForPartyReserveStocks> findByRefDocumentTypeIdAndRefDocIdNoAndOrganizationIdAndCompanyId(
            Integer refDocumentTypeId, Integer refDocIdNo, Integer organizationId, Integer companyId);
}
