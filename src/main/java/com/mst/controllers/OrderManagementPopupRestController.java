package com.mst.controllers;

import com.mst.services.OrderManagementPopupService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.Map;

/**
 * Order Management pop-up -
 * Architecture.WinApp.Dashboard\OrderManagemantPopUpForPurchaseSale.cs.
 *
 * Feeds the modal the Order Management Dashboard opens when an item name is clicked. One endpoint,
 * because the desktop is one form with an InvoiceType switch.
 */
@RestController
public class OrderManagementPopupRestController {

    @Autowired
    private OrderManagementPopupService service;

    /**
     * OrderManagemantPopUpForPurchaseSale_Load, :56-110.
     *
     * @param invoiceType 1 = Purchase (DocumentTypeId 41, approved only),
     *                    2 = Sales (all approval states)
     */
    @GetMapping("/api/dashboard/order-management/orders")
    public ResponseEntity<Map<String, Object>> orders(
            @RequestParam(defaultValue = "1") int invoiceType,
            @RequestParam(defaultValue = "0") int itemId) {
        return ResponseEntity.ok(service.orders(invoiceType, itemId));
    }
}
