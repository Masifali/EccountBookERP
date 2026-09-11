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
	private Long id;

	@Column(name = "doc_no")
	private Integer docNo;

	@Column(name = "doc_date")
	private LocalDate docDate;

	@Column(name = "bank_id")
	private Integer bankId;

	@Column(name = "chart_of_account_id")
	private Integer chartOfAccountId;

	@Column(name = "cb_prefix", length = 50)
	private String cbPrefix;

	@Column(name = "cb_sr_from", length = 50)
	private String cbSrFrom;

	@Column(name = "cb_sr_to", length = 50)
	private String cbSrTo;

	@Column(length = 255)
	private String remarks;

	@Column(name = "entry_user")
	private Integer entryUser;

	@Column(name = "entry_date")
	private LocalDateTime entryDate;

	@Column(name = "company_id")
	private Integer companyId;

	@Column(name = "organization_id")
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
