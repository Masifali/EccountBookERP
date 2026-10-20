package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.InvGrn (GRN header).
 * Source: Sp_InvGrn_Insert / Sp_InvGrn_Update / Sp_InvGRN_GetAllMethod / USp_InvGrnStore_Register.
 * Id is app-assigned (Sp_InvGrn_Insert computes ISNULL(MAX(Id),0)+1), so no @GeneratedValue.
 *
 * NOTE: the registry also lists GatePassId, PurchaseOrderId, RefDocumentTypeId, LabWeightCutOnId, ScaleCharges,
 * ShortageAmountFV, DiscountAmountFV, ChargeToPartyAmountFV, OtherFreightDeduction, TicketNos, FreightId, InvoiceId,
 * InvoiceNo, OtherSupCust, OrderCatagoryId, Freight, NetPaid, GpSrNo, SupplierWeight for "InvGrn". In the procedures
 * those are columns of the temp table #ReadById (aliased InvGrn) or of GatePassInward / InvFreightVoucher, NOT real
 * columns of dbo.InvGrn, so they are intentionally not mapped here (mapping them would break every Hibernate
 * SELECT/INSERT on this table). They are exposed through InvGrnReadByIdRow instead.
 */
@Entity
@Setter
@Getter
@Table(name = "InvGrn")
public class InvGrn implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "DocDate")
    private LocalDateTime docDate;

    @Column(name = "DocNo")
    private Integer docNo;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "GpNo")
    private Integer gpNo;

    @Column(name = "ReferencePartyId")
    private Integer referencePartyId;

    @Column(name = "StockPartyId")
    private Integer stockPartyId;

    @Column(name = "SupplierReference")
    private String supplierReference;

    @Column(name = "ReferenceDocNo")
    private String referenceDocNo;

    @Column(name = "PartyWeight")
    private Double partyWeight;

    @Column(name = "FactoryWeight")
    private Double factoryWeight;

    @Column(name = "VehicleType")
    private String vehicleType;

    @Column(name = "VehicleNo")
    private String vehicleNo;

    @Column(name = "BiltyNo")
    private String biltyNo;

    @Column(name = "TransporterDocRef")
    private String transporterDocRef;

    @Column(name = "TransporterId")
    private Integer transporterId;

    @Column(name = "CarriageAmount")
    private Double carriageAmount;

    @Column(name = "OtherCharges")
    private Double otherCharges;

    @Column(name = "RemarksHeader")
    private String remarksHeader;

    @Column(name = "IsApproved")
    private Boolean isApproved;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "PostDate")
    private LocalDateTime postDate;

    @Column(name = "PostUser")
    private Integer postUser;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "ProjectsId")
    private Integer projectsId;

    @Column(name = "InwardGatePassId")
    private Integer inwardGatePassId;

    @Column(name = "ActionId")
    private Integer actionId;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

    @Column(name = "DeliveryTerm")
    private String deliveryTerm;

    @Column(name = "Status")
    private String status;

    @Column(name = "ScaleKart")
    private Double scaleKart;

    @Column(name = "GhallaMandiId")
    private Integer ghallaMandiId;

    @Column(name = "BillCalculateTypeId")
    private Integer billCalculateTypeId;

    @Column(name = "TransporterSupCustId")
    private Integer transporterSupCustId;

    @Column(name = "BillWeightExceedsStockWeight")
    private Integer billWeightExceedsStockWeight;

    @Column(name = "GrnTypeId")
    private Integer grnTypeId;

    @Column(name = "ReqestedById")
    private Integer reqestedById;

    @Column(name = "ReturnableDate")
    private LocalDate returnableDate;

    @Column(name = "TransType")
    private String transType;

    @Column(name = "ScreenName")
    private String screenName;

    @Column(name = "AddWages")
    private Boolean addWages;

    @Column(name = "ScaleShortWeightApply")
    private Boolean scaleShortWeightApply;

    @Column(name = "SupplierShortWeightApply")
    private Boolean supplierShortWeightApply;

    @Column(name = "RateCut")
    private Double rateCut;

    @Column(name = "RateCutRemarks")
    private String rateCutRemarks;

    @Column(name = "AccessWeight")
    private Double accessWeight;

    @Column(name = "AttachmentsValues")
    private String attachmentsValues;

    @Column(name = "CustomAttachmentsValues")
    private String customAttachmentsValues;

    @Column(name = "DeductionPolicyForGrnId")
    private Integer deductionPolicyForGrnId;

    @Column(name = "SupplierDispatchId")
    private Integer supplierDispatchId;

    @Column(name = "FreightDeduction")
    private Double freightDeduction;

    @Column(name = "BiltyFreight")
    private Double biltyFreight;

    @Column(name = "AdvanceByFactoryFreight")
    private Double advanceByFactoryFreight;

    @Column(name = "AdvanceByPartyFreight")
    private Double advanceByPartyFreight;

    @Column(name = "BaseDocumentTypeId")
    private Integer baseDocumentTypeId;

    @Column(name = "GainLossWeight")
    private BigDecimal gainLossWeight;

    @Column(name = "UserLogId")
    private Long userLogId;

    /** Set by Sp_InvGRN_GetAllMethod @Activity='InvGrnApprove'. */
    @Column(name = "ApprovalRemarks")
    private String approvalRemarks;

    /** Set by Sp_InvGRN_GetAllMethod @Activity='InvGrnApprove' (@ReqType='UP'). */
    @Column(name = "UnApprovedUserId")
    private Integer unApprovedUserId;

    /** Set by Sp_InvGRN_GetAllMethod @Activity='InvGrnApprove' (@ReqType='UP'). */
    @Column(name = "UnApprovedDate")
    private LocalDateTime unApprovedDate;

    /** From registry (not touched by this batch's procedures). */
    @Column(name = "IsOnHold")
    private Boolean isOnHold;

    /** From registry (not touched by this batch's procedures). */
    @Column(name = "OutwardGatePassId")
    private Integer outwardGatePassId;

    /** From registry (not touched by this batch's procedures). */
    @Column(name = "DeliveryChallanHeaderId")
    private Integer deliveryChallanHeaderId;

    /** From registry (not touched by this batch's procedures). */
    @Column(name = "PurchasePreBillHeaderId")
    private Integer purchasePreBillHeaderId;
}
