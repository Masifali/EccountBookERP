package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/** Generated from 0123_Architecture.Model.Service.ExImClearingAgentBill.cs - every non-virtual property, as GenericProvider.SetProc binds it. */
public class LgsAClearingAgentBill extends DesktopModel {
    public boolean IsApproved;
    public LocalDateTime DocDate;
    public LocalDateTime DueDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public double GrandTotal;
    public double NetBillWeight;
    public double OtherChargesTotal;
    public double TaxAmountTotal;
    public double TotalAmount;
    public double USDRate;
    public java.math.BigDecimal BookingRate = java.math.BigDecimal.ZERO;
    public int BranchesId;
    public int CompanyId;
    public int DestinationPortId;
    public int DocNo;
    public int DocTypeId;
    public int DueDays;
    public int EntryUser;
    public int ExportInvoiceId;
    public int ForwarderId;
    public int FinancialYearId;
    public int Id;
    public int LcContractId;
    public int LoadingPortId;
    public int ModifyUser;
    public int NoOfContainer;
    public int OrganizationId;
    public int ProjectId;
    public int ServiceOrderId;
    public int SupCustId;
    public String ContainerNo;
    public String RefBillNo;
    public String CustomAttachmentsValues;
    public String AttachmentsValues;
}
