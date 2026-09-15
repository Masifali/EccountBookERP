package com.mst.models;

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
}
