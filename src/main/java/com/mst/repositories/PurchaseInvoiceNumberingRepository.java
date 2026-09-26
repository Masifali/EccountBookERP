package com.mst.repositories;

import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** BLL Inventory.InvPurchaseInvoice.GenerateInvPurchaseInvoiceCode / CodeBranch. */
@Repository
public class PurchaseInvoiceNumberingRepository {
    private final JdbcTemplate jdbc;
    public PurchaseInvoiceNumberingRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    private void checkType(int type) {
        if (!Set.of(56,57,59,61,138).contains(type)) throw new IllegalArgumentException("Unsupported Purchase Invoice document type");
    }
    public int next(int org, int company, int year, int type) {
        checkType(type);
        var rows=jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@Activity=?",
                org,company,type,year,"GenerateCode");
        return rows.isEmpty() ? 0 : ((Number)rows.get(0).get("DocNo")).intValue();
    }
    public int nextBranch(int org, int company, int year, int branch, int type) {
        checkType(type);
        var rows=jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?,@Activity=?",
                org,company,type,year,branch,"GenerateBranchSrNo");
        return rows.isEmpty() ? 0 : ((Number)rows.get(0).get("BranchSrNo")).intValue();
    }
}
