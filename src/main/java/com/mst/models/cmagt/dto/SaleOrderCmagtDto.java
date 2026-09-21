package com.mst.models.cmagt.dto;

import lombok.Data;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

/**
 * Mirrors Architecture.Model.CommissionAgent.saleOrderMaster exactly.
 *
 * The 42 NON-VIRTUAL properties below are precisely the parameter list of
 * [cmagt].[USP_saleOrderMaster_InsertAndUpdate] - derived from
 * Architecture.DAL.Common.GenericProvider.SetProc, which binds one
 * "@" + PropertyName parameter per non-virtual property. The five List properties and
 * the display-only strings (CommissionAgentName, BuyerName, DeliveryTerm, status) are
 * virtual on the desktop model and are therefore never bound as parameters.
 *
 * Screen: "Sale Order / Deal With Buyer", desktop form
 * Architecture.WinApp.Cmagt/frmSaleOrderCmagt.cs, DocumentTypeId = 1053
 * (DocumentTypeEnum.SaleOrder).
 */
@Data
public class SaleOrderCmagtDto {

    // ---- flags ------------------------------------------------------------
    private Boolean isApproved = false;
    private Boolean isbuyerOtherChargesAllowed = false;
    private Boolean isWhtApplied = false;

    // ---- dates (ISO yyyy-MM-dd from the browser) --------------------------
    private String approvedDate;
    private String deliveryStartDate;
    private String docDate;
    private String entryDate;
    private String modifyDate;
    private String ValidityDate;

    // ---- foreign currency (present on the model; the desktop form does not
    //      expose these controls, so they stay at their model defaults) ------
    private BigDecimal fcyAmount = BigDecimal.ZERO;
    private BigDecimal fcyFxRate = BigDecimal.ZERO;
    private Integer fcyId = 0;

    // ---- identity / context ----------------------------------------------
    /** 1 when inserting, 2 when updating - set server-side, exactly as BLL Save() does. */
    private Integer actionId = 1;
    private Integer approvedUserId = 0;
    private Integer branchId = 0;
    private Integer companyId = 0;
    private Integer documentTypeId = 1053;
    private Integer entryUserId = 0;
    private Integer financialYearId = 0;
    private Integer modifyUserId = 0;
    private Integer organizationId = 0;
    private Integer projectId = 0;
    private Integer saleOrderMasterId = 0;
    private Integer statusId = 1;
    private Integer docNo = 0;

    // ---- parties ----------------------------------------------------------
    private Integer buyerId = 0;
    private Integer DeliveryToPartyId = 0;
    private Integer commissionAgentId = 0;
    private Integer shipToAddressId = 0;
    private String ShipToAddress = "";
    private String buyerReferenceNo = "";

    // ---- delivery ---------------------------------------------------------
    private Integer deliveryDays = 0;
    private Integer deliveryTermId = 0;

    /**
     * Empty-bag weight-deduction policy radio group:
     * 1 = Company Owned (weight cut applies), 2 = Bag Purchase (weight cut applies),
     * 3 = Bag FOC (weight cut does NOT apply), 0 = none selected.
     */
    private Integer EBWeightDeductionTermId = 0;

    // ---- text -------------------------------------------------------------
    private String approvalRemarks = "";
    private String attachmentsValues = "";
    private String customAttachmentsValues = "";
    private String paymentScheduleDescription = "";
    private String remarksHeader = "";
    private String saleOrderbuyerExpenseDetailDescription = "";
    private String saleOrderCommissionDetailDescription = "";
    private String saleOrderEmptyBagDetailDescription = "";

    /**
     * UI-only fields, NOT properties of the desktop model and therefore NOT procedure
     * parameters. They are the "Add Single or Multi Payment Schedule" box's own
     * CmbPaymentTerm / txtDueDays controls, which the desktop reads only when the Payment
     * Schedule grid totals zero - in that case it synthesises one implicit 100% schedule
     * row from them (frmSaleOrderCmagt.Insert(), else-branch of `if (num > 0m)`).
     */
    private Integer headerPaymentTermId = 0;
    private Integer headerDueDays = 0;

    // ---- virtual (display only) - never procedure parameters --------------
    private String CommissionAgentName;
    private String BuyerName;
    private String DeliveryTerm;
    private String status;

    // ---- child lists ------------------------------------------------------
    private List<SaleOrderCmagtDetailDto> saleOrderDetailList = new ArrayList<>();
    private List<SaleOrderCmagtPaymentDto> saleOrderPaymentDetailList = new ArrayList<>();
    private List<SaleOrderCmagtEmptyBagDto> saleOrderEmptyBagDetailList = new ArrayList<>();
    private List<SaleOrderCmagtExpenseDto> saleOrderBuyerExpenseDetailList = new ArrayList<>();
    private List<SaleOrderCmagtCommissionDto> saleOrderCommissionDetailList = new ArrayList<>();

    /**
     * Rows the user removed from the Detail grid during an edit session. The desktop keeps
     * these in lstRemoveRecordDetail and re-submits them pre-marked actionTypeId = 3 so the
     * row-insert procedure performs its soft delete. Row deletion is NEVER implemented as
     * "just don't send the row".
     */
    private List<SaleOrderCmagtDetailDto> removedDetailRows = new ArrayList<>();
}
