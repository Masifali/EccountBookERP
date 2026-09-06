package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real dbo.AcLookUps table (confirmed via goldenAce5_25t.sql, both its
 * CREATE TABLE script and real data rows) - a generic lookup-value table shared by
 * several Accounts screens, partitioned by {@link #acLookUpTypesId} (see
 * dbo.AcLookUpTypes / {@link AcLookUpType}).
 *
 * The Chart Of Account Definition screen's "Custom Group" field (desktop:
 * Account_Definition/frmAccountCustomGroup.cs, "Define Accounts Custom Group") reads
 * and writes rows here where acLookUpTypesId = {@link #ACCOUNTS_CUSTOM_GROUP_TYPE_ID}
 * (confirmed from real data: dbo.AcLookUpTypes.Id=1 has LookUpTypesName='Accounts
 * Custom Group'; real dbo.AcLookUps sample rows "WHT Purchase"/"WHT Sale" both carry
 * AcLookUpTypesId=1). Type Id=2 ("Party Custom Group") is a different screen, not
 * this one, and is intentionally left out of this port for now.
 *
 * An earlier pass of this port had invented its own simplified, dedicated
 * account_custom_group table (name/status only, no real link to any account) instead
 * of finding this real table - replaced now that the real schema is known, per the
 * project's "real DB-verified structures, not guesses" rule. See
 * {@link com.mst.models.AccountsCustomGroup} for the real account-to-group
 * assignment (many-to-many) table this feeds into.
 *
 * Real [Id] has NO IDENTITY clause - app-assigned like most desktop master tables
 * (confirmed via the real DDL).
 */
@Entity
@Table(name = "AcLookUps")
@Data
public class AcLookUp {

	/** Real dbo.AcLookUpTypes.Id for "Accounts Custom Group" (confirmed via real data row). */
	public static final int ACCOUNTS_CUSTOM_GROUP_TYPE_ID = 1;

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "AcLookUpsDescription", nullable = false, length = 250)
	private String acLookUpsDescription;

	@Column(name = "AcLookUpTypesId", nullable = false)
	private Integer acLookUpTypesId;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;
}
