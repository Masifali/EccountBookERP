package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.WeighBridgeExtrasRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The two lookups screens 410 / 411 open from their toolbars. Neither has a ScreenDefinition row
 * and neither checks a right on the desktop (they are plain forms shown by the weigh bridge
 * screens), so there is no rights check here beyond a signed-in accounting user.
 *
 *  - WeighBridgeGeneralLookups (Architecture.WinApp) — parties and items typed on the weigh bridge.
 *  - VehicleWeightLookUp (Architecture.WinApp.Lookups) — tare weight per vehicle.
 *
 * Organisation, company, user and financial year always come from the session, never the caller.
 */
@Service
public class WeighBridgeLookupsService {

    @Autowired private WeighBridgeExtrasRepository repo;
    @Autowired private CurrentUserContext currentUserContext;

    // ============================================================ WeighBridgeGeneralLookups

    /** DefineCity_Load: BindWeighBridgeType() + GridHistoryBind(). */
    public Map<String, Object> generalLookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("types", repo.staticColumns("WeighBridgeType"));
        out.put("rows", generalGrid(u));
        return out;
    }

    /** GridHistoryBind():125 — Id, Type, Description; Description is PartyName when WbType is
     *  "PartyName", ItemName otherwise. */
    private List<Map<String, Object>> generalGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.wbPartyAndItems(u)) {
            String type = str(ci(r, "WbType"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("Type", ci(r, "WbType"));
            m.put("Description", "PartyName".equals(type) ? ci(r, "PartyName") : ci(r, "ItemName"));
            out.add(m);
        }
        return out;
    }

    public static final class GeneralRequest {
        public int id;
        public String type;
        public String description;
    }

    /** Insert():166 — FormValidation(), then Save. id > 0 is the update path. */
    public Map<String, Object> saveGeneral(GeneralRequest r) {
        UserAccount u = currentUserContext.requireAccountingUser();
        String type = r.type == null ? "" : r.type.trim();
        if (type.isEmpty()) throw new IllegalArgumentException("Type Field Required");
        if (r.description == null || r.description.trim().isEmpty()) throw new IllegalArgumentException("Description Field Required");
        if (r.id > 0 && !ownsGeneral(u, r.id)) throw new IllegalArgumentException("Record Not Found");
        String party = "PartyName".equals(type) ? r.description : null;
        String item = "PartyName".equals(type) ? null : r.description;
        repo.saveWbPartyOrItem(u, Math.max(r.id, 0), type, party, item);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", r.id > 0 ? "Record Update Successfully" : "Record Save Successfully");
        out.put("rows", generalGrid(u));
        return out;
    }

    /** The update procedure keys on @Id alone; only ids already listed for this company go through. */
    private boolean ownsGeneral(UserAccount u, int id) {
        for (Map<String, Object> r : repo.wbPartyAndItems(u)) if (intOf(ci(r, "Id")) == id) return true;
        return false;
    }

    // ================================================================= VehicleWeightLookUp

    /** VehicleWeightLookUp_Load: VehicleTypeFill() + GridFill(). */
    public Map<String, Object> vehicleWeights() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("vehicleTypes", repo.vehicleTypes());
        out.put("rows", vehicleGrid(u));
        return out;
    }

    /** btnRefresh_Click — VehicleTypeFill() only. */
    public List<Map<String, Object>> vehicleTypes() {
        currentUserContext.requireAccountingUser();
        return repo.vehicleTypes();
    }

    /** GridFill():262 — Id, VehicleType, VehicleNo, NetWeight, Remarks. */
    private List<Map<String, Object>> vehicleGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.vehicleWeights(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(ci(r, "Id")));
            m.put("VehicleType", ci(r, "VehicleType"));
            m.put("VehicleNo", ci(r, "VehicleNo"));
            m.put("NetWeight", dbl(ci(r, "VehicleNetWeight")));
            m.put("Remarks", ci(r, "Remarks"));
            out.add(m);
        }
        return out;
    }

    public static final class VehicleRequest {
        public int id;
        public String vehicleType;
        public String vehicleNo;
        public String netWeight;
        public String remarks;
    }

    /** Insert():195 — FormValidation(), the duplicate-vehicle check against the grid, then Save. */
    public Map<String, Object> saveVehicle(VehicleRequest r) {
        UserAccount u = currentUserContext.requireAccountingUser();
        String type = r.vehicleType == null ? "" : r.vehicleType;
        String no = r.vehicleNo == null ? "" : r.vehicleNo;
        if (type.trim().isEmpty()) throw new IllegalArgumentException("Vehicle Type Field is Required");
        if (no.trim().isEmpty()) throw new IllegalArgumentException("Vehicle No Field is Required");
        List<Map<String, Object>> grid = vehicleGrid(u);
        boolean owned = r.id <= 0;
        for (Map<String, Object> g : grid) {
            int gid = intOf(g.get("Id"));
            if (gid == r.id) { owned = true; continue; }      // the row being edited (updateIndex)
            if (no.equals(str(g.get("VehicleNo")))) throw new IllegalArgumentException("Vehicle No Already Exit in Grid...");
        }
        if (!owned) throw new IllegalArgumentException("Record Not Found");
        int fy = currentUserContext.currentFinancialYearId();
        int success = repo.saveVehicleWeight(u, fy, Math.max(r.id, 0), type, no, dbl(r.netWeight),
                                             r.remarks == null ? "" : r.remarks);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", (r.id > 0 ? "Record Update Successfully...[" : "Record Save Successfully...[") + success + "]");
        out.put("rows", vehicleGrid(u));
        return out;
    }

    // ================================================================================ helpers

    static Object ci(Map<String, Object> r, String key) {
        if (r == null) return null;
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    static String str(Object o) { return o == null ? "" : o.toString(); }

    static int intOf(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return o == null ? 0 : (int) Double.parseDouble(o.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble — non-numbers become 0. */
    static double dbl(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return o == null ? 0d : Double.parseDouble(o.toString().trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }
}
