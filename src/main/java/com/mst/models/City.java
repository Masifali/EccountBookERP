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
}
