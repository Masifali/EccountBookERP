package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

@Entity
@Table(name = "City")
@Data
public class City {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "CityName", length = 100)
	private String cityName;

	@Column(name = "TehsilId")
	private Integer tehsilId;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	// ---- added by batch B_masters_geo_company (columns seen in SP_City_GetAllMethod:
	//      City.Description is the real city-name column, projected AS CityName by the procedure) ----
	@Column(name = "Code")
	private String code;

	@Column(name = "Description")
	private String description;

	@Column(name = "CityNameOtherLingo")
	private String cityNameOtherLingo;

	@Column(name = "CountryId")
	private Integer countryId;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "EntryUser")
	private Integer entryUser;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "ModifyUser")
	private Integer modifyUser;

	@Column(name = "PostDate")
	private LocalDateTime postDate;

	@Column(name = "PostUser")
	private Integer postUser;

	@Column(name = "PostState")
	private Boolean postState;

	// Explicit Getters and Setters
	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public String getCityName() { return cityName; }
	public void setCityName(String cityName) { this.cityName = cityName; }
	public Integer getTehsilId() { return tehsilId; }
	public void setTehsilId(Integer tehsilId) { this.tehsilId = tehsilId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public String getCode() { return code; }
	public void setCode(String code) { this.code = code; }
	public String getDescription() { return description; }
	public void setDescription(String description) { this.description = description; }
	public String getCityNameOtherLingo() { return cityNameOtherLingo; }
	public void setCityNameOtherLingo(String cityNameOtherLingo) { this.cityNameOtherLingo = cityNameOtherLingo; }
	public Integer getCountryId() { return countryId; }
	public void setCountryId(Integer countryId) { this.countryId = countryId; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public Integer getEntryUser() { return entryUser; }
	public void setEntryUser(Integer entryUser) { this.entryUser = entryUser; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
	public Integer getModifyUser() { return modifyUser; }
	public void setModifyUser(Integer modifyUser) { this.modifyUser = modifyUser; }
	public LocalDateTime getPostDate() { return postDate; }
	public void setPostDate(LocalDateTime postDate) { this.postDate = postDate; }
	public Integer getPostUser() { return postUser; }
	public void setPostUser(Integer postUser) { this.postUser = postUser; }
	public Boolean getPostState() { return postState; }
	public void setPostState(Boolean postState) { this.postState = postState; }
}
