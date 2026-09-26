package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.AuthenticationException;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * The desktop ctrlGrdBar (grid.User_Conrtols.CtrlGrdBar) keys every grid layout as
 * FormName + "_" + GridName + "_" + clsGlobalVariables.UserAccount.UserName
 * (CtrlGrdBar.mGridSaveLayouts_Click / removeLayoutToolStripMenuItem_Click, CommonServices.GetGridLayout /
 * DeleteGrdLayout). The web helper countx_grid_bar.js builds the same key, so it needs the session user name.
 *
 * Read-only: web layouts are kept in the viewer's browser, never in table GridLayout, because that table
 * holds Janus binary layouts which the desktop hands to GridEX.LoadLayoutFile.
 */
@RestController
public class ProductionGridBarController {

    @Autowired private CurrentUserContext currentUserContext;

    /** clsGlobalVariables.UserAccount.UserName of the signed-in user. */
    @GetMapping("/api/production/grid-bar/user")
    public ResponseEntity<?> user() {
        Map<String, Object> m = new LinkedHashMap<>();
        try {
            UserAccount u = currentUserContext.requireAccountingUser();
            String name = u == null ? null : u.getUserName();
            if (name == null || name.isEmpty()) {
                m.put("success", false);
                m.put("message", "No user is signed in.");
                return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(m);
            }
            m.put("success", true);
            m.put("userName", name);
            return ResponseEntity.ok(m);
        } catch (AuthenticationException e) {
            m.put("success", false);
            m.put("message", e.getMessage());
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(m);
        } catch (Exception e) {
            m.put("success", false);
            m.put("message", e.getMessage() == null ? e.getClass().getSimpleName() : e.getMessage());
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(m);
        }
    }
}
