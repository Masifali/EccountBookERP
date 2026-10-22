package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.MultiLanguages;
import com.mst.projections.MultiLanguagesRow;
import com.mst.projections.UnallocatedAccountRow;

@Repository
public interface MultiLanguagesRepository extends JpaRepository<MultiLanguages, Integer> {

    /** Sp_MultiLanguages_GetAll @MethodType='ReadAll' (native: dbo.GetConfigurationByConfigId(...,91) builds FlagUrl) */
    @Query(value = "select Id, LanguageName, LanguageDescription, "
         + "(select dbo.GetConfigurationByConfigId(:organizationId, :companyId, 91) + MultiLanguages.FlagImage) FlagUrl, RTL "
         + "from MultiLanguages", nativeQuery = true)
    List<MultiLanguagesRow> readAll(@Param("organizationId") Integer organizationId,
                                    @Param("companyId") Integer companyId);

    /** Sp_MultiLanguages_GetAll @MethodType='GetUnAllocatedAccountsByLanguageId' (native: ChartOfAccountMultiLingo has no entity) */
    @Query(value = "Select Distinct coa.Id DefaultTitleId, coa.AccountTitle DefaultTitle from ChartofAccount coa "
         + "where coa.OrganizationId = :organizationId and coa.CompanyId = :companyId "
         + "AND (coa.Id NOT IN (SELECT ChartOfAccountId FROM ChartOfAccountMultiLingo WHERE MultiLanguagesId = :languageId))",
         nativeQuery = true)
    List<UnallocatedAccountRow> getUnAllocatedAccountsByLanguageId(@Param("organizationId") Integer organizationId,
                                                                   @Param("companyId") Integer companyId,
                                                                   @Param("languageId") Integer languageId);
}
