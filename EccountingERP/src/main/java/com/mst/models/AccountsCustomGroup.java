package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real dbo.AccountsCustomGroups table (confirmed via goldenAce5_25t.sql,
 * both its CREATE TABLE script and real data rows) - the many-to-many junction
 * between {@link ChartofAccount} and {@link AcLookUp} (type=Accounts Custom Group)
 * that backs the Chart Of Account Definition screen's "Custom Group" field. One row
 * per (account, group) assignment - the real table's own primary key is SortNo, not
 * a composite of the two FKs, so (per the real schema) an account can carry more
 * than one custom-group assignment; this port exposes it as a checkbox mini-grid,
 * the same pattern already used for "Account Allocation To Locations" ({@link
 * COAAllocation}).
 *
 * Column names/casing match the real DDL exactly, including its inconsistencies
 * with sibling tables: ChartOfAccountId (capital O in "Of", unlike the ChartofAccount
 * table name itself), EntryUserId/ModifyUserId (not EntryUser/ModifyUser as on
 * ChartofAccount), and organizationId/companyId (lower camelCase, not the
 * OrganizationId/CompanyId used almost everywhere else in this database).
 *
 * Real [SortNo] (the primary key) has NO IDENTITY clause - app-assigned, matching the
 * real desktop's own "SELECT ISNULL(MAX(CONVERT(int,SortNo)),0) + 1" pattern (see the
 * real Sp_AccountsCustomGroups_Insert-style stored procedure text in the dump).
 */
@Entity
@Table(name = "AccountsCustomGroups")
@Data
public class AccountsCustomGroup {

	@Id
	@Column(name = "SortNo")
	private Integer sortNo;

	@Column(name = "AcLookUpsId", nullable = false)
	private Integer acLookUpsId;

	@Column(name = "ChartOfAccountId", nullable = false)
	private Integer chartOfAccountId;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "EntryUserId")
	private Integer entryUserId;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "ModifyUserId")
	private Integer modifyUserId;

	@Column(name = "organizationId")
	private Integer organizationId;

	@Column(name = "companyId")
	private Integer companyId;
}
