package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InwardGatePassDetail;
import com.mst.projections.GatePassInwardDetailReadAllRow;
import com.mst.projections.GatePassInwardDetailReadByIdRow;

/**
 * dbo.GatePassInwardDetail — source procedures:
 * Sp_GatePassInwardDetail_GetAllMethod (ReadAll / ReadById) and Sp_GatePassInwardDetail_Insert.
 *
 * Pre-insert logic of Sp_GatePassInwardDetail_Insert (callers use save()):
 *  - Id = ISNULL(MAX(CONVERT(int, Id)), 0) + 1 over the whole table (app-assigned; see maxId()).
 *    NOTE: the existing entity keeps @GeneratedValue(IDENTITY) per conventions (never change @Id strategy);
 *    if the column is not an IDENTITY in the database the caller must set id = maxId() + 1 before save().
 *  - all other columns are inserted exactly as supplied (no defaults, no GETDATE()).
 *  - the proc returns SELECT @Id → with save() the caller reads it from the entity.
 */
@Repository
public interface InwardGatePassDetailRepository extends JpaRepository<InwardGatePassDetail, Integer> {

    /** Sp_GatePassInwardDetail_Insert: Id = ISNULL(MAX(CONVERT(int,Id)),0) + 1 (table-wide). */
    @Query("select coalesce(max(e.id),0) from InwardGatePassDetail e")
    Integer maxId();

    /**
     * Sp_GatePassInwardDetail_GetAllMethod @Activity='ReadAll' (no parameter used; whole table).
     * Native because City.Description is not mapped on the existing City entity.
     */
    @Query(value = "SELECT g.[Id], g.RefDocumentTypeId, g.[GatePassInwardId], g.[SupplyScheduleId], g.[ItemId], g.[CropYear], "
         + "g.[JobLotId], g.[ItemUOMId], g.[ItemQty], g.[WareHouseId], g.PurchaseOrderId, p.DocNo OrderNo, "
         + "g.PurchaseOrderDetailId, g.CityId, C.Description CityName, g.SupplierCustomerId, g.RemarksDetail "
         + "FROM [dbo].[GatePassInwardDetail] g "
         + "Left join PurchaseOrder p on p.Id = g.PurchaseOrderId "
         + "Left join City C on C.id = g.CityId", nativeQuery = true)
    List<GatePassInwardDetailReadAllRow> readAll();

    /** Sp_GatePassInwardDetail_GetAllMethod @Activity='ReadById' (@Id = GatePassInwardId; uses view V_UomScheduleAndUom). */
    @Query(value = "SELECT d.*, i.ItemName, pt.PackTypeDesc as PackingType, um.Equivalent, um.UOMCode, '' as JobLotDescription, "
         + "W.WareHouseName, 0 as DocNo, C.Description CityName, p.DocNo OrderNo "
         + "FROM GatePassInwardDetail d "
         + "inner join Item i on d.ItemId = i.Id "
         + "inner join V_UomScheduleAndUom Um on d.ItemUOMId = um.Id "
         + "inner join InvWareHouse W ON D.WareHouseId = W.Id "
         + "left join InvPackingType pt on d.PackingTypeId = pt.Id "
         + "Left join PurchaseOrder p on p.Id = d.PurchaseOrderId "
         + "Left join City C on C.id = d.CityId "
         + "where d.GatePassInwardId = :id", nativeQuery = true)
    List<GatePassInwardDetailReadByIdRow> readById(@Param("id") Integer id);

    /** Sp_GatePassInward_Update: "delete GatePassInwardDetail where GatePassInwardId=@Id" (standalone statement). */
    @Modifying
    @Transactional
    @Query("DELETE FROM InwardGatePassDetail d WHERE d.gatePassInwardId = :gatePassInwardId")
    int deleteByGatePassInwardId(@Param("gatePassInwardId") Integer gatePassInwardId);
}
