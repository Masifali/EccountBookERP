package com.mst.repositories.lab;

import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.ArgumentPreparedStatementSetter;
import org.springframework.jdbc.core.ColumnMapRowMapper;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.sql.Date;
import java.sql.ResultSet;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * Screen 164 "Lab Deduction Policy For Purchase" — every procedure call of the desktop form
 * Architecture.WinApp.QCL.frmQcDeductionPolicy and the BLL / DAL it uses. Nothing here is raw table
 * SQL; each method names the BLL method it reproduces and sends exactly the parameters that method
 * sends (a BLL "if (x != 0)" guard is reproduced by leaving the parameter out).
 *
 * BLL   = projects/architecture.bll/0294_Architecture.BLL.QCL.QcDeductionPolicyHeader.cs
 * DAL   = projects/architecture.dal/0252_Architecture.DAL.QCL.QcDeductionPolicyHeader.cs
 * Model = projects/architecture.model/0398 (header), 0399 (detail)
 * Proc line numbers are procdure.utf8.sql.
 */
@Repository
public class QcDeductionPolicyRepository {

    private final JdbcTemplate jdbc;

    public QcDeductionPolicyRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------------------------ DDL

    /**
     * AnalysisGroup() (form :260) -> InvLabAnalysisGroup.GetAllOrById (BLL 0400 :51): Id and
     * ParentCategoryId are 0 so neither is sent; Activity 'ReadAll' (proc :163915) returns Id,
     * AnalysisGroupCode, AnalysisGroupDescription, OrganizationId, CompanyId, GroupType,
     * InvParentCateDescription.
     */
    public List<Map<String, Object>> analysisGroups(int organizationId, int companyId) {
        return rows(
                "EXEC dbo.Sp_InvLabAnalysisGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAll");
    }

    /**
     * AnalysisParamerterByGroup (form :314) -> InvLabGroupAnalysisStandards.GetParametersFromGroupStandards
     * (BLL 0406 :102): @Id (the group), @OrganizationId, @CompanyId, Activity
     * 'GetParametersFromGroupStandards' (proc :167497, branch at +41). The form keeps
     * InvLabAnalysisItemsId + AnalysisParameterDescription of every returned row.
     */
    public List<Map<String, Object>> parametersByGroup(int organizationId, int companyId, int groupId) {
        return rows(
                "EXEC dbo.Sp_InvLabGroupAnalysisStandards_GetAllMethod @Id=?, @OrganizationId=?, @CompanyId=?, @Activity=?",
                groupId, organizationId, companyId, "GetParametersFromGroupStandards");
    }

    /**
     * UOM() (form :367) -> CommonServices.StaticColumnsService("GetUom"): dbo.SpStaticColumnNames
     * @Activity 'GetUom' (proc :268541) — the fixed list Id / type the procedure itself returns
     * (1, 5, 10, 40, 50, 60, 80, 100).
     */
    public List<Map<String, Object>> uoms() {
        return rows("EXEC dbo.SpStaticColumnNames @Activity=?", "GetUom");
    }

    // ------------------------------------------------------------------------------------ reads

    /** BLL GetByID (:37): @Id, Activity 'ReadById' (proc :563928). */
    public List<Map<String, Object>> readById(int id) {
        return rows(
                "EXEC qcl.USP_QcDeductionPolicy_GetAllMethod @Id=?, @Activity=?", id, "ReadById");
    }

    /**
     * DAL GetData (:120-125): per header, @Id = QcDeductionPolicyHeaderId, Activity 'ReadByHeaderId'.
     * The procedure's branch filters on d.QcDeductionPolicyDetailId = @Id (not on the header id) —
     * that is the database object the desktop uses and it is called unchanged.
     */
    public List<Map<String, Object>> readDetails(int headerId) {
        return rows(
                "EXEC qcl.USP_QcDeductionPolicy_GetAllMethod @Id=?, @Activity=?", headerId, "ReadByHeaderId");
    }

    /**
     * BLL FormHistory (:61): @OrganizationId, @CompanyId, @CanViewAllRecord always; the user only when
     * the user may NOT view all records; each date only when set; Activity 'FormHistory'.
     * The BLL names the user parameter "@EntryUser", which the procedure does not declare (it declares
     * @EntryUserId) — the procedure's own name is used here.
     */
    public List<Map<String, Object>> formHistory(int organizationId, int companyId, boolean canViewAllRecord,
                                                 int entryUserId, Date entryFrom, Date entryTo,
                                                 Date modifyFrom, Date modifyTo, Date approvedFrom, Date approvedTo) {
        StringBuilder sql = new StringBuilder(
                "EXEC qcl.USP_QcDeductionPolicy_GetAllMethod @OrganizationId=?, @CompanyId=?, @CanViewAllRecord=?");
        List<Object> args = new ArrayList<>();
        args.add(organizationId);
        args.add(companyId);
        args.add(canViewAllRecord);
        if (!canViewAllRecord) { sql.append(", @EntryUserId=?"); args.add(entryUserId); }
        if (entryFrom != null) { sql.append(", @EntryFromDate=?"); args.add(entryFrom); }
        if (entryTo != null) { sql.append(", @EntryToDate=?"); args.add(entryTo); }
        if (modifyFrom != null) { sql.append(", @ModifyFromDate=?"); args.add(modifyFrom); }
        if (modifyTo != null) { sql.append(", @ModifyToDate=?"); args.add(modifyTo); }
        if (approvedFrom != null) { sql.append(", @ApprovedFromDate=?"); args.add(approvedFrom); }
        if (approvedTo != null) { sql.append(", @ApprovedToDate=?"); args.add(approvedTo); }
        sql.append(", @Activity=?");
        args.add("FormHistory");
        return rows(sql.toString(), args.toArray());
    }

    // ------------------------------------------------------------------------------------ writes

    /** Architecture.Model.QCL.QcDeductionPolicyHeader — every non-virtual property. */
    public static final class Header {
        public boolean isApproved;                 // never set by the form: false
        public Timestamp approvedDate;
        public Timestamp effectiveFrom;
        public Timestamp efffectiveTo;
        public Timestamp entryDate;
        public Timestamp modifyDate;
        public int actionId;                       // BLL Save: 1 insert, 2 update
        public int approvedUserId;
        public int branchId;                       // never set by the form: 0
        public int companyId;
        public int entryUserId;
        public int modifyUserId;
        public int organizationId;
        public int projectId;                      // never set by the form: 0
        public int qcDeductionPolicyHeaderId;
        public int invLabGroupAnalysisStandardsId;
        public String policyName;
        public String remarksHeader;
    }

    /** Architecture.Model.QCL.QcDeductionPolicyDetail — every non-virtual property. */
    public static final class Detail {
        public BigDecimal dedOnRateRs = BigDecimal.ZERO;
        public BigDecimal dedWeight = BigDecimal.ZERO;
        public BigDecimal rangeFrom = BigDecimal.ZERO;
        public BigDecimal rangeTo = BigDecimal.ZERO;
        public double dedOnQtyKg;
        public int actionTypeId;                   // 1 insert, 2 update, 3 delete
        public int analysisParameterId;
        public int dedQtyUom;                      // never set by the form: 0
        public int deductionParameterId;           // never set by the form: 0
        public int dedWtUom;
        public int qcDeductionPolicyDetailId;
        public int qcDeductionPolicyHeaderId;
        public int sortNo;                         // never set by the form: 0
        public String deductionParameterName;      // never set by the form: null
    }

    /**
     * BLL Save (:15) -> DAL SetData (:16) -> GenericProvider.SetProc (DAL 0207 :283): one parameter per
     * non-virtual model property — the 18 below, each declared by
     * [qcl].[USP_QcDeductionPolicyHeader_InsertAndUpdate] (proc :564357; @PendingForView and @UserLogId
     * are not model properties and keep their defaults). ActionId 1 ends with SELECT of the new id;
     * ActionId 2 returns no row, so the DAL keeps obj.QcDeductionPolicyHeaderId (:45-52).
     */
    public int saveHeader(Header h) {
        Integer id = ProcExec.call(jdbc,
                "EXEC qcl.USP_QcDeductionPolicyHeader_InsertAndUpdate @IsApproved=?, @ApprovedDate=?, @EffectiveFrom=?,"
              + " @EfffectiveTo=?, @EntryDate=?, @ModifyDate=?, @ActionId=?, @ApprovedUserId=?, @BranchId=?, @CompanyId=?,"
              + " @EntryUserId=?, @ModifyUserId=?, @OrganizationId=?, @ProjectId=?, @QcDeductionPolicyHeaderId=?,"
              + " @InvLabGroupAnalysisStandardsId=?, @PolicyName=?, @RemarksHeader=?",
                h.isApproved, h.approvedDate, h.effectiveFrom, h.efffectiveTo, h.entryDate, h.modifyDate, h.actionId,
                h.approvedUserId, h.branchId, h.companyId, h.entryUserId, h.modifyUserId, h.organizationId, h.projectId,
                h.qcDeductionPolicyHeaderId, h.invLabGroupAnalysisStandardsId, h.policyName, h.remarksHeader);
        return id != null && id > 0 ? id : h.qcDeductionPolicyHeaderId;
    }

    /**
     * DAL SetData (:53-57): one [qcl].[USP_QcDeductionPolicyDetail_Insert] (proc :564207) per detail
     * through SetProc — the 14 non-virtual properties, all declared by the procedure.
     * ActionTypeId 1 inserts (Max+1 id), 2 updates by detail id, 3 marks the detail deleted.
     */
    public void saveDetail(Detail d) {
        ProcExec.run(jdbc,
                "EXEC qcl.USP_QcDeductionPolicyDetail_Insert @dedOnRateRs=?, @dedWeight=?, @RangeFrom=?, @RangeTo=?,"
              + " @dedOnQtyKg=?, @ActionTypeId=?, @AnalysisParameterId=?, @dedQtyUom=?, @DeductionParameterId=?,"
              + " @dedWtUom=?, @QcDeductionPolicyDetailId=?, @QcDeductionPolicyHeaderId=?, @SortNo=?,"
              + " @DeductionParameterName=?",
                d.dedOnRateRs, d.dedWeight, d.rangeFrom, d.rangeTo, d.dedOnQtyKg, d.actionTypeId,
                d.analysisParameterId, d.dedQtyUom, d.deductionParameterId, d.dedWtUom,
                d.qcDeductionPolicyDetailId, d.qcDeductionPolicyHeaderId, d.sortNo, d.deductionParameterName);
    }

    /** BLL DeleteByID (:151): @EntryUserId, @Id, Activity 'DeleteById' (proc :563928 — marks ActionId 3). */
    public void deleteById(int id, int entryUserId) {
        ProcExec.run(jdbc,
                "EXEC qcl.USP_QcDeductionPolicy_GetAllMethod @EntryUserId=?, @Id=?, @Activity=?",
                entryUserId, id, "DeleteById");
    }

    // ------------------------------------------------------------------------------------ attachments

    /** DMSAttachments.GetByID (FormHelper.LoadAttachmentsForObject :608): @ScreenName, @Id, 'ReadById'. */
    public List<Map<String, Object>> attachments(int id, String screenName) {
        return rows(
                "EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?",
                screenName, id, "ReadById");
    }

    /** DAL SetData (:60-76): Sp_DMSAttachments_GetAllMethod @ScreenName, @Id, Activity 'DeleteById'. */
    public void removeAttachments(int id, String screenName) {
        ProcExec.run(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?",
                screenName, id, "DeleteById");
    }

    /**
     * DAL SetData (:77-89): one Proc_DMSAttachments_Insert per attachment through SetProc — the 18
     * parameters the procedure declares (proc :36546). RefAccountId = RefDocumentNo = the header id,
     * RefDocumentTypeId 0 (DAL :79-81).
     */
    public void insertAttachment(int refDocumentNo, String attachment, Timestamp entryDate, int entryUser,
                                 Timestamp modifyDate, int modifyUser, int organizationId, int companyId,
                                 String screenName, String uploadedFileCustomName, double uploadedFileSizeMb) {
        ProcExec.run(jdbc,
                "EXEC dbo.Proc_DMSAttachments_Insert @OrganizationId=?, @RefDocumentNo=?, @CompanyId=?, @ModifyUser=?,"
              + " @Attachment=?, @BranchId=?, @Id=?, @EntryDate=?, @RefDocumentTypeId=?, @RefAccountId=?,"
              + " @DMSFoldersLabelsId=?, @ModifyDate=?, @EntryUser=?, @ScreenName=?, @UploadedFileCustomName=?,"
              + " @UploadedFileSizeMb=?, @DetailWiseAttachment=?, @LineId=?",
                organizationId, refDocumentNo, companyId, modifyUser, attachment, 0, 0, entryDate,
                0, refDocumentNo, 0, modifyDate, entryUser, screenName, uploadedFileCustomName,
                uploadedFileSizeMb, false, 0);
    }

    /**
     * The first result set of a procedure call. The 'FormHistory' branch fills temp tables before its
     * final SELECT, so update counts can precede the rows; they are skipped here (SqlDataAdapter.Fill
     * does the same on the desktop).
     */
    private List<Map<String, Object>> rows(String sql, Object... args) {
        List<Map<String, Object>> out = jdbc.execute(sql, (PreparedStatementCallback<List<Map<String, Object>>>) ps -> {
            new ArgumentPreparedStatementSetter(args).setValues(ps);
            ColumnMapRowMapper mapper = new ColumnMapRowMapper();
            List<Map<String, Object>> list = new ArrayList<>();
            boolean isResultSet = ps.execute();
            while (true) {
                if (isResultSet) {
                    try (ResultSet rs = ps.getResultSet()) {
                        int n = 0;
                        while (rs.next()) list.add(mapper.mapRow(rs, n++));
                    }
                    return list;
                }
                if (ps.getUpdateCount() == -1) return list;
                isResultSet = ps.getMoreResults();
            }
        });
        return out == null ? new ArrayList<>() : out;
    }

    /** Case-insensitive column read (SQL Server column names keep the procedure's casing). */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }
}
