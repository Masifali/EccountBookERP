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

	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public String getWareHouseCode() { return wareHouseCode; }
	public void setWareHouseCode(String wareHouseCode) { this.wareHouseCode = wareHouseCode; }
	public String getWareHouseName() { return wareHouseName; }
	public void setWareHouseName(String wareHouseName) { this.wareHouseName = wareHouseName; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public Integer getEntryUser() { return entryUser; }
	public void setEntryUser(Integer entryUser) { this.entryUser = entryUser; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
	public Integer getModifyUser() { return modifyUser; }
	public void setModifyUser(Integer modifyUser) { this.modifyUser = modifyUser; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public Boolean getIsActive() { return isActive; }
	public void setIsActive(Boolean isActive) { this.isActive = isActive; }
	public Integer getBranchesId() { return branchesId; }
	public void setBranchesId(Integer branchesId) { this.branchesId = branchesId; }
	public String getWarehouseType() { return warehouseType; }
	public void setWarehouseType(String warehouseType) { this.warehouseType = warehouseType; }
}
