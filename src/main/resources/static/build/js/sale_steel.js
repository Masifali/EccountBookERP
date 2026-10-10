/*
 * Shared page helpers of the Sale Steel screens (Architecture.WinApp.Steel.Sale.* / Steel.Reports.SalesReports.*).
 * Builds on window.SE (sale_engr.js: api, MessageBox, formatting, tabs, grid, pop-up) and XCombo (acc_c_xcombo.js).
 * window.SS: combo helpers, grids whose columns come from the procedure (RetrieveStructure), attachments dialog, Define Vehicle dialog.
 */
(function (w, d) {
    'use strict';
    var SS = {}, $ = function (id) { return d.getElementById(id); };

    /* ------------------------------------------------------------------ combos */
    SS.selectRow = function (c, i) { var r = c.rows()[i]; if (r) c.setValue(r[c.el.getAttribute('data-vk') || 'Id']); };
    /* combo.Text = value: select the row whose text column equals the text, else nothing */
    SS.byText = function (c, key, text) {
        var rows = c.rows(); for (var i = 0; i < rows.length; i++) if (String(rows[i][key]) === String(text)) { c.setValue(rows[i][c.el.getAttribute('data-vk') || 'Id']); return true; }
        c.clear(); return false;
    };
    SS.dis = function (c, on) { c.el.disabled = on; var x = c.el.__dtcombo; if (x && x.syncFromSelect) x.syncFromSelect(); };
    SS.val = function (id) { return ($(id).value || '').trim(); };
    SS.fail = function (e) { return SE.dbError(e); };
    SS.show = function (id, on) { var e = $(id); if (e) e.style.display = on ? '' : 'none'; };
    SS.on = function (id, ev, fn) { var e = $(id); if (e) e.addEventListener(ev, fn); };

    /* ------------------------------------------------------------------ procedure-driven grid columns (GridEX.RetrieveStructure) */
    /* server grid: { cols:[names in select order], rows:[...] }.  opt: hide[], cap{}, w{}, f{}, sum[], link[], btnFirst:[{k,t,w,btn}] (columns inserted at position 0..), edit[] */
    var ISO = /^\d{4}-\d\d-\d\d(?:[T ]\d\d:\d\d(?::\d\d)?)?/;
    SS.cols = function (grid, opt) {
        opt = opt || {};
        var cols = [], rows = (grid && grid.rows) || [], names = (grid && grid.cols) || [];
        (opt.btnFirst || []).forEach(function (b) { cols.push({ k: b.k, t: b.t || b.k, w: b.w || 50, btn: b.btn || b.k }); });
        names.forEach(function (k) {
            var h = (opt.hide || []).indexOf(k) >= 0;
            var sample = null; for (var i = 0; i < rows.length; i++) if (rows[i][k] != null && rows[i][k] !== '') { sample = rows[i][k]; break; }
            var c = { k: k, t: (opt.cap && opt.cap[k]) || k, hide: h };
            var f = opt.f && opt.f[k];
            if (f) c.f = f;
            else if (typeof sample === 'number') c.cls = 'num', c.render = function (v) { return v == null ? '' : String(v); };
            else if (typeof sample === 'string' && ISO.test(sample)) c.render = function (v) { var p = SE.parts(v); if (!p) return ''; return (p.h || p.i || p.s) ? SE.dMonYTime(v) : SE.dMonY(v); };
            else if (typeof sample === 'boolean') c.f = 'chk';
            c.w = (opt.w && opt.w[k]) || Math.max(60, Math.min(260, String(c.t).length * 8 + 24));
            if (opt.sum && opt.sum.indexOf(k) >= 0) c.sum = true;
            if (opt.link && opt.link.indexOf(k) >= 0) c.link = true;
            if (opt.edit && opt.edit.indexOf(k) >= 0) c.edit = true;
            cols.push(c);
        });
        return cols;
    };
    /* a grid whose columns follow the data each time it is filled */
    SS.autoGrid = function (id, opt, spec) {
        spec = spec || {};
        var g = null, lastCols = null;
        function build(data) {
            spec.cols = SS.cols(data, opt);
            g = SE.grid(id, spec);
            return g;
        }
        var api = {
            fill: function (data) {
                data = data || { cols: [], rows: [] };
                var key = (data.cols || []).join('|');
                if (!g || key !== lastCols) { lastCols = key; build(data); }
                g.setRows(data.rows || []);
                return api;
            },
            grid: function () { return g; },
            rows: function () { return g ? g.rows() : []; },
            cur: function () { return g ? g.cur() : null; },
            clear: function () { if (g) g.clear(); return api; }
        };
        build({ cols: [], rows: [] });
        return api;
    };

    /* ------------------------------------------------------------------ attachments (the AT.Show dialog: list, open, remove, add) */
    /* S: {files:[], removed:[], existing:[]}; getId(): the document id of the open record (0 for a new one) */
    SS.attachmentDialog = function (S, API, getId) {
        var id = getId();
        var rows = (S.existing || []).filter(function (r) { return S.removed.indexOf(r.Id) < 0; });
        render(rows, id, false);
        function render(rows, gpId, readOnly) {
            var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>Attachment</th><th style="width:90px">Action</th></tr></thead><tbody>';
            rows.forEach(function (r) {
                h += '<tr><td>' + SE.esc(r.Attachment) + '</td><td>' + (gpId > 0 ? '<a class="lnk" href="' + API + '/' + gpId + '/attachments/' + r.Id + '" target="_blank">Open</a>' : '') +
                     (readOnly ? '' : ' <a class="lnk" data-rm="' + r.Id + '" style="cursor:pointer">Remove</a>') + '</td></tr>';
            });
            S.files.forEach(function (f, i) { if (!readOnly) h += '<tr><td>' + SE.esc(f.name) + ' (new)</td><td><a class="lnk" data-nf="' + i + '" style="cursor:pointer">Remove</a></td></tr>'; });
            h += '</tbody></table></div>';
            if (!readOnly) h += '<div style="padding:6px"><input type="file" id="atFile" multiple> <span class="se-note">Up to 5 MB each. Files are stored when the document is saved.</span></div>';
            var pop = SE.pop('Attachments', h, [{ t: 'Close' }]);
            pop.open();
            pop.body.addEventListener('click', function (e) {
                var rm = e.target.getAttribute && e.target.getAttribute('data-rm'); var nf = e.target.getAttribute && e.target.getAttribute('data-nf');
                if (rm) { S.removed.push(+rm); pop.close(); SS.attachmentDialog(S, API, getId); }
                if (nf !== null && nf !== undefined && e.target.hasAttribute('data-nf')) { S.files.splice(+nf, 1); pop.close(); SS.attachmentDialog(S, API, getId); }
            });
            var fi = pop.body.querySelector('#atFile');
            if (fi) fi.addEventListener('change', function () {
                var list = Array.prototype.slice.call(fi.files), left = list.length;
                if (!left) return;
                list.forEach(function (f) {
                    if (f.size > 5 * 1024 * 1024) { SE.alert(f.name + ' exceeds 5 MB'); left--; return; }
                    var fr = new FileReader();
                    fr.onload = function () { S.files.push({ name: f.name, base64: String(fr.result).split(',')[1] || '' }); if (--left <= 0) { pop.close(); SS.attachmentDialog(S, API, getId); } };
                    fr.readAsDataURL(f);
                });
            });
        }
    };
    /* grdhistory_LinkClicked NoOfAttachments: read-only list of a stored document's attachments */
    SS.showAttachments = function (API, docId) {
        return SE.api(API + '/' + docId + '/attachments').then(function (rows) {
            var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>Attachment</th><th style="width:90px">Action</th></tr></thead><tbody>';
            rows.forEach(function (r) { h += '<tr><td>' + SE.esc(r.Attachment) + '</td><td><a class="lnk" href="' + API + '/' + docId + '/attachments/' + r.Id + '" target="_blank">Open</a></td></tr>'; });
            h += '</tbody></table></div>';
            SE.pop('Attachments', h, [{ t: 'Close' }]).open();
        }).catch(SS.fail);
    };

    /* ------------------------------------------------------------------ Define Vehicle (the VehicleType form) */
    SS.defineVehicle = function (API, cbVehicle) {
        var h = '<div style="padding:6px">Vehicle Description <input id="vtDesc" class="f" style="position:static;width:260px;height:23px"> <span class="se-note">Record Save / Update as the VehicleType form</span></div><div class="dgrid" id="vtGrid" style="max-height:40vh;height:260px"></div>';
        var editId = 0, grid;
        var pop = SE.pop('Define Vehicle', h, [
            { t: 'New', fn: function () { editId = 0; pop.body.querySelector('#vtDesc').value = ''; pop.body.querySelector('#vtDesc').focus(); } },
            { t: 'Save', fn: function () {
                var desc = pop.body.querySelector('#vtDesc').value.trim(); if (!desc) return;
                SE.ask(editId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                    if (!yes) return;
                    SE.api(API + '/vehicle-type', { method: 'POST', body: { id: editId, description: desc } }).then(function (r) {
                        SE.alert(editId > 0 ? 'Record Update Successfully' : 'Record Save Successfully'); editId = 0; pop.body.querySelector('#vtDesc').value = '';
                        grid.setRows(r.rows); cbVehicle.setData(r.rows);
                    }).catch(SS.fail);
                });
            } }, { t: 'Close' }]);
        pop.open();
        grid = SE.grid(pop.body.querySelector('#vtGrid'), { footer: false, onDbl: function (r) { editId = r.Id; pop.body.querySelector('#vtDesc').value = r.VehicleDescription; },
            cols: [{ k: 'Id', hide: true }, { k: 'VehicleDescription', t: 'VehicleType', w: 300 }] });
        grid.setRows(cbVehicle.rows());
    };

    /* Enter moves to the next control (SendKeys.Send("{TAB}")) */
    SS.enterTab = function (e) {
        var t = e.target;
        if (e.key === 'Enter' && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio') {
            var f = Array.prototype.filter.call(d.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'),
                function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
    };
    SS.visible = function (id) { var b = $(id); return b && b.style.display !== 'none' && !b.disabled; };


    /* ------------------------------------------------------------------ modal form host (Form.ShowDialog) */
    var cssDone = false;
    SS.modal = function (title, html, w, h, onClose) {
        if (!cssDone) {
            cssDone = true;
            var st = d.createElement('style');
            st.textContent = '.ss-modal{position:fixed;inset:0;background:rgba(0,0,0,.35);z-index:3000;overflow:auto;display:flex;align-items:flex-start;justify-content:center;padding:16px}' +
                '.ss-modal>div{background:#fff;border:1px solid #6b7c8c;box-shadow:0 6px 24px rgba(0,0,0,.35);margin:auto 0}' +
                '.ss-modal .ssh{background:#008080;color:#fff;padding:4px 8px;display:flex;justify-content:space-between;font-weight:600}' +
                '.ss-modal .ssh span+span{cursor:pointer}.ss-modal .dform{background:#f0f0f0}';
            d.head.appendChild(st);
        }
        var ov = d.createElement('div'); ov.className = 'ss-modal';
        ov.innerHTML = '<div style="width:' + w + 'px;max-width:100%"><div class="ssh"><span></span><span data-x>&#10005;</span></div><div class="ssb" style="overflow:auto"><div class="dform" style="position:relative;width:' + w + 'px;height:' + h + 'px">' + html + '</div></div></div>';
        ov.querySelector('.ssh span').textContent = title;
        d.body.appendChild(ov);
        var api = { el: ov, q: function (s) { return ov.querySelector(s); },
            close: function () { if (ov.parentNode) ov.parentNode.removeChild(ov); if (onClose) { var f = onClose; onClose = null; f(); } } };
        ov.querySelector('[data-x]').onclick = api.close;
        return api;
    };

    /* ------------------------------------------------------------------ frmLoadSaleOrder_St (the Sale Order loader of the Steel Delivery Order) */
    var LD_HTML = "<div class=\"dpan\" id=\"ld_panel7\" style=\"position:absolute;left:0px;top:0px;width:1082px;height:661px;\"> <div class=\"dpan\" id=\"ld_panel1\" style=\"position:absolute;left:0px;top:0px;width:1082px;height:30px;background:#008080;\"> <span class=\"dl white\" id=\"ld_label23\" style=\"left:3px;top:6px;font-weight:bold;\">Sale Order Load</span> </div> <div class=\"dpan\" id=\"ld_panel8\" style=\"position:absolute;left:0px;top:30px;width:1082px;height:67px;background:#ffffff;\"> <input class=\"f\" type=\"date\" id=\"ld_FromDate\" style=\"left:75px;top:5px;width:110px;height:23px;\"> <input class=\"f\" type=\"date\" id=\"ld_Todate\" style=\"left:75px;top:35px;width:110px;height:23px;\"> <select class=\"f\" id=\"ld_combsuppname\" style=\"left:286px;top:3px;width:351px;height:26px;\"></select> <select class=\"f\" id=\"ld_combitem\" style=\"left:286px;top:33px;width:351px;height:26px;\"></select> <button type=\"button\" class=\"dbtn\" id=\"ld_btngrnlod\" style=\"left:643px;top:33px;width:71px;height:26px;background:#008080;color:#ffffff;\">Search</button> <button type=\"button\" class=\"dbtn\" id=\"ld_btnLoadOnInvoice\" style=\"left:714px;top:33px;width:71px;height:26px;background:#008080;color:#ffffff;\">Load</button> <span class=\"dl\" id=\"ld_label2\" style=\"left:4px;top:39px;font-weight:bold;\">To Date</span> <span class=\"dl\" id=\"ld_label3\" style=\"left:4px;top:9px;font-weight:bold;\">From Date</span> <span class=\"dl\" id=\"ld_label37\" style=\"left:191px;top:39px;font-weight:bold;\">Item Name</span> <span class=\"dl\" id=\"ld_label1\" style=\"left:191px;top:9px;font-weight:bold;\">CustomerName</span> </div> <div class=\"dpan\" id=\"ld_panel9\" style=\"position:absolute;left:0px;top:97px;width:1082px;height:564px;\"> <div class=\"dtc\" id=\"ld_tabControl1\" data-align=\"\" style=\"position:absolute;left:0px;top:0px;width:1082px;height:564px;\"> <div class=\"dsubtabs\"><span class=\"tab on\" data-tab=\"ld_tabPage1\">Main Detail</span><span class=\"tab\" data-tab=\"ld_tabPage2\">Main</span></div> <div class=\"tpane\" id=\"ld_tabPage1\" style=\"position:relative;height:538px;\"> <div class=\"dpan\" id=\"ld_panel10\" style=\"position:absolute;left:3px;top:3px;width:1068px;height:307px;\"> <div class=\"dpan\" id=\"ld_panel5\" style=\"position:absolute;left:0px;top:0px;width:1068px;height:30px;background:#008080;\"> </div> <div class=\"dgrid\" id=\"ld_grd\" tabindex=\"0\" style=\"position:absolute;left:0px;top:30px;width:1068px;height:277px;\"></div> </div> <div class=\"dpan\" id=\"ld_panel11\" style=\"position:absolute;left:3px;top:310px;width:1068px;height:225px;\"> <div class=\"dgrid\" id=\"ld_grdDetail\" tabindex=\"0\" style=\"position:absolute;left:0px;top:29px;width:1068px;height:196px;\"></div> <div class=\"dpan\" id=\"ld_panel6\" style=\"position:absolute;left:0px;top:0px;width:1068px;height:29px;background:#008080;\"> </div> </div> </div> <div class=\"tpane\" id=\"ld_tabPage2\" style=\"position:relative;display:none;height:538px;\"> <div class=\"dpan\" id=\"ld_panel2\" style=\"position:absolute;left:3px;top:3px;width:1068px;height:30px;background:#008080;\"> </div> <div class=\"dgrid\" id=\"ld_GrdSecondView\" tabindex=\"0\" style=\"position:absolute;left:3px;top:33px;width:1068px;height:502px;\"></div> </div> </div> </div> </div>";
    function hashNo(v) { var n = Number(v); if (!isFinite(n)) return ''; var a = Math.round(Math.abs(n)); if (a === 0) return ''; return (n < 0 ? '-' : '') + a.toLocaleString('en-US'); }      // "#,#"
    function zeroComma(v) { var n = Number(v); if (!isFinite(n)) return ''; var a = Math.round(Math.abs(n)); return (n < 0 && a !== 0 ? '-' : '') + a.toLocaleString('en-US'); }                     // "0,0"
    var HASH = ['OrderQty', 'OrderWeight', 'DispatchedWeight', 'BalWeight'], ZERO = ['OrderQTY', 'OrderWeight', 'DispatchedWeight', 'BalWeight'];
    function numCol(k, w, fn) { return { k: k, t: k, w: w, f: 'n0', sum: true, render: fn }; }
    /*
     * o: { API, mainDetail, orderIds, orderMainIds, documentTypeId }.  Resolves to the list of ids the Load button collected (dtGDNS), or null when the form was closed.
     * API serves /loader/combos, /loader/pending, /loader/detail.
     */
    SS.loadSaleOrder = function (o) {
        return new Promise(function (resolve) {
            var result = null, m = SS.modal('Sale Order Load', LD_HTML, 1082, 661, function () { resolve(result); });
            var Q = function (id) { return m.q('#ld_' + id); };
            var cb = {}, G = {}, headerIds = '';
            var tabs = SE.tabs(Q('tabControl1'));
            tabs.show('ld_tabPage1', !!o.mainDetail); tabs.show('ld_tabPage2', !o.mainDetail);
            tabs.select(o.mainDetail ? 'ld_tabPage1' : 'ld_tabPage2');
            cb.cust = XCombo(Q('combsuppname'), { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420 });
            cb.item = XCombo(Q('combitem'), { columns: [{ key: 'ItemName', caption: 'Item Name' }], textKey: 'ItemName', popupWidth: 520 });
            function sel() { return { k: '_sel', t: '', w: 34, sel: true }; }
            G.main = SE.grid(Q('grd'), { footer: true, onSel: function (r) { if (r) detailBind(String(r.OrderId), o.orderIds || ''); else G.det.setRows([]); }, onCheck: function () { checkedHeaderIds(); detailBind(headerIds, o.orderIds || '').then(checkAllDetail); },
                cols: [sel(), { k: 'OrderId', hide: true }, { k: 'DocumentType', t: 'DocumentType', w: 100 }, { k: 'OrderSupCustId', hide: true }, { k: 'PartyName', t: 'PartyName', w: 260 },
                    { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'DocNo', t: 'DocNo', w: 70 },
                    numCol('OrderQty', 90, hashNo), numCol('OrderWeight', 100, hashNo), numCol('DispatchedWeight', 110, hashNo), numCol('BalWeight', 100, hashNo), { k: 'Remarks', t: 'Remarks', w: 260 }] });
            G.det = SE.grid(Q('grdDetail'), { footer: true,
                cols: [sel(), { k: 'OrderType', t: 'OrderType', w: 100 }, { k: 'OrderId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'OrderDetailId', hide: true }, { k: 'OrderSupCustId', hide: true },
                    { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 300 }, { k: 'ItemUomId', hide: true }, { k: 'PackUOM', t: 'PackUOM', w: 80 },
                    numCol('OrderQTY', 90, zeroComma), numCol('OrderWeight', 100, zeroComma), numCol('DispatchedWeight', 110, zeroComma), numCol('BalWeight', 100, zeroComma)] });
            G.second = SE.grid(Q('GrdSecondView'), { footer: true,
                cols: [sel(), { k: 'OrderType', t: 'OrderType', w: 100 }, { k: 'OrderId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'OrderDetailId', hide: true }, { k: 'OrderSupCustId', hide: true },
                    { k: 'PartyName', t: 'PartyName', w: 240 }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 300 }, { k: 'ItemUomId', hide: true }, { k: 'PackUOM', t: 'PackUOM', w: 80 },
                    numCol('OrderQTY', 90, zeroComma), numCol('OrderWeight', 100, zeroComma), numCol('DispatchedWeight', 110, zeroComma), numCol('BalWeight', 100, zeroComma)] });

            function detailRows(rows, withParty) {                                   // GridDetailBind / GridSecondViewFill row mapping
                return (rows || []).map(function (r) {
                    var x = { OrderType: r.DocumentType, OrderId: r.Id, OrderNo: r.DocNo, OrderDetailId: r.OrderDetailId, OrderSupCustId: r.OrderSupCustId };
                    if (withParty) x.PartyName = r.CustomerName;
                    x.ItemId = r.ItemId; x.ItemName = r.ItemName; x.ItemUomId = r.ItemUOMId; x.PackUOM = r.PackUom;
                    x.OrderQTY = num(r.OrderQTY); x.OrderWeight = num(r.OrderWeight); x.DispatchedWeight = num(r.DispatchWeight); x.BalWeight = num(r.BalWeight);
                    return x;
                });
            }
            function num(v) { var n = Number(v); return isFinite(n) ? n : 0; }
            function pending() {                                                      // PendingSaleOrderLoad
                return SE.api(o.API + '/loader/pending' + SE.q({ fromDate: Q('FromDate').value, toDate: Q('Todate').value, customerId: +cb.cust.value() || 0, itemId: +cb.item.value() || 0 })).then(function (g) {
                    var rows = (g.rows || []);
                    if (rows.length > 0) {
                        G.main.setRows(rows.map(function (r) {
                            return { OrderId: r.Id, DocumentType: r.DocumentType, OrderSupCustId: r.OrderSupCustId, PartyName: r.PartyName, DocDate: r.DocDate, DocNo: r.DocNo,
                                OrderQty: num(r.OrderQty), OrderWeight: num(r.OrderWeight), DispatchedWeight: num(r.DispatchWeight), BalWeight: num(r.BalWeight), Remarks: r.RemarksHeader };
                        }));
                    } else { G.main.setRows([]); G.det.setRows([]); }
                });
            }
            function detailBind(ids, orderDetailIds) {                                // GridDetailBind
                if (!ids) { G.det.setRows([]); return Promise.resolve(); }
                return SE.api(o.API + '/loader/detail' + SE.q({ ids: ids, orderDetailIds: orderDetailIds })).then(function (g) {
                    var rows = detailRows(g.rows, false);
                    G.det.setRows(rows.length > 0 ? rows : []);
                });
            }
            function secondFill() {                                                   // GridSecondViewFill (the filters are not passed on by the BLL)
                return SE.api(o.API + '/loader/detail').then(function (g) { var rows = detailRows(g.rows, true); G.second.setRows(rows.length > 0 ? rows : []); });
            }
            function checkedHeaderIds() { headerIds = ''; G.main.checked().forEach(function (r) { headerIds += ',' + (SE.toInt(r.OrderId)); }); }
            function checkAllDetail() { if (G.main.checked().length > 0) G.det.check(function () { return true; }); }
            function checkMain(g, key) {                                              // CheckedAllMainRows / CheckedAllMainRowsSecondView (string Contains)
                var ids = o.orderMainIds || ''; if (ids === '') return;
                g.check(function (r) { return ids.indexOf(String(r.OrderId)) >= 0; });
            }

            Q('FromDate').value = ''; Q('Todate').value = SE.today();
            SE.api(o.API + '/loader/combos').then(function (c) {
                cb.item.setData(c.items || []); cb.cust.setData(c.customers || []); Q('FromDate').value = SE.dateInput(c.fromDate);
                if (!o.mainDetail) return secondFill().then(function () { checkMain(G.second); });
                return pending().then(function () { checkMain(G.main); checkedHeaderIds(); return detailBind(headerIds, o.orderIds || ''); }).then(checkAllDetail);
            }).catch(SS.fail);

            Q('btngrnlod').onclick = function () { pending().then(secondFill).catch(SS.fail); };          // btngrnlod_Click
            Q('btnLoadOnInvoice').onclick = function () {                                                  // btnLoadOnInvoice_Click_1
                var g = o.mainDetail ? G.det : G.second, chk = g.checked();
                if (chk.length === 0) return SE.alert('No Row is Selected');
                var ids = [], orderId = 0;
                for (var i = 0; i < chk.length; i++) {
                    if (o.documentTypeId === 1506) { ids.push(String(chk[i].OrderDetailId)); continue; }
                    var id = SE.toInt(chk[i].OrderId);
                    if (orderId === 0) orderId = id;
                    if (orderId !== id) return SE.alert('Sorry! Select Same OrderNo Rows');
                    ids.push(String(chk[i].OrderId)); orderId = id;
                }
                result = ids; m.close();
            };
        });
    };

    w.SS = SS;
})(window, document);
