package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 193 "Packing Detail" - Architecture.WinApp.Export.PackingDetailForCommercialInvoice (DocumentTypeId 211 rows of
 * ExImPackingListDetail against a Commercial Invoice). Procedures, each with the desktop's own parameters:
 *
 *   UPS_GetCommercialInvoicenumberForPackingDetail @OrganizationId @CompanyId @DocumentTypeId=211        BindInvoices (Id, InvoiceNo)
 *   [dbo].[USP_ExImPackingListDetail_ReadById]     @Id (= the invoice id)                                 ExImInvoice.GetByID -> ExImPackingListDetailList
 *   Sp_ExImInvoice_GetAllMethod  @OrganizationId @CompanyId @DocumentTypeIds='211' @FinancialYearId [@Id] [@NoOfRecords]
 *                                @Activity='FormHistoryForPackingDetailList'                               HistoryGridFill(50) / LoadAll
 *   Sp_InvoicesVouchersandStocksDelete @OrganizationId @CompanyId @Id @DocumentTypeId=211 @UserId           btnDelete (InvPurchaseInvoice.RemoveByID)
 *
 *   DAL ExImInvoice.SetDataForCommercialInvoicePackingList (one transaction):
 *     Sp_ExImInvoice_GetAllMethod @Id @Activity='ReadById' (voucher head values, read BEFORE the transaction as the DAL does)
 *     Sp_SupplierCustomer_GetAllMethod 'GetGlAccountIdandCompanyNameBySupplierCustomerId'
 *     [dbo].[USP_ExportPackingListDetail_Insert]   the 23 non-virtual ExImPackingListDetail properties, SortNo 1..n
 *     USP_GetERPFeaturesByCompanyId (feature 5 = FIFO costing) and, with it:
 *       Sp_Item_GetAllMethod 'GetItemGlIdsandItemName', USP_GetStockByFifoMethod (FIFOImplemention), Sp_Item_GetAllMethod
 *       'GetEqvilentByItemIdAndUomScheduleId', [dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId],
 *       USP_InventoryStockEvalautionDetail_Insert
 *     without it: Sp_InventoryStockEvalautionDetail_Update (Org, Company, RefDocumentTypeId 211, RefDocIdNo)
 *     Sp_InventoryTransactions_GetALLMethod, USP_InventoryValidation per row, and with feature 5 the CGS voucher:
 *     Sp_Vouchers_GetMethods (VoucherHeadId by 211 / invoice), Sp_VoucherHead_Update (ActionId 2), Sp_VoucherDetail_Insert,
 *     USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert.
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportPackingDetailRepository {

    public static final int DOCUMENT_TYPE_ID = 211;

    private final JdbcTemplate jdbc;

    public ExportPackingDetailRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** ExImInvoice.GetCommercialInvoicenumberForPackingDetail - @SupplierCustomerId only when != 0 (never set here). */
    public List<Map<String, Object>> invoices(UserAccount u) {
        return DesktopProc.rows(jdbc, "UPS_GetCommercialInvoicenumberForPackingDetail", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOCUMENT_TYPE_ID));
    }

    public List<Map<String, Object>> packingListByInvoice(int invoiceId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImPackingListDetail_ReadById]", params("Id", invoiceId));
    }

    /** ExImInvoice.FormHistoryForPackingDetailList: @Id / @NoOfRecords only when != 0. */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, int noOfRecords) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID), "FinancialYearId", financialYearId);
        if (noOfRecords != 0) p.put("NoOfRecords", noOfRecords);
        p.put("Activity", "FormHistoryForPackingDetailList");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** InvPurchaseInvoice.RemoveByID -> DAL AccountandInventoryRemoveById. */
    @Transactional(rollbackFor = Exception.class)
    public void delete(UserAccount u, int id) {
        DesktopProc.scalar(jdbc, "Sp_InvoicesVouchersandStocksDelete", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id,
                "DocumentTypeId", DOCUMENT_TYPE_ID, "UserId", u.getId()));
    }

    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
        }
        return false;
    }

    public List<Map<String, Object>> itemGlAccounts(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Activity", "GetItemGlIdsandItemName"));
    }

    public double equivalent(UserAccount u, int itemId, int scheduleId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId,
                "ScheduleId", scheduleId, "Activity", "GetEqvilentByItemIdAndUomScheduleId"));
        return r.isEmpty() ? 0d : dbl(r.get(0).get("Equivalent"));
    }

    public int voucherHeadId(UserAccount u, int documentTypeId, int srNo) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", params(
                "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", srNo));
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("Id");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    // ========================================================================= save

    /** Everything the save needs, as the form hands it to the DAL. */
    public static class SaveInput {
        public int invoiceId, docNo, supplierCustomerId, branchesId, userId;
        public Timestamp invoiceDate;
        public double exchangeRate;
        public List<Map<String, Object>> rows = new ArrayList<>();          // ExImPackingListDetail models (+ "CropYear" virtual)
        public Map<String, Object> voucherHead;                             // VoucherHead model built from the invoice
        public List<Map<String, Object>> customerGl;                        // already read
        public ModelFactory factory;
    }

    /** Builds the evaluation / voucher detail models (the service owns the DTO-to-map conversion). */
    public interface ModelFactory {
        Map<String, Object> evaluation();
        Map<String, Object> voucherDetail(Map<String, Object> values);
        String num(double d);
    }

    /**
     * DAL ExImInvoice.SetDataForCommercialInvoicePackingList, statement for statement. Returns the id the last
     * USP_ExportPackingListDetail_Insert returned (the form ignores it and prints the invoice id).
     */
    @Transactional(rollbackFor = Exception.class)
    public int savePackingList(UserAccount u, SaveInput in) {
        int result = 0;
        int sort = 0;
        for (Map<String, Object> r : in.rows) {
            sort++;
            r.put("SortNo", sort);
            Map<String, Object> p = new java.util.LinkedHashMap<>(r);
            p.remove("CropYear");                                              // virtual - not sent
            result = DesktopProc.setProc(jdbc, "[dbo].[USP_ExportPackingListDetail_Insert]", p);
            r.put("Id", result);
        }
        if (in.rows.isEmpty()) return result;
        boolean fifo = erpFeature(u, 5);
        List<Map<String, Object>> voucherLines = new ArrayList<>();
        if (fifo) {
            List<Map<String, Object>> gl = itemGlAccounts(u);
            List<Map<String, Object>> evals = new ArrayList<>();
            for (Map<String, Object> r : in.rows) {
                Map<String, Object> g = null;
                for (Map<String, Object> x : gl) if (i(x.get("Id")) == i(r.get("ItemId"))) { g = x; break; }
                if (g == null) continue;
                String name = s(g.get("ItemName"));
                for (Map<String, Object> e : fifoImplementation(u, r, in.invoiceDate, in.invoiceId, name, evals, in.factory)) {
                    String text = "ItemQty: " + in.factory.num(dbl(e.get("QtyOut"))) + "   " + name + "  Net Weight: "
                            + in.factory.num(dbl(e.get("StockWeightOut"))) + "  CGS Rate:" + in.factory.num(dbl(e.get("CgsRate")));
                    Map<String, Object> a = new java.util.LinkedHashMap<>();
                    a.put("LineId", i(r.get("SortNo")));
                    a.put("AccountId", i(g.get("COGSGLAC")));
                    a.put("AgainstAccountId", i(g.get("PurchaseGLAC")));
                    a.put("Comments", text);
                    a.put("DebitAmount", dbl(e.get("CgsAmount")));
                    a.put("DocumentTypeIdRef", 2);
                    a.put("InvoiceNoRefId", in.invoiceId);
                    a.put("RefInvoiceNo", "CGS");
                    a.put("IsCGS", 1);
                    a.put("ItemId", i(r.get("ItemId")));
                    a.put("QtyOut", dbl(e.get("QtyOut")));
                    a.put("WeightOut", dbl(e.get("StockWeightOut")));
                    a.put("ItemCgsRate", dbl(e.get("CgsRate")));
                    a.put("ItemAmount", dbl(e.get("AmountOut")));
                    a.put("JobLotId", i(r.get("JobLotid")));
                    voucherLines.add(in.factory.voucherDetail(a));
                    Map<String, Object> b = new java.util.LinkedHashMap<>(a);
                    b.put("AccountId", i(g.get("PurchaseGLAC")));
                    b.put("AgainstAccountId", i(g.get("COGSGLAC")));
                    b.remove("DebitAmount");
                    b.put("CreditAmount", dbl(e.get("CgsAmount")));
                    voucherLines.add(in.factory.voucherDetail(b));
                    evals.add(e);
                }
            }
            if (!evals.isEmpty()) {
                DesktopProc.scalar(jdbc, "[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", params(
                        "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "RefDocumentTypeId", DOCUMENT_TYPE_ID, "RefDocIdNo", in.invoiceId));
                for (Map<String, Object> e : evals) {
                    e.put("OrganizationId", u.getOrganizationId());
                    e.put("CompanyId", u.getCompanyId());
                    e.put("DocDate", in.invoiceDate);
                    e.put("DocCodeNo", in.docNo);
                    e.put("SupplierCustomerId", in.supplierCustomerId);
                    e.put("BranchesId", in.branchesId);
                    e.put("RefDocumentTypeId", DOCUMENT_TYPE_ID);
                    e.put("EntryUser", in.userId);
                    e.put("ModifyUser", in.userId);
                    e.put("CalcType", "Weight");
                    Map<String, Object> row = null;
                    for (Map<String, Object> r : in.rows) {
                        int ln = i(e.get("LineId"));
                        if (i(r.get("SortNo")) == ln && i(r.get("SortNo")) > 0 && ln > 0) { row = r; break; }
                    }
                    if (row != null) {
                        double eq = equivalent(u, i(e.get("ItemId")), i(row.get("RateUomId")));
                        double bw = dbl(e.get("BillWeightOut")), rp = dbl(row.get("RatePrice"));
                        if (bw > 0 && rp > 0 && eq > 0 && in.exchangeRate > 0) {
                            e.put("AmountOut", bw / eq * rp * in.exchangeRate);
                            e.put("ItemRate", rp * in.exchangeRate);
                        }
                        e.put("RefDocIdNo", in.invoiceId);
                        e.put("RefDocSubIdNo", i(row.get("Id")));
                    }
                    DesktopProc.setProc(jdbc, "USP_InventoryStockEvalautionDetail_Insert", e);
                }
            }
        } else {
            Map<String, Object> e = InventoryOpeningDefaults.evaluation();
            e.put("OrganizationId", u.getOrganizationId());
            e.put("CompanyId", u.getCompanyId());
            e.put("RefDocumentTypeId", DOCUMENT_TYPE_ID);
            e.put("RefDocIdNo", in.invoiceId);
            DesktopProc.setProc(jdbc, "Sp_InventoryStockEvalautionDetail_Update", e);
        }
        Map<String, Object> t = InventoryOpeningDefaults.transactions();
        t.put("OrganizationId", u.getOrganizationId());
        t.put("CompanyId", u.getCompanyId());
        t.put("RefDocumentTypeId", DOCUMENT_TYPE_ID);
        t.put("RefDocIdNo", in.invoiceId);
        DesktopProc.setProc(jdbc, "Sp_InventoryTransactions_GetALLMethod", t);
        for (Map<String, Object> r : in.rows) {
            DesktopProc.scalar(jdbc, "USP_InventoryValidation", params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                    "DocumentTypeId", DOCUMENT_TYPE_ID, "DocDate", in.invoiceDate,
                    "ItemId", i(r.get("ItemId")), "WarehouseId", i(r.get("WarehouseId")), "JobLotId", i(r.get("JobLotid")),
                    "CropYear", r.get("CropYear"), "InvPackingTypeId", i(r.get("ExImPackMaterilaTypeId")),
                    "PackUomId", i(r.get("UOMScheduleIdOuter")), "NetWeight", dbl(r.get("NetWeight"))));
        }
        if (fifo) {
            Map<String, Object> vh = in.voucherHead;
            int num = voucherHeadId(u, DOCUMENT_TYPE_ID, in.invoiceId);
            if (num > 0) vh.put("Id", num);
            if (num <= 0) throw new IllegalStateException("VoucherHeadId Not Found against this InvoiceId " + in.invoiceId);
            vh.put("DocumentTypeSrNo", in.invoiceId);
            vh.put("RefDocNoId", in.invoiceId);
            vh.put("ActionId", 2);
            DesktopProc.setProc(jdbc, "Sp_VoucherHead_Update", vh);
            int vhId = i(vh.get("Id"));
            for (Map<String, Object> d : voucherLines) {
                Map<String, Object> row = null;
                for (Map<String, Object> r : in.rows) {
                    int ln = i(d.get("LineId"));
                    if (i(r.get("SortNo")) == ln && i(r.get("SortNo")) > 0 && ln > 0) { row = r; break; }
                }
                d.put("VoucherHeadId", vhId);
                if (row != null) d.put("RefDocSubIdNo", i(row.get("Id")));
                DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", d);
            }
            if (voucherLines.isEmpty()) throw new IllegalStateException("Voucher Detail list Not Found");
            DesktopProc.scalar(jdbc, "USP_VoucherBalanceCheck", params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", vhId));
            int ref = DesktopProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", vh);
            for (Map<String, Object> d : voucherLines) {
                d.put("VoucherHeadId", vhId);
                d.put("DocumentTypeIdRef", ref);
                DesktopProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", d);
            }
        }
        return result;
    }

    /**
     * DAL 0205 CommonServices.FIFOImplemention:725 for one packing-list row. Parameters as the DAL sends them
     * (@PackUomId / @WarehouseId / @CropYearId / @JobLotId / @PackingTypeId / @CropYear / @DocumentTypeId / @Id only
     * when set; @FIFOXML when earlier rows of this save reserved stock); the messages are the DAL's.
     */
    private List<Map<String, Object>> fifoImplementation(UserAccount u, Map<String, Object> r, Timestamp docDate, int invoiceId,
                                                         String name, List<Map<String, Object>> reserved, ModelFactory f) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", i(r.get("ItemId")), "DocDate", docDate);
        int packUom = i(r.get("UOMScheduleIdOuter")), wh = i(r.get("WarehouseId")), cy = i(r.get("InvCropYearId")),
                jl = i(r.get("JobLotid")), pt = i(r.get("ExImPackMaterilaTypeId"));
        if (packUom != 0) p.put("PackUomId", packUom);
        if (wh != 0) p.put("WarehouseId", wh);
        if (cy != 0) p.put("CropYearId", cy);
        if (jl != 0) p.put("JobLotId", jl);
        if (pt != 0) p.put("PackingTypeId", pt);
        String cropYear = s(r.get("CropYear"));
        if (!cropYear.isEmpty()) p.put("CropYear", cropYear);
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        if (invoiceId != 0) p.put("Id", invoiceId);
        if (!reserved.isEmpty()) {
            StringBuilder sb = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
            for (Map<String, Object> x : reserved) {
                sb.append("<FIFOStockEvaluation><RefDocumentTypeId>").append(i(x.get("RefRefDocumentTypeId")))
                  .append("</RefDocumentTypeId><RefDocIdNo>").append(i(x.get("RefRefDocIdNo")))
                  .append("</RefDocIdNo><RefDocSubIdNo>").append(i(x.get("RefRefDocSubIdNo")))
                  .append("</RefDocSubIdNo><ReserveWeight>").append(f.num(dbl(x.get("StockWeightOut"))))
                  .append("</ReserveWeight><ReserveQty>").append(f.num(dbl(x.get("QtyOut"))))
                  .append("</ReserveQty></FIFOStockEvaluation>");
            }
            p.put("FIFOXML", sb.append("\n</ArrayOfFIFOStockEvaluation>").toString());
        }
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_GetStockByFifoMethod", p);
        if (rows.isEmpty()) throw new IllegalStateException("Stock Not Found this Item " + name + " against FIFO Method .....");
        double sumW = 0;
        for (Map<String, Object> x : rows) sumW += dbl(x.get("NetBalWeight"));
        double itemQty = dbl(r.get("OuterQty")), netWeight = dbl(r.get("NetWeight"));
        if (!(netWeight <= new BigDecimal(sumW).setScale(2, RoundingMode.HALF_EVEN).doubleValue()))
            throw new IllegalStateException("Weight available is " + f.num(sumW) + " and row Weight is " + f.num(netWeight)
                    + " this item " + name + " against FIFO....");
        List<Map<String, Object>> out = new ArrayList<>();
        double num2 = netWeight, num3 = itemQty, num5 = 0, num7 = 0;
        for (Map<String, Object> x : rows) {
            double avg = dbl(x.get("AvgRate"));
            if (avg <= 0) throw new IllegalStateException("Rate Not Found this Item " + name + " against FIFO Method");
            int rateUom = i(x.get("RateUomId"));
            if (rateUom == 0) throw new IllegalStateException("RateUomId not found  this " + name + " against FIFO Method");
            double num6 = dbl(x.get("NetBalWeight")), num4 = dbl(x.get("NetBalQty"));
            double eq = equivalent(u, i(r.get("ItemId")), rateUom);
            if (eq == 0) throw new IllegalStateException("RateUom Not Found");
            Map<String, Object> e = null;
            if (num6 <= num2 - num7) {
                num7 += num6;
                num5 += num4;
                e = fifoRow(f, x, r, rateUom, num4, num6, avg * eq, eq);
            } else if (num6 >= num2 - num7) {
                double q = num3 - num5, w = num2 - num7;
                e = fifoRow(f, x, r, rateUom, q, w, avg * eq, eq);
                num5 += q;
                num7 += w;
            }
            if (e != null) out.add(e);
            if (netWeight == num7) break;
        }
        return out;
    }

    private static Map<String, Object> fifoRow(ModelFactory f, Map<String, Object> x, Map<String, Object> r, int rateUom,
                                               double qtyOut, double weightOut, double cgsRate, double eq) {
        Map<String, Object> e = f.evaluation();
        e.put("Id", i(x.get("Id")));
        e.put("LineId", i(r.get("SortNo")));
        e.put("ItemId", i(r.get("ItemId")));
        e.put("WarehouseId", i(r.get("WarehouseId")));
        e.put("RateUom", rateUom);
        e.put("JobLotId", i(r.get("JobLotid")));
        e.put("InvPackingTypeId", i(r.get("ExImPackMaterilaTypeId")));
        e.put("ItemUom", i(r.get("UOMScheduleIdOuter")));
        e.put("CropBatch", s(r.get("CropYear")));
        e.put("RefRefDocumentTypeId", i(x.get("RefDocumentTypeId")));
        e.put("RefRefDocIdNo", i(x.get("RefDocIdNo")));
        e.put("RefRefDocSubIdNo", i(x.get("RefDocSubIdNo")));
        e.put("QtyOut", qtyOut);
        e.put("BillWeightOut", weightOut);
        e.put("StockWeightOut", weightOut);
        e.put("CgsRate", cgsRate);
        e.put("CgsAmount", weightOut / eq * cgsRate);
        return e;
    }

    private static int i(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static double dbl(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        if (v == null) return 0;
        try { return Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static String s(Object v) { return v == null ? "" : String.valueOf(v); }
}
