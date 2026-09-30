/* ============================================================================================
 * countx_logistics_freight_voucher_export_multi.js - 967 Freight Voucher Export (Multi Vehicles)
 * Architecture.WinApp.Service.lgstcm.frmFreightVoucherExportMultiVehicles (DocumentTypeId 1305, one voucher per vehicle
 * under a master document). Filters To Load Data (pending gate passes), Apply Rate, Update City In Detail, the editable
 * voucher grid (RecalculateFreight), New / Save / Update / Delete, prints 1305_01 and the 102 voucher slip, History.
 * Exposes window.LgsFM.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/logistics/freight-voucher-export-multi';
    var P = {};
    window.LgsFM = P;

    var RecId = 0, UpdateMode = false, rights = {}, cfg = {}, removed = [];
    var cities = [], billBases = [], rateUoms = [], grd, grdHistory, grdHistoryDetail;

    function amt() { return HRM.int(cfg.amountDecimals); }
    function opts(rows, textKey) { return function () { return [[0, '']].concat(rows.map(function (r) { return [r.Id, r[textKey]]; })); }; }
    function nameOf(rows, id, textKey) { var n = ''; rows.forEach(function (r) { if (HRM.int(r.Id) === HRM.int(id)) n = r[textKey]; }); return n; }

    // ------------------------------------------------------------------------ voucher grid
    /**
     * FreightVoucherOutward_Helper.VoucherDetailGridCommonSetting / InitializeMasterDataDetailtable are not decompiled:
     * the columns follow FillMasterVoucherDetailFromList. EditColumnsList: DocDate, BiltyDate (date), the four combos,
     * NoOfBages, OtherCharges, Remarks; NetWeightDo / FreightRate / QtyForRate / BiltyFreight only when no P.O is picked.
     */
    function columns(editable, history) {
        var noPo = HRM.comboVal('CmbLogisticPoNo') === 0;
        var e = function (t) { return editable ? t : (t === 'edit-num' ? 'num' : t === 'edit-date' ? 'date' : 'text'); };
        var ep = function (d) { return editable && noPo ? 'edit-num' : 'num'; };
        function combo(key, cap, rows, text, w) {
            if (history) return [{ key: key, hidden: true }, { key: key.replace(/Id$/, ''), caption: cap, width: w }];
            return [{ key: key, caption: cap, width: w, type: editable ? 'select' : 'text', options: opts(rows, text) }];
        }
        return [{ key: 'Id', hidden: true }, { key: 'DocNo', caption: 'DocNo', width: 60 }, { key: 'DocDate', caption: 'DocDate', type: e('edit-date'), width: 110 },
            { key: 'GPId', hidden: true }, { key: 'GpNo', caption: 'GpNo', width: 60 }, { key: 'GpDate', caption: 'GpDate', type: 'date' },
            { key: 'DoId', hidden: true }, { key: 'DoNo', caption: 'DoNo', width: 60 }, { key: 'DoDate', caption: 'DoDate', type: 'date' }]
            .concat(combo('LoadingCityId', 'Loading City', cities, 'Description', 110))
            .concat(combo('UnLoadingCityId', 'Un-Loading City', cities, 'Description', 110))
            .concat([{ key: 'VehicleNo', caption: 'VehicleNo', width: 90 }, { key: 'BiltyNo', caption: 'BiltyNo', width: 80 },
                { key: 'BiltyDate', caption: 'BiltyDate', type: e('edit-date'), width: 110 }, { key: 'PoId', hidden: true }, { key: 'PoNo', caption: 'PoNo', width: 60 },
                { key: 'NoOfBages', caption: 'NoOfBages', type: e('edit-num'), decimals: 2, width: 70 },
                { key: 'NetWeightDo', caption: 'NetWeightDo', type: ep(), decimals: 2, width: 80 },
                { key: 'NetWeightWB', caption: 'NetWeightWB', type: 'num', decimals: 2, width: 80 }])
            .concat(combo('BillWeightBaseId', 'Bill Weight Base', billBases, 'Name', 90))
            .concat([{ key: 'BillWeight', caption: 'BillWeight', type: 'num', decimals: 2, width: 80 },
                { key: 'FreightRate', caption: 'FreightRate', type: ep(), decimals: 2, width: 70 }])
            .concat(combo('RateUomId', 'Rate Uom', rateUoms, 'Name', 70))
            .concat([{ key: 'QtyForRate', caption: 'QtyForRate', type: ep(), decimals: 3, width: 70 },
                { key: 'BiltyFreight', caption: 'BiltyFreight', type: ep(), decimals: amt(), sum: true, width: 90 },
                { key: 'OtherCharges', caption: 'OtherCharges', type: e('edit-num'), decimals: amt(), sum: true, width: 90 },
                { key: 'TotalBiltyFreight', caption: 'TotalBiltyFreight', type: 'num', decimals: amt(), sum: true, width: 90 },
                { key: 'AdvanceOrCashFreight', caption: 'AdvanceOrCashFreight', type: 'num', decimals: amt(), sum: true, width: 90 },
                { key: 'NetCreditToTransporter', caption: 'NetCreditToTransporter', type: 'num', decimals: amt(), sum: true, width: 100 },
                { key: 'Remarks', caption: 'Remarks', type: e('edit'), width: 140 }]);
    }
    /** BindGrids -> grdVoucherSetting (the grid is rebuilt with the editable set of the moment). */
    function BindGrids() {
        var keep = grd ? grd.rows() : [];
        grd = LgsB.freshGrid('grdVoucher', {
            columns: [LgsB.btnCol('Delete', '', 'X', 20)].concat(columns(true, false)),
            totals: true, emptyText: '',
            onChange: function (r, k) { grdVoucher_CellUpdated(r, k); }
        });
        LgsB.trackColumn(grd);
        LgsB.onButton(grd, function (r, act, i) { if (act === 'Delete') DeleteCustomerDetailRow(r, i); });
        LgsB.gridKeys(grd, {
            'ctrl+space': function (r, i) { if (LgsB.lastCol(grd) === 'Delete') DeleteCustomerDetailRow(r, i); },
            'ctrl+delete': function (r, i) { DeleteCustomerDetailRow(r, i); },
            'f1': function (r) { gridLookup(r); }
        });
        grd.set(keep);
    }
    function billWeight(r) {
        var b = HRM.int(r.BillWeightBaseId);
        if (b === 23) r.BillWeight = HRM.num(r.NetWeightDo);
        if (b === 24) r.BillWeight = HRM.num(r.NetWeightWB);
    }
    function qtyForRate(uom, w) { return uom === 3 ? w / 1000.0 : uom === 4 ? w : 1.0; }
    /** RecalculateFreight(row). */
    function RecalculateFreight(r) {
        var bilty = HRM.num(r.FreightRate) * HRM.num(r.QtyForRate);
        r.BiltyFreight = bilty;
        var total = bilty + HRM.num(r.OtherCharges);
        r.TotalBiltyFreight = total;
        r.NetCreditToTransporter = total - HRM.num(r.AdvanceOrCashFreight);
    }
    /** grdVoucher_CellUpdated. */
    function grdVoucher_CellUpdated(r, k) {
        if (k === 'NoOfBages' || k === 'NetWeightDo' || k === 'NetWeightWB' || k === 'BillWeightBaseId') { billWeight(r); RecalculateFreight(r); }
        if (k === 'RateUomId' || k === 'BillWeight') { r.QtyForRate = qtyForRate(HRM.int(r.RateUomId), HRM.num(r.BillWeight)); RecalculateFreight(r); }
        if (k === 'FreightRate' || k === 'QtyForRate' || k === 'OtherCharges' || k === 'AdvanceOrCashFreight') {
            if (HRM.num(r.OtherCharges) < 0) { r.OtherCharges = 0; HRM.box('Invalid Other Charges Amount'); }
            RecalculateFreight(r);
        }
        grd.draw();
    }
    /** F1 (OpenComboSelectionPopUpAndSetGridRow). */
    function gridLookup(r) {
        var c = LgsB.lastCol(grd), cols = function (t) { return [{ key: 'Id', hidden: true }, { key: t, caption: t, type: 'code', width: 220 }]; };
        if (c === 'LoadingCityId' || c === 'UnLoadingCityId') LgsB.pick(c === 'LoadingCityId' ? 'Loading City' : 'Un-Loading City', cities, cols('Description'), function (p) { r[c] = HRM.int(p.Id); grd.draw(); });
        if (c === 'BillWeightBaseId') LgsB.pick('Bill Weight Base', billBases, cols('Name'), function (p) { r.BillWeightBaseId = HRM.int(p.Id); billWeight(r); RecalculateFreight(r); grd.draw(); });
        if (c === 'RateUomId') LgsB.pick('Rate Uom', rateUoms, cols('Name'), function (p) {
            r.RateUomId = HRM.int(p.Id); r.QtyForRate = qtyForRate(r.RateUomId, HRM.num(r.BillWeight)); RecalculateFreight(r); grd.draw();
        });
    }
    function DeleteCustomerDetailRow(r, i) {
        if (HRM.int(r.Id) !== 0) { if (!HRM.ask('Are you sure to Delete?')) return; removed.push(r); }
        grd.remove(i);
    }

    // ------------------------------------------------------------------------ header
    function CmbTransporter_Leave() {
        return HRM.get(API + '/pending-po', { partyId: HRM.comboVal('CmbTransporter'), recId: RecId }).then(function (rows) {
            var po = (rows || []).map(function (r) {
                var c = function (k) { return HRM.col(r, k); };
                return { Id: c('Id'), DocNo: c('DocNo'), DocDate: c('DocDate'), DocDateText: c('DocDate') ? HRM.fmtDate(c('DocDate')) : '', RateBaseUomId: c('RateBaseUomId'), Rate: c('Rate'), Qty: c('Qty') };
            });
            LgsB.fillCols('CmbLogisticPoNo', po, 'Id', 'DocNo', ['DocDateText', 'Rate', 'Qty'], ['DocNo', 'DocDate', 'Rate', 'Qty'], { zero: '' });
            if (po.length) CmbLogisticPoNo_Leave();
        }).catch(HRM.fail);
    }
    function CmbLogisticPoNo_Leave() {
        HRM.enable('CmbRateUom', true); HRM.enable('txtFreightRate', true);
        var r = LgsB.row('CmbLogisticPoNo');
        if (HRM.comboVal('CmbLogisticPoNo') && r) {
            HRM.setVal('txtFreightRate', LgsB.net(r.Rate, '#,##.##'));
            HRM.setCombo('CmbRateUom', HRM.int(r.RateBaseUomId));
            HRM.enable('CmbRateUom', false); HRM.enable('txtFreightRate', false);
        }
    }
    function bindLists(d0) {
        cfg = d0.config || cfg;
        LgsB.fillCols('CmbTransporter', d0.suppliers || [], 'Id', 'CompanyName', ['PartyCode', 'CityName', 'CurrencyCode'], ['CompanyName', 'PartyCode', 'CityName', 'CurrencyCode'], { zero: '' });
        cities = (d0.cities || []).map(function (c) { return { Id: HRM.int(c.Id), Description: HRM.str(c.Description) }; });
        HRM.fill('CmbCityName', cities, 'Id', 'Description', { zero: '', keep: true });
        rateUoms = []; billBases = [];
        (d0.lookups || []).forEach(function (r) {
            var a = HRM.str(HRM.col(r, 'Activity')), o = { Id: HRM.int(HRM.col(r, 'Id')), Name: HRM.str(HRM.col(r, 'ReferenceName')) };
            if (a === 'BillWeightBase') billBases.push(o);
            else if (a === 'RateBaseUom') { o.Equivalent = HRM.num(HRM.col(r, 'OtherReference')); rateUoms.push(o); }
        });
        LgsB.fillCols('CmbRateUom', rateUoms, 'Id', 'Name', ['Equivalent'], ['Name', 'Equivalent'], { zero: '' });
        HRM.fill('CmbCustomerLoader', d0.customers || [], 'Id', 'ReferenceName', { zero: '', keep: true });
        LgsB.fillCols('CmbChargesToDrAccount', d0.chargesAccounts || [], 'Id', 'AccountTitle', ['AccountCode'], ['AccountTitle', 'AccountCode'], { zero: '' });
        if (HRM.int(cfg.chargesToAccount) > 0) HRM.setCombo('CmbChargesToDrAccount', cfg.chargesToAccount);
    }

    // ------------------------------------------------------------------------ Filters To Load Data / Apply Rate / Update City
    /** BtnShowLoader_Click: pending gate passes appended (one row per gate pass not yet in the grid). */
    P.BtnShowLoader = function (btn) {
        return HRM.busy(btn || 'BtnShowLoader', function () {
            return HRM.post(API + '/loader', { recId: RecId, fromDate: HRM.val('txtGpFromDateLoader'), toDate: HRM.val('txtGpToDateLoader'),
                fromDocNo: HRM.val('txtGpNoFromLoader'), toDocNo: HRM.val('txtGpNoToLoader'), customerId: HRM.comboVal('CmbCustomerLoader') }).then(function (rows) {
                if (!(rows || []).length) return;
                var seen = {};
                grd.rows().forEach(function (r) { seen[HRM.int(r.GPId)] = 1; });
                rows.forEach(function (dr) {
                    var c = function (k) { return HRM.col(dr, k); }, gp = HRM.int(c('GpId'));
                    if (seen[gp]) return; seen[gp] = 1;
                    grd.data.push({ Id: 0, DocDate: c('OutDateTime') || HRM.today(), GPId: gp, GpNo: c('GpSrNo'), GpDate: c('GpDate'),
                        DoId: c('DeliveryOrderId'), DoNo: c('DeliveryOrderNo'), DoDate: c('DeliveryOrderDate'), VehicleNo: c('VehicleNo'), BiltyNo: c('BiltyNo'),
                        BiltyDate: c('BiltyDate') || HRM.today(), NoOfBages: c('DoQty'), NetWeightDo: c('DoWeight'), NetWeightWB: c('WbNetWeight'),
                        BillWeightBaseId: 24, BillWeight: c('WbNetWeight'), BiltyFreight: c('GpBiltyFreight'), TotalBiltyFreight: c('GpBiltyFreight'),
                        NetCreditToTransporter: c('GpNetFreight') });
                });
                BindGrids();
            }).catch(HRM.fail);
        });
    };
    /** BtnApplyRate_Click. */
    P.BtnApplyRate = function () {
        var rows = grd.rows(); if (!rows.length) return;
        var poId = HRM.comboVal('CmbLogisticPoNo'), poNo = HRM.int(LgsB.text('CmbLogisticPoNo')), rate = HRM.num(HRM.val('txtFreightRate')), uom = HRM.comboVal('CmbRateUom');
        rows.forEach(function (r) {
            r.PoId = poId; r.PoNo = poNo;
            var q = qtyForRate(uom, HRM.num(r.BillWeight));
            if (poId > 0) { r.FreightRate = rate; r.RateUomId = uom; r.QtyForRate = q; }
            else {
                if (HRM.num(r.FreightRate) === 0) r.FreightRate = rate;
                if (HRM.num(r.RateUomId) === 0) r.RateUomId = uom;
                if (HRM.num(r.QtyForRate) === 0) r.QtyForRate = q;
            }
            RecalculateFreight(r);
        });
        BindGrids();                                                           // GridEditing
    };
    /** BtnUpdateCityInGrid_Click. */
    P.BtnUpdateCityInGrid = function () {
        var lc = HRM.checked('chkLoadingCity'), uc = HRM.checked('chkUnLoadingCity');
        if (!lc && !uc) { HRM.box('Please select Loading or UnLoading city option.'); return; }
        var rows = grd.rows();
        if (!rows.length) { HRM.box('Detail record not found.'); return; }
        var city = HRM.comboVal('CmbCityName'), l = city, u = city;
        if (city === 0) {
            l = HRM.int(rows[0].LoadingCityId); u = HRM.int(rows[0].UnLoadingCityId);
            if (lc && l === 0) { HRM.box('Loading City is empty in detail row no: 01'); return; }
            if (uc && u === 0) { HRM.box('UnLoading City is empty in detail row no: 01'); return; }
        }
        rows.forEach(function (r) { if (lc) r.LoadingCityId = l; if (uc) r.UnLoadingCityId = u; });
        grd.draw();
    };

    // ------------------------------------------------------------------------ New / Refresh / Save / Update / Delete
    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnUpdate', update); HRM.show('btnDelete', update); }
    function FromReset(docNo) {
        RecId = 0; removed = [];
        buttons(false); UpdateMode = false;
        HRM.setCombo('CmbTransporter', 0); HRM.setVal('txtRemarksHeader', ''); HRM.setCombo('CmbLogisticPoNo', 0);
        grd.clear(); BindGrids();
        HRM.setVal('txtFreightRate', ''); HRM.setVal('txtQtyForRate', '');
        HRM.enable('CmbRateUom', true); HRM.enable('txtFreightRate', true);
        HRM.focus('txtdocdate');
        return (docNo !== undefined ? Promise.resolve({ docNo: docNo }) : HRM.get(API + '/setup', { recId: 0 })).then(function (d0) {
            HRM.setVal('txtdocno', HRM.str(d0.docNo));
            return CmbTransporter_Leave();
        }).catch(HRM.fail);
    }
    P.btnnew = function () { return FromReset(); };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () {
            return HRM.get(API + '/refresh', { recId: RecId }).then(function (d0) { bindLists(d0); BindGrids(); return CmbTransporter_Leave(); }).catch(HRM.fail);
        });
    };
    function payload() {
        return { recId: RecId, docNo: HRM.val('txtdocno'), docDate: HRM.val('txtdocdate') + 'T' + HRM.nowTime() + ':00',
            transporterId: HRM.comboVal('CmbTransporter'), chargesToAccountId: HRM.comboVal('CmbChargesToDrAccount'), remarks: HRM.val('txtRemarksHeader'),
            rateUomId: HRM.comboVal('CmbRateUom'), freightRate: HRM.num(HRM.val('txtFreightRate')),
            rows: grd.rows(), removed: RecId > 0 ? removed : [] };
    }
    /** Insert(). */
    function Insert(btn) {
        if (!grd.rows().length) { HRM.box('Please add at least one record in the grid.'); return; }
        if (HRM.num(HRM.val('txtdocno')) === 0) { HRM.box('Doc No must be a non-zero number'); HRM.focus('txtdocno'); return; }
        if (!HRM.val('txtdocdate')) { HRM.box('Doc Date field is required'); HRM.focus('txtdocdate'); return; }
        if (!HRM.comboVal('CmbTransporter')) { HRM.box('Transporter field is required'); HRM.focus('CmbTransporter'); return; }
        if (!HRM.comboVal('CmbChargesToDrAccount')) {
            HRM.box('Please select a Charges To Account (Dr).\n\nIf the list is empty, go to Configuration, set the default Charges Account, and then click Refresh and then Select an Account.');
            HRM.focus('CmbChargesToDrAccount'); return;
        }
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var master = RecId;                                                     // Or.MasterDocId = RecId
        var preview = HRM.checked('ChkPreview') && rights.print, voucher = HRM.checked('chkVoucher') && rights.print;
        var win = preview && window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        var win2 = voucher && master > 0 && window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', payload()).then(function (res) {
                HRM.box(res.message);
                FromReset(res.docNo);
                /* FreightVoucherOutward_Slip(Or.FreightVoucherOutwardId): SaveMulti's result (the last voucher id) as the master id - as in the desktop. */
                if (preview && window.CrystalPrint) window.CrystalPrint.open('lgsb-1305-01', { id: res.id }, null, win);
                if (voucher) {                                                  // PrintVoucher(Or.MasterDocId): 0 on a new document
                    if (!(master > 0)) { HRM.box('Record Not Found For Display'); return; }
                    printVoucher(master, null, win2);
                }
            }).catch(function (e) {
                if (win) window.CrystalPrint.release(win);
                if (win2) window.CrystalPrint.release(win2);
                HRM.fail(e);
            });
        }, 'fm-save');
    }
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnUpdate = function (btn) { return Insert(btn || 'btnUpdate'); };
    P.btnDelete = function (btn) {
        if (!(RecId > 0)) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return HRM.post(API + '/delete?id=' + RecId, {}).then(function (r) { HRM.box(r.message); FromReset(r.docNo); }).catch(HRM.fail);
        });
    };
    function ReadById(id) {
        return HRM.loading(FromReset().then(function () {
            RecId = id;
            return HRM.get(API + '/by-id', { id: id });
        }).then(function (o) {
            var h = o.header, c = function (k) { return HRM.col(h, k); };
            show('tabPage1');
            UpdateMode = true; buttons(true);
            HRM.setVal('txtdocdate', HRM.day(c('MasterDocDate')));
            HRM.setVal('txtdocno', HRM.str(c('MasterDocNo')));
            HRM.setCombo('CmbTransporter', HRM.int(c('transporterId')));
            return CmbTransporter_Leave().then(function () {
                HRM.setCombo('CmbLogisticPoNo', HRM.int(c('PurchaseOrderHeaderId'))); CmbLogisticPoNo_Leave();
                HRM.setVal('txtRemarksHeader', HRM.str(c('MasterDocRemarks')));
                HRM.setCombo('CmbChargesToDrAccount', HRM.int(c('chargeToDrAccountId')));
                grd.set(o.rows || []);
                BindGrids();
                HRM.focus('txtdocdate');
            });
        }).catch(HRM.fail));
    }
    P.btnPrint = function (btn) {                                              // FreightVoucherOutward_Slip_1305_01(RecId)
        return LgsB.print('lgsb-1305-01', { id: RecId }, function () { return HRM.get(API + '/print-check', { id: RecId }); }, btn || 'btnPrint');
    };
    /** PrintVoucher(PrintId): FreightVoucherOutward_GetVoucherHeadIds -> ANewAcRptPaymentReceiptsVoucherSlip_102(0, HeaderIds). */
    function printVoucher(id, btn, reserved) {
        var win = reserved || (window.CrystalPrint ? window.CrystalPrint.reserve() : null);
        return HRM.busy(btn, function () {
            return HRM.get(API + '/voucher-ids', { id: id }).then(function (v) {
                if (!window.CrystalPrint) { HRM.box('The print is not available.'); return; }
                return window.CrystalPrint.open('hrm-102', { ids: HRM.str(v.ids) }, null, win);
            }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
        });
    }
    P.btnPrintVoucher = function (btn) {
        if (!(RecId > 0)) { HRM.box('Record Not Found For Display'); return; }
        return printVoucher(RecId, btn || 'btnPrintVoucher');
    };
    var KEYS = [['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'],
        ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'],
        ['Ctrl+F10', 'For Open Attachments'], ['F1', 'For Combo Lookup on Current Column of any Focused Grid'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On on Doc Date'], ['Ctrl+ArrowRight', 'to change focus from one grid to another'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    P.btnshortcutkeys = function () { LgsB.shortcuts(KEYS); };

    // ------------------------------------------------------------------------ History tab
    function historyGrids() {
        grdHistory = new HRM.Grid('grdHistory', {
            columns: [LgsB.btnCol('Edit', 'Edit', 'Edit', 50), LgsB.btnCol('Print', 'Print', 'Print', 50), LgsB.btnCol('Voucher', 'Voucher', 'Voucher', 70),
                { key: 'Id', hidden: true }, { key: 'DocNo', caption: 'DocNo', width: 70 }, { key: 'DocDate', caption: 'DocDate', type: 'date' },
                { key: 'DocumentTypeId', hidden: true }, { key: 'TransporterId', hidden: true }, { key: 'TransporterAccountTitle', caption: 'TransporterAccountTitle', width: 160 },
                { key: 'PurchaseOrderHeaderId', hidden: true }, { key: 'PurchaseOrderNo', caption: 'PurchaseOrderNo', width: 70 },
                { key: 'ChargeToDrAccountId', hidden: true }, { key: 'ChargedToDrAccountTitle', caption: 'ChargedToDrAccountTitle', width: 160 },
                { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' }, { key: 'EntryUserName', caption: 'EntryUserName' },
                { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUserName', caption: 'ModifyUserName' },
                { key: 'IsApproved', caption: 'IsApproved', type: 'check' }, { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime' },
                { key: 'ApprovalUserName', caption: 'ApprovalUserName' }, { key: 'RemarksHeader', caption: 'RemarksHeader', width: 150 },
                { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }, { key: 'FreightVoucherOutwardId', hidden: true }],
            filterRow: true, emptyText: '',
            onSelect: function (r) { GetDetailGrdByHeadId(HRM.int(r.Id)); },
            onDouble: function (r) { if (!rights.update) { HRM.box("ypu don't have updae rights..."); return; } ReadById(HRM.int(r.Id)); },
            onCode: function () { LgsB.noAttachments(); }
        });
        LgsB.trackColumn(grdHistory);
        function act(r, a) {
            if (a === 'Print') { if (!rights.print) { HRM.box("you don't have print rights..."); return; } LgsB.print('lgsb-1305-01', { id: HRM.int(r.Id) }, function () { return HRM.get(API + '/print-check', { id: r.Id }); }); }
            if (a === 'Voucher') { if (!rights.print) { HRM.box("you don't have print rights..."); return; } printVoucher(HRM.int(r.Id)); }
            if (a === 'Edit') { if (!rights.update) { HRM.box("you don't have update rights..."); return; } ReadById(HRM.int(r.Id)); }
            if (a === 'NoOfAttachments') LgsB.noAttachments();
        }
        LgsB.onButton(grdHistory, function (r, a) { act(r, a); });
        LgsB.gridKeys(grdHistory, {
            'ctrl+enter': function (r) { if (!rights.update) { HRM.box("you don't have update rights..."); return; } ReadById(HRM.int(r.Id)); },
            'ctrl+space': function (r) { act(r, LgsB.lastCol(grdHistory)); }
        });
        grdHistoryDetail = new HRM.Grid('grdHistoryDetail', { columns: columns(false, true), totals: true, emptyText: '' });
    }
    function GetDetailGrdByHeadId(id) {
        HRM.get(API + '/history-detail', { id: id }).then(function (rows) { grdHistoryDetail.set(rows || []); }).catch(HRM.fail);
    }
    P.btnshow = function (btn) {                                               // HistoryGridFill: one row per MasterDocId
        return HRM.busy(btn || 'btnshow', function () {
            return HRM.post(API + '/history', LgsB.historyFilter()).then(function (rows) {
                var seen = {}, out = [];
                (rows || []).forEach(function (r) {
                    var c = function (k) { return HRM.col(r, k); }, id = HRM.int(c('MasterDocId'));
                    if (seen[id]) return; seen[id] = 1;
                    out.push({ Id: id, DocNo: c('MasterDocNo'), DocDate: c('MasterDocDate'), DocumentTypeId: c('DocumentTypeId'), TransporterId: c('TransporterId'),
                        TransporterAccountTitle: c('TransporterAccountTitle'), PurchaseOrderHeaderId: c('PurchaseOrderHeaderId'), PurchaseOrderNo: c('PurchaseOrderNo'),
                        ChargeToDrAccountId: c('ChargeToDrAccountId'), ChargedToDrAccountTitle: c('ChargedToDrAccountTitle'), EntryDate: c('EntryDate'),
                        EntryUserName: c('EntryUserName'), ModifyDate: c('ModifyDate'), ModifyUserName: c('ModifyUserName'), IsApproved: c('IsApproved'),
                        ApprovedDate: c('ApprovedDate'), ApprovalUserName: c('ApprovalUserName'), RemarksHeader: c('RemarksHeader'),
                        NoOfAttachments: c('NoOfAttachments'), FreightVoucherOutwardId: c('FreightVoucherOutwardId') });
                });
                grdHistory.set(out);
                grdHistoryDetail.clear();
            }).catch(HRM.fail);
        });
    };
    P.btnHistoryRefresh = function () { HRM.setCombo('cmbDateTypeHistory', 0); grdHistory.clear(); grdHistoryDetail.clear(); HRM.focus('cmbDateTypeHistory'); };
    P.BtnRefreshHistory = function () { LgsB.dateType('cmbDateTypeHistory', 'FromDateHistory', 'ToDateHistory', LgsB.yearStart); };

    // ------------------------------------------------------------------------ form load
    var show = LgsB.tabs(function (id) { HRM.focus(id === 'tabPage3' ? 'cmbDateTypeHistory' : 'txtdocdate'); });
    function init() {
        BindGrids();
        HRM.setVal('txtdocdate', HRM.today());
        HRM.setVal('txtGpFromDateLoader', HRM.today()); HRM.setVal('txtGpToDateLoader', HRM.today());
        LgsB.onLeave('CmbTransporter', CmbTransporter_Leave);
        LgsB.onLeave('CmbLogisticPoNo', CmbLogisticPoNo_Leave);
        HRM.loading(HRM.get(API + '/setup', { recId: 0 })).then(function (d0) {
            rights = d0.rights || {};
            LgsB.rights(rights, { save: 'btnsave', update: 'btnUpdate', delete: 'btnDelete', print: 'btnPrint' });
            cfg = d0.config || {};
            if (RecId === 0) HRM.setVal('txtdocno', HRM.str(d0.docNo));
            bindLists(d0);
            BindGrids();
            historyGrids();
            LgsB.dateType('cmbDateTypeHistory', 'FromDateHistory', 'ToDateHistory', d0.yearStart);
            HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -(HRM.int(cfg.defaultDays) > 0 ? HRM.int(cfg.defaultDays) : 3)));
            HRM.setVal('ToDateHistory', HRM.today());
            HRM.focus('txtdocdate');
            var id = HRM.int(HRM.param('id'));
            if (id > 0) return ReadById(id);
            return CmbTransporter_Leave();
        }).catch(HRM.fail);
        HRM.footer(function () { show('tabPage3'); HRM.$('boxHistory').scrollIntoView({ block: 'nearest' }); });
        HRM.keys({
            'ctrl+t': function () { if (LgsB.activeTab() === 'tabPage3') show('tabPage1'); else show('tabPage3'); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+alt+control': P.btnshortcutkeys, 'ctrl+alt+alt': P.btnshortcutkeys,
            'ctrl+shift+delete': function () { if (LgsB.activeTab() === 'tabPage1' && rights.delete && !HRM.$('btnDelete').disabled) P.btnDelete(); },
            'ctrl+s': function () {
                if (LgsB.activeTab() === 'tabPage3') { P.btnshow(); return; }
                if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave();
            },
            'ctrl+u': function () { if (LgsB.activeTab() === 'tabPage1' && HRM.visible('btnUpdate') && !HRM.$('btnUpdate').disabled && UpdateMode) P.btnUpdate(); },
            'ctrl+p': function () { if (LgsB.activeTab() === 'tabPage1' && HRM.visible('btnPrint') && !HRM.$('btnPrint').disabled) P.btnPrint(); },
            'ctrl+n': function () { if (LgsB.activeTab() === 'tabPage1') P.btnnew(); },
            'ctrl+r': function () { if (LgsB.activeTab() === 'tabPage1') P.btnRefresh(); },
            'ctrl+f5': function () { if (LgsB.activeTab() === 'tabPage1') HRM.focus('txtdocdate'); },
            'ctrl+f10': function () { if (LgsB.activeTab() === 'tabPage1') LgsB.noAttachments(); },
            'ctrl+arrowdown': function () { var t = LgsB.activeTab() === 'tabPage3' ? 'grdHistoryDetail' : 'grdVoucher'; var tr = HRM.$(t).querySelector('tbody tr'); if (tr) tr.focus(); },
            'ctrl+arrowup': function () { if (LgsB.activeTab() === 'tabPage3') { var tr = HRM.$('grdHistory').querySelector('tbody tr'); if (tr) tr.focus(); } else HRM.focus('txtdocdate'); }
        });
    }
    init();
})();
