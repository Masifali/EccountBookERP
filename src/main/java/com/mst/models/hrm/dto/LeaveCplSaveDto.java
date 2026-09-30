package com.mst.models.hrm.dto;

import java.util.List;

/** Save body of 662/463 and 461: grd.GetRows() and Deletelst. */
public class LeaveCplSaveDto {
    public List<LeaveCplRowDto> rows;
    public List<LeaveCplRowDto> deletes;
}
