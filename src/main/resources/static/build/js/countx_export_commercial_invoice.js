/* ============================================================================================
 * countx_export_commercial_invoice.js - ExImCommercialInvoice.cs (Architecture.WinApp.Export), screen 211
 * "Export Commercial Invoice" (DocumentTypeId 204). Form | History. Every toolstrip button, TextChanged, Leave,
 * grid double-click and grid column button of the desktop form has its counterpart here, with the desktop's
 * messages and order; all data comes from /api/export/commercial-invoice (ExportCommercialInvoiceController ->
 * ExportCommercialInvoiceService -> the desktop's own procedures).
 *
 * Button contract: disabled + spinner while a request runs, duplicates ignored, re-enabled on success and failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/api/export/commercial-invoice';

    // ------------------------------------------------------------------------------ helpers
    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;'); }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    /* .NET custom numeric formats used by the form: "#,##0.###" (min 0 int digits shown as 0), "#,#.###" (0 -> ""), "#,#0.###". */
    function fmtN(v, dec, zeroBlank) {
        var n = netD(v);
        var s = n.toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        if (zeroBlank && Number(s) === 0) return '';
        var neg = s.charAt(0) === '-'; if (neg) s = s.substring(1);
        var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (neg ? '-' : '') + p.join('.');
    }
    function f3(v) { return fmtN(v, 3, false); }          /* "#,##0.###" / "#,#0.###" */
    function f3b(v) { return fmtN(v, 3, true); }          /* "#,#.###" */
    function f4(v) { return fmtN(v, 4, false); }          /* "#,##0.####" */
    function f5(v) { return fmtN(v, 5, false); }          /* "#,##0.#####" */
    function f2(v) { var n = netD(v); var p = n.toFixed(2).split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }   /* stringFormatsingleForFcy */
    function plain(v) { var n = netD(v); return String(Math.round(n * 1e10) / 1e10); }       /* double.ToString() */
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function ymd(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return ymd(new Date()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return ymd(d); }
    function isoDate(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(v)); return m ? m[1] + '-' + m[2] + '-' + m[3] : ''; }
    function shortDate(v) { var s = isoDate(v); if (!s) return ''; return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4); }
    function ddMMMyyyy(v) { var s = isoDate(v); if (!s) return ''; var mn = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec']; return s.substring(8, 10) + '-' + mn[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(0, 4); }
    function dtFmt(v) { var s = str(v); var m = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2})/.exec(s); if (!m) return s; var h = parseInt(m[4], 10), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12; return m[3] + '-' + m[2] + '-' + m[1] + ' ' + pad(h) + ':' + m[5] + ' ' + ap; }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve(); b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); applyRights(); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done(); return Promise.resolve(p);
    }
    function parse(r) { return r.text().then(function (t) { var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ } if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')')); return b; }); }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function shown(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden'); }
    function col(row, name) { if (!row) return null; if (Object.prototype.hasOwnProperty.call(row, name)) return row[name]; var l = name.toLowerCase(); for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k]; return null; }
    function bind(id, rows, valueCol, textCol, extraCols, keepValue) {
        var s = $id(id); if (!s) return;
        var keep = keepValue === undefined ? s.value : str(keepValue);
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            var extra = (extraCols || []).map(function (k) { return str(col(r, k)).replace(/\|/g, '/'); }).join('|');
            h += '<option value="' + esc(col(r, valueCol)) + '" data-extra="' + esc(extra) + '">' + esc(col(r, textCol)) + '</option>';
        });
        s.innerHTML = h; s.value = keep; if (s.value !== keep) s.value = '0';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = '0'; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function selText(id) { var s = $id(id); return s && s.selectedIndex > 0 ? s.options[s.selectedIndex].textContent.trim() : ''; }
    function sel(id) { return netI(val(id)); }
    function findRow(rows, id, key) { key = key || 'Id'; for (var i = 0; i < (rows || []).length; i++) if (netI(col(rows[i], key)) === netI(id)) return rows[i]; return null; }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function sum(rows, k) { var s = 0; (rows || []).forEach(function (r) { s += netD(r[k]); }); return s; }
    function copy(o) { var c = {}; for (var k in o) if (Object.prototype.hasOwnProperty.call(o, k)) c[k] = o[k]; return c; }
    function notPorted(name) { box(name + ' is not part of the web port of this screen.'); }

    // ------------------------------------------------------------------------------ state
    var PERM = { Save: true, Update: true, Print: true };
    var CFG = { defaultDaysToLessFromHistoryFromDate: 0, flagFin: false, autoGenInvoiceNo: false, rateAddLessOnCommercialInvoice: false };
    var F9 = false, F15 = false;
    var L = {};                     /* combo sources */
    var PROJECT_ID = 0;             /* cmbproject: Rows[0].Activate() */
    var REC = { id: 0, updateMode: false, invoiceUpdateFlag: 0, attachments: '', customAttachments: '', forwardingId: 0, exportVoucherId: 0 };
    var DETAIL = [], REMOVED = [], OI = [], PT = [], CM = [], OC = [];
    var IDX = { detail: -1, oi: -1, pt: -1, cm: -1, oc: -1 };
    var CUR = { detail: -1, hist: -1 };
    var HIST = [], HIST_DETAIL = [];
    var COMM_TAG = '';               /* txtCommRateUom.Tag */
    var LDR = { rows: [], header: [], setup: null };

    // ------------------------------------------------------------------------------ tabs
    function tab(group, panelId) {
        var tabs = document.querySelector('.win-tabs[data-tabs="' + group + '"]'); if (!tabs) return;
        tabs.querySelectorAll('.win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        Array.prototype.forEach.call(tabs.parentNode.children, function (p) { if (p.classList.contains('win-tab-panel')) p.classList.toggle('is-active', p.id === panelId); });
        if (group === 'main') { if (panelId === 'tabPage2') focus('FromDateHistory'); else focus('txtDocDate'); }   /* tabControl1_SelectedIndexChanged */
    }
    function activeTab(group) { var b = document.querySelector('.win-tabs[data-tabs="' + group + '"] .win-tab.is-active'); return b ? b.getAttribute('data-tab') : ''; }
    function toggleHistory() { tab('main', activeTab('main') === 'tabPage2' ? 'tabPage1' : 'tabPage2'); }

    function applyRights() {
        var s = $id('btnsave'), u = $id('btnupdate'), p = $id('btnPrint');
        if (s && !s.classList.contains('is-busy')) s.disabled = !PERM.Save;
        if (u && !u.classList.contains('is-busy')) u.disabled = !PERM.Update;
        if (p && !p.classList.contains('is-busy')) p.disabled = !PERM.Print;
    }

    // ------------------------------------------------------------------------------ load
    function bindAll(d) {
        if (d.creditAccounts) { L.creditAccounts = d.creditAccounts; bind('CmbCreditAccount', L.creditAccounts, 'Id', 'Name'); }
        if (d.customers) { L.customers = d.customers; bind('cmbSupCust', L.customers, 'Id', 'Name'); }
        if (d.notifyParties) { L.notifyParties = d.notifyParties; bind('cmbNotifyParty1', L.notifyParties, 'Id', 'Name'); bind('cmbNotifyParty2', L.notifyParties, 'Id', 'Name'); }
        if (d.deliveryTerms) { L.deliveryTerms = d.deliveryTerms; bind('cmbdeliverytermnew', L.deliveryTerms, 'Id', 'Code', ['Description']); }
        if (d.paymentTerms) { L.paymentTerms = d.paymentTerms; bind('dcmbpaymentterm', L.paymentTerms, 'Id', 'lcOrderTerm', ['Description']); }
        if (d.ports) {
            /* LoadingPortFill: PortType "Loading" / "Destination" */
            L.loadingPorts = d.ports.filter(function (r) { return str(r.PortType) === 'Loading'; });
            L.destinationPorts = d.ports.filter(function (r) { return str(r.PortType) === 'Destination'; });
            bind('cmbLoadingPort', L.loadingPorts, 'Id', 'PortName'); bind('cmbDestinationPort', L.destinationPorts, 'Id', 'PortName');
        }
        if (d.banks) {
            /* ImporterandExportBankFill: Home Country -> exporter, Foreign Country -> importer; exporter Rows[1] activated */
            var home = d.banks.filter(function (r) { return str(r.IsHomeland) === 'Home Country'; });
            var foreign = d.banks.filter(function (r) { return str(r.IsHomeland) === 'Foreign Country'; });
            var ex = sel('cmbexporterBankNew');
            bind('cmbexporterBankNew', home, 'Id', 'BranchName'); bind('cmbimporterBankNew', foreign, 'Id', 'BranchName');
            if (home.length > 0) setVal('cmbexporterBankNew', home[0].Id);
            if (ex > 0) { if (findRow(home, ex)) setVal('cmbexporterBankNew', ex); }
        }
        /* CarierType: fixed DataTable By Sea / By Air / By Road, Rows[1] (By Sea) activated */
        L.carrier = [{ Id: 1, Name: 'By Sea' }, { Id: 2, Name: 'By Air' }, { Id: 3, Name: 'By Road' }];
        bind('cmbcareiertype', L.carrier, 'Id', 'Name', [], 1);
        if (d.otherItems) { L.otherItems = d.otherItems; bind('CmbOtherItem', L.otherItems, 'Id', 'Name'); }
        if (d.currencies) { L.currencies = d.currencies; bind('cmbfcycode', L.currencies, 'Id', 'Name'); }
        if (d.cropYears) { L.cropYears = d.cropYears; bind('CmbCropYear', L.cropYears, 'Id', 'Name'); }
        if (d.jobLots) { L.jobLots = d.jobLots; bind('CmbJobLot', L.jobLots, 'Id', 'Name'); }
        if (d.packTypes) { L.packTypes = d.packTypes; bind('combpcktype', L.packTypes, 'Id', 'Name'); }
        if (d.farmingNTrade) { L.farming = d.farmingNTrade; bind('CmbFarmingNTrade', L.farming, 'Id', 'FarmingNTrade'); }
        if (d.exportCharges) { L.exportCharges = d.exportCharges; bind('CmbChargesNameChargeDetail', L.exportCharges, 'Id', 'Name'); }
        if (d.exportCompanies) { L.exportCompanies = d.exportCompanies; bind('CmbExportCompany', L.exportCompanies, 'Id', 'Name'); }
        if (d.salesPersons) { L.salesPersons = d.salesPersons; bind('CmbSalePerson', L.salesPersons, 'Id', 'Name'); }
        Object.keys(d).forEach(function (k) { if (/Error$/.test(k) && d[k]) box(d[k]); });
    }
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            if (d.config) CFG = d.config;
            F9 = !!d.allowExportMultiCompanies; F15 = !!d.saleCommissionTabFeatureWise;
            applyRights();
            show('exportCompanyBox', F9);
            if (!F15) { show('tabPage6Btn', false); }                   /* tabControl2.TabPages.Remove(tabPage6) */
            PROJECT_ID = d.projects && d.projects.length ? netI(d.projects[0].Id) : 0;
            if (netI(d.docNo) > 0) setText('txtdocno', d.docNo);
            invoiceNoState(CFG.autoGenInvoiceNo, d.invoiceNo);
            bindAll(d);
            if (d.historyCombos) bindHistoryCombos(d.historyCombos);
            var days = netI(CFG.defaultDaysToLessFromHistoryFromDate);
            setText('FromDateHistory', days > 0 ? daysAgo(days) : daysAgo(3));
            setText('ToDateHistory', today());
            /* commission type fixed table, Rows[2] ("Commission % of Value") activated */
            L.commTypes = [{ Id: 1, Name: 'Fixed Commission Amount' }, { Id: 2, Name: 'Commission % of Value' }, { Id: 3, Name: 'Commission By Weight Kg' }];
            bind('cmbCommissionType', L.commTypes, 'Id', 'Name', [], F15 ? 2 : 0);
            commissionTypeChanged();
            setText('txtDocDate', today()); setText('txtIformDate', today());
            renderAll();
            focus('txtDocDate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /* AutoGenInvoiceNo: cmbInvoiceNo disabled + GenerateInvoiceNo, else enabled. */
    function invoiceNoState(auto, no) {
        setEnabled('cmbInvoiceNo', !auto);
        if (auto && str(no) !== '') setText('cmbInvoiceNo', no);
    }

    // ============================================================================== calculations
    function detailTotals() { return { mton: sum(DETAIL, 'QtyMTon'), amount: sum(DETAIL, 'Amount') }; }
    /* getTotalnetWeightFromGrid */
    function totalsFromGrid() {
        var t = detailTotals();
        setText('txtNetWeight', f5(t.mton));
        setText('txtfcyAmount', f3(t.amount));
        fcyChanged();
        paymentBalance();
    }
    /* txtfcyAmount_TextChanged -> txtaddlessamount_TextChanged */
    function fcyChanged() { addLessChanged(); }
    function addLessChanged() { totalAmountCalculate(); proportionateAddLess(); }
    /* TotalAmountCalculate */
    function totalAmountCalculate() {
        var other = sum(OI, 'Amount'), addless = netD(val('txtaddlessamount')), fcy = netD(val('txtfcyAmount'));
        if (fcy > 0) setText('txtTotalNetAmount', f3b(fcy + addless + other));
        var total = netD(val('txtTotalNetAmount'));
        if (total > 0 && PT.length > 0) PT.forEach(function (r) { r.FcyAmount = netD(r.PrcntOfTotal) * total / 100.0; });
        calculateLocal();
        renderPt();
    }
    /* CalculateLocalAmount */
    function calculateLocal() {
        var fcy = netD(val('txtTotalNetAmount')), rate = netD(val('txtExRate'));
        setText('txtLocalAmt', fcy > 0 && rate > 0 ? f3(fcy * rate) : '0');
    }
    /* txtExRate_TextChanged */
    function exRateChanged() {
        setText('txtCommissionExchangeRate', val('txtExRate'));
        calculateLocal();
        var rate = netD(val('txtExRate'));
        CM.forEach(function (r) { var f = netD(r.FcyAmount); if (f > 0 && rate > 0) { r.ExchangeRate = rate; r.LcyAmount = f * rate; } else { r.ExchangeRate = 0; r.LcyAmount = 0; } });
        renderCm();
    }
    /* ProportionateAddLessAmount */
    function proportionateAddLess() {
        var net = sum(DETAIL, 'QtyMTon'), addless = netD(val('txtaddlessamount'));
        DETAIL.forEach(function (r) { r.AddLessAmount = addless !== 0 ? addless / net * netD(r.QtyMTon) : 0; });
        renderDetail();
    }
    /* ProportionateOtherItemAmount */
    function proportionateOther() {
        var contract = sum(DETAIL, 'Amount'), other = sum(OI, 'Amount');
        DETAIL.forEach(function (r) { r.OtherItemAmount = other > 0 ? other / contract * netD(r.Amount) : 0; });
        renderDetail();
    }
    /* PaymentGridBalanceAmountFromInvoiceAmount */
    function paymentBalance() { setText('txtBalanceInvoiceAmtPaymentGrid', f2(netD(val('txtTotalNetAmount')) - sum(PT, 'FcyAmount'))); }
    /* UpdateAddLessAmountInHeaderFromChargesGrid */
    function addLessFromCharges() { setText('txtaddlessamount', f3b(sum(OC, 'AddAmount') - sum(OC, 'LessAmount'))); addLessChanged(); }

    // ============================================================================== detail entry
    function uomEq(id) { var s = $id(id); if (!s || s.selectedIndex <= 0) return null; var x = str(s.options[s.selectedIndex].getAttribute('data-extra')).split('|'); return netD(x[0]); }
    /* CalculateWeight */
    function calculateWeight() {
        setText('txtNoOfBags', '0');
        var eq = uomEq('CmbPackUom');
        if (eq !== null) { var mton = netD(val('txtQtyMTon')); if (mton > 0 && eq > 0) setText('txtNoOfBags', String(Math.round(mton * 1000.0 / eq))); }
    }
    /* CalculatAmount */
    function calculateAmount() {
        setText('txtAmount', '0');
        var eq = uomEq('CmbRateUom');
        if (eq !== null) { var rate = netD(val('txtCostMTon')), qty = netD(val('txtQtyMTon')); if (rate > 0 && eq > 0 && qty > 0) setText('txtAmount', f3(rate / eq * (qty * 1000.0))); }
    }
    function weightAndAmount() { calculateWeight(); calculateAmount(); }

    /* bindRateUomAndItemPackUom + HSCodeBind (combitem_Leave) */
    function itemLeave(keepPack, keepRate) {
        var item = sel('combitem');
        return getJson(API + '/item-leave?itemId=' + item + '&customerId=' + sel('cmbSupCust')).then(function (d) {
            L.uoms = (d && d.uoms) || [];
            var pk = keepPack !== undefined ? keepPack : selText('CmbPackUom'), rk = keepRate !== undefined ? keepRate : selText('CmbRateUom');
            bind('CmbPackUom', L.uoms, 'Id', 'UOMCode', ['Equivalent'], 0); bind('CmbRateUom', L.uoms, 'Id', 'UOMCode', ['Equivalent'], 0);
            var p = L.uoms.filter(function (r) { return str(r.UOMCode) === pk; })[0]; if (p) setVal('CmbPackUom', p.Id);
            var q = L.uoms.filter(function (r) { return str(r.UOMCode) === rk; })[0]; if (q) setVal('CmbRateUom', q.Id);
            bindHs((d && d.hsCodes) || []);
        });
    }
    /* HSCodeBind: only when rows come back; Rows[0] activated */
    function bindHs(rows) {
        if (!rows.length) return;
        var keep = val('CmbHSCode');
        L.hs = rows;
        bind('CmbHSCode', rows, 'Id', 'HSCode', [], keep);
        if (sel('CmbHSCode') === 0) setVal('CmbHSCode', rows[0].Id);
    }
    function setHsText(t) {
        if (!t) { setVal('CmbHSCode', '0'); return; }
        var r = (L.hs || []).filter(function (x) { return str(x.HSCode) === t; })[0];
        if (!r) { L.hs = (L.hs || []).concat([{ Id: -1 - (L.hs || []).length, HSCode: t }]); bind('CmbHSCode', L.hs, 'Id', 'HSCode', []); r = L.hs[L.hs.length - 1]; }
        setVal('CmbHSCode', r.Id);
    }

    /* DetailFormValidation */
    function detailFormValidation() {
        if (sel('combitem') === 0) { box('Item field required'); focus('combitem'); return false; }
        if (sel('combpcktype') === 0) { box('Pack Type field required'); focus('combpcktype'); return false; }
        if (sel('CmbCropYear') === 0) { box('Crop Year field required'); focus('CmbCropYear'); return false; }
        if (sel('CmbJobLot') === 0) { box('JobLot field required'); focus('CmbJobLot'); return false; }
        if (sel('CmbPackUom') === 0) { box('Pack uom field required'); focus('CmbPackUom'); return false; }
        if (val('txtQtyMTon').trim() === '' || !(netD(val('txtQtyMTon')) > 0)) { box('Qty/M.Ton field required'); focus('txtQtyMTon'); return false; }
        if (val('txtNoOfBags').trim() === '' || !(netD(val('txtNoOfBags')) > 0)) { box('No of Bags field required'); focus('txtNoOfBags'); return false; }
        if (val('txtCostMTon').trim() === '' || !(netD(val('txtCostMTon')) > 0)) { box('Cost/M.Ton field required'); focus('txtCostMTon'); return false; }
        if (sel('CmbRateUom') === 0) { box('Rate UOM field required'); focus('CmbRateUom'); return false; }
        if (val('txtAmount').trim() === '' || !(netD(val('txtAmount')) > 0)) { box('Amount field required'); focus('txtAmount'); return false; }
        if (sel('CmbContractSchedule') <= 0) { box('Contract Schedule field required'); focus('CmbContractSchedule'); return false; }
        if (val('txtTotalNetAmount').trim() === '' || !(netD(val('txtTotalNetAmount')) > 0)) { box('Total Net Amount  field required'); focus('txtTotalNetAmount'); return false; }
        return true;
    }
    /* btnAddinGrid_Click */
    function detailAdd() { box('Sorry Cannot Add New Record Only Update Detail Record : Thank You'); }
    /* grdDetail_DoubleClick */
    function detailEdit(i) {
        var item = DETAIL[i]; if (!item) return;
        IDX.detail = i;
        setVal('combitem', item.ItemId);
        itemLeave(str(item.PackSize), str(item.RateUOM)).catch(function (e) { box(e.message); }).then(function () {
            setVal('combpcktype', item.PackTypeId);
            setVal('CmbPackUom', item.PackSizeId);
            setVal('CmbCropYear', item.CropYearId);
            setVal('CmbJobLot', item.JobLotId);
            setText('txtQtyMTon', str(item.QtyMTon));
            setVal('CmbRateUom', item.RateUOMId);
            setText('txtNoOfBags', str(item.NoOfBags));
            setText('txtCostMTon', str(item.CostMTon));
            setText('txtAmount', str(item.Amount));
            setText('txtitemdetail', str(item.ItemDetail));
            setHsText(str(item.HSCode));
            setVal('CmbFarmingNTrade', item.ExImFarmingNTradeId);
            setText('txtpackingdetail', str(item.PackingDetail));
            if (netI(item.ContractScheduleDetailId) === 0) {
                setEnabled('CmbContractSchedule', true);
                return getJson(API + '/contract-schedules?contractId=' + netI(item.ContractId) + '&recId=' + REC.id).then(function (rows) {
                    L.schedules = rows || [];
                    bind('CmbContractSchedule', L.schedules, 'Id', 'AttentiveLoadingDate', ['Cell2', 'Cell3'], '0');
                    L.schedules.forEach(function (r) { /* the combo shows the date as text */ });
                });
            }
            setEnabled('CmbContractSchedule', false);
            if (netI(item.ContractScheduleId) > 0) {
                L.schedules = [{ Id: netI(item.ContractScheduleId), AttentiveLoadingDate: str(item.ContractNoInvoiceWise), Cell2: 0, Cell3: str(item.ContractNoInvoiceWise) }];
                bind('CmbContractSchedule', L.schedules, 'Id', 'AttentiveLoadingDate', ['Cell2', 'Cell3'], item.ContractScheduleId);
            }
        }).then(function () {
            show('btnAddinGrid', false); show('btnGridUpdate', true); show('btnCancel', true);
            /* txtCostMTon read-only unless InvoiceUpdateFlag != 0 and FlagFin */
            $id('txtCostMTon').readOnly = (netI(REC.invoiceUpdateFlag) === 0 || !CFG.flagFin);
            focus('combpcktype');
        }).catch(function (e) { box(e.message); });
    }
    /* btnGridUpdate_Click */
    function detailUpdate() {
        try {
            if (!detailFormValidation()) return;
            var r = DETAIL[IDX.detail]; if (!r) return;
            var f = findRow(L.farming, sel('CmbFarmingNTrade'));
            if (netI(r.ContractScheduleDetailId) === 0) {
                var s = findRow(L.schedules, sel('CmbContractSchedule'));
                r.ContractScheduleId = sel('CmbContractSchedule');
                r.ContractSchedule = shortDate(selText('CmbContractSchedule'));
                r.ContractNoInvoiceWise = s ? str(s.Cell3) : '';
                r.ScheduleFCL = s ? netD(s.Cell2) : 0;
            }
            r.ItemId = sel('combitem'); r.ItemName = selText('combitem');
            r.PackTypeId = sel('combpcktype'); r.PackType = selText('combpcktype');
            r.QtyMTon = netD(val('txtQtyMTon'));
            r.PackSizeId = sel('CmbPackUom'); r.PackSize = selText('CmbPackUom');
            r.CropYearId = sel('CmbCropYear'); r.CropYear = selText('CmbCropYear');
            r.JobLotId = sel('CmbJobLot'); r.JobLot = selText('CmbJobLot');
            r.NoOfBags = netD(val('txtNoOfBags'));
            r.CostMTon = netD(val('txtCostMTon'));
            r.RateUOMId = sel('CmbRateUom'); r.RateUOM = selText('CmbRateUom');
            r.RateEquivalent = uomEq('CmbRateUom') || 0;
            r.Amount = netD(val('txtAmount'));
            r.HSCode = selText('CmbHSCode') === '..... Select Any Value .....' ? '' : selText('CmbHSCode');
            r.ItemDetail = val('txtitemdetail'); r.PackingDetail = val('txtpackingdetail');
            r.ExImFarmingNTradeId = f ? netI(f.Id) : 0; r.FarmingNTrade = f ? str(f.FarmingNTrade) : '';
            r.ExImFarmingTypeId = f ? netI(f.ExImFarmingTypeId) : 0; r.ExImTradeTypeId = f ? netI(f.ExImTradeTypeId) : 0;
            totalsFromGrid();
            proportionateOther();
            detailReset();
            calculateLocal();
            focus('combpcktype');
        } catch (e) { box(e.message); }
    }
    function detailCancel() { detailReset(); }
    /* DetailFormReset */
    function detailReset() {
        IDX.detail = -1;
        setVal('combitem', '0'); setVal('CmbFarmingNTrade', '0'); setVal('combpcktype', '0');
        setText('txtQtyMTon', ''); setVal('CmbPackUom', '0'); setText('txtNoOfBags', ''); setText('txtCostMTon', '');
        setVal('CmbJobLot', '0'); setVal('CmbRateUom', '0'); setText('txtAmount', ''); setText('txtitemdetail', ''); setText('txtpackingdetail', '');
        setVal('CmbHSCode', '0');
        show('btnAddinGrid', true); show('btnGridUpdate', false); show('btnCancel', false);
        tab('detail', 'tabPage3');
        L.schedules = []; bind('CmbContractSchedule', [], 'Id', 'AttentiveLoadingDate');
    }
    /* grdDetail_ColumnButtonClick */
    function detailDelete(i) {
        var r = DETAIL[i]; if (!r) return;
        try {
            if (IDX.detail !== -1) throw new Error('Reset the Detail First');
            if (DETAIL.length > 1) {
                if (netI(r.Id) > 0) { if (!ask('Are you sure to Delete?')) return; REMOVED.push(copy(r)); }
                DETAIL.splice(i, 1);
            }
            totalsFromGrid(); proportionateOther();
        } catch (e) { box(e.message); }
    }
    function detailDuplicate(i) {
        var r = DETAIL[i]; if (!r) return;
        var c = copy(r); c.Id = 0; DETAIL.push(c);
        totalsFromGrid(); proportionateOther();
    }
    /* grdDetail_CellUpdated (RateAddLess editable when RateAddLessOnCommercialInvoice) */
    function rateAddLessEdited(i, v) {
        var r = DETAIL[i]; if (!r) return;
        r.RateAddLess = netD(v);
        var rate = netD(r.RateWithoutAddLess) + netD(r.RateAddLess), eq = netD(r.RateEquivalent), qty = netD(r.QtyMTon);
        r.CostMTon = rate;
        if (rate > 0 && eq > 0 && qty > 0) r.Amount = rate / eq * (qty * 1000.0);
        totalsFromGrid(); proportionateOther(); calculateLocal();
    }

    // ============================================================================== grids
    var DET_COLS = [
        ['ContractNoInvoiceWise', 'ContractNoInvoiceWise'], ['ItemName', 'ItemName'], ['PackType', 'PackType'], ['CropYear', 'CropYear'], ['JobLot', 'JobLot'],
        ['QtyMTon', 'Qty/M.Ton', 'n5s'], ['PackSize', 'PackSize'], ['NoOfBags', 'NoOfBags', 'n3s'], ['RateWithoutAddLess', 'RateWithoutAddLess', 'n4', 'ral'],
        ['RateAddLess', 'RateAddLess', 'n4', 'ral'], ['CostMTon', 'Cost/M.Ton', 'n4'], ['ContractRate', 'ContractRate', 'n4', 'fin'], ['RateUOM', 'RateUOM'],
        ['Amount', 'Amount', 'n3s'], ['HSCode', 'HSCode'], ['ItemDetail', 'ItemDetail'], ['PackingDetail', 'PackingDetail'], ['OtherItemAmount', 'OtherItemAmount', 'n3s'],
        ['AddLessAmount', 'AddLessAmount', 'n4s'], ['FarmingNTrade', 'FarmingNTrade']];
    function visibleDetCols() { return DET_COLS.filter(function (c) { if (c[3] === 'ral') return CFG.rateAddLessOnCommercialInvoice; if (c[3] === 'fin') return CFG.flagFin; return true; }); }
    function cell(fmt, v) {
        if (!fmt) return { t: str(v) };
        var n = netD(v), dec = netI(fmt.charAt(1));
        return { t: fmtN(n, dec, false), num: true, sum: fmt.length > 2 };
    }
    function drawTable(headId, bodyId, footId, rows, cols, opts) {
        opts = opts || {};
        if (headId) {
            var hh = '<tr>' + (opts.lead ? opts.lead.map(function (x) { return '<th class="ctr">' + esc(x) + '</th>'; }).join('') : '');
            cols.forEach(function (c) { hh += '<th' + (c[2] ? ' class="num"' : '') + '>' + esc(c[1]) + '</th>'; });
            $id(headId).innerHTML = hh + '</tr>';
        }
        var sums = {};
        $id(bodyId).innerHTML = rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (opts.cur === i ? ' class="is-current"' : '') + '>' + (opts.leadCells ? opts.leadCells(r, i) : '');
            cols.forEach(function (c) {
                var f = cell(c[2], r[c[0]]);
                if (f.sum) sums[c[0]] = (sums[c[0]] || 0) + netD(r[c[0]]);
                var inner = esc(f.t);
                if (opts.edit && opts.edit[c[0]]) inner = '<input type="text" class="num" data-edit-col="' + c[0] + '" data-i="' + i + '" value="' + esc(str(r[c[0]])) + '"/>';
                else if (opts.link && opts.link === c[0] && f.t !== '') inner = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + inner + '</a>';
                if (opts.dateCols && opts.dateCols[c[0]]) inner = esc(opts.dateCols[c[0]](r[c[0]]));
                h += '<td class="' + (f.num ? 'num' : '') + (opts.edit && opts.edit[c[0]] ? ' win-editable' : '') + '">' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        if (footId) {
            if (!rows.length) { $id(footId).innerHTML = ''; return; }
            var t = '<tr>' + (opts.lead ? opts.lead.map(function (x, k) { return '<td class="lbl">' + (k === 0 ? '&Sigma;' : '') + '</td>'; }).join('') : '');
            cols.forEach(function (c, idx) {
                var f = cell(c[2], 0), v = f.sum ? cell(c[2], sums[c[0]] || 0).t : '';
                t += '<td' + (f.num ? '' : ' class="lbl"') + '>' + (idx === 0 && !opts.lead ? '&Sigma;' : esc(v)) + '</td>';
            });
            $id(footId).innerHTML = t + '</tr>';
        }
    }
    function xBtn(kind) { return function (r, i) { return '<td class="win-cell-btn"><button type="button" class="win-x" data-' + kind + '="' + i + '" title="Delete">X</button></td>'; }; }
    function renderDetail() {
        drawTable('detailHead', 'detailBody', 'detailFoot', DETAIL, visibleDetCols(), {
            lead: ['X', '+'], cur: CUR.detail, link: 'ContractNoInvoiceWise',
            leadCells: function (r, i) { return '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '">X</button></td><td class="win-cell-btn"><button type="button" class="win-plus" data-dup="' + i + '">+</button></td>'; },
            edit: CFG.rateAddLessOnCommercialInvoice ? { RateAddLess: true } : null
        });
    }
    var OI_COLS = [['ItemName', 'ItemName'], ['Qty', 'Qty', 'n2s'], ['Rate', 'Rate', 'n4'], ['Amount', 'Amount', 'n2s'], ['Remarks', 'Remarks']];
    function renderOi() { drawTable(null, 'oiBody', 'oiFoot', OI, OI_COLS, { link: 'ItemName' }); }
    var PT_COLS = [['PaymentTerm', 'PaymentTerm'], ['FinancialInstrumentNo', 'FinancialInstrumentNo'], ['PrcntOfTotal', '%OfTotal', 'n2s'], ['FcyAmount', 'FcyAmount', 'n2s'], ['DueDays', 'DueDays'], ['Remarks', 'Remarks']];
    function renderPt() { drawTable(null, 'ptBody', 'ptFoot', PT, PT_COLS, { lead: ['X'], leadCells: xBtn('ptdel'), link: 'PaymentTerm' }); paymentBalance(); }
    var CM_COLS = [['SalesPerson', 'SalesPerson'], ['CommissionType', 'CommissionType'], ['Rate', 'Rate', 'n4'], ['RateUom', 'RateUom'], ['FcyAmount', 'FcyAmount', 'n3s'], ['ExchangeRate', 'ExchangeRate', 'n4'], ['LcyAmount', 'LcyAmount', 'n3s'], ['Remarks', 'Remarks']];
    function renderCm() { drawTable(null, 'cmBody', 'cmFoot', CM, CM_COLS, { lead: ['X'], leadCells: xBtn('cmdel'), link: 'SalesPerson' }); }
    var OC_COLS = [['ChargesItem', 'ChargesItem'], ['AddAmount', 'AddAmount', 'n3s'], ['LessAmount', 'LessAmount', 'n3s'], ['Remarks', 'Remarks']];
    function renderOc() { drawTable(null, 'ocBody', 'ocFoot', OC, OC_COLS, { lead: ['X'], leadCells: xBtn('ocdel'), link: 'ChargesItem' }); }
    function renderAll() { renderDetail(); renderOi(); renderPt(); renderCm(); renderOc(); }

    // ============================================================================== other items
    function oiCalc() { var q = netD(val('txtoQty')), r = netD(val('txtoRate')); setText('txtoAmount', q > 0 && r > 0 ? plain(q * r) : '0'); }
    function oiValidation() {
        if (sel('CmbOtherItem') === 0) { box('Item field required'); focus('CmbOtherItem'); return false; }
        if (netD(val('txtoRate').trim()) === 0) { box('Rate field required'); focus('txtoRate'); return false; }
        return true;
    }
    function oiReset() { IDX.oi = -1; setVal('CmbOtherItem', '0'); setText('txtoQty', ''); setText('txtoRate', ''); setText('txtoAmount', ''); setText('txtoRemarks', ''); }
    function oiAdd() {
        if (!oiValidation()) return;
        OI.push({ ItemId: sel('CmbOtherItem'), ItemName: selText('CmbOtherItem'), Qty: netD(val('txtoQty')), Rate: netD(val('txtoRate')), Amount: netD(val('txtoAmount')), Remarks: val('txtoRemarks').trim() });
        renderOi(); oiReset(); totalAmountCalculate(); proportionateOther();
    }
    function oiEdit(i) {
        var r = OI[i]; if (!r) return; IDX.oi = i;
        setVal('CmbOtherItem', r.ItemId); setText('txtoQty', str(r.Qty)); setText('txtoRate', str(r.Rate)); setText('txtoAmount', str(r.Amount)); setText('txtoRemarks', str(r.Remarks));
        show('btnoadd', false); show('btnoUpdate', true); show('btnoCancel', true); focus('CmbOtherItem');
    }
    function oiUpdate() {
        if (!oiValidation()) return;
        var r = OI[IDX.oi]; if (!r) return;
        r.ItemId = sel('CmbOtherItem'); r.ItemName = selText('CmbOtherItem'); r.Qty = netD(val('txtoQty')); r.Rate = netD(val('txtoRate')); r.Amount = netD(val('txtoAmount')); r.Remarks = val('txtoRemarks');
        show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false);
        oiReset(); renderOi(); focus('txtoQty'); totalAmountCalculate(); proportionateOther();
    }
    function oiCancel() { show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false); oiReset(); }
    /* CmbOtherItem_TextChanged: in update mode, when the combo carries a third (rate) column, Rate = that column. */
    function otherItemChanged() {
        if (!(shown('btnupdate') && !$id('btnupdate').disabled)) return;
        var r = findRow(L.otherItems, sel('CmbOtherItem'));
        if (r && Object.prototype.hasOwnProperty.call(r, 'Rate')) setText('txtoRate', str(r.Rate));
    }

    // ============================================================================== payment detail
    function ptValidation() {
        if (sel('dcmbpaymentterm') === 0) { box('Payment Term field required'); focus('dcmbpaymentterm'); return false; }
        if (sel('dcmbpaymentterm') === 1 && sel('CmbFinInstruments') === 0) throw new Error('Financial Instrument field is required against Advance');
        if (netD(val('dtxtpercentoftotal').trim()) === 0 || val('dtxtpercentoftotal') === '') { box('Percent Field Required'); focus('dtxtpercentoftotal'); return false; }
        if (netD(val('dtxtfcyamount').trim()) === 0 || val('dtxtfcyamount') === '') { box('Fcy Amount  Field Required'); focus('dtxtfcyamount'); return false; }
        return true;
    }
    function fiRow() { return findRow(L.fis, sel('CmbFinInstruments')); }
    function ptAdd() {
        try {
            if (!ptValidation()) return;
            var fi = fiRow();
            PT.forEach(function (r) { if (sel('CmbFinInstruments') > 0 && netI(r.ExImEFormRegistrationId) === sel('CmbFinInstruments') && fi && netI(r.DocumentTypeId) === netI(fi.DocumnetTypeId)) throw new Error('Financial Instrument No already add in Grid Please Check'); });
            PT.push({ Id: 0, PaymentTermId: sel('dcmbpaymentterm'), PaymentTerm: selText('dcmbpaymentterm'), DocumentTypeId: sel('CmbFinInstruments') > 0 && fi ? netI(fi.DocumnetTypeId) : 0,
                ExImEFormRegistrationId: sel('CmbFinInstruments'), FinancialInstrumentNo: sel('CmbFinInstruments') > 0 ? selText('CmbFinInstruments') : '',
                PrcntOfTotal: netD(val('dtxtpercentoftotal')), FcyAmount: netD(val('dtxtfcyamount')), DueDays: netD(val('dtxtdueday')), Remarks: val('txtPaymentRemarksDetail').trim() });
            renderPt(); ptReset(); focus('dcmbpaymentterm'); paymentBalance();
        } catch (e) { box(e.message); }
    }
    function ptEdit(i) {
        var r = PT[i]; if (!r) return; IDX.pt = i;
        setVal('dcmbpaymentterm', r.PaymentTermId); setVal('CmbFinInstruments', r.ExImEFormRegistrationId);
        setText('dtxtpercentoftotal', str(r.PrcntOfTotal)); setText('dtxtfcyamount', str(r.FcyAmount)); setText('dtxtdueday', str(r.DueDays)); setText('txtPaymentRemarksDetail', str(r.Remarks));
        show('btndadd', false); show('btndupdate', true); show('btndcancel', true); focus('dcmbpaymentterm');
    }
    function ptUpdate() {
        try {
            if (!ptValidation()) return;
            var fi = fiRow();
            PT.forEach(function (r, i) { if (fi && sel('CmbFinInstruments') > 0 && IDX.pt !== i && netI(r.ExImEFormRegistrationId) === sel('CmbFinInstruments') && netI(r.DocumentTypeId) === netI(fi.DocumnetTypeId)) throw new Error('Financial Instrument No already add in Grid Please Check'); });
            var r = PT[IDX.pt]; if (!r) return;
            r.PaymentTermId = sel('dcmbpaymentterm'); r.PaymentTerm = selText('dcmbpaymentterm');
            r.DocumentTypeId = sel('CmbFinInstruments') > 0 && fi ? netI(fi.DocumnetTypeId) : 0;
            r.ExImEFormRegistrationId = sel('CmbFinInstruments'); r.FinancialInstrumentNo = sel('CmbFinInstruments') > 0 ? selText('CmbFinInstruments') : '';
            r.PrcntOfTotal = netD(val('dtxtpercentoftotal')); r.FcyAmount = netD(val('dtxtfcyamount')); r.DueDays = val('dtxtdueday'); r.Remarks = val('txtPaymentRemarksDetail');
            show('btndadd', true); show('btndupdate', false); show('btndcancel', false);
            renderPt(); ptReset(); focus('dcmbpaymentterm'); paymentBalance();
        } catch (e) { box(e.message); }
    }
    function ptCancel() { show('btndadd', true); show('btndupdate', false); show('btndcancel', false); ptReset(); }
    /* ResePaymentDetails */
    function ptReset() { IDX.pt = -1; setVal('dcmbpaymentterm', '0'); setVal('CmbFinInstruments', '0'); setText('dtxtpercentoftotal', '0'); setText('dtxtfcyamount', '0'); setText('dtxtdueday', '0'); setText('txtBalanceFI', '0'); }
    function ptDelete(i) { try { if (IDX.pt !== -1) throw new Error('Reset Detail First...'); PT.splice(i, 1); renderPt(); } catch (e) { box(e.message); } paymentBalance(); }
    /* dtxtpercentoftotal_TextChanged -> PaymentDetailPercentCalculate */
    function percentChanged() {
        var total = netD(val('txtTotalNetAmount')), p = netD(val('dtxtpercentoftotal'));
        if (total > 0 && p > 0) {
            if (p > 100) { setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); return; }
            var a = total * p / 100.0; setText('dtxtfcyamount', a === 0 ? '' : fmtN(a, 3, true));
        }
    }
    /* dtxtfcyamount_TextChanged */
    function ptFcyChanged() {
        var total = netD(val('txtTotalNetAmount')), f = netD(val('dtxtfcyamount'));
        if (total > 0 && f > 0) {
            var p = f / total * 100.0; setText('dtxtpercentoftotal', plain(p));
            if (p > 100) { setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); }
        }
    }
    /* dcmbfino_ValueChanged */
    function fiChanged() {
        var fi = fiRow();
        if (fi && sel('CmbFinInstruments') > 0) {
            setVal('dcmbpaymentterm', fi.PaymenttermId); paymentTermChanged();
            getJson(API + '/fi-balance?id=' + netI(fi.Id) + '&documentTypeId=' + netI(fi.DocumnetTypeId)).then(function (d) { setText('txtBalanceFI', fmtN(d && d.balance, 3, false)); }).catch(function (e) { box(e.message); });
        } else setText('txtBalanceFI', '0');
    }
    /* CmbPaymentTermDetail_TextChanged */
    function paymentTermChanged() { if (sel('dcmbpaymentterm') !== 0) { var r = findRow(L.paymentTerms, sel('dcmbpaymentterm')); if (r && str(r.Description) !== '') setText('txtPaymentRemarksDetail', r.Description); } }
    /* CmbDeliveryTerm_TextChanged */
    function deliveryTermChanged() { if (sel('cmbdeliverytermnew') !== 0) { var r = findRow(L.deliveryTerms, sel('cmbdeliverytermnew')); if (r && str(r.Description) !== '') setText('txtDeliveryRemarks', r.Description); } }

    // ============================================================================== commission
    function commissionTypeChanged() {
        var id = sel('cmbCommissionType'), u = $id('txtCommRateUom');
        if (id === 1) { u.value = 'Lump Sum'; u.disabled = true; }
        else if (id === 2) { u.value = '%'; u.disabled = true; }
        else if (id === 3) { if (COMM_TAG === '') u.value = '1000'; u.disabled = false; }
        else { u.value = ''; u.disabled = false; }
        calculateCommission();
    }
    /* CalculateCommissionAmount */
    function calculateCommission() {
        var type = sel('cmbCommissionType'), commRate = netD(val('txtCommRate')), fcy = netD(val('txtfcyAmount'));
        if (type > 0 && commRate > 0 && fcy > 0) {
            var gridAmount = 0, gridRate = 0, kgs = netD(val('txtNetWeight')) * 1000.0, amt = 0;
            CM.forEach(function (r, i) { if (IDX.cm !== i) { gridAmount += netD(r.FcyAmount); gridRate += netD(r.Rate); } });
            if (type === 1) { if (gridAmount + commRate > fcy) { var rem = fcy - gridAmount; commRate = rem >= 0 ? rem : 0; } amt = commRate; }
            else if (type === 2) {
                if (commRate + gridRate > 99.0) { amt = fcy - gridAmount; commRate = amt * 100.0 / fcy; setText('txtCommRate', f4(commRate)); }
                amt = fcy * commRate / 100.0;
                if (amt + gridAmount > fcy) { var rem2 = fcy - gridAmount; amt = rem2 > 0 ? rem2 * commRate / 100.0 : 0; }
            } else if (type === 3) {
                if (kgs > 0) {
                    var uom = netD(val('txtCommRateUom'));
                    amt = kgs / uom * commRate;
                    if (amt + gridAmount > fcy) { var rem3 = fcy - gridAmount; if (rem3 > 0) { amt = rem3; commRate = amt * 100.0 / fcy; setText('txtCommRate', f4(commRate)); amt = kgs / uom * commRate; } else amt = 0; }
                } else amt = 0;
            }
            setText('txtCommissionAmount', f4(amt));
            var ex = netD(val('txtCommissionExchangeRate'));
            setText('txtCommissionLcyAmount', f4(ex > 0 ? ex * amt : 0));
        } else { setText('txtCommissionAmount', '0'); setText('txtCommissionLcyAmount', '0'); }
    }
    function cmValidation() {
        if (sel('CmbSalePerson') === 0) { box('Sales Person field is required'); focus('CmbSalePerson'); return false; }
        if (sel('cmbCommissionType') === 0) { box('Commission Type field is required'); focus('cmbCommissionType'); return false; }
        if (netD(val('txtCommRate')) === 0) { box('Commission Rate Field is Required'); focus('txtCommRate'); return false; }
        if (sel('cmbCommissionType') === 3 && val('txtCommRateUom') === '') { box('Commission Rate Uom Field is Required'); focus('txtCommRateUom'); return false; }
        if (netD(val('txtCommissionAmount').trim()) === 0 || val('txtCommissionAmount') === '') { box('Commission Fcy Amount Field is Required'); focus('txtCommissionAmount'); return false; }
        return true;
    }
    function cmReset() { IDX.cm = -1; setVal('CmbSalePerson', '0'); setVal('cmbCommissionType', '0'); setText('txtCommRate', '0'); setText('txtCommRateUom', '0'); COMM_TAG = ''; setText('txtCommissionAmount', '0'); setText('txtCommissionLcyAmount', '0'); }
    function cmAdd() {
        try {
            if (!cmValidation()) return;
            CM.forEach(function (r) { if (sel('CmbSalePerson') > 0 && netI(r.SalesPersonId) === sel('CmbSalePerson')) throw new Error('Sale person already add in Grid Please select another sale person!'); });
            CM.push({ Id: 0, SalesPersonId: sel('CmbSalePerson'), SalesPerson: selText('CmbSalePerson'), CommissionTypeId: sel('cmbCommissionType'), CommissionType: selText('cmbCommissionType'),
                Rate: netD(val('txtCommRate')), RateUom: val('txtCommRateUom').trim(), FcyAmount: netD(val('txtCommissionAmount')), ExchangeRate: netD(val('txtCommissionExchangeRate')),
                LcyAmount: netD(val('txtCommissionLcyAmount')), Remarks: val('txtCommissionRemarks').trim() });
            renderCm(); cmReset(); focus('CmbSalePerson');
        } catch (e) { box(e.message); }
    }
    function cmEdit(i) {
        var r = CM[i]; if (!r) return; IDX.cm = i;
        setVal('CmbSalePerson', r.SalesPersonId); setVal('cmbCommissionType', r.CommissionTypeId);
        setText('txtCommRate', plain(r.Rate)); setText('txtCommRateUom', str(r.RateUom)); setText('txtCommissionAmount', f3(r.FcyAmount));
        setText('txtCommissionExchangeRate', f3(r.ExchangeRate)); setText('txtCommissionLcyAmount', f3(r.LcyAmount)); setText('txtCommissionRemarks', str(r.Remarks));
        show('btnAddCommission', false); show('btnUpdateCommision', true); show('btnCancelCommission', true); focus('CmbSalePerson');
    }
    function cmUpdate() {
        try {
            if (!cmValidation()) return;
            CM.forEach(function (r, i) { if (sel('CmbSalePerson') > 0 && IDX.cm !== i && netI(r.SalesPersonId) === sel('CmbSalePerson')) throw new Error('Sales person already add in Grid Please selected another SalesPerson!'); });
            var r = CM[IDX.cm]; if (!r) return;
            r.SalesPersonId = sel('CmbSalePerson'); r.SalesPerson = selText('CmbSalePerson'); r.CommissionTypeId = sel('cmbCommissionType'); r.CommissionType = selText('cmbCommissionType');
            r.Rate = netD(val('txtCommRate')); r.RateUom = val('txtCommRateUom').trim(); r.FcyAmount = netD(val('txtCommissionAmount')); r.ExchangeRate = netD(val('txtCommissionExchangeRate'));
            r.LcyAmount = netD(val('txtCommissionLcyAmount')); r.Remarks = val('txtCommissionRemarks');
            show('btnAddCommission', true); show('btnUpdateCommision', false); show('btnCancelCommission', false);
            renderCm(); cmReset(); focus('CmbSalePerson');
        } catch (e) { box(e.message); }
    }
    function cmCancel() { show('btnAddCommission', true); show('btnUpdateCommision', false); show('btnCancelCommission', false); cmReset(); }
    function cmDelete(i) { try { if (IDX.cm !== -1) throw new Error('Reset Detail First...'); if (!$id('btnAddCommission').disabled) { CM.splice(i, 1); renderCm(); } } catch (e) { box(e.message); } }

    // ============================================================================== other charges
    function ocValidation() {
        if (sel('CmbChargesNameChargeDetail') === 0) { box('Charges Item field is required'); focus('CmbChargesNameChargeDetail'); return false; }
        if (netD(val('txtLessAmountChargeDetail')) === 0 && netD(val('txtAddAmountChargeDetail')) === 0) { box('Add Or Less Amount is required'); focus('txtLessAmountChargeDetail'); return false; }
        if (netD(val('txtLessAmountChargeDetail')) > 0 && netD(val('txtAddAmountChargeDetail')) > 0) { box("You Can Only Enter 'Add Or Less Amount'"); focus('txtLessAmountChargeDetail'); return false; }
        return true;
    }
    function ocReset() { IDX.oc = -1; setVal('CmbChargesNameChargeDetail', '0'); setText('txtAddAmountChargeDetail', ''); setText('txtLessAmountChargeDetail', ''); setText('txtRemarksChargeDetail', ''); show('BtnAddChargeDetail', true); show('BtnUpdateChargeDetail', false); show('BtnCancelChargeDetail', false); focus('CmbChargesNameChargeDetail'); }
    function ocAdd() {
        if (!ocValidation()) return;
        OC.push({ Id: 0, ContractId: 0, ContractOtherChargesDetailId: 0, ChargesItemId: sel('CmbChargesNameChargeDetail'), ChargesItem: selText('CmbChargesNameChargeDetail'),
            AddAmount: netD(val('txtAddAmountChargeDetail')), LessAmount: netD(val('txtLessAmountChargeDetail')), Remarks: val('txtRemarksChargeDetail').trim() });
        renderOc(); ocReset(); addLessFromCharges();
    }
    function ocEdit(i) {
        var r = OC[i]; if (!r) return; IDX.oc = i;
        setVal('CmbChargesNameChargeDetail', r.ChargesItemId); setText('txtAddAmountChargeDetail', str(r.AddAmount)); setText('txtLessAmountChargeDetail', str(r.LessAmount)); setText('txtRemarksChargeDetail', str(r.Remarks));
        show('BtnAddChargeDetail', false); show('BtnUpdateChargeDetail', true); show('BtnCancelChargeDetail', true); focus('CmbChargesNameChargeDetail');
    }
    function ocUpdate() {
        if (!ocValidation()) return;
        var r = OC[IDX.oc]; if (!r) return;
        r.ChargesItemId = sel('CmbChargesNameChargeDetail'); r.ChargesItem = selText('CmbChargesNameChargeDetail'); r.AddAmount = netD(val('txtAddAmountChargeDetail')); r.LessAmount = netD(val('txtLessAmountChargeDetail')); r.Remarks = val('txtRemarksChargeDetail');
        renderOc(); ocReset(); addLessFromCharges();
    }
    function ocCancel() { ocReset(); }
    function ocDelete(i) { try { if (IDX.oc !== -1) throw new Error('Reset Detail First...'); OC.splice(i, 1); renderOc(); addLessFromCharges(); } catch (e) { box(e.message); } }

    // ============================================================================== customer / form
    /* cmbSupCust_Leave: BindFinancialInstrument, bindConsigneeAgainstCustomer, HSCodeBind */
    function customerLeave() {
        return getJson(API + '/customer-leave?customerId=' + sel('cmbSupCust') + '&itemId=' + sel('combitem')).then(applyCustomer).catch(function (e) { box(e.message); });
    }
    function applyCustomer(d) {
        d = d || {};
        if ((d.fis || []).length) { L.fis = d.fis; bind('CmbFinInstruments', L.fis, 'Id', 'EFormNo'); }
        else if (sel('CmbFinInstruments') > 0 && !findRow(L.fis, sel('CmbFinInstruments'))) setVal('CmbFinInstruments', '0');
        if ((d.consignees || []).length) { L.consignees = d.consignees; bind('cmbConsignee', L.consignees, 'Id', 'Name'); }
        else if (sel('cmbConsignee') > 0 && !findRow(L.consignees, sel('cmbConsignee'))) setVal('cmbConsignee', '0');
        bindHs(d.hsCodes || []);
    }
    /* FormReset + DetailFormReset (btnNew_Click) */
    function formReset() {
        REMOVED = []; REC = { id: 0, updateMode: false, invoiceUpdateFlag: 0, attachments: '', customAttachments: '', forwardingId: 0, exportVoucherId: 0 };
        show('btnsave', true); show('btnupdate', false);
        setVal('cmbcareiertype', 1);
        setText('txtDocDate', today());
        setVal('cmbSupCust', '0'); setEnabled('cmbSupCust', false);
        setVal('cmbConsignee', '0'); setVal('cmbNotifyParty1', '0'); setVal('cmbNotifyParty2', '0'); setVal('cmbimporterBankNew', '0');
        setVal('cmbdeliverytermnew', '0'); setVal('cmbLoadingPort', '0'); setVal('cmbDestinationPort', '0');
        setText('txtcertificate', ''); setText('txtcertificateOfOrigion', ''); setText('txtSubContractExpiryDays', '0'); setText('txtLotRef', '');
        setVal('cmbexporterBankNew', '0'); setVal('cmbfcycode', '0'); setEnabled('cmbfcycode', false);
        ['txtfcyAmount', 'txtExRate', 'txtLocalAmt', 'txtGrossWeight', 'txtNetWeight', 'txtNoOfContainer', 'txtIformNo', 'txtaddlesscommnets', 'txtaddlessamount', 'txtTotalNetAmount', 'txtCustomerContractNos'].forEach(function (id) { setText(id, ''); });
        setText('txtPaymentRemarks', '');
        DETAIL = []; PT = []; OI = []; CM = []; OC = [];
        ocReset();
        IDX.pt = -1; IDX.cm = -1;
        L.detailItems = []; bind('combitem', [], 'Id', 'Name');
        setText('txtDocDate', today()); setText('txtIformDate', today());
        detailReset(); ptReset();
        $id('btnAddCommission').disabled = false; $id('btnUpdateCommision').disabled = false;
        cmReset();
        renderAll();
        focus('txtDocDate');
        return getJson(API + '/new').then(function (d) {
            d = d || {};
            if (netI(d.docNo) > 0) setText('txtdocno', d.docNo);
            invoiceNoState(!!d.autoGenInvoiceNo, d.invoiceNo);
            return customerLeave();
        }).catch(function (e) { box(e.message); });
    }
    function formNew(btn) { return busy(btn, formReset); }
    /* btnRefresh_Click */
    function refresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh').then(function (d) {
                d = d || {};
                bindAll(d);
                if (sel('combitem') > 0) itemLeave().catch(function () { /* HSCodeBind */ });
                invoiceNoState(!!d.autoGenInvoiceNo, d.invoiceNo);
            });
        });
    }

    // ============================================================================== save
    /* formvalidation (header) - the desktop's order and texts. */
    function formValidation() {
        if (F9 && sel('CmbExportCompany') === 0) { box('Export Company Is required'); focus('CmbExportCompany'); return false; }
        if (val('txtdocno') === '' || netI(val('txtdocno')) === 0) { box('Doc No Is Required'); focus('txtdocno'); return false; }
        if (val('cmbInvoiceNo') === '' || val('txtdocno') === '') { box('Invoice No Is Required'); focus('cmbInvoiceNo'); return false; }
        if (sel('cmbSupCust') === 0) { box('Customer Is Required'); focus('cmbSupCust'); return false; }
        if (CFG.flagFin && sel('CmbCreditAccount') === 0) {
            if (!ask('Are you sure to Credit Value charge to Sale Account?')) { box('Please select CreditAc'); focus('CmbCreditAccount'); return false; }
            CREDIT_CONFIRMED = true;
        }
        if (sel('cmbdeliverytermnew') === 0) { box('Delivery Term Is Required'); focus('cmbdeliverytermnew'); return false; }
        if (sel('cmbLoadingPort') === 0) { box('Loading Port Is Required'); focus('cmbLoadingPort'); return false; }
        if (sel('cmbDestinationPort') === 0) { box('Destination Port  Is Required'); focus('cmbDestinationPort'); return false; }
        if (sel('cmbcareiertype') === 0) { box('Carrier Type  Is Required'); focus('cmbcareiertype'); return false; }
        if (sel('cmbfcycode') === 0) { box('Fcy Code Is Required'); focus('cmbfcycode'); return false; }
        if (netD(val('txtfcyAmount')) === 0) { box('Fcy Amount  Is Required'); focus('txtfcyAmount'); return false; }
        if (netI(val('txtNoOfContainer')) === 0) { box('No.of Containers Field Is Required'); focus('txtNoOfContainer'); return false; }
        if (!(netD(val('txtNetWeight')) > 0)) { box('Net Weight  Is Required'); focus('txtNetWeight'); return false; }
        if (!(netD(val('txtGrossWeight')) >= netD(val('txtNetWeight')))) { box('Gross Weight Must Be Equal Or Greater Than Net Weight Thank You'); focus('txtGrossWeight'); return false; }
        if (!(netD(val('txtExRate')) > 0)) { box('ExchangeRate Field is Required'); focus('txtExRate'); return false; }
        if (!(netD(val('txtTotalNetAmount')) > 0)) { box('Total Amount  Is Required'); focus('txtTotalNetAmount'); return false; }
        return true;
    }
    var CREDIT_CONFIRMED = false;
    function header() {
        return {
            ExportCompanyId: sel('CmbExportCompany'), ProjectsId: PROJECT_ID, DocCode: netI(val('txtdocno')), DocDate: val('txtDocDate') || today(),
            InvoiceNo: val('cmbInvoiceNo'), SupplierCustomerId: sel('cmbSupCust'), ConsigneeId: sel('cmbConsignee'), NotifyParty1: sel('cmbNotifyParty1'), NotifyParty2: sel('cmbNotifyParty2'),
            LotNoRef: val('txtLotRef'), EFormNo: val('txtIformNo'), EFormDate: val('txtIformDate') || today(), FcurrencyId: sel('cmbfcycode'),
            ConversionRate: netD(val('txtExRate')), FCurrencyAmount: netD(val('txtfcyAmount')), EquivalentAmount: netD(val('txtLocalAmt')), AddLessAmount: netD(val('txtaddlessamount')),
            TotalAmount: netD(val('txtTotalNetAmount')), AddLessComments: val('txtaddlesscommnets'), DeliveryTermId: sel('cmbdeliverytermnew'), PaymentRemarks: val('txtPaymentRemarks'),
            DeliveryRemarks: val('txtDeliveryRemarks'), ImporterBankId: sel('cmbimporterBankNew'), ExporteBankId: sel('cmbexporterBankNew'), LoadingPortId: sel('cmbLoadingPort'),
            DestinationPortId: sel('cmbDestinationPort'), CarierTypeId: sel('cmbcareiertype'), CarierType: selText('cmbcareiertype'), GrossMton: netD(val('txtGrossWeight')),
            NoOfContainers: val('txtNoOfContainer'), Certificate1: val('txtcertificate'), Certificate2: val('txtcertificateOfOrigion'), CreditAccountId: sel('CmbCreditAccount'),
            CustomerContractNos: val('txtCustomerContractNos'), SubContractExpiryDays: val('txtSubContractExpiryDays'), AttachmentsValues: REC.attachments,
            CustomAttachmentsValues: REC.customAttachments, creditToSaleConfirmed: CREDIT_CONFIRMED
        };
    }
    /* btnsave_Click */
    function doSave(btn) {
        return busy(btn, function () {
            if (DETAIL.length <= 0) { box('Grid Record not found'); return Promise.resolve(); }
            if (!ask(REC.id === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            setText('txtNetWeight', f3(sum(DETAIL, 'QtyMTon')));
            if (netD(val('txtaddlessamount')) > 0 && OC.length === 0 && !ask("Are you sure to proceed because add less amount not zero but Charges grid don't have any record?")) {
                tab('detail', 'tabOtherCharges'); focus('CmbChargesNameChargeDetail'); return Promise.resolve();
            }
            CREDIT_CONFIRMED = false;
            if (!formValidation()) return Promise.resolve();
            if (PT.length === 0) { tab('detail', 'tabPage5'); focus('dcmbpaymentterm'); box('Payment Detail Not Found. Please Check! '); return Promise.resolve(); }
            var preview = $id('ChkPrintPreview').checked;
            var win = preview && PERM.Print && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
            return postJson(API + '/save', { recId: REC.id, header: header(), detail: DETAIL, removed: REMOVED, otherItems: OI, paymentTerms: PT, commissions: CM, otherCharges: OC })
                .then(function (d) {
                    box((d && d.message) || (REC.id === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                    var printId = d ? netI(d.printId) : 0;
                    return formReset().then(function () {
                        if (preview && win) print('521-exportinvoiceslip', { eximInvoiceId: printId }, null, win);
                    });
                })
                .catch(function (e) { if (win && global.CrystalPrint) global.CrystalPrint.release(win); box(e.message); });
        });
    }
    function save(btn) { return doSave(btn); }
    /* btnupdate_Click */
    function update(btn) { if (REC.id === 0) { box('Rec Id not found...'); return Promise.resolve(); } return doSave(btn); }

    // ============================================================================== read by id
    function readById(id) {
        return getJson(API + '/by-id?id=' + netI(id)).then(function (o) {
            REMOVED = [];
            show('btnsave', false); show('btnupdate', true);
            REC.id = netI(o.Id);
            tab('main', 'tabPage1');
            setText('txtdocno', str(o.DocCode)); setText('txtDocDate', isoDate(o.DocDate));
            setText('cmbInvoiceNo', str(o.InvoiceNo));
            setVal('cmbSupCust', o.SupplierCustomerId);
            applyCustomer(o.customer);
            setVal('cmbConsignee', o.ConsigneeId); setVal('cmbNotifyParty1', o.NotifyParty1); setVal('cmbNotifyParty2', o.NotifyParty2);
            setText('txtLotRef', str(o.LotNoRef));
            setVal('cmbimporterBankNew', o.ImporterBankId); setVal('cmbexporterBankNew', o.ExporteBankId);
            setVal('cmbdeliverytermnew', o.DeliveryTermId); setVal('cmbLoadingPort', o.LoadingPortId); setVal('cmbDestinationPort', o.DestinationPortId);
            var c = L.carrier.filter(function (x) { return x.Name === str(o.CarierType); })[0]; setVal('cmbcareiertype', c ? c.Id : '0');
            setVal('cmbfcycode', o.FcurrencyId);
            setText('txtfcyAmount', f4(o.FCurrencyAmount)); setText('txtLocalAmt', f4(o.EquivalentAmount));
            setVal('CmbExportCompany', o.ExportCompanyId);
            setText('txtGrossWeight', netD(o.GrossMton) > 0 ? f4(o.GrossMton) : f4(netD(o.GrossWeight) / 1000.0));
            setText('txtNetWeight', netD(o.NetMton) > 0 ? f4(o.NetMton) : f4(netD(o.NetWeight) / 1000.0));
            setText('txtNoOfContainer', plain(o.NoOfContainers));
            setText('txtIformNo', str(o.EFormNo)); setText('txtIformDate', isoDate(o.EFormDate));
            setText('txtcertificate', str(o.Certificate1)); setText('txtcertificateOfOrigion', str(o.Certificate2));
            setText('txtPaymentRemarks', str(o.PaymentRemarks)); setText('txtDeliveryRemarks', str(o.DeliveryRemarks));
            setText('txtaddlesscommnets', str(o.AddLessComments)); setText('txtaddlessamount', plain(o.AddLessAmount));
            setText('txtTotalNetAmount', f4(o.TotalAmount));
            setVal('CmbCreditAccount', o.CreditAccountId);
            setText('txtCustomerContractNos', str(o.CustomerContractNos)); setText('txtSubContractExpiryDays', str(o.SubContractExpiryDays));
            REC.attachments = str(o.AttachmentsValues); REC.customAttachments = str(o.CustomAttachmentsValues);
            REC.exportVoucherId = netI(o.ExportVoucherId); REC.forwardingId = netI(o.ForwardingId);
            if (REC.exportVoucherId > 0) { $id('btnAddCommission').disabled = true; $id('btnUpdateCommision').disabled = true; }
            DETAIL = (o.detail || []).map(copy);
            REC.invoiceUpdateFlag = netI(o.invoiceUpdateFlag);
            var items = []; DETAIL.forEach(function (r) { if (!findRow(items, r.ItemId)) items.push({ Id: netI(r.ItemId), Name: str(r.ItemName) }); });
            L.detailItems = items; if (items.length) bind('combitem', items, 'Id', 'Name', [], '0');
            OI = (o.otherItems || []).map(copy);
            PT = (o.paymentTerms || []).map(copy);
            CM = (o.commissions || []).map(copy);
            OC = (o.otherCharges || []).map(copy);
            renderAll();
            if (OC.length) setText('txtaddlessamount', f3b(sum(OC, 'AddAmount') - sum(OC, 'LessAmount')));
            else setText('txtaddlessamount', plain(o.AddLessAmount));
            proportionateOther();
            detailReset();
            REC.updateMode = true;
            setText('txtExRate', f4(o.ConversionRate));
            setText('txtCommissionExchangeRate', val('txtExRate'));
            proportionateAddLess();
            paymentBalance();
            focus('txtDocDate');
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================== prints
    function print(key, args, btn, win) {
        if (!global.CrystalPrint) { box('Print is not available.'); return Promise.resolve(); }
        return global.CrystalPrint.open(key, args, btn, win);
    }
    function noRecord(id, btn, key, args) { if (netI(id) === 0) { box('No Record Found For Display'); return Promise.resolve(); } return print(key, args, btn); }
    function print521(btn) { return noRecord(REC.id, btn, '521-exportinvoiceslip', { eximInvoiceId: REC.id }); }
    function print529(btn) { return noRecord(REC.id, btn, '529-exportinvoiceslippackinglist', { eximInvoiceId: REC.id }); }
    function print501B(btn) { return noRecord(REC.id, btn, '501b-exprptsalescontractexport', { contractId: 0, invoiceId: REC.id }); }

    // ============================================================================== history
    function bindHistoryCombos(d) { L.histCustomers = d.customers || []; L.histInvoices = d.invoices || []; bind('CmbCustomerHistory', L.histCustomers, 'Id', 'Name'); bind('CmbInvoiceNoHistory', L.histInvoices, 'Id', 'Name'); }
    var HIST_COLS = [['InvoiceNo', 'InvoiceNo'], ['DocCode', 'DocCode'], ['DocDate', 'DocDate'], ['CustomerName', 'CustomerName'], ['ContractNos', 'ContractNos'],
        ['NoOfContainers', 'NoOfContainers', 'n3s'], ['GrossWeight', 'GrossWeight', 'n3s'], ['NetWeight', 'NetWeight', 'n3s'], ['FcyAmount', 'FcyAmount', 'n3s'], ['AddLess', 'Add/Less', 'n3s'],
        ['TotalAmount', 'TotalAmount', 'n3s'], ['JobLots', 'JobLots'], ['LoadingPort', 'LoadingPort'], ['DestinationPort', 'DestinationPort'], ['EntryUser', 'EntryUser'],
        ['EntryDate', 'EntryDate'], ['ModifyUser', 'ModifyUser'], ['ModifyDate', 'ModifyDate'], ['NoOfAttachments', 'NoOfAttachments']];
    function histLead() {
        var lead = [];
        if (PERM.Update) lead.push('Edit');
        if (PERM.Print) lead.push('Gp Print', 'Do Print', 'Print Packing List', '521-Slip');
        return lead;
    }
    function renderHistory() {
        var lead = histLead();
        drawTable('histHead', 'histBody', 'histFoot', HIST, HIST_COLS, {
            lead: lead.concat([]), cur: CUR.hist, link: 'InvoiceNo',
            dateCols: { DocDate: ddMMMyyyy, EntryDate: dtFmt, ModifyDate: dtFmt },
            leadCells: function (r, i) {
                return lead.map(function (k) { return '<td class="win-cell-btn"><button type="button" class="win-edit" data-hbtn="' + esc(k) + '" data-i="' + i + '">' + esc(k) + '</button></td>'; }).join('');
            }
        });
        var tr = $id('histHead').querySelector('tr'); if (tr) tr.insertAdjacentHTML('beforeend', '<th>AddAttachment</th>');
        $id('histBody').querySelectorAll('tr').forEach(function (row) { row.insertAdjacentHTML('beforeend', '<td class="win-cell-btn"><button type="button" class="win-edit" data-hbtn="Add Attachment" data-i="' + row.getAttribute('data-i') + '">Add Attachment</button></td>'); });
        show('histEmpty', HIST.length === 0);
        setTextNode('label92', 'Filtered Records' + (HIST.length ? ' (' + HIST.length + ')' : ''));
    }
    function setTextNode(id, t) { var e = $id(id); if (e) e.textContent = t; }
    function renderHistDetail() { drawTable('histDetailHead', 'histDetailBody', 'histDetailFoot', HIST_DETAIL, visibleDetCols(), {}); }
    function historyShow(btn) {
        return busy(btn, function () {
            var body = {
                dateKind: (document.querySelector('input[name="histDate"]:checked') || {}).value || 'doc',
                fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'), toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'),
                fromDocNo: netI(val('FromDocNoHistory')), toDocNo: netI(val('ToDocNoHistory')), customerId: sel('CmbCustomerHistory'), invoiceId: sel('CmbInvoiceNoHistory'),
                actionId: netI((document.querySelector('input[name="histRef"]:checked') || {}).value)
            };
            return postJson(API + '/history', body).then(function (rows) {
                HIST = rows || []; CUR.hist = -1; renderHistory();
                HIST_DETAIL = []; renderHistDetail();
            }).catch(function (e) { box(e.message); });
        });
    }
    /* btnResetHistory_Click: From = today - 3 (not the configured days), To = today, doc nos and customer cleared, grids cleared. */
    function historyReset() {
        setText('FromDateHistory', daysAgo(3)); setText('ToDateHistory', today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', ''); setVal('CmbCustomerHistory', '0');
        HIST = []; HIST_DETAIL = []; renderHistory(); renderHistDetail(); show('histEmpty', false);
    }
    function historyRefresh(btn) { return busy(btn, function () { return getJson(API + '/history-combos').then(bindHistoryCombos).catch(function (e) { box(e.message); }); }); }
    function historySelect(i) {
        CUR.hist = i; var r = HIST[i]; if (!r) return;
        getJson(API + '/history-detail?id=' + netI(r.Id)).then(function (rows) { HIST_DETAIL = rows || []; renderHistDetail(); }).catch(function (e) { box(e.message); });
    }
    function historyButton(k, i) {
        var r = HIST[i]; if (!r) return; var id = netI(r.Id);
        if (k === '521-Slip') noRecord(id, null, '521-exportinvoiceslip', { eximInvoiceId: id });
        else if (k === 'Edit') readById(id);
        else if (k === 'Print Packing List') noRecord(id, null, '529-exportinvoiceslippackinglist', { eximInvoiceId: id });
        else if (k === 'Do Print') { if (id === 0) box('Record Not Found For Display'); else print('527-exportdeliveryorderbyinvoiceidslip', { documentTypeId: 84, eximInvoiceId: id }); }
        else if (k === 'Gp Print') { if (id === 0) box('Record Not Found For Display'); else print('295-invrptoutwardgatepassslipwithitems', { invoiceId: id }); }
        else if (k === 'Add Attachment') notPorted('Add Attachment');
    }

    // ============================================================================== loader (LoadSalesContractForInvoice)
    function loaderOpen(btn) {
        return busy(btn, function () {
            return getJson(API + '/loader/setup').then(function (d) {
                LDR.setup = d || {};
                bind('LdrCmbPartyName', LDR.setup.customers || [], 'Id', 'Name', [], '0');
                bind('LdrcmbItemName', LDR.setup.items || [], 'Id', 'Name', [], '0');
                bind('LdrcmbCurrency', LDR.setup.currencies || [], 'Id', 'Name', [], '0');
                setText('LdrFromDate', isoDate(LDR.setup.fromDate) || today()); setText('LdrTodate', today());
                var saved = scheduleIdsForSaved() !== '';
                show('ldrSavedBox', saved);
                if (saved) $id('RdLoadSavedSchedulesDetail').checked = true;
                show('loaderModal', true);
                return loaderSearchRun();
            });
        });
    }
    /* BtnLoad_Click: when Update is visible and enabled, Loader.ScheduleIds = the distinct schedules of the grid. */
    function scheduleIdsForSaved() {
        if (!(shown('btnupdate') && !$id('btnupdate').disabled)) return '';
        var ids = []; DETAIL.forEach(function (r) { var s = netI(r.ContractScheduleId); if (ids.indexOf(s) < 0) ids.push(s); });
        return ids.join(',');
    }
    function loaderClose() { show('loaderModal', false); }
    var LDR_COLS = [['DocNo', 'DocNo'], ['ContractNo', 'ContractNo'], ['ContractDate', 'ContractDate'], ['OrderDate', 'OrderDate'], ['LoadingDate', 'LoadingDate'], ['ScheduleCode', 'ScheduleCode'],
        ['PartyName', 'PartyName'], ['NoOfContainers', 'NoOfContainers'], ['SalesMan', 'SalesMan'], ['ExporterBank', 'ExporterBank'], ['ShipmentStartDate', 'ShipmentStartDate'],
        ['DeliveryDays', 'DeliveryDays'], ['LastShipmentDate', 'LastShipmentDate'], ['CustomerApprovedDate', 'CustomerApprovedDate'], ['CustomerApprovalRemarks', 'CustomerApprovalRemarks'],
        ['CurrencyCode', 'CurrencyCode'], ['ExchangeRate', 'ExchangeRate'], ['DeliveryTerm', 'DeliveryTerm'], ['PaymentTerm', 'PaymentTerm'], ['LoadingPort', 'LoadingPort'],
        ['DestinationPort', 'DestinationPort'], ['InsuranceRemarks', 'InsuranceRemarks'], ['RemarksHeader', 'RemarksHeader'], ['ApprovedStatus', 'ApprovedStatus'], ['EntryUser', 'EntryUser'],
        ['EntryDate', 'EntryDate'], ['ModifyUser', 'ModifyUser'], ['ModifyDate', 'ModifyDate'], ['NoOfAttachments', 'NoOfAttachments']];
    function loaderSearchRun() {
        var body = { fromDate: val('LdrFromDate'), toDate: val('LdrTodate'), customerId: sel('LdrCmbPartyName'), itemId: sel('LdrcmbItemName'), currencyId: sel('LdrcmbCurrency'),
            loadSaved: shown('ldrSavedBox') && $id('RdLoadSavedSchedulesDetail').checked, scheduleIds: scheduleIdsForSaved() };
        return postJson(API + '/loader/search', body).then(function (rows) {
            LDR.rows = rows || [];
            /* one header row per ScheduleId (schedule wise) */
            var seen = {}; LDR.header = [];
            LDR.rows.forEach(function (r) {
                var k = netI(col(r, 'ScheduleId')); if (seen[k]) return; seen[k] = true;
                LDR.header.push({ Id: netI(col(r, 'Id')), ContractDetailId: col(r, 'ContractDetailId'), ScheduleId: k, ScheduleDetailId: col(r, 'ScheduleDetailId'), DocumentTypeId: col(r, 'DocumentTypeId'),
                    DocNo: col(r, 'DocNo'), ContractNo: col(r, 'ProformaNo'), ContractDate: shortDate(col(r, 'ProformaDate')), OrderDate: shortDate(col(r, 'SalesContratDate')), LoadingDate: shortDate(col(r, 'LoadingDate')),
                    ScheduleStatus: (col(r, 'ContractScheduleStatus') === true || netI(col(r, 'ContractScheduleStatus')) === 1 || str(col(r, 'ContractScheduleStatus')).toLowerCase() === 'true') ? 'Exist' : 'Not Exist',
                    ScheduleCode: col(r, 'ScheduleCode'), SupplierCustomerId: col(r, 'SupCustId'), PartyName: col(r, 'CustomerName'), NoOfContainers: col(r, 'NoOfContainers'), SalesMan: col(r, 'SalesMan'),
                    ExporterBank: col(r, 'ExporterBank'), ShipmentStartDate: shortDate(col(r, 'ShipmentStartDate')), DeliveryDays: col(r, 'DeliveryDays'), LastShipmentDate: shortDate(col(r, 'LastShipmentDate')),
                    CustomerApprovedDate: shortDate(col(r, 'CustomerApprovalDate')), CustomerApprovalRemarks: col(r, 'CustomerApprovalRemarks'), FCurrencyId: col(r, 'FCurrencyId'), CurrencyCode: col(r, 'CurrencyCode'),
                    ExchangeRate: col(r, 'ExchangeRate'), DeliveryTerm: col(r, 'DeliveryTerm'), PaymentTerm: col(r, 'PaymentTerm'), LoadingPort: col(r, 'LoadingPort'), DestinationPort: col(r, 'DestinationPort'),
                    InsuranceRemarks: col(r, 'InsuranceRemarks'), RemarksHeader: col(r, 'RemarksHeader'), IsApproved: col(r, 'IsApproved'), ApprovedStatus: col(r, 'ApprovedStatus'), EntryUser: col(r, 'EntryUser'),
                    EntryDate: dtFmt(col(r, 'EntryDate')), ModifyUser: col(r, 'ModifyUser'), ModifyDate: dtFmt(col(r, 'ModifyDate')), NoOfAttachments: col(r, 'NoOfAttachments') });
            });
            drawTable('ldrHead', 'ldrBody', null, LDR.header, LDR_COLS, { lead: ['Select'], leadCells: function (r, i) { return '<td class="ctr"><input type="checkbox" data-ldr="' + i + '"/></td>'; } });
            var th = $id('ldrHead').querySelector('th'); if (th) th.innerHTML = '<input type="checkbox" id="ldrAll" title="Select all"/>';
            $id('ldrDetailBody').innerHTML = ''; $id('ldrDetailFoot').innerHTML = ''; $id('ldrDetailHead').innerHTML = '';
        }).catch(function (e) { box(e.message); });
    }
    function loaderSearch(btn) { return busy(btn, loaderSearchRun); }
    function loaderReset(btn) { setVal('LdrCmbPartyName', '0'); return busy(btn, loaderSearchRun); }
    var LDR_DET_COLS = [['ItemName', 'ItemName'], ['PackingType', 'PackingType'], ['CropYear', 'CropYear'], ['CommodityDetail', 'CommodityDetail'], ['PackUom', 'PackUom'],
        ['MTon', 'MTon', 'n0s'], ['NoOfBags', 'NoOfBags', 'n0s'], ['NetWeight', 'NetWeight', 'n0s'], ['ItemRate', 'ItemRate', 'n4'], ['Amount', 'Amount', 'n0s']];
    /* grd_SelectionChanged: the detail of the schedule */
    function loaderSelect(i) {
        var h = LDR.header[i]; if (!h) return;
        var rows = LDR.rows.filter(function (r) { return netI(col(r, 'ScheduleId')) === netI(h.ScheduleId); }).map(function (r) {
            return { ItemName: col(r, 'ItemName'), PackingType: col(r, 'PackingType'), CropYear: col(r, 'CropYear'), CommodityDetail: col(r, 'CommodityDetail'), PackUom: col(r, 'PackUom'),
                MTon: col(r, 'M_Ton'), NoOfBags: col(r, 'NoOfBags'), NetWeight: col(r, 'NetWeight'), ItemRate: col(r, 'RatePrice'), Amount: col(r, 'Amount') };
        });
        drawTable('ldrDetailHead', 'ldrDetailBody', 'ldrDetailFoot', rows, LDR_DET_COLS, {});
        $id('ldrBody').querySelectorAll('tr').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
    }
    /* btnLoadOnInvoice_Click_1 */
    function loaderLoad() {
        var checked = []; $id('ldrBody').querySelectorAll('input[data-ldr]:checked').forEach(function (c) { checked.push(LDR.header[+c.getAttribute('data-ldr')]); });
        if (!checked.length) { box('Check the row first'); return; }
        var cust = 0, ids = [];
        for (var k = 0; k < checked.length; k++) {
            var r = checked[k];
            var isApproved = r.IsApproved === true || str(r.IsApproved).toLowerCase() === 'true' || netI(r.IsApproved) === 1;
            if (LDR.setup.approvalMandatory && !isApproved) { box('Record cannot be load because Export sales contract approval pending...'); return; }
            if (r.ScheduleStatus === 'Not Exist') { box("Sorry! The selected ' " + str(r.ContractNo) + " ' Contract. Schedule Not Exist. Please Select Contracts Whose Schedule Exists."); return; }
            if (cust === 0) cust = netI(r.SupplierCustomerId);
            if (cust !== netI(r.SupplierCustomerId)) { box("Sorry! The selected Schedule's are not of the same Customer."); return; }
            ids.push(String(r.ScheduleId)); cust = netI(r.SupplierCustomerId);
        }
        var data = LDR.rows.filter(function (x) { return ids.indexOf(String(netI(col(x, 'ScheduleId')))) >= 0; });
        loaderClose();
        bindLoaderData(data);
    }
    /* BindLoaderData */
    function bindLoaderData(contract) {
        if (!contract.length) return;
        var first = contract[0];
        try {
            if (DETAIL.length > 0) {
                if (netI(col(first, 'ScheduleDetailId')) === 0) {
                    if (netI(col(first, 'SupCustId')) !== sel('cmbSupCust') || netI(col(first, 'FcurrencyId')) !== sel('cmbfcycode'))
                        throw new Error("Already Loaded Row's Have Different Customer And Currency. So you Can't Load Rows Of Different Customer or Currency!");
                } else if (netI(col(first, 'SupCustId')) !== sel('cmbSupCust')) throw new Error("Already Loaded Row's Have Different Customer. So you Can't Load Rows Of Different Customer!");
            }
        } catch (e) { box(e.message); return; }
        setEnabled('cmbSupCust', false); setEnabled('cmbfcycode', false);
        setVal('cmbSupCust', netI(col(first, 'SupCustId')));
        customerLeave();
        setVal('cmbNotifyParty1', netI(col(first, 'NotifyPartyId')));
        setVal('cmbexporterBankNew', netI(col(first, 'ExporterBankId')));
        setVal('cmbfcycode', netI(col(first, 'FcurrencyId')));
        setVal('cmbdeliverytermnew', netI(col(first, 'DeliveryTermId'))); deliveryTermChanged();
        setVal('cmbLoadingPort', netI(col(first, 'LoadingPortId')));
        setVal('cmbDestinationPort', netI(col(first, 'DestinationPortId')));
        setVal('CmbExportCompany', netI(col(first, 'ExportCompanyId')));
        var seenS = {}, containers = 0;
        contract.forEach(function (r) { var s = netI(col(r, 'ScheduleId')); if (!seenS[s]) { seenS[s] = true; containers += netI(col(r, 'NoOfContainers')); } });
        setText('txtNoOfContainer', String(containers));
        var nos = []; contract.forEach(function (r) { var c = str(col(r, 'CustomerContractNo')); if (c.trim() !== '' && nos.indexOf(c) < 0) nos.push(c); });
        setText('txtCustomerContractNos', nos.join(', '));
        var items = REC.id > 0 ? (L.detailItems || []).slice() : [];
        contract.forEach(function (r) {
            var exists = DETAIL.some(function (g) {
                return netI(col(r, 'ScheduleDetailId')) === 0 ? netI(g.ContractDetailId) === netI(col(r, 'ContractDetailId')) : netI(g.ContractScheduleDetailId) === netI(col(r, 'ScheduleDetailId'));
            });
            if (exists) return;
            if (!findRow(items, col(r, 'ExImItemId'))) items.push({ Id: netI(col(r, 'ExImItemId')), Name: str(col(r, 'ItemName')) });
            DETAIL.push({ Id: 0, ContractId: netI(col(r, 'Id')), ContractDetailId: netI(col(r, 'ContractDetailId')), ContractNo: str(col(r, 'ProformaNo')), ContractDate: isoDate(col(r, 'ProformaDate')),
                ContractNoInvoiceWise: str(col(r, 'ScheduleCode')), ItemId: netI(col(r, 'ExImItemId')), ItemName: str(col(r, 'ItemName')), PackTypeId: netI(col(r, 'InvPackingMaterialTypeId')),
                PackType: str(col(r, 'PackingTYpe')), CropYearId: netI(col(r, 'CropYearId')), CropYear: str(col(r, 'CropYear')), JobLotId: netI(col(r, 'JobLotId')), JobLot: str(col(r, 'JobLotDescription')),
                QtyMTon: netD(col(r, 'BalMTon')), PackSizeId: netI(col(r, 'PackUomId')), PackSize: str(col(r, 'PackUom')), NoOfBags: netD(col(r, 'BalNoofBags')), RateWithoutAddLess: netD(col(r, 'RatePrice')),
                RateAddLess: 0, CostMTon: netD(col(r, 'RatePrice')), ContractRate: netD(col(r, 'RatePrice')), RateUOMId: netI(col(r, 'RateUomId')), RateUOM: str(col(r, 'RateUom')),
                RateEquivalent: netD(col(r, 'RateEquivalent')), Amount: netD(col(r, 'BalShipAmount')), ContractScheduleId: netI(col(r, 'ScheduleId')), ContractSchedule: '', HSCode: '',
                ItemDetail: str(col(r, 'CommodityDetail')), PackingDetail: '', OtherItemAmount: 0, AddLessAmount: 0, FlagToUpdateScheduleCombo: false,
                ExImFarmingNTradeId: netI(col(r, 'ExImFarmingNTradeId')), FarmingNTrade: str(col(r, 'FarmingNTrade')), ExImFarmingTypeId: netI(col(r, 'ExImFarmingTypeId')),
                ExImTradeTypeId: netI(col(r, 'ExImTradeTypeId')), ContractScheduleDetailId: netI(col(r, 'ScheduleDetailId')), ScheduleFCL: 0 });
        });
        if (items.length) { L.detailItems = items; bind('combitem', items, 'Id', 'Name'); }
        var distinct = []; contract.forEach(function (r) { var id = netI(col(r, 'Id')); if (distinct.indexOf(id) < 0) distinct.push(id); });
        getJson(API + '/loader/apply?ids=' + encodeURIComponent(distinct.join(','))).then(function (d) {
            d = d || {};
            OI = (d.otherItems || []).map(copy);
            proportionateOther();
            PT = (d.paymentTerms || []).map(copy);
            var existing = OC.map(function (r) { return netI(r.ContractOtherChargesDetailId); });
            (d.otherCharges || []).forEach(function (r) { if (existing.indexOf(netI(r.ContractOtherChargesDetailId)) < 0) { existing.push(netI(r.ContractOtherChargesDetailId)); OC.push(copy(r)); } });
            renderAll();
            setText('txtaddlessamount', f3b(sum(OC, 'AddAmount') - sum(OC, 'LessAmount')));
            setText('txtExRate', plain(col(first, 'ExchangeRate')));
            exRateChanged();
            totalsFromGrid();
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================== shortcut keys
    var SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+L', 'For Load All Records'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowRight', 'For Moving In Detail Tabs'], ['Ctrl+ArrowDown', 'For Focus On Selected Tab Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On First Entry of Detail Selected Tab'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() {
        $id('shortcutBody').innerHTML = SHORTCUTS.map(function (s) { return '<tr><td>' + esc(s[0]) + '</td><td>' + esc(s[1]) + '</td></tr>'; }).join('');
        show('shortcutModal', true);
    }
    function closeForm() { global.close(); setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150); }

    // ============================================================================== wiring
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function wire(bodyId, h) {
        var gb = $id(bodyId); if (!gb) return;
        gb.addEventListener('click', function (e) {
            var b = e.target.closest('button'), a = e.target.closest('a.win-code');
            if (b) {
                var attrs = ['del', 'dup', 'ptdel', 'cmdel', 'ocdel'];
                for (var k = 0; k < attrs.length; k++) if (b.hasAttribute('data-' + attrs[k])) { (h[attrs[k]] || function () {})(+b.getAttribute('data-' + attrs[k])); return; }
                if (b.hasAttribute('data-hbtn')) { (h.hbtn || function () {})(b.getAttribute('data-hbtn'), +b.getAttribute('data-i')); return; }
            }
            if (a) { h.open(+a.getAttribute('data-i')); return; }
            if (e.target.closest('input')) return;
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            if (h.select) h.select(+tr.getAttribute('data-i'));
        });
        gb.addEventListener('dblclick', function (e) { if (e.target.closest('input,button')) return; var tr = e.target.closest('tr[data-i]'); if (tr) h.open(+tr.getAttribute('data-i')); });
        gb.addEventListener('change', function (e) { var inp = e.target.closest('input[data-edit-col]'); if (inp && h.edit) h.edit(+inp.getAttribute('data-i'), inp.getAttribute('data-edit-col'), inp.value); });
        gb.addEventListener('keydown', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); h.open(+tr.getAttribute('data-i')); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.closest('.win-tabs').getAttribute('data-tabs'), b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) { b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); }); });
        on('cmbSupCust', 'change', customerLeave);
        on('cmbdeliverytermnew', 'change', deliveryTermChanged);
        on('dcmbpaymentterm', 'change', paymentTermChanged);
        on('CmbFinInstruments', 'change', fiChanged);
        on('txtExRate', 'input', exRateChanged);
        on('txtQtyMTon', 'input', weightAndAmount);
        on('CmbPackUom', 'change', weightAndAmount);
        on('txtCostMTon', 'input', weightAndAmount);
        on('CmbRateUom', 'change', weightAndAmount);
        on('combitem', 'change', function () { itemLeave().catch(function (e) { box(e.message); }); });
        on('txtoQty', 'input', oiCalc); on('txtoRate', 'input', oiCalc);
        on('CmbOtherItem', 'change', otherItemChanged);
        on('dtxtpercentoftotal', 'input', percentChanged);
        on('dtxtfcyamount', 'input', ptFcyChanged);
        on('cmbCommissionType', 'change', commissionTypeChanged);
        on('txtCommRate', 'input', calculateCommission);
        on('txtCommRateUom', 'input', calculateCommission);
        on('txtCommRateUom', 'keypress', function () { COMM_TAG = val('txtCommRateUom'); });
        wire('detailBody', { del: detailDelete, dup: detailDuplicate, open: detailEdit, select: function (i) { CUR.detail = i; }, edit: function (i, c, v) { if (c === 'RateAddLess') rateAddLessEdited(i, v); } });
        wire('oiBody', { open: oiEdit });
        wire('ptBody', { ptdel: ptDelete, open: ptEdit });
        wire('cmBody', { cmdel: cmDelete, open: cmEdit });
        wire('ocBody', { ocdel: ocDelete, open: ocEdit });
        wire('histBody', { hbtn: historyButton, open: function (i) { var r = HIST[i]; if (r) readById(r.Id); }, select: historySelect });
        wire('ldrBody', { open: loaderSelect, select: loaderSelect });
        document.addEventListener('change', function (e) { if (e.target && e.target.id === 'ldrAll') { var c = e.target.checked; $id('ldrBody').querySelectorAll('input[data-ldr]').forEach(function (x) { x.checked = c; }); } });
        /* ImProformaInvoice_KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (!$id('loaderModal').classList.contains('is-hidden')) { if (e.key === 'Escape') loaderClose(); return; }
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && !e.target.closest('.win-grid')) {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); closeForm(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && e.altKey) { shortcuts(); return; }
            var onForm = activeTab('main') === 'tabPage1';
            if (onForm) {
                if (e.ctrlKey && k === 's' && !REC.updateMode) { e.preventDefault(); save($id('btnsave')); }
                else if (e.ctrlKey && k === 'u' && REC.updateMode) { e.preventDefault(); save($id('btnupdate')); }
                else if (e.ctrlKey && k === 'n') { e.preventDefault(); formNew($id('btnNew')); }
                else if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print521($id('btnPrint')); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
                else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); notPorted('Attachment'); }
                else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('txtDocDate'); }
                else if (e.ctrlKey && e.key === 'ArrowRight') {
                    e.preventDefault(); var t = activeTab('detail');
                    if (t === 'tabPage3') { tab('detail', 'tabPage4'); focus('CmbOtherItem'); } else if (t === 'tabPage4') { tab('detail', 'tabPage5'); focus('dcmbpaymentterm'); } else if (t === 'tabPage5') { tab('detail', 'tabPage3'); focus('combitem'); }
                } else if (e.ctrlKey && e.key === 'ArrowUp') {
                    e.preventDefault(); var t2 = activeTab('detail');
                    if (t2 === 'tabPage3') focus('combpcktype'); else if (t2 === 'tabPage4') focus('CmbOtherItem'); else if (t2 === 'tabPage5') focus('dcmbpaymentterm');
                } else if (e.ctrlKey && e.key === 'ArrowDown') {
                    e.preventDefault(); var t3 = activeTab('detail'), g = t3 === 'tabPage3' ? 'detailBody' : t3 === 'tabPage4' ? 'oiBody' : t3 === 'tabPage5' ? 'ptBody' : null;
                    var tr = g ? $id(g).querySelector('tr') : null; if (tr) tr.scrollIntoView();
                }
            } else {
                if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
                else if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
                else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus('FromDateHistory'); }
                else if (e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowRight')) { e.preventDefault(); var hr = $id('histBody').querySelector('tr'); if (hr) hr.scrollIntoView(); }
                else if (e.ctrlKey && e.key === 'Enter' && CUR.hist >= 0 && PERM.Update) { e.preventDefault(); var r = HIST[CUR.hist]; if (r) readById(r.Id); }
            }
        });
        load();
    });

    global.ExportCi = {
        formNew: formNew, refresh: refresh, save: save, update: update, notPorted: notPorted, print521: print521, print529: print529, print501B: print501B,
        loaderOpen: loaderOpen, loaderClose: loaderClose, loaderSearch: loaderSearch, loaderReset: loaderReset, loaderLoad: loaderLoad, shortcuts: shortcuts,
        detailAdd: detailAdd, detailUpdate: detailUpdate, detailCancel: detailCancel, oiAdd: oiAdd, oiUpdate: oiUpdate, oiCancel: oiCancel,
        ptAdd: ptAdd, ptUpdate: ptUpdate, ptCancel: ptCancel, cmAdd: cmAdd, cmUpdate: cmUpdate, cmCancel: cmCancel, ocAdd: ocAdd, ocUpdate: ocUpdate, ocCancel: ocCancel,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory
    };
}(window));
