package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real dbo.SupplierCustomerType table (confirmed via goldenAce5_25t.sql,
 * both DDL and real data: Golden Ace's own rows are Id=1 "Vendor" (TypeId=1) and
 * Id=2 "Customer" (TypeId=2)) - this is the real lookup {@link SupplierCustomer}
 * .customerTypeId points at, replacing an earlier fabricated plain-string
 * partyType ("Supplier"/"Customer") field that didn't exist on the real table.
 *
 * Real [Id] has NO IDENTITY clause - app-assigned (confirmed via the real DDL).
 */
@Entity
@Table(name = "SupplierCustomerType")
@Data
public class SupplierCustomerType {

	@Id
	@Column(name = "Id")
	private Integer id;

	/** 1=Vendor-side, 2=Customer-side grouping (desktop: cmbPartyType binds off this). */
	@Column(name = "TypeId")
	private Integer typeId;

	@Column(name = "SupplierCustomerType", nullable = false, length = 150)
	private String supplierCustomerType;

	@Column(name = "Prefix", nullable = false, length = 50)
	private String prefix;

	@Column(name = "OrganizationId", nullable = false)
	private Integer organizationId;

	@Column(name = "CompanyId", nullable = false)
	private Integer companyId;
}
