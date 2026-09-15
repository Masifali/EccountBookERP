package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the desktop's real dbo.COAAllocation table (confirmed via
 * goldenAce5_25t.sql) - one row per (ChartofAccount, Company) the account has been
 * "allocated to" (made usable/postable for that company/branch). This backs the
 * Chart Of Account Definition screen's "Account Allocation To Locations" mini-grid
 * (each row there is a Company - "Location" is the desktop's own loose label for it -
 * with a checkbox bound to whether an active allocation row exists) and is also the
 * real table behind the separate "Account Allocation" / "Update COA Allocation"
 * screens listed in the Account Definition module (not yet built).
 *
 * Real [Id] DOES have IDENTITY(1,1) here (unlike ChartofAccount itself), confirmed
 * via the real DDL.
 */
@Entity
@Table(name = "COAAllocation")
@Data
public class COAAllocation {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "Id")
	private Integer id;

	@Column(name = "ChartofAccountId", nullable = false)
	private Integer chartofAccountId;

	@Column(name = "CompanyId", nullable = false)
	private Integer companyId;

	@Column(name = "GLPageNo", length = 50)
	private String glPageNo;

	@Column(name = "IsActive")
	private Boolean isActive = true;

	@Column(name = "BranchId")
	private Integer branchId;

	// Explicit Getters and Setters
	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public Integer getChartofAccountId() { return chartofAccountId; }
	public void setChartofAccountId(Integer chartofAccountId) { this.chartofAccountId = chartofAccountId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public String getGlPageNo() { return glPageNo; }
	public void setGlPageNo(String glPageNo) { this.glPageNo = glPageNo; }
	public Boolean getIsActive() { return isActive; }
	public void setIsActive(Boolean isActive) { this.isActive = isActive; }
	public Integer getBranchId() { return branchId; }
	public void setBranchId(Integer branchId) { this.branchId = branchId; }
}
