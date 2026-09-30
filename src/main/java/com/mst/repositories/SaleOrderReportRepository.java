package com.mst.repositories;

import com.mst.models.SaleOrderReportFilter;
import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.sql.Date;
import java.time.LocalDate;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

/** Native BLL 0596 SaleOrder:531,1512,1712,2544; Projects:126. Procedures remain unchanged. */
@Repository
public class SaleOrderReportRepository {
    private final JdbcTemplate jdbc;
    public SaleOrderReportRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public int screenId() {
        // Seeder has another frmSaleOrderHistory in module 2042. This URL belongs to Sales Reports (53).
        var rows = jdbc.queryForList("SELECT Id FROM dbo.ScreenDefinition WHERE ModuleId=53 AND (ScreenName=? OR ScreenAlias=?)",
                "frmSaleOrderHistory", "frmSaleOrderHistory");
        if (rows.size() != 1) throw new IllegalStateException("Sale Order Report screen definition was not found uniquely");
        return ((Number) rows.get(0).get("Id")).intValue();
    }

    public List<Map<String, Object>> branches(UserAccount u) {
        var p = tenant(u); p.put("UserId", u.getId()); p.put("DocumentTypeId", 81);
        var rows = DesktopProc.rows(jdbc, "USP_GetBranchsAllocatedToUserFromSaleOrder", p);
        // The recovered SQL has an unparenthesized OR before DocumentTypeId. Do not let that
        // expose another tenant's branches or treat them as allocations for the signed-in user.
        Set<Integer> allocated = jdbc.queryForList("SELECT DISTINCT a.BranchId FROM dbo.BranchesAllocationToUser a "
                + "INNER JOIN dbo.Branches b ON b.Id=a.BranchId AND b.CompanyId=a.CompanyId "
                + "INNER JOIN dbo.UserAccount u ON u.Id=a.UserId AND u.CompanyId=a.CompanyId "
                + "WHERE a.OrganizationId=? AND a.CompanyId=? AND a.UserId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId()).stream()
                .map(r -> ((Number) r.get("BranchId")).intValue()).collect(Collectors.toSet());
        return rows.stream().filter(r -> allocated.contains(((Number) r.get("BranchId")).intValue())).toList();
    }

    public List<Map<String, Object>> costCenters(UserAccount u) {
        var p = tenant(u); p.put("UserId", u.getId()); number(p, "AppId", u.getAppId() == null ? 0 : u.getAppId());
        return DesktopProc.rows(jdbc, "usp_getCostCenters", p);
    }

    public List<Map<String, Object>> lookups(UserAccount u, String branches, int costCenterId) {
        var p = tenant(u); p.put("AppId", Objects.requireNonNullElse(u.getAppId(), 0)); p.put("UserId", u.getId());
        p.put("DocumentTypeIds", "81"); p.put("BranchesIds", branches); number(p, "CostCenterId", costCenterId);
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromSaleOrder", p);
    }

    public List<Map<String, Object>> detail(UserAccount u, SaleOrderReportFilter f, String branches) {
        var p = common(u, f, branches);
        // This is CustomerUserId, NOT UserId (which activates the approval-queue path in SQL).
        p.put("CustomerUserId", u.getId());
        number(p, "FromDocNo", f.fromNo()); number(p, "ToDocNo", f.toNo());
        number(p, "SupplierCustomerId", f.customerId()); number(p, "ItemId", f.itemId());
        number(p, "InventoryParentCategoriesId", f.parentCategoryId()); text(p, "Status", f.status());
        // The first set is report data. Desktop's second set contains only the company logo for Crystal printing.
        return DesktopProc.rows(jdbc, "Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt", p);
    }

    public List<Map<String, Object>> summary(UserAccount u, SaleOrderReportFilter f, String branches) {
        var p = common(u, f, branches); p.put("UserId", u.getId());
        number(p, "DocNoFrom", f.fromNo()); number(p, "DocNoTo", f.toNo());
        number(p, "OrderSupCustId", f.customerId()); number(p, "OrderItemId", f.itemId());
        number(p, "InventoryParentCategories", f.parentCategoryId());
        number(p, "PackUom", f.packUom()); text(p, "CropYear", f.cropYear());
        number(p, "JobLotId", f.jobLotId()); number(p, "PackingTypeId", f.packingTypeId());
        number(p, "DistrictId", f.districtId()); text(p, "OrderStatus", f.status());
        p.put("ActivityName", f.activity()); if (f.skipZero()) p.put("SkipZero", 1);
        return DesktopProc.rows(jdbc, "USP_SaleOrderSummaryRegister", p);
    }

    /** Explicit desktop grants; missing definitions or allocations never enable a write action. */
    public Set<String> actionRights(UserAccount u) {
        int screen = screenId();
        return new LinkedHashSet<>(jdbc.queryForList("SELECT DISTINCT r.RightName FROM dbo.ScreenRights r "
                + "INNER JOIN dbo.tblUserRights g ON g.RightId=r.Id AND g.ScreenId=r.ScreenID "
                + "WHERE r.ScreenID=? AND g.CompanyId=? AND g.UserId=? AND g.Value=1 "
                + "AND EXISTS (SELECT 1 FROM dbo.CompanyRights c WHERE c.ScreenId=r.ScreenID "
                + "AND c.CompanyId=g.CompanyId AND c.IsActive=1)", String.class,
                screen, u.getCompanyId(), u.getId()));
    }

    /** frmSaleOrderHistory uses this configuration before invoking the auto-complete BLL. */
    public double completionTolerance(UserAccount u) {
        var p = tenant(u); p.put("ConfigDescription", "ToleranceForOrderCompletion");
        p.put("Activity", "GetConfigurationByOrgCompandConfigDescription");
        var rows = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", p);
        String value = rows.isEmpty() ? "" : Objects.toString(rows.get(0).get("ConfigKey"), "").trim();
        return value.isEmpty() ? 0 : Double.parseDouble(value);
    }

    /** BLL 0596:795-860: one procedure invocation per selection, without a batch transaction. */
    public void updateStatus(UserAccount u, int id, String requestType, String remarks, LocalDate expiryDate) {
        var p = tenant(u); p.put("Id", id); p.put("ReqType", requestType); p.put("PostUser", u.getId());
        p.put("OrderStatusRemarks", remarks);
        if (expiryDate != null) p.put("DocDate", Date.valueOf(expiryDate));
        p.put("Activity", "UpdateStatusandIsApprovedbyOrderId");
        DesktopProc.rows(jdbc, "Sp_SaleOrder_GetAllMethod", p);
    }

    /** BLL 0596:2644-2678: all auto-complete procedure calls share one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public void autoComplete(UserAccount u, List<Map<String, Object>> headers) {
        for (var header : headers) {
            var p = tenant(u); p.put("SaleOrderId", header.get("Id"));
            p.put("DocumentTypeId", header.get("DocumentTypeId")); p.put("NetWeight", 0.0);
            DesktopProc.rows(jdbc, "USP_SaleOrderAutoComplete", p);
        }
    }

    private static Map<String, Object> common(UserAccount u, SaleOrderReportFilter f, String branches) {
        var p = tenant(u); p.put("AppId", Objects.requireNonNullElse(u.getAppId(), 0)); p.put("BranchesIds", branches);
        if (f.fromDate() != null) p.put("FromDate", Date.valueOf(f.fromDate()));
        if (f.toDate() != null) p.put("ToDate", Date.valueOf(f.toDate()));
        number(p, "CostCenterId", f.costCenterId()); number(p, "ItemCategoryId", f.categoryId());
        number(p, "ItemTypeId", f.itemTypeId()); number(p, "CityId", f.cityId());
        number(p, "BookingPersonId", f.bookingPersonId()); number(p, "ReferencePartyId", f.referencePartyId());
        if (f.includeApprovedDo()) p.put("ActionId", 1);
        if (!"All".equals(f.approval())) p.put("IsApproved", "Approve".equals(f.approval()));
        // Neither of these BLL methods sends FinancialYearId or an unconditional DocumentTypeId.
        return p;
    }
    private static Map<String, Object> tenant(UserAccount u) {
        return DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }
    private static void number(Map<String, Object> p, String key, int value) { if (value != 0) p.put(key, value); }
    private static void text(Map<String, Object> p, String key, String value) { if (value != null && !value.isEmpty()) p.put(key, value); }
}
