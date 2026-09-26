package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The weigh bridge satellite screens, one method per desktop BLL call:
 *
 *   WeighBridgeGeneralLookups        (Architecture.WinApp)              — BLL 0089 WbPartyandItemsDefine
 *   VehicleWeightLookUp              (Architecture.WinApp.Lookups)      — BLL 0530 DefineVehicleWeight
 *   frmWeighBridgeRejectedTicketNos  (Inventory_Reports, screen 359)    — BLL 0091 WbTransactions
 *   frmWeightBridgeHistory           (Inventory_Reports, screen 360)    — BLL 0130 WbTransactionsReports
 *
 * Every parameter list below was checked against procdure.sql. A BLL guard of the form
 * {@code if (x != 0) list.Add(...)} is reproduced with {@link Call#opt}. GenericProvider.SetProc
 * sends every model property; a C# null string reaches SQL Server as "not supplied", so the
 * setProc maps below leave null strings out.
 */
@Repository
public class WeighBridgeExtrasRepository {

    @Autowired
    private JdbcTemplate jdbc;

    // ============================================================ WeighBridgeGeneralLookups

    /** BLL 0089 WbPartyandItemsDefine.GetAll — every row of the company (Id, WbType, PartyName, ItemName, ...). */
    public List<Map<String, Object>> wbPartyAndItems(UserAccount u) {
        return new Call("dbo.SP_WbPartyandItemsDefine_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Activity", "ReadAll")
                .rows();
    }

    /** CommonServices.StaticColumnsService → GeneralReprots.StaticColumnNames: @Activity only. */
    public List<Map<String, Object>> staticColumns(String activity) {
        return new Call("dbo.SpStaticColumnNames").p("Activity", activity).rows();
    }

    /**
     * BLL 0089 Save → DAL 0081 SetDate: Id == 0 → Sp_WbPartyandItemsDefine_Insert, else _Update.
     * Model 0106 property order: CompanyId, Id, OrganizationId, ItemName, PartyName, WbType.
     * The form fills only one of PartyName / ItemName, so the other stays a C# null and is not sent
     * (the procedure default NULL applies — an update therefore clears the other column).
     */
    @Transactional
    public int saveWbPartyOrItem(UserAccount u, int id, String wbType, String partyName, String itemName) {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        m.put("CompanyId", u.getCompanyId());
        m.put("Id", id);
        m.put("OrganizationId", u.getOrganizationId());
        if (itemName != null) m.put("ItemName", itemName);
        if (partyName != null) m.put("PartyName", partyName);
        if (wbType != null) m.put("WbType", wbType);
        int num = scalarInt(id == 0 ? "dbo.Sp_WbPartyandItemsDefine_Insert" : "dbo.Sp_WbPartyandItemsDefine_Update", m);
        return num > 0 ? num : id;
    }

    // ================================================================= VehicleWeightLookUp

    /** BLL 0611 VehicleType.GetAll — called with no parameters, as the desktop does. */
    public List<Map<String, Object>> vehicleTypes() {
        return new Call("dbo.Sp_VehicleType_GetAllMethod").rows();
    }

    /** BLL 0530 DefineVehicleWeight.ReadAll — the form never sets FinancialYearId, so it is not sent. */
    public List<Map<String, Object>> vehicleWeights(UserAccount u) {
        return new Call("dbo.USp_DefineVehicleWeight_FormHistory")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .rows();
    }

    /**
     * BLL 0530 Save → DAL 0385 SetData: Id == 0 → USp_DefineVehicleWeight_Insert, else _Update.
     * Model 0956 property order: EntryDate, ModifyDate, VehicleNetWeight, CompanyId, EntryUser,
     * FinancialYearId, Id, ModifyUser, OrganizationId, Remarks, VehicleNo, VehicleType.
     */
    @Transactional
    public int saveVehicleWeight(UserAccount u, int financialYearId, int id, String vehicleType,
                                 String vehicleNo, double netWeight, String remarks) {
        Timestamp now = new Timestamp(System.currentTimeMillis());
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("VehicleNetWeight", netWeight);
        m.put("CompanyId", u.getCompanyId());
        m.put("EntryUser", u.getId());
        m.put("FinancialYearId", financialYearId);
        m.put("Id", id);
        m.put("ModifyUser", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        if (remarks != null) m.put("Remarks", remarks);
        if (vehicleNo != null) m.put("VehicleNo", vehicleNo);
        if (vehicleType != null) m.put("VehicleType", vehicleType);
        int num = scalarInt(id == 0 ? "dbo.USp_DefineVehicleWeight_Insert" : "dbo.USp_DefineVehicleWeight_Update", m);
        return num > 0 ? num : id;
    }

    // ===================================================== 359 frmWeighBridgeRejectedTicketNos

    /** BLL 0091 GetWeighBridgeDataForRejectedTicketNos:880 — the form sets no BranchId, so
     *  @BranchesId is never sent. */
    public List<Map<String, Object>> rejectableTickets(UserAccount u, int refDocumentTypeId) {
        return new Call("dbo.Sp_WbTransation_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("RefDocumentTypeId", refDocumentTypeId)
                .p("Activity", "GetWeighBridgeDataForRejectedTicketNos")
                .rows();
    }

    /** BLL 0091 WeighBridgeRejectedStatusUpdate:921. */
    @Transactional
    public void rejectTicket(UserAccount u, int refDocumentTypeId, int id) {
        new Call("dbo.Sp_WbTransation_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("RefDocumentTypeId", refDocumentTypeId)
                .p("Id", id)
                .p("EntryUser", u.getId())
                .p("RejectedDate", new Timestamp(System.currentTimeMillis()))
                .p("Activity", "WeighBridgeRejectedStatusUpdate")
                .rows();
    }

    // ============================================================ 360 frmWeightBridgeHistory

    /** BLL 0058 Branches.GetBranchesAllocatedToUserFromWeighBridge(org, company, user, 0) — columns BranchId, BranchName. */
    public List<Map<String, Object>> branchesFromWeighBridge(UserAccount u) {
        return new Call("[dbo].[USP_GetBranchsAllocatedToUserFromWeighBridge]")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("UserId", u.getId())
                .rows();
    }

    /** BLL 0298 GatePassPartyProcessing.GetGatePassReferenceType — columns Id, Name. */
    public List<Map<String, Object>> gatePassReferenceTypes(UserAccount u) {
        return new Call("dbo.Sp_GatePassReferenceTypes_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .rows();
    }

    /** CommonServices.GatePassTypeFill("WeighBridge") — columns Id, GpTypeDescription. */
    public List<Map<String, Object>> weighBridgeTypes(UserAccount u) {
        return new Call("dbo.Sp_GatePassType_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Activity", "WeighBridge")
                .rows();
    }

    /** BLL 0600 SupplierCustomer.GetPartiesFromWbTransactions — columns Id, CompanyName. */
    public List<Map<String, Object>> partiesFromWbTransactions(UserAccount u) {
        return new Call("dbo.Sp_WbTransation_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Activity", "GetPartiesFromWbTransactions")
                .rows();
    }

    /** BLL 0089 WbPartyandItemsDefine.GetWbParties — columns Id, PartyName. */
    public List<Map<String, Object>> wbParties(UserAccount u) {
        return new Call("dbo.Usp_GetWbParties")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .rows();
    }

    /** ReportsParameters fields grdweightBridge():230 fills. */
    public static final class HistoryFilter {
        public int financialYearId;
        public int branchesId;                  // → @BranchId
        public Timestamp fromDate, toDate;      // always set on the desktop (DateTimePicker values)
        public double fromDocNo, toDocNo;       // → @TicketNoFrom / @TicketNoTo
        public int gpSrNoF, gpSrNoT;
        public String vehicleNo;
        public String refDocumentTypeIds;       // ",51,91"
        public String branchesIds;              // ",3,4"
        public String activity;                 // → @WeighBridgeType
        public int supplierCustomerId;
        public int documentTypeId;
        public int orderNoFrom, orderNoTo;
        public String companyName;              // → @WbPartyName
    }

    /** BLL 0130 WbTransactionsReports.WbTransactionHistory:133 — every guard as written. */
    public List<Map<String, Object>> history(UserAccount u, HistoryFilter f) {
        return new Call("dbo.Sp_WbTransactions_History")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .opt("FinancialYearId", f.financialYearId)
                .opt("BranchId", f.branchesId)
                .opt("DateFrom", f.fromDate)
                .opt("DateTo", f.toDate)
                .opt("TicketNoFrom", f.fromDocNo)
                .opt("TicketNoTo", f.toDocNo)
                .opt("GpNoFrom", f.gpSrNoF)
                .opt("GpNoTo", f.gpSrNoT)
                .opt("VehicleNo", f.vehicleNo)
                .opt("RefDocumentTypeIds", f.refDocumentTypeIds)
                .opt("BranchesIds", f.branchesIds)
                .opt("WeighBridgeType", f.activity)
                .opt("SupplierCustomerId", f.supplierCustomerId)
                .opt("DocumentTypeId", f.documentTypeId)
                .opt("OrderNoFrom", f.orderNoFrom)
                .opt("OrderNoTo", f.orderNoTo)
                .opt("WbPartyName", f.companyName)
                .rows();
    }

    /** BLL 0130 WbTransactionSlip280:14 — the per-row "Print" button (280 slip). */
    public List<Map<String, Object>> slip280(UserAccount u, int id, int documentTypeId) {
        return new Call("dbo.Sp_WbTransactionsSlip_rpt")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .opt("Id", id)
                .opt("DocumentTypeId", documentTypeId)
                .rows();
    }

    /** clsGlobalVariables.ActiveYr.Start_Period — the active year's row from the login procedure. */
    public Object financialYearStart(UserAccount u, int financialYearId) {
        for (Map<String, Object> r : new Call("dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .rows()) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId) return r.get("Start_Period");
        }
        return null;
    }

    // ======================================= WeighBridge_WeightUpdate (Admin Panel, no screen row)

    /**
     * BLL 0130 WbTransactionsReports.WbTransactionHistory_Updating:300 → USP_WbTransactions_NewReport.
     * Same guards as WbTransactionHistory plus {@code if (Status != "") @GpStatus}. The BLL would
     * also send @BranchId / @mId when non-zero; the form leaves both 0, and the procedure does not
     * declare either, so neither is ever sent.
     */
    public List<Map<String, Object>> historyForUpdating(UserAccount u, HistoryFilter f, String gpStatus) {
        return new Call("[dbo].[USP_WbTransactions_NewReport]")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .opt("FinancialYearId", f.financialYearId)
                .opt("DateFrom", f.fromDate)
                .opt("DateTo", f.toDate)
                .opt("TicketNoFrom", f.fromDocNo)
                .opt("TicketNoTo", f.toDocNo)
                .opt("GpNoFrom", f.gpSrNoF)
                .opt("GpNoTo", f.gpSrNoT)
                .opt("VehicleNo", f.vehicleNo)
                .opt("RefDocumentTypeIds", f.refDocumentTypeIds)
                .opt("BranchesIds", f.branchesIds)
                .opt("WeighBridgeType", f.activity)
                .opt("SupplierCustomerId", f.supplierCustomerId)
                .opt("DocumentTypeId", f.documentTypeId)
                .opt("OrderNoFrom", f.orderNoFrom)
                .opt("OrderNoTo", f.orderNoTo)
                .opt("WbPartyName", f.companyName)
                .opt("GpStatus", gpStatus)
                .rows();
    }

    /** BLL 0091 WbTransactions.GetByID — @Id and @Activity only; used to check the row's company
     *  and read its stored weights before an update. */
    public Map<String, Object> wbTransactionById(int id) {
        List<Map<String, Object>> rows = new Call("dbo.Sp_WbTransation_GetAllMethod")
                .p("Id", id)
                .p("Activity", "ReadById")
                .rows();
        return rows.isEmpty() ? null : rows.get(0);
    }

    /** One entry of ReportsParameterslist. */
    public static final class WeightUpdate {
        public int id;
        public int actionId;
        public double firstWeight;
        public double secondWeight;
        public String comments;
    }

    /**
     * BLL 0091 WbTransactions.WbTransactions_WeightUpdation:1032 — one transaction, one EXEC per
     * row, in the desktop's parameter order (@Id, @UserId, @ActionId, @Comments, @FirstWeight,
     * @SecondWeight). A RAISERROR on any row rolls back every row, as on the desktop.
     */
    @Transactional
    public void weightUpdation(UserAccount u, List<WeightUpdate> rows) {
        for (WeightUpdate r : rows) {
            new Call("[dbo].[USP_WbTransactions_WeightUpdation]")
                    .p("Id", r.id)
                    .p("UserId", u.getId())
                    .p("ActionId", r.actionId)
                    .p("Comments", r.comments)
                    .p("FirstWeight", r.firstWeight)
                    .p("SecondWeight", r.secondWeight)
                    .rows();
        }
    }

    // ================================================================================ plumbing

    private int scalarInt(String proc, LinkedHashMap<String, Object> params) {
        Call c = new Call(proc);
        for (Map.Entry<String, Object> e : params.entrySet()) c.p(e.getKey(), e.getValue());
        List<Map<String, Object>> rows = c.rows();
        if (rows.isEmpty() || rows.get(0).isEmpty()) return 0;
        Object v = rows.get(0).values().iterator().next();
        if (v instanceof Number) return ((Number) v).intValue();
        try { return v == null ? 0 : (int) Double.parseDouble(v.toString()); } catch (NumberFormatException e) { return 0; }
    }

    private final class Call {
        private final String proc;
        private final List<String> names = new ArrayList<>();
        private final List<Object> values = new ArrayList<>();

        Call(String proc) { this.proc = proc; }

        Call p(String name, Object value) { names.add(name); values.add(value); return this; }

        Call opt(String name, Object value) {
            if (value == null) return this;
            if (value instanceof Number && ((Number) value).doubleValue() == 0d) return this;
            if (value instanceof String && ((String) value).isEmpty()) return this;
            return p(name, value);
        }

        List<Map<String, Object>> rows() {
            StringBuilder sql = new StringBuilder("EXEC ").append(proc);
            for (int i = 0; i < names.size(); i++) {
                sql.append(i == 0 ? " " : ", ").append('@').append(names.get(i)).append("=?");
            }
            final Object[] args = values.toArray();
            return jdbc.execute(sql.toString(), (java.sql.PreparedStatement ps) -> {
                for (int i = 0; i < args.length; i++) ps.setObject(i + 1, args[i]);
                boolean hasResultSet = ps.execute();
                while (!hasResultSet && ps.getUpdateCount() != -1) hasResultSet = ps.getMoreResults();
                List<Map<String, Object>> out = new ArrayList<>();
                if (!hasResultSet) return out;
                try (java.sql.ResultSet rs = ps.getResultSet()) {
                    if (rs == null) return out;
                    java.sql.ResultSetMetaData md = rs.getMetaData();
                    int n = md.getColumnCount();
                    while (rs.next()) {
                        Map<String, Object> row = new LinkedHashMap<>();
                        for (int c = 1; c <= n; c++) {
                            String label = md.getColumnLabel(c);
                            if (label == null || label.isEmpty()) label = "col" + c;
                            Object v = rs.getObject(c);
                            if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString();
                            else if (v instanceof java.sql.Date) v = ((java.sql.Date) v).toLocalDate().toString();
                            row.put(label, v);
                        }
                        out.add(row);
                    }
                }
                return out;
            });
        }
    }
}
