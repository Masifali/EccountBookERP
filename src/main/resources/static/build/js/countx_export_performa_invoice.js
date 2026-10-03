/* ============================================================================================
 * countx_export_performa_invoice.js - ExImProformaInvoice.cs (Architecture.WinApp.Export), screen 200
 * "Export Performa Invoice" (table ExImProformaInvoice, DocumentTypeId 200). Form | History; API /api/export/performa-invoice.
 * Every desktop event has its counterpart with the desktop's messages and order. Desktop quirks kept on purpose:
 *   - grdDetail DoubleClick puts the row's InnerUOMId (the id, not the UOM text) into the Inner UOM combo TEXT;
 *     btnGridUpdate then writes InnerUOMId twice - Value, then Text - so InnerUOMId ends up holding the combo text
 *     and the InnerUOM column keeps its old text;
 *   - AmountCalculation only runs on Rate / Rate UOM changes (not when the weight changes); Rate UOM empty -> divide
 *     by 0 (Infinity / NaN as .NET prints them);
 *   - the item list is re-read by every DetailFormReset (item()), and DetailFormReset blanks cmbItem, which fires
 *     PackUOM for item 0 (the UOM lists are only re-bound when rows come back);
 *   - Rate / Inner Qty / Outer Qty / No of Container accept digits only (KeyPress);
 *   - the History toolbar "New" has no handler on the desktop.
 * Button contract: disabled + spinner while a request runs, no duplicates, re-enabled on success and failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExR2;
    var $id = X.$id, box = X.box, esc = X.esc, str = X.str, netI = X.netI, netD = X.netD, clr = X.clr;
    var API = '/api/export/performa-invoice';
    var STATUS = [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Complete' }, { Id: 3, Status: 'Cancel' }];
    var PERM = { Save: true, Update: true };
    var S = { recId: 0, updateMode: false, rows: [], updateIdx: -1, innerRaw: null, cur: -1 };
    var HIST = [], HFILTER = {};
    var uomSeq = 0;

    // ------------------------------------------------------------------------------ tabs

    function tab(panelId) {
        document.querySelectorAll('.win-tabs[data-tabs="epi"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['tabPage1', 'tabPage2'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'tabPage2';
        var fb = $id('btnEpiFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        /* tabControl1_SelectedIndexChanged: SelectedIndex 1 -> HistoryGridFill */
        if (onHist) historyGridFill();
    }
    function onHistory() { return $id('tabPage2').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'tabPage1' : 'tabPage2'); }

    // ------------------------------------------------------------------------------ load

    function activateFirst(id) { var s = $id(id); if (s && s.options.length > 1) { s.selectedIndex = 1; X.refreshCombos(); } }

    /** ImProformaInvoice_Load. */
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            $id('btnsave').disabled = !PERM.Save;
            $id('btnupdate').disabled = !PERM.Update;
            if (d.docNo) X.setText('txtdocno', d.docNo);
            if (d.branches && d.branches.length) { X.bind('cmbbranches', d.branches, 'Id', 'BranchName'); activateFirst('cmbbranches'); }
            if (d.projects && d.projects.length) { X.bind('cmbproject', d.projects, 'Id', 'ProjectName'); activateFirst('cmbproject'); }
            if (d.customers && d.customers.length) X.bind('cmbCustomer', d.customers, 'Id', 'CompanyName');
            if (d.ports && d.ports.length) { X.bind('cmbLoadingPort', d.ports, 'Id', 'PortName'); X.bind('cmbDestinationPort', d.ports, 'Id', 'PortName'); }
            if (d.currencies && d.currencies.length) X.bind('cmbFcyCode', d.currencies, 'Id', 'CurrencyCode');
            X.bind('cmbStatus', STATUS, 'Id', 'Status'); activateFirst('cmbStatus');
            if (d.items && d.items.length) X.bind('cmbItem', d.items, 'Id', 'ItemName');
            if (d.packTypes && d.packTypes.length) X.bind('cmbPackType', d.packTypes, 'Id', 'Description');
            X.setText('txtDocDate', X.today()); X.setText('txtLastDate', X.today());
            showEntryButtons(false);
            render();
            $id('epiFooterInfo').textContent = 'ExImProformaInvoice  -  Document Type 200';
            X.focus('cmbCustomer');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    function generateCode() {
        return X.getJson(API + '/new-code').then(function (d) { if (d && d.docNo) X.setText('txtdocno', d.docNo); }).catch(function (e) { box(e.message); });
    }

    /** item(): re-binds cmbItem when rows come back. */
    function reloadItems() {
        return X.getJson(API + '/items').then(function (rows) { if (rows && rows.length) X.bind('cmbItem', rows, 'Id', 'ItemName'); }).catch(function (e) { box(e.message); });
    }

    /** PackUOM (cmbItem ValueChanged): Rate / Inner / Outer UOM, Id + Equivalent, only re-bound when rows exist. */
    function packUom(itemId) {
        var seq = ++uomSeq;
        return X.getJson(API + '/uoms?itemId=' + encodeURIComponent(netI(itemId))).then(function (rows) {
            if (seq !== uomSeq) return;
            if (rows && rows.length) {
                X.bind('cmbRateUOM', rows, 'Id', 'Equivalent');
                X.bind('cmbInnerUOM', rows, 'Id', 'Equivalent');
                X.bind('cmbOuterUOM', rows, 'Id', 'Equivalent');
            }
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ calculations

    /** WeightCalculation: Outer Qty * Outer UOM text. */
    function weightCalculation() {
        if (X.val('txtOuterUnit') !== '' && innerText('cmbOuterUOM') !== '') {
            X.setText('txtweightDetail', clr(netD(X.val('txtOuterUnit')) * netD(innerText('cmbOuterUOM'))));
        }
    }
    /** AmountCalculation: Rate / Rate UOM text * Weight. */
    function amountCalculation() {
        if (X.val('txtweightDetail') !== '' && X.val('txtrate') !== '') {
            var rate = netD(X.val('txtrate')), c = netD(innerText('cmbRateUOM')), a = netD(X.val('txtweightDetail'));
            X.setText('txtAmountFC', clr(rate / c * a));
        }
    }
    function innerText(id) { return X.txt(id); }

    // ------------------------------------------------------------------------------ detail grid

    var COLS = [
        { key: 'Item' }, { key: 'ItemDesc' }, { key: 'PackType' }, { key: 'InnerQty', num: true }, { key: 'InnerUOM' },
        { key: 'OuterQty', num: true }, { key: 'OuterUOM' }, { key: 'Weight', fmt: '0,0', sum: '0,0' }, { key: 'Rate', fmt: '0,0', sum: '0,0' },
        { key: 'RateUOM' }, { key: 'FcyAmount', fmt: '0,0', sum: '0,0' }
    ];
    function render() {
        X.drawGrid('epiBody', 'epiFoot', S.rows, COLS, { current: S.cur });
    }

    function showEntryButtons(editing) {
        X.show('btnAddinGrid', !editing);
        X.show('btnGridUpdate', editing);
        X.show('btnCancel', editing);
    }

    /** DetailFormValidation. */
    function detailValidation() {
        var checks = [
            [function () { return X.txt('cmbItem').trim() === ''; }, 'Please Select item', 'cmbItem'],
            [function () { return X.txt('cmbPackType').trim() === ''; }, 'Please Select PackType', 'cmbPackType'],
            [function () { return X.val('txtInnerUnit').trim() === ''; }, 'Please Insert InnerUnit', 'txtInnerUnit'],
            [function () { return X.txt('cmbInnerUOM').trim() === ''; }, 'Please Select Inner Uom', 'cmbInnerUOM'],
            [function () { return X.val('txtOuterUnit').trim() === ''; }, 'Please Enter Outer  Uom', 'txtOuterUnit'],
            [function () { return X.txt('cmbOuterUOM').trim() === ''; }, 'Please Select Outer Uom', 'cmbOuterUOM'],
            [function () { return X.val('txtweightDetail').trim() === ''; }, 'Please Insert Weight', 'txtweightDetail'],
            [function () { return X.val('txtrate').trim() === ''; }, 'Please Enter Rate', 'txtrate'],
            [function () { return X.txt('cmbRateUOM').trim() === ''; }, 'Please Select Rate Uom', 'cmbRateUOM'],
            [function () { return X.val('txtAmountFC').trim() === ''; }, 'Please Enter Fc Amount', 'txtAmountFC']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]()) { box(checks[i][1]); X.focus(checks[i][2]); return false; }
        return true;
    }

    function selVal(id) { var v = X.val(id); return v === '__raw' ? null : v; }

    /** btnAddinGrid_Click. */
    function btnAddinGrid() {
        if (!detailValidation()) return;
        S.rows.push({
            ItemId: X.val('cmbItem'), Item: X.txt('cmbItem'), ItemDesc: X.val('txtItemDesc'), PackTypeId: X.val('cmbPackType'), PackType: X.txt('cmbPackType'),
            InnerQty: X.val('txtInnerUnit'), InnerUOMId: selVal('cmbInnerUOM'), InnerUOM: X.txt('cmbInnerUOM'),
            OuterQty: X.val('txtOuterUnit'), OuterUOMId: X.val('cmbOuterUOM'), OuterUOM: X.txt('cmbOuterUOM'),
            Weight: netD(X.val('txtweightDetail')), Rate: netD(X.val('txtrate')), RateUOMId: X.val('cmbRateUOM'), RateUOM: X.txt('cmbRateUOM'),
            FcyAmount: netD(X.val('txtAmountFC'))
        });
        render();
        detailFormReset();
        X.focus('cmbItem');
    }

    function clearRaw() {
        var s = $id('cmbInnerUOM'); if (!s) return;
        var o = s.querySelector('option[value="__raw"]'); if (o) o.parentNode.removeChild(o);
        S.innerRaw = null;
    }
    /** Inner UOM combo Text = the row's InnerUOMId (desktop quirk): a matching display text selects it, otherwise the text is shown as typed. */
    function setInnerText(raw) {
        clearRaw();
        if (X.setByText('cmbInnerUOM', raw)) return;
        if (str(raw) === '') return;
        var s = $id('cmbInnerUOM'), o = document.createElement('option');
        o.value = '__raw'; o.text = str(raw); s.appendChild(o); s.value = '__raw'; S.innerRaw = str(raw);
        X.refreshCombos();
    }

    /** grdDetail_DoubleClick. */
    function rowToEntry(i) {
        var r = S.rows[i]; if (!r) return;
        S.updateIdx = i; S.cur = i;
        X.setVal('cmbItem', r.ItemId);
        X.setVal('cmbPackType', r.PackTypeId);
        X.setText('txtItemDesc', r.ItemDesc);
        X.setText('txtInnerUnit', r.InnerQty);
        X.setText('txtOuterUnit', r.OuterQty);
        X.setText('txtrate', clr(r.Rate));
        /* cmbItem.Value = ... fires ValueChanged -> PackUOM before the UOM texts are set */
        packUom(r.ItemId).then(function () {
            setInnerText(str(r.InnerUOMId));
            X.setByText('cmbOuterUOM', r.OuterUOM);
            X.setByText('cmbRateUOM', r.RateUOM);
            X.setText('txtweightDetail', clr(r.Weight));
            X.setText('txtAmountFC', clr(r.FcyAmount));
        });
        X.setText('txtweightDetail', clr(r.Weight));
        X.setText('txtAmountFC', clr(r.FcyAmount));
        showEntryButtons(true);
        render();
    }

    /** btnGridUpdate_Click. */
    function btnGridUpdate() {
        if (!detailValidation()) return;
        var r = S.rows[S.updateIdx]; if (!r) return;
        r.ItemId = X.val('cmbItem'); r.Item = X.txt('cmbItem'); r.ItemDesc = X.val('txtItemDesc');
        r.PackTypeId = X.val('cmbPackType'); r.PackType = X.txt('cmbPackType');
        r.InnerQty = X.val('txtInnerUnit');
        r.InnerUOMId = selVal('cmbInnerUOM');
        r.InnerUOMId = X.txt('cmbInnerUOM');            /* desktop writes InnerUOMId a second time with the combo Text */
        r.OuterQty = X.val('txtOuterUnit'); r.OuterUOMId = X.val('cmbOuterUOM'); r.OuterUOM = X.txt('cmbOuterUOM');
        r.Weight = netD(X.val('txtweightDetail')); r.Rate = netD(X.val('txtrate'));
        r.RateUOMId = X.val('cmbRateUOM'); r.RateUOM = X.txt('cmbRateUOM'); r.FcyAmount = netD(X.val('txtAmountFC'));
        showEntryButtons(false);
        render();
        detailFormReset();
        X.focus('cmbItem');
    }

    /** DetailFormReset. */
    function detailFormReset() {
        clearRaw();
        ['cmbItem', 'cmbPackType', 'cmbInnerUOM', 'cmbOuterUOM', 'cmbRateUOM'].forEach(function (id) { X.setVal(id, ''); });
        ['txtItemDesc', 'txtInnerUnit', 'txtOuterUnit', 'txtrate', 'txtweightDetail', 'txtAmountFC'].forEach(function (id) { X.setText(id, ''); });
        packUom(0);          /* cmbItem.Text = "" -> ValueChanged -> PackUOM(ItemId 0) */
        reloadItems();       /* item() */
        showEntryButtons(false);
    }

    // ------------------------------------------------------------------------------ header

    /** FormReset. */
    function formReset() {
        X.show('btnsave', true); X.show('btnupdate', false);
        S.recId = 0; S.updateMode = false;
        X.setText('txtdocno', '');
        activateFirst('cmbbranches'); activateFirst('cmbproject'); activateFirst('cmbStatus');
        X.setText('txtDocDate', X.today());
        ['txtcustomerRef', 'txtquotRef', 'txtinquiryRef', 'txtNoofContainer', 'txtRemarks', 'txtShipmentDetail', 'txtPaymentDetail', 'txttermNCondition'].forEach(function (id) { X.setText(id, ''); });
        X.setVal('cmbFcyCode', '');
        X.setText('txtLastDate', X.today());
        generateCode();
        S.rows = []; S.cur = -1; S.updateIdx = -1;
        render();
        detailFormReset();
    }
    function btnNew() { formReset(); }

    /** formvalidation (client side first, the service repeats it). */
    function formValidation() {
        var checks = [
            [X.txt('cmbbranches').trim() === '', 'Branch Is Required', 'cmbbranches'],
            [X.txt('cmbproject').trim() === '', 'Project Is Required', 'cmbproject'],
            [X.val('txtdocno') === '', 'Doc No Is Required', 'txtdocno'],
            [X.txt('cmbDestinationPort').trim() === '', 'Destination Port Is Required', 'cmbDestinationPort'],
            [X.txt('cmbLoadingPort').trim() === '', 'Loading Port Is Required', 'cmbLoadingPort'],
            [X.txt('cmbFcyCode').trim() === '', 'Fcy Code Is Required', 'cmbFcyCode'],
            [netI(X.val('txtNoofContainer')) <= 0, 'No Of Containers Must Be Greater Than 0', 'txtNoofContainer'],
            [X.txt('cmbStatus').trim() === '', 'Status Is Required', 'cmbStatus']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]) { box(checks[i][1]); X.focus(checks[i][2]); return false; }
        return true;
    }

    /** btnsave_Click (btnupdate_Click calls it too). */
    function btnsave(btn) {
        if (!formValidation()) return;
        var header = {
            cmbbranches: X.val('cmbbranches'), cmbbranchesText: X.txt('cmbbranches'), cmbproject: X.val('cmbproject'), cmbprojectText: X.txt('cmbproject'),
            txtdocno: X.val('txtdocno'), txtDocDate: X.val('txtDocDate'), cmbCustomer: X.val('cmbCustomer'),
            txtcustomerRef: X.val('txtcustomerRef'), txtquotRef: X.val('txtquotRef'), txtinquiryRef: X.val('txtinquiryRef'),
            cmbDestinationPort: X.val('cmbDestinationPort'), cmbDestinationPortText: X.txt('cmbDestinationPort'),
            cmbLoadingPort: X.val('cmbLoadingPort'), cmbLoadingPortText: X.txt('cmbLoadingPort'),
            cmbFcyCode: X.val('cmbFcyCode'), cmbFcyCodeText: X.txt('cmbFcyCode'), txtNoofContainer: X.val('txtNoofContainer'),
            txtLastDate: X.val('txtLastDate'), cmbStatusText: X.txt('cmbStatus'), txtRemarks: X.val('txtRemarks'),
            txtShipmentDetail: X.val('txtShipmentDetail'), txtPaymentDetail: X.val('txtPaymentDetail'), txttermNCondition: X.val('txttermNCondition')
        };
        var body = { recId: S.recId, updateMode: S.updateMode, header: header, rows: S.rows };
        var wasUpdate = S.updateMode;
        return X.busy(btn || 'btnsave', function () {
            return X.postJson(API + '/save', body).then(function (r) {
                if (r && r.success) {
                    box(r.message || (wasUpdate ? 'Update SuccessFully' : 'Save SuccessFully'));
                    formReset();
                    X.focus('cmbCustomer');
                }
            });
        });
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = [
        { key: 'DocNo', render: function (r, i) { return '<a href="#" class="win-code" data-i="' + i + '">' + esc(r.DocNo) + '</a>'; }, num: true },
        { key: 'DocDate' }, { key: 'Customer' }, { key: 'NoOfContainer', fmt: '0,0', sum: '0,0' }, { key: 'LoadingPort' }, { key: 'DestinationPort' },
        { key: 'LastShipmentDate' }, { key: 'Status' }, { key: 'Remarks' }
    ];
    var HVIEW = [];
    function renderHistory() {
        HVIEW = X.filterRows(HIST, HFILTER);
        X.drawGrid('epiHistBody', 'epiHistFoot', HVIEW, HCOLS);
        X.show('epiHistEmpty', !HVIEW.length);
    }
    /** HistoryGridFill. */
    function historyGridFill() {
        return X.getJson(API + '/history').then(function (rows) { HIST = rows || []; renderHistory(); }).catch(function (e) { box(e.message); });
    }

    /** ReadById (DataGridHistory DoubleClick). */
    function readById(id) {
        return X.getJson(API + '/by-id?id=' + encodeURIComponent(id)).then(function (d) {
            X.show('btnsave', false); X.show('btnupdate', true);
            S.recId = netI(id);
            var h = d.header;
            tab('tabPage1');
            X.setVal('cmbbranches', h.BranchId);
            X.setVal('cmbproject', h.ProjectId);
            X.setText('txtDocDate', h.DocDate);
            X.setText('txtdocno', h.DocNo);
            X.setVal('cmbCustomer', h.SupplierCustomerId);
            X.setText('txtcustomerRef', h.CustomerRefNo);
            X.setText('txtquotRef', h.QuotReference);
            X.setText('txtinquiryRef', h.InquieryReference);
            X.setVal('cmbDestinationPort', h.SeaPortsIdDestination);
            X.setVal('cmbLoadingPort', h.SeaPortsIdLoading);
            X.setVal('cmbFcyCode', h.MultiCurrencyId);
            X.setText('txtNoofContainer', h.NoOfContainer);
            X.setText('txtLastDate', h.LastShipmentDate);
            X.setByText('cmbStatus', h.Status);
            X.setText('txtRemarks', h.RemarksHeader);
            X.setText('txtShipmentDetail', h.ShipmentDetail);
            X.setText('txtPaymentDetail', h.PaymentDetail);
            X.setText('txttermNCondition', h.OtherTermsConditions);
            S.rows = d.rows || []; S.cur = -1; S.updateIdx = -1;
            render();
            detailFormReset();
            S.updateMode = true;
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ wiring

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs[data-tabs="epi"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        X.wireFullscreen();
        X.on('cmbItem', 'change', function () { clearRaw(); packUom(X.val('cmbItem')); });
        X.on('cmbOuterUOM', 'change', weightCalculation);
        X.on('txtOuterUnit', 'input', weightCalculation);
        X.on('cmbRateUOM', 'change', amountCalculation);
        X.on('txtrate', 'input', amountCalculation);
        X.on('cmbInnerUOM', 'change', function () { if (X.val('cmbInnerUOM') !== '__raw') clearRaw(); });
        var gb = $id('epiBody');
        gb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            S.cur = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) rowToEntry(+tr.getAttribute('data-i')); });
        var hb = $id('epiHistBody');
        hb.addEventListener('click', function (e) {
            var a = e.target.closest('a.win-code');
            if (a) { e.preventDefault(); var r = HVIEW[+a.getAttribute('data-i')]; if (r) readById(r.Id); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (!tr) return; var r = HVIEW[+tr.getAttribute('data-i')]; if (r) readById(r.Id); });
        document.querySelectorAll('#epiHistFilter input[data-f]').forEach(function (inp) {
            inp.addEventListener('input', function () { HFILTER[inp.getAttribute('data-f')] = inp.value; renderHistory(); });
        });
        /* ImProformaInvoice_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (X.enterAsTab(e)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 's' && !onHistory() && !S.updateMode) { e.preventDefault(); if (!$id('btnsave').disabled) btnsave($id('btnsave')); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); X.closeForm(); return; }
            if (e.ctrlKey && k === 'u' && S.updateMode) { e.preventDefault(); if (!$id('btnupdate').disabled) btnsave($id('btnupdate')); }
        });
        load();
    });

    global.ExportEpi = {
        btnNew: btnNew, btnsave: btnsave, toggleHistory: toggleHistory,
        btnAddinGrid: btnAddinGrid, btnGridUpdate: btnGridUpdate, btnCancel: detailFormReset
    };
}(window));
