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

	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public String getUserName() { return userName; }
	public void setUserName(String userName) { this.userName = userName; }
	public String getPassword() { return password; }
	public void setPassword(String password) { this.password = password; }
	public String getFirstName() { return firstName; }
	public void setFirstName(String firstName) { this.firstName = firstName; }
	public String getLastName() { return lastName; }
	public void setLastName(String lastName) { this.lastName = lastName; }
	public String getCellNo() { return cellNo; }
	public void setCellNo(String cellNo) { this.cellNo = cellNo; }
	public UserGroup getUserGroup() { return userGroup; }
	public void setUserGroup(UserGroup userGroup) { this.userGroup = userGroup; }
	public Integer getUserRoleId() { return userRoleId; }
	public void setUserRoleId(Integer userRoleId) { this.userRoleId = userRoleId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public Integer getBranchesId() { return branchesId; }
	public void setBranchesId(Integer branchesId) { this.branchesId = branchesId; }
	public Integer getSupplierCustomerId() { return supplierCustomerId; }
	public void setSupplierCustomerId(Integer supplierCustomerId) { this.supplierCustomerId = supplierCustomerId; }
	public Integer getEntryUserId() { return entryUserId; }
	public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
	public Integer getModifyUserId() { return modifyUserId; }
	public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
	public Integer getUserTypeId() { return userTypeId; }
	public void setUserTypeId(Integer userTypeId) { this.userTypeId = userTypeId; }
	public Integer getAppId() { return appId; }
	public void setAppId(Integer appId) { this.appId = appId; }
	public Integer getEmployeeId() { return employeeId; }
	public void setEmployeeId(Integer employeeId) { this.employeeId = employeeId; }
	public Integer getAuthenticationCode() { return authenticationCode; }
	public void setAuthenticationCode(Integer authenticationCode) { this.authenticationCode = authenticationCode; }
	public Integer getAuthenticationEnabled() { return authenticationEnabled; }
	public void setAuthenticationEnabled(Integer authenticationEnabled) { this.authenticationEnabled = authenticationEnabled; }
	public String getPlayerId() { return playerId; }
	public void setPlayerId(String playerId) { this.playerId = playerId; }
	public String getAppVersion() { return appVersion; }
	public void setAppVersion(String appVersion) { this.appVersion = appVersion; }
	public Boolean getIsActive() { return isActive; }
	public void setIsActive(Boolean isActive) { this.isActive = isActive; }
	public Boolean getDeviceDependency() { return deviceDependency; }
	public void setDeviceDependency(Boolean deviceDependency) { this.deviceDependency = deviceDependency; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
}
