package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.GatePassType;
import com.mst.projections.GatePassTypeRow;

/**
 * dbo.GatePassType - Sp_GatePassType_GetAllMethod (@Activity branches). Four branches UNION ALL fixed rows that only appear when
 * related documents exist (PurchaseOrder, ExImLcOrderPurchaseOrder, ExImInvoice, InvDeliveryOrder, SaleOrder) - native.
 */
@Repository
public interface GatePassTypeRepository extends JpaRepository<GatePassType, Integer> {

    /** Sp_GatePassType_GetAllMethod @Activity='Inward' (types 3,4,5,13 + 'Govt Purchase' (15) when a category-8 PurchaseOrder exists + 'Import' (12) when an ExImLcOrderPurchaseOrder exists). */
    @Query(value = "SELECT Id, GpTypeDescription FROM GatePassType WHERE Id IN (3, 4, 5, 13) "
         + "UNION ALL SELECT 15 AS Id, 'Govt Purchase' AS GpTypeDescription WHERE EXISTS (SELECT 1 FROM PurchaseOrder WHERE OrganizationId = :organizationId AND CompanyId = :companyId AND OrderCatagoryId = 8) "
         + "UNION ALL SELECT 12 AS Id, 'Import' AS GpTypeDescription WHERE EXISTS (SELECT 1 FROM ExImLcOrderPurchaseOrder WHERE OrganizationId = :organizationId AND CompanyId = :companyId)",
           nativeQuery = true)
    List<GatePassTypeRow> inward(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_GatePassType_GetAllMethod @Activity='Inward_Engr' (types 3,4,5,7,11,13 + 'Import' (12) when an ExImLcOrderPurchaseOrder exists). */
    @Query(value = "SELECT Id, GpTypeDescription FROM GatePassType WHERE Id IN (3, 4, 5, 7, 11, 13) "
         + "UNION ALL SELECT 12 AS Id, 'Import' AS GpTypeDescription WHERE (SELECT COUNT(id) FROM ExImLcOrderPurchaseOrder WHERE OrganizationId = :organizationId AND CompanyId = :companyId) > 0",
           nativeQuery = true)
    List<GatePassTypeRow> inward_Engr(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_GatePassType_GetAllMethod @Activity='Outward' (types 3,4,5,13 + 'Export' (9) / 'Stock Transfer' (11) / type 6 / type 7 depending on ExImInvoice, InvDeliveryOrder and SaleOrder rows). */
    @Query(value = "SELECT Id, GpTypeDescription FROM GatePassType WHERE Id IN (3, 4, 5, 13) "
         + "UNION ALL SELECT 9 AS Id, 'Export' AS GpTypeDescription WHERE (SELECT COUNT(id) FROM ExImInvoice WHERE OrganizationId = :organizationId AND CompanyId = :companyId) > 0 "
         + "UNION ALL SELECT 11 AS Id, 'Stock Transfer' AS GpTypeDescription WHERE (SELECT COUNT(id) FROM InvDeliveryOrder WHERE OrganizationId = :organizationId AND CompanyId = :companyId AND DeliveryOrderType = 'StockTransfer') > 0 "
         + "UNION ALL SELECT Id, GpTypeDescription FROM GatePassType WHERE Id = 6 AND (SELECT COUNT(id) FROM SaleOrder WHERE OrganizationId = :organizationId AND CompanyId = :companyId AND OrderCatagoryId = 9) > 0 "
         + "UNION ALL SELECT Id, GpTypeDescription FROM GatePassType WHERE Id = 7 AND (SELECT COUNT(id) FROM SaleOrder WHERE OrganizationId = :organizationId AND CompanyId = :companyId AND OrderCatagoryId = 2) > 0",
           nativeQuery = true)
    List<GatePassTypeRow> outward(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** Sp_GatePassType_GetAllMethod @Activity='OutwardReturnable' (types 2 and 14; no organization/company dependency). */
    @Query("SELECT g FROM GatePassType g WHERE g.id IN (2, 14)")
    List<GatePassType> outwardReturnable();

    /** Sp_GatePassType_GetAllMethod @Activity='WeighBridge' (types 1,3,4,5,11,13 + 'Export' (9) / 'Import' (12) when ExImInvoice / ExImLcOrderPurchaseOrder rows exist). */
    @Query(value = "SELECT Id, GpTypeDescription FROM GatePassType WHERE Id IN (1, 3, 4, 5, 11, 13) "
         + "UNION ALL SELECT 9 AS Id, 'Export' AS GpTypeDescription WHERE (SELECT COUNT(id) FROM ExImInvoice WHERE OrganizationId = :organizationId AND CompanyId = :companyId) > 0 "
         + "UNION ALL SELECT 12 AS Id, 'Import' AS GpTypeDescription WHERE (SELECT COUNT(id) FROM ExImLcOrderPurchaseOrder WHERE OrganizationId = :organizationId AND CompanyId = :companyId) > 0",
           nativeQuery = true)
    List<GatePassTypeRow> weighBridge(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);
}
