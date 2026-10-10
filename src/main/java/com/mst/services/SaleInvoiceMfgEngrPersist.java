package com.mst.services;

import com.mst.models.saleinvoice.SaleInvoiceModels.*;
import com.mst.repositories.SaleInvoiceRepository;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * NEW copy of SaleInvoiceEngrPersist (DAL 0433 InvSaleInvoice.SetData) for the module 134 Sale Engr (Mfg) ports (E4): 1660 (frmSaleInvoiceEngr), 1661 (frmSaleInvoiceDirectEngr) and 1662 (frmSaleInvoiceReturnEngr) are added to the supported set. The branches for them already exist in the E1 code. E1 note: document types
 * 1609 (SaleInvoiceTrading_Engr), 1608 (SaleInvoiceQtyWithTax) and 1611 (frmSaleInvoiceReturn_Engr) are added to the supported set.
 * The SetData branches for them are the generic ones: UpdateInventoryReference (feature 5 off, or 1611), the FIFO branch (feature 5 on),
 * Sp_InventoryTransactions_GetALLMethod for 1608/1611 (DAL :302) and USP_InventoryValidation for 1608/1609 (not 1611, DAL :316).
 * Must run inside the caller's transaction.
 */
@Component
public class SaleInvoiceMfgEngrPersist {
    private final SaleInvoiceRepository repo;
    public SaleInvoiceMfgEngrPersist(SaleInvoiceRepository repo) { this.repo = repo; }

    public int persist(SaleInvoiceFinancialDirect.Invoice obj, SaleInvoiceFinancialDirect.Voucher voucher, String procName) {
        Head h = obj.h;
        if (obj.details.isEmpty()) throw new IllegalStateException("Detail List not found");
        if (h.Id == 0 && obj.details.stream().anyMatch(x -> x.Id > 0)) throw new IllegalStateException("Record cannot be inserted because detailId greater than zero");
        int num3 = repo.setProc(procName, h);
        if (num3 > 0) h.Id = num3; else num3 = h.Id;
        int num4 = 1;
        boolean flag2 = false;
        for (Detail d : obj.details) {
            if (d.RefRefDocumentTypeId > 0 && d.RefRefDocIdNo > 0 && d.RefDocSubId > 0) flag2 = true;
            d.LineId = num4;
            d.InvSaleInvoiceId = h.Id;
            d.BillAmount = d.ItemAmount + d.ExpenseAmount - d.CommissionAmount - d.FreightAmount;
            d.Id = repo.setProc("Sp_InvSaleInvoiceDetail_Insert", d);
            num4++;
        }
        for (Freight f : obj.freights) { f.InvSaleInvoiceId = h.Id; repo.setProc("Sp_InvSaleInvoiceFreight_Insert", f); }
        for (Journal j : obj.journals) { j.InvSaleInvoiceId = h.Id; repo.setProc("Sp_InvSaleInvoiceJournal_Insert", j); }
        for (Expense e : obj.expenses) { e.InvSaleInvoiceId = h.Id; repo.setProc("Sp_InvSaleInvoiceExpense_Insert", e); }
        for (Commission c : obj.commissions) { c.InvSaleInvoiceId = h.Id; repo.setProc("Sp_InvSaleInvoiceCommission_Insert", c); }
        int sort = 1;
        for (PaymentTerm p : obj.paymentTerms) { p.InvSaleInvoiceId = h.Id; p.SortNo = sort++; repo.setProc("USP_SaleInvoicePaymentTermsDetail_Insert", p); }

        int org = h.OrganizationId, company = h.CompanyId;
        final int doc = h.DocumentTypeId;
        Map<Integer, Map<String, Object>> itemGl = repo.itemGl(org, company);
        Map<Integer, Integer> jobLots = repo.jobLotAccounts(org, company);
        boolean num8 = !Set.of(103, 126, 133, 145).contains(doc) && featureOf(org, company, 5);
        boolean feature14 = featureOf(org, company, 14);
        boolean flag3 = truthy(repo.config(org, company, "InventoryFinancialsEffectsInActive"));
        boolean flag4 = !(doc == 99 && feature14);
        boolean num9 = num8 && !flag2 && doc != 1611 && doc != 1662;
        final boolean anyGdn = obj.details.stream().anyMatch(x -> x.InvGdnId > 0);
        final boolean anySaleOrder = obj.details.stream().anyMatch(x -> x.SaleOrderId > 0);
        if (!Set.of(95, 171, 99, 103, 126, 139, 145, 184, 186, 1608, 1609, 1611, 1660, 1661, 1662).contains(doc)) throw new UnsupportedOperationException("DocumentTypeId " + doc + " is not ported");
        List<StockDetail> stock = new ArrayList<>();
        if (num9) {
            boolean flag6 = doc == 99 && anyGdn;
            if (doc == 95 || doc == 171 || flag6) {
                /* DAL 0433:171 - 171: 170 when any line has a sale order else 221; 95: 86; 99: 124. */
                final int refType = doc == 171 ? (anySaleOrder ? 170 : 221) : doc == 95 ? 86 : 124;
                for (Detail item : obj.details) {
                    var refs = repo.stockByOtherIds(org, company, refType, item.InvGdnId, item.InvGdnDetailId);
                    if (refs.isEmpty()) throw new IllegalStateException("InventoryStockEvalautionDetailslist not found other reference.");
                    boolean hasJobLotAccount = jobLots.getOrDefault(item.JobLotId, 0) > 0;
                    for (var r : refs) {
                        StockDetail sd = stockDetail(r);
                        Map<String, Object> ig = itemGl.get(item.ItemId);
                        if (ig == null) throw new IllegalStateException("ItemId Not Found Against CGS Transaction");
                        if (!hasJobLotAccount && !flag3) {
                            String remarks = "ItemQty: " + SaleInvoiceFinancialDirect.g(sd.QtyOut) + " " + s(col(ig, "ItemName")) + " Net Weight: "
                                    + SaleInvoiceFinancialDirect.g(sd.StockWeightOut) + " CGS Rate: " + SaleInvoiceFinancialDirect.g(sd.CgsRate) + " " + obj.ids.CompanyName;
                            voucher.details.add(cgsDetail(item, sd, remarks, i(col(ig, "COGSGLAC")), i(col(ig, "PurchaseGLAC")), sd.CgsAmount, 0.0, h.SupplierCustomerId));
                            voucher.details.add(cgsDetail(item, sd, remarks, i(col(ig, "PurchaseGLAC")), i(col(ig, "COGSGLAC")), 0.0, sd.CgsAmount, h.SupplierCustomerId));
                        }
                        updateStockDetail(sd, item, obj, refType);
                        stock.add(sd);
                    }
                }
                for (StockDetail sd : stock) repo.setProc("USP_InventoryStockEvalautionDetailGdnReferences_Update", sd);
            } else if (doc != 222) {
                if (flag4) {
                    /* DAL 0433:219 - FIFO costing per line (CommonServices.FIFOImplemention), accumulating the reserved list. */
                    for (Detail item3 : obj.details) {
                        Map<String, Object> ig = itemGl.get(item3.ItemId);
                        if (ig == null) continue;
                        String itemName = s(col(ig, "ItemName"));
                        boolean flag8 = jobLots.getOrDefault(item3.JobLotId, 0) > 0;
                        for (StockDetail item11 : fifo(obj, item3, itemName, stock)) {
                            if (!flag8 && !flag3) {
                                String remarks2 = "ItemQty: " + SaleInvoiceFinancialDirect.g(item11.QtyOut) + " " + itemName + " Net Weight: "
                                        + SaleInvoiceFinancialDirect.g(item11.StockWeightOut) + " CGS Rate: " + SaleInvoiceFinancialDirect.g(item11.CgsRate) + " " + obj.ids.CompanyName;
                                voucher.details.add(cgsDetail(item3, item11, remarks2, i(col(ig, "COGSGLAC")), i(col(ig, "PurchaseGLAC")), item11.CgsAmount, 0.0, h.SupplierCustomerId));
                                voucher.details.add(cgsDetail(item3, item11, remarks2, i(col(ig, "PurchaseGLAC")), i(col(ig, "COGSGLAC")), 0.0, item11.CgsAmount, h.SupplierCustomerId));
                            }
                            if (doc == 126) { item11.ItemUom = item3.ItemUOMId; item11.JobLotId = item3.JobLotId; }
                            stock.add(item11);
                        }
                    }
                    if (!stock.isEmpty()) {
                        if (h.ModifyUser > 0)
                            repo.run("EXEC dbo.USP_InventoryQtyReverseAndDeleteByReferenceId @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefDocIdNo=?", org, company, doc, h.Id);
                        for (StockDetail sd : stock) {
                            prepareStockEvaluationDetail(sd, obj);
                            repo.setProc("USP_InventoryStockEvalautionDetail_Insert", sd);
                        }
                    }
                } else if (h.ModifyUser > 0) {
                    repo.run("EXEC dbo.USP_InventoryQtyReverseAndDeleteByReferenceId @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefDocIdNo=?", org, company, doc, h.Id);
                }
            }
        } else {
            boolean flag9 = (doc == 99 || doc == 145 || doc == 126) && anyGdn;
            if (doc != 95 && doc != 171 && doc != 222 && doc != 1660 && !flag9) {
                /* DAL 0433:725 UpdateInventoryReference. */
                StockDetail sd = new StockDetail();
                sd.OrganizationId = org; sd.CompanyId = company; sd.RefDocumentTypeId = doc; sd.RefDocIdNo = h.Id;
                repo.setProc("Sp_InventoryStockEvalautionDetail_Update", sd);
            } else {
                final int refType = doc == 171 ? (anySaleOrder ? 170 : 221) : doc == 1660 ? 1659 : flag9 ? (doc == 145 ? 144 : doc == 126 ? 149 : 124) : 86;
                for (Detail item : obj.details) {
                    var refs = repo.stockByOtherIds(org, company, refType, item.InvGdnId, item.InvGdnDetailId);
                    if (refs.isEmpty()) throw new IllegalStateException("InventoryStockEvalautionDetailslist not found other reference.");
                    for (var r : refs) { StockDetail sd = stockDetail(r); prepareGdnReferenceStockDetail(sd, item, obj, refType); stock.add(sd); }
                }
                for (StockDetail sd : stock) repo.setProc("USP_InventoryStockEvalautionDetailGdnReferences_Update", sd);
            }
        }
        repo.run("EXEC dbo.usp_StockInTransit_VoucherDelete_ByGdnId @Id=?", h.Id);
        if (Set.of(99, 103, 60, 184, 126, 133, 139, 1608, 1661, 1611, 186, 1809, 222, 1662).contains(doc))
            repo.run("EXEC dbo.Sp_InventoryTransactions_GetALLMethod @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefDocIdNo=?", org, company, doc, h.Id);
        /* DAL :315 - flag10 = 126 with GDN lines. */
        if (doc != 95 && doc != 1611 && doc != 1662 && doc != 1660 && !(doc == 126 && anyGdn) && flag4) {
            for (Detail d : obj.details)
                repo.inventoryValidation(org, company, doc, h.DocDate, d.ItemId, d.WarehouseId, d.JobLotId, d.CropYear,
                        d.PackingTypeId, d.ItemUOMId, d.NetStockWeight, d.RefRefDocumentTypeId, d.RefRefDocIdNo, d.RefDocSubId, d.ItemConditionId);
        }

        VoucherHead vh = voucher.head;
        int existing = repo.voucherHeadId(org, company, h.DocumentTypeId, h.Id);
        if (existing > 0) vh.Id = existing;
        vh.DocumentTypeSrNo = h.Id;
        vh.RefDocNoId = h.Id;
        int num = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        if (num > 0) vh.Id = num;
        for (VoucherDetail vd : voucher.details) {
            vd.VoucherHeadId = vh.Id;
            vd.BranchesId = h.BranchesId;
            for (Detail d : obj.details) if (d.LineId == vd.LineId && d.LineId > 0 && vd.LineId > 0) { vd.RefDocSubIdNo = d.Id; break; }
            repo.setProc("Sp_VoucherDetail_Insert", vd);
        }
        if (voucher.details.isEmpty()) throw new IllegalStateException("Voucher Detail list Not Found");
        repo.run("EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", org, company, vh.Id);
        int docTypeRef = repo.setProc("Sp_VoucherHead_H_Insert", vh);
        for (VoucherDetail vd : voucher.details) {
            vd.VoucherHeadId = vh.Id;
            vd.DocumentTypeIdRef = docTypeRef;
            repo.setProc("Sp_VoucherDetail_H_Insert", vd);
        }
        ApprovalDetail a = new ApprovalDetail();
        a.OrganizationId = org; a.CompanyId = company; a.DocumentTypeId = h.DocumentTypeId; a.Id = h.Id;
        a.LimitAmount = BigDecimal.valueOf(h.BillAmount);
        repo.setProc("[DAW].[USp_DocumentApprovalDetail_Insert]", a);
        return num3;
    }

    private boolean featureOf(int org, int company, int id) {
        return repo.q("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", org, company).stream().anyMatch(r -> i(col(r, "Id")) == id);
    }

    /** Conversion.ConvertDataTableToList<InventoryStockEvalautionDetail> - columns matched to properties by name. */
    private static StockDetail stockDetail(Map<String, Object> r) {
        StockDetail x = new StockDetail();
        for (var f : StockDetail.class.getFields()) {
            Object v = col(r, f.getName());
            if (v == null) continue;
            try {
                Class<?> t = f.getType();
                if (t == int.class) f.setInt(x, i(v));
                else if (t == double.class) f.setDouble(x, d(v));
                else if (t == boolean.class) f.setBoolean(x, b(v));
                else if (t == String.class) f.set(x, String.valueOf(v));
                else if (t == LocalDateTime.class) f.set(x, dt(v));
                else if (t == BigDecimal.class) f.set(x, dec(v));
            } catch (IllegalAccessException ignored) { }
        }
        return x;
    }

    /** DAL 0433 UpdateStockDetail (:599). */
    private void updateStockDetail(StockDetail sd, Detail item, SaleInvoiceFinancialDirect.Invoice obj, int refType) {
        Head h = obj.h;
        sd.OrganizationId = h.OrganizationId; sd.CompanyId = h.CompanyId; sd.DocDate = h.DocDate; sd.DocCodeNo = h.DocNo;
        sd.SupplierCustomerId = h.SupplierCustomerId; sd.RefDocumentTypeId = h.DocumentTypeId; sd.BranchesId = h.BranchesId;
        sd.EntryUser = h.EntryUser; sd.ModifyUser = h.ModifyUser; sd.CalcType = "Weight";
        Detail d = obj.details.stream().filter(x -> x.InvGdnId == sd.OtherDocNoId && x.InvGdnDetailId == sd.OtherSubDocNoId && sd.OtherDocumentTypeId == refType)
                .findFirst().orElse(null);
        if (d == null) return;
        double eq = repo.equivalent(h.OrganizationId, h.CompanyId, item.ItemId, d.UomScheduleIdRate);
        if (sd.BillWeightOut > 0.0 && d.ItemRate > 0.0 && eq > 0.0) {
            sd.AmountOut = sd.BillWeightOut / eq * d.ItemRate;
            double num = item.ExpenseAmount > 0 && item.ItemQty > 0 && sd.QtyOut > 0 ? item.ExpenseAmount / item.ItemQty * sd.QtyOut : 0.0;
            double num2 = item.FreightAmount > 0 && item.NetBillWeight > 0 && sd.BillWeightOut > 0 ? item.FreightAmount / item.NetBillWeight * sd.BillWeightOut : 0.0;
            double num3 = item.CommissionAmount > 0 && item.ItemAmount > 0 && sd.AmountOut > 0 ? item.CommissionAmount / item.ItemAmount * sd.AmountOut : 0.0;
            sd.AmountOut += num - num2 - num3;
        }
        sd.ItemRate = d.ItemRate;
        sd.RefDocIdNo = d.InvSaleInvoiceId;
        sd.RefDocSubIdNo = d.Id;
    }

    /** DAL 0433 PrepareGdnReferenceStockDetail (:735). */
    private void prepareGdnReferenceStockDetail(StockDetail sd, Detail item, SaleInvoiceFinancialDirect.Invoice obj, int refType) {
        Head h = obj.h;
        sd.OrganizationId = h.OrganizationId; sd.CompanyId = h.CompanyId; sd.DocDate = h.DocDate; sd.DocCodeNo = h.DocNo;
        sd.SupplierCustomerId = h.SupplierCustomerId; sd.BranchesId = h.BranchesId; sd.RefDocumentTypeId = h.DocumentTypeId;
        sd.EntryUser = h.EntryUser; sd.ModifyUser = h.ModifyUser; sd.CalcType = "Weight";
        Detail d = obj.details.stream().filter(x -> x.InvGdnId == sd.OtherDocNoId && x.InvGdnDetailId == sd.OtherSubDocNoId && sd.OtherDocumentTypeId == refType)
                .findFirst().orElse(null);
        if (d == null) return;
        double eq = repo.equivalent(h.OrganizationId, h.CompanyId, item.ItemId, d.UomScheduleIdRate);
        if (sd.BillWeightOut > 0.0 && d.ItemRate > 0.0 && eq > 0.0) {
            sd.RateUom = d.UomScheduleIdRate;
            sd.AmountOut = d.BillAmount;
            sd.CgsAmount = d.ItemCgsRate > 0.0 ? (h.DocumentTypeId == 126 || h.DocumentTypeId == 103 ? sd.QtyOut * d.ItemCgsRate : sd.StockWeightOut / eq * d.ItemCgsRate) : sd.AmountOut;
        }
        sd.ItemRate = d.ItemRate;
        sd.CgsRate = d.ItemCgsRate > 0.0 ? d.ItemCgsRate : d.ItemRate;
        sd.RefDocIdNo = d.InvSaleInvoiceId;
        sd.RefDocSubIdNo = d.Id;
    }

    /** DAL 0433 CreateCgsVoucherDetail (:565). */
    private static VoucherDetail cgsDetail(Detail item, StockDetail sd, String remarks, int accountId, int againstAccountId,
                                           double debit, double credit, int party) {
        VoucherDetail x = new VoucherDetail();
        x.LineId = item.LineId; x.IsCGS = 1; x.AccountId = accountId; x.AgainstAccountId = againstAccountId; x.Comments = remarks;
        x.DebitAmount = debit; x.CreditAmount = credit;
        x.RefDocumentTypeId = sd.RefRefDocumentTypeId; x.RefDocNoId = sd.RefRefDocIdNo; x.RefDocNoDetailId = sd.RefRefDocSubIdNo;
        x.ItemId = item.ItemId; x.QtyOut = sd.QtyOut; x.WeightOut = sd.StockWeightOut; x.ItemCgsRate = sd.CgsRate;
        x.RateCut = item.RateCut; x.RateCutAmount = item.RateCutAmount; x.ItemAmount = sd.AmountOut; x.Expenses = item.ExpenseAmount;
        x.Commission = item.CommissionAmount; x.Freight = item.FreightAmount; x.OrderNo = item.SaleOrder; x.GpNo = item.GpNo;
        x.VehicleNo = item.VehicleNo; x.JobLotId = item.JobLotId; x.SupplierCustomerId = party; x.BranchesId = item.BranchId;
        x.CostCenterId = item.CostCenterId;
        return x;
    }

    /** DAL 0433 PrepareStockEvaluationDetail (:635). */
    private void prepareStockEvaluationDetail(StockDetail sd, SaleInvoiceFinancialDirect.Invoice obj) {
        Head h = obj.h;
        sd.OrganizationId = h.OrganizationId; sd.CompanyId = h.CompanyId; sd.DocDate = h.DocDate; sd.DocCodeNo = h.DocNo;
        sd.SupplierCustomerId = h.SupplierCustomerId; sd.BranchesId = h.BranchesId; sd.RefDocumentTypeId = h.DocumentTypeId;
        sd.EntryUser = h.EntryUser; sd.ModifyUser = h.ModifyUser; sd.CalcType = "Weight";
        Detail d = obj.details.stream().filter(x -> x.LineId == sd.LineId && sd.LineId > 0).findFirst().orElse(null);
        if (d == null) return;
        double eq = repo.equivalent(h.OrganizationId, h.CompanyId, sd.ItemId, d.UomScheduleIdRate);
        if (sd.BillWeightOut > 0.0 && d.ItemRate > 0.0 && eq > 0.0) {
            sd.AmountOut = sd.BillWeightOut / eq * d.ItemRate;
            if (h.DocumentTypeId == 184) sd.AmountOut -= d.ItemDiscountAmount;
            double num = d.ExpenseAmount > 0.0 && d.ItemQty > 0.0 && sd.QtyOut > 0.0 ? d.ExpenseAmount / d.ItemQty * sd.QtyOut : 0.0;
            double num2 = d.FreightAmount > 0.0 && d.NetBillWeight > 0.0 && sd.BillWeightOut > 0.0 ? d.FreightAmount / d.NetBillWeight * sd.BillWeightOut : 0.0;
            double num3 = d.CommissionAmount > 0.0 && d.ItemAmount > 0.0 && sd.AmountOut > 0.0 ? d.CommissionAmount / d.ItemAmount * sd.AmountOut : 0.0;
            sd.AmountOut += num - num2 - num3;
        }
        sd.ItemRate = d.ItemRate;
        sd.RefDocIdNo = d.InvSaleInvoiceId;
        sd.RefDocSubIdNo = d.Id;
    }

    /** DAL 0433 CreateStockReportParameter (:778) + DAL 0205 CommonServices.FIFOImplemention. */
    private List<StockDetail> fifo(SaleInvoiceFinancialDirect.Invoice obj, Detail item, String itemName, List<StockDetail> reserved) {
        Head h = obj.h;
        int org = h.OrganizationId, company = h.CompanyId, doc = h.DocumentTypeId;
        LocalDateTime docDate = doc == 99 ? h.SupplierInvoiceDate : h.DocDate;
        int stockUom = 0, jobLot = 0, packing = 0;
        String cropYear = null;
        if (doc != 126 && doc != 145) { stockUom = item.ItemUOMId; jobLot = item.JobLotId; cropYear = item.CropYear; packing = item.PackingTypeId; }
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_GetStockByFifoMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @DocDate=?");
        List<Object> args = new ArrayList<>(Arrays.asList(org, company, item.ItemId, java.sql.Timestamp.valueOf(docDate)));
        if (stockUom != 0) { sql.append(", @PackUomId=?"); args.add(stockUom); }
        if (item.WarehouseId != 0) { sql.append(", @WarehouseId=?"); args.add(item.WarehouseId); }
        if (jobLot != 0) { sql.append(", @JobLotId=?"); args.add(jobLot); }
        if (packing != 0) { sql.append(", @PackingTypeId=?"); args.add(packing); }
        if (cropYear != null && !cropYear.isEmpty()) { sql.append(", @CropYear=?"); args.add(cropYear); }
        if (h.ModifyUser > 0) { sql.append(", @DocumentTypeId=?"); args.add(doc); if (h.Id != 0) { sql.append(", @Id=?"); args.add(h.Id); } }
        if (!reserved.isEmpty()) {
            StringBuilder xml = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
            for (StockDetail r : reserved)
                xml.append("<FIFOStockEvaluation><RefDocumentTypeId>").append(r.RefRefDocumentTypeId).append("</RefDocumentTypeId><RefDocIdNo>").append(r.RefRefDocIdNo)
                        .append("</RefDocIdNo><RefDocSubIdNo>").append(r.RefRefDocSubIdNo).append("</RefDocSubIdNo><ReserveQty>").append(plain(r.QtyOut))
                        .append("</ReserveQty><ReserveWeight>").append(plain(r.StockWeightOut)).append("</ReserveWeight></FIFOStockEvaluation>");
            sql.append(", @FIFOXML=?"); args.add(xml.append("</ArrayOfFIFOStockEvaluation>").toString());
        }
        var layers = repo.q(sql.toString(), args.toArray());
        if (layers.isEmpty()) throw new IllegalStateException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0;
        for (var l : layers) available += d(col(l, "NetBalWeight"));
        double netWeight = item.NetStockWeight, itemQty = item.ItemQty;
        /* .NET Math.Round(double, 2) is midpoint-to-even. */
        double roundedAvailable = Math.abs(available) < 1e16 ? Math.rint(available * 100d) / 100d : available;
        if (!(netWeight <= roundedAvailable))
            throw new IllegalStateException("Weight available is " + SaleInvoiceFinancialDirect.g(available) + " and row Weight is " + SaleInvoiceFinancialDirect.g(netWeight) + " this item " + itemName + " against FIFO....");
        List<StockDetail> out = new ArrayList<>();
        double num5 = 0, num7 = 0;
        for (var l : layers) {
            double avgRate = d(col(l, "AvgRate"));
            int rateUomId = i(col(l, "RateUomId"));
            if (avgRate <= 0.0) throw new IllegalStateException("Rate Not Found this Item " + itemName + " against FIFO Method");
            if (rateUomId == 0) throw new IllegalStateException("RateUomId not found  this " + itemName + " against FIFO Method");
            double num6 = d(col(l, "NetBalWeight")), num4 = d(col(l, "NetBalQty"));
            double eq = repo.equivalent(org, company, item.ItemId, rateUomId);
            if (eq == 0.0) throw new IllegalStateException("RateUom Not Found");
            StockDetail sd = new StockDetail();
            sd.Id = i(col(l, "Id")); sd.LineId = item.LineId; sd.ItemId = item.ItemId; sd.WarehouseId = item.WarehouseId; sd.RateUom = rateUomId;
            sd.JobLotId = jobLot; sd.InvPackingTypeId = packing; sd.ItemUom = stockUom; sd.CropBatch = cropYear == null ? "" : cropYear;
            sd.RefRefDocumentTypeId = i(col(l, "RefDocumentTypeId")); sd.RefRefDocIdNo = i(col(l, "RefDocIdNo")); sd.RefRefDocSubIdNo = i(col(l, "RefDocSubIdNo"));
            sd.CgsRate = avgRate * eq;
            if (num6 <= netWeight - num7) {
                num7 += num6; num5 += num4;
                sd.QtyOut = num4; sd.BillWeightOut = num6; sd.StockWeightOut = num6;
                sd.CgsAmount = sd.BillWeightOut / eq * sd.CgsRate;
                out.add(sd);
            } else if (num6 >= netWeight - num7) {
                sd.QtyOut = itemQty - num5; sd.BillWeightOut = netWeight - num7; sd.StockWeightOut = netWeight - num7;
                sd.CgsAmount = sd.BillWeightOut / eq * sd.CgsRate;
                num5 += sd.QtyOut; num7 += sd.BillWeightOut;
                out.add(sd);
            }
            if (netWeight == num7) break;
        }
        return out;
    }

    private static String plain(double v) { return java.math.BigDecimal.valueOf(v).toPlainString(); }
}
