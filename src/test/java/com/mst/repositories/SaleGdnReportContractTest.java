package com.mst.repositories;

import com.mst.models.*;
import com.mst.reports.ReportRegistry;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class SaleGdnReportContractTest {
    @Test void originalBranchAndLookupSourcesUseSessionOrganizationCompanyAndUser(){
        var db=new SaleGdnReadContractTest.Db();var repo=new SaleGdnReportRepository(db);var u=new SaleGdnReadContractTest().user();
        repo.branches(u);assertTrue(db.sql.get(0).contains("USP_GetBranchsAllocatedToUserFromGdn"));assertArrayEquals(new Object[]{78,78,42},db.args.get(0));
        repo.lookups(u,"");assertFalse(db.sql.get(1).contains("@BranchesIds"));
        repo.lookups(u,",58");assertTrue(db.sql.get(2).endsWith("@BranchesIds=?"));assertArrayEquals(new Object[]{78,78,",58"},db.args.get(2));
    }
    @Test void historyUsesGdnProcedureAndOmitsUnsetNumericFilters(){
        var db=new SaleGdnReadContractTest.Db();var repo=new SaleGdnReportRepository(db);var u=new SaleGdnReadContractTest().user();
        var date=LocalDate.of(2026,9,17);
        var filter=new SaleGdnReportFilter(date,date,List.of(58),0,0,0,0,17,3,0,0,0,0,45,0,0);
        repo.history(u,filter,",58");
        assertEquals("EXEC dbo.Sp_InvGdn_History @OrganizationId=?,@CompanyId=?,@GrnDateF=?,@GrnDateT=?,@SupplierCustomerId=?,@InventoryParentCategoriesIds=?,@CropYear=?,@BranchesIds=?",db.sql.get(0));
        assertArrayEquals(new Object[]{78,78,"2026-09-17","2026-09-17",17,3,45,",58"},db.args.get(0));
    }
    @Test void originalTemplatesAndProcedureAreRegistered(){
        var registry=new ReportRegistry();
        assertEquals("260-InvRptGdnRiceSlip.rpt",registry.get("gdn-260").template);
        assertEquals("260A-DeliveryChallansByGDN.rpt",registry.get("gdn-260a").template);
        assertEquals("Sp_InvGdn_SlipAndRegisterRice_Rpt",registry.get("gdn-260").procedure);
        assertEquals("261-InvRptGdnRegister.rpt",registry.get("gdn-261").template);
        assertEquals("Sp_InvGdn_Register_Rpt",registry.get("gdn-261").procedure);
    }
}
