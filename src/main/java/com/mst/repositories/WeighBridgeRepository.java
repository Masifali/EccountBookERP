package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.sql.Types;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 411 "Weigh Bridge" — {@code Architecture.WinApp.WeightBridge.frmWeightbridge}.
 *
 * ---------------------------------------------------------------------------------------------
 * EVERY CALL BELOW IS A DESKTOP BLL METHOD, PARAMETER FOR PARAMETER
 * ---------------------------------------------------------------------------------------------
 * The desktop builds each SqlParameter list by hand, and several parameters are only added when
 * a value is non-zero / non-empty ({@code if (obj.X != 0) list.Add(...)}). Those guards are
 * reproduced exactly with {@link Call#opt}: a guarded parameter that the desktop would leave out
 * is left out here too, so the procedure falls back to its own default instead of receiving a 0
 * the desktop never sends. The BLL file and line each method came from is named on it.
 *
 * ---------------------------------------------------------------------------------------------
 * THE WRITE PATH — DAL 0083 WbTransactions.SetDate, STEP FOR STEP, IN ONE TRANSACTION
 * ---------------------------------------------------------------------------------------------
 *   1  Sp_WbTransactions_Insert | Sp_WbTransactions_Update   -> id (ExecuteScalar)
 *   2  ActionId = Id; Sp_WbTransactionsAudit_Insert           (same parameter set)
 *   3  only when a weighbridge voucher was composed (charges > 0 and both accounts configured):
 *        on update, Sp_Vouchers_GetMethods GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId
 *        Sp_VoucherHead_Insert | Sp_VoucherHead_Update
 *        Sp_VoucherDetail_Insert per line
 *        Sp_VoucherHead_H_Insert -> DocumentTypeIdRef
 *        Sp_VoucherDetail_H_Insert per line
 *
 * DAL 0083 does NOT run USP_VoucherBalanceCheck, the cost-centre insert or the document-approval
 * insert that DAL 0586 (the voucher screens' own writer) does, so neither does this. It also does
 * NOT delete the old voucher lines before re-inserting them on update; that is recorded, not
 * "fixed" — see the service note.
 */
@Repository
public class WeighBridgeRepository {

    private static final Logger LOG = LoggerFactory.getLogger(WeighBridgeRepository.class);

    private final JdbcTemplate jdbc;

    public WeighBridgeRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================================ lookups

    /** BLL 0090 WeighbridgeScalesList.GetAll — frmWeightbridge.WeighBridgeListdtFillDbCall:1164
     *  passes MacAddress = "" (an empty string, which ADO.NET does send). */
    public List<Map<String, Object>> scalesList(UserAccount u) {
        return new Call("dbo.Sp_WeighbridgeScalesList_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("BranchesId", branch(u))
                .p("MacAddress", "")
                .p("Activity", "ReadAll")
                .rows();
    }

    /** BLL 0091 WbTransactions.GenerateCode:475 — reads column TicketNo. */
    public int nextTicketNo(UserAccount u, int financialYearId) {
        List<Map<String, Object>> rows = new Call("dbo.Sp_WbTransation_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .opt("FinancialYearId", financialYearId)
                .opt("BranchId", branch(u))
                .p("Activity", "GenerateDocNo")
                .rows();
        return rows.isEmpty() ? 0 : intOf(ci(rows.get(0), "TicketNo"));
    }

    /** BLL 0298 GatePassPartyProcessing.GetGatePassReferenceType:343, via
     *  CommonServices.GatePassReferenceTypes():5171. Columns Id, Name. */
    public List<Map<String, Object>> gatePassReferenceTypes(UserAccount u) {
        return new Call("dbo.Sp_GatePassReferenceTypes_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .rows();
    }

    /** BLL 0570 GatepassType.GetAll via CommonServices.GatePassTypeFill("WeighBridge"):13979.
     *  Columns Id, GpTypeDescription. */
    public List<Map<String, Object>> gatePassTypes(UserAccount u, String activity) {
        return new Call("dbo.Sp_GatePassType_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Activity", activity)
                .rows();
    }

    /** BLL 0611 VehicleType.GetAll — the method builds an @Activity list and then does NOT pass
     *  it, so the procedure is called with no parameters at all. Reproduced as written. */
    public List<Map<String, Object>> vehicleTypes() {
        return new Call("dbo.Sp_VehicleType_GetAllMethod").rows();
    }

    /** BLL 0089 WbPartyandItemsDefine.GetAll — split into parties/items on WbType by
     *  WeightBridge_Helper.WbPartyandItemsDtFillDbCall. */
    public List<Map<String, Object>> wbPartiesAndItems(UserAccount u) {
        return new Call("dbo.SP_WbPartyandItemsDefine_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Activity", "ReadAll")
                .rows();
    }

    /** clsGlobalVariables.getGlobalAllItems — loaded by GlobalServicesMethods.AllItemsWithModal
     *  (BLL 0379:18) with no paging and no keyword. */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return new Call("[dbo].[USP_Item_AllItemsWithModal]")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .rows();
    }

    /** clsGlobalVariables.globalInvPackingType — GlobalServicesMethods.getGlobalAllPackingType
     *  (BLL 0379:184), @Activity only. */
    public List<Map<String, Object>> packingTypes() {
        return new Call("[dbo].[Sp_InvPackingType_GetAllMethod]").p("Activity", "ReadAll").rows();
    }

    /** CommonServies.GetConfigurationFromAllocation (BLL 0267:18) — ConfigKey, or "". */
    public String config(UserAccount u, String configDescription) {
        List<Map<String, Object>> rows = new Call("dbo.Sp_ConfigrationsAllocation_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("ConfigDescription", configDescription)
                .p("Activity", "GetConfigurationByOrgCompandConfigDescription")
                .rows();
        if (rows.isEmpty()) return "";
        Object v = ci(rows.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    // ============================================================ gate pass pickers (bending)

    /** WeightBridge_Helper.GetGpNoFromLab -> BLL 0403 GetGatePassIdandNoforWeighBridge:890. */
    public List<Map<String, Object>> gatePassesFromLab(UserAccount u, int financialYearId, String wbStatus) {
        return new Call("dbo.USP_GetGatePassIdandNoforWeighBridge")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("WeighBridgeStatus", wbStatus)
                .opt("FinancialYearId", financialYearId)
                .opt("BranchesId", branch(u))
                .rows();
    }

    /** BLL 0403 GetGatePassIdandNoforWeighBridge_Engr:1547 — all four always sent. */
    public List<Map<String, Object>> gatePassesInwardEngr(UserAccount u, int financialYearId) {
        return new Call("dbo.USP_GetGatePassIdandNoforWeighBridge_Engr")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("FinancialYearId", financialYearId)
                .p("BranchesId", branch(u))
                .rows();
    }

    /** BLL 0567 GatePassInward.GetRefDocTypeIdbyRefDocNoId:846, from GetGatePassOutward:923
     *  (RefDocumentTypeId 91, DocumentTypeId 102). */
    public List<Map<String, Object>> gatePassesOutward(UserAccount u, int financialYearId,
                                                       boolean checkConfig, int recId) {
        return new Call("dbo.Sp_GatePassInward_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("RefDocumentTypeId", 91)
                .p("DocumentTypeId", 102)
                .p("CheckConfig", checkConfig)
                .opt("FinancialYearId", financialYearId)
                .opt("BranchesId", branch(u))
                .opt("RecId", recId)
                .p("Activity", "GetRefDocNoidByDocumentTypeId")
                .rows();
    }

    /** BLL 0567 GatePassInward.GetForWeighBridgeGatePassGeneral:972 — DocumentTypeId 52 or 92. */
    public List<Map<String, Object>> gatePassesGeneral(UserAccount u, int financialYearId, int documentTypeId) {
        return new Call("dbo.Sp_GatePassInward_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("DocumentTypeId", documentTypeId)
                .opt("FinancialYearId", financialYearId)
                .opt("BranchesId", branch(u))
                .p("Activity", "GetForWeighBridgeGatePassGeneral")
                .rows();
    }

    /** BLL 0298 GetGatePassPartyProcessingForWeighBridge:366 — WeighBridgeStatus "Auto". */
    public List<Map<String, Object>> gatePassesPartyProcessing(UserAccount u, int documentTypeId) {
        return new Call("dbo.Sp_GatePassPartyProcessing_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("DocumentTypeId", documentTypeId)
                .p("WeighBridgeStatus", "Auto")
                .p("Activity", "GetGatePassPartyProcessingForWeighBridge")
                .rows();
    }

    // ======================================================= the chosen gate pass (Leave fill)

    /** CommonServices.GetByGpNoInward:13846 -> BLL 0567 GatePassInward.GetByGpNo:1021. The
     *  object CommonServices builds has no BranchesId, so the guarded @BranchesId is not sent. */
    public List<Map<String, Object>> gatePassInwardById(UserAccount u, int financialYearId, int documentTypeId, int id) {
        return new Call("dbo.Sp_GatePassInward_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Id", id)
                .p("DocumentTypeId", documentTypeId)
                .p("FinancialYearId", financialYearId)
                .p("Activity", "ReadByGpNo")
                .rows();
    }

    /** CommonServices.GetByGpNo:14746 -> BLL 0568 GatePassOutward.GetByGpNo:212. */
    public List<Map<String, Object>> gatePassOutwardById(UserAccount u, int financialYearId, int documentTypeId, int id) {
        return new Call("dbo.Sp_GatePassOutward_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("DocumentTypeId", documentTypeId)
                .p("Id", id)
                .p("FinancialYearId", financialYearId)
                .p("Activity", "ReadByGpNo")
                .rows();
    }

    /** BLL 0550 GatePassGeneral.GatePassGeneralReadByGpNo:432 — keyed on the GP number TEXT the
     *  combo shows, not on its id (frmWeightbridge :4103 / :4125). */
    public List<Map<String, Object>> gatePassGeneralByNo(UserAccount u, int financialYearId, int documentTypeId, int gpSrNo) {
        return new Call("dbo.Sp_GatePassGeneral_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("GpSrNo", gpSrNo)
                .p("DocumentTypeId", documentTypeId)
                .opt("FinancialYearId", financialYearId)
                .opt("BranchesId", branch(u))
                .p("Activity", "GatePassGeneralReadByGpNo")
                .rows();
    }

    /** BLL 0298 ReadByGpNoForWeighBridge:404. The form also sets BranchesId on the object, but
     *  this BLL method never adds it, so it is not sent. */
    public List<Map<String, Object>> gatePassPartyProcessingByNo(UserAccount u, int financialYearId, int documentTypeId, int gpSrNo) {
        return new Call("dbo.Sp_GatePassPartyProcessing_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("DocumentTypeId", documentTypeId)
                .opt("FinancialYearId", financialYearId)
                .opt("GpSrNo", gpSrNo)
                .p("Activity", "ReadByGpNoForWeighBridge")
                .rows();
    }

    /** BLL 0567 getItemsFromLabOrPurchaseOrderOrAllByGpId:2620 — columns ItemId, ItemName. */
    public List<Map<String, Object>> labItemsByGatePass(UserAccount u, int gpId, int actionId) {
        return new Call("dbo.usp_getItemsFromLabOrPurchaseOrderOrAllByGpId")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Id", gpId)
                .opt("ActionId", actionId)
                .rows();
    }

    /** BLL 0558 InvDeliveryOrder.GetItemsByDeliveryOrderId:2017 — refuses with its own text when
     *  both ids are zero. */
    public List<Map<String, Object>> itemsByDeliveryOrder(UserAccount u, int deliveryOrderId, int gpId) {
        if (deliveryOrderId == 0 && gpId == 0) {
            throw new IllegalArgumentException("Delivery OrderId Or Gate Pass OutwardId not found");
        }
        return new Call("dbo.Sp_InvDeliveryOrder_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .opt("Id", deliveryOrderId)
                .opt("GpId", gpId)
                .p("Activity", "ItemsByDeliveryOrderId")
                .rows();
    }

    /** BLL 0558 GetPartiesAndItemsFromDeliveryOrderByGpId:2083. The BLL names the second
     *  parameter "RecId" WITHOUT the @; ADO.NET adds it silently, raw SQL does not, so it is
     *  written @RecId here (see REPORT-CONTRACTS-SEEDED-239, the six unprefixed parameters). */
    public List<Map<String, Object>> partiesAndItemsFromDeliveryOrder(int gpId, int recId) {
        return new Call("[dbo].[usp_getPartiesAndItemsFromDeliveryOrderByGpId]")
                .p("GpId", gpId)
                .opt("RecId", recId)
                .rows();
    }

    /** BLL 0558 getInvoiceNoFromDeliveryOrderByGpId:2065 — columns Id, InvoiceNo. */
    public List<Map<String, Object>> invoiceNosFromDeliveryOrder(int gpId) {
        return new Call("dbo.usp_getInvoiceNoFromDeliveryOrderByGpId").p("GpId", gpId).rows();
    }

    /** BLL 0596 SaleOrder.GetItemsBySaleOrderId:2307 — columns Id, ItemName. */
    public List<Map<String, Object>> itemsBySaleOrder(UserAccount u, int saleOrderId) {
        return new Call("dbo.Sp_SaleOrder_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Id", saleOrderId)
                .p("Activity", "ItemsBySaleOrderId")
                .rows();
    }

    /** BLL 0568 GatePassOutward.GetDeliveryOrderType:1389 — column DeliveryOrderType. */
    public String deliveryOrderType(UserAccount u, int gatePassOutwardId) {
        List<Map<String, Object>> rows = new Call("dbo.Sp_GatePassOutward_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Id", gatePassOutwardId)
                .p("Activity", "GetDeliveryOrderType")
                .rows();
        if (rows.isEmpty()) return "";
        Object v = ci(rows.get(0), "DeliveryOrderType");
        return v == null ? "" : String.valueOf(v);
    }

    // ================================================ Weigh Bridge Manual (screen 410) extras

    /** BLL 0567 GatePassInward.GetGatePassOutwardForMannualWeighBridge:913 — used by
     *  WeightbridgeMannual.GetGatePassOutward (DocumentTypeId 91, CheckConfig, RecId) and
     *  GetGatePassOutwardSteel (DocumentTypeId 1507, CheckConfig false, RecId 0). */
    public List<Map<String, Object>> gatePassesOutwardManual(UserAccount u, int financialYearId, int documentTypeId,
                                                             boolean checkConfig, int recId) {
        return new Call("dbo.Sp_GatePassInward_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("DocumentTypeId", documentTypeId)
                .p("FinancialYearId", financialYearId)
                .opt("BranchesId", branch(u))
                .opt("RecId", recId)
                .p("CheckConfig", checkConfig)
                .p("Activity", "GetGatePassOutwardForMannualWeighBridge")
                .rows();
    }

    /** WeightbridgeMannual.GatePassPartyProcessing():842 — the same BLL call as screen 411 but
     *  with WeighBridgeStatus "Manual". */
    public List<Map<String, Object>> gatePassesPartyProcessingManual(UserAccount u, int documentTypeId) {
        return new Call("dbo.Sp_GatePassPartyProcessing_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("DocumentTypeId", documentTypeId)
                .p("WeighBridgeStatus", "Manual")
                .p("Activity", "GetGatePassPartyProcessingForWeighBridge")
                .rows();
    }

    /** CommonServices.StaticColumnsService(activity) -> GeneralReprots.StaticColumnNames
     *  (BLL 0136:177) — "PackingTypeForWB" gives Id, type. */
    public List<Map<String, Object>> staticColumns(String activity) {
        return new Call("dbo.SpStaticColumnNames").p("Activity", activity).rows();
    }

    /** CommonServices.ReadAllItemsWithPackingAndStore -> Item.ReadAllItemsWithPackingAndStore
     *  (BLL 0583:2197). */
    public List<Map<String, Object>> itemsWithPackingAndStore(UserAccount u) {
        return new Call("dbo.Sp_Item_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Activity", "ReadAllItemsWithPackingAndStore")
                .rows();
    }

    /** BLL 0298 GatePassPartyProcessing.GetVehicleWeight:590 — the object carries no financial
     *  year, so the guarded @FinancialYearId is not sent. */
    public List<Map<String, Object>> savedVehicleWeights(UserAccount u) {
        return new Call("dbo.Sp_WbTransation_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("Activity", "GetVehicleWeight")
                .rows();
    }

    /** GatePassGeneral.GatePassGeneralReadByGpNo as WeightbridgeMannual calls it: the object it
     *  builds has no BranchesId, so @BranchesId is not sent (frmWeightbridge's call sends it). */
    public List<Map<String, Object>> gatePassGeneralByNoNoBranch(UserAccount u, int financialYearId, int documentTypeId, int gpSrNo) {
        return new Call("dbo.Sp_GatePassGeneral_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("GpSrNo", gpSrNo)
                .p("DocumentTypeId", documentTypeId)
                .opt("FinancialYearId", financialYearId)
                .p("Activity", "GatePassGeneralReadByGpNo")
                .rows();
    }

    /** GatePassPartyProcessing.ReadByGpNoForWeighBridge as WeightbridgeMannual calls it — by the
     *  gate pass Id (GpSrNo left 0, so not sent). */
    public List<Map<String, Object>> gatePassPartyProcessingById(UserAccount u, int financialYearId, int documentTypeId, int id) {
        return new Call("dbo.Sp_GatePassPartyProcessing_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("DocumentTypeId", documentTypeId)
                .opt("FinancialYearId", financialYearId)
                .opt("Id", id)
                .p("Activity", "ReadByGpNoForWeighBridge")
                .rows();
    }

    // ========================================================================= pending grids

    /** BLL 0091 GatepassPendingForWeighBridge:1347 (grdPendingFirst). */
    public List<Map<String, Object>> pendingForFirstWeight(UserAccount u, Timestamp gpDateF, Timestamp gpDateT,
                                                           int gpNoFrom, int gpNoTo) {
        return new Call("dbo.SP_GatepassPendingForWeighBridge")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .opt("BranchesId", branch(u))
                .opt("GpDateF", gpDateF)
                .opt("GpDateT", gpDateT)
                .opt("GpNoFrom", gpNoFrom)
                .opt("GpNoTo", gpNoTo)
                .rows();
    }

    /** BLL 0091 PendingWeighBridgeForSecondWeight:721 (grdPendingForSecond). */
    public List<Map<String, Object>> pendingForSecondWeight(UserAccount u, int financialYearId, int firstWeighBridgeId,
                                                            Timestamp gpDateF, Timestamp gpDateT,
                                                            int gpNoFrom, int gpNoTo) {
        return new Call("dbo.USP_PendingWeighBridgeForSecondWeight")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("FinancialYearId", financialYearId)
                .opt("BranchId", branch(u))
                .opt("FirstWeighBridgeId", firstWeighBridgeId)
                .opt("GpDateF", gpDateF)
                .opt("GpDateT", gpDateT)
                .opt("GpNoFrom", gpNoFrom)
                .opt("GpNoTo", gpNoTo)
                .rows();
    }

    // ================================================================================ history

    /** The ReportsParameters fields BindHistoryGrid():3537 fills. */
    public static final class HistoryFilter {
        public boolean canViewAllRecord;
        public int entryUser;
        public int firstWeighBridgeId;          // ReportsParameters.ActionId
        public int ticketNoFrom, ticketNoTo;    // FromDocNo / ToDocNo
        public Timestamp dateFrom, dateTo, entryFrom, entryTo, modifyFrom, modifyTo, approvedFrom, approvedTo;
        public int gpNoFrom, gpNoTo;
        public String vehicleNo;
        public int refDocumentTypeId;
        public String weighBridgeType;          // ReportsParameters.Activity
    }

    /** BLL 0091 WbTransactions.FormHistory:526. */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, HistoryFilter f) {
        Call c = new Call("dbo.Sp_WbTransation_GetAllMethod")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("CanViewAllRecord", f.canViewAllRecord)
                .p("FinancialYearId", financialYearId)
                .p("BranchId", branch(u));
        if (!f.canViewAllRecord) c.p("EntryUser", f.entryUser);
        c.opt("DateFrom", f.dateFrom).opt("DateTo", f.dateTo)
         .opt("EntryFromDate", f.entryFrom).opt("EntryToDate", f.entryTo)
         .opt("ModifyFromDate", f.modifyFrom).opt("ModifyToDate", f.modifyTo)
         .opt("ApprovedFromDate", f.approvedFrom).opt("ApprovedToDate", f.approvedTo)
         .opt("TicketNoFrom", f.ticketNoFrom).opt("TicketNoTo", f.ticketNoTo)
         .opt("GpNoFrom", f.gpNoFrom).opt("GpNoTo", f.gpNoTo)
         .opt("VehicleNo", f.vehicleNo)
         .opt("WeighBridgeType", f.weighBridgeType)
         .opt("RefDocumentTypeId", f.refDocumentTypeId)
         .opt("FirstWeighBridgeId", f.firstWeighBridgeId)
         .p("Activity", "FormHistory");
        return c.rows();
    }

    /** BLL 0091 WbTransactions.GetByID:841 — @Id and @Activity only, exactly as the desktop. */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> rows = new Call("dbo.Sp_WbTransation_GetAllMethod")
                .p("Id", id)
                .p("Activity", "ReadById")
                .rows();
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ================================================================================ prints

    /** BLL 0130 WbTransactionsReports.WbTransactionSlip280:14 — the 280 / 281 slips. */
    public List<Map<String, Object>> slip280(UserAccount u, int id, int documentTypeId) {
        return new Call("dbo.Sp_WbTransactionsSlip_rpt")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .opt("Id", id)
                .opt("DocumentTypeId", documentTypeId)
                .rows();
    }

    /** CommonServices.InwardGatePassWithWbAndLabSlip:14268 — main set of the 257 slip. */
    public List<Map<String, Object>> inwardGatePassSlip(UserAccount u, int gatePassId) {
        return new Call("dbo.Sp_GatePassInward_SlipAndRegister_Rpt")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .opt("Id", gatePassId)
                .rows();
    }

    /** BLL 0403 LabAnalysisPurchaseByGP_SubReport:1580. */
    public List<Map<String, Object>> labAnalysisByGatePassSubReport(UserAccount u, int gatePassId) {
        return new Call("dbo.USP_InvLabAnalysisPurchaseByGP_SubReport")
                .p("OrganizationId", u.getOrganizationId())
                .p("CompanyId", u.getCompanyId())
                .p("GpId", gatePassId)
                .rows();
    }

    /** BLL 0091 WbTransationByGPID_SubReport:968. */
    public List<Map<String, Object>> wbByGatePassSubReport(int gatePassId) {
        return new Call("[dbo].[USP_WbTransationByGPID_SubReport]").p("GpId", gatePassId).rows();
    }

    // ================================================================================== write

    /**
     * DAL 0083 WbTransactions.SetDate. {@code wb} is the model's non-virtual property set in
     * declaration order (see {@link WeighBridgeRecord}); {@code voucher} is null unless
     * BLL Save():448 composed one.
     *
     * @return what SetDate returns: the id the procedure handed back, or the record's own id when
     *         it handed back nothing (the update path).
     */
    @Transactional(propagation = Propagation.REQUIRED)
    public int save(WeighBridgeRecord wb, String proc,
                    ContraVoucherDto.Head voucherHead, List<ContraVoucherDto.Detail> voucherLines) {
        int returned = scalarInt(proc, wb.parameters());
        int num3;
        if (returned > 0) {
            wb.Id = returned;
            num3 = returned;
        } else {
            num3 = wb.Id == null ? 0 : wb.Id;
        }
        if (wb.Id != null && wb.Id > 0) {
            wb.ActionId = wb.Id;
            scalarInt("dbo.Sp_WbTransactionsAudit_Insert", wb.parameters());
        }

        if (voucherHead != null && voucherLines != null && !voucherLines.isEmpty()) {
            int existingHeadId = 0;
            if (wb.ModifyUser != null && wb.ModifyUser > 0) {
                List<Map<String, Object>> rows = new Call("dbo.Sp_Vouchers_GetMethods")
                        .p("Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId")
                        .p("OrganizationId", wb.OrganizationId)
                        .p("CompanyId", wb.CompanyId)
                        .p("DocumentTypeId", wb.DocTypeId)
                        .p("DocumentTypeSrNo", wb.Id)
                        .rows();
                if (!rows.isEmpty()) {
                    existingHeadId = intOf(ci(rows.get(0), "Id"));
                    voucherHead.Id = existingHeadId;
                }
            }
            voucherHead.DocumentTypeSrNo = wb.Id;
            int headId = objectScalarInt(existingHeadId == 0 ? "dbo.Sp_VoucherHead_Insert" : "dbo.Sp_VoucherHead_Update",
                                         voucherHead);
            if (headId > 0) voucherHead.Id = headId;
            for (ContraVoucherDto.Detail d : voucherLines) {
                d.VoucherHeadId = voucherHead.Id;
                objectScalarInt("dbo.Sp_VoucherDetail_Insert", d);
            }
            int documentTypeIdRef = objectScalarInt("dbo.Sp_VoucherHead_H_Insert", voucherHead);
            for (ContraVoucherDto.Detail d : voucherLines) {
                d.VoucherHeadId = voucherHead.Id;
                d.DocumentTypeIdRef = documentTypeIdRef;
                objectScalarInt("dbo.Sp_VoucherDetail_H_Insert", d);
            }
        }
        return num3;
    }

    /**
     * {@code Architecture.Model.WeightBridge.WbTransactions} — every NON-VIRTUAL property in
     * declaration order (model 0108). GenericProvider.SetProc sends one @Name per property via
     * AddWithValue; a property whose value is a C# null (an unassigned string, a null
     * DateTime?) reaches SQL Server as "not supplied", so the procedure's own default applies.
     * {@link #parameters()} reproduces that by leaving null fields out. Value-type properties
     * (int, double, bool, DateTime) are never null in C#, so they always go.
     *
     * The four virtual members — RefDocumentTypeIdGpIn/GpOut/St and VoucherHeadWeighBridge —
     * are skipped by SetProc and are not here.
     */
    public static final class WeighBridgeRecord {
        public Boolean   IsApproved = Boolean.FALSE;
        public Timestamp ApprovedDateTime;              // DateTime? — null means not sent
        public Timestamp DocDate;
        public Timestamp EntryDate;
        public Timestamp FirstDateTime;
        public Timestamp ModifyDate;
        public Timestamp SecondDateTime;
        public Double    FirstWeight = 0d;
        public Double    ItemQty = 0d;
        public Double    SecondWeight = 0d;
        public Double    WbCharges = 0d;
        public Double    NetWbWeight = 0d;
        public Double    PackingUnitWeight = 0d;
        public Double    PackingTotalWeight = 0d;
        public Double    WeightAfterPackingWeight = 0d;
        public Double    LoadWeight = 0d;
        public Double    TearWeight = 0d;
        public Double    SupplierWbNetWeight = 0d;
        public Boolean   MultiItem = Boolean.FALSE;
        public Integer   ApprovedUserId = 0;
        public Integer   CompanyId = 0;
        public Integer   DocTypeId = 0;
        public Integer   EntryNo = 0;
        public Integer   EntryUser = 0;
        public Integer   Id = 0;
        public Integer   ActionId = 0;
        public Integer   ModifyUser = 0;
        public Integer   OrganizationId = 0;
        public Integer   ReferenceDocNoId = 0;
        public Integer   ReferenceDocTypeId = 0;
        public Integer   SupplierCustomerId = 0;
        public Integer   TicketNo = 0;
        public String    BiltyNo;
        public String    ContainerNo;
        public String    FirstWtFrontPic;
        public String    FirstWtPicReading;
        public String    FirstWtUpperPic;
        public String    ItemDescription;
        public String    PartyName;
        public String    SecondPicReading;
        public String    SecondWtFrontPic;
        public String    SecondWtUpperPic;
        public String    VehicleNo;
        public String    VehicleType;
        public String    WbRemarks;
        public Integer   WorkingReportNo = 0;
        public Integer   FinancialYearId = 0;
        public Integer   ItemId = 0;
        public Integer   BranchId = 0;
        public Integer   ProjectId = 0;
        public Integer   InvoiceId = 0;
        public String    WeightDiffComments;
        public String    WeighBridgeType;
        public String    PackingType;
        public Integer   FirstWeighBridgeId = 0;
        public Integer   SecondWeighBridgeId = 0;

        /** name -> value, in declaration order, nulls omitted. */
        public LinkedHashMap<String, Object> parameters() {
            LinkedHashMap<String, Object> out = new LinkedHashMap<>();
            for (java.lang.reflect.Field f : WeighBridgeRecord.class.getFields()) {
                if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
                Object v;
                try { v = f.get(this); } catch (IllegalAccessException e) { continue; }
                if (v != null) out.put(f.getName(), v);
            }
            return out;
        }
    }

    // ============================================================================== plumbing

    private static int branch(UserAccount u) {
        return u.getBranchesId() == null ? 0 : u.getBranchesId();
    }

    /** EXEC proc @k=?, ... over an ordered name/value map; first column of first row as int. */
    private int scalarInt(String proc, LinkedHashMap<String, Object> params) {
        Call c = new Call(proc);
        for (Map.Entry<String, Object> e : params.entrySet()) c.p(e.getKey(), e.getValue());
        try {
            List<Map<String, Object>> rows = c.rows();
            if (rows.isEmpty() || rows.get(0).isEmpty()) return 0;
            return intOf(rows.get(0).values().iterator().next());
        } catch (RuntimeException e) {
            LOG.warn("{} failed", proc, e);
            throw e;
        }
    }

    /**
     * The voucher shapes are the Contra DTO's, which DesktopVoucherWriter already sends field for
     * field (checked against Sp_VoucherHead_* / Sp_VoucherDetail_* there). A null carries its
     * field's SQL type, as DesktopVoucherWriter does, so a NULL date is not sent as an INT null.
     */
    private int objectScalarInt(String proc, Object obj) {
        Call c = new Call(proc);
        for (java.lang.reflect.Field f : obj.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            Object v;
            try { v = f.get(obj); } catch (IllegalAccessException e) { continue; }
            c.p(f.getName(), v == null ? typedNull(f.getType()) : v);
        }
        try {
            List<Map<String, Object>> rows = c.rows();
            if (rows.isEmpty() || rows.get(0).isEmpty()) return 0;
            return intOf(rows.get(0).values().iterator().next());
        } catch (RuntimeException e) {
            LOG.warn("{} failed", proc, e);
            throw e;
        }
    }

    private static SqlParameterValue typedNull(Class<?> type) {
        if (type == Integer.class || type == int.class) return new SqlParameterValue(Types.INTEGER, null);
        if (type == Double.class || type == double.class) return new SqlParameterValue(Types.DOUBLE, null);
        if (type == BigDecimal.class) return new SqlParameterValue(Types.NUMERIC, null);
        if (type == Boolean.class || type == boolean.class) return new SqlParameterValue(Types.BIT, null);
        return new SqlParameterValue(Types.VARCHAR, null);
    }

    /** One procedure call with named parameters. */
    private final class Call {
        private final String proc;
        private final List<String> names = new ArrayList<>();
        private final List<Object> values = new ArrayList<>();

        Call(String proc) { this.proc = proc; }

        Call p(String name, Object value) { names.add(name); values.add(value); return this; }

        /** The desktop's {@code if (x != 0)} / {@code if (!string.IsNullOrEmpty(x))} /
         *  {@code if (!Conversion.CheckDateTimeNull(x))} guards. */
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
            return exec(sql.toString(), values.toArray());
        }
    }

    /**
     * Execute a procedure that may or may not return a result set, and return its first result
     * set (empty when there is none) — a DataTable fill, not queryForList, which throws when a
     * procedure returns no rows at all. Same approach as DesktopVoucherWriter.exec.
     */
    private List<Map<String, Object>> exec(String sql, Object... args) {
        return jdbc.execute(sql, (java.sql.PreparedStatement ps) -> {
            for (int i = 0; i < args.length; i++) {
                Object a = args[i];
                if (a instanceof SqlParameterValue) {
                    SqlParameterValue v = (SqlParameterValue) a;
                    if (v.getValue() == null) ps.setNull(i + 1, v.getSqlType());
                    else ps.setObject(i + 1, v.getValue(), v.getSqlType());
                } else {
                    ps.setObject(i + 1, a);
                }
            }
            boolean hasResultSet = ps.execute();
            while (!hasResultSet && ps.getUpdateCount() != -1) {
                hasResultSet = ps.getMoreResults();
            }
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
                        if (v instanceof java.sql.Timestamp) v = ((java.sql.Timestamp) v).toLocalDateTime().toString();
                        else if (v instanceof java.sql.Date) v = v.toString();
                        row.put(label, v);
                    }
                    out.add(row);
                }
            }
            return out;
        });
    }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    public static int intOf(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return (int) Math.round(Double.parseDouble(s)); }
        catch (NumberFormatException e) { return 0; }
    }
}
