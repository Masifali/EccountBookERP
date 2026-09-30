package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.Inventory.SupplierCustomer (model 1065): the 58 non-virtual properties, as
 * GenericProvider.SetProc sends them to Sp_SupplierCustomer_Insert / _Update (59 params; @UserLogId is not a
 * model property and keeps its default). AccountTitle / AccountTitleAdvance / CustomerType are virtual.
 * An unset string is null and therefore not sent (AddWithValue(null)), as on the desktop.
 */
public class PpASupplierCustomerModel extends DesktopModel {
    public boolean IsDebitCredit;
    public boolean IsSubSupCust;
    public boolean PostState;
    public boolean Status;
    public LocalDateTime CNIC_EXPIRY_DATE;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public LocalDateTime PostDate;
    public double CreditLimit;
    public double DebitCreditAmount;
    public int ActionId;
    public int AdvanceGlAcId;
    public int BranchId;
    public int CityId;
    public int CompanyId;
    public int CountryId;
    public int CustomerGroupId;
    public int EntryUser;
    public int GlAccountId;
    public int Id;
    public int ModifyUser;
    public int OrganizationId;
    public int ParentsSupCustId;
    public int DiscountPolicyId;
    public int PostUser;
    public int ProfileGroupId;
    public int ProjectId;
    public int StateProvinceId;
    public int CustomerTypeId;
    public int PartyTypeId;
    public int BusinessTypeId;
    public String PartyTypePrefix;
    public String Address1;
    public String Address2;
    public String CNIC;
    public String CompanyName;
    public String Email;
    public String FirstName;
    public String FTN_No;
    public String LastName;
    public String MobileOffice;
    public String MobilePersonal;
    public String NTN_No;
    public String Phone;
    public String PictureURL;
    public String NickName;
    public String ReportingTitle;
    public String STRN_No;
    public String SupCustCode;
    public String Title;
    public String Town;
    public String WebPage;
    public String ZipCode;
    public String ManualPartyCode;
    public String WhatsAppNo;
    public String CompanyIndividuals;
    public boolean IsTaxable;
}
