package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.StockConversionDto;
import com.mst.repositories.StockConversionRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Stock Conversion — invfrmStockConversionProduction.cs, DocTypeId 66.
 *
 * Read side complete; Save refuses. See {@link StockConversionRepository#save} for exactly what is
 * missing and why a partial write would be worse than none.
 *
 * Tenancy, financial year, CanViewAllRecord and EntryUser are all server-derived. Nothing that
 * decides which company's documents are returned comes from the request.
 */
@Service
public class StockConversionService {

    private static final Logger LOG = LoggerFactory.getLogger(StockConversionService.class);

    /** The desktop form name, for the per-screen grant lookup. */
    private static final String SCREEN_NAME = "invfrmStockConversionProduction";
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";
    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
          + "@CompanyId=?, @Activity=?";

    @Autowired private StockConversionRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;

    // ==================================================================================== read

    /** The header with its three child grids, the way DAL 0275 GetData assembles them. */
    public Map<String, Object> load(int id) {
        currentUserContext.requireAccountingUser();
        Map<String, Object> head = repo.header(id);
        if (head == null) return null;

        /* The detail activity depends on the header's OWN DocTypeId, not on this screen's - a
           header read by id carries its type, and 67 reads the trading variant. */
        int docTypeId = asInt(head.get("DocTypeId"));
        if (docTypeId == 0) docTypeId = StockConversionRepository.DOC_TYPE_ID;

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("header", head);
        res.put("details", repo.details(id, docTypeId));
        res.put("packings", repo.packings(id));
        res.put("expenses", repo.expenses(id));
        return res;
    }

    public int nextCode() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.nextCode(u, currentUserContext.currentFinancialYearId());
    }

    public int idByDocNo(int docSrNo) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.idByDocNo(u, docSrNo, currentUserContext.currentFinancialYearId());
    }

    /** Every filter is optional, and an unset one is OMITTED rather than sent as null. */
    public Map<String, Object> history(String fromDate, String toDate,
                                       String entryFromDate, String entryToDate,
                                       String modifyFromDate, String modifyToDate,
                                       String approvedDateFrom, String approvedDateTo,
                                       Integer docNoFrom, Integer docNoTo) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean canViewAll = canViewAllRecords(u);
        List<Map<String, Object>> rows = repo.history(
                u, currentUserContext.currentFinancialYearId(), canViewAll, u.getId(),
                fromDate, toDate, entryFromDate, entryToDate, modifyFromDate, modifyToDate,
                approvedDateFrom, approvedDateTo, docNoFrom, docNoTo);
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("canViewAllRecords", canViewAll);
        res.put("rows", rows);
        return res;
    }

    public List<Map<String, Object>> stockFilter(String activity, Integer itemCategoryId,
                                                 String docDateTo, Integer warehouseId,
                                                 String cropYear, Integer itemId, Integer jobLotId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (activity == null || activity.trim().isEmpty()) {
            throw new IllegalArgumentException("Select what to filter");
        }
        return repo.stockFilter(u, activity.trim(), itemCategoryId, docDateTo,
                                warehouseId, cropYear, itemId, jobLotId);
    }

    public List<Map<String, Object>> availableStock(Integer itemCategoryId, String dateTo,
                                                    Integer warehouseId, Integer itemId,
                                                    Integer jobLotId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.availableStock(u, itemCategoryId, dateTo, warehouseId, itemId, jobLotId);
    }

    public List<Map<String, Object>> storeAndPmItems() {
        return repo.storeAndPmItems(currentUserContext.requireAccountingUser());
    }

    /** cmbEntryType — literal in the desktop form (:840), so literal here too. */
    public List<Map<String, Object>> entryTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        out.add(row(1, "Issue"));
        out.add(row(2, "Recovery By Product"));
        out.add(row(3, "Recovery Head Rice"));
        return out;
    }

    private static Map<String, Object> row(int id, String name) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", id);
        m.put("EntryType", name);
        return m;
    }

    // =================================================================================== write

    /**
     * Refuses. The desktop's Save is a posting engine — see
     * {@link StockConversionRepository#save}. Wired now so the screen and its contract exist, and
     * so that turning it on later is a change in one place rather than a new code path.
     */
    public Map<String, Object> save(StockConversionDto dto) {
        currentUserContext.requireAccountingUser();
        return repo.save(dto) > 0 ? null : null;   // repo.save always throws
    }

    // ========================================================================== authorization

    /**
     * CommonServices.SetRightsValueInRightsObject, the same way the rest of this port reads it:
     * Admin/Administrator short-circuits, otherwise the "CanView AllRecord" ROW of
     * Sp_tblUserRights_GetAllMethod for THIS screen. @RightName is the user's ROLE, which picks
     * the procedure's Admin branch; the right being asked about is not a parameter.
     */
    private boolean canViewAllRecords(UserAccount u) {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS, u.getId(), SCREEN_NAME, role, u.getCompanyId(), "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null
                        && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    return toBool(ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read CanView AllRecord for {}; restricting to own records",
                     SCREEN_NAME, e);
        }
        return false;
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }
}
