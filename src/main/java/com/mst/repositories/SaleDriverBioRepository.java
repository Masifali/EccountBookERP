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

    /* ------------------------------------------------------------------------------------------------------------
     * frmDriverBio opened with RefDocumentTypeId (51 = InwardGatePass.BtnDriverForm_Click / btnsave_Click). The methods
     * above keep serving /sale/driver-bio (91) unchanged; these take the reference type the way frmDriverBio.cs does.
     * ------------------------------------------------------------------------------------------------------------ */

    /** pendingGatepass(): GatePassOutwardDriverInfo.GatePass_PendingForDriverInfo - year/branch/type only when not 0. */
    public List<Map<String,Object>> pending(UserAccount u, int year, int refDocumentTypeId) {
        String sql="EXEC [dbo].[USP_GatePass_PendingForDriverInfo] @OrganizationId=?,@CompanyId=?";
        List<Object> args=new ArrayList<>(Arrays.asList(u.getOrganizationId(),u.getCompanyId()));
        if(year!=0){sql+=",@FinancialYearId=?";args.add(year);}
        if(u.getBranchesId()!=0){sql+=",@BranchesId=?";args.add(u.getBranchesId());}
        if(refDocumentTypeId!=0){sql+=",@DocumentTypeId=?";args.add(refDocumentTypeId);}
        return jdbc.queryForList(sql,args.toArray());
    }

    /** cmbGatePass_TextChanged: GatePassOutward.ReadByGpNoForExportDriverInformation (Sp_GatePassOutward_GetAllMethod). */
    public List<Map<String,Object>> gatePassByNo(UserAccount u, int year, int refDocumentTypeId, int gpSrNo) {
        String sql="EXEC dbo.Sp_GatePassOutward_GetAllMethod @OrganizationId=?,@CompanyId=?,@FinancialYearId=?";
        List<Object> args=new ArrayList<>(Arrays.asList(u.getOrganizationId(),u.getCompanyId(),year));
        if(u.getBranchesId()!=0){sql+=",@BranchesId=?";args.add(u.getBranchesId());}
        if(refDocumentTypeId!=0){sql+=",@DocumentTypeId=?";args.add(refDocumentTypeId);}
        sql+=",@GpSrNo=?,@Activity='ReadByGpNoForExportDriverInformation'"; args.add(gpSrNo);
        return jdbc.queryForList(sql,args.toArray());
    }

    /** HistoryGridFill(): GatePassOutwardDriverInfo.FormHistory - every filter only when set, as the BLL adds them. */
    public List<Map<String,Object>> history(UserAccount u, int year, int refDocumentTypeId, boolean canViewAll, String dateField,
                                            LocalDate from, LocalDate to, int fromDoc, int toDoc) {
        String sql="EXEC dbo.USP_GatePassOutwardDriverInfo_FormHistory @OrganizationId=?,@CompanyId=?";
        List<Object> args=new ArrayList<>(Arrays.asList(u.getOrganizationId(),u.getCompanyId()));
        if(year!=0){sql+=",@FinancialYearId=?";args.add(year);}
        if(u.getBranchesId()!=0){sql+=",@BranchesId=?";args.add(u.getBranchesId());}
        if(refDocumentTypeId!=0){sql+=",@DocumentTypeId=?";args.add(refDocumentTypeId);}
        sql+=",@CanViewAllRecord=?"; args.add(canViewAll);
        if(!canViewAll){sql+=",@EntryUser=?";args.add(u.getId());}
        String fromParam="@FromDate", toParam="@ToDate";
        if("entryDate".equals(dateField)){fromParam="@EntryFromDate";toParam="@EntryToDate";}
        else if("modifyDate".equals(dateField)){fromParam="@ModifyFromDate";toParam="@ModifyToDate";}
        else if(!"docDate".equals(dateField)) throw new IllegalArgumentException("Unknown history date filter");
        if(from!=null){sql+=","+fromParam+"=?";args.add(java.sql.Date.valueOf(from));}
        if(fromDoc!=0){sql+=",@FromDoc=?";args.add(fromDoc);}
        if(toDoc!=0){sql+=",@ToDoc=?";args.add(toDoc);}
        if(to!=null){sql+=","+toParam+"=?";args.add(java.sql.Date.valueOf(to));}
        return jdbc.queryForList(sql,args.toArray());
    }

    /** "CanView AllRecord" right of a frmDriverBio ScreenName (formright.DoHaveCanViewAllRecordRights). */
    public boolean hasRight(UserAccount u, String role, String screenName, String right) {
        boolean admin="Admin".equalsIgnoreCase(role)||"Administrator".equalsIgnoreCase(role);
        if(admin && !"Delete".equals(right) && !"View".equals(right)) return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName=?,@RightName=?,@CompanyId=?,@Activity='GetByUserId'",
                u.getId(),screenName,role==null?"":role,u.getCompanyId()).stream()
                .anyMatch(r->right.equalsIgnoreCase(Objects.toString(r.get("RightName"),"").trim())
                        && (Boolean.TRUE.equals(r.get("Value"))||"1".equals(Objects.toString(r.get("Value"),""))||"true".equalsIgnoreCase(Objects.toString(r.get("Value"),""))));
    }

    /** The inward gate pass a driver-info row points at (GatePassOutwardId holds a GatePassInward.Id when RefDocumentTypeId = 51). */
    public Map<String,Object> inwardGatePass(UserAccount u, int id) {
        List<Map<String,Object>> rows=jdbc.queryForList("SELECT TOP 1 Id,GpDate FROM dbo.GatePassInward WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=51",
                id,u.getOrganizationId(),u.getCompanyId());
        return rows.isEmpty()?null:rows.get(0);
    }

    /**
     * frmDriverBio.Insert() with RefDocumentTypeId > 0: FormValidation, then BLL GatePassOutwardDriverInfo.Save - date lock
     * against driverInfo.GpDate (the picker value), then Sp_GatePassOutwardDriverInfo_Insert / _Update with DocumentTypeId 93,
     * RefDocumentTypeId, ScreenName, EntryDate = ModifyDate = Now. DriverImage / FingerPrintImage are null on the desktop.
     */
    public int saveForReference(UserAccount u, SaleDriverBioRequest r, int refDocumentTypeId, String screenName, LocalDateTime gpDate) {
        Map<String,Object> old=r.id>0?record(u,r.id):null;
        if(old!=null && number(old.get("RefDocumentTypeId"))!=refDocumentTypeId) throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Driver information not found for this gate pass type");
        List<Map<String,Object>> locks=jdbc.queryForList("EXEC dbo.Sp_DateLock_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationCompanyIdandDate'",u.getOrganizationId(),u.getCompanyId());
        if(!locks.isEmpty() && locks.get(0).get("Date")!=null && gpDate!=null) {
            LocalDateTime locked=dateTime(locks.get(0).get("Date"));
            if(!gpDate.isAfter(locked)) throw new IllegalArgumentException("Not Insert or Update record please check lock date");
        }
        Timestamp now=Timestamp.valueOf(LocalDateTime.now());
        String proc=r.id>0?"Sp_GatePassOutwardDriverInfo_Update":"Sp_GatePassOutwardDriverInfo_Insert";
        String sql="EXEC dbo."+proc+" @Id=?,@DocumentTypeId=?,@GatePassOutwardId=?,@ForwarderName=?,@DriverName=?,@FatherName=?,@CnicNo=?,@DriverCellNo=?,@AlternateCellNo=?,@RemarksHeader=?,@DriverPic=?,@ThumbPic=?,@FingerPrintImage=?,@DriverImage=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,@ScreenName=?,@RefDocumentTypeId=?";
        int id=execute(sql,new Object[]{r.id,DOCUMENT_TYPE_ID,r.gatePassOutwardId,r.forwarderName==null?"":r.forwarderName,r.driverName==null?"":r.driverName,r.fatherName==null?"":r.fatherName,
                r.cnicNo==null?"":r.cnicNo,r.driverCellNo==null?"":r.driverCellNo,r.alternateCellNo==null?"":r.alternateCellNo,r.remarksHeader==null?"":r.remarksHeader,
                keep(null,old,"DriverPic"),keep(null,old,"ThumbPic"),null,null,
                now,u.getId(),now,u.getId(),u.getOrganizationId(),u.getCompanyId(),u.getBranchesId(),screenName,refDocumentTypeId},r.id);
        if(id<=0) throw new IllegalStateException("Driver information save returned no record ID");
        return id;
    }
    private static LocalDateTime dateTime(Object v){if(v instanceof java.sql.Timestamp t)return t.toLocalDateTime();if(v instanceof java.sql.Date d)return d.toLocalDate().atStartOfDay();String x=v.toString();return x.length()>10?LocalDateTime.parse(x.substring(0,19).replace(' ','T')):LocalDate.parse(x).atStartOfDay();}

    public List<Map<String,Object>> knownDrivers(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.USP_driverBiodata_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",
                u.getOrganizationId(), u.getCompanyId());
    }

    public List<Map<String,Object>> history(UserAccount u, int year, boolean canViewAll) {
        // frmDriverBio.FillHistoryGrid uses the gate-pass type (91), not the driver document type (93).
        String sql="EXEC dbo.USP_GatePassOutwardDriverInfo_FormHistory @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@CanViewAllRecord=?";
        List<Object> args=new ArrayList<>(Arrays.asList(u.getOrganizationId(),u.getCompanyId(),REF_DOCUMENT_TYPE_ID,canViewAll));
        if(u.getBranchesId()!=0){sql+=",@BranchesId=?";args.add(u.getBranchesId());}
        if(year!=0){sql+=",@FinancialYearId=?";args.add(year);}
        if(!canViewAll){sql+=",@EntryUser=?";args.add(u.getId());}
        return jdbc.queryForList(sql,args.toArray());
    }

    public boolean canViewAllRecords(UserAccount u,String role) {
        if("Admin".equalsIgnoreCase(role))return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName='frmDriverBio',@RightName=?,@CompanyId=?,@Activity='GetByUserId'",
                u.getId(),role==null?"":role,u.getCompanyId()).stream()
                .anyMatch(r->"CanView AllRecord".equalsIgnoreCase(Objects.toString(r.get("RightName"),"").trim())
                        && (Boolean.TRUE.equals(r.get("Value"))||"1".equals(Objects.toString(r.get("Value"),""))||"true".equalsIgnoreCase(Objects.toString(r.get("Value"),""))));
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
                now,u.getId(),now,u.getId(),u.getOrganizationId(),u.getCompanyId(),u.getBranchesId(),"frmDriverBio",REF_DOCUMENT_TYPE_ID},r.id);
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
