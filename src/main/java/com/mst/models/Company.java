package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the desktop app's Architecture.Model.Company - a core subset only (the
 * real table also carries CompanyTypeId/OrgCompanyTypeId, website/fax, other-language
 * name/address variants, a logo byte[], a report-template id and more, not needed by
 * anything ported so far). What other modules call a "Branch" on the desktop (e.g.
 * AcfrmAcAllocation.cmbToCompany, invWarehouse's branch picker) is really just a
 * Company row where CompType = "Branch" rather than the head office.
 *
 * Real dbo.Company.Id has NO IDENTITY clause (confirmed via goldenAce5_25t.sql) -
 * app-assigned like most master tables in this port. There is no save path for
 * Company yet (read-only so far - see AccountController's "Account Allocation To
 * Locations" use), so this was previously mismapped (@GeneratedValue(IDENTITY), a
 * lowercase "company" table name, and no explicit @Column names) without having
 * been exercised; fixed now to the real shape while adding the first real read use.
 */
@Entity
@Table(name = "Company")
@Data
public class Company {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "CompCode", nullable = false, length = 50)
	private String compCode;

	@Column(name = "CompName", length = 150)
	private String compName;

	public String getCompanyName() {
		return compName;
	}

	/** e.g. "Head Office" / "Branch" - desktop: CompType. */
	@Column(name = "CompType", length = 50)
	private String compType;

	@Column(name = "CompAddress")
	private String compAddress;

	@Column(name = "CityName")
	private String cityName;

	@Column(name = "CompState", length = 50)
	private String compState;

	@Column(name = "CompCountry", length = 50)
	private String compCountry;

	@Column(name = "CompContactPerson", length = 200)
	private String compContactPerson;

	@Column(name = "CompTel", length = 50)
	private String compTel;

	@Column(name = "CompMobileA", length = 50)
	private String compMobileA;

	@Column(name = "CompEmailA", length = 50)
	private String compEmailA;

	@Column(name = "CompBaseCurr", length = 50)
	private String compBaseCurr;

	@Column(name = "IsHeadOffice")
	private Boolean isHeadOffice;

	@Column(name = "EntryUser")
	private Integer entryUser;

	@Column(name = "ModifyUser")
	private Integer modifyUser;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	// ---- added by batch B_masters_geo_company (columns seen in Sp_Company_GetAllMethod) ----
	@Column(name = "CompEmailB")
	private String compEmailB;

	@Column(name = "CompMobileB")
	private String compMobileB;

	@Column(name = "CompMobileC")
	private String compMobileC;

	@Column(name = "CompReportingTitle")
	private String compReportingTitle;

	@Column(name = "CompLogo")
	private String compLogo;

	/** varbinary logo (CompanyProfileRepository binds it as Types.VARBINARY). */
	@Column(name = "CompLogoImage")
	private byte[] compLogoImage;

	/** FK to Organization.Id - the procedure's @OrgCompanyTypeId. */
	@Column(name = "OrgCompanyTypeId")
	private Integer orgCompanyTypeId;

	@Column(name = "PictureURL")
	private String pictureURL;

	@Column(name = "CompanyTemplateId")
	private Integer companyTemplateId;

	@Column(name = "AllowedUserCount")
	private Integer allowedUserCount;

	@Column(name = "CompanyTypeId")
	private Integer companyTypeId;

	@Column(name = "CompanyNameOtherLanguage")
	private String companyNameOtherLanguage;

	@Column(name = "CompanyAddressOtherLanguage")
	private String companyAddressOtherLanguage;

	@Column(name = "CompanyWebsite")
	private String companyWebsite;

	@Column(name = "CompanyFaxNo")
	private String companyFaxNo;

	@Column(name = "CompanyNameOtherLing")
	private String companyNameOtherLing;

	@Column(name = "CompanyAddressOtherLing")
	private String companyAddressOtherLing;

	@Column(name = "CompanyId")
	private Integer companyId;

	// Explicit Getters and Setters
	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public String getCompCode() { return compCode; }
	public void setCompCode(String compCode) { this.compCode = compCode; }
	public String getCompName() { return compName; }
	public void setCompName(String compName) { this.compName = compName; }
	public String getCompType() { return compType; }
	public void setCompType(String compType) { this.compType = compType; }
	public String getCompAddress() { return compAddress; }
	public void setCompAddress(String compAddress) { this.compAddress = compAddress; }
	public String getCityName() { return cityName; }
	public void setCityName(String cityName) { this.cityName = cityName; }
	public Boolean getIsHeadOffice() { return isHeadOffice; }
	public void setIsHeadOffice(Boolean isHeadOffice) { this.isHeadOffice = isHeadOffice; }
	public String getCompEmailB() { return compEmailB; }
	public void setCompEmailB(String compEmailB) { this.compEmailB = compEmailB; }
	public String getCompMobileB() { return compMobileB; }
	public void setCompMobileB(String compMobileB) { this.compMobileB = compMobileB; }
	public String getCompMobileC() { return compMobileC; }
	public void setCompMobileC(String compMobileC) { this.compMobileC = compMobileC; }
	public String getCompReportingTitle() { return compReportingTitle; }
	public void setCompReportingTitle(String compReportingTitle) { this.compReportingTitle = compReportingTitle; }
	public String getCompLogo() { return compLogo; }
	public void setCompLogo(String compLogo) { this.compLogo = compLogo; }
	public byte[] getCompLogoImage() { return compLogoImage; }
	public void setCompLogoImage(byte[] compLogoImage) { this.compLogoImage = compLogoImage; }
	public Integer getOrgCompanyTypeId() { return orgCompanyTypeId; }
	public void setOrgCompanyTypeId(Integer orgCompanyTypeId) { this.orgCompanyTypeId = orgCompanyTypeId; }
	public String getPictureURL() { return pictureURL; }
	public void setPictureURL(String pictureURL) { this.pictureURL = pictureURL; }
	public Integer getCompanyTemplateId() { return companyTemplateId; }
	public void setCompanyTemplateId(Integer companyTemplateId) { this.companyTemplateId = companyTemplateId; }
	public Integer getAllowedUserCount() { return allowedUserCount; }
	public void setAllowedUserCount(Integer allowedUserCount) { this.allowedUserCount = allowedUserCount; }
	public Integer getCompanyTypeId() { return companyTypeId; }
	public void setCompanyTypeId(Integer companyTypeId) { this.companyTypeId = companyTypeId; }
	public String getCompanyNameOtherLanguage() { return companyNameOtherLanguage; }
	public void setCompanyNameOtherLanguage(String companyNameOtherLanguage) { this.companyNameOtherLanguage = companyNameOtherLanguage; }
	public String getCompanyAddressOtherLanguage() { return companyAddressOtherLanguage; }
	public void setCompanyAddressOtherLanguage(String companyAddressOtherLanguage) { this.companyAddressOtherLanguage = companyAddressOtherLanguage; }
	public String getCompanyWebsite() { return companyWebsite; }
	public void setCompanyWebsite(String companyWebsite) { this.companyWebsite = companyWebsite; }
	public String getCompanyFaxNo() { return companyFaxNo; }
	public void setCompanyFaxNo(String companyFaxNo) { this.companyFaxNo = companyFaxNo; }
	public String getCompanyNameOtherLing() { return companyNameOtherLing; }
	public void setCompanyNameOtherLing(String companyNameOtherLing) { this.companyNameOtherLing = companyNameOtherLing; }
	public String getCompanyAddressOtherLing() { return companyAddressOtherLing; }
	public void setCompanyAddressOtherLing(String companyAddressOtherLing) { this.companyAddressOtherLing = companyAddressOtherLing; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
}
