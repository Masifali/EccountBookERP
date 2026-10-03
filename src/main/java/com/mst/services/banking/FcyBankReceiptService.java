package com.mst.services.banking;

import com.mst.models.UserAccount;
import com.mst.repositories.AccountsGroupDSupport;
import com.mst.services.ExportFcyReceiptsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Map;
import java.util.function.Supplier;

/**
 * 707 "FCY Bank Receipt" (Banking Managment, module 2032) and 43 "FCY Receipt" (Accounts Transaction, module 2):
 * both ScreenDefinition rows open Architecture.WinApp.Account_Definition.Acfrmfcbankreceipt, the same desktop
 * class as 794 "Fcy Receipts" (Export). Every load, cascade, ReadById, history, Insert / MakeVoucher and DAL
 * SetDate of that form is already ported in {@link ExportFcyReceiptsService} (desktop procedures and parameters,
 * one transaction); this class reuses it and only replaces the rights source.
 *
 * Rights, as the form reads them: Acfrmfcbankreceipt_Load -> CommonServices.SetRightsValueInRightsObject(base.Name)
 * -> tblUserRights.GetByUserId -> Sp_tblUserRights_GetAllMethod @Activity='GetByUserId' @ScreenName='Acfrmfcbankreceipt'
 * (the rows of every ScreenDefinition with that name - 43, 707 and 794 - looped in the order the procedure
 * returns them, the last row of each right name wins; Admin role short-circuits Save / Update / Print /
 * CanView AllRecord). {@link AccountsGroupDSupport#rights} is that port. Delete: the form has no Delete button.
 * Tenancy, year and user come from the session only.
 */
@Service
public class FcyBankReceiptService {

    public static final String SCREEN_NAME = "Acfrmfcbankreceipt";

    @Autowired private ExportFcyReceiptsService form;
    @Autowired private AccountsGroupDSupport support;

    /** Runs one call of the shared form service with this form's own (name-based) rights. */
    public <T> T run(Supplier<T> body) {
        Map<String, Boolean> r = support.rights(SCREEN_NAME);
        return form.withRights((UserAccount u, String action) -> allowed(r, action), body);
    }

    static boolean allowed(Map<String, Boolean> r, String action) {
        String key;
        switch (action == null ? "" : action) {
            case "View": key = "canView"; break;
            case "Save": key = "canSave"; break;
            case "Update": key = "canUpdate"; break;
            case "Print": key = "canPrint"; break;
            case "CanViewAllRecord": key = "canViewAll"; break;
            default: return false;
        }
        return Boolean.TRUE.equals(r.get(key));
    }

    public ExportFcyReceiptsService form() { return form; }
}
