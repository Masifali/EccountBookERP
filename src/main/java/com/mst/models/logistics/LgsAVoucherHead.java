package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/** Generated from 1195_Architecture.Model.Accounts.VoucherHead.cs - every non-virtual property, as GenericProvider.SetProc binds it. */
public class LgsAVoucherHead extends DesktopModel {
    public boolean IncludeWHT;
    public boolean IsApproved;
    public boolean PostState;
    public Boolean InclusiveTax;
    public LocalDateTime ChequeDate;
    public LocalDateTime DueDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public LocalDateTime PostDate;
    public LocalDateTime VoucherDate;
    public double BillAmount;
    public double ExchangeCurrencyRate;
    public double FcAmount;
    public double VoucherAmount;
    public double CostCenterAmount;
    public int AgainstAccountId;
    public int BranchId;
    public int CheqId;
    public int ChequePrintId;
    public int CompanyId;
    public int DocumentTypeId;
    public int DocumentTypeSrNo;
    public int DueDays;
    public int EntryUser;
    public int FinancialYearId;
    public int Id;
    public int ModifyUser;
    public int MultiCurrencyId;
    public int OrganizationId;
    public int PostUser;
    public int ProjectId;
    public int RefAccountId;
    public int RefDocNoId;
    public int VoucherCode;
    public int ActionId;
    public String BankBranch;
    public String ChequeNo;
    public String ConversionFormula;
    public String DrCrNoteType;
    public String ManualBillNo;
    public String PayTitle;
    public String Remarks;
    public String RemarksOtherLingo;
    public String Source;
    public String AttachmentsValues;
    public int RefDocumentTypeId;
    public int FixedAssetEntryTypeId;
    public String CustomAttachmentsValues;
    public boolean CustomAccounts;
    public int BaseDocumentTypeId;
    public int AdvanceTaxAccountId;
    public double AdvanceTaxAmount;
    public int OtherChargesAccountId;
    public double OtherChargesAmount;
    public boolean IsUploaded;
}
