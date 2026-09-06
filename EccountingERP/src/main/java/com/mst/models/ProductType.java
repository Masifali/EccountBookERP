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
}
