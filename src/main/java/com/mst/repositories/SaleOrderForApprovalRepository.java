package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.SaleOrderForApprovalService.Filter;
import java.sql.Date;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** frmSaleOrderForApproval: original SaleOrderHistory and UpdateStatusandIsApprovedbyOrderId calls. */
@Repository
public class SaleOrderForApprovalRepository {
    private final JdbcTemplate jdbc;
    public SaleOrderForApprovalRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public boolean canApprove(UserAccount u, int type) {
        String right = switch (type) { case 81 -> "SaleOrder"; case 90 -> "frmSaleOrderStoreAndPm"; case 127 -> "BookingOrder"; default -> ""; };
        // Same grant that makes this card visible in Sp_Inventory_ReadDashboardPendingForApproval.
        Integer count = jdbc.queryForObject("SELECT COUNT(*) FROM dbo.tblUserRights r "
                + "JOIN dbo.ScreenDefinition s ON s.Id=r.ScreenId JOIN dbo.ScreenRights sr ON sr.Id=r.RightId "
                + "WHERE r.CompanyId=? AND r.UserId=? AND s.ScreenName='PendingVouchersForApproval' "
                + "AND sr.RightName=? AND r.Value=1", Integer.class, u.getCompanyId(), u.getId(), right);
        return count != null && count > 0;
    }

    public boolean branchAllowed(UserAccount u, int branch) {
        Integer count = jdbc.queryForObject("SELECT COUNT(*) FROM dbo.BranchesAllocationToUser a "
                + "JOIN dbo.Branches b ON b.Id=a.BranchId AND b.CompanyId=a.CompanyId "
                + "WHERE a.OrganizationId=? AND a.CompanyId=? AND a.UserId=? AND a.BranchId=?",
                Integer.class, u.getOrganizationId(), u.getCompanyId(), u.getId(), branch);
        return count != null && count > 0;
    }

    public List<Map<String,Object>> combos(UserAccount u, int type) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromSaleOrder", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AppId", Objects.requireNonNullElse(u.getAppId(), 0), "UserId", u.getId(), "DocumentTypeIds", String.valueOf(type)));
    }

    public List<Map<String,Object>> history(UserAccount u, Filter f, int branch) {
        var p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AppId", Objects.requireNonNullElse(u.getAppId(), 0), "CustomerUserId", u.getId(),
                "DocumentTypeId", f.documentTypeId(), "Status", f.status());
        if (f.fromDate() != null) p.put("FromDate", Date.valueOf(f.fromDate()));
        p.put("ToDate", Date.valueOf(f.toDate()));
        if (f.customerId() > 0) p.put("SupplierCustomerId", f.customerId());
        if (f.itemId() > 0) p.put("ItemId", f.itemId());
        if (!"All".equals(f.approved())) p.put("IsApproved", "Approved".equals(f.approved()));
        if (branch > 0) p.put("BranchesIds", String.valueOf(branch));
        return DesktopProc.rows(jdbc, "Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt", p);
    }

    public List<Map<String,Object>> lockOrder(UserAccount u, int type, int id) {
        return jdbc.queryForList("SELECT Id,BranchesId,ISNULL(IsAproved,0) AS IsApproved,OrderStatus "
                + "FROM dbo.SaleOrder WITH (UPDLOCK,HOLDLOCK) WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=?",
                id, u.getOrganizationId(), u.getCompanyId(), type);
    }

    public void update(UserAccount u, int id, String action) {
        DesktopProc.rows(jdbc, "Sp_SaleOrder_GetAllMethod", DesktopProc.params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "PostUser", u.getId(), "Id", id, "ReqType", action,
                "Activity", "UpdateStatusandIsApprovedbyOrderId"));
    }
}
