package com.mst.controllers;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.services.banking.ChequePrintingDesktopService;

/**
 * Screen 700 "Cheque Printing" (desktop Architecture.WinApp.ChequePrinting.ChequePrinting) - page
 * /accounts/banking/cheque-printing (templates/accounts/banking/cheque_printing.html).
 *
 *   GET  /accounts/api/banking/cheque-printing/counts          ChequeCountGridFill  (sp_ChequePrinting_GetChequeCounts)
 *   GET  /accounts/api/banking/cheque-printing/cards?bankId=   GetDetailData(BankId, 0) (sp_ChequePrintList)
 *   POST /accounts/api/banking/cheque-printing/print           PrintCheque(): sp_ChequePrintList_Print -> &lt;Template&gt;.rpt
 *                                                              (Jasper) then sp_ChequePrintList_UpdateStatus
 * Tenancy comes from CurrentUserContext only.
 */
@RestController
@RequestMapping("/accounts/api/banking/cheque-printing")
public class ChequePrintingDesktopController {

    private final ChequePrintingDesktopService service;

    public ChequePrintingDesktopController(ChequePrintingDesktopService service) {
        this.service = service;
    }

    @GetMapping("/counts")
    public ResponseEntity<?> counts() {
        try {
            return ResponseEntity.ok(service.chequeCounts());
        } catch (Exception e) {
            return error(e);
        }
    }

    @GetMapping("/cards")
    public ResponseEntity<?> cards(@RequestParam("bankId") int bankId) {
        try {
            return ResponseEntity.ok(service.cards(bankId));
        } catch (Exception e) {
            return error(e);
        }
    }

    /** Body: {"bankId": 3, "ids": [376, 377]}. Returns application/pdf, or JSON {message} when nothing was found to print. */


    private static ResponseEntity<String> error(Exception e) {
        return ResponseEntity.status(500).contentType(MediaType.TEXT_PLAIN)
                .body(e.getMessage() == null ? e.toString() : e.getMessage());
    }

    private static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
