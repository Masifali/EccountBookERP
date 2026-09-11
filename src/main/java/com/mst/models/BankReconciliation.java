package com.mst.models;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

@Entity
@Table(name = "BankReconciliation")
@Data
public class BankReconciliation {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "DocumentTypeId", nullable = false)
	private Integer documentTypeId = 920;

	@Column(name = "VoucherHeadId")
	private Integer voucherHeadId;

	@Column(name = "BankAccountId")
	private Integer bankAccountId;

	@Column(name = "TransactionDate")
	private LocalDate transactionDate;

	@Column(name = "ChequeNo", length = 50)
	private String chequeNo;

	@Column(name = "Particulars")
	private String particulars;

	@Column(name = "Debit", precision = 18, scale = 2)
	private BigDecimal debit;

	@Column(name = "Credit", precision = 18, scale = 2)
	private BigDecimal credit;

	@Column(name = "IsReconciled")
	private Boolean isReconciled = false;

	@Column(name = "Remarks")
	private String remarks;

	@Column(name = "OrganizationId", nullable = false)
	private Integer organizationId;

	@Column(name = "CompanyId", nullable = false)
	private Integer companyId;

	@Column(name = "BranchesId", nullable = false)
	private Integer branchesId;

	@Column(name = "ProjectsId", nullable = false)
	private Integer projectsId;

	@Column(name = "FinancialYearId", nullable = false)
	private Integer financialYearId;

	@Column(name = "EntryUserId", nullable = false)
	private Integer entryUserId;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "ModifyUserId")
	private Integer modifyUserId;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "IsApproved")
	private Boolean isApproved;

	@Column(name = "ApprovedUserId")
	private Integer approvedUserId;

	@Column(name = "ApprovedDate")
	private LocalDateTime approvedDate;

	@Column(name = "ActionId")
	private Integer actionId;

	@Column(name = "RevisionNo")
	private Integer revisionNo;

	@Column(name = "ReconcileDate")
	private LocalDateTime reconcileDate;

	@Column(name = "ReconcileUserId")
	private Integer reconcileUserId;

	@Column(name = "DiffAmount")
	private Double diffAmount;

	@Column(name = "VoucherDetailId")
	private Integer voucherDetailId;

	public BankReconciliation() {
	}
}
