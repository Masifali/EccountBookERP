package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Model.Import.ImLcOrderPackingDetail (0653): one public field per NON-virtual property, spelled as the C# property, so
 * {@link #toParams()} binds exactly what GenericProvider.SetProc binds. Virtual display properties are not fields.
 */
@SuppressWarnings("unused")
public class ImpALcOrderPackingDetail extends DesktopModel {
    public BigDecimal ExchangeRate = BigDecimal.ZERO;
    public BigDecimal FcyAmount = BigDecimal.ZERO;
    public BigDecimal ItemAmount = BigDecimal.ZERO;
    public BigDecimal ItemRate = BigDecimal.ZERO;
    public BigDecimal LcyAmount = BigDecimal.ZERO;
    public BigDecimal NetWeight = BigDecimal.ZERO;
    public BigDecimal Qty = BigDecimal.ZERO;
    public int ActionTypeId;
    public int CurrencyId;
    public int Id;
    public int ImItemId;
    public int ImLcOrderId;
    public int InvPackingTypeId;
    public int ItemBrandId;
    public int ItemUomId;
    public int RateUomId;
    public String ItemDescription;
    public String RemarksDetail;
}
