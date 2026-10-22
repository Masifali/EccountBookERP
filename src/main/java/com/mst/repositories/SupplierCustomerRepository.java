package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.SupplierCustomer;
import com.mst.projections.ContractorHistoryRow;
import com.mst.projections.SupplierByImportPurchaseOrderRow;
import com.mst.projections.SupplierByPurchaseOrderRow;
import com.mst.projections.SupplierBySaleOrderRow;
import com.mst.projections.SupplierCustomerBindingRow;
import com.mst.projections.SupplierCustomerContractorWagesRow;
import com.mst.projections.SupplierCustomerExportRow;
import com.mst.projections.SupplierCustomerFormHistoryRow;
import com.mst.projections.SupplierCustomerFromOrderRow;
import com.mst.projections.SupplierCustomerGlAccountRow;
import com.mst.projections.SupplierCustomerIdMobileRow;
import com.mst.projections.SupplierCustomerIdNameGlRow;
import com.mst.projections.SupplierCustomerIdNameRow;
import com.mst.projections.SupplierCustomerInfoRow;
import com.mst.projections.SupplierCustomerPartyProcessingRow;
import com.mst.projections.SupplierCustomerPartyTypeRow;
import com.mst.projections.SupplierCustomerReadByIdRow;
import com.mst.projections.SupplierCustomerReadRow;
import com.mst.projections.SupplierCustomerShippingLineRow;
import com.mst.projections.SupplierCustomerWithCityRow;

/**
 * dbo.SupplierCustomer - Sp_SupplierCustomer_GetAllMethod (@Activity branches). Native branches are those that call
 * dbo.GetConfigurationByConfigId (attachment folder URL, config 91), join drmLookUps / UserProfile / DiscountPolicy /
 * ExIm* / InvSaleInvoice / InvOrderCategory (no entity), use fnSplitString or UNION ALL. Where the proc emits the same column
 * twice (AdvanceGlAcId in several branches) it is emitted once here.
 */
@Repository
public interface SupplierCustomerRepository extends JpaRepository<SupplierCustomer, Integer> {

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetAllTaxableSupplierCustomer' (active, taxable parties outside groups 7,8,9,10). */
    @Query("SELECT DISTINCT s.id AS id, s.companyName AS companyName FROM SupplierCustomer s INNER JOIN ChartofAccount c ON s.glAccountId = c.id "
         + "LEFT JOIN SupplierCustomerType t ON t.id = s.partyTypeId "
         + "WHERE s.organizationId = :organizationId AND s.companyId = :companyId AND s.status = true AND s.customerGroupId NOT IN (7, 8, 9, 10) "
         + "AND COALESCE(s.isTaxable, false) = true")
    List<SupplierCustomerIdNameRow> getAllTaxableSupplierCustomer(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='ReadByOrganizationCompanyId' (native: GetConfigurationByConfigId, drmLookUps). @IsTaxable optional (NULL = all). */
    @Query(value = "SELECT DISTINCT SupplierCustomer.Id, SupplierCustomer.CustomerGroupId, SupplierCustomer.CompanyName, SupplierCustomer.SupCustCode, SupplierCustomer.Address1, "
         + "SupplierCustomer.Address2, SupplierCustomer.Title, SupplierCustomer.FirstName, SupplierCustomer.LastName, SupplierCustomer.CountryId, "
         + "SupplierCustomer.StateProvinceId, SupplierCustomer.ZipCode, SupplierCustomer.Town, SupplierCustomer.CityId, SupplierCustomer.Phone, "
         + "SupplierCustomer.MobileOffice, SupplierCustomer.MobilePersonal, SupplierCustomer.Email, SupplierCustomer.WebPage, SupplierCustomer.CNIC, "
         + "SupplierCustomer.CNIC_EXPIRY_DATE, SupplierCustomer.NTN_No, SupplierCustomer.FTN_No, SupplierCustomer.STRN_No, SupplierCustomer.CreditLimit, "
         + "SupplierCustomer.ReportingTitle, SupplierCustomer.ProfileGroupId, SupplierCustomer.AdvanceGlAcId, SupplierCustomer.GlAccountId, "
         + "SupplierCustomer.IsSubSupCust, SupplierCustomer.ParentsSupCustId, SupplierCustomer.Status, SupplierCustomer.IsDebitCredit, "
         + "SupplierCustomer.DebitCreditAmount, SupplierCustomer.PartyTypeId, S.SupplierCustomerType PartyType, SupplierCustomer.PartyTypeNo, "
         + "SupplierCustomer.PartyTypePrefix, SupplierCustomer.IsTaxable, SupplierCustomer.PartyTypePrefix AS PartyTypePrefixNo, "
         + "CASE WHEN ISNULL(SupplierCustomer.PictureURL, '') = '' THEN dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + SupplierCustomer.PictureURL END PictureURL, "
         + "SupplierCustomer.ActionId, SupplierCustomer.EntryDate, SupplierCustomer.EntryUser, SupplierCustomer.ModifyDate, SupplierCustomer.ModifyUser, "
         + "SupplierCustomer.PostDate, SupplierCustomer.PostUser, SupplierCustomer.PostState, SupplierCustomer.BranchId, SupplierCustomer.ProjectId, "
         + "SupplierCustomer.OrganizationId, SupplierCustomer.CompanyId, ChartofAccount.AccountTitle, coa.AccountTitle AS AccountTitleAdvance, "
         + "SupplierCustomer.CustomerTypeId, drm.LookUpName CustomerType, ChartofAccount.AccountTypeId, SupplierCustomer.DiscountPolicyId, "
         + "ChartofAccount.AccountCode GLAccountCode, SupplierCustomer.ManualPartyCode, SupplierCustomer.PricingCustomGroupId, SupplierCustomer.WhatsAppNo, "
         + "SupplierCustomer.NickName, SupplierCustomer.CompanyIndividuals, SupplierCustomer.BusinessTypeId "
         + "FROM SupplierCustomer "
         + "INNER JOIN ChartofAccount ON SupplierCustomer.GlAccountId = ChartofAccount.Id "
         + "INNER JOIN COAAllocation c ON SupplierCustomer.GlAccountId = c.ChartofAccountId "
         + "LEFT OUTER JOIN ChartofAccount coa ON SupplierCustomer.AdvanceGlAcId = coa.Id "
         + "LEFT JOIN drmLookUps drm ON SupplierCustomer.CustomerTypeId = drm.Id "
         + "LEFT JOIN SupplierCustomerType S ON S.Id = SupplierCustomer.PartyTypeId "
         + "WHERE SupplierCustomer.OrganizationId = :organizationId AND SupplierCustomer.CompanyId = :companyId AND c.IsActive = 1 "
         + "AND SupplierCustomer.CustomerGroupId NOT IN (7, 9, 10) AND (:isTaxable IS NULL OR ISNULL(SupplierCustomer.IsTaxable, 0) = :isTaxable)", nativeQuery = true)
    List<SupplierCustomerReadRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("isTaxable") Boolean isTaxable);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='FormHistory' (native: GetConfigurationByConfigId, UserProfile, DiscountPolicy). Filters @PartyTypeId/@CustomerGroupId/@GlAccountId/@CityId/@IsTaxable optional. */
    @Query(value = "SELECT DISTINCT sp.Id, sp.CustomerGroupId, sp.CompanyName, sp.SupCustCode, sp.Address1, sp.Address2, sp.Title, cg.Description CustomerGroup, sp.FirstName, "
         + "sp.LastName, sp.CountryId, sp.StateProvinceId, sp.ZipCode, sp.Town, sp.CityId, sp.Phone, sp.MobileOffice, sp.MobilePersonal, sp.Email, sp.WhatsAppNo, sp.NickName, "
         + "sp.WebPage, sp.CNIC, sp.CNIC_EXPIRY_DATE, sp.NTN_No, sp.FTN_No, sp.STRN_No, sp.CreditLimit, sp.ReportingTitle, sp.ProfileGroupId, sp.AdvanceGlAcId, sp.GlAccountId, "
         + "sp.IsSubSupCust, sp.ParentsSupCustId, sp.Status, sp.IsDebitCredit, dp.PolicyName, sp.DebitCreditAmount, sp.CompanyIndividuals, sp.BusinessTypeId, sp.IsTaxable, "
         + "CASE WHEN ISNULL(sp.PictureURL, '') = '' THEN ISNULL(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91), '') + 'DummyProfilePicture.jpg' "
         + "ELSE ISNULL(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91), '') + sp.PictureURL END PictureURL, "
         + "sp.PictureURL AS PictureName, sp.ActionId, sp.EntryUser, sp.ModifyUser, sp.PostDate, sp.PostUser, sp.PostState, sp.BranchId, sp.ProjectId, sp.OrganizationId, "
         + "sp.CompanyId, Cha.AccountCode, Cha.AccountTitle, coa.AccountTitle AS AccountTitleAdvance, pac.CompanyName AS ParentAccountTitle, sp.CustomerTypeId, "
         + "sp.PartyTypeId, S.SupplierCustomerType PartyType, sp.PartyTypeNo, sp.PartyTypePrefix, sp.PartyTypePrefix AS PartyTypePrefixNo, "
         + "EU.UserTitle EntryUserName, sp.EntryDate, sp.EntryUser EntryUserId, "
         + "CASE WHEN ISNULL(EUP.ProfileImageFileName, '') = '' THEN dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + EUP.ProfileImageFileName END EntryUserProfileImageName, "
         + "MU.UserTitle ModifyUserName, sp.ModifyDate, sp.ModifyUser ModifyUserId, "
         + "CASE WHEN ISNULL(MUP.ProfileImageFileName, '') = '' THEN dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + MUP.ProfileImageFileName END ModifyUserProfileImageName, "
         + "sp.ManualPartyCode, sp.PricingCustomGroupId, ct.Description CityName "
         + "FROM SupplierCustomer sp "
         + "INNER JOIN ChartofAccount Cha ON sp.GlAccountId = Cha.Id "
         + "INNER JOIN COAAllocation c ON sp.GlAccountId = c.ChartofAccountId "
         + "INNER JOIN UserAccount EU ON EU.ID = sp.EntryUser "
         + "LEFT JOIN SupplierCustomer pac ON sp.ParentsSupCustId = pac.Id "
         + "LEFT JOIN UserProfile EUP ON EUP.UserId = EU.ID "
         + "LEFT JOIN UserAccount MU ON MU.ID = sp.ModifyUser "
         + "LEFT JOIN UserProfile MUP ON MUP.UserId = MU.ID "
         + "LEFT JOIN ChartofAccount coa ON sp.AdvanceGlAcId = coa.Id "
         + "LEFT JOIN CustomerGroup cg ON cg.Id = sp.CustomerGroupId "
         + "LEFT JOIN DiscountPolicy dp ON dp.Id = sp.DiscountPolicyId "
         + "LEFT JOIN SupplierCustomerType S ON S.Id = sp.PartyTypeId "
         + "LEFT JOIN City Ct ON Ct.Id = Sp.CityId "
         + "WHERE sp.OrganizationId = :organizationId AND sp.CompanyId = :companyId "
         + "AND (:partyTypeId IS NULL OR sp.PartyTypeId = :partyTypeId) AND (:customerGroupId IS NULL OR sp.CustomerGroupId = :customerGroupId) "
         + "AND (:glAccountId IS NULL OR sp.GlAccountId = :glAccountId) AND (:cityId IS NULL OR sp.CityId = :cityId) "
         + "AND (:isTaxable IS NULL OR ISNULL(sp.IsTaxable, 0) = :isTaxable)", nativeQuery = true)
    List<SupplierCustomerFormHistoryRow> formHistory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("partyTypeId") Integer partyTypeId, @Param("customerGroupId") Integer customerGroupId, @Param("glAccountId") Integer glAccountId,
            @Param("cityId") Integer cityId, @Param("isTaxable") Boolean isTaxable);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='FormHistoryForPartyProcessing' (non-taxable parties of CustomerGroupId 12). */
    @Query("SELECT DISTINCT sc.id AS id, sc.companyName AS companyName, sc.glAccountId AS glAccountId, c.accountTitle AS glAccount, sc.mobilePersonal AS mobilePersonal, "
         + "sc.countryId AS countryId, cn.description AS countryName, cty.description AS cityName, sc.cnic AS cnic, sc.address1 AS address1, sc.pictureUrl AS pictureURL "
         + "FROM SupplierCustomer sc INNER JOIN ChartofAccount c ON sc.glAccountId = c.id LEFT JOIN Country cn ON sc.countryId = cn.id LEFT JOIN City cty ON sc.cityId = cty.id "
         + "WHERE sc.organizationId = :organizationId AND sc.companyId = :companyId AND COALESCE(sc.isTaxable, false) = false AND sc.customerGroupId = 12")
    List<SupplierCustomerPartyProcessingRow> formHistoryForPartyProcessing(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='ReadByOrganizationCompanyIdForExport' (native: drmLookUps; non-taxable parties of CustomerGroupId 7). */
    @Query(value = "SELECT DISTINCT SupplierCustomer.Id, SupplierCustomer.CustomerGroupId, SupplierCustomer.CompanyName, SupplierCustomer.SupCustCode, SupplierCustomer.Address1, "
         + "SupplierCustomer.Address2, SupplierCustomer.Title, SupplierCustomer.FirstName, SupplierCustomer.LastName, SupplierCustomer.CountryId, "
         + "SupplierCustomer.StateProvinceId, SupplierCustomer.ZipCode, SupplierCustomer.Town, SupplierCustomer.CityId, SupplierCustomer.Phone, "
         + "SupplierCustomer.MobileOffice, SupplierCustomer.MobilePersonal, SupplierCustomer.Email, SupplierCustomer.WebPage, SupplierCustomer.CNIC, "
         + "SupplierCustomer.CNIC_EXPIRY_DATE, SupplierCustomer.NTN_No, SupplierCustomer.FTN_No, SupplierCustomer.STRN_No, SupplierCustomer.CreditLimit, "
         + "SupplierCustomer.ReportingTitle, SupplierCustomer.ProfileGroupId, SupplierCustomer.AdvanceGlAcId, SupplierCustomer.GlAccountId, "
         + "SupplierCustomer.IsSubSupCust, SupplierCustomer.ParentsSupCustId, SupplierCustomer.Status, SupplierCustomer.IsDebitCredit, "
         + "SupplierCustomer.DebitCreditAmount, SupplierCustomer.PictureURL, SupplierCustomer.ActionId, SupplierCustomer.EntryDate, SupplierCustomer.EntryUser, "
         + "SupplierCustomer.ModifyDate, SupplierCustomer.ModifyUser, SupplierCustomer.PostDate, SupplierCustomer.PartyTypeId, S.SupplierCustomerType PartyType, "
         + "SupplierCustomer.PartyTypeNo, SupplierCustomer.PartyTypePrefix, CONCAT(SupplierCustomer.PartyTypePrefix, SupplierCustomer.PartyTypeNo) PartyTypePrefixNo, "
         + "SupplierCustomer.PostUser, SupplierCustomer.PostState, SupplierCustomer.BranchId, SupplierCustomer.ProjectId, SupplierCustomer.OrganizationId, "
         + "SupplierCustomer.CompanyId, ChartofAccount.AccountTitle, coa.AccountTitle AS AccountTitleAdvance, SupplierCustomer.CustomerTypeId, "
         + "drm.LookUpName CustomerType, SupplierCustomer.DiscountPolicyId, SupplierCustomer.IsTaxable, SupplierCustomer.ManualPartyCode, "
         + "SupplierCustomer.PricingCustomGroupId, SupplierCustomer.WhatsAppNo, SupplierCustomer.NickName, SupplierCustomer.CompanyIndividuals, SupplierCustomer.BusinessTypeId "
         + "FROM SupplierCustomer "
         + "INNER JOIN ChartofAccount ON SupplierCustomer.GlAccountId = ChartofAccount.Id "
         + "INNER JOIN COAAllocation c ON SupplierCustomer.GlAccountId = c.ChartofAccountId "
         + "LEFT OUTER JOIN ChartofAccount coa ON SupplierCustomer.AdvanceGlAcId = coa.Id "
         + "LEFT JOIN drmLookUps drm ON SupplierCustomer.CustomerTypeId = drm.Id "
         + "LEFT JOIN SupplierCustomerType S ON S.Id = SupplierCustomer.PartyTypeId "
         + "WHERE SupplierCustomer.OrganizationId = :organizationId AND SupplierCustomer.CompanyId = :companyId AND c.IsActive = 1 "
         + "AND SupplierCustomer.CustomerGroupId = 7 AND ISNULL(SupplierCustomer.IsTaxable, 0) = 0", nativeQuery = true)
    List<SupplierCustomerExportRow> readByOrganizationCompanyIdForExport(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='ReadByOrganizationCompanyIdForExportShippinglineNClearingAgents' (parties of CustomerGroupId 9 or 10 with an active allocation). */
    @Query("SELECT DISTINCT s.id AS id, s.customerGroupId AS customerGroupId, s.companyName AS companyName, s.supCustCode AS supCustCode, s.address1 AS address1, "
         + "s.address2 AS address2, s.title AS title, s.firstName AS firstName, s.lastName AS lastName, s.countryId AS countryId, s.stateProvinceId AS stateProvinceId, "
         + "s.zipCode AS zipCode, s.town AS town, s.cityId AS cityId, s.phone AS phone, s.mobileOffice AS mobileOffice, s.mobilePersonal AS mobilePersonal, "
         + "s.email AS email, s.webPage AS webPage, s.cnic AS cnic, s.cnicExpiryDate AS cnicExpiryDate, s.ntnNo AS ntnNo, s.ftnNo AS ftnNo, s.strnNo AS strnNo, "
         + "s.creditLimit AS creditLimit, s.reportingTitle AS reportingTitle, s.profileGroupId AS profileGroupId, s.advanceGlAcId AS advanceGlAcId, "
         + "s.glAccountId AS glAccountId, s.isSubSupCust AS isSubSupCust, s.parentsSupCustId AS parentsSupCustId, s.status AS status, s.isDebitCredit AS isDebitCredit, "
         + "s.isTaxable AS isTaxable, s.debitCreditAmount AS debitCreditAmount, s.pictureUrl AS pictureURL, s.actionId AS actionId, s.entryDate AS entryDate, "
         + "s.entryUser AS entryUser, s.modifyDate AS modifyDate, s.modifyUser AS modifyUser, s.postDate AS postDate, s.partyTypeId AS partyTypeId, "
         + "t.supplierCustomerType AS partyType, s.partyTypeNo AS partyTypeNo, s.partyTypePrefix AS partyTypePrefix, "
         + "CONCAT(s.partyTypePrefix, CAST(s.partyTypeNo AS string)) AS partyTypePrefixNo, s.postUser AS postUser, s.postState AS postState, s.branchId AS branchId, "
         + "s.projectId AS projectId, s.organizationId AS organizationId, s.companyId AS companyId, c.accountTitle AS accountTitle, coa.accountTitle AS accountTitleAdvance, "
         + "s.customerTypeId AS customerTypeId, s.discountPolicyId AS discountPolicyId, s.manualPartyCode AS manualPartyCode, s.pricingCustomGroupId AS pricingCustomGroupId, "
         + "s.whatsAppNo AS whatsAppNo, s.nickName AS nickName, s.companyIndividuals AS companyIndividuals, s.businessTypeId AS businessTypeId "
         + "FROM SupplierCustomer s INNER JOIN ChartofAccount c ON s.glAccountId = c.id INNER JOIN COAAllocation a ON s.glAccountId = a.chartofAccountId "
         + "LEFT JOIN ChartofAccount coa ON s.advanceGlAcId = coa.id LEFT JOIN SupplierCustomerType t ON t.id = s.partyTypeId "
         + "WHERE s.organizationId = :organizationId AND s.companyId = :companyId AND a.isActive = true AND s.customerGroupId IN (10, 9)")
    List<SupplierCustomerShippingLineRow> readByOrganizationCompanyIdForExportShippinglineNClearingAgents(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='ReadByOrganizationCompanyIdForContractorWages' (native: GetConfigurationByConfigId, UserProfile; non-taxable parties of CustomerGroupId 8). */
    @Query(value = "SELECT DISTINCT SupplierCustomer.Id, SupplierCustomer.CustomerGroupId, SupplierCustomer.CompanyName, SupplierCustomer.SupCustCode, SupplierCustomer.Address1, "
         + "SupplierCustomer.Address2, SupplierCustomer.Title, SupplierCustomer.FirstName, SupplierCustomer.LastName, SupplierCustomer.CountryId, "
         + "SupplierCustomer.StateProvinceId, SupplierCustomer.ZipCode, SupplierCustomer.Town, SupplierCustomer.CityId, SupplierCustomer.Phone, "
         + "SupplierCustomer.MobileOffice, SupplierCustomer.MobilePersonal, SupplierCustomer.Email, SupplierCustomer.WebPage, SupplierCustomer.CNIC, "
         + "SupplierCustomer.CNIC_EXPIRY_DATE, SupplierCustomer.NTN_No, SupplierCustomer.FTN_No, SupplierCustomer.STRN_No, SupplierCustomer.CreditLimit, "
         + "SupplierCustomer.ReportingTitle, SupplierCustomer.ProfileGroupId, SupplierCustomer.AdvanceGlAcId, SupplierCustomer.GlAccountId, "
         + "SupplierCustomer.IsSubSupCust, SupplierCustomer.ParentsSupCustId, SupplierCustomer.Status, SupplierCustomer.IsDebitCredit, SupplierCustomer.IsTaxable, "
         + "SupplierCustomer.DebitCreditAmount, SupplierCustomer.ActionId, SupplierCustomer.EntryDate, SupplierCustomer.EntryUser, "
         + "SupplierCustomer.ModifyDate, SupplierCustomer.ModifyUser, SupplierCustomer.PostDate, SupplierCustomer.PartyTypeId, S.SupplierCustomerType PartyType, "
         + "SupplierCustomer.PartyTypeNo, SupplierCustomer.PartyTypePrefix, CONCAT(SupplierCustomer.PartyTypePrefix, SupplierCustomer.PartyTypeNo) PartyTypePrefixNo, "
         + "SupplierCustomer.PostUser, SupplierCustomer.PostState, SupplierCustomer.BranchId, SupplierCustomer.ProjectId, SupplierCustomer.OrganizationId, "
         + "SupplierCustomer.CompanyId, ChartofAccount.AccountTitle, coa.AccountTitle AS AccountTitleAdvance, SupplierCustomer.CustomerTypeId, SupplierCustomer.DiscountPolicyId, "
         + "CONCAT(Usr.FirstName, '-', Usr.LastName) AS EntryUserName, "
         + "CASE WHEN ISNULL(SupplierCustomer.PictureURL, '') = '' THEN dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + SupplierCustomer.PictureURL END PictureURL, "
         + "CASE WHEN ISNULL(entryUserProfile.ProfileImageFileName, '') = '' THEN dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + entryUserProfile.ProfileImageFileName END EntryUserProfileImageName, "
         + "SupplierCustomer.ManualPartyCode, SupplierCustomer.PricingCustomGroupId, SupplierCustomer.WhatsAppNo, SupplierCustomer.NickName, "
         + "SupplierCustomer.CompanyIndividuals, SupplierCustomer.BusinessTypeId "
         + "FROM SupplierCustomer "
         + "INNER JOIN UserAccount Usr ON SupplierCustomer.EntryUser = Usr.ID "
         + "LEFT JOIN ChartofAccount ON SupplierCustomer.GlAccountId = ChartofAccount.Id "
         + "LEFT JOIN COAAllocation c ON SupplierCustomer.GlAccountId = c.ChartofAccountId "
         + "LEFT OUTER JOIN ChartofAccount coa ON SupplierCustomer.AdvanceGlAcId = coa.Id "
         + "LEFT JOIN UserProfile entryUserProfile ON entryUserProfile.UserId = Usr.ID "
         + "LEFT JOIN SupplierCustomerType S ON S.Id = SupplierCustomer.PartyTypeId "
         + "WHERE SupplierCustomer.OrganizationId = :organizationId AND SupplierCustomer.CompanyId = :companyId AND c.IsActive = 1 "
         + "AND SupplierCustomer.CustomerGroupId = 8 AND ISNULL(SupplierCustomer.IsTaxable, 0) = 0", nativeQuery = true)
    List<SupplierCustomerContractorWagesRow> readByOrganizationCompanyIdForContractorWages(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='ReadById' (native: drmLookUps; AccountTitle / AccountTitleAdvance are returned as ''). */
    @Query(value = "SELECT SupplierCustomer.[Id], [CustomerGroupId], [CompanyName], [SupCustCode], [Address1], [Address2], [Title], [FirstName], [LastName], [CountryId], "
         + "[StateProvinceId], [ZipCode], [Town], [CityId], [Phone], [MobileOffice], [MobilePersonal], [Email], [WebPage], [CNIC], [CNIC_EXPIRY_DATE], [NTN_No], [FTN_No], "
         + "[STRN_No], [CreditLimit], [ReportingTitle], [ProfileGroupId], [AdvanceGlAcId], [GlAccountId], [IsSubSupCust], [ParentsSupCustId], [Status], [IsDebitCredit], "
         + "[DebitCreditAmount], [PictureURL], [ActionId], [EntryDate], [EntryUser], [ModifyDate], [ModifyUser], [PostDate], [PostUser], [PostState], [BranchId], [ProjectId], "
         + "SupplierCustomer.CompanyIndividuals, SupplierCustomer.BusinessTypeId, SupplierCustomer.[OrganizationId], SupplierCustomer.[CompanyId], CustomerTypeId, "
         + "SupplierCustomer.DiscountPolicyId, SupplierCustomer.IsTaxable, '' AS AccountTitle, '' AS AccountTitleAdvance, drm.LookUpName CustomerType, "
         + "SupplierCustomer.PartyTypeId, S.SupplierCustomerType PartyType, SupplierCustomer.PartyTypeNo, SupplierCustomer.PartyTypePrefix, "
         + "CONCAT(SupplierCustomer.PartyTypePrefix, SupplierCustomer.PartyTypeNo) PartyTypePrefixNo, SupplierCustomer.ManualPartyCode, "
         + "SupplierCustomer.PricingCustomGroupId, SupplierCustomer.WhatsAppNo, SupplierCustomer.NickName "
         + "FROM [dbo].[SupplierCustomer] "
         + "LEFT JOIN drmLookUps drm ON SupplierCustomer.CustomerTypeId = drm.Id "
         + "LEFT JOIN SupplierCustomerType S ON S.Id = SupplierCustomer.PartyTypeId "
         + "WHERE SupplierCustomer.Id = :id", nativeQuery = true)
    List<SupplierCustomerReadByIdRow> readById(@Param("id") Integer id);

    /**
     * Sp_SupplierCustomer_GetAllMethod @Activity='SupplierByPurchaseOrderNo' - KEEP-EXEC: reads configs 273 (PurchaseOrderBranchWise) and
     * 320 (UsePurchaseOrderBalanceInNextFinYear), looks the order up by DocNo (@PurchaseOrderId), RAISERRORs when it does not exist /
     * is Complete / is Cancel / is not approved / is expired at @GpDate, treats the order already referenced by gate pass @GpId as Open,
     * then returns the supplier + order lines. @FinancialYearId, @BranchesId, @GpDate, @GpId default NULL.
     */
    // KEEP-EXEC: GetConfigurationByConfigId + five RAISERROR validations before the SELECT
    @Query(value = "EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, @PurchaseOrderId = :purchaseOrderId, "
         + "@DocumentTypeId = :documentTypeId, @FinancialYearId = :financialYearId, @BranchesId = :branchesId, @GpDate = :gpDate, @GpId = :gpId, "
         + "@Activity = 'SupplierByPurchaseOrderNo'", nativeQuery = true)
    List<SupplierByPurchaseOrderRow> supplierByPurchaseOrderNo(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("purchaseOrderId") Integer purchaseOrderId, @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId, @Param("gpDate") LocalDate gpDate, @Param("gpId") Integer gpId);

    /**
     * Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierBySaleOrderNo' - KEEP-EXEC: same pattern as SupplierByPurchaseOrderNo for
     * SaleOrder (configs 277 SaleOrderBranchWise / 321 UseSaleOrderBalanceInNextFinYear; RAISERRORs for missing / Complete / Cancel /
     * not approved / expired orders). @FinancialYearId, @BranchesId, @GpDate default NULL.
     */
    // KEEP-EXEC: GetConfigurationByConfigId + five RAISERROR validations before the SELECT
    @Query(value = "EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId = :organizationId, @CompanyId = :companyId, @SaleOrderId = :saleOrderId, "
         + "@DocumentTypeId = :documentTypeId, @FinancialYearId = :financialYearId, @BranchesId = :branchesId, @GpDate = :gpDate, "
         + "@Activity = 'GetSupplierBySaleOrderNo'", nativeQuery = true)
    List<SupplierBySaleOrderRow> getSupplierBySaleOrderNo(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("saleOrderId") Integer saleOrderId, @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId, @Param("gpDate") LocalDate gpDate);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetIdByGlAccountId' (non-taxable parties of a GL account). */
    @Query("SELECT s.id AS id, s.mobilePersonal AS mobilePersonal FROM SupplierCustomer s "
         + "WHERE s.glAccountId = :glAccountId AND s.organizationId = :organizationId AND s.companyId = :companyId AND COALESCE(s.isTaxable, false) = false")
    List<SupplierCustomerIdMobileRow> getIdByGlAccountId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("glAccountId") Integer glAccountId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetCustomerIdByLcOrderAndFcReceipt' (native: ExImLcOrder, ExImFCBankReceipts, UNION ALL). */
    @Query(value = "SELECT DISTINCT a.SupplierCustomerId AS Id, c.CompanyName FROM ("
         + "SELECT DISTINCT lc.SupCustId AS SupplierCustomerId FROM ExImLcOrder lc WHERE lc.OrganizationId = :organizationId AND lc.CompanyId = :companyId "
         + "UNION ALL SELECT DISTINCT r.SupplierCustomerId FROM ExImFCBankReceipts r WHERE r.OrganizationId = :organizationId AND r.CompanyId = :companyId) a "
         + "INNER JOIN SupplierCustomer c ON a.SupplierCustomerId = c.Id", nativeQuery = true)
    List<SupplierCustomerIdNameRow> getCustomerIdByLcOrderAndFcReceipt(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * Sp_SupplierCustomer_GetAllMethod @Activity='ReadByOrganizationIdCompanyIdForBinding' (native: PartyCode = ManualPartyCode when
     * ConfigrationsAllocation(ConfigrationsDefinitionId 227).ConfigKey is 0/NULL, PartyTypePrefix when 1, else ''; inlined as CTE "cfg").
     */
    @Query(value = "WITH cfg AS (SELECT COALESCE((SELECT TOP 1 CAST(ConfigKey AS BIT) FROM ConfigrationsAllocation WHERE OrganizationId = :organizationId "
         + "AND CompanyId = :companyId AND ConfigrationsDefinitionId = 227), 0) AS AutoCode) "
         + "SELECT DISTINCT sp.Id, CompanyName, sp.GlAccountId, "
         + "CASE WHEN cfg.AutoCode = 0 THEN sp.ManualPartyCode WHEN cfg.AutoCode = 1 THEN sp.PartyTypePrefix ELSE '' END AS PartyCode "
         + "FROM SupplierCustomer sp CROSS JOIN cfg "
         + "INNER JOIN ChartofAccount ON sp.GlAccountId = ChartofAccount.Id "
         + "INNER JOIN COAAllocation c ON sp.GlAccountId = c.ChartofAccountId "
         + "WHERE sp.OrganizationId = :organizationId AND sp.CompanyId = :companyId AND c.IsActive = 1 "
         + "AND sp.CustomerGroupId NOT IN (7, 8, 9, 10, 11, 13) AND ISNULL(sp.IsTaxable, 0) = 0", nativeQuery = true)
    List<SupplierCustomerBindingRow> readByOrganizationIdCompanyIdForBinding(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='AllSupplierCustomerWithCityName' (native: same as ReadByOrganizationIdCompanyIdForBinding plus CityId / CityName / MobilePersonal). */
    @Query(value = "WITH cfg AS (SELECT COALESCE((SELECT TOP 1 CAST(ConfigKey AS BIT) FROM ConfigrationsAllocation WHERE OrganizationId = :organizationId "
         + "AND CompanyId = :companyId AND ConfigrationsDefinitionId = 227), 0) AS AutoCode) "
         + "SELECT DISTINCT sp.Id, CompanyName, sp.GlAccountId, "
         + "CASE WHEN cfg.AutoCode = 0 THEN sp.ManualPartyCode WHEN cfg.AutoCode = 1 THEN sp.PartyTypePrefix ELSE '' END AS PartyCode, "
         + "sp.CityId, ct.Description CityName, sp.MobilePersonal "
         + "FROM SupplierCustomer sp CROSS JOIN cfg "
         + "INNER JOIN ChartofAccount ON sp.GlAccountId = ChartofAccount.Id "
         + "INNER JOIN COAAllocation c ON sp.GlAccountId = c.ChartofAccountId "
         + "LEFT JOIN City Ct ON ct.Id = sp.CityId "
         + "WHERE sp.OrganizationId = :organizationId AND sp.CompanyId = :companyId AND c.IsActive = 1 "
         + "AND sp.CustomerGroupId NOT IN (7, 8, 9, 10, 11, 13) AND ISNULL(sp.IsTaxable, 0) = 0", nativeQuery = true)
    List<SupplierCustomerWithCityRow> allSupplierCustomerWithCityName(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetCustomerExistInExportInvoices' (native: ExImInvoice; one row per invoice, as the proc). */
    @Query(value = "SELECT sp.Id, sp.CompanyName FROM SupplierCustomer sp INNER JOIN ExImInvoice ei ON sp.id = ei.SupplierCustomerId "
         + "WHERE sp.OrganizationId = :organizationId AND sp.CompanyId = :companyId AND ISNULL(sp.IsTaxable, 0) = 0", nativeQuery = true)
    List<SupplierCustomerIdNameRow> getCustomerExistInExportInvoices(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierByImportPurchaseOrderNo' (native: ExImLcOrderPurchaseOrder, ImLcOrder; @ShipmentNo = LcOrderNo-DocNoByLcOrder; @FinancialYearId optional). */
    @Query(value = "SELECT Sp.Id, Sp.CompanyName, Sp.SupCustCode, sh.Id AS PurchaseOrderId, CONCAT(lc.LcOrderNo, '-', po.DocNoByLcOrder) AS DocNo, "
         + "CAST(sh.DocDate AS DATE) PoHDocDate, sh.DocumentTypeId "
         + "FROM dbo.SupplierCustomer AS Sp "
         + "INNER JOIN dbo.ExImLcOrderPurchaseOrder Sh ON Sp.Id = sh.SupplierCustomerId "
         + "INNER JOIN dbo.ExImLcOrderPurchaseOrder Po ON sh.DocNoByLcOrder = po.Id AND po.DocumentTypeId = 236 "
         + "INNER JOIN dbo.ImLcOrder lc ON po.ExImLcOrderId = lc.Id "
         + "WHERE sh.OrganizationId = :organizationId AND (sh.CompanyId = :companyId) AND (CONCAT(lc.LcOrderNo, '-', po.DocNoByLcOrder) = :shipmentNo) "
         + "AND sh.LcPoOrderStatus = 'Open' AND (sh.DocumentTypeId = :documentTypeId) AND (:financialYearId IS NULL OR Sh.FinancialYearId = :financialYearId) "
         + "AND ISNULL(sp.IsTaxable, 0) = 0 "
         + "GROUP BY Sp.Id, Sp.CompanyName, Sp.SupCustCode, sh.Id, sh.DocDate, sh.DocumentTypeId, lc.LcOrderNo, po.DocNoByLcOrder", nativeQuery = true)
    List<SupplierByImportPurchaseOrderRow> getSupplierByImportPurchaseOrderNo(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("shipmentNo") String shipmentNo, @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierCustomerFromOrderByInvoice' (distinct order suppliers; @DocumentTypeId optional). Native: PurchaseOrder.OrderSupCustId is not on the existing PurchaseOrder entity. */
    @Query(value = "SELECT DISTINCT h.OrderSupCustId, sp.CompanyName FROM dbo.PurchaseOrder h INNER JOIN dbo.SupplierCustomer sp ON h.OrderSupCustId = sp.Id "
         + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND (:documentTypeId IS NULL OR h.DocumentTypeId = :documentTypeId) "
         + "AND ISNULL(sp.IsTaxable, 0) = 0", nativeQuery = true)
    List<SupplierCustomerFromOrderRow> getSupplierCustomerFromOrderByInvoice(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("documentTypeId") Integer documentTypeId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetGlAccountIdandCompanyNameBySupplierCustomerId' (@SupplierCustomerId optional). */
    @Query("SELECT s.id AS id, s.glAccountId AS glAccountId, s.companyName AS companyName, s.advanceGlAcId AS advanceGlAcId FROM SupplierCustomer s "
         + "WHERE s.organizationId = :organizationId AND s.companyId = :companyId AND (:supplierCustomerId IS NULL OR s.id = :supplierCustomerId) "
         + "AND COALESCE(s.isTaxable, false) = false")
    List<SupplierCustomerGlAccountRow> getGlAccountIdandCompanyNameBySupplierCustomerId(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("supplierCustomerId") Integer supplierCustomerId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetGlAccountIdBySupplierCustomerId'. */
    @Query("SELECT s.glAccountId FROM SupplierCustomer s WHERE s.organizationId = :organizationId AND s.companyId = :companyId AND s.id = :supplierCustomerId "
         + "AND COALESCE(s.isTaxable, false) = false")
    List<Integer> getGlAccountIdBySupplierCustomerId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("supplierCustomerId") Integer supplierCustomerId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetSalesManForSupplierustomer' (CustomerGroupId 4). */
    @Query("SELECT s.id AS id, s.companyName AS companyName FROM SupplierCustomer s WHERE s.organizationId = :organizationId AND s.companyId = :companyId "
         + "AND s.customerGroupId = 4 AND COALESCE(s.isTaxable, false) = false")
    List<SupplierCustomerIdNameRow> getSalesManForSupplierustomer(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierustomerForPartyProcessing' (CustomerGroupId 12). */
    @Query("SELECT s.id AS id, s.companyName AS companyName FROM SupplierCustomer s WHERE s.organizationId = :organizationId AND s.companyId = :companyId "
         + "AND s.customerGroupId = 12 AND COALESCE(s.isTaxable, false) = false")
    List<SupplierCustomerIdNameRow> getSupplierustomerForPartyProcessing(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierCustomerAndCommissionAgentFromPurchaseAndSales' (native: UNION ALL over InvPurchaseInvoice / InvSaleInvoice; @ActionId 1=purchase 2=sales, @PartyType optional). */
    @Query(value = "SELECT DISTINCT * FROM ("
         + "SELECT DISTINCT pih.SupplierCustomerId AS Id, sp.CompanyName, sp.GlAccountId, 1 ActionId, 'Supplier' AS PartyType FROM InvPurchaseInvoice pih "
         + "INNER JOIN SupplierCustomer sp ON pih.SupplierCustomerId = sp.Id WHERE pih.OrganizationId = :organizationId AND pih.CompanyId = :companyId AND ISNULL(sp.IsTaxable, 0) = 0 "
         + "UNION ALL SELECT DISTINCT pih.CommissionAgentId AS Id, sp.CompanyName, sp.GlAccountId, 1 ActionId, 'CommissionAgent' AS PartyType FROM InvPurchaseInvoice pih "
         + "INNER JOIN SupplierCustomer sp ON pih.CommissionAgentId = sp.Id WHERE pih.OrganizationId = :organizationId AND pih.CompanyId = :companyId AND ISNULL(sp.IsTaxable, 0) = 0 "
         + "UNION ALL SELECT DISTINCT pih.BrokerAgentId AS Id, sp.CompanyName, sp.GlAccountId, 1 ActionId, 'BrokerAgent' AS PartyType FROM InvPurchaseInvoice pih "
         + "INNER JOIN SupplierCustomer sp ON pih.BrokerAgentId = sp.Id WHERE pih.OrganizationId = :organizationId AND pih.CompanyId = :companyId AND ISNULL(sp.IsTaxable, 0) = 0 "
         + "UNION ALL SELECT DISTINCT pih.SupplierCustomerId AS Id, sp.CompanyName, sp.GlAccountId, 2 ActionId, 'Customer' AS PartyType FROM InvSaleInvoice pih "
         + "INNER JOIN SupplierCustomer sp ON pih.SupplierCustomerId = sp.Id WHERE pih.OrganizationId = :organizationId AND pih.CompanyId = :companyId AND ISNULL(sp.IsTaxable, 0) = 0 "
         + "UNION ALL SELECT DISTINCT pih.CommissionAgentId AS Id, sp.CompanyName, sp.GlAccountId, 2 ActionId, 'CommissionAgent' AS PartyType FROM InvSaleInvoice pih "
         + "INNER JOIN SupplierCustomer sp ON pih.CommissionAgentId = sp.Id WHERE pih.OrganizationId = :organizationId AND pih.CompanyId = :companyId AND ISNULL(sp.IsTaxable, 0) = 0"
         + ") a WHERE (:actionId IS NULL OR a.ActionId = :actionId) AND (:partyType IS NULL OR a.PartyType = :partyType)", nativeQuery = true)
    List<SupplierCustomerPartyTypeRow> getSupplierCustomerAndCommissionAgentFromPurchaseAndSales(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("actionId") Integer actionId, @Param("partyType") String partyType);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierCustomerInfoByGLAccountId'. */
    @Query("SELECT s.id AS id, s.companyName AS companyName, s.firstName AS firstName, s.lastName AS lastName, s.reportingTitle AS reportingTitle, "
         + "s.mobileOffice AS mobileOffice, s.mobilePersonal AS mobilePersonal, s.email AS email FROM SupplierCustomer s "
         + "WHERE s.glAccountId = :glAccountId AND s.organizationId = :organizationId AND s.companyId = :companyId AND COALESCE(s.isTaxable, false) = false")
    List<SupplierCustomerInfoRow> getSupplierCustomerInfoByGLAccountId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("glAccountId") Integer glAccountId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='ContractorHistory' (native: GetConfigurationByConfigId, UserProfile; CustomerGroupId 8. NB the proc joins MUsr on EntryUser, not ModifyUser - kept). */
    @Query(value = "SELECT DISTINCT sp.Id, sp.CompanyName, sp.Address1, sp.MobilePersonal, sp.Title, sp.CNIC, sp.Email, sp.NTN_No, sp.Status, "
         + "sp.EntryUser, sp.EntryDate, CONCAT(Usr.FirstName, '-', Usr.LastName) AS EntryUserName, "
         + "sp.ModifyUser, sp.ModifyDate, CONCAT(MUsr.FirstName, '-', MUsr.LastName) AS ModifyUserName, "
         + "CASE WHEN ISNULL(sp.PictureURL, '') = '' THEN dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + sp.PictureURL END PictureURL, "
         + "CASE WHEN ISNULL(entryUserProfile.ProfileImageFileName, '') = '' THEN dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + entryUserProfile.ProfileImageFileName END EntryUserProfileImageName, "
         + "CASE WHEN ISNULL(modifyUserProfile.ProfileImageFileName, '') = '' THEN dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + modifyUserProfile.ProfileImageFileName END ModifyUserProfileImageName "
         + "FROM SupplierCustomer sp "
         + "INNER JOIN UserAccount Usr ON sp.EntryUser = Usr.ID "
         + "LEFT JOIN UserAccount MUsr ON sp.EntryUser = MUsr.ID "
         + "LEFT JOIN UserProfile entryUserProfile ON entryUserProfile.UserId = Usr.ID "
         + "LEFT JOIN UserProfile modifyUserProfile ON modifyUserProfile.UserId = MUsr.ID "
         + "WHERE sp.OrganizationId = :organizationId AND sp.CompanyId = :companyId AND sp.CustomerGroupId = 8 AND ISNULL(sp.IsTaxable, 0) = 0", nativeQuery = true)
    List<ContractorHistoryRow> contractorHistory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierustomerByCustomerGroupId' (native: fnSplitString over @CustomerGroupIds; @ParentId optional). */
    @Query(value = "SELECT DISTINCT Id, CompanyName, GlAccountId FROM dbo.SupplierCustomer "
         + "WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND (:customerGroupIds IS NULL OR CustomerGroupId IN (SELECT data FROM dbo.fnSplitString(:customerGroupIds, ','))) "
         + "AND (:parentId IS NULL OR ParentsSupCustId = :parentId) AND ISNULL(SupplierCustomer.IsTaxable, 0) = 0", nativeQuery = true)
    List<SupplierCustomerIdNameGlRow> getSupplierustomerByCustomerGroupId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("customerGroupIds") String customerGroupIds, @Param("parentId") Integer parentId);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='SupplierCustomerDataUpdateById' (UPDATE MobilePersonal of a non-taxable party). */
    @Modifying
    @Transactional
    @Query("UPDATE SupplierCustomer s SET s.mobilePersonal = :mobilePersonal "
         + "WHERE s.organizationId = :organizationId AND s.companyId = :companyId AND s.id = :id AND COALESCE(s.isTaxable, false) = false")
    int supplierCustomerDataUpdateById(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("id") Integer id,
            @Param("mobilePersonal") String mobilePersonal);

    /** Sp_SupplierCustomer_GetAllMethod @Activity='GetSameGLAccountPartiesByPartyId' (all non-taxable parties sharing the GlAccountId of party @Id). */
    @Query("SELECT DISTINCT s.id AS id, s.companyName AS companyName, s.glAccountId AS glAccountId FROM SupplierCustomer s "
         + "WHERE s.organizationId = :organizationId AND s.companyId = :companyId "
         + "AND s.glAccountId = (SELECT p.glAccountId FROM SupplierCustomer p WHERE p.id = :id) AND COALESCE(s.isTaxable, false) = false")
    List<SupplierCustomerIdNameGlRow> getSameGLAccountPartiesByPartyId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("id") Integer id);
}
