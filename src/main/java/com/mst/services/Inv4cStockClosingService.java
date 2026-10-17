package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Inv4cStockClosingRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Inventory hub — screen 97 "Stock Close Run" (Architecture.WinApp.Inventory_Definition.Stock_Closing/StockCloseRun.cs,
 * base.Name "StockCloseRun") and screen 103 "Stock Closing" (.../MonthlyStockClosing.cs, base.Name "MonthlyStockClosing").
 * The two forms are the same source file except for (a) where the rows come from when no closing exists for the date
 * and (b) which grid columns are visible; {@link Variant} carries the two differences. Line refs ":NNN" are StockCloseRun.cs
 * (MonthlyStockClosing.cs is the same code 5-10 lines earlier). DocumentTypeId 300.
 *
 * <pre>
 *   MonthlyStockClosing_Load :101   rights (Save → btnSave, Print → btnprint + btnSlip), StockCreditAccountBind
 *   StockCreditAccountBind :119     CoaAllocationAccountTitleByAccountTypeIds("10,12") → "Difference Account" combo
 *   btnShow_Click :154              GetAvailableStockForMonthlyClosing :172
 *       ReadByClosingDateFromStockClosing(asOnDate) — rows exist → ActionId 2, RecId/Account/Remarks from row 0
 *       else 97: InvStockAdjustment.GetStockEvaludationDataForAdjustment(ToDate)   103: usp_getAvailableStockForMonthlyClosing
 *   grd_CellUpdated :531            OpeningRate edited → OpeningAmount = Weight / PackEquivalent * OpeningRate (page)
 *   CalculateDifferenceAmount :555  Closing = Σ Value, Opening = Σ OpeningAmount, Difference = Closing − Opening (page)
 *   btnSave_Click :405              validations → confirm → StockMonthlyClosingHeader.Save → message → clear → slip if Preview
 *   btnSlip_Click :590 / btnprint_Click :514   300-StockMonthlyClosingSlip.rpt / 103-AcRptPurchaseSalesVoucherSlip.rpt
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED:
 * <ol>
 *  <li>Qty of 0 is saved as 1 (:478-481).</li>
 *  <li>RateUomId is never copied into the detail model (:468-488), so 0 is saved even though the grid holds it.</li>
 *  <li>ReadByClosingDate sends only @ClosingDate (BLL 0525) — the page shows that day's closing. Web-only: rows of another
 *      company are dropped before they reach the page (the procedure returns OrganizationId/CompanyId).</li>
 *  <li>The header's ActionId is row 0's ActionId; Update is chosen when row 0's HeaderId &gt; 0 (:429-437).</li>
 *  <li>DAL 0379: GetERPFeaturesByCompanyId(…,5) is read and its result ignored ({@code if (false)}); only the "else"
 *      voucher branch exists. A closing whose rows produce no voucher line fails with "VoucherDetail List Not Found"
 *      and nothing is written (the transaction rolls back).</li>
 *  <li>"Record not found for display" when nothing was shown yet (RecId == 0) for both prints (:518, :594).</li>
 * </ol>
 * DEVIATIONS: the slip / voucher data rows are returned to the page, which posts them to /reports/print/grid
 * (300-StockMonthlyClosingSlip.rpt is not on disk — see REPORT.md).
 */
@Service
public class Inv4cStockClosingService {

    public static final int DOC_TYPE = 300;

    /** The two forms. */
    public enum Variant {
        STOCK_CLOSE_RUN("StockCloseRun", "Stock Closing"),
        MONTHLY_STOCK_CLOSING("MonthlyStockClosing", "Stock Monthly Closing");
        public final String screen, title;
        Variant(String screen, String title) { this.screen = screen; this.title = title; }
    }

    private final Inv4cStockClosingRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public Inv4cStockClosingService(Inv4cStockClosingRepository repo, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================== load

    /** MonthlyStockClosing_Load :101 + StockCreditAccountBind :119. */
    public Map<String, Object> lookups(Variant v) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(v.screen));
        out.put("accounts", accounts(u));
        out.put("today", LocalDate.now().toString());
        return out;
    }

    /** DDL.BindDDL(dt, CmbDifferenceAccount, "Id", "AccountTitle", ...) — Id / AccountTitle. */
    private List<Map<String, Object>> accounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.differenceAccounts(u, ctx.currentAppId())) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("AccountTitle", str(ci(r, "AccountTitle")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================== show

    /** GetAvailableStockForMonthlyClosing :172 — dtData rows in its column order, plus RecId / AccountId / Remarks of row 0. */
    public Map<String, Object> show(Variant v, String asOnDate) {
        UserAccount u = ctx.requireAccountingUser();
        Timestamp date = day(asOnDate);
        if (date == null) throw new IllegalArgumentException("As On Date is required.");
        String closingDate = asOnDate.substring(0, 10);
        List<Map<String, Object>> rows = new ArrayList<>();
        Map<String, Object> out = new LinkedHashMap<>();
        int recId = 0, accountId = 0;
        String remarks = "";

        List<Map<String, Object>> existing = new ArrayList<>();
        for (Map<String, Object> r : repo.readByClosingDate(date)) {                           // note 3
            if (toInt(ci(r, "OrganizationId")) != u.getOrganizationId() || toInt(ci(r, "CompanyId")) != u.getCompanyId()) continue;
            existing.add(r);
        }
        if (existing.isEmpty()) {
            List<Map<String, Object>> src = v == Variant.STOCK_CLOSE_RUN
                    ? repo.stockEvaluationDataForAdjustment(u, date)                           // :210
                    : repo.availableStockForMonthlyClosing(u, date);                           // MonthlyStockClosing.cs:205
            for (Map<String, Object> r : src) {                                                // :220
                rows.add(row(0, 0, 1, closingDate, toInt(ci(r, "ItemId")), str(ci(r, "ItemName")), toInt(ci(r, "ItemUomId")),
                        str(ci(r, "PackUom")), toDouble(ci(r, "Equivalent")), toInt(ci(r, "WarehouseId")), str(ci(r, "WareHouseName")),
                        str(ci(r, "CropBatch")), toInt(ci(r, "JobLotId")), str(ci(r, "JobLotDescription")), toInt(ci(r, "InvPackingTypeId")),
                        str(ci(r, "PackTypeDesc")), toDouble(ci(r, "BalQty")), toDouble(ci(r, "BalWeight")), toDouble(ci(r, "BalAmount")),
                        toDouble(ci(r, "AvgRate")), 0d, toInt(ci(r, "ItemUomId")), toDouble(ci(r, "Equivalent")), 0d));
            }
        } else {
            for (int j = 0; j < existing.size(); j++) {                                        // :226
                Map<String, Object> r = existing.get(j);
                if (j == 0) {
                    recId = toInt(ci(r, "HeaderId"));
                    accountId = toInt(ci(r, "AccountId"));
                    remarks = str(ci(r, "Remarks"));
                }
                rows.add(row(toInt(ci(r, "HeaderId")), toInt(ci(r, "Id")), 2, closingDate, toInt(ci(r, "ItemId")), str(ci(r, "ItemName")),
                        toInt(ci(r, "ItemUomId")), str(ci(r, "PackUom")), toDouble(ci(r, "PackEquivalent")), toInt(ci(r, "WarehouseId")),
                        str(ci(r, "WareHouseName")), str(ci(r, "CropYear")), toInt(ci(r, "JobLotId")), str(ci(r, "JobLotDescription")),
                        toInt(ci(r, "InvPackingTypeId")), str(ci(r, "PackTypeDesc")), toDouble(ci(r, "Qty")), toDouble(ci(r, "Weight")),
                        toDouble(ci(r, "Amount")), toDouble(ci(r, "ClosingRate")), toDouble(ci(r, "OpeningRate")), toInt(ci(r, "RateUomId")),
                        toDouble(ci(r, "RateUom")), toDouble(ci(r, "OpeningAmount"))));
            }
        }
        out.put("RecId", recId);
        out.put("AccountId", accountId);
        out.put("Remarks", remarks);
        out.put("rows", rows);
        return out;
    }

    /** dtData.Columns :183-206 in order. */
    private static Map<String, Object> row(int headerId, int detailId, int actionId, String closingDate, int itemId, String itemName,
                                           int packUomId, String packUom, double packEquivalent, int warehouseId, String warehouse,
                                           String cropBatch, int jobLotId, String jobLot, int packingTypeId, String packingType,
                                           double qty, double weight, double value, double closingRate, double openingRate,
                                           int rateUomId, double rateUom, double openingAmount) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("HeaderId", headerId);
        o.put("DetailId", detailId);
        o.put("ActionId", actionId);
        o.put("ClosingDate", closingDate);
        o.put("ItemId", itemId);
        o.put("ItemName", itemName);
        o.put("PackUomId", packUomId);
        o.put("PackUom", packUom);
        o.put("PackEquivalent", packEquivalent);
        o.put("WarehouseId", warehouseId);
        o.put("WareHouseName", warehouse);
        o.put("CropBatch", cropBatch);
        o.put("JobLotId", jobLotId);
        o.put("JobLotDescription", jobLot);
        o.put("InvPackingTypeId", packingTypeId);
        o.put("PackingType", packingType);
        o.put("Qty", qty);
        o.put("Weight", weight);
        o.put("Value", value);
        o.put("ClosingRate", closingRate);
        o.put("OpeningRate", openingRate);
        o.put("RateUomId", rateUomId);
        o.put("RateUom", rateUom);
        o.put("OpeningAmount", openingAmount);
        return o;
    }

    // ================================================================================== save

    /**
     * btnSave_Click :405 — the checks before the confirmation box ("No Record found", "Difference Account field is required",
     * "Remarks field is required") and whether the confirmation is "Are you sure to Update?" (row 0 HeaderId &gt; 0).
     */
    public Map<String, Object> precheck(Variant v, Map<String, Object> body) {
        ctx.requireAccountingUser();
        requireSave(v);
        List<Map<String, Object>> rows = rows(body);
        validate(body, rows);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("update", toInt(ci(rows.get(0), "HeaderId")) > 0);
        return out;
    }

    private void requireSave(Variant v) {
        if (!rights.has(v.screen, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rows(Map<String, Object> body) {
        Object o = body.get("rows");
        return o instanceof List ? (List<Map<String, Object>>) o : new ArrayList<>();
    }

    private static void validate(Map<String, Object> body, List<Map<String, Object>> rows) {
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record found");                              // :417
        if (toInt(body.get("AccountId")) == 0) throw new IllegalArgumentException("Difference Account field is required");   // :422
        String remarks = str(body.get("Remarks"));
        if (remarks.isEmpty()) throw new IllegalArgumentException("Remarks field is required");                  // :427
        if (day(str(body.get("AsOnDate"))) == null) throw new IllegalArgumentException("As On Date is required.");
    }

    /** btnSave_Click :442-491 + BLL 0525 Save + DAL 0379 SetData, one transaction. */
    @Transactional
    public Map<String, Object> save(Variant v, Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        requireSave(v);
        List<Map<String, Object>> rows = rows(body);
        validate(body, rows);
        int accountId = toInt(body.get("AccountId"));
        if (!containsId(accounts(u), accountId)) throw new IllegalArgumentException("Difference Account is not one of this company's accounts.");
        String remarks = str(body.get("Remarks"));
        Timestamp docDate = day(str(body.get("AsOnDate")));
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> row0 = rows.get(0);
        int recId = toInt(ci(row0, "HeaderId"));
        int headerActionId = toInt(ci(row0, "ActionId"));
        if (recId > 0) {
            /* web-only: an existing header must be a 300 closing of the signed-in company */
            boolean mine = false;
            for (Map<String, Object> r : repo.readByClosingDate(docDate)) {
                if (toInt(ci(r, "HeaderId")) == recId && toInt(ci(r, "OrganizationId")) == u.getOrganizationId()
                        && toInt(ci(r, "CompanyId")) == u.getCompanyId()) { mine = true; break; }
            }
            if (!mine) throw new IllegalArgumentException("Record Not Found");
        }

        /* Model 0932 StockMonthlyClosingHeader (:442-453) — every non-virtual property. */
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("DocDate", docDate);
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("AccountId", accountId);
        head.put("ActionId", headerActionId);
        head.put("CompanyId", u.getCompanyId());
        head.put("DocumentTypeId", DOC_TYPE);
        head.put("DocNo", 0);
        head.put("EntryUserId", u.getId());
        head.put("FinancialYearId", ctx.currentFinancialYearId());
        head.put("Id", recId);
        head.put("ModifyUserId", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("Remarks", remarks);

        /* Model 0933 StockMonthlyClosing (:457-489). */
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("IsApproved", false);
            d.put("ApprovedDate", now);
            d.put("ClosingDate", day(str(ci(r, "ClosingDate"))) == null ? docDate : day(str(ci(r, "ClosingDate"))));
            d.put("EntryDate", now);
            d.put("ModifyDate", now);
            d.put("Amount", toDouble(ci(r, "Value")));
            d.put("ClosingRate", toDouble(ci(r, "ClosingRate")));
            d.put("OpeningRate", toDouble(ci(r, "OpeningRate")));
            d.put("OpeningAmount", toDouble(ci(r, "OpeningAmount")));
            double qty = toDouble(ci(r, "Qty"));
            d.put("Qty", qty == 0d ? 1d : qty);                                                                  // note 1
            d.put("Weight", toDouble(ci(r, "Weight")));
            d.put("ActionId", toInt(ci(r, "ActionId")));
            d.put("HeaderId", 0);
            d.put("ApprovedUserId", u.getId());
            d.put("CompanyId", u.getCompanyId());
            d.put("DocumentTypeId", DOC_TYPE);
            d.put("EntryUserId", u.getId());
            d.put("FinancialYearId", ctx.currentFinancialYearId());
            d.put("Id", toInt(ci(r, "DetailId")));
            d.put("ItemId", toInt(ci(r, "ItemId")));
            d.put("ItemUomId", toInt(ci(r, "PackUomId")));
            d.put("WarehouseId", toInt(ci(r, "WarehouseId")));
            d.put("JobLotId", toInt(ci(r, "JobLotId")));
            d.put("InvPackingTypeId", toInt(ci(r, "InvPackingTypeId")));
            d.put("ModifyUserId", u.getId());
            d.put("OrganizationId", u.getOrganizationId());
            d.put("AccountId", accountId);
            d.put("RateUomId", 0);                                                                                // note 2
            d.put("LineId", 0);
            d.put("CropYear", str(ci(r, "CropBatch")));
            d.put("Remarks", remarks);
            details.add(d);
        }

        int id = persist(u, head, details);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", headerActionId == 2 ? "Record Update Successfully " : "Record Save Successfully ");           // :494/:498
        out.put("id", id);
        return out;
    }

    /** DAL 0379 SetData — header, details, the "else" voucher branch, the two closing procedures, VoucherHead/Detail (+_H). */
    private int persist(UserAccount u, Map<String, Object> head, List<Map<String, Object>> details) {
        int recId = toInt(head.get("Id"));
        int num = repo.setProc(recId == 0 ? "USP_StockMonthlyClosingHeader_Insert" : "USP_StockMonthlyClosingHeader_Update", head);
        if (num > 0) head.put("Id", num); else num = recId;
        if (num <= 0) throw new RuntimeException("Save returned no closing id.");
        int line = 1;
        for (Map<String, Object> d : details) {
            d.put("LineId", line++);
            d.put("HeaderId", num);
            d.put("Id", repo.setProc("[dbo].[USP_StockMonthlyClosing_InsertAndUpdate]", d));
        }

        Timestamp docDate = (Timestamp) head.get("DocDate");
        int accountId = toInt(head.get("AccountId"));
        List<Map<String, Object>> gl = repo.itemGlIdsAndNames(u);
        List<Map<String, Object>> lines = new ArrayList<>();
        double debit = 0d, credit = 0d;                                                                           // num10 / num11
        for (Map<String, Object> d : details) {
            Map<String, Object> g = null;
            for (Map<String, Object> x : gl) if (toInt(ci(x, "Id")) == toInt(d.get("ItemId"))) { g = x; break; }
            if (g == null) continue;
            double qty = toDouble(d.get("Qty")), weight = toDouble(d.get("Weight")), openingRate = toDouble(d.get("OpeningRate"));
            double openingAmount = toDouble(d.get("OpeningAmount")), amount = toDouble(d.get("Amount"));
            String text = "ItemQty: " + clr(qty) + "   " + str(ci(g, "ItemName")) + " Weight: " + clr(weight);
            if (openingRate > 0d) text += " Opening Rate: " + clr(openingRate);
            int purchaseGl = toInt(ci(g, "PurchaseGLAC"));
            if (openingRate > 0d && openingAmount > 0d) {
                Map<String, Object> vd = Inv4cStockClosingRepository.voucherDetailModel();
                vd.put("LineId", toInt(d.get("LineId")));
                vd.put("IsCGS", 1);
                vd.put("AccountId", purchaseGl);
                vd.put("AgainstAccountId", purchaseGl);
                vd.put("Comments", text);
                vd.put("DebitAmount", openingAmount);
                vd.put("ItemId", toInt(d.get("ItemId")));
                vd.put("QtyIn", qty);
                vd.put("WeightIn", weight);
                vd.put("ItemRate", openingRate);
                vd.put("JobLotId", toInt(d.get("JobLotId")));
                lines.add(vd);
                debit += openingAmount;
            }
            if (amount != 0d) {
                Map<String, Object> vd = Inv4cStockClosingRepository.voucherDetailModel();
                vd.put("LineId", toInt(d.get("LineId")));
                vd.put("IsCGS", 1);
                vd.put("AccountId", purchaseGl);
                vd.put("AgainstAccountId", purchaseGl);
                vd.put("Comments", text);
                if (amount > 0d) {
                    vd.put("CreditAmount", amount);
                } else {
                    vd.put("DebitAmount", Math.abs(amount));
                    debit += Math.abs(amount);
                }
                vd.put("ItemId", toInt(d.get("ItemId")));
                vd.put("QtyOut", qty);
                vd.put("WeightOut", weight);
                vd.put("ItemRate", toDouble(d.get("ClosingRate")));
                vd.put("JobLotId", toInt(d.get("JobLotId")));
                lines.add(vd);
                credit += toDouble(vd.get("CreditAmount"));
            }
        }
        double diff = debit - credit;                                                                              // num6
        if (diff != 0d) {
            Map<String, Object> vd = Inv4cStockClosingRepository.voucherDetailModel();
            vd.put("IsCGS", 1);
            vd.put("AccountId", accountId);
            vd.put("AgainstAccountId", accountId);
            vd.put("Comments", "Stock Difference");
            if (diff > 0d) vd.put("CreditAmount", diff); else vd.put("DebitAmount", Math.abs(diff));
            lines.add(vd);
        }
        Map<String, Object> scope = new LinkedHashMap<>();
        scope.put("OrganizationId", u.getOrganizationId());
        scope.put("CompanyId", u.getCompanyId());
        scope.put("ClosingDate", docDate);
        repo.exec("usp_MonthlyStockClosing_InventoryStockEvalautionDetail", scope);
        repo.exec("usp_MonthlyStockClosing_InventoryTransactions", scope);

        Map<String, Object> vh = Inv4cStockClosingRepository.voucherHeadModel();
        vh.put("VoucherDate", docDate);
        vh.put("VoucherCode", num);
        vh.put("DocumentTypeId", DOC_TYPE);
        vh.put("RefAccountId", accountId);
        vh.put("AgainstAccountId", accountId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        vh.put("EntryDate", now);
        vh.put("ModifyDate", now);
        vh.put("VoucherAmount", diff);
        vh.put("EntryUser", toInt(head.get("EntryUserId")));
        vh.put("ModifyUser", toInt(head.get("ModifyUserId")));
        vh.put("OrganizationId", u.getOrganizationId());
        vh.put("CompanyId", u.getCompanyId());
        vh.put("FinancialYearId", toInt(head.get("FinancialYearId")));
        vh.put("BranchId", 0);
        int num2 = repo.voucherHeadId(u, DOC_TYPE, num);
        if (num2 > 0) vh.put("Id", num2);
        vh.put("DocumentTypeSrNo", num);
        vh.put("RefDocNoId", num);
        int num3 = repo.setProc(num2 == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        if (num3 > 0) vh.put("Id", num3);
        int voucherId = toInt(vh.get("Id"));
        if (lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");                   // note 5
        for (Map<String, Object> vd : lines) {
            vd.put("VoucherHeadId", voucherId);
            repo.setProc("Sp_VoucherDetail_Insert", vd);
        }
        if (voucherId > 0) {
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("OrganizationId", u.getOrganizationId());
            p.put("CompanyId", u.getCompanyId());
            p.put("Id", voucherId);
            repo.exec("USP_VoucherBalanceCheck", p);
        }
        int documentTypeIdRef = repo.setProc("Sp_VoucherHead_H_Insert", vh);
        for (Map<String, Object> vd : lines) {
            vd.put("VoucherHeadId", voucherId);
            vd.put("DocumentTypeIdRef", documentTypeIdRef);
            repo.setProc("Sp_VoucherDetail_H_Insert", vd);
        }
        return num;
    }

    // ================================================================================== print

    /** btnSlip_Click :590 / after-save preview :503 → CommonServices.StockMonthlyClosing_Slip(datAsOnDate). */
    public List<Map<String, Object>> slip(Variant v, int recId, String asOnDate, boolean afterSave) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(v.screen, "print")) throw new IllegalStateException("You do not have the Print right for this screen.");
        if (!afterSave && recId == 0) throw new IllegalArgumentException("Record not found for display");         // :594
        Timestamp date = day(asOnDate);
        if (date == null) throw new IllegalArgumentException("Closing Date Not Found");
        List<Map<String, Object>> rows = repo.slip(u, date);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record not found for display");
        return plain(rows);
    }

    /** btnprint_Click :514 → AcRptPurchaseSalesVoucherSlip_103(VoucherHeadIdGet(RecId, 300), 300). */
    public List<Map<String, Object>> voucher(Variant v, int recId) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(v.screen, "print")) throw new IllegalStateException("You do not have the Print right for this screen.");
        if (recId == 0) throw new IllegalArgumentException("Record not found for display");                       // :520
        int vh = repo.voucherHeadId(u, DOC_TYPE, recId);
        if (vh == 0) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> rows = repo.voucherSlip(u, vh, DOC_TYPE);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        return plain(rows);
    }

    // ================================================================================= helpers

    private static List<Map<String, Object>> plain(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object val = e.getValue();
                if (val instanceof java.sql.Timestamp || val instanceof java.sql.Date || val instanceof java.util.Date
                        || val instanceof java.time.LocalDateTime || val instanceof java.time.LocalDate) val = String.valueOf(val);
                o.put(e.getKey(), val);
            }
            out.add(o);
        }
        return out;
    }

    private static boolean containsId(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(ci(r, "Id")) == id) return true;
        return false;
    }

    /** "yyyy-MM-dd" (or longer) → midnight Timestamp; null when blank/invalid. */
    static Timestamp day(String iso) {
        if (iso == null || iso.trim().length() < 10) return null;
        try { return Timestamp.valueOf(LocalDate.parse(iso.trim().substring(0, 10)).atStartOfDay()); }
        catch (RuntimeException e) { return null; }
    }
}
