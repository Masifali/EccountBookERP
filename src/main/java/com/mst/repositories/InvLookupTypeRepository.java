package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.InvLookupType;

@Repository
public interface InvLookupTypeRepository extends JpaRepository<InvLookupType, Integer> {

    /** Proc_InvLookupType_ReadAll (single branch). @OrganizationId / @CompanyId are declared but the WHERE is commented out in the proc, so no filter is applied. */
    @Query("SELECT t FROM InvLookupType t")
    List<InvLookupType> readAll();

    /**
     * Proc_InvLookUpType_GenerateCodeByLookUpType (single branch).
     * Returns min(InvLookupType.SerialFrom) when the type has no InvLookUp rows yet, otherwise max(Code)+1.
     * @OrganizationId / @CompanyID are declared but their filters are commented out in the proc.
     */
    @Query(value = "SELECT CASE WHEN (MAX(e.Code) IS NULL) "
         + "THEN (SELECT MIN(a.SerialFrom) FROM InvLookupType a WHERE a.id = :invLookupTypeId) "
         + "ELSE ISNULL(MAX(CONVERT(int, e.Code)), 0) + 1 END AS Code "
         + "FROM InvLookUp e WHERE e.InvLookupTypeId = :invLookupTypeId", nativeQuery = true)
    Integer generateCodeByLookUpType(@Param("invLookupTypeId") Integer invLookupTypeId);
}
