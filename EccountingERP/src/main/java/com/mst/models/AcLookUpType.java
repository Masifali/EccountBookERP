package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real dbo.AcLookUpTypes table (confirmed via goldenAce5_25t.sql) -
 * the small fixed category list that {@link AcLookUp} rows are partitioned by. Real
 * data confirmed in the dump: Id=1 "Accounts Custom Group", Id=2 "Party Custom
 * Group" (see {@link AcLookUp#ACCOUNTS_CUSTOM_GROUP_TYPE_ID}). Read-only in this
 * port - no screen to add new types has been rebuilt (the real desktop table only
 * has these two rows anyway).
 *
 * Real [Id] has NO IDENTITY clause - app-assigned (confirmed via the real DDL).
 */
@Entity
@Table(name = "AcLookUpTypes")
@Data
public class AcLookUpType {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "LookUpTypesName", nullable = false, length = 150)
	private String lookUpTypesName;
}
