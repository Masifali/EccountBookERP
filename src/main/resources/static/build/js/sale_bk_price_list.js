/* ============================================================================================
 * Screen 764 PriceList - Architecture.WinApp.WholeSale.frmItemPricingSchedule (.cs)
 * Page: templates/sale/bk/price_list.html (route /sale/reports/price-list).
 *   frmItemPricingSchedule_Load :123   rights -> Save / Update enabled; priceTypeId 6 (7 only for Tag frmPosRetailPricing); ItemFill, ItemTypeFill, ItemCategoryFill;
 *                                     grid dtGrid columns ItemId, ItemName, EffectedDate, BranchId, ScheduleDesc, ItemPrice, NewPrice (all string columns)
 *   dataGridSetting :158              ItemId, ScheduleDesc, EffectedDate, BranchId hidden; ItemName 400 no edit; ItemPrice no edit right aligned; Delete 35 image button
 *   btnAdd_Click :586                 GetPreviousPricesByItemId(cmbItem.Value) when the value is not empty
 *   btnSearch_Click :550 / GetFilteredItems :363
 *                                     category > 0 or type > 0 -> GetPreviousPricesByItemId("", category, type)
 *   GetPreviousPricesByItemId :320    rows -> grid cleared and refilled (BranchId 0, ScheduleDesc = txtDescription.Text, NewPrice 0); no rows -> grid emptied
 *   Insert :378                       "Grid record not found" / confirm "Are you sure to Update?|Save?" / "Item Required" / "Check Item Price This Should be Not Zero"
 *                                     -> ItemPricingSchedule.Save -> "Update Successfully|Save Successfully" -> Reset
 *   tabControl1_SelectedIndexChanged :603  tab 1 -> FormHistory (grid is left as it was when there are no rows)
 *   DataGridHistory_ColumnButtonClick :618 / GetByItemId :510  Edit: more than one grid row -> "You cannot update price of more than 1 items at time!"
 *   datagrid_ColumnButtonClick :633   Delete only while btnsave is visible and enabled, else "You Can not Delete Detail Record In Update Mode... "
 *   Reset :562                        combos / description cleared, EffectedDate = today, grid cleared, Save visible, Update hidden, RecId 0, Add / Search enabled, cmbItem focus
 *   KeyDown :271                      Enter = Tab; Ctrl+S save (tab 0); Ctrl+N new; Ctrl+T switch tab; Ctrl+E / Esc close; Ctrl+U update
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/bk/price-list';
    var st = { canSave: false, canUpdate: false, recId: 0, rows: [], tab: 0, updateMode: false };
    var hist = new S.Grid({ tableId: 'history', gridId: 'gridH', navId: 'navH', navTextId: 'navTextH', headerLines: 1, noFilter: true, noTotal: true,
        onButton: function (row, col) { if (col.button === 'Edit') getByItemId(row); } });

    function say(m) { w.alert(m); }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function nowTime() { var t = new Date(); return pad(t.getHours()) + ':' + pad(t.getMinutes()) + ':' + pad(t.getSeconds()); }
    function esc(s) { return String(s == null ? '' : s).replace(/[&<>"]/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' }[c]; }); }

    // ------------------------------------------------------------------ detail grid (datagrid): ItemName 400 | ItemPrice | NewPrice (editable) | Delete 35
    function renderDetail() {
        var t = el('detail'), cg = t.querySelector('colgroup'), th = t.tHead, tb = t.tBodies[0];
        cg.innerHTML = ''; th.innerHTML = ''; tb.innerHTML = '';
        if (!st.rows.length) return;                                   // ClearStructure / DataSource = null: no columns either
        [400, 100, 100, 35].forEach(function (x) { var c = d.createElement('col'); c.style.width = x + 'px'; cg.appendChild(c); });
        var hr = th.insertRow();
        ['ItemName', 'ItemPrice', 'NewPrice', 'Delete'].forEach(function (h) { var c = d.createElement('th'); c.textContent = h; hr.appendChild(c); });
        st.rows.forEach(function (r, n) {
            var tr = tb.insertRow(); tr.className = 'r' + (n % 2 ? ' alt' : '');
            tr.insertCell().textContent = r.ItemName == null ? '' : r.ItemName;
            var cp = tr.insertCell(); cp.className = 'num'; cp.textContent = r.ItemPrice == null ? '' : String(r.ItemPrice);
            var cn = tr.insertCell(), inp = d.createElement('input');
            inp.type = 'text'; inp.value = r.NewPrice == null ? '' : r.NewPrice; inp.setAttribute('aria-label', 'NewPrice'); inp.style.cssText = 'width:100%;box-sizing:border-box;border:0;background:transparent;font:inherit;text-align:left;';
            inp.oninput = function () { r.NewPrice = inp.value; };
            cn.appendChild(inp);
            var cd = tr.insertCell(); cd.className = 'ctr';
            var b = d.createElement('button'); b.type = 'button'; b.className = 'cellbtn'; b.title = 'Delete'; b.textContent = '✖'; b.style.color = '#c0392b';
            b.onclick = function () { delRow(n); };
            cd.appendChild(b);
        });
    }
    function delRow(n) {                                                // datagrid_ColumnButtonClick
        if (saveVisible() && saveEnabled()) { st.rows.splice(n, 1); renderDetail(); }
        else say('You Can not Delete Detail Record In Update Mode... ');
    }
    function saveVisible() { return !el('btnsave').hidden; }
    function saveEnabled() { return !el('btnsave').disabled; }
    function updateVisible() { return !el('btnUpdate').hidden; }
    function updateEnabled() { return !el('btnUpdate').disabled; }

    // ------------------------------------------------------------------ Load
    function init() {
        return A.getJson(API + '/init').then(function (data) {
            st.canSave = !!data.canSave; st.canUpdate = !!data.canUpdate;
            el('btnsave').disabled = !st.canSave; el('btnUpdate').disabled = !st.canUpdate;
            S.fill(el('cmbItem'), data.items, false);
            S.fill(el('cmbItemType'), data.types, false);
            S.fill(el('cmbItemCategory'), data.categories, false);
        }).catch(function (e) { say(e.message); });
    }

    // ------------------------------------------------------------------ GetPreviousPricesByItemId
    function getPreviousPrices(ids, cat, typ) {
        return A.getJson(API + '/previous?' + S.qs({ ids: ids, categoryId: cat || 0, typeId: typ || 0 })).then(function (rows) {
            if (rows && rows.length) {
                var desc = el('txtDescription').value;
                st.rows = rows.map(function (r) { return { ItemId: r.ItemId, ItemName: r.ItemName, EffectedDate: r.EffectedDate, BranchId: 0, ScheduleDesc: desc, ItemPrice: r.ItemPrice, NewPrice: 0 }; });
            } else st.rows = [];                                        // datagrid.DataSource = null; dtGrid.Rows.Clear()
            renderDetail();
        }).catch(function (e) { say(e.message); });
    }
    function btnAdd() { var ids = S.selInt('cmbItem') ? String(S.selInt('cmbItem')) : ''; if (ids !== '') return getPreviousPrices(ids); }
    function btnSearch() {
        var c = S.selInt('cmbItemCategory'), t = S.selInt('cmbItemType');
        if (c > 0 || t > 0) return getPreviousPrices('', c, t);
    }

    // ------------------------------------------------------------------ Reset / Insert
    function reset() {
        S.setIndex('cmbItem', 0); S.setIndex('cmbItemCategory', 0); S.setIndex('cmbItemType', 0);
        el('txtDescription').value = ''; el('txtEffectedDate').value = A.today();
        st.rows = []; renderDetail();
        el('btnsave').hidden = false; el('btnUpdate').hidden = true; st.recId = 0;
        el('btnAdd').disabled = false; el('btnSearch').disabled = false;
        S.focus('cmbItem');
    }
    function insert() {
        if (!st.rows.length) { say('Grid record not found'); return; }
        if (!w.confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { recId: st.recId, description: el('txtDescription').value, effectedDate: el('txtEffectedDate').value, effectedTime: el('datEffectedTime').value,
            rows: st.rows.map(function (r) { return { ItemId: r.ItemId, BranchId: r.BranchId, NewPrice: r.NewPrice }; }) };
        return S.post(API + '/save', body).then(function (res) { say(res.message); reset(); }).catch(function (e) { say(e.message); });
    }

    // ------------------------------------------------------------------ history tab
    function histCols() {
        return [{ key: 'Id', caption: 'Id', hidden: true }, { key: 'ItemId', caption: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName', width: 300 },
            { key: 'EffectedDate', caption: 'EffectedDate', date: 'dd-MMM-yy hh:mm tt', width: 140 }, { key: 'ItemPrice', caption: 'ItemPrice', align: 'r', width: 100 },
            { key: 'EntryUserName', caption: 'EntryUserName', width: 150 }, { key: '_Edit', caption: 'Edit', width: 50, button: 'Edit' }];
    }
    function formHistory() {
        return A.getJson(API + '/history').then(function (rows) { if (rows && rows.length) hist.setData(histCols(), rows); }).catch(function (e) { say(e.message); });
    }
    function getByItemId(row) {
        if (st.rows.length > 0) { say('You cannot update price of more than 1 items at time!'); return; }
        return A.getJson(API + '/edit?' + S.qs({ itemId: row.ItemId })).then(function () { }).catch(function (e) { st.rows = []; renderDetail(); say(e.message); });
    }
    function selectTab(n) {
        st.tab = n;
        el('page1').hidden = n !== 0; el('page2').hidden = n !== 1;
        el('tab1').classList.toggle('on', n === 0); el('tab2').classList.toggle('on', n === 1);
        if (n === 1) formHistory();
    }

    // ------------------------------------------------------------------ keys
    var order = ['cmbItemCategory', 'cmbItemType', 'btnSearch', 'txtDescription', 'txtEffectedDate', 'datEffectedTime', 'cmbItem', 'btnAdd'];
    d.addEventListener('keydown', function (e) {
        if (e.key !== 'Enter' || e.ctrlKey || e.altKey || A.comboOpen()) return;
        var t = e.target; if (t.tagName === 'BUTTON' || t.closest('.grid')) return;
        var cur = -1;
        order.forEach(function (id, i) { var f = A.focusable(id); if (f === t || (f && f.contains && f.contains(t))) cur = i; });
        if (cur < 0) return;
        e.preventDefault(); S.focus(order[(cur + 1) % order.length]);
    });
    function closeForm() { if (w.history.length > 1) w.history.back(); else w.location.href = '/dashboard'; }
    d.addEventListener('keydown', function (e) {
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 's' && saveVisible() && saveEnabled() && st.tab === 0) { e.preventDefault(); insert(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 't') { e.preventDefault(); selectTab(st.tab === 1 ? 0 : 1); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'u' && updateVisible() && updateEnabled()) { e.preventDefault(); insert(); }
    });
    d.addEventListener('keydown', function (e) { if (e.key === 'Escape' && !A.comboOpen()) closeForm(); });

    el('btnnew').addEventListener('click', reset);
    el('btnsave').addEventListener('click', insert);
    el('btnUpdate').addEventListener('click', insert);
    el('btnAdd').addEventListener('click', btnAdd);
    el('btnSearch').addEventListener('click', btnSearch);
    el('tab1').addEventListener('click', function () { selectTab(0); });
    el('tab2').addEventListener('click', function () { selectTab(1); });

    el('txtEffectedDate').value = A.today();
    el('datEffectedTime').value = nowTime();
    renderDetail();
    init();
}(window, document));
