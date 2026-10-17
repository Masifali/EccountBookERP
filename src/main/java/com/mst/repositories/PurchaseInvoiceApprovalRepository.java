package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.PurchaseInvoiceApprovalService.Filter;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;
import javax.sql.DataSource;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** PendingApprovalPurchaseInvoice.cs, type 56: approval and undo use the same desktop grant. */
@Repository
public class PurchaseInvoiceApprovalRepository {
    private final JdbcTemplate jdbc;
    public PurchaseInvoiceApprovalRepository(DataSource dataSource) {
        jdbc = new JdbcTemplate(dataSource);
        jdbc.setQueryTimeout(15);
    }

    public boolean canApprove(UserAccount u) {
        Integer count = jdbc.queryForObject("SELECT COUNT(*) FROM dbo.tblUserRights r "
                + "JOIN dbo.ScreenDefinition s ON s.Id=r.ScreenId JOIN dbo.ScreenRights sr ON sr.Id=r.RightId "
                + "WHERE r.CompanyId=? AND r.UserId=? AND s.ScreenName='PendingVouchersForApproval' "
                + "AND sr.RightName='PurchaseInvoice' AND r.Value=1", Integer.class, u.getCompanyId(), u.getId());
        return count != null && count > 0;
    }

    public boolean branchAllowed(UserAccount u, int branch) {
        Integer count = jdbc.queryForObject("SELECT COUNT(*) FROM dbo.BranchesAllocationToUser a "
                + "JOIN dbo.Branches b ON b.Id=a.BranchId AND b.CompanyId=a.CompanyId "
                + "WHERE a.OrganizationId=? AND a.CompanyId=? AND a.UserId=? AND a.BranchId=?",
                Integer.class, u.getOrganizationId(), u.getCompanyId(), u.getId(), branch);
        return count != null && count > 0;
    }

    private Map<String,Object> tenant(UserAccount u) {
        return DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    public List<Map<String,Object>> combos(UserAccount u, int branch) {
        var p = tenant(u); p.put("DocumentTypeIds", "56");
        if (branch > 0) p.put("BranchesIds", String.valueOf(branch));
        return DesktopProc.rows(jdbc, "Usp_AllComboAgainstPurchaseInvoice", p);
    }

    public List<Map<String,Object>> pending(UserAccount u, Filter f, int branch) {
        var p = tenant(u); p.put("DocumentTypeId", 56); p.put("IsApproved", f.unApprove());
        if (f.fromDate() != null) p.put("FromDate", Date.valueOf(f.fromDate()));
        p.put("ToDate", Date.valueOf(f.toDate()));
        if (f.supplierId() > 0) p.put("SupplierCustomerId", f.supplierId());
        if (f.itemId() > 0) p.put("ItemId", f.itemId());
        if (branch > 0) p.put("BranchesIds", String.valueOf(branch));
        // @UserId filters DAW assignments instead of pending status; desktop intentionally omits it.
        return DesktopProc.rows(jdbc, "USP_PendingInvPurchaseInvoiceForApproval", p);
    }

    public List<Map<String,Object>> lockInvoice(UserAccount u, int id) {
        return jdbc.queryForList("SELECT Id,BranchesId,IsApproved FROM dbo.InvPurchaseInvoice WITH (UPDLOCK,HOLDLOCK) "
                + "WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=56",
                id, u.getOrganizationId(), u.getCompanyId());
    }

    public void approve(UserAccount u, int id) {
        changeApproval(u, id, "AP");
    }

    public void unapprove(UserAccount u, int id) {
        changeApproval(u, id, "UP");
    }

    private void changeApproval(UserAccount u, int id, String requestType) {
        var p = tenant(u);
        p.put("DocumentTypeId", 56); p.put("Id", id); p.put("EntryUser", u.getId());
        p.put("PostDate", Timestamp.valueOf(LocalDateTime.now())); p.put("ReqType", requestType);
        p.put("Activity", "InvPurchaseInvoiceandVoucherApprove");
        // The original procedure checks production dependencies, updates the linked voucher and
        // stock approval flags, and records the acting user in the desktop audit trail.
        DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod", p);
    }
}
