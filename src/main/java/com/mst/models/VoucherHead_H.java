package com.mst.models;

import java.io.Serializable;
import java.time.LocalDate;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.VoucherHead_H, the history copy of VoucherHead (source: Sp_VoucherHead_H_Insert). Id is an IDENTITY column (proc returns SCOPE_IDENTITY()). */
@Entity
@Setter
@Getter
@Table(name = "VoucherHead_H")
public class VoucherHead_H implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "DocumentTypeSrNo")
    private Integer documentTypeSrNo;

    @Column(name = "RefDocNoId")
    private Integer refDocNoId;

    @Column(name = "VoucherCode")
    private Integer voucherCode;

    @Column(name = "VoucherDate")
    private LocalDate voucherDate;

    @Column(name = "Remarks")
    private String remarks;

    @Column(name = "RemarksOtherLingo")
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

    @Column(name = "ConversionFormula")
    private String conversionFormula;

    @Column(name = "ExchangeCurrencyRate")
    private Double exchangeCurrencyRate;

    @Column(name = "FcAmount")
    private Double fcAmount;

    @Column(name = "CheqId")
    private Integer cheqId;

    @Column(name = "ChequeNo")
    private String chequeNo;

    @Column(name = "ChequeDate")
    private LocalDateTime chequeDate;

    @Column(name = "PayTitle")
    private String payTitle;

    @Column(name = "BankBranch")
    private String bankBranch;

    @Column(name = "ChequePrintId")
    private Integer chequePrintId;

    @Column(name = "Source")
    private String source;

    @Column(name = "DrCrNoteType")
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

    @Column(name = "ManualBillNo")
    private String manualBillNo;

    @Column(name = "BillAmount")
    private Double billAmount;

    @Column(name = "DueDate")
    private LocalDate dueDate;

    @Column(name = "DueDays")
    private Integer dueDays;

    @Column(name = "ActionId")
    private Integer actionId;

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

}
