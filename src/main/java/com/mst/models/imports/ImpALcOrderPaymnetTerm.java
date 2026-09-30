package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Model.Import.ImLcOrderPaymnetTerm (0641): one public field per NON-virtual property, spelled as the C# property, so
 * {@link #toParams()} binds exactly what GenericProvider.SetProc binds. Virtual display properties are not fields.
 */
@SuppressWarnings("unused")
public class ImpALcOrderPaymnetTerm extends DesktopModel {
    public BigDecimal FcyAmount = BigDecimal.ZERO;
    public BigDecimal PrcntOfTotal = BigDecimal.ZERO;
    public int DueDays;
    public int DueTypeId;
    public int Id;
    public int ImLcOrderId;
    public int PaymentTermId;
}
