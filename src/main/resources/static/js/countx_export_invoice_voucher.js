/* Export Voucher - Architecture.WinApp.Account_Definition.frmExportInvoiceVoucher (screen 40, DocumentTypeId 27).
   Event-for-event port of the form; ":NNN" in comments = line in frmExportInvoiceVoucher.cs. */
(function ($) {
    'use strict';
    var API = '/accounts/export-invoice-voucher/api';
    var RecId = 0, Approved = false, ExImInvoiceId = 0, CustomerGlAcId = 0, UpdateMode = false;
    var updateIndex = -1;                    // updateDetailIndexCommissionInfo
    var rights = { save: true, update: true, del: true, print: true, viewAll: true };
    var cfg = { fifo: false, commissionTab: false, cgs: false, jobWise: false, tolerance: 0, dec: { amount: 2, rate: 2, fcyAmount: 2 } };
    var lstRemove = [];                      // lstRemoveRecord
    var att = { keep: [], add: [], removed: 0, list: [] };   // AT (Attachment form)
    var dtBuyers = [], dtCurrencies = [], dtSales = [], histBound = false, defaultDays = 0, prog = 0;
    var HEAD = 'frmExportInvoiceVoucher';

    function busy(btn, on) { var $b = $(btn); if (on) $b.prop('disabled', true).addClass('btn-busy'); else $b.removeClass('btn-busy').prop('disabled', false); }
    function say(m) { if (m != null && m !== '') window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function failText(x) {
        try { var j = x.responseJSON || JSON.parse(x.responseText); if (j && j.message) return j.message; } catch (e) { /* ignore */ }
        return x && x.status ? 'HTTP ' + x.status : 'Request failed';
    }
    function call(method, path, data) {
        return new Promise(function (resolve, reject) {
            var o = { url: API + path, type: method, dataType: 'json', cache: false };
            if (method === 'POST') { o.contentType = 'application/json'; o.data = JSON.stringify(data); } else o.data = data || {};
            $.ajax(o).done(function (r) { resolve(r); }).fail(function (x) { reject(new Error(failText(x))); });
        });
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function isoDate(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function toDouble(s) { var t = String(s == null ? '' : s).replace(/,/g, '').trim(); if (t === '') return 0; var n = Number(t); return isFinite(n) ? n : 0; }
    function toInt(s) { var t = String(s == null ? '' : s).trim(); if (!/^[+-]?\d+$/.test(t)) return 0; var n = parseInt(t, 10); return (n > 2147483647 || n < -2147483648) ? 0 : n; }
    function grp(s) { var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }
    /* "#,##0.####" style: up to d decimals, trailing zeros dropped */
    function fmtUp(v, d) {
        var n = toDouble(v), f = Math.pow(10, d), s = (Math.round(Math.abs(n) * f) / f).toFixed(d);
        if (d > 0) s = s.replace(/\.?0+$/, '');
        return (n < 0 ? '-' : '') + grp(s);
    }
    /* "#,##0.00" style: exactly d decimals */
    function fmtFix(v, d) { var n = toDouble(v); return (n < 0 ? '-' : '') + grp((Math.round(Math.abs(n) * Math.pow(10, d)) / Math.pow(10, d)).toFixed(d)); }
    function fmtFcy(v) { return toDouble(v) === 0 ? '0' : fmtFix(v, cfg.dec.fcyAmount); }    // stringFormatbothForFcy
    function fmtRs(v) { return fmtFix(v, cfg.dec.amount); }                                   // stringFormatsingle
    function dmy(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v == null ? '' : v)); return m ? m[3] + '/' + m[2] + '/' + m[1] : (v == null ? '' : String(v)); }
    function dmyT(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[ T](\d{2}):(\d{2})/.exec(String(v == null ? '' : v));
        if (!m) return dmy(v);
        var h = +m[4], ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 === 0 ? 12 : h % 12;
        return m[1] + '-' + m[2] + '-' + m[3] + ' ' + pad(h) + ':' + m[5] + ' ' + ap;
    }
    function dateOnly(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v == null ? '' : v)); return m ? m[1] : ''; }

    /* ---------------------------------------------------------------- combos */
    function comboVal(sel) { var v = $(sel).val(); return v == null ? '' : String(v); }
    function comboText(sel) { var o = $(sel).find('option:selected'); return o.length && o.val() !== '' ? o.text() : ''; }
    function setCombo(sel, v) {
        var s = v == null ? '' : String(v);
        if (!$(sel).find('option').filter(function () { return this.value === s; }).length) s = '';
        prog++; try { $(sel).val(s).trigger('change'); } finally { prog--; }
    }
    function clearCombo(sel) { prog++; try { $(sel).val('').trigger('change'); } finally { prog--; } }
    function fill(sel, rows, vk, tk) { AccF.fillSelect(sel, (rows || []).map(function (r) { return { V: r[vk], T: r[tk] }; }), 'V', 'T', null); clearCombo(sel); }
    function focus(sel) { try { AccF.focus(sel); } catch (e) { $(sel).trigger('focus'); } }
    function setEnabled(sel, on) { var c = AccF.ctl(sel); $(sel).prop('disabled', !on); $(c).css('pointer-events', on ? '' : 'none').css('opacity', on ? '' : '.7'); }
    function dtRow(list, key, val) { for (var i = 0; i < list.length; i++) if (String(list[i][key]) === String(val)) return list[i]; return null; }

    /* ---------------------------------------------------------------- grids */
    var pendCols = [
        { key: 'Edit', caption: 'Edit', button: 'Edit', width: 40, frozen: true },
        { key: 'Id', hidden: true }, { key: 'SupplierCustomerId', hidden: true }, { key: 'Customer', caption: 'Customer', width: 200 },
        { key: 'Consignee', caption: 'Consignee', width: 180 }, { key: 'ContractNo', caption: 'ContractNo', width: 120 }, { key: 'InvoiceNo', caption: 'InvoiceNo', width: 110 },
        { key: 'InvoiceDate', caption: 'InvoiceDate', width: 85, format: function (v) { return dmy(v); } }, { key: 'InvoiceStatusId', hidden: true },
        { key: 'InvoiceDispatchStatus', caption: 'InvoiceDispatchStatus', width: 130 }, { key: 'FcurrencyId', hidden: true },
        { key: 'FcyCode', caption: 'FcyCode', width: 60 }, { key: 'ExchangeRate', caption: 'ExchangeRate', width: 85, num: true },
        { key: 'FcyAmount', caption: 'FcyAmount', width: 100, num: true, format: function (v) { return fmtUp(v, 4); } }, { key: 'DeliveryTerm', caption: 'DeliveryTerm', width: 90 },
        { key: 'NoOfContainers', caption: 'NoOfContainers', width: 90, num: true }, { key: 'InvoiceNetWeight', caption: 'InvoiceNetWeight', width: 100, num: true, format: function (v) { return fmtUp(v, 4); } },
        { key: 'DispatchWeight', caption: 'DispatchWeight', width: 100, num: true, format: function (v) { return fmtUp(v, 4); } },
        { key: 'DoNetWeight', caption: 'DoNetWeight', width: 100, num: true, format: function (v) { return fmtUp(v, 4); } },
        { key: 'WbGrossWeight', caption: 'WbGrossWeight', width: 100, num: true, format: function (v) { return fmtUp(v, 4); } },
        { key: 'ChartOfAccountId', hidden: true }, { key: 'CreditAccountId', hidden: true }, { key: 'FI_Number', caption: 'FI_Number', width: 110 },
        { key: 'FI_Date', caption: 'FI_Date', width: 85, format: function (v) { return dmy(v); } }, { key: 'SpecialApprovedStatus', caption: 'SpecialApprovedStatus', width: 120, hidden: true }
    ];
    var gPending = CJG.create({ table: '#grdPending', nav: '#grdPendNav', autoResize: true, columns: pendCols,
        onButton: function (col, row) { if (col === 'Edit') pendingRecordBind(row); },
        onDblClick: function (row) { /* grdPending_DoubleClick is not wired on the form */ } });

    function numCol(k, c, w, extra) { var o = { key: k, caption: c, width: w, num: true, format: function (v) { return fmtUp(v, 4); } }; if (extra) for (var p in extra) o[p] = extra[p]; return o; }
    var gridCols = [
        { key: 'EntryTypeId', hidden: true }, { key: 'Id', hidden: true }, { key: 'ExImInvoiceDetailId', hidden: true }, { key: 'ExImInvoiceId', hidden: true },
        { key: 'ContractId', hidden: true }, { key: 'ContractNo', caption: 'ContractNo', width: 110 }, { key: 'InvoiceNo', hidden: true }, { key: 'ItemId', hidden: true },
        { key: 'ItemName', caption: 'ItemName', width: 200 }, { key: 'WarehouseId', hidden: true }, { key: 'CropYearId', hidden: true }, { key: 'CropYear', caption: 'CropYear', width: 70 },
        { key: 'LotJobId', hidden: true }, { key: 'JobLotCode', caption: 'Job Lot', width: 110 }, { key: 'PackingMaterialId', hidden: true },
        { key: 'PackMaterilaType', caption: 'Packing Type', width: 120 }, numCol('Qty', 'Qty', 80), { key: 'OuterQtyUomId', hidden: true }, { key: 'ItemUOM', caption: 'ItemUOM', width: 80 },
        numCol('NetWeight', 'NetWeight', 100), numCol('RatePrice', 'Rate', 90), { key: 'RateUomId', hidden: true }, { key: 'RateUOM', caption: 'RateUOM', width: 80 },
        numCol('FcAmount', 'FcAmount', 110), numCol('StockWeight', 'StockWeight', 100), numCol('CostRate', 'CostRate', 90), numCol('StockValue', 'StockValue', 100),
        numCol('AddLessAmount', 'AddLessAmount', 100, { sum: true, totalFormat: function (t) { return fmtUp(t, 4); } })
    ];
    var gGrid = CJG.create({ table: '#grd', nav: '#grdNav', autoResize: true, columns: gridCols });
    var payCols = [
        { key: 'Id', hidden: true }, { key: 'PaymentTermId', hidden: true }, { key: 'PaymentTerm', caption: 'PaymentTerm', width: 350 }, { key: 'DocumentTypeId', hidden: true },
        { key: 'ExImEFormRegistrationId', hidden: true }, { key: 'FinancialInstrumentNo', caption: 'FinancialInstrumentNo', width: 350 },
        numCol('PctOfTotal', '%OfTotal', 90), numCol('FcyAmount', 'FcyAmount', 110), { key: 'DueDays', caption: 'DueDays', width: 70, num: true }, { key: 'Remarks', caption: 'Remarks', width: 250 }
    ];
    var gPay = CJG.create({ table: '#grdPaymentTermDetail', nav: '#grdPayNav', autoResize: true, columns: payCols });
    var chgCols = [
        { key: 'Id', hidden: true }, { key: 'ChargesItemId', hidden: true }, { key: 'ChargesItem', caption: 'ChargesItem', width: 350 },
        numCol('AddAmount', 'AddAmount', 100), numCol('LessAmount', 'LessAmount', 100), { key: 'ChargesAccountId', hidden: true },
        { key: 'ChargesAccount', caption: 'ChargesAccount', width: 200 }, { key: 'Remarks', caption: 'Remarks', width: 350 }
    ];
    var gChg = CJG.create({ table: '#grdOtherChargesDetail', nav: '#grdChgNav', autoResize: true, columns: chgCols });
    var commCols = [
        { key: 'Delete', caption: 'X', button: 'X', width: 20, frozen: true }, { key: 'Id', hidden: true }, { key: 'SalesPersonId', hidden: true },
        { key: 'SalesPerson', caption: 'SalesPerson', width: 220 }, { key: 'CommissionTypeId', hidden: true }, { key: 'CommissionType', caption: 'CommissionType', width: 150 },
        numCol('Rate', 'Rate', 90), { key: 'RateUom', caption: 'RateUom', width: 100 }, numCol('FcyAmount', 'FcyAmount', 110), numCol('ExchangeRate', 'ExchangeRate', 100),
        numCol('LcyAmount', 'LcyAmount', 110), { key: 'Remarks', caption: 'Remarks', width: 300 }
    ];
    var gComm = CJG.create({ table: '#grdCommissionInfo', nav: '#grdCommNav', autoResize: true, columns: commCols,
        onButton: function (col, row) { if (col === 'Delete') commissionDeleteClick(row); },
        onDblClick: function (row) { commissionDoubleClick(row); } });
    var histCols = [
        { key: 'Print', caption: 'Print', button: 'Print', width: 50, frozen: true }, { key: 'Edit', caption: 'Edit', button: 'Edit', width: 50, frozen: true },
        { key: 'Id', hidden: true }, { key: 'VoucherHeadId', hidden: true }, { key: 'DocumentTypeId', hidden: true },
        { key: 'VoucherCode', caption: 'VoucherCode', width: 80 }, { key: 'VoucherDate', caption: 'VoucherDate', width: 90 }, { key: 'InvoiceNo', caption: 'InvoiceNo', width: 140 },
        { key: 'CustomerName', caption: 'CustomerName', width: 200 }, { key: 'Remarks', caption: 'Remarks', width: 350 }, { key: 'NoOfContainers', caption: 'NoOfContainers', width: 90, num: true },
        numCol('NetWeight', 'NetWeight', 110), { key: 'CurrencyCode', caption: 'CurrencyCode', width: 60 }, numCol('ExchangeRate', 'ExchangeRate', 90), numCol('FcyAmount', 'FcyAmount', 110),
        numCol('VoucherAmount', 'VoucherAmount', 110), { key: 'EntryDate', caption: 'EntryDate', width: 130, format: function (v) { return dmyT(v); } },
        { key: 'EntryUser', caption: 'EntryUser', width: 130 }, { key: 'ModifyDate', caption: 'ModifyDate', width: 130, format: function (v) { return dmyT(v); } },
        { key: 'ModifyUser', caption: 'ModifyUser', width: 130 }, { key: 'ApprovalStatus', caption: 'ApprovalStatus', width: 110 },
        { key: 'NoOfAttachments', caption: 'NoOfAttachments', width: 90, link: true, num: true }
    ];
    var gHist = CJG.create({ table: '#DataGridHistory', nav: '#histNav', autoResize: true, columns: histCols,
        onButton: function (col, row) { if (col === 'Edit') readById(row.Id); else if (col === 'Print') exportInvoiceVoucherSlip(row.Id); },
        onLink: function (col, row) { if (col === 'NoOfAttachments') showAttachments(row.Id); },
        onDblClick: function (row) { readById(row.Id); },
        onCurrent: function (row) { if (row) historyDetail(row.Id); } });
    var detCols = [
        { key: 'ContractNo', caption: 'ContractNo', width: 110 }, { key: 'InvoiceNo', caption: 'InvoiceNo', width: 110 }, { key: 'ItemName', caption: 'ItemName', width: 200 },
        { key: 'CropYear', caption: 'CropYear', width: 70 }, { key: 'JobLotCode', caption: 'Job Lot', width: 110 }, { key: 'PackMaterilaType', caption: 'Packing Type', width: 120 },
        numCol('Qty', 'Qty', 80, { sum: true, totalFormat: function (t) { return fmtUp(t, 4); } }), { key: 'ItemUOM', caption: 'ItemUOM', width: 80 },
        numCol('NetWeight', 'NetWeight', 100, { sum: true, totalFormat: function (t) { return fmtUp(t, 4); } }), numCol('RatePrice', 'Rate', 90), { key: 'RateUOM', caption: 'RateUOM', width: 80 },
        numCol('FcAmount', 'FcAmount', 110, { sum: true, totalFormat: function (t) { return fmtUp(t, 4); } }), numCol('StockWeight', 'StockWeight', 100, { sum: true, totalFormat: function (t) { return fmtUp(t, 4); } }),
        numCol('CostRate', 'CostRate', 90), numCol('StockValue', 'StockValue', 100, { sum: true, totalFormat: function (t) { return fmtUp(t, 4); } }),
        numCol('AddLessAmount', 'AddLessAmount', 100, { sum: true, totalFormat: function (t) { return fmtUp(t, 4); } })
    ];
    var gDet = CJG.create({ table: '#grdDetail', nav: '#histDetNav', autoResize: true, columns: detCols });
    function colByKey(cols, k) { for (var i = 0; i < cols.length; i++) if (cols[i].key === k) return cols[i]; return null; }

    /* grd_ColumnButton / CellUpdated (:grd_CellUpdated): CostRate editable when !CGSFlag && !FIFOCGS */
    $('#grd').on('click', 'tbody td[data-k="CostRate"]', function () {
        if (cfg.cgs || cfg.fifo) return;
        var $td = $(this); if ($td.find('input').length) return;
        var i = +$td.closest('tr').data('i'), row = gGrid.view()[i]; if (!row) return;
        var $in = $('<input type="text" style="width:100%;box-sizing:border-box;border:1px solid #316ac5;font:12px Verdana,sans-serif;height:20px">').val(String(toDouble(row.CostRate)));
        $td.addClass('ed').empty().append($in);
        $in.on('keydown', function (e) {
            if (e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); $in.blur(); } else if (e.key === 'Escape') { $in.data('esc', 1); $in.blur(); }
            else if (!e.ctrlKey && !e.altKey && e.key.length === 1 && !/[\d.]/.test(e.key)) e.preventDefault();
        }).on('blur', function () {
            if (!$in.data('esc')) {
                row.CostRate = toDouble($in.val());
                if (toDouble(row.StockWeight) > 0 && toDouble(row.CostRate) > 0) row.StockValue = toDouble(row.StockWeight) * toDouble(row.CostRate);
            }
            gGrid.rerender();
        }).trigger('focus').trigger('select');
    });

    /* ---------------------------------------------------------------- amounts (:2447 AmountCalculation / :2465 ProportionateAddLessAmount) */
    function sumCol(rows, k) { return rows.reduce(function (a, r) { return a + toDouble(r[k]); }, 0); }
    function amountCalculation() {
        try {
            var without = toDouble($('#txtFcyAmountWithoutAddLess').val()), addLess = toDouble($('#txtAddLessAmount').val());
            var total = without + addLess;
            $('#txtFcyAmount').val($('#txtFcyAmountWithoutAddLess').val() === '' && $('#txtAddLessAmount').val() === '' ? '' : fmtFcy(total));
            var rate = toDouble($('#txtExchangeRate').val());
            $('#txtRsAmount').val($('#txtFcyAmountWithoutAddLess').val() === '' && $('#txtAddLessAmount').val() === '' ? '' : fmtRs(total * rate));
            proportionate();
            calculateCommissionAmount();
        } catch (ex) { say(ex.message); }
    }
    function proportionate() {
        var rows = gGrid.rows(), totalAddLess = toDouble($('#txtAddLessAmount').val()), totalFc = sumCol(rows, 'FcAmount');
        rows.forEach(function (r) {
            var a = totalAddLess / totalFc * toDouble(r.FcAmount);
            r.AddLessAmount = isFinite(a) ? Math.round(a * 10000) / 10000 : 0;
        });
        gGrid.rerender();
    }
    function updateAddLessFromCharges() {
        var rows = gChg.rows();
        $('#txtAddLessAmount').val(fmtUp(sumCol(rows, 'AddAmount') - sumCol(rows, 'LessAmount'), 3));
        amountCalculation();
    }
    function exchangeRateChanged() { $('#txtCommissionExchangeRate').val($('#txtExchangeRate').val()); amountCalculation(); }
    function dueDaysChanged() {
        var t = $('#txtDueDays').val().trim(), d = new Date();
        if (t !== '') d.setDate(d.getDate() + toDouble(t));
        $('#txtDueDate').val(isoDate(d));
    }
    /* datVoucherDate_Leave :2512 */
    function voucherDateLeave() {
        var rows = gGrid.rows(); if (!rows.length) return;
        var body = { recId: RecId, voucherDate: $('#datVoucherDate').val(), rows: rows.map(function (r) {
            return { itemId: toInt(r.ItemId), lotJobId: toInt(r.LotJobId), cropYearId: toInt(r.CropYearId), warehouseId: toInt(r.WarehouseId) }; }) };
        return call('POST', '/rates', body).then(function (out) {
            rows.forEach(function (r, i) {
                var avg = toDouble(out[i] && out[i].rate);
                r.CostRate = avg; r.StockValue = avg * toDouble(r.StockWeight);
            });
            gGrid.rerender();
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- commission tab (:885-1320) */
    function commissionValidation() {
        if (toInt(comboVal('#CmbSalePerson')) === 0) { say('Sales Person field is required'); focus('#CmbSalePerson'); return false; }
        if (toInt(comboVal('#cmbCommissionType')) === 0) { say('Commission Type field is required'); focus('#cmbCommissionType'); return false; }
        if (toDouble($('#txtCommRate').val()) === 0) { say('Commission Rate Field is Required'); $('#txtCommRate').trigger('focus'); return false; }
        if (toInt(comboVal('#cmbCommissionType')) === 3 && $('#txtCommRateUom').val() === '') { say('Commission Rate Uom Field is Required'); $('#txtCommRateUom').trigger('focus'); return false; }
        if (toDouble($('#txtCommissionAmount').val()) === 0 || $('#txtCommissionAmount').val() === '') { say('Commission Fcy Amount Field is Required'); return false; }
        if (toDouble($('#txtCommissionExchangeRate').val()) === 0 || $('#txtCommissionExchangeRate').val() === '') { say('Exchange Rate Field is Required'); $('#txtCommissionExchangeRate').trigger('focus'); return false; }
        return true;
    }
    function commissionRowFromForm(id) {
        return { Id: id, SalesPersonId: toInt(comboVal('#CmbSalePerson')), SalesPerson: comboText('#CmbSalePerson'), CommissionTypeId: toInt(comboVal('#cmbCommissionType')),
            CommissionType: comboText('#cmbCommissionType'), Rate: toDouble($('#txtCommRate').val()), RateUom: $('#txtCommRateUom').val().trim(),
            FcyAmount: toDouble($('#txtCommissionAmount').val()), ExchangeRate: toDouble($('#txtCommissionExchangeRate').val()),
            LcyAmount: toDouble($('#txtCommissionLcyAmount').val()), Remarks: $('#txtCommissionRemarks').val() };
    }
    function resetCommissionDetail() {
        updateIndex = -1; clearCombo('#CmbSalePerson'); clearCombo('#cmbCommissionType');
        $('#txtCommRate').val('0'); $('#txtCommRateUom').val('0').data('tag', '').prop('disabled', false);
        $('#txtCommissionAmount').val('0'); $('#txtCommissionLcyAmount').val('0');
    }
    function commissionButtons(editing) { $('#btnAddCommission').prop('hidden', editing); $('#btnUpdateCommision').prop('hidden', !editing); $('#btnCancelCommission').prop('hidden', !editing); }
    function btnAddCommissionClick() {
        try {
            if (!commissionValidation()) return;
            var rows = gComm.rows(), sp = toInt(comboVal('#CmbSalePerson'));
            for (var i = 0; i < rows.length; i++) if (sp > 0 && toInt(rows[i].SalesPersonId) === sp) throw new Error('Sale person already add in Grid Please select another sale person!');
            rows.push(commissionRowFromForm(0)); gComm.setRows(rows.slice());
            resetCommissionDetail(); focus('#CmbSalePerson');
        } catch (ex) { say(ex.message); }
    }
    function btnUpdateCommissionClick() {
        try {
            if (!commissionValidation()) return;
            var rows = gComm.rows(), sp = toInt(comboVal('#CmbSalePerson'));
            for (var i = 0; i < rows.length; i++) if (sp > 0 && updateIndex !== i && toInt(rows[i].SalesPersonId) === sp) throw new Error('Sales person already add in Grid Please selected another SalesPerson!');
            var keepId = rows[updateIndex].Id; rows[updateIndex] = commissionRowFromForm(keepId); gComm.setRows(rows.slice());
            commissionButtons(false); resetCommissionDetail(); focus('#CmbSalePerson');
        } catch (ex) { say(ex.message); }
    }
    function btnCancelCommissionClick() { commissionButtons(false); resetCommissionDetail(); }
    function commissionDeleteClick(row) {
        try {
            if (updateIndex !== -1) throw new Error('Reset Detail First...');
            var rows = gComm.rows(), ix = rows.indexOf(row); if (ix < 0) return;
            var id = toInt(row.Id);
            if (id > 0) {
                if (!ask('Are you sure to Delete?')) return;
                lstRemove.push({ Id: id, SalesPersonId: row.SalesPersonId, CommissionTypeId: row.CommissionTypeId, SalesPerson: row.SalesPerson, CommissionType: row.CommissionType,
                    Rate: row.Rate, RateUom: row.RateUom, FcyAmount: row.FcyAmount, ExchangeRate: row.ExchangeRate, LcyAmount: row.LcyAmount, Remarks: row.Remarks });
            }
            rows.splice(ix, 1); gComm.setRows(rows.slice());
        } catch (ex) { say(ex.message); }
    }
    function commissionDoubleClick(row) {
        try {
            var ix = gComm.rows().indexOf(row); if (ix < 0) return;
            updateIndex = ix;
            setCombo('#CmbSalePerson', toInt(row.SalesPersonId)); setCombo('#cmbCommissionType', toInt(row.CommissionTypeId)); cmbCommissionTypeChanged();
            $('#txtCommRate').val(String(toDouble(row.Rate))); $('#txtCommRateUom').val(String(row.RateUom == null ? '' : row.RateUom));
            $('#txtCommissionAmount').val(fmtUp(row.FcyAmount, 3)); $('#txtCommissionExchangeRate').val(fmtUp(row.ExchangeRate, 3));
            $('#txtCommissionLcyAmount').val(fmtUp(row.LcyAmount, 3)); $('#txtCommissionRemarks').val(String(row.Remarks == null ? '' : row.Remarks));
            commissionButtons(true); focus('#CmbSalePerson');
        } catch (ex) { say(ex.message); }
    }
    function cmbCommissionTypeChanged() {
        var id = toInt(comboVal('#cmbCommissionType'));
        if (id === 1) $('#txtCommRateUom').val('Lump Sum').prop('disabled', true);
        else if (id === 2) $('#txtCommRateUom').val('%').prop('disabled', true);
        else if (id === 3) { if (!$('#txtCommRateUom').data('tag')) $('#txtCommRateUom').val('1000'); $('#txtCommRateUom').prop('disabled', false); }
        else $('#txtCommRateUom').val('').prop('disabled', false);
        calculateCommissionAmount();
    }
    function calculateCommissionAmount() {
        try {
            var type = toInt(comboVal('#cmbCommissionType')), rate = toDouble($('#txtCommRate').val()), fcy = toDouble($('#txtFcyAmount').val());
            if (type > 0 && rate > 0 && fcy > 0) {
                var gridAmount = 0, gridRate = 0, kgs = toDouble($('#txtTotalWeight').val()), amt = 0;
                gComm.rows().forEach(function (r, i) { if (updateIndex !== i) { gridAmount += toDouble(r.FcyAmount); gridRate += toDouble(r.Rate); } });
                if (type === 1) {
                    if (gridAmount + rate > fcy) { var rem = fcy - gridAmount; rate = rem >= 0 ? rem : 0; }
                    amt = rate;
                } else if (type === 2) {
                    if (rate + gridRate > 99) { amt = fcy - gridAmount; rate = amt * 100 / fcy; $('#txtCommRate').val(fmtUp(rate, 4)); }
                    amt = fcy * rate / 100;
                    if (amt + gridAmount > fcy) { var rem2 = fcy - gridAmount; amt = rem2 > 0 ? rem2 * rate / 100 : 0; }
                } else if (type === 3) {
                    if (kgs > 0) {
                        var uom = toDouble($('#txtCommRateUom').val());
                        amt = kgs / uom * rate;
                        if (amt + gridAmount > fcy) {
                            var rem3 = fcy - gridAmount;
                            if (rem3 > 0) { amt = rem3; rate = amt * 100 / fcy; $('#txtCommRate').val(fmtUp(rate, 4)); amt = kgs / uom * rate; } else amt = 0;
                        }
                    } else amt = 0;
                }
                if (!isFinite(amt)) amt = 0;
                $('#txtCommissionAmount').val(fmtUp(amt, 4));
                var ex = toDouble($('#txtCommissionExchangeRate').val());
                $('#txtCommissionLcyAmount').val(fmtUp(ex > 0 ? ex * amt : 0, 4));
            } else { $('#txtCommissionAmount').val('0'); $('#txtCommissionLcyAmount').val('0'); }
        } catch (ex2) { say(ex2.message); }
    }

    /* ---------------------------------------------------------------- pending list (:719) and its Edit button (:803) */
    function saveVisible() { return !$('#btnSave').prop('hidden') && !$('#btnSave').prop('disabled'); }
    function fillPending(rows) { gPending.setRows(rows || []); }
    function pendingLoad() { return call('GET', '/pending').then(fillPending).catch(function (e) { say(e.message); }); }
    function pendingRecordBind(r) {
        try {
            if (!r || !saveVisible()) throw new Error('Record Not bind in grid');
            if (toInt(r.InvoiceStatusId) === 0) {
                throw new Error((cfg.tolerance === 0 ? 'The invoice status is pending because the dispatch has not been completed against the invoice'
                    : 'The invoice status is pending because the dispatch has not been completed against the invoice or Special Approval is Pending') + '. Please check');
            }
            return call('GET', '/bind', { id: r.Id, voucherDate: $('#datVoucherDate').val() }).then(function (d) {
                var h = d.header;
                setCombo('#CmbRefAccountId', h.SupplierCustomerId); CustomerGlAcId = toInt(h.ChartOfAccountId);
                $('#txtExchangeRate').val(h.ExchangeRate == null ? '' : String(h.ExchangeRate));
                $('#txtInvoiceNo').val(h.InvoiceNo); setCombo('#CmbCurrencyCode', h.FcurrencyId); ExImInvoiceId = toInt(h.Id);
                $('#datInvoiceDate').val(dateOnly(h.InvoiceDate)); $('#txtTotalWeight').val(h.InvoiceNetWeight == null ? '' : String(h.InvoiceNetWeight));
                if (toInt(h.CreditAccountId) > 0) setCombo('#CmbCreditAccount', h.CreditAccountId); else clearCombo('#CmbCreditAccount');
                gGrid.setRows(d.rows || []); gPay.setRows(payRows(d.paymentTerms)); gChg.setRows(d.otherCharges || []); gComm.setRows(d.commission || []);
                $('#txtCommissionExchangeRate').val($('#txtExchangeRate').val());
                $('#txtFcyAmountWithoutAddLess').val(fmtFix(sumCol(gGrid.rows(), 'FcAmount'), cfg.dec.fcyAmount));
                updateAddLessFromCharges();
            }).catch(function (e) { say(e.message); });
        } catch (ex) { say(ex.message); }
    }
    function payRows(rows) { return (rows || []).map(function (r) { var o = $.extend({}, r); o.PctOfTotal = r.PrcntOfTotal; return o; }); }

    /* ---------------------------------------------------------------- form state (:1591 Reset) */
    function setSaveMode(update) {
        $('#btnSave').prop('hidden', update); $('#btnUpdate').prop('hidden', !update); $('#btnDelete').prop('hidden', !update);
    }
    function applyRights() {
        $('#btnSave').prop('disabled', !rights.save); $('#btnUpdate').prop('disabled', !rights.update);
        $('#btnDelete').prop('disabled', !rights.del); $('#btnSlip').prop('disabled', !rights.print);
    }
    function reset() {
        att = { keep: [], add: [], removed: 0, list: [] }; lstRemove = []; UpdateMode = false; CustomerGlAcId = 0; RecId = 0; ExImInvoiceId = 0; Approved = false;
        $('#txtInvoiceNo').val(''); $('#datInvoiceDate').val(isoDate(new Date())); $('#txtTotalWeight').val('');
        clearCombo('#CmbCreditAccount'); clearCombo('#CmbCurrencyCode');
        $('#txtFcyAmountWithoutAddLess').val(''); $('#txtAddLessAmount').val(''); $('#txtFcyAmount').val(''); $('#txtExchangeRate').val(''); $('#txtRsAmount').val('');
        $('#txtRemarks').val(''); $('#txtDueDays').val('');
        gGrid.setRows([]); gPay.setRows([]); gChg.setRows([]); gComm.setRows([]);
        commissionButtons(false); resetCommissionDetail();
        setSaveMode(false);
        call('GET', '/next-code').then(function (n) { $('#txtVoucherCode').val(String(n)); }).catch(function (e) { say(e.message); });
        pendingLoad();
        $('#txtVoucherCode').trigger('focus');
    }
    function btnNewClick() { reset(); clearCombo('#CmbRefAccountId'); }

    /* ---------------------------------------------------------------- ReadById (:1949) */
    function showPage(which) {
        var f = which === 'Form';
        $('#pageForm').toggleClass('on', f); $('#pageHistory').toggleClass('on', !f); $('#tabForm').toggleClass('on', f); $('#tabHistory').toggleClass('on', !f);
        if (!f) $('#FromDateCpvHistory').trigger('focus'); else focus('#CmbCreditAccount');   // tabControl1_SelectedIndexChanged :2575
        try { gHist.rerender(); gPending.rerender(); gGrid.rerender(); } catch (e) { /* ignore */ }
    }
    function readById(id) {
        reset();
        return call('GET', '/voucher/' + id).then(function (d) {
            var h = d.header; if (!h) return;
            RecId = id; setSaveMode(true);
            if (cfg.specialApproval) { /* not reachable from the web hub */ }
            showPage('Form');
            $('#datVoucherDate').val(dateOnly(h.DocDate)); $('#txtVoucherCode').val(String(h.DocNo));
            setCombo('#CmbRefAccountId', h.SupplierCustomerId);
            if (toInt(h.CreditAccountId) > 0) setCombo('#CmbCreditAccount', h.CreditAccountId);
            if (toInt(h.CommissionExpenseAccountId) > 0) setCombo('#CmbCommissionExpenseAccount', h.CommissionExpenseAccountId);
            setCombo('#CmbCurrencyCode', h.CurrencyId); ExImInvoiceId = toInt(h.ExImInvoiceId); $('#txtInvoiceNo').val(h.InvoiceNo);
            $('#txtFcyAmountWithoutAddLess').val(fmtUp(toDouble(h.FcyAmountWithoutAddLess) > 0 ? h.FcyAmountWithoutAddLess : h.FcyAmount, 3));
            $('#txtAddLessAmount').val(fmtUp(h.AddLessAmount, 3)); $('#txtFcyAmount').val(fmtUp(h.FcyAmount, 3)); $('#txtExchangeRate').val(fmtUp(h.ExchangeRate, 3));
            $('#txtRsAmount').val(h.LcyAmount == null ? '' : String(h.LcyAmount)); $('#txtDueDays').val(String(h.DueDays));
            if (dateOnly(h.DueDate)) $('#txtDueDate').val(dateOnly(h.DueDate));
            $('#txtRemarks').val(h.Remarks || ''); Approved = !!h.IsApproved;
            if (d.invoice) { if (dateOnly(d.invoice.InvoiceDate)) $('#datInvoiceDate').val(dateOnly(d.invoice.InvoiceDate)); $('#txtTotalWeight').val(String(d.invoice.NetWeight)); }
            $('#txtCommissionExchangeRate').val($('#txtExchangeRate').val());   // txtTotalWeight_TextChanged -> txtExchangeRate_TextChanged (:4689)
            gGrid.setRows(d.rows || []); gPay.setRows(payRows(d.paymentTerms)); gChg.setRows(d.otherCharges || []); gComm.setRows(d.commission || []);
            updateAddLessFromCharges();
            att.list = (d.attachments || []).slice(); att.keep = att.list.map(function (a) { return a.Id; });
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- FormValidation (:677) / Insert (:1677) */
    function formValidation() {
        if (toInt(comboVal('#CmbRefAccountId')) === 0) { say('Account Field is Required'); focus('#CmbRefAccountId'); return false; }
        if (toInt(comboVal('#CmbCurrencyCode')) === 0) { say('Currency Code Field is Required'); focus('#CmbCurrencyCode'); return false; }
        var t = $('#txtInvoiceNo').val();
        if (t === '' || t === '0') { say('Invoice No Field is Required'); $('#txtInvoiceNo').trigger('focus'); return false; }
        t = $('#txtExchangeRate').val(); if (t === '' || t === '0') { say('ExchangeRate Field is Required'); $('#txtExchangeRate').trigger('focus'); return false; }
        t = $('#txtFcyAmount').val(); if (t === '' || t === '0') { say('FcyAmount Field is Required'); $('#txtFcyAmount').trigger('focus'); return false; }
        t = $('#txtRsAmount').val(); if (t === '' || t === '0') { say('RsAmount Field is Required'); $('#txtRsAmount').trigger('focus'); return false; }
        return true;
    }
    function buildBody(auto, recUpdateId) {
        var rows = gGrid.rows();
        return {
            recId: auto ? recUpdateId : RecId, autoUpdate: !!auto, exImInvoiceId: ExImInvoiceId, voucherDate: $('#datVoucherDate').val(), voucherCode: toInt($('#txtVoucherCode').val().trim()),
            creditAccountId: toInt(comboVal('#CmbCreditAccount')), commissionExpenseAccountId: toInt(comboVal('#CmbCommissionExpenseAccount')),
            exchangeRate: toDouble($('#txtExchangeRate').val()), fcyAmount: toDouble($('#txtFcyAmount').val()), rsAmount: toDouble($('#txtRsAmount').val()),
            fcyAmountWithoutAddLess: toDouble($('#txtFcyAmountWithoutAddLess').val()), addLessAmount: toDouble($('#txtAddLessAmount').val()),
            dueDate: $('#txtDueDate').val(), dueDays: toInt($('#txtDueDays').val()), remarks: $('#txtRemarks').val().trim(),
            rows: rows.map(function (r) { return { Id: toInt(r.Id), EntryTypeId: toInt(r.EntryTypeId), ExImInvoiceDetailId: toInt(r.ExImInvoiceDetailId), CostRate: toDouble(r.CostRate), AddLessAmount: toDouble(r.AddLessAmount) }; }),
            commission: gComm.rows().map(function (c) { return { Id: toInt(c.Id), SalesPersonId: toInt(c.SalesPersonId), SalesPerson: c.SalesPerson, CommissionTypeId: toInt(c.CommissionTypeId), CommissionType: c.CommissionType,
                Rate: toDouble(c.Rate), RateUom: c.RateUom, FcyAmount: toDouble(c.FcyAmount), ExchangeRate: toDouble(c.ExchangeRate), LcyAmount: toDouble(c.LcyAmount), Remarks: c.Remarks }; }),
            removedCommission: RecId > 0 || auto ? lstRemove.slice() : [],
            attachments: { keep: att.keep.slice(), add: att.add.slice(), removed: att.removed }
        };
    }
    /* returns Promise<boolean> (false = Insert returned false) */
    function insert(auto, recUpdateId) {
        try {
            if (!formValidation()) return Promise.resolve(false);
            if (auto) { if (!ask('Are you sure to Update?')) return Promise.resolve(false); }
            else if (RecId > 0) { if (!ask('Are you sure to Update?')) return Promise.resolve(false); }
            else if (!ask('Are you sure to Save?')) return Promise.resolve(false);
            if (toInt(comboVal('#CmbCommissionExpenseAccount')) === 0 && gComm.rows().length > 0) { focus('#CmbCommissionExpenseAccount'); throw new Error('Commission Expense Account required because commission is given in commission info tab!'); }
            var rows = gGrid.rows();
            if (rows.length <= 0) { say('Grid Fields Required'); return Promise.resolve(false); }
            var totalNet = 0, detAddLess = 0, cmp = toDouble($('#txtTotalWeight').val().trim());
            rows.forEach(function (r) {
                if (cfg.cgs && toDouble(r.CostRate) <= 0) throw new Error('CostRate Not Found Please Check');
                totalNet += toDouble(r.NetWeight); detAddLess += toDouble(r.AddLessAmount);
            });
            if (cfg.tolerance > 0) { if (Math.floor(cmp) > Math.floor(totalNet) + cfg.tolerance) throw new Error('TotalInvoiceWeight should be equal to SumOfGridNetWeight please Check'); }
            else if (Math.abs(Math.floor(cmp) - Math.floor(totalNet)) > 0.5) throw new Error('TotalInvoiceWeight not equal to SumOfGridNetWeight please Check');
            if (Math.abs(toDouble($('#txtAddLessAmount').val()) - detAddLess) > 0.3) throw new Error('Header AddLess:' + toDouble($('#txtAddLessAmount').val()) + ' not equal to detail AddLess:' + detAddLess + '. Please Check!');
            return call('POST', '/save', buildBody(auto, recUpdateId)).then(function (out) {
                if (auto) { reset(); return true; }
                var wasNew = RecId === 0, docNo = out && out.docNo != null ? out.docNo : $('#txtVoucherCode').val(), id = out && out.id;
                say((wasNew ? 'Voucher Save Successfully...[' : 'Voucher Update Successfully...[') + docNo + ']');
                var printIt = $('#ChkBox').prop('checked');
                reset(); gGrid.setRows([]);
                if (printIt) exportInvoiceVoucherSlip(id);
                return true;
            }).catch(function (e) { say(e.message); return true; });
        } catch (ex) { say(ex.message); return Promise.resolve(true); }
    }
    function saveClick() { RecId = 0; return insert(false, 0); }
    function updateClick() { try { if (RecId === 0) throw new Error('Rec Id Not found'); return insert(false, 0); } catch (ex) { say(ex.message); } }
    function deleteClick() {
        try {
            if (Approved) throw new Error('Record cannot be Delete, because ots approved');
            if (RecId > 0) {
                if (!ask('Are you sure to Delete?')) return;
                return call('POST', '/delete', { recId: RecId }).then(function () { say('Delete Record Successfully'); reset(); }).catch(function (e) { say(e.message); });
            }
            throw new Error('RecordId Not Found.....');
        } catch (ex) { say(ex.message); }
    }
    function exportInvoiceVoucherSlip(id) {
        try {
            if (!toInt(id)) { say('No Record Found For Display'); return; }
            window.printRpt('102-ANewAcRptExportInvoiceVoucherSlip.rpt', { voucherHeadId: toInt(id) }, $('#btnSlip')[0]);
        } catch (ex) { say(ex.message); }
    }

    /* ---------------------------------------------------------------- history (:2200 HistoryFill) */
    function histBody() {
        var kind = $('input[name="hdate"]:checked').val() || 'doc';
        return { dateType: kind, fromDate: $('#chkFrom').prop('checked') ? $('#FromDateCpvHistory').val() : null, toDate: $('#chkTo').prop('checked') ? $('#ToDateCpvHistory').val() : null,
            fromNo: toInt($('#txtFromDocNoCpv').val()), toNo: toInt($('#txtToDocNoCpv').val()), supplierCustomerId: toInt(comboVal('#cmbAccountTitleCpv')), approved: comboText('#cmbApprovedStatusCpv') };
    }
    function historyFill() {
        return call('POST', '/history', histBody()).then(function (rows) {
            if (rows && rows.length) gHist.setRows(rows); else { gHist.setRows([]); gDet.setRows([]); }
        }).catch(function (e) { say(e.message); });
    }
    function historyDetail(id) {      // GetDeailByHeaderId :2404
        return call('GET', '/history/detail/' + id).then(function (rows) { gDet.setRows(rows || []); }).catch(function (e) { say(e.message); });
    }
    function historyReset() {         // btnNewCpvHistory_Click :2587
        Approved = false; $('#FromDateCpvHistory').val(isoDate(new Date())); $('#ToDateCpvHistory').val(isoDate(new Date()));
        $('#txtFromDocNoCpv').val(''); $('#txtToDocNoCpv').val(''); clearCombo('#cmbAccountTitleCpv'); setCombo('#cmbApprovedStatusCpv', '1');
        gHist.setRows([]); gDet.setRows([]);
    }
    function historyCustomersBind() {
        return call('GET', '/history-customers').then(function (rows) { var cur = comboVal('#cmbAccountTitleCpv'); fill('#cmbAccountTitleCpv', rows, 'Id', 'name'); setCombo('#cmbAccountTitleCpv', cur); }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- attachments (AT / CommonServices.GetNoofAttachmentsByRefDocumentTypeID) */
    function openDlg(title, html) { $('#dlgTitle').text(title); $('#dlgBody').html(html); $('#ovDlg').addClass('on'); }
    function closeDlg() { $('#ovDlg').removeClass('on'); }
    function esc(s) { return String(s == null ? '' : s).replace(/[&<>"]/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' }[c]; }); }
    function showAttachments(id) {
        call('GET', '/attachments', { id: id }).then(function (list) {
            if (!list.length) { say('No Attachments Found'); return; }
            var h = '<table><tr><th>Attachment</th><th>Entry Date</th></tr>';
            list.forEach(function (a) { h += '<tr><td><a href="' + API + '/attachments/' + a.Id + '?id=' + id + '" target="_blank" rel="noopener">' + esc(a.CustomName || a.Attachment) + '</a></td><td>' + esc(dmyT(a.EntryDate)) + '</td></tr>'; });
            openDlg('Attachments', h + '</table>');
        }).catch(function (e) { say(e.message); });
    }
    function attDialog() {            // toolStripButton10_Click :2691 -> AT.Show()
        var h = '<table id="attTbl"><tr><th>File</th><th></th></tr>';
        att.list.forEach(function (a, i) { h += '<tr><td>' + esc(a.CustomName || a.Attachment) + '</td><td><button type="button" data-rm="e' + i + '">Remove</button></td></tr>'; });
        att.add.forEach(function (a, i) { h += '<tr><td>' + esc(a.name) + ' (new)</td><td><button type="button" data-rm="n' + i + '">Remove</button></td></tr>'; });
        h += '</table><p><input type="file" id="attFile" multiple></p>';
        openDlg('Attachments', h);
    }
    $(document).on('click', '#attTbl [data-rm]', function () {
        var k = $(this).data('rm'), i = +String(k).slice(1);
        if (k.charAt(0) === 'e') { var a = att.list.splice(i, 1)[0]; att.keep = att.list.map(function (x) { return x.Id; }); if (a) att.removed++; }
        else att.add.splice(i, 1);
        attDialog();
    });
    $(document).on('change', '#attFile', function () {
        var files = Array.prototype.slice.call(this.files || []), left = files.length;
        if (!left) return;
        files.forEach(function (f) {
            var fr = new FileReader();
            fr.onload = function () { att.add.push({ name: f.name, base64: String(fr.result).replace(/^data:[^,]*,/, '') }); if (--left === 0) attDialog(); };
            fr.onerror = function () { say('Could not read ' + f.name); if (--left === 0) attDialog(); };
            fr.readAsDataURL(f);
        });
    });

    /* ---------------------------------------------------------------- Records Update (:3124) */
    function recordsUpdate() {
        var hideBtn = function () { $('#btnRecordsUpdate').prop('hidden', true); };
        return call('GET', '/auto-update-ids').then(function (ids) {
            if (!ids || !ids.length) { hideBtn(); throw new Error('Ids Not Found For Update'); }
            if (!ask('Are you sure to Update?')) return;
            var ok = true, p = Promise.resolve();
            ids.forEach(function (rid) {
                p = p.then(function () { if (!ok) return; return readById(rid).then(function () { return insert(true, rid); }).then(function (r) { ok = r; }); });
            });
            return p.then(function () { say(ok ? 'Records updated successfully.' : 'Update stopped before all records were processed.'); hideBtn(); });
        }).catch(function (e) { hideBtn(); say(e.message); });
    }

    /* ---------------------------------------------------------------- shortcut keys (:2976) and KeyDown (:2897) */
    function shortcutKeys() {
        var rows = [['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+S', 'For Save'], ['Ctrl+E', 'For Close'], ['Ctrl+P', 'For Print '],
            ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus On Debit Account in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        var h = '<table><tr><th>KeyCombination</th><th>Description</th></tr>';
        rows.forEach(function (r) { h += '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; });
        openDlg('Short Cut Keys', h + '</table>');
    }
    function onHistoryPage() { return $('#pageHistory').hasClass('on'); }
    function nextField(el) {
        var all = $('#pageForm, #pageHistory').filter('.on').find('input:not([type=hidden]):not([type=checkbox]):not([type=radio]), select, textarea')
            .filter(function () { return !this.disabled && !this.readOnly && this.tabIndex !== -1 && $(this).is(':visible'); }).toArray();
        var i = all.indexOf(el); if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }
    $(document).on('keydown', function (e) {
        try {
            if ($('#ovDlg').hasClass('on')) { if (e.key === 'Escape') closeDlg(); return; }
            $('#btnRecordsUpdate').prop('hidden', true);
            var t = e.target, tag = (t.tagName || '').toLowerCase(), inGrid = $(t).closest('.cjg').length > 0;
            if (e.key === 'Enter' && !e.ctrlKey && !inGrid && (tag === 'input' || tag === 'select') && !$(t).is('[data-dtcombo]')) { e.preventDefault(); nextField(t); }
            var k = e.key.toLowerCase();
            if (e.ctrlKey && !e.shiftKey && !e.altKey && k === 's') { e.preventDefault(); if (saveVisible() && !onHistoryPage()) saveClick(); }
            else if (e.ctrlKey && !e.shiftKey && !e.altKey && k === 'n') { e.preventDefault(); btnNewClick(); }
            else if (e.ctrlKey && !e.shiftKey && !e.altKey && k === 'r') { e.preventDefault(); refreshClick(); }
            else if (e.ctrlKey && !e.shiftKey && !e.altKey && k === 't') { e.preventDefault(); showPage(onHistoryPage() ? 'Form' : 'History'); }
            else if (e.ctrlKey && e.shiftKey && k === 'f') { e.preventDefault(); $('#btnRecordsUpdate').prop('hidden', false); }
            else if (e.ctrlKey && !e.shiftKey && !e.altKey && k === 'u') { e.preventDefault(); if (!$('#btnUpdate').prop('hidden') && !$('#btnUpdate').prop('disabled')) updateClick(); }
            else if (e.ctrlKey && e.shiftKey && e.key === 'Delete') { if (!$('#btnDelete').prop('hidden') && !$('#btnDelete').prop('disabled')) deleteClick(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); attDialog(); }
            else if (e.ctrlKey && !e.shiftKey && !e.altKey && k === 'p') { e.preventDefault(); exportInvoiceVoucherSlip(RecId); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('#datVoucherDate').trigger('focus'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $('#grd').trigger('focus'); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); focus('#CmbRefAccountId'); }
            if (e.ctrlKey && e.altKey) shortcutKeys();
        } catch (ex) { say(ex.message); }
    });

    /* ---------------------------------------------------------------- Refresh (:1644) / Load (:417) */
    function bindLists(d) {
        var keep = { b: comboVal('#CmbRefAccountId'), c: comboVal('#CmbCreditAccount'), e: comboVal('#CmbCommissionExpenseAccount'), f: comboVal('#CmbCurrencyCode') };
        dtBuyers = d.buyers || []; dtCurrencies = d.currencies || [];
        fill('#CmbRefAccountId', dtBuyers, 'Id', 'CompanyName'); fill('#CmbCreditAccount', d.creditAccounts, 'Id', 'AccountTitle');
        fill('#CmbCommissionExpenseAccount', d.commissionAccounts, 'Id', 'AccountTitle'); fill('#CmbCurrencyCode', dtCurrencies, 'Id', 'CurrencyCode');
        setCombo('#CmbRefAccountId', keep.b); setCombo('#CmbCreditAccount', keep.c); setCombo('#CmbCommissionExpenseAccount', keep.e); setCombo('#CmbCurrencyCode', keep.f);
    }
    function applyFlags(d) { cfg.cgs = !!d.cgsEntryAllow; cfg.jobWise = !!d.saleCostingJobOrderWise; cfg.tolerance = Math.abs(toDouble(d.tolerance)); }
    function refreshClick() {
        return call('GET', '/refresh').then(function (d) { applyFlags(d); bindLists(d); }).catch(function (e) { say(e.message); });
    }
    function load() {
        return call('GET', '/init').then(function (d) {
            rights = { save: !!d.canSave, update: !!d.canUpdate, del: !!d.canDelete, print: !!d.canPrint, viewAll: !!d.canViewAll };
            cfg.fifo = !!d.fifo; cfg.commissionTab = !!d.commissionTab; cfg.dec = d.decimals || cfg.dec; defaultDays = toInt(d.defaultDays);
            applyFlags(d); applyRights(); setSaveMode(false);
            $('#txtVoucherCode').val(String(d.voucherCode));
            bindLists(d);
            if (cfg.commissionTab) {
                dtSales = d.salesPersons || [];
                fill('#CmbSalePerson', dtSales, 'Id', 'name');
                AccF.fillSelect('#cmbCommissionType', [{ V: 1, T: 'Fixed Commission Amount' }, { V: 2, T: 'Commission % of Value' }, { V: 3, T: 'Commission By Weight Kg' }], 'V', 'T', null);
                setCombo('#cmbCommissionType', '3');
            } else {
                $(AccF.ctl('#CmbCommissionExpenseAccount')).hide(); $('#label19').hide(); $('#tabComm').prop('hidden', true); $('#tpComm').removeClass('on');
            }
            fillPending(d.pending);
            $('#ChkBox').prop('checked', true);
            AccF.fillSelect('#cmbApprovedStatusCpv', [{ V: 1, T: 'Not Approved' }, { V: 2, T: 'Approved' }, { V: 3, T: 'All' }], 'V', 'T', null); setCombo('#cmbApprovedStatusCpv', '1');
            fill('#cmbAccountTitleCpv', d.historyCustomers, 'Id', 'name');
            var n = dateOnly(d.now), base = n ? new Date(+n.slice(0, 4), +n.slice(5, 7) - 1, +n.slice(8, 10)) : new Date(), from = new Date(base.getTime());
            from.setDate(from.getDate() - (defaultDays > 0 ? defaultDays : 3));
            $('#datVoucherDate').val(isoDate(base)); $('#datInvoiceDate').val(isoDate(base)); $('#txtDueDate').val(isoDate(base));
            $('#FromDateCpvHistory').val(isoDate(from)); $('#ToDateCpvHistory').val(isoDate(base));
            $('#txtVoucherCode').trigger('focus');
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- wiring */
    function digitsOnly(sel) { $(sel).on('keypress', function (e) { if (!e.ctrlKey && !e.altKey && e.key.length === 1 && !/\d/.test(e.key)) e.preventDefault(); }); }
    function decimalOnly(sel) {
        $(sel).on('keypress', function (e) {
            if (e.ctrlKey || e.altKey || e.key.length !== 1) return;
            if (!/[\d.]/.test(e.key) || (e.key === '.' && String(this.value).indexOf('.') >= 0)) e.preventDefault();
        });
    }
    digitsOnly('#txtDueDays'); digitsOnly('#txtFromDocNoCpv'); digitsOnly('#txtToDocNoCpv'); decimalOnly('#txtCommRate'); decimalOnly('#txtCommissionExchangeRate');
    $('#txtFcyAmountWithoutAddLess, #txtAddLessAmount').on('input', amountCalculation);
    $('#txtExchangeRate').on('input', exchangeRateChanged);
    $('#txtDueDays').on('input', dueDaysChanged);
    $('#txtCommRate, #txtCommRateUom, #txtCommissionExchangeRate').on('input', calculateCommissionAmount);
    $('#txtCommRateUom').on('keypress', function () { $(this).data('tag', $(this).val()); });
    $('#cmbCommissionType').on('change', cmbCommissionTypeChanged);
    $('#datVoucherDate').on('focusout', function () { voucherDateLeave(); });
    $('#btnAddCommission').on('click', btnAddCommissionClick); $('#btnUpdateCommision').on('click', btnUpdateCommissionClick); $('#btnCancelCommission').on('click', btnCancelCommissionClick);
    $('#btnNew').on('click', btnNewClick); $('#btnRefresh').on('click', refreshClick); $('#btnSave').on('click', saveClick); $('#btnUpdate').on('click', updateClick);
    $('#btnDelete').on('click', deleteClick); $('#btnAttachment').on('click', attDialog); $('#btnSlip').on('click', function () { exportInvoiceVoucherSlip(RecId); });
    $('#btnshortcutkeys').on('click', shortcutKeys); $('#btnRecordsUpdate').on('click', recordsUpdate);
    $('#btnShowCpv').on('click', historyFill); $('#btnNewCpvHistory').on('click', historyReset); $('#btnRefreshCpvHistory').on('click', historyCustomersBind);
    $('#dlgClose').on('click', closeDlg);
    $('#tabForm').on('click', function () { showPage('Form'); }); $('#tabHistory').on('click', function () { showPage('History'); });
    $('#tc2Tab').on('click', 'span[data-tp]', function () {
        if ($(this).prop('hidden')) return;
        $('#tc2Tab span').removeClass('on'); $(this).addClass('on'); $('#tabControl2 .tp').removeClass('on'); $('#' + $(this).data('tp')).addClass('on');
        try { gGrid.rerender(); gPay.rerender(); gChg.rerender(); gComm.rerender(); } catch (e) { /* ignore */ }
    });
    $('#grdPending').on('keydown', function (e) {       // grdPending_KeyDown :3076 (Ctrl+Space on Edit)
        if (e.ctrlKey && e.key === ' ') { var r = gPending.current && gPending.current(); if (r) { e.preventDefault(); pendingRecordBind(r); } }
    });
    load();
})(jQuery);
