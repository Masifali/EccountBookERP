package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.TaxesTypes;
import com.mst.projections.TaxesTypeComboRow;
import com.mst.projections.TaxesTypeRow;

/** dbo.TaxesTypes - Sp_TaxesTypes_GetAllMethod (@Activity branches). */
@Repository
public interface TaxesTypesRepository extends JpaRepository<TaxesTypes, Integer> {

    /** Sp_TaxesTypes_GetAllMethod @Activity='ReadById' (all entity columns). */
    @Query("SELECT t FROM TaxesTypes t WHERE t.id = :id")
    List<TaxesTypes> readById(@Param("id") Integer id);

    /** Sp_TaxesTypes_GetAllMethod @Activity='ReadByOrganizationCompanyId' (INNER JOIN ChartofAccount on TaxGLAccountId). */
    @Query("SELECT t.id AS id, t.taxName AS taxName, t.taxDescription AS taxDescription, t.taxGLAccountId AS taxGLAccountId, t.type AS type, "
         + "t.entryDate AS entryDate, t.entryUser AS entryUser, t.modifyDate AS modifyDate, t.modifyUser AS modifyUser, "
         + "t.organizationId AS organizationId, t.companyId AS companyId, c.accountTitle AS accountTitle "
         + "FROM TaxesTypes t INNER JOIN ChartofAccount c ON t.taxGLAccountId = c.id "
         + "WHERE t.organizationId = :organizationId AND t.companyId = :companyId")
    List<TaxesTypeRow> readByOrganizationCompanyId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_TaxesTypes_GetAllMethod @Activity='ReadByCombo' (native: TaxScheduleMain has no entity; only tax types with a posted schedule; @Type optional). */
    @Query(value = "SELECT TaxesTypes.Id, TaxesTypes.TaxName FROM TaxesTypes "
         + "INNER JOIN ChartofAccount ON TaxesTypes.TaxGLAccountId = ChartofAccount.Id "
         + "INNER JOIN TaxScheduleMain m ON TaxesTypes.Id = m.TaxNameId "
         + "WHERE TaxesTypes.OrganizationId = :organizationId AND TaxesTypes.CompanyId = :companyId "
         + "AND (:type IS NULL OR TaxesTypes.Type = :type) AND (m.PostState = 1)", nativeQuery = true)
    List<TaxesTypeComboRow> readByCombo(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("type") Integer type);
}
