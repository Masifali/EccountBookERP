package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.Company;
import com.mst.projections.CompanyFormHistoryRow;
import com.mst.projections.CompanyGetHistoryRow;
import com.mst.projections.CompanyNewlyGeneratedRow;
import com.mst.projections.CompanyReadAllRow;
import com.mst.projections.CompanyReadByOrganizationIdRow;
import com.mst.projections.ErpFeatureRow;

/**
 * dbo.Company - Sp_Company_GetAllMethod (@Activity branches) and USP_GetERPFeaturesByCompanyId.
 * Every Sp_Company_GetAllMethod branch is native: four of them call the scalar function dbo.GetConfigurationByConfigId
 * (attachment folder URL, config 91) and the others join Organization / NewlyGeneratedOrganizationInfo, which have no entity.
 */
@Repository
public interface CompanyRepository extends JpaRepository<Company, Integer> {

    /**
     * Sp_Company_GetAllMethod @Activity='ReadByOrganizationId' (non-head-office companies of the organization, optionally one @Id).
     * The proc lists CompLogoImage twice; the duplicate column is emitted once here.
     */
    @Query(value = "SELECT [Id], [CompCode], [CompName], [CompContactPerson], [CompEmailA], [CompEmailB], [CompTel], [CompMobileA], [CompMobileB], [CompMobileC], "
         + "[CompAddress], [CompReportingTitle], [CompLogo], [CompCountry], [CompState], [CompBaseCurr], [CompType], [OrgCompanyTypeId], "
         + "CASE WHEN ISNULL(PictureURL, '') = '' THEN '../../../../assets/images/Administration/companyDummyLogo.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:orgCompanyTypeId, :id, 91) + [PictureURL] END PictureURL, "
         + "[CompLogoImage], EntryDate, EntryUser, ModifyDate, ModifyUser, [CityName], CompanyTemplateId, [AllowedUserCount], [CompanyTypeId], [IsHeadOffice], "
         + "[CompanyNameOtherLanguage], [CompanyAddressOtherLanguage], [CompanyWebsite], [CompanyFaxNo], [CompanyNameOtherLing], [CompanyAddressOtherLing], CompanyId "
         + "FROM [dbo].[Company] "
         + "WHERE Company.OrgCompanyTypeId = :orgCompanyTypeId AND (:id IS NULL OR Company.Id = :id) AND ISNULL(Company.IsHeadOffice, 0) <> 1", nativeQuery = true)
    List<CompanyReadByOrganizationIdRow> readByOrganizationId(@Param("orgCompanyTypeId") Integer orgCompanyTypeId, @Param("id") Integer id);

    /** Sp_Company_GetAllMethod @Activity='ReadAll' (all companies; PictureURL built with dbo.GetConfigurationByConfigId(@OrgCompanyTypeId, Id, 91)). */
    @Query(value = "SELECT [Id], [CompName], "
         + "CASE WHEN ISNULL(PictureURL, '') = '' THEN dbo.GetConfigurationByConfigId(:orgCompanyTypeId, Id, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:orgCompanyTypeId, Id, 91) + PictureURL END PictureURL "
         + "FROM [dbo].[Company]", nativeQuery = true)
    List<CompanyReadAllRow> readAll(@Param("orgCompanyTypeId") Integer orgCompanyTypeId);

    /** Sp_Company_GetAllMethod @Activity='FormHistory' (native: Organization; UserAccount matched on company + organization + UserName = CompContactPerson). @OrgCompanyTypeId optional. */
    @Query(value = "SELECT d.[Id], d.[OrgCompanyTypeId], h.OrgName, d.[CompCode], d.[CompName], d.[CompContactPerson], d.[CompEmailA], d.[CompEmailB], d.[CompTel], "
         + "d.[CompMobileA], d.[CompMobileB], d.[CompMobileC], d.[CompAddress], d.[CompReportingTitle], d.[CompLogo], d.[CompCountry], d.[CompState], "
         + "d.[CompBaseCurr], d.[CompType], d.[PictureURL], d.[EntryUser], d.[ModifyUser], d.[EntryDate], d.[ModifyDate], d.[AllowedUserCount], "
         + "d.[CompLogoImage], d.[CompanyTypeId], d.[IsHeadOffice], d.[CompanyNameOtherLanguage], d.[CompanyAddressOtherLanguage], d.[CompanyWebsite], "
         + "d.[CompanyFaxNo], d.[CompanyNameOtherLing], d.[CompanyAddressOtherLing], d.[CompanyTemplateId], d.[CityName], u.FirstName, U.LastName, d.CompanyId "
         + "FROM [dbo].[Company] d "
         + "INNER JOIN [dbo].[Organization] h ON h.Id = d.OrgCompanyTypeId "
         + "LEFT JOIN UserAccount u ON u.companyId = d.Id AND U.OrganizationId = d.OrgCompanyTypeId AND U.UserName = d.CompContactPerson "
         + "WHERE (:orgCompanyTypeId IS NULL OR d.OrgCompanyTypeId = :orgCompanyTypeId)", nativeQuery = true)
    List<CompanyFormHistoryRow> formHistory(@Param("orgCompanyTypeId") Integer orgCompanyTypeId);

    /** Sp_Company_GetAllMethod @Activity='GetHistory' */
    @Query(value = "SELECT c.Id, OrgCompanyTypeId, CompCode, CompName, CompContactPerson, CompEmailA, CompEmailB, CompTel, CompMobileA, CompMobileB, CompMobileC, CompAddress, "
         + "c.CompReportingTitle, CompLogo, CompCountry, CompState, CompBaseCurr, CompType, EntryUser, EntryDate, ModifyUser, ModifyDate, AllowedUserCount, "
         + "CASE WHEN ISNULL(c.PictureURL, '') = '' THEN dbo.GetConfigurationByConfigId(:orgCompanyTypeId, C.Id, 91) + 'DummyProfilePicture.jpg' "
         + "ELSE dbo.GetConfigurationByConfigId(:orgCompanyTypeId, C.Id, 91) + c.PictureURL END PictureURL, c.CompanyId "
         + "FROM Company c WHERE c.OrgCompanyTypeId = :orgCompanyTypeId", nativeQuery = true)
    List<CompanyGetHistoryRow> getHistory(@Param("orgCompanyTypeId") Integer orgCompanyTypeId);

    /** Sp_Company_GetAllMethod @Activity='GetNewlyGeneratedCompanyData' (native: NewlyGeneratedOrganizationInfo). */
    @Query(value = "SELECT o.OrganizationId, o.CompanyId, c.CompName, o.FinancialYearId "
         + "FROM NewlyGeneratedOrganizationInfo o WITH (NOLOCK) INNER JOIN Company c WITH (NOLOCK) ON c.Id = o.CompanyId "
         + "WHERE c.Id = :id", nativeQuery = true)
    List<CompanyNewlyGeneratedRow> getNewlyGeneratedCompanyData(@Param("id") Integer id);

    /** USP_GetERPFeaturesByCompanyId (single branch; native: ERPConfigurations, ERPFeatures). */
    @Query(value = "SELECT DISTINCT C.FeaturesId AS Id, f.ERPFeatures FROM ERPConfigurations c INNER JOIN ERPFeatures f ON c.FeaturesId = f.Id "
         + "WHERE C.OrganizationId = :organizationId AND C.CompanyId = :companyId AND C.IsActive = 1", nativeQuery = true)
    List<ErpFeatureRow> uspGetERPFeaturesByCompanyId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);
}
