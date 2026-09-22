package com.mst.repositories.cmagt;

import com.mst.repositories.support.ProcExec;

import com.mst.models.cmagt.dto.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.sql.Types;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

/**
 * Data access for the Commission Trading "Sale Order / Deal With Buyer" screen
 * (desktop form Architecture.WinApp.Cmagt/frmSaleOrderCmagt.cs, DocumentTypeId = 1053).
 *
 * STRICT DATABASE PRESERVATION
 * ---------------------------------------------------------------------------
 * No table, column or stored procedure is created, renamed or altered here. Every
 * procedure name below was read out of the real decompiled desktop assemblies
 * (Architecture.DAL.CommissionAgent.saleOrderMaster / Architecture.BLL.CommissionAgent
 * .saleOrderMaster) and each one was then confirmed to exist in the real database export
 * index (recovered_source/DATABASE_CONTRACT.md):
 *
 *   cmagt.USP_saleOrderMaster_InsertAndUpdate        (SQL export line 252136)
 *   cmagt.USP_saleOrderMaster_GetAllMethod           (line 251631)
 *   cmagt.USP_saleOrderDetail_Insert                 (line 251003)
 *   cmagt.USP_saleOrderPaymentDetail_Insert          (line 253637)
 *   cmagt.USP_saleOrderEmptyBagDetail_Insert         (line 251212)
 *   cmagt.USP_saleOrderBuyerExpenseDetail_Insert     (line 250817)
 *   cmagt.USP_saleOrderCommissionDetail_Insert       (line 250911)
 *   cmagt.USP_AllComboServices                       (line 241583)
 *
 * PARAMETER LISTS ARE NOT GUESSED. Architecture.DAL.Common.GenericProvider.SetProc binds
 * exactly one "@" + PropertyName parameter for every NON-VIRTUAL property of the model
 * class it is handed, in declaration order. The parameter lists below are therefore a
 * literal transcription of the non-virtual properties of saleOrderMaster,
 * saleOrderDetail, saleOrderPaymentDetail, saleOrderEmptyBagDetail,
 * saleOrderbuyerExpenseDetail and saleOrderCommissionDetail. SQL Server rejects a call
 * with an argument the procedure does not declare, so this list is self-checking against
 * the live database the first time the screen is saved.
 *
 * No lookup below has a hardcoded fallback: a failing query surfaces as an empty list,
 * never as fabricated placeholder rows.
 */
@Repository
public class SaleOrderCmagtRepository {

    @Autowired
    private JdbcTemplate jdbc;

    // =======================================================================
    // Header - [cmagt].[USP_saleOrderMaster_InsertAndUpdate]
    // 42 parameters = the 42 non-virtual properties of saleOrderMaster.
    // =======================================================================
    private static final String SQL_MASTER_SAVE =
            "EXEC [cmagt].[USP_saleOrderMaster_InsertAndUpdate] "
            + "@isApproved=?, @isbuyerOtherChargesAllowed=?, @isWhtApplied=?, "
            + "@approvedDate=?, @deliveryStartDate=?, @docDate=?, @entryDate=?, @modifyDate=?, "
            + "@ValidityDate=?, @fcyAmount=?, @fcyFxRate=?, @actionId=?, @approvedUserId=?, "
            + "@branchId=?, @buyerId=?, @DeliveryToPartyId=?, @commissionAgentId=?, @companyId=?, "
            + "@deliveryDays=?, @deliveryTermId=?, @docNo=?, @documentTypeId=?, "
            + "@EBWeightDeductionTermId=?, @entryUserId=?, @fcyId=?, @financialYearId=?, "
            + "@modifyUserId=?, @organizationId=?, @projectId=?, @saleOrderMasterId=?, @statusId=?, "
            + "@approvalRemarks=?, @attachmentsValues=?, @buyerReferenceNo=?, "
            + "@customAttachmentsValues=?, @paymentScheduleDescription=?, @remarksHeader=?, "
            + "@saleOrderbuyerExpenseDetailDescription=?, @saleOrderCommissionDetailDescription=?, "
            + "@saleOrderEmptyBagDetailDescription=?, @ShipToAddress=?, @shipToAddressId=?";

    // =======================================================================
    // Detail - [cmagt].[USP_saleOrderDetail_Insert]
    // 28 parameters = the 28 non-virtual properties of saleOrderDetail.
    // The procedure itself branches on @actionTypeId (1 insert / 2 update / 3 soft delete).
    // =======================================================================
    private static final String SQL_DETAIL_SAVE =
            "EXEC [cmagt].[USP_saleOrderDetail_Insert] "
            + "@fcyAmount=?, @ItemAmount=?, @itemQty=?, @itemRate=?, @itemWeight=?, "
            + "@TaxAmount=?, @TaxPercent=?, @TotalAmount=?, @actionTypeId=?, @cropYearId=?, "
            + "@inquiryBookingDetailId=?, @inquiryBookingMasterId=?, @inventoryParentCategoryId=?, "
            + "@itemId=?, @packingTypeId=?, @packUomId=?, @purchaseOrderDetailId=?, "
            + "@purchaseOrderMasterId=?, @rateUomId=?, @saleOrderDetailId=?, @saleOrderMasterId=?, "
            + "@sortNo=?, @supplierOfferDetailId=?, @supplierOfferId=?, @TaxNameId=?, "
            + "@cropYear=?, @qualitySpecification=?, @remarks=?";

    // =======================================================================
    // Payment schedule - [cmagt].[USP_saleOrderPaymentDetail_Insert]  (10 params)
    // =======================================================================
    private static final String SQL_PAYMENT_SAVE =
            "EXEC [cmagt].[USP_saleOrderPaymentDetail_Insert] "
            + "@DueDate=?, @dueAmount=?, @pctOfTotal=?, @BaseDueDateTypeId=?, @DueDays=?, "
            + "@PaymentTermId=?, @saleOrderMasterId=?, @saleOrderPaymentDetailId=?, @sortNo=?, "
            + "@remarks=?";

    // =======================================================================
    // Empty bags - [cmagt].[USP_saleOrderEmptyBagDetail_Insert]  (9 params)
    // =======================================================================
    private static final String SQL_EMPTYBAG_SAVE =
            "EXEC [cmagt].[USP_saleOrderEmptyBagDetail_Insert] "
            + "@Rate=?, @weightCutKg=?, @PackingTypeId=?, @purchaseOrderEmptyBagDetailId=?, "
            + "@purchaseOrderMasterId=?, @saleOrderEmptyBagDetailId=?, @saleOrderMasterId=?, "
            + "@sortNo=?, @remarks=?";

    // =======================================================================
    // Buyer other charges - [cmagt].[USP_saleOrderBuyerExpenseDetail_Insert]  (10 params)
    // =======================================================================
    private static final String SQL_EXPENSE_SAVE =
            "EXEC [cmagt].[USP_saleOrderBuyerExpenseDetail_Insert] "
            + "@Qty=?, @amount=?, @rate=?, @ItemId=?, @purchaseOrderMasterId=?, "
            + "@purchaseOrderSupplierExpenseDetailId=?, @saleOrderbuyerExpenseDetailId=?, "
            + "@saleOrderMasterId=?, @sortNo=?, @remarks=?";

    // =======================================================================
    // Commission / brokery - [cmagt].[USP_saleOrderCommissionDetail_Insert]  (10 params)
    // =======================================================================
    private static final String SQL_COMMISSION_SAVE =
            "EXEC [cmagt].[USP_saleOrderCommissionDetail_Insert] "
            + "@commissionAmount=?, @commissionRate=?, @agentTypeId=?, @commissionAgentId=?, "
            + "@commissionTypeId=?, @rateUomId=?, @saleOrderCommissionDetailId=?, "
            + "@saleOrderMasterId=?, @sortNo=?, @commissionRemarks=?";

    // =======================================================================
    // Reads - [cmagt].[USP_saleOrderMaster_GetAllMethod]
    // =======================================================================
    private static final String SQL_READ_BY_ID =
            "EXEC [cmagt].[USP_saleOrderMaster_GetAllMethod] @Id=?, @Activity=?";

    /** GenerateCode - BLL saleOrderMaster.GenerateCode(). Returns a single "DocNo" column. */
    private static final String SQL_GENERATE_CODE =
            "EXEC [cmagt].[USP_saleOrderMaster_GetAllMethod] @OrganizationId=?, @CompanyId=?, "
            + "@BranchesId=?, @FinancialYearId=?, @DocumentTypeId=?, @Activity=?";

    /** DeleteById - BLL saleOrderMaster.DeleteByID(EntryUserId, Id). Whole-order soft delete. */
    private static final String SQL_DELETE_BY_ID =
            "EXEC [cmagt].[USP_saleOrderMaster_GetAllMethod] @EntryUserId=?, @Id=?, @Activity=?";

    /** FormHistory - BLL saleOrderMaster.FormHistory(ReportsParameters). 26 filter parameters. */
    private static final String SQL_FORM_HISTORY =
            "EXEC [cmagt].[USP_saleOrderMaster_GetAllMethod] "
            + "@OrganizationId=?, @CompanyId=?, @BranchesId=?, @FinancialYearId=?, "
            + "@CanViewAllRecord=?, @EntryUserId=?, @FromDate=?, @ToDate=?, "
            + "@EntryFromDate=?, @EntryToDate=?, @ModifyFromDate=?, @ModifyToDate=?, "
            + "@ApprovedFromDate=?, @ApprovedToDate=?, @ValidityDateFrom=?, @ValidityDateTo=?, "
            + "@FromDocNo=?, @ToDocNo=?, @Id=?, @CommissionAgentId=?, @buyerId=?, @ItemId=?, "
            + "@ParentItemIds=?, @DeliveryToPartyId=?, @ShipToAddress=?, @Activity=?";

    // =======================================================================
    // Lookups - every one traced to its real desktop call site
    // =======================================================================

    /**
     * [cmagt].[USP_AllComboServices] - the screen's own combo service. One result set with
     * Id / ReferenceName / Activity columns; frmSaleOrderCmagt.BindViewCombos() splits it on
     * the Activity column into CommissionType, CommissionRateUom, PaymentBaseDate and
     * AllocatedPackingType. Called with no @Activity so all four sets come back at once,
     * matching purchaseOrderMaster.AllComboServices_FromViews when Activity is empty.
     */
    private static final String SQL_ALL_COMBO_SERVICES =
            "EXEC [cmagt].[USP_AllComboServices] @OrganizationId=?, @CompanyId=?";

    /** Sp_InventoryItemsOther_GetAllMethod - Buyer Other Charges item master (Id, OtherItemName). */
    private static final String SQL_OTHER_ITEMS =
            "EXEC Sp_InventoryItemsOther_GetAllMethod @Activity=?, @organizationId=?, @CompanyId=?";

    /** Sp_SupplierCustomerShipToAddress_GetAllMethod - SupplierCustomerShipToAddress.GetAll_Combo. */
    private static final String SQL_SHIP_TO_ADDRESSES =
            "EXEC Sp_SupplierCustomerShipToAddress_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

    /** [dbo].[USP_DeliveryTerm_GetAllMethod] - DeliveryTerm.FormHistory() (Id, Description). */
    private static final String SQL_DELIVERY_TERMS =
            "EXEC [dbo].[USP_DeliveryTerm_GetAllMethod] @Activity=?";

    /** [dbo].[USP_GetBranchsAllocatedToUser] (BranchId, BranchName). */
    private static final String SQL_BRANCHES =
            "EXEC [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId=?, @CompanyId=?, @UserId=?";

    /** Sp_InvDueTerms_GetAllMethod - payment terms, same proc the Sale Order screen uses. */
    private static final String SQL_PAYMENT_TERMS =
            "EXEC Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

    /** USP_Item_AllItemsWithModal - the item master behind clsGlobalVariables.getGlobalAllItems. */
    private static final String SQL_ITEMS =
            "EXEC USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?";

    /** Sp_InvCropYear_GetAllMethod - clsGlobalVariables.globalCropYear. */
    private static final String SQL_CROP_YEARS =
            "EXEC Sp_InvCropYear_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

    /** Sp_InvPackingType_GetAllMethod - clsGlobalVariables.globalInvPackingType. */
    private static final String SQL_PACKING_TYPES =
            "EXEC Sp_InvPackingType_GetAllMethod @Activity=?";

    /** Sp_UOMSchedule_GetAllMethod - the item's UOM schedule (Pack Uom / Rate Uom). */
    private static final String SQL_UOM_BY_ITEM =
            "EXEC Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @ItemId=?, @Activity=?";

    /** Sp_ItemTaxSchedule_GetAllMethod - ItemTaxSchedule.GetTaxScheduleForItemId. */
    private static final String SQL_ITEM_TAX =
            "EXEC Sp_ItemTaxSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, "
            + "@EffectedDate=?, @Activity=?";

    /**
     * USP_GetVendorsAndCustomersWithCityName - the single party master behind
     * clsGlobalVariables.globalAllSupplierCustomer, loaded by
     * Architecture.BLL.Main.GlobalServicesMethods.getGlobalSupplierCustomer. That method
     * adds @PartyTypeId / @PageSize / @PageNumber / @Keyword only when they are non-zero,
     * and this screen calls it with none of them, so the call below passes just the two
     * mandatory parameters and gets the whole list - matching the desktop exactly.
     *
     * Returned columns actually consumed: Id, CompanyName, NickName, PartyCode, GlAccountId,
     * CityName, MobilePersonal, CityId, PartyTypeId, CustomerGroupId, ParentsSupCustId,
     * IsSubSupCust, GlAccountCurrencyId, GlAccountCurrencyCode.
     */
    private static final String SQL_PARTIES =
            "EXEC USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?";

    /**
     * dbo.Sp_tblUserRights_GetAllMethod @Activity='GetByUserId' - the real grant grid, the same
     * call Architecture.BLL.tblUserRights.GetByUserId makes. Returns one row per right defined
     * for the screen, with RightName and Value.
     *
     * Read-only: the GetByUserId branch is SELECT-only in both its Admin and non-Admin forms,
     * and both filter `ScreenDefinition.ScreenName = @ScreenName`. @Activity is the one
     * parameter the procedure declares without a default.
     */
    private static final String SQL_USER_RIGHTS_FOR_SCREEN =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
            + "@CompanyId=?, @Activity=?";

    /** SP_City_GetAllMethod - the City dropdown. */
    private static final String SQL_CITIES =
            "EXEC SP_City_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType=?";

    // =======================================================================
    // Writes
    // =======================================================================

    /**
     * Saves the header. Mirrors GenericProvider.SetProc's ExecuteScalar: the procedure
     * returns the new (or existing) saleOrderMasterId as a scalar; a call that returns no
     * row yields 0, exactly as Convert.ToInt32(null) does on the desktop.
     */
    public int saveMaster(SaleOrderCmagtDto h) {
        return scalarInt(SQL_MASTER_SAVE,
                bool(h.getIsApproved()),
                bool(h.getIsbuyerOtherChargesAllowed()),
                bool(h.getIsWhtApplied()),
                ts(h.getApprovedDate()),
                ts(h.getDeliveryStartDate()),
                ts(h.getDocDate()),
                ts(h.getEntryDate()),
                ts(h.getModifyDate()),
                ts(h.getValidityDate()),
                num(h.getFcyAmount()),
                num(h.getFcyFxRate()),
                i(h.getActionId()),
                i(h.getApprovedUserId()),
                i(h.getBranchId()),
                i(h.getBuyerId()),
                i(h.getDeliveryToPartyId()),
                i(h.getCommissionAgentId()),
                i(h.getCompanyId()),
                i(h.getDeliveryDays()),
                i(h.getDeliveryTermId()),
                i(h.getDocNo()),
                i(h.getDocumentTypeId()),
                i(h.getEBWeightDeductionTermId()),
                i(h.getEntryUserId()),
                i(h.getFcyId()),
                i(h.getFinancialYearId()),
                i(h.getModifyUserId()),
                i(h.getOrganizationId()),
                i(h.getProjectId()),
                i(h.getSaleOrderMasterId()),
                i(h.getStatusId()),
                s(h.getApprovalRemarks()),
                s(h.getAttachmentsValues()),
                s(h.getBuyerReferenceNo()),
                s(h.getCustomAttachmentsValues()),
                s(h.getPaymentScheduleDescription()),
                s(h.getRemarksHeader()),
                s(h.getSaleOrderbuyerExpenseDetailDescription()),
                s(h.getSaleOrderCommissionDetailDescription()),
                s(h.getSaleOrderEmptyBagDetailDescription()),
                s(h.getShipToAddress()),
                i(h.getShipToAddressId()));
    }

    /**
     * Saves one Detail row. Called for EVERY row including rows the user removed (which
     * arrive pre-marked actionTypeId = 3 so the procedure performs its soft delete) -
     * the desktop never physically deletes a saleOrderDetail row.
     */
    public void saveDetailRow(SaleOrderCmagtDetailDto d) {
        scalarInt(SQL_DETAIL_SAVE,
                num(d.getFcyAmount()),
                num(d.getItemAmount()),
                num(d.getItemQty()),
                num(d.getItemRate()),
                num(d.getItemWeight()),
                num(d.getTaxAmount()),
                num(d.getTaxPercent()),
                num(d.getTotalAmount()),
                i(d.getActionTypeId()),
                i(d.getCropYearId()),
                i(d.getInquiryBookingDetailId()),
                i(d.getInquiryBookingMasterId()),
                i(d.getInventoryParentCategoryId()),
                i(d.getItemId()),
                i(d.getPackingTypeId()),
                i(d.getPackUomId()),
                i(d.getPurchaseOrderDetailId()),
                i(d.getPurchaseOrderMasterId()),
                i(d.getRateUomId()),
                i(d.getSaleOrderDetailId()),
                i(d.getSaleOrderMasterId()),
                i(d.getSortNo()),
                i(d.getSupplierOfferDetailId()),
                i(d.getSupplierOfferId()),
                i(d.getTaxNameId()),
                s(d.getCropYear()),
                s(d.getQualitySpecification()),
                s(d.getRemarks()));
    }

    public void savePaymentRow(SaleOrderCmagtPaymentDto p) {
        scalarInt(SQL_PAYMENT_SAVE,
                dateOrNull(p.getDueDate()),
                num(p.getDueAmount()),
                num(p.getPctOfTotal()),
                i(p.getBaseDueDateTypeId()),
                i(p.getDueDays()),
                i(p.getPaymentTermId()),
                i(p.getSaleOrderMasterId()),
                i(p.getSaleOrderPaymentDetailId()),
                i(p.getSortNo()),
                s(p.getRemarks()));
    }

    public void saveEmptyBagRow(SaleOrderCmagtEmptyBagDto e) {
        scalarInt(SQL_EMPTYBAG_SAVE,
                num(e.getRate()),
                num(e.getWeightCutKg()),
                i(e.getPackingTypeId()),
                i(e.getPurchaseOrderEmptyBagDetailId()),
                i(e.getPurchaseOrderMasterId()),
                i(e.getSaleOrderEmptyBagDetailId()),
                i(e.getSaleOrderMasterId()),
                i(e.getSortNo()),
                s(e.getRemarks()));
    }

    public void saveExpenseRow(SaleOrderCmagtExpenseDto x) {
        scalarInt(SQL_EXPENSE_SAVE,
                num(x.getQty()),
                dbl(x.getAmount()),
                dbl(x.getRate()),
                i(x.getItemId()),
                i(x.getPurchaseOrderMasterId()),
                i(x.getPurchaseOrderSupplierExpenseDetailId()),
                i(x.getSaleOrderbuyerExpenseDetailId()),
                i(x.getSaleOrderMasterId()),
                i(x.getSortNo()),
                s(x.getRemarks()));
    }

    public void saveCommissionRow(SaleOrderCmagtCommissionDto c) {
        scalarInt(SQL_COMMISSION_SAVE,
                num(c.getCommissionAmount()),
                num(c.getCommissionRate()),
                i(c.getAgentTypeId()),
                i(c.getCommissionAgentId()),
                i(c.getCommissionTypeId()),
                i(c.getRateUomId()),
                i(c.getSaleOrderCommissionDetailId()),
                i(c.getSaleOrderMasterId()),
                i(c.getSortNo()),
                s(c.getCommissionRemarks()));
    }

    /** Whole-order soft delete, exactly as BLL saleOrderMaster.DeleteByID does. */
    public void deleteById(int entryUserId, int id) {
        ProcExec.call(jdbc, SQL_DELETE_BY_ID, entryUserId, id, "DeleteById");
    }

    // =======================================================================
    // Reads
    // =======================================================================

    public List<Map<String, Object>> readHeaderById(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadById");
    }

    public List<Map<String, Object>> readDetailByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_saleOrderDetail");
    }

    public List<Map<String, Object>> readExpensesByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_saleOrderBuyerExpenseDetail");
    }

    public List<Map<String, Object>> readEmptyBagsByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_saleOrderEmptyBagDetail");
    }

    /** Note the desktop's own activity name really does end in "Id" - preserved verbatim. */
    public List<Map<String, Object>> readCommissionsByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_saleOrderCommissionDetailId");
    }

    public List<Map<String, Object>> readPaymentsByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_saleOrderPaymentDetail");
    }

    public int generateCode(int orgId, int companyId, int branchId, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> rows = jdbc.queryForList(SQL_GENERATE_CODE,
                orgId, companyId, branchId, financialYearId, documentTypeId, "GenerateCode");
        if (rows.isEmpty()) return 0;
        Object v = rows.get(0).get("DocNo");
        return (v instanceof Number) ? ((Number) v).intValue() : 0;
    }

    public List<Map<String, Object>> formHistory(int orgId, int companyId, int branchId, int financialYearId,
                                                 boolean canViewAllRecords, int entryUserId,
                                                 String fromDate, String toDate,
                                                 String entryFrom, String entryTo,
                                                 String modifyFrom, String modifyTo,
                                                 String approvedFrom, String approvedTo,
                                                 String validityFrom, String validityTo,
                                                 Integer fromDocNo, Integer toDocNo, Integer id,
                                                 Integer commissionAgentId, Integer buyerId, Integer itemId,
                                                 String parentItemIds, Integer deliveryToPartyId,
                                                 String shipToAddress) {
        return jdbc.queryForList(SQL_FORM_HISTORY,
                orgId, companyId, branchId, financialYearId,
                // @CanViewAllRecord is declared INT, not BIT - bind it as an int rather than
                // relying on an implicit boolean-to-int conversion.
                (canViewAllRecords ? 1 : 0), entryUserId,
                dateOrNull(fromDate), dateOrNull(toDate),
                dateOrNull(entryFrom), dateOrNull(entryTo),
                dateOrNull(modifyFrom), dateOrNull(modifyTo),
                dateOrNull(approvedFrom), dateOrNull(approvedTo),
                dateOrNull(validityFrom), dateOrNull(validityTo),
                intOrNull(fromDocNo), intOrNull(toDocNo), intOrNull(id),
                intOrNull(commissionAgentId), intOrNull(buyerId), intOrNull(itemId),
                strOrNull(parentItemIds), intOrNull(deliveryToPartyId), strOrNull(shipToAddress),
                "FormHistory");
    }

    // =======================================================================
    // Lookups. A failure returns an empty list - never fabricated placeholder rows.
    // =======================================================================

    public List<Map<String, Object>> allComboServices(int orgId, int companyId) {
        return safeQuery(SQL_ALL_COMBO_SERVICES, orgId, companyId);
    }

    public List<Map<String, Object>> otherItems(int orgId, int companyId) {
        return safeQuery(SQL_OTHER_ITEMS, "ReadAll", orgId, companyId);
    }

    public List<Map<String, Object>> shipToAddresses(int orgId, int companyId) {
        return safeQuery(SQL_SHIP_TO_ADDRESSES, orgId, companyId, "ReadByOrganizationCompanyId_Combo");
    }

    public List<Map<String, Object>> deliveryTerms() {
        return safeQuery(SQL_DELIVERY_TERMS, "FormHistory");
    }

    public List<Map<String, Object>> branches(int orgId, int companyId, int userId) {
        return safeQuery(SQL_BRANCHES, orgId, companyId, userId);
    }

    public List<Map<String, Object>> paymentTerms(int orgId, int companyId) {
        return safeQuery(SQL_PAYMENT_TERMS, orgId, companyId, "GetAll");
    }

    public List<Map<String, Object>> items(int orgId, int companyId) {
        return safeQuery(SQL_ITEMS, orgId, companyId);
    }

    public List<Map<String, Object>> cropYears(int orgId, int companyId) {
        return safeQuery(SQL_CROP_YEARS, orgId, companyId, "ReadAll");
    }

    public List<Map<String, Object>> packingTypes() {
        return safeQuery(SQL_PACKING_TYPES, "ReadAll");
    }

    public List<Map<String, Object>> uomScheduleByItem(int orgId, int itemId) {
        return safeQuery(SQL_UOM_BY_ITEM, orgId, itemId, "ReadByItemID");
    }

    public List<Map<String, Object>> itemTaxSchedule(int orgId, int companyId, int itemId, String effectedDate) {
        return safeQuery(SQL_ITEM_TAX, orgId, companyId, itemId,
                dateOrNull(effectedDate), "GetItemTaxScheduleForItemId");
    }

    public List<Map<String, Object>> parties(int orgId, int companyId) {
        return safeQuery(SQL_PARTIES, orgId, companyId);
    }

    public List<Map<String, Object>> cities(int orgId, int companyId) {
        return safeQuery(SQL_CITIES, orgId, companyId, "GetAll");
    }

    /** The grant grid for one user on one desktop screen. See SQL_USER_RIGHTS_FOR_SCREEN. */
    public List<Map<String, Object>> userRightsForScreen(int userId, String screenName,
                                                         String roleName, int companyId) {
        return jdbc.queryForList(SQL_USER_RIGHTS_FOR_SCREEN,
                userId, screenName, roleName == null ? "" : roleName, companyId, "GetByUserId");
    }

    // =======================================================================
    // Helpers
    // =======================================================================

    /**
     * Runs a statement that the desktop invokes through ExecuteScalar. Returns the first
     * column of the first row, or 0 when the procedure produced no result set - matching
     * Convert.ToInt32(null) == 0 in Architecture.DAL.
     */
    private int scalarInt(String sql, Object... args) {
        Integer v = jdbc.query(sql, rs -> {
            if (rs.next()) {
                Object o = rs.getObject(1);
                if (o instanceof Number) return ((Number) o).intValue();
                if (o != null) {
                    try {
                        return Integer.valueOf(o.toString().trim());
                    } catch (NumberFormatException ignored) {
                        return 0;
                    }
                }
            }
            return 0;
        }, args);
        return v == null ? 0 : v;
    }

    /**
     * A plain lookup query. It deliberately does NOT swallow failures: a broken lookup is
     * surfaced to the caller, which decides what to show. What must never happen - and is
     * what produced the "...Select Any Value..." empty-dropdown bug elsewhere in this
     * migration - is a catch block that substitutes invented placeholder rows.
     */
    private List<Map<String, Object>> safeQuery(String sql, Object... args) {
        return jdbc.queryForList(sql, args);
    }

    private static Object s(String v) { return v == null ? "" : v; }
    private static Object i(Integer v) { return v == null ? 0 : v; }
    private static Object num(java.math.BigDecimal v) { return v == null ? java.math.BigDecimal.ZERO : v; }
    private static Object dbl(Double v) { return v == null ? 0d : v; }
    private static Object bool(Boolean v) { return v != null && v; }

    /** A non-nullable DateTime on the desktop model; defaults to now when absent. */
    private static Object ts(String iso) {
        LocalDateTime t = parse(iso);
        return Timestamp.valueOf(t == null ? LocalDateTime.now() : t);
    }

    /**
     * A nullable filter value. The type is stated explicitly so the SQL Server driver never
     * has to guess it for a NULL - guessing is what produces the driver's
     * "the supplied value is not a valid instance of data type" errors.
     */
    private static Object intOrNull(Integer v) {
        return (v == null || v == 0)
                ? new org.springframework.jdbc.core.SqlParameterValue(Types.INTEGER, null)
                : v;
    }

    private static Object strOrNull(String v) {
        return (v == null || v.trim().isEmpty())
                ? new org.springframework.jdbc.core.SqlParameterValue(Types.NVARCHAR, null)
                : v;
    }

    /** A nullable date (payment DueDate, every history filter). */
    private static Object dateOrNull(String iso) {
        LocalDateTime t = parse(iso);
        return t == null ? new org.springframework.jdbc.core.SqlParameterValue(Types.TIMESTAMP, null)
                         : Timestamp.valueOf(t);
    }

    private static LocalDateTime parse(String iso) {
        if (iso == null || iso.trim().isEmpty()) return null;
        String v = iso.trim();
        try {
            if (v.length() <= 10) return LocalDate.parse(v).atStartOfDay();
            return LocalDateTime.parse(v.replace(' ', 'T').substring(0, Math.min(19, v.length())));
        } catch (Exception ex) {
            return null;
        }
    }
}
