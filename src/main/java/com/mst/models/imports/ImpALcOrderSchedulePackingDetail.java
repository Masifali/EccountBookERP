package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Model.Import.ImLcOrderSchedulePackingDetail (0643): one public field per NON-virtual property, spelled as the C# property, so
 * {@link #toParams()} binds exactly what GenericProvider.SetProc binds. Virtual display properties are not fields.
 */
@SuppressWarnings("unused")
public class ImpALcOrderSchedulePackingDetail extends DesktopModel {
    public BigDecimal NetWeight = BigDecimal.ZERO;
    public int CropYearId;
    public int Id;
    public int ImItemId;
    public int ImLcOrderId;
    public int ImLcOrderScheduleId;
    public int NoOfBags;
    public int PackingTypeId;
    public int ActionTypeId;
    public int PackUomId;
}
