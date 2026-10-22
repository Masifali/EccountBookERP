package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.persistence.Transient;

import lombok.Data;

/**
 * Ditto of the desktop's Architecture.Model.Authentication.ConfigrationsAllocation.
 *
 * Batch B_masters_geo_company: promoted from a plain DTO to a JPA entity mapped 1:1 onto the
 * real dbo.ConfigrationsAllocation table (source: Sp_ConfigrationsAllocation_GetAllMethod and the
 * ConfigrationsDefinitionId=227 lookup inside Sp_SupplierCustomer_GetAllMethod). Every field and
 * accessor of the previous DTO is kept unchanged so ConfigurationServiceImpl (JdbcTemplate based)
 * keeps compiling; the two fields that are NOT columns of this table (configDescription,
 * configModuleDescription - they live on dbo.ConfigrationsDefinition) are marked @Transient.
 *
 * The real table (verified against GoldenAceDb(0509)t.sql line 19906) has ONLY these columns:
 * Id (IDENTITY), ConfigrationsDefinitionId, ConfigValue, ConfigKey, IsActive, OrganizationId,
 * CompanyId, EntryUserId, EntryDate, ModifyUserId, ModifyDate.
 */
@Entity
@Table(name = "ConfigrationsAllocation")
@Data
public class ConfigrationsAllocation {

	/** dbo.ConfigrationsAllocation.Id (IDENTITY). 0/null means "not yet saved" - ditto BLL.Save's Id==0 check. */
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "Id")
	private Integer id;

	/** dbo.ConfigrationsAllocation.ConfigrationsDefinitionId - FK to dbo.ConfigrationsDefinition.Id. */
	@Column(name = "ConfigrationsDefinitionId")
	private Integer configrationsDefinitionId;

	/** dbo.ConfigrationsAllocation.ConfigValue - always saved as "" by every desktop Fire*() handler on this screen. */
	@Column(name = "ConfigValue")
	private String configValue;

	/** dbo.ConfigrationsAllocation.ConfigKey - the actual stored setting value (e.g. "True"/"False", a number, a selected id). */
	@Column(name = "ConfigKey")
	private String configKey;

	/** dbo.ConfigrationsAllocation.IsActive. */
	@Column(name = "IsActive")
	private Boolean isActive;

	/** dbo.ConfigrationsAllocation.OrganizationId. */
	@Column(name = "OrganizationId")
	private Integer organizationId;

	/** dbo.ConfigrationsAllocation.CompanyId. */
	@Column(name = "CompanyId")
	private Integer companyId;

	/** dbo.ConfigrationsAllocation.EntryUserId. */
	@Column(name = "EntryUserId")
	private Integer entryUserId;

	/** dbo.ConfigrationsAllocation.ModifyUserId. */
	@Column(name = "ModifyUserId")
	private Integer modifyUserId;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	/**
	 * NOT a column on this table - projected in from dbo.ConfigrationsDefinition.ConfigDescription
	 * by the JOINed read procedures. This is the desktop's AccessibleName-based lookup key
	 * (every bound control's Control.AccessibleName equals this string exactly).
	 */
	@Transient
	private String configDescription;

	/** NOT a column on this table - projected in from dbo.ConfigrationsDefinition.ConfigModuleDescription. */
	@Transient
	private String configModuleDescription;

	// Explicit Getters and Setters
	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public Integer getConfigrationsDefinitionId() { return configrationsDefinitionId; }
	public void setConfigrationsDefinitionId(Integer configrationsDefinitionId) { this.configrationsDefinitionId = configrationsDefinitionId; }
	public String getConfigValue() { return configValue; }
	public void setConfigValue(String configValue) { this.configValue = configValue; }
	public String getConfigKey() { return configKey; }
	public void setConfigKey(String configKey) { this.configKey = configKey; }
	public Boolean getIsActive() { return isActive; }
	public void setIsActive(Boolean isActive) { this.isActive = isActive; }
	public String getConfigDescription() { return configDescription; }
	public void setConfigDescription(String configDescription) { this.configDescription = configDescription; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public Integer getEntryUserId() { return entryUserId; }
	public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
	public Integer getModifyUserId() { return modifyUserId; }
	public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
	public String getConfigModuleDescription() { return configModuleDescription; }
	public void setConfigModuleDescription(String configModuleDescription) { this.configModuleDescription = configModuleDescription; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
}
