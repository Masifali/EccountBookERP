package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import static org.springframework.http.HttpStatus.NOT_FOUND;

/** Direct URLs use the same scope as InwardGatePass.gridhistory(). */
@Repository
public class InwardGatePassRecordRepository {
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    public InwardGatePassRecordRepository(JdbcTemplate jdbc, CurrentUserContext context) {
        this.jdbc = jdbc; this.context = context;
    }

    public Map<String,Object> require(int id) {
        var rows = jdbc.queryForList("SELECT * FROM dbo.GatePassInward WHERE Id=? AND OrganizationId=? "
                + "AND CompanyId=? AND DocumentTypeId=51 AND BranchesId=? AND FinancialYearId=?",
                id,context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId());
        if (rows.isEmpty()) throw missing();
        var row=rows.get(0);
        if (number(row.get("EntryUser"))!=context.currentUserId() && !canViewAll()) throw missing();
        return row;
    }

    public boolean canViewAll() {
        String role=context.currentRoleName();
        if ("Admin".equalsIgnoreCase(role)||"Administrator".equalsIgnoreCase(role)) return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName=?,"
                + "@RightName=?,@CompanyId=?,@Activity=?",context.currentUserId(),"InwardGatePass",
                role==null?"":role,context.currentCompanyId(),"GetByUserId").stream()
                .anyMatch(r->"CanView AllRecord".equalsIgnoreCase(Objects.toString(r.get("RightName"),"").trim())
                        && (Boolean.TRUE.equals(r.get("Value"))||"1".equals(Objects.toString(r.get("Value"),""))));
    }
    private static int number(Object v) { return v instanceof Number?((Number)v).intValue():0; }
    private static ResponseStatusException missing() { return new ResponseStatusException(NOT_FOUND,"Inward Gate Pass not found in your accessible records"); }
}
