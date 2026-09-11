package com.mst.models;

import java.time.LocalDate;
import org.springframework.format.annotation.DateTimeFormat;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real dbo.SupplierCustomer table (confirmed via goldenAce5_25t.sql's
 * own CREATE TABLE script) - backs the desktop's "Define Supplier" / "Define
 * Customer" screen (Supplier_Purchase/supfrmDefineSupplier.cs, real
 * ScreenDefinition.Id=11, Account Definition module). One flat table for both
 * suppliers and customers - {@link #customerTypeId} (real lookup: {@link
 * SupplierCustomerType}, Golden Ace's own data: Id=1 "Vendor", Id=2 "Customer") is
 * what the desktop's cmbPartyType actually binds off, not a free-text "Supplier"/
 * "Customer" string - an earlier pass of this port had invented such a string field
 * (and a fabricated "supplier_customer" table with ~20 fields) before the real
 * table was known; replaced now with every real column, matching the "real
 * DB-verified structures, not guesses" rule for this project.
 *
 * Left out on purpose (desktop-side sub-forms not covered by this pass, same scope
 * as before): bank details / ship-to-address / supplier-limits sub-forms (separate
 * real tables - SupplierCustomerShipToAddress etc.), multi-language names
 * (SupplierCustomerMultiLingo), the picture upload's actual file storage (PictureURL
 * is mapped as a plain string), and sub-supplier parenting UI (IsSubSupCust /
 * ParentsSupCustId are mapped but not yet exposed as a parent picker).
 *
 * Real [Id] has NO IDENTITY clause - app-assigned (confirmed via the real DDL), so
 * SupplierCustomerService computes the next id itself (max(id)+1), same convention
 * as every other non-identity master table in this port.
 */
@Entity
@Table(name = "SupplierCustomer")
@Data
public class SupplierCustomer {

	@Id
	@Column(name = "Id")
	private Integer id;

	/** Real lookup: {@link CustomerGroup} - a rice-mill-specific party grouping. */
	@Column(name = "CustomerGroupId")
	private Integer customerGroupId;

	/** Real lookup: {@link SupplierCustomerType} (Golden Ace: 1=Vendor, 2=Customer). */
	@Column(name = "CustomerTypeId")
	private Integer customerTypeId;

	@Column(name = "CompanyName", length = 150)
	private String companyName;

	@Column(name = "SupCustCode", length = 50)
	private String supCustCode;

	@Column(name = "Address1")
	private String address1;

	@Column(name = "Address2")
	private String address2;

	@Column(name = "Title")
	private String title;

	@Column(name = "FirstName", length = 150)
	private String firstName;

	@Column(name = "LastName", length = 50)
	private String lastName;

	@Column(name = "CountryId")
	private Integer countryId;

	@Column(name = "StateProvinceId")
	private Integer stateProvinceId;

	@Column(name = "ZipCode", length = 50)
	private String zipCode;

	@Column(name = "Town", length = 150)
	private String town;

	@Column(name = "CityId")
	private Integer cityId;

	@Column(name = "Phone", length = 50)
	private String phone;

	@Column(name = "MobileOffice", length = 50)
	private String mobileOffice;

	@Column(name = "MobilePersonal", length = 50)
	private String mobilePersonal;

	@Column(name = "Email", length = 250)
	private String email;

	@Column(name = "WebPage", length = 150)
	private String webPage;

	@Column(name = "CNIC", length = 50)
	private String cnic;

	@Column(name = "CNIC_EXPIRY_DATE")
	@DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
	private LocalDate cnicExpiryDate;

	@Column(name = "NTN_No", length = 50)
	private String ntnNo;

	@Column(name = "FTN_No", length = 50)
	private String ftnNo;

	@Column(name = "STRN_No", length = 50)
	private String strnNo;

	@Column(name = "CreditLimit")
	private Double creditLimit;

	@Column(name = "ReportingTitle", length = 200)
	private String reportingTitle;

	@Column(name = "ProfileGroupId")
	private Integer profileGroupId;

	@Column(name = "AdvanceGlAcId")
	private Integer advanceGlAcId;

	/** The linked ledger (Detail) account - real ChartofAccount.Id. */
	@Column(name = "GlAccountId")
	private Integer glAccountId;

	@Column(name = "IsSubSupCust")
	private Boolean isSubSupCust;

	@Column(name = "ParentsSupCustId")
	private Integer parentsSupCustId;

	@Column(name = "Status")
	private Boolean status;

	@Column(name = "IsDebitCredit")
	private Boolean isDebitCredit;

	@Column(name = "DebitCreditAmount")
	private Double debitCreditAmount;

	@Column(name = "PictureURL", length = 100)
	private String pictureUrl;

	@Column(name = "ActionId")
	private Integer actionId;

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

	@Column(name = "BranchId")
	private Integer branchId;

	@Column(name = "ProjectId")
	private Integer projectId;

	@Column(name = "OrganizationId")
	private Integer organizationId;

	@Column(name = "CompanyId")
	private Integer companyId;

	@Column(name = "DiscountPolicyId")
	private Integer discountPolicyId;

	@Column(name = "PartyTypeId")
	private Integer partyTypeId;

	@Column(name = "PartyTypeNo")
	private Integer partyTypeNo;

	@Column(name = "PartyTypePrefix", length = 50)
	private String partyTypePrefix;

	@Column(name = "IsTaxable")
	private Boolean isTaxable;

	@Column(name = "ManualPartyCode", length = 255)
	private String manualPartyCode;

	@Column(name = "PricingCustomGroupId")
	private Integer pricingCustomGroupId;

	@Column(name = "WhatsAppNo", length = 50)
	private String whatsAppNo;

	@Column(name = "CompanyIndividuals", length = 50)
	private String companyIndividuals;

	@Column(name = "NickName", length = 500)
	private String nickName;

	@Column(name = "BusinessTypeId")
	private Integer businessTypeId;
}
