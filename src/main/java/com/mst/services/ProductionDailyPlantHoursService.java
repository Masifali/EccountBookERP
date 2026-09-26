package com.mst.services;

import com.mst.repositories.ProductionDailyPlantHoursRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ProductionDailyPlantHoursRepository.col;
import static com.mst.repositories.ProductionDailyPlantHoursRepository.toInt;

/**
 * Screen 877 - frmDailyPlantConsumedHours.cs. The page keeps the form state; every database step,
 * and every refusal Insert() makes before its write, runs here again in the desktop's order and
 * wording, so a crafted request cannot skip one. Tenancy, branch, financial year and user always
 * come from {@link CurrentUserContext}, never from the request.
 *
 * The form has no rights object (it never calls SetRightsValueInRightsObject), so nothing here is
 * gated by rights - exactly as on the desktop.
 */
@Service
public class ProductionDailyPlantHoursService {

    @Autowired private ProductionDailyPlantHoursRepository repo;
    @Autowired private CurrentUserContext ctx;

    /* ===================================================================== InitializeComponentMethod */

    /** InitializeComponentMethod:159 - JobOrderNoDbCall + DownTimeReasonDbCall in one task. */
    public Map<String, Object> load() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("screenName", ProductionDailyPlantHoursRepository.DESKTOP_SCREEN_NAME);
        out.put("jobOrders", jobOrders());
        out.put("reasons", reasons());
        return out;
    }

    /** JobOrderNoDbCall:188 - Id / PlanCode (the combo shows PlanCode). */
    public List<Map<String, Object>> jobOrders() {
        return project(repo.jobOrdersForProduction(ctx.currentOrganizationId(), ctx.currentCompanyId(),
                                                   ctx.currentBranchId()), "Id", "PlanCode");
    }

    /** DownTimeReasonDbCall:256 - Id / LookupName. */
    public List<Map<String, Object>> reasons() {
        return project(repo.lookupsByTypeId(ctx.currentOrganizationId(), ctx.currentCompanyId(),
                ProductionDailyPlantHoursRepository.DOWNTIME_REASON_LOOKUP_TYPE), "Id", "LookupName");
    }

    /** PlantBindByJobOrderId:224 - PlantId / PlantName. */
    public List<Map<String, Object>> plants(int jobOrderId) {
        return project(repo.plantsForProductionByJobOrderId(ctx.currentOrganizationId(), ctx.currentCompanyId(),
                ctx.currentBranchId(), ctx.currentFinancialYearId(), jobOrderId), "PlantId", "PlantName");
    }

    /* ================================================================================= Insert */

    /**
     * Insert():304, check for check, then DailyPlantConsumedHours.Save.
     * FormHelper.ValidateControls runs first (combo: ActiveRow null or Value 0 -> "X field is required";
     * text box ValidType.String: whitespace -> "X field is required"; date picker: CheckDateTimeNull
     * -> "X is not valid"), then the four hour rules, each one's exact text.
     *
     * "ActiveRow null" is tested against the same lists the combos are bound from, which is also
     * what keeps a crafted request inside this tenant's job orders and plants.
     *
     * @return true when RecId was > 0 (an update), for the success message.
     */
    public boolean save(Map<String, Object> body) {
        int org = ctx.currentOrganizationId();
        int comp = ctx.currentCompanyId();
        int user = ctx.currentUserId();

        int recId = toInt(body.get("id"));
        int jobOrderId = toInt(body.get("jobOrderId"));
        int plantId = toInt(body.get("plantId"));
        int reasonId = toInt(body.get("reasonId"));
        String consumedText = str(body.get("consumedHours"));
        String totalText = str(body.get("totalWorkingHours"));
        String downtimeText = str(body.get("downtime"));
        LocalDateTime reportDate = dateTime(body.get("reportDate"));

        /* ValidateControls - in the list's order. */
        if (jobOrderId == 0 || !contains(jobOrders(), "Id", jobOrderId)) {
            throw new IllegalArgumentException("Job Order No field is required");
        }
        if (plantId == 0 || !contains(plants(jobOrderId), "PlantId", plantId)) {
            throw new IllegalArgumentException("Plant field is required");
        }
        if (consumedText.trim().isEmpty()) throw new IllegalArgumentException("Occupied Time (hours) field is required");
        if (totalText.trim().isEmpty()) throw new IllegalArgumentException("Total Working Hour field is required");
        if (reportDate == null || reportDate.getYear() <= 1900) throw new IllegalArgumentException("Report Date is not valid");

        double consumed = toDouble(consumedText);
        double total = toDouble(totalText);
        if (consumed > total) throw new IllegalArgumentException("Occupied Time (hours) cannot be greater than Total Working Hours");
        if (total > 24.0) throw new IllegalArgumentException("Total Working Hours cannot be greater than 24 hours");
        if (consumed > 24.0) throw new IllegalArgumentException("Occupied Time (hours) cannot be greater than 24 hours");
        if (toDouble(downtimeText) > 0.0 && reasonId == 0) {
            throw new IllegalArgumentException("DownTime Reason field is required");
        }
        /* Conversion.ToInt(CmbReason.Value) of a value outside the reason list: keep it inside the tenant. */
        if (reasonId != 0 && !contains(reasons(), "Id", reasonId)) {
            throw new IllegalArgumentException("DownTime Reason field is required");
        }

        /* The procedure's UPDATE is `WHERE Id = @Id` with no tenancy; the desktop can only reach
           ids its own history grid showed. The same history procedure (its @Id guard) proves the
           row is this company's before it is touched. */
        if (recId > 0 && repo.formHistory(org, comp, null, null, null, null, null, null, 0, 0, recId).isEmpty()) {
            throw new IllegalArgumentException("Record not found.");
        }

        LocalDateTime now = LocalDateTime.now();
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", recId);
        m.put("@ReportDate", Timestamp.valueOf(reportDate));
        m.put("@JobOrderId", jobOrderId);
        m.put("@PlantId", plantId);
        m.put("@ConsumedHours", consumed);                     /* double  */
        m.put("@EntryDate", Timestamp.valueOf(now));
        m.put("@EntryUserId", user);
        m.put("@ModifyDate", Timestamp.valueOf(now));
        m.put("@ModifyUserId", user);
        m.put("@OrganizationId", org);
        m.put("@CompanyId", comp);
        m.put("@TotalWorkingHours", toDecimal(totalText));     /* decimal */
        m.put("@Downtime", toDecimal(downtimeText));           /* decimal */
        m.put("@ReasonId", reasonId);
        repo.save(m);
        return recId > 0;
    }

    /* ================================================================================ BindGrid */

    /**
     * BindGrid:435. dateType is the checked radio (drdocdate / rdentrydate / rdmodifydate); a date
     * the page sends is a ticked picker (FromDateHistory.Checked), an absent one is unticked.
     */
    public List<Map<String, Object>> history(String dateType, String from, String to, int jobOrderId, int reasonId) {
        Timestamp f = ts(from), t = ts(to);
        Timestamp df = null, dt = null, ef = null, et = null, mf = null, mt = null;
        if ("entry".equals(dateType)) { ef = f; et = t; }
        else if ("modify".equals(dateType)) { mf = f; mt = t; }
        else if ("doc".equals(dateType)) { df = f; dt = t; }
        return repo.formHistory(ctx.currentOrganizationId(), ctx.currentCompanyId(),
                                df, dt, ef, et, mf, mt, jobOrderId, reasonId);
    }

    /* ================================================================================ helpers */

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String text) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put(id, col(r, id));
            m.put(text, col(r, text));
            out.add(m);
        }
        return out;
    }

    private static boolean contains(List<Map<String, Object>> rows, String key, int v) {
        for (Map<String, Object> r : rows) if (toInt(col(r, key)) == v) return true;
        return false;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    /** Conversion.ToDouble: blank or unparsable -> 0. */
    private static double toDouble(String s) {
        String t = s == null ? "" : s.trim().replace(",", "");
        if (t.isEmpty()) return 0d;
        try { return Double.parseDouble(t); } catch (NumberFormatException e) { return 0d; }
    }

    /** Conversion.ToDecimal: blank or unparsable -> 0 (scale kept, as decimal.Parse keeps it). */
    private static BigDecimal toDecimal(String s) {
        String t = s == null ? "" : s.trim().replace(",", "");
        if (t.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(t); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    private static LocalDateTime dateTime(Object o) {
        String s = o == null ? "" : String.valueOf(o).trim();
        if (s.isEmpty()) return null;
        try {
            if (s.length() == 10) return LocalDate.parse(s).atStartOfDay();
            return LocalDateTime.parse(s.length() > 19 ? s.substring(0, 19) : s);
        } catch (Exception e) {
            return null;
        }
    }

    private static Timestamp ts(String s) {
        LocalDateTime d = dateTime(s);
        return d == null ? null : Timestamp.valueOf(d);
    }
}
