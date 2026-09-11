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
}
