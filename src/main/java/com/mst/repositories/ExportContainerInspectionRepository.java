package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 205 "Container Inspection" - Architecture.WinApp.Export.GatePassInspection ("Vehicle Cargo Inspection Report").
 * Data layer: Architecture.BLL.ExImVCITransaction (BLL 0038 / DAL 0037, models 0050 + 0051), ExImVCIParameter
 * (BLL 0037), GatePassOutward (BLL 0568). Procedures:
 *
 *   Sp_GatePassOutward_GetAllMethod 'AllGatePassNOForGPInspection'  @OrganizationId @CompanyId @DocumentTypeId=91 @FinancialYearId [@Id] @Activity
 *   Sp_GatePassOutward_GetAllMethod 'ReadById'                      @Id @Activity                       (Container / Container1 of the gate pass)
 *   USP_ExImVCIParameter_GetAllMethod 'GetAll'                      @OrganizationId @CompanyId @Activity (VCIParameterId, ExImVCICartegoryId, VciCategoryDescription, VciParameterDescription)
 *   [dbo].[USP_ExImVCITransaction_GetAllMethod] 'ReadByGatePassId'  @OrganizationId @CompanyId @Id @Activity
 *   [dbo].[USP_ExImVCITransaction_GetAllMethod] 'ReadById' / 'ReadDetailByHeaderId'   @Id @Activity
 *   USP_ExImVCITransaction_GetAllMethod 'GatePassInformationForHistory'  @OrganizationId @CompanyId [@FromDate @ToDate @DocNoFrom @DocNoTo] @Activity
 *   USP_ExImVCITransaction_Insert / _Update                         the 22 non-virtual model properties (SetProc)
 *   USP_ExImVCITransactionDetail_Insert                             6 detail properties, one call per grid row (SetProc)
 */
@Repository
public class ExportContainerInspectionRepository {

    private final JdbcTemplate jdbc;

    public ExportContainerInspectionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> gatePasses(UserAccount u, int financialYearId, int id) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", 91, "FinancialYearId", financialYearId);
        if (id != 0) p.put("Id", id);
        p.put("Activity", "AllGatePassNOForGPInspection");
        return DesktopProc.rows(jdbc, "Sp_GatePassOutward_GetAllMethod", p);
    }

    public List<Map<String, Object>> gatePassById(int id) {
        return DesktopProc.rows(jdbc, "Sp_GatePassOutward_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> parameters(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_ExImVCIParameter_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    public List<Map<String, Object>> byGatePassId(UserAccount u, int gatePassId) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImVCITransaction_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", gatePassId, "Activity", "ReadByGatePassId"));
    }

    public List<Map<String, Object>> byId(int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImVCITransaction_GetAllMethod]", params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> detailsByHeaderId(int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImVCITransaction_GetAllMethod]", params("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    public List<Map<String, Object>> history(UserAccount u, java.sql.Date from, java.sql.Date to, int docNoFrom, int docNoTo) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (docNoFrom != 0) p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0) p.put("DocNoTo", docNoTo);
        p.put("Activity", "GatePassInformationForHistory");
        return DesktopProc.rows(jdbc, "USP_ExImVCITransaction_GetAllMethod", p);
    }

    /** DAL ExImVCITransaction.SetData: header SetProc (Insert when Id == 0), then one Detail_Insert per row; one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> details) {
        int id = header.get("Id") == null ? 0 : ((Number) header.get("Id")).intValue();
        Integer n = DesktopProc.scalar(jdbc, id == 0 ? "USP_ExImVCITransaction_Insert" : "USP_ExImVCITransaction_Update", header);
        int headerId = (n == null || n == 0) ? id : n;
        for (Map<String, Object> d : details) {
            d.put("ExImVCITransactionId", headerId);
            DesktopProc.scalar(jdbc, "USP_ExImVCITransactionDetail_Insert", d);
        }
        return headerId;
    }
}
