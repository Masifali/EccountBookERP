package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import static org.springframework.http.HttpStatus.NOT_FOUND;

/** Bounds direct record URLs and child writes to the same company and visibility as History. */
@Repository
public class PurchaseOrderRecordRepository {
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    public PurchaseOrderRecordRepository(JdbcTemplate jdbc, CurrentUserContext context) {
        this.jdbc = jdbc; this.context = context;
    }

    public Map<String, Object> require(int id) {
        var rows = jdbc.queryForList("SELECT Id,DocNo,OrderSupCustId,IsAproved,BranchesId,EntryUser,"
                + "AttachmentsValues,CustomAttachmentsValues FROM dbo.PurchaseOrder "
                + "WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=41",
                id, context.currentOrganizationId(), context.currentCompanyId());
        if (rows.isEmpty()) throw missing();
        var row = rows.get(0);
        if (number(row.get("EntryUser")) != context.currentUserId() && !canViewAll()) throw missing();
        var config = jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod "
                + "@OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity=?",
                context.currentOrganizationId(), context.currentCompanyId(), "PurchaseOrderBranchWise",
                "GetConfigurationByOrgCompandConfigDescription");
        if (!config.isEmpty() && truth(config.get(0).get("ConfigKey"))) {
            if (number(row.get("BranchesId")) != context.currentBranchId()) throw missing();
        } else {
            var branches = jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUserFromPurchaseOrder "
                    + "@OrganizationId=?,@CompanyId=?,@UserId=?,@DocumentTypeId=?",
                    context.currentOrganizationId(), context.currentCompanyId(), context.currentUserId(), 41);
            if (branches.stream().noneMatch(branch -> number(branch.get("BranchId")) == number(row.get("BranchesId")))) throw missing();
        }
        return row;
    }

    public boolean canViewAll() {
        String role = context.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName=?,"
                + "@RightName=?,@CompanyId=?,@Activity=?", context.currentUserId(), "PurchsaeOrder",
                role == null ? "" : role, context.currentCompanyId(), "GetByUserId").stream()
                .anyMatch(r -> "CanView AllRecord".equalsIgnoreCase(Objects.toString(r.get("RightName"), "").trim())
                        && truth(r.get("Value")));
    }

    private static boolean truth(Object v) { return Boolean.TRUE.equals(v) || "1".equals(String.valueOf(v)) || "true".equalsIgnoreCase(String.valueOf(v)) || "yes".equalsIgnoreCase(String.valueOf(v)); }
    private static int number(Object v) { return v instanceof Number ? ((Number)v).intValue() : 0; }
    private static ResponseStatusException missing() { return new ResponseStatusException(NOT_FOUND, "Purchase Order not found in your accessible records"); }
}
