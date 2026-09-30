package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.repositories.imports.ImpARepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.imports.ImpASupport.*;

/**
 * The four Import registers (Architecture.WinApp.ImportReports). All four forms are the same shape: frmSaleInvoiceRegister_Load
 * binds CmbSupplier (CommonServices.ReadByOrganizationCompanyIdForExportServiceBind) and cmbitem ("Variety",
 * ItemGetForComboServiceBind) and sets From Date to the active year's Start_Period; btnshow_Click runs the register; print_Click
 * hands the same DataTable to the .rpt ("Record Not Found For Dispaly" when empty). Refresh and the export button have no handler.
 *
 *   392 ImportInvoiceRegister   SP_ImInvoiceRegister_WithAvgRates     807-ImInvoiceRegister_WithAvgRates.rpt
 *   393 ImportContractRegister  Sp_ImLcOrderNo_ImportSlip_Rpt         808-ImportContractRegister.rpt
 *   394 ImportGrnRegister       Sp_ImGRN_SlipAndRegister (234)        811-ImportGrnRegister.rpt
 *   395 ImportPurchaseRegister  Sp_ImportPurchaseOrder_Slip_rpt (236) 810-ImportPurchaseOrder.rpt
 */
@Service
public class ImpARegistersService {

    @Autowired private ImpASupport sup;
    @Autowired private ImpARepository repo;

    private HrmSupport hrm() { return sup.hrm(); }

    public static int screenOf(String kind) {
        switch (str(kind)) {
            case "invoice": return 392;
            case "contract": return 393;
            case "grn": return 394;
            case "purchase": return 395;
            default: throw invalid("Unknown register.");
        }
    }

    /** frmSaleInvoiceRegister_Load. */
    public Map<String, Object> setup(String kind) {
        int screen = screenOf(kind);
        UserAccount u = hrm().user(screen);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", hrm().rights(u, screen));
        out.put("suppliers", pick(repo.suppliersForExport(u), "Id", "CompanyName"));
        out.put("items", pick(repo.itemsTwoColumns(u), "Id", "ItemName"));
        out.put("fromDate", sup.yearStart(u));
        return out;
    }

    /** btnshow_Click -> gridHisory(): the pickers' values, supplier / item only when picked. */
    public List<Map<String, Object>> show(String kind, String from, String to, int supplierId, int itemId) {
        int screen = screenOf(kind);
        UserAccount u = hrm().user(screen);
        Timestamp f = dayStart(from), t = dayNow(to);
        switch (screen) {
            case 392: return repo.invoiceRegister(u, f, t, supplierId, itemId);
            case 393: return repo.contractRegister(u, f, t, supplierId, itemId);
            case 394: return repo.grnRegister(u, f, t, supplierId, itemId);
            default:  return repo.purchaseRegister(u, f, t, supplierId, itemId);
        }
    }

    /** print_Click is enabled by nothing on the desktop; View on the screen is the only check. */
    public Map<String, Object> printCheck(String kind) {
        hrm().user(screenOf(kind));
        return map("ok", true);
    }

    private static Timestamp dayStart(String s) {
        java.time.LocalDateTime d = HrmSupport.toDay(s);
        return d == null ? Timestamp.valueOf(java.time.LocalDate.now().atStartOfDay()) : Timestamp.valueOf(d);
    }
}
