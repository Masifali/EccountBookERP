package com.mst.controllers;

import com.mst.services.W5BBomProfile;
import com.mst.services.W5BBomService;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;

/**
 * Production hub - screen 845 "Bill Of Material Casting" (Architecture.WinApp.Mfg.frmBillOfMaterialCasting, DocumentTypeId 1664).
 * Page /production/mfg/bill-of-material-casting, JSON under /production/mfg/bill-of-material-casting/api. All handlers are inherited from {@link W5BBomControllerBase}.
 */
@Controller
@RequestMapping("/production/mfg/bill-of-material-casting")
public class W5BBomCastingController extends W5BBomControllerBase {
    public W5BBomCastingController(W5BBomService service) { super(service, W5BBomProfile.CASTING); }
}
