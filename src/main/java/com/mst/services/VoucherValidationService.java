package com.mst.services;

import com.mst.models.dto.VoucherValidationFilterDto;
import com.mst.models.dto.VoucherValidationReportDto;
import com.mst.security.CurrentUserContext;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Voucher Validation Report backend — ditto VoucherValidation.cs (Architecture.WinApp.
 * Account_Reports, 2312 lines; one of the 27 Account Reports tiles, routed here under
 * /accounts/vouchers/voucher-validation for historical reasons, not under /accounts/reports/).
 *
 * Real proc: VoucherReports.VoucherValidationReport(ReportsParameters) ->
 * Sp_Accounts_VouchersValidation_Rpt, confirmed by reading the BLL method body directly (not
 * inferred from the proc name alone). This replaces a previous fabricated pass that hardcoded
 * @OrganizationId=1/@CompanyId=1 and silently fell back to guessed raw SQL against
 * tbl_vouchers_head/tbl_vouchers_detail/ChartofAccount/tbl_DocumentType/tbl_AcLookUps whenever the
 * (also-guessed) proc call failed — that fallback is removed entirely, not left as a safety net.
 */
@Service
public class VoucherValidationService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /** Ditto VoucherValidation.cs's GetAllDetailAccount() -> CommonServices.GetAllDetailAccount()
     *  -> ChartofAccount.DetailAccount(obj) -> Sp_ChartofAccount_GetAllMethodFromCOA
     *  @CoaType='DetailAccount' - the same real proc already confirmed for the Account Reports'
     *  own getAllDetailAccounts() (AccountsReportService.java). Feeds "Account Title" cmbAccountTitle. */
    public List<Map<String, Object>> getAllDetailAccounts() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();
        int appId = currentUserContext.currentAppId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @UsersId=?, @AppId=?, @CoaType=?",
                    orgId, compId, userId, appId, "DetailAccount");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto CustomeGroupsDefine(1) -> CommonServices.CustomeGroupsDefine(1) -> AcLookUps.GetAll
     *  -> Sp_AcLookUps_GetAllMethod @Activity='ReadAll', @AcLookUpTypesId=1. Feeds "Custom Group". */
    public List<Map<String, Object>> getCustomGroups() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?, @AcLookUpTypesId=?",
                    orgId, compId, "ReadAll", 1);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto VouchertypeFill() -> VoucherHead.GetDocumentTypesFromVouchers(obj) ->
     *  Sp_Vouchers_GetMethods @Activity='GetDocumentTypesFromVouchers'. Feeds "Document Type"
     *  cmbdoctype - a real multi-select checked-list combo on desktop, ditto Trial Balance's
     *  "Documents Not To Include". */
    public List<Map<String, Object>> getDocumentTypes() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @Activity=?",
                    orgId, compId, "GetDocumentTypesFromVouchers");
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto MultiLanguagesGetAll() -> CommonServices.MultiLanguagesGetAll() ->
     *  Sp_MultiLanguages_GetAll @MethodType='ReadAll'. Feeds the hidden-by-default
     *  CmbLanguage/lblLanguage pair, revealed only by Ctrl+L, ditto VoucherValidation_KeyDown. */
    public List<Map<String, Object>> getLanguages() {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            return jdbcTemplate.queryForList(
                    "EXEC Sp_MultiLanguages_GetAll @MethodType=?, @OrganizationId=?, @CompanyId=?",
                    "ReadAll", orgId, compId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** Ditto btnshow_Click -> VoucherReports.VoucherValidationReport(obj) ->
     *  Sp_Accounts_VouchersValidation_Rpt. Real params, read directly from the BLL method body:
     *  @OrganizationId/@CompanyId always; [@UserId] when >0 (real btnshow_Click always sets it
     *  from the logged-in user, ditto CurrentUserContext); [@VoucherCodeF]/[@VoucherCodeT] when
     *  the doc-no range is set; [@VoucherDateF]/[@VoucherDateT] when filterType is "DocDate"
     *  (ditto rdDocFromToDate) OR [@EntryDateFrom]/[@EntryDateTo] when filterType is "EntryDate"
     *  (ditto rdEntryFromToDate) - the two are mutually exclusive on this screen, matching the real
     *  if/else-if radio check; [@CustomGroupId]/[@AccountId] when set; [@DocumentTypeIds] (CSV)
     *  when the multi-select combo has a selection; [@IsApproved]=true only for "Approved",
     *  =false only for "UnApproved", omitted entirely for "All" (ditto the real
     *  ApprovedFilter != "All" guard - NOT simply "send unless All"); [@ManualBillNo] when set;
     *  [@IsCGS]=1 only when Skip CGS Entries is checked (ditto chkSkipCgs.Checked -> ActionId=1);
     *  [@LanguageId] when >0. There is no @FinancialYearId/@BranchesId/@ProjectsId/@Id/
     *  @ModifyFromDate/@ModifyToDate/@ApprovedFromDate/@ApprovedToDate on THIS screen's own call
     *  (those BLL-supported params exist for other callers of the same proc, not for
     *  VoucherValidation.cs's own btnshow_Click - never assumed present here). */
    public List<VoucherValidationReportDto> getVoucherValidationReport(VoucherValidationFilterDto filter) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();

        StringBuilder sql = new StringBuilder("EXEC Sp_Accounts_VouchersValidation_Rpt @OrganizationId=?, @CompanyId=?");
        List<Object> params = new ArrayList<>();
        params.add(orgId);
        params.add(compId);

        if (userId > 0) { sql.append(", @UserId=?"); params.add(userId); }
        if (filter.getDocNoFrom() != null && filter.getDocNoFrom() != 0) { sql.append(", @VoucherCodeF=?"); params.add(filter.getDocNoFrom()); }
        if (filter.getDocNoTo() != null && filter.getDocNoTo() != 0) { sql.append(", @VoucherCodeT=?"); params.add(filter.getDocNoTo()); }

        String filterType = filter.getFilterType();
        if ("EntryDate".equalsIgnoreCase(filterType)) {
            if (filter.getFromDate() != null && !filter.getFromDate().isBlank()) { sql.append(", @EntryDateFrom=?"); params.add(filter.getFromDate()); }
            if (filter.getToDate() != null && !filter.getToDate().isBlank()) { sql.append(", @EntryDateTo=?"); params.add(filter.getToDate()); }
        } else {
            if (filter.getFromDate() != null && !filter.getFromDate().isBlank()) { sql.append(", @VoucherDateF=?"); params.add(filter.getFromDate()); }
            if (filter.getToDate() != null && !filter.getToDate().isBlank()) { sql.append(", @VoucherDateT=?"); params.add(filter.getToDate()); }
        }

        if (filter.getCustomGroupId() != null && filter.getCustomGroupId() != 0) { sql.append(", @CustomGroupId=?"); params.add(filter.getCustomGroupId()); }
        if (filter.getAccountId() != null && filter.getAccountId() != 0) { sql.append(", @AccountId=?"); params.add(filter.getAccountId()); }
        if (filter.getDocumentTypeIds() != null && !filter.getDocumentTypeIds().isBlank()) { sql.append(", @DocumentTypeIds=?"); params.add(filter.getDocumentTypeIds()); }

        String approvedFilter = filter.getApprovedFilter();
        if ("Approved".equalsIgnoreCase(approvedFilter)) { sql.append(", @IsApproved=?"); params.add(true); }
        else if ("UnApproved".equalsIgnoreCase(approvedFilter)) { sql.append(", @IsApproved=?"); params.add(false); }
        // "All" (or unset) omits @IsApproved entirely, ditto ApprovedFilter != "All" guard.

        if (filter.getManualNo() != null && !filter.getManualNo().trim().isEmpty()) { sql.append(", @ManualBillNo=?"); params.add(filter.getManualNo().trim()); }
        if (Boolean.TRUE.equals(filter.getSkipCgs())) { sql.append(", @IsCGS=?"); params.add(1); }
        Integer languageId = filter.getLanguageId();
        if (languageId != null && languageId != 0) { sql.append(", @LanguageId=?"); params.add(languageId); }

        List<VoucherValidationReportDto> result = new ArrayList<>();
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(sql.toString(), params.toArray());
        for (Map<String, Object> r : rows) {
            result.add(mapRowToDto(r));
        }
        return result;
    }

    private VoucherValidationReportDto mapRowToDto(Map<String, Object> r) {
        VoucherValidationReportDto dto = new VoucherValidationReportDto();
        dto.setId(r.get("Id") != null ? ((Number) r.get("Id")).longValue() : 0L);
        dto.setAccountId(r.get("AccountId") != null ? ((Number) r.get("AccountId")).longValue() : 0L);
        dto.setDocumentType(r.get("DocumentTypeDescription") != null ? r.get("DocumentTypeDescription").toString() : "");
        dto.setDocumentTypeId(r.get("DocumentTypeId") != null ? ((Number) r.get("DocumentTypeId")).intValue() : 0);
        dto.setDocumentTypeSrNo(r.get("DocumentTypeSrNo") != null ? ((Number) r.get("DocumentTypeSrNo")).intValue() : 0);
        dto.setVoucherCode(r.get("VoucherCode") != null ? ((Number) r.get("VoucherCode")).intValue() : 0);

        Object vDate = r.get("VoucherDate");
        if (vDate instanceof Timestamp) {
            dto.setVoucherDate(new SimpleDateFormat("dd/MM/yyyy").format((Timestamp) vDate));
        } else if (vDate != null) {
            dto.setVoucherDate(vDate.toString());
        } else {
            dto.setVoucherDate("");
        }

        dto.setAccountCode(r.get("AccountCode") != null ? r.get("AccountCode").toString() : "");
        dto.setAccountTitle(r.get("AccountTitle") != null ? r.get("AccountTitle").toString() : "");
        dto.setOffsetAccount(r.get("AccountTitleCoag") != null ? r.get("AccountTitleCoag").toString() : "");

        Object debit = r.get("DebitAmount");
        dto.setDebitAmount(debit != null ? new BigDecimal(debit.toString()) : BigDecimal.ZERO);

        Object credit = r.get("CreditAmount");
        dto.setCreditAmount(credit != null ? new BigDecimal(credit.toString()) : BigDecimal.ZERO);

        dto.setChequeNo(r.get("CheqNoDetail") != null ? r.get("CheqNoDetail").toString() : (r.get("ChequeNo") != null ? r.get("ChequeNo").toString() : ""));
        dto.setManualBillNo(r.get("ManualBillNo") != null ? r.get("ManualBillNo").toString() : "");
        dto.setRefInvoiceNo(r.get("RefInvoiceNo") != null ? r.get("RefInvoiceNo").toString() : "");
        dto.setComments(r.get("Comments") != null ? r.get("Comments").toString() : "");
        dto.setNoOfAttachments(r.get("NoOfAttachments") != null ? ((Number) r.get("NoOfAttachments")).intValue() : 0);

        return dto;
    }
}
