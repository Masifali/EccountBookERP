package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real dbo.CustomerGroup table (confirmed via goldenAce5_25t.sql) - the
 * real lookup {@link SupplierCustomer}.customerGroupId points at (desktop sample
 * data includes groups like "Rice Supplier", "Paddy Supplier", "Rice Customer",
 * "Sales Man", "Broker" - confirming this is a rice-mill-specific party grouping,
 * not the generic Accounts "Custom Group" - {@link AcLookUp} - which is a
 * different, unrelated real table despite the similar name).
 *
 * Real [Id] has NO IDENTITY clause - app-assigned (confirmed via the real DDL).
 */
@Entity
@Table(name = "CustomerGroup")
@Data
public class CustomerGroup {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "Code", length = 50)
	private String code;

	@Column(name = "Description")
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
