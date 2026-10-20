package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_AccountsPayRecFollowUpHistory_GetAllMethod @Activity='ReadAll' (JPQL). */
public interface AccountsPayRecFollowUpHistoryRow {
    Integer getId();
    Integer getDocumentTypeIdRef();
    Integer getChartOfAccountId();
    String getCommentsDetail();
    LocalDateTime getCommentsDate();
    LocalDateTime getFollowupDate();
    Integer getNextFollowupDays();
    LocalDateTime getPromiseDate();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    String getAccountTitle();
}
