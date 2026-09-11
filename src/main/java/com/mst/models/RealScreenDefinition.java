package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * READ-ONLY mapping of the desktop's real dbo.ScreenDefinition master table
 * containing ~900 screen definitions across all ERP modules.
 */
@Entity
@Table(name = "ScreenDefinition")
@Data
public class RealScreenDefinition {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "ScreenName")
	private String screenName;

	@Column(name = "ScreenAlias")
	private String screenAlias;

	@Column(name = "ModuleId")
	private Integer moduleId;

	@Column(name = "IsCreateSMSTemplate")
	private Boolean isCreateSMSTemplate;

	@Column(name = "ReferencedTableName", length = 150)
	private String referencedTableName;

	@Column(name = "ReferencedTableId")
	private Integer referencedTableId;

	@Column(name = "MenuControllName")
	private String menuControllName;

	@Column(name = "TargetUrl")
	private String targetUrl;

	@Column(name = "IsActive")
	private Boolean isActive;

	@Column(name = "SortNo")
	private Integer sortNo;

	@Column(name = "HasSpecialRight")
	private Boolean hasSpecialRight;

	@Column(name = "AppId")
	private Integer appId;

	@Column(name = "CompanyIds")
	private String companyIds;
}
