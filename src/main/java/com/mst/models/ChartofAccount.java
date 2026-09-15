package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.persistence.UniqueConstraint;

import lombok.Data;

/**
 * Ditto of the desktop app's Architecture.Model.Accounts.ChartofAccount - one flat,
 * self-referencing (by parentAccountCode) table that backs every Accounts screen
 * (Chart Of Account Definition, Account Allocation, Vouchers, Reports, ...).
 *
 * Mapped 1:1 onto the REAL dbo.ChartofAccount table (confirmed via goldenAce5_25t.sql's
 * own CREATE TABLE script) - every field below carries an explicit @Column(name=...)
 * because the real column names are plain concatenated PascalCase (e.g. AccountTypeId,
 * ParentAccountCode) except Account_Level, which keeps its one underscore; letting
 * Hibernate's ImprovedNamingStrategy infer names from the camelCase Java fields would
 * produce underscored names (account_type_id) that don't exist in the real table, and
 * with spring.jpa.hibernate.ddl-auto=none that's a hard "invalid column name" at runtime,
 * not a schema Hibernate can paper over.
 *
 * [Id] has NO IDENTITY clause in the real DDL - like most desktop master tables, the id
 * is app-assigned (see ChartofAccountService's max-id+1 on insert), not DB-generated.
 *
 * Left out on purpose (desktop-side "virtual" properties that are either UI-only
 * display concatenations or computed rollups, never actual persisted columns):
 * ChartofAccountsList / COAAllocationList (navigation collections - use the
 * repository's findByParentAccountCode instead), AccountDisplay / ParentAccountCodeTitle /
 * ActionId / PhoneNumber / SearchDetailAccount / SearchGroupAccount (UI-only on the
 * desktop grid). Opening-balance figures (YearObDebit/YearObCredit) are NOT columns on
 * this table - the real desktop keeps them on a separate dbo.AccountsOpeningBalances
 * table (see {@link AccountOpeningBalance}); an earlier pass of this port had bolted a
 * simplified openingDebit/openingCredit stand-in directly onto this entity, which has
 * been removed now that the real table is known.
 */
@Entity
@Table(name = "ChartofAccount", uniqueConstraints = @UniqueConstraint(columnNames = "AccountCode"))
@Data
public class ChartofAccount {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "AccountCode", nullable = false, length = 30)
    private String accountCode;

    /** "0" for a top-level (root) account - matches the desktop BLL's Save() default. */
    @Column(name = "ParentAccountCode", length = 30)
    private String parentAccountCode = "0";
    /** Numeric id of the parent row (desktop: ParentCodeId), alongside parentAccountCode. */
    @Column(name = "ParentCodeId")
    private Integer parentCodeId;


    /** "Group" or "Detail" - matches the desktop's AccountGroup values. */
    @Column(name = "AccountGroup", length = 20)
    private String accountGroup = "Group";

    @Column(name = "AccountTitle", nullable = false, length = 250)
    private String accountTitle;

    /** Desktop: AccountTitleOtherLingo (secondary-language title, e.g. Urdu). */
    @Column(name = "AccountTitleOtherLingo", length = 250)
    private String accountTitleOtherLingo;

    /** Desktop: Account_Level (1 = root ... 4 = normal Detail depth, but not hard-capped). */
    @Column(name = "Account_Level")
    private Integer accountLevel = 1;

    /**
     * Desktop: AccountClass - confirmed from goldenAce5_25t.sql's own AccountClassName
     * CASE expression: 1=Capital, 2=Assets, 3=Liability, 4=Expense, 5=Revenue.
     */
    @Column(name = "AccountClass")
    private Integer accountClass;

    @Column(name = "AccountTypeId")
    private Integer accountTypeId;

    /** Balance-sheet note reference (desktop: BSNoteId). */
    @Column(name = "BSNoteId")
    private Integer bsNoteId;

    /** Profit-and-loss note reference (desktop: PLNoteId). */
    @Column(name = "PLNoteId")
    private Integer plNoteId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

    @Column(name = "BranchId")
    private Integer branchId;

    @Column(name = "CustomerGroupId")
    private Integer customerGroupId;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "CurrencyId")
    private Integer currencyId;

    @Column(name = "OtherErpCode", length = 60)
    private String otherErpCode;

    @Column(name = "ContactNo", length = 30)
    private String contactNo;

    @Column(name = "QrCode", length = 250)
    private String qrCode;

    /** Real column, not previously mapped: subsidiary-account status flag. */
    @Column(name = "SubSidiaryAcStatus")
    private Integer subSidiaryAcStatus;

    /** Real columns, not previously mapped: the account's own code at each rollup level. */
    @Column(name = "Lvl01Id")
    private Integer lvl01Id;

    @Column(name = "Lvl02Id")
    private Integer lvl02Id;

    @Column(name = "Lvl03Id")
    private Integer lvl03Id;

    @Column(name = "Lvl04Id")
    private Integer lvl04Id;

    @Column(name = "IsActive")
    private Boolean isActive = true;

    @Column(name = "PostState")
    private Boolean postState = false;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "PostDate")
    private LocalDateTime postDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "PostUser")
    private Integer postUser;

    // Explicit Getters and Setters
    public Integer getId() { return id; }
    public void setId(Integer id) { this.id = id; }
    public String getAccountCode() { return accountCode; }
    public void setAccountCode(String accountCode) { this.accountCode = accountCode; }
    public String getParentAccountCode() { return parentAccountCode; }
    public void setParentAccountCode(String parentAccountCode) { this.parentAccountCode = parentAccountCode; }
    public Integer getParentCodeId() { return parentCodeId; }
    public void setParentCodeId(Integer parentCodeId) { this.parentCodeId = parentCodeId; }
    public String getAccountGroup() { return accountGroup; }
    public void setAccountGroup(String accountGroup) { this.accountGroup = accountGroup; }
    public String getAccountTitle() { return accountTitle; }
    public void setAccountTitle(String accountTitle) { this.accountTitle = accountTitle; }
    public String getAccountTitleOtherLingo() { return accountTitleOtherLingo; }
    public void setAccountTitleOtherLingo(String accountTitleOtherLingo) { this.accountTitleOtherLingo = accountTitleOtherLingo; }
    public Integer getAccountLevel() { return accountLevel; }
    public void setAccountLevel(Integer accountLevel) { this.accountLevel = accountLevel; }
    public Integer getAccountClass() { return accountClass; }
    public void setAccountClass(Integer accountClass) { this.accountClass = accountClass; }
    public Integer getAccountTypeId() { return accountTypeId; }
    public void setAccountTypeId(Integer accountTypeId) { this.accountTypeId = accountTypeId; }
    public Integer getBsNoteId() { return bsNoteId; }
    public void setBsNoteId(Integer bsNoteId) { this.bsNoteId = bsNoteId; }
    public Integer getPlNoteId() { return plNoteId; }
    public void setPlNoteId(Integer plNoteId) { this.plNoteId = plNoteId; }
    public Integer getCompanyId() { return companyId; }
    public void setCompanyId(Integer companyId) { this.companyId = companyId; }
    public Integer getOrganizationId() { return organizationId; }
    public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
    public Integer getFinancialYearId() { return financialYearId; }
    public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }
    public Integer getBranchId() { return branchId; }
    public void setBranchId(Integer branchId) { this.branchId = branchId; }
    public Integer getCustomerGroupId() { return customerGroupId; }
    public void setCustomerGroupId(Integer customerGroupId) { this.customerGroupId = customerGroupId; }
    public Integer getCityId() { return cityId; }
    public void setCityId(Integer cityId) { this.cityId = cityId; }
    public Integer getCurrencyId() { return currencyId; }
    public void setCurrencyId(Integer currencyId) { this.currencyId = currencyId; }
    public String getOtherErpCode() { return otherErpCode; }
    public void setOtherErpCode(String otherErpCode) { this.otherErpCode = otherErpCode; }
    public String getContactNo() { return contactNo; }
    public void setContactNo(String contactNo) { this.contactNo = contactNo; }
    public String getQrCode() { return qrCode; }
    public void setQrCode(String qrCode) { this.qrCode = qrCode; }
    public Integer getSubSidiaryAcStatus() { return subSidiaryAcStatus; }
    public void setSubSidiaryAcStatus(Integer subSidiaryAcStatus) { this.subSidiaryAcStatus = subSidiaryAcStatus; }
    public Integer getLvl01Id() { return lvl01Id; }
    public void setLvl01Id(Integer lvl01Id) { this.lvl01Id = lvl01Id; }
    public Integer getLvl02Id() { return lvl02Id; }
    public void setLvl02Id(Integer lvl02Id) { this.lvl02Id = lvl02Id; }
    public Integer getLvl03Id() { return lvl03Id; }
    public void setLvl03Id(Integer lvl03Id) { this.lvl03Id = lvl03Id; }
    public Integer getLvl04Id() { return lvl04Id; }
    public void setLvl04Id(Integer lvl04Id) { this.lvl04Id = lvl04Id; }
    public Boolean getIsActive() { return isActive; }
    public void setIsActive(Boolean isActive) { this.isActive = isActive; }
    public Boolean getPostState() { return postState; }
    public void setPostState(Boolean postState) { this.postState = postState; }
    public LocalDateTime getEntryDate() { return entryDate; }
    public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
    public LocalDateTime getModifyDate() { return modifyDate; }
    public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
    public LocalDateTime getPostDate() { return postDate; }
    public void setPostDate(LocalDateTime postDate) { this.postDate = postDate; }
    public Integer getEntryUser() { return entryUser; }
    public void setEntryUser(Integer entryUser) { this.entryUser = entryUser; }
    public Integer getModifyUser() { return modifyUser; }
    public void setModifyUser(Integer modifyUser) { this.modifyUser = modifyUser; }
    public Integer getPostUser() { return postUser; }
    public void setPostUser(Integer postUser) { this.postUser = postUser; }
}

