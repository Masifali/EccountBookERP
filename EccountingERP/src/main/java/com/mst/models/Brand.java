package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.Brand table (desktop: Inventory_Definition/
 * DefineBrand.cs). Id is NOT a SQL Server IDENTITY column in the real schema (same
 * as Item/ItemCategory/ItemType/ProductType) - the desktop assigns the next Id
 * itself, so BrandService does the equivalent (see IBrandRepository.findMaxId()).
 */
@Entity
@Table(name = "Brand")
@Data
public class Brand {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "BrandCode", nullable = false, length = 50)
	private String brandCode;

	@Column(name = "BrandName", nullable = false, length = 250)
	private String brandName;

	@Column(name = "IsActive", nullable = false)
	private Boolean isActive = true;

	@Column(name = "EntryDate", nullable = false)
	private LocalDateTime entryDate;

	@Column(name = "EntryUserId", nullable = false)
	private Integer entryUserId;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "ModifyUserId")
	private Integer modifyUserId;

	@Column(name = "OrganizationId", nullable = false)
	private Integer organizationId;

	@Column(name = "CompanyId", nullable = false)
	private Integer companyId;
}
