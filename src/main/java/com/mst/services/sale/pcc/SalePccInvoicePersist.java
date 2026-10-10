package com.mst.services.sale.pcc;

import com.mst.models.saleinvoice.SaleInvoiceModels.ApprovalDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.StockDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherHead;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.services.sale.pcc.SalePccInvoiceModels.*;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Architecture.DAL.pcc.InvSaleInvoice.SetData (0285) for the Sale Pcc invoices 1856 / 1861 / 1862: header, details, freight, journal, expense and
 * wages rows, the inventory evaluation (GDN references, FIFO for concrete, UpdateInventoryReference), Sp_InventoryTransactions_GetALLMethod, the
 * stock validation, then the voucher (head, details, balance check, history copy) and the approval limit. Runs inside the caller's transaction
 * (the desktop's single SqlTransaction); attachments are written by the caller (SalePccAttachments).
 */
@Component
public class SalePccInvoicePersist {
    private final SaleInvoiceRepository repo;

    public SalePccInvoicePersist(SaleInvoiceRepository repo) { this.repo = repo; }

    private boolean featureOf(int org, int company, int id) {
        return repo.q("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", org, company).stream().anyMatch(r -> i(col(r, "Id")) == id);
    }

    /** @return the invoice id (SetData's return value). */
    public int persist(Invoice obj, String procName) {
        Head h = obj.h;
        int org = h.OrganizationId, company = h.CompanyId;
        final int doc = h.DocumentTypeId;
        int num3 = repo.setProc(procName, h);
        if (num3 > 0) h.Id = num3; else num3 = h.Id;
        int line = 1;
        int num5 = 0;
        for (Detail d : obj.details) {
            d.LineId = line;
            d.InvSaleInvoiceId = h.Id;
            num5 = d.InvGdnId;
            d.ItemNetAmount = d.ItemAmountWithDisc.add(d.ExpenseAmount).add(d.WagesAmount).subtract(d.CommissionAmount);
            d.Id = repo.setProc("pcc.USP_InvSaleInvoiceDetail_Insert", d);
            line++;
        }
        for (Freight f : obj.freights) { f.InvSaleInvoiceId = h.Id; repo.setProc("pcc.USP_InvSaleInvoiceFreight_Insert", f); }
        for (Journal j : obj.journals) { j.InvSaleInvoiceId = h.Id; repo.setProc("pcc.USP_InvSaleInvoiceJournal_Insert", j); }
        for (Expense e : obj.expenses) { e.InvSaleInvoiceId = h.Id; repo.setProc("pcc.USP_InvSaleInvoiceExpense_Insert", e); }
        for (Wages w : obj.wages) { w.InvSaleInvoiceId = h.Id; repo.setProc("pcc.USP_InvSaleInvoiceWagesDetail_Insert", w); }

        Map<Integer, Map<String, Object>> itemGl = repo.itemGl(org, company);
        int otherDocumentTypeId = 0;
        List<VoucherDetail> vds = obj.voucherDetails;
        List<StockDetail> stock = new ArrayList<>();
        if (featureOf(org, company, 5) && doc != 1862) {
            if (num5 > 0) {
                otherDocumentTypeId = doc == 1861 ? 1855 : 1866;
                for (Detail item : obj.details) {
                    var refs = repo.stockByOtherIds(org, company, otherDocumentTypeId, item.InvGdnId, item.InvGdnDetailId);
                    if (refs.isEmpty()) throw new IllegalStateException("InventoryStockEvalautionDetailslist not found against CGS other reference");
                    for (var r : refs) {
                        StockDetail sd = stockDetail(r);
                        Map<String, Object> ig = itemGl.get(item.ItemId);
                        if (ig == null) throw new IllegalStateException("ItemId Not Found Against CGS Transaction");
                        String text = s(col(ig, "ItemName")) + " Qty: " + SalePccInvoiceFinancialFormat.g(sd.QtyOut) + " Attribute Varient  " + item.VarientEquivalent.toPlainString()
                                + " CGS Rate: " + SalePccInvoiceFinancialFormat.g(sd.CgsRate);
                        addCgs(vds, item, sd, text + "  " + obj.companyName, ig, h);
                        sd.OrganizationId = org; sd.CompanyId = company; sd.DocDate = h.DocDate; sd.DocCodeNo = h.DocNo;
                        sd.SupplierCustomerId = h.SupplierCustomerId; sd.BranchesId = h.BranchesId; sd.RefDocumentTypeId = doc;
                        sd.EntryUser = h.EntryUserId; sd.ModifyUser = h.ModifyUserId; sd.CalcType = "Qty";
                        final int refType = otherDocumentTypeId;
                        Detail m = obj.details.stream().filter(x -> x.InvGdnId == sd.OtherDocNoId && x.InvGdnDetailId == sd.OtherSubDocNoId && sd.OtherDocumentTypeId == refType)
                                .findFirst().orElse(null);
                        if (m != null) {
                            if (sd.QtyOut > 0.0 && m.ItemRate.signum() > 0) sd.AmountOut = sd.QtyOut * m.ItemRate.doubleValue();
                            sd.ItemRate = m.ItemRate.doubleValue();
                            sd.RefDocIdNo = m.InvSaleInvoiceId;
                            sd.RefDocSubIdNo = m.Id;
                        }
                        stock.add(sd);
                    }
                }
                for (StockDetail sd : stock) repo.setProc("USP_InventoryStockEvalautionDetailGdnReferences_Update", sd);
            } else {
                for (Detail item : obj.details) {
                    Map<String, Object> ig = itemGl.get(item.ItemId);
                    if (ig == null) continue;
                    String itemName = s(col(ig, "ItemName"));
                    for (StockDetail sd : fifoConcrete(obj, item, itemName, stock)) {
                        String text = itemName + " Qty: " + SalePccInvoiceFinancialFormat.g(sd.QtyOut) + " Attribute Varient  " + item.VarientEquivalent.toPlainString()
                                + " CGS Rate: " + SalePccInvoiceFinancialFormat.g(sd.CgsRate);
                        addCgs(vds, item, sd, text + "  " + obj.companyName, ig, h);
                        sd.VarientId = item.ItemAttributeVarientId;
                        sd.ItemUom = item.ItemUomId;
                        sd.JobLotId = item.JobLotId;
                        stock.add(sd);
                    }
                }
                if (!stock.isEmpty()) {
                    if (h.ModifyUserId > 0)
                        repo.run("EXEC [dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId] @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefDocIdNo=?", org, company, doc, h.Id);
                    for (StockDetail sd : stock) {
                        sd.OrganizationId = org; sd.CompanyId = company; sd.DocDate = h.DocDate; sd.DocCodeNo = h.DocNo;
                        sd.SupplierCustomerId = h.SupplierCustomerId; sd.BranchesId = h.BranchesId; sd.RefDocumentTypeId = doc;
                        sd.EntryUser = h.EntryUserId; sd.ModifyUser = h.ModifyUserId; sd.CalcType = "Qty";
                        Detail m = obj.details.stream().filter(x -> x.LineId == sd.LineId && x.LineId > 0 && sd.LineId > 0).findFirst().orElse(null);
                        if (m != null) {
                            if (sd.QtyOut > 0.0 && m.ItemRate.signum() > 0) sd.AmountOut = sd.QtyOut * m.ItemRate.doubleValue();
                            sd.ItemRate = m.ItemRate.doubleValue();
                            sd.RefDocIdNo = m.InvSaleInvoiceId;
                            sd.RefDocSubIdNo = m.Id;
                        }
                        repo.setProc("USP_InventoryStockEvalautionDetail_Insert", sd);
                    }
                }
            }
        } else if (num5 > 0) {
            /* feature 5 off (or 1862): the GDN's evaluation rows are referenced; OtherDocumentTypeId is still 0 here, as in the desktop. */
            for (Detail item : obj.details) {
                var refs = repo.stockByOtherIds(org, company, otherDocumentTypeId, item.InvGdnId, item.InvGdnDetailId);
                if (refs.isEmpty()) throw new IllegalStateException("InventoryStockEvalautionDetailslist not found other reference");
                for (var r : refs) {
                    StockDetail sd = stockDetail(r);
                    sd.OrganizationId = org; sd.CompanyId = company; sd.DocDate = h.DocDate; sd.DocCodeNo = h.DocNo;
                    sd.SupplierCustomerId = h.SupplierCustomerId; sd.BranchesId = h.BranchesId; sd.RefDocumentTypeId = doc;
                    sd.EntryUser = h.EntryUserId; sd.ModifyUser = h.ModifyUserId; sd.CalcType = "Qty";
                    Detail m = obj.details.stream().filter(x -> x.InvGdnId == sd.OtherDocNoId && x.InvGdnDetailId == sd.OtherSubDocNoId && sd.OtherDocumentTypeId == 1855)
                            .findFirst().orElse(null);
                    if (m != null) {
                        if (sd.QtyOut > 0.0 && m.ItemRate.signum() > 0) { sd.AmountOut = sd.QtyOut * m.ItemRate.doubleValue(); sd.CgsAmount = sd.AmountOut; }
                        sd.ItemRate = m.ItemRate.doubleValue();
                        sd.CgsRate = sd.ItemRate;
                        sd.RefDocIdNo = m.InvSaleInvoiceId;
                        sd.RefDocSubIdNo = m.Id;
                    }
                    stock.add(sd);
                }
            }
            for (StockDetail sd : stock) repo.setProc("USP_InventoryStockEvalautionDetailGdnReferences_Update", sd);
        } else {
            StockDetail sd = new StockDetail();                                     // UpdateInventoryReference
            sd.OrganizationId = org; sd.CompanyId = company; sd.RefDocumentTypeId = doc; sd.RefDocIdNo = h.Id;
            repo.setProc("Sp_InventoryStockEvalautionDetail_Update", sd);
        }
        if (doc == 1856 || doc == 1862)
            repo.run("EXEC dbo.Sp_InventoryTransactions_GetALLMethod @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefDocIdNo=?", org, company, doc, h.Id);
        if (doc == 1856 && num5 == 0) {
            for (Detail d : obj.details) {
                if (d.ItemAttributeVarientId > 0)
                    repo.run("EXEC dbo.USP_StockValidationConcrete @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @DocDate=?, @ItemId=?, @WarehouseId=?, @VarientId=?, @Qty=?",
                            org, company, doc, java.sql.Timestamp.valueOf(h.DocDate), d.ItemId, d.WarehouseId, d.ItemAttributeVarientId, d.ItemQty);
                else
                    repo.run("EXEC dbo.USP_StockValidationConcrete @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @DocDate=?, @ItemId=?, @WarehouseId=?, @Qty=?",
                            org, company, doc, java.sql.Timestamp.valueOf(h.DocDate), d.ItemId, d.WarehouseId, d.ItemQty);
            }
        }

        VoucherHead vh = obj.voucherHead;
        int existing = repo.voucherHeadId(org, company, doc, h.Id);
        if (existing > 0) vh.Id = existing;
        vh.DocumentTypeSrNo = h.Id;
        vh.RefDocNoId = h.Id;
        int num = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        if (num > 0) vh.Id = num;
        for (VoucherDetail vd : vds) {
            vd.VoucherHeadId = vh.Id;
            for (Detail d : obj.details) if (d.LineId == vd.LineId && d.LineId > 0 && vd.LineId > 0) { vd.RefDocSubIdNo = d.Id; break; }
            repo.setProc("Sp_VoucherDetail_Insert", vd);
        }
        if (vds.isEmpty()) throw new IllegalStateException("Voucher Detail list Not Found");
        repo.run("EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", org, company, vh.Id);
        int docTypeRef = repo.setProc("Sp_VoucherHead_H_Insert", vh);
        for (VoucherDetail vd : vds) {
            vd.VoucherHeadId = vh.Id;
            vd.DocumentTypeIdRef = docTypeRef;
            repo.setProc("Sp_VoucherDetail_H_Insert", vd);
        }
        ApprovalDetail a = new ApprovalDetail();
        a.OrganizationId = org; a.CompanyId = company; a.DocumentTypeId = doc; a.Id = h.Id;
        a.LimitAmount = h.BillAmount;
        repo.setProc("[DAW].[USp_DocumentApprovalDetail_Insert]", a);
        return num3;
    }

    /** The two CGS voucher rows of a stock row (debit COGS / credit stock). */
    private static void addCgs(List<VoucherDetail> vds, Detail item, StockDetail sd, String comment, Map<String, Object> ig, Head h) {
        for (int k = 0; k < 2; k++) {
            VoucherDetail v = new VoucherDetail();
            v.LineId = item.LineId; v.IsCGS = 1;
            v.AccountId = k == 0 ? i(col(ig, "COGSGLAC")) : i(col(ig, "PurchaseGLAC"));
            v.AgainstAccountId = k == 0 ? i(col(ig, "PurchaseGLAC")) : i(col(ig, "COGSGLAC"));
            v.Comments = comment;
            if (k == 0) v.DebitAmount = sd.CgsAmount; else v.CreditAmount = sd.CgsAmount;
            v.ItemId = item.ItemId; v.QtyOut = sd.QtyOut; v.WeightOut = sd.StockWeightOut; v.ItemCgsRate = sd.CgsRate;
            v.RateCut = item.RateCut.doubleValue(); v.RateCutAmount = item.RateCutAmount.doubleValue(); v.ItemAmount = sd.AmountOut;
            v.Expenses = item.ExpenseAmount.doubleValue(); v.Commission = item.CommissionAmount.doubleValue(); v.Freight = item.FreightAmount.doubleValue();
            v.GpNo = item.GpNo; v.VehicleNo = item.VehicleNo; v.JobLotId = item.JobLotId; v.SupplierCustomerId = h.SupplierCustomerId;
            vds.add(v);
        }
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

    /** CommonServices.FIFOImplementionForConcrete (DAL 0205:1148): quantity based FIFO over [pcc].[USP_GetStockByFifoMethod]. */
    private List<StockDetail> fifoConcrete(Invoice obj, Detail item, String itemName, List<StockDetail> reserved) {
        Head h = obj.h;
        int org = h.OrganizationId, company = h.CompanyId;
        StringBuilder sql = new StringBuilder("EXEC [pcc].[USP_GetStockByFifoMethod] @OrganizationId=?, @CompanyId=?, @ItemId=?, @DocDate=?");
        List<Object> args = new ArrayList<>(Arrays.asList(org, company, item.ItemId, java.sql.Timestamp.valueOf(h.DocDate)));
        if (item.ItemAttributeVarientId != 0) { sql.append(", @VarientId=?"); args.add(item.ItemAttributeVarientId); }
        if (item.WarehouseId != 0) { sql.append(", @WarehouseId=?"); args.add(item.WarehouseId); }
        /* ReportsParameters.JobLotId is never set by the DAL, so no @JobLotId is sent (the lot is copied onto the rows afterwards). */
        if (h.ModifyUserId > 0) {                                                   // obj.DocumentTypeId / obj.Id are only set when the invoice is being modified
            sql.append(", @DocumentTypeId=?"); args.add(h.DocumentTypeId);
            if (h.Id != 0) { sql.append(", @Id=?"); args.add(h.Id); }
        }
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
        for (var l : layers) available += d(col(l, "NetBalQty"));
        double itemQty = item.ItemQty.doubleValue();
        if (!(itemQty <= available))
            throw new IllegalStateException("Qty available is " + com.mst.services.SaleInvoiceFinancialDirect.g(available) + " and row Qty is " + com.mst.services.SaleInvoiceFinancialDirect.g(itemQty)
                    + " this item " + itemName + " against FIFO....");
        List<StockDetail> out = new ArrayList<>();
        double num4 = 0;
        for (var l : layers) {
            double avgRate = d(col(l, "AvgRate"));
            if (avgRate <= 0.0) throw new IllegalStateException("Rate Not Found this Item " + itemName + " against FIFO Method");
            double num3 = d(col(l, "NetBalQty"));
            StockDetail sd = new StockDetail();
            sd.Id = i(col(l, "Id")); sd.LineId = item.LineId; sd.ItemId = item.ItemId; sd.WarehouseId = item.WarehouseId;
            sd.JobLotId = item.JobLotId; sd.InvPackingTypeId = 0; sd.ItemUom = item.ItemAttributeVarientId; sd.VarientId = item.ItemAttributeVarientId; sd.CropBatch = "";
            sd.RefRefDocumentTypeId = i(col(l, "RefDocumentTypeId")); sd.RefRefDocIdNo = i(col(l, "RefDocIdNo")); sd.RefRefDocSubIdNo = i(col(l, "RefDocSubIdNo"));
            sd.CgsRate = avgRate;
            if (num3 <= itemQty - num4) {
                sd.RateUom = i(col(l, "RateUomId"));
                sd.QtyOut = num3; sd.BillWeightOut = sd.QtyOut; sd.StockWeightOut = sd.QtyOut;
                sd.CgsAmount = sd.QtyOut * sd.CgsRate;
                num4 += num3;
                out.add(sd);
            } else if (num3 >= itemQty - num4) {
                sd.QtyOut = itemQty - num4; sd.BillWeightOut = sd.QtyOut; sd.StockWeightOut = sd.QtyOut;
                sd.RateUom = i(col(l, "RateUomId"));
                sd.CgsAmount = sd.QtyOut * sd.CgsRate;
                num4 += sd.QtyOut;
                out.add(sd);
            }
            if (itemQty == num4) break;
        }
        return out;
    }

    private static String plain(double v) { return BigDecimal.valueOf(v).toPlainString(); }
}
