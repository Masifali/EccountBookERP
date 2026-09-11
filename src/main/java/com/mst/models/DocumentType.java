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
}
