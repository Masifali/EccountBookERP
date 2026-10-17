/* Paddy Purchase invoice screens (module 48): 466 frmPurchaseInvoiceForGate and 467 frmPurchaseInvoiceForGhallaMandi share this factory.
   PI.create(cfg) ports the form logic of Architecture.WinApp.Paddy_Purchase/frmPurchaseInvoiceForGate.cs; every point where the two desktop forms differ
   is a cfg option (api, docType, voucherDocType, mainCols, ghalla, editableCols, freightToGL, ...). Line numbers in comments are to frmPurchaseInvoiceForGate.cs;
   "Ghalla" comments refer to frmPurchaseInvoiceForGhallaMandi.cs / frmLoadGrnForGhallaMandi.cs. */
(function (global) {
    'use strict';
    const PI = {};
    PI.create = function (cfg) {
        const $ = PP.$, N = PP.num, API = cfg.api;
        const g = (row, key) => { if (!row) return null; for (const x in row) if (x.toLowerCase() === key.toLowerCase()) return row[x]; return null; };
        const str = v => (v == null ? '' : String(v));
        const show = msg => alert(msg);
        // Math.Round(double): banker's rounding
        const bankers = x => { const f = Math.floor(x), d = x - f; if (d < 0.5 - 1e-12) return f; if (d > 0.5 + 1e-12) return f + 1; return f % 2 === 0 ? f : f + 1; };
        const f0 = x => Math.floor(Math.abs(x) + 0.5) * (x < 0 ? -1 : 1);                       // ToString("0,0") rounds away from zero
        const thou = (x, dec) => Number(x).toLocaleString('en-US', { maximumFractionDigits: dec == null ? 0 : dec, minimumFractionDigits: 0 });
        const dateInput = v => { if (v == null || v === '') return PP.today(); const d = typeof v === 'number' ? new Date(v) : new Date(String(v).replace(' ', 'T')); return isNaN(d) ? PP.today() : PP.localStamp(d).slice(0, 10); };
        function fmt(v, f) {
            const n = Number(v); if (v === '' || v == null || isNaN(n)) return PP.esc(str(v));
            if (f === 'n2') return thou(n, 2);
            if (f === 'rate') return n === 0 ? '0' : n < 0 ? '(' + Math.abs(n).toFixed(2) + ')' : n.toFixed(2);
            if (f === 'int') return n === 0 ? '0' : n < 0 ? '(' + Math.abs(Math.round(n)) + ')' : String(Math.round(n));
            if (f === 'c0') return thou(f0(n), 0);
            if (f === 'c1') return thou(f0(n), 0);
            return PP.esc(str(v));
        }

        let Id = 0, POID = 0, GRNID = 0, VoucherHeadId = 0, Approved = false;
        let rights = { canSave: false, canUpdate: false, canPrint: false };
        let lookups = { suppliers: [], accounts: [], otherItems: [] };
        let dtGrid = [], dtGL = [], dtExp = [], dtFreight = [], dtBags = [];
        let popRows = [], popChecked = new Set(), popSuppliers = [], dtGRNS = [];
        let historyGen = 0;

        const blankGL = () => ({ AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 });          // AddRowInGLGrid
        const blankExp = () => ({ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' });                                       // AddRowInvExpGrid
        const blankFreight = () => ({ OrderId: POID, InvGrnId: GRNID, Transporter: 0, Percentage: 0, Qty: 0, Rate: 0, Freight: 0, Debit: 0, Remarks: '' });   // AddRowInFreightGrid
        const sum = (rows, k) => rows.reduce((s, r) => s + N(r[k]), 0);

        /* ---------- grdSettings: the main grid ---------- */
        const mainCols = cfg.mainCols;
        const editCols = cfg.editableCols || [];                                                    // Ghalla grd: ItemRate and RateUom are EditType 1 (text box), every other column NoEdit
        const mainText = (r, c) => (c.fmt ? fmt(r[c.k], c.fmt) : PP.esc(str(r[c.k])));
        const mainFoot = cols => (dtGrid.length ? '<tr>' + cols.map(c => '<td>' + (c.sum ? thou(sum(dtGrid, c.k), 2) : '') + '</td>').join('') + '</tr>' : '');
        function renderMain() {
            const t = $('grd'), cols = mainCols.filter(c => !c.hide);
            t.querySelector('thead').innerHTML = '<tr>' + cols.map(c => '<th' + (c.w ? ' style="min-width:' + c.w + 'px"' : '') + '>' + PP.esc(c.cap || c.k) + '</th>').join('') + '</tr>';
            t.querySelector('tbody').innerHTML = dtGrid.map((r, i) => '<tr data-i="' + i + '">' + cols.map(c => editCols.indexOf(c.k) >= 0
                ? '<td class="n pi-edit"><input type="text" class="pi-cell" data-k="' + c.k + '"></td>'
                : '<td' + (c.num ? ' class="n"' : '') + '>' + mainText(r, c) + '</td>').join('') + '</tr>').join('');
            t.querySelector('tfoot').innerHTML = mainFoot(cols);
            fillEdit();
            t.querySelector('tbody').onclick = e => { const tr = e.target.closest('tr'); if (!tr) return; t.querySelectorAll('tr.selected').forEach(x => x.classList.remove('selected')); tr.classList.add('selected'); t._current = dtGrid[Number(tr.dataset.i)]; };
        }
        function fillEdit() {
            if (!editCols.length) return;
            const body = $('grd').querySelector('tbody');
            Array.from(body.rows).forEach(tr => {
                const r = dtGrid[Number(tr.dataset.i)]; if (!r) return;
                tr.querySelectorAll('input[data-k]').forEach(el => { if (el === document.activeElement) return; const c = mainCols.find(x => x.k === el.dataset.k); const v = c.fmt ? fmt(r[c.k], c.fmt) : str(r[c.k]); if (el.value !== v) el.value = v; });
            });
        }
        // in-place refresh for the editable grid (a rebuild would drop the focus of the cell being tabbed out of)
        function syncMain() {
            if (!editCols.length) { renderMain(); return; }
            const t = $('grd'), cols = mainCols.filter(c => !c.hide), body = t.querySelector('tbody');
            if (body.rows.length !== dtGrid.length) { renderMain(); return; }
            Array.from(body.rows).forEach(tr => {
                const r = dtGrid[Number(tr.dataset.i)];
                cols.forEach((c, j) => { if (editCols.indexOf(c.k) < 0) { const h = mainText(r, c); if (tr.cells[j].innerHTML !== h) tr.cells[j].innerHTML = h; } });
            });
            t.querySelector('tfoot').innerHTML = mainFoot(cols);
            fillEdit();
        }
        function mainCellUpdated(i, k) {                                                              // Ghalla grd_CellUpdated (:846): ItemAmount = NetBillWeight / RateUom * ItemRate, then BillAmount()
            if (k !== 'ItemRate' && k !== 'RateUom') return;
            const r = dtGrid[i], uom = N(fmt(r.RateUom, 'int'));                                      // item.Cells["RateUom"].Text
            r.ItemAmount = uom === 0 ? 0 : N(r.NetBillWeight) / uom * N(r.ItemRate);                 // the desktop yields Infinity on a zero RateUom; the web keeps 0 and Insert refuses "RateUom Field Required"
            billAmount();
        }
        function wireMain() {
            if (!editCols.length) return;
            $('grd').querySelector('tbody').addEventListener('change', e => {
                const el = e.target.closest('input[data-k]'); if (!el) return;
                const i = Number(el.closest('tr').dataset.i), k = el.dataset.k;
                dtGrid[i][k] = N(el.value);
                mainCellUpdated(i, k); syncAll();
            });
        }

        /* ---------- editable grids (grdFreight, grdInvExp, grdGLedger, grdEmptyBags) ---------- */
        const acctOpts = () => lookups.accounts, otherOpts = () => (lookups.otherItems || []).map(o => ({ id: g(o, 'Id'), name: g(o, 'OtherItemName') }));
        const specs = {
            grdFreight: { rows: () => dtFreight, blank: blankFreight, buttons: true, cols: [
                { k: 'Transporter', cap: 'Charge To Product', w: 150, t: 'sel', opts: acctOpts }, { k: 'Percentage', cap: '%', w: 50, t: 'num' }, { k: 'Qty', t: 'num' }, { k: 'Rate', t: 'num' },
                { k: 'Freight', cap: 'Credit', w: 80, t: 'num', sum: true, fmt: 'c0' }, { k: 'Remarks', w: 120, t: 'txt' }] },
            grdInvExp: { rows: () => dtExp, blank: blankExp, buttons: true, cols: [
                { k: 'ItemId', cap: 'Other Item', w: 250, t: 'sel', opts: otherOpts }, { k: 'Qty', t: 'num' }, { k: 'Rate', t: 'num' }, { k: 'Amount', t: 'num', sum: true }, { k: 'Remarks', t: 'txt' }] },
            grdGLedger: { rows: () => dtGL, blank: blankGL, buttons: true, cols: [
                { k: 'AccountId', cap: 'Supplier Add/Less', w: 250, t: 'sel', opts: acctOpts }, { k: 'Remarks', t: 'txt' }, { k: 'Percentage', t: 'num' }, { k: 'Qty', t: 'num' }, { k: 'Rate', t: 'num' },
                { k: 'Debit', t: 'num', sum: true }, { k: 'Credit', t: 'num', sum: true }] },
            grdEmptyBags: { rows: () => dtBags, blank: null, buttons: false, cols: [
                { k: 'Type', t: 'ro' }, { k: 'ItemName', w: 180, t: 'ro' }, { k: 'PurchaseQty', t: 'ro', sum: true }, { k: 'Rate', t: 'num' }, { k: 'Amount', t: 'ro', sum: true }, { k: 'Remarks', w: 150, t: 'txt' }] }
        };
        function renderEG(id) {
            const s = specs[id], t = $(id), rows = s.rows();
            t.querySelector('thead').innerHTML = '<tr>' + s.cols.map(c => '<th' + (c.w ? ' style="min-width:' + c.w + 'px"' : '') + '>' + PP.esc(c.cap || c.k) + '</th>').join('') + (s.buttons ? '<th style="width:20px">X</th><th style="width:20px">+</th>' : '') + '</tr>';
            t.querySelector('tbody').innerHTML = rows.map((r, i) => '<tr data-i="' + i + '">' + s.cols.map(c => {
                if (c.t === 'sel') return '<td><select data-k="' + c.k + '">' + '<option value="0"></option>' + c.opts().map(o => '<option value="' + PP.esc(o.id) + '">' + PP.esc(o.name) + '</option>').join('') + '</select></td>';
                if (c.t === 'ro') return '<td' + (c.sum || c.k === 'PurchaseQty' ? ' class="n"' : '') + '><input type="text" data-k="' + c.k + '" readonly tabindex="-1"' + (c.k === 'Type' || c.k === 'ItemName' ? ' class="pi-left"' : '') + '></td>';
                return '<td><input type="text" data-k="' + c.k + '"' + (c.t === 'txt' ? ' class="pi-left"' : '') + '></td>';
            }).join('') + (s.buttons ? '<td class="btn"><button type="button" data-act="Delete">X</button></td><td class="btn"><button type="button" data-act="Add">+</button></td>' : '') + '</tr>').join('');
            syncEG(id);
        }
        function syncEG(id) {
            const s = specs[id], t = $(id), rows = s.rows();
            t.querySelectorAll('tbody tr').forEach(tr => {
                const r = rows[Number(tr.dataset.i)]; if (!r) return;
                tr.querySelectorAll('[data-k]').forEach(el => { if (el === document.activeElement) return; const v = r[el.dataset.k]; const nv = v == null ? '' : String(v); if (el.value !== nv) el.value = nv; });
            });
            const foot = s.cols.some(c => c.sum) && rows.length ? '<tr>' + s.cols.map(c => '<td>' + (c.sum ? thou(sum(rows, c.k), 2) : '') + '</td>').join('') + (s.buttons ? '<td></td><td></td>' : '') + '</tr>' : '';
            t.querySelector('tfoot').innerHTML = foot;
        }
        const renderAll = () => { renderMain(); Object.keys(specs).forEach(renderEG); };
        const syncAll = () => { syncMain(); Object.keys(specs).forEach(syncEG); };

        /* ---------- calculations (BillAmount :1003, TotalCommissionAmount :1222, proportions :1541..) ---------- */
        function commissionUomValue() { return N(PP.selText('cmbcommuom')); }
        function totalCommissionAmount() {
            try {
                const rate = $('txtcommrate').value.trim(), type = PP.selText('cmbcommtype');
                let amt = N($('txtcommamount').value);
                if (rate !== '') { if (type === 'Flat') amt = f0(N(rate)); } else amt = 0;
                if (rate !== '' && type === 'Percent') amt = bankers(sum(dtGrid, 'ItemAmount') * N(rate) / 100);
                if (rate !== '' && type === 'Comm Weight') { const uom = commissionUomValue(); if (uom === 0) throw Error('Attempted to divide by zero.'); amt = bankers(sum(dtGrid, 'NetBillWeight') / uom * N(rate)); }
                $('txtcommamount').value = thou(amt, 0);
            } catch (e) { show(e.message); }
            commissionProportion();
        }
        function billProportion() { dtGrid.forEach(r => { r.ItemNetAmount = N(r.ItemAmount) + N(r.Expense) + N(r.Freights) + N(r.Commission); }); }
        function billAmount() {
            let jd = 0, jc = 0, td = 0, tc = 0, bags = 0;
            const supplier = N($('cmbsuppliername').value), glId = $('txtSupplierGLId').value;
            dtGL.forEach(r => { if (supplier > 0 && N(r.AccountId) > 0) { jd += N(r.Debit); jc += N(r.Credit); } });
            dtFreight.forEach(r => { if (N(r.Transporter) > 0 && glId === String(r.Transporter)) { td += N(r.Debit); tc += N(r.Freight); } });
            dtBags.forEach(r => { if (N(r.Amount) > 0 && N(r.TypeId) === 1) bags += N(r.Amount); });
            let bill = sum(dtGrid, 'ItemAmount') + sum(dtExp, 'Amount') + bags;
            bill = bill + jd - jc;
            bill = bill + tc - td;
            if (supplier === N($('cmbcommagent').value)) { if ($('txtcommrate').value !== '') bill += N($('txtcommamount').value.trim()); else $('txtcommamount').value = '0'; }
            $('txtBillAmount').value = thou(bankers(bill), 0);
            billProportion();
        }
        function expProportion() {
            const total = sum(dtExp, 'Amount'), qty = sum(dtGrid, 'ItemQty');
            dtGrid.forEach(r => { r.Expense = total > 0 ? total / qty * N(r.ItemQty) : 0; });
            billProportion();
        }
        function freightProportion() {
            const net = sum(dtGrid, 'NetBillWeight'), credit = sum(dtFreight, 'Freight'), debit = sum(dtFreight, 'Debit');
            dtGrid.forEach(r => { r.Freights = debit < credit ? bankers(credit - debit) / net * N(r.NetBillWeight) : 0; });
            billProportion();
        }
        function ledgerProportion() { /* LedgerProportion (:1632): empty body on the desktop */ }
        function commissionProportion() {
            const total = N($('txtcommamount').value), pct = N($('txtcommrate').value), net = sum(dtGrid, 'NetBillWeight'), type = PP.selText('cmbcommtype');
            dtGrid.forEach(r => {
                if (total > 0) r.Commission = type === 'Percent' ? N(r.ItemAmount) * pct / 100 : total / net * N(r.NetBillWeight);
                else r.Commission = 0;
            });
            billProportion();
        }
        const recalcAll = () => { totalCommissionAmount(); freightProportion(); expProportion(); ledgerProportion(); commissionProportion(); billProportion(); billAmount(); };

        /* ---------- editable-grid cell events ---------- */
        function freightCell(i, k) {                                                                  // grdFreight_CellUpdated (:671)
            const r = dtFreight[i];
            if ((k === 'Qty' || k === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') { r.Freight = N(r.Qty) * N(r.Rate); r.Percentage = 0; }
            if (k === 'Percentage' && String(r.Percentage) !== '') {
                const pct = sum(dtGrid, 'ItemAmount') / 100 * N(r.Percentage);
                if (pct > 0) { r.Freight = bankers(pct); r.Qty = 0; r.Rate = 0; }
            }
            billAmount(); freightProportion();
        }
        function expCell(i, k) {                                                                      // grdInvExp_CellUpdated (:699)
            const r = dtExp[i];
            if ((k === 'Qty' || k === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') r.Amount = N(r.Qty) * N(r.Rate);
            billAmount(); expProportion();
        }
        function glCell(i, k) {                                                                       // grdGLedger_CellUpdated (:719)
            const r = dtGL[i];
            if ((k === 'Qty' || k === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') { r.Credit = N(r.Qty) * N(r.Rate); r.Debit = 0; r.Percentage = 0; }
            if (k === 'Percentage' && String(r.Percentage) !== '') {
                const pct = sum(dtGrid, 'ItemAmount') / 100 * N(r.Percentage);
                if (pct > 0) { r.Credit = bankers(pct); r.Debit = 0; } else { r.Debit = Math.abs(bankers(pct)); r.Credit = 0; }
                r.Qty = 0; r.Rate = 0;
            }
            if (k === 'Credit' && N(r.Debit) > 0) { r.Credit = 0; show('Debit Side is aleady added'); }
            if (k === 'Debit' && N(r.Credit) > 0) { r.Debit = 0; show('Credit Side is aleady added'); }
            if (k === 'AccountId' && N($('txtSupplierGLId').value) === N(r.AccountId)) { show('Supplier Account Not select'); r.AccountId = 0; }
            else { billAmount(); ledgerProportion(); }
        }
        function bagCell(i, k) {                                                                      // grdEmptyBags_CellUpdated (:962)
            const r = dtBags[i];
            if (k === 'Rate' && String(r.PurchaseQty) !== '' && String(r.Rate) !== '') r.Amount = N(r.PurchaseQty) * N(r.Rate);
            billAmount();
        }
        const cellHandlers = { grdFreight: freightCell, grdInvExp: expCell, grdGLedger: glCell, grdEmptyBags: bagCell };
        const rowsOf = { grdFreight: () => dtFreight, grdInvExp: () => dtExp, grdGLedger: () => dtGL, grdEmptyBags: () => dtBags };
        function wireEditable() {
            Object.keys(specs).forEach(id => {
                const tb = $(id).querySelector('tbody');
                tb.addEventListener('change', e => {
                    const el = e.target.closest('[data-k]'); if (!el || el.readOnly) return;
                    const i = Number(el.closest('tr').dataset.i), k = el.dataset.k, col = specs[id].cols.find(c => c.k === k), rows = rowsOf[id]();
                    rows[i][k] = (col.t === 'num') ? N(el.value) : (col.t === 'sel') ? el.value : el.value;
                    cellHandlers[id](i, k); syncAll();
                });
                tb.addEventListener('click', e => {                                                   // ColumnButtonClick (Delete / Add)
                    const b = e.target.closest('[data-act]'); if (!b) return;
                    const i = Number(b.closest('tr').dataset.i), s = specs[id], rows = rowsOf[id]();
                    if (b.dataset.act === 'Delete') { rows.splice(i, 1); if (rows.length === 0) rows.push(s.blank()); }
                    else rows.push(s.blank());
                    renderEG(id);
                    if (id === 'grdGLedger') billAmount();
                    if (id === 'grdInvExp') { billAmount(); expProportion(); }
                    if (id === 'grdFreight') { billAmount(); freightProportion(); }
                    syncAll();
                });
            });
        }

        /* ---------- form load / reset ---------- */
        function bindSuppliers() {                                                                    // SupplierNameFilll (:1090)
            const list = lookups.suppliers || [];
            PP.bind('cmbsuppliername', list, 'Id', 'CompanyName');
            PP.bind('cmbcommagent', list, 'Id', 'CompanyName');
        }
        function bindCommissionLists() {                                                              // CommissionTypeFill (:1174), CommissionUOMFill (:1149)
            PP.bind('cmbcommtype', [{ id: 1, n: 'Flat' }, { id: 2, n: 'Percent' }, { id: 3, n: 'Comm Weight' }], 'id', 'n');
            PP.bind('cmbcommuom', [1, 5, 10, 25, 40, 50, 60, 65, 80, 100].map((u, i) => ({ id: i + 1, n: String(u) })), 'id', 'n');
        }
        function gridsBlank() {
            dtGrid = []; dtGL = [blankGL()]; dtExp = [blankExp()]; dtFreight = [blankFreight()]; dtBags = [];
        }
        async function load() {                                                                       // frmPurchaseInvoiceForGate_Load (:260)
            const d = await PP.fetch(API + '/init');
            rights = d; lookups = Object.assign(lookups, d);
            $('btnSave').disabled = !d.canSave; $('btnPrint').disabled = !d.canPrint; $('btnUpdate').disabled = !d.canUpdate;
            bindSuppliers(); bindCommissionLists();
            if (cfg.onLoad) await cfg.onLoad(d, api);
            gridsBlank(); renderAll();
            $('txtdocno').value = d.docNo > 0 ? d.docNo : '0';
            $('DocDate').value = PP.today(); duedaysCalculates();
            $('btnSave').style.display = ''; $('btnUpdate').style.display = 'none';
            $('ChkBok').checked = true; $('ChkBoxParty').checked = true;
            $('txtBillAmount').value = '0';
            $('DocDate').focus();
        }
        const api = { API, g, str, show, lookups: () => lookups, bankers };
        async function documentNo() { const d = await PP.fetch(API + '/doc-no'); $('txtdocno').value = d.docNo; }
        async function reset() {                                                                      // Reset (:1789)
            Id = 0; POID = 0; VoucherHeadId = 0; Approved = false;
            PP.setSel('cmbcommagent', ''); PP.setSel('cmbcommtype', ''); $('txtcommrate').value = ''; $('txtcommamount').value = ''; PP.setSel('cmbcommuom', '');
            $('txtcommremarks').value = ''; $('txtbillno').value = ''; $('txtRemarks').value = ''; $('txtDueDays').value = ''; if ($('txtOrderNo')) $('txtOrderNo').value = '';
            $('DueDate').value = PP.today();
            gridsBlank(); renderAll();
            await documentNo();
            $('btnSave').style.display = ''; $('btnUpdate').style.display = 'none';
            dtGRNS = [];
            $('cmbsuppliername').disabled = false; PP.setSel('cmbsuppliername', '');
            $('txtBillAmount').value = '0';
            if (cfg.onReset) cfg.onReset();
            focusCombo('cmbsuppliername');
        }
        function focusCombo(id) { const w = $(id).parentElement && $(id).parentElement.querySelector('.dtcombo-input'); (w || $(id)).focus(); }
        function duedaysCalculates() {                                                                // DuedaysCalculates (:2400)
            const days = $('txtDueDays').value.trim(), base = $('DocDate').value;
            if (days !== '' && base) { const d = new Date(base + 'T00:00:00'); d.setDate(d.getDate() + N(days)); $('DueDate').value = PP.localStamp(d).slice(0, 10); }
            else $('DueDate').value = PP.today();
        }
        function supplierChanged() {                                                                  // cmbsuppliername_ValueChanged (:2382)
            const id = N($('cmbsuppliername').value);
            if ((lookups.suppliers || []).length && id > 0) { const r = lookups.suppliers.find(x => N(g(x, 'Id')) === id); $('txtSupplierGLId').value = r ? str(g(r, 'GlAccountId') != null ? g(r, 'GlAccountId') : Object.values(r)[2]) : ''; }   // Ghalla: SelectedRow.Cells[2]
        }
        function setSupplier(id) { PP.setSel('cmbsuppliername', id); supplierChanged(); }

        /* ---------- Load GRN popup (frmLoadGRNForPaddyPurchase) ---------- */
        async function openPopup() {                                                                  // toolStripButton3_Click_1 (:2455)
            if (!(btnVisible('btnSave') && !$('btnSave').disabled)) { await reset(); return; }
            if (cfg.ghalla) {                                                                         // frmLoadGRN_Load (Ghalla): LoadingContractorBind, GhallaMandiBind, PendingGrnLoad; the date pickers keep their designer value (today)
                const pl = await PP.fetch(API + '/popup/lookups');
                PP.bind('popSupplier', pl.suppliers || [], 'Id', 'CompanyName'); PP.bind('popGhalla', pl.ghallaMandi || [], 'Id', 'GhallaMandi');
                $('popFromDate').value = PP.today(); $('popToDate').value = PP.today();
                dtGRNS = []; $('grnModal').style.display = 'flex';
                await pendingGrnLoad();
                return;
            }
            const [pl, ys] = await Promise.all([PP.fetch(API + '/popup/lookups'), PP.fetch(API + '/popup/year-start')]);   // frmLoadGRN_Load: SupplierNameFilll, FromDate = Start_Period
            popSuppliers = pl.suppliers || [];
            PP.bind('popSupplier', popSuppliers, 'Id', 'CompanyName'); PP.bind('popOrder', [], 'Id', 'DocNo');
            $('popFromDate').value = ys.startDate ? dateInput(ys.startDate) : PP.today(); $('popToDate').value = PP.today();
            dtGRNS = [];
            $('grnModal').style.display = 'flex';
            await pendingGrnLoad();
        }
        async function pendingGrnLoad() {                                                             // PendingGrnLoad
            if (cfg.ghalla) popRows = await PP.fetch(API + '/popup/pending?supplierId=' + N($('popSupplier').value) + '&ghallaMandiId=' + N($('popGhalla').value) + '&from=' + $('popFromDate').value + '&to=' + $('popToDate').value);
            else popRows = await PP.fetch(API + '/popup/pending?orderId=' + N($('popOrder').value) + '&from=' + $('popFromDate').value + '&to=' + $('popToDate').value);
            popChecked = new Set(); renderPop();
        }
        function renderPop() {
            const t = $('grdPop'), rows = popRows;
            if (!rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; return; }
            if (cfg.ghalla) {                                                                         // grdSettingsForGrnPending: every column of the proc result except Id and GhallaMandiId, plus the Select selector column
                const keys = Object.keys(rows[0]).filter(k => k.toLowerCase() !== 'id' && k.toLowerCase() !== 'ghallamandiid');
                t.querySelector('thead').innerHTML = '<tr><th><input type="checkbox" id="popAll"></th>' + keys.map(k => '<th>' + PP.esc(k) + '</th>').join('') + '</tr>';
                t.querySelector('tbody').innerHTML = rows.map((r, i) => '<tr data-i="' + i + '"><td class="chk"><input type="checkbox" class="popchk"' + (popChecked.has(i) ? ' checked' : '') + '></td>' +
                    keys.map(k => '<td>' + PP.esc(/date$/i.test(k) && r[k] ? PP.fmtDate(r[k]) : str(r[k])) + '</td>').join('') + '</tr>').join('');
                const all = $('popAll'); all.checked = popChecked.size === rows.length;
                all.onchange = () => { popChecked = all.checked ? new Set(rows.map((_, i) => i)) : new Set(); t.querySelectorAll('.popchk').forEach((c, i) => { c.checked = popChecked.has(i); }); };
                t.querySelector('tbody').onchange = e => { const c = e.target.closest('.popchk'); if (!c) return; const i = Number(c.closest('tr').dataset.i); if (c.checked) popChecked.add(i); else popChecked.delete(i); all.checked = popChecked.size === rows.length; };
                return;
            }
            const head = ['Select', 'DocDate', 'DocNo', 'SupplierCustomer', 'GpNO', 'BiltyNo', 'VehicleNo', 'ItemQty', 'ItemName'];
            t.querySelector('thead').innerHTML = '<tr><th><input type="checkbox" id="popAll"></th>' + head.slice(1).map(h => '<th>' + h + '</th>').join('') + '</tr>';
            t.querySelector('tbody').innerHTML = rows.map((r, i) => '<tr data-i="' + i + '"><td class="chk"><input type="checkbox" class="popchk"' + (popChecked.has(i) ? ' checked' : '') + '></td>' +
                '<td>' + PP.esc(PP.fmtDate(g(r, 'DocDate')).replace(/-(\d{4})/, (m, y) => '.' + y.slice(2))) + '</td><td>' + PP.esc(str(g(r, 'DocNo'))) + '</td><td style="min-width:250px">' + PP.esc(str(g(r, 'SupplierCustomer'))) + '</td>' +
                '<td>' + PP.esc(str(g(r, 'GpNO'))) + '</td><td>' + PP.esc(str(g(r, 'BiltyNo'))) + '</td><td>' + PP.esc(str(g(r, 'VehicleNo'))) + '</td><td>' + PP.esc(str(g(r, 'ItemQty'))) + '</td><td style="min-width:250px">' + PP.esc(str(g(r, 'ItemName'))) + '</td></tr>').join('');
            const all = $('popAll'); all.checked = popChecked.size === rows.length;
            all.onchange = () => { popChecked = all.checked ? new Set(rows.map((_, i) => i)) : new Set(); t.querySelectorAll('.popchk').forEach((c, i) => { c.checked = popChecked.has(i); }); };
            t.querySelector('tbody').onchange = e => { const c = e.target.closest('.popchk'); if (!c) return; const i = Number(c.closest('tr').dataset.i); if (c.checked) popChecked.add(i); else popChecked.delete(i); all.checked = popChecked.size === rows.length; };
        }
        async function popSupplierLeave() {                                                           // cmbsuppliername_Leave -> GetOrderBySupplierId
            const sid = N($('popSupplier').value);
            if (sid <= 0) { PP.bind('popOrder', [], 'Id', 'DocNo'); return; }
            const rows = await PP.fetch(API + '/popup/orders?supplierId=' + sid);
            PP.bind('popOrder', rows, 'Id', 'DocNo');
        }
        async function popReset() {                                                                   // btnReset_Click
            if (cfg.ghalla) { $('popFromDate').value = PP.today(); $('popToDate').value = PP.today(); PP.setSel('popGhalla', ''); PP.setSel('popSupplier', ''); return; }   // Ghalla: dates = today, both combos cleared
            PP.setSel('popSupplier', ''); PP.bind('popOrder', [], 'Id', 'DocNo');
            const ys = await PP.fetch(API + '/popup/year-start'); $('popFromDate').value = ys.startDate ? dateInput(ys.startDate) : PP.today();
        }
        async function btnLoadOnInvoice() {                                                           // btnLoadOnInvoice_Click_1
            if (popChecked.size === 0) { show('Chek the row first'); return; }
            if (cfg.ghalla) {                                                                         // frmLoadGrnForGhallaMandi.btnLoadOnInvoice_Click_1: GrnType 2, rows without a Ghalla_Mandi are skipped
                let gm = 0, lod = 0; dtGRNS = [];
                for (const i of Array.from(popChecked).sort((a, b) => a - b)) {
                    const r = popRows[i], gId = N(g(r, 'GhallaMandiId')), lc = N(g(r, 'SupplierCustomerId'));
                    if (gId === 0) continue;
                    if (gm === 0) gm = gId;
                    if (gm !== gId) { dtGRNS = []; show('Sorry the Ghalla_Mandi should be not of same'); return; }
                    if (lod === 0) lod = lc;
                    if (lod !== lc) { dtGRNS = []; show('Sorry the LoadingContractor should be not of same'); return; }
                    dtGRNS.push([String(g(r, 'Id')), '2']);
                    gm = gId; lod = lc;
                }
                $('grnModal').style.display = 'none';
                await loadInGridDetail();
                return;
            }
            let sID = 0, refId = 0, sDocId = 1, ok = true; dtGRNS = [];
            const orderId = N($('popOrder').value), hasOrder = !!$('popOrder').value;
            for (const i of Array.from(popChecked).sort((a, b) => a - b)) {
                const r = popRows[i], supplierCustomer = N(g(r, 'SupplierCustomerId')), refDoc = N(g(r, 'RefDocumentTypeId')), docId = N(g(r, 'Id'));
                if (supplierCustomer !== 0) {
                    if (sID === 0) sID = supplierCustomer;
                    if (refId === 0) refId = refDoc;
                    if (sDocId === 0) sDocId = docId;
                    // (:DocumentTypeId == 166 && DocId != sDocId) 'only single grn select' is dead code on the desktop: the popup's DocumentTypeId is 165/167
                    if (sID !== supplierCustomer) { show('Sorry the Grn Not should be of same Supplier'); ok = false; break; }
                    if (refId !== refDoc) { show('Sorry the Grn should be of same PurchaseAgainst type'); ok = false; break; }
                    dtGRNS.push(hasOrder ? [String(docId), '3', String(supplierCustomer), orderId] : [String(docId), '3', '0', 0]);
                    sID = supplierCustomer; refId = refDoc; sDocId = docId;
                }
            }
            if (!ok) dtGRNS = [];
            $('grnModal').style.display = 'none';
            await loadInGridDetail();
        }

        /* ---------- LoadInGridDetail (:1476) ---------- */
        async function loadDataDetailGridAgainstGP(grnId, orderId) {                                  // LoadDataDetailGridAgainstGP (:1332)
            const rows = await PP.fetch(API + '/load-grn?grnId=' + grnId + '&orderId=' + (orderId || 0));
            if (!rows.length) return;
            const r0 = rows[0];
            if (orderId > 0) POID = orderId;
            setSupplier(g(r0, 'SupplierCustomerId'));
            PP.setSel('cmbcommagent', g(r0, 'BrokerAgentSupCustId'));
            PP.setSelText('cmbcommtype', ''); PP.setSelText('cmbcommtype', str(g(r0, 'CommissionType')));
            $('txtcommrate').value = str(g(r0, 'CommRate'));
            PP.setSelText('cmbcommuom', ''); PP.setSelText('cmbcommuom', str(g(r0, 'UomScheduleIdCmRate')));
            $('txtcommamount').value = str(g(r0, 'CommAmount'));
            $('txtcommremarks').value = str(g(r0, 'CommissionRemarks'));
            $('txtRemarks').value = str(g(r0, 'RemarksHeader'));
            $('txtDueDays').value = str(g(r0, 'OrderDueDays')); duedaysCalculates();
            GRNID = N(g(r0, 'InvGrnId'));
            $('txtDeliveryTerm').value = str(g(r0, 'DeliveryTerm'));
            if (orderId > 0) { $('cmbsuppliername').disabled = true; $('txtOrderNo').value = str(g(r0, 'OrderNo')); }
            else { $('cmbsuppliername').disabled = false; $('txtOrderNo').value = '0'; }
            rows.forEach(r => dtGrid.push(cfg.gridRowFromGrn(r, g)));
        }
        async function loadGhalla(ids) {                                                              // LoadDataDetailGridAgainstGP (Ghalla :1372)
            const rows = await PP.fetch(API + '/load-grn?ids=' + encodeURIComponent(ids));
            if (!rows.length) return;
            const r0 = rows[0];
            setSupplier(g(r0, 'SupplierCustomerId'));
            PP.setSel('cmbGhallaMandi', g(r0, 'GhallaMandiId'));
            PP.setSel('cmbcommagent', g(r0, 'SupplierCustomerId'));
            $('txtRemarks').value = str(g(r0, 'RemarksHeader'));
            $('txtDeliveryTerm').value = str(g(r0, 'DeliveryTerm'));
            rows.forEach(r => dtGrid.push(cfg.gridRowFromGrn(r, g)));
        }
        async function loadInGridDetailGhalla() {                                                     // LoadInGridDetail (Ghalla :1463): one call with GrnIds = ",id,id"; no LoadExpData, no order
            dtGrid = [];
            const ids = (dtGRNS || []).map(r => r[0]).join(',');
            dtFreight = []; dtGL = []; dtExp = []; dtBags = [];
            if (ids !== '') {
                await loadGhalla(ids);
                const ex = await PP.fetch(API + '/load-extras?ids=' + encodeURIComponent(ids));
                loadFreightData(ex.freight || []);
                loadEmptyBagsData(ex.emptyBags || []);
            }
            if (dtFreight.length === 0) dtFreight.push(blankFreight());
            if (dtGL.length === 0) dtGL.push(blankGL());
            if (dtExp.length === 0) dtExp.push(blankExp());
            totalCommissionAmount();
            renderAll();
            freightProportion(); expProportion(); ledgerProportion(); commissionProportion(); billProportion(); billAmount();
            syncAll();
        }
        async function loadInGridDetail() {
            if (cfg.ghalla) { await loadInGridDetailGhalla(); return; }
            dtGrid = [];
            let grnIds = '', orderId = 0;
            for (const row of dtGRNS) {
                grnIds = String(row[0]); orderId = N(row[3]);
                await loadDataDetailGridAgainstGP(N(grnIds), orderId > 0 ? orderId : 0);
            }
            dtFreight = []; dtGL = [];
            if (grnIds !== '') {
                const ex = await PP.fetch(API + '/load-extras?ids=' + encodeURIComponent(grnIds) + '&orderId=' + orderId);
                loadFreightData(ex.freight || []);
                dtExp = []; loadExpData(ex.expenses || []);
                dtBags = []; loadEmptyBagsData(ex.emptyBags || []);
            } else { dtExp = []; dtBags = []; }
            if (dtFreight.length === 0) dtFreight.push(blankFreight());
            if (dtGL.length === 0) dtGL.push(blankGL());
            if (dtExp.length === 0) dtExp.push(blankExp());
            totalCommissionAmount();
            renderAll();
            freightProportion(); expProportion(); ledgerProportion(); commissionProportion(); billProportion(); billAmount();
            syncAll();
        }
        function loadFreightData(items) {                                                             // LoadFreightData (:1383)
            const term = $('txtDeliveryTerm').value.trim();
            items.forEach(it => {
                if (N(g(it, 'CarriageAmount')) > 0) {
                    let rem = '';
                    if (N(g(it, 'GpNo')) > 0) rem += 'GpNo: ' + str(g(it, 'GpNo'));
                    if (str(g(it, 'VehicleNo')) !== '') rem += '   VehicleNo:  ' + str(g(it, 'VehicleNo'));
                    if (str(g(it, 'BiltyNo')) !== '') rem += '   BiltyNo: ' + str(g(it, 'BiltyNo'));
                    if (term === 'Load' || term === 'Load & PartyWeight' || term === 'Load & FactoryWeight')
                        dtFreight.push({ OrderId: POID, InvGrnId: g(it, 'MainId'), Transporter: g(it, 'Transporter'), Percentage: 0, Qty: 0, Rate: 0, Freight: N(g(it, 'CarriageAmount')), Debit: 0, Remarks: rem });
                    else if (cfg.freightToGL !== false) dtGL.push({ AccountId: g(it, 'Transporter'), Remarks: rem, Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: N(g(it, 'CarriageAmount')) });
                }
            });
        }
        function loadEmptyBagsData(items) {                                                           // LoadEmptyBagsData (:1430)
            items.forEach(it => {
                if (Math.trunc(N(g(it, 'PurchaseQty'))) > 0)
                    dtBags.push({ OrderId: g(it, 'PurchaseOrderId'), TypeId: g(it, 'TypeId'), Type: g(it, 'EmptyBagsType'), InvGrnId: g(it, 'InvGrnId'), ItemId: g(it, 'ItemId'), ItemName: g(it, 'ItemName'),
                        PurchaseQty: N(g(it, 'PurchaseQty')), Rate: g(it, 'Rate'), Amount: N(g(it, 'PurchaseQty')) * N(g(it, 'Rate')), Remarks: str(g(it, 'Remarks')) });
            });
        }
        function loadExpData(items) {                                                                 // LoadExpData (:1453)
            items.forEach(it => { if (Math.trunc(N(g(it, 'BagPrice'))) > 0) dtExp.push({ ItemId: 0, Qty: N(g(it, 'ItemQty')), Rate: g(it, 'BagPrice'), Amount: N(g(it, 'ItemQty')) * N(g(it, 'BagPrice')), Remarks: 0 }); });
        }

        /* ---------- Insert (:1973) ---------- */
        function validateForm() {                                                                     // FormValidation (:1067)
            if (!$('cmbsuppliername').value) { show('Supplier Name is Required'); focusCombo('cmbsuppliername'); return false; }
            if (cfg.validateExtra && !cfg.validateExtra()) return false;
            const doc = $('txtdocno').value.trim(); if (doc === '' || doc === '0') { show('DocNo is Required'); $('txtdocno').focus(); return false; }
            const dd = $('txtDueDays').value.trim(); if (dd === '' || dd === '0') { show('DueDays Field is Required'); $('txtDueDays').focus(); return false; }
            return true;
        }
        async function insert() {
            if (!validateForm()) return;
            if (!PP.confirm(Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            totalCommissionAmount(); billAmount(); expProportion(); freightProportion(); commissionProportion(); syncAll();
            for (const r of dtFreight) {
                if (!(N(r.Freight) > 0)) continue;
                if (N(r.Transporter) === 0) throw Error('Please Select an Account Against Freight First');
                if (str(r.Remarks) === '') throw Error('Grid Charge to Product Remarks required Please Check');
            }
            for (const r of dtGL) if ((N(r.Credit) > 0 || Math.trunc(N(r.Debit)) > 0) && N(r.AccountId) === 0) throw Error('Please Select an Account Against JL First');
            for (const r of dtExp) if (N(r.Amount) > 0 && N(r.ItemId) === 0) throw Error('Please Select an Item Against Expense First');
            for (const r of dtBags) if (N(r.ItemId) > 0 && N(r.TypeId) === 1 && N(r.Amount) === 0) throw Error('Packing Material Amount Required');
            if (N($('txtcommamount').value) > 0 && N($('cmbcommagent').value) === 0) throw Error('Please Select Commission Agent Account First');
            if (!dtGrid.length) throw Error('Grid Record Not found');
            const body = Object.assign({
                id: Id, docNo: $('txtdocno').value.trim(), docDate: $('DocDate').value, supplierId: N($('cmbsuppliername').value), manualBillNo: $('txtbillno').value.trim(), remarksHeader: $('txtRemarks').value.trim(),
                dueDays: $('txtDueDays').value.trim(), commAgentId: N($('cmbcommagent').value), commType: PP.selText('cmbcommtype'), commRate: $('txtcommrate').value.trim(), commUom: PP.selText('cmbcommuom'),
                commAmount: N($('txtcommamount').value), commRemarks: $('txtcommremarks').value.trim(), billAmount: N($('txtBillAmount').value),
                details: dtGrid, freight: dtFreight, expenses: dtExp, journal: dtGL, emptyBags: dtBags
            }, cfg.extraBody ? cfg.extraBody() : {});
            const res = await PP.post(API + '/save', body);
            show(res.message);
            const party = $('ChkBoxParty').checked, voucher = $('ChkBok').checked;
            await reset();
            if (party) print220A(res.id, res.supplierId);
            if (voucher) print103(res.voucherHeadId);
        }
        async function btnSaveClick() { Id = 0; await insert(); }
        async function btnUpdateClick() {                                                             // btnUpdate_Click (:1952)
            if (Approved) { show('Record Not Update beacause Record has approved'); return; }
            if (Id === 0) { show('Record Not Update beacause Record Id not found'); return; }
            await insert();
        }

        /* ---------- ReadById (:1853) ---------- */
        async function readById(ID) {
            Id = ID;
            const d = await PP.fetch(API + '/' + ID), h = d.header;
            $('txtdocno').value = str(g(h, 'DocNo')); $('DocDate').value = dateInput(g(h, 'DocDate'));
            switchView('Form');
            setSupplier(g(h, 'SupplierCustomerId')); $('cmbsuppliername').disabled = true;
            $('txtbillno').value = str(g(h, 'ManualBillNo'));
            PP.setSel('cmbcommagent', g(h, 'CommissionAgentId'));
            PP.setSelText('cmbcommtype', str(g(h, 'CommissionType')));
            $('txtcommrate').value = str(g(h, 'CommRate'));
            PP.setSelText('cmbcommuom', str(g(h, 'UomScheduleIdCmRate')));
            $('txtcommamount').value = str(g(h, 'CommAmount'));
            $('txtcommremarks').value = str(g(h, 'CommissionRemarks')); $('txtRemarks').value = str(g(h, 'RemarksHeader'));
            $('txtBillAmount').value = str(g(h, 'BillAmount'));
            $('DueDate').value = dateInput(g(h, 'DueDate')); $('txtDueDays').value = str(g(h, 'DueDays'));
            Approved = d.approved; VoucherHeadId = d.voucherHeadId || 0;
            if (cfg.onRead) cfg.onRead(h);
            dtGrid = d.details || []; dtExp = d.expenses || []; dtGL = d.journal || []; dtFreight = d.freight || []; dtBags = d.emptyBags || [];
            if (dtExp.length === 0) dtExp.push(blankExp());
            if (dtGL.length === 0) dtGL.push(blankGL());
            if (dtFreight.length === 0) dtFreight.push(blankFreight());
            renderAll();
            freightProportion(); expProportion(); ledgerProportion(); commissionProportion(); billProportion();
            syncAll();
            $('btnSave').style.display = 'none'; $('btnUpdate').style.display = '';
        }

        /* ---------- history ---------- */
        async function getAll(n) {                                                                    // GetAll (:1699)
            const rows = await PP.fetch(API + '/history?noOfRecords=' + (n || 0));
            PP.renderGrid('grdHistory', rows, { hide: ['Id', 'VoucherHeadId', 'DocumentTypeId'], dateCols: ['DueDate', 'EntryDate', 'ModifyDate'], link: ['InvoiceNo', 'NoOfAttachments'], linkAlways: ['InvoiceNo', 'NoOfAttachments'],
                numeric: ['BillAmount'], dec: { BillAmount: 0 }, totals: { BillAmount: 0 },
                caption: {},
                post: [
                    { key: 'Detail', label: 'Detail', onclick: r => getDetailGrdByHeadId(r.Id) },
                    { key: 'Print', label: 'Print', onclick: r => print220A(r.Id, 0) },
                    { key: 'Voucher', label: 'Voucher', onclick: r => print103(r.VoucherHeadId) },
                    { key: 'Edit', label: 'Edit', onclick: async r => { await reset(); await readById(r.Id); } }],
                onDbl: r => readById(r.Id).catch(PP.error),
                onLink: (k, r) => { if (k === 'NoOfAttachments') showAttachments(r.Id); else show('Purchase Invoice Reverse Activity has no web screen yet.'); } });
            if (!rows.length) PP.renderGrid('grdDetail', [], {});
        }
        async function getDetailGrdByHeadId(id) {                                                     // GetDetailGrdByHeadId (:960)
            const rows = await PP.fetch(API + '/history-detail/' + id);
            if (!rows.length) { PP.renderGrid('grdDetail', [], {}); return; }
            const z = { ItemAmount: 0, ItemQty: 0, NetBillWeight: 0, CommissionAmount: 0, EBTotalWt: 0, ExpenseAmount: 0, FreightAmount: 0, GrossWeight: 0 };
            PP.renderGrid('grdDetail', rows, { numeric: Object.keys(z).concat(['ItemRate']), dec: Object.assign({ ItemRate: 0 }, z), totals: z });
        }
        async function showAttachments(id) {                                                          // GetNoofAttachmentsByScreenName(Id, Name)
            const rows = await PP.fetch(API + '/' + id + '/attachments');
            $('attBody').innerHTML = rows.length ? rows.map(r => '<div>' + PP.esc(str(g(r, 'Attachment')).split(/[\\/]/).pop()) + '</div>').join('') : 'No attachments';
            $('attModal').style.display = 'flex';
        }

        /* ---------- print ---------- */
        function print103(vh) {                                                                       // AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId)
            if (!vh) { show('No Record Found For Display'); return; }
            if (window.printRpt) window.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: Number(vh), documentTypeId: cfg.voucherDocType }); else show('The print runtime is not loaded on this page.');
        }
        function print220A(id, supplierId) {                                                          // PurchaseInvoicettSlip_220A(RecId)
            if (!id) { show('Record Id Not Found'); return; }
            const d = PP.today();
            if (window.printRpt) window.printRpt('220A-InvRptPurchaseBillSupplierRiceSlip.rpt', { id: Number(id), supplierCustomerId: Number(supplierId || N($('cmbsuppliername').value) || 0), fromDate: d, toDate: d }); else show('The print runtime is not loaded on this page.');
        }

        /* ---------- tabs and keyboard ---------- */
        const btnVisible = id => $(id).style.display !== 'none';
        function isHistory() { return $('viewHistory').style.display !== 'none'; }
        function switchView(mode) {                                                                   // tabControl1_SelectedIndexChanged (:2440): History loads 50 rows
            const h = mode === 'History';
            $('viewForm').style.display = h ? 'none' : 'block'; $('viewHistory').style.display = h ? 'block' : 'none';
            if (h) getAll(50).catch(PP.error);
        }
        function closeForm() { if (window.opener) window.close(); else history.back(); }
        async function btnFrmRefreshClick() {                                                         // btnFrmRefresh_Click (:2399): OtherItemsBind, AccountsFill, SupplierNameFilll
            const d = await PP.fetch(API + '/lookups'); lookups = Object.assign(lookups, d); bindSuppliers(); if (cfg.onRefresh) await cfg.onRefresh(); renderAll();
        }
        function onKey(e) {                                                                           // InvfrmPurchaseInvoice_KeyDown (:2343)
            const k = e.key.toLowerCase(), formTab = !isHistory();
            if (e.ctrlKey && k === 's' && formTab) { e.preventDefault(); if (btnVisible('btnSave') && !$('btnSave').disabled) PP.withButton($('btnSave'), btnSaveClick); }
            if (e.ctrlKey && k === 'n' && formTab) { e.preventDefault(); PP.withButton($('btnNew'), reset); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); switchView(isHistory() ? 'Form' : 'History'); }
            if (e.ctrlKey && k === 'e') { e.preventDefault(); closeForm(); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); if (btnVisible('btnUpdate') && !$('btnUpdate').disabled) PP.withButton($('btnUpdate'), btnUpdateClick); }
            if (e.ctrlKey && k === 'p') { e.preventDefault(); if (btnVisible('btnPrint') && !$('btnPrint').disabled) print103(VoucherHeadId); }
            if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); btnAttachmentClick(); }
            if (e.ctrlKey && k === 'l') { e.preventDefault(); PP.withButton($('toolStripButton3'), openPopup); }
            if (e.ctrlKey && e.key === 'Enter' && isHistory()) { const r = $('grdHistory')._current; if (r) readById(r.Id).catch(PP.error); }
            const focusWrap = id => { const t = $(id); t.tabIndex = -1; t.focus(); };
            if (e.ctrlKey && e.key === 'ArrowLeft' && formTab) focusWrap('wrapFreight');
            if (e.ctrlKey && e.key === 'ArrowUp' && formTab) focusWrap('wrapInvExp');
            if (e.ctrlKey && e.key === 'ArrowDown' && formTab) focusWrap('wrapGL');
            if (e.ctrlKey && e.key === 'ArrowRight' && formTab) focusWrap('wrapEmptyBags');
            if (e.altKey && k === 'r') { e.preventDefault(); PP.withButton($('btnFrmRefresh'), btnFrmRefreshClick); }
        }
        async function btnAttachmentClick() {                                                         // btnAttachment_Click (:2392): AT.Show()
            if (Id > 0) return showAttachments(Id);
            show('Attachments can be viewed for a saved record here. Adding files from the web is not available yet.');
        }

        function start() {
            PP.installGuards(); PP.enterToTab($('viewForm'));
            if (window.PurchaseChrome) {
                PurchaseChrome.footer({ isHistory, toggle: () => switchView(isHistory() ? 'Form' : 'History'), watch: '#viewHistory' });
                ['#wrapFreight', '#wrapInvExp', '#wrapGL', '#wrapEmptyBags', '#wrapGrd', '#wrapHistory', '#wrapDetail', '#wrapPop'].forEach(b => PurchaseChrome.fullscreen(b, 'grid'));
            }
            wireEditable(); wireMain();
            $('btnNew').onclick = () => PP.withButton($('btnNew'), reset);
            $('btnSave').onclick = () => PP.withButton($('btnSave'), btnSaveClick);
            $('btnUpdate').onclick = () => PP.withButton($('btnUpdate'), btnUpdateClick);
            $('btnPrint').onclick = () => { try { print103(VoucherHeadId); } catch (e) { show(e.message); } };
            $('btnSlip220A').onclick = () => print220A(Id, 0);
            $('btnAttachment').onclick = btnAttachmentClick;
            $('toolStripButton3').onclick = () => PP.withButton($('toolStripButton3'), openPopup);
            $('btnFrmRefresh').onclick = () => PP.withButton($('btnFrmRefresh'), btnFrmRefreshClick);
            $('btnLoadAll').onclick = () => PP.withButton($('btnLoadAll'), () => getAll(0));
            $('btnRegister').onclick = () => show('The Paddy Purchase Register form (frmPaddyPurchaseRegister) has no web screen yet.');
            $('attClose').onclick = () => { $('attModal').style.display = 'none'; };
            $('popClose').onclick = () => { dtGRNS = null; $('grnModal').style.display = 'none'; };
            $('btngrnlod').onclick = () => PP.withButton($('btngrnlod'), pendingGrnLoad);
            $('btnLoadOnInvoice').onclick = () => PP.withButton($('btnLoadOnInvoice'), btnLoadOnInvoice);
            $('popReset').onclick = () => PP.withButton($('popReset'), popReset);
            if (!cfg.ghalla) {
                PP.onComboLeave('popSupplier', () => popSupplierLeave().catch(PP.error));
                PP.onComboLeave('popOrder', () => pendingGrnLoad().catch(PP.error));
            }
            $('cmbsuppliername').addEventListener('change', supplierChanged);
            $('DocDate').addEventListener('change', duedaysCalculates);
            $('txtDueDays').addEventListener('input', duedaysCalculates);
            const comm = () => { totalCommissionAmount(); billAmount(); commissionProportion(); syncAll(); };
            ['cmbcommagent', 'cmbcommtype', 'cmbcommuom'].forEach(i => $(i).addEventListener('change', comm));
            $('txtcommrate').addEventListener('input', comm);
            document.addEventListener('keydown', onKey);
            if (cfg.onStart) cfg.onStart({ $, g, str, show, api });
            load().catch(PP.error);
        }
        document.addEventListener('DOMContentLoaded', start);
        return { reset, readById, recalcAll };
    };
    global.PI = PI;
}(window));
