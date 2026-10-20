package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.mst.models.InvLookUp;
import com.mst.projections.ItemProductionStageRow;
import com.mst.projections.ItemVarietyNatureRow;
import com.mst.projections.LocationTypeRow;

/**
 * Query-only repository for the parameterless lookup procedures whose tables have no entity
 * (ItemProductionStage, ItemVarietyNature, LocationType). Bound to the owned InvLookUp entity.
 */
@Repository
public interface ItemMasterLookupQueryRepository extends org.springframework.data.repository.Repository<InvLookUp, Integer> {

    /** usp_getItemProductionStage (single branch; native: ItemProductionStage has no entity) */
    @Query(value = "select Id, productionStageName from ItemProductionStage", nativeQuery = true)
    List<ItemProductionStageRow> getItemProductionStage();

    /** usp_getItemVarietyNature (single branch; native: ItemVarietyNature has no entity) */
    @Query(value = "select Id, VarietyNatureName from ItemVarietyNature", nativeQuery = true)
    List<ItemVarietyNatureRow> getItemVarietyNature();

    /** usp_getLocationType (single branch; native: LocationType has no entity) */
    @Query(value = "select Id, Location from LocationType", nativeQuery = true)
    List<LocationTypeRow> getLocationType();
}
