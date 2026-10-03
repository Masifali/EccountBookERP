package com.mst.repositories.lab;

import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Date;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * Screen 155 "Sample Log Register" — every procedure call of the desktop form
 * Architecture.WinApp.Lab.InvLabSampleLogRegister and the BLL / DAL it uses. Nothing here is raw
 * table SQL; each method names the BLL method it reproduces and sends exactly the parameters that
 * method sends (a BLL "if (x != 0)" guard is reproduced by leaving the parameter out).
 *
 * BLL  = projects/architecture.bll/0408_Architecture.BLL.Lab.InvLabSampleLogRegister.cs
 * DAL  = projects/architecture.dal/0363_Architecture.DAL.Lab.InvLabSampleLogRegister.cs
 * Proc line numbers are procdure.utf8.sql.
 */
@Repository
public class SampleLogRegisterRepository {

    private final JdbcTemplate jdbc;

    public SampleLogRegisterRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------------------------ DDL

    /**
     * ItemFill (form :230) -> CommonServices.ItemGetForComboServiceBind() (CommonServices.cs:1550,
     * ParentCategoryId 0) -> Item.GetAllbyCombobind (BLL 0583 :138): @InventoryParentCategoriesId is
     * added only when non-zero, so it is not sent. Activity 'ReadAllForComboTwoColumns' (proc :198969)
     * returns Id, ItemName, ItemCategory, ItemCode, InventoryParentCategoriesId.
     */
    public List<Map<String, Object>> items(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAllForComboTwoColumns");
    }

    /**
     * SupplierFillGetAll (form :273) -> CommonServices.SupplierCustomerGetAllServiceBind()
     * (CommonServices.cs:1164) -> SupplierCustomer.Getall (BLL 0600 :110), Activity
     * 'ReadByOrganizationCompanyId' (proc :229282). The form keeps Id + CompanyName (:301, :352).
     */
    public List<Map<String, Object>> suppliers(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadByOrganizationCompanyId");
    }

    /**
     * CropYearFill (form :313) -> CommonServices.CropYearGetAllService() (CommonServices.cs:2276) ->
     * InvCropYear.Getall (BLL 0571 :53), Activity 'ReadAll' (proc :124083). Value Id, display CropYear.
     */
    public List<Map<String, Object>> cropYears(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadAll");
    }

    /**
     * JobLotFill (form :327) -> CommonServices.JobLotGetAllService() (CommonServices.cs:2091) ->
     * jobLot.GetAll (BLL 0594 :70), Activity 'GetAll' (proc :208247). Value Id, display JobLotDescription.
     */
    public List<Map<String, Object>> jobLots(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "GetAll");
    }

    /**
     * CityFill (form :366) -> City.GetAll (BLL 0060 :31): the parameter is @MethodType (not @Activity),
     * value 'GetAll' (proc :52099). Value Id, display CityName.
     */
    public List<Map<String, Object>> cities(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.SP_City_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType=?",
                organizationId, companyId, "GetAll");
    }

    /**
     * bindPackSizeInput (form :247) -> UOMSchedule.SearchByObject (BLL 0610 :82): @ItemId is always sent
     * (no guard), Activity 'ReadByItemID' (proc :235716). Value Id, display Equivalent.
     */
    public List<Map<String, Object>> uomSchedule(int organizationId, int companyId, int itemId) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",
                organizationId, companyId, itemId, "ReadByItemID");
    }

    // ------------------------------------------------------------------------------------ reads

    /** SampleCode (form :206) -> BLL GenerateCode (:33), Activity 'GenerateDocNo' (proc :169548). */
    public int generateCode(int organizationId, int companyId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_InvLabSampleLogRegister_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "GenerateDocNo");
        if (rows.isEmpty()) return 0;
        Object v = ci(rows.get(0), "SampleNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    /**
     * grdHistoryForLoad (form :628) -> BLL GetAll (:68): @NoOfRecords only when non-zero (the form sends
     * 50), @CanViewAllRecord always, @EntryUser only when the user may NOT view all records (:96),
     * Activity 'ReadAll' (proc :169521).
     */
    public List<Map<String, Object>> readAll(int organizationId, int companyId, int noOfRecords,
                                             boolean canViewAllRecord, int userId) {
        StringBuilder sql = new StringBuilder(
                "EXEC dbo.Sp_InvLabSampleLogRegister_GetAllMethod @OrganizationId=?, @CompanyId=?");
        List<Object> args = new ArrayList<>();
        args.add(organizationId);
        args.add(companyId);
        if (noOfRecords != 0) { sql.append(", @NoOfRecords=?"); args.add(noOfRecords); }
        sql.append(", @CanViewAllRecord=?");
        args.add(canViewAllRecord);
        if (!canViewAllRecord) { sql.append(", @EntryUser=?"); args.add(userId); }
        sql.append(", @Activity=?");
        args.add("ReadAll");
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    /** ReadById (form :752) -> BLL GetById (:117): @Id, Activity 'ReadById' (proc :169555). */
    public List<Map<String, Object>> readById(int id) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_InvLabSampleLogRegister_GetAllMethod @Id=?, @Activity=?", id, "ReadById");
    }

    /**
     * btnshow_Click (form :1120) -> BLL GetPrintSlipAndReport (:168), proc
     * Sp_InvLabSampleLogRegister_RiceSlipAndRegister_Rep (:169731). The form leaves Id, FromDocNo,
     * BranchesId and ProjectsId at 0, so none of those is sent; the two dates are always sent; the four
     * ids are sent only when non-zero and @CropYear only when the text is not empty; @OrganizationId
     * and @CompanyId last.
     */
    public List<Map<String, Object>> register(int organizationId, int companyId, Date fromDate, Date toDate,
                                              int supplierCustomerId, int referencePartyId, int cityId,
                                              int itemId, String cropYear) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvLabSampleLogRegister_RiceSlipAndRegister_Rep ");
        List<Object> args = new ArrayList<>();
        sql.append("@FromDate=?, @ToDate=?");
        args.add(fromDate);
        args.add(toDate);
        if (supplierCustomerId != 0) { sql.append(", @SupplierCustomerId=?"); args.add(supplierCustomerId); }
        if (referencePartyId != 0) { sql.append(", @ReferencePartyId=?"); args.add(referencePartyId); }
        if (cityId != 0) { sql.append(", @CityAreaId=?"); args.add(cityId); }
        if (itemId != 0) { sql.append(", @ItemId=?"); args.add(itemId); }
        if (cropYear != null && !cropYear.isEmpty()) { sql.append(", @CropYear=?"); args.add(cropYear); }
        sql.append(", @OrganizationId=?, @CompanyId=?");
        args.add(organizationId);
        args.add(companyId);
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    // ------------------------------------------------------------------------------------ writes

    /** The model Architecture.Model.Lab.InvLabSampleLogRegister — every non-virtual property. */
    public static final class Header {
        public int id;
        public int sampleNo;
        public Date sampleDate;
        public int supplierCustomerId;
        public int cityId;
        public int referencePartyId;
        public int stockPartyId;            // never set by the form: 0
        public int itemId;
        public String crop;
        public String lotDesc;
        public int qty;
        public double weight;
        public String otherRemarks;
        public String samplePic;
        public String sampleCookingPic;     // Save: null (:516); Update: never set -> null
        public boolean isAttachment;        // false (:517)
        public int organizationId;
        public int companyId;
        public int branchId;                // never set by the form: 0
        public int projectId;               // never set by the form: 0
        public int invLabSampleAnalysisId;  // never set by the form: 0
        public Timestamp entryDate;
        public int entryUser;
        public Timestamp modifyDate;
        public int modifyUser;
        public int documentTypeId;
        public int jobLotId;
        public int itemUomId;
    }

    /**
     * GenericProvider.SetProc (DAL 0207 :283) sends one parameter per non-virtual model property — the
     * 28 below, each of which is declared by both procedures (Insert :169620, Update :169786).
     */
    private static final String HEADER_PARAMS =
            " @IsAttachment=?, @SampleDate=?, @Weight=?, @BranchId=?, @CityId=?, @CompanyId=?, @Id=?,"
          + " @InvLabSampleAnalysisId=?, @ItemId=?, @JobLotId=?, @ItemUomId=?, @OrganizationId=?, @ProjectId=?,"
          + " @Qty=?, @ReferencePartyId=?, @SampleNo=?, @StockPartyId=?, @SupplierCustomerId=?, @Crop=?,"
          + " @LotDesc=?, @OtherRemarks=?, @SampleCookingPic=?, @SamplePic=?, @DocumentTypeId=?, @EntryDate=?,"
          + " @ModifyDate=?, @EntryUser=?, @ModifyUser=?";

    private static Object[] headerArgs(Header h) {
        return new Object[] {
                h.isAttachment, h.sampleDate, h.weight, h.branchId, h.cityId, h.companyId, h.id,
                h.invLabSampleAnalysisId, h.itemId, h.jobLotId, h.itemUomId, h.organizationId, h.projectId,
                h.qty, h.referencePartyId, h.sampleNo, h.stockPartyId, h.supplierCustomerId, h.crop,
                h.lotDesc, h.otherRemarks, h.sampleCookingPic, h.samplePic, h.documentTypeId, h.entryDate,
                h.modifyDate, h.entryUser, h.modifyUser };
    }

    /**
     * BLL Save (:15) with Id == 0 -> DAL SetDate(obj, "Sp_InvLabSampleLogRegister_Insert"). The procedure
     * assigns Max(Id)+1 and ends with SELECT @Id (:169715) — the new Id.
     */
    public int insert(Header h) {
        Integer id = ProcExec.call(jdbc, "EXEC dbo.Sp_InvLabSampleLogRegister_Insert" + HEADER_PARAMS, headerArgs(h));
        return id == null ? 0 : id;
    }

    /**
     * BLL Save (:15) with Id != 0 -> "Sp_InvLabSampleLogRegister_Update". It returns no row, so
     * ExecuteScalar is null -> 0 and the DAL keeps obj.Id (DAL :20-27).
     */
    public void update(Header h) {
        ProcExec.run(jdbc, "EXEC dbo.Sp_InvLabSampleLogRegister_Update" + HEADER_PARAMS, headerArgs(h));
    }

    /** DMSAttachments.GetByID (BLL 0069 :50): @ScreenName, @Id, Activity 'ReadById' (proc :64059). */
    public List<Map<String, Object>> attachments(int id, String screenName) {
        return jdbc.queryForList(
                "EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?",
                screenName, id, "ReadById");
    }

    /** DMSAttachments.RemoveById (BLL 0069 :216): @ScreenName, @Id, Activity 'DeleteById' (proc :64165). */
    public void removeAttachments(int id, String screenName) {
        ProcExec.run(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?",
                screenName, id, "DeleteById");
    }

    /**
     * DAL SetDate (:28-41): one Proc_DMSAttachments_Insert per attachment through SetProc — the 18
     * non-virtual properties of Architecture.Model.DMSAttachments (proc :36556 declares all of them).
     * DMSFoldersLabelsId 0 and BranchId 0 are the DAL's own values (:39-40); Id, DetailWiseAttachment and
     * LineId are the model defaults.
     */
    public void insertAttachment(int refDocumentNo, int refAccountId, int refDocumentTypeId, String attachment,
                                 Timestamp entryDate, int entryUser, Timestamp modifyDate, int modifyUser,
                                 int organizationId, int companyId, String screenName,
                                 String uploadedFileCustomName, double uploadedFileSizeMb) {
        ProcExec.run(jdbc,
                "EXEC dbo.Proc_DMSAttachments_Insert @OrganizationId=?, @RefDocumentNo=?, @CompanyId=?, @ModifyUser=?,"
              + " @Attachment=?, @BranchId=?, @Id=?, @EntryDate=?, @RefDocumentTypeId=?, @RefAccountId=?,"
              + " @DMSFoldersLabelsId=?, @ModifyDate=?, @EntryUser=?, @ScreenName=?, @UploadedFileCustomName=?,"
              + " @UploadedFileSizeMb=?, @DetailWiseAttachment=?, @LineId=?",
                organizationId, refDocumentNo, companyId, modifyUser, attachment, 0, 0, entryDate,
                refDocumentTypeId, refAccountId, 0, modifyDate, entryUser, screenName, uploadedFileCustomName,
                uploadedFileSizeMb, false, 0);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period (form :731) — the active years of the company. */
    public List<Map<String, Object>> activeFinancialYears(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                organizationId, companyId);
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
