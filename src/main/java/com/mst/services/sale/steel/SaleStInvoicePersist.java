package com.mst.services.sale.steel;

import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherHead;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.stereotype.Component;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.Map;

import static com.mst.services.sale.engr.SaleEngrSupport.row;

/**
 * Architecture.DAL.Steel.Inventory.InvSaleInvoice.SetData (DAL 0215) - one transaction (the caller's @Transactional):
 * header, details (ItemNetAmount = ItemAmount + ExpenseAmount - CommissionAmount), freight, journal, expense, attachments, voucher head / details,
 * USP_VoucherBalanceCheck, the _H history rows, Sp_InventoryStockEvalautionDetail_Update and, for document type 1510 only, Sp_InventoryTransactions_GetALLMethod.
 */
@Component
public class SaleStInvoicePersist {
    private final SaleEngrSupport sup;
    private final SaleSteelSupport steel;
    private final SaleInvoiceRepository repo;
    private final SaleEngrAttachments attachments;

    public SaleStInvoicePersist(SaleEngrSupport sup, SaleSteelSupport steel, SaleInvoiceRepository repo, SaleEngrAttachments attachments) {
        this.sup = sup; this.steel = steel; this.repo = repo; this.attachments = attachments;
    }

    private int call(String proc, Map<String, Object> m) { return sup.setProcMap(proc, steel.full(proc, m)); }

    private static Timestamp ts(LocalDateTime t) { return t == null ? null : Timestamp.valueOf(t); }

    /** @return the invoice id (num3). */
    public int setData(SaleStInvoiceFinancial.Doc obj, SaleStInvoiceFinancial.Voucher voucher, String screen, SaleEngrAttachments.Change attach) {
        if (obj.details == null || obj.details.isEmpty()) throw new IllegalArgumentException("Detail List not found");
        Timestamp now = SaleEngrSupport.now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", obj.id);
        h.put("DocumentTypeId", obj.documentTypeId);
        h.put("DocNo", obj.docNo);
        h.put("DocDate", ts(obj.docDate));
        h.put("SupplierCustomerId", obj.supplierCustomerId);
        h.put("SupplierReferenceNo", obj.supplierReferenceNo);
        h.put("ManualBillNo", obj.manualBillNo);
        h.put("CommAgentId", obj.commAgentId);
        h.put("CommType", obj.commType);
        h.put("CommRate", obj.commRate);
        h.put("CommUOM", obj.commUom);
        h.put("CommAmount", obj.commAmount);
        h.put("CommRemarks", obj.commRemarks);
        h.put("RemarksHeader", obj.remarksHeader);
        h.put("EntryDate", now);
        h.put("EntryUserId", obj.entryUserId);
        h.put("ModifyDate", now);
        h.put("ModifyUserId", obj.modifyUserId);
        h.put("ApprovedDate", now);
        h.put("OrganizationId", obj.organizationId);
        h.put("CompanyId", obj.companyId);
        h.put("BranchesId", obj.branchesId);
        h.put("ProjectsId", obj.projectsId);
        h.put("FinancialYearId", obj.financialYearId);
        h.put("TransporterId", obj.transporterId);
        h.put("FreightAmount", obj.freightAmount);
        h.put("DeliveryTerm", obj.deliveryTerm);
        h.put("DueDays", obj.dueDays);
        h.put("DueDate", ts(obj.dueDate));
        h.put("DeliverystartDate", obj.deliveryStartDate != null ? ts(obj.deliveryStartDate) : now);
        h.put("ExpiryDate", obj.expiryDate != null ? ts(obj.expiryDate) : now);
        h.put("DeliveryDays", obj.deliveryDays);
        h.put("ApprovedUserId", obj.approvedUserId);
        h.put("CurrencyId", obj.currencyId);
        h.put("ExchangeRate", obj.exchangeRate);
        h.put("FcyAmount", obj.fcyAmount);
        h.put("TotalQty", obj.totalQty);
        h.put("TotalWeight", obj.totalWeight);
        h.put("BillAmount", obj.billAmount);
        h.put("PaymentTermsId", obj.paymentTermsId);
        h.put("InvoiceTypeId", obj.invoiceTypeId);
        String hp = obj.id > 0 ? "[ST].[USP_InvSaleInvoice_Update]" : "[ST].[USP_InvSaleInvoice_Insert]";
        int num3 = call(hp, h);
        if (num3 > 0) obj.id = num3; else num3 = obj.id;

        for (SaleStInvoiceFinancial.Dt d : obj.details) {
            d.itemNetAmount = d.itemAmount.add(d.expenseAmount).subtract(d.commissionAmount);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", d.id);
            m.put("InvSaleInvoiceId", obj.id);
            m.put("InvGdnId", d.invGdnId);
            m.put("InvGdnDetailId", d.invGdnDetailId);
            m.put("SaleOrderId", d.saleOrderId);
            m.put("SaleOrderDetailId", d.saleOrderDetailId);
            m.put("ItemId", d.itemId);
            m.put("PackingTypeId", d.packingTypeId);
            m.put("JobLotId", d.jobLotId);
            m.put("ItemQty", d.itemQty);
            m.put("PackUOMId", d.packUomId);
            m.put("GrossWeight", d.grossWeight);
            m.put("WeightCut", d.weightCut);
            m.put("WeightCutTotal", d.weightCutTotal);
            m.put("AdLsWeight", d.adLsWeight);
            m.put("NetBillWeight", d.netBillWeight);
            m.put("NetStockWeight", d.netStockWeight);
            m.put("ItemRate", d.itemRate);
            m.put("RateUomScheduleId", d.rateUomScheduleId);
            m.put("ItemAmount", d.itemAmount);
            m.put("WarehouseId", d.warehouseId);
            m.put("CityId", d.cityId);
            m.put("GpDate", d.gpDate != null ? ts(d.gpDate) : now);
            m.put("GpNo", d.gpNo);
            m.put("VehicleNo", d.vehicleNo);
            if (d.remarksSent != null) m.put("RemarksDetail", d.remarksSent);
            m.put("RateCut", d.rateCut);
            m.put("RateCutAmount", d.rateCutAmount);
            m.put("ItemNetAmount", d.itemNetAmount);
            m.put("FreightAmount", d.freightAmount);
            m.put("ExpenseAmount", d.expenseAmount);
            m.put("JournalAmount", d.journalAmount);
            m.put("CommissionAmount", d.commissionAmount);
            m.put("LineId", d.lineId);
            m.put("ActionTypeId", d.actionTypeId);
            m.put("CurrencyId", d.currencyId);
            m.put("ExchangeRate", d.exchangeRate);
            m.put("FcyAmount", d.fcyAmount);
            d.id = call("ST.USP_InvSaleInvoiceDetail_Insert", m);
        }
        for (SaleStInvoiceFinancial.Frt f : obj.freights)
            {
            Map<String, Object> fm = row("InvSaleInvoiceId", obj.id, "InvGdnId", f.invGdnId, "CreditAmount", f.creditAmount, "DebitAmount", f.debitAmount, "TansporterId", f.tansporterId);
            fm.put("FrQty", f.frQty); fm.put("FrRate", f.frRate); fm.put("Percentage", f.percentage);
            if (f.remarks != null) fm.put("Remarks", f.remarks);
            call("[ST].[USP_InvSaleInvoiceFreight_Insert]", fm);
        }
        for (SaleStInvoiceFinancial.Jrn j : obj.journals)
            call("[ST].[USP_InvSaleInvoiceJournal_Insert]", row("InvSaleInvoiceId", obj.id, "ChartofAccountId", j.chartofAccountId, "JvRemarks", j.jvRemarks, "JvDebit", j.jvDebit,
                    "JvCredit", j.jvCredit, "JvPrcnt", j.jvPrcnt, "JvQty", j.jvQty, "JvRate", j.jvRate));
        for (SaleStInvoiceFinancial.Exp e : obj.expenses)
            call("[ST].[USP_InvSaleInvoiceExpense_Insert]", row("InvSaleInvoiceId", obj.id, "InvOtherItemId", e.invOtherItemId, "Qty", e.qty, "Rate", e.rate, "Amount", e.amount, "Remarks", e.remarks));

        attachments.apply(screen, obj.documentTypeId, num3, obj.supplierCustomerId, attach);

        int existing = repo.voucherHeadId(obj.organizationId, obj.companyId, obj.documentTypeId, obj.id);
        VoucherHead vh = voucher.head;
        if (existing > 0) vh.Id = existing;
        vh.DocumentTypeSrNo = obj.id;
        vh.RefDocNoId = obj.id;
        int num = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        if (num > 0) vh.Id = num; else num = vh.Id;
        for (VoucherDetail vd : voucher.details) {
            vd.VoucherHeadId = vh.Id;
            repo.setProc("Sp_VoucherDetail_Insert", vd);
        }
        if (voucher.details.isEmpty()) throw new IllegalStateException("Voucher Detail list Not Found");
        repo.run("EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", obj.organizationId, obj.companyId, vh.Id);
        int docTypeRef = repo.setProc("Sp_VoucherHead_H_Insert", vh);
        for (VoucherDetail vd : voucher.details) {
            vd.VoucherHeadId = vh.Id;
            vd.DocumentTypeIdRef = docTypeRef;
            repo.setProc("Sp_VoucherDetail_H_Insert", vd);
        }
        call("Sp_InventoryStockEvalautionDetail_Update", row("OrganizationId", obj.organizationId, "CompanyId", obj.companyId, "RefDocumentTypeId", obj.documentTypeId, "RefDocIdNo", obj.id));
        if (obj.documentTypeId == 1510)
            call("Sp_InventoryTransactions_GetALLMethod", row("OrganizationId", obj.organizationId, "CompanyId", obj.companyId, "RefDocumentTypeId", obj.documentTypeId, "RefDocIdNo", obj.id));
        return num3;
    }
}
