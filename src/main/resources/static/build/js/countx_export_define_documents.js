/* ============================================================================================
 * countx_export_define_documents.js - ExImfrmDefineDocuments.cs (Architecture.WinApp.Export), screen 207
 * "Define Documents". Three independent lists (Document List, Document Group, Assign Document To Group), each
 * with its own entry box and save button, exactly as the desktop form; data from /api/export/define-documents
 * (ExportPopupDefinesController -> ExportDefinesService -> the desktop's own procedures).
 *
 * Desktop quirks kept: no validation on any save; a list whose procedure returns no row keeps its previous
 * rows (the desktop binds only inside the row loop); the group / assignment saves send CompanyId and
 * OrganizationId 0 (the form never sets them) so a saved group does not come back in this company's list;
 * the assignment grid raises "Column 'ExImDocGroupCode' does not belong to table ." whenever a row exists
 * (the procedure does not return that column) and stays empty; after a document is opened the toolbar Save
 * stays hidden and Update shows (neither has a handler).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var Q = global.ExportQ;
    var API = '/api/export/define-documents';
    var $id = Q.$id, box = Q.box, netI = Q.netI, str = Q.str;

    var S = {
        docs: [], groups: [], assign: [],
        recIdDoc: 0, recIdGrp: 0, recIdDocGrp: 0,
        updDoc: false, updGrp: false, updDocGrp: false,
        cur: { doc: -1, grp: -1, asg: -1 }
    };

    var DOC_COLS = [{ key: 'DocCode', link: true }, { key: 'DocumentName' }];
    var GRP_COLS = [{ key: 'GroupCode', link: true }, { key: 'GroupName' }];
    var ASG_COLS = [{ key: 'GroupCode', link: true }, { key: 'DocumentName' }, { key: 'Copies', fmt: 'int' }, { key: 'Orignial', fmt: 'int' }];

    function renderDocs() { Q.drawGrid('docBody', null, S.docs, DOC_COLS, S.cur.doc); }
    function renderGroups() { Q.drawGrid('grpBody', null, S.groups, GRP_COLS, S.cur.grp); }
    function renderAssign() { Q.drawGrid('asgBody', null, S.assign, ASG_COLS, S.cur.asg); }

    /* DocListFill / DocNameBind: bound only when the procedure returned rows (else the old rows stay). */
    function applyDocs(rows) {
        if (!rows || !rows.length) return;
        S.docs = rows; S.cur.doc = -1; renderDocs();
        Q.bind('cmbDocName', rows, 'Id', 'DocumentName', []);
    }
    /* GroupListFill / GroupCodeBind. */
    function applyGroups(rows) {
        if (!rows || !rows.length) return;
        S.groups = rows; S.cur.grp = -1; renderGroups();
        Q.bind('cmbGroupCode', rows, 'Id', 'GroupCode', []);
    }
    /* AssigndoctogrpFill. */
    function applyAssign(d) {
        if (d.assignError) { box(d.assignError); return; }
        if (!d.assign || !d.assign.length) return;
        S.assign = d.assign; S.cur.asg = -1; renderAssign();
    }

    /* frmList_Load: GroupCodeBind, DocNameBind, DocListFill, GroupListFill, AssigndoctogrpFill. */
    function load() {
        return Q.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.groupsError) box(d.groupsError); else applyGroups(d.groups);
            if (d.documentsError) box(d.documentsError); else applyDocs(d.documents);
            applyAssign(d);
            Q.focus('txtDocCode');
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ Document List

    function docListReset() {
        Q.setText('txtDocCode', ''); Q.setText('txtDocName', '');
        S.updDoc = false; S.recIdDoc = 0;
        return Q.getJson(API + '/documents').then(function (d) {
            if (d && d.documentsError) box(d.documentsError); else applyDocs(d && d.documents);
        }).catch(function (e) { box(e.message); });
    }
    /* btnAddinGrid_Click. */
    function saveDocument(btn) {
        return Q.busy(btn, function () {
            var upd = S.updDoc;
            return Q.postJson(API + '/save-document', { recId: S.recIdDoc, updateMode: upd, code: Q.val('txtDocCode'), name: Q.val('txtDocName') })
                .then(function (r) {
                    if (r && netI(r.id) > 0) {
                        box(r.message);
                        return docListReset().then(function () { Q.focus('txtDocCode'); });
                    }
                });
        });
    }
    /* grdDocList_DoubleClick -> ReadById. */
    function readDocument(i) {
        var r = S.docs[i]; if (!r) return;
        Q.show('btnsave', false); Q.show('btnUpdate', true);
        S.recIdDoc = netI(r.Id);
        return Q.getJson(API + '/document?id=' + S.recIdDoc).then(function (m) {
            if (!m) return;
            Q.setText('txtDocCode', m.exImDocCode); Q.setText('txtDocName', m.exImDocName);
            S.updDoc = true;
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ Document Group

    function groupListReset() {
        Q.setText('txtGrpCode', ''); Q.setText('txtgrpName', '');
        S.updGrp = false; S.recIdGrp = 0;
        return Q.getJson(API + '/groups').then(function (d) {
            if (d && d.groupsError) box(d.groupsError); else applyGroups(d && d.groups);
        }).catch(function (e) { box(e.message); });
    }
    /* btnSaveGroup_Click. */
    function saveGroup(btn) {
        return Q.busy(btn, function () {
            return Q.postJson(API + '/save-group', { recId: S.recIdGrp, updateMode: S.updGrp, code: Q.val('txtGrpCode'), name: Q.val('txtgrpName') })
                .then(function (r) {
                    if (r && netI(r.id) > 0) {
                        box(r.message);
                        return groupListReset().then(function () { Q.focus('txtGrpCode'); });
                    }
                });
        });
    }
    /* grdDocGroup_DoubleClick -> ReadByGrpId. */
    function readGroup(i) {
        var r = S.groups[i]; if (!r) return;
        S.recIdGrp = netI(r.Id);
        return Q.getJson(API + '/group?id=' + S.recIdGrp).then(function (m) {
            if (!m) return;
            Q.setText('txtGrpCode', m.ExImDocGroupCode); Q.setText('txtgrpName', m.ExImDocGroupName);
            S.updGrp = true;
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ Assign Document To Group

    /* DocGroupListReset: combos cleared, boxes cleared, combos + grid re-read. */
    function docGroupListReset() {
        Q.setVal('cmbDocName', '0'); Q.setVal('cmbGroupCode', '0');
        Q.setText('txtorginal', ''); Q.setText('txtcopies', '');
        S.updDocGrp = false; S.recIdDocGrp = 0;
        return Q.getJson(API + '/assign').then(function (d) {
            d = d || {};
            if (d.groupsError) box(d.groupsError); else if (d.groups && d.groups.length) Q.bind('cmbGroupCode', d.groups, 'Id', 'GroupCode', []);
            if (d.documentsError) box(d.documentsError); else if (d.documents && d.documents.length) Q.bind('cmbDocName', d.documents, 'Id', 'DocumentName', []);
            applyAssign(d);
        }).catch(function (e) { box(e.message); });
    }
    /* btnSavedoctoGroup_Click. */
    function saveSchedule(btn) {
        return Q.busy(btn, function () {
            return Q.postJson(API + '/save-schedule', {
                recId: S.recIdDocGrp, updateMode: S.updDocGrp, groupId: netI(Q.val('cmbGroupCode')), documentId: netI(Q.val('cmbDocName')),
                copies: Q.val('txtcopies'), original: Q.val('txtorginal')
            }).then(function (r) {
                if (r && netI(r.id) > 0) {
                    box(r.message);
                    return docGroupListReset().then(function () { Q.focus('cmbGroupCode'); });
                }
            });
        });
    }
    /* grdAssignDocToGrp_DoubleClick -> ReadByDocGrpId. */
    function readSchedule(i) {
        var r = S.assign[i]; if (!r) return;
        S.recIdDocGrp = netI(r.Id);
        return Q.getJson(API + '/schedule?id=' + S.recIdDocGrp).then(function (m) {
            if (!m) return;
            Q.setVal('cmbDocName', m.ExImShipmentDocuments);
            Q.setVal('cmbGroupCode', m.ExImShipmentDocGroupId);
            Q.setText('txtcopies', str(m.Copies)); Q.setText('txtorginal', str(m.Original));
            S.updDocGrp = true;
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ toolbar / footer

    /* btnnew_Click: DocGroupListReset, DocListReset, GroupListReset, GroupCodeBind, DocNameBind. */
    function newAll(btn) {
        return Q.busy(btn, function () {
            return docGroupListReset().then(docListReset).then(groupListReset);
        });
    }
    /* btnsave_Click is an empty handler on the desktop. */
    function toolSave() { }
    /* Footer History: the three lists are this form's history - re-read them and bring them into view. */
    function history(btn) {
        return Q.busy(btn, function () {
            return Q.getJson(API + '/setup').then(function (d) {
                d = d || {};
                if (d.groupsError) box(d.groupsError); else applyGroups(d.groups);
                if (d.documentsError) box(d.documentsError); else applyDocs(d.documents);
                applyAssign(d);
                var g = $id('grdDocListBox'); if (g && g.scrollIntoView) g.scrollIntoView({ behavior: 'smooth', block: 'start' });
            });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        Q.wireTabs(null, null);
        Q.wireGrid('docBody', { select: function (i) { S.cur.doc = i; }, open: readDocument });
        Q.wireGrid('grpBody', { select: function (i) { S.cur.grp = i; }, open: readGroup });
        Q.wireGrid('asgBody', { select: function (i) { S.cur.asg = i; }, open: readSchedule });
        load();
    });

    global.ExportDefineDocuments = {
        newAll: newAll, toolSave: toolSave, history: history,
        saveDocument: saveDocument, saveGroup: saveGroup, saveSchedule: saveSchedule
    };
}(window));
