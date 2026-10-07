package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.CmtrRegisterRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.time.LocalDate;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** Desktop BLL parity for Commission Delivery Loading Register and Commission Transaction Register. */
@Service
public class CmtrRegisterService {
    public static final String LOADING_DELIVERY = "loading-delivery";
    public static final String TRANSACTION = "transaction";

    private final CmtrRegisterRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public CmtrRegisterService(CmtrRegisterRepository repository, CurrentUserContext context,
                               DesktopReportRights rights) {
        this.repository = repository;
        this.context = context;
        this.rights = rights;
    }

    public Map<String, Object> initialize(String register) {
        UserAccount user = requireUser(register);
        Map<String, Object> response = new LinkedHashMap<>();
        // Desktop BLL supplies only OrganizationId and CompanyId to the shared lookup procedure.
        // Branch and FinancialYear belong on the report query, not this dropdown DDL.
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", user.getOrganizationId());
        p.put("CompanyId", user.getCompanyId());
        String proc = LOADING_DELIVERY.equals(register)
                ? "[CmTr].[USP_GetDataForDropDownFromCommTradeLoadingDelivery]"
                : "[CmTr].[USP_GetDataForDropDownFromCommTradeTransaction]";
        response.put("lookups", repository.call(proc, p));
        response.put("today", LocalDate.now().toString());
        response.put("branchId", user.getBranchesId());
        response.put("permissions", Map.of(
                "view", true,
                "gridPrint", allowed(user, register, "Grid Print"),
                "gridExport", allowed(user, register, "Grid Export"),
                "print", allowed(user, register, "Print")));
        return response;
    }

    public List<Map<String, Object>> report(String register, Map<String, String> filters) {
        UserAccount user = requireUser(register);
        LinkedHashMap<String, Object> p = tenancy(user);
        // The WinForms report assigns both date pickers and both doc-number boxes on each Show.
        p.put("FromDate", date(filters.get("fromDate"), "From Date"));
        p.put("ToDate", date(filters.get("toDate"), "To Date"));
        optionalInt(p, "FromDocNo", filters, "fromDocNo");
        optionalInt(p, "ToDocNo", filters, "toDocNo");
        optionalInt(p, "CustomerId", filters, "customerId");
        optionalInt(p, "SupplierId", filters, "supplierId");
        if (LOADING_DELIVERY.equals(register)) {
            optionalInt(p, "BrokerId", filters, "brokerId");
        } else {
            optionalInt(p, "CommissionAgentId", filters, "brokerId");
            optionalInt(p, "SubBrokerId", filters, "subBrokerId");
        }
        optionalInt(p, "ItemId", filters, "itemId");
        optionalInt(p, "ItemCategoryId", filters, "itemCategoryId");
        optionalInt(p, "InventoryParentCategories", filters, "parentCategoryId");
        optionalInt(p, "ItemTypeId", filters, "itemTypeId");
        String procedure = LOADING_DELIVERY.equals(register)
                ? "[CmTr].[USP_CommTradeLoadingDelivery_Register]"
                : "[CmTr].[USP_CommTradeTransactionHeader_Register]";
        return repository.call(procedure, p);
    }

    public Integer printScreenId(String register, String action) {
        UserAccount user = requireUser(register);
        Integer screen = screenId(register);
        if (screen == null) throw new IllegalStateException("The desktop screen right is not configured.");
        rights.require(user, screen, action);
        return screen;
    }

    private UserAccount requireUser(String register) {
        if (!LOADING_DELIVERY.equals(register) && !TRANSACTION.equals(register)) {
            throw new IllegalArgumentException("Unknown Commission Trading register.");
        }
        UserAccount user = context.requireAccountingUser();
        Integer screen = screenId(register);
        if (screen == null) throw new IllegalStateException("The desktop screen definition was not found.");
        rights.require(user, screen, "View");
        return user;
    }

    private boolean allowed(UserAccount user, String register, String action) {
        try {
            Integer screen = screenId(register);
            if (screen == null) return false;
            rights.require(user, screen, action);
            return true;
        } catch (AccessDeniedException ex) {
            return false;
        }
    }

    private Integer screenId(String register) {
        return repository.screenId(LOADING_DELIVERY.equals(register)
                ? "CommisisionDeleiveryLoadingRegister" : "CommissionTransactionRegister");
    }

    private LinkedHashMap<String, Object> tenancy(UserAccount user) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", user.getOrganizationId());
        p.put("CompanyId", user.getCompanyId());
        p.put("BranchesId", user.getBranchesId());
        p.put("FinancialYearId", context.currentFinancialYearId());
        return p;
    }

    private static Date date(String value, String label) {
        if (value == null || value.isBlank()) throw new IllegalArgumentException(label + " is required.");
        try { return Date.valueOf(value); }
        catch (IllegalArgumentException ex) { throw new IllegalArgumentException(label + " is invalid."); }
    }

    private static void optionalInt(LinkedHashMap<String, Object> p, String name,
                                    Map<String, String> filters, String key) {
        String raw = filters.get(key);
        if (raw == null || raw.isBlank()) return;
        try {
            int value = Integer.parseInt(raw);
            if (value != 0) p.put(name, value);
        } catch (NumberFormatException ex) {
            throw new IllegalArgumentException("Invalid " + key + " filter.");
        }
    }
}
