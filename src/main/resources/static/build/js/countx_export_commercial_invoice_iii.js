/* ============================================================================================
 * countx_export_commercial_invoice_iii.js - frmCommercialInvoiceIII.cs (Architecture.WinApp.Export), screen 880
 * "Commercial Invoice III" (DocumentTypeId 204). Form | History. Each toolstrip button, TextChanged, Leave, grid
 * double-click, grid column button, grid CellUpdated and grid F1 list of the desktop form has its counterpart
 * here, with the desktop's messages and order; all data comes from /api/export/commercial-invoice-iii
 * (ExportCommercialInvoiceController -> ExportCommercialInvoiceIIIService -> the desktop's own procedures).
 *
 * Button contract: disabled + spinner while a request runs, duplicates ignored, re-enabled on success and failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/api/export/commercial-invoice-iii';

    // ------------------------------------------------------------------------------ helpers
    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;'); }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function commas(s) { var neg = s.charAt(0) === '-'; if (neg) s = s.substring(1); var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return (neg ? '-' : '') + p.join('.'); }
    /* .NET custom numeric formats: "#,##0.###" (0 -> "0"), "#,#.###" (0 -> ""), "#,##0.#####", "#,##0.####". */
    function fmtN(v, dec, zeroBlank) {
        var s = netD(v).toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        if (s === '-0') s = '0';
        if (zeroBlank && Number(s) === 0) return '';
        return commas(s);
    }
    function f3(v) { return fmtN(v, 3, false); }
    function f3b(v) { return fmtN(v, 3, true); }
    function f4(v) { return fmtN(v, 4, false); }
    function f5(v) { return fmtN(v, 5, false); }
    /* double.ToString("0,0"): whole number, thousands separators, at least two digits (0 -> "00"). */
    function f00(v) { var n = netD(v), r = Math.round(Math.abs(n)), s = String(r); if (s.length < 2) s = '0' + s; return (n < 0 && r !== 0 ? '-' : '') + commas(s); }
    function plain(v) { var n = netD(v); return String(Math.round(n * 1e10) / 1e10); }       /* double.ToString() */
    /* Math.Round(double): to even. */
    function rint(x) { var f = Math.floor(x), d = x - f; if (d > 0.5) return f + 1; if (d < 0.5) return f; return (f % 2 === 0) ? f : f + 1; }
    function fin(x) { return isFinite(x) ? x : 0; }
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
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function sum(rows, k) { var s = 0; (rows || []).forEach(function (r) { s += netD(r[k]); }); return s; }
    function copy(o) { var c = {}; for (var k in o) if (Object.prototype.hasOwnProperty.call(o, k)) c[k] = o[k]; return c; }
    function notPorted(name) { box(name + ' is not part of the web port of this screen.'); }
    /* FormHelper.ValidateControls texts */
    function reqCombo(id, f) { if (sel(id) === 0) { box(f + ' field is required'); focus(id); return false; } return true; }
    function reqText(id, f) { if (val(id).trim() === '') { box(f + ' field is required'); focus(id); return false; } return true; }
    function reqNum(id, f) { if (val(id).trim() === '' || netD(val(id)) === 0) { box(f + ' must be a non-zero number'); focus(id); return false; } return true; }

    // ------------------------------------------------------------------------------ state
    var PERM = { Save: true, Update: true, Print: true };
    var CFG = { rateAddLessOnCommercialInvoice: false, allItemsBindonExportContract: false, defaultDaysToLessFromHistoryFromDate: 0, itemSearchByCode: false };
    var F9 = false;
    var L = { cropYears: [], jobLots: [], packTypes: [], brands: [], pmItems: [], fis: [], subs: [], paymentTerms: [], deliveryTerms: [] };
    var REC = { id: 0, updateMode: false, attachments: '', customAttachments: '' };
    var DET = [], DETC = [], REM = [], REMC = [], PT = [], PTC = [], OC = [], OCC = [], OI = [];
    var IDX = { pt: -1, ptc: -1, oc: -1, occ: -1, oi: -1 };
    var CUR = { hist: -1 };
    var HIST = [];
    var LDR = { rows: [], header: [], setup: null };
    var CARRIER = [{ Id: 1, Name: 'By Sea' }, { Id: 2, Name: 'By Air' }, { Id: 3, Name: 'By Road' }];

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
    function bindInvoiceNos(rows) {
        var dl = $id('invoiceNoList'); if (!dl) return;
        dl.innerHTML = (rows || []).map(function (r) { return '<option value="' + esc(r.Name) + '"></option>'; }).join('');
    }
    /* OtherItemsBind: dtPMItem, value member ItemName or ItemCode by RadItemNameOIDetail */
    function oiBind() {
        var byName = $id('RadItemNameOIDetail').checked;
        bind('CmbItemOIDetail', L.pmItems, 'Id', byName ? 'ItemName' : 'ItemCode', byName ? ['ItemCode'] : ['ItemName']);
    }
    function bindAll(d) {
        if (d.exportCompanies) bind('CmbExportCompany', d.exportCompanies, 'Id', 'Name');
        if (d.invoiceNos) bindInvoiceNos(d.invoiceNos);
        if (d.customers) bind('CmbCustomer', d.customers, 'Id', 'CompanyName', ['PartyCode', 'CityName']);
        if (d.banks) {
            /* ImporterAndExportBankBind: "Foreign Country" -> importer, "Home Country" -> exporter */
            bind('CmbImporterBank', d.banks.filter(function (r) { return str(r.IsHomeland) === 'Foreign Country'; }), 'Id', 'BranchName', ['BankIBANNo']);
            bind('CmbExporterBank', d.banks.filter(function (r) { return str(r.IsHomeland) === 'Home Country'; }), 'Id', 'BranchName', ['BankIBANNo']);
        }
        if (d.ports) { bind('CmbLoadingPort', d.ports, 'Id', 'Name'); bind('CmbDestinationPort', d.ports, 'Id', 'Name'); }
        bind('CmbCareierType', CARRIER, 'Id', 'Name');
        if (d.deliveryTerms) { L.deliveryTerms = d.deliveryTerms; bind('CmbDeliveryTerm', d.deliveryTerms, 'Id', 'Code', ['Description']); }
        if (d.continents) bind('CmbContinent', d.continents, 'Id', 'Name');
        if (d.countries) { bind('CmbOriginCountry', d.countries, 'Id', 'Name'); bind('CmbImporterCountry', d.countries, 'Id', 'Name'); }
        if (d.cities) bind('CmbPlaceOfDelivery', d.cities, 'Id', 'Name');
        if (d.currencies) bind('CmbFcyCode', d.currencies, 'Id', 'Name');
        if (d.cropYears) L.cropYears = d.cropYears;
        if (d.jobLots) L.jobLots = d.jobLots;
        if (d.packTypes) L.packTypes = d.packTypes;
        if (d.brands) L.brands = d.brands;
        if (d.pmItems) { L.pmItems = d.pmItems; oiBind(); }
        if (d.paymentTerms) {
            L.paymentTerms = d.paymentTerms;
            bind('CmbPaymentTermPTDetail', d.paymentTerms, 'Id', 'lcOrderTerm', ['Description']);
            bind('CmbPaymentTermPTDetailCustom', d.paymentTerms, 'Id', 'lcOrderTerm', ['Description']);
        }
        if (d.exportCharges) { bind('CmbChargesNameChargeDetail', d.exportCharges, 'Id', 'Name'); bind('CmbChargesNameChargeDetailCustom', d.exportCharges, 'Id', 'Name'); }
        Object.keys(d).forEach(function (k) { if (/Error$/.test(k) && d[k]) box(d[k]); });
    }
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            if (d.config) CFG = d.config;
            F9 = !!d.allowExportMultiCompanies;
            applyRights();
            /* ItemSearchWithNameOrCode: both branches check RadItemCode... */
            $id('RadItemCodeOIDetail').checked = true;
            if (REC.id === 0 && netI(d.docNo) > 0) setText('txtdocno', d.docNo);
            show('exportCompanyBox', F9);
            bindAll(d);
            if (d.historyCombos) bindHistoryCombos(d.historyCombos);
            var days = netI(CFG.defaultDaysToLessFromHistoryFromDate);
            setText('FromDateHistory', days > 0 ? daysAgo(days) : daysAgo(3));
            setText('ToDateHistory', today());
            setText('txtDocDate', today()); setText('txtDocumentaryCreditNoDate', today()); setText('txtGdDate', today());
            bindGrids();
            tab('inv', 'tabItemDetail');
            $id('ChkPrintPreview').checked = true;
            focus('txtDocDate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    // ============================================================================== calculations
    /* UpdateAddLessAmountInHeaderFromChargesGrid (+ the add-less box TextChanged -> UpdateHeaderWeightAndAmountFromDetailGrid) */
    function addLessFromCharges(c) {
        var rows = c ? OCC : OC;
        setText(c ? 'txtAddLessHeaderCustom' : 'txtaddlessamount', f3b(sum(rows, 'AddAmount') - sum(rows, 'LessAmount')));
        updateHeader(c);
    }
    /* UpdateHeaderWeightAndAmountFromDetailGrid(isCustom) */
    function updateHeader(c) {
        var rows = c ? DETC : DET;
        var mton = sum(rows, 'QtyMTon'), gross = sum(rows, 'GrossWeight'), amount = sum(rows, 'Amount'), other = sum(OI, 'Amount');
        var addless = netD(val(c ? 'txtAddLessHeaderCustom' : 'txtaddlessamount'));
        var net = amount + addless + (c ? 0 : other);
        if (c) {
            setText('txtFcyAmountHeaderCustom', f3(amount));
            setText('txtTotalFcyAmountHeaderCustom', f3b(net));
        } else {
            setText('txtNetWeight', f5(mton));
            setText('txtGrossWeight', gross > 0 ? f5(gross / 1000.0) : '0');
            setText('txtFcyAmount', f3(amount));
            setText('txtTotalNetAmount', f3b(net));
        }
        /* UpdatePaymentTerms */
        if (net > 0) (c ? PTC : PT).forEach(function (r) { r.FcyAmount = netD(r.PrcntOfTotal) * net / 100.0; });
        calculateLocal(c);
        renderPt(c);
    }
    /* CalculateLocalAmount(isCustom): ToString("0,0") */
    function calculateLocal(c) {
        var rate = netD(val('txtExRate')), fcy = netD(val(c ? 'txtTotalFcyAmountHeaderCustom' : 'txtTotalNetAmount'));
        setText(c ? 'txtLocalAmountHeaderCustom' : 'txtLocalAmt', f00(fcy > 0 && rate > 0 ? fcy * rate : 0));
    }
    /* txtExRate_TextChanged */
    function exRateChanged() { calculateLocal(false); calculateLocal(true); }
    /* ProportionateOtherItemAmount */
    function proportionateOther() {
        var contract = sum(DET, 'Amount'), other = sum(OI, 'Amount');
        DET.forEach(function (r) { r.OtherItemAmount = other > 0 ? other / contract * netD(r.Amount) : 0; });
        renderDet(false);
    }
    /* BindGrids: the grids, then the invoice and the custom header values */
    function bindGrids() {
        renderAll();
        addLessFromCharges(false);
        updateHeader(false);
        proportionateOther();
        addLessFromCharges(true);
    }

    // ============================================================================== item detail grids (grdDetail / grdDetailCustom)
    /* dtdetail columns as the grid shows them (grdSettings / grdSettingsDetailCustom hide the id columns). */
    var DCOLS = [
        { k: 'ContractNo', h: 'ContractNo' }, { k: 'ContractDate', h: 'ContractDate', d: true }, { k: 'ItemCategory', h: 'ItemCategory' }, { k: 'ItemCode', h: 'ItemCode' },
        { k: 'ItemName', h: 'ItemName' }, { k: 'CropYearId', h: 'CropYearId', lk: 'cropYears', f1: 'crop' }, { k: 'JobLotId', h: 'JobLotId', lk: 'jobLots', f1: 'job' },
        { k: 'PackingTypeId', h: 'PackingTypeId', lk: 'packTypes', f1: 'pack' }, { k: 'PmItemCode', h: 'PmItemCode', f1: 'pm' }, { k: 'PmItemName', h: 'PmItemName', f1: 'pm' },
        { k: 'BrandCode', h: 'BrandCode', f1: 'brand' }, { k: 'BrandName', h: 'BrandName', f1: 'brand' }, { k: 'QtyMTon', h: 'Qty/M.Ton', n: 3, s: true, edit: true },
        { k: 'OuterUom', h: 'OuterUom', f1: 'outer' }, { k: 'OuterQty', h: 'OuterQty', n: 2, s: true, edit: true }, { k: 'InnerUom', h: 'InnerUom', f1: 'inner' },
        { k: 'InnerQty', h: 'InnerQty', n: 2, s: true, edit: true }, { k: 'PalletQty', h: 'PalletQty', n: 2, s: true, edit: true }, { k: 'GrossWeight', h: 'GrossWeight', n: 2, s: true, edit: true },
        { k: 'RateWithoutAddLess', h: 'RateWithoutAddLess', n: 4, ral: true }, { k: 'RateAddLess', h: 'RateAddLess', n: 4, ral: true, editRal: true }, { k: 'NetRate', h: 'NetRate', n: 4 },
        { k: 'ContractRate', h: 'ContractRate', n: 4 }, { k: 'RateUOM', h: 'RateUOM' }, { k: 'Amount', h: 'Amount', n: 3, s: true }, { k: 'LabInspectionId', h: 'LabInspectionId', f1: 'lab' },
        { k: 'BuyerLotNo', h: 'BuyerLotNo', edit: true, txt: true }, { k: 'ProductCode', h: 'ProductCode', edit: true, txt: true }, { k: 'BuyerHSCode', h: 'BuyerHSCode', edit: true, txt: true },
        { k: 'HSCode', h: 'HSCode', edit: true, txt: true }, { k: 'CommodityDetail', h: 'CommodityDetail', edit: true, txt: true, f1: 'commodity' },
        { k: 'PackingDetail', h: 'PackingDetail', edit: true, txt: true, f1: 'packing' }, { k: 'Remarks', h: 'Remarks', edit: true, txt: true },
        { k: 'OtherItemAmount', h: 'OtherItemAmount', n: 3, s: true }, { k: 'ScheduleFCL', h: 'ScheduleFCL', n: 2 }];
    var NUM_EDIT = { QtyMTon: 1, OuterQty: 1, InnerQty: 1, PalletQty: 1, GrossWeight: 1, RateAddLess: 1 };
    /* grdDetailCommonSetting: RateWithoutAddLess / RateAddLess visible by RateAddLessOnCommercialInvoice; always on the custom grid. */
    function detCols(c) { return DCOLS.filter(function (x) { return !x.ral || c || CFG.rateAddLessOnCommercialInvoice; }); }
    function editable(c, x) { if (x.editRal) return c || CFG.rateAddLessOnCommercialInvoice; return !!x.edit; }
    function lookupName(list, id) { var r = findRow(L[list], id); return r ? str(r.Name) : (netI(id) === 0 ? '' : str(id)); }
    function cellText(x, r) {
        var v = r[x.k];
        if (x.lk) return lookupName(x.lk, v);
        if (x.d) return shortDate(v);
        if (x.n !== undefined) return fmtN(v, x.n, false);
        return str(v);
    }
    function renderDet(c) {
        var rows = c ? DETC : DET, p = c ? 'detailCustom' : 'detail', cols = detCols(c);
        var ae = document.activeElement, keep = ae && ae.closest && ae.closest('#' + p + 'Body') && ae.getAttribute('data-i') !== null ? { i: ae.getAttribute('data-i'), c: ae.getAttribute('data-edit-col'), f: ae.getAttribute('data-f1') } : null;
        $id(p + 'Head').innerHTML = '<tr><th class="ctr">X</th>' + cols.map(function (x) { return '<th' + (x.n !== undefined ? ' class="num"' : '') + (x.f1 ? ' title="F1: list"' : '') + '>' + esc(x.h) + (x.f1 ? ' <small>(F1)</small>' : '') + '</th>'; }).join('') + '</tr>';
        $id(p + 'Body').innerHTML = rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"><td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
            cols.forEach(function (x) {
                var f1 = x.f1 ? ' data-f1="' + x.f1 + '" data-i="' + i + '"' : '';
                var btn = x.f1 ? '<button type="button" class="ex-f1-btn" data-f1="' + x.f1 + '" data-i="' + i + '" title="List (F1)">&hellip;</button>' : '';
                if (editable(c, x)) {
                    h += '<td class="win-editable' + (x.f1 ? ' ex-f1' : '') + '"><span style="display:flex;"><input type="text" class="' + (x.txt ? 'txt' : 'num') + '" data-edit-col="' + x.k + '" data-i="' + i + '"' + f1 + ' value="' + esc(str(r[x.k])) + '"/>' + btn + '</span></td>';
                } else if (x.f1) {
                    h += '<td class="ex-f1' + (x.n !== undefined ? ' num' : '') + '" tabindex="0"' + f1 + '>' + esc(cellText(x, r)) + btn + '</td>';
                } else {
                    h += '<td' + (x.n !== undefined ? ' class="num"' : '') + '>' + esc(cellText(x, r)) + '</td>';
                }
            });
            return h + '</tr>';
        }).join('');
        if (keep) {
            var sel2 = keep.c ? 'input[data-edit-col="' + keep.c + '"][data-i="' + keep.i + '"]' : 'td[data-f1="' + keep.f + '"][data-i="' + keep.i + '"]';
            var t = $id(p + 'Body').querySelector(sel2); if (t) try { t.focus(); } catch (x) { /* ignore */ }
        }
        if (!rows.length) { $id(p + 'Foot').innerHTML = ''; return; }
        $id(p + 'Foot').innerHTML = '<tr><td class="lbl">&Sigma;</td>' + cols.map(function (x) { return x.s ? '<td>' + esc(fmtN(sum(rows, x.k), x.n, false)) + '</td>' : '<td class="lbl"></td>'; }).join('') + '</tr>';
    }
    /* grdDetail_CellUpdated / grdDetailCustom_CellUpdated */
    function cellEdit(c, i, key, v) {
        var rows = c ? DETC : DET, r = rows[i]; if (!r) return;
        r[key] = NUM_EDIT[key] ? netD(v) : v;
        var q = netD(r.QtyMTon), oe = netD(r.OuterEquivalent), re = netD(r.RateEquivalent), nr = netD(r.NetRate), ie = netD(r.InnerEquivalent);
        if (key === 'QtyMTon' || key === 'RateAddLess') {
            if (key === 'QtyMTon') {
                r.OuterQty = oe > 0 ? q * 1000.0 / oe : 0;
                if (!c) r.InnerQty = rint(q > 0 && ie > 0 ? q * 1000.0 / ie : 0);
            } else { nr = netD(r.RateWithoutAddLess) + netD(r.RateAddLess); r.NetRate = nr; }
            r.Amount = fin(nr / re * (q * 1000.0));
        } else if (key === 'OuterQty') {
            q = oe > 0 ? netD(r.OuterQty) * oe / 1000.0 : 0;
            r.QtyMTon = q;
            if (!c) r.InnerQty = rint(q > 0 && ie > 0 ? q * 1000.0 / ie : 0);
            r.Amount = fin(nr / re * (q * 1000.0));
        }
        updateHeader(c);
        calculateLocal(c);
        if (!c) proportionateOther(); else renderDet(true);
    }
    /* DeleteDetailRow / DeleteDetailCustomRow */
    function detDelete(c, i) {
        var rows = c ? DETC : DET, r = rows[i]; if (!r) return;
        if (netI(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            (c ? REMC : REM).push(copy(r));
        }
        rows.splice(i, 1);
        updateHeader(c);
        if (!c) proportionateOther(); else renderDet(true);
    }

    // ------------------------------------------------------------------------------ GrdPopUp (F1 lists)
    var PICK = { rows: [], idField: 'Id', disp: 'Name', resolve: null };
    function pickerOpen(rows, idField, disp, title) {
        return new Promise(function (resolve) {
            PICK = { rows: rows || [], idField: idField, disp: disp, resolve: resolve };
            $id('pickerTitle').textContent = title || 'Select';
            $id('pickerCol').textContent = disp;
            setText('pickerSearch', '');
            pickerDraw();
            show('pickerModal', true);
            setTimeout(function () { focus('pickerSearch'); }, 0);
        });
    }
    function pickerDraw() {
        var q = val('pickerSearch').toLowerCase();
        $id('pickerBody').innerHTML = PICK.rows.map(function (r, i) {
            var t = str(col(r, PICK.disp));
            if (q && t.toLowerCase().indexOf(q) < 0 && str(col(r, PICK.idField)).indexOf(q) < 0) return '';
            return '<tr data-pi="' + i + '" tabindex="0"><td class="num">' + esc(col(r, PICK.idField)) + '</td><td>' + esc(t) + '</td></tr>';
        }).join('');
    }
    /* closing the popup without a pick returns ReturnId 0 / ReturnName "" - the desktop writes those into the cell */
    function pickerDone(i) {
        var r = i === null ? null : PICK.rows[i], res = PICK.resolve;
        show('pickerModal', false); PICK.resolve = null;
        if (res) res(r ? { id: netI(col(r, PICK.idField)), name: str(col(r, PICK.disp)), row: r } : { id: 0, name: '', row: null });
    }
    function pickerClose() { pickerDone(null); }

    /* grdDetail_KeyDown / grdDetailCustom_KeyDown F1 */
    function f1(c, i, kind) {
        var rows = c ? DETC : DET, r = rows[i]; if (!r) return;
        var redraw = function () { renderDet(c); };
        var needItem = function () { if (netI(r.ItemId) <= 0) { box('Please Select an Item First'); return false; } return true; };
        /* ShowPopupAndSetValue */
        var simple = function (list, key, title) { pickerOpen(L[list], 'Id', 'Name', title).then(function (p) { r[key] = p.id; redraw(); }); };
        /* HandleItemSelection */
        var item = function (list, prefix, title) {
            var nameCol = prefix === 'PmItem' ? 'ItemName' : 'BrandName', codeCol = prefix === 'PmItem' ? 'ItemCode' : 'BrandCode';
            pickerOpen(L[list], 'Id', nameCol, title).then(function (p) {
                r[prefix + 'Id'] = p.id; r[prefix + 'Name'] = p.name;
                if (p.id > 0) r[prefix + 'Code'] = p.row ? str(p.row[codeCol]) : '';
                redraw();
            });
        };
        /* HandleUomSelection */
        var uom = function (prefix) {
            if (!needItem()) return;
            getJson(API + '/uoms?itemId=' + netI(r.ItemId)).then(function (list) {
                list = list || [];
                return pickerOpen(list, 'Id', 'UomCode', 'Uom').then(function (p) {
                    r[prefix + 'UomId'] = p.id; r[prefix + 'Uom'] = p.name;
                    if (p.id > 0) {
                        var found = findRow(list, p.id), eq = found ? netD(found.Equivalent) : 0, q = netD(r.QtyMTon);
                        r[prefix + 'Equivalent'] = eq;
                        r[prefix + 'Qty'] = eq > 0 ? q * 1000.0 / eq : 0;
                        if (prefix === 'Outer') r.Amount = fin(netD(r.NetRate) / netD(r.RateEquivalent) * q * 1000.0);
                    }
                    redraw();
                });
            }).catch(function (e) { box(e.message); });
        };
        /* HandleCommodityOrPackingDetail */
        var detail = function (typeId, key) {
            if (!needItem()) return;
            getJson(API + '/commodity?itemId=' + netI(r.ItemId)).then(function (list) {
                var rowsF = (list || []).filter(function (x) { return netI(x.DetailTypeId) === typeId; });
                if (!rowsF.length) { box('No Record Found'); return; }
                return pickerOpen(rowsF, 'Id', 'Remarks', key).then(function (p) { r[key] = p.name; redraw(); });
            }).catch(function (e) { box(e.message); });
        };
        if (kind === 'crop') simple('cropYears', 'CropYearId', 'Crop Year');
        else if (kind === 'job') simple('jobLots', 'JobLotId', 'Job Lot');
        else if (kind === 'pack') simple('packTypes', 'PackingTypeId', 'Packing Type');
        else if (kind === 'pm') item('pmItems', 'PmItem', 'Pm Item');
        else if (kind === 'brand') item('brands', 'Brand', 'Brand');
        else if (kind === 'outer') uom('Outer');
        else if (kind === 'inner') uom('Inner');
        else if (kind === 'commodity') detail(1, 'CommodityDetail');
        else if (kind === 'packing') detail(2, 'PackingDetail');
        else if (kind === 'lab') {
            if (!needItem()) return;
            getJson(API + '/third-party?itemId=' + netI(r.ItemId)).then(function (list) {
                return pickerOpen(list || [], 'Id', 'InspectionNo', 'Inspection No').then(function (p) { r.LabInspectionId = p.id; redraw(); });
            }).catch(function (e) { box(e.message); });
        }
    }

    // ============================================================================== payment detail (invoice / custom)
    function ptx(c) {
        return c ? { term: 'CmbPaymentTermPTDetailCustom', fi: 'CmbFinInstrumentsPTDetailCustom', bal: 'txtBalanceFIPTDetailCustom', pct: 'txtPrcntOfTotalPTDetailCustom', fcy: 'txtFcyAmountPTDetailCustom',
            due: 'txtDueDaysPTDetailCustom', rem: 'txtRemarksPTDetailCustom', add: 'BtnAddPTDetailCustom', upd: 'BtnUpdatePTDetailCustom', can: 'BtnCancelPTDetailCustom', body: 'ptcBody', foot: 'ptcFoot', idx: 'ptc' }
            : { term: 'CmbPaymentTermPTDetail', fi: 'CmbFinInstrumentsPTDetail', bal: 'txtBalanceFIPTDetail', pct: 'txtPrcntOfTotalPTDetail', fcy: 'txtFcyAmountPTDetail',
            due: 'txtDueDaysPTDetail', rem: 'txtRemarksPTDetail', add: 'BtnAddPTDetail', upd: 'BtnUpdatePTDetail', can: 'BtnCancelPTDetail', body: 'ptBody', foot: 'ptFoot', idx: 'pt' };
    }
    function ptRows(c) { return c ? PTC : PT; }
    function renderPt(c) {
        var x = ptx(c), rows = ptRows(c);
        $id(x.body).innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="win-cell-btn"><button type="button" class="win-x" data-ptdel="' + i + '">X</button></td><td><a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(r.PaymentTerm) + '</a></td><td>' + esc(r.FinancialInstrumentNo) +
                '</td><td class="num">' + esc(fmtN(r.PrcntOfTotal, 2, false)) + '</td><td class="num">' + esc(fmtN(r.FcyAmount, 3, false)) + '</td><td class="num">' + esc(str(r.DueDays)) + '</td><td>' + esc(r.Remarks) + '</td></tr>';
        }).join('');
        $id(x.foot).innerHTML = rows.length ? '<tr><td class="lbl">&Sigma;</td><td class="lbl"></td><td class="lbl"></td><td>' + esc(fmtN(sum(rows, 'PrcntOfTotal'), 2, false)) + '</td><td>' + esc(fmtN(sum(rows, 'FcyAmount'), 3, false)) + '</td><td class="lbl"></td><td class="lbl"></td></tr>' : '';
    }
    /* PaymentDetailFormValidation / PaymentDetailCustomFormValidation */
    function ptValidation(c) {
        var x = ptx(c);
        if (!reqCombo(x.term, 'Payment Term')) return false;
        if (sel(x.term) === 1 && selText(x.fi) === '') { box('Financial Instrument field is required'); focus(x.fi); return false; }
        if (!reqNum(x.pct, 'Percent of Total')) return false;
        if (!reqNum(x.fcy, 'FCY Amount')) return false;
        return true;
    }
    /* IsDuplicateFinancialInstrument(Custom) */
    function ptDuplicate(c, updateMode) {
        var x = ptx(c), cur = sel(x.fi); if (cur <= 0) return false;
        var fi = findRow(L.fis, cur), docType = fi ? netI(fi.DocumnetTypeId) : 0;
        return ptRows(c).some(function (r, i) { return !(updateMode && i === IDX[x.idx]) && netI(r.ExImEFormRegistrationId) === cur && netI(r.DocumentTypeId) === docType; });
    }
    function ptAdd(c) {
        var x = ptx(c);
        if (!ptValidation(c)) return;
        if (ptDuplicate(c, false)) { box('Financial Instrument No already added in the grid. Please check.'); return; }
        var fiId = sel(x.fi), fi = findRow(L.fis, fiId);
        ptRows(c).push({ Id: 0, PaymentTermId: sel(x.term), PaymentTerm: selText(x.term), DocumentTypeId: fiId > 0 && fi ? netI(fi.DocumnetTypeId) : 0, ExImEFormRegistrationId: fiId,
            FinancialInstrumentNo: fiId > 0 ? selText(x.fi) : '', PrcntOfTotal: netD(val(x.pct)), FcyAmount: netD(val(x.fcy)), DueDays: netD(val(x.due)), Remarks: val(x.rem).trim() });
        renderPt(c); ptReset(c);
    }
    function ptEdit(c, i) {
        var x = ptx(c), r = ptRows(c)[i]; if (!r) return;
        IDX[x.idx] = i;
        setVal(x.term, r.PaymentTermId); setVal(x.fi, r.ExImEFormRegistrationId);
        setText(x.pct, str(r.PrcntOfTotal)); setText(x.fcy, str(r.FcyAmount)); setText(x.due, str(r.DueDays)); setText(x.rem, str(r.Remarks));
        show(x.add, false); show(x.upd, true); show(x.can, true); focus(x.term);
    }
    /* btndupdate_Click: a duplicate FI throws into an empty catch - nothing happens */
    function ptUpdate(c) {
        var x = ptx(c);
        if (!ptValidation(c)) return;
        if (ptDuplicate(c, true)) return;
        var r = ptRows(c)[IDX[x.idx]]; if (!r) return;
        var fiId = sel(x.fi), fi = findRow(L.fis, fiId);
        r.PaymentTermId = sel(x.term); r.PaymentTerm = selText(x.term);
        r.DocumentTypeId = fiId > 0 && fi ? netI(fi.DocumnetTypeId) : 0;
        r.ExImEFormRegistrationId = fiId; r.FinancialInstrumentNo = fiId > 0 ? selText(x.fi) : '';
        r.PrcntOfTotal = netD(val(x.pct)); r.FcyAmount = netD(val(x.fcy)); r.DueDays = val(x.due); r.Remarks = val(x.rem);
        renderPt(c); ptReset(c);
    }
    function ptCancel(c) { ptReset(c); }
    /* ResetPaymentDetail / ResetPaymentDetailCustom */
    function ptReset(c) {
        var x = ptx(c); IDX[x.idx] = -1;
        setVal(x.term, '0'); setVal(x.fi, '0'); setText(x.pct, ''); setText(x.fcy, ''); setText(x.due, ''); setText(x.rem, '');
        show(x.add, true); show(x.upd, false); show(x.can, false); focus(x.term);
    }
    /* DeletePTDetailRow: "Reset Detail First..." is thrown into an empty catch - nothing happens while a row is in edit */
    function ptDelete(c, i) { if (IDX[ptx(c).idx] !== -1) return; ptRows(c).splice(i, 1); renderPt(c); }
    /* PaymentDetailPercentCalculate / PaymentDetailCustomPercentCalculate */
    function percentChanged(c) {
        var x = ptx(c), total = netD(val(c ? 'txtTotalFcyAmountHeaderCustom' : 'txtTotalNetAmount')), p = netD(val(x.pct));
        if (total > 0 && p > 0) {
            if (p > 100) { setText(x.pct, '0'); return; }                 /* the exception goes to an empty catch */
            setText(x.fcy, f3b(total * p / 100.0));
        }
    }
    /* dtxtfcyamount_TextChanged (the custom Fcy box is disabled and its handler is empty) */
    function ptFcyChanged() {
        var total = netD(val('txtTotalNetAmount')), f = netD(val('txtFcyAmountPTDetail'));
        if (total > 0 && f > 0) {
            var p = f / total * 100.0;
            setText('txtPrcntOfTotalPTDetail', plain(p));
            if (p > 100) setText('txtPrcntOfTotalPTDetail', '0');
        }
    }
    /* CmbPaymentTermDetail_TextChanged / ...Custom */
    function paymentTermChanged(c) {
        var x = ptx(c);
        if (sel(x.term) !== 0) { var r = findRow(L.paymentTerms, sel(x.term)); if (r && str(r.Description) !== '') setText(x.rem, r.Description); }
    }
    /* dcmbfino_ValueChanged / CmbFinInstrumentsPTDetailCustom_ValueChanged */
    function fiChanged(c) {
        var x = ptx(c), fi = findRow(L.fis, sel(x.fi));
        if (fi && sel(x.fi) > 0) {
            setVal(x.term, fi.PaymenttermId); paymentTermChanged(c);
            fiBalance(c);
        }
    }
    /* FinancialInstrumentsBalanceBindWithDbCall: ToString("0,0") */
    function fiBalance(c) {
        var x = ptx(c), fi = findRow(L.fis, sel(x.fi)); if (!fi) return;
        getJson(API + '/fi-balance?id=' + netI(fi.Id) + '&documentTypeId=' + netI(fi.DocumnetTypeId)).then(function (d) { setText(x.bal, f00(d && d.balance)); }).catch(function () { /* empty catch */ });
    }

    // ============================================================================== other charges (invoice / custom)
    function ocx(c) {
        return c ? { item: 'CmbChargesNameChargeDetailCustom', add: 'txtAddAmountChargeDetailCustom', less: 'txtLessAmountChargeDetailCustom', rem: 'txtRemarksChargeDetailCustom',
            bAdd: 'BtnAddChargeDetailCustom', bUpd: 'BtnUpdateChargeDetailCustom', bCan: 'BtnCancelChargeDetailCustom', body: 'occBody', foot: 'occFoot', idx: 'occ' }
            : { item: 'CmbChargesNameChargeDetail', add: 'txtAddAmountChargeDetail', less: 'txtLessAmountChargeDetail', rem: 'txtRemarksChargeDetail',
            bAdd: 'BtnAddChargeDetail', bUpd: 'BtnUpdateChargeDetail', bCan: 'BtnCancelChargeDetail', body: 'ocBody', foot: 'ocFoot', idx: 'oc' };
    }
    function ocRows(c) { return c ? OCC : OC; }
    function renderOc(c) {
        var x = ocx(c), rows = ocRows(c);
        $id(x.body).innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="win-cell-btn"><button type="button" class="win-x" data-ocdel="' + i + '">X</button></td><td><a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(r.ChargesItem) + '</a></td><td class="num">' +
                esc(fmtN(r.AddAmount, 3, false)) + '</td><td class="num">' + esc(fmtN(r.LessAmount, 3, false)) + '</td><td>' + esc(r.Remarks) + '</td></tr>';
        }).join('');
        $id(x.foot).innerHTML = rows.length ? '<tr><td class="lbl">&Sigma;</td><td class="lbl"></td><td>' + esc(fmtN(sum(rows, 'AddAmount'), 3, false)) + '</td><td>' + esc(fmtN(sum(rows, 'LessAmount'), 3, false)) + '</td><td class="lbl"></td></tr>' : '';
    }
    /* OtherChargesFormValidation / OtherChargesCustomFormValidation */
    function ocValidation(c) {
        var x = ocx(c);
        if (sel(x.item) === 0) { box('Charges Item field is required'); focus(x.item); return false; }
        if (netD(val(x.less)) === 0 && netD(val(x.add)) === 0) { box('Add Or Less Amount is required'); focus(x.less); return false; }
        if (netD(val(x.less)) > 0 && netD(val(x.add)) > 0) { box("You Can Only Enter 'Add Or Less Amount'"); focus(x.less); return false; }
        return true;
    }
    function ocAdd(c) {
        var x = ocx(c);
        if (!ocValidation(c)) return;
        var r = { Id: 0 };
        if (!c) { r.ContractId = 0; r.ContractOtherChargesDetailId = 0; }
        r.ChargesItemId = sel(x.item); r.ChargesItem = selText(x.item); r.AddAmount = netD(val(x.add)); r.LessAmount = netD(val(x.less)); r.Remarks = val(x.rem).trim();
        ocRows(c).push(r);
        renderOc(c); ocReset(c); addLessFromCharges(c);
    }
    function ocEdit(c, i) {
        var x = ocx(c), r = ocRows(c)[i]; if (!r) return;
        IDX[x.idx] = i;
        setVal(x.item, r.ChargesItemId); setText(x.add, str(r.AddAmount)); setText(x.less, str(r.LessAmount)); setText(x.rem, str(r.Remarks));
        show(x.bAdd, false); show(x.bUpd, true); show(x.bCan, true); focus(x.item);
    }
    function ocUpdate(c) {
        var x = ocx(c);
        if (!ocValidation(c)) return;
        var r = ocRows(c)[IDX[x.idx]]; if (!r) return;
        r.ChargesItemId = sel(x.item); r.ChargesItem = selText(x.item); r.AddAmount = netD(val(x.add)); r.LessAmount = netD(val(x.less)); r.Remarks = val(x.rem);
        renderOc(c); ocReset(c); addLessFromCharges(c);
    }
    function ocCancel(c) { ocReset(c); }
    function ocReset(c) {
        var x = ocx(c); IDX[x.idx] = -1;
        setVal(x.item, '0'); setText(x.add, ''); setText(x.less, ''); setText(x.rem, '');
        show(x.bAdd, true); show(x.bUpd, false); show(x.bCan, false); focus(x.item);
    }
    function ocDelete(c, i) { if (IDX[ocx(c).idx] !== -1) return; ocRows(c).splice(i, 1); renderOc(c); addLessFromCharges(c); }

    // ============================================================================== other items
    function renderOi() {
        $id('oiBody').innerHTML = OI.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="win-cell-btn"><button type="button" class="win-x" data-oidel="' + i + '">X</button></td><td><a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(r.ItemName) + '</a></td><td class="num">' +
                esc(fmtN(r.Qty, 2, false)) + '</td><td class="num">' + esc(fmtN(r.Rate, 4, false)) + '</td><td class="num">' + esc(fmtN(r.Amount, 3, false)) + '</td><td>' + esc(r.Remarks) + '</td></tr>';
        }).join('');
        $id('oiFoot').innerHTML = OI.length ? '<tr><td class="lbl">&Sigma;</td><td class="lbl"></td><td>' + esc(fmtN(sum(OI, 'Qty'), 2, false)) + '</td><td class="lbl"></td><td>' + esc(fmtN(sum(OI, 'Amount'), 3, false)) + '</td><td class="lbl"></td></tr>' : '';
    }
    /* OtherItemsCalculations */
    function oiCalc() { var q = netD(val('txtQtyOIDetail')), r = netD(val('txtRateOIDetail')); setText('txtAmountOIDetail', q > 0 && r > 0 ? plain(q * r) : '0'); }
    /* OtherItemFormValidation */
    function oiValidation() { return reqCombo('CmbItemOIDetail', 'Item') && reqNum('txtQtyOIDetail', 'Qty') && reqNum('txtRateOIDetail', 'Rate'); }
    function oiReset() {
        IDX.oi = -1; setVal('CmbItemOIDetail', '0'); setText('txtQtyOIDetail', ''); setText('txtRateOIDetail', ''); setText('txtAmountOIDetail', ''); setText('txtRemarksOIDetail', '');
        show('BtnAddOIDetail', true); show('BtnUpdateOIDetail', false); show('BtnCancelOIDetail', false); focus('CmbItemOIDetail');
    }
    function oiAdd() {
        if (!oiValidation()) return;
        OI.push({ ItemId: sel('CmbItemOIDetail'), ItemName: selText('CmbItemOIDetail'), Qty: netD(val('txtQtyOIDetail')), Rate: netD(val('txtRateOIDetail')), Amount: netD(val('txtAmountOIDetail')), Remarks: val('txtRemarksOIDetail').trim() });
        renderOi(); oiReset(); updateHeader(false); proportionateOther();
    }
    function oiEdit(i) {
        var r = OI[i]; if (!r) return; IDX.oi = i;
        setVal('CmbItemOIDetail', r.ItemId); setText('txtQtyOIDetail', str(r.Qty)); setText('txtRateOIDetail', str(r.Rate)); setText('txtAmountOIDetail', str(r.Amount)); setText('txtRemarksOIDetail', str(r.Remarks));
        show('BtnAddOIDetail', false); show('BtnUpdateOIDetail', true); show('BtnCancelOIDetail', true); focus('CmbItemOIDetail');
    }
    function oiUpdate() {
        if (!oiValidation()) return;
        var r = OI[IDX.oi]; if (!r) return;
        r.ItemId = sel('CmbItemOIDetail'); r.ItemName = selText('CmbItemOIDetail'); r.Qty = netD(val('txtQtyOIDetail')); r.Rate = netD(val('txtRateOIDetail')); r.Amount = netD(val('txtAmountOIDetail')); r.Remarks = val('txtRemarksOIDetail');
        renderOi(); oiReset(); updateHeader(false); proportionateOther();
    }
    function oiCancel() { oiReset(); }
    /* DeleteOIDetailRow: no recalculation of the header (as the desktop) */
    function oiDelete(i) { if (IDX.oi !== -1) return; OI.splice(i, 1); renderOi(); }
    /* CmbOtherItem_TextChanged: in update mode the rate box takes SelectedRow.Cells[2] - the ItemCode column (sic). */
    function otherItemChanged() {
        if (!(shown('btnupdate') && !$id('btnupdate').disabled)) return;
        var r = findRow(L.pmItems, sel('CmbItemOIDetail'));
        if (r) { setText('txtRateOIDetail', str(r.ItemCode)); oiCalc(); }
    }

    function renderAll() { renderDet(false); renderDet(true); renderPt(false); renderPt(true); renderOc(false); renderOc(true); renderOi(); }

    // ============================================================================== customer / form
    /* cmbSupCust_Leave: FinancialInstrumentDtFillDbCall + FinancialInstrumentBind (+ balance) + ConsigneeAgainstCustomerBind */
    function customerLeave() {
        return getJson(API + '/customer-leave?customerId=' + sel('CmbCustomer')).then(applyCustomer).catch(function () { /* empty catch */ });
    }
    function applyCustomer(d) {
        d = d || {};
        L.fis = d.fis || [];
        bind('CmbFinInstrumentsPTDetail', L.fis, 'Id', 'EFormNo');
        bind('CmbFinInstrumentsPTDetailCustom', L.fis, 'Id', 'EFormNo');
        if (sel('CmbFinInstrumentsPTDetail') > 0) fiBalance(false);
        L.subs = d.subParties || [];
        ['CmbConsignee', 'CmbNotifyParty1', 'CmbNotifyParty2', 'CmbNotifyParty3'].forEach(function (id) { bind(id, L.subs, 'Id', 'CompanyName', ['PartyCode']); });
    }
    /* CmbDeliveryTerm_TextChanged */
    function deliveryTermChanged() { if (sel('CmbDeliveryTerm') !== 0) { var r = findRow(L.deliveryTerms, sel('CmbDeliveryTerm')); if (r && str(r.Description) !== '') setText('txtDeliveryRemarks', r.Description); } }

    /* FormReset */
    function formReset(skipServer) {
        REM = []; REMC = [];
        REC = { id: 0, updateMode: false, attachments: '', customAttachments: '' };
        show('btnsave', true); show('btnupdate', false);
        setText('cmbInvoiceNo', ''); setText('txtDocumentaryCreditNo', '');
        setVal('CmbCustomer', '0');
        applyCustomer({ fis: [], subParties: [] });                     /* cmbSupCust_Leave with no customer */
        ['CmbConsignee', 'CmbNotifyParty1', 'CmbNotifyParty2', 'CmbNotifyParty3', 'CmbImporterBank', 'CmbExporterBank', 'CmbLoadingPort', 'CmbDestinationPort', 'CmbDeliveryTerm',
            'CmbContinent', 'CmbOriginCountry', 'CmbImporterCountry', 'CmbPlaceOfDelivery', 'CmbFcyCode'].forEach(function (id) { setVal(id, '0'); });
        setVal('CmbCareierType', 2);                                        /* CmbCareierType.Rows[1].Activate() - "By Air" */
        ['txtCustomerContractsNos', 'txtCustomerContractsDates', 'txtLotRefHeader', 'txtGdNo', 'txtDeliveryRemarks', 'txtTcpRegNo', 'txtExRate', 'txtFcyAmount', 'txtLocalAmt', 'txtaddlessamount',
            'txtaddlesscommnets', 'txtNoOfContainer', 'txtTotalNetAmount', 'txtGrossWeight', 'txtNetWeight', 'txtCertificate1', 'txtCertificateOfOrigion', 'txtRemarks1', 'txtRemarks2',
            'txtOtherRemarks1', 'txtOtherRemarks2'].forEach(function (id) { setText(id, ''); });
        DET = []; ptReset(false); PT = []; ocReset(false); OC = []; oiReset(); OI = [];
        DETC = []; ptReset(true); PTC = []; ocReset(true); OCC = [];
        bindGrids();
        tab('outer', 'tabInvoiceDetailll'); tab('inv', 'tabItemDetail');
        focus('txtDocDate');
        if (skipServer) return Promise.resolve();
        return getJson(API + '/new').then(function (d) {
            d = d || {};
            setText('txtdocno', str(d.docNo));
            bindInvoiceNos(d.invoiceNos);
        }).catch(function () { /* empty catch */ });
    }
    function formNew(btn) { return busy(btn, function () { return formReset(false); }); }
    /* btnRefresh_Click */
    function refresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh?recId=' + REC.id).then(function (d) {
                d = d || {};
                F9 = !!d.allowExportMultiCompanies; show('exportCompanyBox', F9);
                bindAll(d);
                return customerLeave();
            });
        });
    }

    // ============================================================================== save (Insert)
    function header() {
        return {
            ExportCompanyId: sel('CmbExportCompany'), DocCode: netI(val('txtdocno')), DocDate: val('txtDocDate') || today(), InvoiceNo: val('cmbInvoiceNo'),
            DocumentaryCreditNo: val('txtDocumentaryCreditNo'), DocumentaryCreditNoIssueDate: val('txtDocumentaryCreditNoDate') || today(), SupplierCustomerId: sel('CmbCustomer'),
            ConsigneeId: sel('CmbConsignee'), CustomerContractNos: val('txtCustomerContractsNos'), ContractDates: val('txtCustomerContractsDates'), NotifyParty1: sel('CmbNotifyParty1'),
            NotifyParty2: sel('CmbNotifyParty2'), NotifyParty3: sel('CmbNotifyParty3'), ImporterBankId: sel('CmbImporterBank'), ExporteBankId: sel('CmbExporterBank'),
            LotNoRef: val('txtLotRefHeader'), GDNo: val('txtGdNo'), GDDate: val('txtGdDate') || today(), LoadingPortId: sel('CmbLoadingPort'), DestinationPortId: sel('CmbDestinationPort'),
            CarierTypeId: sel('CmbCareierType'), CarierType: selText('CmbCareierType'), DeliveryTermId: sel('CmbDeliveryTerm'), DeliveryRemarks: val('txtDeliveryRemarks'),
            TCPRegNo: val('txtTcpRegNo'), ContinentId: sel('CmbContinent'), OriginCountryId: sel('CmbOriginCountry'), ImporterCountryId: sel('CmbImporterCountry'),
            PlaceOfDeliveryId: sel('CmbPlaceOfDelivery'), FcurrencyId: sel('CmbFcyCode'), ConversionRate: netD(val('txtExRate')), FCurrencyAmount: netD(val('txtFcyAmount')),
            EquivalentAmount: netD(val('txtLocalAmt')), TotalAmount: netD(val('txtTotalNetAmount')), NoOfContainers: netD(val('txtNoOfContainer')), GrossMton: netD(val('txtGrossWeight')),
            NetMton: netD(val('txtNetWeight')), AddLessComments: val('txtaddlesscommnets'), Certificate1: val('txtCertificate1'), Certificate2: val('txtCertificateOfOrigion'),
            Remarks1: val('txtRemarks1'), Remarks2: val('txtRemarks2'), OtherRemarks1: val('txtOtherRemarks1'), OtherRemarks2: val('txtOtherRemarks2'),
            AttachmentsValues: REC.attachments, CustomAttachmentsValues: REC.customAttachments
        };
    }
    /* Insert(): the controlsToValidate list, in the desktop's order */
    function headerValidation() {
        if (F9 && !reqCombo('CmbExportCompany', 'Export Company')) return false;
        if (netI(val('txtdocno')) === 0) { box('Doc No must be a non-zero number'); focus('txtdocno'); return false; }
        return reqText('cmbInvoiceNo', 'Invoice No') && reqText('txtDocumentaryCreditNo', 'Documentary Credit No') && reqCombo('CmbCustomer', 'Customer') &&
            reqCombo('CmbConsignee', 'Consignee') && reqCombo('CmbLoadingPort', 'Loading Port') && reqCombo('CmbDestinationPort', 'Destination Port') &&
            reqCombo('CmbCareierType', 'Carrier Type') && reqCombo('CmbDeliveryTerm', 'Delivery Term') && reqCombo('CmbContinent', 'Continent') &&
            reqCombo('CmbOriginCountry', 'Origin') && reqCombo('CmbImporterCountry', 'Importer Country') && reqCombo('CmbPlaceOfDelivery', 'Delivery Place') &&
            reqCombo('CmbFcyCode', 'FCY Code') && reqNum('txtExRate', 'Exchange Rate') && reqNum('txtFcyAmount', 'FCY Amount') && reqNum('txtLocalAmt', 'Local Amount') &&
            reqNum('txtTotalNetAmount', 'Total Amount') && reqNum('txtNoOfContainer', 'No Of Container') && reqNum('txtGrossWeight', 'Gross MTon') && reqNum('txtNetWeight', 'Net MTon');
    }
    function insert(btn) {
        return busy(btn, function () {
            if (!DET.length) { tab('outer', 'tabInvoiceDetailll'); tab('inv', 'tabItemDetail'); box('Item Detail Record Not Found'); return; }
            if (!PT.length) { tab('outer', 'tabInvoiceDetailll'); tab('inv', 'tabPaymentDetail'); focus('CmbPaymentTermPTDetail'); box('Payment Detail Not Found. Please Check! '); return; }
            if (!DETC.length) { tab('outer', 'tabCustomDetail'); tab('cus', 'tabItemDetailCustom'); box('Custom Item Detail Record Not Found'); return; }
            if (!PTC.length) { tab('outer', 'tabCustomDetail'); tab('cus', 'tabPaymentDetailCustom'); focus('CmbPaymentTermPTDetailCustom'); box('Custom Payment Detail Not Found. Please Check! '); return; }
            if (!headerValidation()) return;
            addLessFromCharges(false);
            updateHeader(false);
            if (netD(val('txtGrossWeight')) < netD(val('txtNetWeight'))) { box('Gross Weight must be equal to or greater than Net Weight. Thank you.'); focus('txtGrossWeight'); return; }
            if (sum(PT, 'PrcntOfTotal') !== 100.0) { box('Payment Percent Not Equal To 100%'); tab('outer', 'tabInvoiceDetailll'); tab('inv', 'tabPaymentDetail'); focus('CmbPaymentTermPTDetail'); return; }
            if (sum(PTC, 'PrcntOfTotal') !== 100.0) { box('Payment Percent Not Equal To 100%'); tab('outer', 'tabCustomDetail'); tab('cus', 'tabPaymentDetailCustom'); focus('CmbPaymentTermPTDetailCustom'); return; }
            /* CheckItemIds(dtdetail, dtPackingListDetail): the Packing List page is removed - its table is always empty */
            if (!ask(REC.id === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return;
            var recId = REC.id;
            var preview = $id('ChkPrintPreview').checked;
            var win = preview && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
            return postJson(API + '/save', { recId: recId, header: header(), detail: DET, removed: REM, detailCustom: DETC, removedCustom: REMC, paymentTerms: PT, paymentTermsCustom: PTC,
                otherCharges: OC, otherChargesCustom: OCC, otherItems: OI })
                .then(function (d) {
                    box((d && d.message) || (recId === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                    var printId = d ? netI(d.printId) : 0;
                    return formReset(false).then(function () {
                        if (preview) { if (printId === 0) { if (win) global.CrystalPrint.release(win); box('No Record Found For Display'); } else print('521-exportinvoiceslip', { eximInvoiceId: printId }, null, win); }
                    });
                })
                .catch(function (e) { if (win && global.CrystalPrint) global.CrystalPrint.release(win); box(e.message); });
        });
    }
    /* btnsave_Click: RecId = 0 first - a Save after an Edit inserts a new invoice (kept) */
    function save(btn) { REC.id = 0; return insert(btn); }
    /* btnupdate_Click */
    function update(btn) { if (REC.id === 0) { box('Rec Id not found...'); return Promise.resolve(); } return insert(btn); }

    // ============================================================================== read by id
    function readById(id) {
        formReset(true);
        return getJson(API + '/by-id?id=' + netI(id)).then(function (o) {
            o = o || {};
            REC.id = netI(o.Id);
            tab('main', 'tabPage1');
            show('btnsave', false); show('btnupdate', true);
            REC.updateMode = true;
            setVal('CmbExportCompany', o.ExportCompanyId);
            setText('txtdocno', str(o.DocCode)); setText('txtDocDate', isoDate(o.DocDate) || today());
            bindInvoiceNos(o.invoiceNos);
            setText('cmbInvoiceNo', str(o.InvoiceNo));
            setText('txtDocumentaryCreditNo', str(o.DocumentaryCreditNo)); setText('txtDocumentaryCreditNoDate', isoDate(o.DocumentaryCreditNoIssueDate) || today());
            setVal('CmbCustomer', o.SupplierCustomerId);
            applyCustomer(o.customer);
            if (netI(o.ConsigneeId) > 0) setVal('CmbConsignee', o.ConsigneeId);
            if (str(o.CustomerContractNos).trim() !== '') setText('txtCustomerContractsNos', o.CustomerContractNos);
            if (str(o.ContractDates).trim() !== '') setText('txtCustomerContractsDates', o.ContractDates);
            if (netI(o.NotifyParty1) > 0) setVal('CmbNotifyParty1', o.NotifyParty1);
            if (netI(o.NotifyParty2) > 0) setVal('CmbNotifyParty2', o.NotifyParty2);
            if (netI(o.NotifyParty3) > 0) setVal('CmbNotifyParty3', o.NotifyParty3);
            if (netI(o.ImporterBankId) > 0) setVal('CmbImporterBank', o.ImporterBankId);
            if (netI(o.ExporteBankId) > 0) setVal('CmbExporterBank', o.ExporteBankId);
            if (str(o.LotNoRef).trim() !== '') setText('txtLotRefHeader', o.LotNoRef);
            if (str(o.GDNo).trim() !== '') setText('txtGdNo', o.GDNo);
            if (isoDate(o.GDDate)) setText('txtGdDate', isoDate(o.GDDate));
            if (netI(o.LoadingPortId) > 0) setVal('CmbLoadingPort', o.LoadingPortId);
            if (netI(o.DestinationPortId) > 0) setVal('CmbDestinationPort', o.DestinationPortId);
            if (str(o.CarierType).trim() !== '') { var cr = CARRIER.filter(function (x) { return x.Name === str(o.CarierType); })[0]; setVal('CmbCareierType', cr ? cr.Id : '0'); }
            if (netI(o.DeliveryTermId) > 0) setVal('CmbDeliveryTerm', o.DeliveryTermId);
            if (str(o.DeliveryRemarks).trim() !== '') setText('txtDeliveryRemarks', o.DeliveryRemarks); else deliveryTermChanged();
            if (str(o.TCPRegNo).trim() !== '') setText('txtTcpRegNo', o.TCPRegNo);
            if (netI(o.ContinentId) > 0) setVal('CmbContinent', o.ContinentId);
            if (netI(o.OriginCountryId) > 0) setVal('CmbOriginCountry', o.OriginCountryId);
            if (netI(o.ImporterCountryId) > 0) setVal('CmbImporterCountry', o.ImporterCountryId);
            if (netI(o.PlaceOfDeliveryId) > 0) setVal('CmbPlaceOfDelivery', o.PlaceOfDeliveryId);
            if (netI(o.FcurrencyId) > 0) setVal('CmbFcyCode', o.FcurrencyId);
            setText('txtFcyAmount', f4(o.FCurrencyAmount)); setText('txtExRate', f4(o.ConversionRate)); setText('txtLocalAmt', f4(o.EquivalentAmount));
            setText('txtaddlesscommnets', str(o.AddLessComments)); setText('txtaddlessamount', plain(o.AddLessAmount)); setText('txtTotalNetAmount', f4(o.TotalAmount));
            setText('txtNoOfContainer', plain(o.NoOfContainers));
            setText('txtGrossWeight', netD(o.GrossMton) > 0 ? f4(o.GrossMton) : f4(netD(o.GrossWeight) / 1000.0));
            setText('txtNetWeight', netD(o.NetMton) > 0 ? f4(o.NetMton) : f4(netD(o.NetWeight) / 1000.0));
            setText('txtCertificate1', str(o.Certificate1)); setText('txtCertificateOfOrigion', str(o.Certificate2));
            setText('txtRemarks1', str(o.Remarks1)); setText('txtRemarks2', str(o.Remarks2)); setText('txtOtherRemarks1', str(o.OtherRemarks1)); setText('txtOtherRemarks2', str(o.OtherRemarks2));
            REC.attachments = str(o.AttachmentsValues); REC.customAttachments = str(o.CustomAttachmentsValues);
            DET = (o.detail || []).map(copy); PT = (o.paymentTerms || []).map(copy); OC = (o.otherCharges || []).map(copy); OI = (o.otherItems || []).map(copy);
            DETC = (o.detailCustom || []).map(copy); PTC = (o.paymentTermsCustom || []).map(copy); OCC = (o.otherChargesCustom || []).map(copy);
            bindGrids();
            focus('txtDocDate');
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================== prints
    function print(key, args, btn, win) {
        if (!global.CrystalPrint) { box('Print is not available.'); return Promise.resolve(); }
        return global.CrystalPrint.open(key, args, btn, win);
    }
    /* CommonServices.CommercialInvoiceSlip521 / CommercialInvoiceSlipWithName: "No Record Found For Display" when the id is 0 */
    function noRecord(id, btn, key) { if (netI(id) === 0) { box('No Record Found For Display'); return Promise.resolve(); } return print(key, { eximInvoiceId: netI(id) }, btn); }
    function key521(letter) { return letter ? '521-' + letter.toLowerCase() + '-exportinvoiceslip' : '521-exportinvoiceslip'; }
    function print521(btn) { return noRecord(REC.id, btn, key521(null)); }
    /* BtnPrint521A..D: CommercialInvoiceSlipWithName(RecId, "521_X_ExportInvoiceSlip") */
    function print521X(btn, letter) { return noRecord(REC.id, btn, key521(letter)); }

    // ============================================================================== history
    function bindHistoryCombos(d) { bind('CmbCustomerHistory', d.customers || [], 'Id', 'Name'); bind('CmbInvoiceNoHistory', d.invoices || [], 'Id', 'Name'); }
    var HIST_COLS = [['InvoiceNo', 'InvoiceNo'], ['DocCode', 'DocCode'], ['DocDate', 'DocDate'], ['CustomerName', 'CustomerName'], ['NoOfContainers', 'NoOfContainers', 3, true],
        ['GrossWeight', 'GrossWeight', 3, true], ['NetWeight', 'NetWeight', 3, true], ['FcyAmount', 'FcyAmount', 3, true], ['AddLess', 'Add/Less', 3, true], ['TotalAmount', 'TotalAmount', 3, true],
        ['LoadingPort', 'LoadingPort'], ['DestinationPort', 'DestinationPort'], ['EntryUser', 'EntryUser'], ['EntryDate', 'EntryDate'], ['ModifyUser', 'ModifyUser'], ['ModifyDate', 'ModifyDate'],
        ['NoOfAttachments', 'NoOfAttachments']];
    /* HistoryGridSettings: Slip, PrintA..D (print right), Edit (update right) before InvoiceNo; AddAttachment at the end */
    function histLead() { var l = []; if (PERM.Print) l.push('Slip', 'PrintA', 'PrintB', 'PrintC', 'PrintD'); if (PERM.Update) l.push('Edit'); return l; }
    function renderHistory() {
        var lead = histLead();
        $id('histHead').innerHTML = '<tr>' + lead.map(function (k) { return '<th class="ctr">' + esc(k) + '</th>'; }).join('') + HIST_COLS.map(function (c) { return '<th' + (c[2] ? ' class="num"' : '') + '>' + esc(c[1]) + '</th>'; }).join('') + '<th>AddAttachment</th></tr>';
        $id('histBody').innerHTML = HIST.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (CUR.hist === i ? ' class="is-current"' : '') + '>' + lead.map(function (k) { return '<td class="win-cell-btn"><button type="button" class="win-edit" data-hbtn="' + k + '" data-i="' + i + '">' + k + '</button></td>'; }).join('');
            HIST_COLS.forEach(function (c) {
                var v = r[c[0]], t = c[2] ? fmtN(v, c[2], false) : (c[0] === 'DocDate' ? ddMMMyyyy(v) : (c[0] === 'EntryDate' || c[0] === 'ModifyDate') ? dtFmt(v) : str(v));
                if (c[0] === 'InvoiceNo') t = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(t) + '</a>'; else t = esc(t);
                h += '<td' + (c[2] ? ' class="num"' : '') + '>' + t + '</td>';
            });
            return h + '<td class="win-cell-btn"><button type="button" class="win-edit" data-hbtn="AddAttachment" data-i="' + i + '">AddAttachment</button></td></tr>';
        }).join('');
        $id('histFoot').innerHTML = HIST.length ? '<tr>' + lead.map(function (x, k) { return '<td class="lbl">' + (k === 0 ? '&Sigma;' : '') + '</td>'; }).join('') + HIST_COLS.map(function (c) { return c[3] ? '<td>' + esc(fmtN(sum(HIST, c[0]), c[2], false)) + '</td>' : '<td class="lbl"></td>'; }).join('') + '<td class="lbl"></td></tr>' : '';
        show('histEmpty', HIST.length === 0);
        var lb = $id('label92'); if (lb) lb.textContent = 'Filtered Records' + (HIST.length ? ' (' + HIST.length + ')' : '');
    }
    /* grddetailhistorySettings: the id columns hidden, CropYear / JobLot / PackingType names in their place */
    function histDetCols(c) {
        var out = [];
        detCols(c).forEach(function (x) {
            if (x.k === 'CropYearId') out.push({ k: 'CropYear', h: 'CropYear' });
            else if (x.k === 'JobLotId') out.push({ k: 'JobLot', h: 'JobLot' });
            else if (x.k === 'PackingTypeId') out.push({ k: 'PackingType', h: 'PackingType' });
            else out.push(x);
        });
        return out;
    }
    function drawPlain(p, rows, cols) {
        $id(p + 'Head').innerHTML = '<tr>' + cols.map(function (x) { return '<th' + (x.n !== undefined ? ' class="num"' : '') + '>' + esc(x.h) + '</th>'; }).join('') + '</tr>';
        $id(p + 'Body').innerHTML = (rows || []).map(function (r) {
            return '<tr>' + cols.map(function (x) { var v = r[x.k]; var t = x.d ? shortDate(v) : x.n !== undefined ? fmtN(v, x.n, false) : str(v); return '<td' + (x.n !== undefined ? ' class="num"' : '') + '>' + esc(t) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $id(p + 'Foot').innerHTML = (rows || []).length ? '<tr>' + cols.map(function (x, i) { return x.s ? '<td>' + esc(fmtN(sum(rows, x.k), x.n, false)) + '</td>' : '<td class="lbl">' + (i === 0 ? '&Sigma;' : '') + '</td>'; }).join('') + '</tr>' : '';
    }
    var H_PT = [{ k: 'PaymentTerm', h: 'PaymentTerm' }, { k: 'FinancialInstrumentNo', h: 'FinancialInstrumentNo' }, { k: 'PrcntOfTotal', h: '%OfTotal', n: 2, s: true }, { k: 'FcyAmount', h: 'FcyAmount', n: 3, s: true }, { k: 'DueDays', h: 'DueDays' }, { k: 'Remarks', h: 'Remarks' }];
    var H_OC = [{ k: 'ChargesItem', h: 'ChargesItem' }, { k: 'AddAmount', h: 'AddAmount', n: 3, s: true }, { k: 'LessAmount', h: 'LessAmount', n: 3, s: true }, { k: 'Remarks', h: 'Remarks' }];
    var H_PL = [{ k: 'ContainerNo', h: 'ContainerNo' }, { k: 'ItemCode', h: 'ItemCode' }, { k: 'ItemName', h: 'ItemName' }, { k: 'CommodityDetail', h: 'ItemCommodityDetail' }, { k: 'PmDetail', h: 'ItemPmDetail' },
        { k: 'PackageDate', h: 'PackageDate', d: true }, { k: 'ExpiryDate', h: 'ExpiryDate', d: true }, { k: 'LotNo', h: 'LotNo' }, { k: 'ThirdPartyAnalysisNo', h: 'LabInspectionNo' }, { k: 'SubLotNo', h: 'SubLotNo' },
        { k: 'OuterQty', h: 'OuterQty', n: 2, s: true }, { k: 'NetWeight', h: 'NetWeight', n: 2, s: true }, { k: 'GrossWeight', h: 'GrossWeight', n: 2, s: true }, { k: 'Remarks', h: 'Remarks' }];
    var H_OI = [{ k: 'ItemName', h: 'ItemName' }, { k: 'Qty', h: 'Qty', n: 2, s: true }, { k: 'Rate', h: 'Rate', n: 4 }, { k: 'Amount', h: 'Amount', n: 3, s: true }, { k: 'Remarks', h: 'Remarks' }];
    function renderHistDetail(d) {
        d = d || {};
        drawPlain('hd', d.detail, histDetCols(false)); drawPlain('hp', d.paymentTerms, H_PT); drawPlain('ho', d.otherCharges, H_OC); drawPlain('hl', d.packingList, H_PL); drawPlain('hi', d.otherItems, H_OI);
        drawPlain('hdc', d.detailCustom, histDetCols(true)); drawPlain('hpc', d.paymentTermsCustom, H_PT); drawPlain('hoc', d.otherChargesCustom, H_OC);
    }
    /* HistoryGridFill */
    function historyShow(btn) {
        return busy(btn, function () {
            var body = {
                dateKind: (document.querySelector('input[name="histDate"]:checked') || {}).value || 'doc',
                fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'), toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'),
                fromDocNo: netI(val('FromDocNoHistory')), toDocNo: netI(val('ToDocNoHistory')), customerId: sel('CmbCustomerHistory'), invoiceId: sel('CmbInvoiceNoHistory'),
                actionId: netI((document.querySelector('input[name="histRef"]:checked') || {}).value)
            };
            return postJson(API + '/history', body).then(function (rows) {
                HIST = rows || []; CUR.hist = -1; renderHistory(); renderHistDetail(null);
            }).catch(function (e) { box(e.message); });
        });
    }
    /* btnResetHistory_Click: From = today - 3 (not the configured days), To = today, doc nos and customer cleared, grids cleared */
    function historyReset() {
        setText('FromDateHistory', daysAgo(3)); setText('ToDateHistory', today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', ''); setVal('CmbCustomerHistory', '0');
        HIST = []; renderHistory(); renderHistDetail(null); show('histEmpty', false);
    }
    function historyRefresh(btn) { return busy(btn, function () { return getJson(API + '/history-combos').then(bindHistoryCombos).catch(function (e) { box(e.message); }); }); }
    /* DataGridHistory_SelectionChanged -> GetDetailGrdByHeadId */
    function historySelect(i) {
        CUR.hist = i; var r = HIST[i]; if (!r) return;
        getJson(API + '/history-detail?id=' + netI(r.Id)).then(renderHistDetail).catch(function () { /* empty catch */ });
    }
    /* DataGridHistory_ColumnButtonClick */
    function historyButton(k, i) {
        var r = HIST[i]; if (!r) return; var id = netI(r.Id);
        if (k === 'Slip') noRecord(id, null, key521(null));
        else if (k === 'Edit') readById(id);
        else if (k === 'AddAttachment') notPorted('Add Attachment');
        else if (k === 'PrintA' || k === 'PrintB' || k === 'PrintC' || k === 'PrintD') noRecord(id, null, key521(k.charAt(5)));
    }

    // ============================================================================== loader (LoadSalesContractForInvoice, DocumentTypeId 223)
    /* BtnLoad_Click: Loader.ScheduleIds = the grid's distinct schedules when Update is visible and enabled */
    function scheduleIdsForSaved() {
        if (!(shown('btnupdate') && !$id('btnupdate').disabled)) return '';
        var ids = []; DET.forEach(function (r) { var s = netI(r.ContractScheduleId); if (ids.indexOf(s) < 0) ids.push(s); });
        return ids.join(',');
    }
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
            var seen = {}; LDR.header = [];
            LDR.rows.forEach(function (r) {
                var k = netI(col(r, 'ScheduleId')); if (seen[k]) return; seen[k] = true;
                LDR.header.push({ Id: netI(col(r, 'Id')), ScheduleId: k, DocNo: col(r, 'DocNo'), ContractNo: col(r, 'ProformaNo'), ContractDate: shortDate(col(r, 'ProformaDate')),
                    OrderDate: shortDate(col(r, 'SalesContratDate')), LoadingDate: shortDate(col(r, 'LoadingDate')),
                    ScheduleStatus: (col(r, 'ContractScheduleStatus') === true || netI(col(r, 'ContractScheduleStatus')) === 1 || str(col(r, 'ContractScheduleStatus')).toLowerCase() === 'true') ? 'Exist' : 'Not Exist',
                    ScheduleCode: col(r, 'ScheduleCode'), SupplierCustomerId: col(r, 'SupCustId'), PartyName: col(r, 'CustomerName'), NoOfContainers: col(r, 'NoOfContainers'), SalesMan: col(r, 'SalesMan'),
                    ExporterBank: col(r, 'ExporterBank'), ShipmentStartDate: shortDate(col(r, 'ShipmentStartDate')), DeliveryDays: col(r, 'DeliveryDays'), LastShipmentDate: shortDate(col(r, 'LastShipmentDate')),
                    CustomerApprovedDate: shortDate(col(r, 'CustomerApprovalDate')), CustomerApprovalRemarks: col(r, 'CustomerApprovalRemarks'), CurrencyCode: col(r, 'CurrencyCode'),
                    ExchangeRate: col(r, 'ExchangeRate'), DeliveryTerm: col(r, 'DeliveryTerm'), PaymentTerm: col(r, 'PaymentTerm'), LoadingPort: col(r, 'LoadingPort'), DestinationPort: col(r, 'DestinationPort'),
                    InsuranceRemarks: col(r, 'InsuranceRemarks'), RemarksHeader: col(r, 'RemarksHeader'), IsApproved: col(r, 'IsApproved'), ApprovedStatus: col(r, 'ApprovedStatus'), EntryUser: col(r, 'EntryUser'),
                    EntryDate: dtFmt(col(r, 'EntryDate')), ModifyUser: col(r, 'ModifyUser'), ModifyDate: dtFmt(col(r, 'ModifyDate')), NoOfAttachments: col(r, 'NoOfAttachments') });
            });
            $id('ldrHead').innerHTML = '<tr><th class="ctr"><input type="checkbox" id="ldrAll" title="Select all"/></th>' + LDR_COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
            $id('ldrBody').innerHTML = LDR.header.map(function (h, i) {
                return '<tr data-i="' + i + '"><td class="ctr"><input type="checkbox" data-ldr="' + i + '"/></td>' + LDR_COLS.map(function (c) { return '<td>' + esc(str(h[c[0]])) + '</td>'; }).join('') + '</tr>';
            }).join('');
            $id('ldrDetailBody').innerHTML = ''; $id('ldrDetailFoot').innerHTML = ''; $id('ldrDetailHead').innerHTML = '';
        }).catch(function (e) { box(e.message); });
    }
    function loaderSearch(btn) { return busy(btn, loaderSearchRun); }
    function loaderReset(btn) { setVal('LdrCmbPartyName', '0'); return busy(btn, loaderSearchRun); }
    var LDR_DET = [{ k: 'ItemName', h: 'ItemName' }, { k: 'PackingType', h: 'PackingType' }, { k: 'CropYear', h: 'CropYear' }, { k: 'CommodityDetail', h: 'CommodityDetail' }, { k: 'PackUom', h: 'PackUom' },
        { k: 'MTon', h: 'MTon', n: 0, s: true }, { k: 'NoOfBags', h: 'NoOfBags', n: 0, s: true }, { k: 'NetWeight', h: 'NetWeight', n: 0, s: true }, { k: 'ItemRate', h: 'ItemRate', n: 4 }, { k: 'Amount', h: 'Amount', n: 0, s: true }];
    function loaderSelect(i) {
        var h = LDR.header[i]; if (!h) return;
        var rows = LDR.rows.filter(function (r) { return netI(col(r, 'ScheduleId')) === netI(h.ScheduleId); }).map(function (r) {
            return { ItemName: col(r, 'ItemName'), PackingType: col(r, 'PackingType'), CropYear: col(r, 'CropYear'), CommodityDetail: col(r, 'CommodityDetail'), PackUom: col(r, 'PackUom'),
                MTon: col(r, 'M_Ton'), NoOfBags: col(r, 'NoOfBags'), NetWeight: col(r, 'NetWeight'), ItemRate: col(r, 'RatePrice'), Amount: col(r, 'Amount') };
        });
        drawPlain('ldrDetail', rows, LDR_DET);
        $id('ldrBody').querySelectorAll('tr').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
    }
    /* btnLoadOnInvoice_Click (schedule wise) */
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
            ids.push(String(r.ScheduleId));
        }
        var data = LDR.rows.filter(function (x) { return ids.indexOf(String(netI(col(x, 'ScheduleId')))) >= 0; });
        loaderClose();
        bindLoaderData(data);
    }
    /* BindLoaderData -> BindAndValidateHeaderDataFromLoader, FillDetaildtsFromLoaderData, payment terms / other items / other charges by contract ids, BindGrids.
       A customer / currency mismatch is thrown into BindLoaderData's empty catch: nothing is loaded and nothing is said. */
    function bindLoaderData(contract) {
        if (!contract.length) return;
        var dr = contract[0];
        if (DET.length > 0) {
            if (netI(col(dr, 'ScheduleDetailId')) === 0) {
                if (netI(col(dr, 'SupCustId')) !== sel('CmbCustomer') || netI(col(dr, 'FcurrencyId')) !== sel('CmbFcyCode')) return;
            } else if (netI(col(dr, 'SupCustId')) !== sel('CmbCustomer')) return;
        }
        var el = $id('CmbCustomer'); if (el) el.disabled = true; el = $id('CmbFcyCode'); if (el) el.disabled = true; refreshCombos();
        setVal('CmbExportCompany', netI(col(dr, 'ExportCompanyId')));
        setVal('CmbCustomer', netI(col(dr, 'SupCustId')));
        var distinct = []; contract.forEach(function (r) { var id = netI(col(r, 'Id')); if (distinct.indexOf(id) < 0) distinct.push(id); });
        return Promise.all([
            getJson(API + '/customer-leave?customerId=' + sel('CmbCustomer')).catch(function () { return { fis: [], subParties: [] }; }),
            getJson(API + '/loader/apply?ids=' + encodeURIComponent(distinct.join(',')))
        ]).then(function (res) {
            applyCustomer(res[0]);
            var setIf = function (id, v) { if (netI(v) > 0) setVal(id, netI(v)); };
            setIf('CmbConsignee', col(dr, 'ConsigneeId')); setIf('CmbNotifyParty1', col(dr, 'NotifyPartyId')); setIf('CmbNotifyParty2', col(dr, 'NotifyParty2Id'));
            setIf('CmbNotifyParty3', col(dr, 'NotifyParty3Id')); setIf('CmbExporterBank', col(dr, 'ExporterBankId')); setIf('CmbImporterBank', col(dr, 'ImporterBankId'));
            setIf('CmbLoadingPort', col(dr, 'LoadingPortId')); setIf('CmbDestinationPort', col(dr, 'DestinationPortId'));
            if (netI(col(dr, 'DeliveryTermId')) > 0) { setVal('CmbDeliveryTerm', netI(col(dr, 'DeliveryTermId'))); deliveryTermChanged(); }
            setIf('CmbFcyCode', col(dr, 'FcurrencyId'));
            var seenS = {}, containers = 0;
            contract.forEach(function (r) { var s = netI(col(r, 'ScheduleId')); if (!seenS[s]) { seenS[s] = true; containers += netI(col(r, 'NoOfContainers')); } });
            setText('txtNoOfContainer', String(containers));
            var nos = []; contract.forEach(function (r) { var c = str(col(r, 'CustomerContractNo')); if (c.trim() !== '' && nos.indexOf(c) < 0) nos.push(c); });
            setText('txtCustomerContractsNos', nos.join(', '));
            setText('txtExRate', plain(col(dr, 'ExchangeRate')));
            /* FillDetaildtsFromLoaderData: skip rows already in the grid (schedule detail id, else contract detail id); same row into both grids */
            var schedIds = {}, detIds = {};
            DET.forEach(function (g) { var s = netI(g.ContractScheduleDetailId); if (s > 0) schedIds[s] = true; else detIds[netI(g.ContractDetailId)] = true; });
            contract.forEach(function (r) {
                var sd = netI(col(r, 'ScheduleDetailId')), cd = netI(col(r, 'ContractDetailId'));
                if (sd > 0 ? schedIds[sd] : detIds[cd]) return;
                var ie = netD(col(r, 'InnerEquivalent')), bal = netD(col(r, 'BalMTon'));
                var row = { Id: 0, ContractId: netI(col(r, 'Id')), ContractDetailId: cd, ContractNo: str(col(r, 'ProformaNo')), ContractDate: isoDate(col(r, 'ProformaDate')),
                    ContractNoInvoiceWise: str(col(r, 'ScheduleCode')), ContractScheduleId: netI(col(r, 'ScheduleId')), ContractSchedule: str(col(r, 'ScheduleCode')), ContractScheduleDetailId: sd,
                    ItemCategoryId: netI(col(r, 'ItemCategoryId')), ItemCategory: str(col(r, 'ItemCategory')), ItemId: netI(col(r, 'ExImItemId')), ItemCode: str(col(r, 'ItemCode')),
                    ItemName: str(col(r, 'ItemName')), CropYearId: netI(col(r, 'CropYearId')), JobLotId: netI(col(r, 'JobLotId')), PackingTypeId: netI(col(r, 'InvPackingMaterialTypeId')),
                    PmItemId: netI(col(r, 'PmItemId')), PmItemCode: str(col(r, 'PmItemCode')), PmItemName: str(col(r, 'PmItemName')), BrandId: netI(col(r, 'ItemBrandId')),
                    BrandCode: str(col(r, 'BrandCode')), BrandName: str(col(r, 'BrandName')), QtyMTon: bal, OuterUomId: netI(col(r, 'PackUomId')), OuterUom: str(col(r, 'PackUom')),
                    OuterEquivalent: netD(col(r, 'OuterEquivalent')), OuterQtyEquivalent: netD(col(r, 'OuterQtyEquivalent')), OuterQty: netD(col(r, 'BalNoofBags')),
                    InnerUomId: netI(col(r, 'InnerUomId')), InnerUom: str(col(r, 'InnerUom')), InnerEquivalent: ie, InnerQty: rint(bal > 0 && ie > 0 ? bal * 1000.0 / ie : 0),
                    PalletQty: netD(col(r, 'PalletQty')), GrossWeight: 0, RateWithoutAddLess: netD(col(r, 'RatePrice')), RateAddLess: 0, NetRate: netD(col(r, 'RatePrice')),
                    ContractRate: netD(col(r, 'RatePrice')), RateUOMId: netI(col(r, 'RateUomId')), RateUOM: str(col(r, 'RateUom')), RateEquivalent: netD(col(r, 'RateEquivalent')),
                    Amount: netD(col(r, 'BalShipAmount')), LabInspectionId: 0, BuyerLotNo: '', ProductCode: '', BuyerHSCode: '', HSCode: '', CommodityDetail: str(col(r, 'CommodityDetail')),
                    PackingDetail: str(col(r, 'PackingDetail')), Remarks: '', OtherItemAmount: 0, ScheduleFCL: 0 };
                DET.push(row); DETC.push(copy(row));
            });
            var d = res[1] || {};
            /* BindPaymentTermDtByContractIdsDBCall: both payment tables replaced */
            PT = (d.paymentTerms || []).map(copy); PTC = (d.paymentTerms || []).map(copy);
            /* BindOtherItemDtByContractIdsDBCall */
            OI = (d.otherItems || []).map(copy);
            /* FillOtherchargesDetailByContractIdsDBCall: appended unless the contract charge line is already there (invoice grid only) */
            var existing = OC.map(function (r) { return netI(r.ContractOtherChargesDetailId); });
            (d.otherCharges || []).forEach(function (r) { var k = netI(r.ContractOtherChargesDetailId); if (existing.indexOf(k) < 0) { existing.push(k); OC.push(copy(r)); } });
            bindGrids();
        }).catch(function () { /* empty catch */ });
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
    /* btnPackingList_Click -> frmExportInvoicePackingList (screen 882) */
    function packingList() { global.open('/export/invoice-packing-list', '_blank'); }
    /* btnDefineBankForeign_Click / btnDefineBankLocal_Click -> AcfrmDefineBank (CountryTypeId 1 / 2) */
    function defineBank(countryTypeId) { global.open('/master-data/bank?countryTypeId=' + countryTypeId, '_blank'); }
    function gridFocus(bodyId) { var tr = $id(bodyId) && $id(bodyId).querySelector('tr'); if (tr) { tr.scrollIntoView({ block: 'nearest' }); var f = tr.querySelector('input,button,[tabindex]'); if (f) f.focus(); } }

    // ============================================================================== wiring
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function wire(bodyId, h) {
        var gb = $id(bodyId); if (!gb) return;
        gb.addEventListener('click', function (e) {
            var b = e.target.closest('button'), a = e.target.closest('a.win-code');
            if (b) {
                var attrs = ['del', 'ptdel', 'ocdel', 'oidel'];
                for (var k = 0; k < attrs.length; k++) if (b.hasAttribute('data-' + attrs[k])) { if (h.del) h.del(+b.getAttribute('data-' + attrs[k])); return; }
                if (b.hasAttribute('data-f1')) { if (h.f1) h.f1(+b.getAttribute('data-i'), b.getAttribute('data-f1')); return; }
                if (b.hasAttribute('data-hbtn')) { if (h.hbtn) h.hbtn(b.getAttribute('data-hbtn'), +b.getAttribute('data-i')); return; }
            }
            if (a) { if (h.open) h.open(+a.getAttribute('data-i')); return; }
            if (e.target.closest('input')) return;
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            if (h.select) h.select(+tr.getAttribute('data-i'));
        });
        gb.addEventListener('dblclick', function (e) { if (e.target.closest('input,button')) return; var tr = e.target.closest('tr[data-i]'); if (tr && h.open) h.open(+tr.getAttribute('data-i')); });
        /* deferred so the focus has already moved on; renderDet puts it back on the same cell after the redraw */
        gb.addEventListener('change', function (e) { var inp = e.target.closest('input[data-edit-col]'); if (inp && h.edit) { var i = +inp.getAttribute('data-i'), c = inp.getAttribute('data-edit-col'), v = inp.value; setTimeout(function () { h.edit(i, c, v); }, 0); } });
        gb.addEventListener('keydown', function (e) {
            var f = e.target.closest('[data-f1]');
            if (e.key === 'F1' && f && h.f1) { e.preventDefault(); if (e.target.matches('input[data-edit-col]') && h.edit) h.edit(+e.target.getAttribute('data-i'), e.target.getAttribute('data-edit-col'), e.target.value); h.f1(+f.getAttribute('data-i'), f.getAttribute('data-f1')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            if (e.ctrlKey && e.key === 'Enter' && h.open) { e.preventDefault(); h.open(+tr.getAttribute('data-i')); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.closest('.win-tabs').getAttribute('data-tabs'), b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) { b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); }); });
        on('CmbCustomer', 'change', customerLeave);
        on('CmbDeliveryTerm', 'change', deliveryTermChanged);
        on('CmbPaymentTermPTDetail', 'change', function () { paymentTermChanged(false); });
        on('CmbPaymentTermPTDetailCustom', 'change', function () { paymentTermChanged(true); });
        on('CmbFinInstrumentsPTDetail', 'change', function () { fiChanged(false); });
        on('CmbFinInstrumentsPTDetailCustom', 'change', function () { fiChanged(true); });
        on('txtExRate', 'input', exRateChanged);
        on('txtPrcntOfTotalPTDetail', 'input', function () { percentChanged(false); });
        on('txtPrcntOfTotalPTDetailCustom', 'input', function () { percentChanged(true); });
        on('txtFcyAmountPTDetail', 'input', ptFcyChanged);
        on('txtQtyOIDetail', 'input', oiCalc); on('txtRateOIDetail', 'input', oiCalc);
        on('CmbItemOIDetail', 'change', otherItemChanged);
        on('RadItemNameOIDetail', 'change', oiBind); on('RadItemCodeOIDetail', 'change', oiBind);
        on('pickerSearch', 'input', pickerDraw);
        on('pickerSearch', 'keydown', function (e) { if (e.key === 'Enter') { e.preventDefault(); var tr = $id('pickerBody').querySelector('tr[data-pi]'); if (tr) pickerDone(+tr.getAttribute('data-pi')); } else if (e.key === 'ArrowDown') { e.preventDefault(); var t = $id('pickerBody').querySelector('tr[data-pi]'); if (t) t.focus(); } });
        on('pickerBody', 'dblclick', function (e) { var tr = e.target.closest('tr[data-pi]'); if (tr) pickerDone(+tr.getAttribute('data-pi')); });
        on('pickerBody', 'click', function (e) { var tr = e.target.closest('tr[data-pi]'); if (!tr) return; $id('pickerBody').querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); });
        on('pickerBody', 'keydown', function (e) {
            var tr = e.target.closest('tr[data-pi]'); if (!tr) return;
            if (e.key === 'Enter') { e.preventDefault(); pickerDone(+tr.getAttribute('data-pi')); }
            else if (e.key === 'ArrowDown' && tr.nextElementSibling) { e.preventDefault(); tr.nextElementSibling.focus(); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); if (tr.previousElementSibling) tr.previousElementSibling.focus(); else focus('pickerSearch'); }
        });
        wire('detailBody', { del: function (i) { detDelete(false, i); }, f1: function (i, k) { f1(false, i, k); }, edit: function (i, c, v) { cellEdit(false, i, c, v); } });
        wire('detailCustomBody', { del: function (i) { detDelete(true, i); }, f1: function (i, k) { f1(true, i, k); }, edit: function (i, c, v) { cellEdit(true, i, c, v); } });
        wire('ptBody', { del: function (i) { ptDelete(false, i); }, open: function (i) { ptEdit(false, i); } });
        wire('ptcBody', { del: function (i) { ptDelete(true, i); }, open: function (i) { ptEdit(true, i); } });
        wire('ocBody', { del: function (i) { ocDelete(false, i); }, open: function (i) { ocEdit(false, i); } });
        wire('occBody', { del: function (i) { ocDelete(true, i); }, open: function (i) { ocEdit(true, i); } });
        wire('oiBody', { del: oiDelete, open: oiEdit });
        wire('histBody', { hbtn: historyButton, open: function (i) { var r = HIST[i]; if (r) readById(r.Id); }, select: historySelect });
        wire('ldrBody', { open: loaderSelect, select: loaderSelect });
        document.addEventListener('change', function (e) { if (e.target && e.target.id === 'ldrAll') { var c = e.target.checked; $id('ldrBody').querySelectorAll('input[data-ldr]').forEach(function (x) { x.checked = c; }); } });
        /* ImProformaInvoice_KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (!$id('pickerModal').classList.contains('is-hidden')) { if (e.key === 'Escape') { e.preventDefault(); pickerClose(); } return; }
            if (!$id('loaderModal').classList.contains('is-hidden')) { if (e.key === 'Escape') loaderClose(); return; }
            if (!$id('shortcutModal').classList.contains('is-hidden')) { if (e.key === 'Escape') show('shortcutModal', false); return; }
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && e.target.type !== 'radio') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); closeForm(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && e.altKey) { shortcuts(); return; }
            if (activeTab('main') === 'tabPage1') {
                if (e.ctrlKey && k === 's' && shown('btnsave') && !$id('btnsave').disabled) { e.preventDefault(); save($id('btnsave')); }
                else if (e.ctrlKey && k === 'u' && shown('btnupdate') && !$id('btnupdate').disabled) { e.preventDefault(); save($id('btnupdate')); }   /* calls btnsave_Click (sic) */
                else if (e.ctrlKey && k === 'n') { e.preventDefault(); formNew($id('btnNew')); }
                else if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print521($id('btnPrint')); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
                else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); notPorted('Attachment'); }
                else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('txtDocDate'); }
                else if (e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowUp' || e.key === 'ArrowRight')) {
                    e.preventDefault();
                    var t = activeTab('inv'), idx = t === 'tabItemDetail' ? 0 : t === 'tabPaymentDetail' ? 1 : t === 'tabChargesDetail' ? 2 : 3;
                    if (e.key === 'ArrowDown') { if (idx === 0) gridFocus('detailBody'); else if (idx === 1) gridFocus('oiBody'); else if (idx === 2) gridFocus('ptBody'); }
                    else if (e.key === 'ArrowUp') { if (idx === 0) gridFocus('detailBody'); else if (idx === 1) focus('CmbItemOIDetail'); else if (idx === 2) focus('CmbPaymentTermPTDetail'); }
                    else { if (idx === 0) { tab('inv', 'tabPaymentDetail'); focus('CmbItemOIDetail'); } else if (idx === 1) { tab('inv', 'tabChargesDetail'); focus('CmbPaymentTermPTDetail'); } else if (idx === 2) { tab('inv', 'tabItemDetail'); gridFocus('detailBody'); } }
                }
            } else {
                if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
                else if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
                else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus('FromDateHistory'); }
                else if (e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowRight')) { e.preventDefault(); gridFocus('histBody'); }
                else if (e.ctrlKey && e.key === 'Enter' && CUR.hist >= 0 && PERM.Update) { e.preventDefault(); var r = HIST[CUR.hist]; if (r) readById(r.Id); }
            }
        });
        load();
    });

    global.ExportCi3 = {
        formNew: formNew, refresh: refresh, save: save, update: update, notPorted: notPorted, print521: print521, print521X: print521X, packingList: packingList, defineBank: defineBank,
        loaderOpen: loaderOpen, loaderClose: loaderClose, loaderSearch: loaderSearch, loaderReset: loaderReset, loaderLoad: loaderLoad, shortcuts: shortcuts, pickerClose: pickerClose,
        ptAdd: ptAdd, ptUpdate: ptUpdate, ptCancel: ptCancel, ocAdd: ocAdd, ocUpdate: ocUpdate, ocCancel: ocCancel, oiAdd: oiAdd, oiUpdate: oiUpdate, oiCancel: oiCancel,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory
    };
}(window));
