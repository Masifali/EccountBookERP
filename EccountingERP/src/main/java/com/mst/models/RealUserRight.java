package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * READ-ONLY mapping of the real desktop's dbo.tblUserRights table - the actual grant
 * grid: one row per (UserId, ScreenId, RightId) the user has been given, Value=1
 * meaning granted (confirmed via goldenAce5_25t.sql, e.g. UserId=77 has a row for
 * ScreenId=42/RightId=39, which {@link RealScreenRight} resolves to "View" on
 * ScreenDefinition.Id=42, Cheque Book Registration).
 *
 * IMPORTANT FINDING: the real desktop app has NO hardcoded "admin sees everything"
 * shortcut anywhere in Architecture.BLL/DAL - UserGroup.UserGroupRole (the column
 * this port's earlier CustomUserDetailsService keyed an "ADMIN" bypass off of) is
 * never even read by the real app. Every user, including whoever administers the
 * system, only sees what an explicit tblUserRights row (plus a matching
 * {@link RealCompanyRight} row at the tenant level) actually grants. This port's
 * CustomUserDetailsService now reproduces that for every {@link Screen} (MstScreen)
 * row that has been linked to a real ScreenDefinition.Id via realScreenDefinitionId.
 *
 * This port never writes to tblUserRights (it's real, already-populated desktop
 * data) - only reads it to decide what a real user sees.
 */
@Entity
@Table(name = "tblUserRights")
@Data
public class RealUserRight {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "UserId")
	private Integer userId;

	@Column(name = "ScreenId")
	private Integer screenId;

	/** FK into {@link RealScreenRight#getId()}. */
	@Column(name = "RightId")
	private Integer rightId;

	@Column(name = "Value")
	private Boolean value;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "AppId")
	private Integer appId;
}
