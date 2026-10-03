/* ============================================================================================
 * countx_export_pre_invoice.js - two desktop forms that are one piece of code (Architecture.WinApp.Export):
 *   192 "Pre Invoice"            PreCommiercialInvoice              DocumentTypeId 209   page /export/pre-invoice
 *   194 "Export Opening Balance" CommiercialInvoiceForOpeningBalance DocumentTypeId 212   page /export/opening-balance
 * The page's body[data-form] picks the variant; every API call goes to /api/export/<variant>/... .
 *
 * Every button, Leave, TextChanged, grid button, double-click and shortcut of the desktop form has its counterpart
 * here, with the desktop's messages and order. Desktop quirks kept on purpose (see the comments where they apply):
 *   - Pre Invoice: grdDetail_ColumnButtonClick has no column-key check, so the "+" (Add) button runs the delete branch
 *     first (asks "Are you sure to Delete?" for a saved row) and then copies the row now at that index.
 *   - the entry panel's "+" never adds ("Sorry Cannot Add New Record Only Update Detail Record : Thank You").
 *   - RemarksHeader is saved from txtoRemarks (Other Items "Remarks"); txtremarksheader is display only.
 *   - FormReset does not clear txtoRemarks, does not re-enable Customer / Fcy Code after the loader, and (194) keeps
 *     the typed Invoice No.
 *   - Bank Print-546 prints the plain 546 slip on 192, the bank slip on 194.
 *   - closing the loader without loading clears the grid and the other items (LoadInGridDetail runs with no ids).
 * Button contract: disabled + spinner while a request runs, duplicates ignored, re-enabled on success and failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var FORM = (document.body && document.body.getAttribute('data-form')) || 'pre-invoice';
    var PRE = FORM === 'pre-invoice';
    var API = '/api/export/' + FORM;
    var DOCUMENT_TYPE_ID = PRE ? 209 : 212;
    var SCREEN_NAME = PRE ? 'PreCommiercialInvoice' : 'CommiercialInvoiceForOpeningBalance';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    /* Conversion.ToDouble(text): thousands separators allowed, anything unparsable is 0. */
    function netD(v) {
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim();
        if (!s || !/^[-+]?(\d+\.?\d*|\.\d+)([eE][-+]?\d+)?$/.test(s)) return 0;
        var n = parseFloat(s); return isFinite(n) ? n : 0;
    }
    /* double.ToString(): integral values without decimals, else up to 15 significant digits. */
    function clr(v) {
        var n = netD(v);
        if (n === Math.round(n) && Math.abs(n) < 1e15) return String(n);
        var s = Number(n.toPrecision(15)).toString();
        return s;
    }
    function roundAway(n, d) { var p = Math.pow(10, d); var r = Math.round(Math.abs(n) * p + 1e-9) / p; return n < 0 ? -r : r; }
    function group(intPart) { return intPart.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /* .NET custom numeric formats used by the form: '#,#.##' '#,#.###' '#,#' (0 -> ''), '#,##0.##' '#,##0.###', '0,0', '#,##0.00'. */
    function fmtNet(v, pattern) {
        var n = netD(v);
        var m = /\.(#+|0+)$/.exec(pattern);
        var dec = m ? m[1].length : 0, fixed = m ? m[1].charAt(0) === '0' : false;
        var r = roundAway(n, dec);
        var neg = r < 0; r = Math.abs(r);
        var s = r.toFixed(dec);
        var parts = s.split('.');
        var ip = parts[0], fp = parts[1] || '';
        if (!fixed) fp = fp.replace(/0+$/, '');
        if (pattern.indexOf('#,#') === 0 && pattern.indexOf('#,##0') !== 0) { if (ip === '0') ip = ''; }
        if (pattern === '0,0' && ip.length < 2) ip = ('00' + ip).slice(-2);
        ip = group(ip);
        var out = ip + (fp ? '.' + fp : '');
        if (out === '' ) return '';
        return (neg && out !== '0' ? '-' : '') + out;
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoDate(v) {
        var s = str(v).trim(); if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s); if (m) return m[1] + '-' + m[2] + '-' + m[3];
        return '';
    }
    /* GridEX DateTime column: short date (M/d/yyyy). */
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
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function bind(id, rows, valueCol, textCol, keep) {
        var s = $id(id); if (!s) return;
        var old = s.value;
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(col(r, valueCol)) + '">' + esc(col(r, textCol)) + '</option>'; });
        s.innerHTML = h;
        s.value = keep === false ? '' : old;
        if (s.value !== old && keep !== false) s.value = '';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; var t = (v === null || v === undefined || v === 0 || v === '0') ? '' : String(v); s.value = t; if (s.value !== t) s.value = ''; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function txt(id) { var s = $id(id); if (!s) return ''; if (s.tagName === 'SELECT') { var o = s.options[s.selectedIndex]; return o && s.value !== '' ? o.text : ''; } return s.value; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function radio(name) { var r = document.querySelector('input[name="' + name + '"]:checked'); return r ? r.value : ''; }
    function find(list, id) { id = netI(id); for (var i = 0; i < (list || []).length; i++) if (netI(col(list[i], 'Id')) === id) return list[i]; return null; }
    function cancel() { global.close(); setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150); }

    // ------------------------------------------------------------------------------ state

    var PERM = { Save: true, Update: true, Print: true };
    var DAYS = 0;
    var L = { packTypes: [], fis: [], otherList: [], packUoms: [], rateUoms: [] };
    var S = { recId: 0, updateMode: false, rows: [], removed: [], other: [], pay: [], updateIdx: -1, otherIdx: -1, payIdx: -1, flag: 0, cur: -1 };
    var HIST = [], HCUR = -1;
    var CARRIER = [{ Id: 1, Type: 'By Sea' }, { Id: 2, Type: 'By Air' }, { Id: 3, Type: 'By Road' }];

    // ------------------------------------------------------------------------------ tabs

    function tab(panelId) {
        document.querySelectorAll('.win-tabs[data-tabs="pi"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['piForm', 'piHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'piHistory';
        var fb = $id('btnPiFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        /* tabControl1_SelectedIndexChanged */
        if (onHist) focus('FromDateHistory'); else focus('txtDocDate');
    }
    function onHistory() { return $id('piHistory').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'piForm' : 'piHistory'); }
    function subtab(id) {
        document.querySelectorAll('[data-subtabs="pi"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-subtab') === id); });
        document.querySelectorAll('[data-subpanel="pi"]').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    // ------------------------------------------------------------------------------ load

    function bindLookups(d) {
        bind('cmbbranches', d.branches, 'Id', 'BranchName');
        bind('cmbproject', d.projects, 'Id', 'ProjectName');
        if (!PRE) bind('CmbCreditAccount', d.creditAccounts, 'Id', 'AccountTitle');
        ['cmbSupCust', 'cmbOtherCustomer', 'cmbNotifyParty1', 'cmbNotifyParty2', 'cmbCommissionAgent'].forEach(function (id) { bind(id, d.customers, 'Id', 'CompanyName'); });
        bind('cmbdeliverytermnew', d.deliveryTerms, 'Id', 'Code');
        bind('cmbPaymentTermsNew', d.paymentTerms, 'Id', 'LcOrderTerm');
        bind('dcmbpaymentterm', d.paymentTerms, 'Id', 'LcOrderTerm');
        ['cmbLoadingPort', 'cmbDestinationPort', 'cmbOtherDestinationPort'].forEach(function (id) { bind(id, d.ports, 'Id', 'PortName'); });
        bind('cmbfcycode', d.currencies, 'Id', 'CurrencyName');
        bind('cmbexporterBankNew', d.exporterBanks, 'Id', 'BranchName');
        bind('cmbimporterBankNew', d.importerBanks, 'Id', 'BranchName');
        bind('cmbcareiertype', CARRIER, 'Id', 'Type');
        L.packTypes = d.packTypes || []; bind('combpcktype', L.packTypes, 'Id', 'Description');
        bind('CmbCropYear', d.cropYears, 'Id', 'CropYear');
    }
    /* Rows[0].Activate() of the hidden Branch / Project combos and of the Carier Type combo. */
    function firstRows() {
        ['cmbbranches', 'cmbproject', 'cmbcareiertype'].forEach(function (id) { var s = $id(id); if (s && s.options.length > 1) s.selectedIndex = 1; });
        refreshCombos();
    }
    /** ImProformaInvoice_Load. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save;
            $id('btnupdate').disabled = !PERM.Update;
            bindLookups(d);
            firstRows();
            setText('txtdocno', d.docNo || '');
            if (PRE) setText('txtinvoiceno', d.invoiceNo || '');
            setText('txtDocDate', today());
            setText('txtIformDate', today());
            DAYS = netI(d.defaultDaysToLessFromHistoryFromDate);
            bind('CmbCustomerHistory', d.historyCustomers, 'Id', 'Customer');
            setText('FromDateHistory', DAYS > 0 ? daysAgo(DAYS) : daysAgo(3));
            setText('ToDateHistory', today());
            $id('ChkPrintPreview').checked = true;
            subtab('tabPage3');
            render();
            bindFinancialInstruments();
            $id('piFooterInfo').textContent = SCREEN_NAME + '  -  Document Type ' + DOCUMENT_TYPE_ID;
            focus(PRE ? 'txtDocDate' : 'txtinvoiceno');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    /** BindFinancialInstrument (cmbSupCust Leave / after ReadById): usp_GetFINoForInvoice for the customer. */
    function bindFinancialInstruments() {
        if (!$id('CmbFinInstruments')) return Promise.resolve();
        return getJson(API + '/financial-instruments?customerId=' + netI(val('cmbSupCust'))).then(function (rows) {
            L.fis = rows || [];
            bind('CmbFinInstruments', L.fis, 'Id', 'EFormNo');
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ header calculations

    /** CalculateLocalAmount: Total Fcy * Exchange Rate as "0,0", else "0". */
    function calculateLocalAmount() {
        var fcy = netD(val('txtTotalNetAmount')), rate = netD(val('txtExRate'));
        setText('txtLocalAmt', fcy > 0 && rate > 0 ? fmtNet(fcy * rate, '0,0') : '0');
    }
    function sumOf(list, c) { var s = 0; list.forEach(function (r) { s += netD(col(r, c)); }); return s; }
    /** TotalAmountCalculate: needs a current grid row; Total = Fcy + Add/Less + Other Items ("#,#.###"); payment rows re-spread by %. */
    function totalAmountCalculate() {
        if (!S.rows.length) return;
        var add = netD(val('txtaddlessamount')), fcy = netD(val('txtfcyAmount')), other = sumOf(visibleOther(), 'Amount');
        if (fcy > 0) setText('txtTotalNetAmount', fmtNet(fcy + add + other, '#,#.###'));
        var total = netD(val('txtTotalNetAmount'));
        if (total > 0 && S.pay.length) { S.pay.forEach(function (p) { p.FcyAmount = netD(p.PrcntOfTotal) * total / 100.0; }); renderPay(); }
        calculateLocalAmount();
    }
    /** getTotalnetWeightFromGrid: Net / Gross ("#,#.##"), Fcy-Amount ("#,#.##" - its TextChanged recomputes the total), containers. */
    function getTotalnetWeightFromGrid() {
        setText('txtNetWeight', fmtNet(sumOf(S.rows, 'NetWeight'), '#,#.##'));
        setText('txtGrossWeight', fmtNet(sumOf(S.rows, 'GrossWeight'), '#,#.##'));
        setText('txtfcyAmount', fmtNet(sumOf(S.rows, 'Amount'), '#,#.##'));
        setText('txtNoOfContainer', clr(sumOf(S.rows, 'NoOfContainers')));
        totalAmountCalculate();                              /* txtfcyAmount_TextChanged -> txtaddlessamount_TextChanged */
    }
    /** txtCommPercent_TextChanged: > 100 -> message and 100; else Math.Round(grid Amount * % / 100). */
    function commPercentChanged() {
        var rate = netD(val('txtCommPercent'));
        if (rate > 100) { box('Commission % Can not Greater than 100...'); setText('txtCommPercent', '100'); return commPercentChanged(); }
        if (S.rows.length) {
            var total = sumOf(S.rows, 'Amount');
            setText('txtCommAmount', isNaN(total) ? '0' : String(bankersRound(total * rate / 100.0)));
        }
    }
    function bankersRound(n) { var r = Math.round(n); if (Math.abs(n % 1) === 0.5) r = 2 * Math.round(n / 2); return r; }

    // ------------------------------------------------------------------------------ detail grid

    var DCOLS = ['PerformaDocNo', 'PerformaDate', 'PriRefNo', 'ItemName', 'ItemHSCode', 'PackType', 'PackingWeight', 'CropYear', 'NoOfContainers',
        'NoOfBagsCntnr', 'PackSize', 'QtyMTon', 'NoOfBags', 'NetWeight', 'TotalPackingWeight', 'GrossWeight', 'CostMTon', 'ContractRate', 'RateUOM',
        'Amount', 'OtherRate', 'OtherAmount', 'OtherHSCode', 'PackingExpiryDate', 'ItemCommodityDetail', 'HealthPermitNo'];
    /* grdSettings formats; Σ on M.Ton, NoOfBags, Amount, NetWeight, GrossWeight, TotalPackingWeight. */
    var DFMT = { QtyMTon: '#,#.###', NoOfBags: '#,#', Amount: '#,#.###', NetWeight: '0,0', GrossWeight: '0,0', TotalPackingWeight: '0,0', CostMTon: '#,#.###' };
    var DTOT = { QtyMTon: '#,##0.###', NoOfBags: '#,#', Amount: '#,##0.###', NetWeight: '#,##0.##', GrossWeight: '#,##0.##', TotalPackingWeight: '#,##0.##' };
    var DNUM = { PerformaDocNo: 1, PackingWeight: 1, NoOfContainers: 1, NoOfBagsCntnr: 1, QtyMTon: 1, NoOfBags: 1, NetWeight: 1, TotalPackingWeight: 1,
        GrossWeight: 1, CostMTon: 1, ContractRate: 1, Amount: 1, OtherRate: 1, OtherAmount: 1 };
    function dcell(c, v) {
        if (DFMT[c]) return fmtNet(v, DFMT[c]);
        if (c === 'PerformaDate') return shortDate(v);
        if (DNUM[c]) return clr(v);
        return str(v);
    }
    function render() {
        var sums = {};
        $id('piBody').innerHTML = S.rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === S.cur ? ' class="is-current"' : '') + '>';
            h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-act="Delete" data-i="' + i + '">Delete</button></td>';
            h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-act="Add" data-i="' + i + '">+</button></td>';
            DCOLS.forEach(function (c) {
                var v = col(r, c);
                if (DTOT[c]) sums[c] = (sums[c] || 0) + netD(v);
                var inner = esc(dcell(c, v));
                if (c === 'PriRefNo' && netI(r.PerformaId) > 0) inner = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)" title="Edit this row">' + inner + '</a>';
                h += '<td' + (DNUM[c] ? ' class="num"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        var foot = $id('piFoot');
        if (!S.rows.length) { foot.innerHTML = ''; return; }
        var t = '<tr><td class="lbl" colspan="2">&Sigma;</td>';
        DCOLS.forEach(function (c) { t += '<td' + (DTOT[c] ? '' : ' class="lbl"') + '>' + (DTOT[c] ? esc(fmtNet(sums[c] || 0, DTOT[c])) : '') + '</td>'; });
        foot.innerHTML = t + '</tr>';
    }

    /**
     * grdDetail_ColumnButtonClick. 194 checks e.Column.Key == "Delete"; 192 does not (desktop quirk), so on 192 the "+"
     * button first runs the delete branch and then copies the row that is now at the same index.
     */
    function columnButton(act, i) {
        var r = S.rows[i]; if (!r) return;
        var deleteBranch = PRE ? true : act === 'Delete';
        var idx = i;
        if (deleteBranch) {
            if (netI(r.Id) > 0) {
                if (!ask(PRE ? 'Are you sure to Delete?' : 'Are you sure to Delete the row?')) return;
                var vd = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) vd[k] = r[k];
                S.removed.push(vd);
            }
            S.rows.splice(i, 1);
            if (!PRE) getTotalnetWeightFromGrid();
        }
        if (act === 'Add') {
            var src = S.rows[idx];
            if (!src) { render(); box('There is no row at position ' + idx + '.'); getTotalnetWeightFromGrid(); return; }
            var copy = {}; for (var k2 in src) if (Object.prototype.hasOwnProperty.call(src, k2)) copy[k2] = src[k2];
            copy.Id = 0;
            S.rows.push(copy);
        }
        S.cur = -1;
        render();
        getTotalnetWeightFromGrid();
        calculateLocalAmount();
        commPercentChanged();
    }

    // ------------------------------------------------------------------------------ entry panel (GroupDetail)

    function bindUoms(itemId) {
        return getJson(API + '/uoms?itemId=' + netI(itemId)).then(function (rows) {
            rows = rows || [];
            L.packUoms = rows; L.rateUoms = rows;
            bind('CmbPackUom', rows, 'Id', 'UOMCode', false);
            bind('CmbRateUom', rows, 'Id', 'UOMCode', false);
        }).catch(function (e) { box(e.message); });
    }
    /** CalculateWeight. */
    function calculateWeight() {
        if (netD(val('txtNoOfContainersDetail')) > 0 && netD(val('txtNoOfBagsPerCntnr')) > 0) {
            var bags = netD(val('txtNoOfContainersDetail')) * netD(val('txtNoOfBagsPerCntnr'));
            setText('txtNoOfBags', clr(bags));
            var pw = netD(val('txtPackingWeight'));
            setText('txttotalpackingweight', clr(bags * pw));
            var u = find(L.packUoms, val('CmbPackUom'));
            if (u) {
                var eq = netD(col(u, 'Equivalent'));
                setText('txtQtyMTon', clr(bags * eq / 1000.0));
                setText('txtNetWeightdetail', clr(bags * eq));
                setText('txtgrossWeightdetail', clr(bags * eq + bags * pw));
            } else { setText('txtNoOfBags', '0'); setText('txtQtyMTon', '0'); }
        }
    }
    /** CalculatAmount. */
    function calculatAmount() {
        setText('txtAmount', '0');
        var u = find(L.rateUoms, val('CmbRateUom'));
        if (u) {
            var rate = netD(val('txtCostMTon')), other = netD(val('txtOtherRateDetail')), eq = netD(col(u, 'Equivalent')), qty = netD(val('txtQtyMTon'));
            if (rate > 0 && eq > 0 && qty > 0) {
                var kg = qty * 1000.0;
                setText('txtAmount', clr(rate / eq * kg));
                setText('txtOtherAmountDetil', clr(other / eq * kg));
            }
        }
    }
    function recalc() { calculateWeight(); calculatAmount(); }
    /** txtOtherRateDetail_TextChanged: recalculation, then decimal.Parse(Bank Amount).ToString("#,##0.00") - an empty box raises the parse error. */
    function otherRateChanged() {
        recalc();
        var s = val('txtOtherAmountDetil').replace(/,/g, '').trim();
        if (!/^[-+]?(\d+\.?\d*|\.\d+)$/.test(s)) { box('Input string was not in a correct format.'); return; }
        setText('txtOtherAmountDetil', fmtNet(parseFloat(s), '#,##0.00'));
    }
    /** DetailFormReset. */
    function detailFormReset() {
        ['txtQtyMTon', 'txtNoOfBags', 'txtCostMTon', 'txtAmount', 'txtOtherRateDetail', 'txtOtherHSCodeDetail', 'txtItemHsCodeDetail', 'txtOtherAmountDetil',
         'txtNoOfContainersDetail', 'txtNoOfBagsPerCntnr', 'txtweightcheck', 'txtqtycheck', 'txttotalpackingweight', 'txtPackingWeight', 'txtNetWeightdetail',
         'txtgrossWeightdetail', 'txtPackingExpiryDate'].forEach(function (id) { setText(id, ''); });
        setVal('combitem', ''); setVal('combpcktype', ''); setVal('CmbPackUom', ''); setVal('CmbRateUom', '');
        show('btnAddinGrid', true); show('btnGridUpdate', false); show('btnCancel', false);
        setEnabled('combitem', true);
        $id('txtCostMTon').readOnly = false;
        S.updateIdx = -1;
        subtab('tabPage3');
    }
    /** grdDetail_DoubleClick: the row into the entry panel (item combo bound to that item only). */
    function rowToEntry(i) {
        var r = S.rows[i]; if (!r) return;
        S.updateIdx = i;
        bind('combitem', [{ ItemId: r.ItemId, ItemName: r.ItemName }], 'ItemId', 'ItemName', false);
        setVal('combitem', r.ItemId);
        return bindUoms(r.ItemId).then(function () {
            setText('txtItemHsCodeDetail', r.ItemHSCode);
            setVal('combpcktype', r.PackTypeId);
            setText('txtPackingWeight', clr(r.PackingWeight));
            setVal('CmbCropYear', r.CropYearId);
            setText('txtNoOfContainersDetail', clr(r.NoOfContainers));
            setText('txtNoOfBagsPerCntnr', clr(r.NoOfBagsCntnr));
            setVal('CmbPackUom', r.PackSizeId);
            setText('txtQtyMTon', clr(r.QtyMTon));
            setText('txtNoOfBags', clr(r.NoOfBags));
            setText('txtqtycheck', clr(r.ValidateBags));
            setText('txtNetWeightdetail', clr(r.NetWeight));
            setText('txtweightcheck', clr(r.ValidateNetWeight));
            setText('txttotalpackingweight', clr(r.TotalPackingWeight));
            setText('txtgrossWeightdetail', clr(r.GrossWeight));
            setVal('CmbRateUom', r.RateUOMId);
            setText('txtCostMTon', clr(r.CostMTon));
            setText('txtAmount', clr(r.Amount));
            setText('txtOtherRateDetail', clr(r.OtherRate));
            setText('txtOtherAmountDetil', clr(r.OtherAmount));
            setText('txtOtherHSCodeDetail', r.OtherHSCode);
            setText('txtPackingExpiryDate', r.PackingExpiryDate);
            setText('txtItemCommodityDetail', r.ItemCommodityDetail);
            setText('txtHealthPermitnodetail', r.HealthPermitNo);
            show('btnAddinGrid', false); show('btnGridUpdate', true); show('btnCancel', true);
            /* InvoiceUpdateFlag (ContractType of the first saved row; 0 in a new document) == 0 -> the rate is read-only. */
            $id('txtCostMTon').readOnly = S.flag === 0;
            calculatAmount();
            calculateLocalAmount();
            subtab('tabPage3');
        });
    }
    /** DetailFormValidation - the desktop's order and texts. */
    function detailFormValidation() {
        function need(ok, msg, id) { if (!ok) { box(msg); focus(id); return false; } return true; }
        if (!need(netI(val('combitem')) > 0, 'Item field required', 'combitem')) return false;
        if (!need(!!find(L.packUoms, val('CmbPackUom')), 'Pack Type field required', 'CmbPackUom')) return false;
        if (!need(val('txtPackingWeight').trim() !== '' && netD(val('txtPackingWeight')) > 0, 'PackingWeight field required', 'txtPackingWeight')) return false;
        if (!need(val('txtNoOfContainersDetail').trim() !== '' && netD(val('txtNoOfContainersDetail')) > 0, 'NoOfContainer field required', 'txtNoOfContainersDetail')) return false;
        if (!need(val('txtNoOfBagsPerCntnr').trim() !== '' && netD(val('txtNoOfBagsPerCntnr')) > 0, 'NoOfBagsPerCntnr field required', 'txtNoOfBagsPerCntnr')) return false;
        if (!need(val('txtQtyMTon').trim() !== '' && netD(val('txtQtyMTon')) > 0, 'Qty/M.Ton field required', 'txtQtyMTon')) return false;
        if (!need(val('txtNoOfBags').trim() !== '' && netD(val('txtNoOfBags')) > 0, 'No of Bags field required', 'txtNoOfBags')) return false;
        if (!need(val('txtNetWeightdetail').trim() !== '' && netD(val('txtNetWeightdetail')) > 0, 'Net Weight field required', 'txtNetWeightdetail')) return false;
        if (!need(val('txttotalpackingweight').trim() !== '' && netD(val('txttotalpackingweight')) > 0, 'Total Packing Weight field required', 'txttotalpackingweight')) return false;
        if (!need(val('txtgrossWeightdetail').trim() !== '' && netD(val('txtgrossWeightdetail')) > 0, 'Gross Weight field required', 'txtgrossWeightdetail')) return false;
        if (!need(val('txtCostMTon').trim() !== '' && netD(val('txtCostMTon')) > 0, 'Cost/M.Ton field required', 'txtCostMTon')) return false;
        if (!need(!!find(L.rateUoms, val('CmbRateUom')), 'Rate UOM field required', 'CmbRateUom')) return false;
        if (!need(val('txtAmount').trim() !== '' && netD(val('txtAmount')) > 0, 'Amount field required', 'txtAmount')) return false;
        if (!need(val('txtTotalNetAmount').trim() !== '' && netD(val('txtTotalNetAmount')) > 0, 'Total Net Amount  field required', 'txtTotalNetAmount')) return false;
        return true;
    }
    /** btnAddinGrid_Click - the desktop never adds from the panel. */
    function btnAddinGrid() { box('Sorry Cannot Add New Record Only Update Detail Record : Thank You'); }
    /** btnGridUpdate_Click. */
    function btnGridUpdate() {
        if (!detailFormValidation()) return;
        if (visible('btnsave') && !$id('btnsave').disabled && netD(val('txtNetWeightdetail')) > netD(val('txtweightcheck'))) {
            box('Net Weight Can not be greater than Bal Weight...\n Balance Net Weight Is ' + val('txtweightcheck'));
            focus('txtNoOfContainersDetail');
            return;
        }
        var r = S.rows[S.updateIdx]; if (!r) return;
        r.ItemId = netI(val('combitem')); r.ItemName = txt('combitem'); r.ItemHSCode = val('txtItemHsCodeDetail');
        r.PackTypeId = netI(val('combpcktype')); r.PackType = txt('combpcktype'); r.PackingWeight = netD(val('txtPackingWeight'));
        r.CropYearId = netI(val('CmbCropYear')); r.CropYear = txt('CmbCropYear');
        r.NoOfContainers = netD(val('txtNoOfContainersDetail')); r.NoOfBagsCntnr = netD(val('txtNoOfBagsPerCntnr'));
        r.PackSizeId = netI(val('CmbPackUom')); r.PackSize = txt('CmbPackUom'); r.QtyMTon = netD(val('txtQtyMTon')); r.NoOfBags = netD(val('txtNoOfBags'));
        r.NetWeight = netD(val('txtNetWeightdetail')); r.TotalPackingWeight = netD(val('txttotalpackingweight')); r.GrossWeight = netD(val('txtgrossWeightdetail'));
        r.CostMTon = netD(val('txtCostMTon')); r.RateUOMId = netI(val('CmbRateUom')); r.RateUOM = txt('CmbRateUom'); r.Amount = netD(val('txtAmount'));
        r.OtherRate = netD(val('txtOtherRateDetail')); r.OtherAmount = netD(val('txtOtherAmountDetil')); r.OtherHSCode = val('txtOtherHSCodeDetail');
        r.ItemCommodityDetail = val('txtItemCommodityDetail'); r.HealthPermitNo = val('txtHealthPermitnodetail'); r.PackingExpiryDate = val('txtPackingExpiryDate');
        render();
        getTotalnetWeightFromGrid();
        detailFormReset();
        getTotalnetWeightFromGrid();
        calculateLocalAmount();
        commPercentChanged();
        focus('combitem');
    }
    function btnCancel() { detailFormReset(); }

    // ------------------------------------------------------------------------------ Other Items (192)

    function visibleOther() { return S.other; }
    function renderOther() {
        if (!$id('piOtherBody')) return;
        var q = 0, a = 0;
        $id('piOtherBody').innerHTML = S.other.map(function (r, i) {
            q += netD(r.Qty); a += netD(r.Amount);
            return '<tr data-i="' + i + '"' + (i === S.otherIdx ? ' class="is-current"' : '') + '><td>' + esc(r.ItemName) + '</td><td class="num">' + esc(fmtNet(r.Qty, '0,0'))
                + '</td><td class="num">' + esc(fmtNet(r.Rate, '0,0')) + '</td><td class="num">' + esc(fmtNet(r.Amount, '0,0')) + '</td><td>' + esc(r.Remarks) + '</td></tr>';
        }).join('');
        $id('piOtherFoot').innerHTML = S.other.length ? '<tr><td class="lbl">&Sigma;</td><td>' + esc(fmtNet(q, '#,##0.##')) + '</td><td></td><td>' + esc(fmtNet(a, '#,##0.##')) + '</td><td class="lbl"></td></tr>' : '';
    }
    function otherItemsCalculations() {
        var q = netD(val('txtoQty')), r = netD(val('txtoRate'));
        setText('txtoAmount', q > 0 && r > 0 ? clr(q * r) : '0');
    }
    function otherItemFormValidation() {
        if (netI(val('CmbOtherItem')) === 0) { box('Item field required'); focus('CmbOtherItem'); return false; }
        if (netD(val('txtoRate')) === 0) { box('Rate field required'); focus('txtoRate'); return false; }
        return true;
    }
    function resetOtherItems() { S.otherIdx = -1; setVal('CmbOtherItem', ''); setText('txtoQty', ''); setText('txtoRate', ''); setText('txtoAmount', ''); setText('txtoRemarks', ''); }
    function btnoadd() {
        if (!otherItemFormValidation()) return;
        S.other.push({ ItemId: netI(val('CmbOtherItem')), ItemName: txt('CmbOtherItem'), Qty: netD(val('txtoQty')), Rate: netD(val('txtoRate')),
            Amount: netD(val('txtoAmount')), Remarks: val('txtoRemarks').trim() });
        renderOther(); resetOtherItems(); totalAmountCalculate();
    }
    function btnoUpdate() {
        if (!otherItemFormValidation()) return;
        var r = S.other[S.otherIdx]; if (!r) return;
        r.ItemId = netI(val('CmbOtherItem')); r.ItemName = txt('CmbOtherItem'); r.Qty = netD(val('txtoQty')); r.Rate = netD(val('txtoRate'));
        r.Amount = netD(val('txtoAmount')); r.Remarks = val('txtoRemarks');
        show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false);
        resetOtherItems(); renderOther(); focus('txtoQty'); totalAmountCalculate();
    }
    function btnoCancel() { show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false); resetOtherItems(); }
    function otherToEntry(i) {
        var r = S.other[i]; if (!r) return;
        S.otherIdx = i;
        setVal('CmbOtherItem', r.ItemId); setText('txtoQty', clr(r.Qty)); setText('txtoRate', clr(r.Rate)); setText('txtoAmount', clr(r.Amount)); setText('txtoRemarks', r.Remarks);
        show('btnoadd', false); show('btnoUpdate', true); show('btnoCancel', true); focus('txtoQty'); renderOther();
    }
    /** CmbOtherItem_TextChanged: in update mode the item's rate (third column) into Rate. */
    function otherItemChanged() {
        if (visible('btnupdate') && !$id('btnupdate').disabled) {
            var r = null; L.otherList.forEach(function (x) { if (netI(x.ItemId) === netI(val('CmbOtherItem'))) r = x; });
            if (r) { setText('txtoRate', clr(r.Rate)); otherItemsCalculations(); }
        }
    }

    // ------------------------------------------------------------------------------ Payment Detail (192)

    function renderPay() {
        if (!$id('piPayBody')) return;
        var p = 0, f = 0;
        $id('piPayBody').innerHTML = S.pay.map(function (r, i) {
            p += netD(r.PrcntOfTotal); f += netD(r.FcyAmount);
            return '<tr data-i="' + i + '"' + (i === S.payIdx ? ' class="is-current"' : '') + '><td>' + esc(r.FinancialInstrumentNo) + '</td><td>' + esc(r.PaymentTerm)
                + '</td><td class="num">' + esc(fmtNet(r.PrcntOfTotal, '#,#.##')) + '</td><td class="num">' + esc(fmtNet(r.FcyAmount, '#,#.##')) + '</td><td class="num">' + esc(clr(r.DueDays)) + '</td></tr>';
        }).join('');
        $id('piPayFoot').innerHTML = S.pay.length ? '<tr><td class="lbl">&Sigma;</td><td class="lbl"></td><td>' + esc(fmtNet(p, '#,#.##')) + '</td><td>' + esc(fmtNet(f, '#,#.##')) + '</td><td></td></tr>' : '';
    }
    function resePaymentDetails() {
        S.payIdx = -1;
        setVal('CmbFinInstruments', ''); setVal('dcmbpaymentterm', '');
        setText('dtxtpercentoftotal', '0'); setText('dtxtfcyamount', '0'); setText('dtxtdueday', '0');
    }
    function paymentDetailFormValidation() {
        if (!find(L.fis, val('CmbFinInstruments'))) { box('Financial Instrument No field required'); focus('CmbFinInstruments'); return false; }
        if (netI(val('dcmbpaymentterm')) === 0) { box('Payment Term field required'); focus('dcmbpaymentterm'); return false; }
        if (netD(val('dtxtpercentoftotal')) === 0 || val('dtxtpercentoftotal') === '') { box('Percent Field Required'); focus('dtxtpercentoftotal'); return false; }
        if (netD(val('dtxtfcyamount')) === 0 || val('dtxtfcyamount') === '') { box('Fcy Amount  Field Required'); focus('dtxtfcyamount'); return false; }
        return true;
    }
    /** dcmbfino_ValueChanged: Payment Term from the FI row, then GetFinancialInstrumentsBalance. */
    function finInstrumentChanged() {
        var fi = find(L.fis, val('CmbFinInstruments'));
        if (!fi) return;
        setVal('dcmbpaymentterm', fi.PaymenttermId);
        getJson(API + '/fi-balance?documentTypeId=' + netI(fi.DocumentTypeId) + '&id=' + netI(fi.Id))
            .then(function (d) { setText('txtBalanceFI', clr(d ? d.balance : 0)); }).catch(function (e) { box(e.message); });
    }
    /** PaymentDetailPercentCalculate (dtxtpercentoftotal / txtBalanceFI Leave). */
    function percentLeave() {
        var total = netD(val('txtTotalNetAmount')), pct = netD(val('dtxtpercentoftotal'));
        if (total > 0 && pct > 0) {
            if (pct > 100) { setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); return; }
            setText('dtxtfcyamount', fmtNet(total * pct / 100.0, '#,#.###'));
        }
    }
    /** dtxtfcyamount Leave. */
    function fcyLeave() {
        var total = netD(val('txtTotalNetAmount')), fcy = netD(val('dtxtfcyamount'));
        if (total > 0 && fcy > 0) {
            var pct = fcy / total * 100.0;
            setText('dtxtpercentoftotal', clr(pct));
            if (pct > 100) { setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); }
        }
    }
    function payDuplicate(except) {
        var fi = find(L.fis, val('CmbFinInstruments'));
        for (var i = 0; i < S.pay.length; i++) {
            if (i === except) continue;
            if (netI(S.pay[i].ExImEFormRegistrationId) === netI(val('CmbFinInstruments')) && netI(S.pay[i].DocumentTypeId) === netI(fi && fi.DocumentTypeId)) return true;
        }
        return false;
    }
    function btndadd() {
        if (!paymentDetailFormValidation()) return;
        if (payDuplicate(-1)) { box('Financial Instrument No already add in Grid Please Check'); return; }
        var fi = find(L.fis, val('CmbFinInstruments'));
        S.pay.push({ Id: 0, DocumentTypeId: netI(fi.DocumentTypeId), ExImEFormRegistrationId: netI(val('CmbFinInstruments')), FinancialInstrumentNo: txt('CmbFinInstruments'),
            PaymentTermId: netI(val('dcmbpaymentterm')), PaymentTerm: txt('dcmbpaymentterm'), PrcntOfTotal: netD(val('dtxtpercentoftotal')),
            FcyAmount: netD(val('dtxtfcyamount')), DueDays: netD(val('dtxtdueday')) });
        renderPay(); resePaymentDetails(); focus('CmbFinInstruments');
    }
    function btndupdate() {
        if (!paymentDetailFormValidation()) return;
        if (payDuplicate(S.payIdx)) { box('Financial Instrument No already add in Grid Please Check'); return; }
        var r = S.pay[S.payIdx]; if (!r) return;
        var fi = find(L.fis, val('CmbFinInstruments'));
        r.DocumentTypeId = netI(fi.DocumentTypeId); r.ExImEFormRegistrationId = netI(val('CmbFinInstruments')); r.FinancialInstrumentNo = txt('CmbFinInstruments');
        r.PaymentTermId = netI(val('dcmbpaymentterm')); r.PaymentTerm = txt('dcmbpaymentterm'); r.PrcntOfTotal = netD(val('dtxtpercentoftotal'));
        r.FcyAmount = netD(val('dtxtfcyamount')); r.DueDays = val('dtxtdueday');
        show('btndadd', true); show('btndupdate', false); show('btndcancel', false);
        resePaymentDetails(); renderPay(); focus('CmbFinInstruments');
    }
    function btndcancel() { show('btndadd', true); show('btndupdate', false); show('btndcancel', false); resePaymentDetails(); }
    function payToEntry(i) {
        var r = S.pay[i]; if (!r) return;
        S.payIdx = i;
        setVal('CmbFinInstruments', r.ExImEFormRegistrationId); setVal('dcmbpaymentterm', r.PaymentTermId);
        setText('dtxtpercentoftotal', clr(r.PrcntOfTotal)); setText('dtxtfcyamount', clr(r.FcyAmount)); setText('dtxtdueday', clr(r.DueDays));
        show('btndadd', false); show('btndupdate', true); show('btndcancel', true);
        focus('txtoQty');
        renderPay();
    }

    // ------------------------------------------------------------------------------ reset / read / save

    /** FormReset (+ DetailFormReset from btnNew). */
    function formReset() {
        show('btnsave', true); show('btnupdate', false);
        S.updateMode = false; S.recId = 0; S.flag = 0;
        firstRows();
        setText('txtDocDate', today());
        ['cmbNotifyParty1', 'cmbNotifyParty2', 'cmbSupCust', 'cmbOtherCustomer', 'cmbimporterBankNew', 'cmbdeliverytermnew', 'cmbCommissionAgent',
         'cmbLoadingPort', 'cmbDestinationPort', 'cmbOtherDestinationPort', 'cmbexporterBankNew', 'cmbfcycode', 'cmbPaymentTermsNew'].forEach(function (id) { setVal(id, ''); });
        if (!PRE) setVal('CmbCreditAccount', '');
        ['txtcertificate', 'txtCommPercent', 'txtCommAmount', 'txtcertificateOfOrigion', 'txtHealthPermitnodetail', 'txtItemCommodityDetail', 'txtLotRef',
         'txtfcyAmount', 'txtExRate', 'txtLocalAmt', 'txtGrossWeight', 'txtNetWeight', 'txtNoOfContainer', 'txtIformNo', 'txtaddlesscommnets',
         'txtaddlessamount', 'txtTotalNetAmount'].forEach(function (id) { setText(id, ''); });
        S.rows = []; S.removed = []; S.other = []; S.pay = []; S.otherIdx = -1; S.payIdx = -1; S.cur = -1;
        render(); renderOther(); renderPay();
        bind('combitem', [], 'Id', 'ItemName', false);
        detailFormReset();
        resePaymentDetails();
        focus(PRE ? 'txtDocDate' : 'txtinvoiceno');
        return getJson(API + '/new-codes').then(function (d) {
            setText('txtdocno', (d && d.docNo) || '');
            if (PRE) setText('txtinvoiceno', (d && d.invoiceNo) || '');
        }).catch(function (e) { box(e.message); });
    }
    function btnNew() { return formReset(); }
    /** btnrefersh_Click. */
    function btnRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh').then(function (d) { bindLookups(d || {}); }).catch(function (e) { box(e.message); });
        });
    }

    /** ReadById(Id). */
    function readById(id) {
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            d = d || {};
            var h = d.header || {};
            S.removed = [];
            show('btnsave', false); show('btnupdate', true);
            S.recId = id;
            tab('piForm');
            setVal('cmbbranches', h.BranchesId); setVal('cmbproject', h.ProjectsId);
            setText('txtdocno', str(h.DocCode)); setText('txtDocDate', isoDate(h.DocDate));
            setText('txtinvoiceno', h.InvoiceNo);
            setVal('cmbSupCust', h.SupplierCustomerId); setVal('cmbCommissionAgent', h.CommissionAgentId);
            setText('txtCommPercent', clr(h.CommissionPercentage)); setText('txtCommAmount', clr(h.CommissionAmount));
            setVal('cmbNotifyParty1', h.NotifyParty1); setVal('cmbNotifyParty2', h.NotifyParty2);
            setText('txtLotRef', h.LotNoRef);
            setVal('cmbPaymentTermsNew', h.PaymentTermId); setVal('cmbimporterBankNew', h.ImporterBankId); setVal('cmbexporterBankNew', h.ExporteBankId);
            setVal('cmbdeliverytermnew', h.DeliveryTermId); setVal('cmbLoadingPort', h.LoadingPortId); setVal('cmbDestinationPort', h.DestinationPortId);
            setVal('cmbOtherCustomer', h.OtherCustomerId); setVal('cmbOtherDestinationPort', h.OtherDestinationPortId);
            var ct = null; CARRIER.forEach(function (c) { if (c.Type === str(h.CarierType)) ct = c.Id; }); setVal('cmbcareiertype', ct);
            setVal('cmbfcycode', h.FcurrencyId);
            if (!PRE) setVal('CmbCreditAccount', h.CreditAccountId);
            setText('txtfcyAmount', clr(h.FCurrencyAmount)); setText('txtExRate', clr(h.ConversionRate)); setText('txtLocalAmt', clr(h.EquivalentAmount));
            setText('txtGrossWeight', clr(h.GrossWeight)); setText('txtNetWeight', clr(h.NetWeight)); setText('txtNoOfContainer', clr(h.NoOfContainers));
            setText('txtIformNo', h.EFormNo); setText('txtIformDate', isoDate(h.EFormDate));
            setText('txtcertificate', h.Certificate1); setText('txtcertificateOfOrigion', h.Certificate2);
            setText('txtaddlesscommnets', h.AddLessComments); setText('txtaddlessamount', clr(h.AddLessAmount)); setText('txtTotalNetAmount', clr(h.TotalAmount));
            L.otherList = d.otherItemList || [];
            bind('CmbOtherItem', L.otherList, 'ItemId', 'ItemName', false);
            S.rows = d.rows || [];
            S.flag = netI(d.invoiceUpdateFlag);
            var items = []; S.rows.forEach(function (r) { items.push({ Id: r.ItemId, Name: r.ItemName }); });
            bind('combitem', items, 'Id', 'Name', false);
            S.cur = -1; render();
            S.other = d.otherItems || []; renderOther();
            S.pay = d.paymentTerms || []; renderPay();
            detailFormReset();
            S.updateMode = true;
            L.fis = d.financialInstruments || []; bind('CmbFinInstruments', L.fis, 'Id', 'EFormNo');
        }).catch(function (e) { box(e.message); });
    }

    function headerBody() {
        var h = {};
        ['cmbbranches', 'cmbproject', 'txtdocno', 'txtDocDate', 'txtinvoiceno', 'cmbSupCust', 'cmbNotifyParty1', 'cmbNotifyParty2', 'txtLotRef',
         'cmbPaymentTermsNew', 'cmbimporterBankNew', 'cmbexporterBankNew', 'cmbdeliverytermnew', 'cmbLoadingPort', 'cmbDestinationPort',
         'cmbOtherDestinationPort', 'cmbOtherCustomer', 'cmbCommissionAgent', 'txtCommPercent', 'txtCommAmount', 'cmbfcycode', 'txtfcyAmount',
         'txtExRate', 'txtLocalAmt', 'txtGrossWeight', 'txtNetWeight', 'txtNoOfContainer', 'txtIformNo', 'txtIformDate', 'txtcertificateOfOrigion',
         'txtcertificate', 'txtaddlesscommnets', 'txtaddlessamount', 'txtTotalNetAmount', 'txtoRemarks', 'CmbCreditAccount'].forEach(function (id) { if ($id(id)) h[id] = val(id); });
        h.cmbcareiertypeText = txt('cmbcareiertype');
        h.cmbLoadingPortText = txt('cmbLoadingPort');
        h.cmbDestinationPortText = txt('cmbDestinationPort');
        return h;
    }
    /** formvalidation() - repeated on the page so the message comes in the desktop's order (the server checks again). */
    function formValidation() {
        function need(ok, msg, id) { if (!ok) { box(msg); focus(id); return false; } return true; }
        if (!need(!(val('txtdocno') === '' || netI(val('txtdocno')) === 0), 'Doc No Is Required', 'txtdocno')) return false;
        if (!need(!(val('txtinvoiceno') === '' || val('txtinvoiceno').trim() === '0'), 'Invoice No Is Required', 'txtinvoiceno')) return false;
        if (!need(netI(val('cmbSupCust')) > 0, 'Customer Is Required', 'cmbSupCust')) return false;
        if (!PRE && !need(netI(val('CmbCreditAccount')) > 0, 'Please select CreditAc', 'CmbCreditAccount')) return false;
        if (!need(netI(val('cmbPaymentTermsNew')) > 0, 'Payment Term Is Required', 'cmbPaymentTermsNew')) return false;
        if (!need(netI(val('cmbdeliverytermnew')) > 0, 'Delivery Term Is Required', 'cmbdeliverytermnew')) return false;
        if (!need(netI(val('cmbLoadingPort')) > 0, 'Loading Port Is Required', 'cmbLoadingPort')) return false;
        if (!need(netI(val('cmbDestinationPort')) > 0, 'Destination Port  Is Required', 'cmbDestinationPort')) return false;
        if (!need(txt('cmbDestinationPort').trim() !== txt('cmbLoadingPort').trim(), 'Destination Port and Loaing Port cannot be same...', 'cmbDestinationPort')) return false;
        if (!need(netI(val('cmbcareiertype')) > 0, 'Carier Type  Is Required', 'cmbcareiertype')) return false;
        if (!need(!(val('txtLotRef') === '' || val('txtLotRef') === '0'), 'Lot Ref No Is Required', 'txtLotRef')) return false;
        if (!need(netI(val('cmbfcycode')) > 0, 'Fcy Code Is Required', 'cmbfcycode')) return false;
        if (!need(netD(val('txtfcyAmount')) !== 0, 'Fcy Amount  Is Required', 'txtfcyAmount')) return false;
        if (!need(netD(val('txtNetWeight')) > 0, 'Net Weight  Is Required', 'txtNetWeight')) return false;
        if (!need(netD(val('txtGrossWeight')) >= netD(val('txtNetWeight')), 'Gross Weight Must Be Equal Or Greater Than Net Weight Thank You', 'txtGrossWeight')) return false;
        var agent = netI(val('cmbCommissionAgent')) > 0;
        if (!need(!(netD(val('txtCommPercent')) > 0 && !agent), "As you have entered 'Commission %', the 'Commission Agent' field is required. ", 'cmbCommissionAgent')) return false;
        if (!need(!(netD(val('txtCommPercent')) === 0 && agent), "As you have selected 'Commission agent', please enter the commission percentage.", 'txtCommPercent')) return false;
        if (!PRE && !need(!(val('txtIformNo') === '' || val('txtIformNo') === '0'), 'GD No Is Required', 'txtIformNo')) return false;
        if (!need(netD(val('txtExRate')) > 0, 'ExchangeRate Field is Required', 'txtExRate')) return false;
        if (!need(netD(val('txtTotalNetAmount')) > 0, 'Total Amount  Is Required', 'txtTotalNetAmount')) return false;
        return true;
    }
    /** btnsave_Click (Update calls it too). */
    function btnsave(btn) {
        return busy(btn, function () {
            if (!S.rows.length) { box('Grid Record not found'); return Promise.resolve(); }
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            setText('txtNetWeight', clr(sumOf(S.rows, 'QtyMTon') * 1000.0));
            if (!formValidation()) return Promise.resolve();
            var win = $id('ChkPrintPreview').checked && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
            var body = { recId: S.recId, header: headerBody(), rows: S.rows, removed: S.removed, otherItems: S.other, paymentTerms: S.pay };
            return postJson(API + '/save', body).then(function (d) {
                box((d && d.message) || (S.recId === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                var id = (d && d.id) || 0;
                if (win) { if (id) global.CrystalPrint.open('exp-546', { id: id }, null, win); else { global.CrystalPrint.release(win); box('No Record Found For Display'); } }
                return formReset();
            }).catch(function (e) { if (win) global.CrystalPrint.release(win); box(e.message); });
        });
    }
    /**
     * btnPrint_Click / btnPrint546_Click: CommonServices.ExportPreCommercialInvoiceSlip546(RecId) - and on 194 the bank button
     * calls ExportPreInvoiceSlipForBank546; on 192 both buttons print the plain slip (desktop quirk).
     */
    function printSlip(which, btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        if (S.recId === 0) { box('No Record Found For Display'); return; }
        var key = (which === 'btnPrint546' && !PRE) ? 'exp-546-bank' : 'exp-546';
        return global.CrystalPrint.open(key, { id: S.recId }, btn);
    }

    // ------------------------------------------------------------------------------ loader (LoadExportPerformaInvoice)

    var LD = { rows: [], heads: [], cur: -1, open: false };
    function loaderOpen(btn) {
        return busy(btn, function () {
            return getJson(API + '/loader/setup').then(function (d) {
                d = d || {};
                bind('ldCmbPartyName', d.parties, 'Id', 'CompanyName', false);
                bind('ldCmbCurrency', d.currencies, 'Id', 'CurrencyName', false);
                bind('ldCmbItemName', d.items, 'Id', 'ItemName', false);
                setText('ldFromDate', today().substring(0, 4) + '-01-01');
                setText('ldToDate', today());
                LD.open = true;
                show('piLoader', true);
                return loaderSearch();
            }).catch(function (e) { box(e.message); });
        });
    }
    function loaderSearch(btn) {
        var run = function () {
            return getJson(API + '/loader/rows?partyId=' + netI(val('ldCmbPartyName')) + '&itemId=' + netI(val('ldCmbItemName')) + '&currencyId=' + netI(val('ldCmbCurrency')))
                .then(function (rows) {
                    LD.rows = rows || [];
                    var seen = {}; LD.heads = [];
                    LD.rows.forEach(function (r) { var id = netI(r.Id); if (!seen[id]) { seen[id] = 1; LD.heads.push(r); } });
                    LD.cur = -1; loaderRender();
                }).catch(function (e) { box(e.message); });
        };
        return btn ? busy(btn, run) : run();
    }
    function loaderRender() {
        $id('ldSelectAll').checked = false;
        $id('ldBody').innerHTML = LD.heads.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === LD.cur ? ' class="is-current"' : '') + '><td class="win-cell-chk"><input type="checkbox" data-sel="' + i + '"/></td>'
                + '<td class="num">' + esc(r.DocNo) + '</td><td>' + esc(shortDate(r.ProformaDate)) + '</td><td>' + esc(r.ProformaNo) + '</td><td>' + esc(shortDate(r.SalesContratDate))
                + '</td><td>' + esc(r.CustomerName) + '</td><td>' + esc(r.SalesMan) + '</td><td>' + esc(r.CommissionAgent) + '</td><td class="num">' + esc(clr(r.CommRate))
                + '</td><td class="num">' + esc(clr(r.CommAmount)) + '</td><td>' + esc(r.CurrencyCode) + '</td><td>' + esc(r.DeliveryTerm) + '</td><td>' + esc(r.PaymentTerm)
                + '</td><td>' + esc(r.LoadingPort) + '</td><td>' + esc(r.DestinationPort) + '</td><td>' + esc(r.InsuranceRemarks) + '</td><td>' + esc(r.RemarksHeader)
                + '</td><td>' + esc(r.EntryUser) + '</td><td>' + esc(shortDate(r.EntryDate)) + '</td><td>' + esc(r.ModifyUser) + '</td><td>' + esc(shortDate(r.ModifyDate))
                + '</td><td class="num">' + esc(r.NoOfAttachments) + '</td></tr>';
        }).join('');
        $id('ldDetailBody').innerHTML = ''; $id('ldDetailFoot').innerHTML = '';
    }
    /** grd_SelectionChanged: the selected proforma's detail rows. */
    function loaderSelect(i) {
        LD.cur = i;
        var head = LD.heads[i]; if (!head) return;
        var rows = LD.rows.filter(function (r) { return netI(r.Id) === netI(head.Id); });
        var cols = ['ItemName', 'PackingType', 'PackingWeight', 'CropYear', 'NoofContainerDetail', 'ShipContainer', 'BalContainers', 'NoofBagsPerContainer', 'PackUom',
            'M_Ton', 'ShipMTon', 'BalMTon', 'NoOfBags', 'ShipBags', 'BalNoofBags', 'PackingTotalWeight', 'NetWeight', 'ShipWeight', 'BalWeight', 'RatePrice', 'Amount',
            'ShipAmount', 'BalShipAmount', 'PackingExpiryDate', 'ContainerSize', 'Other_Description', 'OuterPackDescription'];
        var f0 = { Amount: 1, ShipAmount: 1, BalShipAmount: 1, NoOfBags: 1, ShipBags: 1, BalNoofBags: 1, NetWeight: 1, ShipWeight: 1, BalWeight: 1 };
        var numc = { PackingWeight: 1, NoofContainerDetail: 1, ShipContainer: 1, BalContainers: 1, NoofBagsPerContainer: 1, M_Ton: 1, ShipMTon: 1, BalMTon: 1, PackingTotalWeight: 1, RatePrice: 1 };
        var sums = {};
        $id('ldDetailBody').innerHTML = rows.map(function (r) {
            return '<tr>' + cols.map(function (c) {
                var v = col(r, c);
                if (f0[c]) { sums[c] = (sums[c] || 0) + netD(v); return '<td class="num">' + esc(fmtNet(v, '#,##0')) + '</td>'; }
                if (numc[c]) return '<td class="num">' + esc(clr(v)) + '</td>';
                return '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $id('ldDetailFoot').innerHTML = rows.length ? '<tr>' + cols.map(function (c, k) { return '<td' + (f0[c] ? '' : ' class="lbl"') + '>' + (f0[c] ? esc(fmtNet(sums[c] || 0, '#,##0')) : (k === 0 ? '&Sigma;' : '')) + '</td>'; }).join('') + '</tr>' : '';
        document.querySelectorAll('#ldBody tr').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
    }
    function loaderReset(btn) {
        return busy(btn, function () {
            setVal('ldCmbPartyName', ''); setVal('ldCmbItemName', ''); setVal('ldCmbCurrency', '');
            return getJson(API + '/loader/setup').then(function (d) {
                d = d || {};
                bind('ldCmbPartyName', d.parties, 'Id', 'CompanyName', false);
                bind('ldCmbItemName', d.items, 'Id', 'ItemName', false);
                bind('ldCmbCurrency', d.currencies, 'Id', 'CurrencyName', false);
                return loaderSearch();
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btnLoadOnInvoice_Click_1: same Customer/Importer and Currency, else the desktop's message and the popup stays open. */
    function loaderLoad(btn) {
        var checked = [];
        document.querySelectorAll('#ldBody input[data-sel]').forEach(function (c) { if (c.checked) checked.push(LD.heads[+c.getAttribute('data-sel')]); });
        if (!checked.length) { box('Check the row first'); return; }
        var sup = 0, cur = 0, ids = [];
        for (var i = 0; i < checked.length; i++) {
            var cs = netI(checked[i].SupCustId), cc = netI(checked[i].FCurrencyId);
            if (sup === 0) { sup = cs; cur = cc; }
            if (sup !== cs || cur !== cc) { box("Sorry! The selected Proforma's are not of the same Customer/Importer or Currency."); return; }
            ids.push(netI(checked[i].Id));
            sup = cs; cur = cc;
        }
        return busy(btn, function () { return loadInGrid(ids); });
    }
    function loaderClose() {
        /* The popup is a ShowDialog: closing it still runs LoadInGridDetail with the (empty) selection. */
        return loadInGrid([]);
    }
    /** LoadInGridDetail -> LoadDataDetailGridAgainstPerformaInvoice + LoadOtherItemsData, then the recalculations. */
    function loadInGrid(ids) {
        show('piLoader', false); LD.open = false;
        return postJson(API + '/loader/load', { ids: ids }).then(function (d) {
            d = d || {};
            S.rows = [];
            var h = d.header;
            if (h) {
                setVal('cmbSupCust', h.SupCustId); setVal('cmbOtherCustomer', h.SupCustId); setVal('cmbfcycode', h.FCurrencyId);
                setVal('cmbLoadingPort', h.LoadingPortId); setVal('cmbDestinationPort', h.DestinationPortId); setVal('cmbOtherDestinationPort', h.DestinationPortId);
                setVal('cmbdeliverytermnew', h.DeliveryTermId);
                if (netI(h.NotifyPartyId) > 0) setVal('cmbNotifyParty1', h.NotifyPartyId);
                setVal('cmbPaymentTermsNew', h.PaymentTermId);
                setText('txtfcyAmount', clr(h.FCurrencyAmount)); setText('txtNoOfContainer', clr(h.NoOfContainers)); setText('txtLotRef', str(h.LotReference));
                setVal('cmbCommissionAgent', h.CommissionAgentId); setText('txtCommPercent', clr(h.CommRate)); setText('txtCommAmount', clr(h.CommAmount));
                setEnabled('cmbSupCust', false); setEnabled('cmbfcycode', false);
                setText('txtoRemarks', str(h.RemarksHeader)); setText('txtcertificate', str(h.InsuranceRemarks));
                S.rows = d.rows || [];
            }
            S.cur = S.rows.length ? 0 : -1;
            render();
            S.other = d.otherItems || []; renderOther();
            calculatAmount();
            calculateLocalAmount();
            calculateWeight();
            totalAmountCalculate();
            getTotalnetWeightFromGrid();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = ['InvoiceNo', 'DocCode', 'DocDate', 'CustomerName', 'NoOfContainers', 'GrossWeight', 'NetWeight', 'FcyAmount', 'AddLess', 'TotalAmount',
        'LoadingPort', 'DestinationPort', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'ApprovedStatus', 'ApprovedUser', 'ApprovedDate', 'NoOfAttachments'];
    var HFMT = { NoOfContainers: '0,0', AddLess: '0,0', GrossWeight: '#,#.###', NetWeight: '#,#.###', FcyAmount: '#,#.###', TotalAmount: '#,#.###' };
    var HDATE = { DocDate: 1, EntryDate: 1, ModifyDate: 1, ApprovedDate: 1 };
    function histRender() {
        var sums = {};
        $id('piHistBody').innerHTML = HIST.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === HCUR ? ' class="is-current"' : '') + '>';
            h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-hact="Slip" data-i="' + i + '">Slip</button></td>';
            h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-hact="CustomSlip" data-i="' + i + '">BankSlip</button></td>';
            h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-hact="Edit" data-i="' + i + '">Edit</button></td>';
            HCOLS.forEach(function (c) {
                var v = col(r, c), inner;
                if (HFMT[c]) { sums[c] = (sums[c] || 0) + netD(v); inner = fmtNet(v, HFMT[c]); }
                else if (HDATE[c]) inner = shortDate(v);
                else inner = str(v);
                inner = esc(inner);
                if (c === 'InvoiceNo') inner = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + inner + '</a>';
                h += '<td' + (HFMT[c] || c === 'DocCode' ? ' class="num"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        var foot = $id('piHistFoot');
        if (!HIST.length) foot.innerHTML = '';
        else {
            var t = '<tr><td class="lbl" colspan="3">&Sigma;</td>';
            HCOLS.forEach(function (c) { t += '<td' + (HFMT[c] ? '' : ' class="lbl"') + '>' + (HFMT[c] ? esc(fmtNet(sums[c] || 0, HFMT[c])) : '') + '</td>'; });
            foot.innerHTML = t + '</tr>';
        }
        show('piHistEmpty', HIST.length === 0);
        $id('piHistDetailBody').innerHTML = '';
    }
    /** btnShowHistory_Click -> HistoryGridFill. */
    function historyShow(btn) {
        return busy(btn, function () {
            var f = { dateBy: radio('dateBy'), fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'),
                toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'), fromDocNo: val('FromDocNoHistory'), toDocNo: val('ToDocNoHistory'),
                customerId: netI(val('CmbCustomerHistory')) };
            return postJson(API + '/history', f).then(function (rows) { HIST = rows || []; HCUR = -1; histRender(); }).catch(function (e) { box(e.message); });
        });
    }
    /** btnResetHistory_Click (From date = today - 3, as the desktop's reset). */
    function historyReset() {
        setText('FromDateHistory', daysAgo(3)); setText('ToDateHistory', today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', '');
        setVal('CmbCustomerHistory', '');
        HIST = []; HCUR = -1; histRender(); show('piHistEmpty', false);
    }
    function historyRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/history-combos').then(function (rows) { bind('CmbCustomerHistory', rows, 'Id', 'Customer'); }).catch(function (e) { box(e.message); });
        });
    }
    /** DataGridHistory_SelectionChanged -> GetDetailByHeaderId. */
    function historySelect(i) {
        HCUR = i;
        var r = HIST[i]; if (!r) return;
        getJson(API + '/history-detail?id=' + netI(r.Id)).then(function (rows) {
            $id('piHistDetailBody').innerHTML = (rows || []).map(function (d) {
                var c = [d.PerformaDocNo, d.PriRefNo, d.ItemName, d.ItemHSCode, d.PackType, clr(d.PackingWeight), d.CropYear, clr(d.NoOfContainers), clr(d.NoOfBagsCntnr),
                    d.PackSize, clr(d.QtyMTon), clr(d.NoOfBags), clr(d.NetWeight), clr(d.TotalPackingWeight), clr(d.GrossWeight), clr(d.CostMTon), clr(d.ContractRate),
                    d.RateUOM, clr(d.Amount), clr(d.OtherRate), clr(d.OtherAmount), d.OtherHSCode, d.PackingExpiryDate, d.ItemCommodityDetail, d.HealthPermitNo];
                var numIdx = { 0: 1, 5: 1, 7: 1, 8: 1, 10: 1, 11: 1, 12: 1, 13: 1, 14: 1, 15: 1, 16: 1, 18: 1, 19: 1, 20: 1 };
                return '<tr>' + c.map(function (v, k) { return '<td' + (numIdx[k] ? ' class="num"' : '') + '>' + esc(v) + '</td>'; }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { box(e.message); });
    }
    function historyButton(act, i, btn) {
        var r = HIST[i]; if (!r) return;
        var id = netI(r.Id);
        if (act === 'Edit') { readById(id); return; }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        if (act === 'Slip') global.CrystalPrint.open('exp-546', { id: id }, btn);
        if (act === 'CustomSlip') global.CrystalPrint.open('exp-546-bank', { id: id }, btn);
    }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs[data-tabs="pi"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('[data-subtabs="pi"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { subtab(b.getAttribute('data-subtab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
        /* header events */
        on('txtExRate', 'input', calculateLocalAmount);
        on('txtaddlessamount', 'input', totalAmountCalculate);
        on('txtCommPercent', 'input', commPercentChanged);
        if (!PRE) on('txtinvoiceno', 'blur', function () { if (val('txtIformNo') === '') setText('txtIformNo', val('txtinvoiceno')); });
        /* entry panel events */
        on('combitem', 'change', function () { bindUoms(val('combitem')); });
        ['CmbPackUom', 'CmbRateUom'].forEach(function (id) { on(id, 'change', recalc); });
        ['txtCostMTon', 'txtPackingWeight', 'txtNoOfContainersDetail', 'txtNoOfBagsPerCntnr'].forEach(function (id) { on(id, 'input', recalc); });
        on('txtOtherRateDetail', 'input', otherRateChanged);
        /* grid buttons / link / double-click */
        var gb = $id('piBody');
        gb.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]');
            if (b) { columnButton(b.getAttribute('data-act'), +b.getAttribute('data-i')); return; }
            var a = e.target.closest('a.win-code');
            if (a) { rowToEntry(+a.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            S.cur = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) rowToEntry(+tr.getAttribute('data-i')); });
        /* other items / payment detail */
        on('txtoQty', 'input', otherItemsCalculations);
        on('CmbOtherItem', 'change', otherItemChanged);
        if ($id('piOtherBody')) $id('piOtherBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) otherToEntry(+tr.getAttribute('data-i')); });
        on('CmbFinInstruments', 'change', finInstrumentChanged);
        on('dtxtpercentoftotal', 'blur', percentLeave);
        on('txtBalanceFI', 'blur', percentLeave);
        on('dtxtfcyamount', 'blur', fcyLeave);
        if ($id('piPayBody')) $id('piPayBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) payToEntry(+tr.getAttribute('data-i')); });
        /* loader */
        $id('ldBody').addEventListener('click', function (e) {
            if (e.target.closest('input[data-sel]')) return;
            var tr = e.target.closest('tr[data-i]'); if (tr) loaderSelect(+tr.getAttribute('data-i'));
        });
        on('ldSelectAll', 'change', function () { var on2 = $id('ldSelectAll').checked; document.querySelectorAll('#ldBody input[data-sel]').forEach(function (c) { c.checked = on2; }); });
        /* history */
        var hb = $id('piHistBody');
        hb.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-hact]');
            if (b) { historyButton(b.getAttribute('data-hact'), +b.getAttribute('data-i'), b); return; }
            var a = e.target.closest('a.win-code');
            if (a) { historyButton('Edit', +a.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            historySelect(+tr.getAttribute('data-i'));
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) readById(netI(HIST[+tr.getAttribute('data-i')].Id)); });
        /* ImProformaInvoice_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (LD.open) { if (e.key === 'Escape') { e.preventDefault(); loaderClose(); } return; }
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target);
                if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); return; }
            if (e.ctrlKey && k === 's' && !onHistory() && !S.updateMode) { e.preventDefault(); btnsave($id('btnsave')); return; }
            if (e.ctrlKey && k === 'u' && S.updateMode) { e.preventDefault(); btnsave($id('btnupdate')); }
        });
        load();
    });

    global.ExportPi = {
        btnNew: btnNew, btnsave: btnsave, btnRefresh: btnRefresh, printSlip: printSlip, toggleHistory: toggleHistory,
        btnAddinGrid: btnAddinGrid, btnGridUpdate: btnGridUpdate, btnCancel: btnCancel,
        btnoadd: btnoadd, btnoUpdate: btnoUpdate, btnoCancel: btnoCancel, btndadd: btndadd, btndupdate: btndupdate, btndcancel: btndcancel,
        loaderOpen: loaderOpen, loaderSearch: loaderSearch, loaderReset: loaderReset, loaderLoad: loaderLoad, loaderClose: loaderClose,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh
    };
}(window));
