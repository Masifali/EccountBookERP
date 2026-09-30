package com.mst.models.hrm.payroll;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

/**
 * Architecture.Model.Accounts.VoucherHead (model 1195) - the voucher BLL Payroll.MakeVoucher builds
 * and DAL Payroll.SetData saves through Sp_VoucherHead_Insert / _Update / _H_Insert. Every non-virtual
 * property, as SetProc sends it (each one is declared by those procedures; checked). Nullable C#
 * types are boxed so an unset value is omitted, as AddWithValue(null) omits it. The virtual lists are
 * transient; the constructor's new List&lt;VoucherDetail&gt;() is kept.
 */
public class VoucherHead extends DesktopModel {
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

    public transient List<VoucherDetail> voucherDetailList = new ArrayList<>();
}
