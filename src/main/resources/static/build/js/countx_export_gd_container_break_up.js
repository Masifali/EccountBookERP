/* ============================================================================================
 * countx_export_gd_container_break_up.js - GdContainerBreakUp.cs (Architecture.WinApp.Export) "GD Container
 * BreakUp", pop-up of 952 GD / Bank Invoice Mapping (GdIdFromBreakUp = ?gdId=). Data from
 * /api/export/gd-container-break-up.
 *
 * Desktop behaviour kept: leaving the Gd No combo (here: changing it) loads that GD's saved break-up and
 * REPLACES the grid (asks "Are you sure to load other gd data?" only when another GD is in the grid; an empty
 * combo clears the grid); adding a row locks the combo until the grid is empty again; the X button deletes
 * without asking; Save replaces every saved row of the grid's GD (usp_DeleteGdContainerBreakUpAgainstGd +
 * inserts); New forgets GdIdFromBreakUp, so the combo then lists only GDs without a break-up; the toolbar
 * Update stays hidden, Ctrl+U saves only when the page was opened with a GD ("RecId not found" otherwise).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var Q = global.ExportQ;
    var API = '/api/export/gd-container-break-up';
    var box = Q.box, netI = Q.netI, netD = Q.netD, str = Q.str;

    var S = { gdIdFromBreakUp: 0, gds: [], rows: [], cur: -1, rowIndex: -1, lastGd: '0' };
    var COLS = [{ key: 'GdNo', link: true }, { key: 'InvoiceNo' }, { key: 'ContainerNo' }, { key: 'ContainerSize' },
        { key: 'NoOfBags', fmt: 'num3', sum: true }, { key: 'NetWeight', fmt: 'num3', sum: true }];

    function render() {
        Q.drawGrid('gdcBody', 'gdcFoot', S.rows, COLS, S.cur, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
        });
    }
    /* GdNoBind: GDNO with InvoiceNo shown, ExImInvoiceId hidden. */
    function bindGds() { Q.bind('CmbGdNo', S.gds, 'Id', 'GDNO', ['InvoiceNo']); S.lastGd = Q.val('CmbGdNo'); }
    function gdNoBind() {
        return Q.getJson(API + '/gds?gdId=' + S.gdIdFromBreakUp).then(function (d) {
            if (d && d.gdsError) { box(d.gdsError); return; }
            S.gds = (d && d.gds) || []; bindGds();
        }).catch(function (e) { box(e.message); });
    }

    /* GdBreakUpManual_Load. */
    function load() {
        S.gdIdFromBreakUp = netI(Q.qs('gdId'));
        render();
        return gdNoBind().then(function () {
            if (S.gdIdFromBreakUp > 0) {
                Q.setVal('CmbGdNo', S.gdIdFromBreakUp);
                S.lastGd = Q.val('CmbGdNo');
                /* the desktop focuses the combo; its first Leave loads the GD - done here at once */
                return gdLeave(true);
            }
            Q.focus('CmbGdNo');
        });
    }

    /* CmbGdNo_Leave. */
    function gdLeave(fromLoad) {
        var id = netI(Q.val('CmbGdNo'));
        var gdInGrid = S.rows.length > 0 ? netI(S.rows[0].GdId) : 0;
        if (id > 0 && gdInGrid > 0 && id !== gdInGrid && !Q.ask('Are you sure to load other gd data?')) { S.lastGd = Q.val('CmbGdNo'); return Promise.resolve(); }
        if (gdInGrid > 0) { S.rows = []; }
        S.lastGd = Q.val('CmbGdNo');
        return Q.getJson(API + '/rows?gdId=' + id).then(function (rows) {
            (rows || []).forEach(function (r) { S.rows.push(r); });
            S.cur = -1; render();
            if (fromLoad === true) Q.focus('CmbGdNo');
        }).catch(function (e) { box(e.message); render(); });
    }

    /* GdBreakUpFormValidation. */
    function validate() {
        if (!Q.hasSel('CmbGdNo')) { box('GdNo field is required'); Q.focus('CmbGdNo'); return false; }
        if (!Q.val('txtContainerNo')) { box('Container No field is required'); Q.focus('txtContainerNo'); return false; }
        if (!Q.val('txtContainerSize')) { box('Container Size field is required'); Q.focus('txtContainerSize'); return false; }
        if (netD(Q.val('txtNoOfBags').trim()) <= 0) { box('NoOfBags field is Required'); Q.focus('txtNoOfBags'); return false; }
        if (netD(Q.val('txtNetWeight').trim()) <= 0) { box('Net Weight field is Required'); Q.focus('txtNetWeight'); return false; }
        return true;
    }
    function selectedGd() { return Q.findRow(S.gds, Q.val('CmbGdNo')) || {}; }

    /* btnAdd_Click. */
    function addRow() {
        if (!validate()) return;
        var gd = netI(Q.val('CmbGdNo'));
        for (var i = 0; i < S.rows.length; i++) {
            if (netI(S.rows[i].GdId) !== gd) { box('You can only add data against one Gd in the grid'); return; }
        }
        var g = selectedGd();
        S.rows.push({ Id: 0, GdId: gd, GdNo: Q.selText('CmbGdNo'), ExImInvoiceId: netI(g.ExImInvoiceId), InvoiceNo: str(g.InvoiceNo),
            ContainerNo: Q.val('txtContainerNo'), ContainerSize: Q.val('txtContainerSize'),
            NoOfBags: netD(Q.val('txtNoOfBags')), NetWeight: netD(Q.val('txtNetWeight')) });
        S.cur = -1; render();
        resetDetails();
        Q.setEnabled('CmbGdNo', false);
        Q.focus('txtContainerNo');
    }
    /* grdGdBreakUp_DoubleClick. */
    function editRow(i) {
        var r = S.rows[i]; if (!r) return;
        S.rowIndex = i;
        Q.setValOrAdd('CmbGdNo', r.GdId, r.GdNo);
        S.lastGd = Q.val('CmbGdNo');
        Q.setText('txtContainerNo', str(r.ContainerNo));
        Q.setText('txtContainerSize', str(r.ContainerSize));
        Q.setText('txtNoOfBags', Q.fmt(r.NoOfBags));
        Q.setText('txtNetWeight', Q.fmt(r.NetWeight));
        Q.show('btnAdd', false); Q.show('btnUpdateDetail', true); Q.show('btnCancelDetail', true);
        Q.focus('CmbGdNo');
    }
    /* btnUpdateDetail_Click. */
    function updateRow() {
        if (!validate()) return;
        if (S.rowIndex < 0 || !S.rows[S.rowIndex]) return;
        var gd = netI(Q.val('CmbGdNo'));
        for (var i = 0; i < S.rows.length; i++) {
            if (i !== S.rowIndex && netI(S.rows[i].GdId) !== gd) { box('You can only add data against one Gd in the grid'); return; }
        }
        var g = selectedGd(), r = S.rows[S.rowIndex];
        r.GdId = gd; r.GdNo = Q.selText('CmbGdNo');
        r.ExImInvoiceId = netI(g.ExImInvoiceId); r.InvoiceNo = str(g.InvoiceNo);
        r.ContainerNo = Q.val('txtContainerNo').trim(); r.ContainerSize = Q.val('txtContainerSize').trim();
        r.NoOfBags = netD(Q.val('txtNoOfBags')); r.NetWeight = netD(Q.val('txtNetWeight'));
        render();
        resetDetails();
        Q.focus('txtContainerNo');
    }
    function cancelRow() { resetDetails(); }
    /* ResetGdBreakUpDetails. */
    function resetDetails() {
        S.rowIndex = -1;
        Q.setText('txtContainerNo', ''); Q.setText('txtContainerSize', ''); Q.setText('txtNoOfBags', ''); Q.setText('txtNetWeight', '');
        Q.show('btnAdd', true); Q.show('btnUpdateDetail', false); Q.show('btnCancelDetail', false);
    }
    /* grdGdBreakUp_ColumnButtonClick "Delete": no confirmation; an empty grid unlocks the combo. */
    function deleteRow(i) {
        if (!S.rows[i]) return;
        S.rows.splice(i, 1);
        if (S.rows.length === 0) Q.setEnabled('CmbGdNo', true);
        if (S.rowIndex === i) S.rowIndex = -1;
        S.cur = -1; render();
    }

    /* Reset (btnNew_Click). */
    function reset(btn) {
        var run = function () {
            Q.show('btnsave', true); Q.show('btnupdate', false);
            S.gdIdFromBreakUp = 0;
            S.rows = []; S.cur = -1; render();
            Q.setVal('CmbGdNo', '0'); Q.setEnabled('CmbGdNo', true);
            Q.setText('txtContainerNo', ''); Q.setText('txtContainerSize', ''); Q.setText('txtNoOfBags', ''); Q.setText('txtNetWeight', '');
            Q.focus('CmbGdNo');
            return gdNoBind();
        };
        return btn ? Q.busy(btn, run) : run();
    }
    /* Insert(). */
    function insert(btn) {
        return Q.busy(btn, function () {
            if (S.rows.length === 0) { box('Detail Grid Not found.Please Enter at Least one entry'); return Promise.resolve(); }
            if (!Q.ask('Are you sure to Save?')) return Promise.resolve();
            return Q.postJson(API + '/save', { rows: S.rows }).then(function (r) {
                box((r && r.message) || 'Save SuccessFully');
                return reset();
            });
        });
    }
    /* btnsave_Click: GdIdFromBreakUp = 0, Insert(). */
    function save(btn) { S.gdIdFromBreakUp = 0; return insert(btn); }
    /* btnupdate_Click. */
    function update(btn) {
        if (S.gdIdFromBreakUp === 0) { box('RecId not found'); return Promise.resolve(); }
        return insert(btn);
    }
    /* Footer History: the saved break-up of the GD in the combo (the Leave load). */
    function history(btn) {
        return Q.busy(btn, function () {
            return gdLeave().then(function () { var b = Q.$id('gridBar'); if (b) b.scrollIntoView({ behavior: 'smooth' }); });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        Q.wireTabs(null, null);
        Q.wireGrid('gdcBody', { select: function (i) { S.cur = i; }, open: editRow, del: deleteRow, link: function (k, i) { editRow(i); } });
        Q.on('CmbGdNo', 'change', function () { if (Q.val('CmbGdNo') !== S.lastGd) gdLeave(); });
        /* GdBreakUpManual_KeyDown. */
        document.addEventListener('keydown', function (e) {
            if (Q.enterMoves(e)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); Q.cancel(); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); update(Q.$id('btnupdate')); }
        });
        load();
    });

    global.ExportGdContainerBreakUp = { reset: reset, save: save, update: update, addRow: addRow, updateRow: updateRow, cancelRow: cancelRow, history: history };
}(window));
