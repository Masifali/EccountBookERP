/* ============================================================================================
 * countx_export_lc_order.js - LcOrder.cs (Architecture.WinApp.Export), screen 210 "Lc Order", DocumentTypeId 201.
 * Every desktop event has its counterpart with the desktop's messages and order: LcOrder_Load, cmbcustomer_Leave
 * (Performa Invoice), cmbitem_Leave (UOMs), the TextChanged calculations (Local Amount, Weight, Amount), btnaddnew,
 * grdDetail double-click, btnGridUpdate, btnCancel, btnNew, btnSave / btnUpdate, tabControl1 change -> gridFill,
 * DataGridHistory double-click -> ReadById, LcOrder_KeyDown shortcuts.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/api/export/lc-order';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v), 10); return isFinite(n) ? n : 0; }
    /* Conversion.ToInt(text): Convert.ToInt32(string), failures 0 */
    function toIntText(v) { var s = str(v).trim(); return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0; }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    /* FormatString "0,0" (cells) and "#,##0.##" (totals) */
    function fmt00(v) { var n = Math.round(netD(v)); var s = String(Math.abs(n)).replace(/\B(?=(\d{3})+(?!\d))/g, ','); if (Math.abs(n) < 10) s = '0' + s; return (n < 0 ? '-' : '') + s; }
    function fmtTot(v) { var n = Math.round(netD(v) * 100) / 100; var p = String(n).split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }
    function numStr(v) { var n = netD(v); return String(n); }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve(); b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done(); return Promise.resolve(p);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function bind(id, rows) {
        var s = $id(id); if (!s) return;
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.name) + '</option>'; });
        s.innerHTML = h; s.value = '0'; refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = '0'; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function comboText(id) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o && o.value !== '0' ? o.text : ''; }
    /* UltraCombo.Text = x selects the row whose display text is x (or none) */
    function setComboText(id, text) {
        var s = $id(id); if (!s) return; s.value = '0';
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value !== '0' && s.options[i].text === str(text)) { s.selectedIndex = i; break; }
        refreshCombos();
    }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() { global.close(); setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150); }

    // ------------------------------------------------------------------------------ state
    var PERM = { Save: true, Update: true };
    var S = { recId: 0, updateMode: false, rows: [], gridBound: false, idx: -1 };
    var HIST = [];
    var FIXED = {
        two: [{ Id: 1, name: 'Required' }, { Id: 2, name: 'Non-Required' }],
        status: [{ Id: 1, name: 'Open' }, { Id: 2, name: 'Complete' }, { Id: 3, name: 'Cancel' }],
        ship: [{ Id: 1, name: 'Allowed' }, { Id: 2, name: 'Not-Allowed' }]
    };

    // ------------------------------------------------------------------------------ tabs
    function tab(panelId) {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['lcForm', 'lcHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        $id('btnFooterHistory').querySelector('span').textContent = panelId === 'lcHistory' ? 'Form' : 'History';
        gridFill();   /* tabControl1_SelectedIndexChanged runs gridFill on every change */
    }
    function onHistory() { return $id('lcHistory').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'lcForm' : 'lcHistory'); }

    // ------------------------------------------------------------------------------ load
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = d.permissions;
            $id('btnSave').disabled = !PERM.Save;
            $id('btnUpdate').disabled = !PERM.Update;
            Object.keys(d).forEach(function (k) { if (/Error$/.test(k) && d[k]) box(d[k]); });
            if (d.branches && d.branches.length) { bind('cmbbranch', d.branches); setVal('cmbbranch', d.branches[0].Id); }
            if (d.projects && d.projects.length) { bind('cmbproject', d.projects); setVal('cmbproject', d.projects[0].Id); }
            if (d.customers && d.customers.length) bind('cmbcustomer', d.customers);
            if (d.currencies && d.currencies.length) bind('cmbfc', d.currencies);
            if (d.ports && d.ports.length) { bind('cmbloadingport', d.ports); bind('cmbdestinationport', d.ports); }
            if (d.items && d.items.length) bind('cmbitem', d.items);
            if (d.packTypes && d.packTypes.length) bind('cmbinnerpacktype', d.packTypes);
            if (d.banks) {
                if (d.banks.importer && d.banks.importer.length) bind('cmbimporterbank', d.banks.importer);
                if (d.banks.exporter && d.banks.exporter.length) bind('cmbExporterBanks', d.banks.exporter);
            }
            bind('cmblegalization', FIXED.two); bind('cmbinspection', FIXED.two);
            bind('cmbstatus', FIXED.status); setVal('cmbstatus', 1);
            if (d.deliveryTerms && d.deliveryTerms.length) bind('cmbdeliveryterm', d.deliveryTerms);
            if (d.paymentTerms && d.paymentTerms.length) bind('cmbpaymenterm', d.paymentTerms);
            bind('cmbPartialShipment', FIXED.ship); bind('cmbTransShipment', FIXED.ship);
            setDocNo(d.docNo);
            ['txtdocdate', 'txtexpirydate', 'txtlastshipmentdate', 'txtreferencedate'].forEach(function (id) { setText(id, today()); });
            render();
            focus('txtdocdate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function setDocNo(code) { if (code !== undefined && code !== null && (str(code) !== '' || netI(code) !== 0)) setText('txtdocno', String(netI(code))); }

    // ------------------------------------------------------------------------------ leave / calculations
    /** cmbcustomer_Leave -> PerformaInvoie: re-binds only when the proforma list itself has rows; first row activated. */
    function customerLeave() {
        return getJson(API + '/proforma-invoices?customerId=' + netI(val('cmbcustomer'))).then(function (rows) {
            setVal('cmbPerformaInvoice', '0');
            if (rows && rows.length) { bind('cmbPerformaInvoice', rows); setVal('cmbPerformaInvoice', rows[0].Id); }
        }).catch(function (e) { box(e.message); });
    }
    /** cmbitem_Leave: the three UOM combos (display Equivalent). */
    function itemLeave() {
        return getJson(API + '/uoms?itemId=' + netI(val('cmbitem'))).then(function (rows) {
            if (rows && rows.length) { bind('cmbouterpackuom', rows); bind('cmbinnerpackuom', rows); bind('cmbrateuom', rows); }
        }).catch(function (e) { box(e.message); });
    }
    function calculateLocalAmount() {
        var fcy = netD(val('txttotalfcamount')), rate = netD(val('txtexchangerate'));
        if (fcy > 0 && rate > 0) setText('txtlocalcurramount', String(fcy * rate));
    }
    function calculateWeight() {
        var uom = netD(comboText('cmbouterpackuom')), q = netD(val('txtOuterQty'));
        if (uom > 0 && q > 0) { setText('txtweight', String(uom * q)); calculateAmount(); }
    }
    function calculateAmount() {
        var w = netD(val('txtweight')), ru = netD(comboText('cmbrateuom')), r = netD(val('txtrate'));
        if (w > 0 && ru > 0 && r > 0) setText('txtamount', String(r / ru * w));
    }

    // ------------------------------------------------------------------------------ detail
    var COLS = ['Item', 'PackType', 'InnerQty', 'InnerPackUOM', 'OuterQty', 'OuterPackUOM', 'Weigth', 'Rate', 'RateUOM', 'Amount', 'Description'];
    var NUM = { InnerQty: 'sum', OuterQty: 'sum', Weigth: 'sum', Rate: 'avg', Amount: 'sum' };
    function render() {
        var sums = {};
        $id('detBody').innerHTML = S.rows.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === S.idx ? ' class="is-current"' : '') + '>' + COLS.map(function (c) {
                if (NUM[c]) { sums[c] = (sums[c] || 0) + netD(r[c]); return '<td class="num">' + esc(fmt00(r[c])) + '</td>'; }
                return '<td>' + esc(str(r[c])) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        var n = S.rows.length;
        $id('detFoot').innerHTML = n ? '<tr>' + COLS.map(function (c, k) {
            if (NUM[c]) return '<td>' + esc(fmtTot(NUM[c] === 'avg' ? (sums[c] || 0) / n : (sums[c] || 0))) + '</td>';
            return '<td class="lbl">' + (k === 0 ? '&Sigma;' : '') + '</td>';
        }).join('') + '</tr>' : '';
    }
    function sum(c) { var t = 0; S.rows.forEach(function (r) { t += netD(r[c]); }); return t; }
    function detailFormValidation() {
        if (comboText('cmbitem') === '' || netI(val('cmbitem')) === 0) { box('Item Field is Required!'); focus('cmbitem'); return false; }
        if (comboText('cmbinnerpacktype') === '' || netI(val('cmbinnerpacktype')) === 0) { box('Packing Type Field is Required!'); focus('cmbinnerpacktype'); return false; }
        if (val('txtOuterQty') === '' || toIntText(val('txtOuterQty')) === 0) { box('Outer Qty Field is Required!'); focus('txtOuterQty'); return false; }
        if (comboText('cmbouterpackuom') === '' || netI(val('cmbouterpackuom')) === 0) { box('Outer Pack UOM Field is Required!'); focus('cmbouterpackuom'); return false; }
        if (val('txtweight') === '') { box('Weight Field is Required!'); focus('txtweight'); return false; }
        if (val('txtrate') === '' || toIntText(val('txtrate')) === 0) { box('Rate Field is Required!'); focus('txtrate'); return false; }
        if (comboText('cmbrateuom') === '' || netI(val('cmbrateuom')) === 0) { box('Rate UOM Field is Required!'); focus('cmbrateuom'); return false; }
        if (val('txtamount') === '' || toIntText(val('txtamount')) === 0) { box('Amount Field is Required!'); focus('txtamount'); return false; }
        return true;
    }
    function rowFromForm() {
        return {
            ItemId: netI(val('cmbitem')), Item: comboText('cmbitem'), PackTypeId: netI(val('cmbinnerpacktype')), PackType: comboText('cmbinnerpacktype'),
            InnerQty: val('txtinnerQty') === '' ? null : netD(val('txtinnerQty')), InnerPackUOMId: netI(val('cmbinnerpackuom')), InnerPackUOM: comboText('cmbinnerpackuom'),
            OuterQty: netD(val('txtOuterQty')), OuterPackUOMId: netI(val('cmbouterpackuom')), OuterPackUOM: comboText('cmbouterpackuom'),
            Weigth: netD(val('txtweight')), Rate: netD(val('txtrate')), RateUOMId: netI(val('cmbrateuom')), RateUOM: comboText('cmbrateuom'),
            Amount: netD(val('txtamount')), Description: val('txtdescription')
        };
    }
    function afterDetailChange() {
        formResetDetail();
        setText('txtnetweight', numStr(sum('Weigth')));          /* AddNetWeightandGrossWeightInHeader */
        setText('txttotalfcamount', numStr(sum('Amount')));      /* GetTotalAmountTotalFromGrid */
        calculateLocalAmount();
        focus('cmbitem');
    }
    function btnaddnew() {
        if (!detailFormValidation()) return;
        S.rows.push(rowFromForm()); S.gridBound = true;
        render(); afterDetailChange();
    }
    function detDblClick(i) {
        var r = S.rows[i]; if (!r) return;
        S.idx = i;
        setComboText('cmbitem', r.Item);
        setComboText('cmbinnerpacktype', r.PackType);
        setText('txtinnerQty', str(r.InnerQty));
        setComboText('cmbinnerpackuom', r.InnerPackUOM);
        setText('txtOuterQty', str(r.OuterQty));
        setComboText('cmbouterpackuom', r.OuterPackUOM);
        setText('txtweight', str(r.Weigth));
        setText('txtrate', str(r.Rate));
        setComboText('cmbrateuom', r.RateUOM);
        calculateWeight(); calculateAmount();
        setText('txtdescription', r.Description);
        show('btnaddnew', false); show('btnGridUpdate', true); show('btnCancel', true);
        render();
    }
    function btnGridUpdate() {
        if (!detailFormValidation()) return;
        if (!S.rows[S.idx]) { box('There is no row at position ' + S.idx + '.'); return; }
        S.rows[S.idx] = rowFromForm();
        show('btnGridUpdate', false); show('btnCancel', false); show('btnaddnew', true);
        render(); afterDetailChange();
    }
    /** btnCancel_Click - the desktop hides Add as well (it stays hidden until a record is opened / the form reopens). */
    function btnCancel() {
        show('btnGridUpdate', false); show('btnCancel', false); show('btnaddnew', false);
        formResetDetail(); focus('cmbitem');
    }
    function formResetDetail() {
        ['cmbitem', 'cmbinnerpacktype', 'cmbinnerpackuom', 'cmbouterpackuom', 'cmbrateuom'].forEach(function (id) { setVal(id, '0'); });
        ['txtdescription', 'txtinnerQty', 'txtOuterQty', 'txtweight', 'txtrate', 'txtamount'].forEach(function (id) { setText(id, ''); });
        /* ItemDetailFill */
        return getJson(API + '/refresh').then(function (d) { if (d && d.items && d.items.length) bind('cmbitem', d.items); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ reset / save / read
    function formReset() {
        show('btnSave', true); show('btnUpdate', false);
        S.updateMode = false;
        ['txtamount', 'txtcommoditydesc', 'txtcustomercode', 'txtcustomerrefno', 'txtdescription', 'txtexchangerate', 'txtexpplace', 'txtgrossweight',
         'txtinquiryref', 'txtlegalizationdesc', 'txtlocalcurramount', 'txtnetweight', 'txtnoofcontainer', 'txtquorationref', 'txtrate', 'txtremarks',
         'txtshippingmark', 'txttotalfcamount', 'txtweight', 'txtinnerQty', 'txtinspectiondesc', 'txtOuterQty'].forEach(function (id) { setText(id, ''); });
        ['txtdocdate', 'txtexpirydate', 'txtlastshipmentdate', 'txtreferencedate'].forEach(function (id) { setText(id, today()); });
        ['cmbPerformaInvoice', 'cmbcustomer', 'cmbdeliveryterm', 'cmbdestinationport', 'cmbExporterBanks', 'cmbfc', 'cmbimporterbank', 'cmbinnerpacktype',
         'cmbinnerpackuom', 'cmbinspection', 'cmbitem', 'cmblegalization', 'cmbloadingport', 'cmbouterpackuom', 'cmbPartialShipment', 'cmbpaymenterm',
         'cmbstatus', 'cmbrateuom', 'cmbTransShipment'].forEach(function (id) { setVal(id, '0'); });
        focus('txtdocdate');
        S.rows = []; S.gridBound = false; S.idx = -1; render();
        return getJson(API + '/refresh').then(function (d) { setDocNo(d && d.docNo); }).catch(function (e) { box(e.message); });
    }
    function btnNew() { return formReset().then(formResetDetail); }
    function headerPayload() {
        var h = {};
        ['txtdocno', 'txtdocdate', 'txtexpirydate', 'txtexpplace', 'txtlastshipmentdate', 'txtquorationref', 'txtinquiryref', 'txtcustomerrefno', 'txtreferencedate',
         'txtcustomercode', 'txtinspectiondesc', 'txttotalfcamount', 'txtexchangerate', 'txtlocalcurramount', 'txtcommoditydesc', 'txtshippingmark',
         'txtgrossweight', 'txtnetweight', 'txtnoofcontainer', 'txtremarks', 'txtlegalizationdesc'].forEach(function (id) { h[id] = val(id); });
        ['cmbbranch', 'cmbproject', 'cmbcustomer', 'cmbdeliveryterm', 'cmbpaymenterm', 'cmbimporterbank', 'cmbExporterBanks', 'cmbloadingport',
         'cmbdestinationport', 'cmbstatus', 'cmbfc', 'cmbPerformaInvoice', 'cmbPartialShipment', 'cmbTransShipment', 'cmbinspection', 'cmblegalization'].forEach(function (id) {
            h[id] = netI(val(id)); h[id + 'Text'] = comboText(id);
        });
        return h;
    }
    var FOCUS = {
        'Branh Field is Required!': 'cmbbranch', 'Project Field is Required!': 'cmbproject', 'Customer Field is Required!': 'cmbcustomer',
        'Delivery Term Field is Required!': 'cmbdeliveryterm', 'Payment Term Field is Required!': 'cmbpaymenterm', 'Importer Bank Field is Required!': 'cmbimporterbank',
        'Exporter Bank Field is Required!': 'cmbExporterBanks', 'Loading Port Field is Required!': 'cmbloadingport', 'Destination Port Field is Required!': 'cmbdestinationport',
        'Status Field is Required!': 'cmbstatus', 'Doc No Field is Required!': 'txtdocno', 'Fcy Amount Field is Required!': 'txttotalfcamount',
        'Exchange Rate Field is Required!': 'txtexchangerate', 'Local Amount Field is Required!': 'txtlocalcurramount', 'Gross Weight Field is Required!': 'txtgrossweight',
        'Net Weight Field is Required!': 'txtnetweight'
    };
    /** btnSave_Click: the confirm first, then FormValidation (server) and ExImLcOrder.Save. */
    function btnSave(btn) {
        return busy(btn, function () {
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            return postJson(API + '/save', { recId: S.recId, updateMode: S.updateMode, header: headerPayload(), rows: S.rows, gridBound: S.gridBound }).then(function (d) {
                box(d.message);
                return formReset();
            }).catch(function (e) { box(e.message); if (FOCUS[e.message]) focus(FOCUS[e.message]); });
        });
    }
    function btnUpdate(btn) { return btnSave(btn); }
    function readById(id) {
        show('btnSave', false); show('btnUpdate', true);
        S.recId = id;
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            var h = d.header || {};
            $id('lcHistory').classList.remove('is-active'); $id('lcForm').classList.add('is-active');
            document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === 'lcForm'); });
            $id('btnFooterHistory').querySelector('span').textContent = 'History';
            setVal('cmbbranch', h.BranchesId); setVal('cmbPerformaInvoice', h.ExImProformaInvoice);
            setText('txtdocno', h.LcOrderNo); setText('txtdocdate', h.LcOrderDate || today()); setText('txtexpirydate', h.ExpiryDate || today());
            setText('txtexpplace', h.ExpiryPlace); setComboText('cmbPartialShipment', h.PartialShipment); setComboText('cmbTransShipment', h.TransShipment);
            setText('txtlastshipmentdate', h.LastShipmentDate || today()); setVal('cmbcustomer', h.SupCustId);
            setText('txtquorationref', h.QuotReference); setText('txtinquiryref', h.InquiryReference); setText('txtcustomerrefno', h.SalesContractRef);
            setText('txtreferencedate', h.SalesContratDate || today()); setText('txtcustomercode', h.CustomerCode);
            setComboText('cmbinspection', h.InsepctionRequired); setText('txtinspectiondesc', h.InsepctionDescription);
            setVal('cmbdeliveryterm', h.DeliveryTermId); setVal('cmbpaymenterm', h.PaymentTermId); setVal('cmbimporterbank', h.ImporterBankId);
            setVal('cmbExporterBanks', h.ExporterBankId); setVal('cmbloadingport', h.LoadingPortId); setVal('cmbdestinationport', h.DestinationPortId);
            setVal('cmbfc', h.FcurrencyId); setText('txttotalfcamount', h.FCurrencyAmount); setText('txtexchangerate', h.ConversionRate);
            calculateLocalAmount();
            setText('txtcommoditydesc', h.CommodityDetial); setText('txtshippingmark', h.ShippingMarks); setText('txtgrossweight', h.GrossWeightKgs);
            setText('txtnetweight', h.NetWeightKgs); setText('txtnoofcontainer', h.NoOfContainers); setText('txtremarks', h.RemarksHeader);
            setComboText('cmblegalization', h.LegalizationRequired); setText('txtlegalizationdesc', h.LegalizationDescription); setComboText('cmbstatus', h.Status);
            /* dtdetail is not cleared first: the rows are appended to whatever the grid holds (desktop behaviour) */
            (d.rows || []).forEach(function (r) { S.rows.push(r); });
            S.gridBound = true; render();
            S.updateMode = true;
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history
    var HCOLS = ['Customer', 'PerformaInvoice', 'Quotation Ref', 'InquiryRef', 'CustomerCode', 'ExpiryDate', 'LastShipmentDate', 'PartialShipment', 'LoadingPort',
        'DestinationPort', 'ImporterBank', 'ExporterBank', 'ExchangeRate', 'GrossWeight', 'NoOfContainer', 'NoOfAttachments'];
    function gridFill() {
        return getJson(API + '/history').then(function (rows) {
            HIST = rows || [];
            var gw = 0;
            $id('histBody').innerHTML = HIST.map(function (r, i) {
                return '<tr data-i="' + i + '">' + HCOLS.map(function (c) {
                    var v = r[c];
                    if (c === 'GrossWeight') { gw += netD(v); return '<td class="num">' + esc(fmt00(v)) + '</td>'; }
                    if (c === 'Customer') return '<td><a class="win-code" href="javascript:void(0)" data-i="' + i + '">' + esc(str(v)) + '</a></td>';
                    return '<td>' + esc(str(v)) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            $id('histFoot').innerHTML = HIST.length ? '<tr>' + HCOLS.map(function (c, k) { return c === 'GrossWeight' ? '<td>' + esc(fmtTot(gw)) + '</td>' : '<td class="lbl">' + (k === 0 ? '&Sigma;' : '') + '</td>'; }).join('') + '</tr>' : '';
            show('histEmpty', HIST.length === 0);
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ wiring
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) { b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); }); });
        on('cmbcustomer', 'change', customerLeave);
        on('cmbitem', 'change', itemLeave);
        on('txttotalfcamount', 'input', calculateLocalAmount);
        on('txtexchangerate', 'input', calculateLocalAmount);
        on('txtOuterQty', 'input', calculateWeight);
        on('cmbouterpackuom', 'change', calculateWeight);
        on('txtrate', 'input', calculateAmount);
        on('cmbrateuom', 'change', calculateAmount);
        var db = $id('detBody');
        db.addEventListener('click', function (e) { var tr = e.target.closest('tr[data-i]'); if (!tr) return; db.querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); });
        db.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) detDblClick(+tr.getAttribute('data-i')); });
        var hb = $id('histBody');
        hb.addEventListener('click', function (e) { var a = e.target.closest('a.win-code'); if (a) { var r = HIST[+a.getAttribute('data-i')]; if (r) readById(r.Id); } });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) { var r = HIST[+tr.getAttribute('data-i')]; if (r) readById(r.Id); } });
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if (e.ctrlKey && k === 's' && !onHistory() && !S.updateMode) { e.preventDefault(); btnSave($id('btnSave')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
            if (e.ctrlKey && k === 'u' && S.updateMode) { e.preventDefault(); btnUpdate($id('btnUpdate')); }
        });
        load();
    });

    global.ExportLc = {
        btnNew: btnNew, btnSave: btnSave, btnUpdate: btnUpdate, btnaddnew: btnaddnew, btnGridUpdate: btnGridUpdate, btnCancel: btnCancel, toggleHistory: toggleHistory,
        print: function () { /* "Print" has no Click handler on the desktop */ },
        attachment: function () { box('Attachments are not available on the web page for this form.'); }
    };
}(window));
