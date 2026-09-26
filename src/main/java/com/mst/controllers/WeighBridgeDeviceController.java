package com.mst.controllers;

import com.mst.services.WeighBridgeDeviceService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.CacheControl;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.concurrent.Callable;

/**
 * The weighbridge hardware on the server PC: the indicator (F5) and the Hikvision cameras.
 * See {@link WeighBridgeDeviceService}. Nothing about the port, the cameras or their credentials
 * is accepted from, or sent to, the browser.
 */
@RestController
public class WeighBridgeDeviceController {

    private static final String API = "/api/weighbridge/device";

    @Autowired
    private WeighBridgeDeviceService devices;

    /** ReadWeightFromWeightBridge() — F5 / "Read" on screen 411. */
    @PostMapping(API + "/read-weight")
    public ResponseEntity<?> readWeight(@RequestBody WeighBridgeDeviceService.ReadRequest r) {
        return run(() -> devices.readWeight(r), "Could not read the weighbridge indicator.");
    }

    /** LoadCamera() — log the three cameras in; second = the second-weight set (Camera 04..06). */
    @GetMapping(API + "/cameras")
    public ResponseEntity<?> cameras(@RequestParam(defaultValue = "false") boolean second) {
        return run(() -> devices.initCameras(second), "Could not reach the cameras.");
    }

    /** One live-view frame from camera 1..3. */
    @GetMapping(API + "/camera/{n}/snapshot")
    public ResponseEntity<?> snapshot(@PathVariable int n) {
        try {
            return jpeg(devices.snapshot(n));
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.SERVICE_UNAVAILABLE).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Camera error.")));
        }
    }

    /** A stored picture of a saved ticket (FirstWtPicReading … SecondWtFrontPic). */
    @GetMapping(API + "/ticket/{id}/picture/{field}")
    public ResponseEntity<?> ticketPicture(@PathVariable int id, @PathVariable String field) {
        try {
            byte[] b = devices.ticketPicture(id, field);
            return b == null ? ResponseEntity.notFound().build() : jpeg(b);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not read the picture.")));
        }
    }

    /** A picture F5 took in this session, before the ticket is saved. */
    @GetMapping(API + "/captured/{name:.+}")
    public ResponseEntity<?> captured(@PathVariable String name) {
        try {
            byte[] b = devices.capturedPicture(name);
            return b == null ? ResponseEntity.notFound().build() : jpeg(b);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not read the picture.")));
        }
    }

    private static ResponseEntity<byte[]> jpeg(byte[] b) {
        return ResponseEntity.ok().contentType(MediaType.IMAGE_JPEG).cacheControl(CacheControl.noStore()).body(b);
    }

    private static ResponseEntity<?> run(Callable<?> body, String fallback) {
        try {
            return ResponseEntity.ok(body.call());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (org.springframework.security.access.AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, fallback)));
        }
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }

    private static String msg(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return (m == null || m.trim().isEmpty()) ? fallback : m;
    }
}
