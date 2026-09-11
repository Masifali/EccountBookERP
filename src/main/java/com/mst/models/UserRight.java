package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.EnumType;
import javax.persistence.Enumerated;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;
import javax.persistence.UniqueConstraint;

import com.mst.constants.RightType;

import lombok.Data;

/**
 * This port's own rights matrix - one row per (user, screen, right-type), parallel
 * in spirit to the desktop's dbo.tblUserRights (UserId, ScreenID, RightsID) -&gt;
 * Value shape, but NOT mapped onto that real table: tblUserRights.RightId points at
 * ScreenRights (one row per real (screen, right-name) combination, with large
 * scattered Ids - not a small fixed enum), which doesn't fit this port's simpler
 * {@link RightType} enum or its own {@link Screen} master (see Screen's Javadoc for
 * why that's a separate, additively-new table too). This is MstUserRight, its own
 * additively-new table - see the one-time CREATE TABLE script provided for it.
 */
@Entity
@Table(name = "MstUserRight", uniqueConstraints = @UniqueConstraint(columnNames = {"UserAccountId", "ScreenId", "RightType"}))
@Data
public class UserRight {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "Id")
	private Integer id;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "UserAccountId", nullable = false)
	private UserAccount user;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "ScreenId", nullable = false)
	private Screen screen;

	@Enumerated(EnumType.STRING)
	@Column(name = "RightType", nullable = false, length = 20)
	private RightType rightType;

	@Column(name = "Value", nullable = false)
	private Boolean value = false;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "AppId")
	private Integer appId;
}
