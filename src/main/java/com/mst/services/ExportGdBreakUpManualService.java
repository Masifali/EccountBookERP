package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportGdBreakUpManualRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportBankGdSupport.*;

/**
 * The BLL side of 198 GdBreakUpManual "GD Breakup Manual" (Architecture.WinApp.Export). One Form
 * tab: toolStrip4 New / Save / Update(hidden until a history row is double-clicked); panel30 entry;
 * panel1 (Teal) "GD Breakup Manual" caption over grdGdBreakUp, which IS the history (HistoryBind on
 * load and after every Reset). Rights by ScreenDefinition.Id 198: View to load, Save for Save,
 * Update for Update (the desktop form sets no rights on its buttons - it only sets the grid-bar
 * rights - so Save/Update follow the web rights chain). DocumentTypeId is 3 on every row.
 */
@Service
public class ExportGdBreakUpManualService {

    public static final int SCREEN_ID = 198;
    public static final int DOCUMENT_TYPE_ID = 3;

    @Autowired private ExportGdBreakUpManualRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    /** GdBreakUpManual_Load: BankBind + HistoryBind. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        out.put("permissions", perm);
        put(out, "banks", () -> bankRows(repo.banks(u)));
        put(out, "history", () -> historyRows(repo.history(u)));
        return out;
    }

    /** BankBind: Home Country banks only, Id / BranchName. */
    private static List<Map<String, Object>> bankRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!"Home Country".equals(text(ci(r, "IsHomeland")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("BranchName", text(ci(r, "BranchName")));
            out.add(m);
        }
        return out;
    }

    /** HistoryBind -> dtHis columns in the desktop's order (Id, DocumentTypeId, GDWeight, FCL are hidden on the grid). */
    public List<Map<String, Object>> history() { return historyRows(repo.history(user("View"))); }

    private static List<Map<String, Object>> historyRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("BankInvoiceNo", text(ci(r, "BankInvoiceNo")));
            m.put("BankInvoiceDate", iso(ci(r, "BankInvoiceDate")));
            m.put("GDNO", text(ci(r, "GDNO")));
            m.put("GDDate", iso(ci(r, "GDDate")));
            m.put("GDValue", asDouble(ci(r, "GDValue")));
            m.put("GDWeight", asDouble(ci(r, "GDWeight")));
            m.put("FCL", asInt(ci(r, "GDNoofContrainers")));
            m.put("BankName", text(ci(r, "BankName")));
            m.put("DueDays", asInt(ci(r, "DueDays")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("Freight", asDouble(ci(r, "Freight")));
            m.put("FobValue", asDouble(ci(r, "FobValue")));
            m.put("FTTPercent", asDouble(ci(r, "CommPercent")));
            m.put("FttAmount", asDouble(ci(r, "CommAmount")));
            m.put("NetAmount", asDouble(ci(r, "NetToBeRealized")));
            m.put("Remarks", text(ci(r, "Remarks")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            out.add(m);
        }
        return out;
    }

    /** grdGdBreakUp_DoubleClick -> GDBreakUpManual.GetByID(RecId); [0] of an empty list throws on the desktop. */
    public Map<String, Object> byId(int id) {
        user("View");
        List<Map<String, Object>> rows = repo.byId(id);
        if (rows.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> r = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("GDNO", text(ci(r, "GDNO")));
        m.put("GDDate", iso(ci(r, "GDDate")));
        m.put("BankInvoiceNo", text(ci(r, "BankInvoiceNo")));
        m.put("BankInvoiceDate", iso(ci(r, "BankInvoiceDate")));
        m.put("BankId", asInt(ci(r, "BankId")));
        m.put("DueDays", asInt(ci(r, "DueDays")));
        m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
        m.put("GDValue", asDouble(ci(r, "GDValue")));
        m.put("Freight", asDouble(ci(r, "Freight")));
        m.put("FobValue", asDouble(ci(r, "FobValue")));
        m.put("CommPercent", asDouble(ci(r, "CommPercent")));
        m.put("CommAmount", asDouble(ci(r, "CommAmount")));
        m.put("NetToBeRealized", asDouble(ci(r, "NetToBeRealized")));
        m.put("Remarks", text(ci(r, "Remarks")));
        return m;
    }

    /**
     * Insert(): RecId 0 = save (Save right), else update (Update right; btnupdate_Click throws
     * "RecId not found" at 0). GdBreakUpFormValidation in the desktop's order, then the model
     * (DocumentTypeId 3, EntryUserId/ModifyUserId/OrganizationId/CompanyId from the session,
     * ActionTypeId 1/2, EntryDate/ModifyDate = now as the BLL stamps them) through the DAL.
     * Messages: "Save SuccessFully" / "Update SuccessFully".
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId == 0 ? "Save" : "Update");

        String gdNo = text(body.get("gdNo"));
        String bankInvoiceNo = text(body.get("bankInvoiceNo"));
        if (gdNo.isEmpty()) throw new IllegalArgumentException("GdNo field is required");
        if (bankInvoiceNo.isEmpty()) throw new IllegalArgumentException("BankInvoiceNo field is required");
        if (asInt(body.get("dueDays")) <= 0) throw new IllegalArgumentException("Due Days field is Required");
        if (asDouble(body.get("gdValue")) <= 0) throw new IllegalArgumentException("GD Value field is Required");
        if (asDouble(body.get("fobValue")) <= 0) throw new IllegalArgumentException("FOb Value field is Required");
        if (asDouble(body.get("netAmount")) <= 0) throw new IllegalArgumentException("Net Amount field is Required");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", recId);
        m.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        m.put("BankInvoiceNo", bankInvoiceNo);
        m.put("BankInvoiceDate", ts(body.get("bankInvoiceDate")));
        m.put("GDNO", gdNo);
        m.put("GDDate", ts(body.get("gdDate")));
        m.put("GDValue", dec(body.get("gdValue")));
        m.put("GDNoofContrainers", 0);
        m.put("GDWeight", BigDecimal.ZERO);
        m.put("BankId", asInt(body.get("bankId")));
        m.put("DueDays", asInt(body.get("dueDays")));
        m.put("ExchangeRate", dec(body.get("exchangeRate")));
        m.put("Freight", dec(body.get("freight")));
        m.put("FobValue", dec(body.get("fobValue")));
        m.put("CommAmount", dec(body.get("fttAmount")));
        m.put("NetToBeRealized", dec(body.get("netAmount")));
        m.put("CommPercent", dec(body.get("fttPercent")));
        m.put("Remarks", text(body.get("remarks")));
        m.put("ActionTypeId", recId <= 0 ? 1 : 2);
        m.put("EntryDate", now);
        m.put("EntryUserId", u.getId());
        m.put("ModifyDate", now);
        m.put("ModifyUserId", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        m.put("CompanyId", u.getCompanyId());
        int id = repo.save(m);
        return ok(id, recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
    }
}
