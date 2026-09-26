package com.mst.controllers;

import java.util.Map;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;
import org.springframework.web.server.ResponseStatusException;

/** Keep deliberate Purchase permission/validation reasons visible to the form. */
@RestControllerAdvice(assignableTypes={PurchaseDirectInvoiceRestController.class,PurchaseInvoiceFullRestController.class,GrnLoaderRestController.class,PurchaseInvoiceAttachmentController.class})
public class PurchaseRefusalAdvice {
    @ExceptionHandler(org.springframework.dao.DataAccessException.class)
    public ResponseEntity<Map<String,Object>> database(org.springframework.dao.DataAccessException error){
        Throwable cause=error.getMostSpecificCause();
        if(cause instanceof java.sql.SQLException sql&&(sql.getErrorCode()==50000||sql.getErrorCode()==50001))
            return ResponseEntity.status(409).body(Map.of("success",false,"message",sql.getMessage()));
        org.slf4j.LoggerFactory.getLogger(PurchaseRefusalAdvice.class).error("Purchase database operation failed",error);
        return ResponseEntity.status(500).body(Map.of("success",false,"message","The database request failed. No partial invoice changes were saved."));
    }
    @ExceptionHandler(ResponseStatusException.class)
    public ResponseEntity<Map<String,Object>> refused(ResponseStatusException error){
        return ResponseEntity.status(error.getRawStatusCode()).body(Map.of("success",false,"message",error.getReason()==null?"This action is unavailable":error.getReason()));
    }
}
