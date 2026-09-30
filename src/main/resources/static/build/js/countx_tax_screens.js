/* ============================================================================================
 * countx_tax_screens.js - three Taxation screens. <body data-tax-page="..."> picks the one:
 *
 *   allocation        RegularItemsAllocateToTaxItem.cs (179)  "Items Allocation To Tax Item"
 *   sale-tax-summary  frmSaleTaxSummaryRpt.cs (175)           "Sale Tax Summary"
 *   wht-challan       frmWhtTaxChallanDeposit.cs (174)        "Wht Challan Deposit"
 * ============================================================================================ */
(function () {
    'use strict';

    function $id(id) { return document.getElementById(id); }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function netD(v) { var n = parseFloat(v); return isFinite(n) ? n : 0; }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
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
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function bind(id, rows, valueCol, textCol, keepBlank) {
        var s = $id(id); if (!s) return;
        var html = keepBlank === false ? '' : '<option value="0"></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, valueCol)) + '">' + esc(col(r, textCol)) + '</option>'; });
        s.innerHTML = html;
        s.value = '0';
        if (keepBlank === false && s.options.length) s.selectedIndex = 0;
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoDate(v) {
        var s = str(v).trim(); if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s); if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s); return isNaN(d.getTime()) ? '' : d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function ddMMMyyyy(v) { var s = isoDate(v); if (!s) return ''; return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(0, 4); }
    /** clsGlobalVariables.stringFormatsingle - "#,##0.00" style. */
    function money(v) { return netD(v).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }); }
    /** FormatString "0,0" - whole number with thousands. */
    function whole(v) { return Math.round(netD(v)).toLocaleString('en-US'); }
    function cancel(fallback) {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = fallback || '/taxation'; } }, 150);
    }
    function checkedIds(tbodyId) {
        var out = [];
        document.querySelectorAll('#' + tbodyId + ' input[type=checkbox]:checked').forEach(function (c) { out.push(netI(c.getAttribute('data-id'))); });
        return out;
    }
    function selectorGrid(tbodyId, rows, idCol, cells) {
        var tb = $id(tbodyId);
        tb.innerHTML = (rows || []).map(function (r) {
            return '<tr><td class="sel"><input type="checkbox" data-id="' + esc(col(r, idCol)) + '"></td>' + cells(r).map(function (c) {
                return c.hidden ? '<td style="display:none;">' + esc(c.v) + '</td>' : '<td' + (c.right ? ' class="num"' : '') + '>' + esc(c.v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
    }
    function headerSelector(headId, tbodyId) {
        var h = $id(headId); if (!h) return;
        h.addEventListener('change', function () { document.querySelectorAll('#' + tbodyId + ' input[type=checkbox]').forEach(function (c) { c.checked = h.checked; }); });
    }

    var PAGE = document.body.getAttribute('data-tax-page');

    // ============================================================================== 179 Items Allocation To Tax Item

    var Allocation = (function () {
        var api = '/api/taxation/allocation';

        /* frmItemsCustomizedGroups_Load: TaxableItemFill (Item Name, ZeroIndex true), ItemCategoryFill, ItemTypeFill; focus Tax Item. */
        function load() {
            return getJson(api + '/setup').then(function (d) {
                d = d || {};
                if (d.taxItemsError) box(d.taxItemsError); else if ((d.taxItems || []).length) bind('CmbTaxableItem', d.taxItems, 'Id', 'ItemName');
                if (d.categoriesError) box(d.categoriesError); else if ((d.categories || []).length) bind('CmbItemCategory', d.categories, 'Id', 'CategoryDescription');
                if (d.typesError) box(d.typesError); else if ((d.types || []).length) bind('CmbItemType', d.types, 'Id', 'TypeDescription');
                if (d.permissions && d.permissions.Save === false) { $id('BtnAllocateItems').disabled = true; $id('btnDeAllocate').disabled = true; }
                refreshCombos();
            }).catch(function (e) { box(e.message); }).then(function () { $id('CmbTaxableItem').focus(); });
        }
        function params() {
            return { taxItemId: netI($id('CmbTaxableItem').value), categoryId: netI($id('CmbItemCategory').value), typeId: netI($id('CmbItemType').value) };
        }
        /* ShowData(): "Select Product Type First" when no tax item; otherwise both grids. */
        function showData(btn) {
            var p = params();
            if (p.taxItemId <= 0) { box('Select Product Type First'); return; }
            return busy(btn, function () {
                return getJson(api + '/lists?taxItemId=' + p.taxItemId + '&categoryId=' + p.categoryId + '&typeId=' + p.typeId).then(function (d) {
                    d = d || {};
                    selectorGrid('unallocBody', d.unallocated || [], 'ItemId', function (r) { return [{ v: col(r, 'ItemId'), hidden: true }, { v: col(r, 'ItemName') }]; });
                    selectorGrid('allocBody', d.allocated || [], 'ItemId', function (r) { return [{ v: col(r, 'ItemId'), hidden: true }, { v: col(r, 'ItemName') }]; });
                    $id('unallocAll').checked = false; $id('allocAll').checked = false;
                }).catch(function (e) { box(e.message); });
            });
        }
        /* BtnAllocateItems_Click. */
        function allocate(btn) {
            var p = params();
            if (p.taxItemId === 0) { box('Select Tax Item First'); $id('CmbTaxableItem').focus(); return; }
            var ids = checkedIds('unallocBody');
            if (!ids.length) { box("Checked Row's first To Allocate Items"); return; }
            return busy(btn || 'BtnAllocateItems', function () {
                return postJson(api + '/allocate', { taxItemId: p.taxItemId, categoryId: p.categoryId, typeId: p.typeId, itemIds: ids })
                    .then(function () { box("Item's Allocated Successfully"); return showData(null); })
                    .catch(function (e) { box(e.message); });
            });
        }
        /* btnDeAllocate_Click. */
        function deallocate(btn) {
            var p = params();
            if (p.taxItemId === 0) { box('Select Tax Item First'); $id('CmbTaxableItem').focus(); return; }
            var ids = checkedIds('allocBody');
            if (!ids.length) { box("Checked Row's first To Un-Allocate Items"); return; }
            if (!ask("Are you sure to UnAllocate Item's?")) return;
            return busy(btn || 'btnDeAllocate', function () {
                return postJson(api + '/deallocate', { taxItemId: p.taxItemId, categoryId: p.categoryId, typeId: p.typeId, itemIds: ids })
                    .then(function () { box("Item's UnAllocated Successfully"); return showData(null); })
                    .catch(function (e) { box(e.message); });
            });
        }
        /* btnRefresh_Click: the three combos again. */
        function refresh(btn) { return busy(btn || 'btnRefresh', load); }
        function boot() {
            headerSelector('unallocAll', 'unallocBody');
            headerSelector('allocAll', 'allocBody');
            /* CmbTaxableItem_TextChanged -> ShowData(); a fresh page has no selection, so the change event only. */
            $id('CmbTaxableItem').addEventListener('change', function () { if (netI($id('CmbTaxableItem').value) > 0) showData(null); });
            /* frmItemsCustomizedGroups_KeyDown: Ctrl+R refresh, Ctrl+A allocate, Ctrl+D de-allocate, Ctrl+E / Esc close. */
            document.addEventListener('keydown', function (e) {
                var k = (e.key || '').toLowerCase();
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
                if (e.ctrlKey && k === 'a') { e.preventDefault(); allocate(); }
                if (e.ctrlKey && k === 'd') { e.preventDefault(); deallocate(); }
                if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
            });
            load();
        }
        return { boot: boot, show: showData, allocate: allocate, deallocate: deallocate, refresh: refresh, cancel: cancel };
    }());

    // ============================================================================== 175 Sale Tax Summary

    var Summary = (function () {
        var api = '/api/taxation/sale-tax-summary';
        function fill(tbodyId, footId, rows) {
            var d = 0, c = 0, a = 0;
            $id(tbodyId).innerHTML = (rows || []).map(function (r) {
                d += netD(col(r, 'DebitAmount')); c += netD(col(r, 'CreditAmount')); a += netD(col(r, 'Amount'));
                return '<tr><td>' + esc(col(r, 'TaxTypes')) + '</td><td>' + esc(col(r, 'TaxNotesHeading')) + '</td><td class="num">' + money(col(r, 'DebitAmount')) + '</td><td class="num">' + money(col(r, 'CreditAmount')) + '</td><td class="num">' + money(col(r, 'Amount')) + '</td></tr>';
            }).join('');
            $id(footId).innerHTML = '<td></td><td></td><td class="num">' + money(d) + '</td><td class="num">' + money(c) + '</td><td class="num">' + money(a) + '</td>';
        }
        /* frmSaleTaxSummaryRpt_Load / btnShow_Click: OutputGridBind then InputGridBind with DateFrom/DateTo. */
        function showData(btn) {
            return busy(btn, function () {
                return getJson(api + '?from=' + encodeURIComponent($id('DateFrom').value) + '&to=' + encodeURIComponent($id('DateTo').value)).then(function (d) {
                    d = d || {};
                    fill('outputBody', 'outputFoot', d.output || []);
                    fill('inputBody', 'inputFoot', d.input || []);
                }).catch(function (e) { box(e.message); });
            });
        }
        /* btnnew_Click -> Reset(): DateFrom.Focus(). */
        function btnnew() { $id('DateFrom').focus(); }
        function boot() {
            $id('DateFrom').value = today(); $id('DateTo').value = today();
            document.addEventListener('keydown', function (e) {
                var k = (e.key || '').toLowerCase();
                if (e.ctrlKey && k === 'n') { e.preventDefault(); btnnew(); }
                if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
            });
            showData(null);
        }
        return { boot: boot, show: showData, btnnew: btnnew, cancel: cancel };
    }());

    // ============================================================================== 174 Wht Challan Deposit

    var Challan = (function () {
        var api = '/api/taxation/wht-challan';
        var PENDING = [];
        var HISTORY = [];
        var PERM = {};

        /* frmWhtTaxChallanDeposit_Load. */
        function load() {
            return getJson(api + '/setup').then(function (d) {
                d = d || {};
                PERM = d.permissions || {};
                $id('btnsave').disabled = PERM.Save === false;
                $id('Print').disabled = true;                     // Print_Click is an empty method on the desktop
                $id('btnUpdate').disabled = true;                 // btnUpdate_Click is an empty method on the desktop
                if (d.docNoError) box(d.docNoError); else $id('txtDocNo').value = str(d.docNo);
                if (d.companiesError) box(d.companiesError); else if ((d.companies || []).length) { bind('comCompany', d.companies, 'Id', 'CompName'); $id('comCompany').selectedIndex = 1; }
                if (d.branchesError) box(d.branchesError); else if ((d.branches || []).length) { bind('combranches', d.branches, 'Id', 'BranchName'); $id('combranches').selectedIndex = 1; }
                if (d.projectsError) box(d.projectsError); else if ((d.projects || []).length) { bind('comproject', d.projects, 'Id', 'ProjectName'); $id('comproject').selectedIndex = 1; }
                if (d.accountsError) box(d.accountsError);
                else {
                    if ((d.partyAccounts || []).length) bind('cmbSupplierCustomerGlAcc', d.partyAccounts, 'Id', 'AccountTitle');
                    if ((d.creditAccounts || []).length) bind('cmbGLCreditAcc', d.creditAccounts, 'Id', 'AccountTitle');
                }
                refreshCombos();
            }).catch(function (e) { box(e.message); }).then(function () {
                if (!$id('txtDocDate').value) $id('txtDocDate').value = today();
                $id('cmbSupplierCustomerGlAcc').focus();
            });
        }
        /* cmbSupplierCustomerGlAcc_Leave: DebitAccountBind + the pending grid. */
        function partyChanged() {
            var party = netI($id('cmbSupplierCustomerGlAcc').value);
            if (party <= 0) { bind('cmbDebitGLAc', [], 'RefDocNoId', 'AccountTitle'); PENDING = []; drawPending(); refreshCombos(); return; }
            return getJson(api + '/pending?partyAccountId=' + party + '&branchId=' + netI($id('combranches').value) + '&projectId=' + netI($id('comproject').value)).then(function (d) {
                d = d || {};
                if ((d.debitAccounts || []).length) bind('cmbDebitGLAc', d.debitAccounts, 'RefDocNoId', 'AccountTitle');
                else bind('cmbDebitGLAc', [], 'RefDocNoId', 'AccountTitle');
                PENDING = d.pending || [];
                drawPending();
                refreshCombos();
            }).catch(function (e) { box(e.message); });
        }
        /* DetailGridSetting: hId, InvoiceNoRefId, DocumentTypeId, AccountId, TaxTypeId hidden; Select as first column; CreditAmount "0,0" right, totalled. */
        function drawPending() {
            var total = 0;
            $id('detailBody').innerHTML = PENDING.map(function (r) {
                total += netD(col(r, 'CreditAmount'));
                return '<tr><td class="sel"><input type="checkbox" data-id="' + esc(col(r, 'hId')) + '"></td>'
                    + '<td style="display:none;">' + esc(col(r, 'hId')) + '</td><td style="display:none;">' + esc(col(r, 'InvoiceNoRefId')) + '</td><td style="display:none;">' + esc(col(r, 'DocumentTypeId')) + '</td>'
                    + '<td>' + esc(col(r, 'Code')) + '</td><td>' + esc(ddMMMyyyy(col(r, 'VoucherDate'))) + '</td><td>' + esc(col(r, 'VoucherCode')) + '</td>'
                    + '<td style="display:none;">' + esc(col(r, 'AccountId')) + '</td><td>' + esc(col(r, 'AccountTitle')) + '</td>'
                    + '<td style="display:none;">' + esc(col(r, 'TaxTypeId')) + '</td><td>' + esc(col(r, 'TaxDescription')) + '</td>'
                    + '<td class="num">' + whole(col(r, 'CreditAmount')) + '</td></tr>';
            }).join('');
            $id('detailFoot').innerHTML = '<td></td><td style="display:none;"></td><td style="display:none;"></td><td style="display:none;"></td><td></td><td></td><td></td><td style="display:none;"></td><td></td><td style="display:none;"></td><td></td><td class="num">' + whole(total) + '</td>';
            $id('detailAll').checked = false;
        }
        function sel(id) { var s = $id(id); return s && s.value !== '0' && s.value !== ''; }
        /* FormValidation - the desktop's order. */
        function formValidation() {
            if (!sel('cmbSupplierCustomerGlAcc')) { box('SupplierCustomer Field required'); $id('cmbSupplierCustomerGlAcc').focus(); return false; }
            if (!sel('cmbDebitGLAc')) { box('Debit Gl Account Field required'); $id('cmbDebitGLAc').focus(); return false; }
            if (!sel('cmbGLCreditAcc')) { box('Credit Gl Account Field required'); $id('cmbGLCreditAcc').focus(); return false; }
            if (!sel('comCompany')) { box('Company Name Field required'); $id('comCompany').focus(); return false; }
            if (!sel('combranches')) { box('Branches Name Field required'); $id('combranches').focus(); return false; }
            if (!sel('comproject')) { box('Project Name Field required'); $id('comproject').focus(); return false; }
            var n = $id('txtDocNo').value.trim();
            if (n === '' || n === '0') { box('Doc NO Field required'); $id('txtDocNo').focus(); return false; }
            return true;
        }
        /* FormRefresh(). */
        function formRefresh() {
            ['cmbDebitGLAc', 'cmbGLCreditAcc', 'cmbSupplierCustomerGlAcc'].forEach(function (id) { $id(id).value = '0'; });
            $id('txtRemarks').value = '';
            PENDING = []; drawPending();
            refreshCombos();
            return getJson(api + '/doc-no').then(function (d) { $id('txtDocNo').value = str(d && d.docNo); }).catch(function (e) { box(e.message); });
        }
        /* btnsave_Click. */
        function btnsave(btn) {
            if (!formValidation()) return;
            var ids = checkedIds('detailBody');
            if (!PENDING.length) { box('Grid Record not found'); return; }
            if (!ids.length) { box('Checked row first'); return; }
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', {
                    partyAccountId: netI($id('cmbSupplierCustomerGlAcc').value), debitAccountId: netI($id('cmbDebitGLAc').value), creditAccountId: netI($id('cmbGLCreditAcc').value),
                    companyId: netI($id('comCompany').value), branchId: netI($id('combranches').value), projectId: netI($id('comproject').value),
                    docNo: $id('txtDocNo').value.trim(), docDate: $id('txtDocDate').value, remarks: $id('txtRemarks').value, sourceIds: ids
                }).then(function (d) {
                    box((d && d.message) || 'Record Save Successfully');
                    return formRefresh();
                }).catch(function (e) { box(e.message); });
            });
        }
        /* tabControl1_SelectedIndexChanged -> HistoryFill(). */
        function historyFill() {
            return getJson(api + '/history').then(function (d) {
                HISTORY = (d && d.rows) || [];
                $id('historyBody').innerHTML = HISTORY.map(function (r, i) {
                    return '<tr data-i="' + i + '"><td style="display:none;">' + esc(r.id) + '</td><td>' + esc(r.voucherCode) + '</td><td>' + esc(r.documentType) + '</td><td>' + esc(ddMMMyyyy(r.voucherDate)) + '</td>'
                        + '<td>' + esc(r.accountTitle) + '</td><td>' + esc(r.remarks) + '</td><td>' + esc(r.entryUser) + '</td><td class="num">' + esc(r.attachment === null || r.attachment === undefined ? 0 : r.attachment) + '</td>'
                        + '<td><button type="button" class="win-btn-mini" data-view="' + i + '">View</button></td><td><button type="button" class="win-btn-mini" data-print="' + i + '">Print</button></td></tr>';
                }).join('');
            }).catch(function (e) { box(e.message); });
        }
        /* DataGridHistory "View": VoucherHead.GetByID -> grd (AccountTitle, Remarks, DebitAmount, CreditAmount, totals). */
        function view(i) {
            var r = HISTORY[i]; if (!r) return;
            return getJson(api + '/voucher?id=' + encodeURIComponent(r.id)).then(function (v) {
                var lines = (v && v.details) || [], d = 0, c = 0;
                $id('viewBody').innerHTML = lines.map(function (l) {
                    d += netD(l.debitAmount); c += netD(l.creditAmount);
                    return '<tr><td>' + esc(l.accountTitle) + '</td><td>' + esc(l.comments) + '</td><td class="num">' + money(l.debitAmount) + '</td><td class="num">' + money(l.creditAmount) + '</td></tr>';
                }).join('');
                $id('viewFoot').innerHTML = '<td></td><td></td><td class="num">' + money(d) + '</td><td class="num">' + money(c) + '</td>';
            }).catch(function (e) { box(e.message); });
        }
        function tab(history) {
            show('tabForm', !history); show('tabHistory', history);
            $id('tabFormBtn').classList.toggle('is-active', !history); $id('tabHistoryBtn').classList.toggle('is-active', history);
            if (history) historyFill();
        }
        function boot() {
            headerSelector('detailAll', 'detailBody');
            $id('cmbSupplierCustomerGlAcc').addEventListener('change', function () { partyChanged(); });
            $id('historyBody').addEventListener('click', function (e) {
                var v = e.target.closest('button[data-view]'); if (v) { view(+v.getAttribute('data-view')); return; }
                var p = e.target.closest('button[data-print]'); if (p) box('ANewAcRptPaymentReceiptsVoucherSlip_102 (the Crystal voucher slip) is not available in the web application yet.');
            });
            $id('tabFormBtn').addEventListener('click', function () { tab(false); });
            $id('tabHistoryBtn').addEventListener('click', function () { tab(true); });
            /* frmWhtTaxChallanDeposit_KeyDown: Ctrl+S (form tab), Ctrl+N, Ctrl+T switches tab, Ctrl+E / Esc close. */
            document.addEventListener('keydown', function (e) {
                var k = (e.key || '').toLowerCase();
                if (e.ctrlKey && k === 's' && !$id('tabForm').classList.contains('is-hidden')) { e.preventDefault(); btnsave(); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); formRefresh(); }
                if (e.ctrlKey && k === 't') { e.preventDefault(); tab($id('tabHistory').classList.contains('is-hidden')); }
                if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
            });
            load();
        }
        function attachment() { box('The Attachment dialog (Architecture.WinApp.Common.Attachment) is not available in the web application yet.'); }
        return { boot: boot, btnsave: btnsave, btnnew: formRefresh, attachment: attachment, cancel: cancel };
    }());

    var PAGES = { 'allocation': Allocation, 'sale-tax-summary': Summary, 'wht-challan': Challan };
    var page = PAGES[PAGE];
    if (!page) return;
    window.TaxPage = page;
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', page.boot);
    else page.boot();
}());
