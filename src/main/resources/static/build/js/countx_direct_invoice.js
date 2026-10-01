/* InvfrmPurchasedirectInvoice (screen 117, DocumentTypeId 57) - Purchase Invoice Direct.
 * Layout, grid columns and events follow the desktop form (InitializeComponent :6145-10410); every Designer event is
 * accounted for in the recheck report. Amounts, proportions, validations and saving run on the server
 * (/api/purchase/purchase-direct-invoice, PurchaseDirectInvoiceService / PurchaseDirectInvoiceDesktopRules); the grid
 * CellUpdated rules (:2522, :2711, :2984, :1921) run here as the desktop runs them in the grid. */
const directInvoice = (() => {
    'use strict';
    const base = '/api/purchase/purchase-direct-invoice', route = '/purchase/purchase-invoice-direct', SCREEN_ID = 117, DOC_TYPE = 57;
    const GRIDS_BY_INDEX = ['freight', 'expenses', 'journal', 'emptyBags'];
    const state = {
        id: 0, h: {}, details: [], parts: { freight: [], expenses: [], journal: [], emptyBags: [] },
        lists: {}, rights: {}, config: {}, voucherHeadId: 0, approved: false, edit: -1, firstNew: true,
        current: { grd: 0, freight: 0, expenses: 0, journal: 0, emptyBags: 0 },
        pending: Promise.resolve(), error: null, generation: 0, lineVersion: 0, linePending: Promise.resolve(), lineError: null,
        historyRows: [], historySelected: 0, loaderRows: [], supplierGl: 0
    };
    const el = id => document.getElementById(id);
    const get = (r, k) => r?.[k] ?? r?.[Object.keys(r || {}).find(x => x.toLowerCase() === String(k).toLowerCase())];
    const num = (r, k) => Number(get(r, k)) || 0;
    const esc = v => String(v ?? '').replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    const date = v => String(v || '').slice(0, 10);
    const ymd = d => `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
    const today = () => ymd(new Date());
    const MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    const ddMMMyy = v => { const s = date(v); if (!/^\d{4}-\d{2}-\d{2}$/.test(s)) return ''; return s.slice(8, 10) + '-' + MONTHS[Number(s.slice(5, 7)) - 1] + '-' + s.slice(2, 4); };
    const dtShort = v => { const s = String(v || ''); if (!s) return ''; const d = new Date(s.replace(' ', 'T')); if (isNaN(d)) return s; const h = d.getHours() % 12 || 12; return `${String(d.getDate()).padStart(2, '0')}-${String(d.getMonth() + 1).padStart(2, '0')}-${d.getFullYear()} ${String(h).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')} ${d.getHours() < 12 ? 'AM' : 'PM'}`; };
    const digits = () => Number(state.lists.amountDigits) || 0;
    const fmtW = v => { if (v === null || v === undefined || v === '') return ''; const n = Number(v); return Number.isFinite(n) ? n.toLocaleString('en-US', { maximumFractionDigits: 3 }) : String(v); };
    const fmtR = v => { if (v === null || v === undefined || v === '') return ''; const n = Number(v); return Number.isFinite(n) ? n.toLocaleString('en-US', { maximumFractionDigits: 4 }) : String(v); };
    const fmtA = v => { if (v === null || v === undefined || v === '') return ''; const n = Number(v); return Number.isFinite(n) ? n.toLocaleString('en-US', { minimumFractionDigits: digits(), maximumFractionDigits: digits() }) : String(v); };
    /** Math.Round(x, n, MidpointRounding.AwayFromZero) */
    const afz = (v, d = digits()) => { const p = 10 ** d, n = Number(v) || 0; return Math.sign(n) * Math.round(Math.abs(n) * p + 1e-9) / p; };
    /** Convert.ToInt32(double) / Math.Round(x) - banker's rounding */
    const bankers = v => { const n = Number(v) || 0, f = Math.floor(n), r = n - f; if (Math.abs(r - 0.5) < 1e-12) return f % 2 === 0 ? f : f + 1; return Math.round(n); };
    const parse = v => { const n = Number(String(v ?? '').replace(/,/g, '').trim()); return Number.isFinite(n) ? n : 0; };
    const isNumeric = v => { const s = String(v ?? '').replace(/,/g, '').trim(); return s === '' || Number.isFinite(Number(s)); };

    // ------------------------------------------------------------------ messages / requests
    function message(text = '', error = false, target = 'fxMsg') { const m = el(target); if (!m) return; m.textContent = text; m.classList.toggle('is-error', !!error && !!text); }
    const fail = (e, target) => message(e?.message || String(e), true, target);
    async function request(url, body, method) {
        return PurchaseRequest.track(async () => {
            const r = await fetch(url, { method: method || (body ? 'POST' : 'GET'), credentials: 'same-origin', headers: { Accept: 'application/json', ...(body ? { 'Content-Type': 'application/json' } : {}) }, ...(body ? { body: JSON.stringify(body) } : {}) });
            if (r.redirected || r.status === 401) throw Error('Please sign in to continue');
            const text = await r.text(); let data = null; try { data = text ? JSON.parse(text) : null; } catch (x) { data = { message: text }; }
            if (!r.ok || data?.success === false) throw Error(data?.message || data?.detail || data?.error || 'The request could not be completed');
            return data;
        });
    }
    const api = (path, body, method) => request(base + path, body, method);
    async function run(button, work, target) { try { await PurchaseRequest.run(button, work); } catch (e) { fail(e, target); } finally { rights(); } }
    function enqueue(work) {
        const previous = state.pending, generation = state.generation;
        state.pending = PurchaseRequest.track(async () => {
            await previous;
            try { if (generation !== state.generation) return; await work(generation); if (generation === state.generation) state.error = null; }
            catch (e) { if (generation === state.generation) { state.error = e; fail(e); } }
            finally { rights(); }
        });
        return state.pending;
    }
    async function settled() { let p; do { p = state.pending; await p; } while (p !== state.pending); if (state.error) throw state.error; }
    async function lineSettled() { let p; do { p = state.linePending; await p; } while (p !== state.linePending); if (state.lineError) throw state.lineError; }

    // ------------------------------------------------------------------ combos
    function option(value, label, data) { const o = new Option(String(label ?? ''), String(value ?? '')); for (const [k, v] of Object.entries(data || {})) if (v !== undefined && v !== null) o.dataset[k] = v; return o; }
    function fill(id, rows, key, label, blank = true, data) { const s = el(id); s.replaceChildren(...(blank ? [option('', '')] : []), ...(rows || []).map(r => option(get(r, key), get(r, label), data ? data(r) : null))); }
    function choose(id, v, label) {
        const node = el(id); if (!node) return;
        if (node.tagName === 'SELECT') {
            if (v === 0 || v === '0' || v === null || v === undefined) v = '';
            if (v !== '' && !Array.from(node.options).some(o => o.value === String(v))) node.add(option(v, label ?? v));
        }
        node.value = v ?? '';
    }
    function chooseText(id, text) { const s = el(id), t = String(text ?? '').trim(); const o = Array.from(s.options).find(x => x.textContent.trim() === t && x.value !== ''); s.value = o ? o.value : ''; if (!o && t) { s.add(option(t, t)); s.value = t; } }
    const selectedText = id => { const s = el(id); return s.value === '' ? '' : (s.selectedOptions[0]?.textContent || ''); };
    const selectedData = (id, key) => el(id).selectedOptions[0]?.dataset?.[key];
    const partyData = r => ({ code: get(r, 'PartyCode'), city: get(r, 'CityName'), mobile: get(r, 'MobileNo') ?? get(r, 'MobilePersonal'), gl: get(r, 'GlAccountId') });
    const sub = () => !!state.lists.subsidiary;
    function accountOptions(rows) {
        // dtAccountlst / dtAccountlstForFreight: subsidiary = (Id = GlAccountId, SupplierCustomerId = party Id, AccountTitle = CompanyName), value SupplierCustomerId; else ChartOfAccountId / AccountTitle.
        return (rows || []).map(r => sub() ? { v: num(r, 'Id'), t: get(r, 'CompanyName'), gl: num(r, 'GlAccountId') } : { v: num(r, 'ChartOfAccountId'), t: get(r, 'AccountTitle'), gl: num(r, 'ChartOfAccountId') });
    }

    function bind(data) {
        state.lists = data; state.rights = data.rights || {}; state.config = data.configuration || {};
        state.lists.accountOpts = accountOptions(data.accounts); state.lists.freightOpts = accountOptions(data.freightAccounts);
        // BindSupplierName :1084 - supplier, commission agent and broker share dtSupplier (CustomerGroupId 7 removed).
        for (const id of ['comsupplier', 'combcommAgent', 'CmbBrokeryAc', 'ldrSupplier']) { const keep = el(id).value; fill(id, data.suppliers, 'Id', 'CompanyName', true, partyData); choose(id, keep); }
        { const keep = el('combpttrm').value; fill('combpttrm', data.paymentTerms, 'Id', 'TermsDescription', true); choose('combpttrm', keep); }
        { const keep = el('combdeliverytrm').value; fill('combdeliverytrm', (data.deliveryTerms || []).map(v => ({ v })), 'v', 'v', true); choose('combdeliverytrm', keep); }
        for (const id of ['combCommType', 'CmbBrokeryType']) { const keep = el(id).value; fill(id, ['Flat', 'Percent', 'Comm Weight'].map(v => ({ v })), 'v', 'v', true); choose(id, keep); }
        for (const id of ['combcommUOM', 'CmbBrokeryRateUom']) { const keep = el(id).value; fill(id, data.commissionUoms, 'type', 'type', true); choose(id, keep); }
        { const keep = el('CmbWarehouse').value; fill('CmbWarehouse', data.warehouses, 'Id', 'WareHouseName', true, r => ({ branch: get(r, 'BranchId'), branchName: get(r, 'BranchName') })); choose('CmbWarehouse', keep); }
        { const keep = el('CmbJobLot').value; fill('CmbJobLot', data.jobLots, 'Id', 'JobLotDescription', true, r => ({ branch: get(r, 'BranchId'), branchName: get(r, 'BranchName') })); choose('CmbJobLot', keep); }
        { const keep = el('CmbCropYear').value; fill('CmbCropYear', data.cropYears, 'CropYear', 'CropYear', true, r => ({ id: get(r, 'Id') })); choose('CmbCropYear', keep); }
        { const keep = el('CmbPackingType').value; fill('CmbPackingType', data.packingTypes, 'Id', 'PackTypeDesc', true); choose('CmbPackingType', keep); }
        fill('cmbSupplierNameHistory', data.historySuppliers, 'Id', 'ReferenceName', true);
        // GetConfigurationsFromGlobal :992 - ItemSearchByCode checks the Code radio.
        el(state.config.ItemSearchByCode ? 'rdSearchByCode' : 'rdSearchByName').checked = true;
        itemNameBind();
        multi('cmbBranchName', data.historyBranches || [], historySuppliersReload);
    }
    // ItemNameBind :1317 - display member ItemName or ItemCode by the Name/Code radio; selection retained by Id.
    function itemNameBind() {
        const keep = el('CmbItemName').value, code = el('rdSearchByCode').checked;
        fill('CmbItemName', state.lists.items, 'Id', code ? 'ItemCode' : 'ItemName', true, r => ({ itemCode: code ? get(r, 'ItemName') : get(r, 'ItemCode'), itemCategory: get(r, 'ItemCategory'), itemType: get(r, 'ItemType'), productionStage: get(r, 'productionStageName') }));
        el('CmbItemName').setAttribute('data-dtcombo-caption', code ? 'Item Code' : 'Item Name');
        choose('CmbItemName', keep);
        packUomBind(Number(el('CmbItemName').value || 0));
    }
    // PackUomFromGlobalBind :1399 - both UOM lists reload for the item; both keep the option whose UOMCode equals the Pack UOM text (quirk: Rateuom = comPackUOM.Text).
    function packUomBind(itemId) {
        const packText = selectedText('comPackUOM');
        const rows = (state.lists.uoms || []).filter(r => num(r, 'ItemId') === itemId);
        for (const id of ['comPackUOM', 'comRateUOM']) {
            fill(id, rows, 'Id', 'UOMCode', true, r => ({ eq: get(r, 'Equivalent') }));
            const match = rows.find(r => String(get(r, 'UOMCode')) === packText);
            el(id).value = match ? String(get(match, 'Id')) : '';
        }
    }

    // ------------------------------------------------------------------ grid definitions
    // dtGrid (:655-697) + grdSettings (:2286): kind n = weight/qty (sum), r = rate, a = amount (sum), d = date, po = order link.
    const DETAIL = [['Order No', 'PurchaseOrder', 'po'], ['Item', 'ItemName'], ['CropYear', 'CropYear'], ['JobLot', 'JobLotDescription'], ['PackingType', 'PackTypeDesc'], ['PackUOM', 'UOMCodeItem'],
        ['ItemQty', 'ItemQty', 'n'], ['GrossWeight', 'GrossWeight', 'n'], ['EmptyBags', 'EBWeight', 'n'], ['EmptyBagsTotal', 'EBTotalWt', 'n'], ['WeightCut', 'WeightCut', 'n'], ['WeightCutTotal', 'WeightCutTotal', 'n'],
        ['AddLss', 'AdLsWeight', 'n'], ['NetBillWeight', 'NetBillWeight', 'n'], ['StockWeight', 'NetStockWeight', 'n'], ['Rate', 'ItemRate', 'r'], ['RateUOM', 'RateUom'], ['RateCut', 'RateCut', 'r'],
        ['RateCutTotal', 'RateCutAmount', 'a'], ['ItemAmount', 'ItemAmount', 'a'], ['Warehouse', 'WareHouseName'], ['LabSampleNo', 'LabAnalisysNo'], ['GpDate', 'GpDate', 'd'], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo'],
        ['Item Net Amount', 'BillAmount', 'a'], ['Expense', 'ExpenseAmount', 'a'], ['Charge To Product', 'FreightAmount', 'a'], ['Journal', 'JournalAmount', 'a'], ['Commission', 'CommissionAmount', 'a'], ['BranchName', 'BranchName', 'branch']];
    const branchColumns = () => !!state.lists.branchFeature && !state.config.PurchaseInvoiceDirectBranchWise;
    const cellFmt = (v, kind) => kind === 'n' ? fmtW(v) : kind === 'r' ? fmtR(v) : kind === 'a' ? fmtA(v) : kind === 'd' ? ddMMMyy(v) : v;
    const link = (path, id, caption) => id > 0 ? `<a href="${path}?id=${id}" target="_blank" rel="noopener">${esc(caption)}</a>` : esc(caption);
    function renderDetails() {
        const rows = state.details, cols = DETAIL.filter(c => c[2] !== 'branch' || branchColumns()), t = el('grd');
        t.tHead.innerHTML = '<tr><th>X</th><th>+</th>' + cols.map(c => `<th>${esc(c[0])}</th>`).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map((r, i) => `<tr data-row="${i}" class="${i === state.current.grd ? 'sel' : ''}"><td class="c frz" style="left:0"><button type="button" class="gb" data-del="grd" data-row="${i}" aria-label="Delete row ${i + 1}">X</button></td><td class="c frz" style="left:28px"><button type="button" class="gb" data-add="grd" data-row="${i}" aria-label="Copy row ${i + 1}">+</button></td>`
            + cols.map(c => `<td${['n', 'r', 'a'].includes(c[2]) ? ' class="n"' : ''}>${c[2] === 'po' ? (num(r, 'PurchaseOrderId') > 0 ? link('/purchase/purchase-order', num(r, 'PurchaseOrderId'), get(r, 'PurchaseOrder')) : esc(num(r, 'PurchaseOrder') || '')) : esc(cellFmt(get(r, c[1]), c[2]))}</td>`).join('') + '</tr>').join('');
        t.tFoot.innerHTML = rows.length ? '<tr><td></td><td></td>' + cols.map(c => '<td>' + (['n', 'a'].includes(c[2]) ? esc(cellFmt(rows.reduce((s, r) => s + num(r, c[1]), 0), c[2])) : '') + '</td>').join('') + '</tr>' : '';
    }

    // Supplement grids. kinds: acct (freight/journal account), other (InventoryItemsOther), type / pm / cond / bagacct (EBComboBind), n/r/a numbers, t text, ra read-only amount.
    const GRIDS = {
        // grdFreightSettings :2837 - Transporter "Charge To Product", Percentage "%", Qty, Rate, Freight "Credit", Debit (FreightDebitToExpenses only), Remarks; X and + added last.
        freight: { table: 'grdFreight', buttonsFirst: false, cols: [['Charge To Product', 'SupplierCustomerId', 'acct', 'freightOpts'], ['%', 'Percentage', 'n'], ['Qty', 'FrQty', 'n', true], ['Rate', 'FrRate', 'r'], ['Credit', 'FreightAmount', 'a', true], ['Debit', 'Debit', 'a', true], ['Remarks', 'Remarks', 't']] },
        // grdInvExpSettings :2473 - ItemId "Other Item", Qty, Rate, Amount, Remarks; X and + last.
        expenses: { table: 'grdInvExp', buttonsFirst: false, cols: [['Other Item', 'InvRevExpItemId', 'other'], ['Qty', 'Qty', 'n', true], ['Rate', 'Rate', 'r'], ['Amount', 'Amount', 'a', true], ['Remarks', 'CustomRemarks', 'tx']] },
        // gridGLSettings :2609 - AccountId "Supplier Add/Less", Remarks, Percentage, Qty, Rate, Debit, Credit; X and + last.
        journal: { table: 'grdGLedger', buttonsFirst: false, cols: [['Supplier Add/Less', 'AccountId', 'acct', 'accountOpts'], ['Remarks', 'JvRemarks', 't'], ['Percentage', 'JvPrcnt', 'n'], ['Qty', 'JvQty', 'n', true], ['Rate', 'JvRate', 'r'], ['Debit', 'JvDebit', 'a', true], ['Credit', 'JvCredit', 'a', true]] },
        // grdEmptyBagsSettings :1782 - X and + at positions 0/1; Type, Item Name, Item Condition, ReceivedQty, PurchaseQty, Rate, Amount, Remarks (400), Credit Account.
        emptyBags: { table: 'grdEmptyBags', buttonsFirst: true, cols: [['Type', 'TypeId', 'type'], ['Item Name', 'ItemId', 'pm'], ['Item Condition', 'ItemConditionId', 'cond'], ['ReceivedQty', 'ReceivedQty', 'n', true], ['PurchaseQty', 'PurchaseQty', 'n', true], ['Rate', 'Rate', 'r'], ['Amount', 'Amount', 'ra', true], ['Remarks', 'CustomRemarks', 'tx'], ['Credit Account', 'CreditAccountId', 'bagacct']] }
    };
    const BLANK = {
        freight: () => ({ Id: 0, PurchaseOrderId: 0, FreightId: 0, SupplierCustomerId: 0, TansporterId: 0, Percentage: 0, FrQty: 0, FrRate: 0, FreightAmount: 0, Debit: 0, Remarks: '' }),
        expenses: () => ({ Id: 0, PurchaseOrderId: 0, PurchaseOrderSupplierExpId: 0, InvRevExpItemId: 0, Qty: 0, Rate: 0, Amount: 0, CustomRemarks: '' }),
        journal: () => ({ Id: 0, AccountId: 0, GlAccountId: 0, JvRemarks: '', JvPrcnt: 0, JvQty: 0, JvRate: 0, JvDebit: 0, JvCredit: 0 }),
        emptyBags: () => ({ PurchaseOrderId: null, TypeId: null, ItemId: null, ItemConditionId: null, ReceivedQty: null, PurchaseQty: null, Rate: null, Amount: null, CustomRemarks: null, CreditAccountId: null })
    };
    function comboRows(kind, listKey) {
        if (kind === 'acct') return { rows: state.lists[listKey] || [], v: 'v', t: 't' };
        if (kind === 'other') return { rows: state.lists.otherItems || [], v: 'Id', t: 'OtherItemName' };
        if (kind === 'type') return { rows: state.lists.emptyBagTypes || [], v: 'Id', t: 'type' };
        if (kind === 'pm') return { rows: state.lists.pmItems || [], v: 'ItemId', t: 'ItemName' };
        if (kind === 'cond') return { rows: state.lists.itemConditions || [], v: 'Id', t: 'ConditionStatus' };
        if (kind === 'bagacct') return { rows: state.lists.bagCreditAccounts || [], v: 'ChartOfAccountId', t: 'AccountTitle' };
        return null;
    }
    function options(rows, key, label, current) {
        let html = '<option value=""></option>', found = false;
        for (const r of rows) { const v = String(get(r, key)), on = v === String(current ?? '') && current !== null && current !== '' && Number(current) !== 0; if (on) found = true; html += `<option value="${esc(v)}"${on ? ' selected' : ''}>${esc(get(r, label))}</option>`; }
        if (!found && current !== undefined && current !== null && current !== '' && Number(current) !== 0) html += `<option selected value="${esc(current)}">${esc(current)}</option>`;
        return html;
    }
    // EmptyGridsColumnsEdit :1844 - an order row keeps Type and Rate read-only, and ReceivedQty when its Type is not 1.
    function bagLocked(r, key) {
        const po = num(r, 'PurchaseOrderId') > 0;
        if (key === 'Amount') return true;
        if (po && (key === 'TypeId' || key === 'Rate')) return true;
        if (po && key === 'ReceivedQty' && num(r, 'TypeId') !== 1) return true;
        return false;
    }
    function gridCell(grid, r, index, [caption, key, kind, listKey]) {
        const attrs = `data-grid="${grid}" data-row="${index}" data-field="${key}" aria-label="${esc(caption)} row ${index + 1}"`;
        const value = get(r, key), combo = comboRows(kind, listKey);
        const locked = (grid === 'emptyBags' && bagLocked(r, key)) || kind === 'ra';
        if (combo) {
            if (locked) { const hit = combo.rows.find(x => String(get(x, combo.v)) === String(value)); return esc(hit ? get(hit, combo.t) : (value || '')); }
            return `<select class="win-combo" ${attrs}>${options(combo.rows, combo.v, combo.t, value)}</select>`;
        }
        if (locked) return esc(kind === 't' || kind === 'tx' ? value : fmtA(value));
        if (kind === 't' || kind === 'tx') return `<input type="text" class="${kind === 'tx' ? 'xwide' : 'wide'}" ${attrs} value="${esc(value)}">`;
        return `<input type="text" inputmode="decimal" class="n" ${attrs} value="${esc(value ?? '')}">`;
    }
    function renderGrid(grid) {
        const def = GRIDS[grid], rows = state.parts[grid], t = el(def.table);
        const cols = def.cols.filter(c => !(grid === 'freight' && c[1] === 'Debit' && !state.config.DebitAmountChargetoExpenseAcFreightGridPurchase));
        const btns = i => `<td class="c"><button type="button" class="gb" data-del="${grid}" data-row="${i}" aria-label="Delete row ${i + 1}">X</button></td><td class="c"><button type="button" class="gb" data-add="${grid}" data-row="${i}" aria-label="Add row">+</button></td>`;
        const head = def.buttonsFirst ? '<th>X</th><th>+</th>' : '';
        const tail = def.buttonsFirst ? '' : '<th>X</th><th>+</th>';
        t.tHead.innerHTML = '<tr>' + head + cols.map(c => `<th>${esc(c[0])}</th>`).join('') + tail + '</tr>';
        t.tBodies[0].innerHTML = rows.map((r, i) => `<tr data-row="${i}" class="${i === state.current[grid] ? 'sel' : ''}">` + (def.buttonsFirst ? btns(i) : '') + cols.map(c => `<td${['n', 'r', 'a', 'ra'].includes(c[2]) ? ' class="n"' : ''}>${gridCell(grid, r, i, c)}</td>`).join('') + (def.buttonsFirst ? '' : btns(i)) + '</tr>').join('');
        t.tFoot.innerHTML = rows.length ? '<tr>' + (def.buttonsFirst ? '<td></td><td></td>' : '') + cols.map(c => '<td>' + (c[3] === true ? esc((c[2] === 'n' ? fmtW : fmtA)(rows.reduce((s, r) => s + num(r, c[1]), 0))) : '') + '</td>').join('') + (def.buttonsFirst ? '' : '<td></td><td></td>') + '</tr>' : '';
    }
    function renderParts() { for (const g of Object.keys(GRIDS)) renderGrid(g); }

    // ------------------------------------------------------------------ rights / buttons (InitializeComponentMethod :747-754)
    function rights() {
        const r = state.rights, loaded = state.id > 0;
        el('btnSave').hidden = loaded; el('btnUpdate').hidden = !loaded; el('btnDelete').hidden = !loaded;
        el('btnSave').disabled = r.Save !== true || PurchaseRequest.isBusy(el('btnSave'));
        // An invoice of another branch (opened from History) is read-only on the web (OtherBranch).
        el('btnUpdate').disabled = r.Update !== true || state.otherBranch || PurchaseRequest.isBusy(el('btnUpdate'));
        el('btnDelete').disabled = r.Delete !== true || state.otherBranch || PurchaseRequest.isBusy(el('btnDelete'));
        // btnPrint only gets .Checked = Print right (:751), so it stays enabled (quirk kept); btnSlip / btn225PartySlip follow Print.
        el('btnSlip').disabled = r.Print !== true || PurchaseRequest.isBusy(el('btnSlip'));
        el('btn225PartySlip').disabled = r.Print !== true || PurchaseRequest.isBusy(el('btn225PartySlip'));
    }
    const canSaveNew = () => !state.id && state.rights.Save === true; // btnSave.Visible && btnSave.Enabled

    // ------------------------------------------------------------------ header
    const HEADER = { DocDate: 'DocDate', txtbillno: 'ManualBillNo', comsupplier: 'SupplierCustomerId', combdeliverytrm: 'DeliveryTerm', combpttrm: 'PaymentTermsId', txtduedays: 'DueDays', duedate: 'DueDate', txtSupplierReference: 'SupplierReferenceNo', txtremarks: 'RemarksHeader', combcommAgent: 'CommissionAgentId', combCommType: 'CommissionType', txtcommrate: 'CommRate', combcommUOM: 'UomScheduleIdCmRate', txtCommissionRemarks: 'CommissionRemarks', CmbBrokeryAc: 'BrokerAgentId', CmbBrokeryType: 'BrokeryType', txtBrokeryRate: 'BrokeryRate', CmbBrokeryRateUom: 'BrokeryUom' };
    const NUMERIC = new Set(['SupplierCustomerId', 'PaymentTermsId', 'DueDays', 'CommissionAgentId', 'CommRate', 'BrokerAgentId', 'BrokeryRate', 'BrokeryUom']);
    function header() {
        const h = { Id: state.id, DocNo: parse(el('txtdocno').value) };
        for (const [id, key] of Object.entries(HEADER)) h[key] = NUMERIC.has(key) ? parse(el(id).value) : el(id).value.trim();
        h.CommissionRemarks = el('txtCommissionRemarks').value.trim();
        h.CustomAccounts = el('chkCustomAccounts').checked;
        h.CommAmount = parse(el('txtcommamount').value); h.BrokeryAmount = parse(el('txtBrokeryAmount').value); h.BillAmount = parse(el('txtBillAmount').value);
        return h;
    }
    const payload = () => ({ ...header(), details: state.details, ...state.parts });
    function supplierGlUpdate() { const s = (state.lists.suppliers || []).find(x => num(x, 'Id') === Number(el('comsupplier').value || 0)); state.supplierGl = s ? num(s, 'GlAccountId') : state.supplierGl; }

    async function bill(generation = state.generation) {
        const data = await api('/calculate-bill', payload());
        if (generation !== state.generation) return;
        state.details = data.details || [];
        choose('txtBillAmount', fmtA(data.billAmount)); choose('txtcommamount', fmtA(data.commAmount)); choose('txtBrokeryAmount', fmtA(data.brokeryAmount));
        renderDetails();
    }
    const recalc = () => enqueue(bill);

    // ------------------------------------------------------------------ detail editor (groupBox1)
    const LINE = { txtQty: 'ItemQty', txtGrossWeight: 'GrossWeight', txtEmptybagsUnit: 'EBWeight', txtEmptyBagsTotal: 'EBTotalWt', txtwtcut: 'WeightCut', txtWeightCutTotal: 'WeightCutTotal', txtAddLss: 'AdLsWeight', txtNetBillWeight: 'NetBillWeight', txtStockWeight: 'NetStockWeight', txtRate: 'ItemRate', txtratecut: 'RateCut', txtratecuttotal: 'RateCutAmount', txtAmount: 'ItemAmount' };
    function setNum(id, v, fmt = fmtW) { const node = el(id); if (document.activeElement === node) return; node.value = v === null || v === undefined || v === '' ? '' : node.dataset.commas ? fmt(v) : String(v); }
    function editorLine() {
        const d = { ItemId: Number(el('CmbItemName').value || 0), ItemUOMId: Number(el('comPackUOM').value || 0), UomScheduleIdRate: Number(el('comRateUOM').value || 0) };
        for (const [id, key] of Object.entries(LINE)) d[key] = parse(el(id).value);
        return d;
    }
    const bagsAgainstWeight = () => state.parts.emptyBags.some(b => num(b, 'TypeId') === 2);
    // Total :4234 / TotalWeight :4319 / AmountCaluculation :4349 (server), the result written back to the text boxes.
    function calculate(changed) {
        const version = ++state.lineVersion; state.lineError = null;
        const line = editorLine();
        state.linePending = PurchaseRequest.track(async () => {
            try {
                const data = await api('/calculate-line', { line, changed, newInvoice: canSaveNew(), bagsAgainstWeight: bagsAgainstWeight() });
                if (version !== state.lineVersion) return;
                for (const [id, key] of Object.entries(LINE)) if (['GrossWeight', 'EBWeight', 'EBTotalWt', 'WeightCut', 'WeightCutTotal', 'NetBillWeight', 'NetStockWeight', 'RateCutAmount', 'ItemAmount'].includes(key)) setNum(id, get(data, key), ['RateCutAmount', 'ItemAmount'].includes(key) ? fmtA : fmtW);
            } catch (e) { if (version === state.lineVersion) { state.lineError = e; fail(e); } }
        });
        return state.linePending;
    }
    // FormValidationDetail :1473 in its order and words.
    function formValidationDetail() {
        const checks = [['CmbItemName', 'Item Name Field is Required', v => Number(v) > 0], ['CmbCropYear', 'Crop Year Field is Required', v => v !== ''], ['CmbJobLot', 'Job/Lot Field is Required', v => Number(v) > 0],
            ['CmbPackingType', 'Packing Type Field is Required', v => Number(v) > 0], ['txtQty', 'Qty Field is Required', v => parse(v) !== 0], ['comPackUOM', 'Pack Unit Field is Required', v => Number(v) > 0],
            ['txtGrossWeight', 'Gross Weight Field is Required', v => parse(v) > 0], ['txtNetBillWeight', 'Net Bill Weight Field is Required', v => parse(v) > 0], ['txtStockWeight', 'Stock Weight Field is Required', v => parse(v) > 0],
            ['txtRate', 'Rate Field is Required', v => parse(v) !== 0], ['comRateUOM', 'Rate UOM Field is Required', v => Number(v) > 0], ['CmbWarehouse', 'Warehouse Field is Required', v => Number(v) > 0]];
        for (const [id, text, ok] of checks) if (!ok(el(id).value.trim ? el(id).value.trim() : el(id).value)) { el(id).focus(); throw Error(text); }
    }
    // Warehouse and JobLot branch check (:1986-2006) when the branch feature is on and invoices are not branch-wise.
    function branchCheck() {
        const wb = Number(selectedData('CmbWarehouse', 'branch') || 0), jb = Number(selectedData('CmbJobLot', 'branch') || 0);
        if (state.lists.branchFeature && !state.config.PurchaseInvoiceDirectBranchWise && wb !== jb)
            throw Error(`Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '${selectedData('CmbWarehouse', 'branchName') || ''}' and JobLot is of Branch '${selectedData('CmbJobLot', 'branchName') || ''}'`);
        return { BranchId: wb, BranchName: selectedData('CmbWarehouse', 'branchName') || '' };
    }
    function lineFromEditor(base = {}) {
        const b = branchCheck(), order = Number(el('CmbOrderNo').value || 0);
        return { ...base, PurchaseOrderId: order, PurchaseOrderDetailId: order > 0 ? Number(selectedData('CmbOrderNo', 'detail') || 0) : 0, PurchaseOrder: parse(selectedText('CmbOrderNo')),
            ItemId: Number(el('CmbItemName').value || 0), ItemName: selectedText('CmbItemName'), CropYear: selectedText('CmbCropYear'), JobLotId: Number(el('CmbJobLot').value || 0), JobLotDescription: selectedText('CmbJobLot'),
            PackingTypeId: Number(el('CmbPackingType').value || 0), PackTypeDesc: selectedText('CmbPackingType'), ItemUOMId: Number(el('comPackUOM').value || 0), UOMCodeItem: selectedText('comPackUOM'),
            ItemQty: parse(el('txtQty').value), GrossWeight: parse(el('txtGrossWeight').value), EBWeight: parse(el('txtEmptybagsUnit').value), EBTotalWt: parse(el('txtEmptyBagsTotal').value),
            WeightCut: parse(el('txtwtcut').value), WeightCutTotal: parse(el('txtWeightCutTotal').value), AdLsWeight: parse(el('txtAddLss').value), NetBillWeight: parse(el('txtNetBillWeight').value),
            NetStockWeight: parse(el('txtStockWeight').value), ItemRate: parse(el('txtRate').value), UomScheduleIdRate: Number(el('comRateUOM').value || 0), RateUom: selectedText('comRateUOM'),
            RateCut: parse(el('txtratecut').value), RateCutAmount: parse(el('txtratecuttotal').value), ItemAmount: parse(el('txtAmount').value), WarehouseId: Number(el('CmbWarehouse').value || 0),
            WareHouseName: selectedText('CmbWarehouse'), LabAnalisysNo: el('txtlabanalysisno').value, GpDate: el('txtgpdate').value || today(), GpNo: parse(el('txtgatepassno').value), VehicleNo: el('txtvehicleno').value.toUpperCase(),
            BranchId: b.BranchId, BranchName: b.BranchName };
    }
    // btnAdd_Click :1944
    async function addLine(button) {
        await run(button, async () => {
            await lineSettled();
            formValidationDetail();
            if (parse(el('txtAmount').value) === 0) throw Error('Please Check ItemAmount');
            const row = { Id: 0, ...lineFromEditor(), BillAmount: 0, ExpenseAmount: 0, FreightAmount: 0, JournalAmount: 0, CommissionAmount: 0, PurchaseGLAC: 0 };
            state.details.push(row); renderDetails(); resetDetail(); message(); await enqueue(bill);
        });
    }
    // btnUpdateDetail_Click :2121
    async function updateLine(button) {
        await run(button, async () => {
            await lineSettled();
            formValidationDetail();
            const index = state.edit; if (index < 0 || !state.details[index]) return;
            state.details[index] = lineFromEditor(state.details[index]);
            el('btnAdd').hidden = false; el('btnUpdateDetail').hidden = true; el('btnCancelUpdateDetial').hidden = true;
            fill('CmbOrderNo', [], '', '', true);
            renderDetails(); resetDetail(); message(); await enqueue(bill);
        });
    }
    // btnCancelUpdateDetial_Click :2206 - CmbOrderNo, txtRate and CmbItemName keep their state (quirk kept).
    function cancelLine() { el('btnAdd').hidden = false; el('btnUpdateDetail').hidden = true; el('btnCancelUpdateDetial').hidden = true; resetDetail(); }
    // ResetDetail :3825 - only these fields are cleared.
    function resetDetail() {
        state.edit = -1; ++state.lineVersion; state.lineError = null;
        choose('CmbItemName', '');
        for (const id of ['txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtWeightCutTotal', 'txtAddLss', 'txtNetBillWeight', 'txtStockWeight', 'txtRate', 'txtratecut', 'txtratecuttotal', 'txtAmount']) el(id).value = '';
        el('comRateUOM').value = '';
    }
    // grd_DoubleClick :2058
    function editLine(index) {
        const r = state.details[index]; if (!r) return;
        state.edit = index; ++state.lineVersion; state.lineError = null;
        if (num(r, 'PurchaseOrderId') > 0) {
            el('CmbOrderNo').replaceChildren(option('', ''), option(num(r, 'PurchaseOrderId'), get(r, 'PurchaseOrder') ?? '', { detail: num(r, 'PurchaseOrderDetailId') }));
            el('CmbOrderNo').value = String(num(r, 'PurchaseOrderId'));
            el('txtRate').disabled = true; el('CmbItemName').disabled = true;
        } else { el('txtRate').disabled = false; el('CmbItemName').disabled = false; }
        choose('CmbItemName', num(r, 'ItemId'), get(r, 'ItemName')); packUomBind(num(r, 'ItemId'));
        chooseText('CmbCropYear', get(r, 'CropYear')); choose('CmbJobLot', num(r, 'JobLotId'), get(r, 'JobLotDescription')); choose('CmbPackingType', num(r, 'PackingTypeId'), get(r, 'PackTypeDesc'));
        choose('comPackUOM', num(r, 'ItemUOMId'), get(r, 'UOMCodeItem')); choose('comRateUOM', num(r, 'UomScheduleIdRate'), get(r, 'RateUom'));
        for (const [id, key] of Object.entries(LINE)) el(id).value = ['ItemRate', 'RateCut'].includes(key) ? fmtR(get(r, key)) : ['RateCutAmount', 'ItemAmount'].includes(key) ? fmtA(get(r, key)) : fmtW(get(r, key));
        choose('CmbWarehouse', num(r, 'WarehouseId'), get(r, 'WareHouseName'));
        el('txtgpdate').value = date(get(r, 'GpDate')) || el('txtgpdate').value; el('txtgatepassno').value = get(r, 'GpNo') ?? ''; el('txtvehicleno').value = get(r, 'VehicleNo') ?? ''; el('txtlabanalysisno').value = get(r, 'LabAnalisysNo') ?? '';
        el('btnAdd').hidden = true; el('btnUpdateDetail').hidden = false; el('btnCancelUpdateDetial').hidden = false;
    }
    // grd_ColumnButtonClick :2214 - X = DeleteDetailrow :2240; + copies the row (its Id included).
    async function detailButton(action, index) {
        const r = state.details[index]; if (!r) return; state.current.grd = index;
        if (action === 'add') { state.details.push({ ...r }); renderDetails(); return; }
        await run(null, async () => {
            if (num(r, 'Id') > 0) {
                if (!confirm('Are you sure you want to delete this record?')) return;
                await api('/' + state.id + '/detail-row-check/' + num(r, 'Id'), {});
            }
            state.details.splice(index, 1); state.current.grd = 0; renderDetails(); await enqueue(bill);
        });
    }

    // ------------------------------------------------------------------ supplement grid edits (CellUpdated / ColumnButtonClick)
    const itemTotal = () => state.details.reduce((s, d) => s + num(d, 'ItemAmount'), 0);
    function glFor(listKey, value) { const hit = (state.lists[listKey] || []).find(o => o.v === Number(value)); return hit ? hit.gl : 0; }
    function cellUpdated(grid, index, key, raw) {
        const r = state.parts[grid][index]; if (!r) return;
        const numericKeys = { freight: ['Percentage', 'FrQty', 'FrRate', 'FreightAmount', 'Debit'], expenses: ['Qty', 'Rate', 'Amount'], journal: ['JvPrcnt', 'JvQty', 'JvRate', 'JvCredit', 'JvDebit'], emptyBags: ['ReceivedQty', 'PurchaseQty', 'Rate', 'Amount'] }[grid];
        // UpdatingCell (:2557, :2665, :3059, :1876)
        if (numericKeys.includes(key) && !isNumeric(raw)) { renderGrid(grid); message('Please Type Only Numeric Value', true); return; }
        const value = numericKeys.includes(key) ? parse(raw) : ['SupplierCustomerId', 'AccountId', 'InvRevExpItemId', 'TypeId', 'ItemId', 'ItemConditionId', 'CreditAccountId'].includes(key) ? Number(raw || 0) : raw;
        r[key] = value; let note = '';
        try {
            if (grid === 'freight') {
                // grdFreight_CellUpdated :2984
                if (key === 'FrQty' || key === 'FrRate') { r.FreightAmount = afz(num(r, 'FrQty') * num(r, 'FrRate')); r.Percentage = 0; }
                if (key === 'Percentage') {
                    if (Math.abs(bankers(num(r, 'Percentage'))) > 100) { r.Percentage = 0; throw Error('Percentage mustbe less than 100'); }
                    const t = itemTotal() / 100 * num(r, 'Percentage'); if (t > 0) r.FreightAmount = afz(t); r.FrQty = 0; r.FrRate = 0;
                }
                if (key === 'SupplierCustomerId') r.TansporterId = glFor('freightOpts', value);   // SupCustIdUpdateforFrieghtGrid :2919
                if (key === 'FreightAmount') r.FreightAmount = afz(num(r, 'FreightAmount'));
                if (state.config.DebitAmountChargetoExpenseAcFreightGridPurchase) {
                    if (key === 'FreightAmount' && num(r, 'Debit') > 0) { r.FreightAmount = 0; note = 'Debit Side is aleady added'; }
                    if (key === 'Debit') { r.Debit = afz(num(r, 'Debit')); if (num(r, 'FreightAmount') > 0) { r.Debit = 0; note = 'Credit Side is aleady added'; } }
                }
            } else if (grid === 'expenses') {
                // grdInvExp_CellUpdated :2522
                if (key === 'Qty' || key === 'Rate') r.Amount = afz(num(r, 'Qty') * num(r, 'Rate'));
                else if (key === 'Amount') { r.Qty = 0; r.Rate = 0; r.Amount = afz(num(r, 'Amount')); }
            } else if (grid === 'journal') {
                // grdGLedger_CellUpdated :2711
                if (key === 'JvQty' || key === 'JvRate') { r.JvCredit = afz(num(r, 'JvQty') * num(r, 'JvRate')); r.JvDebit = 0; r.JvPrcnt = 0; }
                if (key === 'JvPrcnt') { const t = itemTotal() / 100 * num(r, 'JvPrcnt'); if (t > 0) { r.JvCredit = afz(t); r.JvDebit = 0; } else { r.JvDebit = afz(Math.abs(t)); r.JvCredit = 0; } r.JvQty = 0; r.JvRate = 0; }
                if (key === 'JvCredit') { r.JvCredit = afz(num(r, 'JvCredit')); if (num(r, 'JvDebit') > 0) { r.JvCredit = 0; note = 'Debit Side is aleady added'; } }
                if (key === 'JvDebit') { r.JvDebit = afz(num(r, 'JvDebit')); if (num(r, 'JvCredit') > 0) { r.JvDebit = 0; note = 'Credit Side is aleady added'; } }
                if (key === 'AccountId') {
                    // Subsidiary: comsupplier.Value against the row's (previous) GlAccountId; otherwise txtSupplierGLId against the new AccountId.
                    if (sub()) { if (Number(el('comsupplier').value || 0) === num(r, 'GlAccountId')) { r.GlAccountId = 0; renderGrid(grid); message('Supplier Account Not select', true); return; } }
                    else if (state.supplierGl === num(r, 'AccountId')) { r.AccountId = 0; renderGrid(grid); message('Supplier Account Not select', true); return; }
                    for (const j of state.parts.journal) if (num(j, 'AccountId')) j.GlAccountId = glFor('accountOpts', num(j, 'AccountId')) || num(j, 'GlAccountId');   // SupCustIdUpdateforGLGrid :2794
                }
            } else if (grid === 'emptyBags') {
                // grdEmptyBags_CellUpdated :1921
                if ((key === 'Rate' || key === 'PurchaseQty') && get(r, 'PurchaseQty') !== null && get(r, 'Rate') !== null && get(r, 'PurchaseQty') !== undefined && get(r, 'Rate') !== undefined) r.Amount = afz(num(r, 'PurchaseQty') * num(r, 'Rate'));
            }
        } catch (e) { renderGrid(grid); message(e.message, true); return; }
        renderGrid(grid); message(note, !!note); recalc();
    }
    // grdFreight/grdInvExp/grdGLedger/grdEmptyBags ColumnButtonClick (:2952, :2572, :2680, :1891).
    function removeRow(grid, index) {
        const rows = state.parts[grid]; if (!rows[index]) return;
        rows.splice(index, 1); if (!rows.length) rows.push(BLANK[grid]());
        // grdEmptyBags_ColumnButtonClick :1891 does not call BillAmount (the others do).
        state.current[grid] = 0; renderGrid(grid); if (grid !== 'emptyBags') recalc();
    }
    function addRow(grid) {
        // grdEmptyBags: "+" only adds while the grid has exactly one row (:1911, quirk kept).
        if (grid === 'emptyBags' && state.parts.emptyBags.length !== 1) return;
        state.parts[grid].push(BLANK[grid]()); renderGrid(grid);
        if (grid !== 'expenses' && grid !== 'emptyBags') recalc();
    }

    // ------------------------------------------------------------------ display / load / reset
    function toGridRows(data) {
        const freight = (data.freight || []).map(f => ({ Id: num(f, 'Id'), PurchaseOrderId: num(f, 'PurchaseOrderId'), FreightId: num(f, 'FreightId'), SupplierCustomerId: num(f, 'SupplierCustomerId'), TansporterId: num(f, 'TansporterId'), Percentage: num(f, 'Percentage'), FrQty: num(f, 'FrQty'), FrRate: num(f, 'FrRate'), FreightAmount: bankers(num(f, 'FreightAmount')), Debit: num(f, 'Debit'), Remarks: get(f, 'Remarks') ?? '' }));
        const expenses = (data.expenses || []).map(e => ({ Id: num(e, 'Id'), PurchaseOrderId: num(e, 'PurchaseOrderId'), PurchaseOrderSupplierExpId: num(e, 'PurchaseOrderSupplierExpId'), InvRevExpItemId: num(e, 'InvRevExpItemId'), Qty: num(e, 'Qty'), Rate: num(e, 'Rate'), Amount: num(e, 'Amount'), CustomRemarks: get(e, 'CustomRemarks') ?? '' }));
        const journal = (data.journal || []).map(j => ({ Id: num(j, 'Id'), AccountId: sub() ? num(j, 'SupplierCustomerId') : num(j, 'ChartofAccountId'), GlAccountId: num(j, 'ChartofAccountId'), JvRemarks: get(j, 'JvRemarks') ?? '', JvPrcnt: num(j, 'JvPrcnt'), JvQty: num(j, 'JvQty'), JvRate: num(j, 'JvRate'), JvDebit: num(j, 'JvDebit'), JvCredit: num(j, 'JvCredit') }));
        const emptyBags = (data.emptyBags || []).map(b => ({ PurchaseOrderId: num(b, 'PurchaseOrderId'), TypeId: num(b, 'TypeId'), ItemId: num(b, 'ItemId'), ItemConditionId: num(b, 'ItemConditionId'), ReceivedQty: num(b, 'ReceivedQty'), PurchaseQty: num(b, 'PurchaseQty'), Rate: num(b, 'Rate'), Amount: num(b, 'Amount'), CustomRemarks: get(b, 'CustomRemarks') ?? '', CreditAccountId: num(b, 'CreditAccountId') }));
        return { freight, expenses, journal, emptyBags };
    }
    function ensureBlankRows() { for (const g of Object.keys(GRIDS)) if (!state.parts[g].length) state.parts[g].push(BLANK[g]()); }
    function lockFromOrders(on) { for (const id of ['comsupplier', 'combdeliverytrm', 'combpttrm']) el(id).disabled = on; }
    // ReadById :3546
    function display(data) {
        attachmentEditor.reset();
        state.id = num(data, 'Id'); state.h = data; state.approved = !!get(data, 'approved'); state.voucherHeadId = num(data, 'voucherHeadId'); state.error = null; state.otherBranch = !!get(data, 'OtherBranch');
        // EmptyBagsTotal and WeightCutTotal enter the grid through Math.Round (0 decimals) (:3609, quirk kept).
        state.details = (data.details || []).map(d => ({ ...d, EBTotalWt: bankers(num(d, 'EBTotalWt')), WeightCutTotal: bankers(num(d, 'WeightCutTotal')) }));
        state.parts = toGridRows(data); ensureBlankRows();
        choose('txtBranchSrNo', get(data, 'BranchSrNo')); choose('txtdocno', get(data, 'DocNo')); choose('DocDate', date(get(data, 'DocDate')));
        choose('comsupplier', num(data, 'SupplierCustomerId'), get(data, 'supplierName')); supplierGlUpdate();
        el('txtSupplierReference').value = get(data, 'SupplierReferenceNo') ?? ''; el('txtbillno').value = get(data, 'ManualBillNo') ?? '';
        if (num(data, 'CommissionAgentId') > 0) choose('combcommAgent', num(data, 'CommissionAgentId'));
        chooseText('combCommType', get(data, 'CommissionType')); el('txtcommrate').value = get(data, 'CommRate') ?? ''; chooseText('combcommUOM', get(data, 'UomScheduleIdCmRate'));
        choose('txtcommamount', fmtA(get(data, 'CommAmount'))); el('txtCommissionRemarks').value = get(data, 'CommissionRemarks') ?? '';
        if (num(data, 'BrokerAgentId') > 0) choose('CmbBrokeryAc', num(data, 'BrokerAgentId'));
        chooseText('CmbBrokeryType', get(data, 'BrokeryType')); el('txtBrokeryRate').value = get(data, 'BrokeryRate') ?? ''; choose('txtBrokeryAmount', fmtA(get(data, 'BrokeryAmount')));
        chooseText('CmbBrokeryRateUom', num(data, 'BrokeryUom') ? String(num(data, 'BrokeryUom')) : '');
        el('txtremarks').value = get(data, 'OtherRemarks') ?? ''; choose('txtBillAmount', fmtA(get(data, 'BillAmount')));
        choose('combpttrm', num(data, 'PaymentTermsId')); chooseText('combdeliverytrm', get(data, 'DeliveryTerm'));
        choose('duedate', date(get(data, 'DueDate'))); el('txtduedays').value = get(data, 'DueDays') ?? '';
        el('chkCustomAccounts').checked = get(data, 'CustomAccounts') === true || get(data, 'CustomAccounts') === 1;
        paymentTermChanged();
        lockFromOrders(state.details.some(d => num(d, 'PurchaseOrderId') > 0));
        el('lblDocCode').textContent = '[' + (get(data, 'DocNo') ?? '') + ']' + (state.otherBranch ? ' (other branch - read only)' : '');
        renderDetails(); renderParts(); rights();
    }
    async function load(id) {
        const version = ++state.generation, data = await api('/' + Number(id));
        if (version !== state.generation) return;
        display(data); showTab('tabForm'); message();
        window.history.replaceState(null, '', route + '?id=' + state.id);
        enqueue(bill);
    }
    // GetConfigurationsFromGlobalAndBindValuesInColumns :1009
    function configDefaults() {
        const d = state.lists.defaults || {};
        const wh = parseInt(d.Warehouse, 10); if (wh) choose('CmbWarehouse', wh);
        const crop = (state.lists.cropYears || []).find(r => num(r, 'Id') === parseInt(d['Default Crop Year'], 10)); if (crop) choose('CmbCropYear', get(crop, 'CropYear'));
        const job = parseInt(d['Job/Lot'], 10); if (job) choose('CmbJobLot', job);
        const pack = parseInt(d['Paking Type'], 10); if (pack) choose('CmbPackingType', pack);
    }
    // Reset :3741 (btnNew). On the first open InitializeComponentMethod binds instead: Comm/Brokery type "Percent" (Rows[1]),
    // UOM Rows[0], Warehouse/JobLot Rows[1], no delivery term; Reset activates the first delivery term and empties the rest.
    async function reset() {
        const version = ++state.generation; state.error = null;
        const code = await api('/next-code'); if (version !== state.generation) return;
        attachmentEditor.reset();
        state.id = 0; state.h = {}; state.voucherHeadId = 0; state.approved = false; state.otherBranch = false; state.details = []; state.current = { grd: 0, freight: 0, expenses: 0, journal: 0, emptyBags: 0 };
        state.parts = { freight: [], expenses: [], journal: [], emptyBags: [] }; ensureBlankRows();
        choose('txtdocno', code.docNo); choose('txtBranchSrNo', code.branchSrNo ?? '');
        for (const id of ['comsupplier', 'combcommAgent', 'combCommType', 'combcommUOM', 'CmbBrokeryAc', 'CmbBrokeryRateUom', 'CmbBrokeryType', 'CmbOrderNo', 'CmbItemName', 'CmbCropYear', 'CmbJobLot', 'CmbPackingType', 'comPackUOM', 'comRateUOM', 'CmbWarehouse']) choose(id, '');
        fill('CmbOrderNo', [], '', '', true);
        for (const id of ['txtSupplierReference', 'txtbillno', 'txtCommissionRemarks', 'txtcommrate', 'txtcommamount', 'txtremarks', 'txtBrokeryRate', 'txtBrokeryAmount', 'txtBillAmount', 'txtlabanalysisno', 'txtgatepassno', 'txtvehicleno']) el(id).value = '';
        for (const id of Object.keys(LINE)) el(id).value = '';
        el('chkCustomAccounts').checked = false;
        lockFromOrders(false); el('CmbItemName').disabled = false; el('txtRate').disabled = false;
        el('btnAdd').hidden = false; el('btnUpdateDetail').hidden = true; el('btnCancelUpdateDetial').hidden = true; state.edit = -1;
        if (state.firstNew) {
            state.firstNew = false;
            for (const id of ['combCommType', 'CmbBrokeryType']) choose(id, 'Percent');
            for (const id of ['combcommUOM', 'CmbBrokeryRateUom']) { const first = (state.lists.commissionUoms || [])[0]; if (first) choose(id, get(first, 'type')); }
            const wh = (state.lists.warehouses || [])[0]; if (wh) choose('CmbWarehouse', num(wh, 'Id'));
            const job = (state.lists.jobLots || [])[0]; if (job) choose('CmbJobLot', num(job, 'Id'));
            if (!el('DocDate').value) choose('DocDate', today());
            if (!el('duedate').value) choose('duedate', today());
            if (!el('txtgpdate').value) choose('txtgpdate', today());
        } else {
            const first = (state.lists.deliveryTerms || [])[0]; if (first) choose('combdeliverytrm', first);
        }
        el('txtduedays').value = '0'; dueDaysCalculate();
        configDefaults(); packUomBind(0);
        el('lblDocCode').textContent = '';
        showSubTab('tabDetail'); renderDetails(); renderParts(); rights(); paymentTermChanged(false);
        window.history.replaceState(null, '', route);
    }
    const newRecord = button => run(button, async () => { await reset(); message(); });
    // btnRefresh_Click :3858 - global lists re-read, special rights re-applied, combos re-bound (selection kept).
    async function refresh(button) {
        await run(button, async () => { bind(await api('/dropdowns')); configDefaults(); specialRights(); renderDetails(); renderParts(); });
    }

    // ------------------------------------------------------------------ due days (combpttrm_ValueChanged :5757, DueDaysCalculate :5780, duedate_ValueChanged :5823)
    function paymentTermChanged(apply = true) {
        const cash = selectedText('combpttrm') === 'Cash';
        el('txtduedays').disabled = cash; el('duedate').disabled = cash;
        if (cash && apply) { el('txtduedays').value = ''; choose('duedate', el('DocDate').value); }
    }
    function dueDaysCalculate() {
        const days = el('txtduedays').value.trim();
        if (days !== '' && el('DocDate').value) { const d = new Date(el('DocDate').value + 'T12:00:00'); d.setDate(d.getDate() + parse(days)); choose('duedate', ymd(d)); }
        else choose('duedate', today());
    }
    function dueDateChanged() {
        const doc = el('DocDate').value; if (!doc || !el('duedate').value) return;
        if (el('duedate').value < doc) choose('duedate', doc);
        el('txtduedays').value = String(Math.round((Date.parse(el('duedate').value) - Date.parse(doc)) / 86400000));
    }

    // ------------------------------------------------------------------ save / delete / prints
    function formValidation() {
        // FormValidation :1444 (the server repeats it with the rest of Insert).
        if (!Number(el('comsupplier').value || 0)) { el('comsupplier').focus(); throw Error('Supplier Field is Required'); }
        if (!el('combdeliverytrm').value) { el('combdeliverytrm').focus(); throw Error('DeliveryTerm Field is Required'); }
        if ((Number(el('combcommAgent').value || 0) > 0 || parse(el('txtcommamount').value) > 0) && el('txtCommissionRemarks').value === '') { el('txtCommissionRemarks').focus(); throw Error('Commission Remarks Field is Required'); }
        const docNo = el('txtdocno').value.trim(); if (docNo === '' || docNo === '0') throw Error('DocNo Field is Required');
    }
    async function save(button) {
        await run(button, async () => {
            await lineSettled(); await settled(); await attachmentEditor.settled();
            // btnUpdate_Click :3534 - approved first.
            if (state.id && state.approved) throw Error('Record Not Update because Record has approved');
            formValidation();
            if (!confirm(state.id ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            const data = await api('/save', { ...payload(), attachments: attachmentEditor.payload() });
            const savedId = Number(data.id);
            message(data.message);
            const after = async () => {
                await reset(); message(data.message);
                // :3492-3506 - prints after save by the header check boxes.
                if (el('ChkBok').checked) { const v = num(await api('/' + savedId), 'voucherHeadId'); print('118-AcRptVoucherSlip.rpt', { id: v, documentTypeId: DOC_TYPE }); }
                if (el('ChkPrintSlip').checked) print('225-InvRepPurchaseBillDirectWithoutPo.rpt', { id: savedId });
                if (el('chkPartySlip').checked) print('225A-InvRepPurchaseBillDirectWithoutPo.rpt', { id: savedId });
            };
            // :3482 - frmwagesBillHeader(RefDocTypeId 57, RefDocId, GrossWeightTotal).ShowDialog() before Reset.
            if (data.openWages) await new Promise(resolve => openWages(savedId, Number(data.grossWeightTotal) || 0, resolve));
            await after();
        });
    }
    function openWages(refDocId, gross, onClosed) {
        const ov = document.createElement('div'); ov.className = 'fx-overlay';
        const fr = document.createElement('iframe'); fr.title = 'Contractor Wages Bill';
        fr.src = '/production/wages-bill?' + new URLSearchParams({ refDocTypeId: DOC_TYPE, refDocId, grossWeightTotal: gross });
        const close = document.createElement('button'); close.type = 'button'; close.className = 'fx-btn fx-overlay-close'; close.textContent = 'Close';
        ov.append(fr, close); document.body.appendChild(ov);
        const done = () => { ov.remove(); window.P280WagesClosed = null; onClosed(); };
        close.addEventListener('click', done);
        window.P280WagesClosed = done;
    }
    // btnDelete_Click :3699
    async function remove(button) {
        if (state.approved) { message('Record Not Delete because Record has approved', true); return; }
        if (!state.id) { message('Record Not Found', true); return; }
        if (!confirm('Are you sure to Delete?')) return;
        await run(button, async () => { await settled(); const r = await api('/' + state.id, null, 'DELETE'); await reset(); message(r.message || 'Delete Voucher Successfully'); });
    }
    function print(rpt, args) {
        const w = window.open('', '_blank'); try { if (w) w.document.write('<p style="font:13px Segoe UI">Preparing report...</p>'); } catch (x) { /* popup blocked */ }
        return fetch('/api/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify(args || {}) })
            .then(r => (r.ok && (r.headers.get('Content-Type') || '').includes('application/pdf')) ? r.blob().then(b => { const u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); })
                : r.text().then(t => { if (w) w.close(); message(t || ('Print failed (' + r.status + ')'), true); }))
            .catch(e => { if (w) w.close(); fail(e); });
    }
    // btnPrint_Click :5489 (VoucherReport_118(VoucherHeadId, 57)), btnSlip_Click :5501 (225), btn225PartySlip_Click :5513 (225A).
    const printVoucher = () => print('118-AcRptVoucherSlip.rpt', { id: state.voucherHeadId, documentTypeId: DOC_TYPE });
    const printSlip = () => print('225-InvRepPurchaseBillDirectWithoutPo.rpt', { id: state.id });
    const printPartySlip = () => print('225A-InvRepPurchaseBillDirectWithoutPo.rpt', { id: state.id });

    // ------------------------------------------------------------------ Purchase Order loader (btnPurchaseOrderLoader_Click :5246)
    const LOADER_COLS = [['DocNo', 'DocNo', 'po'], ['DocDate', 'DocDate', 'd'], ['SupplierName', 'SupplierName'], ['ItemName', 'ItemName'], ['ItemCode', 'ItemCode'], ['Crop', 'Crop'], ['JobLot', 'JobLotDescription'], ['PackUom', 'PackUom'], ['ItemQty', 'ItemQty', 'n'], ['ItemWeight', 'ItemWeight', 'n'], ['RateUOM', 'UOMCode'], ['OrderItemRate', 'OrderItemRate', 'r'], ['DeliveryTerm', 'DeliveryTerm']];
    let gridIds = '';
    async function openLoader(button) {
        if (state.details.length) {
            gridIds = state.details.map(d => ',' + num(d, 'PurchaseOrderId')).join('');
            const cur = state.details[state.current.grd];
            if (cur && num(cur, 'PurchaseOrderId') === 0) { message('You cannot Load PurchaseOrder because you add record without Order', true); gridIds = ''; return; }
        } else gridIds = '';
        el('dlgLoadOrder').hidden = false;
        await searchOrders(button);
    }
    async function searchOrders(button) {
        await run(button, async () => {
            const q = new URLSearchParams();
            if (Number(el('ldrSupplier').value) > 0) q.set('supplierId', el('ldrSupplier').value);
            if (el('ldrFromDate').value) q.set('fromDate', el('ldrFromDate').value); if (el('ldrToDate').value) q.set('toDate', el('ldrToDate').value);
            if (parse(el('ldrFromDocNo').value) > 0) q.set('fromDocNo', String(parse(el('ldrFromDocNo').value))); if (parse(el('ldrToDocNo').value) > 0) q.set('toDocNo', String(parse(el('ldrToDocNo').value)));
            if (gridIds) q.set('excludeIds', gridIds);
            state.loaderRows = await api('/order-loader?' + q);
            const t = el('ldrGrid');
            t.tHead.innerHTML = '<tr><th><input type="checkbox" id="ldrAll" aria-label="Select all orders"></th>' + LOADER_COLS.map(c => `<th>${c[0]}</th>`).join('') + '</tr>';
            t.tBodies[0].innerHTML = state.loaderRows.map((r, i) => `<tr data-ldr="${i}"><td class="c"><input type="checkbox" data-order-id="${num(r, 'Id')}" aria-label="Select order ${esc(get(r, 'DocNo'))}"></td>` + LOADER_COLS.map(c => `<td${['n', 'r'].includes(c[2]) ? ' class="n"' : ''}>${c[2] === 'po' ? link('/purchase/purchase-order', num(r, 'Id'), get(r, c[1])) : esc(c[2] === 'd' ? ddMMMyy(get(r, c[1])) : c[2] === 'n' ? fmtW(get(r, c[1])) : c[2] === 'r' ? fmtR(get(r, c[1])) : get(r, c[1]))}</td>`).join('') + '</tr>').join('');
            message(state.loaderRows.length + ' pending order lines', false, 'ldrMsg');
        }, 'ldrMsg');
    }
    async function loadOrders(button) {
        await run(button, async () => {
            const ids = Array.from(document.querySelectorAll('#ldrGrid [data-order-id]:checked')).map(x => Number(x.dataset.orderId));
            el('dlgLoadOrder').hidden = true; gridIds = '';
            if (!ids.length) return;
            await settled();
            const data = await api('/order-load?orderIds=' + encodeURIComponent(',' + ids.join(',')));
            loadPurchaseOrderDataForInvoice(data.orders || []);
            loadExpensesChargeToProduct(data.freight || []);
            loadExpData(data.expenses || []);
            renderDetails(); renderParts();
            await emptyBagsFromOrder();
            await enqueue(bill);
        }, 'ldrMsg');
    }
    // LoadPurchaseOrderDataForInvoice :5316
    function loadPurchaseOrderDataForInvoice(rows) {
        if (!rows.length) return;
        const o = rows[0];
        choose('comsupplier', num(o, 'SupplierCustomerId'), get(o, 'SupplierName')); supplierGlUpdate();
        choose('combcommAgent', num(o, 'BrokerAgentSupCustId')); chooseText('combCommType', get(o, 'CommissionType')); el('txtcommrate').value = get(o, 'CommRate') ?? '';
        chooseText('combcommUOM', get(o, 'UomScheduleIdCmRate')); choose('txtcommamount', fmtA(get(o, 'CommAmount'))); el('txtCommissionRemarks').value = get(o, 'CommissionRemarks') ?? '';
        choose('CmbBrokeryAc', num(o, 'BrokerAgentId')); chooseText('CmbBrokeryType', get(o, 'BrokeryType')); el('txtBrokeryRate').value = get(o, 'BrokeryRate') ?? '';
        chooseText('CmbBrokeryRateUom', num(o, 'BrokeryUom') ? String(num(o, 'BrokeryUom')) : ''); choose('txtBrokeryAmount', fmtA(get(o, 'BrokeryAmount')));
        el('txtremarks').value = get(o, 'RemarksHeader') ?? ''; chooseText('combdeliverytrm', get(o, 'DeliveryTerm')); choose('combpttrm', num(o, 'PaymentTermsId'));
        choose('duedate', date(get(o, 'OrderDueDate'))); el('txtduedays').value = get(o, 'OrderDueDays') ?? '';
        lockFromOrders(true); el('CmbItemName').disabled = true;
        dueDaysCalculate(); paymentTermChanged();
        const term = selectedText('combdeliverytrm');
        if (['Ponch', 'Ponch & PartyWeight', 'Ponch & FactoryWeight'].includes(term))
            for (const f of state.parts.freight) if (num(f, 'FreightAmount') > 0) { f.FreightAmount = 0; f.SupplierCustomerId = 0; }
        for (const r of rows) {
            let exists = false;
            for (const d of state.details) {
                if (num(d, 'PurchaseOrderId') !== num(r, 'Id')) { message('Data against another OrderNo Already Exist in Detail', true); return; }
                if (num(d, 'PurchaseOrderDetailId') === num(r, 'PODetailId')) { exists = true; break; }
            }
            if (exists) continue;
            const net = num(r, 'GrossWeight') - num(r, 'EmptyBagsTotal'), amount = num(r, 'RateUOM') ? net / num(r, 'RateUOM') * num(r, 'ItemRate') : 0;
            state.details.push({ Id: 0, PurchaseOrderId: num(r, 'Id'), PurchaseOrderDetailId: num(r, 'PODetailId'), PurchaseOrder: num(r, 'OrderNo'), ItemId: num(r, 'ItemId'), ItemName: get(r, 'ItemName'), CropYear: get(r, 'Crop') ?? '',
                JobLotId: num(r, 'JobLotId'), JobLotDescription: get(r, 'JobLotDescription'), PackingTypeId: 0, PackTypeDesc: '0', ItemUOMId: num(r, 'OrderItemUOMId'), UOMCodeItem: get(r, 'PackUom'),
                ItemQty: num(r, 'ItemQty'), GrossWeight: num(r, 'GrossWeight'), EBWeight: num(r, 'EmptyBags'), EBTotalWt: num(r, 'EmptyBagsTotal'), WeightCut: 0, WeightCutTotal: 0, AdLsWeight: 0,
                NetBillWeight: net, NetStockWeight: net, ItemRate: num(r, 'ItemRate'), UomScheduleIdRate: num(r, 'OrderItemRateUOMId'), RateUom: get(r, 'RateUomCode'), RateCut: 0, RateCutAmount: 0,
                ItemAmount: amount, WarehouseId: 0, WareHouseName: '', LabAnalisysNo: get(r, 'LabSampleNo') ?? '', GpDate: null, GpNo: 0, VehicleNo: '', BillAmount: amount, ExpenseAmount: 0, FreightAmount: 0,
                JournalAmount: 0, CommissionAmount: 0, PurchaseGLAC: num(r, 'PurchaseGLAC'), BranchId: num(r, 'BranchId'), BranchName: get(r, 'BranchName') ?? '' });
        }
    }
    // LoadPurchaseOrderExpensesChargToProductData :5398 - only for a "Load..." delivery term; Freight = Math.Round(Amount).
    function loadExpensesChargeToProduct(rows) {
        if (!rows.length) return;
        const term = selectedText('combdeliverytrm');
        state.parts.freight = [];
        if (!['Load', 'Load & PartyWeight', 'Load & FactoryWeight'].includes(term)) return;
        for (const r of rows) {
            const value = sub() ? num(r, 'SupplierCustomerId') : num(r, 'AccountId');
            state.parts.freight.push({ Id: 0, PurchaseOrderId: num(r, 'PurchaseOrderId'), FreightId: 0, SupplierCustomerId: value, TansporterId: sub() ? num(r, 'AccountId') : glFor('freightOpts', value), Percentage: num(r, 'Percentage'), FrQty: num(r, 'Qty'), FrRate: num(r, 'Rate'), FreightAmount: bankers(num(r, 'Amount')), Debit: 0, Remarks: get(r, 'Remarks') ?? '' });
        }
    }
    // LoadExpData :5448
    function loadExpData(rows) {
        for (const r of rows) {
            let exists = false;
            for (const e of state.parts.expenses) {
                if (num(e, 'PurchaseOrderId') > 0 && num(e, 'PurchaseOrderId') !== num(r, 'PurchaseOrderId')) { message('Data against another OrderNo Already Exist in Expense Grid', true); return; }
                if (num(e, 'PurchaseOrderSupplierExpId') === num(r, 'Id')) { exists = true; break; }
            }
            if (!exists) state.parts.expenses.push({ Id: 0, PurchaseOrderId: num(r, 'PurchaseOrderId'), PurchaseOrderSupplierExpId: num(r, 'Id'), InvRevExpItemId: num(r, 'InvRevExpItemId'), Qty: num(r, 'Qty'), Rate: num(r, 'Rate'), Amount: num(r, 'Amount'), CustomRemarks: get(r, 'Remarks') ?? '' });
        }
    }
    // GetEmptyBagsInformationFromOrder :1607 (after LoadInGridDetail, CmbOrderNo_Leave, comPackingType_Leave).
    async function emptyBagsFromOrder() {
        const ids = [...new Set(state.details.map(d => num(d, 'PurchaseOrderId')))].join(',');
        if (!ids) return;
        const rows = await api('/order-empty-bags?orderIds=' + encodeURIComponent(ids));
        if (!rows.length) return;
        const qtyByOrder = new Map(); for (const d of state.details) qtyByOrder.set(num(d, 'PurchaseOrderId'), (qtyByOrder.get(num(d, 'PurchaseOrderId')) || 0) + num(d, 'ItemQty'));
        const done = new Set(); let unitSet = false;
        for (const r of rows) {
            const type = num(r, 'Type'); if (type >= 1 && type <= 3) { el('txtEmptybagsUnit').value = String(get(r, 'WeightCut') ?? ''); unitSet = true; }
            let purQty = 0; const order = num(r, 'PurchaseOrderId');
            if (!done.has(order)) { purQty = qtyByOrder.get(order) || 0; done.add(order); }
            addOrUpdateBagFromOrder(r, purQty);
        }
        if (state.parts.emptyBags.length) state.parts.emptyBags[0].ItemConditionId = 1;
        if (!state.parts.emptyBags.length) state.parts.emptyBags.push(BLANK.emptyBags());
        renderGrid('emptyBags');
        // txtEmptybagsUnit_TextChanged -> Total(): the unit is recomputed from PMW Total / Qty (the set value does not stay - quirk kept).
        if (unitSet) calculate('');
        recalc();
    }
    // AddOrUpdateEmptyBagRowFromOrder :1689
    function addOrUpdateBagFromOrder(r, purQty) {
        const order = num(r, 'PurchaseOrderId'), type = num(r, 'Type'), item = num(r, 'ItemId'), canSave = canSaveNew();
        const hit = state.parts.emptyBags.find(b => num(b, 'PurchaseOrderId') === order && num(b, 'TypeId') === type && (num(b, 'ItemId') === 0 || num(b, 'ItemId') === item));
        if (!canSave) purQty = 0;
        if (hit) {
            if (num(hit, 'PurchaseQty') === 0) hit.PurchaseQty = purQty;
            if (num(hit, 'ItemId') === 0) hit.ItemId = item;
            if (num(hit, 'ItemConditionId') === 0) hit.ItemConditionId = 1;
            return;
        }
        state.parts.emptyBags.push({ PurchaseOrderId: order, TypeId: type, ItemId: item, ItemConditionId: 1, ReceivedQty: 0, PurchaseQty: purQty, Rate: num(r, 'Rate'), Amount: 0, CustomRemarks: '', CreditAccountId: 0 });
    }

    // ------------------------------------------------------------------ History (GetAll :4676, HistoryGridSettings :4809)
    const HISTORY = [['OrderNo', 'OrderNo', 'po'], ['DocNo', 'DocNo', 'doc'], ['BranchSrNo', 'BranchSrNo', 'branch'], ['BranchName', 'BranchName', 'branch'], ['DocDate', 'DocDate', 'd'], ['PurchaseAgainst', 'PurchaseAgainst'], ['DueDays', 'DueDays'], ['DueDate', 'DueDate', 'd'],
        ['ManualBillNo', 'ManualBillNo'], ['SupplierName', 'SupplierName'], ['SupReference', 'ReferencePartyName'], ['CommAgent', 'CommissionAgent'], ['CommType', 'CommissionType'], ['CommRate', 'CommRate', 'r'], ['CommAmount', 'CommAmount', 'a'],
        ['CommRemarks', 'CommissionRemarks'], ['BillAmount', 'BillAmount', 'a'], ['ApprovedStatus', 'ApprovedStatus'], ['EntryUser', 'EntryUser'], ['EntryDate', 'EntryDate', 'dt'], ['ModifyUser', 'ModifyUser'], ['ModifyDate', 'ModifyDate', 'dt'],
        ['ApprovedUser', 'ApprovedUser'], ['ApprovedDate', 'PostDate', 'dt'], ['NoOfAttachments', 'NoOfAttachments', 'att'], ['Remarks', 'RemarksHeader']];
    // GetDetailGrdByHeadId :5000 / DetailGridSetting :5058
    const HISTORY_DETAIL = [['OrderNo', 'PurchaseOrder', 'po'], ['ItemName', 'ItemName'], ['CropYear', 'CropYear'], ['JobLot', 'JobLotDescription'], ['PackType', 'PackTypeDesc'], ['Pack UOM', 'UOMCodeItem'], ['Qty', 'ItemQty', 'n'], ['GrossWeight', 'GrossWeight', 'n'],
        ['EBTotalWt', 'EBTotalWt', 'n'], ['WeightCutTotal', 'WeightCutTotal', 'n'], ['Ad / Ls', 'AdLsWeight', 'n'], ['NetBillWeight', 'NetBillWeight', 'n'], ['Stock Weight', 'NetStockWeight', 'n'], ['Rate', 'ItemRate', 'r'], ['RateUOM', 'RateUom'],
        ['RateCut', 'RateCut', 'r'], ['RateCutAmount', 'RateCutAmount', 'a'], ['ItemAmount', 'ItemAmount', 'a'], ['WareHouseName', 'WareHouseName'], ['GpDate', 'GpDate', 'ds'], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo'],
        ['ExpenseAmount', 'ExpenseAmount', 'a'], ['FreightAmount', 'FreightAmount', 'a'], ['CommissionAmount', 'CommissionAmount', 'a'], ['ItemNetAmount', 'BillAmount', 'a'], ['BranchName', 'BranchName', 'branch']];
    function historyCell(r, [, key, kind]) {
        const v = get(r, key);
        if (kind === 'po') return num(r, 'PurchaseOrderId') > 0 ? link('/purchase/purchase-order', num(r, 'PurchaseOrderId'), v) : esc(v ?? '');
        if (kind === 'doc') return `<a href="${route}?id=${num(r, 'Id')}" data-hload="1">${esc(v)}</a>`;
        if (kind === 'att') return `<a href="javascript:void(0)" data-hatt="1">${esc(v ?? 0)}</a>`;
        if (kind === 'd' || kind === 'ds') return esc(date(v));
        if (kind === 'dt') return esc(dtShort(v));
        return esc(cellFmt(v, kind));
    }
    function resetHistory() {
        // btnNewHistory_Click :5201
        choose('FromDateHistory', today()); choose('ToDateHistory', today());
        choose('txtFromDocNoHistory', ''); choose('txtToDocNoHistory', ''); choose('cmbSupplierNameHistory', '');
        for (const t of ['grdHistory', 'grdDetail']) { el(t).tHead.innerHTML = ''; el(t).tBodies[0].innerHTML = ''; el(t).tFoot.innerHTML = ''; }
        document.querySelector('input[name=histDate][value=DocDate]').checked = true; message('', false, 'historyMsg');
    }
    function historyDefaults() {
        const days = Number(state.lists.historyFromDays) > 0 ? Number(state.lists.historyFromDays) : 3, d = new Date(); d.setDate(d.getDate() - days);
        choose('FromDateHistory', ymd(d)); choose('ToDateHistory', today());
    }
    // cmbBranchName_Leave :1248 - suppliers reloaded for the ticked branches; no branch clears the supplier combo.
    async function historySuppliersReload() {
        const branches = multiValue('cmbBranchName');
        if (!branches) { fill('cmbSupplierNameHistory', [], 'Id', 'ReferenceName', true); return; }
        await run(null, async () => { const keep = el('cmbSupplierNameHistory').value; fill('cmbSupplierNameHistory', await api('/history-suppliers?branchIds=' + encodeURIComponent(branches)), 'Id', 'ReferenceName', true); choose('cmbSupplierNameHistory', keep); }, 'historyMsg');
    }
    async function refreshHistory(button) {
        // btnRefreshHistory_Click :5220
        await run(button, async () => { const data = await api('/history-refresh'); state.lists.historyBranches = data.historyBranches; multi('cmbBranchName', data.historyBranches || [], historySuppliersReload); }, 'historyMsg');
        await historySuppliersReload();
    }
    async function history(button) {
        await run(button, async () => {
            const branches = multiValue('cmbBranchName'); if (!branches) throw Error('Select branch first');
            const q = new URLSearchParams({ branchIds: branches, dateType: document.querySelector('input[name=histDate]:checked').value });
            if (el('chkFromDateHistory').checked && el('FromDateHistory').value) q.set('fromDate', el('FromDateHistory').value);
            if (el('chkToDateHistory').checked && el('ToDateHistory').value) q.set('toDate', el('ToDateHistory').value);
            for (const [id, key] of [['txtFromDocNoHistory', 'fromDocNo'], ['txtToDocNoHistory', 'toDocNo'], ['cmbSupplierNameHistory', 'supplierId']]) if (parse(el(id).value) > 0) q.set(key, String(parse(el(id).value)));
            state.historyRows = await api('/history?' + q); state.historySelected = 0;
            const t = el('grdHistory'), cols = HISTORY.filter(c => c[2] !== 'branch' || branchColumns());
            for (const x of ['grdHistory', 'grdDetail']) { el(x).tHead.innerHTML = ''; el(x).tBodies[0].innerHTML = ''; el(x).tFoot.innerHTML = ''; }
            if (!state.historyRows.length) { message('0 records', false, 'historyMsg'); return; }
            // Edit, Slip, PartySlip, Voucher at positions 0-3 (frozen), Add Attachment last.
            t.tHead.innerHTML = '<tr><th>Edit</th><th>Slip</th><th>PartySlip</th><th>Voucher</th>' + cols.map(c => `<th>${c[0]}</th>`).join('') + '<th>AddAttachment</th></tr>';
            t.tBodies[0].innerHTML = state.historyRows.map((r, i) => `<tr data-hist="${i}"><td class="c"><button type="button" class="hb" data-hact="Edit">Edit</button></td><td class="c"><button type="button" class="hb" data-hact="Slip">Slip</button></td><td class="c"><button type="button" class="hb" data-hact="PartySlip">PartySlip</button></td><td class="c"><button type="button" class="hb" data-hact="Voucher">Voucher</button></td>`
                + cols.map(c => `<td${['n', 'r', 'a'].includes(c[2]) ? ' class="n"' : ''}>${historyCell(r, c)}</td>`).join('') + '<td class="c"><button type="button" class="hb" data-hact="AddAttachment">Add Attachment</button></td></tr>').join('');
            t.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td></td>' + cols.map(c => '<td>' + (['BillAmount', 'CommAmount'].includes(c[1]) ? esc(fmtA(state.historyRows.reduce((s, r) => s + num(r, c[1]), 0))) : '') + '</td>').join('') + '<td></td></tr>';
            message(state.historyRows.length + ' records', false, 'historyMsg');
            historyDetail(0);
        }, 'historyMsg');
    }
    async function historyDetail(index) {
        const r = state.historyRows[index]; if (!r) return; state.historySelected = index;
        for (const tr of el('grdHistory').tBodies[0].rows) tr.classList.toggle('sel', Number(tr.dataset.hist) === index);
        await run(null, async () => {
            const rows = await api('/history-detail/' + num(r, 'Id') + '?branchIds=' + encodeURIComponent(multiValue('cmbBranchName')));
            const t = el('grdDetail'), cols = HISTORY_DETAIL.filter(c => c[2] !== 'branch' || branchColumns());
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr>' + cols.map(c => `<th>${esc(c[0])}</th>`).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(d => '<tr>' + cols.map(c => `<td${['n', 'r', 'a'].includes(c[2]) ? ' class="n"' : ''}>${historyCell(d, c)}</td>`).join('') + '</tr>').join('');
            const sums = new Set(['ItemQty', 'GrossWeight', 'EBTotalWt', 'WeightCutTotal', 'AdLsWeight', 'NetBillWeight', 'NetStockWeight', 'ItemAmount', 'RateCutAmount', 'CommissionAmount', 'ExpenseAmount', 'FreightAmount', 'BillAmount']);
            t.tFoot.innerHTML = '<tr>' + cols.map(c => '<td>' + (sums.has(c[1]) ? esc(cellFmt(rows.reduce((s, x) => s + num(x, c[1]), 0), c[2])) : '') + '</td>').join('') + '</tr>';
        }, 'historyMsg');
    }
    // grdHistory_ColumnButtonClick :4941
    function historyAction(action, index) {
        const r = state.historyRows[index]; if (!r) return; const id = num(r, 'Id');
        if (action === 'Edit') { run(null, async () => { await reset(); await load(id); }, 'historyMsg'); return; }
        if (action === 'Voucher') { print('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: num(r, 'VoucherHeadId'), documentTypeId: DOC_TYPE }); return; }
        if (action === 'Slip') { print('225-InvRepPurchaseBillDirectWithoutPo.rpt', { id }); return; }
        if (action === 'PartySlip') { print('225A-InvRepPurchaseBillDirectWithoutPo.rpt', { id }); return; }
        if (action === 'AddAttachment') { run(null, () => load(id), 'historyMsg').then(() => { if (state.id === id) attachmentEditor.open(el('btnAttachment')); }); }
    }

    // ------------------------------------------------------------------ multi-select branch combo (UltraCombo CheckedList)
    function multi(id, rows, onChange) {
        const list = el(id + 'List'), text = el(id + 'Text'), host = el(id);
        const current = String(get((rows || []).find(r => num(r, 'BranchId') === Number(state.lists.currentBranchId)), 'BranchId') ?? '');
        list.innerHTML = (rows || []).map((r, i) => `<label><input type="checkbox" value="${esc(get(r, 'BranchId'))}" ${(current ? String(get(r, 'BranchId')) === current : i === 0) ? 'checked' : ''}> ${esc(get(r, 'BranchName'))}</label>`).join('');
        const sync = () => { text.textContent = Array.from(list.querySelectorAll('input:checked')).map(x => x.parentElement.textContent.trim()).join(','); };
        list.onchange = () => { sync(); host.dataset.dirty = '1'; };
        host.querySelector('.fx-multi-btn').onclick = () => { list.hidden = !list.hidden; if (list.hidden && host.dataset.dirty) { host.dataset.dirty = ''; onChange(); } };
        host.onLeave = () => { if (host.dataset.dirty) { host.dataset.dirty = ''; onChange(); } };
        sync();
    }
    const multiValue = id => Array.from(el(id + 'List').querySelectorAll('input:checked')).map(x => x.value).join(',');

    // ------------------------------------------------------------------ tabs / special rights / shortcuts
    function showTab(pane) {
        for (const b of document.querySelectorAll('[data-tabs="main"] button')) { const on = b.dataset.pane === pane; b.classList.toggle('is-active', on); el(b.dataset.pane).classList.toggle('is-active', on); }
        const onHist = pane === 'tabHistory';
        el('btnFooterHistory').querySelector('span').textContent = onHist ? 'Form' : 'History';
        el('btnFooterHistory').querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        // tabControl1_SelectedIndexChanged :4904
        if (onHist) el('FromDateHistory').focus();
    }
    const onHistory = () => el('tabHistory').classList.contains('is-active');
    function showSubTab(pane) { for (const b of document.querySelectorAll('[data-tabs="grids"] button')) { const on = b.dataset.pane === pane; b.classList.toggle('is-active', on); el(b.dataset.pane).classList.toggle('is-active', on); } }
    const subTabIndex = () => Number(document.querySelector('[data-tabs="grids"] button.is-active')?.dataset.index || 0);
    const subTabByIndex = i => document.querySelector(`[data-tabs="grids"] button[data-index="${i}"]`)?.dataset.pane;
    // SpecialRightsImplement :815 - RightId 1 Item Slip, 2 Print Voucher, 7 Party Slip; no rows -> Item Slip checked.
    function specialRights() {
        el('ChkPrintSlip').checked = state.rights.Print === true;   // :754
        if (!window.SpecialRights) return;
        window.SpecialRights.mine(SCREEN_ID).then(rows => {
            if (!rows || !rows.length) { el('ChkPrintSlip').checked = true; return; }
            for (const r of rows) {
                const on = get(r, 'IsActive') === true || get(r, 'IsActive') === 1, id = num(r, 'RightId');
                if (id === 1) el('ChkPrintSlip').checked = on; else if (id === 2) el('ChkBok').checked = on; else if (id === 7) el('chkPartySlip').checked = on;
            }
        }, x => message(PurchaseRequest.error(x), true));
    }
    function focusGrid(grid) { const t = el(GRIDS[grid]?.table || 'grd'); t.closest('.fx-grid-host').focus(); }
    // InvfrmPurchasedirectInvoice_KeyDown_1 :5840
    function keydown(e) {
        if (e.key === 'Escape') { el('dlgLoadOrder').hidden = true; return; }
        const t = e.target;
        // Enter -> SendKeys("{TAB}") for the form's edit controls.
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && t && (t.matches?.('.fx-root input:not([type=checkbox]):not([type=radio]), .fx-root select')) && !t.closest('.fx-grid-host') && !t.classList.contains('dtcombo-input')) {
            e.preventDefault(); const f = Array.from(document.querySelectorAll('.fx-pane.is-active input:not([disabled]):not([type=hidden]), .fx-pane.is-active select:not([disabled]), .fx-pane.is-active .dtcombo-input')).filter(x => x.offsetParent !== null);
            const i = f.indexOf(t); if (i >= 0 && f[i + 1]) f[i + 1].focus(); return;
        }
        if (e.altKey && !e.ctrlKey && (e.key === '1' || e.key === '2')) { e.preventDefault(); if (state.rights.Print === true) (e.key === '1' ? printSlip : printVoucher)(); return; }
        if (!e.ctrlKey || e.altKey) return;
        const k = e.key.toLowerCase(), idx = subTabIndex();
        if (e.key === 'F10') { e.preventDefault(); attachmentEditor.open(el('btnAttachment')); return; }
        if (e.key === 'F5') { e.preventDefault(); el('CmbItemName').focus(); return; }
        // Ctrl+Left: tabControl2 index 0 and the freight grid focused; Ctrl+Right steps 0->1->2->3 (quirk: the indexes are one below the grids' tabs).
        if (e.key === 'ArrowLeft') { e.preventDefault(); showSubTab('tabDetail'); focusGrid('freight'); return; }
        if (e.key === 'ArrowRight') { e.preventDefault(); if (idx <= 2) { showSubTab(subTabByIndex(idx + 1)); focusGrid(GRIDS_BY_INDEX[idx + 1]); } return; }
        // Ctrl+Delete / Ctrl+D act on the grid whose index matches tabControl2.SelectedIndex 0-3 (freight, expenses, journal, empty bags - quirk kept).
        if (e.key === 'Delete' || k === 'd') {
            if (t && t.closest && t.closest('input,select') && !t.closest('.fx-grid-host') && e.key === 'Delete') return;
            e.preventDefault(); const grid = GRIDS_BY_INDEX[idx]; if (!grid) return;
            if (e.key === 'Delete') removeRow(grid, state.current[grid]); else { state.parts[grid].push(BLANK[grid]()); renderGrid(grid); }
            return;
        }
        if (e.key === 'ArrowDown') { e.preventDefault(); (onHistory() ? el('grdHistory') : el('grd')).closest('.fx-grid-host').focus(); return; }
        if (e.key === 'ArrowUp') { e.preventDefault(); (onHistory() ? el('FromDateHistory') : el('CmbOrderNo')).focus(); return; }
        if (e.key === 'Enter') { e.preventDefault(); if (onHistory() && state.historyRows[state.historySelected]) run(null, () => load(num(state.historyRows[state.historySelected], 'Id')), 'historyMsg'); return; }
        const act = {
            n: () => onHistory() ? resetHistory() : newRecord(el('btnNew')),
            r: () => onHistory() ? refreshHistory(el('btnRefreshHistory')) : refresh(el('btnRefresh')),
            s: () => onHistory() ? history(el('btnshow')) : (!el('btnSave').hidden && !el('btnSave').disabled && save(el('btnSave'))),
            u: () => !el('btnUpdate').hidden && !el('btnUpdate').disabled && update(el('btnUpdate')),
            t: () => showTab(onHistory() ? 'tabForm' : 'tabHistory'),
            e: () => { window.location.href = '/purchase'; }
        }[k];
        if (act) { e.preventDefault(); act(); }
    }
    const update = button => save(button);
    // KeyPress filters: OnlytextdecimelFunction, ...WithMinus (txtAddLss), digits only (txtduedays, history doc numbers).
    function keyFilter(e) {
        const mode = e.target?.dataset?.keys; if (!mode || e.ctrlKey || e.metaKey || e.key.length !== 1) return;
        const ok = mode === 'int' ? /\d/.test(e.key) : mode === 'decminus' ? /[\d.\-]/.test(e.key) : /[\d.]/.test(e.key);
        if (!ok) e.preventDefault();
    }
    // CommonServices.CommasApplyWhileTyping (txtQty, txtGrossWeight, txtRate, txtStockWeight).
    function commas(node) {
        const raw = node.value.replace(/,/g, ''); if (!/^-?\d*(\.\d*)?$/.test(raw) || raw === '' || raw === '-') return;
        const [i, f] = raw.split('.'); const grouped = i.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        const v = f !== undefined ? grouped + '.' + f : grouped; if (v !== node.value) { node.value = v; node.setSelectionRange(v.length, v.length); }
    }

    // ------------------------------------------------------------------ wiring
    const attachmentEditor = PurchaseInvoiceAttachments.create({ type: DOC_TYPE, getId: () => state.id, canEdit: () => state.rights[state.id ? 'Update' : 'Save'] === true, message });
    function initEvents() {
        for (const b of document.querySelectorAll('[data-tabs="main"] button')) b.addEventListener('click', () => showTab(b.dataset.pane));
        for (const b of document.querySelectorAll('[data-tabs="grids"] button')) b.addEventListener('click', () => showSubTab(b.dataset.pane));
        el('btnFooterHistory').addEventListener('click', () => showTab(onHistory() ? 'tabForm' : 'tabHistory'));
        el('btnNew').addEventListener('click', () => newRecord(el('btnNew')));
        el('btnRefresh').addEventListener('click', () => refresh(el('btnRefresh')));
        el('btnSave').addEventListener('click', () => save(el('btnSave')));
        el('btnUpdate').addEventListener('click', () => update(el('btnUpdate')));
        el('btnDelete').addEventListener('click', () => remove(el('btnDelete')));
        el('btnAttachment').addEventListener('click', () => attachmentEditor.open(el('btnAttachment')));
        el('btnSlip').addEventListener('click', () => run(el('btnSlip'), printSlip));
        el('btn225PartySlip').addEventListener('click', () => run(el('btn225PartySlip'), printPartySlip));
        el('btnPrint').addEventListener('click', () => run(el('btnPrint'), printVoucher));
        el('btnPurchaseOrderLoader').addEventListener('click', () => openLoader(el('btnPurchaseOrderLoader')));
        el('BtnSpecialRights').addEventListener('click', () => { if (window.SpecialRights) window.SpecialRights.open(SCREEN_ID); });
        // Header events.
        el('DocDate').addEventListener('change', dueDaysCalculate);
        el('txtduedays').addEventListener('input', dueDaysCalculate);
        el('duedate').addEventListener('change', dueDateChanged);
        el('combpttrm').addEventListener('change', () => paymentTermChanged(true));
        el('comsupplier').addEventListener('change', () => { supplierGlUpdate(); recalc(); });
        // combCommType TextChanged, combcommAgent TextChanged, txtcommrate TextChanged, brokery Leave/TextChanged -> TotalCommissionAmount / TotalBrokeryAmount / BillAmount.
        // combcommUOM has no handler on the desktop form (its Leave method is never attached): the amount follows on the next event.
        for (const id of ['combCommType', 'combcommAgent', 'CmbBrokeryAc', 'CmbBrokeryType', 'CmbBrokeryRateUom']) el(id).addEventListener('change', recalc);
        for (const id of ['txtcommrate', 'txtBrokeryRate']) el(id).addEventListener('input', recalc);
        // Detail editor events.
        el('rdSearchByName').addEventListener('change', itemNameBind); el('rdSearchByCode').addEventListener('change', itemNameBind);
        el('CmbItemName').addEventListener('change', () => packUomBind(Number(el('CmbItemName').value || 0)));
        el('CmbPackingType').addEventListener('change', () => run(null, emptyBagsFromOrder));
        el('CmbOrderNo').addEventListener('change', () => run(null, emptyBagsFromOrder));
        el('comPackUOM').addEventListener('change', () => calculate('ItemUOMId'));
        el('comRateUOM').addEventListener('change', () => calculate('UomScheduleIdRate'));
        for (const [id, key] of Object.entries({ txtQty: 'ItemQty', txtGrossWeight: 'GrossWeight', txtEmptybagsUnit: 'EBWeight', txtEmptyBagsTotal: 'EBTotalWt', txtwtcut: 'WeightCut', txtWeightCutTotal: 'WeightCutTotal', txtAddLss: 'AdLsWeight', txtRate: 'ItemRate', txtratecut: 'RateCut' }))
            el(id).addEventListener('input', () => { if (el(id).dataset.commas) commas(el(id)); calculate(key); });
        el('txtvehicleno').addEventListener('input', () => { const n = el('txtvehicleno'), p = n.selectionStart; n.value = n.value.toUpperCase(); n.setSelectionRange(p, p); });
        el('btnAdd').addEventListener('click', () => addLine(el('btnAdd')));
        el('btnUpdateDetail').addEventListener('click', () => updateLine(el('btnUpdateDetail')));
        el('btnCancelUpdateDetial').addEventListener('click', cancelLine);
        document.addEventListener('keypress', keyFilter);
        // Detail grid.
        el('grd').addEventListener('click', e => { const tr = e.target.closest('tr[data-row]'); if (tr) { state.current.grd = Number(tr.dataset.row); for (const x of el('grd').tBodies[0].rows) x.classList.toggle('sel', x === tr); }
            const b = e.target.closest('[data-del],[data-add]'); if (b) detailButton(b.dataset.del ? 'delete' : 'add', Number(b.dataset.row)); });
        el('grd').addEventListener('dblclick', e => { const tr = e.target.closest('tr[data-row]'); if (tr && !e.target.closest('button,a')) editLine(Number(tr.dataset.row)); });
        // Supplement grids.
        for (const [grid, def] of Object.entries(GRIDS)) {
            const t = el(def.table);
            t.addEventListener('change', e => { const x = e.target; if (!x.dataset.grid) return; cellUpdated(x.dataset.grid, Number(x.dataset.row), x.dataset.field, x.value); });
            t.addEventListener('click', e => {
                const tr = e.target.closest('tr[data-row]'); if (tr) { state.current[grid] = Number(tr.dataset.row); for (const x of t.tBodies[0].rows) x.classList.toggle('sel', x === tr); }
                const del = e.target.closest('[data-del]'), add = e.target.closest('[data-add]');
                if (del) removeRow(grid, Number(del.dataset.row)); if (add) addRow(grid);
            });
            t.addEventListener('focusin', e => { const tr = e.target.closest('tr[data-row]'); if (tr) state.current[grid] = Number(tr.dataset.row); });
            // grdFreight_KeyDown :6060 / grdGLedger_KeyDown :6092 - F1 on the account cell opens the searchable list.
            t.addEventListener('keydown', e => { if (e.key === 'F1' && e.target.dataset?.field && ['SupplierCustomerId', 'AccountId'].includes(e.target.dataset.field)) { e.preventDefault(); const w = e.target.closest('td')?.querySelector('.dtcombo-input, .dtcombo-wrap'); (w || e.target).click?.(); (w || e.target).focus?.(); } });
        }
        // Loader.
        el('ldrClose').addEventListener('click', () => { el('dlgLoadOrder').hidden = true; gridIds = ''; });
        el('ldrSearch').addEventListener('click', () => searchOrders(el('ldrSearch')));
        el('ldrLoad').addEventListener('click', () => loadOrders(el('ldrLoad')));
        el('ldrGrid').addEventListener('change', e => { if (e.target.id === 'ldrAll') for (const x of document.querySelectorAll('#ldrGrid [data-order-id]')) x.checked = e.target.checked; });
        // History.
        el('btnshow').addEventListener('click', () => history(el('btnshow')));
        el('btnNewHistory').addEventListener('click', resetHistory);
        el('btnRefreshHistory').addEventListener('click', () => refreshHistory(el('btnRefreshHistory')));
        el('grdHistory').addEventListener('click', e => {
            const tr = e.target.closest('tr[data-hist]'); if (!tr) return; const index = Number(tr.dataset.hist), b = e.target.closest('[data-hact]');
            if (e.target.closest('[data-hload]')) { e.preventDefault(); run(null, () => load(num(state.historyRows[index], 'Id')), 'historyMsg'); return; }
            // grdHistory_LinkClicked :4912 - GetNoofAttachmentsByRefDocumentTypeID.
            if (e.target.closest('[data-hatt]')) { e.preventDefault(); PurchaseInvoiceAttachments.view({ type: DOC_TYPE, id: num(state.historyRows[index], 'Id'), message: t => message(t, true, 'historyMsg') }); return; }
            if (b) historyAction(b.dataset.hact, index); else historyDetail(index);
        });
        el('grdHistory').addEventListener('dblclick', e => { const tr = e.target.closest('tr[data-hist]'); if (tr && !e.target.closest('button,a')) run(null, () => load(num(state.historyRows[Number(tr.dataset.hist)], 'Id')), 'historyMsg'); });
        document.addEventListener('mousedown', e => { const host = el('cmbBranchName'); if (!host.contains(e.target) && !el('cmbBranchNameList').hidden) { el('cmbBranchNameList').hidden = true; host.onLeave?.(); } });
        document.addEventListener('keydown', keydown);
    }
    document.addEventListener('DOMContentLoaded', async () => {
        initEvents();
        await run(null, async () => {
            bind(await api('/dropdowns'));
            historyDefaults(); specialRights();
            const id = new URLSearchParams(window.location.search).get('id');
            await reset();
            if (id) await load(id);
        });
    });
    return { newRecord, refresh, save, remove, history, openLoader };
})();
