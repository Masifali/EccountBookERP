package com.mst.repositories.lab;

import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

/**
 * Screen 166 "Lab Standard Policy For Deduction (Not Use)" — every procedure call of the desktop form
 * Architecture.WinApp.Lab.InvLabStandardPolicyForDeduction and the BLL / DAL it uses. Nothing here is
 * raw table SQL; each method names the BLL method it reproduces and sends exactly the parameters that
 * method sends (a BLL "if (x != 0)" guard is reproduced by leaving the parameter out).
 *
 * BLL  = projects/architecture.bll/0398_Architecture.BLL.Lab.InvLabAnalysisStandardDeductionPolicyHeader.cs
 * DAL  = projects/architecture.dal/0355_Architecture.DAL.Lab.InvLabAnalysisStandardDeductionPolicyHeader.cs
 * Proc line numbers are procdure.utf8.sql.
 */
@Repository
public class LabStandardPolicyForDeductionRepository {

    private final JdbcTemplate jdbc;

    public LabStandardPolicyForDeductionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------------------------ DDL

    /**
     * ApplyOnFill (form :235, 'LabApplyOn'), DeductionTypeFill (:256, 'LabDeductionOn'), UOM (:276,
     * 'GetUom') -> CommonServices.StaticColumnsService (CommonServices.cs:2296) ->
     * GeneralReprots.StaticColumnNames (BLL 0136 :177): SpStaticColumnNames @Activity (proc :268273).
     * Each activity returns Id, type.
     */
    public List<Map<String, Object>> staticColumns(String activity) {
        return jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity=?", activity);
    }

    /**
     * ItemBind (form :192) -> CommonServices.ItemGetForComboServiceBind() (CommonServices.cs:1550,
     * ParentCategoryId 0) -> Item.GetAllbyCombobind (BLL 0583 :138): @InventoryParentCategoriesId is
     * added only when non-zero, so it is not sent. Activity 'ReadAllForComboTwoColumns' (proc :198554)
     * returns Id, ItemName, ItemCategory, ItemCode, InventoryParentCategoriesId.
     */
    public List<Map<String, Object>> items(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAllForComboTwoColumns");
    }

    /**
     * LabItemPerameterFill (form :209) -> InvLabAnalysisItems.GetAllOrById (BLL 0402 :29): @Id only when
     * non-zero (the form leaves it 0), @OrganizationId, @CompanyId, Activity 'ReadAll' (proc :164401).
     * The form binds Id + AnalysisParameterDescription (BindDDLNew, DropDownBind.cs:11).
     */
    public List<Map<String, Object>> analysisParameters(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvLabAnalysisItems_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAll");
    }

    // ------------------------------------------------------------------------------------ reads

    /**
     * gridhistoryfill (form :712) -> BLL GetAll (:32): @OrganizationId, @CompanyId, Activity 'ReadAll'
     * (proc :167222) -> Id, PolicyApplyOn, ItemName, EffectiveFrom, EfffectiveTo, RemarksHeader.
     */
    public List<Map<String, Object>> readAll(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvLabAnalysisStandardDeductionPolicyHeader_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAll");
    }

    /** getUpdate (form :600) -> BLL GetById (:61): @Id, Activity 'ReadById' -> DAL GetData (:44). */
    public List<Map<String, Object>> readById(int id) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvLabAnalysisStandardDeductionPolicyHeader_GetAllMethod @Id=?, @Activity=?",
                id, "ReadById");
    }

    /** DAL GetData (:56-62): per header, @Id + Activity 'ReadByHeaderId' -> the detail rows. */
    public List<Map<String, Object>> readDetail(int headerId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvLabAnalysisStandardDeductionPolicyHeader_GetAllMethod @Id=?, @Activity=?",
                headerId, "ReadByHeaderId");
    }

    // ------------------------------------------------------------------------------------ writes

    /** Architecture.Model.Lab.InvLabAnalysisStandardDeductionPolicyHeader (Model 0610) — non-virtual properties. */
    public static final class Header {
        public int id;
        public String policyApplyOn;
        public Timestamp effectiveFrom;
        public Timestamp efffectiveTo;
        public int itemId;
        public String remarksHeader;
        public boolean isApproved;          // never set by the form: false
        public int organizationId;
        public int companyId;
        public int branchId;                // never set by the form: 0
        public int projectId;               // never set by the form: 0
        public Timestamp entryDate;
        public int entryUserId;
        public Timestamp modifyDate;
        public Timestamp approvedDate;
        public int approvedUserId;          // never set by the form: 0
        /* PolicyName is never set by the form: a null string, which SqlCommand does not send, so the
           procedure's own default (null) applies. It is therefore left out of the call below. */
    }

    /**
     * GenericProvider.SetProc (DAL 0207 :283) sends one parameter per non-virtual model property. Both
     * procedures (Insert :167293, Update :167396) declare the same 17 parameters; @PolicyName is the one
     * not sent (see {@link Header}).
     */
    private static final String HEADER_PARAMS =
            " @IsApproved=?, @ApprovedDate=?, @EffectiveFrom=?, @EfffectiveTo=?, @EntryDate=?, @ModifyDate=?,"
          + " @ApprovedUserId=?, @BranchId=?, @CompanyId=?, @EntryUserId=?, @Id=?, @ItemId=?, @OrganizationId=?,"
          + " @ProjectId=?, @PolicyApplyOn=?, @RemarksHeader=?";

    private static Object[] headerArgs(Header h) {
        return new Object[] {
                h.isApproved, h.approvedDate, h.effectiveFrom, h.efffectiveTo, h.entryDate, h.modifyDate,
                h.approvedUserId, h.branchId, h.companyId, h.entryUserId, h.id, h.itemId, h.organizationId,
                h.projectId, h.policyApplyOn, h.remarksHeader };
    }

    /**
     * BLL Save (:15) with Id == 0 -> DAL SetData(obj, "Sp_InvLabAnalysisStandardDeductionPolicyHeader_Insert").
     * The procedure ends with SELECT SCOPE_IDENTITY() — the new Id (DAL :19-23).
     */
    public int insertHeader(Header h) {
        Integer id = ProcExec.call(jdbc,
                "EXEC dbo.Sp_InvLabAnalysisStandardDeductionPolicyHeader_Insert" + HEADER_PARAMS, headerArgs(h));
        return id == null ? 0 : id;
    }

    /**
     * BLL Save (:15) with Id != 0 -> "Sp_InvLabAnalysisStandardDeductionPolicyHeader_Update". The procedure
     * updates the header, DELETES every detail row of it (:167489) and returns no row, so ExecuteScalar is
     * null -> 0 and the DAL keeps obj.Id (DAL :24-27).
     */
    public void updateHeader(Header h) {
        ProcExec.run(jdbc,
                "EXEC dbo.Sp_InvLabAnalysisStandardDeductionPolicyHeader_Update" + HEADER_PARAMS, headerArgs(h));
    }

    /**
     * DAL SetData (:28-32): one Sp_InvLabAnalysisStandardDeductionPolicyDetail_Insert (proc :167135) per
     * grid row through SetProc — the 9 non-virtual properties of the detail model (Model 0609). Id and
     * SortNo are never set by the form: 0.
     */
    public void insertDetail(int headerId, int analysisParameterId, BigDecimal rangeFrom, BigDecimal rangeTo,
                             String deductFrom, BigDecimal dedValue, double weightKg) {
        ProcExec.run(jdbc,
                "EXEC dbo.Sp_InvLabAnalysisStandardDeductionPolicyDetail_Insert @DedValue=?, @RangeFrom=?, @RangeTo=?,"
              + " @WeightKg=?, @AnalysisParameterId=?, @Id=?, @InvLabAnalysisStandardDeductionPolicyHeaderId=?,"
              + " @SortNo=?, @DeductFrom=?",
                dedValue, rangeFrom, rangeTo, weightKg, analysisParameterId, 0, headerId, 0, deductFrom);
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
