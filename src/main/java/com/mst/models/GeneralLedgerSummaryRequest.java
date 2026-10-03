package com.mst.models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import lombok.Data;
import org.springframework.format.annotation.DateTimeFormat;

import javax.validation.constraints.AssertTrue;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Positive;
import javax.validation.constraints.PositiveOrZero;
import java.time.LocalDate;

/** Filters for GET or POST /reports/general-ledger-summary. */
@Data
public class GeneralLedgerSummaryRequest {
    @NotNull
    @Positive
    private Integer accountId;

    @NotNull
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
    private LocalDate fromDate;

    @NotNull
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE)
    private LocalDate toDate;

    private boolean includeUnposted = true;

    @PositiveOrZero
    private Integer branchId;
    @PositiveOrZero
    private Integer subsidiaryAccountId;
    @PositiveOrZero
    private Integer subsidiaryTypeId;
    @PositiveOrZero
    private Integer costCenterId;
    @PositiveOrZero
    private Integer languageId;

    @JsonIgnore
    @AssertTrue(message = "From date must be on or before To date")
    public boolean isDateRangeValid() {
        return fromDate == null || toDate == null || !fromDate.isAfter(toDate);
    }
}
