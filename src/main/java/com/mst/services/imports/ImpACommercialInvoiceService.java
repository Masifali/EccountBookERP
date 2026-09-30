package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.models.imports.ImpADtos;
import com.mst.repositories.imports.ImpARepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.imports.ImpASupport.*;

/**
 * BLL of screen 221 "IMPORT INVOICE" - Architecture.WinApp.Import/ImCommercialInvoice.cs (BLL 0418 ImInvoice, 0415
 * ExImLcOrderPurchaseOrder, 0135 ImportReports). The form is dual-mode through its public DocumentTypeId: 233 = against an
 * Import Purchase Order (default here), 237 = against an Import Contract (?mode=237).
 *
 * Two procedures the desktop calls do NOT exist in the database (procdure.utf8.sql / procdure_index.csv):
 *   Sp_ImInvoice_GetAllMethod        - GenerateCode (Doc No), GetData (History tab) and GetByID (edit)
 *   Sp_ImInvoicePackingDetail_Insert - the detail rows of ImInvoice.Save (DAL 0475 SetDate)
 * so Doc No, History / Edit and Save report that on the action instead of calling an invented procedure. Everything else
 * (the combos, the order / contract loading, the calculations, the validation, the 806 print) is ported.
 */
@Service
public class ImpACommercialInvoiceService {

    public static final int SCREEN = 221;
    public static final String MISSING_READ =
            "Procedure Sp_ImInvoice_GetAllMethod (ImInvoice.GenerateCode / GetData / GetByID) does not exist in the database.";
    public static final String MISSING_SAVE =
            "Save is not possible: ImInvoice.Save writes the detail rows through procedure Sp_ImInvoicePackingDetail_Insert, which does not exist in the database.";

    @Autowired private ImpASupport sup;
    @Autowired private ImpARepository repo;

    private HrmSupport hrm() { return sup.hrm(); }

    /** DocumentTypeId of the page: 237 when asked for, else 233. */
    public static int docType(Integer mode) { return mode != null && mode == 237 ? 237 : 233; }

    // ================================================================== load

    /**
     * ImProformaInvoice_Load: lblPoNo "Purchase Order" (233, home currency read-only) or "Contract No" (237, + warehouses /
     * job lots for the detail grid value lists); AccountsFill, generateCode (reports the missing procedure), branches, projects,
     * CustomerGetAll, DeliveryTermFill, PaymentTermsFill, LoadingPortFill, MultiCurrencyfill, ImporterandExportBankFill, CarierType.
     */
    public Map<String, Object> setup(Integer mode) {
        UserAccount u = hrm().user(SCREEN);
        int dt = docType(mode);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("documentTypeId", dt);
        out.put("formats", sup.formats(u));
        out.put("docNoError", MISSING_READ);
        out.put("branches", pick(repo.branches(u), "Id", "BranchName"));
        out.put("projects", pick(repo.projects(u), "Id", "ProjectName"));
        out.put("suppliers", pick(repo.suppliersForExport(u), "Id", "CompanyName", "GlAccountId"));
        out.put("deliveryTerms", pick(repo.deliveryTerms(), "Id", "Code"));
        out.put("paymentTerms", pick(repo.lcPaymentTerms(u), "Id", "LcOrderTerm"));
        out.put("ports", pick(repo.seaPorts(u), "Id", "PortName"));
        out.put("currencies", pick(repo.currencies(u), "Id", "CurrencyCode"));
        // ImporterandExportBankFill: here "Foreign Country" banks are the EXPORTER banks and "Home Country" the importer banks,
        // and both combos are bound only when there is at least one foreign bank.
        Map<String, List<Map<String, Object>>> banks = sup.banks(u);
        boolean anyForeign = !banks.get("foreign").isEmpty();
        out.put("exporterBanks", anyForeign ? banks.get("foreign") : new ArrayList<>());
        out.put("importerBanks", anyForeign ? banks.get("home") : new ArrayList<>());
        out.put("accounts", accounts(u));
        if (dt == 237) {
            out.put("warehouses", pick(repo.activeWarehouses(u), "Id", "WareHouseName"));
            out.put("jobLots", pick(repo.jobLots(u), "Id", "JobLotDescription"));
        }
        return out;
    }

    /** GrdSettingShipmentExpenses: CoaAllocationGetAllServiceBind rows whose AccountTypeId is not 2, 11 or 15. */
    private List<Map<String, Object>> accounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.coaAllocations(u)) {
            int t = toInt(r.get("AccountTypeId"));
            if (t != 2 && t != 11 && t != 15) out.add(map("Id", r.get("Id"), "AccountTitle", str(r.get("AccountTitle"))));
        }
        return out;
    }

    /** generateCode(): ImInvoice.GenerateCode -> Sp_ImInvoice_GetAllMethod, which does not exist. */
    public Map<String, Object> docNo() {
        hrm().user(SCREEN);
        throw invalid(MISSING_READ);
    }

    // ================================================================== supplier -> orders -> order detail

    /**
     * cmbSupCust_Leave_1: 237 -> ImLcOrder.GetLcOrderNo(DocumentTypeId 231) rows of this supplier (Id, LcOrderNo);
     * 233 -> ExImLcOrderPurchaseOrder.GetPurchaseOrderNo(DocumentTypeId 232) rows of this supplier (Id, OrderNo).
     * The desktop filters the 237 list on a column "SupCustId" that the procedure does not return (it returns SupplierCustomerId),
     * so on the desktop the contract list always fails; the real column is used here (see report).
     */
    public List<Map<String, Object>> orders(Integer mode, int supplierId) {
        UserAccount u = hrm().user(SCREEN);
        List<Map<String, Object>> out = new ArrayList<>();
        if (supplierId <= 0) return out;
        if (docType(mode) == 237) {
            for (Map<String, Object> r : repo.lcOrderNos(u, 231)) {
                if (toInt(r.get("SupplierCustomerId")) == supplierId) out.add(map("Id", r.get("Id"), "OrderNo", r.get("LcOrderNo")));
            }
        } else {
            for (Map<String, Object> r : repo.purchaseOrderNos(u, 232)) {
                if (toInt(r.get("SupplierCustomerId")) == supplierId) out.add(map("Id", r.get("Id"), "OrderNo", r.get("OrderNo")));
            }
        }
        return out;
    }

    /**
     * cmbLcContractNo_Leave: GetImportPurchaseOrderDetailById -> the header boxes (rows[0]; no row -> the desktop's
     * "There is no row at position 0."), then GETIMPORTCONTRACTDETAILBYCONTRACTID (237) / GETIMPORTGRNDETAILBYPURCHASEORDERID (233)
     * -> dtdetail, with Qty/M.Ton and NoOfBags computed as the form does.
     */
    public Map<String, Object> order(Integer mode, int supplierId, int orderId) {
        UserAccount u = hrm().user(SCREEN);
        int dt = docType(mode);
        if (orderId <= 0 || !ids(orders(mode, supplierId), "Id").contains(orderId)) throw invalid("Contract No  Is Required");
        List<Map<String, Object>> h = repo.purchaseOrderHeader(u, orderId, dt);
        if (h.isEmpty()) throw invalid("There is no row at position 0.");
        Map<String, Object> r0 = h.get(0);
        Map<String, Object> head = map("PaymentTermId", toInt(r0.get("PaymentTermId")), "DeliveryTermId", toInt(r0.get("DeliveryTermId")),
                "ExporterBankId", toInt(r0.get("ExporterBankId")), "ImporterBankId", toInt(r0.get("ImporterBankId")),
                "LoadingPortId", toInt(r0.get("LoadingPortId")), "DestinationPortId", toInt(r0.get("DestinationPortId")),
                "FcyId", toInt(r0.get("FcyId")), "HomeCurrencyId", toInt(r0.get("HomeCurrencyId")),
                "FcyAmount", str(r0.get("FcyAmount")), "ExchangeRate", str(r0.get("ExchangeRate")), "Amount", str(r0.get("Amount")),
                "GrossWeight", str(r0.get("GrossWeight")), "NetWeight", str(r0.get("NetWeight")), "Containers", str(r0.get("Containers")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.purchaseOrderDetail(u, orderId, dt == 237)) {
            double qty = toDouble(r.get("ItemQty")), pack = toDouble(r.get("PackUom"));
            double mton, bags;
            if (dt == 237) { mton = qty; bags = qty * pack; }
            else { mton = qty * pack / 1000.0; bags = pack; }
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", r.get("ItemId"));
            m.put("ItemName", r.get("ItemName"));
            m.put("PackTypeId", r.get("PackingMaterialId"));
            m.put("PackType", r.get("PackingType"));
            m.put("Qty/M.Ton", mton);
            m.put("PackSizeId", dt == 237 ? r.get("ItemUomId") : r.get("UOMScheduleIdOuter"));
            m.put("PackSize", r.get("PackUom"));
            m.put("NoOfBags", bags);
            m.put("Cost/M.Ton", toDouble(r.get("ItemRate")));
            m.put("RateUOMId", r.get("RateUomId"));
            m.put("RateUOM", r.get("RateUom"));
            m.put("Amount", toDouble(r.get("ItemAmount")));
            m.put("ItemDetail", str(r.get("ItemDescription")));
            m.put("WareHouseId", r.get("WarehouseId"));
            m.put("WareHouseName", r.get("WareHouseName"));
            m.put("JobLotId", r.get("JobLotId"));
            m.put("JobLot", r.get("JobLotDescription"));
            m.put("AddLssAmount", 0);
            m.put("Expenses", 0);
            rows.add(m);
        }
        return map("header", head, "details", rows);
    }

    // ================================================================== save

    /**
     * btnsave_Click: "Grid Record not found", the shipment-expense account check, formvalidation() in the form's order and
     * wording, the per-row checks of the detail and expense grids - then ImInvoice.Save, which cannot run (MISSING_SAVE).
     */
    public Map<String, Object> save(ImpADtos.Invoice b, Integer mode) {
        UserAccount u = hrm().user(SCREEN);
        // The form never calls SetRightsValueInRightsObject: Save / Update / Print are not right-checked on the desktop.
        if (b.details == null || b.details.isEmpty()) throw invalid("Grid Record not found");
        for (ImpADtos.InvoiceExpense e : b.expenses) {
            if (toDouble(e.fcAmount) > 0.0 && netInt(e.localAmount) > 0 && e.accountId == 0) throw invalid("Please Select an Account Against JL First");
        }
        double netKg = 0;
        for (ImpADtos.InvoiceDetail d : b.details) netKg += toDouble(d.qtyMTon);
        netKg *= 1000.0;
        Set<Integer> suppliers = ids(repo.suppliersForExport(u), "Id");
        Map<String, List<Map<String, Object>>> banks = sup.banks(u);
        if (b.branchesId == 0 || !ids(repo.branches(u), "Id").contains(b.branchesId)) throw invalid("Branch Is Required");
        if (b.projectsId == 0 || !ids(repo.projects(u), "Id").contains(b.projectsId)) throw invalid("Project Is Required");
        if (str(b.docNo).isEmpty() || toInt(b.docNo) == 0) throw invalid("Doc No Is Required");
        if (str(b.invoiceNo).isEmpty() || "0".equals(trim(b.invoiceNo))) throw invalid("Invoice No Is Required");
        if (b.supplierId == 0 || !suppliers.contains(b.supplierId)) throw invalid("Customer Is Required");
        if (b.orderId == 0 || !ids(orders(mode, b.supplierId), "Id").contains(b.orderId)) throw invalid("Contract No  Is Required");
        if (b.paymentTermId == 0 || !ids(repo.lcPaymentTerms(u), "Id").contains(b.paymentTermId)) throw invalid("Payment Term Is Required");
        if (b.deliveryTermId == 0 || !ids(repo.deliveryTerms(), "Id").contains(b.deliveryTermId)) throw invalid("Delivery Term Is Required");
        Set<Integer> ports = ids(repo.seaPorts(u), "Id");
        if (!ports.contains(b.loadingPortId)) throw invalid("Loading Port Is Required");
        if (!ports.contains(b.destinationPortId)) throw invalid("Destination Port  Is Required");
        String carier = str(b.carierType);
        if (!("By Sea".equals(carier) || "By Air".equals(carier) || "By Road".equals(carier))) throw invalid("Carier Type  Is Required");
        Set<Integer> cur = ids(repo.currencies(u), "Id");
        if (!cur.contains(b.fcyId)) throw invalid("Fcy Code Is Required");
        if (toDouble(b.fcyAmount) == 0.0) throw invalid("Fcy Amount  Is Required");
        if (!cur.contains(b.homeCurrencyId)) throw invalid("HomeCurrency field Required");
        if (toDouble(b.exchangeRate) == 0.0) throw invalid("Exchange Rate  Is Required");
        if (toDouble(b.localAmount) == 0.0) throw invalid("LocalAmount Is Required");
        if (!(netKg > 0.0)) throw invalid("Net Weight  Is Required");
        if (!(toDouble(b.grossWeight) >= netKg)) throw invalid("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
        if (str(b.iFormNo).isEmpty() || "0".equals(str(b.iFormNo))) throw invalid("I-Form No Is Required");
        if (!(toDouble(b.totalAmount) > 0.0)) throw invalid("Total Amount  Is Required");
        if (b.importerBankId != 0 && !ids(banks.get("home"), "Id").contains(b.importerBankId)) throw invalid("Record not found.");
        if (b.exporterBankId != 0 && !ids(banks.get("foreign"), "Id").contains(b.exporterBankId)) throw invalid("Record not found.");
        for (ImpADtos.InvoiceDetail d : b.details) {
            if (d.itemId == 0) throw invalid("ItemName field required");
            if (toDouble(d.qtyMTon) * 1000.0 == 0.0) throw invalid("Qty/M.Ton field required");
            if (d.packSizeId == 0) throw invalid("PackUom field required");
            if (toDouble(d.noOfBags) == 0.0) throw invalid("NoOfBags field required");
            if (toDouble(d.costMTon) == 0.0) throw invalid("RatePrice field required");
            if (d.rateUomId == 0) throw invalid("RateUom field required");
            if (toDouble(d.amount) == 0.0) throw invalid("FcAmount field required");
            if (d.wareHouseId == 0) throw invalid("Warehouse field required");
            if (d.jobLotId == 0) throw invalid("JobLot field required");
        }
        for (ImpADtos.InvoiceExpense e : b.expenses) {
            if (e.accountId == 0) throw invalid("AccountTitle field required");
            if (toDouble(e.fcAmount) == 0.0) throw invalid("FcAmount field required");
            if (toDouble(e.localAmount) == 0.0) throw invalid("LocalAmount field required");
        }
        throw invalid(MISSING_SAVE);
    }

    /** tabControl1_SelectedIndexChanged(1) -> HistoryGridFill: ImInvoice.GetData -> Sp_ImInvoice_GetAllMethod (missing). */
    public List<Map<String, Object>> history() {
        hrm().user(SCREEN);
        throw invalid(MISSING_READ);
    }

    // ================================================================== print

    /**
     * btnPrint_Click / DataGridHistory Print: ImportReports.ImInvoiceSlip_806 (Sp_ImInvoice_SlipandRegister, @Id only when != 0,
     * ApprovedFilter "All") must return rows, else "Not Record Found For Display"; the page then prints impa-806.
     * RecId is 0 until a record is loaded (which cannot happen, see MISSING_READ), so this prints every invoice as the desktop does.
     */
    public Map<String, Object> printCheck(int id) {
        UserAccount u = hrm().user(SCREEN);
        if (repo.invoiceSlip(u, Math.max(0, id)).isEmpty()) throw invalid("Not Record Found For Display");
        return map("id", Math.max(0, id));
    }
}
