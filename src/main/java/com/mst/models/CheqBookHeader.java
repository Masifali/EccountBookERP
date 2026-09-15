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

	// Explicit Getters and Setters
	public Long getId() { return id; }
	public void setId(Long id) { this.id = id; }
	public Integer getDocNo() { return docNo; }
	public void setDocNo(Integer docNo) { this.docNo = docNo; }
	public LocalDate getDocDate() { return docDate; }
	public void setDocDate(LocalDate docDate) { this.docDate = docDate; }
	public Integer getBankId() { return bankId; }
	public void setBankId(Integer bankId) { this.bankId = bankId; }
	public Integer getChartOfAccountId() { return chartOfAccountId; }
	public void setChartOfAccountId(Integer chartOfAccountId) { this.chartOfAccountId = chartOfAccountId; }
	public String getCbSrFrom() { return cbSrFrom; }
	public void setCbSrFrom(String cbSrFrom) { this.cbSrFrom = cbSrFrom; }
	public String getCbSrTo() { return cbSrTo; }
	public void setCbSrTo(String cbSrTo) { this.cbSrTo = cbSrTo; }
	public String getBankName() { return bankName; }
	public void setBankName(String bankName) { this.bankName = bankName; }
	public String getAccountTitle() { return accountTitle; }
	public void setAccountTitle(String accountTitle) { this.accountTitle = accountTitle; }
	public Integer getEntryUser() { return entryUser; }
	public void setEntryUser(Integer entryUser) { this.entryUser = entryUser; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public String getCbPrefix() { return cbPrefix; }
	public void setCbPrefix(String cbPrefix) { this.cbPrefix = cbPrefix; }
	public String getRemarks() { return remarks; }
	public void setRemarks(String remarks) { this.remarks = remarks; }
}
