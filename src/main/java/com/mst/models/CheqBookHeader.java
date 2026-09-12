package com.mst.models;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import javax.persistence.Transient;

import lombok.Data;

@Entity
@Table(name = "CheqBookHeader")
@Data
public class CheqBookHeader {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "Id")
	private Long id;

	@Column(name = "DocNo")
	private Integer docNo;

	@Column(name = "DocDate")
	@org.springframework.format.annotation.DateTimeFormat(iso = org.springframework.format.annotation.DateTimeFormat.ISO.DATE)
	private LocalDate docDate;

	@Column(name = "BankId")
	private Integer bankId;

	@Column(name = "ChartOfAccountId")
	private Integer chartOfAccountId;

	@Column(name = "CbPrefix", length = 50)
	private String cbPrefix;

	@Column(name = "CbSrFrom", length = 50)
	private String cbSrFrom;

	@Column(name = "CbSrTo", length = 50)
	private String cbSrTo;

	@Column(name = "Remarks", length = 255)
	private String remarks;

	@Column(name = "EntryUser")
	private Integer entryUser;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Transient
	private String bankName;

	@Transient
	private String accountTitle;

	@Transient
	private String entryUserName;

	@OneToMany(mappedBy = "cheqBookHeaderId", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
	private List<CheqBookDetail> details = new ArrayList<>();
}
