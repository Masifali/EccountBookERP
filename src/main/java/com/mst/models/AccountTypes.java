package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the desktop's real dbo.AccountTypes table (Architecture.Model.Accounts.AccountTypes,
 * read via Architecture.BLL.Accounts.AccountTypes::GetAll / Proc_AccountTypes_ReadAll) - the
 * actual master list ChartofAccount.AccountTypeId is a real FK into (bound as key=Id,
 * value=AccountType by both AcfrmDefCoa's cmbactype and CmbAccountTypeFilter, per
 * AcfrmDefCoa::CmbAccountType). Confirmed directly against the production GoldenAcedb
 * schema+data dump (Gscript.sql): Id=2 is AccountType='Cash Equivalent', Abbreviation='Cash' -
 * exactly the value the Chart Of Account Definition screen shows for a "CASH AND CASH
 * EQUIVALENT" parent. This is NOT the same table as AcLookUp (an unrelated generic lookup
 * table) - an earlier pass of this port had wired the Account Type dropdowns to AcLookUp
 * (AcLookUpTypesId=1) by mistake, which never matches the real desktop's values.
 *
 * [Id] has no IDENTITY clause in the real DDL (app-assigned), matching every other master
 * table in this port.
 */
@Entity
@Table(name = "AccountTypes")
@Data
public class AccountTypes {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "AccountType", length = 50)
	private String accountType;

	@Column(name = "Abbreviation", length = 50)
	private String abbreviation;

	@Column(name = "Description", length = 500)
	private String description;

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
}
