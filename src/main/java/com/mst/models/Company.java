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
}
