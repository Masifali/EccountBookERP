package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.ItemGroup table (desktop: Inventory_Definition/
 * DefineItemGroup.cs). Real PK column is GroupId, not Id, and the table is a plain
 * named lookup - no code, no audit columns beyond Organization/Company. Id is not a
 * SQL Server IDENTITY column, so ItemGroupService assigns the next one itself (see
 * IItemGroupRepository.findMaxId()), same pattern as Brand/ItemCategory/ItemType.
 */
@Entity
@Table(name = "ItemGroup")
@Data
public class ItemGroup {

	@Id
	@Column(name = "GroupId")
	private Integer id;

	@Column(name = "ItemGroupName", length = 50)
	private String itemGroupName;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;

	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public String getItemGroupName() { return itemGroupName; }
	public void setItemGroupName(String itemGroupName) { this.itemGroupName = itemGroupName; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
}
