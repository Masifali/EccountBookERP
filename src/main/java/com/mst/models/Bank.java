package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real dbo.Bank table (confirmed via goldenAce5_25t.sql) - the desktop's
 * "Define Bank" master (Architecture.WinApp.AcfrmDefineBank, real
 * ScreenDefinition.Id=709, Banking Managment module). Each row is one bank branch
 * account, linked to its own GL ledger account via ChartOfAccountId.
 *
 * Real [Id] has NO IDENTITY clause - app-assigned (confirmed via the real DDL).
 */
@Entity
@Table(name = "Bank")
@Data
public class Bank {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "BranchCode", length = 50)
	private String branchCode;

	@Column(name = "BranchName", length = 50)
	private String branchName;

	@Column(name = "BranchAddress")
	private String branchAddress;

	@Column(name = "BranchCity")
	private Integer branchCity;

	@Column(name = "CountryId")
	private Integer countryId;

	@Column(name = "Contact1Tel", length = 50)
	private String contact1Tel;

	@Column(name = "Contact2Tel", length = 50)
	private String contact2Tel;

	@Column(name = "Contact3Mobile", length = 50)
	private String contact3Mobile;

	@Column(name = "emailPrimery", length = 50)
	private String emailPrimery;

	@Column(name = "emailAlternate", length = 50)
	private String emailAlternate;

	@Column(name = "IsHomeland", length = 50)
	private String isHomeland;

	@Column(name = "OtherInfo")
	private String otherInfo;

	/** The bank account's own GL ledger account (real ChartofAccount.Id). */
	@Column(name = "ChartOfAccountId")
	private Integer chartOfAccountId;

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

	@Column(name = "ChequeTemplete", length = 50)
	private String chequeTemplete;

	@Column(name = "BankAccountNo", length = 150)
	private String bankAccountNo;

	@Column(name = "BankIBANNo", length = 150)
	private String bankIBANNo;

	@Column(name = "BankAccountTitle")
	private String bankAccountTitle;

	// Explicit Getters and Setters
	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public String getBranchCode() { return branchCode; }
	public void setBranchCode(String branchCode) { this.branchCode = branchCode; }
	public String getBranchName() { return branchName; }
	public void setBranchName(String branchName) { this.branchName = branchName; }
	public Integer getChartOfAccountId() { return chartOfAccountId; }
	public void setChartOfAccountId(Integer chartOfAccountId) { this.chartOfAccountId = chartOfAccountId; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public Integer getEntryUser() { return entryUser; }
	public void setEntryUser(Integer entryUser) { this.entryUser = entryUser; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
	public Integer getModifyUser() { return modifyUser; }
	public void setModifyUser(Integer modifyUser) { this.modifyUser = modifyUser; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public String getBankAccountNo() { return bankAccountNo; }
	public void setBankAccountNo(String bankAccountNo) { this.bankAccountNo = bankAccountNo; }
	public String getBankAccountTitle() { return bankAccountTitle; }
	public void setBankAccountTitle(String bankAccountTitle) { this.bankAccountTitle = bankAccountTitle; }
}
