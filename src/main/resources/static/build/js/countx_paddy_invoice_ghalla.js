/* Screen 467 "Purchase Invoice For Ghalla Mandi" - Architecture.WinApp.Paddy_Purchase/frmPurchaseInvoiceForGhallaMandi.cs (InvPurchaseInvoice DocumentTypeId 168).
   Logic: countx_paddy_invoice_common.js with cfg.ghalla (popup frmLoadGrnForGhallaMandi, combined-ids load, editable ItemRate / RateUom). */
(function () {
    'use strict';
    const N = PP.num, $ = PP.$;
    const col = (k, extra) => Object.assign({ k }, extra || {});
    // dtGrid columns (:281): no PurchaseOrderId, no ScaleKart. grdSettings: hidden Id, InvGrnDetailId, InvGrnId, ItemId, ItemUomId, ItemRateUOMId, JobLotId, WarehouseId, PackingTypeId; ItemName width 250;
    // AllowEdit true with only ItemRate and RateUom editable (EditType 1), every other column EditType 0
    const mainCols = [
        col('Id', { hide: true }), col('InvGrnDetailId', { hide: true }), col('InvGrnId', { hide: true }), col('ItemId', { hide: true }), col('ItemName', { w: 250 }), col('CropYear'),
        col('ItemUomId', { hide: true }), col('PackUom'),
        col('ItemQty', { num: true, fmt: 'n2', sum: true }), col('GrossWeight', { num: true, fmt: 'n2', sum: true }), col('EbTotal', { num: true, fmt: 'n2', sum: true }),
        col('NetBillWeight', { num: true, fmt: 'n2', sum: true }), col('StockWeight', { num: true, fmt: 'n2', sum: true }), col('ItemRate', { num: true, fmt: 'rate' }),
        col('ItemRateUOMId', { hide: true }), col('RateUom', { num: true, fmt: 'int' }), col('ItemAmount', { num: true, fmt: 'n2', sum: true }), col('ItemNetAmount', { num: true, fmt: 'n2', sum: true }),
        col('Freights', { num: true, fmt: 'c0', sum: true }), col('Expense', { num: true, fmt: 'c0', sum: true }), col('Commission', { num: true, fmt: 'c0', sum: true }),
        col('JobLotId', { hide: true }), col('WarehouseId', { hide: true }), col('WareHouseName'), col('PackingTypeId', { hide: true }), col('LabNo'), col('GpNo'), col('VehicleNo')
    ];
    // dtGrid.Rows.Add(0, InvGrnDetailId, InvGrnId, ItemId, ItemName, CropYear, ItemUOMId, UOMCodeItem, ItemQty, GrossWeight, EBTotalWt, NetBillWeight, NetStockWeight, ItemRate, UomScheduleIdRate,
    //                 ToDouble(EquivalentPoRate), ItemAmount, 0, 0, 0, 0, JobLotId, WarehouseId, WareHouseName, PackingTypeId, LabAnalisysNo, GpNo, VehicleNo)  (:1386)
    const gridRowFromGrn = (r, g) => ({
        Id: 0, InvGrnDetailId: g(r, 'InvGrnDetailId'), InvGrnId: g(r, 'InvGrnId'), ItemId: g(r, 'ItemId'), ItemName: g(r, 'ItemName'), CropYear: g(r, 'CropYear'),
        ItemUomId: g(r, 'ItemUOMId'), PackUom: g(r, 'UOMCodeItem'), ItemQty: N(g(r, 'ItemQty')), GrossWeight: N(g(r, 'GrossWeight')), EbTotal: N(g(r, 'EBTotalWt')),
        NetBillWeight: N(g(r, 'NetBillWeight')), StockWeight: N(g(r, 'NetStockWeight')), ItemRate: g(r, 'ItemRate'), ItemRateUOMId: g(r, 'UomScheduleIdRate'), RateUom: N(g(r, 'EquivalentPoRate')),
        ItemAmount: N(g(r, 'ItemAmount')), ItemNetAmount: 0, Freights: 0, Expense: 0, Commission: 0, JobLotId: g(r, 'JobLotId'), WarehouseId: g(r, 'WarehouseId'), WareHouseName: g(r, 'WareHouseName'),
        PackingTypeId: g(r, 'PackingTypeId'), LabNo: g(r, 'LabAnalisysNo'), GpNo: g(r, 'GpNo'), VehicleNo: g(r, 'VehicleNo')
    });
    const pick = (row, key) => { if (!row) return null; for (const x in row) if (x.toLowerCase() === key.toLowerCase()) return row[x]; return null; };
    PI.create({
        api: '/api/paddy-purchase/invoice-ghalla', docType: 168, voucherDocType: 168, ghalla: true, freightToGL: false, editableCols: ['ItemRate', 'RateUom'], mainCols, gridRowFromGrn,
        onLoad: d => {                                                                                  // GhallaMandiBind (:1234); cmbGhallaMandi.Enabled = false (:367)
            PP.bind('cmbGhallaMandi', d.ghallaMandi || [], 'Id', 'GhallaMandi');
            $('cmbGhallaMandi').disabled = true;
        },
        onReset: () => { PP.setSel('cmbGhallaMandi', ''); },                                          // Reset: cmbGhallaMandi.Text = string.Empty (:1775)
        onRead: h => { PP.setSel('cmbGhallaMandi', pick(h, 'GhallaMandiId')); },                      // ReadById_Update: cmbGhallaMandi.Value = invPI.GhallaMandiId (:1840)
        validateExtra: () => {                                                                          // FormValidation (:1091)
            if (!$('cmbGhallaMandi').value) { alert('Ghalla_Mandi  Field is Required'); return false; }
            return true;
        },
        extraBody: () => ({ ghallaMandiId: N($('cmbGhallaMandi').value) })                              // purchaseInvoice.GhallaMandiId = Conversion.ToInt(cmbGhallaMandi.Value) (:2058)
    });
}());
