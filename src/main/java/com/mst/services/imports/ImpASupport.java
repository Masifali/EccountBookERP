package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.repositories.imports.ImpARepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.str;
import static com.mst.services.hrm.HrmSupport.toInt;

/**
 * Plumbing shared by the ImpA* Import services (525 / 526 / 221 / 392-395): rights, the combos several
 * forms bind the same way, the decimal configuration (clsGlobalVariables formats) and .NET conversion
 * helpers the HrmSupport statics do not cover (Convert.ToInt32 of a string, "#,#"-style rounding).
 */
@Component
public class ImpASupport {

    @Autowired private HrmSupport hrm;
    @Autowired private ImpARepository repo;

    public HrmSupport hrm() { return hrm; }

    public ImpARepository repo() { return repo; }

    /** { save, update, delete, print, canViewAll } (desktop Rightsobjects). */
    public Map<String, Object> rights(UserAccount u, int screenId) {
        Map<String, Object> r = hrm.rights(u, screenId);
        r.put("canViewAll", hrm.can(u, screenId, "CanView AllRecord"));
        return r;
    }

    /** CommonServices.GetDecimalConfiguration: the three configurations the grids / boxes format with. */
    public Map<String, Object> formats(UserAccount u) {
        int amountRaw = toInt(repo.config(u, "Default NoofDecimal Points For Amount"));
        int rateRaw = toInt(repo.config(u, "Default NoofDecimal Points For Rate"));
        int fcyRaw = toInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("amountDecimals", amountRaw >= 1 && amountRaw <= 4 ? amountRaw : 0);            // stringFormatsingle
        m.put("rateDecimals", rateRaw >= 1 && rateRaw <= 4 ? rateRaw : (rateRaw == 0 ? 2 : 0)); // DecimalRateFormate
        m.put("fcyDecimals", fcyRaw >= 1 && fcyRaw <= 4 ? fcyRaw : 0);                         // stringFormatsingleForFcy
        m.put("fcyRound", Math.max(0, fcyRaw));                                                // DefaultNoofDecimalPointsForFcyAmount
        return m;
    }

    // ------------------------------------------------------------------ combos

    /** Rows reduced to the named columns (the DataTable columns the form binds / reads). */
    public static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) m.put(c, r.get(c));
            out.add(m);
        }
        return out;
    }

    public static Set<Integer> ids(List<Map<String, Object>> rows, String key) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(toInt(r.get(key)));
        return s;
    }

    /** Bank.GetAll split by IsHomeland: "Foreign Country" / "Home Country" rows, as the forms build them. */
    public Map<String, List<Map<String, Object>>> banks(UserAccount u) {
        List<Map<String, Object>> foreign = new ArrayList<>(), home = new ArrayList<>();
        for (Map<String, Object> r : repo.banks(u)) {
            String h = str(r.get("IsHomeland"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id"));
            m.put("BranchName", r.get("BranchName"));
            if ("Foreign Country".equals(h)) foreign.add(m);
            else if ("Home Country".equals(h)) home.add(m);
        }
        Map<String, List<Map<String, Object>>> out = new LinkedHashMap<>();
        out.put("foreign", foreign);
        out.put("home", home);
        return out;
    }

    /** CommonServices.GetUomScheduleByItemId: Id, UOMCode, Equivalent (Cells[2] of the combo is Equivalent). */
    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        if (itemId <= 0) return new ArrayList<>();
        return pick(repo.uomsByItem(u, itemId), "Id", "UOMCode", "Equivalent", "QtyEquivalent", "BaseRateUom");
    }

    /** clsGlobalVariables.ActiveYr.Start_Period as yyyy-MM-dd (null when not found). */
    public String yearStart(UserAccount u) {
        try {
            Object v = repo.financialYearStart(u, hrm.financialYearId());
            LocalDateTime d = HrmSupport.toDate(v);
            return d == null ? null : d.toLocalDate().toString();
        } catch (RuntimeException e) {
            return null;
        }
    }

    // ------------------------------------------------------------------ .NET conversions

    /**
     * Conversion.ToInt of a TextBox text: Convert.ToInt32(string) - only a whole number parses (no group
     * separator, no decimals); anything else is 0.
     */
    public static int netInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return new BigDecimal(String.valueOf(v)).setScale(0, RoundingMode.HALF_EVEN).intValue();
        String s = String.valueOf(v).trim();
        if (!s.matches("[+-]?\\d+")) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDecimal of a text: Convert.ToDecimal (group separators allowed), 0 when not a number. */
    public static BigDecimal dec(Object v) { return HrmSupport.toDec(v); }

    /** decimal.ToString("##,#.##") / ToString(format with n decimals) read back: rounded half away from zero. */
    public static BigDecimal fmtRound(BigDecimal v, int places) {
        return v == null ? BigDecimal.ZERO : v.setScale(places, RoundingMode.HALF_UP);
    }

    public static Timestamp ts(Object day) {
        LocalDateTime d = HrmSupport.toDate(day);
        return d == null ? null : Timestamp.valueOf(d);
    }

    /** A picker's Value: the chosen day with the current time of day (DateTimePicker keeps the time it was created with). */
    public static Timestamp dayNow(Object day) {
        LocalDateTime d = HrmSupport.toDay(day);
        if (d == null) return Timestamp.valueOf(LocalDateTime.now());
        return Timestamp.valueOf(d.toLocalDate().atTime(LocalDateTime.now().toLocalTime()));
    }

    /** Company guard for a row read by id (the ReadById activities do not filter the tenant). */
    public static boolean sameCompany(Map<String, Object> row, UserAccount u) {
        return row != null && toInt(row.get("OrganizationId")) == toInt(u.getOrganizationId())
                && toInt(row.get("CompanyId")) == toInt(u.getCompanyId());
    }
}
