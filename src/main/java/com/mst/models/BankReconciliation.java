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

	// Explicit Getters and Setters
	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public Integer getVoucherHeadId() { return voucherHeadId; }
	public void setVoucherHeadId(Integer voucherHeadId) { this.voucherHeadId = voucherHeadId; }
	public Integer getBankAccountId() { return bankAccountId; }
	public void setBankAccountId(Integer bankAccountId) { this.bankAccountId = bankAccountId; }
	public Boolean getIsReconciled() { return isReconciled; }
	public void setIsReconciled(Boolean isReconciled) { this.isReconciled = isReconciled; }
	public String getRemarks() { return remarks; }
	public void setRemarks(String remarks) { this.remarks = remarks; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public Integer getBranchesId() { return branchesId; }
	public void setBranchesId(Integer branchesId) { this.branchesId = branchesId; }
	public Integer getProjectsId() { return projectsId; }
	public void setProjectsId(Integer projectsId) { this.projectsId = projectsId; }
	public Integer getFinancialYearId() { return financialYearId; }
	public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }
	public Integer getEntryUserId() { return entryUserId; }
	public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public LocalDateTime getReconcileDate() { return reconcileDate; }
	public void setReconcileDate(LocalDateTime reconcileDate) { this.reconcileDate = reconcileDate; }
	public String getParticulars() { return particulars; }
	public void setParticulars(String particulars) { this.particulars = particulars; }
	public BigDecimal getDebit() { return debit; }
	public void setDebit(BigDecimal debit) { this.debit = debit; }
	public BigDecimal getCredit() { return credit; }
	public void setCredit(BigDecimal credit) { this.credit = credit; }
	public Integer getActionId() { return actionId; }
	public void setActionId(Integer actionId) { this.actionId = actionId; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
	public Integer getModifyUserId() { return modifyUserId; }
	public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
	public Integer getDocumentTypeId() { return documentTypeId; }
	public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }
}
