package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Architecture.Model.Accounts.VoucherDetail - every NON-virtual property, spelled as the C# property, as GenericProvider.SetProc
 * binds it to dbo.Sp_VoucherDetail_Insert / _H_Insert. Virtual properties (AccountCode, BranchName, Location, CostCenterName, AccountTitle, AgainstAccount, CheqPartyName, JobLotDescription, SubsidiaryAccountTitle, InstrumentType, ReferenceAccount, AccountTypeId, BaseFcyCode, GlcyCode, TcyCode) are not
 * sent and are not declared here.
 */
public class LgsBVoucherDetail extends DesktopModel {
    public LocalDateTime DCheqDate;
    public LocalDateTime GpDate;
    public double Adjustment;
    public double AdvanceAmount;
    public double Commission;
    public double CreditAmount;
    public double DCurrencyAmount;
    public double DebitAmount;
    public double TaxAmount;
    public double DExchangeCurrencyRate;
    public double Expenses;
    public double ExTax;
    public double Freight;
    public double ItemAmount;
    public double ItemRate;
    public double Journal;
    public double QtyIn;
    public double QtyOut;
    public double RateCut;
    public double RateCutAmount;
    public double SaleTax;
    public double TaxesTotalAmount;
    public double TaxPrcnt;
    public double WeightIn;
    public double WeightOut;
    public double WhtHolding;
    public double ItemCgsRate;
    public double TotalDebitAmount;
    public double TotalCreditAmount;
    public double ThirdCurrencyFcyExchangeRate;
    public double ThirdCurrencyHcyExchangeRate;
    public double ThirdCurrencyAmount;
    public double ThirdCurrencyReceiverExchangeRate;
    public double ThirdCurrencyReceiverFcyAmount;
    public int ThirdCurrencyId;
    public int AccountId;
    public int AgainstAccountId;
    public int DMultiCurrencyId;
    public int DocumentTypeIdRef;
    public int GpNo;
    public int Id;
    public int InvoiceNoRefId;
    public int ItemId;
    public int JobLotId;
    public int OrderNo;
    public int SupplierCustomerId;
    public int EmployeeId;
    public int SubsidiaryTypeId;
    public int SubsidiaryAccountId;
    public int SubsidiaryAgainstTypeId;
    public int SubsidiaryAgainstAccountId;
    public int TaxTypeId;
    public int ActionId;
    public int VoucherHeadId;
    public int RefDocumentTypeId;
    public int RefDocNoId;
    public int RefDocNoDetailId;
    public int RefDocSubIdNo;
    public int LineId;
    public int InstrumentTypeId;
    public int SubNo;
    public int SortNo;
    public int IsCGS;
    public String CheqNoDetail;
    public String Comments;
    public String CommentsOtherLingo;
    public String DConversionFormula;
    public String IsTaxable;
    public String PaymentType;
    public String RefInvoiceNo;
    public String TaxesRemarks;
    public String VehicleNo;
    public String PayeeTitle;
    public int PaymentTypeId;
    public int ChequeTypeId;
    public int BranchesId;
    public int CostCenterId;
    public double SBRTaxAmount;
    public double DiscountPercent;
    public double DiscountAmount;
    public int ReferenceAccountId;
    public int LocationTypeId;
    public int BaseFcyId;
    public double BaseFcyExchangeRate;
    public double BaseFcyAmount;
}
