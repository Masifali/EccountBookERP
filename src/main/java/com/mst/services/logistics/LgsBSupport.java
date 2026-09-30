package com.mst.services.logistics;

import com.mst.models.UserAccount;
import com.mst.repositories.logistics.LgsBRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * Shared BLL plumbing of the LgsB logistics screens (949 / 955 / 965 / 967): the desktop's global caches
 * (clsGlobalVariables) rebuilt from their procedures, rights, configuration, the history filter and the
 * .NET number formatting the voucher remarks carry into the database.
 *
 * Tenancy (organization, company, branch, user) always comes from the signed-in user; never from a request.
 */
@Component
public class LgsBSupport {

    @Autowired private HrmSupport hrm;
    @Autowired private LgsBRepository repo;
    @Autowired private CurrentUserContext ctx;

    public UserAccount user(int screenId) { return hrm.user(screenId); }

    public void require(UserAccount u, int screenId, String action) { hrm.require(u, screenId, action); }

    public int fy() { return hrm.financialYearId(); }

    /** SetRightsValueInRightsObject: save / update / delete / print on the screen, plus DoHaveCanViewAllRecordRights. */
    public Map<String, Object> rights(UserAccount u, int screenId, String screenName) {
        Map<String, Object> r = hrm.rights(u, screenId);
        r.put("viewAll", canViewAll(u, screenName));
        return r;
    }

    /**
     * formright.DoHaveCanViewAllRecordRights: Admin / Administrator short-circuit to true, otherwise the
     * "CanView AllRecord" row of Sp_tblUserRights_GetAllMethod 'GetByUserId' for the form's ScreenName.
     * Any failure answers false (the history then shows the user's own entries only).
     */
    public boolean canViewAll(UserAccount u, String screenName) {
        String role = ctx.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            for (Map<String, Object> r : repo.userRights(u, screenName, role)) {
                Object n = r.get("RightName");
                if (n != null && "CanView AllRecord".equalsIgnoreCase(String.valueOf(n).trim())) return toBool(r.get("Value"));
            }
        } catch (RuntimeException ignored) {
            // restrictive answer
        }
        return false;
    }

    public String config(UserAccount u, String description) {
        try { return repo.config(u, description); } catch (RuntimeException e) { return ""; }
    }

    public boolean feature(UserAccount u, int id) {
        try { return repo.feature(u, id); } catch (RuntimeException e) { return false; }
    }

    // ------------------------------------------------------------------ global caches

    /**
     * SupplierDtFillFromGlobal: dtSupplier columns Id, CompanyName, PartyCode, GlAccountId, CityName, MobileNo,
     * CurrencyId, CurrencyCode from clsGlobalVariables.globalAllSupplierCustomer (group 10 only when asked).
     */
    public List<Map<String, Object>> suppliers(UserAccount u, boolean onlyGroup10) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.suppliers(u)) {
            if (onlyGroup10 && toInt(r.get("CustomerGroupId")) != 10) continue;
            out.add(map("Id", toInt(r.get("Id")), "CompanyName", str(r.get("CompanyName")), "PartyCode", str(r.get("PartyCode")),
                    "GlAccountId", toInt(r.get("GlAccountId")), "CityName", str(r.get("CityName")), "MobileNo", str(r.get("MobilePersonal")),
                    "CurrencyId", toInt(r.get("GlAccountCurrencyId")), "CurrencyCode", str(r.get("GlAccountCurrencyCode"))));
        }
        return out;
    }

    /** clsGlobalVariables.getGlobalAllSerivesItems (Id, ItemName, ItemCode, ServicesMasterItemId/ServiceMasterItem, ItemCategoryId/ItemCategory). */
    public List<Map<String, Object>> serviceItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.serviceItems(u)) {
            out.add(map("Id", toInt(r.get("Id")), "ItemName", str(r.get("ItemName")), "ItemCode", str(r.get("ItemCode")),
                    "MasterItemId", toInt(r.get("ServicesMasterItemId")), "MasterItem", str(r.get("ServiceMasterItem")),
                    "ItemCategoryId", toInt(r.get("ItemCategoryId")), "ItemCategory", str(r.get("ItemCategory"))));
        }
        return out;
    }

    /** CurrencyBindFromGlobal: Id, CurrencyCode, CurrencyRate. */
    public List<Map<String, Object>> currencies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.currencies(u)) {
            out.add(map("Id", toInt(r.get("Id")), "CurrencyCode", str(r.get("CurrencyCode")), "CurrencyRate", toDouble(r.get("CurrencyRate"))));
        }
        return out;
    }

    /** clsGlobalVariables.globalAllCities: Id, CityName (DatatableHelper.PopulateDataTableAndReturn names it Description). */
    public List<Map<String, Object>> cities(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cities(u)) out.add(map("Id", toInt(r.get("Id")), "Description", str(r.get("CityName"))));
        return out;
    }

    /** clsGlobalVariables.AllAccountsWithCustomGroupId (raw rows). */
    public List<Map<String, Object>> accounts(UserAccount u) { return repo.accounts(u); }

    /**
     * DatatableHelper.GetAccountsFromGlobalByTypeIds / BillToCrAccountBindFromGlobal: distinct ChartOfAccountId of the
     * given AccountTypeIds -> Id, AccountTitle, AccountCode, AccountClass, AccountType, ParentAccountTitle.
     */
    public List<Map<String, Object>> accountsOfTypes(List<Map<String, Object>> all, int... typeIds) {
        Set<Integer> types = new HashSet<>();
        for (int t : typeIds) types.add(t);
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> a : all) {
            if (!types.contains(toInt(a.get("AccountTypeId")))) continue;
            int id = toInt(a.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            out.add(map("Id", id, "AccountTitle", str(a.get("AccountTitle")), "AccountCode", str(a.get("AccountCode")),
                    "AccountClass", str(a.get("AccountClassName")), "AccountType", str(a.get("AccountType")),
                    "ParentAccountTitle", str(a.get("ParentAccountTitle"))));
        }
        return out;
    }

    /** LookUps.LookUpAllServices rows: Activity, Id, ReferenceName, OtherReference. */
    public List<Map<String, Object>> lookUps(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.lookUpAllServices(u)) {
            out.add(map("Activity", str(r.get("Activity")), "Id", toInt(r.get("Id")), "ReferenceName", str(r.get("ReferenceName")),
                    "OtherReference", str(r.get("OtherReference"))));
        }
        return out;
    }

    /** SeaPorts (LocationdtFill case 16): Id, LocationName. */
    public List<Map<String, Object>> ports(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.seaPorts(u)) out.add(map("Id", toInt(r.get("Id")), "LocationName", str(r.get("PortName"))));
        return out;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period for the date-type combo (5 = Financial Year). */
    public String yearStart(UserAccount u) {
        int fy = fy();
        try {
            for (Map<String, Object> r : repo.financialYears(u)) {
                if (toInt(r.get("Id")) == fy) {
                    LocalDateTime d = toDate(r.get("Start_Period"));
                    return d == null ? null : d.toLocalDate().toString();
                }
            }
        } catch (RuntimeException ignored) {
            // no year start: the page leaves From Date as it is
        }
        return null;
    }

    /**
     * CommonServices.GetDecimalConfiguration: amount decimals (Math.Round digits: the raw configured value) and
     * stringFormatsingle ("#,##0." + N zeros when N is 1-4, otherwise no decimals).
     */
    public Map<String, Object> formats(UserAccount u) {
        int amountRaw = toInt(config(u, "Default NoofDecimal Points For Amount"));
        int rateRaw = toInt(config(u, "Default NoofDecimal Points For Rate"));
        int fcyRaw = toInt(config(u, "DefaultNoOfDecimalPointsForFcyRate"));
        /* DecimalFCYRateFormate "#,#0." + N zeros: N = DefaultNoOfDecimalPointsForFcyRate (1-10), 4 when unset. */
        return map("amountRound", amountRaw, "amountDecimals", amountRaw >= 1 && amountRaw <= 4 ? amountRaw : 0,
                "rateDecimals", rateRaw >= 1 && rateRaw <= 4 ? rateRaw : (rateRaw == 0 ? 2 : 0),
                "fcyRateDecimals", fcyRaw >= 1 && fcyRaw <= 10 ? fcyRaw : 4);
    }

    // ------------------------------------------------------------------ history filter (ReportsParameters)

    /**
     * HistoryGridFill: the date pair goes to @FromDate/@ToDate, @EntryFromDate/..., @ModifyFromDate/... or
     * @ApprovedFromDate/... by the ticked radio, each only when its picker is checked (the page sends blank when
     * not); @FromDocNo / @ToDocNo only when != 0; @CanViewAllRecord always, @EntryUserId only when it is false.
     */
    public Map<String, Object> historyParams(UserAccount u, int documentTypeId, boolean viewAll, Map<String, Object> f) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Activity", "FormHistory");
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", u.getBranchesId());
        p.put("FinancialYearId", fy());
        p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUserId", u.getId());
        String kind = str(f.get("dateKind"));
        LocalDateTime from = toDate(f.get("fromDate")), to = toDate(f.get("toDate"));
        String a = "doc".equals(kind) || kind.isEmpty() ? "" : kind;
        switch (a) {
            case "entry": p.put("EntryFromDate", from); p.put("EntryToDate", to); break;
            case "modify": p.put("ModifyFromDate", from); p.put("ModifyToDate", to); break;
            case "approved": p.put("ApprovedFromDate", from); p.put("ApprovedToDate", to); break;
            default: p.put("FromDate", from); p.put("ToDate", to); break;
        }
        int fromNo = toInt(f.get("fromDocNo")), toNo = toInt(f.get("toDocNo"));
        if (fromNo != 0) p.put("FromDocNo", fromNo);
        if (toNo != 0) p.put("ToDocNo", toNo);
        return p;
    }

    // ------------------------------------------------------------------ tenancy guard

    /** The row belongs to the signed-in organization and company (ReadById / DeleteById procedures do not filter them). */
    public static boolean ownRow(UserAccount u, Map<String, Object> r) {
        return r != null && toInt(r.get("OrganizationId")) == u.getOrganizationId() && toInt(r.get("CompanyId")) == u.getCompanyId();
    }

    // ------------------------------------------------------------------ .NET formatting of values that are stored in text

    /** double.ToString() on .NET Framework: "G15" - 15 significant digits, no exponent for the ranges used here. */
    public static String netDouble(double d) {
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        BigDecimal b = new BigDecimal(d).round(new java.math.MathContext(15, RoundingMode.HALF_EVEN)).stripTrailingZeros();
        return b.toPlainString();
    }

    /** decimal.ToString(): the value with its own scale ("100.50" stays "100.50"). */
    public static String netDecimal(BigDecimal d) { return d == null ? "0" : d.toPlainString(); }

    /** Conversion.ToDecimal of page text, scale kept as typed (the decimal the desktop parsed). */
    public static BigDecimal dec(Object v) {
        if (v == null) return BigDecimal.ZERO;
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Integer || v instanceof Long) return BigDecimal.valueOf(((Number) v).longValue());
        if (v instanceof Number) return new BigDecimal(String.valueOf(((Number) v).doubleValue())).stripTrailingZeros();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(s); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    /** DateTime default ToString() (en-US culture): "M/d/yyyy h:mm:ss tt". */
    public static String netDateTime(LocalDateTime d) {
        if (d == null) return "1/1/0001 12:00:00 AM";
        return d.format(java.time.format.DateTimeFormatter.ofPattern("M/d/yyyy h:mm:ss a", java.util.Locale.US));
    }

    /** Math.Round(v, digits) (MidpointRounding.ToEven). */
    public static double roundEven(double v, int digits) {
        if (digits < 0) digits = 0;
        return new BigDecimal(Double.toString(v)).setScale(Math.min(digits, 15), RoundingMode.HALF_EVEN).doubleValue();
    }

    /** A number read from a request map (Conversion.ToDouble). */
    public static double d(Map<String, Object> m, String k) { return toDouble(m.get(k)); }

    public static int i(Map<String, Object> m, String k) { return toInt(m.get(k)); }

    public static String s(Map<String, Object> m, String k) { return str(m.get(k)); }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Map<String, Object> m, String k) {
        Object v = m == null ? null : m.get(k);
        if (v instanceof List) {
            List<Map<String, Object>> out = new ArrayList<>();
            for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
            return out;
        }
        return new ArrayList<>();
    }
}
