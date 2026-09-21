/* ============================================================================================
 * Stock Conversion - invfrmStockConversionProduction.cs, DocTypeId 66.
 *
 * READ SIDE ONLY, and that is deliberate rather than unfinished.
 *
 * The desktop's Save writes, in one transaction: the header and its three child tables, inventory
 * stock evaluation, inventory transactions, an accounting voucher with a balance check, and
 * contractor wages bills - behind usp_StockConversionValidation and USP_InventoryValidation.
 * Three of those cannot be reproduced from the desktop call sites alone, so the server answers
 * 501 to a save and this file does not pretend otherwise: the Save button says so when clicked
 * rather than posting a document with no stock movement behind it.
 *
 * Everything else - load by document number, the three grids, the history grid with its optional
 * filters, clickable document codes, the button-loading contract, full-width tables that scroll
 * inside their own container - works exactly as on the other ported screens.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/stock-conversion';
    var RECID = 0;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m; }
    function box(m) { window.alert(m); }

    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function num(v) {
        var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function int(v) {
        var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10);
        return isNaN(n) ? 0 : n;
    }
    function fmt(n) {
        return (Math.round(n * 100) / 100).toLocaleString('en-US', { maximumFractionDigits: 2 });
    }

    /* Case-insensitive column read: a procedure's casing is not guaranteed, and a grid that
       silently shows blanks because the column was "ItemName" not "itemname" is the hardest kind
       of bug to see. */
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) {
                return row[k];
            }
        }
        return '';
    }

    /** yyyy-MM-dd without toISOString, which shifts the day in any timezone behind UTC. */
    function dateOnly(v) {
        if (!v) return '';
        var s = String(v);
        var m = s.match(/^(\d{4})-(\d{2})-(\d{2})/);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        if (isNaN(d.getTime())) return '';
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0')
             + '-' + String(d.getDate()).padStart(2, '0');
    }
    function today() {
        var d = new Date();
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0')
             + '-' + String(d.getDate()).padStart(2, '0');
    }

    /* The button contract, one place: disabled at once, spinner, duplicate clicks ignored,
       re-enabled on success AND on failure. */
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }

    function getJson(url) {
        return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' })
            .then(function (r) {
                return r.text().then(function (t) {
                    var body = null;
                    try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                    if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
                    return body;
                });
            });
    }

    // ------------------------------------------------------------------------------ lookups

    function loadEntryTypes() {
        return getJson(api + '/entry-types').then(function (rows) {
            var sel = $id('cmbEntryType');
            if (!sel) return;
            var html = '<option value="0"></option>';
            (rows || []).forEach(function (r) {
                html += '<option value="' + esc(col(r, 'Id')) + '">' + esc(col(r, 'EntryType')) + '</option>';
            });
            sel.innerHTML = html;
            if (window.DesktopCombo) window.DesktopCombo.refresh();
        });
    }

    // --------------------------------------------------------------------------------- load

    function loadByDocNo() {
        var docNo = int(val('txtLoadDocNo'));
        if (docNo <= 0) { box('Enter a document number'); return; }
        return busy('btnLoad', function () {
            say('Loading...');
            return getJson(api + '/id-by-doc-no?docSrNo=' + docNo).then(function (d) {
                var id = d && d.id ? d.id : 0;
                if (id <= 0) { say('Not found.'); box('No Stock Conversion with document number ' + docNo); return; }
                return load(id);
            }).catch(function (e) { say('Not loaded.'); box(e.message); });
        });
    }

    function load(id) {
        return getJson(api + '/' + id).then(function (d) {
            if (!d || !d.header) { box('Stock Conversion ' + id + ' not found.'); return; }
            var h = d.header;
            RECID = int(col(h, 'Id'));

            setVal('txtdocnumber', col(h, 'DocSrNo'));
            $id('lblDocNo').textContent = col(h, 'DocSrNo') || '-';
            setVal('txtDocdate', dateOnly(col(h, 'DocDate')));
            setVal('txtDocManualRef', col(h, 'DocManualRef'));
            setVal('txtProductionNo', col(h, 'ProductionNo'));
            setVal('txtRemarks', col(h, 'Remarks'));
            /* The header returns ids; their descriptions come from whatever the procedure
               projects alongside them. Shown read-only rather than guessed at. */
            setVal('txtEBDepartment', col(h, 'WareHouseName') || col(h, 'EBDepartmentId'));
            setVal('txtParentCategory', col(h, 'InvParentCateDescription') || col(h, 'parentCategoryId'));
            setVal('txtDifferenceAccount', col(h, 'AccountTitle') || col(h, 'DifferenceAccountId'));

            renderDetails(d.details || []);
            renderPackings(d.packings || []);
            renderExpenses(d.expenses || []);

            showView('form');
            say('Document ' + col(h, 'DocSrNo') + ' loaded.');
        });
    }

    // -------------------------------------------------------------------------------- grids

    function renderDetails(rows) {
        var html = '', tQty = 0, tWeight = 0, tAmount = 0;
        rows.forEach(function (r) {
            tQty += num(col(r, 'Qty'));
            tWeight += num(col(r, 'Weight'));
            tAmount += num(col(r, 'Amount'));
            html += '<tr>'
                 + '<td>' + esc(col(r, 'EntryType')) + '</td>'
                 + '<td>' + esc(col(r, 'ItemName')) + '</td>'
                 + '<td>' + esc(col(r, 'WareHouseName')) + '</td>'
                 + '<td>' + esc(col(r, 'JobLotDescription')) + '</td>'
                 + '<td>' + esc(col(r, 'CropBatch')) + '</td>'
                 + '<td style="text-align:right;">' + esc(col(r, 'Qty')) + '</td>'
                 + '<td>' + esc(col(r, 'UomCode')) + '</td>'
                 + '<td style="text-align:right;">' + esc(col(r, 'Weight')) + '</td>'
                 + '<td style="text-align:right;">' + esc(col(r, 'Rate')) + '</td>'
                 + '<td>' + esc(col(r, 'RateUom')) + '</td>'
                 + '<td style="text-align:right;">' + esc(col(r, 'Amount')) + '</td>'
                 + '<td style="text-align:right;">' + esc(col(r, 'Moisture')) + '</td>'
                 + '<td>' + esc(col(r, 'MoistureSlabDescription')) + '</td>'
                 + '<td>' + esc(col(r, 'ItemCondition')) + '</td>'
                 + '<td>' + esc(col(r, 'rackName')) + '</td>'
                 + '<td>' + esc(col(r, 'Remarks')) + '</td>'
                 + '</tr>';
        });
        $id('gridDetail').innerHTML = html;
        $id('lblDetailTotals').textContent =
            'Qty ' + fmt(tQty) + '   Weight ' + fmt(tWeight) + '   Amount ' + fmt(tAmount) + '   ';
    }

    function renderPackings(rows) {
        var html = '', tQty = 0, tAmount = 0;
        rows.forEach(function (r) {
            tQty += num(col(r, 'ItemQty'));
            tAmount += num(col(r, 'ItemAmount'));
            html += '<tr>'
                 + '<td>' + esc(col(r, 'ItemName')) + '</td>'
                 + '<td>' + esc(col(r, 'WarehouseName')) + '</td>'
                 + '<td>' + esc(col(r, 'UOMDescription')) + '</td>'
                 + '<td>' + esc(col(r, 'BrandName')) + '</td>'
                 + '<td>' + esc(col(r, 'BrandUom')) + '</td>'
                 + '<td style="text-align:right;">' + esc(col(r, 'ItemQty')) + '</td>'
                 + '<td style="text-align:right;">' + esc(col(r, 'ItemRate')) + '</td>'
                 + '<td style="text-align:right;">' + esc(col(r, 'ItemAmount')) + '</td>'
                 + '<td>' + esc(col(r, 'ItemCondition')) + '</td>'
                 + '<td>' + esc(col(r, 'rackName')) + '</td>'
                 + '<td>' + esc(col(r, 'ChargeTo')) + '</td>'
                 + '</tr>';
        });
        $id('gridPacking').innerHTML = html;
        $id('lblPackingTotals').textContent = 'Qty ' + fmt(tQty) + '   Amount ' + fmt(tAmount) + '   ';
    }

    function renderExpenses(rows) {
        var html = '', tAmount = 0;
        rows.forEach(function (r) {
            tAmount += num(col(r, 'ExpAmount'));
            html += '<tr>'
                 + '<td>' + esc(col(r, 'AccountTitle')) + '</td>'
                 + '<td>' + esc(col(r, 'BrandName')) + '</td>'
                 + '<td>' + esc(col(r, 'BrandUom')) + '</td>'
                 + '<td style="text-align:right;">' + esc(col(r, 'ExpAmount')) + '</td>'
                 + '<td>' + esc(col(r, 'ChargeTo')) + '</td>'
                 + '<td>' + esc(col(r, 'LedgerRemarks')) + '</td>'
                 + '</tr>';
        });
        $id('gridExpense').innerHTML = html;
        $id('lblExpenseTotals').textContent = 'Amount ' + fmt(tAmount) + '   ';
    }

    // ------------------------------------------------------------------------------ history

    /**
     * The history grid is built from whatever columns the procedure returns rather than from a
     * fixed list: USP_InvStockConversion_FormHistory has not been read, so hard-coding headers
     * would quietly drop any column it projects that was not guessed. Id is hidden and the
     * document number is the clickable link back into the record.
     */
    function loadHistory() {
        return busy('btnHistoryRefresh', function () {
            var q = [];
            function add(name, v) { if (v !== '' && v !== null && v !== undefined) q.push(name + '=' + encodeURIComponent(v)); }
            /* BindHistoryGrid:5420 - one range, and the radio decides WHICH pair of
               parameters carries it. A picker that is not ticked is omitted entirely, the way
               an unticked DateTimePicker leaves obj.FromDate unset. */
            var mode = (document.querySelector('input[name="scDateMode"]:checked') || {}).value || 'doc';
            var names = { doc:      ['fromDate',        'toDate'],
                          entry:    ['entryFromDate',   'entryToDate'],
                          modify:   ['modifyFromDate',  'modifyToDate'],
                          approved: ['approvedDateFrom','approvedDateTo'] }[mode];
            var fromOn = $id('fFromOn'), toOn = $id('fToOn');
            if (fromOn && fromOn.checked) add(names[0], val('fFromDate'));
            if (toOn   && toOn.checked)   add(names[1], val('fToDate'));
            add('docNoFrom', val('fDocNoFrom'));
            add('docNoTo', val('fDocNoTo'));

            return getJson(api + '/history' + (q.length ? '?' + q.join('&') : '')).then(function (d) {
                var rows = (d && d.rows) || [];
                var head = $id('historyHead'), body = $id('gridHistory');
                if (!rows.length) {
                    head.innerHTML = '';
                    body.innerHTML = '<tr><td>No records</td></tr>';
                    $id('lblHistoryCount').textContent = '0 record(s)';
                    return;
                }
                var cols = Object.keys(rows[0]).filter(function (c) { return c.toLowerCase() !== 'id'; });
                head.innerHTML = cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');

                var docCol = cols.filter(function (c) {
                    var l = c.toLowerCase();
                    return l === 'docsrno' || l === 'docno';
                })[0];

                body.innerHTML = rows.map(function (r) {
                    var id = int(col(r, 'Id'));
                    return '<tr>' + cols.map(function (c) {
                        var v = esc(r[c]);
                        if (c === docCol && id > 0) {
                            return '<td><a class="win-doc-link" onclick="StockConversion.load(' + id + ')">' + v + '</a></td>';
                        }
                        return '<td>' + v + '</td>';
                    }).join('') + '</tr>';
                }).join('');

                $id('lblHistoryCount').textContent = rows.length + ' record(s)'
                    + (d.canViewAllRecords ? '' : ' - your own records only');
            }).catch(function (e) { box(e.message); });
        });
    }

    // -------------------------------------------------------------------------------- views

    function showView(which) {
        var form = which !== 'history';
        $id('mainViewForm').style.display = form ? '' : 'none';
        $id('mainViewHistory').style.display = form ? 'none' : '';
        $id('tabForm').className = form ? 'active' : '';
        $id('tabHistory').className = form ? '' : 'active';
        if (!form) loadHistory();
    }

    function toggleFullscreen(boxId) {
        var el = $id(boxId);
        if (el) el.classList.toggle('is-fullscreen');
    }

    function btnNew() {
        RECID = 0;
        ['txtdocnumber', 'txtDocManualRef', 'txtProductionNo', 'txtRemarks',
         'txtEBDepartment', 'txtParentCategory', 'txtDifferenceAccount', 'txtLoadDocNo']
            .forEach(function (id) { setVal(id, ''); });
        setVal('txtDocdate', today());
        setVal('cmbEntryType', '0');
        renderDetails([]); renderPackings([]); renderExpenses([]);
        $id('lblDocNo').textContent = '-';
        say('Ready');
        if (window.DesktopCombo) window.DesktopCombo.refresh();
        return getJson(api + '/next-code').then(function (d) {
            var code = d && d.docSrNo ? d.docSrNo : 0;
            if (code > 0) { setVal('txtdocnumber', code); $id('lblDocNo').textContent = code; }
        }).catch(function (e) { box(e.message); });
    }

    function btnRefresh() { busy('btnRefresh', function () { return btnNew(); }); }

    /* Deliberately does not POST. The server answers 501 and explains; saying so here costs the
       operator one click instead of a failed request they have to interpret. */
    function btnSave() {
        box('Save is not enabled for Stock Conversion yet.\n\n'
          + 'The desktop Save also writes inventory transactions, an accounting voucher and '
          + 'contractor wages bills in the same transaction. Until those are traced against '
          + 'their procedures, saving here would create a conversion with no stock movement and '
          + 'no voucher.');
    }

    function boot() {
        setVal('txtDocdate', today());
        loadEntryTypes()
            .then(function () { return btnNew(); })
            .catch(function (e) { box(e.message); });
    }

    window.StockConversion = {
        load: load,
        loadByDocNo: loadByDocNo,
        loadHistory: loadHistory,
        showView: showView,
        toggleFullscreen: toggleFullscreen,
        btnNew: btnNew,
        btnRefresh: btnRefresh,
        btnSave: btnSave
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
