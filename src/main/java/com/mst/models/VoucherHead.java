package com.mst.models;

import java.time.LocalDate;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.VoucherHead table - the single shared header
 * table behind EVERY voucher screen in the desktop app (Cash/Bank Payment, Cash/Bank
 * Receipt, Journal, Contra, Expense, Party Payment/Receipt, and every Purchase/Sale/
 * Production voucher too), discriminated by DocumentTypeId (see DocumentType.java).
 * Id IS a SQL Server IDENTITY column here (unlike Item/ItemCategory/Brand/...).
 *
 * Only DocumentTypeId is wired as a JPA relation - RefAccountId/AgainstAccountId/
 * CheqId/BaseDocumentTypeId/etc. are kept as plain Integer: the real table has no
 * FK constraints on them, and most only matter to voucher types not built yet.
 */

@Entity
@Table(name = "VoucherHead")
@Data
public class VoucherHead {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "Id")
	private Integer id;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "DocumentTypeId", nullable = false)
	private DocumentType documentType;

	@Column(name = "DocumentTypeSrNo")
	private Integer documentTypeSrNo;

	@Column(name = "RefDocNoId")
	private Integer refDocNoId;

	@Column(name = "VoucherCode")
	private Integer voucherCode;

	@Column(name = "VoucherDate")
	private LocalDate voucherDate;

	@Column(name = "Remarks", length = 1000)
	private String remarks;

	@Column(name = "RemarksOtherLingo", length = 1000)
	private String remarksOtherLingo;

	@Column(name = "VoucherAmount")
	private Double voucherAmount;

	@Column(name = "FinancialYearId")
	private Integer financialYearId;

	@Column(name = "RefAccountId")
	private Integer refAccountId;

	@Column(name = "AgainstAccountId")
	private Integer againstAccountId;

	@Column(name = "MultiCurrencyId")
	private Integer multiCurrencyId;

	@Column(name = "ConversionFormula", length = 50)
	private String conversionFormula;

	@Column(name = "ExchangeCurrencyRate")
	private Double exchangeCurrencyRate;

	@Column(name = "FcAmount")
	private Double fcAmount;

	@Column(name = "CheqId")
	private Integer cheqId;

	@Column(name = "ChequeNo", length = 20)
	private String chequeNo;

	@Column(name = "ChequeDate")
	private LocalDateTime chequeDate;

	@Column(name = "PayTitle", length = 100)
	private String payTitle;

	@Column(name = "BankBranch", length = 300)
	private String bankBranch;

	@Column(name = "ChequePrintId")
	private Integer chequePrintId;

	@Column(name = "Source", length = 250)
	private String source;

	@Column(name = "DrCrNoteType", length = 50)
	private String drCrNoteType;

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

	@Column(name = "PostState")
	private Boolean postState;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "IncludeWHT")
	private Boolean includeWHT;

	@Column(name = "BranchId")
	private Integer branchId;

	@Column(name = "ProjectId")
	private Integer projectId;

	@Column(name = "ManualBillNo", length = 50)
	private String manualBillNo;

	@Column(name = "BillAmount")
	private Double billAmount;

	@Column(name = "DueDate")
	private LocalDate dueDate;

	@Column(name = "DueDays")
	private Integer dueDays;

	@Column(name = "ActionId")
	private Integer actionId;

	@Column(name = "ActionTypeId")
	private Boolean actionTypeId;

	@Column(name = "UnApproveDate")
	private LocalDateTime unApproveDate;

	@Column(name = "UnApproveUserId")
	private Integer unApproveUserId;

	@Column(name = "CostCenterAmount")
	private Double costCenterAmount;

	@Column(name = "AttachmentsValues")
	private String attachmentsValues;

	@Column(name = "RefDocumentTypeId")
	private Integer refDocumentTypeId;

	@Column(name = "CustomAttachmentsValues")
	private String customAttachmentsValues;

	@Column(name = "CustomAccounts")
	private Boolean customAccounts;

	@Column(name = "BaseDocumentTypeId")
	private Integer baseDocumentTypeId;

	@Column(name = "AdvanceTaxAccountId")
	private Integer advanceTaxAccountId;

	@Column(name = "AdvanceTaxAmount")
	private Double advanceTaxAmount;

	@Column(name = "OtherChargesAccountId")
	private Integer otherChargesAccountId;

	@Column(name = "OtherChargesAmount")
	private Double otherChargesAmount;

	@Column(name = "IsUploaded")
	private Boolean isUploaded;

	@Column(name = "UploadedDate")
	private LocalDateTime uploadedDate;

	@Column(name = "UploadedById")
	private Integer uploadedById;

	@Column(name = "InclusiveTax")
	private Boolean inclusiveTax;

	@Column(name = "FixedAssetEntryTypeId")
	private Integer fixedAssetEntryTypeId;

}