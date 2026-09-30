package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpBModels;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpBSupport.*;

/**
 * BLL of Architecture.WinApp.PartyProcessing.GatePassInwardPartyProcessing (one form, two ScreenDefinition rows):
 *
 *   681  Tag "GatePassInwardPartyProcessing"  DocumentTypeId 54  (?mode=inward)
 *   682  Tag "GatePassOutwardParyProcessing"  DocumentTypeId 55  (?mode=outward)
 *
 * Save / Update: GatePassPartyProcessing.Save (BLL 0298) -> DAL 0301 SetData:
 * Sp_GatePassPartyProcessing_Insert / _Update (ActionId 1 / 2) in one transaction (attachments not ported).
 * Delete: DAL DeleteById -> Sp_GatePassPartyProcessing_GetAllMethod @Id @EntryUser 'DeleteById'.
 */
@Service
public class PpBGatePassService {

    public static final int SCREEN_INWARD = 681;
    public static final int SCREEN_OUTWARD = 682;

    @Autowired private PpBSupport pp;

    /** The two desktop Tags of the form. */
    static final class Mode {
        final int screen, doc;
        final String screenName;
        Mode(String m) {
            boolean out = "outward".equalsIgnoreCase(str(m));
            screen = out ? SCREEN_OUTWARD : SCREEN_INWARD;
            doc = out ? 55 : 54;
            screenName = out ? "GatePassOutwardParyProcessing" : "GatePassInwardPartyProcessing";
        }
    }

    private static final String PROC = "Sp_GatePassPartyProcessing_GetAllMethod";

    // ------------------------------------------------------------------ Load

    /**
     * InwardGatePass_Load: rights, gpnofill, GatePassTypeBind(StaticColumnsService("GatePassTypesForPartyProcessing"))
     * (inward drops Id 4 "AdvanceDO"), StockPartyBind, CmbReferencePartiesFill, vehicleTypefill, CityFill, Status,
     * cmbWeighBridgeFill, grdfrmfill, "City Area" configuration as the default city, ValidateAdvanceDoExpiryDateOnGPO,
     * HistoryCombosFill.
     */
    public Map<String, Object> setup(String mode) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        List<Map<String, Object>> types = new ArrayList<>();
        for (Map<String, Object> r : pp.lookups().staticColumns("GatePassTypesForPartyProcessing")) {
            int id = toInt(col(r, "Id"));
            if (m.doc != 55 && id == 4) continue;
            types.add(map("Id", id, "type", str(col(r, "type"))));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", pp.rights(u, m.screen));
        out.put("documentTypeId", m.doc);
        out.put("gpNo", gpNo(u, m));
        out.put("gpTypes", types);
        out.put("stockParties", pick(pp.lookups().stockParties(u), "Id", "CompanyName"));
        out.put("refParties", pick(pp.lookups().referenceParties(u), "Id", "ReferencePartyName"));
        out.put("vehicleTypes", pick(pp.lookups().vehicleTypes(), "Id", "VehicleDescription"));
        out.put("cities", pick(pp.lookups().cities(u), "Id", "CityName"));
        out.put("defaultCityId", toInt(pp.lookups().config(u, "City Area")));
        out.put("openRows", history(u, m, null, true));
        out.put("historyParties", historyParties(u, m));
        return out;
    }

    /** gpnofill: GatePassPartyProcessing.GenerateCode -> 'GenerateCode' @Org @Company @FinancialYearId @DocumentTypeId -> GpSrNo (0 when no row). */
    private int gpNo(UserAccount u, Mode m) {
        List<Map<String, Object>> r = pp.db().rows(PROC, "Activity", "GenerateCode", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "FinancialYearId", pp.hrm().financialYearId(), "DocumentTypeId", m.doc);
        return r.isEmpty() ? 0 : toInt(col(r.get(0), "GpSrNo"));
    }

    public Map<String, Object> code(String mode) {
        Mode m = new Mode(mode);
        return map("gpNo", gpNo(pp.user(m.screen), m));
    }

    /** btnRefresh_Click: StockPartyBind, CmbReferencePartiesFill, ItemNameFill() (no parent ids), GatePassTypeBind, vehicleTypefill, CityFill, grdfrmfill. */
    public Map<String, Object> refresh(String mode) {
        Map<String, Object> s = setup(mode);
        Mode m = new Mode(mode);
        s.put("items", items(pp.user(m.screen), ""));
        return s;
    }

    /** ItemNameFill(parentIds): CommonServices.GetItemPartyProcessing(parentIds) - Id, ItemName. */
    private List<Map<String, Object>> items(UserAccount u, String parentIds) {
        return pick(pp.lookups().itemPartyProcessing(u, parentIds), "Id", "ItemName");
    }

    /** cmbgptype_Leave: "Packing Material" -> "7,8"; "Rice" / "Paddy" -> "1,2,4"; the stock parties are re-bound too. */
    public Map<String, Object> itemsForType(String mode, String gpType) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        String ids = "Packing Material".equals(gpType) ? "7,8" : ("Rice".equals(gpType) || "Paddy".equals(gpType)) ? "1,2,4" : null;
        if (ids == null) return map("items", new ArrayList<>(), "stockParties", pick(pp.lookups().stockParties(u), "Id", "CompanyName"));
        return map("items", items(u, ids), "stockParties", pick(pp.lookups().stockParties(u), "Id", "CompanyName"));
    }

    /**
     * AdvanceDoBind: InvDeliveryOrder.DeliveryOrder_GetAdvanceDo(org, co, branch, year, 0, RecId, 0) -> [dbo].[USP_DeliveryOrder_GetAdvanceDo]
     * (@SupplierCustomerId / @GdnRecId omitted for 0, @GpRecId only when the record is loaded).
     */
    public List<Map<String, Object>> advanceDos(String mode, int recId) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        return adoRows(u, recId);
    }

    private List<Map<String, Object>> adoRows(UserAccount u, int recId) {
        return pp.db().rows("[dbo].[USP_DeliveryOrder_GetAdvanceDo]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", pp.hrm().financialYearId(), "BranchesId", branch(u), "GpRecId", nz0(recId));
    }

    /**
     * BindDataFromADO(ADOId): InvDeliveryOrder.GetTotalWeightFromDeliveryOrder {DocumentTypeId 910, Id} ->
     * Sp_InvDeliveryOrder_GetAllMethod 'GetTotalWeightFromDeliveryOrder': items (ItemId / ItemName), vehicle type / no,
     * ExpiryDate and the distinct customers (Id / CompanyName) that become the only stock parties.
     */
    public Map<String, Object> adoData(String mode, int adoId) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        List<Map<String, Object>> rows = doWeightRows(u, adoId);
        List<Map<String, Object>> parties = new ArrayList<>();
        List<Integer> seen = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(col(r, "Id"));
            if (!seen.contains(id)) { seen.add(id); parties.add(map("Id", id, "CompanyName", col(r, "CompanyName"))); }
        }
        Map<String, Object> first = rows.isEmpty() ? null : rows.get(0);
        return map("found", first != null,
                "items", pick(rows, "Id=ItemId", "ItemName"),
                "itemId", first == null ? 0 : toInt(col(first, "ItemId")),
                "vehicleType", first == null ? "" : str(col(first, "VehicleType")),
                "vehicleNo", first == null ? "" : str(col(first, "VehicleNo")),
                "expiryDate", first == null ? null : col(first, "ExpiryDate"),
                "stockParties", parties);
    }

    private List<Map<String, Object>> doWeightRows(UserAccount u, int adoId) {
        return pp.db().rows("Sp_InvDeliveryOrder_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", adoId, "DocumentTypeId", 910, "Activity", "GetTotalWeightFromDeliveryOrder");
    }

    // ------------------------------------------------------------------ history

    /** HistoryCombosFill: GetDataForDropDownFromGatePass(org, co, "54"/"55") -> rows of Activity "StockParty" (Id / ReferenceName). */
    private List<Map<String, Object>> historyParties(UserAccount u, Mode m) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pp.db().rows("[dbo].[USP_GetDataForDropDownFromGatePassPartyProcessing]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "DocumentTypeIds", String.valueOf(m.doc))) {
            if ("StockParty".equals(str(col(r, "Activity")))) out.add(map("Id", col(r, "Id"), "name", col(r, "ReferenceName")));
        }
        return out;
    }

    public List<Map<String, Object>> historyParties(String mode) {
        Mode m = new Mode(mode);
        return historyParties(pp.user(m.screen), m);
    }

    public List<Map<String, Object>> open(String mode) {
        Mode m = new Mode(mode);
        return history(pp.user(m.screen), m, null, true);
    }

    public List<Map<String, Object>> history(String mode, Map<String, Object> filters) {
        Mode m = new Mode(mode);
        return history(pp.user(m.screen), m, filters, false);
    }

    /**
     * gridhistory / grdfrmfill: GatePassPartyProcessing.GatepassHistory -> [dbo].[USP_GatePassPartyProcessing_FormHistory]
     * (@BranchesId always, @CanViewAllRecord, @EntryUser when not; open grid = @Status 'Open' only; history = date pair by
     * the radio, doc no range, stock party). Status of the history call is the unset ReportsParameters.Status (null, omitted).
     */
    private List<Map<String, Object>> history(UserAccount u, Mode m, Map<String, Object> b, boolean open) {
        boolean all = pp.canViewAll(u, m.screen);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("organizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", branch(u));
        p.put("FinancialYearId", pp.hrm().financialYearId());
        p.put("DocumentTypeId", m.doc);
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUser", u.getId());
        if (open) {
            p.put("Status", "Open");
        } else {
            dateFilter(p, b, "FromDate", "ToDate", "EntryFromDate", "EntryToDate", "ModifyFromDate", "ModifyToDate", null, null);
            p.put("DocNoFrom", nz0(toInt(b.get("fromDocNo"))));
            p.put("DocNoTo", nz0(toInt(b.get("toDocNo"))));
            p.put("StockPartyId", nz0(toInt(b.get("stockPartyId"))));
        }
        List<Map<String, Object>> rows = pp.db().rows("[dbo].[USP_GatePassPartyProcessing_FormHistory]", p);
        return pick(rows, "Id", "GpSrNo", "GpDate", "GatepassType", "StockParty=StockPartyName", "ReferenceParty", "City=CityName",
                "VarietyName", "Qty=ItemQty", "VehicleType", "VehicleNo", "BiltyNo", "InTime=InDateTimeStamp", "OutTime=OutDateTimeStamp",
                "Freight", "Status", "SupplierWeight", "FactoryWeight", "DifferenceWeight", "WeighBridgeStatus", "EntryUser=EntryUserName",
                "EntryDate", "ModifyUser=ModifyUserName", "ModifyDate", "NoOfAttachments", "Remarks=OtherRemarks");
    }

    // ------------------------------------------------------------------ read

    /**
     * getUpdate(ID): GatePassPartyProcessing.GetByID -> 'ReadById', then GetFactoryWeightFromWb():
     * WbTransactions.GetNetWeightFromWbTransactions -> Sp_WbTransation_GetAllMethod @Id=RecId @RefDocumentTypeId
     * 'GetNetWeightFromWbTransactions' - the sum of NetWbWeight becomes Factory Weight (and Supplier Weight on 55), "0" when no row.
     */
    public Map<String, Object> byId(String mode, int id) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        Map<String, Object> r = read(u, m, id);
        Map<String, Object> out = new LinkedHashMap<>(r);
        List<Map<String, Object>> wb = pp.db().rows("Sp_WbTransation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", id, "RefDocumentTypeId", m.doc, "Activity", "GetNetWeightFromWbTransactions");
        out.put("wbFound", !wb.isEmpty());
        double net = 0;
        StringBuilder tickets = new StringBuilder();
        for (Map<String, Object> w : wb) { tickets.append(',').append(str(col(w, "TicketNo"))); net += d(col(w, "NetWbWeight")); }
        out.put("wbNetWeight", wb.isEmpty() ? "0" : net(net));
        out.put("advanceDos", adoRows(u, id));
        return out;
    }

    private Map<String, Object> read(UserAccount u, Mode m, int id) {
        List<Map<String, Object>> rows = pp.db().rows(PROC, "Id", id, "Activity", "ReadById");
        if (rows.isEmpty()) throw invalid("Record not found");
        Map<String, Object> r = rows.get(0);
        if (col(r, "CompanyId") != null && toInt(col(r, "CompanyId")) != u.getCompanyId()) throw invalid("Record not found");
        if (col(r, "DocumentTypeId") != null && toInt(col(r, "DocumentTypeId")) != m.doc) throw invalid("Record not found");
        return pick(rows, "Id", "GpSrNo", "GpDate", "GatepassType", "AdvanceDeliveryOrderId", "BiltyNo", "CityId", "Freight", "ItemQty",
                "OtherRemarks", "VehicleNo", "VehicleType", "SupplierCustomerId", "StockPartyId", "Status", "IsWeighable", "ItemId", "VarietyName",
                "SupplierWeight", "FactoryWeight", "WeighBridgeStatus", "DifferenceWeight", "EntryDate", "EntryUser").get(0);
    }

    // ------------------------------------------------------------------ save

    /**
     * btnsave_Click / btnupdate_Click -> Insert(): formvalidation() in the form's order and wording, the Advance DO expiry and
     * balance checks, then the model exactly as the form fills it and GatePassPartyProcessing.Save.
     */
    public Map<String, Object> save(String mode, Map<String, Object> b) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.hrm().require(u, m.screen, recId > 0 ? "Update" : "Save");
        if (recId > 0) read(u, m, recId);

        String statusText = str(b.get("statusText")).trim();
        boolean weighable = toBool(b.get("isWeighable"));
        // formvalidation()
        if (weighable && "Accepted".equals(statusText) && toDouble(b.get("factoryWeight")) == 0.0) throw invalid("Factory Weight Field Required");
        if (toInt(b.get("itemId")) == 0) throw invalid("ItemName Field Required");
        if (toInt(b.get("gpTypeId")) == 0) throw invalid("GP Type Field Required");
        if (toInt(b.get("stockPartyId")) == 0) throw invalid("Stock Party Field Required");
        if (trim(b.get("vehicleNo")).isEmpty()) throw invalid("VehicleNo Field Required");
        if (toInt(b.get("vehicleTypeId")) == 0) throw invalid("VehicleType Field Required");
        if (toDouble(trim(b.get("qty"))) == 0.0) throw invalid("ItemQty Field Required");
        // desktop: txtgpno empty || txtqty == "0" (sic - the quantity box) -> "GPNo Field Required"
        if (trim(b.get("gpNo")).isEmpty() || "0".equals(trim(b.get("qty")))) throw invalid("GPNo Field Required");
        int statusId = toInt(b.get("statusId"));
        if (statusId == 0) throw invalid("Status Field Required");
        int wbId = toInt(b.get("wbStatusId"));
        if (wbId == 0) throw invalid("W.b Status Field Required");

        String gpType = gpTypeText(toInt(b.get("gpTypeId")), m);
        LocalDateTime gpDate = toDate(b.get("gpDate"));
        if (gpDate == null) gpDate = LocalDateTime.now();
        PpBModels.GatePassPartyProcessing g = new PpBModels.GatePassPartyProcessing();
        if (recId > 0) {
            g.Id = recId;
            g.FactoryWeight = toDouble(trim(b.get("factoryWeight")));
        }
        int adoId = toInt(b.get("adoId"));
        if (adoId > 0) {
            Map<String, Object> ado = null;
            for (Map<String, Object> r : adoRows(u, recId)) if (toInt(col(r, "Id")) == adoId) ado = r;
            if (ado == null) throw invalid("Advance DO not found");
            String adoText = str(col(ado, "DocNo"));
            List<Map<String, Object>> doRows = doWeightRows(u, adoId);
            LocalDateTime expiry = doRows.isEmpty() ? LocalDateTime.now() : toDate(col(doRows.get(0), "ExpiryDate"));
            if (expiry == null) expiry = LocalDateTime.now();
            boolean validateExpiry = toBool(pp.lookups().config(u, "ValidateAdvanceDoExpiryDateOnGPO"));
            DateTimeFormatter f = DateTimeFormatter.ofPattern("dd/MM/yyyy");
            if (validateExpiry && gpDate.toLocalDate().isAfter(expiry.toLocalDate())) {
                throw invalid("The expiry date '" + expiry.format(f) + "' of the selected Advance DO '" + adoText + "' is earlier than the Gatepass date '"
                        + gpDate.format(f) + "'.\nPlease select a valid Advance DO or Change GP Date.");
            }
            double balQty = toInt(col(ado, "BalQty"));
            double qty = toDouble(trim(b.get("qty")));
            if (qty > balQty) {
                throw invalid("Gp Qty '" + net(qty) + "' Can not be Greater than BalQty '" + net(balQty) + "',For Advance DO '" + adoText + "'");
            }
        }
        g.BiltyNo = trim(b.get("biltyNo"));
        g.CityId = toInt(b.get("cityId"));
        g.CompanyId = u.getCompanyId();
        g.BranchesId = branch(u);
        g.FinancialYearId = pp.hrm().financialYearId();
        g.DocumentTypeId = m.doc;
        g.Freight = toDouble(str(b.get("freight")));
        g.GatepassType = gpType;
        g.GpDate = gpDate;
        g.GpSrNo = toInt(trim(b.get("gpNo")));
        LocalDateTime now = LocalDateTime.now();
        g.InDateTimeStamp = now;
        g.ItemQty = toDouble(trim(b.get("qty")));
        g.OtherRemarks = trim(b.get("remarks"));
        g.VehicleNo = trim(b.get("vehicleNo"));
        g.VehicleType = trim(b.get("vehicleTypeText"));
        g.SupplierCustomerId = toInt(b.get("refPartyId"));
        g.Status = statusText;
        g.WeighBridgeStatus = trim(b.get("wbStatusText"));
        g.AdvanceDeliveryOrderId = adoId;
        g.IsWeighable = weighable;
        g.StockPartyId = toInt(b.get("stockPartyId"));
        if (m.doc == 55) {
            g.SupplierWeight = g.FactoryWeight;
        } else {
            g.SupplierWeight = toDouble(trim(b.get("supplierWeight")));
            g.DifferenceWeight = toDouble(trim(b.get("differenceWeight")));
        }
        g.ItemId = toInt(b.get("itemId"));
        g.VarietyName = trim(b.get("itemName"));
        g.OrganizationId = u.getOrganizationId();
        g.EntryDate = now;
        g.ModifyDate = now;
        g.EntryUser = u.getId();
        g.ModifyUser = u.getId();
        g.PostDate = now;
        g.OutDateTimeStamp = now;
        g.ScreenName = m.screenName;
        final PpBModels.GatePassPartyProcessing model = g;
        int saved = pp.db().tx(() -> {
            if (model.Id == 0) {
                model.ActionId = 1;
                int n = pp.db().set("Sp_GatePassPartyProcessing_Insert", model);
                return n > 0 ? n : model.Id;
            }
            model.ActionId = 2;
            model.ModifyDate = LocalDateTime.now();
            int n = pp.db().set("Sp_GatePassPartyProcessing_Update", model);
            return n > 0 ? n : model.Id;
        });
        String msg = recId == 0 ? "Record Save Successfully[" + g.GpSrNo + "]" : "Record Update Successfully [" + g.GpSrNo + "]";
        return saved(saved, msg);
    }

    private String gpTypeText(int id, Mode m) {
        for (Map<String, Object> r : pp.lookups().staticColumns("GatePassTypesForPartyProcessing")) {
            if (toInt(col(r, "Id")) == id && !(m.doc != 55 && id == 4)) return str(col(r, "type"));
        }
        throw invalid("GP Type Field Required");
    }

    // ------------------------------------------------------------------ delete / print

    /** btnDelete_Click: "No record found to Delete" / DAL DeleteById (@Id, @EntryUser, 'DeleteById') -> "Record Deleted Successfully". */
    public Map<String, Object> delete(String mode, int id) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        pp.hrm().require(u, m.screen, "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        read(u, m, id);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", id);
        p.put("EntryUser", u.getId());
        p.put("Activity", "DeleteById");
        pp.db().tx(() -> pp.db().scalar(PROC, p));
        return saved(id, "Record Deleted Successfully");
    }

    /**
     * GenerateReport(Id) -> CommonServices.PartyProcessingInwardGatePassSlip299 / ...Outward...299: the slip data is
     * PartyProcessingGatePassReports.PartyProcessingGatePassSlipandRegister -> [dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt]
     * (@Org @Company @FinancialYearId @DocumentTypeId @Id). The .rpt file name is not in the available sources, so the page
     * prints these rows through the grid-to-PDF printer.
     */
    public List<Map<String, Object>> slip(String mode, int id) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        pp.hrm().require(u, m.screen, "Print");
        if (id == 0) throw invalid("Record not found for display");
        List<Map<String, Object>> rows = pp.db().rows("[dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "FinancialYearId", pp.hrm().financialYearId(), "DocumentTypeId", m.doc, "Id", id);
        if (rows.isEmpty()) throw invalid("No Record Found For Display");
        return rows;
    }

    static LocalDate today() { return LocalDate.now(); }

    static Timestamp ts0(LocalDateTime d) { return d == null ? null : Timestamp.valueOf(d); }
}
