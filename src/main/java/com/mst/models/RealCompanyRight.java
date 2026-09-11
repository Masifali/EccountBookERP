package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * READ-ONLY mapping of the real desktop's dbo.CompanyRights table - the
 * tenant-level gate underneath tblUserRights: a screen must ALSO have an
 * (CompanyId, ScreenId, IsActive=1) row here before any user of that company can
 * use it at all, regardless of their own per-user grants (confirmed via
 * goldenAce5_25t.sql). This is how the real desktop turns whole screens on/off per
 * customer/company (e.g. a company that hasn't licensed a module never sees its
 * screens, no matter what an individual user's tblUserRights rows say).
 *
 * This port never writes to CompanyRights (it's real, already-populated desktop
 * data) - only reads it, alongside {@link RealUserRight} and
 * {@link RealScreenRight}, to decide what a real user sees.
 */
@Entity
@Table(name = "CompanyRights")
@Data
public class RealCompanyRight {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "ScreenId")
	private Integer screenId;

	@Column(name = "IsActive")
	private Boolean isActive;
}
