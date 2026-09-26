/* ============================================================================================
 * 460 "General Gate Pass  Report" — frmGatePassGeneral.cs (a register, no save/update/delete).
 * Filters → Sp_GatePassGeneral_Register; grid as gridHisory (:220) / GridSetting (:276); Slip
 * button → 254 (mouse, :359) or 290 (Ctrl+Space, :616); "255-Register" prints the last rows (:337).
 *
 * Opening parameters (the public fields other desktop forms set before Show()):
 *   ?documentTypeId=52|92   documentTypeId
 *   ?onlyPending=1          OnlyPending  (caption gets " (Pending)", @OnlyPending = 1)
 *   ?fromDate=..&toDate=..  FromDatePublic / ToDatePublic — From becomes the FY start (sic), To = toDate, then Show.
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci;
    var API = '/api/store/reports/general-gate-pass';
    var look = {};
    var dtReg = [];         // frmGatePassGeneral.dtReg — the raw rows of the last search
    var gridRows = [];
    var opts = { documentTypeId: 0, onlyPending: false, fromDate: '', toDate: '' };
    var filters = {};
    var searchSeq = 0;

    /** Conversion.ToInt(text) — Convert.ToInt32 inside try/catch: a value that does not fit an
        Int32 becomes 0 (no filter) instead of an error. */
    function convToInt(s) {
        var t = String(s || '').trim();
        if (!/^\d+$/.test(t)) return 0;
        var n = parseInt(t, 10);
        return n > 2147483647 ? 0 : n;
    }

    function fmt2(v) {
        return C.num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 });
    }

    /* Slip first and frozen (:297 Position 0, FrozenColumns 1); Id / DocumentTypeId hidden (:280).
       Captions are the DataTable column names (:241). */
    var COLS = [
        { key: 'Slip', button: 'Slip' },
        { key: 'GPSrNo' },                                    // no alignment set by GridSetting (Janus default)
        { key: 'DeliveryOrderNo' },
        { key: 'gatePassType' },
        { key: 'GpDate', date: true },
        { key: 'PartyName' },
        { key: 'VarietyName' },
        { key: 'ItemQty', num: true, total: true },          // :282 Sum "#,##0.##"
        { key: 'FactoryWeight', num: true, total: true },    // :286
        { key: 'WareHouseName' },
        { key: 'WeightStatus' },
        { key: 'VehicleType' },
        { key: 'VehicleNo' },
        { key: 'DocAttachment' },
        { key: 'OtherRemarks' }
    ];

    /* ------------------------------------------------------------------ load */
    function init() {
        var p = new URLSearchParams(window.location.search);
        opts.documentTypeId = C.intOf(p.get('documentTypeId'));
        opts.onlyPending = p.get('onlyPending') === '1' || p.get('onlyPending') === 'true';
        opts.fromDate = C.isoDay(p.get('fromDate'));
        opts.toDate = C.isoDay(p.get('toDate'));
        onlyDigits('GpsNoFrom'); onlyDigits('GpsNoTo');                                   // :535 / :547
        /* DateTimePicker default value: now. */
        $id('fromdate').value = C.today();
        $id('ToDate').value = C.today();
        C.getJson(API + '/lookups').then(function (d) {
            look = d || {};
            statusFill(); weightStatus(); documentTypeFill();                            // frmGPOutward_Load :120-122
            datetypefill();                                                               // :123 — fires ValueChanged → Show with Document Type still empty
            if (opts.documentTypeId === 0) $id('cmbDocumentType').selectedIndex = 1;     // :127 Rows[0].Activate() (index 0 is the empty text)
            else $id('cmbDocumentType').value = String(opts.documentTypeId);             // :131
            $id('cmbDateType').focus();                                                   // :124
            if (opts.onlyPending) $id('label23').textContent += ' (Pending)';            // :135
            if (opts.fromDate && opts.toDate) {                                          // :137
                $id('fromdate').value = C.isoDay(look.financialYearStart) || $id('fromdate').value;
                $id('ToDate').value = opts.toDate;
                show();
            }
        }).catch(function (e) { alert(e.message); });
        document.addEventListener('keydown', function (e) {                               // frmGPOutward_KeyDown :450 (KeyPreview)
            if (document.querySelector('.cx-modal.is-open')) return;
            if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { shortcutKeys(); return; }   // :486
            if (!e.ctrlKey || e.altKey) return;
            var k = (e.key || '').toLowerCase();
            if (k === 'p') { e.preventDefault(); print255(); }                           // :454
            else if (k === 's') { e.preventDefault(); show(); }                          // :458
            else if (k === 'r') { e.preventDefault(); refresh(); }                       // :462
            else if (k === 'n') { e.preventDefault(); reset(); }                         // :470
            else if (e.key === 'F5' || e.key === 'ArrowUp') { e.preventDefault(); $id('cmbDateType').focus(); }  // :474 / :478
            else if (e.key === 'ArrowDown') { e.preventDefault(); focusGrid(); }         // :482
        });
        render();
    }

    function focusGrid() {
        var b = document.querySelector('#DataGridHistory tbody button');
        if (b) b.focus(); else $id('DataGridHistory').scrollIntoView({ block: 'nearest' });
    }

    /** btnshortcut_Click → MakeShortCutKeys (:497). */
    function shortcutKeys() {
        C.pickFrom('ShortCut Keys', [
            { KeyCombination: 'Ctrl+S', Description: 'For Show' },
            { KeyCombination: 'Ctrl+E', Description: 'For Close' },
            { KeyCombination: 'Ctrl+R', Description: 'For Refresh' },
            { KeyCombination: 'Ctrl+N', Description: 'For New' },
            { KeyCombination: 'Ctrl + P', Description: 'For print' },
            { KeyCombination: 'Ctrl+alt', Description: 'To Show ShortCut Keys Form' },
            { KeyCombination: 'Ctrl+ArrowDown', Description: 'For Focus On Grid' },
            { KeyCombination: 'Ctrl+ArrowUp', Description: 'For Focus On DateType in Filter' },
            { KeyCombination: 'Ctrl+Space', Description: "When Focus On Any Grid To Call Function's On Button Or Link" }
        ], [{ key: 'KeyCombination', caption: 'KeyCombination' }, { key: 'Description', caption: 'Description' }]);
    }

    /** The footer's History button — the grid is on the same screen, under the filters. */
    function gotoHistory() {
        $id('historySection').scrollIntoView({ behavior: 'smooth', block: 'start' });
        focusGrid();
    }

    function onlyDigits(id) {
        $id(id).addEventListener('input', function () {
            var v = this.value.replace(/\D/g, '');
            if (v !== this.value) this.value = v;
        });
    }

    /* The three fixed lists are bound with ZeroIndex false; a leading empty option stands for the
       combo's empty Text (reset() sets Text = string.Empty). */
    function bindFixed(id, rows, textKey) {
        var el = $id(id), keep = el.value;
        el.innerHTML = '<option value=""></option>' + (rows || []).map(function (r) {
            return '<option value="' + esc(r.Id) + '">' + esc(r[textKey]) + '</option>';
        }).join('');
        if (keep) el.value = keep;
    }
    function statusFill() { bindFixed('cmbStatus', look.statuses, 'Status'); }                    // :183
    function weightStatus() { bindFixed('cmbWeightStatus', look.weightStatuses, 'WeightStatus'); } // :165
    function documentTypeFill() { bindFixed('cmbDocumentType', look.documentTypes, 'DocumentType'); } // :202

    /** Datetypefill (:150) — ZeroIndex true (a blank first row), then Rows[2] ("This Week") activated. */
    function datetypefill() {
        var el = $id('cmbDateType');
        el.innerHTML = '<option value="0">...Select Any Value...</option>' + (look.dateTypes || []).map(function (r) {
            return '<option value="' + esc(r.Id) + '">' + esc(r.Parameters) + '</option>';
        }).join('');
        el.selectedIndex = 2;
        dateTypeChanged();
    }

    function iso(d) {
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }

    /** cmbDateType_ValueChanged (:559). */
    function dateTypeChanged() {
        var v = C.intOf($id('cmbDateType').value), now = new Date();
        if (v === 1) {
            $id('fromdate').value = C.today();
            show();
        } else if (v === 2) {
            var w = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 7);
            $id('fromdate').value = iso(w);
            show();
        } else if (v === 3) {
            /* DateTime.UtcNow's month, first day. */
            $id('fromdate').value = now.getUTCFullYear() + '-' + String(now.getUTCMonth() + 1).padStart(2, '0') + '-01';
            $id('ToDate').value = C.today();
            show();
        } else if (v === 4) {
            $id('fromdate').value = now.getFullYear() + '-01-01';
            $id('ToDate').value = C.today();
            show();
        } else if (v === 5) {
            $id('fromdate').value = C.isoDay(look.financialYearStart) || $id('fromdate').value;
            show();
        }
    }

    function textOf(id) {
        var el = $id(id);
        return el.selectedIndex >= 0 && el.value !== '' ? el.options[el.selectedIndex].text : '';
    }

    /* ------------------------------------------------------------------ toolbar */
    /** btnshow_Click → gridHisory (:220). */
    function show() {
        var q = {
            fromDate: $id('fromdate').value, toDate: $id('ToDate').value,
            gpNoFrom: convToInt($id('GpsNoFrom').value), gpNoTo: convToInt($id('GpsNoTo').value),   // :231 / :232
            status: textOf('cmbStatus'),                                          // :233 Text
            documentTypeId: C.intOf($id('cmbDocumentType').value),                // :234 Value
            weightStatus: textOf('cmbWeightStatus'),                              // :235 Text
            onlyPending: opts.onlyPending ? 'true' : ''
        };
        var mine = ++searchSeq;   /* Load can start two searches back to back; only the last one's rows count */
        C.getJson(API + '/search' + C.qs(q)).then(function (rows) {
            if (mine !== searchSeq) return;
            dtReg = rows || [];
            gridRows = dtReg.map(function (r) {                                   // :259
                return {
                    Id: C.intOf(ci(r, 'Id')), DocumentTypeId: C.intOf(ci(r, 'DocumentTypeId')),
                    GPSrNo: C.intOf(ci(r, 'GPSrNo')), DeliveryOrderNo: String(C.intOf(ci(r, 'DeliveryOrderNo'))),
                    gatePassType: str(ci(r, 'gatePassType')), GpDate: C.isoDay(ci(r, 'GpDate')),
                    PartyName: str(ci(r, 'PartyName')), VarietyName: str(ci(r, 'VarietyName')),
                    ItemQty: C.num(ci(r, 'ItemQty')), FactoryWeight: C.num(ci(r, 'FactoryWeight')),
                    WareHouseName: str(ci(r, 'WareHouseName')), WeightStatus: str(ci(r, 'WeightStatus')),
                    VehicleType: str(ci(r, 'VehicleType')), VehicleNo: str(ci(r, 'VehicleNo')),
                    DocAttachment: str(ci(r, 'DocAttachment')), OtherRemarks: str(ci(r, 'OtherRemarks'))
                };
            });
            filters = {};
            render();                                                              // empty → ClearStructure (:267)
        }).catch(function (e) { alert(e.message); });
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }

    /** btnRefresh_Click (:394). */
    function refresh() { documentTypeFill(); weightStatus(); statusFill(); }

    /** btnNew_Click → reset (:420). */
    function reset() {
        $id('GpsNoFrom').value = '';
        $id('GpsNoTo').value = '';
        $id('cmbStatus').value = '';
        $id('cmbDocumentType').value = '';
        $id('cmbWeightStatus').value = '';
        $id('cmbDateType').focus();
        datetypefill();
    }

    /** btnPrint_Click "255-Register" (:337) — dtReg into 255-GatePassGeneralRpt.rpt. No Crystal
        contract exists for 255 (procedure-only in the report catalogue), so the rows are printed. */
    function print255() {
        if (!dtReg.length) { alert('Record Not Found For Display'); return; }
        printRows(dtReg, '255-GatePassGeneralRpt');
    }

    /* ------------------------------------------------------------------ grid */
    function cellText(c, r) {
        if (c.button) return c.button;
        var v = r[c.key];
        if (c.date) return v ? C.gridDate(v) : '';
        if (c.num) return fmt2(v);
        return str(v);
    }
    function visibleRows() {
        return gridRows.filter(function (r) {
            return COLS.every(function (c) {
                var f = filters[c.key];
                if (!f || c.button) return true;
                return cellText(c, r).toLowerCase().indexOf(f.toLowerCase()) >= 0;
            });
        });
    }
    function render() {
        var t = $id('DataGridHistory');
        if (!gridRows.length) {
            t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = '';
            $id('DataGridHistoryNavigator').innerHTML = '&nbsp;';
            return;
        }
        t.querySelector('thead').innerHTML =
            '<tr>' + COLS.map(function (c, i) { return '<th' + (i === 0 ? ' class="cx-frozen"' : '') + '>' + esc(c.key) + '</th>'; }).join('') + '</tr>' +
            '<tr class="cx-filter-row">' + COLS.map(function (c, i) {
                return c.button ? '<th class="cx-frozen"></th>' : '<th><input type="text" class="win-textbox" data-f="' + esc(c.key) +
                    '" value="' + esc(filters[c.key] || '') + '"></th>';
            }).join('') + '</tr>';
        t.querySelectorAll('thead input[data-f]').forEach(function (inp) {
            inp.oninput = function () { filters[inp.getAttribute('data-f')] = inp.value; body(); };
        });
        body();
    }
    function body() {
        var t = $id('DataGridHistory'), rows = visibleRows();
        t.querySelector('tbody').innerHTML = rows.map(function (r) {
            return '<tr>' + COLS.map(function (c) {
                if (c.button) {
                    return '<td class="cx-frozen"><button type="button" class="win-btn-small" data-i="' + gridRows.indexOf(r) +
                        '" title="Click: 254 slip · Ctrl+Space: 290 slip">Slip</button></td>';
                }
                return '<td' + (c.num ? ' class="num" style="text-align:right"' : '') + '>' + esc(cellText(c, r)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.querySelector('tfoot').innerHTML = '<tr>' + COLS.map(function (c, i) {
            if (!c.total) return '<td' + (i === 0 ? ' class="cx-frozen"' : '') + '></td>';
            var s = rows.reduce(function (a, r) { return a + C.num(r[c.key]); }, 0);
            return '<td class="num" style="text-align:right"><b>' + esc(fmt2(s)) + '</b></td>';
        }).join('') + '</tr>';
        t.querySelectorAll('tbody button[data-i]').forEach(function (b) {
            var r = gridRows[parseInt(b.getAttribute('data-i'), 10)];
            b.onclick = function () {
                if (b._ctrlSpace) { b._ctrlSpace = false; return; }   /* the keyup of Ctrl+Space must not also print 254 */
                slip254(r);
            };
            b.onkeydown = function (e) {                                            // DataGridHistory_KeyDown (:616)
                if (e.ctrlKey && (e.key === ' ' || e.code === 'Space')) {
                    e.preventDefault(); b._ctrlSpace = true;
                    setTimeout(function () { b._ctrlSpace = false; }, 400);
                    slip290(r);
                }
            };
        });
        $id('DataGridHistoryNavigator').textContent = 'Rows: ' + rows.length + (rows.length !== gridRows.length ? ' of ' + gridRows.length : '');
    }

    /** DataGridHistory_ColumnButtonClick (:359) — Cells[1] DocumentTypeId, Cells[0] Id. */
    function slip254(r) {
        crystalOrRows('254-rptinwardgatepassslipgeneral', { id: r.Id, documentTypeId: r.DocumentTypeId },
            API + '/slip-254' + C.qs({ id: r.Id, documentTypeId: r.DocumentTypeId }), '254-RptInwardGatePassSlipGeneral');
    }
    /** Ctrl+Space on Slip (:639). KNOWN DESKTOP DEFECT, reproduced: the GatePassGeneral row's Id
        is sent to Sp_GatePassOutward_SlipAndRegister_Rpt, which filters dbo.GatePassOutward by Id —
        the wrong table — so it prints whichever outward gate pass shares that Id, or "Record Not
        Found For Display". The seeded Crystal contract for 290 binds @Id to "arg:object" (a
        seeding defect), so this one prints its rows directly rather than a PDF missing @Id. */
    function slip290(r) {
        C.getJson(API + '/slip-290' + C.qs({ id: r.Id })).then(function (rows) {
            printRows(rows, '290-InvRptOutwardGatePassSlip');
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ printing */
    function crystalOrRows(key, args, rowsUrl, title) {
        var headers = { 'Content-Type': 'application/json', Accept: 'application/pdf' };
        var token = document.querySelector('meta[name="_csrf"]'), header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header) headers[header.getAttribute('content')] = token.getAttribute('content');
        C.getJson(rowsUrl).then(function (rows) {
            return fetch('/api/reports/' + encodeURIComponent(key) + '/print.pdf', {
                method: 'POST', credentials: 'same-origin', headers: headers, body: JSON.stringify(args)
            }).then(function (res) {
                var ct = res.headers.get('Content-Type') || '';
                if (res.ok && ct.indexOf('application/pdf') >= 0) {
                    return res.blob().then(function (b) { window.open(URL.createObjectURL(b), '_blank'); });
                }
                printRows(rows, title);
            }, function () { printRows(rows, title); });
        }).catch(function (e) { alert(e.message); });
    }

    function printRows(rows, title) {
        if (!rows || !rows.length) { alert('Record Not Found For Display'); return; }
        var cols = Object.keys(rows[0]);
        var w = window.open('', '_blank');
        if (!w) { alert('Allow pop-ups to print.'); return; }
        w.document.write('<html><head><title>' + esc(title) + '</title><style>body{font-family:Verdana;font-size:10px}' +
            'table{border-collapse:collapse}td,th{border:1px solid #444;padding:2px 4px;white-space:nowrap}</style></head><body>' +
            '<h3>' + esc(title) + '</h3><table><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') +
            '</tr></thead><tbody>' + rows.map(function (r) {
                return '<tr>' + cols.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>';
            }).join('') + '</tbody></table><script>window.print()<\/script></body></html>');
        w.document.close();
    }

    document.addEventListener('DOMContentLoaded', init);
    window.RptGatePassGeneral = { show: show, reset: reset, refresh: refresh, print255: print255,
                                  dateTypeChanged: dateTypeChanged, shortcutKeys: shortcutKeys, gotoHistory: gotoHistory };
})();
