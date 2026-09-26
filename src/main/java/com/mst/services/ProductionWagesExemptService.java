package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ProductionWagesExemptRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Wages Exempt Item Schedule - WagesExemptItemSchedule.cs (Lookups), opened from Stock Conversion's
 * "Wages Exempt" button (invfrmStockConversionProduction.cs:7549, new WagesExemptItemSchedule(UserAccount).Show()).
 *
 * The form's checks (FormValiadation, the date order, the overlap against the history grid) and its
 * confirmations run on the page, with the desktop's messages. Everything that decides whose record it
 * is - organisation, company, branch, users, entry/modify/approved dates - is set here and never read
 * from the request.
 */
@Service
public class ProductionWagesExemptService {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionWagesExemptService.class);

    @Autowired private ProductionWagesExemptRepository repo;
    @Autowired private CurrentUserContext currentUserContext;

    /** PackingChangePriceSchedule_Load: ItemBind, WagesAccountBind, DocumentType. Each catches its own error. */
    public Map<String, Object> setup() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("items", repo.items(u)); }
        catch (Exception e) { LOG.warn("Wages exempt: items failed", e); out.put("itemsError", msg(e)); }
        try { out.put("wagesAccounts", repo.wagesAccounts(u)); }
        catch (Exception e) { LOG.warn("Wages exempt: wages accounts failed", e); out.put("wagesAccountsError", msg(e)); }
        try { out.put("documentTypes", repo.documentTypes()); }
        catch (Exception e) { LOG.warn("Wages exempt: document types failed", e); out.put("documentTypesError", msg(e)); }
        return out;
    }

    /** BindHistory - the procedure's rows; the page builds the grid table from them as :180 does. */
    public List<Map<String, Object>> history() {
        return repo.history(currentUserContext.requireAccountingUser());
    }

    /**
     * Insert() :305 from "obj.CompanyId = ..." on. Body: id (RecId), effectedFrom / effectedTo (the two
     * pickers' Value, date and time), refDocumentTypeId, itemId, wagesAccountId.
     * Returns {id} - the Convert.ToInt32 of ExecuteScalar (0 for an update).
     */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = currentUserContext.requireAccountingUser();
        int recId = asInt(body.get("id"));
        /* The Update procedure changes the row by Id alone and rewrites its organisation/company, so an
           id is accepted only when this company's own history lists it (the only ids the desktop's grid
           can hand it). */
        if (recId > 0) {
            boolean own = false;
            for (Map<String, Object> r : repo.history(u)) if (asInt(ci(r, "Id")) == recId) { own = true; break; }
            if (!own) throw new IllegalArgumentException("Record not found.");
        }
        LocalDateTime now = LocalDateTime.now();
        int userId = u.getId();
        Map<String, Object> m = new LinkedHashMap<>();
        /* The model's sixteen properties, every one sent (GenericProvider.SetProc). */
        m.put("@IsApproved", false);
        m.put("@ApprovedDate", now);
        m.put("@EffectedFrom", dateTime(body.get("effectedFrom")));
        m.put("@EffectedTo", dateTime(body.get("effectedTo")));
        m.put("@EntryDate", now);
        m.put("@ModifyDate", now);
        m.put("@ApprovedUserId", 0);
        m.put("@BranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@EntryUserId", userId);
        m.put("@Id", recId);
        m.put("@ItemId", asInt(body.get("itemId")));
        m.put("@ModifyUserId", userId);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@RefDocumentTypeId", asInt(body.get("refDocumentTypeId")));
        m.put("@WagesAccountId", asInt(body.get("wagesAccountId")));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", repo.save(m));
        return out;
    }

    private static LocalDateTime dateTime(Object v) {
        if (v == null || String.valueOf(v).trim().isEmpty()) return LocalDateTime.now();
        String s = String.valueOf(v).trim();
        if (s.length() <= 10) return LocalDate.parse(s).atStartOfDay();
        return LocalDateTime.parse(s.length() > 19 ? s.substring(0, 19) : s);
    }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }
}
