package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.ProductType table (desktop:
 * Inventory_Definition/ProductType.cs). Id is not a SQL Server IDENTITY column, so
 * ProductTypeService assigns the next one itself (see findMaxId()), same pattern as
 * Brand/ItemGroup/ItemCategory/ItemType.
 */
@Entity
@Table(name = "ProductType")
@Data
public class ProductType {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "ProductTypeCode")
	private String productTypeCode;

	@Column(name = "ProductTypeDescription")
	private String productTypeDescription;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "EntryUserId")
	private Integer entryUserId;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "ModifyUserId")
	private Integer modifyUserId;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "FinancialYearId")
	private Integer financialYearId;

	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public String getProductTypeCode() { return productTypeCode; }
	public void setProductTypeCode(String productTypeCode) { this.productTypeCode = productTypeCode; }
	public String getProductTypeDescription() { return productTypeDescription; }
	public void setProductTypeDescription(String productTypeDescription) { this.productTypeDescription = productTypeDescription; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public Integer getEntryUserId() { return entryUserId; }
	public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
	public Integer getModifyUserId() { return modifyUserId; }
	public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public Integer getFinancialYearId() { return financialYearId; }
	public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }
}
