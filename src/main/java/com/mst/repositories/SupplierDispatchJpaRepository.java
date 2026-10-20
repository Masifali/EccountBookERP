package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.SupplierDispatch;
import com.mst.projections.SupplierDispatchNoForGpGrnRow;

/**
 * Repository for dbo.SupplierDispatch (named {@code ...JpaRepository} because the project already has
 * {@code SupplierDispatchRepository}).
 */
@Repository
public interface SupplierDispatchJpaRepository extends JpaRepository<SupplierDispatch, Integer> {

    /**
     * USP_SupplierDispatch_GetNoForGpandGrn (single branch). Defaults: gpId = null, gpRecId = null, grnRecId = null,
     * orderId = null.
     * <p>
     * Temp-table pipeline rewritten as one statement: {@code #Data} = open dispatches of the supplier not yet used by a
     * GatePassInward / InvGrn (other than the records being edited), filtered by order mapping; the
     * {@code IF ISNULL(@GpId,0) > 0 INSERT INTO #Data ...} step is the second UNION ALL leg (dispatch already linked to the
     * gate pass {@code gpId}). The proc's {@code @OrderMapingDispatch} flag (1 when the order has NO dispatch mapping with
     * ActionId &lt;&gt; 3, else NULL) is inlined as EXISTS / NOT EXISTS of the same sub-query. Output identical to the proc's
     * final {@code SELECT DISTINCT ... CONVERT(float, AdvanceFreight) AdvanceFreight ...}.
     */
    @Query(value = "SELECT DISTINCT h.SupplierDispatchId, h.SupplierDispatchNo, h.VehicleNo, "
         + "CONVERT(float, h.AdvanceFreight) AdvanceFreight, h.BiltyNo, h.Freight, h.SupplierWbWeight, h.CityId "
         + "FROM ( "
         + "  SELECT DISTINCT h.Id SupplierDispatchId, h.DocNo AS SupplierDispatchNo, h.VehicleNo, "
         + "         h.AdvanceFreight, h.BiltyNo, h.Freight, h.SupplierWbWeight, h.CityId "
         + "  FROM dbo.SupplierDispatch h "
         + "  INNER JOIN dbo.SupplierDispatchDetail D ON D.SupplierDispatchId = h.Id "
         + "  WHERE h.OrganizationId = :organizationId "
         + "  AND h.CompanyId = :companyId "
         + "  AND d.SupplierId = :supplierId "
         + "  AND h.Status = 'Open' "
         + "  AND NOT EXISTS (SELECT 1 FROM GatePassInward gp "
         + "                  WHERE gp.SupplierDispatchId = h.Id "
         + "                  AND (:gpRecId IS NULL OR gp.Id <> :gpRecId)) "
         + "  AND NOT EXISTS (SELECT 1 FROM InvGrn grn "
         + "                  WHERE grn.SupplierDispatchId = h.Id "
         + "                  AND (:grnRecId IS NULL OR grn.Id <> :grnRecId) "
         + "                  AND (:gpRecId IS NULL OR grn.InwardGatePassId <> :gpRecId)) "
         + "  AND ( "
         + "        (:orderId IS NULL) "
         + "        OR "
         + "        (EXISTS (SELECT 1 FROM PurchaseOrder po "
         + "                 INNER JOIN PurchaseOrderSupplierDispatchDetail pod ON pod.PurchaseOrderId = po.Id "
         + "                 INNER JOIN SupplierDispatch S ON pod.SupplierDispatchId = S.Id AND S.ActionId <> 3 "
         + "                 WHERE po.OrganizationId = :organizationId AND po.CompanyId = :companyId "
         + "                 AND pod.SupplierDispatchId = S.Id AND pod.PurchaseOrderId = :orderId) "
         + "         AND EXISTS (SELECT 1 FROM PurchaseOrder po "
         + "                     INNER JOIN PurchaseOrderSupplierDispatchDetail pod ON pod.PurchaseOrderId = po.Id "
         + "                     WHERE po.OrganizationId = :organizationId AND po.CompanyId = :companyId "
         + "                     AND pod.SupplierDispatchId = h.Id AND pod.PurchaseOrderId = :orderId)) "
         + "        OR "
         + "        (NOT EXISTS (SELECT 1 FROM PurchaseOrder po "
         + "                     INNER JOIN PurchaseOrderSupplierDispatchDetail pod ON pod.PurchaseOrderId = po.Id "
         + "                     INNER JOIN SupplierDispatch S ON pod.SupplierDispatchId = S.Id AND S.ActionId <> 3 "
         + "                     WHERE po.OrganizationId = :organizationId AND po.CompanyId = :companyId "
         + "                     AND pod.SupplierDispatchId = S.Id AND pod.PurchaseOrderId = :orderId) "
         + "         AND NOT EXISTS (SELECT 1 FROM PurchaseOrder po "
         + "                         INNER JOIN PurchaseOrderSupplierDispatchDetail pod ON pod.PurchaseOrderId = po.Id "
         + "                         WHERE po.OrganizationId = :organizationId AND po.CompanyId = :companyId "
         + "                         AND pod.SupplierDispatchId = h.Id)) "
         + "      ) "
         + "  UNION ALL "
         + "  SELECT h.SupplierDispatchId, D.DocNo, h.VehicleNo, D.AdvanceFreight, d.BiltyNo, d.Freight, d.SupplierWbWeight, d.CityId "
         + "  FROM GatePassInward h "
         + "  INNER JOIN dbo.SupplierDispatch D ON D.Id = h.SupplierDispatchId "
         + "  WHERE ISNULL(:gpId, 0) > 0 AND h.Id = :gpId "
         + ") h", nativeQuery = true)
    List<SupplierDispatchNoForGpGrnRow> getNoForGpAndGrn(@Param("organizationId") Integer organizationId,
                                                         @Param("companyId") Integer companyId,
                                                         @Param("supplierId") Integer supplierId,
                                                         @Param("gpId") Integer gpId,
                                                         @Param("gpRecId") Integer gpRecId,
                                                         @Param("grnRecId") Integer grnRecId,
                                                         @Param("orderId") Integer orderId);
}
