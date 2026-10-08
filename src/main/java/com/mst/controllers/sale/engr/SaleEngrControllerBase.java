package com.mst.controllers.sale.engr;

import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * The desktop shows a failed procedure as MessageBox.Show(ex.Message). ApiExceptionAdvice leaves database errors as a bare 500,
 * so the Sale Engr controllers answer them with the procedure's own message (RAISERROR text) the way the desktop's catch block does.
 */
public abstract class SaleEngrControllerBase {

    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> databaseError(DataAccessException e) {
        Throwable t = e.getMostSpecificCause();
        String m = t == null || t.getMessage() == null ? e.getMessage() : t.getMessage();
        Map<String, Object> body = new LinkedHashMap<>();
        body.put("message", m);
        body.put("status", 409);
        return ResponseEntity.status(HttpStatus.CONFLICT).body(body);
    }
}
