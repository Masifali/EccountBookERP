package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.persistence.Transient;

import lombok.Data;

@Entity
@Table(name = "CheqBookDetail")
@Data
public class CheqBookDetail {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "Id")
	private Long id;

	@Column(name = "CheqBookHeaderId")
	private Long cheqBookHeaderId;

	@Column(name = "CheqNo", length = 50)
	private String cheqNo;

	@Column(name = "CheqStatus", length = 50)
	private String cheqStatus = "Blank";

	@Column(name = "OtherRemarks", length = 255)
	private String otherRemarks;

	@Column(name = "CheqCancelStatus")
	private Boolean cheqCancelStatus = false;

	@Column(name = "ChequeStatusDate")
	private LocalDateTime chequeStatusDate;

	@Column(name = "StatusChangeUserId")
	private Integer statusChangeUserId;

	@Transient
	private String bankName;

	// Explicit Getters and Setters
	public Long getId() { return id; }
	public void setId(Long id) { this.id = id; }
	public Long getCheqBookHeaderId() { return cheqBookHeaderId; }
	public void setCheqBookHeaderId(Long cheqBookHeaderId) { this.cheqBookHeaderId = cheqBookHeaderId; }
	public String getCheqNo() { return cheqNo; }
	public void setCheqNo(String cheqNo) { this.cheqNo = cheqNo; }
	public String getCheqStatus() { return cheqStatus; }
	public void setCheqStatus(String cheqStatus) { this.cheqStatus = cheqStatus; }
	public String getOtherRemarks() { return otherRemarks; }
	public void setOtherRemarks(String otherRemarks) { this.otherRemarks = otherRemarks; }
	public Boolean getCheqCancelStatus() { return cheqCancelStatus; }
	public void setCheqCancelStatus(Boolean cheqCancelStatus) { this.cheqCancelStatus = cheqCancelStatus; }
	public LocalDateTime getChequeStatusDate() { return chequeStatusDate; }
	public void setChequeStatusDate(LocalDateTime chequeStatusDate) { this.chequeStatusDate = chequeStatusDate; }
	public Integer getStatusChangeUserId() { return statusChangeUserId; }
	public void setStatusChangeUserId(Integer statusChangeUserId) { this.statusChangeUserId = statusChangeUserId; }
}
