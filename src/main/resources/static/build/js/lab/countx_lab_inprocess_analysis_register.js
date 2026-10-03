/* 631 "In-Process Analysis Report" - Architecture.WinApp.Lab.InProcessLabAnalysisRegister
   (ScreenName InProcessLabAnalysisRegister). ":NNN" = line in InProcessLabAnalysisRegister.cs.
   API /api/lab/reports/inprocess-register. Built on pr_common.js (window.PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/api/lab/reports/inprocess-register', BACK = '/quality';
    var st = { count: 0, dates: {}, printArgs: null };
    var stepGrid = new PR.Grid('grdStepAnalysis', 'lblStepCount');
    var recoveryGrid = new PR.Grid('grdGroupAnalysis', 'lblRecoveryCount');

    /** dtMainGrid (:287-300) + GridSettings :413 - the four id columns hidden, StepName / Parameter 150. */
    function stepColumns(dynamic) {
        var cols = [{ key: 'ScheduleId', hidden: true }, { key: 'PlantId', hidden: true }, { key: 'StepId', hidden: true },
            { key: 'ParameterId', hidden: true }, { key: 'StepName', width: 150 }, { key: 'Parameter', width: 150 }];
        (dynamic || []).forEach(function (c) { cols.push({ key: c.key, caption: c.caption, width: 80 }); });   // one per reading time of the first row
        return cols;
    }
    /** dtGroupAnalysisGrid (:336-349) + GridGroupAnalysisSettings :405 - ParameterId hidden, PlantName 150, ItemName 180, Parameter 150. */
    function recoveryColumns(dynamic) {
        var cols = [{ key: 'ParameterId', hidden: true }, { key: 'PlantName', width: 150 }, { key: 'ItemName', width: 180 },
            { key: 'Parameter', width: 150 }, { key: 'MinValue', width: 80 }, { key: 'MaxValue', width: 80 }];
        (dynamic || []).forEach(function (c) { cols.push({ key: c.key, caption: c.caption, width: 80 }); });
        return cols;
    }

    /** InfragisticsHelper.BindAndRetainSelection: the previous value stays when the new list still has it, else the text is cleared.
        The empty first row is the web's way to clear a combo again (the desktop user deletes the text). */
    function bind(id, rows, valueKey, textKey) { LabRep.fill(id, rows, { value: valueKey, text: textKey, blank: true, keep: true }); }

    /** cmbPlantName_Leave :207 - OtherComboFill (Item Name, Job Order), then cmbItemName_Leave. */
    function plantLeave() {
        var current = LabRep.latest('plant');                  // only the newest answer may fill the combos
        return PR.request(API + '/lookups').then(function (d) {
            if (!current()) return;
            bind('cmbItemName', d.items, 'Id', 'name');
            bind('CmbJobOrderNo', d.jobOrders, 'Id', 'name');
            return DateBindDbCall();
        });
    }
    /** DateBindDbCall :177 (cmbItemName_Leave :220, also wired to CmbJobOrderNo.Leave) - Id = row number, DocDate = short date. */
    function DateBindDbCall() {
        var current = LabRep.latest('dates');
        var q = '?plantId=' + PR.int($('cmbPlantName').value) + '&jobOrderId=' + PR.int($('CmbJobOrderNo').value) + '&itemId=' + PR.int($('cmbItemName').value);
        return PR.request(API + '/dates' + q).then(function (rows) {
            if (!current()) return;
            st.dates = {};
            (rows || []).forEach(function (r) { st.dates[String(r.Id)] = String(r.DocDate || '').slice(0, 10); r.Text = PR.shortDate(r.DocDate); });
            bind('cmbDocDate', rows, 'Id', 'Text');
        });
    }

    /** btnshow_Click :257. */
    function show() {
        if (!PR.val('cmbPlantName')) { PR.box('Plant Name Feild Required...'); $('cmbPlantName').focus(); return Promise.resolve(); }   // :263
        if (!PR.val('cmbItemName')) { PR.box('Item Name Feild Required...'); $('cmbItemName').focus(); return Promise.resolve(); }     // :269
        var args = { plantId: PR.val('cmbPlantName'), jobOrderId: PR.val('CmbJobOrderNo'), itemId: PR.val('cmbItemName'),
            docDate: st.dates[$('cmbDocDate').value] || '' };          // Conversion.ToDateTime(cmbDocDate.Text) (:277)
        return PR.request(API + '/rows', args).then(function (res) {
            st.count = PR.int(res.count); st.printArgs = args;
            var s = res.step || {}, g = res.recovery || {};
            if (s.rows && s.rows.length) stepGrid.show(stepColumns(s.columns), s.rows, {});
            else { stepGrid.clear(); PR.box('No Record found For Display...'); }                  // :326
            if (g.rows && g.rows.length) recoveryGrid.show(recoveryColumns(g.columns), g.rows, {});
            else recoveryGrid.clear();                                                            // :372
        });
    }

    function showClick() { return LabRep.run('btnshow', show); }

    /** print_Click_1 :433 - 660-LabSInProcessAnalysisRegister.rpt over the rows of the last Show. */
    function print() {
        if (!st.count) { PR.box('Record Not Found For Display'); return; }                        // :439
        if (!window.CrystalPrint) { PR.box('The print helper is not available on this page.'); return; }
        var a = {};
        Object.keys(st.printArgs).forEach(function (k) { if (st.printArgs[k]) a[k] = st.printArgs[k]; });
        window.CrystalPrint.open('660-labsinprocessanalysisregister', a, 'print');
    }

    /** reset :236 (btnNew "&New"). */
    function reset() {
        $('CmbJobOrderNo').value = ''; $('cmbItemName').value = ''; $('cmbDocDate').value = '';
        $('cmbPlantName').focus();
    }
    /** MakeShortCutKeys :494. */
    function shortcuts() {
        PR.shortcuts([['Ctrl+S', 'For show data'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+alt', 'To Show ShortCut Date Type'],
            ['Ctrl+ArrowDown', 'For Focus On Grid']]);
    }

    /** VoucherValidation_Load :101 - only the Plant combo is filled. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            bind('cmbPlantName', d.plants, 'Id', 'ReferenceName');
            $('cmbPlantName').focus();
        });
    }

    $('cmbPlantName').addEventListener('change', function () { PR.run(plantLeave); });
    $('CmbJobOrderNo').addEventListener('change', function () { PR.run(DateBindDbCall); });
    $('cmbItemName').addEventListener('change', function () { PR.run(DateBindDbCall); });
    $('btnshow').addEventListener('click', showClick);
    LabRep.fullscreen('btnStepFullscreen', 'stepSection');
    LabRep.fullscreen('btnRecoveryFullscreen', 'recoverySection');
    $('btnNew').addEventListener('click', reset);
    $('print').addEventListener('click', print);
    $('toolStripButton2').addEventListener('click', shortcuts);
    PR.gridTools(stepGrid, 'btnStepPrint', 'btnStepExport', function () { return "Plant's In Process Step Analysis Detail"; });
    PR.gridTools(recoveryGrid, 'btnRecoveryPrint', 'btnRecoveryExport', function () { return "Plant's In Process Recovery Analysis Detail"; });
    PR.enterAsTab();                                       // :458 Enter -> {TAB}
    /** VoucherValidation_KeyDown :452. */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = BACK; } return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (!e.ctrlKey) return;
        var a = { n: reset, r: reset, s: showClick, arrowdown: function () { $('recoveryWrap').focus(); },
            arrowup: function () { $('cmbPlantName').focus(); }, f5: function () { $('cmbPlantName').focus(); }, p: print }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
