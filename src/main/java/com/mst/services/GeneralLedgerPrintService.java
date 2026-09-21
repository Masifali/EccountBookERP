package com.mst.services;

import com.mst.models.Company;
import com.mst.models.UserAccount;
import com.mst.repositories.GeneralLedgerPrintRepository;
import com.mst.repositories.GeneralLedgerPrintRepository.Format;
import com.mst.repositories.ICompanyRepository;
import com.mst.security.CurrentUserContext;
import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class GeneralLedgerPrintService {
    public record Request(String format, int accountId, LocalDate fromDate, LocalDate toDate,
                          boolean includeUnposted, Integer languageId) {}
    public record Column(String field, String title) {}
    public record Report(String code, String title, String companyName, String companyAddress,
                         LocalDate fromDate, LocalDate toDate, List<Column> columns,
                         List<Map<String, Object>> rows) {}

    private final GeneralLedgerPrintRepository repository;
    private final CurrentUserContext context;
    private final ICompanyRepository companies;
    public GeneralLedgerPrintService(GeneralLedgerPrintRepository repository, CurrentUserContext context,
                                     ICompanyRepository companies) {
        this.repository = repository; this.context = context; this.companies = companies;
    }

    @Transactional(readOnly = true)
    public Report load(Request request) {
        Format format = Format.fromCode(request.format());
        UserAccount user = context.requireAccountingUser();
        if (Integer.valueOf(5).equals(user.getAppId())) {
            throw new IllegalStateException("Cost Center Not Found: select a cost center before printing");
        }
        int year = format == Format.SUMMARY_II ? 0
                : repository.activeFinancialYear(user.getOrganizationId(), user.getCompanyId());
        List<Map<String, Object>> rows = repository.load(format, user.getOrganizationId(), user.getCompanyId(),
                year, user.getId(), request.accountId(), request.fromDate(), request.toDate(),
                request.includeUnposted(), request.languageId());
        Company company = companies.findById(user.getCompanyId())
                .orElseThrow(() -> new IllegalStateException("Authenticated company was not found"));
        return new Report(format.code, title(format), company.getCompName(), company.getCompAddress(),
                request.fromDate(), request.toDate(), columns(format), rows);
    }

    private static String title(Format format) {
        return switch (format) {
            case STANDARD -> "General Ledger";
            case OFFSET -> "General Ledger with Offset Accounts";
            case QUANTITATIVE -> "Quantitative General Ledger";
            case SUMMARY_I -> "General Ledger Summary-I";
            case SUMMARY_II -> "General Ledger Summary-II";
        };
    }

    // Exact dataset column spellings from GeneralLedger.cs. No client regrouping or renamed SQL fields.
    private static List<Column> columns(Format format) {
        if (format == Format.SUMMARY_I) return List.of(
                col("VoucherDate", "Voucher Date"), col("DocumentTypeDescription", "Doc Type"),
                col("VoucherCode", "Voucher No"), col("AccountTitle", "Account Title"), col("Remarks", "Remarks"),
                col("DebitAmount", "Debit"), col("CreditAmount", "Credit"), col("RunningBalance", "Running Balance"));
        if (format == Format.SUMMARY_II) return List.of(
                col("VoucherDate", "Voucher Date"), col("VoucherType", "Doc Type"), col("VoucherNo", "Voucher No"),
                col("AccountTitle", "Account Title"), col("OffSetAccountTitle", "Offset Account"), col("Remarks", "Remarks"),
                col("DebitAmount", "Debit"), col("CreditAmount", "Credit"), col("RunningBalance", "Running Balance"));
        java.util.ArrayList<Column> result = new java.util.ArrayList<>(List.of(
                col("VoucherDate", "Voucher Date"), col("DocTypeCode", "Doc Type"), col("VoucherCode", "Voucher No"),
                col("AccountCode", "Account Code")));
        if (format != Format.STANDARD) result.add(col("OffsetAccountTitle", "Offset Account"));
        result.add(col("Comments", "Narration"));
        result.addAll(List.of(col("DebitAmount", "Debit"), col("CreditAmount", "Credit"), col("Balance", "Balance")));
        if (format == Format.QUANTITATIVE) result.addAll(List.of(
                col("QtyIn", "Qty In"), col("QtyOut", "Qty Out"), col("ItemRate", "Item Rate"),
                col("VehcileNo", "Vehicle No"), col("RefParty", "Ref Party"), col("JobLot", "Job Lot")));
        return List.copyOf(result);
    }
    private static Column col(String field, String title) { return new Column(field, title); }
}
