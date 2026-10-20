package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ConfigrationsAllocation;
import com.mst.projections.ConfigKeyValueRow;
import com.mst.projections.ConfigurationRow;

/**
 * dbo.ConfigrationsAllocation - Sp_ConfigrationsAllocation_GetAllMethod (@Activity branches). All three branches join
 * dbo.ConfigrationsDefinition (no entity) and two use fnSplitString, so they are native.
 */
@Repository
public interface ConfigrationsAllocationRepository extends JpaRepository<ConfigrationsAllocation, Integer> {

    /** Sp_ConfigrationsAllocation_GetAllMethod @Activity='GetConfigurationByOrgCompandConfigDescription' (the ConfigKey text of one configuration). */
    @Query(value = "SELECT AC.ConfigKey FROM ConfigrationsAllocation AC INNER JOIN ConfigrationsDefinition CD ON AC.ConfigrationsDefinitionId = CD.Id "
         + "WHERE OrganizationId = :organizationId AND CompanyId = :companyId AND CD.ConfigDescription = :configDescription", nativeQuery = true)
    List<String> getConfigurationByOrgCompandConfigDescription(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("configDescription") String configDescription);

    /** Sp_ConfigrationsAllocation_GetAllMethod @Activity='GetMultipleConfigurationsByConfigDescriptions' (@ConfigDescription is a CSV of descriptions). */
    @Query(value = "SELECT CD.ConfigDescription, AC.ConfigKey FROM ConfigrationsAllocation AC INNER JOIN ConfigrationsDefinition CD ON AC.ConfigrationsDefinitionId = CD.Id "
         + "WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND CD.ConfigDescription IN (SELECT data FROM dbo.fnSplitString(:configDescription, ','))", nativeQuery = true)
    List<ConfigurationRow> getMultipleConfigurationsByConfigDescriptions(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("configDescription") String configDescription);

    /** Sp_ConfigrationsAllocation_GetAllMethod @Activity='GetConfigurationsByDefinitionIds' (@DefinitionIds is a CSV of ConfigrationsDefinition ids). */
    @Query(value = "SELECT CD.ConfigDescription, AC.ConfigKey, AC.ConfigValue FROM ConfigrationsAllocation AC INNER JOIN ConfigrationsDefinition CD ON AC.ConfigrationsDefinitionId = CD.Id "
         + "WHERE AC.OrganizationId = :organizationId AND AC.CompanyId = :companyId "
         + "AND CD.Id IN (SELECT data FROM dbo.fnSplitString(:definitionIds, ','))", nativeQuery = true)
    List<ConfigKeyValueRow> getConfigurationsByDefinitionIds(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("definitionIds") String definitionIds);

    /** Helper used by Sp_SupplierCustomer_GetAllMethod ('ReadByOrganizationIdCompanyIdForBinding' / 'AllSupplierCustomerWithCityName'): ConfigKey of ConfigrationsDefinitionId 227. */
    @Query("SELECT c.configKey FROM ConfigrationsAllocation c WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND c.configrationsDefinitionId = :configrationsDefinitionId")
    List<String> configKeyByDefinitionId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("configrationsDefinitionId") Integer configrationsDefinitionId);
}
