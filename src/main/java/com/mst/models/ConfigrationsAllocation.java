package com.mst.models;

import lombok.Data;

/**
 * Ditto of the desktop's Architecture.Model.Authentication.ConfigrationsAllocation.
 *
 * NOT a JPA @Entity. The real dbo.ConfigrationsAllocation table (verified against
 * GoldenAceDb(0509)t.sql line 19906) has ONLY these columns: Id (IDENTITY),
 * ConfigrationsDefinitionId, ConfigValue, ConfigKey, IsActive, OrganizationId,
 * CompanyId, EntryUserId, EntryDate, ModifyUserId, ModifyDate. It has NO
 * ConfigDescription or ConfigModuleDescription column - those two live only on the
 * separate dbo.ConfigrationsDefinition table (line 19929) and reach this model only
 * because the desktop's own read procedures (Proc_ConfigrationsAllocation_ReadByKey,
 * Proc_ConfigrationsAllocation_History) LEFT/INNER JOIN the two tables and project
 * Definition's columns into the same result row - see
 * Architecture.DAL.Authentication.ConfigrationsAllocation.FillDTO, which reads both
 * from one IDataReader without distinguishing which physical table each column
 * came from. An earlier version of this class was mapped as a JPA @Entity with
 * @Table(name = "tbl_ConfigrationsAllocation") and @Column(name = "ConfigDescription")
 * on this class - "tbl_ConfigrationsAllocation" does not exist anywhere in the
 * database (real name has no "tbl_" prefix) and neither does a ConfigDescription
 * column on the real ConfigrationsAllocation table, so any JPA query touching either
 * would fail at runtime with "Invalid object/column name". This screen is entirely
 * stored-procedure driven (ConfigurationServiceImpl, using JdbcTemplate) precisely
 * because of this: see that class for the five real procedure calls.
 */
@Data
public class ConfigrationsAllocation {

	/** dbo.ConfigrationsAllocation.Id (IDENTITY). 0/null means "not yet saved" - ditto BLL.Save's Id==0 check. */
	private Integer id;

	/** dbo.ConfigrationsAllocation.ConfigrationsDefinitionId - FK to dbo.ConfigrationsDefinition.Id. */
	private Integer configrationsDefinitionId;

	/** dbo.ConfigrationsAllocation.ConfigValue - always saved as "" by every desktop Fire*() handler on this screen. */
	private String configValue;

	/** dbo.ConfigrationsAllocation.ConfigKey - the actual stored setting value (e.g. "True"/"False", a number, a selected id). */
	private String configKey;

	/** dbo.ConfigrationsAllocation.IsActive. */
	private Boolean isActive;

	/** dbo.ConfigrationsAllocation.OrganizationId. */
	private Integer organizationId;

	/** dbo.ConfigrationsAllocation.CompanyId. */
	private Integer companyId;

	/** dbo.ConfigrationsAllocation.EntryUserId. */
	private Integer entryUserId;

	/** dbo.ConfigrationsAllocation.ModifyUserId. */
	private Integer modifyUserId;

	/**
	 * NOT a column on this table - projected in from dbo.ConfigrationsDefinition.ConfigDescription
	 * by the JOINed read procedures. This is the desktop's AccessibleName-based lookup key
	 * (every bound control's Control.AccessibleName equals this string exactly).
	 */
	private String configDescription;

	/** NOT a column on this table - projected in from dbo.ConfigrationsDefinition.ConfigModuleDescription. */
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
}
