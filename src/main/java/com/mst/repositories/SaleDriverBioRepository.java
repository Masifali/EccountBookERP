package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.SaleDriverBioRequest;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

@Repository
public class SaleDriverBioRepository {
    public static final int DOCUMENT_TYPE_ID = 93;
    public static final int REF_DOCUMENT_TYPE_ID = 91;
    private final JdbcTemplate jdbc;

    public SaleDriverBioRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String,Object>> pending(UserAccount u, int year) {
        return jdbc.queryForList("EXEC dbo.USP_GatePass_PendingForDriverInfo @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?",
                u.getOrganizationId(), u.getCompanyId(), REF_DOCUMENT_TYPE_ID, year, u.getBranchesId());
    }

    public List<Map<String,Object>> knownDrivers(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.USP_driverBiodata_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",
                u.getOrganizationId(), u.getCompanyId());
    }

    public List<Map<String,Object>> history(UserAccount u, int year, boolean canViewAll) {
        return jdbc.queryForList("EXEC dbo.USP_GatePassOutwardDriverInfo_FormHistory @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=?,@EntryUser=?,@CanViewAllRecord=?",
                u.getOrganizationId(), u.getCompanyId(), u.getBranchesId(), year, DOCUMENT_TYPE_ID, u.getId(), canViewAll);
    }

    public Map<String,Object> record(UserAccount u, int id) {
        List<Map<String,Object>> rows = jdbc.queryForList("EXEC dbo.Sp_GatePassOutwardDriverInfo_GetAllMethod @Id=?,@Activity='ReadById'", id);
        if (rows.size()!=1 || number(rows.get(0).get("OrganizationId"))!=u.getOrganizationId() || number(rows.get(0).get("CompanyId"))!=u.getCompanyId())
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Driver information not found in this company");
        Map<String,Object> result=new LinkedHashMap<>(rows.get(0));
        result.putAll(jdbc.queryForMap("SELECT DocumentTypeId,EntryDate,EntryUser,ModifyDate,ModifyUser,DriverImage,FingerPrintImage FROM dbo.GatePassOutwardDriverInfo WHERE Id=?",id));
        return result;
    }

    public Map<String,Object> save(UserAccount u, SaleDriverBioRequest r) {
        validate(u, r);
        Map<String,Object> old = r.id>0 ? record(u,r.id) : null;
        Map<String,Object> gp = gatePass(u, r.gatePassOutwardId);
        checkDateLock(u, date(gp.get("GpDate")));
        Timestamp now=Timestamp.valueOf(LocalDateTime.now());
        String proc=r.id>0?"Sp_GatePassOutwardDriverInfo_Update":"Sp_GatePassOutwardDriverInfo_Insert";
        String sql="EXEC dbo."+proc+" @Id=?,@DocumentTypeId=?,@GatePassOutwardId=?,@ForwarderName=?,@DriverName=?,@FatherName=?,@CnicNo=?,@DriverCellNo=?,@AlternateCellNo=?,@RemarksHeader=?,@DriverPic=?,@ThumbPic=?,@FingerPrintImage=?,@DriverImage=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,@ScreenName=?,@RefDocumentTypeId=?";
        int id=execute(sql,new Object[]{r.id,DOCUMENT_TYPE_ID,r.gatePassOutwardId,text(r.forwarderName),text(r.driverName),text(r.fatherName),text(r.cnicNo),text(r.driverCellNo),text(r.alternateCellNo),text(r.remarksHeader),
                keep(r.driverPic,old,"DriverPic"),keep(r.thumbPic,old,"ThumbPic"),keepBytes(r.fingerPrintImage,old,"FingerPrintImage"),keepBytes(r.driverImage,old,"DriverImage"),
                old==null?now:old.get("EntryDate"),old==null?u.getId():old.get("EntryUser"),now,u.getId(),u.getOrganizationId(),u.getCompanyId(),u.getBranchesId(),"frmDriverBio",REF_DOCUMENT_TYPE_ID},r.id);
        if(id<=0) throw new IllegalStateException("Driver information save returned no record ID");
        return record(u,id);
    }

    private Map<String,Object> gatePass(UserAccount u,int id) {
        List<Map<String,Object>> rows=jdbc.queryForList("SELECT TOP 1 Id,GpDate FROM dbo.GatePassOutward WHERE Id=? AND OrganizationId=? AND CompanyId=? AND BranchesId=? AND DocumentTypeId=?",
                id,u.getOrganizationId(),u.getCompanyId(),u.getBranchesId(),REF_DOCUMENT_TYPE_ID);
        if(rows.isEmpty()) throw new IllegalArgumentException("Select a valid pending outward gate pass");
        return rows.get(0);
    }

    private void checkDateLock(UserAccount u, LocalDate gpDate) {
        List<Map<String,Object>> locks=jdbc.queryForList("EXEC dbo.Sp_DateLock_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationCompanyIdandDate'",u.getOrganizationId(),u.getCompanyId());
        if(!locks.isEmpty() && locks.get(0).get("Date")!=null) {
            LocalDate locked=date(locks.get(0).get("Date"));
            if(!gpDate.isAfter(locked)) throw new IllegalArgumentException("Not Insert or Update record please check lock date");
        }
    }

    private void validate(UserAccount u,SaleDriverBioRequest r) {
        if(r.id>0) record(u,r.id);
        if(r.gatePassOutwardId<=0) throw new IllegalArgumentException("Gate pass is required");
        if(!text(r.cnicNo).matches("\\d{5}-\\d{7}-\\d")) throw new IllegalArgumentException("CNIC Field Required (00000-0000000-0)");
        if(!text(r.driverCellNo).matches("0092-3\\d{2}-\\d{7}")) throw new IllegalArgumentException("Cell No. Field Required (0092-300-0000000)");
        if(text(r.driverName).isEmpty()) throw new IllegalArgumentException("Driver Name Field Required");
        if(text(r.fatherName).isEmpty()) throw new IllegalArgumentException("Father Name Field Required");
        if(text(r.forwarderName).isEmpty()) throw new IllegalArgumentException("Forwarder Name Field Required");
    }

    private int execute(String sql,Object[] args,int fallback){return jdbc.execute(sql,(PreparedStatementCallback<Integer>)s->{for(int i=0;i<args.length;i++){if(args[i]==null&&(i==12||i==13))s.setNull(i+1,java.sql.Types.VARBINARY);else s.setObject(i+1,args[i]);}int id=fallback;boolean captured=fallback>0,more=s.execute();while(true){if(more)try(var rs=s.getResultSet()){while(rs.next())if(!captured&&rs.getObject(1) instanceof Number n&&n.intValue()>0){id=n.intValue();captured=true;}}else if(s.getUpdateCount()==-1)break;more=s.getMoreResults();}return id;});}
    private static int number(Object v){return v==null?0:Integer.parseInt(v.toString());}
    private static LocalDate date(Object v){if(v instanceof java.sql.Date d)return d.toLocalDate();if(v instanceof java.sql.Timestamp t)return t.toLocalDateTime().toLocalDate();return LocalDate.parse(v.toString().substring(0,10));}
    private static String text(String v){return v==null?"":v.trim();}
    private static String keep(String value,Map<String,Object> old,String key){return !text(value).isEmpty()?text(value):old==null?"":text(Objects.toString(old.get(key),""));}
    private static byte[] keepBytes(byte[] value,Map<String,Object> old,String key){return value!=null&&value.length>0?value:old!=null&&old.get(key) instanceof byte[] b?b:null;}
}
