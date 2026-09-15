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
 * Ditto of the real GoldenAcedb dbo.invWarehouseRack table (desktop:
 * Inventory_Definition/frmRackDefine.cs). Id is not a SQL Server IDENTITY column
 * (unlike its parent InvWareHouse), so RackService assigns the next one itself. The
 * real table has no separate "rack code" - just rackName and a sortNo used to order
 * racks within a warehouse.
 */
@Entity
@Table(name = "invWarehouseRack")
@Data
public class Rack {

	@Id
	@Column(name = "Id")
	private Integer id;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "invWarehouseId", nullable = false)
	private Warehouse warehouse;

	@Column(name = "rackName", nullable = false, length = 150)
	private String rackName;

	@Column(name = "sortNo", nullable = false)
	private Integer sortNo = 0;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "EntryUserId")
	private Integer entryUserId;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "ModifyUserId")
	private Integer modifyUserId;

	@Column(name = "isActive")
	private Boolean isActive = true;

	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public Warehouse getWarehouse() { return warehouse; }
	public void setWarehouse(Warehouse warehouse) { this.warehouse = warehouse; }
	public String getRackName() { return rackName; }
	public void setRackName(String rackName) { this.rackName = rackName; }
	public Integer getSortNo() { return sortNo; }
	public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public Integer getEntryUserId() { return entryUserId; }
	public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
	public Integer getModifyUserId() { return modifyUserId; }
	public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
	public Boolean getIsActive() { return isActive; }
	public void setIsActive(Boolean isActive) { this.isActive = isActive; }
}
