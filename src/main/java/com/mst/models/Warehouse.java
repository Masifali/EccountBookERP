package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.InvWareHouse table (desktop: Inventory_Definition/
 * InvDeffrmItemUomSchedule.cs family / InvDefrmWarehouse). Unlike most other
 * Inventory_Definition tables, Id here IS a SQL Server IDENTITY(1,1) column, so this
 * keeps the standard JPA auto-increment generator.
 *
 * BranchesId is kept as a plain Integer, not a JPA relation to Company - this
 * project's Company.java already documents that the desktop's "Branch" concept is
 * really just a Company row with compType = "Branch", so branchesId is that
 * Company.Id.
 */
@Entity
@Table(name = "InvWareHouse")
@Data
public class Warehouse {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "Id")
	private Integer id;

	@Column(name = "WareHouseCode", length = 50)
	private String wareHouseCode;

	@Column(name = "WareHouseName", length = 150)
	private String wareHouseName;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "EntryUser")
	private Integer entryUser;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "ModifyUser")
	private Integer modifyUser;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "IsActive")
	private Boolean isActive = true;

	/** Company.Id of the Company row whose compType = "Branch" - see class Javadoc. */
	@Column(name = "BranchesId")
	private Integer branchesId;

	@Column(name = "WarehouseType", length = 50)
	private String warehouseType;
}
