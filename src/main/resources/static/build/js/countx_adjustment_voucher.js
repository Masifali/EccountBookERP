/* ============================================================================================
 * Screens 861 "Invoices Adjustment Voucher" (frmInvoicesAdjustmentVoucher, kind "payment") and
 * 863 "Receipt Invoices Adjustment Voucher" (frmReceiptInvoicesAdjustmentVoucher, kind "receipt").
 * The two desktop forms are one code base; the differences are marked "861" / "863" below.
 *
 * Kept exactly as the desktop does it:
 *  - Pending rows put MobilePersonal into "PartyCity" and CityName into "PartyCellNo", so after a
 *    Load the City box shows the mobile number and the Cell box the city (a ReadById shows them
 *    the right way round).
 *  - With ERP feature 4 on, the Party combo gets a "...Select Any Value..." first row and a Load
 *    or an Edit leaves it selected, so the party has to be picked again before saving.
 *  - The Detail grid has no Delete column: a row cannot be removed.
 *  - After a save the form re-runs Show with the Party filter still set, which reloads that
 *    party's open invoices into the Detail grid.
 * Not ported: attachments.
 * ============================================================================================ */
(function () {
    'use strict';
    var KIND = document.body.getAttribute('data-kind') || 'payment';
    var RECEIPT = KIND === 'receipt';
    var SLIP = document.body.getAttribute('data-slip') || '901_VoucherInvoicesAdjustment_Slip';
    var API = '/accounts/api/adjustment-voucher/' + KIND;
    var $ = function (id) { return document.getElementById(id); };
    var SELECT_TEXT = '...Select Any Value...';

    var S = {
        rights: {}, feature4: false, defaultDays: 0,
        pending: [], shown: [], checkMode: false,
        voucher: null, paymentType: null, suppliers: [], grid: [],
        recId: 0, multiUtilization: false
    };

    function toInt(v) { var n = parseInt(String(v === undefined || v === null ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(r, c) { return GD.col(r, c); }
    function fail(e) { if (e === BUSY) return; alert(e && e.message ? e.message : e); }

    /* R2 2026-10-02: every server button is disabled at once with a spinner, a second request is
       refused, and the button goes back to its previous state on success and on failure. */
    var BUSY = { busy: true }, inflight = {};
    function busy(id, key, run) {
        var b = id ? $(id) : null;
        if (inflight[key]) return Promise.resolve(false);
        inflight[key] = true;
        var was = b ? b.disabled : false;
        if (b) { b.disabled = true; b.classList.add('busy'); }
        function done() { inflight[key] = false; if (b) { b.classList.remove('busy'); b.disabled = was; } }
        var p;
        try { p = Promise.resolve(run()); } catch (e) { done(); return Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    }

    /** focus a combo: the searchable field when countx_prod_combo has replaced the select */
    function focusCombo(id) {
        var el = $(id); if (!el) return;
        if (el.__dtcombo && el.__dtcombo.input) el.__dtcombo.input.focus(); else el.focus();
    }
    function val(id) { var e = $(id); return e ? e.value : ''; }

    /* R3 2026-10-02: desktop column widths - grdPendingOrdersSetting / grdSetting / HistoryGridSetting with
       Constants.InventoryConstants (DocumentTypeCode 70, DateConstant 73, DocNoConstant 60, VoucherNo 65, AmountConstant 90,
       AccountTitle 150, AccountCode 100, InvoiceNoConstant 100, RemarksConstants 100, NoOfAttachmentsConstants 100,
       DateTimeConstantddMMM 135, EntryUser/ModifyUserConstants 80). */
    var W_PENDING = { Load: 50, PaymentType: 130, 'V.Type': 70, VoucherDate: 73, VoucherCode: 60, AccountCode: 100, AccountTitle: 150,
        Amount: 90, VoucherRemarks: 130, NoOfAttachments: 100 };
    var W_GRID = { 'Invoice Type': 70, InvoiceNo: 100, InvoiceDate: 73, InvoiceDueDate: 73, InvoiceAmount: 90, AvailableInvoiceAmount: 90,
        AdjustmentAmount: 90, BalanceAmount: 90, Remarks: 100 };
    var W_HISTORY = { Edit: 50, Print: 50, VoucherCode: 65, VoucherDate: 73, VoucherAmount: 90, AccountCode: 100, AccountTitle: 150,
        PaymentType: 130, 'Invoice Type': 70, InvoiceNos: 100, InvoiceDates: 73, AdjustmentAmount: 90, EntryDate: 135, EntryUserName: 80,
        ModifyDate: 135, ModifyUserName: 80, VoucherRemarks: 110 };
    function widths(h, map) {
        return h.replace(/<th>([^<]*)<\/th>/g, function (m, c) {
            return map[c] ? '<th style="width:' + map[c] + 'px;min-width:' + map[c] + 'px">' + c + '</th>' : m;
        });
    }

    /** .NET "#,##.##" : no digits for zero, no trailing zeros */
    function fmtHash(v) {
        var n = GD.num(v);
        if (n === 0) return '';
        var s = Math.abs(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 });
        return (n < 0 ? '-' : '') + s;
    }

    // ================================================================ InitializeComponentMethod
    function load() {
        GD.get(API + '/init').then(function (d) {
            S.rights = d.rights || {};
            S.feature4 = !!d.subsidiaryFeature;
            S.defaultDays = d.defaultDaysToLessFromHistoryFromDate || 0;
            S.yearStart = d.financialYearStart || '';
            GD.setDecimals(d.amountDecimals, 2);
            $('btnSave').disabled = !S.rights.canSave;
            $('btnUpdate').disabled = !S.rights.canUpdate;
            $('btnDelete').disabled = !S.rights.canDelete;
            $('btnSlip').disabled = !S.rights.canPrint;
            $('ChkPrintslip').checked = !!S.rights.canPrint;
            renderGrid();
            fillPending(d.pending || []);
            var from = new Date(); from.setDate(from.getDate() - (S.defaultDays > 0 ? S.defaultDays : 3));
            $('txtFromdateHistory').value = GD.iso(from);
            $('txtToDateHistory').value = GD.iso(new Date());
            /* txtVoucherDate: DateTimePicker, no designer Value -> today until a voucher is picked (:509/:1194). */
            if (!$('txtVoucherDate').value) $('txtVoucherDate').value = GD.iso(new Date());
            focusCombo('CmbAccountFilter');
            /* ?id=<VoucherHeadId>&paymentTypeId=&supplierCustomerId=&partyGlId= opens that adjustment
               (ReadById needs all four keys - the history row carries them). */
            var q = new URLSearchParams(window.location.search);
            if (toInt(q.get('id')) > 0 && toInt(q.get('paymentTypeId')) > 0)
                readById(toInt(q.get('id')), toInt(q.get('paymentTypeId')), toInt(q.get('supplierCustomerId')), toInt(q.get('partyGlId')));
        }).catch(function () { alert('Error occurred during database call.'); });
    }

    // ===================================================== FillGrdPendingOrders / GetDistinctParties
    function fillPending(rows) {
        S.pending = rows.map(function (r) {
            return {
                Id: col(r, 'Id'), PaymentTypeId: col(r, 'PaymentTypeId'), PaymentType: col(r, 'PaymentType'),
                DocumentTypeId: col(r, 'DocumentTypeId'), DocumentType: col(r, 'DocumentType'), VoucherDate: col(r, 'VoucherDate'),
                VoucherCode: col(r, 'VoucherCode'), VoucherAmount: col(r, 'VoucherAmount'), AccountId: col(r, 'AccountId'),
                AccountCode: col(r, 'AccountCode'), AccountTitle: col(r, 'AccountTitle'), Amount: col(r, 'Amount'),
                SupplierCustomerId: col(r, 'SupplierCustomerId'), PartyName: col(r, 'PartyName'), PartyCode: col(r, 'PartyCode'),
                PartyCity: col(r, 'MobilePersonal'), PartyCellNo: col(r, 'CityName'),
                VoucherRemarks: col(r, 'Remarks'), NoOfAttachments: col(r, 'NoOfAttachments')
            };
        });
        S.shown = S.pending; S.checkMode = false;
        renderPending();
        // GetDistinctParties: DISTINCT (SupplierCustomerId, PartyName, AccountId) of the raw rows
        var seen = {}, parties = [];
        rows.forEach(function (r) {
            var key = col(r, 'SupplierCustomerId') + '|' + col(r, 'PartyName') + '|' + col(r, 'AccountId');
            if (seen[key]) return; seen[key] = true;
            parties.push({ SupplierCustomerId: col(r, 'SupplierCustomerId'), PartyName: col(r, 'PartyName'), AccountId: col(r, 'AccountId') });
        });
        var keep = val('CmbAccountFilter');
        GD.fill('CmbAccountFilter', parties, 'SupplierCustomerId', 'PartyName', { account: 'AccountId' }, '');
        $('CmbAccountFilter').value = keep;
    }

    /** grdPendingOrdersSetting(addcheckBox) */
    function renderPending() {
        var h = '<thead><tr>' + (S.checkMode ? '<th><input type="checkbox" onclick="ADJ.checkAll(this.checked)"></th>' : '')
            + '<th>Load</th><th>PaymentType</th><th>V.Type</th><th>VoucherDate</th><th>VoucherCode</th><th>AccountCode</th>'
            + '<th>AccountTitle</th><th>Amount</th><th>VoucherRemarks</th><th>NoOfAttachments</th></tr></thead><tbody>';
        var t = 0;
        S.shown.forEach(function (r, i) {
            t += GD.num(r.Amount);
            h += '<tr>' + (S.checkMode ? '<td><input type="checkbox" class="pchk" data-i="' + i + '"></td>' : '')
                + '<td><button type="button" class="pload" onclick="ADJ.loadPending(' + i + ', this)">Load</button></td>'
                + '<td>' + GD.esc(r.PaymentType) + '</td><td>' + GD.esc(r.DocumentType) + '</td>'
                + '<td>' + GD.fmtDate(r.VoucherDate, 'dd-MMM-yy') + '</td><td>' + GD.esc(r.VoucherCode) + '</td>'
                + '<td>' + GD.esc(r.AccountCode) + '</td><td>' + GD.esc(r.AccountTitle) + '</td>'
                + '<td class="num">' + GD.fmtSingle(r.Amount) + '</td><td>' + GD.esc(r.VoucherRemarks) + '</td>'
                + '<td class="num"><a href="#" onclick="alert(\'Attachments are not ported to the web.\');return false;">' + GD.esc(r.NoOfAttachments) + '</a></td></tr>';
        });
        h += '</tbody><tfoot><tr><td colspan="' + (S.checkMode ? 8 : 7) + '"></td><td class="num">' + GD.fmtSingle(t) + '</td><td colspan="2"></td></tr></tfoot>';
        $('grdPendingVouchers').innerHTML = widths(h, W_PENDING);
    }

    // ======================================== grdPendingOrders_ColumnButtonClick "Load"
    function loadPending(i, btn) {
        return busy(null, 'load', function () { if (btn) btn.classList.add('busy'); return loadPendingRow(i); })
            .then(function (v) { if (btn) btn.classList.remove('busy'); return v; }, function (e) { if (btn) btn.classList.remove('busy'); fail(e); });
    }

    function loadPendingRow(i) {
        var r = S.shown[i];
        if (!r) return Promise.resolve();
        if ($('btnSave').hidden || $('btnSave').disabled) { alert('Please Reset the Form First to Load New Data'); return Promise.resolve(); }
        bindVoucherFromPending(r);
        bindPaymentTypeFromPending(r);
        supplierdtFromPendingGrid(toInt(val('CmbVoucher')), toInt(val('CmbPaymentType')));
        bindPartyName();
        return supplierLeave().then(function () { focusCombo('CmbSupplier'); });
    }

    /** BindVoucherFromPending: the one row Id / VoucherCode / VoucherDate / Amount (as VoucherAmount). */
    function bindVoucherFromPending(r) {
        S.voucher = { Id: r.Id, VoucherNo: r.VoucherCode, VoucherDate: r.VoucherDate, VoucherAmount: r.Amount };
        GD.fill('CmbVoucher', [S.voucher], 'Id', 'VoucherNo', null, null);
        voucherLeave();
    }

    /** CmbVoucher_Leave / ValueChanged */
    function voucherLeave() {
        if (!S.voucher || toInt(val('CmbVoucher')) <= 0) return;
        $('txtVoucherDate').value = GD.iso(S.voucher.VoucherDate);
        $('txtVoucherAmountRegular').value = fmtHash(S.voucher.VoucherAmount);
    }

    function bindPaymentTypeFromPending(r) {
        S.paymentType = { Id: r.PaymentTypeId, PaymentType: r.PaymentType };
        GD.fill('CmbPaymentType', [S.paymentType], 'Id', 'PaymentType', null, null);
    }

    /** SupplierdtFromPendingGrid: distinct parties of the DISPLAYED pending rows of this voucher + payment type. */
    function supplierdtFromPendingGrid(voucherHeadId, paymentTypeId) {
        var seen = {};
        S.suppliers = [];
        S.shown.forEach(function (r) {
            if (toInt(r.Id) !== voucherHeadId || toInt(r.PaymentTypeId) !== paymentTypeId) return;
            var sid = toInt(r.SupplierCustomerId);
            if (seen[sid]) return; seen[sid] = true;
            S.suppliers.push({ SupplierCustomerId: sid, PartyName: r.PartyName, PartyCode: r.PartyCode, AccountId: toInt(r.AccountId),
                AccountTitle: r.AccountTitle, AccountCode: r.AccountCode, CityName: r.PartyCity, MobileNo: r.PartyCellNo });
        });
    }

    /** BindPartyName: SupplierCustomerId / PartyName with a zero row (feature 4 on), else AccountId / AccountTitle. */
    function bindPartyName() {
        var prev = toInt(val('CmbSupplier'));
        var sel = $('CmbSupplier');
        if (S.suppliers.length > 0) {
            var h = '';
            if (S.feature4) {
                h += '<option value="0" data-sid="0" data-gl="0">' + SELECT_TEXT + '</option>';
                S.suppliers.forEach(function (r) { h += option(r.SupplierCustomerId, r.PartyName, r); });
                sel.innerHTML = h;
                sel.value = '0';
                if (prev > 0 && S.suppliers.some(function (r) { return r.SupplierCustomerId === prev; })) sel.value = String(prev);
            } else {
                S.suppliers.forEach(function (r) { h += option(r.AccountId, r.AccountTitle, r); });
                sel.innerHTML = h;
                if (prev !== 0 && S.suppliers.some(function (r) { return r.AccountId === prev; })) sel.value = String(prev);
                else sel.selectedIndex = 0;
            }
        } else sel.innerHTML = '';
    }

    function option(v, t, r) {
        return '<option value="' + GD.esc(v) + '" data-sid="' + GD.esc(r.SupplierCustomerId) + '" data-gl="' + GD.esc(r.AccountId)
            + '" data-city="' + GD.esc(r.CityName) + '" data-mobile="' + GD.esc(r.MobileNo) + '">' + GD.esc(t) + '</option>';
    }

    function supplierRow() {
        var sel = $('CmbSupplier');
        if (sel.selectedIndex < 0 || sel.value === '') return null;
        var o = sel.options[sel.selectedIndex];
        return { value: toInt(sel.value), sid: toInt(o.getAttribute('data-sid')), gl: toInt(o.getAttribute('data-gl')),
                 city: o.getAttribute('data-city') || '', mobile: o.getAttribute('data-mobile') || '' };
    }

    /** comsupplier_ValueChanged = BindCustomerCity + BindGridData (CmbSupplier / CmbPaymentType Leave). */
    function supplierLeave() {
        var p = supplierRow();
        $('txtPartyCity').value = ''; $('txtPartyCellNo').value = '';
        if (p && p.value > 0) { $('txtPartyCellNo').value = p.mobile; $('txtPartyCity').value = p.city; }
        return bindGridData();
    }

    /** BindGridData (:681) */
    function bindGridData() {
        var p = supplierRow();
        if (S.suppliers.length === 0 || !p) return Promise.resolve();
        var sid = p.sid;
        if (sid !== toInt(val('CmbAccountFilter')) && S.grid.length === 0) {
            if (toInt(val('CmbVoucher')) === 0) { focusCombo('CmbVoucher'); alert('VoucherNo is Required...'); return Promise.resolve(); }
            if (sid === 0) { focusCombo('CmbSupplier'); alert('Party is Required...'); return Promise.resolve(); }
            S.grid = [];
            return loadInvoiceData(sid);
        } else if (S.multiUtilization && S.grid.length > 0) {
            return loadInvoiceData(sid);
        }
        return Promise.resolve();
    }

    /** LoadInvoiceData + LoadDetailDataFromInvoice: rows are cleared first, so nothing is merged. */
    function loadInvoiceData(supplierId) {
        S.grid = [];
        return GD.get(API + '/invoices', { supplierCustomerId: supplierId }).then(function (rows) {
            (rows || []).forEach(function (x) {
                var bill = GD.num(col(x, 'BillAmount')), avail = GD.num(col(x, 'AvailableBillAmount'));
                S.grid.push({ Id: 0, RefDocumentTypeId: col(x, 'DocumentTypeId'), RefDocumentType: col(x, 'DocumentType'),
                    InvoiceId: toInt(col(x, 'Id')), InvoiceNo: col(x, 'InvoiceNo'), InvoiceDate: col(x, 'InvoiceDate'),
                    InvoiceDueDate: col(x, 'InvoiceDueDate'), InvoiceAmount: bill, AvailableInvoiceAmount: avail,
                    AdjustmentAmount: 0, BalanceAmount: avail, Remarks: '' });
            });
            renderGrid();
        }).catch(fail);
    }

    /** grdSettings: AdjustmentAmount and Remarks are the only editable cells. */
    function renderGrid() {
        var h = '<thead><tr><th>Invoice Type</th><th>InvoiceNo</th><th>InvoiceDate</th><th>InvoiceDueDate</th><th>InvoiceAmount</th>'
            + '<th>AvailableInvoiceAmount</th><th>AdjustmentAmount</th><th>BalanceAmount</th><th>Remarks</th></tr></thead><tbody>';
        var t = { i: 0, a: 0, j: 0, b: 0 };
        S.grid.forEach(function (r, i) {
            t.i += GD.num(r.InvoiceAmount); t.a += GD.num(r.AvailableInvoiceAmount); t.j += GD.num(r.AdjustmentAmount); t.b += GD.num(r.BalanceAmount);
            h += '<tr><td>' + GD.esc(r.RefDocumentType) + '</td><td>' + GD.esc(r.InvoiceNo) + '</td>'
                + '<td>' + GD.fmtDate(r.InvoiceDate, 'dd-MMM-yy') + '</td><td>' + GD.fmtDate(r.InvoiceDueDate, 'dd-MMM-yy') + '</td>'
                + '<td class="num">' + GD.fmtSingle(r.InvoiceAmount) + '</td><td class="num">' + GD.fmtSingle(r.AvailableInvoiceAmount) + '</td>'
                + '<td><input class="cell num" value="' + GD.esc(r.AdjustmentAmount) + '" onchange="ADJ.cellUpdated(' + i + ',this.value)"></td>'
                + '<td class="num">' + GD.fmtSingle(r.BalanceAmount) + '</td>'
                + '<td><input class="cell" value="' + GD.esc(r.Remarks) + '" onchange="ADJ.remarks(' + i + ',this.value)"></td></tr>';
        });
        h += '</tbody><tfoot><tr><td colspan="4"></td><td class="num">' + GD.fmtSingle(t.i) + '</td><td class="num">' + GD.fmtSingle(t.a)
            + '</td><td class="num">' + GD.fmtSingle(t.j) + '</td><td class="num">' + GD.fmtSingle(t.b) + '</td><td></td></tr></tfoot>';
        $('grd').innerHTML = widths(h, W_GRID);
    }

    /** grd_CellUpdated (:810) */
    function cellUpdated(i, v) {
        var r = S.grid[i]; if (!r) return;
        var adj = GD.num(v), avail = GD.num(r.AvailableInvoiceAmount);
        if (adj > avail) { adj = avail; r.AdjustmentAmount = adj; alert('AdjustmentAmount can not be greater than AvailableInvoiceAmount'); }
        r.AdjustmentAmount = adj;
        r.BalanceAmount = avail - adj;
        renderGrid();
    }

    // ============================================================ TotalAmountAutoUtilizeInGrid
    function autoUtilize() {
        var total = GD.num(val('txtVoucherAmountRegular'));
        var gridTotal = 0; S.grid.forEach(function (r) { gridTotal += GD.num(r.AdjustmentAmount); });
        var zero = S.grid.filter(function (r) { return GD.num(r.AdjustmentAmount) === 0; });
        if (total >= gridTotal) {
            total -= gridTotal;
            for (var i = 0; i < zero.length; i++) {
                var r = zero[i];
                var inv = GD.num(r.AvailableInvoiceAmount);
                var apply = Math.min(total, Math.min(GD.num(r.BalanceAmount), inv));
                r.AdjustmentAmount = apply;
                r.BalanceAmount = inv - apply;
                total -= apply;
                if (total <= 0) break;
            }
        } else {
            var used = 0;
            S.grid.forEach(function (r) {
                if (used >= total) { r.AdjustmentAmount = 0; r.BalanceAmount = GD.num(r.InvoiceAmount); }
                used += GD.num(r.AdjustmentAmount);
            });
        }
        renderGrid();
    }

    // ===================================================================== Reset / New
    function reset() {
        S.recId = 0; S.multiUtilization = false;
        $('btnSave').hidden = false; $('btnUpdate').hidden = true; $('btnDelete').hidden = true;
        S.voucher = null; $('CmbVoucher').innerHTML = '';
        $('txtVoucherAmountRegular').value = '';
        S.paymentType = null; $('CmbPaymentType').innerHTML = '';
        S.suppliers = []; $('CmbSupplier').innerHTML = '';
        $('txtPartyCellNo').value = ''; $('txtPartyCity').value = '';
        S.grid = []; renderGrid();
        return GD.get(API + '/pending').then(function (rows) { fillPending(rows || []); focusCombo('CmbAccountFilter'); }).catch(fail);
    }

    function newForm() { return reset().then(function () { $('CmbAccountFilter').value = ''; }); }

    /** btnRefresh_Click */
    function refresh() {
        supplierdtFromPendingGrid(toInt(val('CmbVoucher')), toInt(val('CmbPaymentType')));
        bindPartyName();
        return supplierLeave();
    }

    // =================================================================== btnShowRecords_Click
    function showRecords(clicked) {
        S.grid = [];
        var acc = toInt(val('CmbAccountFilter'));
        if (acc > 0) {
            var filtered = S.pending.filter(function (r) { return toInt(r.SupplierCustomerId) === acc; });
            var p = loadInvoiceData(acc);
            if (filtered.length === 0) {
                // 861 throws (the message surfaces); 863 shows it and leaves the pending grid as it was
                return p.then(function () { alert('No pending vouchers found for the selected account.'); });
            }
            S.shown = filtered; S.checkMode = true;
            renderPending();
            return p;
        }
        S.shown = S.pending; S.checkMode = false;
        renderPending();
        renderGrid();
        return Promise.resolve();
    }

    function checkAll(on) { Array.prototype.forEach.call(document.querySelectorAll('.pchk'), function (c) { c.checked = on; }); }

    // =================================================================== Insert(Auto)
    function body() {
        var p = supplierRow();
        return {
            recId: S.recId, voucherHeadId: toInt(val('CmbVoucher')),
            voucherCode: $('CmbVoucher').selectedIndex >= 0 ? $('CmbVoucher').options[$('CmbVoucher').selectedIndex].text : '',
            voucherDate: S.voucher ? GD.isoTime(S.voucher.VoucherDate) : '', voucherAmountText: val('txtVoucherAmountRegular'),
            paymentTypeId: toInt(val('CmbPaymentType')), supplierCustomerId: p ? p.sid : 0, partyGlId: p ? p.gl : 0,
            partyValue: p ? p.value : 0,
            lines: S.grid.map(function (r) {
                return { id: toInt(r.Id), refDocumentTypeId: toInt(r.RefDocumentTypeId), invoiceId: toInt(r.InvoiceId),
                    invoiceAmount: GD.num(r.InvoiceAmount), adjustmentAmount: GD.num(r.AdjustmentAmount), remarks: r.Remarks || '' };
            })
        };
    }

    function validateHeader() {
        if (toInt(val('CmbVoucher')) === 0) { alert('Voucher No Field is Required'); return false; }
        var a = val('txtVoucherAmountRegular').trim();
        if (a === '' || a === '0') { alert('VoucherAmount Field is Required'); return false; }
        if (toInt(val('CmbPaymentType')) === 0) { alert('Payment Type Field is Required'); return false; }
        var p = supplierRow();
        if (!p || p.value === 0) { alert('Party Field is Required'); focusCombo('CmbSupplier'); return false; }
        return true;
    }

    function insert(auto) {
        if (S.grid.length === 0) { alert('Grid Record Not Found'); return Promise.resolve(false); }
        if (!validateHeader()) return Promise.resolve(false);
        if (!auto && !confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve(false);
        var b = body();
        var call = function () { return GD.api('POST', API + '/save', b); };
        return (auto ? call() : busy(S.recId > 0 ? 'btnUpdate' : 'btnSave', 'save', call)).then(function (res) {
            if (res === false) return false;
            if (auto) return true;
            alert(res.message);
            return reset().then(function () {
                if ($('ChkPrintslip').checked) generateSlip(b.voucherHeadId, b.paymentTypeId, b.supplierCustomerId, b.partyGlId, 1);
                return showRecords(false);
            }).then(function () { return true; });
        }).catch(function (e) { fail(e); return false; });
    }

    function save(isUpdate) {
        if (!isUpdate) { S.recId = 0; return insert(false); }
        if (S.recId === 0) { alert('Record Not found'); return Promise.resolve(false); }
        return insert(false);
    }

    /** btnDelete_Click (:1224) */
    function del() {
        if (S.recId === 0) { alert('Record Id not found for deletion...'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        busy('btnDelete', 'delete', function () { return GD.api('POST', API + '/delete', body()); })
            .then(function (res) { if (res === false) return; alert(res.message); return reset(); }).catch(fail);
    }

    // ============================================================ BtnAutoUtilizedAllRowsOfPendingGrid
    function autoAllClick() { return busy('BtnAutoUtilizedAllRowsOfPendingGrid', 'autoAll', autoAll).catch(fail); }

    function autoAll() {
        if ($('btnSave').hidden || $('btnSave').disabled) { alert('Please Reset the Form First to Load New Data'); return; }
        var acc = toInt(val('CmbAccountFilter'));
        if (acc === 0) { focusCombo('CmbAccountFilter'); alert('Please select an account first.'); return; }
        if (S.shown.length === 0) { alert('Pending Vouchers Detail have no Records...'); return; }
        var checked = Array.prototype.filter.call(document.querySelectorAll('.pchk'), function (c) { return c.checked; })
            .map(function (c) { return S.shown[toInt(c.getAttribute('data-i'))]; });
        if (checked.length === 0) { alert("Please Select Any row First.Press show Button If 'Select' Button not Appear"); return; }
        S.multiUtilization = true;
        var chain = Promise.resolve();
        checked.forEach(function (row) {
            chain = chain.then(function () {
                bindVoucherFromPending(row);
                bindPaymentTypeFromPending(row);
                supplierdtFromPendingGrid(toInt(val('CmbVoucher')), toInt(val('CmbPaymentType')));
                bindPartyName();
                return supplierLeave().then(function () { autoUtilize(); return insert(true); });
            });
        });
        chain = chain.then(function () { return reset(); }).then(function () { $('CmbAccountFilter').value = ''; });
        return chain;
    }

    // ============================================================================ prints
    function generateSlip(voucherHeadId, paymentTypeId, supplierCustomerId, partyGlId, id) {
        if (RECEIPT && id === 0) { alert('No Record Found For Display'); return; }
        GD.get(API + '/by-voucher-head', { voucherHeadId: voucherHeadId, paymentTypeId: paymentTypeId,
            supplierCustomerId: supplierCustomerId, partyGlId: partyGlId }).then(function (rows) {
            if (!rows || rows.length === 0) { alert(RECEIPT ? 'No Record Found For Display' : 'Not Record Found For Display'); return; }
            var args = { transactionTypeId: RECEIPT ? 2 : 1, voucherHeadId: voucherHeadId, paymentTypeId: paymentTypeId,
                         supplierCustomerId: supplierCustomerId, partyGlId: partyGlId };
            if (window.printRpt) window.printRpt(SLIP + '.rpt', args); else GD.printRows(SLIP, rows);
        }).catch(fail);
    }

    /** btnSlip_Click */
    function slipClick() {
        var p = supplierRow();
        var sid = 0, gl = 0;
        if (p && p.value > 0) { sid = p.sid; gl = p.gl; }
        generateSlip(toInt(val('CmbVoucher')), toInt(val('CmbPaymentType')), sid, gl, S.recId);
    }

    // ============================================================================ History
    function tab(i) {
        $('tabForm').classList.toggle('on', i === 0); $('tabHistory').classList.toggle('on', i === 1);
        $('pageForm').hidden = i !== 0; $('pageHistory').hidden = i !== 1;
        if (i === 1) {
            if ($('CmbCustomerHistory').options.length === 0) historyParties();
            if (!S.dateTypeBound) { S.dateTypeBound = true; $('cmbDateTypeHistory').value = '1'; dateTypeChanged(); }
        }
    }

    /** HistoryComboBind */
    function historyParties() {
        return GD.get(API + '/parties').then(function (rows) {
            var h = '<option value="0" data-sid="0" data-gl="0">' + SELECT_TEXT + '</option>';
            (rows || []).forEach(function (r) {
                var v = S.feature4 ? col(r, 'SupplierCustomerId') : col(r, 'AccountId');
                var t = S.feature4 ? col(r, 'PartyName') : col(r, 'AccountTitle');
                h += '<option value="' + GD.esc(v) + '" data-sid="' + GD.esc(col(r, 'SupplierCustomerId')) + '" data-gl="' + GD.esc(col(r, 'AccountId')) + '">' + GD.esc(t) + '</option>';
            });
            var keep = val('CmbCustomerHistory');
            $('CmbCustomerHistory').innerHTML = rows && rows.length ? h : '';
            if (keep) $('CmbCustomerHistory').value = keep;
        }).catch(fail);
    }

    /** cmbDateTypeHistory_ValueChanged */
    function dateTypeChanged() {
        var v = toInt(val('cmbDateTypeHistory')), now = new Date();
        if (v === 1) $('txtFromdateHistory').value = GD.iso(now);
        else if (v === 2) { var w = new Date(); w.setDate(w.getDate() - 7); $('txtFromdateHistory').value = GD.iso(w); }
        else if (v === 3) { $('txtFromdateHistory').value = GD.iso(new Date(now.getUTCFullYear(), now.getUTCMonth(), 1)); $('txtToDateHistory').value = GD.iso(now); }
        else if (v === 4) { $('txtFromdateHistory').value = GD.iso(new Date(now.getFullYear(), 0, 1)); $('txtToDateHistory').value = GD.iso(now); }
        else if (v === 5) { $('txtFromdateHistory').value = GD.iso(S.yearStart) || GD.iso('1900-01-01'); }
    }

    function resetHistory() {
        $('cmbDateTypeHistory').value = '3'; dateTypeChanged();
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = '';
        $('CmbCustomerHistory').value = '';
    }

    /** FillHistory (:1446) */
    function history() {
        var dt = (document.querySelector('input[name=histDate]:checked') || {}).value || 'doc';
        var p = { dateType: dt, fromDocNo: toInt(val('txtFromNoHistory')), toDocNo: toInt(val('txtToDocNoHistory')) };
        if ($('chkFrom').checked) p.from = val('txtFromdateHistory');
        if ($('chkTo').checked) p.to = val('txtToDateHistory');
        var sel = $('CmbCustomerHistory');
        if (sel.selectedIndex >= 0 && toInt(sel.value) > 0) {
            var o = sel.options[sel.selectedIndex];
            p.supplierCustomerId = toInt(o.getAttribute('data-sid'));
            p.glAccountId = toInt(o.getAttribute('data-gl'));
        }
        return GD.get(API + '/history', p).then(function (rows) {
            S.histSel = -1;
            if (!rows || rows.length === 0) { $('grdHistory').innerHTML = ''; S.history = []; return; }
            S.history = rows.map(function (r) {
                return { VoucherHeadId: col(r, 'VoucherHeadId'), VoucherCode: col(r, 'VoucherCode'), VoucherDate: col(r, 'VoucherDate'),
                    VoucherAmount: col(r, 'VoucherAmount'), PartyGlId: col(r, 'PartyGlId'), AccountCode: col(r, 'PartyGlAccountCode'),
                    AccountTitle: col(r, 'PartyGlAccountTitle'), PaymentTypeId: col(r, 'PaymentTypeId'), PaymentType: col(r, 'PaymentType'),
                    RefDocumentType: col(r, 'RefDocumentType'), InvoiceNos: col(r, 'RefDocNos'), InvoiceDates: col(r, 'RefDocDates'),
                    // PaymentTypeId 1 reads AdvanceAmount on BOTH forms
                    AdjustmentAmount: toInt(col(r, 'PaymentTypeId')) === 1 ? col(r, 'AdvanceAmount') : col(r, 'AdjustmentAmount'),
                    SupplierCustomerId: col(r, 'SupplierCustomerId'), EntryDate: col(r, 'EntryDate'), EntryUserName: col(r, 'EntryUserName'),
                    ModifyDate: col(r, 'ModifyDate'), ModifyUserName: col(r, 'ModifyUserName'), VoucherRemarks: col(r, 'Remarks') };
            });
            var h = '<thead><tr><th>Edit</th><th>Print</th><th>VoucherCode</th><th>VoucherDate</th><th>VoucherAmount</th><th>AccountCode</th>'
                + '<th>AccountTitle</th><th>PaymentType</th><th>Invoice Type</th><th>InvoiceNos</th><th>InvoiceDates</th><th>AdjustmentAmount</th>'
                + '<th>EntryDate</th><th>EntryUserName</th><th>ModifyDate</th><th>ModifyUserName</th><th>VoucherRemarks</th></tr></thead><tbody>';
            var tv = 0, ta = 0;
            S.history.forEach(function (r, i) {
                tv += GD.num(r.VoucherAmount); ta += GD.num(r.AdjustmentAmount);
                h += '<tr data-i="' + i + '" onclick="ADJ.selHist(' + i + ', this)"><td><button type="button" onclick="ADJ.edit(' + i + ')">Edit</button></td>'
                    + '<td><button type="button" onclick="ADJ.printRow(' + i + ')">Print</button></td>'
                    + '<td><a href="#" class="vlink" title="Open this voucher" onclick="ADJ.edit(' + i + ');return false;">' + GD.esc(r.VoucherCode) + '</a></td><td>' + GD.fmtDate(r.VoucherDate, 'dd-MMM-yy') + '</td>'
                    + '<td class="num">' + GD.fmtSingle(r.VoucherAmount) + '</td><td>' + GD.esc(r.AccountCode) + '</td>'
                    + '<td>' + GD.esc(r.AccountTitle) + '</td><td>' + GD.esc(r.PaymentType) + '</td><td>' + GD.esc(r.RefDocumentType) + '</td>'
                    + '<td>' + GD.esc(r.InvoiceNos) + '</td><td>' + GD.esc(r.InvoiceDates) + '</td>'
                    + '<td class="num">' + GD.fmtSingle(r.AdjustmentAmount) + '</td>'
                    + '<td>' + GD.fmtDate(r.EntryDate, 'dd-MMM-yyyy hh:mm tt') + '</td><td>' + GD.esc(r.EntryUserName) + '</td>'
                    + '<td>' + GD.fmtDate(r.ModifyDate, 'dd-MMM-yyyy hh:mm tt') + '</td><td>' + GD.esc(r.ModifyUserName) + '</td>'
                    + '<td>' + GD.esc(r.VoucherRemarks) + '</td></tr>';
            });
            h += '</tbody><tfoot><tr><td colspan="4"></td><td class="num">' + GD.fmtSingle(tv) + '</td><td colspan="6"></td><td class="num">'
                + GD.fmtSingle(ta) + '</td><td colspan="5"></td></tr></tfoot>';
            $('grdHistory').innerHTML = widths(h, W_HISTORY);
        }).catch(fail);
    }

    function selHist(i, tr) {
        S.histSel = i;
        Array.prototype.forEach.call(document.querySelectorAll('#grdHistory tr.sel'), function (x) { x.classList.remove('sel'); });
        if (tr) tr.classList.add('sel');
    }

    /** grdHistory_ColumnButtonClick "Edit" -> ReadById (:1170) */
    function edit(i) {
        var r = S.history[i]; if (!r) return;
        return readById(toInt(r.VoucherHeadId), toInt(r.PaymentTypeId), toInt(r.SupplierCustomerId), toInt(r.PartyGlId));
    }

    function printRow(i) {
        var r = S.history[i]; if (!r) return;
        generateSlip(toInt(r.VoucherHeadId), toInt(r.PaymentTypeId), toInt(r.SupplierCustomerId), toInt(r.PartyGlId), toInt(r.VoucherHeadId));
    }

    function readById(voucherHeadId, paymentTypeId, supplierCustomerId, partyGlId) {
        return busy(null, 'load', function () { return readByIdRun(voucherHeadId, paymentTypeId, supplierCustomerId, partyGlId); });
    }

    function readByIdRun(voucherHeadId, paymentTypeId, supplierCustomerId, partyGlId) {
        return reset().then(function () {
            S.recId = voucherHeadId;
            return GD.get(API + '/by-voucher-head', { voucherHeadId: voucherHeadId, paymentTypeId: paymentTypeId,
                supplierCustomerId: supplierCustomerId, partyGlId: partyGlId });
        }).then(function (rows) {
            if (!rows || rows.length === 0) return;
            tab(0);
            $('btnSave').hidden = true; $('btnUpdate').hidden = false; $('btnDelete').hidden = false;
            var f = rows[0];
            S.voucher = { Id: col(f, 'VoucherHeadId'), VoucherNo: col(f, 'VoucherCode'), VoucherDate: col(f, 'VoucherDate'), VoucherAmount: col(f, 'VoucherAmount') };
            GD.fill('CmbVoucher', [S.voucher], 'Id', 'VoucherNo', null, null);
            voucherLeave();
            S.paymentType = { Id: col(f, 'PaymentTypeId'), PaymentType: col(f, 'PaymentType') };
            GD.fill('CmbPaymentType', [S.paymentType], 'Id', 'PaymentType', null, null);
            S.suppliers = [{ SupplierCustomerId: toInt(col(f, 'SupplierCustomerId')), PartyName: col(f, 'PartyName'), PartyCode: col(f, 'PartyCode'),
                AccountId: toInt(col(f, 'PartyGlId')), AccountTitle: col(f, 'PartyGlAccountTitle'), AccountCode: col(f, 'PartyGlAccountCode'),
                CityName: col(f, 'PartyCityName'), MobileNo: col(f, 'MobilePersonal') }];
            bindPartyName();
            $('CmbSupplier').selectedIndex = 0;            // Rows[0].Activate() — the zero row when feature 4 is on
            S.grid = rows.map(function (x) {
                var inv = GD.num(col(x, 'InvoiceAmount'));
                var adj = paymentTypeId === 1 ? GD.num(col(x, 'AdvanceAmount')) : GD.num(col(x, 'AdjustmentAmount'));
                return { Id: toInt(col(x, 'Id')), RefDocumentTypeId: col(x, 'RefDocumentTypeId'), RefDocumentType: col(x, 'RefDocumentType'),
                    InvoiceId: toInt(col(x, 'RefDocNoId')), InvoiceNo: toInt(col(x, 'RefDocNo')), InvoiceDate: col(x, 'RefDocDate'),
                    InvoiceDueDate: col(x, 'DueDate'), InvoiceAmount: inv, AvailableInvoiceAmount: inv, AdjustmentAmount: adj,
                    BalanceAmount: inv - adj, Remarks: col(x, 'Remarks') || '' };
            });
            renderGrid();
            return supplierLeave();
        }).catch(fail);
    }

    function shortcuts() {
        alert(['Ctrl+S  For Save', 'Ctrl+U  For Update', 'Ctrl+Shift+Delete  For Delete', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh',
            'Ctrl+N  For New', 'Ctrl+P  For Print Slip 901', 'Alt+1  For Print Slip 901', 'Ctrl+F5  For Focus on DocDate',
            'Ctrl+F10  For Open Attachments', 'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form',
            'Ctrl+D  For Adding an row in Focused Grid', 'Ctrl+Delete  For Deleting an row of Focused Grid',
            'Ctrl+ArrowDown  For Focus On Detail Grid', 'Ctrl+ArrowRight  For Change Focus from one Grid To another Grid',
            "Ctrl+Space  When Focus On Any Grid To Call Function's On Button Or Link"].join('\n'));
    }

    document.addEventListener('keydown', function (e) {
        var onForm = !$('pageForm').hidden;
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (e.ctrlKey && e.key.toLowerCase() === 't') { e.preventDefault(); tab(onForm ? 1 : 0); return; }
        if (!onForm) {
            if (!e.ctrlKey) return;
            /* grdHistory_KeyDown: Ctrl+Enter (Update right) = ReadById, Ctrl+P (Print right) = the slip */
            var hr = S.history && S.histSel >= 0 ? S.history[S.histSel] : null;
            if (e.key === 'Enter' && hr) { e.preventDefault(); if (S.rights.canUpdate) edit(S.histSel); return; }
            if (e.key.toLowerCase() === 'p' && hr) { e.preventDefault(); if (S.rights.canPrint) printRow(S.histSel); return; }
            /* Ctrl+F5 / Down / Right / Up on the History tab all focus grdHistory */
            if (e.key === 'F5' || e.key === 'ArrowDown' || e.key === 'ArrowRight' || e.key === 'ArrowUp') { e.preventDefault(); $('grdHistoryWrap').focus(); }
            return;
        }
        if (e.altKey && e.key === '1') { e.preventDefault(); slipClick(); return; }
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase();
        if (k === 's') { e.preventDefault(); if (!$('btnSave').hidden && !$('btnSave').disabled) save(false); }
        else if (k === 'u') { e.preventDefault(); if (!$('btnUpdate').hidden && !$('btnUpdate').disabled) save(true); }
        else if (k === 'n') { e.preventDefault(); newClick(); }
        else if (k === 'r') { e.preventDefault(); refreshClick(); }
        else if (k === 'p') { e.preventDefault(); slipClick(); }
        else if (e.key === 'F5' || e.key === 'ArrowUp') { e.preventDefault(); focusCombo('CmbSupplier'); }
        else if (e.key === 'ArrowDown') { e.preventDefault(); $('grdWrap').focus(); }
        else if (e.key === 'ArrowRight') { e.preventDefault(); if (document.activeElement === $('grdWrap') || $('grdWrap').contains(document.activeElement)) $('grdPendingWrap').focus(); else $('grdWrap').focus(); }
    });

    function newClick() { return busy('btnNew', 'new', newForm).catch(fail); }
    function refreshClick() { return busy('btnRefresh', 'refresh', refresh).catch(fail); }
    function showClick() { return busy('btnShowRecords', 'show', function () { return showRecords(true); }).catch(fail); }
    function historyClick() { return busy('btnshow', 'history', history).catch(fail); }
    function partiesClick() { return busy('BtnRefreshHistory', 'parties', historyParties).catch(fail); }

    window.ADJ = {
        tab: tab, newForm: newForm, refresh: refresh, save: save, del: del, slipClick: slipClick, shortcuts: shortcuts,
        autoAll: autoAll, autoUtilize: autoUtilize, voucherLeave: voucherLeave, supplierLeave: supplierLeave,
        showRecords: showRecords, loadPending: loadPending, checkAll: checkAll, cellUpdated: cellUpdated,
        remarks: function (i, v) { if (S.grid[i]) S.grid[i].Remarks = v; },
        resetHistory: resetHistory, historyParties: historyParties, dateTypeChanged: dateTypeChanged, history: history,
        edit: edit, printRow: printRow, selHist: selHist,
        newClick: newClick, refreshClick: refreshClick, showClick: showClick, historyClick: historyClick,
        partiesClick: partiesClick, autoAllClick: autoAllClick
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', load); else load();
}());
