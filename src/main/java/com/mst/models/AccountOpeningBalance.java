package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the desktop's Architecture.Model.Accounts.AccountsOpeningBalances - one row
 * per account carrying its opening Debit or Credit balance (desktop screen:
 * AcfrmOpeningBalance.cs, "Save All" grid). Mapped 1:1 onto the real dbo.AccountsOpeningBalances
 * table (confirmed via goldenAce5_25t.sql).
 *
 * This replaces an earlier pass of this port that had bolted a simplified
 * openingDebit/openingCredit stand-in directly onto {@link ChartofAccount} because the
 * real table hadn't been located yet - see AccountOpeningBalanceService for the actual
 * desktop-matching persistence now that it has been.
 *
 * [Id] has NO IDENTITY clause in the real DDL - app-assigned (max-id+1), same convention
 * as ChartofAccount and every other non-identity master table in this port.
 *
 * chartOfAccountId/accountCode/chartOfAccountTitle are deliberately plain scalars, not a
 * @ManyToOne to ChartofAccount: the real table denormalizes the code/title onto the row
 * itself (so the desktop grid doesn't need a join to display them), and this port follows
 * the same shape rather than inventing a relation the desktop model doesn't have.
 */
@Entity
@Table(name = "AccountsOpeningBalances")
@Data
public class AccountOpeningBalance {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ChartOfAccountId")
    private Integer chartOfAccountId;

    @Column(name = "AccountCode", length = 50)
    private String accountCode;

    @Column(name = "ChartOfAccountTitle", length = 100)
    private String chartOfAccountTitle;

    @Column(name = "YearObDebit")
    private Double yearObDebit = 0d;

    @Column(name = "YearObCredit")
    private Double yearObCredit = 0d;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

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
    private Boolean postState = false;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    // Explicit Getters and Setters
    public Integer getId() { return id; }
    public void setId(Integer id) { this.id = id; }
    public Integer getChartOfAccountId() { return chartOfAccountId; }
    public void setChartOfAccountId(Integer chartOfAccountId) { this.chartOfAccountId = chartOfAccountId; }
    public String getAccountCode() { return accountCode; }
    public void setAccountCode(String accountCode) { this.accountCode = accountCode; }
    public String getChartOfAccountTitle() { return chartOfAccountTitle; }
    public void setChartOfAccountTitle(String chartOfAccountTitle) { this.chartOfAccountTitle = chartOfAccountTitle; }
    public Double getYearObDebit() { return yearObDebit; }
    public void setYearObDebit(Double yearObDebit) { this.yearObDebit = yearObDebit; }
    public Double getYearObCredit() { return yearObCredit; }
    public void setYearObCredit(Double yearObCredit) { this.yearObCredit = yearObCredit; }
    public Integer getFinancialYearId() { return financialYearId; }
    public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }
    public LocalDateTime getEntryDate() { return entryDate; }
    public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
    public Integer getEntryUser() { return entryUser; }
    public void setEntryUser(Integer entryUser) { this.entryUser = entryUser; }
    public LocalDateTime getModifyDate() { return modifyDate; }
    public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
    public Integer getModifyUser() { return modifyUser; }
    public void setModifyUser(Integer modifyUser) { this.modifyUser = modifyUser; }
    public LocalDateTime getPostDate() { return postDate; }
    public void setPostDate(LocalDateTime postDate) { this.postDate = postDate; }
    public Integer getPostUser() { return postUser; }
    public void setPostUser(Integer postUser) { this.postUser = postUser; }
    public Boolean getPostState() { return postState; }
    public void setPostState(Boolean postState) { this.postState = postState; }
    public Integer getOrganizationId() { return organizationId; }
    public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
    public Integer getCompanyId() { return companyId; }
    public void setCompanyId(Integer companyId) { this.companyId = companyId; }
    public Integer getBranchesId() { return branchesId; }
    public void setBranchesId(Integer branchesId) { this.branchesId = branchesId; }
}
