package com.mst.repositories;

import com.mst.models.SaleGatePassTimeFilter;
import com.mst.models.UserAccount;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Native BLL GatePassOutwardReports (0124:212/304) and GatePassOutward (0568:1085/1801). */
@Repository
public class SaleGatePassTimeRepository {
    public static final String SCREEN="frmGatePassVehicleEntryAndExitTimeAnalysisReport";
    private final JdbcTemplate jdbc;
    public SaleGatePassTimeRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}

    public int screenId(){
        var rows=jdbc.queryForList("SELECT Id FROM dbo.ScreenDefinition WHERE ScreenName=? OR ScreenAlias=?",SCREEN,SCREEN);
        if(rows.size()!=1)throw new IllegalStateException("Gate pass time analysis screen definition was not found uniquely");
        return ((Number)rows.get(0).get("Id")).intValue();
    }
    public List<Map<String,Object>> branches(UserAccount u){
        return jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUserFromGPInwardAndOutward @OrganizationId=?,@CompanyId=?,@UserId=?",
                u.getOrganizationId(),u.getCompanyId(),u.getId());
    }
    public List<Map<String,Object>> lookups(UserAccount u,String branches){
        // Bind by SQL parameter name. The recovered form reverses the BLL's two positional argument names.
        // The Java tenant context always supplies its actual organization/company to their SQL predicates.
        return jdbc.queryForList("EXEC dbo.USP_GetDataForDropDownFromGPoutwardAndInward @OrganizationId=?,@CompanyId=?,@BranchesIds=?",
                u.getOrganizationId(),u.getCompanyId(),branches);
    }
    public String yearStart(UserAccount u,int year){
        return jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?,@CompanyId=?",
                u.getOrganizationId(),u.getCompanyId()).stream().filter(r->SaleInvoiceRepository.i(r.get("Id"))==year)
                .map(r->Objects.toString(r.get("Start_Period"),"")).filter(s->s.length()>=10).map(s->s.substring(0,10)).findFirst().orElse("");
    }
    public List<Map<String,Object>> analysis(UserAccount u,int year,SaleGatePassTimeFilter f,String branches){
        return report("USP_GatePass_VehicleEntryAndExitTime_AnalysisReport",u,year,f,branches);
    }
    public List<Map<String,Object>> summary(UserAccount u,int year,SaleGatePassTimeFilter f,String branches){
        return report("USP_GatePass_VehicleEntryAndExitTime_SummaryReport",u,year,f,branches);
    }
    private List<Map<String,Object>> report(String procedure,UserAccount u,int year,SaleGatePassTimeFilter f,String branches){
        StringBuilder sql=new StringBuilder("EXEC dbo."+procedure+" @OrganizationId=?,@CompanyId=?,@FinancialYearId=?");
        List<Object> values=new ArrayList<>(Arrays.asList(u.getOrganizationId(),u.getCompanyId(),year));
        add(sql,values,"SupplierCustomerId",f.customerId());add(sql,values,"DocumentTypeId",f.documentTypeId());
        if(f.fromDate()!=null){sql.append(",@FromDate=?");values.add(f.fromDate().toString());}
        if(f.toDate()!=null){sql.append(",@ToDate=?");values.add(f.toDate().toString());}
        add(sql,values,"FromDocNo",f.fromNo());add(sql,values,"ToDocNo",f.toNo());
        if(f.status()!=null&&!f.status().isEmpty()){sql.append(",@Status=?");values.add(f.status());}
        sql.append(",@BranchesIds=?");values.add(branches);
        return jdbc.queryForList(sql.toString(),values.toArray());
    }
    private static void add(StringBuilder sql,List<Object> values,String key,int value){
        if(value!=0){sql.append(",@").append(key).append("=?");values.add(value);}
    }
}
