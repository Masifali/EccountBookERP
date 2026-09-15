package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.ItemType table (desktop: Inventory_Definition/
 * InvDeffrmItemType.cs). Child of ItemCategory via ParentCategoryId. Id is not a SQL
 * Server IDENTITY column, so ItemTypeService assigns the next one itself.
 */
@Entity
@Table(name = "ItemType")
@Data
public class ItemType {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "TypeCode", length = 50)
	private String typeCode;

	@Column(name = "TypeDescription", length = 100)
	private String typeDescription;

	/** Desktop-internal numeric type flag (distinct from the Category link below). */
	@Column(name = "Type")
	private Integer type;

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

	@Column(name = "ParentCategoryId")
	private Integer parentCategoryId;

	@Column(name = "IsMother")
	private Boolean isMother = false;

	// Explicit Getters and Setters
	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public String getTypeCode() { return typeCode; }
	public void setTypeCode(String typeCode) { this.typeCode = typeCode; }
	public String getTypeDescription() { return typeDescription; }
	public void setTypeDescription(String typeDescription) { this.typeDescription = typeDescription; }
	public Integer getType() { return type; }
	public void setType(Integer type) { this.type = type; }
	public Integer getParentCategoryId() { return parentCategoryId; }
	public void setParentCategoryId(Integer parentCategoryId) { this.parentCategoryId = parentCategoryId; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public Integer getEntryUser() { return entryUser; }
	public void setEntryUser(Integer entryUser) { this.entryUser = entryUser; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public Boolean getPostState() { return postState; }
	public void setPostState(Boolean postState) { this.postState = postState; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
	public Integer getModifyUser() { return modifyUser; }
	public void setModifyUser(Integer modifyUser) { this.modifyUser = modifyUser; }
	public Boolean getIsMother() { return isMother; }
	public void setIsMother(Boolean isMother) { this.isMother = isMother; }
}
