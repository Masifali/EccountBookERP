package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.persistence.UniqueConstraint;

import lombok.Data;

/**
 * Ditto of the desktop app's Architecture.Model.UserGroup - this is the desktop's
 * "Role" (Configurations/frmUserModuleAdmin.cs manages these; UserAccount.UserGroupId
 * points here). Mapped onto the real dbo.UserGroup table (confirmed via
 * goldenAce5_25t.sql: UserGroupID is a real IDENTITY column here, unlike most other
 * master tables in this port).
 */
@Entity
@Table(name = "UserGroup", uniqueConstraints = @UniqueConstraint(columnNames = "UserGroupName"))
@Data
public class UserGroup {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "UserGroupID")
	private Integer userGroupId;

	@Column(name = "UserGroupName", nullable = false, length = 100)
	private String userGroupName;

	/** Desktop: UserGroupRole - the short code used elsewhere as the role (e.g. "ADMIN", "ACCOUNTS"). */
	@Column(name = "UserGroupRole", nullable = false, length = 50)
	private String userGroupRole;

	@Column(name = "Status", nullable = false)
	private Boolean status = true;
}
