/* Screen 466 "Purchase Invoice For Gate" - Architecture.WinApp.Paddy_Purchase/frmPurchaseInvoiceForGate.cs (InvPurchaseInvoice DocumentTypeId 166). Logic: countx_paddy_invoice_common.js. */
(function () {
    'use strict';
    // grdSettings (:378): hidden Id, PurchaseOrderId, InvGrnDetailId, InvGrnId, ItemId, ItemUomId, ItemRateUOMId, JobLotId, WarehouseId, PackingTypeId; the grid is read-only (AllowEdit false)
    const N = PP.num;
    const col = (k, extra) => Object.assign({ k }, extra || {});
    const mainCols = [
        col('Id', { hide: true }), col('PurchaseOrderId', { hide: true }), col('InvGrnDetailId', { hide: true }), col('InvGrnId', { hide: true }), col('ItemId', { hide: true }), col('ItemName'), col('CropYear'),
        col('ItemUomId', { hide: true }), col('PackUom'),
        col('ItemQty', { num: true, fmt: 'n2', sum: true }), col('GrossWeight', { num: true, fmt: 'n2', sum: true }), col('ScaleKart', { num: true, fmt: 'n2', sum: true }), col('EbTotal', { num: true, fmt: 'n2', sum: true }),
        col('NetBillWeight', { num: true, fmt: 'n2', sum: true }), col('StockWeight', { num: true, fmt: 'n2', sum: true }), col('ItemRate', { num: true, fmt: 'rate' }),
        col('ItemRateUOMId', { hide: true }), col('RateUom', { num: true, fmt: 'int' }), col('ItemAmount', { num: true, fmt: 'n2', sum: true }), col('ItemNetAmount', { num: true, fmt: 'n2', sum: true }),
        col('Freights', { num: true, fmt: 'c0', sum: true }), col('Expense', { num: true, fmt: 'c0', sum: true }), col('Commission', { num: true, fmt: 'c0', sum: true }),
        col('JobLotId', { hide: true }), col('WarehouseId', { hide: true }), col('WareHouseName'), col('PackingTypeId', { hide: true }), col('LabNo'), col('GpNo'), col('VehicleNo')
    ];
    // dtGrid.Rows.Add(0, PurchaseOrderId, InvGrnDetailId, InvGrnId, ItemId, ItemName, CropYear, ItemUOMId, UOMCodeItem, ItemQty, GrossWeight, ScaleKart, EBTotalWt, NetBillWeight, NetStockWeight,
    //                 ItemRate, UomScheduleIdRate, EquivalentPoRate, ItemAmount, 0, 0, 0, 0, JobLotId, WarehouseId, WareHouseName, PackingTypeId, LabAnalisysNo, GpNo, VehicleNo)  (:1373)
    const gridRowFromGrn = (r, g) => ({
        Id: 0, PurchaseOrderId: g(r, 'PurchaseOrderId'), InvGrnDetailId: g(r, 'InvGrnDetailId'), InvGrnId: g(r, 'InvGrnId'), ItemId: g(r, 'ItemId'), ItemName: g(r, 'ItemName'), CropYear: g(r, 'CropYear'),
        ItemUomId: g(r, 'ItemUOMId'), PackUom: g(r, 'UOMCodeItem'), ItemQty: N(g(r, 'ItemQty')), GrossWeight: N(g(r, 'GrossWeight')), ScaleKart: N(g(r, 'ScaleKart')), EbTotal: N(g(r, 'EBTotalWt')),
        NetBillWeight: N(g(r, 'NetBillWeight')), StockWeight: N(g(r, 'NetStockWeight')), ItemRate: g(r, 'ItemRate'), ItemRateUOMId: g(r, 'UomScheduleIdRate'), RateUom: N(g(r, 'EquivalentPoRate')),
        ItemAmount: N(g(r, 'ItemAmount')), ItemNetAmount: 0, Freights: 0, Expense: 0, Commission: 0, JobLotId: g(r, 'JobLotId'), WarehouseId: g(r, 'WarehouseId'), WareHouseName: g(r, 'WareHouseName'),
        PackingTypeId: g(r, 'PackingTypeId'), LabNo: g(r, 'LabAnalisysNo'), GpNo: g(r, 'GpNo'), VehicleNo: g(r, 'VehicleNo')
    });
    PI.create({ api: '/api/paddy-purchase/invoice-gate', docType: 166, voucherDocType: 166, mainCols, gridRowFromGrn });
}());
