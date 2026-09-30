package com.mst.repositories;

import com.mst.models.SaleOutwardReportFilter;
import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.sql.Date;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** frmGPOutward -> BLL 0124 GatePassOutwardReports / 0568 GatePassOutward -> original SQL. */
@Repository
public class SaleOutwardReportRepository {
    private final JdbcTemplate jdbc;
    public SaleOutwardReportRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }
    public int screenId() {
        var rows = jdbc.queryForList("SELECT Id FROM dbo.ScreenDefinition WHERE ScreenName=? OR ScreenAlias=?", "frmGPOutward", "frmGPOutward");
        if (rows.size() != 1) throw new IllegalStateException("Outward Gate Pass Report screen definition was not found uniquely");
        return ((Number) rows.get(0).get("Id")).intValue();
    }
    public List<Map<String, Object>> branches(UserAccount u) {
        var p = tenant(u); p.put("UserId", u.getId()); p.put("DocumentTypeId", 91);
        var rows = DesktopProc.rows(jdbc, "USP_GetBranchsAllocatedToUserFromGatePassOutward", p);
        // The original query's final OR can bypass its tenant predicates. Restrict the returned allocation list.
        Set<Integer> allocated = jdbc.queryForList("SELECT DISTINCT a.BranchId FROM dbo.BranchesAllocationToUser a "
                + "INNER JOIN dbo.Branches b ON b.Id=a.BranchId AND b.CompanyId=a.CompanyId "
                + "INNER JOIN dbo.UserAccount u ON u.Id=a.UserId AND u.CompanyId=a.CompanyId "
                + "WHERE a.OrganizationId=? AND a.CompanyId=? AND a.UserId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId()).stream()
                .map(r -> ((Number) r.get("BranchId")).intValue()).collect(Collectors.toSet());
        return rows.stream().filter(r -> allocated.contains(((Number) r.get("BranchId")).intValue())).toList();
    }
    public List<Map<String, Object>> lookups(UserAccount u, String branches) {
        // Native form reverses the BLL positional company/org arguments. Bind the actual session tenant by SQL name.
        var p = tenant(u); p.put("BranchesIds", branches);
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromOutWardGP", p);
    }
    public String yearStart(UserAccount u, int year) {
        return DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", tenant(u)).stream()
                .filter(r -> ((Number) r.get("Id")).intValue() == year)
                .map(r -> Objects.toString(r.get("Start_Period"), "")).filter(s -> s.length() >= 10).map(s -> s.substring(0, 10)).findFirst().orElse("");
    }
    public List<Map<String, Object>> rows(UserAccount u, SaleOutwardReportFilter f, String branches) {
        var p = tenant(u); p.put("BranchesIds", branches);
        p.put("FromDate", Date.valueOf(f.fromDate())); p.put("ToDate", Date.valueOf(f.toDate()));
        if (f.fromNo() != 0) p.put("FromDocNo", f.fromNo()); if (f.toNo() != 0) p.put("ToDocNo", f.toNo());
        if (f.customerId() != 0) p.put("SupplierCustomerId", f.customerId());
        if (f.gatePassType() != null && !f.gatePassType().isEmpty()) p.put("GatepassType", f.gatePassType());
        if (f.status() != null && !f.status().isEmpty()) p.put("Status", f.status());
        if (f.onlyPending()) p.put("OnlyPending", 1);
        // This report BLL has no FinancialYearId/AppId/UserId filter. Branch access is enforced by the service.
        var rows = DesktopProc.rows(jdbc, "Sp_GatePassOutward_SlipAndRegister_Rpt", p);
        if (!rows.isEmpty()) {
            // The report's final SELECT omits DocumentTypeId. Read link identity from the same
            // scoped headers; a displayed GP/DO number is never a record ID or a document type.
            String ids = rows.stream().map(r -> r.get("Id").toString()).distinct().collect(Collectors.joining(","));
            var identities = jdbc.queryForList("SELECT Id,DocumentTypeId FROM dbo.GatePassOutward "
                    + "WHERE OrganizationId=? AND CompanyId=? AND Id IN (SELECT TRY_CONVERT(int,Data) FROM dbo.fnSplitString(?,','))",
                    u.getOrganizationId(), u.getCompanyId(), ids);
            Map<Integer,Object> types = new HashMap<>();
            identities.forEach(r -> types.put(((Number)r.get("Id")).intValue(), r.get("DocumentTypeId")));
            for (var row : rows) {
                Object type = types.get(((Number)row.get("Id")).intValue());
                if (type == null) throw new IllegalStateException("Gate pass document identity was not found for this company");
                row.put("DocumentTypeId", type);
            }
        }
        return rows;
    }
    private static Map<String, Object> tenant(UserAccount u) {
        return DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }
}
