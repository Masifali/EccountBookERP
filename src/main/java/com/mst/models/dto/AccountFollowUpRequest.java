package com.mst.models.dto;

import java.time.LocalDate;
import javax.validation.constraints.*;
import lombok.Data;

@Data
public class AccountFollowUpRequest {
    @Min(0) private int id;
    @Min(1) private int accountId;
    @NotNull private LocalDate followupDate;
    @NotNull @Min(0) private Integer nextFollowupDays;
    @NotNull private LocalDate promiseDate;
    @Size(max=1000) private String comments="";
    @NotNull private java.util.UUID requestId;
    private int accountClass=3;
    private int reportScreenId;
}
