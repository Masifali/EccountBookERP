/* ============================================================================================
 * Purchase Invoice PM - screen 501, module 54, DocumentTypeId 702.
 * Desktop: Architecture.WinApp.PackingMaterial_Store.PurchaseInvoicePackingMaterial + LoadGrnForStore.
 *
 *   InvfrmPurchaseInvoice_Load :294   FormValidation :1109   Insert :1916   ReadById :1817   Reset :1758
 *   LoadDataDetailGridAgainstGrnIds :1240   LoadFreightData :1318   LoadInGridDetail :1386
 *   grd_CellUpdated :1017   CalculateTaxAmount :3013   grd_KeyDown F1 :2956
 *   FreightProportion :1486   WagesAmountProportion :1436   BillProportion :1540   BillAmount :1058
 *   grdGLedger_CellUpdated :867   grdFreight_CellUpdated :828   GetAll :1569
 * The server repeats every figure and validation; nothing here is trusted by it.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/purchase-invoice';
    var L = null, cfg = {}, perms = {};
    var dtGrid = [], dtFreight = [], dtGrdGL = [];
    var RecId = 0, VoucherHeadId = 0;
    var newFiles = [], removeAttachmentIds = [], existingAttachments = [];
    var historyRows = [];
    var currentTab = 0;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    /* Math.Round(x, n, AwayFromZero) */
    function away(x, n) { var f = Math.pow(10, n || 0); var s = x < 0 ? -1 : 1; return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    /* Math.Round(x) - to even */
    function even(x) { var r = Math.round(x); if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2); return r; }
    function dp() { return int(cfg.amountDecimals); }
    function fmt(n, d) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d === undefined ? 4 : d }); }
    function amt(n) { return num(n).toLocaleString('en-US', { minimumFractionDigits: dp(), maximumFractionDigits: dp() }); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function addDays(isoDate, days) { var p = String(isoDate || today()).split('-'); var d = new Date(+p[0], +p[1] - 1, +p[2]); d.setDate(d.getDate() + days); return iso(d); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddmmm(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0]; }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } applyRights(); };
        var p; try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function http(method, url, body) {
        var opt = { method: method, headers: { 'Accept': 'application/json' }, credentials: 'same-origin' };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
                return b;
            });
        });
    }
    function getJson(url) { return http('GET', url); }
    function fill(id, rows, v, t, blank) {
        var sel = $id(id); if (!sel) return; var keep = sel.value;
        var html = blank === false ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, v)) + '">' + esc(col(r, t)) + '</option>'; });
        sel.innerHTML = html;
        if (keep && sel.querySelector('option[value="' + String(keep).replace(/"/g, '') + '"]')) sel.value = keep;
    }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function focus(id) { var e = $id(id); if (e) try { e.focus(); } catch (x) { /* ignore */ } }
    function show(id, on) { var e = $id(id); if (e) e.style.display = on ? '' : 'none'; }
    function accKey() { return cfg.subsidiaryAccounts ? 'SupplierCustomerId' : 'Id'; }
    function accountOptions(current) {
        var html = '<option value="0"></option>', k = accKey();
        (L && L.accounts || []).forEach(function (a) {
            var v = int(col(a, k));
            html += '<option value="' + v + '"' + (v === int(current) ? ' selected' : '') + '>' + esc(col(a, 'AccountTitle')) + '</option>';
        });
        return html;
    }
    function billTypeText(id) { id = int(id); return id === 1 ? 'On Weight' : id === 2 ? 'On Qty' : ''; }

    // ------------------------------------------------------------------ load

    function init() {
        bindEvents();
        setVal('DocDate', today());
        setVal('DueDate', today());
        setVal('FromDateHistory', today()); setVal('ToDateHistory', today());
        loadLookups().then(function () { AddRowInGLGrid(); AddRowInFreightGrid(); renderAll(); });
    }

    function loadLookups() {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d; cfg = d.configuration || {}; perms = d.permissions || {};
            fill('cmbsuppliername', d.suppliers, 'Id', 'CompanyName');
            fill('CmbTaxAccount', d.taxAccounts, 'Id', 'AccountTitle');
            fill('CmbPaymentTerm', d.paymentTerms, 'Id', 'Description');
            if (!val('CmbPaymentTerm') && (d.paymentTerms || []).length) setVal('CmbPaymentTerm', col(d.paymentTerms[0], 'Id'));   // Rows[1].Activate()
            if (RecId === 0) { setVal('txtdocno', d.docNo); setVal('txtBranchSrNo', d.branchSrNo); $id('txtDocNoShow').textContent = 'PI-' + d.docNo; }
            var bl = '';
            (d.branches || []).forEach(function (b) {
                bl += '<label class="win-check" style="display:block"><input type="checkbox" value="' + int(b.Id) + '"' + (int(b.Id) === int(d.userBranchId) ? ' checked' : '') + '/> ' + esc(b.Description) + '</label>';
            });
            $id('cmbBranchName').innerHTML = bl;
            fill('cmbSupplierNameHistory', d.historySuppliers, 'Id', 'Description');
            CmbPaymentTerm_TextChanged();
            refreshCombos(); applyRights(); say('');
        }).catch(function (e) { say(''); box(e.message); });
    }

    function applyRights() {
        var upd = RecId > 0;
        show('btnSave', !upd); show('btnUpdate', upd); show('btnDelete', upd);
        $id('btnSave').disabled = !perms.Save; $id('btnUpdate').disabled = !perms.Update; $id('btnDelete').disabled = !perms.Delete;
        ['btnSlipDetail', 'BtnPrintII', 'btnPrint'].forEach(function (b) { $id(b).disabled = !perms.Print; });
    }

    // ------------------------------------------------------------------ grids

    function AddRowInGLGrid() { dtGrdGL.push({ AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }); }
    function AddRowInFreightGrid() { dtFreight.push({ InvGrnId: 0, Transporter: 0, Freight: 0, Debit: 0, Remarks: '' }); }

    function renderAll() { recalc(); renderGrid(); renderFreight(); renderGL(); }

    var GRID = [['OrderNo', 'OrderNo'], ['ItemName', 'ItemName'], ['WareHouseName', 'WareHouseName'], ['RackName', 'RackName'],
        ['ItemCondition', 'ItemCondition'], ['UOMCodeItem', 'Pack Uom'], ['ItemQty', 'ItemQty', 'n'], ['OrderBagWt', 'OrderBagWt', 'n2'],
        ['BagWt', 'BagWt', 'n2'], ['NetBillWeight', 'NetBillWeight', 'n'], ['Rate', 'Rate', 'n'], ['RateUomCode', 'Rate Uom']];

    function renderGrid() {
        var head = ''; GRID.forEach(function (c) { head += '<th>' + c[1] + '</th>'; });
        head += '<th>AddLsAmount</th><th>ItemAmount</th><th>TaxName</th><th>Tax%</th><th>TaxAmount</th><th>Item Net Amount</th><th>Freights</th><th>Wages</th>';
        $id('grdHead').innerHTML = head;
        var html = '', t = { q: 0, w: 0, a: 0, tx: 0, b: 0, f: 0, wg: 0 };
        dtGrid.forEach(function (r, i) {
            html += '<tr class="data-row" data-i="' + i + '">';
            GRID.forEach(function (c) {
                var v = r[c[0]];
                html += '<td' + (c[2] ? ' class="num"' : '') + '>' + (c[2] === 'n2' ? fmt(v, 2) : c[2] ? fmt(v) : esc(v)) + '</td>';
            });
            html += '<td><input class="cell-sm" data-f="AddLsAmount" data-guard="decimal" value="' + esc(r.AddLsAmount || 0) + '"/></td>'
                + '<td class="num">' + fmt(r.ItemAmount) + '</td>'
                + '<td><span class="win-link" data-tax="' + i + '">' + (esc(r.TaxName) || '[select]') + '</span></td>'
                + '<td>' + (cfg.taxPercentEditable ? '<input class="cell-sm" data-f="TaxPercent" data-guard="decimal" value="' + esc(r.TaxPercent || 0) + '"/>' : fmt(r.TaxPercent)) + '</td>'
                + '<td class="num">' + amt(r.TaxAmount) + '</td><td class="num">' + amt(r.BillAmount) + '</td>'
                + '<td class="num">' + fmt(r.Freights, 3) + '</td><td class="num">' + fmt(r.Wages, 3) + '</td></tr>';
            t.q += num(r.ItemQty); t.w += num(r.NetBillWeight); t.a += num(r.ItemAmount); t.tx += num(r.TaxAmount); t.b += num(r.BillAmount); t.f += num(r.Freights); t.wg += num(r.Wages);
        });
        $id('grd').innerHTML = html;
        $id('grdFoot').innerHTML = dtGrid.length ? '<td colspan="6">' + dtGrid.length + ' row(s)</td><td class="num">' + fmt(t.q) + '</td><td></td><td></td><td class="num">' + fmt(t.w)
            + '</td><td colspan="3"></td><td class="num">' + fmt(t.a) + '</td><td colspan="2"></td><td class="num">' + amt(t.tx) + '</td><td class="num">' + amt(t.b)
            + '</td><td class="num">' + fmt(t.f, 3) + '</td><td class="num">' + fmt(t.wg, 3) + '</td>' : '';
    }

    function renderFreight() {
        $id('frHead').innerHTML = '<th>+</th><th>X</th><th>Transporter</th><th>Credit</th><th>Remarks</th>';
        var html = '', tot = 0;
        dtFreight.forEach(function (r, i) {
            tot += num(r.Freight);
            html += '<tr data-fr="' + i + '"><td><button type="button" class="win-btn-mini" data-act="add">+</button></td>'
                + '<td><button type="button" class="win-btn-mini" data-act="del">X</button></td>'
                + '<td><select class="cell-sel" data-f="Transporter">' + accountOptions(r.Transporter) + '</select></td>'
                + '<td><input class="cell-sm" data-f="Freight" data-guard="decimal" value="' + esc(r.Freight) + '"/></td>'
                + '<td><input class="cell-text" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td></tr>';
        });
        $id('grdFreight').innerHTML = html;
        $id('frFoot').innerHTML = '<td colspan="3"></td><td class="num">' + fmt(tot, 3) + '</td><td></td>';
    }

    function renderGL() {
        $id('glHead').innerHTML = '<th>+</th><th>X</th><th>Account</th><th>Remarks</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th>';
        var html = '', d = 0, c = 0;
        dtGrdGL.forEach(function (r, i) {
            d += num(r.Debit); c += num(r.Credit);
            html += '<tr data-gl="' + i + '"><td><button type="button" class="win-btn-mini" data-act="add">+</button></td>'
                + '<td><button type="button" class="win-btn-mini" data-act="del">X</button></td>'
                + '<td><select class="cell-sel" data-f="AccountId">' + accountOptions(r.AccountId) + '</select></td>'
                + '<td><input class="cell-text" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td>';
            ['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'].forEach(function (f) {
                html += '<td><input class="cell-sm" data-f="' + f + '" data-guard="decimal" value="' + esc(r[f]) + '"/></td>';
            });
            html += '</tr>';
        });
        $id('grdGLedger').innerHTML = html;
        $id('glFoot').innerHTML = '<td colspan="7"></td><td class="num">' + fmt(d) + '</td><td class="num">' + fmt(c) + '</td>';
    }

    // ------------------------------------------------------------------ the form's maths

    function ItemAmountFor(r) {
        var bt = val('txtBillType');
        var a = bt === 'On Weight' ? (num(r.RateUom) ? num(r.NetBillWeight) / num(r.RateUom) * num(r.Rate) : 0) : bt === 'On Qty' ? num(r.ItemQty) * num(r.Rate) : 0;
        if (num(r.AddLsAmount) !== 0 && a > 0) a += num(r.AddLsAmount);
        return a;
    }
    /** grd_CellUpdated :1017 / CalculateTaxAmount :3013 */
    function cellUpdated(r) {
        r.Edited = true;
        r.ItemAmount = ItemAmountFor(r);
        var pct = num(r.TaxPercent);
        if (pct > 100) { pct = 0; box('Tax% cannot be greater than 100. It has been reset to 0.'); }
        r.TaxPercent = pct;
        r.TaxAmount = (r.ItemAmount > 0 && pct > 0) ? away(r.ItemAmount * pct / 100, dp()) : 0;
    }
    function supplierGl() {
        var id = int(val('cmbsuppliername'));
        var s = (L && L.suppliers || []).filter(function (x) { return int(col(x, 'Id')) === id; })[0];
        return s ? int(col(s, 'GlAccountId')) : 0;
    }
    /** FreightProportion, WagesAmountProportion, BillProportion, BillAmount. */
    function recalc() {
        var netQty = 0, netW = 0, itemTotal = 0, taxTotal = 0;
        dtGrid.forEach(function (r) { netQty += num(r.ItemQty); netW += num(r.NetBillWeight); itemTotal += num(r.ItemAmount); taxTotal += num(r.TaxAmount); });
        var credit = 0, debit = 0; dtFreight.forEach(function (f) { credit += num(f.Freight); debit += num(f.Debit); });
        var bt = val('txtBillType');
        dtGrid.forEach(function (r) {
            if (debit < credit) {
                var diff = even(credit - debit);
                r.Freights = bt === 'On Qty' ? diff / netQty * num(r.ItemQty) : bt === 'On Weight' ? diff / netW * num(r.NetBillWeight) : 0;
            } else r.Freights = 0;
        });
        var wages = num(val('txtWagesAmountHeader'));
        dtGrid.forEach(function (r) { r.Wages = wages > 0 ? (cfg.wagesOnQty ? wages / netQty * num(r.ItemQty) : wages / netW * num(r.NetBillWeight)) : 0; });
        dtGrid.forEach(function (r) {
            r.BillAmount = away(num(r.ItemAmount) + num(r.TaxAmount) + num(r.Freights) + (cfg.wagesChargeToProduct ? num(r.Wages) : 0), dp());
        });
        var jd = 0, jc = 0;
        dtGrdGL.forEach(function (j) { if (int(val('cmbsuppliername')) && int(j.AccountId) > 0) { jd += num(j.Debit); jc += num(j.Credit); } });
        var bill = away(even(itemTotal) + taxTotal + jd - jc, dp());
        setVal('txtBillAmount', amt(bill));
    }

    // ------------------------------------------------------------------ GRN loading

    /** LoadDataDetailGridAgainstGrnIds :1240 + LoadFreightData :1318 */
    function LoadInGridDetail(ids) {
        return http('POST', api + '/load-grns', ids).then(function (d) {
            var rows = d.lines || [];
            if (!rows.length) return;
            var r0 = rows[0];
            if (dtGrid.length) {
                if (int(col(r0, 'RefDocumentTypeId')) === 52) {
                    if (int(val('cmbsuppliername')) > 0 && int(val('cmbsuppliername')) !== int(col(r0, 'SupplierCustomerId'))) { box("You Can't load Grn of other customer"); return; }
                    var s = billTypeText(col(r0, 'BillCalculateTypeId'));
                    if (val('txtBillType') && s !== val('txtBillType')) { box("You Can't load Grn of other Bill Type"); return; }
                } else if (int(col(r0, 'PurchaseOrderId')) !== int(dtGrid[0].PurchaseOrderId)) { box("You Can't load Grn of other Order"); return; }
            }
            if (int(col(r0, 'RefDocumentTypeId')) === 52) $id('CmbPaymentTerm').disabled = false;
            else { $id('CmbPaymentTerm').disabled = true; setVal('CmbPaymentTerm', int(col(r0, 'PaymentTermsId'))); }
            setVal('cmbsuppliername', int(col(r0, 'SupplierCustomerId')));
            setVal('txtRemarks', col(r0, 'RemarksHeader'));
            setVal('txtDueDays', col(r0, 'OrderDueDays'));
            setVal('txtDeliveryTerm', col(r0, 'DeliveryTerm'));
            setVal('txtBillType', billTypeText(col(r0, 'BillCalculateTypeId')));
            var have = {}; dtGrid.forEach(function (g) { have[int(g.InvGrnDetailId)] = 1; });
            rows.forEach(function (g) {
                if (have[int(col(g, 'Id'))]) return;
                dtGrid.push({ Id: 0, PurchaseOrderId: int(col(g, 'PurchaseOrderId')), OrderNo: col(g, 'OrderNo'), InvGrnDetailId: int(col(g, 'Id')),
                    InvGrnId: int(col(g, 'InvGrnId')), ItemId: int(col(g, 'ItemId')), ItemName: col(g, 'ItemName'), WareHouseName: col(g, 'WareHouseName'),
                    RackName: col(g, 'rackName'), ItemCondition: col(g, 'ItemCondition'), UOMCodeItem: col(g, 'UOMCode'), ItemQty: num(col(g, 'ItemQty')),
                    OrderBagWt: num(col(g, 'OrderBagWt')), BagWt: num(col(g, 'BagWt')), NetBillWeight: num(col(g, 'NetBillWeight')), Rate: num(col(g, 'ItemRate')),
                    RateUom: num(col(g, 'RateUom')), RateUomCode: col(g, 'RateUomCode'), AddLsAmount: 0, ItemAmount: num(col(g, 'ItemAmount')),
                    TaxNameId: int(col(g, 'TaxNameId')), TaxName: col(g, 'TaxName'), TaxPercent: num(col(g, 'TaxPercent')), TaxAmount: num(col(g, 'TaxAmount')),
                    BillAmount: 0, Freights: 0, Wages: 0, Edited: false });
            });
            $id('cmbsuppliername').disabled = true;
            setVal('txtWagesAmountHeader', d.wagesAmount || 0);
            LoadFreightData(d.freight || []);
            if (!dtFreight.length) AddRowInFreightGrid();
            if (!dtGrdGL.length) AddRowInGLGrid();
            CmbPaymentTerm_TextChanged(); dueDate();
            refreshCombos(); renderAll();
        }).catch(function (e) { box(e.message); });
    }

    function LoadFreightData(items) {
        if (!items.length) return;
        var term = val('txtDeliveryTerm');
        var fsum = 0; dtFreight.forEach(function (f) { fsum += num(f.Freight); }); if (fsum === 0) dtFreight = [];
        var d = 0, c = 0; dtGrdGL.forEach(function (j) { d += num(j.Debit); c += num(j.Credit); }); if (d === 0 && c === 0) dtGrdGL = [];
        items.forEach(function (it) {
            if (Math.round(num(col(it, 'CarriageAmount'))) <= 0) return;
            var rem = '';
            if (int(col(it, 'GpNo')) > 0) rem += 'GpNo: ' + col(it, 'GpNo');
            if (col(it, 'VehicleNo')) rem += '   VehicleNo:  ' + col(it, 'VehicleNo');
            if (col(it, 'BiltyNo')) rem += '   BiltyNo: ' + col(it, 'BiltyNo');
            var acc = cfg.subsidiaryAccounts ? int(col(it, 'TransporterSupCustId')) : int(col(it, 'Transporter'));
            if (term === 'Load' || term === 'Load & PartyWeight' || term === 'Load & FactoryWeight')
                dtFreight.push({ InvGrnId: int(col(it, 'MainId')), Transporter: acc, Freight: num(col(it, 'CarriageAmount')), Debit: 0, Remarks: rem });
            else
                dtGrdGL.push({ AccountId: acc, Remarks: rem, Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: num(col(it, 'CarriageAmount')) });
        });
    }

    var loader = (function () {
        var rows = [];
        var COLS = [['DocDate', 'DocDate', 'date'], ['DocNo', 'DocNo'], ['BranchName', 'BranchName'], ['GpNo', 'GpNo'], ['OrderType', 'OrderType'],
            ['BillCalculateType', 'BillCalculateType'], ['VehicleNo', 'VehicleNo'], ['BiltyNo', 'BiltyNo'], ['SupplierName', 'SupplierName'],
            ['OrderNo', 'OrderNo'], ['ItemQty', 'ItemQty', 'n'], ['ItemName', 'ItemName'], ['ItemCode', 'ItemCode'], ['UOMCode', 'UOMCode'],
            ['WareHouseName', 'WareHouseName'], ['ConditionStatus', 'ItemCondition']];
        function open() {
            $id('dlgLoadGrn').classList.add('open');
            if (!val('LoaderFromDate')) setVal('LoaderFromDate', cfg.financialYearStart || today());
            if (!val('LoaderToDate')) setVal('LoaderToDate', today());
            return load();
        }
        function close() { $id('dlgLoadGrn').classList.remove('open'); }
        function load() {
            return getJson(api + '/pending-grns?fromDate=' + encodeURIComponent(val('LoaderFromDate')) + '&toDate=' + encodeURIComponent(val('LoaderToDate'))).then(function (d) {
                rows = d || [];
                var head = '<th></th>'; COLS.forEach(function (c) { head += '<th>' + c[1] + '</th>'; });
                $id('loaderHead').innerHTML = head;
                var html = '';
                rows.forEach(function (r, i) {
                    html += '<tr class="data-row"><td><input type="checkbox" data-pick="' + i + '"/></td>';
                    COLS.forEach(function (c) { var v = col(r, c[0]); html += '<td' + (c[2] === 'n' ? ' class="num"' : '') + '>' + (c[2] === 'date' ? ddmmm(v) : c[2] === 'n' ? fmt(v) : esc(v)) + '</td>'; });
                    html += '</tr>';
                });
                $id('loaderBody').innerHTML = html;
            }).catch(function (e) { box(e.message); });
        }
        /** btnLoadOnInvoice_Click_1 - DocumentTypeId 701 branch. */
        function take() {
            var picked = Array.prototype.filter.call(document.querySelectorAll('#loaderBody input[data-pick]'), function (x) { return x.checked; })
                .map(function (x) { return rows[int(x.getAttribute('data-pick'))]; });
            if (!picked.length) { box('Chek the row first'); return; }
            var type = 0, order = 0, party = 0, bill = 0, ids = [];
            for (var i = 0; i < picked.length; i++) {
                var r = picked[i], t = int(col(r, 'RefDocumentTypeId'));
                if (!type) type = t;
                if (type !== t) { box('You Can Only Select Rows Of Same Order Type'); return; }
                if (t === 52) {
                    var s = int(col(r, 'SupplierCustomerId')), b = int(col(r, 'BillCalculateTypeId'));
                    if (!party) party = s; if (!bill) bill = b;
                    if (party !== s) { box('You Can Only Select Rows Of Same Party'); return; }
                    if (bill !== b) { box('You Can Only Select Rows Of Bill Calculate Type'); return; }
                } else {
                    var o = int(col(r, 'PurchaseOrderId')); if (!order) order = o;
                    if (order !== o) { box('You Can Only Select Rows Of Same Order'); return; }
                }
                ids.push(int(col(r, 'Id')));
            }
            close();
            return LoadInGridDetail(ids);
        }
        return { open: open, close: close, load: load, take: take };
    })();

    // ------------------------------------------------------------------ tax popup (F1)

    var taxRow = -1, taxRows = [];
    function openTax(i) {
        taxRow = i; var r = dtGrid[i];
        return getJson(api + '/tax-options?itemId=' + int(r.ItemId) + '&docDate=' + encodeURIComponent(val('DocDate'))).then(function (rows) {
            taxRows = rows || [];
            if (!taxRows.length) return;
            var html = '';
            taxRows.forEach(function (t, k) { html += '<tr><td>' + esc(col(t, 'TaxName')) + '</td><td class="num">' + fmt(col(t, 'TaxPercent')) + '</td><td><button type="button" class="win-btn-mini" data-pick-tax="' + k + '">Select</button></td></tr>'; });
            html += '<tr><td colspan="2"><em>none</em></td><td><button type="button" class="win-btn-mini" data-pick-tax="-1">Select</button></td></tr>';
            $id('taxBody').innerHTML = html;
            $id('dlgTax').classList.add('open');
        }).catch(function (e) { box(e.message); });
    }
    function pickTax(k) {
        var r = dtGrid[taxRow]; if (!r) return;
        var t = k >= 0 ? taxRows[k] : null;
        var id = t ? int(col(t, 'TaxNameId')) : 0, pct = t ? num(col(t, 'TaxPercent')) : 0;
        r.TaxNameId = id; r.TaxName = t ? col(t, 'TaxName') : '';
        if (!cfg.taxPercentEditable || id === 0) r.TaxPercent = pct;
        else if (num(r.TaxPercent) === 0) r.TaxPercent = pct;
        cellUpdated(r);
        taxClose(); renderAll();
    }
    function taxClose() { $id('dlgTax').classList.remove('open'); }

    // ------------------------------------------------------------------ header events

    function CmbPaymentTerm_TextChanged() {
        if (int(val('CmbPaymentTerm')) === 1) { setVal('txtDueDays', ''); $id('txtDueDays').disabled = true; }
        else $id('txtDueDays').disabled = false;
        dueDate();
    }
    function dueDate() { setVal('DueDate', val('txtDueDays').trim() !== '' ? addDays(val('DocDate'), num(val('txtDueDays'))) : val('DocDate')); }

    // ------------------------------------------------------------------ save

    function FormValidation() {
        if (!val('txtdocno').trim() || val('txtdocno').trim() === '0') { box('DocNo Field is Required'); return false; }
        if (!int(val('cmbsuppliername'))) { box('Supplier Name Field is Required'); return false; }
        if (int(val('CmbPaymentTerm')) <= 0) { box('Payment Term Field is Required'); return false; }
        if (int(val('CmbPaymentTerm')) === 2 && int(val('txtDueDays')) <= 0) { box('Due Days Field is Required'); return false; }
        return true;
    }

    function Insert(btnId) {
        if (!FormValidation()) return;
        if (!window.confirm(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var i = 0; i < dtFreight.length; i++) if (num(dtFreight[i].Freight) > 0 && !int(dtFreight[i].Transporter)) { box('Please Select an Account Against Freight First'); return; }
        for (var j = 0; j < dtGrdGL.length; j++) if ((num(dtGrdGL[j].Credit) > 0 || Math.round(num(dtGrdGL[j].Debit)) > 0) && !int(dtGrdGL[j].AccountId)) { box('Please Select an Account Against JL First'); return; }
        if (dtGrid.some(function (r) { return num(r.TaxAmount) > 0; }) && !int(val('CmbTaxAccount'))) { box('Tax Account is Required'); focus('CmbTaxAccount'); return; }
        if (!dtGrid.length) { box('Grid Record Not found'); return; }
        var req = {
            Id: RecId, DocDate: val('DocDate'), ManualBillNo: val('txtbillno'), RemarksHeader: val('txtRemarks'),
            PaymentTermsId: int(val('CmbPaymentTerm')), DueDays: val('txtDueDays'), TaxAccountId: int(val('CmbTaxAccount')),
            lines: dtGrid.map(function (r) { return { Id: int(r.Id), InvGrnId: int(r.InvGrnId), InvGrnDetailId: int(r.InvGrnDetailId), Edited: !!r.Edited,
                AddLsAmount: num(r.AddLsAmount), TaxNameId: int(r.TaxNameId), TaxPercent: num(r.TaxPercent) }; }),
            freight: dtFreight.map(function (f) { return { InvGrnId: int(f.InvGrnId), Transporter: int(f.Transporter), Freight: num(f.Freight), Debit: num(f.Debit), Remarks: f.Remarks || '' }; }),
            journal: dtGrdGL.map(function (g) { return { AccountId: int(g.AccountId), Remarks: g.Remarks || '', Percentage: num(g.Percentage), Qty: num(g.Qty), Rate: num(g.Rate), Debit: num(g.Debit), Credit: num(g.Credit) }; }),
            files: newFiles, removeAttachmentIds: removeAttachmentIds
        };
        return busy(btnId, function () {
            say('Saving...');
            return http('POST', api + '/save', req).then(function (d) {
                say(''); box(d.message);
                var id = d.id, vh = d.voucherHeadId;
                return Reset().then(function () {
                    if ($id('ChkBok').checked) GeneratePurchaseSlip231(id);
                    if ($id('chkPreviewII').checked) PrintII(id);
                    if ($id('ChkvoucherPre').checked) VoucherSlip(vh);
                });
            }).catch(function (e) { say(''); box(e.message); });
        });
    }

    function btnDelete_Click() {
        if (RecId <= 0) { box('Record Not Found'); return; }
        if (!perms.Delete || !window.confirm('Are you sure to Delete?')) return;
        return busy('btnDelete', function () {
            return http('POST', api + '/' + RecId + '/delete').then(function (d) { box(d.message); return Reset(); }).catch(function (e) { box(e.message); });
        });
    }

    function GeneratePurchaseSlip231(id) { if (!id) { box('Record Id Not Found'); return; } window.open('/api/reports/pi-pm-231/data?id=' + encodeURIComponent(id), '_blank'); }
    function PrintII(id) { if (!id) { box('Record Id Not Found'); return; } window.open('/api/reports/pi-pm-231-01/data?id=' + encodeURIComponent(id), '_blank'); }
    function VoucherSlip(vh) { if (!vh) { box('No Record Found For Display'); return; } window.open('/api/reports/acc-103/data?id=' + encodeURIComponent(vh), '_blank'); }

    // ------------------------------------------------------------------ reset / read

    function Reset() {
        RecId = 0; VoucherHeadId = 0;
        newFiles = []; removeAttachmentIds = []; existingAttachments = []; renderAttachments();
        ['txtDeliveryTerm', 'txtbillno', 'txtDueDays', 'txtRemarks', 'txtBillAmount', 'cmbsuppliername', 'txtBillType', 'txtWagesAmountHeader'].forEach(function (k) { setVal(k, ''); });
        $id('cmbsuppliername').disabled = false; $id('CmbPaymentTerm').disabled = false;
        dtGrid = []; dtFreight = []; dtGrdGL = []; AddRowInGLGrid(); AddRowInFreightGrid();
        setVal('DocDate', today());
        renderAll(); applyRights();
        return getJson(api + '/numbers').then(function (d) {
            setVal('txtdocno', d.docNo); setVal('txtBranchSrNo', d.branchSrNo); $id('txtDocNoShow').textContent = 'PI-' + d.docNo;
            CmbPaymentTerm_TextChanged(); refreshCombos(); focus('DocDate');
        }).catch(function (e) { box(e.message); });
    }
    function btnNew_Click() { return busy('btnNew', Reset); }
    function btnFrmRefresh_Click() { return busy('btnFrmRefresh', function () { return loadLookups().then(renderAll); }); }

    function ReadById(id) {
        say('Loading...');
        return getJson(api + '/' + id).then(function (d) {
            say('');
            var h = d.header;
            RecId = int(col(h, 'Id')); VoucherHeadId = int(d.voucherHeadId);
            setVal('txtdocno', col(h, 'DocNo')); $id('txtDocNoShow').textContent = 'PI-' + col(h, 'DocNo');
            setVal('DocDate', dateOnly(col(h, 'DocDate')));
            if (int(col(h, 'PaymentTermsId')) > 0) setVal('CmbPaymentTerm', int(col(h, 'PaymentTermsId')));
            setVal('txtDueDays', col(h, 'DueDays'));
            setVal('txtBranchSrNo', col(h, 'BranchSrNo'));
            showTab(0);
            setVal('cmbsuppliername', int(col(h, 'SupplierCustomerId'))); $id('cmbsuppliername').disabled = true;
            setVal('txtbillno', col(h, 'ManualBillNo'));
            setVal('txtRemarks', col(h, 'RemarksHeader'));
            setVal('txtDeliveryTerm', col(h, 'DeliveryTerm'));
            setVal('txtBillType', billTypeText(col(h, 'BillCalculateTypeId')));
            if (int(col(h, 'TaxAccountId')) > 0) setVal('CmbTaxAccount', int(col(h, 'TaxAccountId')));
            dtGrid = (d.lines || []).map(function (l) { var r = {}; for (var k in l) r[k] = l[k]; r.Edited = false; return r; });
            dtGrdGL = (d.journal || []).map(function (j) {
                return { AccountId: cfg.subsidiaryAccounts ? int(col(j, 'SupplierCustomerId')) : int(col(j, 'ChartofAccountId')), Remarks: col(j, 'JvRemarks'),
                    Percentage: num(col(j, 'JvPrcnt')), Qty: num(col(j, 'JvQty')), Rate: num(col(j, 'JvRate')), Debit: num(col(j, 'JvDebit')), Credit: num(col(j, 'JvCredit')) };
            });
            dtFreight = (d.freight || []).map(function (f) {
                return { InvGrnId: int(col(f, 'InvGrnId')), Transporter: cfg.subsidiaryAccounts ? int(col(f, 'SupplierCustomerId')) : int(col(f, 'TansporterId')),
                    Freight: num(col(f, 'FreightAmount')), Debit: num(col(f, 'Debit')), Remarks: col(f, 'Remarks') };
            });
            if (!dtGrdGL.length) AddRowInGLGrid();
            if (!dtFreight.length) AddRowInFreightGrid();
            if (d.wagesAmount !== null && d.wagesAmount !== undefined) setVal('txtWagesAmountHeader', d.wagesAmount);
            if (dtGrid.length && int(dtGrid[0].PurchaseOrderId) > 0) $id('CmbPaymentTerm').disabled = true;
            existingAttachments = d.attachments || []; newFiles = []; removeAttachmentIds = []; renderAttachments();
            CmbPaymentTerm_TextChanged(); refreshCombos(); renderAll(); applyRights();
        }).catch(function (e) { say(''); box(e.message); });
    }

    // ------------------------------------------------------------------ history

    function branchIds() {
        return Array.prototype.filter.call(document.querySelectorAll('#cmbBranchName input'), function (x) { return x.checked; }).map(function (x) { return x.value; }).join(',');
    }
    var H = [['BranchSrNo', 'BranchSrNo'], ['BranchName', 'BranchName'], ['DocNo', 'DocNo'], ['DocDate', 'DocDate', 'date'], ['SupplierName', 'SupplierName'],
        ['ManualBillNo', 'ManualBillNo'], ['PurchaseAgainst', 'PurchaseAgainst'], ['DueDays', 'DueDays'], ['DueDate', 'DueDate', 'date'],
        ['BillType', 'BillType'], ['BillAmount', 'BillAmount', 'n'], ['ApprovedStatus', 'ApprovedStatus'], ['EntryUser', 'EntryUser'],
        ['ModifyUser', 'ModifyUser'], ['ApprovedUser', 'ApprovedUser'], ['NoOfAttachments', 'NoOfAttachments'], ['RemarksHeader', 'Remarks']];
    function GetAll() {
        var b = branchIds();
        if (!b) { box('Select branch first'); return; }
        var q = '?branchIds=' + encodeURIComponent(b)
            + ($id('chkFromDate').checked ? '&fromDate=' + encodeURIComponent(val('FromDateHistory')) : '')
            + ($id('chkToDate').checked ? '&toDate=' + encodeURIComponent(val('ToDateHistory')) : '')
            + '&fromDocNo=' + int(val('txtFromDocNoHistory')) + '&toDocNo=' + int(val('txtToDocNoHistory')) + '&supplierId=' + int(val('cmbSupplierNameHistory'));
        return getJson(api + '/history' + q).then(function (rows) {
            historyRows = rows || [];
            var head = '<th>Edit</th><th>Print</th><th>PrintII</th><th>Voucher</th>'; H.forEach(function (c) { head += '<th>' + c[1] + '</th>'; });
            $id('histHead').innerHTML = historyRows.length ? head : '';
            var html = '';
            historyRows.forEach(function (r, i) {
                html += '<tr class="data-row" data-h="' + i + '">' + ['edit', 'print', 'print2', 'voucher'].map(function (a) {
                    return '<td><button type="button" class="win-btn-mini" data-h-act="' + a + '">' + a.charAt(0).toUpperCase() + a.slice(1) + '</button></td>';
                }).join('');
                H.forEach(function (c) { var v = col(r, c[0]); html += '<td' + (c[2] === 'n' ? ' class="num"' : '') + '>' + (c[2] === 'date' ? ddmmm(v) : c[2] === 'n' ? amt(v) : esc(v)) + '</td>'; });
                html += '</tr>';
            });
            $id('grdHistory').innerHTML = html; $id('grdDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
            $id('lblHistCount').textContent = historyRows.length ? historyRows.length + ' record(s)' : '';
        }).catch(function (e) { box(e.message); });
    }
    function onHistoryClick(e) {
        var tr = e.target.closest('tr[data-h]'); if (!tr) return;
        var r = historyRows[int(tr.getAttribute('data-h'))]; if (!r) return;
        Array.prototype.forEach.call(document.querySelectorAll('#grdHistory tr'), function (x) { x.classList.toggle('sel', x === tr); });
        var id = int(col(r, 'Id')), b = e.target.closest('button[data-h-act]');
        if (!b) { detail(id); return; }
        var a = b.getAttribute('data-h-act');
        if (a === 'edit') { if (String(col(r, 'ApprovedStatus')) === 'APPROVED') { box("Can't Update Approved Record"); return; } ReadById(id); }
        else if (a === 'print') { if (perms.Print) GeneratePurchaseSlip231(id); }
        else if (a === 'print2') { if (perms.Print) PrintII(id); }
        else if (a === 'voucher') { if (perms.Print) getJson(api + '/' + id + '/voucher').then(function (v) { VoucherSlip(v.voucherHeadId); }).catch(function (x) { box(x.message); }); }
    }
    function detail(id) {
        return getJson(api + '/' + id).then(function (d) {
            var cols = [['OrderNo', 'OrderNo'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['WareHouseName', 'Warehouse'], ['RackName', 'RackName'],
                ['ItemCondition', 'ItemCondition'], ['UOMCodeItem', 'PackUom'], ['ItemQty', 'ItemQty', 'n'], ['NetBillWeight', 'NetBillWeight', 'n'], ['Rate', 'Rate', 'n'],
                ['RateUomCode', 'RateUom'], ['AddLsAmount', 'AddLessAmount', 'n'], ['ItemAmount', 'ItemAmount', 'n'], ['TaxName', 'TaxName'], ['TaxPercent', 'TaxPrcnt', 'n'],
                ['TaxAmount', 'TaxAmount', 'n'], ['Freights', 'Freight', 'n'], ['Wages', 'Wages', 'n'], ['BillAmount', 'NetAmount', 'n']];
            var head = ''; cols.forEach(function (c) { head += '<th>' + c[1] + '</th>'; }); $id('histDetailHead').innerHTML = head;
            var html = ''; (d.lines || []).forEach(function (l) { html += '<tr>'; cols.forEach(function (c) { html += '<td' + (c[2] ? ' class="num"' : '') + '>' + (c[2] ? fmt(l[c[0]]) : esc(l[c[0]])) + '</td>'; }); html += '</tr>'; });
            $id('grdDetail').innerHTML = html;
        }).catch(function (e) { box(e.message); });
    }
    function btnNewHistory_Click() {
        setVal('FromDateHistory', today()); setVal('ToDateHistory', today()); setVal('txtFromDocNoHistory', ''); setVal('txtToDocNoHistory', ''); setVal('cmbSupplierNameHistory', '');
        historyRows = []; $id('grdHistory').innerHTML = ''; $id('histHead').innerHTML = ''; $id('grdDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
        refreshCombos();
    }
    function btnRefreshHistory_Click() {
        var b = branchIds(); if (!b) { box('Select branch first'); return; }
        return getJson(api + '/history-suppliers?branchIds=' + encodeURIComponent(b)).then(function (rows) { fill('cmbSupplierNameHistory', rows, 'Id', 'Description'); refreshCombos(); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ attachments

    function btnAttachment_Click() { var g = $id('grpAttachments'); g.style.display = g.style.display === 'none' ? '' : 'none'; }
    function onAttachmentsPicked(e) {
        Array.prototype.slice.call(e.target.files || []).forEach(function (file) {
            if (file.size > 5 * 1024 * 1024) { box('File Size Exceeds 5MB Of File: ' + file.name); return; }
            var reader = new FileReader();
            reader.onload = function () { var s = String(reader.result); newFiles.push({ name: file.name, base64: s.substring(s.indexOf(',') + 1) }); renderAttachments(); };
            reader.readAsDataURL(file);
        });
        e.target.value = '';
    }
    function renderAttachments() {
        var html = '';
        existingAttachments.forEach(function (a) {
            var id = int(col(a, 'Id')); if (removeAttachmentIds.indexOf(id) >= 0) return;
            html += '<div><a class="win-link" href="' + api + '/' + RecId + '/attachments/' + id + '">' + esc(col(a, 'Attachment')) + '</a> <span class="win-link" data-remove="' + id + '">[remove]</span></div>';
        });
        newFiles.forEach(function (f, i) { html += '<div>' + esc(f.name) + ' <em>(new)</em> <span class="win-link" data-drop="' + i + '">[remove]</span></div>'; });
        $id('lstAttachments').innerHTML = html;
    }

    // ------------------------------------------------------------------ events

    function showTab(i) {
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none'; $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0); $id('tabHistory').classList.toggle('active', i === 1);
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function later(fn) { return function (e) { var t = e.target; setTimeout(function () { fn(t, e); }, 0); }; }

    function bindEvents() {
        on('grd', 'change', later(function (t) {
            var f = t.getAttribute('data-f'), tr = t.closest('tr'); if (!f || !tr) return;
            var r = dtGrid[int(tr.getAttribute('data-i'))]; if (!r) return;
            r[f] = num(t.value); cellUpdated(r); renderAll();
        }));
        on('grd', 'click', function (e) { var s = e.target.closest('[data-tax]'); if (s) openTax(int(s.getAttribute('data-tax'))); });
        on('taxBody', 'click', function (e) { var b = e.target.closest('[data-pick-tax]'); if (b) pickTax(int(b.getAttribute('data-pick-tax'))); });

        on('grdFreight', 'change', later(function (t) {
            var f = t.getAttribute('data-f'), tr = t.closest('tr'); if (!f || !tr) return;
            var r = dtFreight[int(tr.getAttribute('data-fr'))]; if (!r) return;
            if (f === 'Remarks') { r.Remarks = t.value; return; }
            if (f === 'Transporter') r.Transporter = int(t.value);
            else if (f === 'Freight') { r.Freight = num(t.value); if (r.Freight > 0 && num(r.Debit) > 0) { r.Freight = 0; box('Debit Side is aleady added'); } }
            renderAll();
        }));
        on('grdFreight', 'click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = int(b.closest('tr').getAttribute('data-fr'));
            if (b.getAttribute('data-act') === 'del') { dtFreight.splice(i, 1); if (!dtFreight.length) AddRowInFreightGrid(); } else AddRowInFreightGrid();
            renderAll();
        });
        on('grdGLedger', 'change', later(function (t) {
            var f = t.getAttribute('data-f'), tr = t.closest('tr'); if (!f || !tr) return;
            var r = dtGrdGL[int(tr.getAttribute('data-gl'))]; if (!r) return;
            if (f === 'Remarks') { r.Remarks = t.value; return; }
            if (f === 'AccountId') {
                r.AccountId = int(t.value);
                var sgl = supplierGl(), acc = (L.accounts || []).filter(function (a) { return int(col(a, accKey())) === r.AccountId; })[0];
                var clash = cfg.subsidiaryAccounts ? int(val('cmbsuppliername')) === r.AccountId : (acc && int(val('cmbsuppliername')) > 0 && sgl === int(acc.Id));
                if (r.AccountId && clash) { box('Selected Account Can not be Same As Supplier Account'); r.AccountId = 0; }
                else if (r.AccountId && sgl === r.AccountId) { box('You cannot select supplier Account'); r.AccountId = 0; }
            } else {
                r[f] = num(t.value);
                if (f === 'Qty' || f === 'Rate') { if (t.value !== '') { r.Credit = num(r.Qty) * num(r.Rate); r.Debit = 0; r.Percentage = 0; } }
                else if (f === 'Percentage') {
                    var tot = 0; dtGrid.forEach(function (g) { tot += num(g.ItemAmount); });
                    var v = tot / 100 * num(r.Percentage);
                    if (v > 0) { r.Credit = even(v); r.Debit = 0; } else { r.Debit = Math.abs(even(v)); r.Credit = 0; }
                    r.Qty = 0; r.Rate = 0;
                } else if (f === 'Credit') { if (num(r.Debit) > 0) { r.Credit = 0; box('Debit Side is aleady added'); } }
                else if (f === 'Debit') { if (num(r.Credit) > 0) { r.Debit = 0; box('Credit Side is aleady added'); } }
            }
            renderAll();
        }));
        on('grdGLedger', 'click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = int(b.closest('tr').getAttribute('data-gl'));
            if (b.getAttribute('data-act') === 'del') { dtGrdGL.splice(i, 1); if (!dtGrdGL.length) AddRowInGLGrid(); } else AddRowInGLGrid();
            renderAll();
        });
        on('CmbPaymentTerm', 'change', CmbPaymentTerm_TextChanged);
        on('txtDueDays', 'input', dueDate);
        on('DocDate', 'change', dueDate);
        on('grdHistory', 'click', onHistoryClick);
        on('grdHistory', 'dblclick', function (e) {
            var tr = e.target.closest('tr[data-h]'); if (!tr || e.target.closest('button')) return;
            var r = historyRows[int(tr.getAttribute('data-h'))];
            if (r) { if (String(col(r, 'ApprovedStatus')) === 'APPROVED') { box("Can't Update Approved Record"); return; } ReadById(int(col(r, 'Id'))); }
        });
        on('fileAttachment', 'change', onAttachmentsPicked);
        on('lstAttachments', 'click', function (e) {
            var rm = e.target.getAttribute('data-remove'), dr = e.target.getAttribute('data-drop');
            if (rm) { removeAttachmentIds.push(int(rm)); renderAttachments(); }
            if (dr !== null && dr !== undefined) { newFiles.splice(int(dr), 1); renderAttachments(); }
        });
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = e.key.toLowerCase();
            if (k === 's') { e.preventDefault(); if (currentTab === 0) GrnPiSave(); else GetAll(); }
            else if (k === 'u') { e.preventDefault(); if (RecId > 0 && perms.Update) Insert('btnUpdate'); }
            else if (k === 'n') { e.preventDefault(); if (currentTab === 0) btnNew_Click(); else btnNewHistory_Click(); }
            else if (k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); }
            else if (k === 'e') { e.preventDefault(); window.location.href = '/dashboard'; }
        });
    }
    function GrnPiSave() { if (RecId === 0 && perms.Save) return Insert('btnSave'); }

    window.PiPm = {
        btnNew_Click: btnNew_Click, btnFrmRefresh_Click: btnFrmRefresh_Click,
        btnSave_Click: function () { if (RecId !== 0 || $id('btnSave').disabled) return; return Insert('btnSave'); },
        btnUpdate_Click: function () { if (RecId === 0 || $id('btnUpdate').disabled) return; return Insert('btnUpdate'); },
        btnDelete_Click: btnDelete_Click,
        btnSlipDetail_Click: function () { if (perms.Print) GeneratePurchaseSlip231(RecId); },
        BtnPrintII_Click: function () { if (perms.Print) PrintII(RecId); },
        btnPrint_Click: function () { if (perms.Print) VoucherSlip(VoucherHeadId); },
        btnAttachment_Click: btnAttachment_Click, loader: loader, taxClose: taxClose,
        showTab: showTab, btnshow_Click: function () { return busy('btnshow', GetAll); },
        btnNewHistory_Click: btnNewHistory_Click, btnRefreshHistory_Click: btnRefreshHistory_Click
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
