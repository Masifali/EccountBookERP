package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.DocumentType table - the master list of every
 * voucher/document type in the whole ERP (393 real rows across every module: Cash
 * Payment Voucher, Bank Payment Voucher, Journal Voucher, ..., Sale Invoice, Import
 * Purchase Contract, ...). VoucherHead.DocumentTypeId points here. Id is not a SQL
 * Server IDENTITY column (the real Ids are fixed/well-known, e.g. 1=CPV, 2=BPV,
 * 3=CRV, 4=BRV, 5=JV, 10=CONTRA, 26=EV, 34=PRV, 35=PPV - see DocumentTypeSeeder),
 * so this port keeps those exact real Ids rather than assigning new ones.
 */
@Entity
@Table(name = "DocumentType")
@Data
public class DocumentType {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "DocumentTypeCode", length = 10)
	private String documentTypeCode;

	@Column(name = "DocumentTypeDescription", length = 500)
	private String documentTypeDescription;

	@Column(name = "SortNo")
	private Integer sortNo;

	@Column(name = "ControlAccountId")
	private Integer controlAccountId;

	@Column(name = "Status")
	private Boolean status = true;

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

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "Remarks")
	private String remarks;

	@Column(name = "DocumentTypeDescriptionOtherLing")
	private String documentTypeDescriptionOtherLing;

	@Column(name = "ScreenName")
	private String screenName;

	@Column(name = "TargetUrl")
	private String targetUrl;

	@Column(name = "DocumentTypeAlias", length = 200)
	private String documentTypeAlias;

	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public String getDocumentTypeCode() { return documentTypeCode; }
	public void setDocumentTypeCode(String documentTypeCode) { this.documentTypeCode = documentTypeCode; }
	public String getDocumentTypeDescription() { return documentTypeDescription; }
	public void setDocumentTypeDescription(String documentTypeDescription) { this.documentTypeDescription = documentTypeDescription; }
	public Integer getSortNo() { return sortNo; }
	public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
	public Integer getControlAccountId() { return controlAccountId; }
	public void setControlAccountId(Integer controlAccountId) { this.controlAccountId = controlAccountId; }
	public Boolean getStatus() { return status; }
	public void setStatus(Boolean status) { this.status = status; }
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
	public Integer getOrganizationId() { return organizationId; }
	public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
	public Integer getCompanyId() { return companyId; }
	public void setCompanyId(Integer companyId) { this.companyId = companyId; }
	public String getRemarks() { return remarks; }
	public void setRemarks(String remarks) { this.remarks = remarks; }
	public String getDocumentTypeDescriptionOtherLing() { return documentTypeDescriptionOtherLing; }
	public void setDocumentTypeDescriptionOtherLing(String documentTypeDescriptionOtherLing) { this.documentTypeDescriptionOtherLing = documentTypeDescriptionOtherLing; }
	public String getScreenName() { return screenName; }
	public void setScreenName(String screenName) { this.screenName = screenName; }
	public String getTargetUrl() { return targetUrl; }
	public void setTargetUrl(String targetUrl) { this.targetUrl = targetUrl; }
	public String getDocumentTypeAlias() { return documentTypeAlias; }
	public void setDocumentTypeAlias(String documentTypeAlias) { this.documentTypeAlias = documentTypeAlias; }
}
