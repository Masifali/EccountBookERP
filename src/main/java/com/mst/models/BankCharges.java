package com.mst.models;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

@Entity
@Table(name = "BankCharges")
@Data
public class BankCharges {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "BankAccountId")
	private Integer bankAccountId;

	@Column(name = "ChargeDate", length = 50)
	private String chargeDate;

	@Column(name = "ChargeType", length = 100)
	private String chargeType;

	@Column(name = "Amount", precision = 18, scale = 2)
	private BigDecimal amount;

	@Column(name = "TaxAmount", precision = 18, scale = 2)
	private BigDecimal taxAmount;

	@Column(name = "TotalAmount", precision = 18, scale = 2)
	private BigDecimal totalAmount;

	@Column(name = "RefNo", length = 100)
	private String refNo;

	@Column(name = "Remarks")
	private String remarks;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;

	// Explicit Getters and Setters
	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public Integer getBankAccountId() { return bankAccountId; }
	public void setBankAccountId(Integer bankAccountId) { this.bankAccountId = bankAccountId; }
	public String getChargeDate() { return chargeDate; }
	public void setChargeDate(String chargeDate) { this.chargeDate = chargeDate; }
	public String getChargeType() { return chargeType; }
	public void setChargeType(String chargeType) { this.chargeType = chargeType; }
	public BigDecimal getAmount() { return amount; }
	public void setAmount(BigDecimal amount) { this.amount = amount; }
	public BigDecimal getTaxAmount() { return taxAmount; }
	public void setTaxAmount(BigDecimal taxAmount) { this.taxAmount = taxAmount; }
	public BigDecimal getTotalAmount() { return totalAmount; }
	public void setTotalAmount(BigDecimal totalAmount) { this.totalAmount = totalAmount; }
	public String getRefNo() { return refNo; }
	public void setRefNo(String refNo) { this.refNo = refNo; }
	public String getRemarks() { return remarks; }
	public void setRemarks(String remarks) { this.remarks = remarks; }
}
