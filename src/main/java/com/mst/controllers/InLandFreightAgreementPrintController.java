package com.mst.controllers;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;

import javax.servlet.http.HttpServletResponse;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import com.mst.controllers.ERPPrint.ReportPrintSupport;
import com.mst.models.Company;
import com.mst.repositories.ICompanyRepository;
import com.mst.services.InLandFreightAgreementDesktopService;

/**
 * toolStripButton1_Click ("242-Print") and btnGrnFormHistory_Click ("Register") of InLandFreightAgreement.
 * Both run VoucherReports.InlandFreightAggreement (SP_InLandFreightAgreementSlipAndRegister: org, company, financial year,
 * DocumentTypeId 161 and, for the slip only, @Id = RecId when it is not 0) and hand the table plus the report parameters
 * @CompanyAddress / @CompanyName to 242-InLandFreightAgreementSlip.rpt / 243-InLandFreightAgreementRegister.rpt through the
 * normal Jasper pipeline. The seeded print contract takes @Id only for 243, so this screen loads its own rows exactly as the
 * desktop does and renders them with the shared pipeline (the existing /reports/print endpoints are left as they are).
 */
@Controller
@RequestMapping("/accounts/inland-freight-agreement/print")
public class InLandFreightAgreementPrintController extends ReportPrintSupport {

    @Autowired private InLandFreightAgreementDesktopService svc;
    @Autowired private ICompanyRepository companyRepository;

    private static final String SLIP = "242-InLandFreightAgreementSlip.rpt";
    private static final String REGISTER = "243-InLandFreightAgreementRegister.rpt";

    /** "Not Record Found For Display" check made before the PDF is opened (the desktop shows it as a MessageBox). */
    @GetMapping("/{kind}/count")
    @ResponseBody
    public Map<String, Object> count(@org.springframework.web.bind.annotation.PathVariable("kind") String kind,
                                     @RequestParam(value = "id", defaultValue = "0") int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            res.put("count", svc.printRows("slip".equals(kind) ? id : 0).size());
            res.put("success", true);
        } catch (Exception e) {
            Throwable t = e;
            while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            res.put("success", false);
            res.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
        }
        return res;
    }

    @GetMapping("/{kind}")
    public void print(HttpServletResponse response, @org.springframework.web.bind.annotation.PathVariable("kind") String kind,
                      @RequestParam(value = "id", defaultValue = "0") int id) throws Exception {
        boolean slip = "slip".equals(kind);
        if (!slip && !"register".equals(kind)) { response.sendError(404, "Unknown report"); return; }
        currentUserContext.currentUserId();
        List<Map<String, Object>> rows = svc.printRows(slip ? id : 0);
        Company company = companyRepository.findById(currentUserContext.currentCompanyId()).orElse(null);
        Map<String, Object> parameters = new HashMap<>();
        parameters.put("CompanyAddress", company == null ? "" : Objects.toString(company.getCompAddress(), ""));
        parameters.put("CompanyName", company == null ? "" : Objects.toString(company.getCompName(), ""));
        Map<String, Object> result = new HashMap<>();
        result.put("rows", rows);
        result.put("reportParameters", parameters);
        printReportData(response, slip ? SLIP : REGISTER, result);
    }
}
