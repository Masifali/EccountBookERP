package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 285 "Receipe (Branding)" — Architecture.WinApp.Mfg.frmBillOfMaterial_Manufacturing
 * (DocumentTypeId 1120). BLL BomHeader → the [fed] schema procedures (procdure_index.csv):
 *
 * <pre>
 *   BomHeader.GenerateCode                [fed].[USP_BomHeader_GetAllMethod] 'GenerateCode'  (DocNo of row 0)       (:400)
 *   BomHeader.ReadById                    ... 'ReadById' + 'ReadByHeaderId_BomInputDetail'
 *                                         + 'ReadByHeaderId_BomByConsumptionDetail' + 'ReadByHeaderId_BomFohDetail'   (:1728)
 *   BomHeader.FormHistory                 ... 'FormHistory'                                                            (:2018)
 *   BomHeader.DeleteByID(user, id)        ... 'DeleteById' @Id @EntryUserId                                            (:1822)
 *   BomHeader.GetDataForDropDownFromBOM   [fed].[USP_GetDataForDropDownFromBomHeader] (Activity BOMName / ItemHeader) (:1896)
 *   BomHeader.Save                        [fed].[USP_BomHeader_Insert] | [USP_BomHeader_Update], then per row
 *                                         [fed].[USP_BomInputDetail_Insert], [fed].[USP_BomConsumptionDetail_Insert],
 *                                         [fed].[USP_BomFohDetail_Insert]                                               (:1640)
 * </pre>
 */
@Repository
public class Prod273RecipeBrandingRepository {

    private static final Logger LOG = LoggerFactory.getLogger(Prod273RecipeBrandingRepository.class);

    public static final String P_GET = "[fed].[USP_BomHeader_GetAllMethod]";

    private final JdbcTemplate jdbc;
    public Prod273RecipeBrandingRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }

    private List<Map<String, Object>> safe(String label, String proc, Map<String, Object> p) {
        try {
            return DesktopProc.rows(jdbc, proc, p);
        } catch (RuntimeException e) {
            LOG.warn("Screen 285 lookup '{}' ({}) failed", label, proc, e);
            return new ArrayList<>();
        }
    }

    /** DocumentNoFill:400 — GenerateCode(Org, Comp, Branch, 1120, ActiveYr) → DocNo. */
    public int generateCode(UserAccount u, int branchId, int documentTypeId, int financialYearId) {
        List<Map<String, Object>> r = rows(P_GET, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branchId,
                "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        Object v = r.get(0).get("DocNo");
        if (v == null) v = r.get(0).values().iterator().next();
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = rows(P_GET, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    public List<Map<String, Object>> inputRows(int id) { return rows(P_GET, params("Id", id, "Activity", "ReadByHeaderId_BomInputDetail")); }
    public List<Map<String, Object>> consumptionRows(int id) { return rows(P_GET, params("Id", id, "Activity", "ReadByHeaderId_BomByConsumptionDetail")); }
    public List<Map<String, Object>> fohRows(int id) { return rows(P_GET, params("Id", id, "Activity", "ReadByHeaderId_BomFohDetail")); }

    /** AllCombobind:1896 — one result set, split on the Activity column by the service. */
    public List<Map<String, Object>> dropDowns(UserAccount u) {
        return safe("dropDowns", "[fed].[USP_GetDataForDropDownFromBomHeader]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** HistoryGridFill:1998 — every ReportsParameters value the form sets. */
    public List<Map<String, Object>> formHistory(UserAccount u, int branchId, int financialYearId, int documentTypeId,
                                                 int fromDocNo, int toDocNo, Timestamp from, Timestamp to, int bomId, int itemId,
                                                 boolean canViewAll, int entryUserId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branchId, "FinancialYearId", financialYearId, "DocumentTypeId", documentTypeId);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (bomId != 0) p.put("BOMNo", bomId);
        if (itemId != 0) p.put("ItemId", itemId);
        p.put("CanViewAllRecord", canViewAll);
        p.put("EntryUserId", entryUserId);
        p.put("Activity", "FormHistory");
        return rows(P_GET, p);
    }

    /** btnDelete_Click:1822 — DeleteByID(UserAccount.ID, RecId). */
    public void deleteById(int userId, int id) {
        rows(P_GET, params("Id", id, "EntryUserId", userId, "Activity", "DeleteById"));
    }

    public int saveHeader(boolean insert, Map<String, Object> head) {
        return DesktopProc.setProc(jdbc, insert ? "[fed].[USP_BomHeader_Insert]" : "[fed].[USP_BomHeader_Update]", head);
    }
    public void insertInput(Map<String, Object> p)       { DesktopProc.setProc(jdbc, "[fed].[USP_BomInputDetail_Insert]", p); }
    public void insertConsumption(Map<String, Object> p) { DesktopProc.setProc(jdbc, "[fed].[USP_BomConsumptionDetail_Insert]", p); }
    public void insertFoh(Map<String, Object> p)         { DesktopProc.setProc(jdbc, "[fed].[USP_BomFohDetail_Insert]", p); }
}
