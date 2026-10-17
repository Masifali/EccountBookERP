package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ProductionInputRepository;
import com.mst.repositories.ProductionOutputRepository;
import com.mst.repositories.ProductionOverheadRepository;
import com.mst.repositories.ProductionSettlementRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
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

import static com.mst.repositories.ProductionInputRepository.ci;
import static com.mst.repositories.ProductionInputRepository.toDouble;
import static com.mst.repositories.ProductionInputRepository.toInt;
import static com.mst.services.Prod284ProductionNewService.DOC_INPUT;
import static com.mst.services.Prod284ProductionNewService.DOC_OH;
import static com.mst.services.Prod284ProductionNewService.DOC_OUTPUT;
import static com.mst.services.Prod284ProductionNewService.DOC_PM;
import static com.mst.services.Prod284ProductionNewService.DOC_SETTLEMENT;
import static com.mst.services.Prod284ProductionNewService.Prod284Message;
import static com.mst.services.Prod284ProductionNewService.docDateTime;
import static com.mst.services.Prod284ProductionNewService.str;

/**
 * Screen 284 "ProductionNew Against (Pre Costing Job Order)" - the WRITE side of
 * FoodProductionWithValuesNew.cs: InsertInput:1691 (DocumentTypeId 80), OutPutInsert:2891 (112),
 * PackingMaterialInsert:3531 (111), OverHeadInsert:4275 (110), SettlementFinancials:6577 (142) and
 * btnApproveSettlement_Click:6796.
 *
 * The form fills the same BLL models as the screen 280 forms and calls the same BLL Save methods
 * (InvFoodProduction.Save, InvFoodProductionPackingMaterial.Save, InvFoodProductionOverHeads.Save,
 * InvFoodProduction.SetDataForSettlementFinancials). The BLL/DAL flows are reproduced here exactly as
 * the 280 port established them from the disassembly, with THIS form's own field values: no WIP
 * warehouse, no move order, no gross weight / EB units, JobOrderInPutId / OutPutIdFromJobOrder /
 * JobOrderPackingMaterialId / JobOrderOverHeadId taken from the loaded job-order rows, and the
 * validations and MessageBox texts of this form.
 */
@Service
public class Prod284ProductionNewPostingService {

    private static final int ERP_FEATURE_FIFO_CGS = 5;

    @Autowired private ProductionInputRepository inRepo;
    @Autowired private ProductionOutputRepository outRepo;
    @Autowired private ProductionOverheadRepository ohRepo;
    @Autowired private ProductionSettlementRepository stRepo;
    @Autowired private Prod284ProductionNewService reads;
    @Autowired private CurrentUserContext ctx;
    @Autowired private PlatformTransactionManager transactionManager;

    private UserAccount user() { return ctx.requireAccountingUser(); }
    private int fy() { return ctx.currentFinancialYearId(); }
    private static int branchId(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    /** BLL InvFoodProduction.Save - CommonServices.GetConfigurationFromAllocation(name) -> Conversion.ToBool. */
    private boolean flag(UserAccount u, String name) {
        String v = inRepo.config(u, name);
        return Prod284ProductionNewService.asBool(v);
    }

    private boolean fifo(UserAccount u) { return inRepo.erpFeature(u, ERP_FEATURE_FIFO_CGS); }

    private void requireRight(int recId, Map<String, Object> rights) {
        boolean ok = recId > 0 ? Boolean.TRUE.equals(rights.get("update")) : Boolean.TRUE.equals(rights.get("save"));
        if (!ok) throw new IllegalStateException(recId > 0 ? "Update is not allowed for this user." : "Save is not allowed for this user.");
    }

    // ===================================================================================== INPUT (80)

    /**
     * InsertInput:1691. Body: { id, docNumber, docDate, remarks, jobOrderId, jobOrderText, plantId,
     * wipAccountId, wipItemId, departmentId, rows:[grid rows as `table` holds them] }.
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> saveInput(Map<String, Object> body) {
        UserAccount u = user();
        int recId = toInt(body.get("id"));
        Map<String, Object> rights = reads.rights();
        requireRight(recId, rights);

        /* FormValidation:1656 */
        String docNumber = str(body.get("docNumber"));
        if (docNumber.isEmpty() || "0".equals(docNumber)) throw new Prod284Message("document Number Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        if (jobOrderId == 0) throw new Prod284Message("Job Order Number Field Required");
        int plantId = toInt(body.get("plantId"));
        if (plantId == 0) throw new Prod284Message("Plant Name Field Required");
        int wipAccountId = toInt(body.get("wipAccountId"));
        if (wipAccountId == 0) throw new Prod284Message("WorkInProcess Field Required");
        int wipItemId = toInt(body.get("wipItemId"));
        if (wipItemId == 0) throw new Prod284Message("WIP Item Field Required");

        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        if (rows.isEmpty()) throw new Prod284Message("Enter Detail First ...");   // :1847

        /* :901 IssuanceByLoader */
        boolean loaderStatus = loaderStatus(u);

        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId);
        h.put("DocumentTypeId", DOC_INPUT);
        h.put("DocCode", toInt(docNumber));
        h.put("DocDate", docDateTime(str(body.get("docDate"))));
        h.put("MainRemarks", str(body.get("remarks")));
        h.put("EntryType", "Input");
        h.put("InvFoodProductionPlanId", 0);
        h.put("WIPAccountId", wipAccountId);
        h.put("WIPItemId", wipItemId);
        h.put("EBDepartmentId", toInt(body.get("departmentId")));
        h.put("InvJobOrderId", jobOrderId);
        h.put("InvJobOrderNo", str(body.get("jobOrderText")));
        h.put("PlantId", plantId);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchId", branchId(u));
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("EntryUser", u.getId());
        h.put("ModifyUser", u.getId());
        h.put("FinancialYearId", fy());
        h.put("IsApproved", false);
        h.put("ProjectId", 0);
        h.put("ContractScheduleId", 0);
        h.put("WipWareHouseId", 0);
        h.put("BaseDocumentTypeId", 0);
        h.put("StockHoldForLabApproval", false);

        /* :1738-1820 the row loop: only rows with Quantity > 0 AND Weight > 0 are taken (:1744) */
        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeight = 0.0;
        for (Map<String, Object> r : rows) {
            if (!(toDouble(r.get("Quantity")) > 0.0) || !(toDouble(r.get("Weight")) > 0.0)) continue;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("EntryType", "Issue");
            d.put("Id", toInt(r.get("Id")));
            d.put("JobOrderInPutId", toInt(r.get("JobOrderInPutId")));
            d.put("RefDocNoId", toInt(r.get("RefDocIdNo")));
            d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
            d.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
            if (loaderStatus) {
                if (toInt(d.get("RefDocumentTypeId")) == 0) throw new Prod284Message("RefDocumentTypeId not Found");
                if (toInt(d.get("RefDocNoId")) == 0) throw new Prod284Message("RefDocNoId not Found");
                if (toInt(d.get("RefDocSubIdNo")) == 0) throw new Prod284Message("RefDocSubIdNo not Found");
            }
            String wh = str(r.get("WareHouseId"));
            if (wh.isEmpty() || "0".equals(wh)) throw new Prod284Message("WareHouse Field Require in Input Detail");
            d.put("WarehouseId", toInt(r.get("WareHouseId")));
            String item = str(r.get("ItemId"));
            if (item.isEmpty() || "0".equals(item)) throw new Prod284Message("Item Field Require in Input Detail");
            d.put("ItemId", toInt(r.get("ItemId")));
            d.put("ItemUomId", toInt(r.get("ItemUOMId")));
            String crop = str(r.get("CropYear"));
            if (crop.isEmpty() || "0".equals(crop)) throw new Prod284Message("CropYear Field Require in Input Detail");
            d.put("CropBatch", crop);
            if (str(r.get("JobLotId")).isEmpty()) throw new Prod284Message("JobLot Field Require in Input Detail");
            d.put("JobLotId", toInt(r.get("JobLotId")));
            d.put("PackingtypeId", toInt(r.get("PackingTypeId")));
            /* :1799 Conversion.ToInt of the pack UOM TEXT - 0 unless the code is a whole number */
            d.put("PackUnit", toInt(r.get("ItemUOM")));
            double qty = toDouble(r.get("Quantity"));
            d.put("Qty", qty);
            if (qty == 0.0) throw new Prod284Message("ItemQty Field Required");
            double weight = toDouble(r.get("Weight"));
            d.put("Weight", weight);
            grossWeight += weight;
            if (weight == 0.0) throw new Prod284Message("ItemWeight Field Required");
            double rate = toDouble(r.get("Rate"));
            d.put("Rate", rate);
            d.put("NetRate", rate);
            d.put("InvProductionJobOrderId", jobOrderId);
            /* :1814 Conversion.ToInt(ItemAmount) - the amount is saved as a whole number */
            double amount = toInt(r.get("ItemAmount"));
            d.put("Amount", amount);
            d.put("TotalAmount", amount);
            d.put("RateUOMId", toInt(r.get("RateUOMId")));
            d.put("VoucherHeadId", 0);
            d.put("Remarks", str(r.get("Remarks")));
            /* model value-type defaults this form never sets */
            d.put("ItemPmCost", 0.0); d.put("GeneralPmCost", 0.0); d.put("ItemOhCost", 0.0); d.put("GeneralOhCost", 0.0);
            d.put("StockAcId", 0); d.put("OutPutIdFromJobOrder", 0); d.put("DeleteFlag", 0); d.put("JobOrderScheduleId", 0);
            d.put("GrossWeight", 0.0); d.put("EbUnit", 0.0); d.put("EbTotal", 0.0);
            d.put("MoveOrderDocumentTypeId", 0); d.put("MoveOrderDocId", 0);
            d.put("InvProductionJobOrderNo", 0);
            details.add(d);
        }
        if (details.isEmpty()) throw new Prod284Message("Grid Record Not Found");   // :1823

        /* ---- BLL InvFoodProduction.Save */
        boolean financialsInactive = flag(u, "InventoryFinancialsEffectsInActive");
        int headerRefDocumentTypeId = financialsInactive ? 1 : 0;
        boolean fifo = fifo(u);
        Map<String, Object> voucher = null;
        List<Map<String, Object>> voucherLines = new ArrayList<>();
        if (headerRefDocumentTypeId != 1) voucher = makeVoucher80(u, h, details, fifo, voucherLines);
        boolean insert = recId == 0;
        h.put("ActionId", insert ? 1 : 2);
        if (insert) h.put("ModifyUser", 0); else h.put("EntryUser", 0);

        final Map<String, Object> voucherF = voucher;
        final int refDocF = headerRefDocumentTypeId;
        Integer savedId = new TransactionTemplate(transactionManager)
                .execute(status -> setData80(u, h, details, refDocF, voucherF, voucherLines));
        int id = savedId == null ? 0 : savedId;

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("docCode", toInt(docNumber));
        /* :1828/:1832 - no space before the number */
        out.put("message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + toInt(docNumber));
        /* :1834 WagesStatus -> frmwagesBillHeader(RefDocTypeId 80, RefDocId, GrossWeightTotal) as a modal */
        out.put("openWages", wagesStatus(u));
        out.put("grossWeightTotal", grossWeight);
        return out;
    }

    /** :901 ContractorWagesCompulsoryBeforeInvoices / IssuanceByLoader (the single-value read of the same rows). */
    private boolean wagesStatus(UserAccount u) { return flag(u, "ContractorWagesCompulsoryBeforeInvoices"); }
    private boolean loaderStatus(UserAccount u) { return flag(u, "IssuanceByLoader"); }

    /**
     * BLL InvFoodProduction.MakeVoucher, the DocumentTypeId 80 branch: a Dr WIP / Cr purchase-GL pair
     * per detail whose item is in the GL list, when FIFO is off or the row came from a reference
     * document (RefDocumentTypeId > 0 and RefDocNoId > 0). LineId counts pairs.
     */
    private Map<String, Object> makeVoucher80(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details,
                                              boolean fifo, List<Map<String, Object>> lines) {
        List<Map<String, Object>> items = inRepo.itemGlAccounts(u);
        Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
        vh.put("DocumentTypeSrNo", h.get("Id"));
        vh.put("VoucherDate", h.get("DocDate"));
        vh.put("VoucherCode", h.get("DocCode"));
        vh.put("DocumentTypeId", h.get("DocumentTypeId"));
        vh.put("BaseDocumentTypeId", h.get("BaseDocumentTypeId"));
        vh.put("RefAccountId", h.get("WIPAccountId"));
        vh.put("AgainstAccountId", h.get("WIPItemId"));
        vh.put("EntryDate", LocalDateTime.now());
        vh.put("ModifyDate", LocalDateTime.now());
        vh.put("EntryUser", h.get("EntryUser"));
        vh.put("ModifyUser", h.get("ModifyUser"));
        vh.put("OrganizationId", h.get("OrganizationId"));
        vh.put("CompanyId", h.get("CompanyId"));
        vh.put("FinancialYearId", h.get("FinancialYearId"));
        vh.put("BranchId", h.get("BranchId"));
        double total = 0.0;
        int lineId = 1;
        int wip = toInt(h.get("WIPAccountId"));
        String jobNo = str(h.get("InvJobOrderNo"));
        for (Map<String, Object> d : details) {
            boolean eligible = !fifo || (toInt(d.get("RefDocumentTypeId")) > 0 && toInt(d.get("RefDocNoId")) > 0);
            if (!eligible) continue;
            if (items.isEmpty()) throw new Prod284Message("Item GL Account Not Found");
            Map<String, Object> item = firstById(items, toInt(d.get("ItemId")));
            if (item == null) continue;
            String remarks = str(d.get("Remarks"));
            String comment = jobNo + " " + (remarks.isEmpty() ? "" : remarks + " ") + " ItemName: "
                    + str(ci(item, "ItemName")) + " Qty " + net(toDouble(d.get("Qty"))) + " Weight "
                    + net(toDouble(d.get("Weight"))) + " Rate " + net(toDouble(d.get("Rate")));
            int purchaseGl = toInt(ci(item, "PurchaseGLAC"));
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

    /** DAL InvFoodProduction.SetData for DocumentTypeId 80, inside one transaction. */
    private int setData80(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details,
                          int headerRefDocumentTypeId, Map<String, Object> voucher, List<Map<String, Object>> lines) {
        int org = toInt(h.get("OrganizationId")), comp = toInt(h.get("CompanyId"));
        boolean insert = toInt(h.get("Id")) == 0;

        int result = inRepo.setProc(insert ? "Sp_InvFoodProduction_Insert" : "Sp_InvFoodProduction_Update", h, ProductionInputRepository.P_HEADER);
        int id;
        if (result > 0) { h.put("Id", result); id = result; } else { id = toInt(h.get("Id")); }

        int line = 0;
        for (Map<String, Object> d : details) {
            line++;
            d.put("LineId", line);
            d.put("InvFoodProductionId", id);
            int did = inRepo.setProc("Sp_InvFoodProductionDetail_Insert", d, ProductionInputRepository.P_DETAIL);
            d.put("Id", did);
        }

        /* FIFO branch when ERP feature 5 is on and no detail carries a full reference triple;
           otherwise on insert the stock evaluation is rebuilt. */
        boolean loaderRow = false;
        for (Map<String, Object> d : details) {
            if (toInt(d.get("RefDocumentTypeId")) > 0 && toInt(d.get("RefDocNoId")) > 0 && toInt(d.get("RefDocSubIdNo")) > 0) loaderRow = true;
        }
        if (inRepo.erpFeature(u, ERP_FEATURE_FIFO_CGS) && !loaderRow) {
            fifoBranch(u, h, details, voucher, lines);
        } else if (toInt(h.get("ActionId")) == 1) {
            Map<String, Object> se = ProductionInputRepository.stockEvaluationDefaults();
            se.put("OrganizationId", org);
            se.put("CompanyId", comp);
            se.put("RefDocumentTypeId", DOC_INPUT);
            se.put("RefDocIdNo", id);
            inRepo.setProc("Sp_InventoryStockEvalautionDetail_Update", se, ProductionInputRepository.P_STOCK_EVALUATION_UPDATE);
        }

        Map<String, Object> it = ProductionInputRepository.inventoryTransactionsDefaults();
        it.put("OrganizationId", org);
        it.put("CompanyId", comp);
        it.put("RefDocumentTypeId", DOC_INPUT);
        it.put("RefDocIdNo", id);
        inRepo.setProc("Sp_InventoryTransactions_GetALLMethod", it, ProductionInputRepository.P_INV_TRANSACTIONS);

        /* InputDetailRowsRemoveIds is never set by this form (the grid deletes rows only while Save
           is visible, :1995), so USP_InvFoodProductionDetailRowsDeleteByIds is not reached. */

        for (Map<String, Object> d : details) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", DOC_INPUT);
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
            inRepo.command("dbo.USP_InventoryValidation", p);
        }

        if (toInt(h.get("ActionId")) == 2) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", DOC_INPUT);
            p.put("@InvProductionJobOrderId", h.get("InvJobOrderId"));
            p.put("@PlantId", h.get("PlantId"));
            inRepo.command("dbo.USP_ProductionInPutAndOutPutWeightValidation", p);
        }

        /* per detail, the job-order weight comparison (@RefId = JobOrderInPutId - set on this form) */
        for (Map<String, Object> d : details) {
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@DocumentTypeId", DOC_INPUT);
            p.put("@JobOrderId", h.get("InvJobOrderId"));
            p.put("@RefId", d.get("JobOrderInPutId"));
            p.put("@NetWeight", d.get("Weight"));
            inRepo.command("dbo.usp_ProductionWeightCompareFromJobOrderWeight", p);
        }

        if (headerRefDocumentTypeId != 1 && voucher != null) {
            int existing = inRepo.voucherHeadId(u, DOC_INPUT, id);
            if (existing > 0) voucher.put("Id", existing);
            voucher.put("DocumentTypeSrNo", id);
            int vr = inRepo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", voucher, ProductionInputRepository.P_VOUCHER_HEAD);
            if (vr > 0) voucher.put("Id", vr);
            int voucherId = toInt(voucher.get("Id"));
            if (!lines.isEmpty()) {
                for (Map<String, Object> l : lines) {
                    l.put("VoucherHeadId", voucherId);
                    l.put("BranchesId", h.get("BranchId"));
                    int lid = toInt(l.get("LineId"));
                    for (Map<String, Object> d : details) {
                        if (toInt(d.get("LineId")) == lid && lid > 0) { l.put("RefDocSubIdNo", d.get("Id")); break; }
                    }
                    inRepo.setProc("Sp_VoucherDetail_Insert", l, ProductionInputRepository.P_VOUCHER_DETAIL);
                }
                if (voucherId > 0) {
                    LinkedHashMap<String, Object> p = ProductionInputRepository.params();
                    p.put("@OrganizationId", org);
                    p.put("@CompanyId", comp);
                    p.put("@Id", voucherId);
                    inRepo.command("dbo.USP_VoucherBalanceCheck", p);
                }
            }
        }
        return id;
    }

    /** DAL SetData - the FIFO branch (DocumentTypeId 80, ERP feature 5 on, no reference row). */
    private void fifoBranch(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details,
                            Map<String, Object> voucher, List<Map<String, Object>> lines) {
        int headerId = toInt(h.get("Id"));
        int modifyUser = toInt(h.get("ModifyUser"));
        LocalDateTime docDate = (LocalDateTime) h.get("DocDate");
        List<Map<String, Object>> itemGl = inRepo.itemGlAccounts(u);
        List<Map<String, Object>> jobLotGl = inRepo.jobLotGlIdsAndName(u);
        List<Map<String, Object>> evaluations = new ArrayList<>();

        for (Map<String, Object> item : new ArrayList<>(details)) {
            Map<String, Object> gl = firstById(itemGl, toInt(item.get("ItemId")));
            if (gl == null) continue;
            int rpDocType = 0, rpId = 0;
            if (modifyUser > 0) { rpDocType = toInt(h.get("DocumentTypeId")); rpId = headerId; }
            Map<String, Object> jl = firstById(jobLotGl, toInt(item.get("JobLotId")));
            boolean jobLotHasAccount = jl != null && toInt(ci(jl, "AccountId")) > 0;

            for (Map<String, Object> se : fifoImplementation(u, item, docDate, rpDocType, rpId, evaluations)) {
                if (!jobLotHasAccount) {
                    String comments = "ItemQty: " + net(toDouble(se.get("QtyOut"))) + "   " + str(ci(gl, "ItemName"))
                            + "  Net Weight: " + net(toDouble(se.get("StockWeightOut")))
                            + "  CGS Rate:" + net(toDouble(se.get("CgsRate")));
                    if (voucher == null) throw new Prod284Message("Object reference not set to an instance of an object.");
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
            inRepo.command("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", p);
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
            for (Map<String, Object> x : details) {
                if (toInt(x.get("LineId")) == toInt(se.get("LineId"))) { d = x; break; }
            }
            if (d != null) {
                double eq = inRepo.equivalentByItemAndSchedule(u, toInt(se.get("ItemId")), toInt(d.get("RateUOMId")));
                double rate = toDouble(d.get("Rate"));
                if (toDouble(se.get("BillWeightOut")) > 0.0 && rate > 0.0 && eq > 0.0) {
                    se.put("AmountOut", toDouble(se.get("BillWeightOut")) / eq * rate);
                }
                se.put("ItemRate", rate);
                se.put("RefDocIdNo", d.get("InvFoodProductionId"));
                se.put("RefDocSubIdNo", d.get("Id"));
            }
            inRepo.setProc("USP_InventoryStockEvalautionDetail_Insert", se, ProductionInputRepository.P_STOCK_EVALUATION_INSERT);
        }
    }

    private static Map<String, Object> fifoVoucherLine(Map<String, Object> h, Map<String, Object> item,
                                                       Map<String, Object> gl, Map<String, Object> se,
                                                       String comments, boolean debit) {
        Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
        int wip = toInt(h.get("WIPAccountId"));
        int purchaseGl = toInt(ci(gl, "PurchaseGLAC"));
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
        v.put("RateCut", 0.0);
        v.put("RateCutAmount", 0.0);
        v.put("ItemAmount", cgsAmount);
        v.put("JobLotId", toInt(item.get("JobLotId")));
        v.put("BranchesId", h.get("BranchId"));
        v.put("OrderNo", toInt(item.get("InvProductionJobOrderId")));
        return v;
    }

    /** DAL CommonServices.FIFOImplemention for one detail (the ItemName in the messages is never set - empty). */
    private List<Map<String, Object>> fifoImplementation(UserAccount u, Map<String, Object> item, LocalDateTime docDate,
                                                         int docType, int id, List<Map<String, Object>> reserved) {
        String itemName = "";
        String xml = reserved.isEmpty() ? null : fifoXml(reserved);
        List<Map<String, Object>> rows = inRepo.stockByFifoForSave(u, toInt(item.get("ItemId")), toInt(item.get("ItemUomId")),
                docDate, toInt(item.get("WarehouseId")), toInt(item.get("JobLotId")), toInt(item.get("PackingtypeId")),
                (String) item.get("CropBatch"), docType, id, xml);
        if (rows.isEmpty()) throw new Prod284Message("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0.0;
        for (Map<String, Object> r : rows) available += toDouble(ci(r, "NetBalWeight"));
        double itemQty = toDouble(item.get("Qty"));
        double netWeight = toDouble(item.get("Weight"));
        if (!(netWeight <= netRound2(available))) {
            throw new Prod284Message("Weight available is " + net(available) + " and row Weight is " + net(netWeight)
                    + " this item " + itemName + " against FIFO....");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        double l12 = netWeight, l13 = itemQty, l15 = 0.0, l17 = 0.0;
        for (Map<String, Object> r : rows) {
            double avgRate = toDouble(ci(r, "AvgRate"));
            if (!(avgRate > 0.0)) throw new Prod284Message("Rate Not Found this Item " + itemName + " against FIFO Method");
            int rateUomId = toInt(ci(r, "RateUomId"));
            if (rateUomId == 0) throw new Prod284Message("RateUomId not found  this " + itemName + " against FIFO Method");
            double l16 = toDouble(ci(r, "NetBalWeight"));
            double l14 = toDouble(ci(r, "NetBalQty"));
            double eq = inRepo.equivalentByItemAndSchedule(u, toInt(item.get("ItemId")), rateUomId);
            if (eq == 0.0) throw new Prod284Message("RateUom Not Found");
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
        se.put("Id", toInt(ci(r, "Id")));
        se.put("LineId", toInt(item.get("LineId")));
        se.put("ItemId", toInt(item.get("ItemId")));
        se.put("WarehouseId", toInt(item.get("WarehouseId")));
        se.put("RateUom", rateUomId);
        se.put("JobLotId", toInt(item.get("JobLotId")));
        se.put("InvPackingTypeId", toInt(item.get("PackingtypeId")));
        se.put("ItemUom", toInt(item.get("ItemUomId")));
        se.put("CropBatch", str(item.get("CropBatch")));
        se.put("RefRefDocumentTypeId", toInt(ci(r, "RefDocumentTypeId")));
        se.put("RefRefDocIdNo", toInt(ci(r, "RefDocIdNo")));
        se.put("RefRefDocSubIdNo", toInt(ci(r, "RefDocSubIdNo")));
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

    // ==================================================================================== OUTPUT (112)

    /**
     * OutPutInsert:2891. Body: { id, docNumber, docDate, remarks, jobOrderId, jobOrderText, plantId,
     * wipAccountId, wipItemId, rows:[grid rows as `tableByProduct` holds them] }.
     * The DAL SetData for 112 is ProductionOutputRepository.save (one transaction).
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> saveOutput(Map<String, Object> body) {
        UserAccount u = user();
        int recId = toInt(body.get("id"));
        Map<String, Object> rights = reads.rights();
        requireRight(recId, rights);

        /* FormValidationOutPut:2403 */
        String docNumber = str(body.get("docNumber"));
        if (docNumber.isEmpty() || "0".equals(docNumber)) throw new Prod284Message("DocNo Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        if (jobOrderId == 0) throw new Prod284Message("Job Order Number Field Required");
        int plantId = toInt(body.get("plantId"));
        if (plantId == 0) throw new Prod284Message("Plant Name Field Is Required");
        int wipAccountId = toInt(body.get("wipAccountId"));
        if (wipAccountId == 0) throw new Prod284Message("WorkInProcess Field Required");
        int wipItemId = toInt(body.get("wipItemId"));
        if (wipItemId == 0) throw new Prod284Message("WIP Item Field Required");

        /* :2918 - a NEW document re-reads its serial right before the save */
        int docCode = recId > 0 ? toInt(docNumber) : Math.max(reads.serial(DOC_OUTPUT), 0);
        if (docCode == 0) docCode = toInt(docNumber);

        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        if (rows.isEmpty()) throw new Prod284Message("Enter Detail First ...");   // :3019

        LocalDateTime now = LocalDateTime.now();
        int userId = u.getId();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("@Id", recId);
        h.put("@DocDate", docDateTime(str(body.get("docDate"))));
        h.put("@DocCode", docCode);
        h.put("@DocumentTypeId", DOC_OUTPUT);
        h.put("@WIPAccountId", wipAccountId);
        h.put("@WIPItemId", wipItemId);
        h.put("@MainRemarks", str(body.get("remarks")));
        h.put("@IsApproved", false);
        h.put("@EntryDate", now);
        h.put("@EntryUser", userId);
        h.put("@ModifyDate", now);
        h.put("@ModifyUser", userId);
        h.put("@OrganizationId", u.getOrganizationId());
        h.put("@CompanyId", u.getCompanyId());
        h.put("@InvFoodProductionPlanId", 0);
        h.put("@InvJobOrderId", jobOrderId);
        h.put("@InvJobOrderNo", str(body.get("jobOrderText")));
        h.put("@EntryType", "Output");
        h.put("@BranchId", branchId(u));
        h.put("@ProjectId", 0);
        h.put("@ActionId", 0);
        h.put("@FinancialYearId", fy());
        h.put("@PlantId", plantId);
        h.put("@EBDepartmentId", 0);
        h.put("@ContractScheduleId", 0);
        h.put("@WipWareHouseId", 0);
        h.put("@BaseDocumentTypeId", 0);
        h.put("@StockHoldForLabApproval", false);

        /* :2944-2992 the row loop: Conversion.ToInt(Quantity) > 0 && Weight > 0 */
        double grossWeight = 0.0;
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!(Math.rint(toDouble(r.get("Quantity"))) > 0 && toDouble(r.get("Weight")) > 0.0)) continue;
            String entryType = str(r.get("EntryType"));
            if (entryType.isEmpty()) throw new Prod284Message("EntryType Field Require in Output Detail");
            Map<String, Object> det = new LinkedHashMap<>();
            det.put("@EntryType", entryType);
            det.put("@Id", toInt(r.get("Id")));
            det.put("@OutPutIdFromJobOrder", toInt(r.get("OutPutIdFromJobOrder")));
            if (str(r.get("WareHouseId")).isEmpty() || toInt(r.get("WareHouseId")) == 0) throw new Prod284Message("WareHouse Field Require in Output Detail");
            det.put("@WarehouseId", toInt(r.get("WareHouseId")));
            if (str(r.get("ItemId")).isEmpty() || toInt(r.get("ItemId")) == 0) throw new Prod284Message("Item Field Require in Output Detail");
            det.put("@ItemId", toInt(r.get("ItemId")));
            det.put("@ItemUomId", toInt(r.get("ItemUOMId")));
            det.put("@CropBatch", str(r.get("CropYear")));
            String jl = str(r.get("JobLotId"));
            if (jl.isEmpty() || "0".equals(jl)) throw new Prod284Message("JobLot Field Require in Output Detail");
            det.put("@JobLotId", toInt(r.get("JobLotId")));
            det.put("@PackingtypeId", toInt(r.get("PackingTypeId")));
            double qty = toDouble(r.get("Quantity"));
            double weight = toDouble(r.get("Weight"));
            det.put("@Qty", qty);
            det.put("@Weight", weight);
            grossWeight += weight;
            det.put("@Rate", toDouble(r.get("Rate")));
            det.put("@RateUOMId", toInt(r.get("RateUOMId")));
            det.put("@InvProductionJobOrderId", jobOrderId);
            /* :2985 Conversion.ToInt(ItemAmount) */
            det.put("@Amount", (double) toInt(r.get("ItemAmount")));
            det.put("@NetRate", toDouble(r.get("NetRate")));
            det.put("@TotalAmount", toDouble(r.get("NetAmount")));
            det.put("@VoucherHeadId", 0);
            det.put("@Remarks", str(r.get("Remarks")));
            /* model value-type defaults this form never sets */
            det.put("@InvFoodProductionId", 0);
            det.put("@RefDocumentTypeId", 0);
            det.put("@RefDocNoId", 0);
            det.put("@RefDocSubIdNo", 0);
            det.put("@InvProductionJobOrderNo", 0);
            det.put("@PackUnit", 0);
            det.put("@ItemPmCost", 0.0);
            det.put("@GeneralPmCost", 0.0);
            det.put("@ItemOhCost", 0.0);
            det.put("@GeneralOhCost", 0.0);
            det.put("@StockAcId", 0);
            det.put("@JobOrderInPutId", 0);
            det.put("@DeleteFlag", 0);
            det.put("@LineId", 0);
            det.put("@JobOrderScheduleId", 0);
            det.put("@GrossWeight", 0.0);
            det.put("@EbUnit", 0.0);
            det.put("@EbTotal", 0.0);
            det.put("@MoveOrderDocumentTypeId", 0);
            det.put("@MoveOrderDocId", 0);
            details.add(det);
        }
        if (details.isEmpty()) throw new Prod284Message("grid Record not found please check!");   // :2995

        /* BLL InvFoodProduction.Save - the two switches; 112 also honours JobOrderCreatewithoutRates */
        boolean withoutRates = flag(u, "JobOrderCreatewithoutRates");
        boolean financialsInactive = flag(u, "InventoryFinancialsEffectsInActive");
        boolean refDocTypeOne = financialsInactive || withoutRates;
        Map<String, Object> voucher = null;
        List<Map<String, Object>> voucherLines = null;
        if (!refDocTypeOne) {
            voucherLines = new ArrayList<>();
            voucher = makeVoucher112(h, details, voucherLines);
        }
        boolean insert = recId == 0;
        h.put("@ActionId", insert ? 1 : 2);
        if (insert) h.put("@ModifyUser", 0); else h.put("@EntryUser", 0);

        /* OutputDetailRowsRemoveIds is never set by this form (:2729 refuses deletes in update mode) */
        int id = outRepo.save(h, details, voucher, voucherLines, withoutRates, null);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("docCode", docCode);
        out.put("message", (recId > 0 ? "Record Update Successfully" : "Record Save Successfully") + docCode);
        out.put("openWages", wagesStatus(u));   // :3006 -> frmwagesBillHeader(112, id, GrossWeight)
        out.put("grossWeightTotal", grossWeight);
        return out;
    }

    /** BLL InvFoodProduction.MakeVoucher, the DocumentTypeId 112 branch (as the 280 Output port reads it). */
    private Map<String, Object> makeVoucher112(Map<String, Object> h, List<Map<String, Object>> details,
                                               List<Map<String, Object>> lines) {
        List<Map<String, Object>> items = outRepo.itemGlAccounts(toInt(h.get("@OrganizationId")), toInt(h.get("@CompanyId")));
        Map<String, Object> v = ProductionOutputRepository.voucherHeadDefaults();
        v.put("@DocumentTypeSrNo", toInt(h.get("@Id")));
        v.put("@VoucherDate", h.get("@DocDate"));
        v.put("@VoucherCode", h.get("@DocCode"));
        v.put("@DocumentTypeId", DOC_OUTPUT);
        v.put("@BaseDocumentTypeId", h.get("@BaseDocumentTypeId"));
        v.put("@RefAccountId", h.get("@WIPAccountId"));
        v.put("@AgainstAccountId", h.get("@WIPItemId"));
        v.put("@EntryDate", LocalDateTime.now());
        v.put("@ModifyDate", LocalDateTime.now());
        v.put("@EntryUser", h.get("@EntryUser"));
        v.put("@ModifyUser", h.get("@ModifyUser"));
        v.put("@OrganizationId", h.get("@OrganizationId"));
        v.put("@CompanyId", h.get("@CompanyId"));
        v.put("@FinancialYearId", h.get("@FinancialYearId"));
        v.put("@BranchId", h.get("@BranchId"));
        double total = 0;
        int ln = 1;
        String jobNo = str(h.get("@InvJobOrderNo"));
        int wip = toInt(h.get("@WIPAccountId"));
        for (Map<String, Object> det : details) {
            if (items.isEmpty()) throw new Prod284Message("Item GL Account Not Found");
            Map<String, Object> gl = firstById(items, toInt(det.get("@ItemId")));
            if (gl == null) continue;
            String remarks = str(det.get("@Remarks"));
            double qty = toDouble(det.get("@Qty")), weight = toDouble(det.get("@Weight")), rate = toDouble(det.get("@Rate"));
            double amount = toDouble(det.get("@Amount"));
            String comments = jobNo + " " + (remarks.isEmpty() ? "" : remarks + " ") + " ItemName: "
                    + str(ci(gl, "ItemName")) + " Qty " + net(qty) + " Weight " + net(weight) + " Rate " + net(rate);
            int stockAc = toInt(det.get("@StockAcId")) > 0 ? toInt(det.get("@StockAcId")) : toInt(ci(gl, "PurchaseGLAC"));

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

    // =========================================================================== PACKING MATERIAL (111)

    /**
     * PackingMaterialInsert:3531. Body: { id, docNo, docDate, jobOrderId, plantId,
     * rows:[grid rows as `tablePackingMaterial` holds them] }. Then InvFoodProductionPackingMaterial.Save
     * -> DAL SetData per row, one transaction (the 280 PM port's reading of the disassembly, with
     * this form's JobOrderPackingMaterialId carried through).
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> savePm(Map<String, Object> body) {
        UserAccount u = user();
        int recId = toInt(body.get("id"));
        Map<String, Object> rights = reads.rights();
        requireRight(recId, rights);
        boolean rateRight = Boolean.TRUE.equals(rights.get("rateAndAmount"));

        /* FormValidationOfPackingMaterial:3328 */
        String docNoText = str(body.get("docNo"));
        if (docNoText.isEmpty()) throw new Prod284Message("Doc No. Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        if (jobOrderId == 0) throw new Prod284Message("Job Order No. Field Required");
        if (toInt(body.get("plantId")) == 0) throw new Prod284Message("Plant Name Field Required");

        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        LocalDateTime now = LocalDateTime.now();
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!(toInt(r.get("ItemQty")) > 0)) continue;   // :3567 Conversion.ToInt(ItemQty) > 0
            String plantText = str(r.get("PlantId"));
            if (toInt(r.get("PlantId")) == 0 && plantText.isEmpty()) throw new Prod284Message("Plant Field Required in Detail");
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("PlantId", toInt(r.get("PlantId")));
            m.put("InvProductionJobOrderId", jobOrderId);
            m.put("DocumentTypeId", DOC_PM);
            m.put("EntryDate", now);
            m.put("ModifyDate", now);
            m.put("EntryUser", u.getId());
            m.put("ModifyUser", u.getId());
            m.put("OrganizationId", u.getOrganizationId());
            m.put("CompanyId", u.getCompanyId());
            m.put("BranchesId", branchId(u));
            m.put("FinancialYearId", fy());
            m.put("Id", toInt(r.get("Id")));
            m.put("JobOrderPackingMaterialId", toInt(r.get("JobOrderPackingMaterialId")));
            m.put("DocNo", toInt(r.get("DocNo")));
            m.put("DocDate", docDateTime(str(r.get("DocDate"))));
            if (str(r.get("PackingItemId")).isEmpty() || toInt(r.get("PackingItemId")) == 0) throw new Prod284Message("Packing Material Item Field Require in Detail");
            m.put("ItemId", toInt(r.get("PackingItemId")));
            int brand = toInt(r.get("BrandId")), brandUom = toInt(r.get("BrandUomId"));
            if (brand == 0 && brandUom != 0) throw new Prod284Message("OutPut Item Field Require in Detail");
            m.put("BrandId", brand);
            if (brandUom == 0 && brand != 0) throw new Prod284Message("OutPut UomField Require in Detail");
            m.put("BrandUomId", brandUom);
            String qtyText = str(r.get("ItemQty")), rateText = str(r.get("ItemRate")), amtText = str(r.get("Amount"));
            double qty = toDouble(r.get("ItemQty")), rate = toDouble(r.get("ItemRate")), amount = toDouble(r.get("Amount"));
            if ((qtyText.isEmpty() || qty == 0.0) && (!rateText.isEmpty() || rate != 0.0)) throw new Prod284Message("ItemQty Required in Detail");
            m.put("Qty", qty);
            if ((rateText.isEmpty() || rate == 0.0) && (!qtyText.isEmpty() || qty != 0.0) && rateRight) throw new Prod284Message("ItemRate Required in Detail");
            m.put("Rate", rate);
            if ((amtText.isEmpty() || amount == 0.0) && rateRight) throw new Prod284Message("Amount Required in Detail");
            m.put("Amount", amount);
            m.put("pmRemarks", str(r.get("Remarks")));
            m.put("WarehouseId", toInt(r.get("WarehouseId")));
            /* model value-type defaults this form never sets */
            m.put("itemUomSchId", 0);
            m.put("ItemConditionId", 0);
            m.put("ContractScheduleId", 0);
            m.put("ExImInvoiceId", 0);
            m.put("BaseDocumentTypeId", 0);
            m.put("RackId", 0);
            list.add(m);
        }
        if (!rows.isEmpty() && list.isEmpty()) throw new Prod284Message("Grid Record not found please check!");   // :3632

        boolean finEffectsOff = flag(u, "InventoryFinancialsEffectsInActive");
        new TransactionTemplate(transactionManager).execute(status -> { setDataPm(u, list, finEffectsOff); return null; });

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");
        return out;
    }

    /** DAL InvFoodProductionPackingMaterial.SetData, per row, on the save's transaction. */
    private void setDataPm(UserAccount u, List<Map<String, Object>> rows, boolean finEffectsOff) {
        Map<String, Object> voucher = ProductionInputRepository.voucherHeadDefaults();
        List<Map<String, Object>> voucherDetails = new ArrayList<>();
        for (Map<String, Object> pm : rows) {
            int loc0, loc1 = 0, loc2;
            int org = toInt(pm.get("OrganizationId")), comp = toInt(pm.get("CompanyId"));

            /* 1. the row - Insert when Id == 0, else Update */
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            for (Map.Entry<String, Object> e : pm.entrySet()) p.put("@" + e.getKey(), e.getValue());
            loc0 = toInt(inRepo.scalar(toInt(pm.get("Id")) != 0 ? "dbo.Sp_InvFoodProductionPackingMaterial_Update"
                                                                : "dbo.Sp_InvFoodProductionPackingMaterial_Insert", p));
            if (loc0 > 0) { pm.put("ModifyUser", 0); pm.put("Id", loc0); } else { pm.put("EntryUser", 0); loc0 = toInt(pm.get("Id")); }

            /* 2. item GL list, 3. the job order's WIP account */
            List<Map<String, Object>> itemGl = inRepo.itemGlAccounts(u);
            List<Map<String, Object>> wip = inRepo.glAccountsByJobOrder(u, toInt(pm.get("InvProductionJobOrderId")));
            if (wip == null || wip.isEmpty()) throw new Prod284Message("Work in process Account not found against JobOrder");
            int wipAccountId = toInt(ci(wip.get(0), "WorkInProccessAcId"));

            /* 5. stock evaluation, 6. stock movements */
            Map<String, Object> ev = ProductionInputRepository.stockEvaluationDefaults();
            ev.put("OrganizationId", org);
            ev.put("CompanyId", comp);
            ev.put("RefDocumentTypeId", DOC_PM);
            ev.put("RefDocIdNo", loc0);
            inRepo.setProc("Sp_InventoryStockEvalautionDetail_Update", ev, ProductionInputRepository.P_STOCK_EVALUATION_UPDATE);
            Map<String, Object> tr = ProductionInputRepository.inventoryTransactionsDefaults();
            tr.put("OrganizationId", org);
            tr.put("CompanyId", comp);
            tr.put("RefDocumentTypeId", DOC_PM);
            tr.put("RefDocIdNo", loc0);
            inRepo.setProc("Sp_InventoryTransactions_GetALLMethod", tr, ProductionInputRepository.P_INV_TRANSACTIONS);

            /* 7. USP_InventoryValidation for every row of the list, @Activity='PM' */
            for (Map<String, Object> v : rows) {
                LinkedHashMap<String, Object> vp = ProductionInputRepository.params();
                vp.put("@OrganizationId", v.get("OrganizationId"));
                vp.put("@CompanyId", v.get("CompanyId"));
                vp.put("@DocumentTypeId", v.get("DocumentTypeId"));
                vp.put("@DocDate", v.get("DocDate"));
                vp.put("@ItemId", v.get("ItemId"));
                vp.put("@WarehouseId", v.get("WarehouseId"));
                vp.put("@NetWeight", v.get("Qty"));
                vp.put("@ItemConditionId", v.get("ItemConditionId"));
                vp.put("@RackId", v.get("RackId"));
                vp.put("@Activity", "PM");
                inRepo.command("dbo.USP_InventoryValidation", vp);
            }

            /* 8. the voucher */
            if (loc0 <= 0 || finEffectsOff) continue;
            if (toInt(pm.get("ModifyUser")) > 0) {
                loc1 = inRepo.voucherHeadId(u, DOC_PM, toInt(pm.get("Id")));
                voucher.put("Id", loc1);
            }
            voucher.put("DocumentTypeSrNo", loc0);
            voucher.put("VoucherDate", pm.get("DocDate"));
            voucher.put("VoucherCode", pm.get("DocNo"));
            voucher.put("DocumentTypeId", DOC_PM);
            voucher.put("BaseDocumentTypeId", pm.get("BaseDocumentTypeId"));
            voucher.put("RefAccountId", wipAccountId);
            voucher.put("AgainstAccountId", wipAccountId);
            voucher.put("EntryDate", LocalDateTime.now());
            voucher.put("ModifyDate", LocalDateTime.now());
            voucher.put("VoucherAmount", pm.get("Amount"));
            voucher.put("EntryUser", pm.get("EntryUser"));
            voucher.put("ModifyUser", pm.get("ModifyUser"));
            voucher.put("OrganizationId", org);
            voucher.put("CompanyId", comp);
            voucher.put("FinancialYearId", pm.get("FinancialYearId"));
            voucher.put("BranchId", pm.get("BranchesId"));

            if (itemGl == null || itemGl.isEmpty()) throw new Prod284Message("Item list Not Found");
            Map<String, Object> gl = firstById(itemGl, toInt(pm.get("ItemId")));
            if (gl == null) throw new Prod284Message("Item GlAccountId Not Found");
            int purchaseGlac = toInt(ci(gl, "PurchaseGLAC"));
            double amount = toDouble(pm.get("Amount"));
            String remarks = str(pm.get("pmRemarks"));

            Map<String, Object> d1 = ProductionInputRepository.voucherDetailDefaults();
            d1.put("AccountId", wipAccountId);
            d1.put("AgainstAccountId", purchaseGlac);
            d1.put("Comments", remarks);
            d1.put("DebitAmount", amount);
            d1.put("ItemId", pm.get("ItemId"));
            d1.put("ItemAmount", amount);
            d1.put("OrderNo", pm.get("InvProductionJobOrderId"));
            d1.put("BranchesId", pm.get("BranchesId"));
            voucherDetails.add(d1);
            Map<String, Object> d2 = ProductionInputRepository.voucherDetailDefaults();
            d2.put("AccountId", purchaseGlac);
            d2.put("AgainstAccountId", wipAccountId);
            d2.put("Comments", remarks);
            d2.put("CreditAmount", amount);
            d2.put("ItemId", pm.get("ItemId"));
            d2.put("ItemAmount", amount);
            d2.put("QtyOut", pm.get("Qty"));
            d2.put("WeightOut", pm.get("Qty"));
            d2.put("OrderNo", pm.get("InvProductionJobOrderId"));
            d2.put("BranchesId", pm.get("BranchesId"));
            voucherDetails.add(d2);

            loc2 = inRepo.setProc(loc1 == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", voucher, ProductionInputRepository.P_VOUCHER_HEAD);
            if (loc2 > 0) voucher.put("Id", loc2);
            int headId = toInt(voucher.get("Id"));
            for (Map<String, Object> d : voucherDetails) {
                d.put("VoucherHeadId", headId);
                inRepo.setProc("Sp_VoucherDetail_Insert", d, ProductionInputRepository.P_VOUCHER_DETAIL);
            }
            if (headId > 0) {
                LinkedHashMap<String, Object> bp = ProductionInputRepository.params();
                bp.put("@OrganizationId", org);
                bp.put("@CompanyId", comp);
                bp.put("@Id", headId);
                inRepo.command("dbo.USP_VoucherBalanceCheck", bp);
            }
        }
    }

    // ================================================================================= OVERHEAD (110)

    /**
     * OverHeadInsert:4275. Body: { id (RecOverHeadId), docNo, jobOrderId, plantId,
     * rows:[grid rows as `tableOverHeads` holds them] }. Then InvFoodProductionOverHeads.Save -> DAL
     * SetData per row, one transaction (ProductionOverheadRepository's writers).
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> saveOh(Map<String, Object> body) {
        UserAccount u = user();
        int recId = toInt(body.get("id"));
        Map<String, Object> rights = reads.rights();
        requireRight(recId, rights);

        /* FormValidationOfOverHead:4954 */
        if (str(body.get("docNo")).isEmpty()) throw new Prod284Message("Doc No. Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        if (jobOrderId == 0) throw new Prod284Message("Job Order No. Field Required");
        if (toInt(body.get("plantId")) == 0) throw new Prod284Message("Plant Field Required");

        List<Map<String, Object>> rows = body.get("rows") instanceof List ? (List<Map<String, Object>>) body.get("rows") : new ArrayList<>();
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            /* :4311 ChartOfAccountId > 0 && Amount > 0 */
            if (!(toInt(r.get("ChartOfAccountId")) > 0 && toDouble(r.get("Amount")) > 0.0)) continue;
            if (toInt(r.get("PlantId")) == 0 && str(r.get("PlantId")).isEmpty()) throw new Prod284Message("Plant Field Required in Detail");
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("PlantId", toInt(r.get("PlantId")));
            m.put("InvProductionJobOrderId", jobOrderId);
            m.put("DocumentTypeId", DOC_OH);
            m.put("EntryDate", now);
            m.put("ModifyDate", now);
            m.put("EntryUser", u.getId());
            m.put("ModifyUser", u.getId());
            m.put("OrganizationId", u.getOrganizationId());
            m.put("CompanyId", u.getCompanyId());
            m.put("BranchesId", branchId(u));
            m.put("FinancialYearId", fy());
            m.put("Id", toInt(r.get("Id")));
            m.put("JobOrderOverHeadId", toInt(r.get("JobOrderOverHeadId")));
            m.put("DocNo", toInt(r.get("DocNo")));
            LocalDate dd = parseDate(r.get("DocDate"));
            m.put("DocDate", dd == null ? LocalDate.now() : dd);
            if (str(r.get("ChargesTypeId")).isEmpty() || toInt(r.get("ChargesTypeId")) == 0) throw new Prod284Message("ChargesType Field Require in Detail");
            m.put("ChargesTypeId", toInt(r.get("ChargesTypeId")));
            if (str(r.get("ChartOfAccountId")).isEmpty() || toInt(r.get("ChartOfAccountId")) == 0) throw new Prod284Message("OverHead Account Field Require in Detail");
            m.put("CharOfAccountId", toInt(r.get("ChartOfAccountId")));
            int brand = toInt(r.get("BrandId")), brandUom = toInt(r.get("BrandUomId"));
            if (brand == 0 && brandUom != 0) throw new Prod284Message("OutPut Item Field Require in Detail");
            m.put("BrandId", brand);
            if (brandUom == 0 && brand != 0) throw new Prod284Message("OutPut UomField Require in Detail");
            m.put("BrandUomId", brandUom);
            String qtyText = str(r.get("ItemQty")), rateText = str(r.get("ItemRate"));
            double qty = toDouble(r.get("ItemQty")), rate = toDouble(r.get("ItemRate"));
            if ((qtyText.isEmpty() || qty == 0.0) && (!rateText.isEmpty() || rate != 0.0)) throw new Prod284Message("ItemQty Required in Detail");
            m.put("ItemQty", qty);
            if ((rateText.isEmpty() || rate == 0.0) && (!qtyText.isEmpty() || qty != 0.0)) throw new Prod284Message("ItemRate Required in Detail");
            m.put("ItemRate", rate);
            int rateUom = toInt(r.get("RateUomId"));
            if ((str(r.get("RateUomId")).isEmpty() || rateUom == 0) && (str(r.get("BrandId")).isEmpty() || brand == 0)
                    && (str(r.get("BrandUomId")).isEmpty() || brandUom == 0) && toInt(r.get("ChargesTypeId")) == 1)
                throw new Prod284Message("RateUom Required in Detail");
            m.put("RateUomId", rateUom);
            double amount = toDouble(r.get("Amount"));
            if (str(r.get("Amount")).isEmpty() || amount == 0.0) throw new Prod284Message("Amount Required in Detail");
            m.put("Amount", amount);
            m.put("ohRemarks", str(r.get("Remarks")));
            /* never assigned by this form: the C# defaults */
            m.put("BaseDocumentTypeId", 0);
            m.put("DetailDocumentTypeId", 0);
            list.add(m);
        }
        if (!rows.isEmpty() && list.isEmpty()) throw new Prod284Message("Grid Record not found please check!");   // :4387

        new TransactionTemplate(transactionManager).execute(status -> { setDataOh(list); return null; });
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");
        return out;
    }

    /** DAL InvFoodProductionOverHeads.SetData, per row (the 280 Overhead port's reading). */
    private void setDataOh(List<Map<String, Object>> list) {
        int o = ctx.currentOrganizationId(), c = ctx.currentCompanyId();
        boolean financialsInactive = Prod284ProductionNewService.asBool(ohRepo.configKey(o, c, "InventoryFinancialsEffectsInActive"));
        for (Map<String, Object> row : list) {
            boolean insert = toInt(row.get("Id")) == 0;
            int loc0 = ohRepo.saveOverheadRow(insert, at(row));
            if (loc0 > 0) { row.put("ModifyUser", 0); row.put("Id", loc0); } else { row.put("EntryUser", 0); loc0 = toInt(row.get("Id")); }
            if (financialsInactive) continue;

            Map<String, Object> vh = ProductionOverheadRepository.voucherHeadDefaults();
            int loc1 = 0;
            if (toInt(row.get("ModifyUser")) > 0) {
                loc1 = ohRepo.voucherHeadIdInTransaction(o, c, toInt(row.get("DocumentTypeId")), toInt(row.get("Id")));
                vh.put("Id", loc1);
            }
            int jobOrderId = toInt(row.get("InvProductionJobOrderId"));
            List<Map<String, Object>> gl = ohRepo.glAccountsByJobOrderId(o, c, jobOrderId);
            if (gl.isEmpty()) throw new Prod284Message("JobOrder WIPAccount Not Found");
            int wip = toInt(ProductionOverheadRepository.col(gl.get(0), "WorkInProccessAcId"));
            int coa = toInt(row.get("CharOfAccountId"));
            double amount = toDouble(row.get("Amount"));
            String remarks = str(row.get("ohRemarks"));
            Object docDate = row.get("DocDate");

            vh.put("DocumentTypeSrNo", loc0);
            vh.put("VoucherDate", docDate == null ? null : Timestamp.valueOf(((LocalDate) docDate).atStartOfDay()));
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
            vh.put("OrganizationId", o);
            vh.put("CompanyId", c);
            vh.put("FinancialYearId", toInt(row.get("FinancialYearId")));
            vh.put("BranchId", toInt(row.get("BranchesId")));

            List<Map<String, Object>> lines = new ArrayList<>();
            Map<String, Object> d1 = ProductionOverheadRepository.voucherDetailDefaults();
            d1.put("AccountId", wip);
            d1.put("AgainstAccountId", coa);
            d1.put("Comments", remarks);
            d1.put("DebitAmount", amount);
            d1.put("OrderNo", jobOrderId);
            lines.add(d1);
            Map<String, Object> d2 = ProductionOverheadRepository.voucherDetailDefaults();
            d2.put("AccountId", coa);
            d2.put("AgainstAccountId", wip);
            d2.put("Comments", remarks);
            d2.put("CreditAmount", amount);
            d2.put("OrderNo", jobOrderId);
            lines.add(d2);

            int loc2 = ohRepo.saveVoucherHead(loc1 == 0, at(vh));
            if (loc2 > 0) vh.put("Id", loc2);
            int headId = toInt(vh.get("Id"));
            for (Map<String, Object> d : lines) {
                d.put("VoucherHeadId", headId);
                ohRepo.insertVoucherDetail(at(d));
            }
            if (headId > 0) ohRepo.voucherBalanceCheck(o, c, headId);
        }
    }

    // ================================================================================ SETTLEMENT (142)

    /**
     * SettlementFinancials:6577 (btnUpdateSettlement_Click:6682 after "Are you sure to Update Settlement?").
     * Body: { jobOrderId, differenceAccountId, actualFinishGoodsText, finishAmountAgainstJobOrderText,
     * differenceAmountText }. Returns the DAL result: > 0 -> "Record Save Successfully", else
     * "Record Update Successfully" (:6664-6670).
     */
    public Map<String, Object> settlementFinancials(Map<String, Object> body) {
        UserAccount u = user();
        int jobOrderId = toInt(body.get("jobOrderId"));
        if (jobOrderId == 0) throw new Prod284Message("JobOrder Settlement Field is Required");
        int diffAccountId = toInt(body.get("differenceAccountId"));
        if (diffAccountId == 0) throw new Prod284Message("Difference Account Field is Required");

        List<Map<String, Object>> gl = inRepo.glAccountsByJobOrder(u, jobOrderId);
        if (gl == null || gl.isEmpty()) throw new Prod284Message("WIP Account Not Found");
        int wip = toInt(ci(gl.get(0), "WorkInProccessAcId"));
        int voucherId = inRepo.voucherHeadId(u, DOC_SETTLEMENT, jobOrderId);

        String actual = str(body.get("actualFinishGoodsText"));
        String againstJo = str(body.get("finishAmountAgainstJobOrderText"));
        String diffText = str(body.get("differenceAmountText"));
        /* :6634 Conversion.ToDouble(txtDifferenceAmount.Text) - the "0,0" display text */
        double differenceAmount = toDouble(diffText.replace(",", ""));
        String remarks = "Finish Goods ActualAmount " + actual + " Finish Goods Amount against JobOrder Rates " + againstJo
                + " Difference Amount " + diffText;

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        LinkedHashMap<String, Object> head = ProductionSettlementRepository.voucherHeadDefaults();
        head.put("@Id", voucherId);
        head.put("@DocumentTypeSrNo", jobOrderId);
        head.put("@VoucherDate", now);
        head.put("@DocumentTypeId", DOC_SETTLEMENT);
        head.put("@RefAccountId", wip);
        head.put("@AgainstAccountId", diffAccountId);
        head.put("@EntryDate", now);
        head.put("@ModifyDate", now);
        head.put("@EntryUser", u.getId());
        head.put("@ModifyUser", u.getId());
        head.put("@OrganizationId", u.getOrganizationId());
        head.put("@CompanyId", u.getCompanyId());
        head.put("@FinancialYearId", fy());
        head.put("@BranchId", branchId(u));
        head.put("@VoucherAmount", Math.abs(differenceAmount));

        List<LinkedHashMap<String, Object>> lines = new ArrayList<>();
        LinkedHashMap<String, Object> vd = ProductionSettlementRepository.voucherDetailDefaults();
        vd.put("@AccountId", diffAccountId);
        vd.put("@AgainstAccountId", wip);
        vd.put("@Comments", remarks);
        if (differenceAmount > 0.0) vd.put("@DebitAmount", differenceAmount); else vd.put("@CreditAmount", differenceAmount);
        vd.put("@OrderNo", jobOrderId);
        lines.add(vd);
        LinkedHashMap<String, Object> vd2 = ProductionSettlementRepository.voucherDetailDefaults();
        vd2.put("@AccountId", wip);
        vd2.put("@AgainstAccountId", diffAccountId);
        vd2.put("@Comments", remarks);
        if (differenceAmount > 0.0) vd2.put("@CreditAmount", differenceAmount); else vd2.put("@DebitAmount", differenceAmount);
        vd2.put("@OrderNo", jobOrderId);
        lines.add(vd2);

        int result = stRepo.settlementFinancials(head, lines);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("result", result);
        out.put("message", result > 0 ? "Record Save Successfully" : "Record Update Successfully");
        return out;
    }

    /** btnApproveSettlement_Click:6796 - InvProductionJobOrder.JobOrderApproveList with this one job order. */
    public Map<String, Object> approveSettlement(int jobOrderId) {
        if (jobOrderId <= 0) throw new Prod284Message("JobOrder Settlement Field is Required");
        stRepo.jobOrderApprove(ctx.currentOrganizationId(), ctx.currentCompanyId(), jobOrderId, ctx.currentUserId());
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Record Approved Successfully");
        return out;
    }

    // ================================================================================== helpers

    private static Map<String, Object> firstById(List<Map<String, Object>> rows, int id) {
        if (rows == null) return null;
        for (Map<String, Object> r : rows) if (toInt(ci(r, "Id")) == id) return r;
        return null;
    }

    /** Model property -> @PropertyName (GenericProvider.SetProc); LocalDate -> SQL date. */
    private static Map<String, Object> at(Map<String, Object> model) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : model.entrySet()) {
            Object v = e.getValue();
            if (v instanceof LocalDate) v = java.sql.Date.valueOf((LocalDate) v);
            p.put("@" + e.getKey(), v);
        }
        return p;
    }

    private static LocalDate parseDate(Object o) {
        String s = str(o);
        if (s.length() >= 10) {
            try { return LocalDate.parse(s.substring(0, 10)); } catch (Exception ignored) { }
        }
        return null;
    }

    /** .NET Framework double.ToString(): up to 15 significant digits, no trailing zeros. */
    static String net(double d) {
        if (Double.isNaN(d)) return "NaN";
        if (Double.isInfinite(d)) return d > 0 ? "Infinity" : "-Infinity";
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return new BigDecimal(d).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }

    private static String xmlDouble(double d) {
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    private static double netRound2(double v) {
        if (Math.abs(v) >= 1e16) return v;
        return Math.rint(v * 100.0) / 100.0;
    }
}
