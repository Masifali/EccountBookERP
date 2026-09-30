/* ============================================================================================
 * countx_imports_impb_core.js - shared client pieces of the four Import pages ported from
 * frmProformaInvoice (782), frmImportInvoice (783), frmInvoicePackingDetail (784) and
 * frmShipmentBooking (785). Loaded after countx_hrm.js; exposes window.ImpB.
 *
 *   ImpB.tabs(root)                the WinForms TabControl (buttons .hrm-tab[data-tab] -> #page)
 *   ImpB.fmt(v, maxDec, minDec)    .NET "#,##0.###"-style text (grouped, trailing zeros trimmed)
 *   ImpB.fmtInt(v)                 .NET "0,0" (rounded, grouped)
 *   ImpB.roundEven(v, d)           Math.Round (MidpointRounding.ToEven)
 *   ImpB.shortcuts(rows)           ShortCutKeyPopUp
 *   ImpB.attachments(opts)         the Attachment dialog (existing rows: keep / remove / download; new files)
 *   ImpB.historyFilter(prefix)     the From / To date (with tick box) + date-type radio values of a history tab
 *   ImpB.serialDialog(opts)        frmProformaDocumentSerial / frmMasterDocumentSerial as a dialog
 * ============================================================================================ */
(function (global) {
    'use strict';
    var ImpB = {};

    ImpB.tabs = function (root) {
        (root || document).querySelectorAll('[data-tabs]').forEach(function (bar) {
            bar.addEventListener('click', function (e) {
                var b = e.target.closest('.hrm-tab'); if (!b || b.disabled) return;
                ImpB.showTab(bar.getAttribute('data-tabs'), b.getAttribute('data-tab'));
            });
        });
    };
    ImpB.showTab = function (group, pageId) {
        document.querySelectorAll('[data-tabs="' + group + '"] .hrm-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === pageId); });
        document.querySelectorAll('[data-tabs-page="' + group + '"]').forEach(function (p) { p.classList.toggle('is-active', p.id === pageId); });
        var ev = new CustomEvent('impb-tab', { detail: { group: group, page: pageId } });
        document.dispatchEvent(ev);
    };
    ImpB.activeTab = function (group) {
        var t = document.querySelector('[data-tabs="' + group + '"] .hrm-tab.is-active');
        return t ? t.getAttribute('data-tab') : null;
    };

    /** Math.Round(v, d) with MidpointRounding.ToEven. */
    ImpB.roundEven = function (v, d) {
        var p = Math.pow(10, d || 0), x = HRM.num(v) * p, r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / p;
    };
    /** ToString("#,##0.###") family: grouped, up to maxDec decimals, at least minDec. */
    ImpB.fmt = function (v, maxDec, minDec) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.num(v), s = n.toFixed(maxDec === undefined ? 3 : maxDec);
        var p = s.split('.'), dec = p[1] || '';
        while (dec.length > (minDec || 0) && dec.charAt(dec.length - 1) === '0') dec = dec.slice(0, -1);
        p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return dec ? p[0] + '.' + dec : p[0];
    };
    /** ToString("0,0") - whole number, grouped (away-from-zero rounding as .NET formatting does). */
    ImpB.fmtInt = function (v) {
        var n = HRM.num(v), r = (n < 0 ? -1 : 1) * Math.round(Math.abs(n));
        var s = String(Math.abs(r)).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        if (Math.abs(r) < 10) s = '0' + s;            // "0,0" always prints two digits
        return (r < 0 ? '-' : '') + s;
    };
    /** Numeric text for an input (plain, no grouping). */
    ImpB.plain = function (v) { var n = HRM.num(v); return String(Math.round(n * 1e6) / 1e6); };

    ImpB.shortcuts = function (rows) {
        var html = '<table class="win-grid hrm-grid" style="width:100%"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr><td>' + HRM.esc(r[0]) + '</td><td>' + HRM.esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table>';
        HRM.modal({ title: 'ShortCut Keys', width: 'min(520px, 96vw)', html: '<div class="hrm-grid-wrap" style="max-height:70vh">' + html + '</div>' });
    };

    /**
     * History filter values: { dateType: 'doc'|'entry'|'modify'|'approved', fromChecked, fromDate, toChecked, toDate }.
     * Ids: <p>FromDate, <p>FromChk, <p>ToDate, <p>ToChk, radios name=<p>DateType.
     */
    ImpB.historyFilter = function (p) {
        var r = document.querySelector('input[name="' + p + 'DateType"]:checked');
        return {
            dateType: r ? r.value : 'doc',
            fromChecked: HRM.checked(p + 'FromChk'), fromDate: HRM.val(p + 'FromDate'),
            toChecked: HRM.checked(p + 'ToChk'), toDate: HRM.val(p + 'ToDate')
        };
    };
    /** FromDateHistory = today - DefaultDaysToLessFromHistoryFromDate (3 when not configured); ToDateHistory = today. */
    ImpB.historyDefaults = function (p, days) {
        HRM.setVal(p + 'FromDate', HRM.addDays(HRM.today(), -(HRM.int(days) > 0 ? HRM.int(days) : 3)));
        HRM.setVal(p + 'ToDate', HRM.today());
        HRM.check(p + 'FromChk', true); HRM.check(p + 'ToChk', true);
    };
    /** Keeps a date input disabled while its tick box is off (DateTimePicker.ShowCheckBox). */
    ImpB.wireDateChecks = function (p) {
        ['From', 'To'].forEach(function (k) {
            var c = HRM.$(p + k + 'Chk'), d = HRM.$(p + k + 'Date');
            if (!c || !d) return;
            var sync = function () { d.disabled = !c.checked; };
            c.addEventListener('change', sync); sync();
        });
    };

    // ------------------------------------------------------------------ attachments (Attachment form)
    /**
     * state = ImpB.attachmentState(): { existing: [...rows], removed: {id:true}, files: [{name, base64, size}] }.
     * ImpB.attachments({ state, fileUrl(row) }) opens the dialog; ImpB.attachmentPayload(state) is what the save sends.
     */
    ImpB.attachmentState = function () { return { existing: [], removed: {}, files: [] }; };
    ImpB.attachmentPayload = function (st) {
        var changed = st.files.length > 0 || Object.keys(st.removed).length > 0;
        if (!changed) return { files: [] };
        return {
            keep: st.existing.filter(function (r) { return !st.removed[HRM.int(HRM.col(r, 'Id'))]; }).map(function (r) { return HRM.int(HRM.col(r, 'Id')); }),
            files: st.files.map(function (f) { return { name: f.name, base64: f.base64 }; })
        };
    };
    ImpB.attachments = function (o) {
        var st = o.state;
        var m = HRM.modal({ title: 'Attachments', width: 'min(760px, 96vw)',
            html: '<div class="hrm-history-bar"><input type="file" multiple class="impb-att-file"><span class="hrm-history-count">Max 5 MB per file</span></div>' +
                  '<div class="hrm-grid-wrap" style="max-height:60vh"><table class="win-grid hrm-grid impb-att-grid" style="width:100%"></table></div>' +
                  '<div class="hrm-actions"><button type="button" class="win-btn-action impb-att-ok">OK</button></div>' });
        var table = m.body.querySelector('.impb-att-grid');
        function draw() {
            var rows = '<thead><tr><th>File</th><th>Uploaded By</th><th>Status</th><th></th></tr></thead><tbody>';
            st.existing.forEach(function (r) {
                var id = HRM.int(HRM.col(r, 'Id')), gone = !!st.removed[id];
                var name = HRM.str(HRM.col(r, 'UploadedFileCustomName')) || HRM.str(HRM.col(r, 'Attachment'));
                rows += '<tr><td>' + (o.fileUrl ? '<a href="' + HRM.esc(o.fileUrl(r)) + '" target="_blank">' + HRM.esc(name) + '</a>' : HRM.esc(name)) + '</td>' +
                    '<td>' + HRM.esc(HRM.col(r, 'EntryUserName')) + '</td><td>' + (gone ? 'Removed' : 'Saved') + '</td>' +
                    '<td><button type="button" class="win-btn-action hrm-cell-btn" data-ex="' + id + '">' + (gone ? 'Undo' : 'Remove') + '</button></td></tr>';
            });
            st.files.forEach(function (f, i) {
                rows += '<tr><td>' + HRM.esc(f.name) + '</td><td></td><td>New</td><td><button type="button" class="win-btn-action hrm-cell-btn" data-new="' + i + '">Remove</button></td></tr>';
            });
            if (!st.existing.length && !st.files.length) rows += '<tr class="hrm-empty"><td colspan="4">No attachment.</td></tr>';
            table.innerHTML = rows + '</tbody>';
        }
        table.addEventListener('click', function (e) {
            var b = e.target.closest('button'); if (!b) return;
            if (b.hasAttribute('data-ex')) { var id = +b.getAttribute('data-ex'); if (st.removed[id]) delete st.removed[id]; else st.removed[id] = true; }
            if (b.hasAttribute('data-new')) st.files.splice(+b.getAttribute('data-new'), 1);
            draw();
        });
        m.body.querySelector('.impb-att-file').addEventListener('change', function (e) {
            Array.prototype.forEach.call(e.target.files || [], function (file) {
                if (file.size > 5 * 1024 * 1024) { HRM.box('Attachment must be between 1 byte and 5 MB: ' + file.name); return; }
                var rd = new FileReader();
                rd.onload = function () {
                    var s = String(rd.result), k = s.indexOf(',');
                    st.files.push({ name: file.name, base64: k >= 0 ? s.slice(k + 1) : s, size: file.size });
                    draw();
                };
                rd.readAsDataURL(file);
            });
            e.target.value = '';
        });
        m.body.querySelector('.impb-att-ok').addEventListener('click', m.close);
        draw();
    };

    // ------------------------------------------------------------------ Define ... No dialogs
    /**
     * frmProformaDocumentSerial (kind 'proforma': Proforma No + Date) / frmMasterDocumentSerial (kind 'master':
     * LcOrder No + Date, Invoice No + Date). opts: { kind, api, onClose }.
     */
    ImpB.serialDialog = function (o) {
        var master = o.kind === 'master', rec = 0;
        var fields = master
            ? '<div class="hrm-field"><label class="hrm-req" for="sdLcNo">LcOrder No</label><input id="sdLcNo" class="win-textbox" autocomplete="off"></div>' +
              '<div class="hrm-field"><label for="sdLcDate">LcOrder Date</label><input id="sdLcDate" type="date" class="win-textbox"></div>' +
              '<div class="hrm-field"><label class="hrm-req" for="sdInvNo">Invoice No</label><input id="sdInvNo" class="win-textbox" autocomplete="off"></div>' +
              '<div class="hrm-field"><label for="sdInvDate">Invoice Date</label><input id="sdInvDate" type="date" class="win-textbox"></div>'
            : '<div class="hrm-field"><label class="hrm-req" for="sdPfNo">Proforma No</label><input id="sdPfNo" class="win-textbox" autocomplete="off"></div>' +
              '<div class="hrm-field"><label for="sdPfDate">Proforma Date</label><input id="sdPfDate" type="date" class="win-textbox"></div>';
        var m = HRM.modal({ title: master ? 'Master Document Serial' : 'Proforma Document Serial', width: 'min(640px, 96vw)', onClose: o.onClose,
            html: '<div class="win-tool-strip"><button type="button" class="win-btn-tool sd-new"><i class="fa fa-plus"></i> <span>New</span></button>' +
                  '<button type="button" class="win-btn-tool sd-save"><i class="fa fa-save"></i> <span>Save</span></button>' +
                  '<button type="button" class="win-btn-tool sd-update is-hidden"><i class="fa fa-pencil"></i> <span>Update</span></button></div>' +
                  '<div class="hrm-entry hrm-cols-2">' + fields + '</div>' +
                  '<div class="hrm-grid-box hrm-grid-box-modal"><div class="hrm-grid-wrap"><table class="sd-grid"></table></div></div>' });
        var b = m.body, $ = function (s) { return b.querySelector(s); };
        var cols = master
            ? [{ key: 'Edit', caption: 'Edit', width: 40, render: function () { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="edit">Edit</button>'; } },
               { key: 'Id', hidden: true }, { key: 'InvoiceMasterId', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'LocOrderMasterId', hidden: true },
               { key: 'LocOrderNo', caption: 'LocOrderNo' }, { key: 'LocOrderDate', caption: 'LocOrderDate', type: 'date' },
               { key: 'InvoiceDate', caption: 'InvoiceDate', type: 'date' }, { key: 'InvoiceNo', caption: 'InvoiceNo' },
               { key: 'CreatedUserName', caption: 'CreatedUserName' }, { key: 'CreatedOn', caption: 'CreatedOn', type: 'datetime' },
               { key: 'LastModifyUserName', caption: 'LastModifyUserName' }, { key: 'LastModifiedOn', caption: 'LastModifiedOn', type: 'datetime' }]
            : [{ key: 'Edit', caption: 'Edit', width: 40, render: function () { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="edit">Edit</button>'; } },
               { key: 'Id', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'ProformaMasterId', hidden: true },
               { key: 'ProformaNo', caption: 'ProformaNo' }, { key: 'ProformaDate', caption: 'ProformaDate', type: 'date' },
               { key: 'CreatedUserName', caption: 'CreatedUserName' }, { key: 'CreatedOn', caption: 'CreatedOn', type: 'datetime' },
               { key: 'LastModifyUserName', caption: 'LastModifyUserName' }, { key: 'LastModifiedOn', caption: 'LastModifiedOn', type: 'datetime' }];
        var g = new HRM.Grid($('.sd-grid'), { columns: cols, filterRow: true, onDouble: read });
        $('.sd-grid').addEventListener('click', function (e) {
            var btn = e.target.closest('button[data-act]'); if (!btn) return;
            var tr = btn.closest('tr[data-i]'); if (tr) read(g.rows()[+tr.getAttribute('data-i')]);
        });
        function buttons(upd) { $('.sd-save').classList.toggle('is-hidden', upd); $('.sd-update').classList.toggle('is-hidden', !upd); }
        function load() { return HRM.get(o.api).then(function (rows) { g.set(rows || []); }).catch(HRM.fail); }
        function reset() {
            rec = 0; buttons(false);
            if (master) { HRM.setVal('sdLcNo', ''); HRM.setVal('sdInvNo', ''); } else HRM.setVal('sdPfNo', '');
            return load();
        }
        function read(r) {                                                // ReadById(): the row into the fields
            rec = HRM.int(HRM.col(r, 'Id'));
            if (master) {
                HRM.setVal('sdLcNo', HRM.str(HRM.col(r, 'LocOrderNo'))); HRM.setVal('sdLcDate', HRM.day(HRM.col(r, 'LocOrderDate')));
                HRM.setVal('sdInvNo', HRM.str(HRM.col(r, 'InvoiceNo'))); HRM.setVal('sdInvDate', HRM.day(HRM.col(r, 'InvoiceDate')));
            } else { HRM.setVal('sdPfNo', HRM.str(HRM.col(r, 'ProformaNo'))); HRM.setVal('sdPfDate', HRM.day(HRM.col(r, 'ProformaDate'))); }
            buttons(true);
        }
        function insert(btn, upd) {
            if (upd && rec === 0) { HRM.box('RecId Not Found...'); return; }
            if (!upd) rec = 0;
            if (master) {
                var lc = HRM.val('sdLcNo'), inv = HRM.val('sdInvNo');
                if (lc === '' || lc === '0') { HRM.box('LcOrder No Is Required'); HRM.focus('sdLcNo'); return; }
                if (inv === '' || inv === '0') { HRM.box('InvoiceNo Field Is Required'); HRM.focus('sdInvNo'); return; }
                if (HRM.val('sdInvDate') > HRM.val('sdLcDate')) { HRM.box('InvoiceDate Can  not Less than Lc Order Date Field Is Required'); HRM.focus('sdInvDate'); return; }
            } else {
                var pf = HRM.val('sdPfNo');
                if (pf === '' || pf === '0') { HRM.box('ProformaNo No Is Required'); HRM.focus('sdPfNo'); return; }
            }
            if (!HRM.ask(rec > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var body = master
                ? { id: rec, locOrderNo: HRM.val('sdLcNo'), locOrderDate: HRM.val('sdLcDate'), invoiceNo: HRM.val('sdInvNo'), invoiceDate: HRM.val('sdInvDate') }
                : { id: rec, proformaNo: HRM.val('sdPfNo'), proformaDate: HRM.val('sdPfDate') };
            return HRM.busy(btn, function () {
                return HRM.post(o.api + '/save', body).then(function (d) { HRM.box(d && d.message); return reset(); }).catch(HRM.fail);
            });
        }
        $('.sd-new').addEventListener('click', reset);
        $('.sd-save').addEventListener('click', function (e) { insert(e.currentTarget, false); });
        $('.sd-update').addEventListener('click', function (e) { insert(e.currentTarget, true); });
        m.el.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            if (e.ctrlKey && k === 's' && !$('.sd-save').classList.contains('is-hidden')) { e.preventDefault(); insert($('.sd-save'), false); }
            if (e.ctrlKey && k === 'u' && !$('.sd-update').classList.contains('is-hidden')) { e.preventDefault(); insert($('.sd-update'), true); }
            if (e.ctrlKey && k === 'e') { e.preventDefault(); m.close(); }
        });
        var today = HRM.today();
        if (master) { HRM.setVal('sdLcDate', today); HRM.setVal('sdInvDate', today); HRM.focus('sdLcNo'); }
        else { HRM.setVal('sdPfDate', today); HRM.focus('sdPfNo'); }
        load();
        return m;
    };

    /** UltraCombo.Leave: `change` on the hidden select or focus leaving the combo wrap. */
    ImpB.onLeave = function (id, fn) {
        var sel = HRM.$(id); if (!sel) return;
        /* A pick made while the combo has focus is followed by the Leave itself; a change made without focus
           (touch pick, script) is treated as the Leave. */
        sel.addEventListener('change', function () {
            var w = sel.closest('.dtcombo-wrap');
            if (w && w.contains(document.activeElement)) return;
            fn();
        });
        document.addEventListener('focusout', function (e) {
            var w = e.target && e.target.closest ? e.target.closest('.dtcombo-wrap') : null;
            if (!w || !w.contains(sel)) return;
            if (e.relatedTarget && w.contains(e.relatedTarget)) return;
            setTimeout(fn, 0);
        });
    };

    /** The page's print buttons: the desktop's CommonServices slip is not traceable - the server says so. */
    ImpB.print = function (api, id, btn) {
        return HRM.busy(btn, function () { return HRM.get(api + '/print', { id: id || 0 }).catch(HRM.fail); });
    };

    // ------------------------------------------------------------------ Qty/M.Ton <-> NoOfBags <-> Amount (both invoice forms)
    /**
     * The detail entry arithmetic of frmProformaInvoice / frmImportInvoice:
     *   txtQtyMTon_TextChanged  Weight(kg) = Qty/M.Ton × 1000; CalculateWeight(); CalculatAmount()
     *   txtNoOfBags_TextChanged CalculateWeight()
     *   CmbPackUom_TextChanged  CalculateWeight() (+ CalculatAmount() on the Import Invoice)
     *   CmbRateUom_TextChanged  CalculatAmount() (+ CalculateWeight() on the Import Invoice)
     *   txtCostMTon_TextChanged CalculatAmount()
     * CalculateWeight: typing in Qty/M.Ton sets NoOfBags = Math.Round(MTon × 1000 / PackEquivalent); typing in NoOfBags sets
     * Qty/M.Ton = Bags × PackEquivalent / 1000 ("#,##0.###"); otherwise the last typed box (AccessibleName) drives. No Pack
     * Size -> both 0. CalculatAmount: Amount = Rate / RateUomEquivalent × MTon × 1000, ToString("0,0"); "0" otherwise.
     * o: { uoms: () => rows, importInvoice: bool }
     */
    ImpB.qtyEntry = function (o) {
        var flagMton = '', flagBags = '';
        function eq(id) {
            var v = HRM.comboVal(id), out = null;
            (o.uoms() || []).forEach(function (u) { if (HRM.int(HRM.col(u, 'Id')) === v) out = HRM.num(HRM.col(u, 'Equivalent')); });
            return v ? out : null;
        }
        function weight() { var q = HRM.num(HRM.val('txtQtyMTon')); HRM.setVal('txtWeightDetail', q > 0 ? String(Math.round(q * 1000 * 1e6) / 1e6) : '0'); }
        function qtyChanged(tag) { weight(); calcWeight(tag); calcAmount(); }
        function calcWeight(tag) {
            var mton = HRM.num(HRM.val('txtQtyMTon')), bags = HRM.num(HRM.val('txtNoOfBags')), outer = eq('CmbPackUom');
            if (outer === null) {
                HRM.setVal('txtNoOfBags', '0');
                if (HRM.val('txtQtyMTon') !== '0') { HRM.setVal('txtQtyMTon', '0'); qtyChanged(tag); }
                return;
            }
            if (tag === 'Mton') {
                HRM.setVal('txtNoOfBags', String(ImpB.roundEven(mton > 0 ? mton * 1000 / outer : 0, 0)));
                flagMton = 'True'; flagBags = 'False';
            } else if (tag === 'NoOfBags') {
                mton = bags > 0 ? bags * outer / 1000 : 0;
                flagBags = 'True'; flagMton = 'False';
                setMton(ImpB.fmt(mton, 3, 0), tag);
            } else if (flagMton === 'True') {
                HRM.setVal('txtNoOfBags', String(ImpB.roundEven(mton > 0 ? mton * 1000 / outer : 0, 0)));
            } else if (flagBags === 'True') {
                mton = bags > 0 ? bags * outer / 1000 : 0;
                setMton(ImpB.fmt(mton, 3, 0), tag);
            }
        }
        function setMton(text, tag) { if (HRM.val('txtQtyMTon') === text) return; HRM.setVal('txtQtyMTon', text); weight(); calcAmount(); }
        function calcAmount() {
            HRM.setVal('txtAmount', '0');
            var req = eq('CmbRateUom');
            if (req === null) return;
            var rate = HRM.num(HRM.val('txtCostMTon')), q = HRM.num(HRM.val('txtQtyMTon'));
            if (rate > 0 && req > 0 && q > 0) HRM.setVal('txtAmount', ImpB.fmtInt(rate / req * (q * 1000)));
        }
        HRM.$('txtQtyMTon').addEventListener('input', function () { qtyChanged('Mton'); });
        HRM.$('txtNoOfBags').addEventListener('input', function () { calcWeight('NoOfBags'); });
        HRM.$('CmbPackUom').addEventListener('change', function () { calcWeight(null); if (o.importInvoice) calcAmount(); });
        HRM.$('CmbRateUom').addEventListener('change', function () { if (o.importInvoice) calcWeight(null); calcAmount(); });
        HRM.$('txtCostMTon').addEventListener('input', calcAmount);
        return { weight: weight, calcWeight: calcWeight, calcAmount: calcAmount, reset: function () { flagMton = ''; flagBags = ''; } };
    };

    global.ImpB = ImpB;
})(window);
