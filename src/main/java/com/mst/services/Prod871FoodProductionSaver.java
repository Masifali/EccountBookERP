package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository;
import com.mst.repositories.ProductionInputRepository;
import com.mst.repositories.ProductionOutputRepository;
import com.mst.repositories.ProductionOverheadRepository;
import com.mst.repositories.ProductionSettlementRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.col;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.str;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.toDouble;
import static com.mst.repositories.Prod871ProductionAgainstPreCostingJobOrderRepository.toInt;

/**
 * Screen 871 - the BLL/DAL write paths the form's Save buttons reach:
 *
 * <pre>
 *  InsertInput:2257          -> InvFoodProduction.Save   (DocumentTypeId 80,  detail Sp_InvFoodProductionDetail_Insert)
 *  OutPutInsert:3825         -> InvFoodProduction.Save   (DocumentTypeId 112, DAL = ProductionOutputRepository.save)
 *  InsertConsumption:10116   -> InvFoodProduction.Save   (DocumentTypeId 181, detail USP_InvFoodProductionConsumptionDetail_Insert)
 *  PackingMaterialInsert:4702 -> InvFoodProductionPackingMaterial.Save (111)
 *  OverHeadInsert:5436       -> InvFoodProductionOverHeads.Save (110)
 *  UpdateSettlement:8139 / SettlementFinancials:8228 / btnApproveSettlement:8546 -> ProductionSettlementRepository
 * </pre>
 *
 * BLL InvFoodProduction.Save (:4994) and DAL SetData are reproduced statement for statement from the
 * sibling 280 port (same BLL, same procedures), here parameterised by document type. The generic DAL
 * plumbing of ProductionInputRepository (SetProc parameter sets, model defaults, FIFO reads) is reused
 * unchanged. Everything runs in ONE transaction per save, as the desktop's SqlTransaction does.
 */
@Service
public class Prod871FoodProductionSaver {

    private static final int ERP_FEATURE_FIFO_CGS = 5;
    private static final int MOVE_ORDER_DOCUMENT_TYPE_ID = 74;

    private final Prod871ProductionAgainstPreCostingJobOrderRepository repo;
    private final ProductionInputRepository dal;
    private final ProductionOutputRepository outputDal;
    private final ProductionOverheadRepository overheadDal;
    private final ProductionSettlementRepository settlementDal;
    private final CurrentUserContext ctx;
    private final PlatformTransactionManager txManager;

    public Prod871FoodProductionSaver(Prod871ProductionAgainstPreCostingJobOrderRepository repo,
                                      ProductionInputRepository dal, ProductionOutputRepository outputDal,
                                      ProductionOverheadRepository overheadDal, ProductionSettlementRepository settlementDal,
                                      CurrentUserContext ctx, PlatformTransactionManager txManager) {
        this.repo = repo;
        this.dal = dal;
        this.outputDal = outputDal;
        this.overheadDal = overheadDal;
        this.settlementDal = settlementDal;
        this.ctx = ctx;
        this.txManager = txManager;
    }

    /** MessageBox.Show(ex.Message) - the text reaches the operator as it is. */
    public static class FormMessage extends RuntimeException {
        public FormMessage(String m) { super(m); }
    }

    // ============================================================ InvFoodProduction.Save (80 / 181)

    /**
     * BLL InvFoodProduction.Save for DocumentTypeId 80 (Input) and 181 (Consumption).
     *
     * @param h        header model (property -> value, no "@"), as InsertInput / InsertConsumption fill it
     * @param details  detail models (80: InvFoodProductionDetail; 181: InvFoodProductionConsumptionDetail)
     * @param removeIds InputDetailRowsRemoveIds (80) / OutputDetailRowsRemoveIds (181 - the form puts the
     *                  consumption removals there, :10166); ",12,15" or ""
     * @return the saved header id
     */
    public int saveInputOrConsumption(Map<String, Object> h, List<Map<String, Object>> details, String removeIds) {
        UserAccount u = ctx.requireAccountingUser();
        int org = toInt(h.get("OrganizationId")), comp = toInt(h.get("CompanyId"));
        int docType = toInt(h.get("DocumentTypeId"));

        /* BLL Save:4994 - the two switches, then RefDocumentTypeId = 1 when financial effects are off
           (JobOrderCreatewithoutRates only matters for 112). */
        boolean withoutRates = bool(repo.configValue(org, comp, "JobOrderCreatewithoutRates"));
        boolean financialsInactive = bool(repo.configValue(org, comp, "InventoryFinancialsEffectsInActive"));
        int headerRefDocumentTypeId = (financialsInactive || (withoutRates && docType == 112)) ? 1 : 0;
        boolean fifo = repo.erpFeature(org, comp, ERP_FEATURE_FIFO_CGS);

        Map<String, Object> voucher = null;
        List<Map<String, Object>> lines = new ArrayList<>();
        if (headerRefDocumentTypeId != 1) voucher = makeVoucher(u, h, details, fifo, lines);

        boolean insert = toInt(h.get("Id")) == 0;
        h.put("ActionId", insert ? 1 : 2);
        if (insert) h.put("ModifyUser", 0); else h.put("EntryUser", 0);

        final Map<String, Object> voucherF = voucher;
        final int refDocF = headerRefDocumentTypeId;
        TransactionTemplate tx = new TransactionTemplate(txManager);
        Integer id = tx.execute(status -> setData(u, h, details, removeIds, refDocF, voucherF, lines));
        return id == null ? 0 : id;
    }

    /**
     * BLL InvFoodProduction.MakeVoucher (:4080), the 80 / 181 half: a detail posts its Dr WIP / Cr item
     * purchase-GL pair when FIFO is off, or when it came from a loader (RefDocumentTypeId and RefDocNoId > 0),
     * and only when its item is in the GL list. LineId counts posted pairs. AgainstAccountId of the head
     * is the WIP ITEM id, as the BLL writes it.
     */
    private Map<String, Object> makeVoucher(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details,
                                            boolean fifo, List<Map<String, Object>> lines) {
        List<Map<String, Object>> items = dal.itemGlAccounts(u);
        Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
        vh.put("DocumentTypeSrNo", h.get("Id"));
        vh.put("VoucherDate", h.get("DocDate"));
        vh.put("VoucherCode", h.get("DocCode"));
        vh.put("DocumentTypeId", h.get("DocumentTypeId"));
        vh.put("BaseDocumentTypeId", h.get("BaseDocumentTypeId"));
        vh.put("RefAccountId", h.get("WIPAccountId"));
        vh.put("AgainstAccountId", h.get("WIPItemId"));
        LocalDateTime now = LocalDateTime.now().withNano(0);
        vh.put("EntryDate", now);
        vh.put("ModifyDate", now);
        vh.put("EntryUser", h.get("EntryUser"));
        vh.put("ModifyUser", h.get("ModifyUser"));
        vh.put("OrganizationId", h.get("OrganizationId"));
        vh.put("CompanyId", h.get("CompanyId"));
        vh.put("FinancialYearId", h.get("FinancialYearId"));
        vh.put("BranchId", h.get("BranchId"));
        double total = 0d;
        int lineId = 1;
        int wip = toInt(h.get("WIPAccountId"));
        String jobNo = str(h.get("InvJobOrderNo"));
        for (Map<String, Object> d : details) {
            boolean eligible = !fifo || (toInt(d.get("RefDocumentTypeId")) > 0 && toInt(d.get("RefDocNoId")) > 0);
            if (!eligible) continue;
            if (items.isEmpty()) throw new FormMessage("Item GL Account Not Found");
            Map<String, Object> item = firstById(items, toInt(d.get("ItemId")));
            if (item == null) continue;
            String remarks = str(d.get("Remarks"));
            String comment = jobNo + " " + (remarks.trim().isEmpty() ? "" : remarks + " ") + " ItemName: "
                    + str(col(item, "ItemName")) + " Qty " + net(toDouble(d.get("Qty"))) + " Weight "
                    + net(toDouble(d.get("Weight"))) + " Rate " + net(toDouble(d.get("Rate")));
            int purchaseGl = toInt(col(item, "PurchaseGLAC"));
            double amount = toDouble(d.get("Amount"));

            Map<String, Object> dr = ProductionInputRepository.voucherDetailDefaults();
            dr.put("LineId", lineId);
            dr.put("AccountId", wip);
            dr.put("AgainstAccountId", purchaseGl);
            dr.put("Comments", comment);
            dr.put("DebitAmount", amount);
            dr.put("QtyIn", toDouble(d.get("Qty")));
            dr.put("WeightIn", toDouble(d.get("Weight")));
            dr.put("ItemAmount", amount);
            dr.put("OrderNo", toInt(d.get("InvProductionJobOrderId")));
            dr.put("BranchesId", h.get("BranchId"));
            total += amount;
            lines.add(dr);

            Map<String, Object> cr = ProductionInputRepository.voucherDetailDefaults();
            cr.put("LineId", lineId);
            cr.put("AccountId", purchaseGl);
            cr.put("AgainstAccountId", wip);
            cr.put("Comments", comment);
            cr.put("CreditAmount", amount);
            cr.put("QtyOut", toDouble(d.get("Qty")));
            cr.put("WeightOut", toDouble(d.get("Weight")));
            cr.put("ItemId", toInt(d.get("ItemId")));
            cr.put("ItemAmount", amount);
            cr.put("OrderNo", toInt(d.get("InvProductionJobOrderId")));
            cr.put("BranchesId", h.get("BranchId"));
            lines.add(cr);
            lineId++;
        }
        vh.put("VoucherAmount", total);
        return vh;
    }

    /** DAL InvFoodProduction.SetData for 80 / 181, inside the save transaction. */
    private int setData(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details, String removeIds,
                        int headerRefDocumentTypeId, Map<String, Object> voucher, List<Map<String, Object>> lines) {
        int org = toInt(h.get("OrganizationId")), comp = toInt(h.get("CompanyId"));
        int docType = toInt(h.get("DocumentTypeId"));
        boolean insert = toInt(h.get("Id")) == 0;
        boolean consumption = docType == Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_CONSUMPTION;

        int result = dal.setProc(insert ? "Sp_InvFoodProduction_Insert" : "Sp_InvFoodProduction_Update",
                h, ProductionInputRepository.P_HEADER);
        int id;
        if (result > 0) { h.put("Id", result); id = result; } else { id = toInt(h.get("Id")); }

        /* details - LineId 1..n, parent stamped, new Id read back (upsert on Id) */
        int line = 0;
        boolean loaderRow = false;
        for (Map<String, Object> d : details) {
            line++;
            d.put("LineId", line);
            d.put("InvFoodProductionId", id);
            int did = consumption
                    ? toInt(dal.scalar("dbo.USP_InvFoodProductionConsumptionDetail_Insert", consumptionDetailParams(d)))
                    : dal.setProc("Sp_InvFoodProductionDetail_Insert", d, ProductionInputRepository.P_DETAIL);
            d.put("Id", did);
            if (toInt(d.get("RefDocumentTypeId")) > 0 && toInt(d.get("RefDocNoId")) > 0 && toInt(d.get("RefDocSubIdNo")) > 0) loaderRow = true;
        }

        /* ERP feature 5 and no loader row -> the FIFO branch; else on insert the evaluation is rebuilt. */
        if (dal.erpFeature(u, ERP_FEATURE_FIFO_CGS) && !loaderRow) {
            fifoBranch(u, h, details, voucher, lines);
        } else if (toInt(h.get("ActionId")) == 1) {
            Map<String, Object> se = ProductionInputRepository.stockEvaluationDefaults();
            se.put("OrganizationId", org);
            se.put("CompanyId", comp);
            se.put("RefDocumentTypeId", docType);
            se.put("RefDocIdNo", id);
            dal.setProc("Sp_InventoryStockEvalautionDetail_Update", se, ProductionInputRepository.P_STOCK_EVALUATION_UPDATE);
        }

        /* inventory transactions - the procedure rebuilds them from the saved rows */
        Map<String, Object> it = ProductionInputRepository.inventoryTransactionsDefaults();
        it.put("OrganizationId", org);
        it.put("CompanyId", comp);
        it.put("RefDocumentTypeId", docType);
        it.put("RefDocIdNo", id);
        dal.setProc("Sp_InventoryTransactions_GetALLMethod", it, ProductionInputRepository.P_INV_TRANSACTIONS);

        /* removed rows (InputDetailRowsRemoveIds / OutputDetailRowsRemoveIds) */
        if (removeIds != null && !removeIds.isEmpty()) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", docType);
            p.put("@Id", id);
            p.put("@DetailIds", removeIds);
            dal.command("dbo.USP_InvFoodProductionDetailRowsDeleteByIds", p);
        }

        /* USP_InventoryValidation per detail (RAISERROR guard) */
        for (Map<String, Object> d : details) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", docType);
            p.put("@DocDate", h.get("DocDate"));
            p.put("@ItemId", d.get("ItemId"));
            p.put("@WarehouseId", d.get("WarehouseId"));
            p.put("@JobLotId", d.get("JobLotId"));
            p.put("@CropYear", d.get("CropBatch"));
            p.put("@InvPackingTypeId", d.get("PackingtypeId"));
            p.put("@PackUomId", d.get("ItemUomId"));
            p.put("@RefDocumentTypeId", d.get("RefDocumentTypeId"));
            p.put("@RefDocNoId", d.get("RefDocNoId"));
            p.put("@RefDocSubIdNo", d.get("RefDocSubIdNo"));
            p.put("@NetWeight", d.get("Weight"));
            dal.command("dbo.USP_InventoryValidation", p);
        }

        /* update -> input/output weight validation */
        if (toInt(h.get("ActionId")) == 2) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", docType);
            p.put("@InvProductionJobOrderId", h.get("InvJobOrderId"));
            p.put("@PlantId", h.get("PlantId"));
            dal.command("dbo.USP_ProductionInPutAndOutPutWeightValidation", p);
        }

        /* 80 -> per detail the job-order weight comparison (@RefId = JobOrderInPutId) */
        if (!consumption) {
            for (Map<String, Object> d : details) {
                LinkedHashMap<String, Object> p = ProductionInputRepository.params();
                p.put("@DocumentTypeId", docType);
                p.put("@JobOrderId", h.get("InvJobOrderId"));
                p.put("@RefId", d.get("JobOrderInPutId"));
                p.put("@NetWeight", d.get("Weight"));
                dal.command("dbo.usp_ProductionWeightCompareFromJobOrderWeight", p);
            }
        }

        /* voucher - unless the header's RefDocumentTypeId is 1 */
        if (headerRefDocumentTypeId != 1 && voucher != null) {
            int existing = dal.voucherHeadId(u, docType, id);
            if (existing > 0) voucher.put("Id", existing);
            voucher.put("DocumentTypeSrNo", id);
            int vr = dal.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update",
                    voucher, ProductionInputRepository.P_VOUCHER_HEAD);
            if (vr > 0) voucher.put("Id", vr);
            int voucherId = toInt(voucher.get("Id"));
            if (!lines.isEmpty()) {
                for (Map<String, Object> l : lines) {
                    l.put("VoucherHeadId", voucherId);
                    l.put("BranchesId", h.get("BranchId"));
                    int lid = toInt(l.get("LineId"));
                    /* <SetData>b__6 searches invFoodProductionDetails - for 181 that list is empty. */
                    if (!consumption) {
                        for (Map<String, Object> d : details) {
                            if (toInt(d.get("LineId")) == lid && lid > 0) { l.put("RefDocSubIdNo", d.get("Id")); break; }
                        }
                    }
                    dal.setProc("Sp_VoucherDetail_Insert", l, ProductionInputRepository.P_VOUCHER_DETAIL);
                }
                if (voucherId > 0) {
                    LinkedHashMap<String, Object> p = ProductionInputRepository.params();
                    p.put("@OrganizationId", org);
                    p.put("@CompanyId", comp);
                    p.put("@Id", voucherId);
                    dal.command("dbo.USP_VoucherBalanceCheck", p);
                }
            }
        }
        return id;
    }

    /** USP_InvFoodProductionConsumptionDetail_Insert - the 21 declared parameters, from the model. */
    private static LinkedHashMap<String, Object> consumptionDetailParams(Map<String, Object> d) {
        LinkedHashMap<String, Object> p = ProductionInputRepository.params();
        p.put("@Amount", toDouble(d.get("Amount")));
        p.put("@Qty", toDouble(d.get("Qty")));
        p.put("@Rate", toDouble(d.get("Rate")));
        p.put("@Weight", toDouble(d.get("Weight")));
        p.put("@Id", toInt(d.get("Id")));
        p.put("@InvFoodProductionId", toInt(d.get("InvFoodProductionId")));
        p.put("@InvProductionJobOrderId", toInt(d.get("InvProductionJobOrderId")));
        p.put("@InvProductionJobOrderNo", 0);
        p.put("@ItemId", toInt(d.get("ItemId")));
        p.put("@ItemUomId", toInt(d.get("ItemUomId")));
        p.put("@JobLotId", toInt(d.get("JobLotId")));
        p.put("@PackingtypeId", toInt(d.get("PackingtypeId")));
        p.put("@RateUOMId", toInt(d.get("RateUOMId")));
        p.put("@RefDocumentTypeId", toInt(d.get("RefDocumentTypeId")));
        p.put("@RefDocNoId", toInt(d.get("RefDocNoId")));
        p.put("@RefDocSubIdNo", toInt(d.get("RefDocSubIdNo")));
        p.put("@WarehouseId", toInt(d.get("WarehouseId")));
        p.put("@LineId", toInt(d.get("LineId")));
        p.put("@CropBatch", str(d.get("CropBatch")));
        p.put("@EntryType", str(d.get("EntryType")));
        p.put("@Remarks", str(d.get("Remarks")));
        return p;
    }

    // ------------------------------------------------------------ DAL SetData - FIFO branch

    /**
     * DAL SetData, FIFO branch (ERP feature 5 on, no loader row): per detail (grid order) the item's GL
     * row (none -> skipped), the job lot's GL row, CommonServices.FIFOImplemention with the layers
     * already taken as reservations; a job lot without an account appends two CGS voucher lines per
     * layer (NullReference when there is no voucher, i.e. financial effects off). Then on update the
     * old stock-out rows are reversed and deleted, and every layer is inserted as an evaluation row.
     */
    private void fifoBranch(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details,
                            Map<String, Object> voucher, List<Map<String, Object>> lines) {
        int headerId = toInt(h.get("Id"));
        int modifyUser = toInt(h.get("ModifyUser"));
        LocalDateTime docDate = (LocalDateTime) h.get("DocDate");
        List<Map<String, Object>> itemGl = dal.itemGlAccounts(u);
        List<Map<String, Object>> jobLotGl = dal.jobLotGlIdsAndName(u);
        List<Map<String, Object>> evaluations = new ArrayList<>();

        for (Map<String, Object> item : new ArrayList<>(details)) {
            Map<String, Object> gl = firstById(itemGl, toInt(item.get("ItemId")));
            if (gl == null) continue;
            int rpDocType = 0, rpId = 0;
            if (modifyUser > 0) { rpDocType = toInt(h.get("DocumentTypeId")); rpId = headerId; }
            Map<String, Object> jl = firstById(jobLotGl, toInt(item.get("JobLotId")));
            boolean jobLotHasAccount = jl != null && toInt(col(jl, "AccountId")) > 0;
            for (Map<String, Object> se : fifoImplementation(u, item, docDate, rpDocType, rpId, evaluations)) {
                if (!jobLotHasAccount) {
                    String comments = "ItemQty: " + net(toDouble(se.get("QtyOut"))) + "   " + str(col(gl, "ItemName"))
                            + "  Net Weight: " + net(toDouble(se.get("StockWeightOut")))
                            + "  CGS Rate:" + net(toDouble(se.get("CgsRate")));
                    if (voucher == null) throw new FormMessage("Object reference not set to an instance of an object.");
                    lines.add(fifoVoucherLine(h, item, gl, se, comments, true));
                    lines.add(fifoVoucherLine(h, item, gl, se, comments, false));
                }
                evaluations.add(se);
            }
        }
        if (evaluations.isEmpty()) return;
        int org = toInt(h.get("OrganizationId")), comp = toInt(h.get("CompanyId"));
        if (modifyUser > 0) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@RefDocumentTypeId", h.get("DocumentTypeId"));
            p.put("@RefDocIdNo", headerId);
            dal.command("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", p);
        }
        for (Map<String, Object> se : evaluations) {
            se.put("OrganizationId", org);
            se.put("CompanyId", comp);
            se.put("DocDate", docDate);
            se.put("DocCodeNo", toInt(h.get("DocCode")));
            se.put("SupplierCustomerId", 0);
            se.put("BranchesId", h.get("BranchId"));
            se.put("RefDocumentTypeId", h.get("DocumentTypeId"));
            se.put("EntryUser", h.get("EntryUser"));
            se.put("ModifyUser", h.get("ModifyUser"));
            se.put("CalcType", "Weight");
            Map<String, Object> d = null;
            for (Map<String, Object> x : details) if (toInt(x.get("LineId")) == toInt(se.get("LineId"))) { d = x; break; }
            if (d != null) {
                double eq = dal.equivalentByItemAndSchedule(u, toInt(se.get("ItemId")), toInt(d.get("RateUOMId")));
                double rate = toDouble(d.get("Rate"));
                if (toDouble(se.get("BillWeightOut")) > 0d && rate > 0d && eq > 0d) {
                    se.put("AmountOut", toDouble(se.get("BillWeightOut")) / eq * rate);
                }
                se.put("ItemRate", rate);
                se.put("RefDocIdNo", d.get("InvFoodProductionId"));
                se.put("RefDocSubIdNo", d.get("Id"));
            }
            dal.setProc("USP_InventoryStockEvalautionDetail_Insert", se, ProductionInputRepository.P_STOCK_EVALUATION_INSERT);
        }
    }

    private static Map<String, Object> fifoVoucherLine(Map<String, Object> h, Map<String, Object> item, Map<String, Object> gl,
                                                       Map<String, Object> se, String comments, boolean debit) {
        Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
        int wip = toInt(h.get("WIPAccountId"));
        int purchaseGl = toInt(col(gl, "PurchaseGLAC"));
        double cgsAmount = toDouble(se.get("CgsAmount"));
        v.put("LineId", toInt(item.get("LineId")));
        v.put("AccountId", debit ? wip : purchaseGl);
        v.put("AgainstAccountId", debit ? purchaseGl : wip);
        v.put("Comments", comments);
        v.put(debit ? "DebitAmount" : "CreditAmount", cgsAmount);
        v.put("DocumentTypeIdRef", toInt(item.get("RefDocumentTypeId")));
        v.put("InvoiceNoRefId", toInt(item.get("RefDocNoId")));
        v.put("RefInvoiceNo", String.valueOf(toInt(item.get("RefDocSubIdNo"))));
        v.put("ItemId", toInt(item.get("ItemId")));
        v.put("QtyOut", toDouble(se.get("QtyOut")));
        v.put("WeightOut", toDouble(se.get("StockWeightOut")));
        v.put("ItemCgsRate", toDouble(se.get("CgsRate")));
        v.put("RateCut", 0d);
        v.put("RateCutAmount", 0d);
        v.put("ItemAmount", cgsAmount);
        v.put("JobLotId", toInt(item.get("JobLotId")));
        v.put("BranchesId", h.get("BranchId"));
        v.put("OrderNo", toInt(item.get("InvProductionJobOrderId")));
        return v;
    }

    /** DAL CommonServices.FIFOImplemention for one detail (ItemName never set -> empty in every message). */
    private List<Map<String, Object>> fifoImplementation(UserAccount u, Map<String, Object> item, LocalDateTime docDate,
                                                         int docType, int id, List<Map<String, Object>> reserved) {
        String itemName = "";
        String xml = reserved.isEmpty() ? null : fifoXml(reserved);
        List<Map<String, Object>> rows = dal.stockByFifoForSave(u, toInt(item.get("ItemId")), toInt(item.get("ItemUomId")),
                docDate, toInt(item.get("WarehouseId")), toInt(item.get("JobLotId")), toInt(item.get("PackingtypeId")),
                str(item.get("CropBatch")), docType, id, xml);
        if (rows.isEmpty()) throw new FormMessage("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0d;
        for (Map<String, Object> r : rows) available += toDouble(col(r, "NetBalWeight"));
        double itemQty = toDouble(item.get("Qty"));
        double netWeight = toDouble(item.get("Weight"));
        if (!(netWeight <= Math.rint(available * 100d) / 100d)) {
            throw new FormMessage("Weight available is " + net(available) + " and row Weight is " + net(netWeight)
                    + " this item " + itemName + " against FIFO....");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        double l12 = netWeight, l13 = itemQty, l15 = 0d, l17 = 0d;
        for (Map<String, Object> r : rows) {
            double avgRate = toDouble(col(r, "AvgRate"));
            if (!(avgRate > 0d)) throw new FormMessage("Rate Not Found this Item " + itemName + " against FIFO Method");
            int rateUomId = toInt(col(r, "RateUomId"));
            if (rateUomId == 0) throw new FormMessage("RateUomId not found  this " + itemName + " against FIFO Method");
            double l16 = toDouble(col(r, "NetBalWeight"));
            double l14 = toDouble(col(r, "NetBalQty"));
            double eq = dal.equivalentByItemAndSchedule(u, toInt(item.get("ItemId")), rateUomId);
            if (eq == 0d) throw new FormMessage("RateUom Not Found");
            Map<String, Object> se = ProductionInputRepository.stockEvaluationDefaults();
            if (!(l16 > l12 - l17)) {
                l17 += l16;
                l15 += l14;
                fillEvaluation(se, r, item, rateUomId);
                se.put("QtyOut", l14);
                se.put("BillWeightOut", l16);
                se.put("StockWeightOut", l16);
                se.put("CgsRate", avgRate * eq);
                se.put("CgsAmount", l16 / eq * (avgRate * eq));
                out.add(se);
            } else if (!(l16 < l12 - l17)) {
                fillEvaluation(se, r, item, rateUomId);
                double q = l13 - l15, w = l12 - l17;
                se.put("QtyOut", q);
                se.put("BillWeightOut", w);
                se.put("StockWeightOut", w);
                se.put("CgsRate", avgRate * eq);
                se.put("CgsAmount", w / eq * (avgRate * eq));
                l15 += q;
                l17 += w;
                out.add(se);
            }
            if (netWeight == l17) break;
        }
        return out;
    }

    private static void fillEvaluation(Map<String, Object> se, Map<String, Object> r, Map<String, Object> item, int rateUomId) {
        se.put("Id", toInt(col(r, "Id")));
        se.put("LineId", toInt(item.get("LineId")));
        se.put("ItemId", toInt(item.get("ItemId")));
        se.put("WarehouseId", toInt(item.get("WarehouseId")));
        se.put("RateUom", rateUomId);
        se.put("JobLotId", toInt(item.get("JobLotId")));
        se.put("InvPackingTypeId", toInt(item.get("PackingtypeId")));
        se.put("ItemUom", toInt(item.get("ItemUomId")));
        se.put("CropBatch", str(item.get("CropBatch")));
        se.put("RefRefDocumentTypeId", toInt(col(r, "RefDocumentTypeId")));
        se.put("RefRefDocIdNo", toInt(col(r, "RefDocIdNo")));
        se.put("RefRefDocSubIdNo", toInt(col(r, "RefDocSubIdNo")));
    }

    private static String fifoXml(List<Map<String, Object>> reserved) {
        StringBuilder sb = new StringBuilder("<ArrayOfFIFOStockEvaluation xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                + "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
        for (Map<String, Object> r : reserved) {
            sb.append("<FIFOStockEvaluation>")
              .append("<RefDocumentTypeId>").append(toInt(r.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId>")
              .append("<RefDocIdNo>").append(toInt(r.get("RefRefDocIdNo"))).append("</RefDocIdNo>")
              .append("<RefDocSubIdNo>").append(toInt(r.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo>")
              .append("<ReserveQty>").append(xmlDouble(toDouble(r.get("QtyOut")))).append("</ReserveQty>")
              .append("<ReserveWeight>").append(xmlDouble(toDouble(r.get("StockWeightOut")))).append("</ReserveWeight>")
              .append("</FIFOStockEvaluation>");
        }
        return sb.append("</ArrayOfFIFOStockEvaluation>").toString();
    }

    // ================================================================ InvFoodProduction.Save (112)

    /**
     * BLL InvFoodProduction.Save for DocumentTypeId 112 (Output): MakeVoucher's 112 branch then the DAL
     * through ProductionOutputRepository.save (one transaction). Maps carry "@Parameter" keys.
     */
    public int saveOutput(Map<String, Object> h, List<Map<String, Object>> details, String removeIds) {
        int org = toInt(h.get("@OrganizationId")), comp = toInt(h.get("@CompanyId"));
        boolean withoutRates = bool(repo.configValue(org, comp, "JobOrderCreatewithoutRates"));
        boolean financialsInactive = bool(repo.configValue(org, comp, "InventoryFinancialsEffectsInActive"));
        boolean refDocTypeOne = financialsInactive || withoutRates;

        Map<String, Object> voucher = null;
        List<Map<String, Object>> lines = null;
        if (!refDocTypeOne) {
            lines = new ArrayList<>();
            voucher = makeOutputVoucher(org, comp, h, details, lines);
        }
        boolean insert = toInt(h.get("@Id")) == 0;
        h.put("@ActionId", insert ? 1 : 2);
        if (insert) h.put("@ModifyUser", 0); else h.put("@EntryUser", 0);
        return outputDal.save(h, details, voucher, lines, withoutRates, removeIds == null || removeIds.isEmpty() ? null : removeIds);
    }

    /** BLL MakeVoucher, the 112 branch - reproduced as written (credit line carries QtyIn/WeightIn, total of credit DebitAmount). */
    private Map<String, Object> makeOutputVoucher(int org, int comp, Map<String, Object> h, List<Map<String, Object>> details,
                                                  List<Map<String, Object>> lines) {
        List<Map<String, Object>> items = repo.itemGlAccounts(org, comp);
        Map<String, Object> v = ProductionOutputRepository.voucherHeadDefaults();
        v.put("@DocumentTypeSrNo", toInt(h.get("@Id")));
        v.put("@VoucherDate", h.get("@DocDate"));
        v.put("@VoucherCode", h.get("@DocCode"));
        v.put("@DocumentTypeId", Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_OUTPUT);
        v.put("@BaseDocumentTypeId", h.get("@BaseDocumentTypeId"));
        v.put("@RefAccountId", h.get("@WIPAccountId"));
        v.put("@AgainstAccountId", h.get("@WIPItemId"));
        v.put("@EntryDate", LocalDateTime.now());
        v.put("@ModifyDate", LocalDateTime.now());
        v.put("@EntryUser", h.get("@EntryUser"));
        v.put("@ModifyUser", h.get("@ModifyUser"));
        v.put("@OrganizationId", org);
        v.put("@CompanyId", comp);
        v.put("@FinancialYearId", h.get("@FinancialYearId"));
        v.put("@BranchId", h.get("@BranchId"));
        double total = 0d;
        int ln = 1;
        String jobNo = str(h.get("@InvJobOrderNo"));
        int wip = toInt(h.get("@WIPAccountId"));
        for (Map<String, Object> det : details) {
            if (items.isEmpty()) throw new FormMessage("Item GL Account Not Found");
            Map<String, Object> gl = firstById(items, toInt(det.get("@ItemId")));
            if (gl == null) continue;
            String remarks = str(det.get("@Remarks"));
            double qty = toDouble(det.get("@Qty")), weight = toDouble(det.get("@Weight")), rate = toDouble(det.get("@Rate"));
            double amount = toDouble(det.get("@Amount"));
            String comments = jobNo + " " + (remarks.trim().isEmpty() ? "" : remarks + " ") + " ItemName: "
                    + str(col(gl, "ItemName")) + " Qty " + net(qty) + " Weight " + net(weight) + " Rate " + net(rate);
            int stockAc = toInt(det.get("@StockAcId")) > 0 ? toInt(det.get("@StockAcId")) : toInt(col(gl, "PurchaseGLAC"));

            Map<String, Object> a = ProductionOutputRepository.voucherDetailDefaults();
            a.put("@AccountId", stockAc);
            a.put("@LineId", ln);
            a.put("@AgainstAccountId", wip);
            a.put("@Comments", comments);
            a.put("@DebitAmount", amount);
            a.put("@QtyIn", qty);
            a.put("@WeightIn", weight);
            a.put("@ItemAmount", amount);
            a.put("@ItemId", toInt(det.get("@ItemId")));
            a.put("@OrderNo", toInt(det.get("@InvProductionJobOrderId")));
            a.put("@BranchesId", h.get("@BranchId"));
            lines.add(a);

            Map<String, Object> c = ProductionOutputRepository.voucherDetailDefaults();
            c.put("@AccountId", wip);
            c.put("@AgainstAccountId", stockAc);
            c.put("@LineId", ln);
            c.put("@Comments", comments);
            c.put("@CreditAmount", amount);
            c.put("@QtyIn", qty);
            c.put("@WeightIn", weight);
            c.put("@ItemAmount", amount);
            c.put("@OrderNo", toInt(det.get("@InvProductionJobOrderId")));
            c.put("@BranchesId", h.get("@BranchId"));
            total += toDouble(c.get("@DebitAmount"));
            lines.add(c);
            ln++;
        }
        v.put("@VoucherAmount", total);
        return v;
    }

    // ======================================================= InvFoodProductionPackingMaterial.Save (111)

    /**
     * BLL InvFoodProductionPackingMaterial.Save + DAL SetData (PackingMaterialInsert:4702), one transaction.
     * Rows carry the 29 model properties (no "@"); JobOrderPackingMaterialId comes from the loader here
     * (the 280 form never sets it). PmDetailRowsRemoveIds -> USP_InvFoodProductionPmRowsDeleteByIds.
     */
    public void savePackingMaterial(List<Map<String, Object>> rows, String removeIds) {
        if (rows.isEmpty()) throw new FormMessage("Grid Record not found please check!");
        int org = toInt(rows.get(0).get("OrganizationId")), comp = toInt(rows.get(0).get("CompanyId"));
        boolean finEffectsOff = bool(repo.configValue(org, comp, "InventoryFinancialsEffectsInActive"));
        TransactionTemplate tx = new TransactionTemplate(txManager);
        tx.execute(status -> {
            Map<String, Object> voucher = at(ProductionInputRepository.voucherHeadDefaults());
            List<Map<String, Object>> voucherDetails = new ArrayList<>();
            for (Map<String, Object> pm : rows) {
                int loc0, loc1 = 0;
                boolean insert = toInt(pm.get("Id")) == 0;
                loc0 = toInt(repo.scalar(insert ? "dbo.Sp_InvFoodProductionPackingMaterial_Insert" : "dbo.Sp_InvFoodProductionPackingMaterial_Update", at(pm)));
                if (loc0 > 0) { pm.put("ModifyUser", 0); pm.put("Id", loc0); }
                else { pm.put("EntryUser", 0); loc0 = toInt(pm.get("Id")); }
                int docType = toInt(pm.get("DocumentTypeId"));
                List<Map<String, Object>> itemGl = repo.itemGlAccounts(org, comp);
                List<Map<String, Object>> wipRows = repo.glAccountsByJobOrderId(org, comp, toInt(pm.get("InvProductionJobOrderId")));
                if (wipRows.isEmpty()) throw new FormMessage("Work in process Account not found against JobOrder");
                int wipAccountId = toInt(col(wipRows.get(0), "WorkInProccessAcId"));

                Map<String, Object> ev = ProductionInputRepository.stockEvaluationDefaults();
                ev.put("OrganizationId", org); ev.put("CompanyId", comp);
                ev.put("RefDocumentTypeId", docType); ev.put("RefDocIdNo", loc0);
                dal.setProc("Sp_InventoryStockEvalautionDetail_Update", ev, ProductionInputRepository.P_STOCK_EVALUATION_UPDATE);

                Map<String, Object> tr = ProductionInputRepository.inventoryTransactionsDefaults();
                tr.put("OrganizationId", org); tr.put("CompanyId", comp);
                tr.put("RefDocumentTypeId", docType); tr.put("RefDocIdNo", loc0);
                dal.setProc("Sp_InventoryTransactions_GetALLMethod", tr, ProductionInputRepository.P_INV_TRANSACTIONS);

                for (Map<String, Object> v : rows) {
                    LinkedHashMap<String, Object> vp = ProductionInputRepository.params();
                    vp.put("@OrganizationId", org); vp.put("@CompanyId", comp);
                    vp.put("@DocumentTypeId", toInt(v.get("DocumentTypeId")));
                    vp.put("@DocDate", v.get("DocDate"));
                    vp.put("@ItemId", toInt(v.get("ItemId")));
                    vp.put("@WarehouseId", toInt(v.get("WarehouseId")));
                    vp.put("@NetWeight", toDouble(v.get("Qty")));
                    vp.put("@ItemConditionId", toInt(v.get("ItemConditionId")));
                    vp.put("@RackId", toInt(v.get("RackId")));
                    vp.put("@Activity", "PM");
                    dal.command("dbo.USP_InventoryValidation", vp);
                }
                if (loc0 <= 0 || finEffectsOff) continue;
                if (toInt(pm.get("ModifyUser")) > 0) {
                    loc1 = repo.voucherHeadId(org, comp, docType, toInt(pm.get("Id")));
                    voucher.put("@Id", loc1);
                }
                voucher.put("@DocumentTypeSrNo", loc0);
                voucher.put("@VoucherDate", pm.get("DocDate"));
                voucher.put("@VoucherCode", toInt(pm.get("DocNo")));
                voucher.put("@DocumentTypeId", docType);
                voucher.put("@BaseDocumentTypeId", toInt(pm.get("BaseDocumentTypeId")));
                voucher.put("@RefAccountId", wipAccountId);
                voucher.put("@AgainstAccountId", wipAccountId);
                voucher.put("@EntryDate", LocalDateTime.now());
                voucher.put("@ModifyDate", LocalDateTime.now());
                voucher.put("@VoucherAmount", toDouble(pm.get("Amount")));
                voucher.put("@EntryUser", toInt(pm.get("EntryUser")));
                voucher.put("@ModifyUser", toInt(pm.get("ModifyUser")));
                voucher.put("@OrganizationId", org);
                voucher.put("@CompanyId", comp);
                voucher.put("@FinancialYearId", toInt(pm.get("FinancialYearId")));
                voucher.put("@BranchId", toInt(pm.get("BranchesId")));
                if (itemGl.isEmpty()) throw new FormMessage("Item list Not Found");
                Map<String, Object> gl = firstById(itemGl, toInt(pm.get("ItemId")));
                if (gl == null) throw new FormMessage("Item GlAccountId Not Found");
                int purchaseGlac = toInt(col(gl, "PurchaseGLAC"));
                double amount = toDouble(pm.get("Amount"));
                String remarks = str(pm.get("pmRemarks"));
                Map<String, Object> d1 = at(ProductionInputRepository.voucherDetailDefaults());
                d1.put("@AccountId", wipAccountId); d1.put("@AgainstAccountId", purchaseGlac);
                d1.put("@Comments", remarks); d1.put("@DebitAmount", amount);
                d1.put("@ItemId", toInt(pm.get("ItemId"))); d1.put("@ItemAmount", amount);
                d1.put("@OrderNo", toInt(pm.get("InvProductionJobOrderId"))); d1.put("@BranchesId", toInt(pm.get("BranchesId")));
                voucherDetails.add(d1);
                Map<String, Object> d2 = at(ProductionInputRepository.voucherDetailDefaults());
                d2.put("@AccountId", purchaseGlac); d2.put("@AgainstAccountId", wipAccountId);
                d2.put("@Comments", remarks); d2.put("@CreditAmount", amount);
                d2.put("@ItemId", toInt(pm.get("ItemId"))); d2.put("@ItemAmount", amount);
                d2.put("@QtyOut", toDouble(pm.get("Qty"))); d2.put("@WeightOut", toDouble(pm.get("Qty")));
                d2.put("@OrderNo", toInt(pm.get("InvProductionJobOrderId"))); d2.put("@BranchesId", toInt(pm.get("BranchesId")));
                voucherDetails.add(d2);
                int loc2 = toInt(repo.scalar(loc1 == 0 ? "dbo.Sp_VoucherHead_Insert" : "dbo.Sp_VoucherHead_Update", voucher));
                if (loc2 > 0) voucher.put("@Id", loc2);
                int headId = toInt(voucher.get("@Id"));
                for (Map<String, Object> d : voucherDetails) {
                    d.put("@VoucherHeadId", headId);
                    repo.scalar("dbo.Sp_VoucherDetail_Insert", d);
                }
                if (headId > 0) {
                    LinkedHashMap<String, Object> bp = ProductionInputRepository.params();
                    bp.put("@OrganizationId", org); bp.put("@CompanyId", comp); bp.put("@Id", headId);
                    dal.command("dbo.USP_VoucherBalanceCheck", bp);
                }
            }
            if (removeIds != null && !removeIds.isEmpty()) {
                repo.deleteRowsByIds("dbo.USP_InvFoodProductionPmRowsDeleteByIds", org, comp,
                        Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_PACKING_MATERIAL, removeIds);
            }
            return null;
        });
    }

    // ============================================================ InvFoodProductionOverHeads.Save (110)

    /**
     * BLL InvFoodProductionOverHeads.Save + DAL SetData (OverHeadInsert:5436), one transaction: per row
     * Insert/Update, then (financial effects on) the voucher Dr WIP / Cr overhead account with
     * Comments = ohRemarks, and USP_VoucherBalanceCheck. OHDetailRowsRemoveIds -> USP_InvFoodProductionOHRowsDeleteByIds.
     */
    public void saveOverheads(List<Map<String, Object>> rows, String removeIds) {
        if (rows.isEmpty()) throw new FormMessage("Grid Record not found please check!");
        int org = toInt(rows.get(0).get("OrganizationId")), comp = toInt(rows.get(0).get("CompanyId"));
        boolean financialsInactive = bool(repo.configValue(org, comp, "InventoryFinancialsEffectsInActive"));
        TransactionTemplate tx = new TransactionTemplate(txManager);
        tx.execute(status -> {
            for (Map<String, Object> row : rows) {
                boolean insert = toInt(row.get("Id")) == 0;
                int loc0 = overheadDal.saveOverheadRow(insert, at(row));
                if (loc0 > 0) { row.put("ModifyUser", 0); row.put("Id", loc0); }
                else { row.put("EntryUser", 0); loc0 = toInt(row.get("Id")); }
                if (financialsInactive) continue;
                Map<String, Object> vh = ProductionOverheadRepository.voucherHeadDefaults();
                int loc1 = 0;
                if (toInt(row.get("ModifyUser")) > 0) {
                    loc1 = overheadDal.voucherHeadIdInTransaction(org, comp, toInt(row.get("DocumentTypeId")), toInt(row.get("Id")));
                    vh.put("Id", loc1);
                }
                int jobOrderId = toInt(row.get("InvProductionJobOrderId"));
                List<Map<String, Object>> gl = overheadDal.glAccountsByJobOrderId(org, comp, jobOrderId);
                if (gl.isEmpty()) throw new FormMessage("JobOrder WIPAccount Not Found");
                int wip = toInt(col(gl.get(0), "WorkInProccessAcId"));
                int coa = toInt(row.get("CharOfAccountId"));
                double amount = toDouble(row.get("Amount"));
                String remarks = str(row.get("ohRemarks"));
                vh.put("DocumentTypeSrNo", loc0);
                vh.put("VoucherDate", row.get("DocDate"));
                vh.put("VoucherCode", toInt(row.get("DocNo")));
                vh.put("DocumentTypeId", toInt(row.get("DocumentTypeId")));
                vh.put("BaseDocumentTypeId", toInt(row.get("BaseDocumentTypeId")));
                vh.put("RefAccountId", coa);
                vh.put("AgainstAccountId", wip);
                Timestamp now = Timestamp.valueOf(LocalDateTime.now());
                vh.put("EntryDate", now);
                vh.put("ModifyDate", now);
                vh.put("VoucherAmount", amount);
                vh.put("EntryUser", toInt(row.get("EntryUser")));
                vh.put("ModifyUser", toInt(row.get("ModifyUser")));
                vh.put("OrganizationId", org);
                vh.put("CompanyId", comp);
                vh.put("FinancialYearId", toInt(row.get("FinancialYearId")));
                vh.put("BranchId", toInt(row.get("BranchesId")));
                List<Map<String, Object>> lines = new ArrayList<>();
                Map<String, Object> d1 = ProductionOverheadRepository.voucherDetailDefaults();
                d1.put("AccountId", wip); d1.put("AgainstAccountId", coa); d1.put("Comments", remarks);
                d1.put("DebitAmount", amount); d1.put("OrderNo", jobOrderId);
                lines.add(d1);
                Map<String, Object> d2 = ProductionOverheadRepository.voucherDetailDefaults();
                d2.put("AccountId", coa); d2.put("AgainstAccountId", wip); d2.put("Comments", remarks);
                d2.put("CreditAmount", amount); d2.put("OrderNo", jobOrderId);
                lines.add(d2);
                int loc2 = overheadDal.saveVoucherHead(loc1 == 0, at(vh));
                if (loc2 > 0) vh.put("Id", loc2);
                int headId = toInt(vh.get("Id"));
                for (Map<String, Object> d : lines) {
                    d.put("VoucherHeadId", headId);
                    overheadDal.insertVoucherDetail(at(d));
                }
                if (headId > 0) overheadDal.voucherBalanceCheck(org, comp, headId);
            }
            if (removeIds != null && !removeIds.isEmpty()) {
                repo.deleteRowsByIds("dbo.USP_InvFoodProductionOHRowsDeleteByIds", org, comp,
                        Prod871ProductionAgainstPreCostingJobOrderRepository.DOC_OVERHEAD, removeIds);
            }
            return null;
        });
    }

    // ============================================================================== settlement

    /** InvFoodProduction.UpdateSettlement (UpdateSettlement:8220) - DAL: UpdateIsSettled, usp_SettlementDeleteByJobOrderId, SP_InvFoodProductionDetail_Update per row. */
    public int updateSettlement(int org, int comp, int jobOrderId, List<LinkedHashMap<String, Object>> details) {
        return settlementDal.updateSettlement(org, comp, jobOrderId, details);
    }

    /** InvFoodProduction.SetDataForSettlementFinancials (SettlementFinancials:8320). */
    public int settlementFinancials(LinkedHashMap<String, Object> head, List<LinkedHashMap<String, Object>> lines) {
        return settlementDal.settlementFinancials(head, lines);
    }

    /** InvProductionJobOrder.JobOrderApproveList with the one job order (btnApproveSettlement_Click:8568). */
    public void approveJobOrder(int org, int comp, int jobOrderId, int userId) {
        settlementDal.jobOrderApprove(org, comp, jobOrderId, userId);
    }

    // ================================================================================= helpers

    /** Model property -> "@Property" (GenericProvider.SetProc), dates as SQL types. */
    public static Map<String, Object> at(Map<String, Object> model) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : model.entrySet()) {
            Object v = e.getValue();
            if (v instanceof LocalDate) v = java.sql.Date.valueOf((LocalDate) v);
            if (v instanceof LocalDateTime) v = Timestamp.valueOf((LocalDateTime) v);
            p.put(e.getKey().startsWith("@") ? e.getKey() : "@" + e.getKey(), v);
        }
        return p;
    }

    private static Map<String, Object> firstById(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(col(r, "Id")) == id) return r;
        return null;
    }

    private static boolean bool(String v) {
        String s = v == null ? "" : v.trim().toLowerCase();
        return "1".equals(s) || "true".equals(s) || "yes".equals(s);
    }

    /** .NET double.ToString(): up to 15 significant digits, no trailing zeros. */
    public static String net(double d) {
        if (Double.isNaN(d)) return "NaN";
        if (Double.isInfinite(d)) return d > 0 ? "Infinity" : "-Infinity";
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return new BigDecimal(d).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }

    private static String xmlDouble(double d) {
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }
}
