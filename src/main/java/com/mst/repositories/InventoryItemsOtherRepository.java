package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.InventoryItemsOther;
import com.mst.projections.InventoryItemsOtherRow;
import com.mst.projections.InventoryParentCategoryRow;

/**
 * Sp_InventoryItemsOther_GetAllMethod (@Activity = 'ReadAll' | 'ReadById' | 'InventoryParentCategories').
 * All proc parameters default to NULL.
 */
@Repository
public interface InventoryItemsOtherRepository extends JpaRepository<InventoryItemsOther, Integer> {

    /** Sp_InventoryItemsOther_GetAllMethod @Activity='ReadAll' */
    @Query("SELECT io.id AS id, io.otherItemName AS otherItemName, io.otherItemType AS otherItemType, "
         + "io.stockGLAcId AS stockGLAcId, sCoa.accountTitle AS accountTitleStock, "
         + "io.saleGLAcId AS saleGLAcId, rCoa.accountTitle AS accountTitleSales, "
         + "io.cogsGLAcId AS cogsGLAcId, cCoa.accountTitle AS accountTitleCgs, "
         + "io.companyId AS companyId, io.organizationId AS organizationId, io.branchesId AS branchesId, io.projectsId AS projectsId, "
         + "sCoa.accountTitle AS stockGLAc, rCoa.accountTitle AS saleGLAc, cCoa.accountTitle AS cogsGLAc "
         + "FROM InventoryItemsOther io "
         + "LEFT JOIN ChartofAccount sCoa ON io.stockGLAcId = sCoa.id "
         + "LEFT JOIN ChartofAccount rCoa ON io.saleGLAcId = rCoa.id "
         + "LEFT JOIN ChartofAccount cCoa ON io.cogsGLAcId = cCoa.id "
         + "WHERE io.organizationId = :organizationId AND io.companyId = :companyId")
    List<InventoryItemsOtherRow> readAll(@Param("organizationId") Integer organizationId,
                                         @Param("companyId") Integer companyId);

    /** Sp_InventoryItemsOther_GetAllMethod @Activity='ReadById' */
    @Query("SELECT io.id AS id, io.otherItemName AS otherItemName, io.otherItemType AS otherItemType, "
         + "io.stockGLAcId AS stockGLAcId, sCoa.accountTitle AS accountTitleStock, "
         + "io.saleGLAcId AS saleGLAcId, rCoa.accountTitle AS accountTitleSales, "
         + "io.cogsGLAcId AS cogsGLAcId, cCoa.accountTitle AS accountTitleCgs, "
         + "io.companyId AS companyId, io.organizationId AS organizationId, io.branchesId AS branchesId, io.projectsId AS projectsId, "
         + "sCoa.accountTitle AS stockGLAc, rCoa.accountTitle AS saleGLAc, cCoa.accountTitle AS cogsGLAc "
         + "FROM InventoryItemsOther io "
         + "LEFT JOIN ChartofAccount sCoa ON io.stockGLAcId = sCoa.id "
         + "LEFT JOIN ChartofAccount rCoa ON io.saleGLAcId = rCoa.id "
         + "LEFT JOIN ChartofAccount cCoa ON io.cogsGLAcId = cCoa.id "
         + "WHERE io.id = :id")
    List<InventoryItemsOtherRow> readById(@Param("id") Integer id);

    /**
     * Sp_InventoryItemsOther_GetAllMethod @Activity='InventoryParentCategories'.
     * Native: InventoryParentCategories has no entity; @Ids is a comma-separated list split by dbo.fnSplitString (NULL = all).
     */
    @Query(value = "SELECT InventoryParentCategories.Id, InventoryParentCategories.InvParentCateDescription "
                 + "FROM InventoryParentCategories "
                 + "WHERE :ids IS NULL OR InventoryParentCategories.Id IN (SELECT Data FROM fnSplitString(:ids, ','))",
           nativeQuery = true)
    List<InventoryParentCategoryRow> inventoryParentCategories(@Param("ids") String ids);
}
