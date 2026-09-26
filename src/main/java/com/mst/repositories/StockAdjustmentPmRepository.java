package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 492 "Stock Adjustment For PM" (StockAdjustmentForPM.cs, DocumentTypeId 215): the calls of
 * BLL 0546 / DAL 0401 that {@link StockAdjustmentRepository} hard-wires to DocumentTypeId 70, taken
 * here with 215. The doc-type-free calls (header, details, setProc, features) stay on that class.
 */
@Repository
public class StockAdjustmentPmRepository {

    public static final int DOC = 215;

    private final JdbcTemplate jdbc;
    public StockAdjustmentPmRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** DocumentNoFill:302 - BLL 0546 GenerateCode (org, company, 215, year) - Rows[0]["DocNo"]. */
    public int nextDocNo(UserAccount u, int financialYearId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, StockAdjustmentRepository.P_GETALL, params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC,
                "FinancialYearId", financialYearId,
                "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** EntryTypeBind:330 - StaticColumnsService("StockAdjustmentTypeForPM"); also the history combo. */
    public List<Map<String, Object>> entryTypes() {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", "StockAdjustmentTypeForPM"));
    }

    /** gridhistoryfill:1142 - BLL 0546 FormHistory with DocumentTypeId 215. */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, boolean canViewAll, int entryUser,
                                                 Timestamp from, Timestamp to, int fromDocNo, int toDocNo,
                                                 int adjustmentTypeId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC,
                "FinancialYearId", financialYearId,
                "CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", entryUser);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (adjustmentTypeId != 0) p.put("AdjustmentTypeId", adjustmentTypeId);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, StockAdjustmentRepository.P_GETALL, p);
    }

    /** DAL 0401 :238-256 - USP_InventoryValidation per detail (Loss only). */
    public void inventoryValidation(UserAccount u, Timestamp docDate, Map<String, Object> d) {
        DesktopProc.scalar(jdbc, "USP_InventoryValidation", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC,
                "DocDate", docDate,
                "ItemId", d.get("ItemId"),
                "WarehouseId", d.get("WarehouseId"),
                "JobLotId", d.get("JobLotId"),
                "CropYearId", d.get("CropYearId"),
                "InvPackingTypeId", d.get("PackingTypeId"),
                "PackUomId", d.get("PackUomId"),
                "NetWeight", d.get("NetWeight"),
                "RefDocumentTypeId", d.get("RefDocumentTypeId"),
                "RefDocNoId", d.get("RefDocNoId"),
                "RefDocSubIdNo", d.get("RefDocSubIdNo"),
                "ItemConditionId", d.get("ItemConditionId")));
    }

    /** DAL 0401 :220-236 - evaluation update and transactions recalc for 215. */
    public void evaluationUpdate(UserAccount u, int id) {
        Map<String, Object> e = InventoryOpeningDefaults.evaluation();
        e.put("OrganizationId", u.getOrganizationId());
        e.put("CompanyId", u.getCompanyId());
        e.put("RefDocumentTypeId", DOC);
        e.put("RefDocIdNo", id);
        DesktopProc.setProc(jdbc, "Sp_InventoryStockEvalautionDetail_Update", e);
    }

    public void transactionsRecalc(UserAccount u, int id) {
        Map<String, Object> t = InventoryOpeningDefaults.transactions();
        t.put("OrganizationId", u.getOrganizationId());
        t.put("CompanyId", u.getCompanyId());
        t.put("RefDocumentTypeId", DOC);
        t.put("RefDocIdNo", id);
        DesktopProc.setProc(jdbc, "Sp_InventoryTransactions_GetALLMethod", t);
    }
}
