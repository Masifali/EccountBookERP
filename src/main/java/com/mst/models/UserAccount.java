package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;
import javax.persistence.UniqueConstraint;

import lombok.Data;
import org.hibernate.annotations.NotFound;
import org.hibernate.annotations.NotFoundAction;

/**
 * Ditto of the desktop app's Architecture.Model.UserAccount - the real login-user
 * entity, mapped 1:1 onto the real dbo.UserAccount table (confirmed via
 * goldenAce5_25t.sql). Only the model's own persisted properties are carried over
 * here - its many "virtual" properties (OrgName, CompName, BranchName, RoleName,
 * PictureURL, LicenseKey, SystemDate, ...) are display-time joins on the desktop
 * side, not columns; UserTitle is a PERSISTED computed column (FirstName+' '+LastName)
 * and so isn't written by this port either.
 *
 * [ID] has NO IDENTITY clause in the real DDL (app-assigned, like most master tables
 * in this port) - see UserAccountService's max-id+1 on insert.
 *
 * Password storage: per the user's explicit instruction, login now reads/writes the
 * real dbo.UserAccount.Password column directly (nvarchar(50)) - no separate
 * credential table. This column is NOT a hash: the real desktop app
 * (Architecture.Model.Encryption.UserPasswordEncryptString/DecryptString) stores it
 * as Rijndael/AES-128, CBC mode, PKCS7 padding, Key=IV=UTF-8 bytes of the hardcoded
 * string "3024482831469469", Base64-encoded - confirmed by decrypting real rows from
 * goldenAce5_25t.sql (e.g. user "GOLDEN" -> "Dealing@3", user "KAREEM" ->
 * "kareem@654"). See {@link com.mst.security.LegacyUserPasswordEncoder}, which
 * reproduces this exact scheme and is registered as this app's Spring Security
 * PasswordEncoder, so a real desktop user's existing password logs them into this
 * port unchanged, and a password set/changed here logs them into the desktop
 * unchanged too.
 *
 * userGroupId is a real {@link UserGroup} relation here (was a bare int on the
 * desktop model, resolved through UserGroupId).
 */
@Entity
@Table(name = "UserAccount", uniqueConstraints = @UniqueConstraint(columnNames = "UserName"))
@Data
public class UserAccount {

	@Id
	@Column(name = "ID")
	private Integer id;

	@Column(name = "UserName", length = 50)
	private String userName;

	/**
	 * Rijndael/AES-encrypted, Base64-encoded - see the class Javadoc above. Never a
	 * raw plaintext password at rest. {@link com.mst.services.UserAccountService}
	 * is careful to overwrite whatever Spring's form binding pours into this field
	 * (the User Registration form's "password" input binds here too) with either
	 * the freshly-encrypted new password or the previously-stored encrypted value -
	 * this field should never be persisted as plaintext.
	 */
	@Column(name = "Password", length = 50)
	private String password;

	@Column(name = "FirstName", length = 50)
	private String firstName;

	@Column(name = "LastName", length = 50)
	private String lastName;

	@Column(name = "CellNo", length = 200)
	private String cellNo;

	// Authentication reads the Desktop user's role while the repository session
	// is closed.  Keep the real relationship, but initialize it for the login
	// principal so Spring Security cannot retain a detached proxy.
	@ManyToOne(fetch = FetchType.EAGER)
	@NotFound(action = NotFoundAction.IGNORE)
	@JoinColumn(name = "UserGroupId")
	private UserGroup userGroup;

	/** Desktop: UserRoleId - a separate, older role reference not used by this port (UserGroupId is). */
	@Column(name = "UserRoleId")
	private Integer userRoleId;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "BranchesId")
	private Integer branchesId;

	@Column(name = "SupplierCustomerId")
	private Integer supplierCustomerId;

	@Column(name = "EntryUserId")
	private Integer entryUserId;

	@Column(name = "ModifyUserId")
	private Integer modifyUserId;

	/** Desktop: UserTypeId (e.g. system user vs. employee-linked user). */
	@Column(name = "UserTypeId")
	private Integer userTypeId;

	@Column(name = "AppId")
	private Integer appId;

	@Column(name = "EmployeeId")
	private Integer employeeId;

	@Column(name = "AuthenticationCode")
	private Integer authenticationCode;

	@Column(name = "AuthenticationEnabled")
	private Integer authenticationEnabled;

	@Column(name = "PlayerId")
	private String playerId;

	@Column(name = "AppVersion", length = 50)
	private String appVersion;

	@Column(name = "IsActive")
	private Boolean isActive = true;

	@Column(name = "DeviceDependency")
	private Boolean deviceDependency = false;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;
}
