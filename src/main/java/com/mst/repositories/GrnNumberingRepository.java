package com.mst.repositories;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Architecture.BLL.Inventory.InvGrn.GenerateInvGrnCode (desktop BLL 0576). */
@Repository
public class GrnNumberingRepository {
    private final JdbcTemplate jdbc;
    public GrnNumberingRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }

    public int next(int organization,int company,int branch,int year,int documentType) {
        var row=jdbc.queryForMap("EXEC dbo.Sp_InvGrn_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?,@Activity='GenerateInvGrnCode'",
                organization,company,documentType,year,branch);
        if (!(row.get("DocNo") instanceof Number)) throw new IllegalStateException("GRN numbering did not return DocNo");
        return ((Number)row.get("DocNo")).intValue();
    }
}
