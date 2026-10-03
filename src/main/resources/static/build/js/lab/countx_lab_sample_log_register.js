/* ============================================================================================
 * Screen 155 "Sample Log Register" — Architecture.WinApp.Lab/InvLabSampleLogRegister.cs,
 * DocumentTypeId 301. Line references (:n) are InvLabSampleLogRegister.cs.
 *
 * Every event below is the form's own: InvLabSampleLogRegister_Load (:724), refresh (:444),
 * formvalidation (:403), btnSave_Click (:489), btnUpdate_Click (:557), ReadById (:752),
 * cmbitem_Leave (:1019), CalculateWeight (:1055), btnshow_Click (:1120), the key handler (:916).
 * The server re-validates, re-reads the sample number and recomputes the weight it saves; the
 * Sample No and Weight boxes here are display only.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/sample-log-register';
    var PRINT_RPT = 'RptInvLabSampleLogRegisterSlipA.rpt';                  // PrintSlip :1100
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var MAX_BYTES = 5 * 1024 * 1024;                                        // DesktopAttachmentStore.MAX_BYTES

    /* ItemFill (:230): DDL.BindDDL over the procedure's table — column 0 (Id) hidden, column 1 captioned
       "Item Name", the remaining columns stay visible under their own names (DropDownBind.cs:80-84). */
    if (window.DesktopCombo) {
        window.DesktopCombo.define('labSlrItem', [
            { caption: 'Item Name', flex: 4 },
            { caption: 'ItemCategory', flex: 3, key: 'category' },
            { caption: 'ItemCode', flex: 2, key: 'code' },
            { caption: 'InventoryParentCategoriesId', flex: 2, key: 'parent', type: 'num' }
        ]);
    }

    var L = null;                       // lookups
    var st = {
        recId: 0, updateMode: false,
        grid: [], gridSel: -1,
        hist: [], histSel: -1, histFilter: {},
        picture: null,                  // {name, base64} chosen with Browse (ofd / fileSavePath)
        keepPicture: false,             // the loaded record's picture is still in picbox1
        att: [],                        // AT.lst rows that came from the record (DMSAttachments.GetByID)
        files: [],                      // AT.lst rows added with Browse: {name, base64, size}
        printAvailable: false
    };

    /* grd: the table of :648-662 after RetrieveStructure, with the "Slip" button column at Position 0 (:709-715). */
    var GRD_COLS = [
        { key: 'Slip', caption: 'Slip', width: 40, type: 'slip' },
        { key: 'LogNo', caption: 'LogNo', type: 'open' },                    // rule 4: the sample number opens the record (= ReadById)
        { key: 'LogDate', caption: 'LogDate', type: 'date' },
        { key: 'Supplier', caption: 'Supplier', width: 200 },
        { key: 'ReferenceParty', caption: 'ReferenceParty', width: 200 },
        { key: 'City', caption: 'City', width: 150 },
        { key: 'ItemNames', caption: 'ItemNames', width: 200 },
        { key: 'Crop', caption: 'Crop', width: 100 },
        { key: 'Lot', caption: 'Lot', width: 150 },
        { key: 'Qty', caption: 'Qty', type: 'num' },
        { key: 'PackSize', caption: 'PackSize' },
        { key: 'EntryUser', caption: 'EntryUser', width: 150 },
        { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
        { key: 'Remarks', caption: 'Remarks', width: 200 },
        { key: 'Attachments', caption: 'Attachments', type: 'link' }       // ColumnType Link (:708)
    ];
    /* grdhistory: the table of :1150-1163 with grdhistorySetting (:1182) — Qty summed, "0,0", right aligned. */
    var HIST_COLS = [
        { key: 'LogNo', caption: 'LogNo', type: 'open' },                    // rule 4: the sample number opens the record (= ReadById)
        { key: 'LogDate', caption: 'LogDate', type: 'date' },
        { key: 'Supplier', caption: 'Supplier', width: 200 },
        { key: 'ReferenceParty', caption: 'ReferenceParty', width: 200 },
        { key: 'City', caption: 'City', width: 200 },
        { key: 'ItemNames', caption: 'ItemNames', width: 200 },
        { key: 'Crop', caption: 'Crop', width: 120 },
        { key: 'Lot', caption: 'Lot', width: 150 },
        { key: 'Qty', caption: 'Qty', type: 'qty00' },
        { key: 'PackSize', caption: 'PackSize' },
        { key: 'EntryUser', caption: 'EntryUser', width: 150 },
        { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
        { key: 'Remarks', caption: 'Remarks', width: 200 }
    ];

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function intOf(v) { var n = parseInt(String(v === undefined || v === null ? '' : v), 10); return isNaN(n) ? 0 : n; }
    /** Conversion.ToDouble(text) = Convert.ToDouble(string): a float, thousands separators allowed; anything else 0. */
    function netToDouble(v) {
        var s = String(v === undefined || v === null ? '' : v).trim();
        if (!/^[+-]?(\d[\d,]*\.?\d*|\.\d+)([eE][+-]?\d+)?$/.test(s)) return 0;
        var n = Number(s.replace(/,/g, ''));
        return isFinite(n) ? n : 0;
    }
    /** Conversion.ToInt(text) = Convert.ToInt32(string): sign and digits only; anything else 0. */
    function netToInt(v) {
        var s = String(v === undefined || v === null ? '' : v).trim();
        if (!/^[+-]?\d+$/.test(s)) return 0;
        var n = Number(s);
        return (n > 2147483647 || n < -2147483648) ? 0 : n;
    }
    /** x.ToString("0,0") — whole number, grouped, at least two digits. */
    function fmt00(x) {
        var r = Math.round(Math.abs(x)) * (x < 0 ? -1 : 1);
        var s = Math.abs(r).toLocaleString('en-US', { maximumFractionDigits: 0 });
        if (s.length < 2) s = '0' + s;
        return (r < 0 ? '-' : '') + s;
    }
    function pad(n) { return String(n).padStart(2, '0'); }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function isoDay(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v || '')); return m ? m[1] : ''; }
    function dMMMyyyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] : '';
    }
    function dMMMyyyyTime(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return dMMMyyyy(v);
        var h = +m[4], ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] + ' ' + pad(h) + ':' + m[5] + ' ' + ap;
    }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function rights() { return (L && L.rights) || {}; }
    function show(el, on) { if (typeof el === 'string') el = $(el); if (el) el.hidden = !on; }

    async function api(method, url, body) {
        var opt = { method: method, credentials: 'same-origin', headers: { Accept: 'application/json' } };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        var res = await fetch(url, opt);
        var text = await res.text();
        var data = null;
        try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
        if (!res.ok) {
            var m = data && (data.message || data.error) || ('The request failed (' + res.status + ').');
            throw new Error(m);
        }
        return data;
    }
    /** countx_purchase_request.js busy panel + button lock; any refusal is shown as the desktop's MessageBox. */
    function act(button, work) {
        /* Rule 5 (countx_lab_page_kit.js): the button is disabled at once, carries a spinner (btn-busy), ignores
           further clicks while the request is in flight and is re-enabled in finally - success or failure. */
        if (window.LabKit) return window.LabKit.act(button || null, work);
        var runner = window.PurchaseRequest ? window.PurchaseRequest.run.bind(window.PurchaseRequest) : function (b, w) { return w(); };
        return runner(button || null, async function () {
            try { await work(); } catch (e) { alert(msg(e)); }
        });
    }

    // ============================================================================ combos

    function opt(value, text, attrs) {
        var a = '';
        if (attrs) Object.keys(attrs).forEach(function (k) { a += ' data-' + k + '="' + esc(attrs[k]) + '"'; });
        return '<option value="' + esc(value) + '"' + a + '>' + esc(text) + '</option>';
    }
    /** BindDDL / BindDDLNew with ZeroIndex false: no default row, nothing selected. */
    function fill(id, rows, valueKey, textKey, attrFn) {
        var el = $(id), html = '';
        (rows || []).forEach(function (r) { html += opt(r[valueKey], r[textKey], attrFn ? attrFn(r) : null); });
        el.innerHTML = html;
        el.selectedIndex = -1;
    }
    function has(id, v) { return Array.prototype.some.call($(id).options, function (o) { return o.value === String(v); }); }
    /** combo.Value = v — a value that is not in the list leaves the combo empty. */
    function setVal(id, v) {
        var el = $(id);
        dropAdhoc(id);
        if (v !== null && v !== undefined && has(id, v)) el.value = String(v); else el.selectedIndex = -1;
    }
    /** Conversion.ToInt(combo.Value) */
    function comboVal(id) {
        var el = $(id);
        if (el.selectedIndex < 0) return 0;
        var o = el.options[el.selectedIndex];
        return o.hasAttribute('data-adhoc') ? 0 : intOf(o.value);
    }
    /** combo.Text */
    function comboText(id) { var el = $(id); return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].textContent; }
    function dropAdhoc(id) { Array.prototype.slice.call($(id).querySelectorAll('option[data-adhoc]')).forEach(function (o) { o.remove(); }); }
    function clearCombo(id) { dropAdhoc(id); $(id).selectedIndex = -1; }                 // combo.Text = string.Empty
    /** combo.Text = text — selects the row with that text; a text that matches no row stays in the box. */
    function setText(id, text) {
        var el = $(id), t = String(text === undefined || text === null ? '' : text);
        dropAdhoc(id);
        if (t.trim() === '') { el.selectedIndex = -1; return; }
        for (var i = 0; i < el.options.length; i++) {
            if (el.options[i].textContent.trim().toLowerCase() === t.trim().toLowerCase()) { el.selectedIndex = i; return; }
        }
        el.insertAdjacentHTML('beforeend', '<option value="' + esc(t) + '" data-adhoc="1">' + esc(t) + '</option>');
        el.value = t;
    }

    function itemAttrs(r) { return { category: r.ItemCategory, code: r.ItemCode, parent: r.InventoryParentCategoriesId }; }

    function bindLookups() {
        /* ItemFill (:230) — only when the procedure returned rows */
        if ((L.items || []).length > 0) {
            fill('cmbitem', L.items, 'Id', 'ItemName', itemAttrs);
            fill('CmbItemCriteria', L.items, 'Id', 'ItemName', itemAttrs);
        }
        /* CityFill (:366) */
        if ((L.cities || []).length > 0) {
            fill('cmbcity', L.cities, 'Id', 'CityName');
            fill('CmbCityCriteria', L.cities, 'Id', 'CityName');
        }
        /* CropYearFill (:313) */
        fill('cmbCropYear', L.cropYears, 'Id', 'CropYear');
        fill('CmbCropCriteria', L.cropYears, 'Id', 'CropYear');
        /* JobLotFill (:327) */
        if ((L.jobLots || []).length > 0) fill('cmbJobLot', L.jobLots, 'Id', 'JobLotDescription');
        /* SupplierFillGetAll (:273) -> supplierfill (:290) + referencepartyFill (:343) */
        if ((L.suppliers || []).length > 0) {
            fill('cmbsup', L.suppliers, 'Id', 'CompanyName');
            fill('CmbSupplierCriteria', L.suppliers, 'Id', 'CompanyName');
            fill('cmbreferenceparty', L.suppliers, 'Id', 'CompanyName');
            fill('CmbRefrencePartyCriteria', L.suppliers, 'Id', 'CompanyName');
        }
    }

    /** bindPackSizeInput (:247) — the list is rebound only when the item has a UOM schedule (:262). */
    function bindPackSize(rows) {
        if (!rows || rows.length === 0) return;
        var prev = $('CmbPackSize').selectedIndex < 0 ? null : $('CmbPackSize').value;
        fill('CmbPackSize', rows, 'Id', 'Equivalent');
        setVal('CmbPackSize', prev);
        calculateWeight();                                                   // CmbPackSize_TextChanged :1043
    }

    /** CalculateWeight (:1055) */
    function calculateWeight() {
        var qty = netToDouble($('txtqty').value);
        var pack = netToInt(comboText('CmbPackSize'));
        $('txtweight').value = (qty > 0 && pack > 0) ? fmt00(qty * pack) : '0';
    }

    // ============================================================================ grids

    function cell(c, r) {
        var v = r[c.key];
        if (c.type === 'date') return '<td>' + esc(dMMMyyyy(v)) + '</td>';
        if (c.type === 'datetime') return '<td>' + esc(dMMMyyyyTime(v)) + '</td>';
        if (c.type === 'num') return '<td class="n">' + esc(v === null || v === undefined ? '' : v) + '</td>';
        if (c.type === 'qty00') return '<td class="n">' + esc(fmt00(Number(v) || 0)) + '</td>';
        if (c.type === 'open') return '<td><a class="lab-open" data-open="1" title="Open this record">' + esc(v === null || v === undefined ? '' : v) + '</a></td>';
        if (c.type === 'link') return '<td><a class="lnk" data-att="1">' + esc(v === null || v === undefined ? '' : v) + '</a></td>';
        if (c.type === 'slip') {
            return '<td><button type="button" class="gbtn" data-slip="1"' + (st.printAvailable ? '' :
                ' disabled title="The print of ' + PRINT_RPT + ' is not registered on the web yet"') + '>Slip</button></td>';
        }
        return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
    }
    function head(cols) {
        return '<tr>' + cols.map(function (c) {
            return '<th' + (c.width ? ' style="min-width:' + c.width + 'px;max-width:' + c.width + 'px;"' : '') + '>' + esc(c.caption) + '</th>';
        }).join('') + '</tr>';
    }

    function renderGrd() {
        var t = $('grd');
        t.tHead.innerHTML = head(GRD_COLS);
        t.tBodies[0].innerHTML = st.grid.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === st.gridSel ? ' class="is-sel"' : '') + '>' + GRD_COLS.map(function (c) { return cell(c, r); }).join('') + '</tr>';
        }).join('');
        /* TotalRow = True with no aggregate defined on any column: an empty total row. */
        t.tFoot.innerHTML = '<tr>' + GRD_COLS.map(function () { return '<td></td>'; }).join('') + '</tr>';
        $('grdNav').textContent = 'Record ' + (st.grid.length ? st.gridSel + 1 : 0) + ' of ' + st.grid.length;
    }
    function setGrid(rows) {
        st.grid = rows || [];
        st.gridSel = st.grid.length ? 0 : -1;
        renderGrd();
    }

    function histVisible() {
        return st.hist.map(function (r, i) { return { r: r, i: i }; }).filter(function (x) {
            return HIST_COLS.every(function (c) {
                var f = (st.histFilter[c.key] || '').trim().toLowerCase();
                if (!f) return true;
                var v = x.r[c.key];
                var shown = c.type === 'date' ? dMMMyyyy(v) : c.type === 'datetime' ? dMMMyyyyTime(v)
                    : c.type === 'qty00' ? fmt00(Number(v) || 0) : String(v === null || v === undefined ? '' : v);
                return shown.toLowerCase().indexOf(f) >= 0;
            });
        });
    }
    function renderHistBody() {
        var t = $('grdhistory'), vis = histVisible(), total = 0;
        t.tBodies[0].innerHTML = vis.map(function (x) {
            total += Number(x.r.Qty) || 0;
            return '<tr data-i="' + x.i + '"' + (x.i === st.histSel ? ' class="is-sel"' : '') + '>' + HIST_COLS.map(function (c) { return cell(c, x.r); }).join('') + '</tr>';
        }).join('');
        /* Qty AggregateFunction Sum, FormatString "0,0" (:1205-1207) */
        t.tFoot.innerHTML = '<tr>' + HIST_COLS.map(function (c) {
            return c.key === 'Qty' ? '<td class="n">' + esc(fmt00(total)) + '</td>' : '<td></td>';
        }).join('') + '</tr>';
        $('grdhistoryNav').textContent = 'Record ' + (vis.length && st.histSel >= 0 ? vis.map(function (x) { return x.i; }).indexOf(st.histSel) + 1 : 0) + ' of ' + vis.length;
    }
    function renderHist() {
        var t = $('grdhistory');
        /* FilterMode Automatic: a filter row under the headers */
        t.tHead.innerHTML = head(HIST_COLS) + '<tr class="flt">' + HIST_COLS.map(function (c) {
            return '<th><input type="text" data-flt="' + esc(c.key) + '" value="' + esc(st.histFilter[c.key] || '') + '"></th>';
        }).join('') + '</tr>';
        renderHistBody();
    }

    // ============================================================================ picture

    function setPicture(src) {
        var img = $('picbox1Img');
        if (src) { img.src = src; img.hidden = false; } else { img.removeAttribute('src'); img.hidden = true; }
    }
    function readFile(file) {
        return new Promise(function (resolve, reject) {
            var fr = new FileReader();
            fr.onload = function () {
                var s = String(fr.result || ''), i = s.indexOf(',');
                resolve({ name: file.name, base64: i >= 0 ? s.substring(i + 1) : '', size: file.size, url: s });
            };
            fr.onerror = function () { reject(new Error('The file could not be read.')); };
            fr.readAsDataURL(file);
        });
    }

    /** btnimg1_Click (:847) */
    function browsePicture() {
        /* definition() (:822) — without the "Attachment Folder Path" configuration nothing can be chosen. */
        if (!L || !L.attachmentFolderConfigured) { alert('Please Map the Path in Configration'); return; }
        $('picFile').value = '';
        $('picFile').click();
    }
    async function pictureChosen() {
        var f = $('picFile').files && $('picFile').files[0];
        if (!f) return;
        if (!/\.(jpg|jpeg|png)$/i.test(f.name)) { alert('Select a .jpg, .jpeg or .png picture'); return; }     // OpenFileDialog filter :855
        if (f.size === 0 || f.size > MAX_BYTES) { alert('Attachment must be between 1 byte and 5 MB'); return; }
        try {
            var p = await readFile(f);
            st.picture = { name: p.name, base64: p.base64 };
            st.keepPicture = false;
            setPicture(p.url);                                               // picbox1.Image = new Bitmap(...) :864
        } catch (e) { alert(msg(e)); }
    }

    // ============================================================================ attachments

    function sizeMb(v) { var n = Number(v); return isFinite(n) && n > 0 ? n.toFixed(2) + ' Mb' : ''; }

    function renderAttachment() {
        var html = '';
        st.att.forEach(function (a, i) {
            html += '<tr><td>' + (st.recId ? '<a class="lnk" href="' + API + '/' + st.recId + '/attachments/' + intOf(a.Id) + '" target="_blank" rel="noopener">' + esc(a.Attachment) + '</a>' : esc(a.Attachment))
                + '</td><td class="n">' + esc(sizeMb(a.UploadedFileSizeMb)) + '</td><td><button type="button" class="gbtn" data-del-att="' + i + '">Delete</button></td></tr>';
        });
        st.files.forEach(function (f, i) {
            html += '<tr><td>' + esc(f.name) + '</td><td class="n">' + esc(sizeMb(f.size / 1048576)) + '</td><td><button type="button" class="gbtn" data-del-file="' + i + '">Delete</button></td></tr>';
        });
        $('grdAttachment').tBodies[0].innerHTML = html;
    }
    /** Choose_Click (Attachment.cs:501) */
    async function attachmentsChosen() {
        var list = Array.prototype.slice.call($('attFile').files || []);
        for (var i = 0; i < list.length; i++) {
            var f = list[i];
            var present = st.files.some(function (x) { return x.name === f.name; }) || st.att.some(function (x) { return x.Attachment === f.name; });
            if (present) { alert(f.name + ' File Already Present'); continue; }                // Attachment.cs:525
            if (f.size === 0 || f.size > MAX_BYTES) { alert(f.name + ': Attachment must be between 1 byte and 5 MB'); continue; }
            if (st.files.length >= 10) { alert('At most ten attachments may be uploaded at once'); break; }
            try {
                var p = await readFile(f);
                st.files.push({ name: p.name, base64: p.base64, size: p.size });
            } catch (e) { alert(msg(e)); }
        }
        $('attFile').value = '';
        renderAttachment();
    }

    /** grd_LinkClicked (:974) — DMSAttachments.GetByID for the row, shown as the AttachmentView dialog. */
    function viewAttachments(id) {
        act(null, async function () {
            var rows = await api('GET', API + '/' + intOf(id) + '/attachments');
            var total = 0;
            $('grdAttachmentView').tBodies[0].innerHTML = (rows || []).map(function (a) {
                total += Number(a.UploadedFileSizeMb) || 0;
                return '<tr><td><a class="lnk" href="' + API + '/' + intOf(id) + '/attachments/' + intOf(a.Id) + '" target="_blank" rel="noopener">' + esc(a.Attachment) + '</a></td><td class="n">'
                    + esc(sizeMb(a.UploadedFileSizeMb)) + '</td><td>' + esc(a.EntryUserName || '') + '</td><td>' + esc(dMMMyyyyTime(a.EntryDate)) + '</td></tr>';
            }).join('');
            $('lblTotalFiles').textContent = String((rows || []).length);
            $('lblTotalSize').textContent = total.toFixed(3);
            show('dlgAttachmentView', true);
        });
    }

    // ============================================================================ form

    /** refresh() (:444) */
    async function refresh() {
        st.att = []; st.files = [];                                          // AT = new Attachment()
        ['cmbsup', 'cmbitem', 'cmbcity', 'cmbJobLot', 'cmbCropYear', 'cmbreferenceparty'].forEach(clearCombo);
        $('CmbPackSize').innerHTML = ''; $('CmbPackSize').selectedIndex = -1;  // Text = "", DataSource = null
        st.updateMode = false; st.recId = 0;
        show('btnSave', true); show('btnUpdate', false);
        $('txtqty').value = '';
        $('txtremarks').value = '';
        $('txtsampleno').value = '';
        $('txtweight').value = '';
        st.picture = null; st.keepPicture = false; setPicture(null);         // picbox1.Image = null
        var d = await api('GET', API + '/refresh');                           // SampleCode + grdHistoryForLoad
        if (intOf(d.sampleNo) > 0) $('txtsampleno').value = String(d.sampleNo);
        setGrid(d.grid);
        focusCombo('cmbsup');
    }

    function focusCombo(id) {
        var el = $(id), wrap = el && el.parentNode;
        var input = wrap && wrap.classList && wrap.classList.contains('dtcombo-wrap') ? wrap.querySelector('.dtcombo-input') : null;
        try { (input || el).focus(); } catch (e) { /* not focusable yet */ }
    }

    /** formvalidation (:403) — desktop order and texts, on what the form shows. */
    function formvalidation() {
        if ($('txtsampleno').value.trim() === '') { alert('Please Insert sample no'); $('txtsampleno').focus(); return false; }
        if (comboText('cmbsup').trim() === '') { alert('Please Select Supplier '); focusCombo('cmbsup'); return false; }
        if (comboText('cmbitem').trim() === '') { alert('Please Select Item'); focusCombo('cmbitem'); return false; }
        if ($('txtqty').value.trim() === '') { alert('Please Insert Quantity'); $('txtqty').focus(); return false; }
        if (comboText('CmbPackSize').trim() === '') { alert('Please Select Pack Size'); focusCombo('CmbPackSize'); return false; }
        if ($('txtweight').value.trim() === '' || $('txtweight').value === '0') { alert('weight Should Be Greater Than 0'); $('txtqty').focus(); return false; }
        return true;
    }

    function payload() {
        return {
            id: st.updateMode ? st.recId : 0,
            sampleNo: $('txtsampleno').value,
            sampleDate: $('txtsampled').value || today(),
            supplierCustomerId: comboVal('cmbsup'),
            referencePartyId: comboVal('cmbreferenceparty'),
            cityId: comboVal('cmbcity'),
            itemId: comboVal('cmbitem'),
            crop: comboText('cmbCropYear'),
            jobLotId: comboVal('cmbJobLot'),
            lotDesc: comboText('cmbJobLot'),
            qty: $('txtqty').value,
            itemUomId: comboVal('CmbPackSize'),
            packSize: comboText('CmbPackSize'),
            otherRemarks: $('txtremarks').value,
            picture: st.picture,
            keepPicture: st.updateMode && st.keepPicture,
            files: st.files.map(function (f) { return { name: f.name, base64: f.base64 }; }),
            keepAttachmentIds: st.att.map(function (a) { return intOf(a.Id); })
        };
    }

    /** btnSave_Click (:489) / btnUpdate_Click (:557) */
    function save(update) {
        var button = $(update ? 'btnUpdate' : 'btnSave');
        if (button.disabled) return;                                         // Enabled = the Save / Update right (:729-730)
        if (!formvalidation()) return;
        if (!confirm(update ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        act(button, async function () {
            var d = await api('POST', API + '/save', payload());
            alert(d.message);                                                // "Save Successfully" / "Update Successfully"
            await refresh();                                                 // grdHistoryForLoad + refresh
            if ($('isprint').checked) printSlip(d.id);                       // :545 / :616
        });
    }

    /** ReadById (:752) */
    function readById(id) {
        if (!intOf(id)) return;
        act(null, async function () {
            var d = await api('GET', API + '/' + intOf(id));
            if (!d || !d.found) return;                                      // dtGetById.Rows.Count <= 0 (:759)
            selectTab('tabForm', true);                                      // tabControl1.SelectedIndex = 0
            st.recId = intOf(d.Id);
            $('txtsampleno').value = d.SampleNo;
            $('txtsampled').value = isoDay(d.SampleDate) || today();
            setVal('cmbsup', d.SupplierCustomerId);
            setVal('cmbreferenceparty', d.ReferencePartyId);
            setVal('cmbcity', d.CityId);
            setVal('cmbitem', d.ItemId);
            bindPackSize(d.uoms);                                            // :770
            setVal('CmbPackSize', d.ItemUomId);
            setText('cmbCropYear', d.Crop);                                  // :772
            setVal('cmbJobLot', d.JobLotId);
            $('txtqty').value = d.Qty;
            $('txtweight').value = d.Weight;                                 // :775 — the stored weight, after the recalculation
            $('txtremarks').value = d.OtherRemarks;
            st.picture = null;
            st.keepPicture = !!d.hasPicture;                                 // :778-796
            setPicture(d.hasPicture ? API + '/' + st.recId + '/picture?t=' + Date.now() : null);
            st.updateMode = true;
            show('btnSave', false); show('btnUpdate', true);
            st.att = d.attachments || []; st.files = [];                     // :800-808
        });
    }

    /** PrintSlip (:1076) — RptInvLabSampleLogRegisterSlipA.rpt for one Id. */
    function printSlip(id) {
        if (!st.printAvailable || !intOf(id)) return;
        window.open('/reports/print/by-template/' + encodeURIComponent(PRINT_RPT) + '/pdf?id=' + intOf(id), '_blank');
    }
    /** Is the slip registered with the print engine? (PrintController /controls answers key = null when it is not.) */
    async function probePrint() {
        try {
            var d = await api('GET', '/api/print/controls?rpt=' + encodeURIComponent(PRINT_RPT));
            st.printAvailable = !!(d && d.key);
        } catch (e) { st.printAvailable = false; }
        var cb = $('isprint');
        if (st.printAvailable) { cb.disabled = false; cb.checked = true; cb.parentNode.removeAttribute('title'); }   // isprint.Checked = true (:749)
        else { cb.checked = false; cb.disabled = true; cb.parentNode.title = 'The print of ' + PRINT_RPT + ' is not registered on the web yet'; }
    }

    // ============================================================================ history

    /** btnshow_Click (:1120) */
    function showHistory(button) {
        return act(button || null, async function () {
            var q = '?fromDate=' + encodeURIComponent($('FromDate').value || '') + '&toDate=' + encodeURIComponent($('ToDate').value || '')
                + '&supplierId=' + comboVal('CmbSupplierCriteria') + '&referencePartyId=' + comboVal('CmbRefrencePartyCriteria')
                + '&itemId=' + comboVal('CmbItemCriteria') + '&cityId=' + comboVal('CmbCityCriteria')
                + '&cropYear=' + encodeURIComponent(comboText('CmbCropCriteria'));
            var rows = await api('GET', API + '/history' + q);
            if (rows && rows.length > 0) {                                   // :1165 — an empty result leaves the grid as it was
                st.hist = rows;
                st.histSel = 0;
                renderHist();
            }
        });
    }

    function selectTab(id, silent) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $(t).classList.toggle('is-active', t === id); });
        $('tabBtnForm').classList.toggle('is-active', id === 'tabForm');
        $('tabBtnHistory').classList.toggle('is-active', id === 'tabHistory');
        $('btnHistoryText').textContent = id === 'tabHistory' ? 'Form' : 'History';
        if (id === 'tabHistory' && !silent) return showHistory(null);        // tabControl1_SelectedIndexChanged :1011
    }

    function historyClick() {
        if ($('tabHistory').classList.contains('is-active')) { selectTab('tabForm'); return; }
        selectTab('tabHistory', true);
        return showHistory($('btnHistory'));
    }

    // ============================================================================ keys

    function modalOpen() { return !$('dlgAttachment').hidden || !$('dlgAttachmentView').hidden; }

    /** InvLabSampleLogRegister_KeyDown (:916) */
    function keyDown(e) {
        if (e.defaultPrevented) return;                                      // an open drop-down owns the key
        var k = e.key, c = e.ctrlKey && !e.altKey;
        if (modalOpen()) {
            if (k === 'Escape') { show('dlgAttachment', false); show('dlgAttachmentView', false); e.preventDefault(); }
            return;
        }
        if (k === 'Enter' && !c) {                                           // SendKeys.Send("{TAB}")
            var t = e.target;
            if (t && t.tagName !== 'BUTTON' && t.tagName !== 'A' && t.closest && t.closest('.slr-page')) { e.preventDefault(); focusNext(t); }
            return;
        }
        if (c && (k === 's' || k === 'S')) { e.preventDefault(); if (!st.updateMode) save(false); return; }
        if (c && (k === 'n' || k === 'N')) { e.preventDefault(); newClick(); return; }
        if ((c && (k === 'e' || k === 'E')) || k === 'Escape') { e.preventDefault(); window.location.href = '/quality'; return; }   // Close()
        if (c && (k === 'u' || k === 'U')) { e.preventDefault(); if (st.updateMode) save(true); }
    }
    function focusNext(from) {
        var page = from.closest('.slr-page');
        var list = Array.prototype.slice.call(page.querySelectorAll('input, select, button')).filter(function (el) {
            return !el.disabled && el.type !== 'hidden' && el.type !== 'file' && el.tabIndex >= 0 && el.offsetParent !== null
                && !el.classList.contains('wf-tool') && !el.closest('table');
        });
        var i = list.indexOf(from);
        if (i >= 0 && list.length > 1) list[(i + 1) % list.length].focus();
    }

    /** btnNew_Click (:476) */
    function newClick() { act($('btnNew'), refresh); }

    // ============================================================================ wiring

    function wire() {
        $('btnNew').addEventListener('click', newClick);
        $('btnSave').addEventListener('click', function () { save(false); });
        $('btnUpdate').addEventListener('click', function () { save(true); });
        $('btnattachment').addEventListener('click', function () { renderAttachment(); show('dlgAttachment', true); });   // AT.Show() :951
        /* new InvLabAnalysisGroup / InvLabAnalysisItems / InvLabGroupAnalysisStandards(UserAccount).Show() (:959-972) */
        $('btnLabAnalysisGroup').addEventListener('click', function () { window.open('/quality/analysis-group', '_blank'); });
        $('btnLabAnalysisItem').addEventListener('click', function () { window.open('/quality/item-analysis-parameter', '_blank'); });
        $('btnGroupPerameter').addEventListener('click', function () { window.open('/quality/group-analysis-standards', '_blank'); });

        $('btnimg1').addEventListener('click', browsePicture);
        $('picFile').addEventListener('change', pictureChosen);

        /* cmbitem_Leave (:1019) */
        $('cmbitem').addEventListener('change', function () {
            var itemId = comboVal('cmbitem');
            act(null, async function () { bindPackSize(await api('GET', API + '/uoms?itemId=' + itemId)); });
        });
        $('txtqty').addEventListener('input', calculateWeight);              // txtqty_TextChanged :1031
        $('CmbPackSize').addEventListener('change', calculateWeight);        // CmbPackSize_TextChanged :1043

        /* grd_DoubleClick (:816), grd_ColumnButtonClick (:1108), grd_LinkClicked (:974) */
        var grd = $('grd');
        grd.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            st.gridSel = intOf(tr.getAttribute('data-i'));
            Array.prototype.forEach.call(grd.tBodies[0].rows, function (r) { r.classList.toggle('is-sel', r === tr); });
            $('grdNav').textContent = 'Record ' + (st.gridSel + 1) + ' of ' + st.grid.length;
            var row = st.grid[st.gridSel];
            if (!row) return;
            if (e.target.closest('[data-slip]')) printSlip(row.Id);
            else if (e.target.closest('[data-att]')) viewAttachments(row.Id);
            else if (e.target.closest('[data-open]')) readById(row.Id);       // rule 4: LogNo link = grd_DoubleClick
        });
        grd.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr || e.target.closest('[data-slip]') || e.target.closest('[data-att]') || e.target.closest('[data-open]')) return;
            var row = st.grid[intOf(tr.getAttribute('data-i'))];
            if (row) readById(row.Id);
        });
        /* panel7.DoubleClick is wired to grd_DoubleClick too (designer): the grid's current row is opened. */
        $('panel7').addEventListener('dblclick', function (e) {
            if (e.target !== $('panel7')) return;
            var row = st.grid[st.gridSel];
            if (row) readById(row.Id);
        });

        /* grdhistory_DoubleClick (:1005) and its filter row */
        var gh = $('grdhistory');
        gh.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            st.histSel = intOf(tr.getAttribute('data-i'));
            Array.prototype.forEach.call(gh.tBodies[0].rows, function (r) { r.classList.toggle('is-sel', r === tr); });
            $('grdhistoryNav').textContent = 'Record ' + (tr.sectionRowIndex + 1) + ' of ' + gh.tBodies[0].rows.length;
            if (e.target.closest('[data-open]')) {                           // rule 4: LogNo link = grdhistory_DoubleClick
                var hrow = st.hist[st.histSel];
                if (hrow) readById(hrow.Id);
            }
        });
        gh.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr || e.target.closest('[data-open]')) return;
            var row = st.hist[intOf(tr.getAttribute('data-i'))];
            if (row) readById(row.Id);
        });
        gh.addEventListener('input', function (e) {
            var k = e.target.getAttribute && e.target.getAttribute('data-flt');
            if (!k) return;
            st.histFilter[k] = e.target.value;
            renderHistBody();
        });
        $('btnshow').addEventListener('click', function () { showHistory($('btnshow')); });

        $('tabBtnForm').addEventListener('click', function () { selectTab('tabForm'); });
        $('tabBtnHistory').addEventListener('click', function () { selectTab('tabHistory'); });
        /* Rule 4: History button at the right of the footer. It selects the History tab, which runs btnshow_Click
           (:1011-1017); the button is busy until the rows are in. From the History tab it returns to the form. */
        $('btnHistory').addEventListener('click', historyClick);

        /* Attachment dialog */
        $('Choose').addEventListener('click', function () { $('attFile').value = ''; $('attFile').click(); });
        $('attFile').addEventListener('change', attachmentsChosen);
        $('btnClose').addEventListener('click', function () { show('dlgAttachment', false); });
        $('dlgAttachmentX').addEventListener('click', function () { show('dlgAttachment', false); });
        $('grdAttachment').addEventListener('click', function (e) {
            var a = e.target.getAttribute('data-del-att'), f = e.target.getAttribute('data-del-file');
            if (a !== null) st.att.splice(intOf(a), 1);
            else if (f !== null) st.files.splice(intOf(f), 1);
            else return;
            renderAttachment();
        });
        $('dlgAttachmentViewX').addEventListener('click', function () { show('dlgAttachmentView', false); });

        document.addEventListener('keydown', keyDown);
    }

    /** InvLabSampleLogRegister_Load (:724) */
    async function init() {
        wire();
        renderGrd();
        renderHist();
        try {
            L = await api('GET', API + '/lookups');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            show('rightsNote', true);
            ['btnNew', 'btnSave', 'btnUpdate', 'btnattachment', 'btnimg1', 'btnshow'].forEach(function (id) { $(id).disabled = true; });
            return;
        }
        var r = rights();
        $('btnSave').disabled = !r.save;                                     // :729
        $('btnUpdate').disabled = !r.update;                                 // :730
        $('FromDate').value = isoDay(L.financialYearStart) || today();       // :731
        $('ToDate').value = isoDay(L.today) || today();                      // :732
        $('txtsampled').value = today();                                     // DateTimePicker default
        show('btnSave', true); show('btnUpdate', false);                     // :740-741
        if (intOf(L.sampleNo) > 0) $('txtsampleno').value = String(L.sampleNo);   // SampleCode :742
        (L.errors || []).forEach(function (m) { alert(m); });               // each desktop fill has its own catch -> MessageBox
        bindLookups();                                                       // :743-747
        await probePrint();                                                  // isprint.Checked = true (:749) when the slip can be printed
        setGrid(L.grid);                                                     // grdHistoryForLoad :748
        focusCombo('cmbsup');
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else init();
})();
