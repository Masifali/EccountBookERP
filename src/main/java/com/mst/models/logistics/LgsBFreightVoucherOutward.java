package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.lgstcm.FreightVoucherOutward - every NON-virtual property, spelled as the C# property, as GenericProvider.SetProc
 * binds it to [lgstcm].[USP_FreightVoucherOutward_InsertAndUpdate]. Virtual properties (ChargedToDrAccountTitle, ChargedToDrAccountCode, TransporterAccountTitle, TransporterAccountCode, RateUomCode, RateUomEquivalent, BillWeightBase, GpSrNo, GpDate, DeliveryOrderNo, DeliveryOrderDate, UnloadingToCityName, LoadingFromCityName, PurchaseOrderNo, PurchaseOrderDate, FreightVoucherOutwardList, FreightVoucherOutwardPaymentDetailList, FreightVoucherOutwardExpenseDetailList, AttachmentsList, DeleteAttachmentsList, VoucherHeadList) are not
 * sent and are not declared here.
 */
public class LgsBFreightVoucherOutward extends DesktopModel {
    public boolean IsApproved;
    public LocalDateTime ApprovedDate;
    public LocalDateTime DocDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public BigDecimal biltyFreight = BigDecimal.ZERO;
    public BigDecimal freightRate = BigDecimal.ZERO;
    public BigDecimal netWeight = BigDecimal.ZERO;
    public BigDecimal NetWeightDo = BigDecimal.ZERO;
    public BigDecimal NetWeightWB = BigDecimal.ZERO;
    public BigDecimal otherCharges = BigDecimal.ZERO;
    public BigDecimal QtyforRate = BigDecimal.ZERO;
    public BigDecimal totalbiltyfreight = BigDecimal.ZERO;
    public BigDecimal AdvanceOrCashFreight = BigDecimal.ZERO;
    public BigDecimal totalFreight = BigDecimal.ZERO;
    public int ActionId;
    public int ApprovedUserId;
    public int BranchesId;
    public int CompanyId;
    public int DocNo;
    public int documentTypeId;
    public int EntryUserId;
    public int FinancialYearId;
    public int FreightVoucherOutwardId;
    public int GatePassOutwardId;
    public int InvDeliveryOrderId;
    public int chargeToDrAccountId;
    public int loadingFromCityId;
    public int ModifyUserId;
    public BigDecimal NoOfBags = BigDecimal.ZERO;
    public int OrganizationId;
    public int ProjectsId;
    public int PurchaseOrderHeaderId;
    public int rateUomId;
    public int BillWeightBaseId;
    public int TransporterSupCustId;
    public int transporterId;
    public int unloadingToCityId;
    public String ApprovalRemarks;
    public String AttachmentsValues;
    public LocalDateTime biltyDate;
    public String biltyNo;
    public String CustomAttachmentsValues;
    public String RemarksHeader;
    public String vehicleNo;
    public int MasterDocId;
    public int MasterDocNo;
    public LocalDateTime MasterDocDate;
    public String MasterDocRemarks;
}
