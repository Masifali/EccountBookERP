package com.mst.controllers;

import com.mst.repositories.GrnDirectRepository;
import java.util.Map;
import java.util.Set;
import org.springframework.web.bind.annotation.*;

/**
 * clsGlobalVariables.ScreenViewReights lookups the GRN forms make before opening another screen from a link or
 * a toolbar button (InvFrmGRN btnGrnFormHistory_Click :5739 / GrdHistory_LinkClicked "OrderNo" :6030,
 * SaleReturnGrn btnGrnFormHistory_Click :3298): View right of "frmGRNHistory" or "PurchsaeOrder" for the signed-in
 * user (Sp_tblUserRights_GetAllMethod 'GetByUserId', the same read as GrnDirectRepository.hasRight). Only those
 * two screen names are answered.
 */
@RestController
@RequestMapping("/api/purchase/screen-view-right")
public class PurchaseScreenViewRightController {
    private static final Set<String> SCREENS = Set.of("frmGRNHistory", "PurchsaeOrder");
    private final GrnDirectRepository rights;

    public PurchaseScreenViewRightController(GrnDirectRepository rights) { this.rights = rights; }

    @GetMapping("/{screen}")
    public Map<String, Object> view(@PathVariable String screen) {
        if (!SCREENS.contains(screen)) throw new IllegalArgumentException("Unsupported screen");
        return Map.of("screen", screen, "view", rights.hasRight(screen, "View"));
    }
}
