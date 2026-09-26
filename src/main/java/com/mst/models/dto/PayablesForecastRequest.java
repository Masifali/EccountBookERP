package com.mst.models.dto;
import java.time.LocalDate;
import lombok.Data;
import org.springframework.format.annotation.DateTimeFormat;
@Data
public class PayablesForecastRequest {
    @DateTimeFormat(iso=DateTimeFormat.ISO.DATE) private LocalDate fromDate;
    @DateTimeFormat(iso=DateTimeFormat.ISO.DATE) private LocalDate toDate;
    private int intervalDays=8;
    private int sortNo;
}
