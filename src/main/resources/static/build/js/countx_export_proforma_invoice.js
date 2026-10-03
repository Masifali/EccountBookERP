/* ============================================================================================
 * countx_export_proforma_invoice.js - ProformaInvoice.cs (Architecture.WinApp.Export), screen 215 "Proforma Invoice"
 * (ExImLcOrder, DocumentTypeId 151). Form | History; API /api/export/proforma-invoice.
 * Every desktop event has its counterpart with the desktop's messages and order. Kept quirks:
 *   - ResetDetail also empties the header "No Of Containers"; Reset hides the Other Items rows but keeps them, so they come
 *     back with the next "+" on Other Items (grdotheritems.ClearStructure without dtOtherItem.Clear);
 *   - Save asks first, then (new records) regenerates the Doc No, then validates;
 *   - "Status Should Be Open In Save Mode" only applies when the Update button is neither visible nor enabled;
 *   - the Other Items amount is only recalculated when Qty and Rate are both > 0.
 * Button contract: disabled + spinner while a request runs, no duplicates, re-enabled on success and failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/api/export/proforma-invoice';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) {
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim();
        if (!s || !/^[-+]?(\d+\.?\d*|\.\d+)([eE][-+]?\d+)?$/.test(s)) return 0;
        var n = parseFloat(s); return isFinite(n) ? n : 0;
    }
    function clr(v) { var n = netD(v); if (n === Math.round(n) && Math.abs(n) < 1e15) return String(n); return Number(n.toPrecision(15)).toString(); }
    function roundAway(n, d) { var p = Math.pow(10, d); var r = Math.round(Math.abs(n) * p + 1e-9) / p; return n < 0 ? -r : r; }
    function bankersRound(n) { var f = Math.floor(n), diff = n - f; if (Math.abs(diff - 0.5) < 1e-9) return f % 2 === 0 ? f : f + 1; return Math.round(n); }
    function group(ip) { return ip.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function fmtNet(v, pattern) {
        var n = netD(v);
        var m = /\.(#+|0+)$/.exec(pattern);
        var dec = m ? m[1].length : 0, fixed = m ? m[1].charAt(0) === '0' : false;
        var r = roundAway(n, dec), neg = r < 0; r = Math.abs(r);
        var parts = r.toFixed(dec).split('.'), ip = parts[0], fp = parts[1] || '';
        if (!fixed) fp = fp.replace(/0+$/, '');
        if (pattern.indexOf('#,#') === 0 && pattern.indexOf('#,##0') !== 0 && ip === '0') ip = '';
        if (pattern.indexOf('0,0') === 0 && ip.length < 2) ip = ('00' + ip).slice(-2);
        var out = group(ip) + (fp ? '.' + fp : '');
        if (out === '') return '';
        return (neg && out !== '0' ? '-' : '') + out;
    }
    /* CommonServices.CommasApplyWhileTyping: thousands separators on the integer part, decimals kept. */
    function commas(v) {
        var s = clr(v); var neg = s.charAt(0) === '-'; if (neg) s = s.substring(1);
        var p = s.split('.'); return (neg ? '-' : '') + group(p[0]) + (p.length > 1 ? '.' + p[1] : '');
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoDate(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(v).trim()); return m ? m[1] + '-' + m[2] + '-' + m[3] : ''; }
    function shortDate(v) { var s = isoDate(v); if (!s) return ''; return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4); }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve();
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done();
        return Promise.resolve(p);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
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
    function visible(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden'); }
    function bind(id, rows, valueCol, textCol, keep) {
        var s = $id(id); if (!s) return;
        var old = s.value, h = '<option value=""></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r[valueCol]) + '">' + esc(r[textCol]) + '</option>'; });
        s.innerHTML = h;
        s.value = keep === false ? '' : old;
        if (s.value !== old && keep !== false) s.value = '';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; var t = (v === null || v === undefined || v === 0 || v === '0') ? '' : String(v); s.value = t; if (s.value !== t) s.value = ''; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function txt(id) { var s = $id(id); if (!s) return ''; if (s.tagName === 'SELECT') { var o = s.options[s.selectedIndex]; return o && s.value !== '' ? o.text : ''; } return s.value; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function radio(name) { var r = document.querySelector('input[name="' + name + '"]:checked'); return r ? r.value : ''; }
    function find(list, id) { id = netI(id); for (var i = 0; i < (list || []).length; i++) if (netI(list[i].Id) === id) return list[i]; return null; }
    function cancel() { global.close(); setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150); }

    // ------------------------------------------------------------------------------ state

    var PERM = { Save: true, Update: true, Print: true };
    var DAYS = 0;
    var L = { packTypes: [], uoms: [] };
    var S = { recId: 0, rows: [], removed: [], other: [], otherHidden: false, updateIdx: -1, otherIdx: -1, cur: -1 };
    var HIST = [], HCUR = -1;
    var STATUS = [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Complete' }, { Id: 3, Status: 'Cancel' }];

    function tab(panelId) {
        document.querySelectorAll('.win-tabs[data-tabs="pf"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['pfForm', 'pfHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'pfHistory';
        var fb = $id('btnPfFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        if (onHist) focus('FromDateHistory'); else focus('txtDocDate');
    }
    function onHistory() { return $id('pfHistory').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'pfForm' : 'pfHistory'); }
    function subtab(id) {
        document.querySelectorAll('[data-subtabs="pf"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-subtab') === id); });
        document.querySelectorAll('[data-subpanel="pf"]').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    // ------------------------------------------------------------------------------ load

    function bindLookups(d) {
        ['CmbCustomer', 'CmbNotifyParty', 'cmbCommAgent'].forEach(function (id) { bind(id, d.customers, 'Id', 'CompanyName'); });
        if (d.salesPersons) { bind('CmbSalePerson', d.salesPersons, 'Id', 'CompanyName'); var s = $id('CmbSalePerson'); if (s.options.length > 1 && !s.value) s.selectedIndex = 1; refreshCombos(); }
        bind('CmbDeliveryTerm', d.deliveryTerms, 'Id', 'Code');
        bind('CmbPaymentTerms', d.paymentTerms, 'Id', 'LcOrderTerm');
        bind('CmbLoadingPort', d.ports, 'Id', 'PortName');
        bind('CmbDestinationPort', d.ports, 'Id', 'PortName');
        if (d.importerBanks && d.importerBanks.length) bind('CmbImporterBank', d.importerBanks, 'Id', 'BranchName');
        if (d.exporterBanks && d.exporterBanks.length) bind('CmbExporterbank', d.exporterBanks, 'Id', 'BranchName');
        bind('CmbFCYCode', d.currencies, 'Id', 'CurrencyCode');
        bind('combitem', d.items, 'Id', 'ItemName');
        L.packTypes = d.packTypes || []; bind('combpcktype', L.packTypes, 'Id', 'Description');
        if (d.cropYears) bind('CmbCropYear', d.cropYears, 'Id', 'CropYear');
        if (d.otherItemList) bind('CmbOtherItem', d.otherItemList, 'Id', 'ItemName');
    }
    /** FrmExportSalesContract_Load. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save;
            $id('Print').disabled = !PERM.Print;
            $id('btnUpdate').disabled = !PERM.Update;
            bindLookups(d);
            bind('cmbStatus', STATUS, 'Id', 'Status'); setVal('cmbStatus', 1);
            setText('txtExportSoNo', d.docNo || '');
            setText('txtDocDate', today()); setText('datOrderDate', today()); setText('txtShipmentDate', today());
            DAYS = netI(d.defaultDaysToLessFromHistoryFromDate);
            bind('CmbCustomerHistory', d.historyCustomers, 'Id', 'Customer');
            setText('FromDateHistory', DAYS > 0 ? daysAgo(DAYS) : daysAgo(3));
            setText('ToDateHistory', today());
            subtab('tabContractDetail');
            render(); renderOther();
            $id('pfFooterInfo').textContent = 'ProformaInvoice  -  Document Type 151';
            focus('txtDocDate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    // ------------------------------------------------------------------------------ header totals

    function sumOf(list, c) { var s = 0; list.forEach(function (r) { s += netD(r[c]); }); return s; }
    /** InsterAmountandQtyMtonInHeader. */
    function insertTotals() {
        var other = S.otherHidden ? 0 : sumOf(S.other, 'Amount');
        setText('txtMTon', clr(sumOf(S.rows, 'QtyMTon')));
        setText('txtNoOfContainers', clr(sumOf(S.rows, 'NoOfContainers')));
        setText('txtFCYAmount', commas(sumOf(S.rows, 'Amount') + other));
    }
    /** CalculateCommission: Math.Round(total Amount * % / 100) when the grid has a current row. */
    function calculateCommission() {
        if (!S.rows.length) return;
        var rate = netD(val('txtCommPercent')), total = sumOf(S.rows, 'Amount');
        setText('txtCommAmount', isNaN(total) ? '0' : commas(bankersRound(total * rate / 100.0)));
    }
    function commPercentChanged() {
        if (netD(val('txtCommPercent')) > 100) { box('Commission % Can not Greater than 100...'); setText('txtCommPercent', '100'); return commPercentChanged(); }
        calculateCommission();
    }

    // ------------------------------------------------------------------------------ detail grid

    var COLS = ['ItemName', 'PackType', 'PackingWeight', 'CropYear', 'NoOfContainers', 'NoOfBagsCntnr', 'PackSize', 'QtyMTon', 'NoOfBags', 'PackingTotalWeight',
        'GrossWeight', 'Rate', 'RateUOM', 'Amount', 'ContainerSize', 'Remarks', 'PackingExpiryDate', 'PackingDetail'];
    var FMT = { QtyMTon: '#,##0.##', NoOfBags: '#,##0.##', Amount: '0,0.00', PackingWeight: '#,##0.##', PackingTotalWeight: '#,##0.##', Rate: '#,##0.##' };
    var TOT = { QtyMTon: '#,##0.##', NoOfBags: '#,##0.##', Amount: '#,##0.##', PackingWeight: '#,##0.##', PackingTotalWeight: '#,##0.##' };
    var NUM = { PackingWeight: 1, NoOfContainers: 1, NoOfBagsCntnr: 1, QtyMTon: 1, NoOfBags: 1, PackingTotalWeight: 1, GrossWeight: 1, Rate: 1, Amount: 1 };
    function render() {
        var sums = {};
        $id('pfBody').innerHTML = S.rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === S.cur ? ' class="is-current"' : '') + '><td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
            COLS.forEach(function (c) {
                var v = r[c];
                if (TOT[c]) sums[c] = (sums[c] || 0) + netD(v);
                var inner = esc(FMT[c] ? fmtNet(v, FMT[c]) : (NUM[c] ? clr(v) : str(v)));
                if (c === 'ItemName') inner = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + inner + '</a>';
                h += '<td' + (NUM[c] ? ' class="num"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        var foot = $id('pfFoot');
        if (!S.rows.length) { foot.innerHTML = ''; return; }
        var t = '<tr><td class="lbl">&Sigma;</td>';
        COLS.forEach(function (c) { t += '<td' + (TOT[c] ? '' : ' class="lbl"') + '>' + (TOT[c] ? esc(fmtNet(sums[c] || 0, TOT[c])) : '') + '</td>'; });
        foot.innerHTML = t + '</tr>';
    }
    /** grdDetails_ColumnButtonClick (X): a saved row asks and is kept for the update with ActionTypeId 3. */
    function deleteRow(i) {
        var r = S.rows[i]; if (!r) return;
        if (netI(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            var vd = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) vd[k] = r[k];
            vd.Id = S.recId > 0 ? netI(r.Id) : 0;
            S.removed.push(vd);
        }
        S.rows.splice(i, 1);
        S.cur = -1;
        render();
        insertTotals();
    }

    // ------------------------------------------------------------------------------ entry panel (panel6)

    function bindUoms(itemId) {
        return getJson(API + '/uoms?itemId=' + netI(itemId)).then(function (rows) {
            L.uoms = rows || [];
            if (L.uoms.length) { bind('combitempck', L.uoms, 'Id', 'UOMCode', false); bind('combrateuom', L.uoms, 'Id', 'UOMCode', false); }
            else { bind('combitempck', [], 'Id', 'UOMCode', false); bind('combrateuom', [], 'Id', 'UOMCode', false); }
        }).catch(function (e) { box(e.message); });
    }
    /** CalculateWeight. */
    function calculateWeight() {
        if (netD(val('txtNoOfContainersDetail')) > 0 && netD(val('txtNoOfBAgsPerCntnr')) > 0) {
            var bags = netD(val('txtNoOfContainersDetail')) * netD(val('txtNoOfBAgsPerCntnr'));
            setText('txtNoOfBags', clr(bags));
            var pw = netD(val('txtPackingWeight'));
            setText('txtPackingTotalWeight', clr(bags * pw));
            var u = find(L.uoms, val('combitempck'));
            if (u) {
                var eq = netD(u.Equivalent);
                setText('txtQtyMTon', clr(bags * eq / 1000.0));
                setText('txtGrossWeightdetail', clr(bags * eq + bags * pw));
            } else { setText('txtNoOfBags', '0'); setText('txtQtyMTon', '0'); }
        }
    }
    /** CalculatAmount: Math.Round(rate / rate-uom equivalent * M.Ton * 1000), then CalculateCommission. */
    function calculatAmount() {
        setText('txtAmount', '0');
        var u = find(L.uoms, val('combrateuom'));
        if (u) {
            var rate = netD(val('txtCostMTon')), eq = netD(u.Equivalent), qty = netD(val('txtQtyMTon'));
            if (rate > 0 && eq > 0 && qty > 0) setText('txtAmount', commas(bankersRound(rate / eq * qty * 1000.0)));
        }
        calculateCommission();
    }
    function recalc() { calculateWeight(); calculatAmount(); }
    /** combpcktype_Leave: the packing type's PackingWeight, else 0. */
    function packTypeLeave() {
        var p = find(L.packTypes, val('combpcktype'));
        setText('txtPackingWeight', p && netD(p.PackingWeight) > 0 ? clr(p.PackingWeight) : '0');
        recalc();
    }
    function detailFormValidation() {
        function need(ok, msg, id) { if (!ok) { box(msg); focus(id); return false; } return true; }
        if (!need(netI(val('combitem')) > 0, 'Item field required', 'combitem')) return false;
        if (!need(!!find(L.uoms, val('combitempck')), 'Pack Type field required', 'combitempck')) return false;
        if (!need(netD(val('txtPackingWeight')) !== 0, 'Packing Weight field required', 'txtPackingWeight')) return false;
        if (!need(netI(val('CmbCropYear')) > 0, 'CropYear field required', 'CmbCropYear')) return false;
        if (!need(netD(val('txtNoOfContainersDetail')) !== 0, 'NoOfContainers field required', 'txtNoOfContainersDetail')) return false;
        if (!need(netD(val('txtNoOfBAgsPerCntnr')) !== 0, 'NoOfBagsPerCntnr field required', 'txtNoOfBAgsPerCntnr')) return false;
        if (!need(!!find(L.uoms, val('combitempck')), 'Pack Size field required', 'combitempck')) return false;
        if (!need(netD(val('txtQtyMTon')) !== 0, 'M.Ton field required', 'txtQtyMTon')) return false;
        if (!need(netD(val('txtNoOfBags')) !== 0, 'No of Bags field required', 'txtNoOfBags')) return false;
        if (!need(netD(val('txtGrossWeightdetail')) !== 0, 'Gross Weight field required', 'txtGrossWeightdetail')) return false;
        if (!need(netD(val('txtCostMTon')) !== 0, 'Rate field required', 'txtCostMTon')) return false;
        if (!need(!!find(L.uoms, val('combrateuom')), 'Rate UOM field required', 'combrateuom')) return false;
        if (!need(netD(val('txtAmount')) !== 0, 'Amount field required', 'txtAmount')) return false;
        return true;
    }
    function entryRow() {
        var pt = find(L.packTypes, val('combpcktype'));
        return { ItemId: netI(val('combitem')), ItemName: txt('combitem'), PackTypeId: netI(val('combpcktype')), PackType: pt ? str(pt.Description) : '',
            PackingWeight: netD(val('txtPackingWeight')), CropYearId: netI(val('CmbCropYear')), CropYear: txt('CmbCropYear'),
            NoOfContainers: netD(val('txtNoOfContainersDetail')), NoOfBagsCntnr: netD(val('txtNoOfBAgsPerCntnr')), PackSizeId: netI(val('combitempck')),
            PackSize: txt('combitempck'), QtyMTon: netD(val('txtQtyMTon')), NoOfBags: netD(val('txtNoOfBags')), PackingTotalWeight: netD(val('txtPackingTotalWeight')),
            GrossWeight: netD(val('txtGrossWeightdetail')), Rate: netD(val('txtCostMTon')), RateUOMId: netI(val('combrateuom')), RateUOM: txt('combrateuom'),
            Amount: netD(val('txtAmount')), PackingExpiryDate: val('txtexpirydatedetail'), ContainerSize: val('txtcontainersizedetail'), Remarks: val('txtRemarks'),
            PackingDetail: val('txtPackingDetail') };
    }
    /** ResetDetail (also clears the header No Of Containers - desktop). */
    function resetDetail() {
        setVal('combitem', ''); setVal('combpcktype', ''); setVal('combitempck', ''); setVal('CmbCropYear', ''); setVal('combrateuom', '');
        ['txtQtyMTon', 'txtNoOfBags', 'txtNoOfBAgsPerCntnr', 'txtCostMTon', 'txtGrossWeightdetail', 'txtPackingWeight', 'txtPackingTotalWeight',
         'txtexpirydatedetail', 'txtNoOfContainers', 'txtAmount', 'txtRemarks'].forEach(function (id) { setText(id, ''); });
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        S.updateIdx = -1;
    }
    /** btnplus_Click -> AddtoGrid, CalculateCommission. */
    function btnplus() {
        if (!detailFormValidation()) return;
        var r = entryRow(); r.Id = 0;
        S.rows.push(r);
        render(); resetDetail(); focus('combitem'); insertTotals();
        calculateCommission();
    }
    /** btnUpdateDetail_Click. */
    function btnUpdateDetail() {
        if (!detailFormValidation()) return;
        var old = S.rows[S.updateIdx]; if (!old) return;
        var r = entryRow(); r.Id = old.Id;
        S.rows[S.updateIdx] = r;
        render();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        resetDetail(); focus('combitem'); insertTotals(); calculateCommission();
    }
    function btnCancelUpdateDetial() { show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); resetDetail(); }
    /** grdDetails_DoubleClick. */
    function rowToEntry(i) {
        var r = S.rows[i]; if (!r) return;
        S.updateIdx = i;
        setVal('combitem', r.ItemId);
        return bindUoms(r.ItemId).then(function () {
            setVal('combpcktype', r.PackTypeId);
            setText('txtPackingWeight', clr(r.PackingWeight));
            setVal('CmbCropYear', r.CropYearId);
            setText('txtNoOfContainersDetail', clr(r.NoOfContainers));
            setText('txtNoOfBAgsPerCntnr', clr(r.NoOfBagsCntnr));
            setVal('combitempck', r.PackSizeId);
            setText('txtQtyMTon', clr(r.QtyMTon));
            setText('txtNoOfBags', clr(r.NoOfBags));
            setText('txtPackingTotalWeight', clr(r.PackingTotalWeight));
            setText('txtGrossWeightdetail', clr(r.GrossWeight));
            setVal('combrateuom', r.RateUOMId);
            setText('txtCostMTon', clr(r.Rate));
            setText('txtAmount', commas(r.Amount));
            setText('txtexpirydatedetail', r.PackingExpiryDate);
            setText('txtcontainersizedetail', r.ContainerSize);
            setText('txtRemarks', r.Remarks);
            setText('txtPackingDetail', r.PackingDetail);
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            subtab('tabContractDetail');
        });
    }

    // ------------------------------------------------------------------------------ Other Items

    function renderOther() {
        var rows = S.otherHidden ? [] : S.other, q = 0, a = 0;
        $id('pfOtherBody').innerHTML = rows.map(function (r, i) {
            q += netD(r.Qty); a += netD(r.Amount);
            return '<tr data-i="' + i + '"' + (i === S.otherIdx ? ' class="is-current"' : '') + '><td>' + esc(r.ItemName) + '</td><td class="num">' + esc(fmtNet(r.Qty, '0,0'))
                + '</td><td class="num">' + esc(fmtNet(r.Rate, '0,0')) + '</td><td class="num">' + esc(fmtNet(r.Amount, '0,0')) + '</td><td>' + esc(r.Remarks) + '</td></tr>';
        }).join('');
        $id('pfOtherFoot').innerHTML = rows.length ? '<tr><td class="lbl">&Sigma;</td><td>' + esc(fmtNet(q, '#,##0.##')) + '</td><td></td><td>' + esc(fmtNet(a, '#,##0.##')) + '</td><td class="lbl"></td></tr>' : '';
    }
    function otherItemsCalculations() {
        var q = netD(val('txtoQty')), r = netD(val('txtoRate'));
        if (q > 0 && r > 0) setText('txtoAmount', clr(q * r));
    }
    function otherItemFormValidation() {
        if (netI(val('CmbOtherItem')) === 0) { box('Item field required'); focus('CmbOtherItem'); return false; }
        if (netD(val('txtoQty')) === 0) { box('Qty field required'); focus('txtoQty'); return false; }
        if (netD(val('txtoRate')) === 0) { box('Rate field required'); focus('txtoRate'); return false; }
        if (netD(val('txtoAmount')) === 0) { box('Amount field required'); focus('txtoAmount'); return false; }
        return true;
    }
    function resetOtherItems() { setVal('CmbOtherItem', ''); setText('txtoQty', ''); setText('txtoRate', ''); setText('txtoAmount', ''); setText('txtoRemarks', ''); }
    function btnoadd() {
        if (!otherItemFormValidation()) return;
        S.otherHidden = false;
        S.other.push({ ItemId: netI(val('CmbOtherItem')), ItemName: txt('CmbOtherItem'), Qty: netD(val('txtoQty')), Rate: netD(val('txtoRate')),
            Amount: netD(val('txtoAmount')), Remarks: val('txtoRemarks').trim() });
        renderOther(); resetOtherItems(); insertTotals();
    }
    function btnoUpdate() {
        if (!otherItemFormValidation()) return;
        var r = S.other[S.otherIdx]; if (!r) return;
        r.ItemId = netI(val('CmbOtherItem')); r.ItemName = txt('CmbOtherItem'); r.Qty = netD(val('txtoQty')); r.Rate = netD(val('txtoRate'));
        r.Amount = netD(val('txtoAmount')); r.Remarks = val('txtoRemarks');
        show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false);
        resetOtherItems(); focus('txtoQty'); renderOther(); insertTotals();
    }
    function btnoCancel() { show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false); resetOtherItems(); }
    function otherToEntry(i) {
        var r = S.other[i]; if (!r) return;
        S.otherIdx = i;
        setVal('CmbOtherItem', r.ItemId); setText('txtoQty', clr(r.Qty)); setText('txtoRate', clr(r.Rate)); setText('txtoAmount', clr(r.Amount)); setText('txtoRemarks', r.Remarks);
        show('btnoadd', false); show('btnoUpdate', true); show('btnoCancel', true); focus('txtoQty');
    }

    // ------------------------------------------------------------------------------ reset / read / save

    /** Reset() (+ ResetDetail from btnnew). */
    function reset() {
        S.recId = 0;
        setVal('cmbCommAgent', ''); setText('txtCommAmount', ''); setText('txtCommPercent', '');
        ['CmbCustomer', 'CmbNotifyParty', 'CmbFCYCode', 'CmbDeliveryTerm', 'CmbPaymentTerms', 'CmbLoadingPort', 'CmbDestinationPort', 'CmbImporterBank',
         'CmbExporterbank', 'combitem', 'combitempck', 'combrateuom', 'combpcktype'].forEach(function (id) { setVal(id, ''); });
        ['txtFCYAmount', 'txtSpecialInstructions', 'txtMTon', 'txtCommodityDetail', 'txtInsuraneRemarks', 'txtQtyMTon', 'txtBuyerOrderNo', 'txtLotRefNo',
         'txtNoOfContainers', 'txtProductSpecification', 'txtcontainersizedetail'].forEach(function (id) { setText(id, ''); });
        show('btnsave', true); show('btnUpdate', false); show('btnSaveAs', false);
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        S.rows = []; S.cur = -1; render();
        S.otherHidden = true; renderOther();
        focus('txtDocDate');
        subtab('tabContractDetail');
        return getJson(API + '/new-code').then(function (d) { if (d && d.docNo) setText('txtExportSoNo', d.docNo); }).catch(function (e) { box(e.message); });
    }
    function btnnew() { var p = reset(); resetDetail(); return p; }
    /** toolStripButton1_Click (Refresh). */
    function refresh(btn) {
        return busy(btn, function () { return getJson(API + '/refresh').then(function (d) { bindLookups(d || {}); }).catch(function (e) { box(e.message); }); });
    }
    /** ReadById(ID). */
    function readById(id, saveAs) {
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            d = d || {};
            var h = d.header || {};
            show('btnsave', false); show('btnSaveAs', false); show('btnUpdate', true);
            S.recId = id; S.removed = [];
            tab('pfForm');
            var st = null; STATUS.forEach(function (s) { if (s.Status === h.Status) st = s.Id; }); setVal('cmbStatus', st);
            setText('txtExportSoNo', h.LcOrderDocNo); setText('txtDocDate', isoDate(h.LcOrderDate));
            setVal('CmbSalePerson', h.SalesPersonId); setVal('CmbCustomer', h.SupCustId); setVal('CmbNotifyParty', h.NotifyPartyId); setVal('cmbCommAgent', h.CommissionAgentId);
            setText('txtLotRefNo', h.LotReference); setText('txtCommPercent', h.CommRate); setText('txtCommAmount', commas(h.CommAmount));
            setText('txtBuyerOrderNo', h.LcOrderNo); setText('datOrderDate', isoDate(h.SalesContratDate)); setText('txtShipmentDate', isoDate(h.LastShipmentDate));
            setVal('CmbDeliveryTerm', h.DeliveryTermId); setVal('CmbPaymentTerms', h.PaymentTermId); setVal('CmbLoadingPort', h.LoadingPortId);
            setVal('CmbDestinationPort', h.DestinationPortId); setVal('CmbImporterBank', h.ImporterBankId); setVal('CmbExporterbank', h.ExporterBankId);
            setText('txtNoOfContainers', h.NoOfContainers); setText('txtInsuraneRemarks', h.InsuranceRemarks); setText('txtcontainersizedetail', h.ContainerSize);
            setVal('CmbFCYCode', h.FcurrencyId); setText('txtFCYAmount', commas(h.FCurrencyAmount)); setText('txtMTon', h.NetWeightKgs);
            setText('txtCommodityDetail', h.CommodityDetial); setText('txtSpecialInstructions', h.RemarksHeader); setText('txtProductSpecification', h.ProductSpecification);
            $id('ChkOnCommission').checked = netI(h.ContractType) === 1;
            S.rows = d.rows || []; S.cur = -1; render();
            S.other = d.otherItems || []; S.otherHidden = false; renderOther();
            if (saveAs) { show('btnSaveAs', true); show('btnsave', false); show('btnUpdate', false); }
        }).catch(function (e) { box(e.message); });
    }
    function headerBody() {
        var h = {};
        ['txtExportSoNo', 'txtDocDate', 'CmbSalePerson', 'CmbCustomer', 'CmbNotifyParty', 'txtLotRefNo', 'txtBuyerOrderNo', 'datOrderDate', 'txtShipmentDate',
         'CmbDeliveryTerm', 'CmbPaymentTerms', 'CmbLoadingPort', 'CmbDestinationPort', 'CmbImporterBank', 'CmbExporterbank', 'txtNoOfContainers', 'CmbFCYCode',
         'txtFCYAmount', 'cmbCommAgent', 'txtCommPercent', 'txtCommAmount', 'txtMTon', 'txtCommodityDetail', 'txtSpecialInstructions', 'txtInsuraneRemarks',
         'txtProductSpecification'].forEach(function (id) { h[id] = val(id); });
        ['CmbSalePerson', 'CmbCustomer', 'CmbDeliveryTerm', 'CmbPaymentTerms', 'CmbLoadingPort', 'CmbDestinationPort', 'CmbFCYCode', 'cmbStatus'].forEach(function (id) { h[id + 'Text'] = txt(id); });
        return h;
    }
    /** FormValidation (page copy, so the message order matches; the server checks again). */
    function formValidation() {
        function need(ok, msg, id) { if (!ok) { box(msg); focus(id); return false; } return true; }
        var doc = val('txtExportSoNo').trim();
        if (!need(!(doc === '' || doc === '0'), 'DocNo Is required', 'txtExportSoNo')) return false;
        if (!need(txt('CmbSalePerson').trim() !== '', 'Sale Person Is required', 'CmbSalePerson')) return false;
        if (!need(txt('CmbCustomer').trim() !== '', 'Customer/Importer Is required', 'CmbCustomer')) return false;
        if (!need(val('txtBuyerOrderNo').trim() !== '', 'Proforma Ref # Is required', 'txtBuyerOrderNo')) return false;
        if (!need(txt('CmbDeliveryTerm').trim() !== '', 'DeliveryTerms Is  required', 'CmbDeliveryTerm')) return false;
        if (!need(txt('CmbPaymentTerms').trim() !== '', 'PaymentTerms Is required', 'CmbPaymentTerms')) return false;
        if (!need(txt('CmbLoadingPort').trim() !== '', 'LoadingPort Is required', 'CmbLoadingPort')) return false;
        if (!need(txt('CmbDestinationPort').trim() !== '', 'Destination Port  Is required', 'CmbDestinationPort')) return false;
        if (!need(txt('CmbDestinationPort').trim() !== txt('CmbLoadingPort').trim(), 'Destination Port and Loaing Port cannot be same...', 'CmbDestinationPort')) return false;
        var agent = netI(val('cmbCommAgent')) > 0;
        if (!need(!(netD(val('txtCommPercent')) > 0 && !agent), "As you have entered 'Commission %', the 'Commission Agent' field is required. ", 'cmbCommAgent')) return false;
        if (!need(!(netD(val('txtCommPercent')) === 0 && agent), "As you have selected 'Commission agent', please enter the commission percentage.", 'txtCommPercent')) return false;
        if (!need(txt('CmbFCYCode').trim() !== '', 'Fcy Code Is  required', 'CmbFCYCode')) return false;
        if (!need(netD(val('txtFCYAmount')) > 0 && val('txtFCYAmount').trim() !== '', 'FCY Amount Must Be Greater Than 0 ThankYou', 'txtFCYAmount')) return false;
        if (!need(netD(val('txtMTon')) > 0, 'M.Ton Must Be Greater Than 0 ThankYou', 'txtMTon')) return false;
        return true;
    }
    /** btnsave_Click (Update and Save As call it). */
    function btnsave(btn) {
        return busy(btn, function () {
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            var pre = S.recId === 0 ? getJson(API + '/new-code').then(function (d) { if (d && d.docNo) setText('txtExportSoNo', d.docNo); }) : Promise.resolve();
            return pre.then(function () {
                if (!formValidation()) return;
                if (!visible('btnUpdate') && $id('btnUpdate').disabled && txt('cmbStatus').trim() !== 'Open') { box('Status Should Be Open In Save Mode'); focus('cmbStatus'); return; }
                var win = $id('chkPrint').checked && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
                var body = { recId: S.recId, header: headerBody(), rows: S.rows, removed: S.removed, otherItems: S.otherHidden ? [] : S.other, updateVisible: visible('btnUpdate') };
                return postJson(API + '/save', body).then(function (d) {
                    box((d && d.message) || (S.recId === 0 ? 'Record Save Successfully' : 'Record Update Successfully'));
                    var id = (d && d.id) || 0;
                    return reset().then(function () {
                        if (win) { if (id) global.CrystalPrint.open('501-exprptsalescontractexport', { id: id, r: id }, null, win); else { global.CrystalPrint.release(win); box('No Record Found For Display'); } }
                    });
                }).catch(function (e) { if (win) global.CrystalPrint.release(win); box(e.message); });
            });
        });
    }
    /** btnSaveAs_Click: RecId = 0, then Save. */
    function btnSaveAs(btn) { S.recId = 0; S.removed = []; return btnsave(btn); }
    /** Print_Click: GenerateReport(RecId) -> ExportSalesContractExport501. */
    function print(btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        if (S.recId === 0) { box('No Record Found For Display'); return; }
        return global.CrystalPrint.open('501-exprptsalescontractexport', { id: S.recId, r: S.recId }, btn);
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = ['DocNo', 'DocDate', 'PriRef', 'PriRefDate', 'Customer', 'NotifyParty', 'LotRefNo', 'PaymentTerm', 'DeliveryTerm', 'NetMTon', 'FcyCode', 'FcyAmount',
        'NoOfContainers', 'LoadingPort', 'DestinationPort', 'InsuranceRemarks', 'NoOfAttachments'];
    var HFMT = { NetMTon: '#,#', FcyAmount: '#,#' };
    function histRender() {
        var sums = {};
        $id('pfHistBody').innerHTML = HIST.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === HCUR ? ' class="is-current"' : '') + '>';
            ['Print', 'Print523', 'Edit', 'SaveAs'].forEach(function (b) { h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-hact="' + b + '" data-i="' + i + '">' + b + '</button></td>'; });
            HCOLS.forEach(function (c) {
                var v = r[c], inner;
                if (HFMT[c]) { sums[c] = (sums[c] || 0) + netD(v); inner = fmtNet(v, HFMT[c]); }
                else if (c === 'DocDate' || c === 'PriRefDate') inner = shortDate(v);
                else if (c === 'NoOfContainers') inner = clr(v);
                else inner = str(v);
                inner = esc(inner);
                if (c === 'DocNo') inner = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + inner + '</a>';
                h += '<td' + (HFMT[c] || c === 'DocNo' || c === 'NoOfContainers' ? ' class="num"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        var foot = $id('pfHistFoot');
        if (!HIST.length) foot.innerHTML = '';
        else {
            var t = '<tr><td class="lbl" colspan="4">&Sigma;</td>';
            HCOLS.forEach(function (c) { t += '<td' + (HFMT[c] ? '' : ' class="lbl"') + '>' + (HFMT[c] ? esc(fmtNet(sums[c] || 0, HFMT[c])) : '') + '</td>'; });
            foot.innerHTML = t + '</tr>';
        }
        show('pfHistEmpty', HIST.length === 0);
        $id('pfHistDetailBody').innerHTML = ''; $id('pfHistDetailFoot').innerHTML = '';
    }
    /** btnShowHistory_Click -> HistoryFill. */
    function historyShow(btn) {
        return busy(btn, function () {
            var f = { dateBy: radio('dateBy'), fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'),
                toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'), fromDocNo: val('FromDocNoHistory'), toDocNo: val('ToDocNoHistory'),
                customerId: netI(val('CmbCustomerHistory')) };
            return postJson(API + '/history', f).then(function (rows) { HIST = rows || []; HCUR = -1; histRender(); }).catch(function (e) { box(e.message); });
        });
    }
    function historyReset() {
        setText('FromDateHistory', daysAgo(3)); setText('ToDateHistory', today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', ''); setVal('CmbCustomerHistory', '');
        HIST = []; HCUR = -1; histRender(); show('pfHistEmpty', false);
    }
    function historyRefresh(btn) {
        return busy(btn, function () { return getJson(API + '/history-combos').then(function (rows) { bind('CmbCustomerHistory', rows, 'Id', 'Customer'); }).catch(function (e) { box(e.message); }); });
    }
    /** DataGridHistory_SelectionChanged -> GetDetailGrdByHeadId. */
    function historySelect(i) {
        HCUR = i;
        var r = HIST[i]; if (!r) return;
        getJson(API + '/history-detail?id=' + netI(r.Id)).then(function (rows) {
            rows = rows || [];
            var s = { PackingWeight: 0, NoOfContainers: 0, QtyMTon: 0, NoOfBags: 0, PackingTotalWeight: 0, GrossWeight: 0, Amount: 0 };
            $id('pfHistDetailBody').innerHTML = rows.map(function (d) {
                for (var k in s) s[k] += netD(d[k]);
                return '<tr><td>' + esc(d.ItemName) + '</td><td>' + esc(d.PackType) + '</td><td class="num">' + esc(fmtNet(d.PackingWeight, '#,#.##')) + '</td><td>' + esc(d.CropYear)
                    + '</td><td class="num">' + esc(fmtNet(d.NoOfContainers, '#,#.##')) + '</td><td class="num">' + esc(clr(d.NoOfBagsCntnr)) + '</td><td>' + esc(d.PackSize)
                    + '</td><td class="num">' + esc(fmtNet(d.QtyMTon, '#,#')) + '</td><td class="num">' + esc(fmtNet(d.NoOfBags, '#,#')) + '</td><td class="num">'
                    + esc(fmtNet(d.PackingTotalWeight, '#,#.##')) + '</td><td class="num">' + esc(fmtNet(d.GrossWeight, '#,#')) + '</td><td class="num">' + esc(clr(d.Rate))
                    + '</td><td>' + esc(d.RateUOM) + '</td><td class="num">' + esc(fmtNet(d.Amount, '#,#')) + '</td><td>' + esc(d.PackingExpiryDate) + '</td><td>'
                    + esc(d.ContainerSize) + '</td><td>' + esc(d.Remarks) + '</td></tr>';
            }).join('');
            $id('pfHistDetailFoot').innerHTML = rows.length ? '<tr><td class="lbl">&Sigma;</td><td class="lbl"></td><td>' + esc(fmtNet(s.PackingWeight, '#,#.##')) + '</td><td class="lbl"></td><td>'
                + esc(fmtNet(s.NoOfContainers, '#,#.##')) + '</td><td></td><td class="lbl"></td><td>' + esc(fmtNet(s.QtyMTon, '#,#')) + '</td><td>' + esc(fmtNet(s.NoOfBags, '#,#'))
                + '</td><td>' + esc(fmtNet(s.PackingTotalWeight, '#,#.##')) + '</td><td>' + esc(fmtNet(s.GrossWeight, '#,#')) + '</td><td></td><td class="lbl"></td><td>'
                + esc(fmtNet(s.Amount, '#,#')) + '</td><td class="lbl"></td><td class="lbl"></td><td class="lbl"></td></tr>' : '';
        }).catch(function (e) { box(e.message); });
    }
    /** DataGridHistory_ColumnButtonClick. */
    function historyButton(act, i, btn) {
        var r = HIST[i]; if (!r) return;
        var id = netI(r.Id);
        if (act === 'Edit') { readById(id, false); return; }
        if (act === 'SaveAs') { readById(id, true); return; }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        if (act === 'Print') global.CrystalPrint.open('501-exprptsalescontractexport', { id: id, r: id }, btn);
        if (act === 'Print523') global.CrystalPrint.open('exp-523-contract', { id: id }, btn);
    }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs[data-tabs="pf"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('[data-subtabs="pf"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { subtab(b.getAttribute('data-subtab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
        on('combitem', 'change', function () { bindUoms(val('combitem')); });
        on('combpcktype', 'change', packTypeLeave);
        ['combitempck', 'combrateuom'].forEach(function (id) { on(id, 'change', recalc); });
        ['txtPackingWeight', 'txtNoOfContainersDetail', 'txtNoOfBAgsPerCntnr', 'txtCostMTon'].forEach(function (id) { on(id, 'input', recalc); });
        on('txtCommPercent', 'input', commPercentChanged);
        on('txtoQty', 'input', otherItemsCalculations);
        on('txtoRate', 'input', otherItemsCalculations);
        var gb = $id('pfBody');
        gb.addEventListener('click', function (e) {
            var d = e.target.closest('button[data-del]');
            if (d) { deleteRow(+d.getAttribute('data-del')); return; }
            var a = e.target.closest('a.win-code');
            if (a) { rowToEntry(+a.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            S.cur = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) rowToEntry(+tr.getAttribute('data-i')); });
        $id('pfOtherBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) otherToEntry(+tr.getAttribute('data-i')); });
        var hb = $id('pfHistBody');
        hb.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-hact]');
            if (b) { historyButton(b.getAttribute('data-hact'), +b.getAttribute('data-i'), b); return; }
            var a = e.target.closest('a.win-code');
            if (a) { historyButton('Edit', +a.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            historySelect(+tr.getAttribute('data-i'));
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) readById(netI(HIST[+tr.getAttribute('data-i')].Id), false); });
        /* FrmExportSalesContract_KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target);
                if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if (e.ctrlKey && k === 's' && !onHistory() && visible('btnsave')) { e.preventDefault(); btnsave($id('btnsave')); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnnew(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return; }
            if (e.ctrlKey && k === 'u' && visible('btnUpdate') && S.recId > 0) { e.preventDefault(); btnsave($id('btnUpdate')); }
        });
        load();
    });

    global.ExportPf = {
        btnnew: btnnew, btnsave: btnsave, btnSaveAs: btnSaveAs, print: print, refresh: refresh, toggleHistory: toggleHistory,
        btnplus: btnplus, btnUpdateDetail: btnUpdateDetail, btnCancelUpdateDetial: btnCancelUpdateDetial,
        btnoadd: btnoadd, btnoUpdate: btnoUpdate, btnoCancel: btnoCancel,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh
    };
}(window));
