/* Fcy Receipt Adjustment Voucher - Architecture.WinApp.Account_Definition.AdjustmentVouchers.frmExportReceiptInvoicesAdjustmentVoucher (screen 916).
   Event-for-event port of the form; desktop line numbers in the comments refer to frmExportReceiptInvoicesAdjustmentVoucher.cs. */
(function ($) {
    'use strict';
    var API = '/accounts/export-receipt-invoices-adjustment-voucher/api';
    var RecId = 0, Approved = false, MultiUtilization = false;
    var rights = { save: true, update: true, del: true, print: true, viewAll: true };
    var dtPending = [];                 // dtPendingVouchers
    var dtVoucherNo = [], dtPaymentType = [], dtSupplier = [];
    var lstRemove = [];                 // lstRemoveDetailRecord
    var histParties = [], yearStart = '', defaultDays = 0, histBound = false;
    var HEAD = 'frmExportReceiptInvoicesAdjustmentVoucher';

    function busy(btn, on) { var $b = $(btn); if (on) $b.prop('disabled', true).addClass('btn-busy'); else $b.removeClass('btn-busy').prop('disabled', false); }
    function say(m) { if (m != null && m !== '') window.alert(m); }
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
    function r3(n) { return Math.round(n * 1000) / 1000; }
    /* "#,##0.###" */
    function fmt3(v) {
        var n = toDouble(v), s = r3(Math.abs(n)).toFixed(3).replace(/\.?0+$/, '');
        var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (n < 0 ? '-' : '') + p.join('.');
    }
    /* "#,##.##" (ReadById) */
    function fmt2(v) {
        var n = toDouble(v); if (n === 0) return '';
        var s = (Math.round(Math.abs(n) * 100) / 100).toFixed(2).replace(/\.?0+$/, '');
        var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (n < 0 ? '-' : '') + p.join('.');
    }
    function dmy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v == null ? '' : v));
        return m ? m[3] + '/' + m[2] + '/' + m[1] : (v == null ? '' : String(v));
    }
    function dmyT(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[ T](\d{2}):(\d{2}):(\d{2})/.exec(String(v == null ? '' : v));
        if (!m) return dmy(v);
        var h = +m[4], ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 === 0 ? 12 : h % 12;
        return m[3] + '/' + m[2] + '/' + m[1] + ' ' + pad(h) + ':' + m[5] + ':' + m[6] + ' ' + ap;
    }
    function dateOnly(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v == null ? '' : v)); return m ? m[1] : ''; }

    /* ---------------------------------------------------------------- combos (DDL.BindDDL ..., ZeroIndex: false) */
    function comboVal(sel) { var v = $(sel).val(); return v == null ? '' : String(v); }
    function setCombo(sel, v) {
        var s = v == null ? '' : String(v);
        if (!$(sel).find('option').filter(function () { return this.value === s; }).length) s = '';
        prog++; try { $(sel).val(s).trigger('change'); } finally { prog--; }
    }
    var prog = 0;
    function clearCombo(sel) { prog++; try { $(sel).val('').trigger('change'); } finally { prog--; } }
    function fill(sel, rows, vk, tk) { AccF.fillSelect(sel, (rows || []).map(function (r) { return { V: r[vk], T: r[tk] }; }), 'V', 'T', null); clearCombo(sel); }
    function emptyCombo(sel) { $(sel).empty(); AccF.fillSelect(sel, [], 'V', 'T', null); clearCombo(sel); }
    function dtRow(list, key, val) { for (var i = 0; i < list.length; i++) if (String(list[i][key]) === String(val)) return list[i]; return null; }
    function focus(sel) { try { AccF.focus(sel); } catch (e) { $(sel).trigger('focus'); } }
    function setEnabled(sel, on) { var c = AccF.ctl(sel); $(sel).prop('disabled', !on); $(c).css('pointer-events', on ? '' : 'none').css('opacity', on ? '' : '.7'); }

    /* ---------------------------------------------------------------- grids */
    var pendingCols = [];
    function pendingColumns(addCheck) {
        pendingCols.length = 0;
        pendingCols.push({ key: 'Load', caption: 'Load', button: 'Load', width: 50, frozen: true });
        if (addCheck) pendingCols.push({ key: 'Select', caption: 'Select', selector: true, width: 30 });
        [['Id', 'Id', 0, 1], ['PaymentTypeId', 'PaymentTypeId', 0, 1], ['PaymentType', 'PaymentType', 130], ['DocumentTypeId', 'DocumentTypeId', 0, 1],
         ['DocumentType', 'V.Type', 60], ['VoucherDate', 'VoucherDate', 80, 0, 'd'], ['VoucherCode', 'VoucherCode', 80], ['VoucherAmount', 'VoucherAmount', 0, 1],
         ['AccountId', 'AccountId', 0, 1], ['AccountCode', 'AccountCode', 90], ['AccountTitle', 'AccountTitle', 220], ['FcyId', 'FcyId', 0, 1], ['FcyCode', 'FcyCode', 60],
         ['Amount', 'Amount', 100, 0, 'n'], ['SupplierCustomerId', 'SupplierCustomerId', 0, 1], ['PartyName', 'PartyName', 0, 1], ['PartyCode', 'PartyCode', 0, 1],
         ['PartyCity', 'PartyCity', 0, 1], ['PartyCellNo', 'PartyCellNo', 0, 1], ['VoucherRemarks', 'VoucherRemarks', 230], ['NoOfAttachments', 'NoOfAttachments', 90, 0, 'l']
        ].forEach(function (c) {
            var o = { key: c[0], caption: c[1], width: c[2] || 100 };
            if (c[3]) o.hidden = true;
            if (c[4] === 'd') o.format = function (v) { return dmy(v); };
            if (c[4] === 'n') { o.num = true; o.format = function (v) { return fmt3(v); }; }
            if (c[4] === 'l') { o.link = true; o.num = true; }
            pendingCols.push(o);
        });
    }
    pendingColumns(false);
    var gPending = CJG.create({
        table: '#grdPendingVouchers', nav: '#grdPendingNav', autoResize: true, columns: pendingCols,
        onButton: function (col, row) { if (col === 'Load') grdPendingOrders_ColumnButtonClick(row); },
        onLink: function (col, row) { if (col === 'NoOfAttachments') grdPendingOrders_LinkClicked(row); }
    });
    var gridCols = [
        { key: 'Id', hidden: true }, { key: 'ExImInvoicePaymentTermsDetailId', hidden: true }, { key: 'RefDocumentTypeId', hidden: true },
        { key: 'RefDocumentType', caption: 'Invoice Type', width: 60 }, { key: 'ExportVoucherId', hidden: true }, { key: 'InvoiceId', hidden: true },
        { key: 'InvoiceNo', caption: 'InvoiceNo', width: 110 },
        { key: 'InvoiceDate', caption: 'InvoiceDate', width: 90, format: function (v) { return dmy(v); } },
        { key: 'InvoiceDueDate', caption: 'InvoiceDueDate', width: 90, format: function (v) { return dmy(v); } },
        { key: 'PaymentTermId', hidden: true }, { key: 'PaymentTerm', caption: 'PaymentTerm', width: 100 }, { key: 'FcyId', hidden: true },
        { key: 'FcyCode', caption: 'FcyCode', width: 60 },
        { key: 'InvoiceAmount', caption: 'InvoiceAmount', width: 100, num: true, format: function (v) { return fmt3(v); } },
        { key: 'AvailableInvoiceAmount', caption: 'AvailableInvoiceAmount', width: 100, num: true, sum: true, format: function (v) { return fmt3(v); }, totalFormat: function (t) { return fmt3(t); } },
        { key: 'AdjustmentAmount', caption: 'AdjustmentAmount', width: 100, num: true, sum: true, format: function (v) { return fmt3(v); }, totalFormat: function (t) { return fmt3(t); } },
        { key: 'BalanceAmount', caption: 'BalanceAmount', width: 100, num: true, sum: true, format: function (v) { return fmt3(v); }, totalFormat: function (t) { return fmt3(t); } },
        { key: 'ExchangeRate', caption: 'ExchangeRate', width: 80, num: true },
        { key: 'Remarks', caption: 'Remarks', width: 230 }
    ];
    var gGrid = CJG.create({ table: '#grd', nav: '#grdNav', autoResize: true, columns: gridCols });
    var histCols = [
        { key: 'Edit', caption: 'Edit', button: 'Edit', width: 50 }, { key: 'Print', caption: 'Print', button: 'Print', width: 50 },
        { key: 'TransactionTypeId', hidden: true }, { key: 'VoucherHeadId', hidden: true },
        { key: 'VoucherCode', caption: 'VoucherCode', width: 80 }, { key: 'VoucherDate', caption: 'VoucherDate', width: 80, format: function (v) { return dmy(v); } },
        { key: 'VoucherAmount', caption: 'VoucherAmount', width: 100, num: true, format: function (v) { return fmt3(v); } },
        { key: 'PartyGlId', hidden: true }, { key: 'AccountCode', caption: 'AccountCode', width: 90 }, { key: 'AccountTitle', caption: 'AccountTitle', width: 220 },
        { key: 'PaymentTypeId', hidden: true }, { key: 'PaymentType', caption: 'PaymentType', width: 130 }, { key: 'RefDocumentTypeId', hidden: true },
        { key: 'RefDocumentType', caption: 'Invoice Type', width: 60 }, { key: 'InvoiceNos', caption: 'InvoiceNos', width: 110 }, { key: 'InvoiceDates', caption: 'InvoiceDates', width: 80 },
        { key: 'AdjustmentAmount', caption: 'AdjustmentAmount', width: 100, num: true, format: function (v) { return fmt3(v); } },
        { key: 'SupplierCustomerId', hidden: true }, { key: 'PartyName', hidden: true },
        { key: 'EntryDate', caption: 'EntryDate', width: 130, format: function (v) { return dmyT(v); } }, { key: 'EntryUserName', caption: 'EntryUserName', width: 90 },
        { key: 'ModifyDate', caption: 'ModifyDate', width: 130, format: function (v) { return dmyT(v); } }, { key: 'ModifyUserName', caption: 'ModifyUserName', width: 90 },
        { key: 'VoucherRemarks', caption: 'VoucherRemarks', width: 190 }
    ];
    var gHist = CJG.create({
        table: '#grdHistory', nav: '#grdHistoryNav', autoResize: true, columns: histCols,
        onButton: function (col, row) { grdHistory_ColumnButtonClick(col, row); },
        onDblClick: function (row) { grdHistory_DoubleClick(row); }
    });

    /* ---------------------------------------------------------------- pending vouchers :510-:610 */
    function normalizePending(rows) {
        /* :536 - the desktop copies MobilePersonal into PartyCity and CityName into PartyCellNo (kept as is) */
        return (rows || []).map(function (r) {
            var o = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) o[k] = r[k];
            o.VoucherAmount = r.VoucherAmount; o.PartyCity = r.MobilePersonal; o.PartyCellNo = r.CityName; o.VoucherRemarks = r.Remarks;
            return o;
        });
    }
    /* GetDistinctParties :553 - ToTable(true, SupplierCustomerId, PartyName, AccountId, FcyId) bound to CmbAccountFilter */
    function getDistinctParties(src) {
        var seen = {}, out = [];
        (src || []).forEach(function (r) {
            var k = [r.SupplierCustomerId, r.PartyName, r.AccountId, r.FcyId].join('|');
            if (!seen[k]) { seen[k] = 1; out.push({ SupplierCustomerId: r.SupplierCustomerId, PartyName: r.PartyName, AccountId: r.AccountId, FcyId: r.FcyId }); }
        });
        distinctParties = out;
        var cur = comboVal('#CmbAccountFilter');
        fill('#CmbAccountFilter', out, 'SupplierCustomerId', 'PartyName');
        if (cur !== '') setCombo('#CmbAccountFilter', cur);
    }
    var distinctParties = [];
    function fillGrdPendingOrders(raw) {
        var rows = normalizePending(raw);
        getDistinctParties(rows);
        dtPending = rows;
        pendingColumns(false);
        gPending.setRows(dtPending);
    }
    function pendingLoad() {
        return call('GET', '/pending').then(function (r) { fillGrdPendingOrders(r); }).catch(function (e) { say(e.message); });
    }
    function saveVisible() { return !$('#btnSave').prop('hidden') && !$('#btnSave').prop('disabled'); }

    /* :610 grdPendingOrders_ColumnButtonClick */
    function grdPendingOrders_ColumnButtonClick(r) {
        try {
            if (!r) return;
            if (!saveVisible()) throw new Error('Please Reset the Form First to Load New Data');
            emptyCombo('#CmbPaymentType'); dtPaymentType = []; setEnabled('#CmbPaymentType', true);
            emptyCombo('#CmbSupplier'); dtSupplier = []; setEnabled('#CmbSupplier', true);
            $('#txtPartyCellNo').val(''); $('#txtPartyCity').val('');
            lstRemove = [];
            bindVoucherFromPending(r);
            bindPaymentTypeFromPending(r);
            supplierdtFromPendingGrid(toInt(comboVal('#CmbVoucher')), toInt(comboVal('#CmbPaymentType')));
            bindPartyName();
            comsupplierValueChanged();
            focus('#CmbSupplier');
        } catch (ex) { say(ex.message); }
    }
    /* :614 grdPendingOrders_LinkClicked -> CommonServices.GetNoofAttachmentsByRefDocumentTypeID */
    function grdPendingOrders_LinkClicked(r) {
        if (!r) return;
        var id = toInt(r.Id), dt = toInt(r.DocumentTypeId);
        call('GET', '/attachments', { id: id, documentTypeId: dt }).then(function (rows) {
            var h = '<table><tr><th>Attachment</th><th>Name</th><th>Entry Date</th></tr>';
            (rows || []).forEach(function (a) {
                h += '<tr><td><a href="' + API + '/attachments/' + a.Id + '?id=' + id + '&documentTypeId=' + dt + '" target="_blank">' + CJG.esc(a.AttachmentName) + '</a></td><td>' + CJG.esc(a.CustomName) + '</td><td>' + CJG.esc(dmyT(a.EntryDate)) + '</td></tr>';
            });
            if (!rows || !rows.length) h += '<tr><td colspan="3">No attachments</td></tr>';
            showDialog('Attachments', h + '</table>');
        }).catch(function (e) { say(e.message); });
    }
    function showDialog(title, html) { $('#dlgTitle').text(title); $('#dlgBody').html(html); $('#ovDlg').addClass('on'); }
    function hideDialog() { $('#ovDlg').removeClass('on'); }

    /* BindVoucherFromPending :433 / BindPaymentTypeFromPending :416 / SupplierdtFromPendingGrid :357 / BindPartyName :381 */
    function bindVoucherFromPending(r) {
        dtVoucherNo = [{ Id: r.Id, VoucherNo: r.VoucherCode, VoucherDate: r.VoucherDate, VoucherAmount: r.Amount }];
        fill('#CmbVoucher', dtVoucherNo, 'Id', 'VoucherNo');
        setCombo('#CmbVoucher', r.Id);
        cmbVoucherLeave();
    }
    function bindPaymentTypeFromPending(r) {
        dtPaymentType = [{ Id: r.PaymentTypeId, PaymentType: r.PaymentType }];
        fill('#CmbPaymentType', dtPaymentType, 'Id', 'PaymentType');
        setCombo('#CmbPaymentType', r.PaymentTypeId);
    }
    function supplierdtFromPendingGrid(voucherHeadId, paymentTypeId) {
        dtSupplier = [];
        var seen = {};
        dtPending.forEach(function (r) {
            if (toInt(r.Id) === voucherHeadId && toInt(r.PaymentTypeId) === paymentTypeId) {
                var sid = toInt(r.SupplierCustomerId);
                if (!seen[sid]) {
                    seen[sid] = 1;
                    dtSupplier.push({ SupplierCustomerId: sid, PartyName: r.PartyName, PartyCode: r.PartyCode, AccountId: toInt(r.AccountId), AccountTitle: r.AccountTitle,
                        AccountCode: r.AccountCode, FcyId: r.FcyId, FcyCode: r.FcyCode, CityName: r.PartyCity, MobileNo: r.PartyCellNo });
                }
            }
        });
    }
    function bindPartyName() {
        var cur = comboVal('#CmbSupplier');
        if (dtSupplier.length > 0) {
            fill('#CmbSupplier', dtSupplier, 'AccountId', 'AccountTitle');
            /* CommonServices.SetComboValue(..., ActivateRow: true): the previous value when it is still there, else the first row */
            if (cur !== '' && dtRow(dtSupplier, 'AccountId', cur)) setCombo('#CmbSupplier', cur); else setCombo('#CmbSupplier', dtSupplier[0].AccountId);
        } else emptyCombo('#CmbSupplier');
    }
    function supplierRow() { var v = comboVal('#CmbSupplier'); return v === '' ? null : dtRow(dtSupplier, 'AccountId', v); }
    /* BindCustomerCity :346 */
    function bindCustomerCity() {
        $('#txtPartyCity').val(''); $('#txtPartyCellNo').val('');
        var s = supplierRow();
        if (s && toInt(comboVal('#CmbSupplier')) > 0) { $('#txtPartyCellNo').val(s.MobileNo == null ? '' : s.MobileNo); $('#txtPartyCity').val(s.CityName == null ? '' : s.CityName); }
    }
    /* CmbVoucher_Leave :493 */
    function cmbVoucherLeave() {
        var id = toInt(comboVal('#CmbVoucher')), v = id > 0 ? dtRow(dtVoucherNo, 'Id', id) : null;
        if (v) { $('#txtVoucherDate').val(dateOnly(v.VoucherDate)); $('#txtVoucherAmountRegular').val(fmt3(v.VoucherAmount)); }
    }
    /* comsupplier_ValueChanged :479 */
    function comsupplierValueChanged() {
        try { bindCustomerCity(); return bindGridData(); } catch (ex) { say(ex.message); }
    }
    /* BindGridData :668 */
    function bindGridData() {
        var s = supplierRow();
        if (!dtSupplier.length || !s) return Promise.resolve();
        var sid = toInt(s.SupplierCustomerId), fcy = toInt(s.FcyId);
        var n = gGrid.count();
        if (sid !== toInt(comboVal('#CmbAccountFilter')) && n === 0) {
            if (toInt(comboVal('#CmbVoucher')) === 0) { focus('#CmbVoucher'); throw new Error('VoucherNo is Required...'); }
            if (sid === 0) { focus('#CmbSupplier'); throw new Error('Party is Required...'); }
            gGrid.setRows([]);
            return loadInvoiceData(sid, fcy);
        } else if (MultiUtilization && n > 0) return loadInvoiceData(sid, fcy);
        return Promise.resolve();
    }
    /* LoadInvoiceData :738 - dtGrid.Rows.Clear() then the proc */
    function loadInvoiceData(sid, fcy) {
        var keep = gGrid.rows().slice();
        gGrid.setRows([]);
        return call('GET', '/invoices', { supplierCustomerId: sid, fcyId: fcy }).then(function (rows) {
            if (rows && rows.length) loadDetailDataFromInvoice(rows, []);
        }).catch(function (e) { say(e.message); });
    }
    /* LoadDetailDataFromInvoice :752 (existing rows keyed by ExImInvoicePaymentTermsDetailId are refreshed, the others added) */
    function loadDetailDataFromInvoice(data, existing) {
        var byId = {};
        existing.forEach(function (r) { var k = toInt(r.ExImInvoicePaymentTermsDetailId); (byId[k] = byId[k] || []).push(r); });
        var out = existing.slice();
        data.forEach(function (d) {
            var id = toInt(d.Id), bill = toDouble(d.FcyAmount), avail = toDouble(d.Balance);
            if (byId[id]) {
                byId[id].forEach(function (it) {
                    it.InvoiceAmount = bill; avail += toDouble(it.AdjustmentAmount); it.AvailableInvoiceAmount = avail; it.BalanceAmount = avail - toDouble(it.AdjustmentAmount);
                });
                return;
            }
            out.push({ Id: 0, ExImInvoicePaymentTermsDetailId: d.Id, RefDocumentTypeId: d.DocumentTypeId, RefDocumentType: d.DocumentType, ExportVoucherId: d.ExportVoucherId,
                InvoiceId: d.InvoiceId, InvoiceNo: d.InvoiceNo, InvoiceDate: d.InvoiceDate, InvoiceDueDate: d.InvoiceDueDate, PaymentTermId: d.PaymentTermId,
                PaymentTerm: d.PaymentTerm, FcyId: d.FcyId, FcyCode: d.FcyCode, InvoiceAmount: bill, AvailableInvoiceAmount: avail, AdjustmentAmount: 0,
                BalanceAmount: avail, ExchangeRate: d.ExchangeRate, Remarks: '' });
        });
        gGrid.setRows(out);
    }

    /* ---------------------------------------------------------------- grd inline edit (EditType 1: AdjustmentAmount, Remarks) and grd_CellUpdated :819 */
    $('#grd').on('click', 'tbody td[data-k="AdjustmentAmount"], tbody td[data-k="Remarks"]', function () {
        var $td = $(this); if ($td.find('input').length) return;
        var key = $td.data('k'), i = +$td.closest('tr').data('i'), row = gGrid.view()[i];
        if (!row) return;
        var $in = $('<input type="text" style="width:100%;box-sizing:border-box;border:1px solid #316ac5;font:12px Verdana,sans-serif;height:20px">').val(key === 'Remarks' ? (row.Remarks || '') : (toDouble(row[key]) === 0 ? '0' : String(toDouble(row[key]))));
        $td.addClass('ed').empty().append($in);
        $in.on('keydown', function (e) {
            if (e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); $in.blur(); } else if (e.key === 'Escape') { $in.data('esc', 1); $in.blur(); }
            else if (key === 'AdjustmentAmount' && !e.ctrlKey && !e.altKey && e.key.length === 1 && !/[\d.]/.test(e.key)) e.preventDefault();
        }).on('blur', function () {
            if (!$in.data('esc')) cellUpdated(row, key, $in.val());
            gGrid.rerender();
        }).trigger('focus').trigger('select');
    });
    function cellUpdated(item, key, text) {
        try {
            if (key === 'Remarks') { item.Remarks = text; return; }
            var adj = toDouble(text), inv = toDouble(item.AvailableInvoiceAmount);
            item.AdjustmentAmount = adj;
            if (adj > inv) { adj = inv; item.AdjustmentAmount = adj; say('AdjustmentAmount can not be greater than AvailableInvoiceAmount'); }
            item.BalanceAmount = inv - adj;
        } catch (ex) { say(ex.message); }
    }

    /* ---------------------------------------------------------------- Auto Utilize :2164 */
    function totalAmountAutoUtilizeInGrid() {
        var total = toDouble($('#txtVoucherAmountRegular').val());
        var rowsAll = gGrid.rows();
        var gridTotal = rowsAll.length ? rowsAll.reduce(function (a, r) { return a + toDouble(r.AdjustmentAmount); }, 0) : 0;
        if (total >= gridTotal) {
            total -= gridTotal;
            var zero = rowsAll.filter(function (r) { return toDouble(r.AdjustmentAmount) === 0; });
            for (var i = 0; i < zero.length; i++) {
                var it = zero[i], inv = toDouble(it.AvailableInvoiceAmount);
                var apply = Math.min(total, Math.min(toDouble(it.BalanceAmount), inv));
                it.AdjustmentAmount = apply; it.BalanceAmount = inv - apply; total -= apply;
                if (total <= 0) break;
            }
        } else {
            var used = 0;
            rowsAll.forEach(function (row) {
                if (used >= total) { row.AdjustmentAmount = 0; row.BalanceAmount = toDouble(row.InvoiceAmount); }
                used += toDouble(row.AdjustmentAmount);
            });
        }
        gGrid.rerender();
    }
    function btnAutoUtilizeClick() {
        try { totalAmountAutoUtilizeInGrid(); } catch (ex) { say('Error while auto-utilizing: ' + ex.message); }
    }

    /* ---------------------------------------------------------------- Show :2216 */
    function btnShowRecordsClick() {
        try {
            gGrid.setRows([]);
            var f = comboVal('#CmbAccountFilter');
            if (f !== '' && toInt(f) > 0) {
                var p = dtRow(distinctParties, 'SupplierCustomerId', f), fcy = p ? toInt(p.FcyId) : 0, acc = toInt(f);
                var filtered = dtPending.filter(function (r) { return toInt(r.SupplierCustomerId) === acc && toInt(r.FcyId) === fcy; });
                var pr = loadInvoiceData(acc, fcy);
                if (filtered.length) { pendingColumns(true); gPending.setRows(filtered); } else say('No pending vouchers found for the selected account.');
                return pr;
            }
            pendingColumns(false); gPending.setRows(dtPending);
        } catch (ex) { say(ex.message); }
        return Promise.resolve();
    }
    /* BtnAutoUtilizedAllRowsOfPendingGrid_Click :2257 (Visible = false on the form) */
    function btnAutoUtilizedAllRows() {
        try {
            if (!saveVisible()) { say('Please Reset the Form First to Load New Data'); return Promise.resolve(); }
            var acc = toInt(comboVal('#CmbAccountFilter'));
            if (acc === 0) { focus('#CmbAccountFilter'); say('Please select an account first.'); return Promise.resolve(); }
            if (gPending.count() === 0) throw new Error('Pending Vouchers Detail have no Records...');
            var checked = gPending.checked();
            if (checked.length === 0) throw new Error("Please Select Any row First.Press show Button If 'Select' Button not Appear");
            MultiUtilization = true;
            var chain = Promise.resolve();
            checked.forEach(function (row) {
                chain = chain.then(function () {
                    bindVoucherFromPending(row); bindPaymentTypeFromPending(row);
                    supplierdtFromPendingGrid(toInt(comboVal('#CmbVoucher')), toInt(comboVal('#CmbPaymentType')));
                    bindPartyName();
                    return Promise.resolve(comsupplierValueChanged()).then(function () {
                        btnAutoUtilizeClick();
                        return insert(true);
                    }).then(function () {
                        gGrid.rows().forEach(function (r) { r.AdjustmentAmount = 0; r.BalanceAmount = toDouble(r.InvoiceAmount); });
                        gGrid.rerender();
                    });
                });
            });
            return chain.then(function () { return reset(); }).then(function () { $('#CmbAccountFilter').val('').trigger('change'); }).catch(function (e) { say(e.message); });
        } catch (ex) { say(ex.message); return Promise.resolve(); }
    }

    /* ---------------------------------------------------------------- Reset :1100 / New / Refresh */
    function setSaveMode(isNew) { $('#btnSave').prop('hidden', !isNew); $('#btnUpdate').prop('hidden', isNew); $('#btnDelete').prop('hidden', isNew); }
    function applyRights() {
        $('#btnSave').prop('disabled', !rights.save); $('#btnUpdate').prop('disabled', !rights.update);
        $('#btnDelete').prop('disabled', !rights.del); $('#btnSlip').prop('disabled', !rights.print);
    }
    function reset() {
        RecId = 0; Approved = false; setSaveMode(true); MultiUtilization = false;
        emptyCombo('#CmbVoucher'); dtVoucherNo = []; setEnabled('#CmbVoucher', true); $('#txtVoucherAmountRegular').val(''); $('#txtVoucherDate').val('');
        emptyCombo('#CmbPaymentType'); dtPaymentType = []; setEnabled('#CmbPaymentType', true);
        emptyCombo('#CmbSupplier'); dtSupplier = []; setEnabled('#CmbSupplier', true);
        $('#txtPartyCellNo').val(''); $('#txtPartyCity').val('');
        lstRemove = []; gGrid.setRows([]);
        return pendingLoad().then(function () { focus('#CmbAccountFilter'); });
    }
    function btnRefreshClick() {
        try {
            supplierdtFromPendingGrid(toInt(comboVal('#CmbVoucher')), toInt(comboVal('#CmbPaymentType')));
            bindPartyName();
            comsupplierValueChanged();
        } catch (ex) { say(ex.message); }
    }
    function btnNewClick() { return reset().then(function () { $('#CmbAccountFilter').val('').trigger('change'); }); }

    /* ---------------------------------------------------------------- FormValidation :1070 / Insert :1112 */
    function formValidation() {
        if (toInt(comboVal('#CmbVoucher')) === 0) { say('Voucher No Field is Required'); focus('#CmbVoucher'); return false; }
        var t = $('#txtVoucherAmountRegular').val().trim();
        if (t === '' || t === '0') { say('VoucherAmount Field is Required'); $('#txtVoucherAmountRegular').trigger('focus'); return false; }
        if (toInt(comboVal('#CmbPaymentType')) === 0) { say('Payment Type Field is Required'); focus('#CmbPaymentType'); return false; }
        if (toInt(comboVal('#CmbSupplier')) === 0) { say('Party Field is Required'); focus('#CmbSupplier'); return false; }
        return true;
    }
    function insert(auto) {
        try {
            if (gGrid.count() === 0) throw new Error('Grid Record Not Found');
            if (!formValidation()) return Promise.resolve();
            if (!auto) {
                if (RecId > 0) { if (!window.confirm('Are you sure to Update?')) return Promise.resolve(); }
                else if (!window.confirm('Are you sure to Save?')) return Promise.resolve();
            }
            var s = supplierRow() || {};
            var body = {
                recId: RecId, approved: Approved, voucherHeadId: toInt(comboVal('#CmbVoucher')), voucherCode: toInt($('#CmbVoucher option:selected').text()),
                voucherDate: $('#txtVoucherDate').val(), voucherAmount: $('#txtVoucherAmountRegular').val(), paymentTypeId: toInt(comboVal('#CmbPaymentType')),
                supplierCustomerId: toInt(s.SupplierCustomerId), partyGlId: toInt(s.AccountId), removed: lstRemove,
                lines: gGrid.rows().map(function (r) {
                    return { Id: RecId !== 0 ? toInt(r.Id) : 0, ExImInvoicePaymentTermsDetailId: toInt(r.ExImInvoicePaymentTermsDetailId), RefDocumentTypeId: toInt(r.RefDocumentTypeId),
                        RefDocumentType: r.RefDocumentType, ExportVoucherId: toInt(r.ExportVoucherId), InvoiceId: toInt(r.InvoiceId), InvoiceNo: r.InvoiceNo, InvoiceDueDate: dateOnly(r.InvoiceDueDate),
                        PaymentTermId: toInt(r.PaymentTermId), PaymentTerm: r.PaymentTerm, FcyId: toInt(r.FcyId), FcyCode: r.FcyCode, InvoiceAmount: toDouble(r.InvoiceAmount),
                        AdjustmentAmount: toDouble(r.AdjustmentAmount), ExchangeRate: toDouble(r.ExchangeRate), Remarks: r.Remarks == null ? '' : r.Remarks };
                })
            };
            var wasNew = RecId === 0;
            return call('POST', '/save', body).then(function () {
                if (auto) return;
                say((wasNew ? 'Record Saved Successfully [' : 'Record Update Successfully [') + body.voucherCode + '] ');
                return reset().then(function () {
                    if ($('#ChkPrintslip').prop('checked')) generateSlip(body.voucherHeadId, body.paymentTypeId, body.supplierCustomerId, body.partyGlId, 1);
                    return btnShowRecordsClick();
                });
            }).catch(function (e) { say(e.message); });
        } catch (ex) { say(ex.message); return Promise.resolve(); }
    }
    function btnSaveClick() { RecId = 0; return insert(false); }
    function btnUpdateClick() {
        try {
            if (RecId === 0) throw new Error('Record Not found');
            if (Approved) throw new Error('Record Not Update because Record has approved');
            return insert(false);
        } catch (ex) { say(ex.message); }
    }
    /* btnDelete_Click :1389 */
    function btnDeleteClick() {
        try {
            if (RecId === 0) throw new Error('Record Id not found for deletion...');
            if (!window.confirm('Are you sure to Delete?')) return;
            var s = supplierRow() || {};
            var body = { recId: RecId, voucherHeadId: toInt(comboVal('#CmbVoucher')), voucherCode: toInt($('#CmbVoucher option:selected').text()),
                voucherDate: $('#txtVoucherDate').val(), supplierCustomerId: toInt(s.SupplierCustomerId), partyGlId: toInt(s.AccountId),
                paymentTypeId: toInt(comboVal('#CmbPaymentType')), voucherAmount: $('#txtVoucherAmountRegular').val(),
                lines: gGrid.rows().filter(function (r) { return toInt(r.Id) > 0; }).map(function (r) { return { Id: toInt(r.Id), RefDocumentTypeId: toInt(r.RefDocumentTypeId), InvoiceId: toInt(r.InvoiceId), InvoiceAmount: toDouble(r.InvoiceAmount), AdjustmentAmount: toDouble(r.AdjustmentAmount), Remarks: r.Remarks || '' }; }) };
            return call('POST', '/delete', body).then(function () { say('Deleted Successfully'); return reset(); })
                .catch(function (e) { say(e.message); });
        } catch (ex) { say(ex.message); }
    }

    /* ---------------------------------------------------------------- ReadById :1280 */
    function readById(voucherHeadId, paymentTypeId, supplierCustomerId, partyGlId) {
        return reset().then(function () {
            RecId = voucherHeadId;
            return call('GET', '/by-voucher-head', { voucherHeadId: voucherHeadId, paymentTypeId: paymentTypeId, supplierCustomerId: supplierCustomerId, partyGlId: partyGlId });
        }).then(function (rows) {
            if (!rows || rows.length === 0) return;
            var h = rows[0];
            selectTab(0);
            setSaveMode(false);
            dtVoucherNo = [{ Id: h.VoucherHeadId, VoucherNo: h.VoucherCode, VoucherDate: h.VoucherDate, VoucherAmount: h.VoucherAmount }];
            fill('#CmbVoucher', dtVoucherNo, 'Id', 'VoucherNo'); setCombo('#CmbVoucher', h.VoucherHeadId);
            if (voucherHeadId > 0) { $('#txtVoucherDate').val(dateOnly(h.VoucherDate)); $('#txtVoucherAmountRegular').val(fmt2(h.VoucherAmount)); }
            dtPaymentType = [{ Id: h.PaymentTypeId, PaymentType: h.PaymentType }];
            fill('#CmbPaymentType', dtPaymentType, 'Id', 'PaymentType'); setCombo('#CmbPaymentType', h.PaymentTypeId);
            dtSupplier = [{ SupplierCustomerId: h.SupplierCustomerId, PartyName: h.PartyName, PartyCode: h.PartyCode, AccountId: h.PartyGlId, AccountTitle: h.PartyGlAccountTitle,
                AccountCode: h.PartyGlAccountCode, FcyId: h.FcyId, FcyCode: h.FcyCode, CityName: h.PartyCityName, MobileNo: h.MobilePersonal }];
            bindPartyName();
            var out = rows.map(function (d) {
                var inv = toDouble(d.InvoiceAmount), adj = paymentTypeId === 1 ? toDouble(d.AdvanceAmount) : toDouble(d.AdjustmentAmount);
                return { Id: d.Id, ExImInvoicePaymentTermsDetailId: d.ExImInvoicePaymentTermsDetailId, RefDocumentTypeId: d.RefDocumentTypeId, RefDocumentType: d.RefDocumentType,
                    ExportVoucherId: toInt(d.RefDocNoId), InvoiceId: toInt(d.ExImInvoiceId), InvoiceNo: d.RefDocNo == null ? '' : String(d.RefDocNo), InvoiceDate: d.RefDocDate,
                    InvoiceDueDate: d.DueDate, PaymentTermId: d.PaymentTermId, PaymentTerm: d.PaymentTerm, FcyId: d.FcyId, FcyCode: d.FcyCode, InvoiceAmount: inv,
                    AvailableInvoiceAmount: inv, AdjustmentAmount: adj, BalanceAmount: inv - adj, ExchangeRate: d.ExchangeRate, Remarks: d.Remarks == null ? '' : d.Remarks };
            });
            gGrid.setRows(out);
            comsupplierValueChanged();
            $('#grd').trigger('focus');
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- slip :1706 / :1721 */
    function btnSlipClick() {
        try {
            var vh = toInt(comboVal('#CmbVoucher')), pt = toInt(comboVal('#CmbPaymentType')), sc = 0, gl = 0;
            var s = supplierRow();
            if (s && toInt(comboVal('#CmbSupplier')) > 0) { sc = toInt(s.SupplierCustomerId); gl = toInt(s.AccountId); }
            generateSlip(vh, pt, sc, gl, RecId);
        } catch (ex) { say(ex.message); }
    }
    function generateSlip(vh, pt, sc, gl, id) {
        try {
            if (id === 0) throw new Error('Not Record Found For Display');
            call('GET', '/by-voucher-head', { voucherHeadId: vh, paymentTypeId: pt, supplierCustomerId: sc, partyGlId: gl }).then(function (rows) {
                if (!rows || rows.length === 0) { say('Not Record Found For Display'); return; }
                window.printRpt('901_02_VoucherInvoicesAdjustment_Slip.rpt', { transactionTypeId: 3, voucherHeadId: vh, paymentTypeId: pt, supplierCustomerId: sc, partyGlId: gl });
            }).catch(function (e) { say(e.message); });
        } catch (ex) { say(ex.message); }
    }

    /* ---------------------------------------------------------------- history :1343-:1700 */
    function parameterFill() { fill('#cmbDateTypeHistory', AccF.DATE_TYPES, 'Id', 'Parameters'); }
    function setHistDates(from, to) { if (from) $('#txtFromdateHistory').val(from); if (to) $('#txtToDateHistory').val(to); }
    function cmbDateTypeHistoryValueChanged() {
        var v = toInt(comboVal('#cmbDateTypeHistory')), t = new Date(), today = isoDate(t);
        if (v === 1) setHistDates(today, null);
        else if (v === 2) { var d = new Date(); d.setDate(d.getDate() - 7); setHistDates(isoDate(d), null); }
        else if (v === 3) setHistDates(t.getFullYear() + '-' + pad(t.getMonth() + 1) + '-01', today);
        else if (v === 4) setHistDates(t.getFullYear() + '-01-01', today);
        else if (v === 5) setHistDates(dateOnly(yearStart), null);
    }
    function historyComboBind() {
        var cur = comboVal('#CmbCustomerHistory');
        return call('GET', '/history-parties').then(function (rows) {
            if (rows && rows.length) {
                histParties = rows;
                AccF.fillSelect('#CmbCustomerHistory', rows.map(function (r) { return { V: r.AccountId, T: r.AccountTitle }; }), 'V', 'T', '');
                if (toInt(cur) > 0) setCombo('#CmbCustomerHistory', cur); else clearCombo('#CmbCustomerHistory');
            }
        }).catch(function (e) { say(e.message); });
    }
    function fillHistory() {
        var dt = $('input[name=hdate]:checked').val() || 'doc', body = { dateType: dt, fromNo: toInt($('#txtFromNoHistory').val()), toNo: toInt($('#txtToDocNoHistory').val()) };
        if ($('#chkFromHistory').prop('checked')) body.fromDate = $('#txtFromdateHistory').val();
        if ($('#chkToHistory').prop('checked')) body.toDate = $('#txtToDateHistory').val();
        var p = toInt(comboVal('#CmbCustomerHistory')) > 0 ? dtRow(histParties, 'AccountId', comboVal('#CmbCustomerHistory')) : null;
        if (p) { body.supplierCustomerId = toInt(p.SupplierCustomerId); body.glAccountId = toInt(p.AccountId); }
        return call('POST', '/history', body).then(function (rows) {
            if (rows && rows.length) gHist.setRows(rows); else gHist.clear();
        }).catch(function (e) { say(e.message); });
    }
    function histKeys(r) { return [toInt(r.VoucherHeadId), toInt(r.PaymentTypeId), toInt(r.SupplierCustomerId), toInt(r.PartyGlId)]; }
    function grdHistory_DoubleClick(r) { if (r) readById.apply(null, histKeys(r)); }
    function grdHistory_ColumnButtonClick(col, r) {
        if (!r) return; var k = histKeys(r);
        if (col === 'Edit') readById.apply(null, k);
        if (col === 'Print') generateSlip(k[0], k[1], k[2], k[3], k[0]);
    }
    function resetHistory() {
        setCombo('#cmbDateTypeHistory', 3); $('#txtFromNoHistory').val(''); $('#txtToDocNoHistory').val(''); clearCombo('#CmbCustomerHistory'); focus('#cmbDateTypeHistory');
    }
    /* tabControl1_SelectedIndexChanged :1534 */
    function selectTab(i) {
        $('#pageForm').toggleClass('on', i === 0); $('#pageHistory').toggleClass('on', i === 1);
        $('#tabForm').toggleClass('on', i === 0); $('#tabHistory').toggleClass('on', i === 1);
        if (i === 1 && !histBound) {
            histBound = true;
            parameterFill(); focus('#cmbDateTypeHistory');
            if (toInt(comboVal('#cmbDateTypeHistory')) === 0) { var f = $('#cmbDateTypeHistory option').eq(0).val(); if (f != null) setCombo('#cmbDateTypeHistory', f); }
            historyComboBind();
        }
        if (i === 0) { gGrid.rerender(); gPending.rerender(); } else gHist.rerender();
    }
    function tabIndex() { return $('#pageHistory').hasClass('on') ? 1 : 0; }

    /* ---------------------------------------------------------------- MakeShortCutKeys :1960 */
    function makeShortCutKeys() {
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print Slip 901'], ['Alt+1', 'For Print Slip 901'], ['Ctrl+F5', 'For Focus on DocDate'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
            ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        var h = '<table><tr><th>KeyCombination</th><th>Description</th></tr>';
        rows.forEach(function (r) { h += '<tr><td>' + r[0] + '</td><td>' + r[1] + '</td></tr>'; });
        showDialog('ShortCut Keys', h + '</table>');
    }

    /* ---------------------------------------------------------------- keys :1853 InvfrmPurchasedirectInvoice_KeyDown_1 */
    function isEnabledBtn(sel) { return !$(sel).prop('hidden') && !$(sel).prop('disabled'); }
    $(document).on('keydown', function (e) {
        try {
            var k = e.key, ctrl = e.ctrlKey, alt = e.altKey;
            if ($('#ovDlg').hasClass('on')) { if (k === 'Escape') hideDialog(); return; }
            /* Return -> SendKeys {TAB} (not inside a text area / button / grid editor) */
            if (k === 'Enter' && !ctrl && !alt && $(e.target).is('input:not([type=checkbox]):not([type=radio]), select') && !$(e.target).closest('#grd').length) {
                var f = $('input:visible:not(:disabled):not([type=hidden]), select:visible:not(:disabled), button:visible:not(:disabled)').filter(function () { return this.tabIndex >= 0; }).toArray();
                var ix = f.indexOf(e.target); if (ix >= 0 && f[ix + 1]) { e.preventDefault(); f[ix + 1].focus(); }
            }
            var kl = String(k).toLowerCase();
            if (ctrl && kl === 't') { e.preventDefault(); if (tabIndex() === 1) { selectTab(0); $('#grd').trigger('focus'); } else { selectTab(1); focus('#cmbDateTypeHistory'); } }
            if (ctrl && kl === 'e') { e.preventDefault(); window.location.href = '/accounts'; return; }
            if (tabIndex() === 0) {
                if (ctrl && !e.shiftKey && kl === 's' && isEnabledBtn('#btnSave')) { e.preventDefault(); btnSaveClick(); }
                if (ctrl && kl === 'u' && isEnabledBtn('#btnUpdate')) { e.preventDefault(); btnUpdateClick(); }
                if (ctrl && e.shiftKey && k === 'Delete' && isEnabledBtn('#btnDelete')) { e.preventDefault(); btnDeleteClick(); }
                if (ctrl && kl === 'n') { e.preventDefault(); btnNewClick(); }
                if (ctrl && kl === 'r') { e.preventDefault(); btnRefreshClick(); }
                if (ctrl && k === 'F5') { e.preventDefault(); focus('#CmbSupplier'); }
                if (ctrl && k === 'ArrowDown') { e.preventDefault(); $('#grd').trigger('focus'); }
                if (ctrl && k === 'ArrowRight') { e.preventDefault(); if ($(document.activeElement).closest('#gridDetail').length) $('#grdPendingVouchers').trigger('focus'); else $('#grd').trigger('focus'); }
                if (ctrl && k === 'ArrowUp') { e.preventDefault(); focus('#CmbSupplier'); }
                if (ctrl && k === 'F10') { e.preventDefault(); /* btnAttachment_Click is empty on the form */ }
                if (ctrl && kl === 'p') { e.preventDefault(); btnSlipClick(); }
                if (alt && (k === '1')) { e.preventDefault(); btnSlipClick(); }
            } else {
                if (ctrl && (k === 'F5' || k === 'ArrowDown' || k === 'ArrowUp' || k === 'ArrowRight')) { e.preventDefault(); $('#grdHistory').trigger('focus'); }
                var cur = gHist.current();
                if (cur && ctrl && k === 'Enter' && rights.update) { var h = histKeys(cur); readById.apply(null, h); }
                if (cur && ctrl && kl === 'p' && rights.print) { e.preventDefault(); var g = histKeys(cur); generateSlip(g[0], g[1], g[2], g[3], g[0]); }
            }
        } catch (ex) { say(ex.message); }
    });
    /* Ctrl+Alt (e.Control && e.Alt) -> ShortCut Keys form */
    $(document).on('keydown', function (e) { if (e.ctrlKey && e.altKey && !e.shiftKey && (e.key === 'Control' || e.key === 'Alt')) makeShortCutKeys(); });

    /* ---------------------------------------------------------------- wiring */
    $('#btnNew').on('click', btnNewClick);
    $('#btnRefresh').on('click', btnRefreshClick);
    $('#btnSave').on('click', btnSaveClick);
    $('#btnUpdate').on('click', btnUpdateClick);
    $('#btnDelete').on('click', btnDeleteClick);
    $('#btnAttachment').on('click', function () { /* btnAttachment_Click :1717 is empty */ });
    $('#btnSlip').on('click', btnSlipClick);
    $('#btnshortcutkeys').on('click', makeShortCutKeys);
    $('#btnShowRecords').on('click', function () { return busyRun(this, btnShowRecordsClick); });
    $('#BtnAutoUtilize').on('click', btnAutoUtilizeClick);
    $('#BtnAutoUtilizedAllRowsOfPendingGrid').on('click', function () { return busyRun(this, btnAutoUtilizedAllRows); });
    $('#btnshow').on('click', function () { return busyRun(this, fillHistory); });
    $('#BtnNewHistory').on('click', resetHistory);
    $('#BtnRefreshHistory').on('click', historyComboBind);
    $('#tabForm').on('click', function () { selectTab(0); });
    $('#tabHistory').on('click', function () { selectTab(1); });
    $('#dlgClose').on('click', hideDialog);
    $('#CmbSupplier').on('change', function () { if (!prog) comsupplierValueChanged(); });
    $('#cmbDateTypeHistory').on('change', cmbDateTypeHistoryValueChanged);
    /* CmbVoucher_Leave: the UltraCombo's Leave -> focusout of its wrap */
    $(document).on('focusout', '#CmbVoucher, .dtcombo-wrap:has(#CmbVoucher)', function () { setTimeout(cmbVoucherLeave, 0); });
    /* txtFromNoHistory / txtToDocNoHistory_KeyPress: CommonServices.OnlytextNumberFunction (digits) */
    $('#txtFromNoHistory, #txtToDocNoHistory').on('keypress', function (e) { if (!e.ctrlKey && e.key.length === 1 && !/\d/.test(e.key)) e.preventDefault(); });
    $('#chkFromHistory').on('change', function () { $('#txtFromdateHistory').prop('disabled', !this.checked); });
    $('#chkToHistory').on('change', function () { $('#txtToDateHistory').prop('disabled', !this.checked); });
    function busyRun(btn, fn) {
        if ($(btn).hasClass('btn-busy')) return;
        busy(btn, true);
        return Promise.resolve(fn()).then(function () { busy(btn, false); }, function () { busy(btn, false); });
    }

    /* ---------------------------------------------------------------- load: InitializeComponentCustom :246 / InitializeComponentMethod :293 */
    function load() {
        setSaveMode(true);
        focus('#CmbAccountFilter');
        return call('GET', '/load').then(function (r) {
            rights.save = !!r.canSave; rights.update = !!r.canUpdate; rights.del = !!r.canDelete; rights.print = !!r.canPrint; rights.viewAll = !!r.canViewAll;
            applyRights();
            $('#ChkPrintslip').prop('checked', rights.print);
            defaultDays = toInt(r.defaultDays); yearStart = r.yearStart || '';
            fillGrdPendingOrders(r.pending);
            var t = new Date(), f = new Date(); f.setDate(f.getDate() - (defaultDays > 0 ? defaultDays : 3));
            $('#txtFromdateHistory').val(isoDate(f)); $('#txtToDateHistory').val(isoDate(t));
            if (!$('#CmbVoucher').closest('.dtcombo-wrap').length) AccF.combos();
        }).catch(function (e) { say('Error occurred during database call.'); });
    }
    $(function () { AccF.combos(); load(); });
})(window.jQuery);
