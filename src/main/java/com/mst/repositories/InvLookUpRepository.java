package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InvLookUp;
import com.mst.projections.InvLookUpNameRow;
import com.mst.projections.InvLookUpRow;

/**
 * Proc_InvLookup_Insert -> {@code save()}. Pre-insert logic in the proc: Id = max(Id)+1 over the whole table
 * (use {@link #maxId()}); ModifyUser / ModifyDate are NOT inserted; proc returns the new @Id (caller reads it
 * from the saved entity).
 */
@Repository
public interface InvLookUpRepository extends JpaRepository<InvLookUp, Integer> {

    /** Proc_InvLookup_Insert: next app-assigned Id = maxId()+1 */
    @Query("select coalesce(max(e.id),0) from InvLookUp e")
    Integer maxId();

    /** Proc_InvLookup_ReadById (single branch) */
    @Query("SELECT l.id AS id, l.invLookupTypeId AS invLookupTypeId, l.code AS code, l.lookupName AS lookupName, "
         + "l.entryUser AS entryUser, l.entryDate AS entryDate, l.modifyUser AS modifyUser, l.modifyDate AS modifyDate, "
         + "l.postDate AS postDate, l.postUser AS postUser, l.postState AS postState, l.organizationId AS organizationId, "
         + "l.companyId AS companyId, t.profileTypeName AS profileTypeName "
         + "FROM InvLookUp l JOIN InvLookupType t ON l.invLookupTypeId = t.id "
         + "WHERE l.id = :id")
    List<InvLookUpRow> readById(@Param("id") Integer id);

    /** Sp_InvLookup_GetAllMethod @Activity='ReadByInvlookTypeId' (@OrganizationId/@CompanyId filters are commented out in the proc) */
    @Query("SELECT l.id AS id, l.invLookupTypeId AS invLookupTypeId, l.code AS code, l.lookupName AS lookupName, "
         + "l.entryUser AS entryUser, l.entryDate AS entryDate, l.modifyUser AS modifyUser, l.modifyDate AS modifyDate, "
         + "l.postDate AS postDate, l.postUser AS postUser, l.postState AS postState, l.organizationId AS organizationId, "
         + "l.companyId AS companyId, t.profileTypeName AS profileTypeName "
         + "FROM InvLookUp l JOIN InvLookupType t ON l.invLookupTypeId = t.id "
         + "WHERE l.invLookupTypeId = :invLookupTypeId")
    List<InvLookUpRow> readByInvlookTypeId(@Param("invLookupTypeId") Integer invLookupTypeId);

    /** Sp_InvLookup_GetAllMethod @Activity='ReadAll' (no filter; @OrganizationId/@CompanyId commented out in the proc) */
    @Query("SELECT l.id AS id, l.lookupName AS lookupName, l.invLookupTypeId AS invLookupTypeId "
         + "FROM InvLookUp l JOIN InvLookupType t ON l.invLookupTypeId = t.id")
    List<InvLookUpNameRow> readAll();

    /** Proc_InvLookup_Update (single branch): every SET column of the proc; EntryUser/EntryDate are not updated. */
    @Modifying
    @Transactional
    @Query("UPDATE InvLookUp l SET l.invLookupTypeId = :invLookupTypeId, l.code = :code, l.modifyUser = :modifyUser, "
         + "l.modifyDate = :modifyDate, l.postDate = :postDate, l.postUser = :postUser, l.postState = :postState, "
         + "l.organizationId = :organizationId, l.companyId = :companyId, l.lookupName = :lookupName "
         + "WHERE l.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("invLookupTypeId") Integer invLookupTypeId,
                   @Param("code") Integer code,
                   @Param("lookupName") String lookupName,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("modifyDate") String modifyDate,
                   @Param("postDate") LocalDateTime postDate,
                   @Param("postUser") Integer postUser,
                   @Param("postState") Boolean postState,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId);
}
