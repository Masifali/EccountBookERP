/* 627 "Lab Purchase Analysis Report (Not Use)" (ScreenName InvLabPurchaseReport) - Architecture.WinApp.Lab.InvLabPurchaseReport
   (module 1011 Lab Report). ":NNN" = line in InvLabPurchaseReport.cs. API /api/lab/reports/lab-purchase-report.
   Built on pr_common.js (window.PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/api/lab/reports/lab-purchase-report';
    var st = { suppliers: [], RecId: 0 };
    var from = new PR.Picker('txtdatef'), to = new PR.Picker('txtdatet');
    var grdhistory = new PR.Grid('grdhistory', 'lblCount');
    var griddetail = new PR.Grid('griddetail');

    /** Disable + spinner while the request is in flight; re-enabled on success AND on failure; extra clicks ignored. */
    function withBusy(btn, fn) {
        if (btn && btn.disabled) return Promise.resolve();
        if (btn) { btn.disabled = true; btn.classList.add('btn-busy'); }
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(function (v) { return v; }, function () { /* PR.run has already shown the message */ })
            .then(function (v) { if (btn) { btn.disabled = false; btn.classList.remove('btn-busy'); } return v; });
    }

    /** The grid table of historygridfill :160-172 with gridsetting :196 - Id / AnalysisPicture / CookingPicture hidden,
        the widths of :218-225, then the two button columns "Detail" and "View" (caption "Slip", 50 wide, :200-211).
        DocumentNo is a link here (web list rule): it opens the Purchase Analysis page for the row. */
    function columns() {
        return [
            { key: 'Id', hidden: true },
            { key: 'DocumentNo', width: 70, link: true },
            { key: 'SupplierName', width: 300 },
            { key: 'AnalysisItem', caption: 'Analysis Item', width: 300 },
            { key: 'GatePassNo', caption: 'GatePass#', width: 70 },
            { key: 'BuiltyNo', width: 70 },
            { key: 'VehicleNo', caption: 'Vehicle No', width: 100 },
            { key: 'CropYear', caption: 'Crop Year', width: 120 },
            { key: 'Remarks', width: 300 },
            { key: 'AnalysisPicture', hidden: true },
            { key: 'CookingPicture', hidden: true },
            { key: 'AnalystName', width: 110 },
            { key: 'Detail', caption: 'Detail', width: 50, link: true },      // ButtonText "Detail"
            { key: 'View', caption: 'Slip', width: 50, link: true }];         // ButtonText "Slip"
    }
    /** griddetailsetting :346. */
    function detailColumns() {
        return [
            { key: 'ParameterDescription', caption: 'Perameter Description', width: 200 },   // sic
            { key: 'MinValue', caption: 'Min Value', width: 70 },
            { key: 'MaxValue', caption: 'Max Value', width: 70 },
            { key: 'AnalysisResult', caption: 'Analysis Result', width: 70 },
            { key: 'RemarksDetail', caption: 'Remarks Detail', width: 250 }];
    }

    /** cmbsupplierfill :117 - BindDDL(dt, cmbsupplier, "Id", "CompanyName", "Supplier Customer", ZeroIndex: false). */
    function cmbsupplierfill(rows) {
        st.suppliers = rows || [];
        $('cmbsupplier').value = '';
        if (st.suppliers.length) PR.fill('cmbsupplier', st.suppliers, { value: 'Id', text: 'CompanyName' });
    }

    /** historygridfill :133. */
    function historygridfill() {
        return PR.request(API + '/rows', {
            fromDate: from.value(), toDate: to.value(),
            supplierId: PR.val('cmbsupplier'),                                   // Conversion.ToInt(cmbsupplier.Value)
            gpNoFrom: $('txtgpnof').value.trim(), gpNoTo: $('txtgpnot').value.trim()
        }).then(function (res) {
            var rows = res.rows || [];
            if (!rows.length) { grdhistory.clear(); griddetail.clear(); return; }   // ClearStructure x 2 (:186-187)
            rows.forEach(function (r) { r.Detail = 'Detail'; r.View = 'Slip'; });
            grdhistory.show(columns(), rows, { onLink: grdhistory_ColumnButtonClick });
        });
    }

    function setPicture(id, url) {
        var img = $(id);
        img.onload = function () { img.hidden = false; };
        img.onerror = function () { img.hidden = true; img.removeAttribute('src'); };   // !File.Exists -> Image = null
        img.hidden = true;
        if (url) img.src = url; else img.removeAttribute('src');
    }

    /** grdhistory_ColumnButtonClick :231 - "View" prints the 653 slip of the row, "Detail" fills griddetail and the
        two pictures from GetById(RecId). */
    function grdhistory_ColumnButtonClick(col, r) {
        var id = PR.int(r.Id);
        if (col === 'DocumentNo') { window.open('/quality/purchase-analysis?id=' + id, '_blank'); return; }
        if (col === 'View') {                                                    // 653-RptInvLabPurchaseAnalysisSlip.rpt by Id (:249-262)
            PR.run(function () { return PR.printTemplate('653-RptInvLabPurchaseAnalysisSlip.rpt', { history: String(id) }); });
            return;
        }
        if (col !== 'Detail') return;
        PR.run(function () {
            st.RecId = id;                                                       // :273
            return PR.request(API + '/' + id + '/detail').then(function (d) {
                if (!d.found) { griddetail.clear(); return; }                    // :336
                var stamp = '?t=' + Date.now();
                setPicture('sampleanalysispicture', d.analysisPic ? API + '/' + id + '/picture/analysis' + stamp : '');   // :291-309
                setPicture('cookingpic', d.cookingPic ? API + '/' + id + '/picture/cooking' + stamp : '');              // :310-328
                if ((d.rows || []).length) griddetail.show(detailColumns(), d.rows, {});
                else griddetail.clear();
            });
        });
    }

    /** btnimgpreanalysispic_Click :411 / btnimgpreviewcookingpic_Click :425 - LabImagesPreview with the picture box's image. */
    function preview(id, title) {
        var img = $(id);
        var html = '<div class="lab-or-preview">' + (img.hidden || !img.getAttribute('src') ? '' : '<img alt="" src="' + img.getAttribute('src') + '">') + '</div>';
        PR.dialog(title, html);
    }

    /** btnnew_Click :374 - GpNo boxes emptied, both pickers back to "now" (DateTimePicker.Text = "" resets the value),
        supplier text cleared, focus on Date From, griddetail cleared. The history grid and the pictures stay. */
    function btnnew_Click() {
        $('txtgpnof').value = '';
        $('txtgpnot').value = '';
        var now = new Date(), p = function (n) { return (n < 10 ? '0' : '') + n; };
        var iso = now.getFullYear() + '-' + p(now.getMonth() + 1) + '-' + p(now.getDate()) + 'T' + p(now.getHours()) + ':' + p(now.getMinutes()) + ':' + p(now.getSeconds());
        from.set(iso); to.set(iso);
        cmbsupplierfill(st.suppliers);
        $('txtdatef').focus();
        griddetail.clear();
    }

    /** InvLabPurchaseReport_Load :396 - cmbsupplierfill, focus, txtdatef = ActiveYr.Start_Period, historygridfill. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            cmbsupplierfill(d.suppliers);
            $('txtdatef').focus();
            to.set(d.now);
            from.set(d.yearStart || d.now);                                      // :402
            return historygridfill();
        });
    }

    function search(btn) { return withBusy(btn, function () { return PR.run(historygridfill); }); }
    $('btnsearch').addEventListener('click', function () { search(this); });                 // btnsearch_Click :362
    $('btnAllRecord').addEventListener('click', function () { search(this); });              // btnAllRecord_Click :439
    $('btnnew').addEventListener('click', function () { var b = this; withBusy(b, btnnew_Click); });
    $('btnimgpreanalysispic').addEventListener('click', function () { preview('sampleanalysispicture', 'Analysis Pic'); });
    $('btnimgpreviewcookingpic').addEventListener('click', function () { preview('cookingpic', 'Cooking Pic'); });
    PR.gridTools(grdhistory, 'btnGridPrint', 'btnGridExport', function () { return 'Purchase Analysis Report'; });
    PR.fullscreen('btnFullscreen', 'historySection');
    /* The form has no KeyDown handler (KeyPreview is not set); "&New" is Alt+N (accesskey on the button). */
    PR.run(load);
})();
