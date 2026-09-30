package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.repositories.partyprocessing.PpCRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpCSupport.*;

/**
 * LoadavailableTransactionsForIssuancePartyProcessing.cs - the issuance loader dialog opened by Stock Adjustment
 * (btnLoadAvailableData), Stock Transfer (btnLoadInvoices), Stock Conversion (btnLoadInput) and Production input
 * (btnLoadInvoices). It has no rights of its own: the caller's screen id is checked for View.
 *
 * Load: ComboFill (USP_InventoryTransactionsPartyProcessing_DropDownAndLists, split by ActivityType), FromDate =
 * ActiveYr.Start_Period, PendingInventoryTransactionsForIssuanceLoad (SpInventoryTransactionsPartyProcessing_
 * GetAvailableTransactionsForIssuance with @StockPartyId = the caller's StockPartyId property). The dialog's grid,
 * header selector, Selected Qty / Weight and Load (same-stock-party check, dtIssuance) run in the page.
 */
@Service
public class PpCLoaderService {

    @Autowired private PpCRepository repo;
    @Autowired private PpCSupport pp;

    private static final String[][] GROUPS = {
            {"StockParty", "stockParties"}, {"ReferenceParty", "referenceParties"}, {"RefDocumentType", "refDocTypes"},
            {"RefWarehouse", "refWarehouses"}, {"Warehouse", "warehouses"}, {"JobLot", "jobLots"}, {"Items", "items"},
            {"PackingType", "packingTypes"}, {"CropYear", "cropYears"}};

    public Map<String, Object> combos(int screenId) {
        UserAccount u = pp.user(screenId);
        Map<String, Object> out = new LinkedHashMap<>();
        for (String[] g : GROUPS) out.put(g[1], new ArrayList<Map<String, Object>>());
        for (Map<String, Object> r : repo.loaderCombos(u)) {
            String a = str(r.get("ActivityType"));
            for (String[] g : GROUPS) {
                if (g[0].equals(a)) {
                    @SuppressWarnings("unchecked") List<Map<String, Object>> l = (List<Map<String, Object>>) out.get(g[1]);
                    l.add(m("Id", r.get("Id"), "name", r.get("name")));
                }
            }
        }
        out.put("fromDate", yearStart(u));
        return out;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period (Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId, the active year's row). */
    public Object yearStart(UserAccount u) {
        try {
            List<Map<String, Object>> years = repo.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                    PpCRepository.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
            int y = pp.year();
            Map<String, Object> row = null;
            for (Map<String, Object> r : years) if (toInt(r.get("Id")) == y) { row = r; break; }
            if (row == null && !years.isEmpty()) row = years.get(0);
            return row == null ? null : row.get("Start_Period");
        } catch (RuntimeException e) {
            return null;
        }
    }

    /**
     * PendingInventoryTransactionsForIssuanceLoad. Each row carries the procedure's own columns (dtlst - what the
     * caller reads back from dtIssuance) plus the display columns of the dialog's dtTarget (RefDocType, WareHouse,
     * ReferenceParty).
     */
    public List<Map<String, Object>> list(int screenId, Map<String, Object> b) {
        UserAccount u = pp.user(screenId);
        List<Map<String, Object>> rows = repo.availableForIssuance(u, picker(b.get("fromDate")), picker(b.get("toDate")), toInt(b.get("stockPartyId")),
                toInt(b.get("referencePartyId")), toInt(b.get("refDocumentTypeId")), toInt(b.get("refWarehouseId")), toInt(b.get("warehouseId")),
                toInt(b.get("jobLotId")), toInt(b.get("itemId")), toInt(b.get("packingTypeId")), toInt(b.get("cropYearId")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>(r);
            o.put("RefDocType", r.get("RefDocumentType"));
            o.put("WareHouse", r.get("WareHouseCode"));
            o.put("ReferenceParty", r.get("ReferencePartyName"));
            out.add(o);
        }
        return out;
    }
}
