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
}
