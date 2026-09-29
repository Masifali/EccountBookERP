package com.mst.controllers.cmagt;

import com.mst.models.cmagt.dto.GoodsDispatchingNoteCmagtDto;
import com.mst.services.cmagt.GoodsDispatchingNoteCmagtService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/commission/goods-dispatching-note")
public class GoodsDispatchingNoteCmagtRestController {

    @Autowired
    private GoodsDispatchingNoteCmagtService service;

    /* Company, organization, branch, year and user are NEVER taken from the request; the
       service reads them from the session (desktop UserAccount / clsGlobalVariables.ActiveYr). */

    /** btnsave_Click :2072 - always a new document (RecId = 0). */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody GoodsDispatchingNoteCmagtDto dto) {
        return ResponseEntity.ok(service.saveOrUpdate(dto, false));
    }

    /** btnUpdate_Click :2085 - updates the loaded document; refused without an id. */
    @PostMapping("/update")
    public ResponseEntity<Map<String, Object>> update(@RequestBody GoodsDispatchingNoteCmagtDto dto) {
        return ResponseEntity.ok(service.saveOrUpdate(dto, true));
    }

    /** btnDelete_Click :2191 -> BLL DeleteByID. */
    @DeleteMapping("/{id}")
    public ResponseEntity<Map<String, Object>> delete(@PathVariable Integer id) {
        return ResponseEntity.ok(service.delete(id));
    }

    /** GenerateCode (:677) for the Doc No box on New. */
    @GetMapping("/generate-code")
    public ResponseEntity<Map<String, Object>> generateCode() {
        return ResponseEntity.ok(service.generateCode());
    }

    /** btnLoadGrn_Click :3448 -> frmPendingGrnLoadingChallanLoader (BLL 0488 PendingDataLoaderForGdn).
        recId > 0 excludes that GDN's own rows from the used quantities (ReadById :2173). */
    @GetMapping("/pending-grn")
    public ResponseEntity<Map<String, Object>> pendingGrn(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "0") int fromDocNo,
            @RequestParam(defaultValue = "0") int toDocNo,
            @RequestParam(defaultValue = "0") int recId,
            @RequestParam(defaultValue = "0") int commissionAgentId,
            @RequestParam(defaultValue = "0") int supplierId,
            @RequestParam(defaultValue = "0") int buyerId,
            @RequestParam(defaultValue = "0") int itemId,
            @RequestParam(defaultValue = "0") int deliverToPartyId,
            @RequestParam(required = false) String shipToAddress) {
        return ResponseEntity.ok(service.pendingGrn(fromDate, toDate, fromDocNo, toDocNo, recId,
                commissionAgentId, supplierId, buyerId, itemId, deliverToPartyId, shipToAddress));
    }

    /** Loader filter combos (frmPendingGrnLoadingChallanLoader.ComboDbCall / CombosFill). */
    @GetMapping("/pending-grn/combos")
    public ResponseEntity<Map<String, Object>> pendingGrnCombos() {
        return ResponseEntity.ok(service.pendingGrnLoaderCombos());
    }

    /** BlockEntry.../ShowWarning...ForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal (:607-608). */
    /** GetCommissionAgentConfigurationsFromGlobalandBind :705-735 - defaults re-applied on every Reset. */
    @GetMapping("/portal-defaults")
    public ResponseEntity<Map<String, Object>> portalDefaults() {
        return ResponseEntity.ok(service.portalDefaults());
    }

    /** formright (:522, :552-555): Save / Update / Delete / Print rights of the signed-in user. */
    @GetMapping("/rights")
    public ResponseEntity<Map<String, Boolean>> rights() {
        return ResponseEntity.ok(service.formRights());
    }

    @GetMapping("/late-vehicle-config")
    public ResponseEntity<Map<String, Object>> lateVehicleConfig() {
        return ResponseEntity.ok(service.lateVehicleConfig());
    }

    /** btnshow_Click -> HistoryFill :2745-2833. dateType: doc (drdocdate) / entry (rdentrydate) /
        modify (rdmodifydate). Only the filters BLL 0487 FormHistory actually sends are accepted. */
    @GetMapping("/history")
    public ResponseEntity<List<Map<String, Object>>> getHistory(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "doc") String dateType,
            @RequestParam(defaultValue = "0") int commissionAgentId,
            @RequestParam(defaultValue = "0") int buyerId,
            @RequestParam(defaultValue = "0") int deliverToPartyId,
            @RequestParam(required = false) String shipToAddress) {
        return ResponseEntity.ok(service.getHistory(fromDate, toDate, dateType,
                commissionAgentId, buyerId, deliverToPartyId, shipToAddress));
    }

    /** HistoryComboDbCall :589 -> BLL 0487 GetDataForDropDown -> USP_GetDataForDropDownFromgdnBuyerDispatchMaster
        (raw Id / ReferenceName / ParentCategoryId / Activity rows; the page splits them as HistoryComboBind). */
    @GetMapping("/history/combos")
    public ResponseEntity<List<Map<String, Object>>> historyCombos() {
        return ResponseEntity.ok(service.historyCombos());
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getById(@PathVariable Integer id) {
        return ResponseEntity.ok(service.getById(id));
    }
}
