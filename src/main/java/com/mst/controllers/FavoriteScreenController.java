package com.mst.controllers;

import com.mst.services.DashboardModuleService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;

/** Star on a dashboard screen card (desktop ScreenDynamicallyGenerateCards.pictureBox1_Click). */
@RestController
public class FavoriteScreenController {

    @Autowired private DashboardModuleService dashboardModuleService;

    @PostMapping("/api/dashboard/favorite")
    public ResponseEntity<?> toggle(@RequestParam("screenId") int screenId) {
        try {
            return ResponseEntity.ok(dashboardModuleService.toggleFavorite(screenId));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable t = e;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(t.getMessage() != null ? t.getMessage() : "Favourite could not be saved."));
        }
    }

    private static Map<String, Object> fail(String m) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", m);
        return r;
    }
}
