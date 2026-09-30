/* ============================================================================================
 * countx_dms_folders.js - 473 "Create Folder Structure" (Architecture.WinApp.DMS_FolderHierarchy.CreateFolderStructure).
 * Built on countx_hrm.js (window.HRM); exports window.Dms.
 * API /api/upload-documents/folder-structure/{setup|tree|documents|add-folder|delete-folder|delete-file}.
 * What the browser cannot do on the share (create / delete directories and files, open, upload, size) is
 * stated on the page and in each action's confirmation; port notes: DmsFolderStructureService.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/upload-documents/folder-structure';
    var P = {};
    window.Dms = P;

    var tree = [], current = null, open = {}, menu = null;
    var NO_OPEN = 'Opening a file (Process.Start on the share path) is not available in the browser. Stored path: ';

    // ------------------------------------------------------------------------ tree
    function find(nodes, id) {
        for (var i = 0; i < nodes.length; i++) {
            if (HRM.int(nodes[i].FolderHierarchyId) === id) return nodes[i];
            var c = find(nodes[i].children || [], id);
            if (c) return c;
        }
        return null;
    }
    function render() {
        function ul(nodes) {
            if (!nodes || !nodes.length) return '';
            return '<ul>' + nodes.map(function (n) {
                var id = HRM.int(n.FolderHierarchyId), kids = n.children || [], isOpen = !!open[id];
                return '<li><div class="dms-node' + (current && HRM.int(current.FolderHierarchyId) === id ? ' is-current' : '') + '" data-id="' + id + '" tabindex="-1" title="' + HRM.esc(n.FolderPath) + '">' +
                    '<span class="dms-tog" data-tog="' + id + '">' + (kids.length ? (isOpen ? '&#9662;' : '&#9656;') : '') + '</span>' +
                    '<i class="fa ' + (isOpen && kids.length ? 'fa-folder-open' : 'fa-folder') + '"></i> <span>' + HRM.esc(n.FolderName) + '</span></div>' +
                    (kids.length && isOpen ? ul(kids) : '') + '</li>';
            }).join('') + '</ul>';
        }
        HRM.$('treeView').innerHTML = tree.length ? ul(tree) : '<div class="dms-empty">No folder found.</div>';
    }
    function setTree(t) {
        tree = t || [];
        if (current) current = find(tree, HRM.int(current.FolderHierarchyId));
        render();
    }

    // ------------------------------------------------------------------------ documents
    var grid = new HRM.Grid('GrdDocuments', {                                        // GridFillForAllocatedAndUnAllocatedDocuments():619
        columns: [
            { key: 'Delete', caption: 'Delete', width: 50, render: function () { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="del" style="min-width:0;width:auto;height:20px;padding:0 6px;font-size:8pt">Delete</button>'; } },
            { key: 'FolderName', caption: 'FolderName' },
            { key: 'DocFilePathId', hidden: true },
            { key: 'FilePath', hidden: true },
            { key: 'FileName', caption: 'FileName', type: 'code', width: 250 },
            { key: 'FileExtension', caption: 'FileExtension', width: 50 },
            { key: 'CreatedDate', caption: 'Upload Date', type: 'date', width: 60 },
            { key: 'UserName', caption: 'Upload User', width: 80 }
        ],
        filterRow: true,
        onCode: function (r) { HRM.box(NO_OPEN + HRM.str(r.FilePath)); }             // GrdDocuments_LinkClicked
    });
    HRM.$('GrdDocuments').addEventListener('click', function (e) {                   // GrdDocuments_ColumnButtonClick (Delete)
        var b = e.target.closest('button[data-act="del"]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        deleteFile(grid.rows()[+tr.getAttribute('data-i')], b);
    });
    function icon(ext) {                                                             // DocumnetsPanelInfo(): the picture per extension
        var x = String(ext || '').toUpperCase().replace(/^\./, '');
        if (['JPG', 'JPEG', 'PNG', 'GIF', 'BMP', 'JFIF'].indexOf(x) >= 0) return 'fa-file-image-o';
        if (['XLSX', 'XLSM', 'XLS', 'XLSB', 'XLTX'].indexOf(x) >= 0) return 'fa-file-excel-o';
        if (x === 'DOCX' || x === 'DOC') return 'fa-file-word-o';
        if (x === 'PDF') return 'fa-file-pdf-o';
        if (x === 'TXT') return 'fa-file-text-o';
        if (x === 'ZIP' || x === 'RAR') return 'fa-file-archive-o';
        return 'fa-file-o';
    }
    function cards(rows) {
        HRM.$('DocumentsFlowLayout').innerHTML = rows.map(function (r, i) {
            return '<div class="dms-card"><div class="dms-ico" data-open="' + i + '" title="' + HRM.esc(r.FilePath) + '"><i class="fa ' + icon(r.FileExtension) + '"></i></div>' +
                '<div class="dms-name">' + HRM.esc(r.FileName) + '</div>' +
                '<div class="dms-meta">' + HRM.esc(HRM.fmtDate(r.CreatedDate)) + ' &middot; size n/a</div>' +
                '<button type="button" class="win-btn-action" data-del="' + i + '">Delete</button></div>';
        }).join('');
    }
    HRM.$('DocumentsFlowLayout').addEventListener('click', function (e) {             // UserControl_Click
        var d = e.target.closest('[data-del]'), o = e.target.closest('[data-open]');
        var rows = grid.rows();
        if (d) deleteFile(rows[+d.getAttribute('data-del')], d);
        else if (o) HRM.box(NO_OPEN + HRM.str(rows[+o.getAttribute('data-open')].FilePath));
    });
    /** treeView_AfterSelect():466 with the (hidden) User Name combo at 0. */
    function afterSelect() {
        HRM.text('lblTotalFiles', '0');
        HRM.$('DocumentsFlowLayout').innerHTML = '';
        grid.clear();
        if (!current) { HRM.show('FolderInfoPanel', false); HRM.show('lblNoFiles', true); return Promise.resolve(); }
        var id = HRM.int(current.FolderHierarchyId);
        HRM.show('FolderInfoPanel', false);
        return HRM.loading(HRM.get(API + '/documents', { folderId: id })).then(function (d) {
            if (!current || HRM.int(current.FolderHierarchyId) !== id) return;
            var rows = (d && d.rows) || [];
            HRM.text('lblPathOfFolder', '');
            if (rows.length) {
                HRM.show('FolderInfoPanel', true);
                HRM.show('lblNoFiles', false);
                cards(rows);
                HRM.text('lblTotalFiles', String(rows.length));
                grid.set(rows);
                showTab('cards');                                                          // TabControl.SelectedTab = DocumentsTab
            } else {
                HRM.show('lblNoFiles', true);
                HRM.text('lblNoFiles', 'No document in "' + HRM.str(current.FolderName) + '".');
            }
        }).catch(HRM.fail);
    }
    function showTab(t) {
        document.querySelectorAll('[data-dms-tab]').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-dms-tab') === t); });
        document.querySelectorAll('[data-dms-page]').forEach(function (p) { p.classList.toggle('is-hidden', p.getAttribute('data-dms-page') !== t); });
    }
    document.addEventListener('click', function (e) {
        var b = e.target.closest ? e.target.closest('[data-dms-tab]') : null;
        if (b) showTab(b.getAttribute('data-dms-tab'));
    });

    // ------------------------------------------------------------------------ actions
    /** AddNewFolder_Click():417 -> SubFolderPopUp ("Folder Name Required!") -> InsertSubFolder(). */
    function addNewFolder() {
        if (!current) { HRM.box('No tree node selected. Please Select!'); return; }
        var parent = current;
        var m = HRM.modal({
            title: 'Sub Folder', width: 'min(420px, 96vw)',
            html: '<div class="hrm-entry" style="grid-template-columns:1fr"><div class="hrm-field"><label for="txtNodeName">Enter Name:</label>' +
                '<input type="text" id="txtNodeName" class="win-textbox" autocomplete="off"/></div>' +
                '<div class="dms-note" style="margin:0">The directory is not created on the share from the browser; only the folder record is added under "' +
                HRM.esc(parent.FolderPath) + '".</div></div>' +
                '<div class="hrm-actions"><button type="button" id="btnOk" class="win-btn-action">OK</button></div>'
        });
        var box = m.body.querySelector('#txtNodeName'), ok = m.body.querySelector('#btnOk');
        setTimeout(function () { box.focus(); }, 0);
        function go() {
            var name = box.value;
            if (name === '') { HRM.box('Folder Name Required!'); box.focus(); return; }            // SubFolderPopUp.btnOk_Click
            HRM.busy(ok, function () {
                return HRM.post(API + '/add-folder', { parentId: HRM.int(parent.FolderHierarchyId), name: name }).then(function (d) {
                    m.close();
                    HRM.box(d && d.message ? d.message : 'Folder Created Successfully...');
                    open[HRM.int(parent.FolderHierarchyId)] = true;
                    setTree(d && d.tree);
                }).catch(HRM.fail);
            });
        }
        ok.addEventListener('click', go);
        box.addEventListener('keydown', function (e) { if (e.key === 'Enter') { e.preventDefault(); go(); } });
    }
    /** BtnDeleteFolder_Click():1045 */
    function deleteFolder() {
        if (!current) { HRM.box('No tree node selected. Please Select!'); return; }
        var f = current;
        if (!HRM.ask('Delete the folder "' + f.FolderName + '"?\n\nOnly the folder record is deleted; the directory ' + f.FolderPath + ' is not removed from the share by the browser.')) return;
        HRM.loading(HRM.post(API + '/delete-folder', { folderId: HRM.int(f.FolderHierarchyId) })).then(function (d) {
            HRM.box(d && d.message ? d.message : 'Fodler Deleted Successfully');
            current = null;
            setTree(d && d.tree);                                                              // (fix) the tree is rebuilt
            afterSelect();
        }).catch(HRM.fail);
    }
    /** UserControl_Click (Delete) / GrdDocuments_ColumnButtonClick (Delete) -> DocFilePath.DeleteFile. */
    function deleteFile(r, btnEl) {
        if (!r || !current) return;
        if (!HRM.ask('Delete the file record "' + r.FileName + '"?\n\nThe file itself (' + r.FilePath + ') is not deleted from the share by the browser.')) return;
        HRM.busy(btnEl, function () {
            return HRM.post(API + '/delete-file', { folderId: HRM.int(current.FolderHierarchyId), docFilePathId: HRM.int(r.DocFilePathId) }).then(function (d) {
                HRM.box(d && d.message ? d.message : 'File Deleted Successfully');
                return afterSelect();
            }).catch(HRM.fail);
        });
    }
    function uploadFile() {                                                              // btnUploadFileInSelectedFolder_Click
        HRM.box('Upload File (DMSAttachmentsUpload copies the chosen files to the folder on the share) is not available in the browser.');
    }

    // ------------------------------------------------------------------------ context menu (MenuStrip)
    function closeMenu() { if (menu) { menu.remove(); menu = null; } }
    function showMenu(x, y) {
        closeMenu();
        menu = document.createElement('div');
        menu.className = 'dms-menu';
        menu.innerHTML = '<button type="button" data-m="add">Add Child New Folder</button>' +
            '<button type="button" data-m="upload" title="Not available in the browser">Upload File</button>' +
            '<button type="button" data-m="del">Delete Folder</button>';
        document.body.appendChild(menu);
        var w = menu.offsetWidth, h = menu.offsetHeight;
        menu.style.left = Math.max(4, Math.min(x, window.innerWidth - w - 4)) + 'px';
        menu.style.top = Math.max(4, Math.min(y, window.innerHeight - h - 4)) + 'px';
        menu.addEventListener('click', function (e) {
            var b = e.target.closest('[data-m]'); if (!b) return;
            var a = b.getAttribute('data-m');
            closeMenu();
            if (a === 'add') addNewFolder(); else if (a === 'upload') uploadFile(); else deleteFolder();
        });
    }
    document.addEventListener('mousedown', function (e) { if (menu && !menu.contains(e.target)) closeMenu(); });
    document.addEventListener('keydown', function (e) { if (e.key === 'Escape') closeMenu(); });

    var tv = HRM.$('treeView');
    function nodeOf(e) { return e.target.closest ? e.target.closest('.dms-node') : null; }
    function select(el) {
        current = find(tree, HRM.int(el.getAttribute('data-id')));
        render();
        return afterSelect();
    }
    tv.addEventListener('click', function (e) {
        var t = e.target.closest('[data-tog]');
        if (t && t.textContent) { var id = HRM.int(t.getAttribute('data-tog')); open[id] = !open[id]; render(); return; }
        var n = nodeOf(e); if (n) select(n);                                             // treeView_AfterSelect
    });
    tv.addEventListener('dblclick', function (e) {
        var n = nodeOf(e); if (!n) return;
        var id = HRM.int(n.getAttribute('data-id')); open[id] = !open[id]; render();
    });
    tv.addEventListener('contextmenu', function (e) {                                    // treeView_MouseUp (Right) -> MenuStrip.Show
        var n = nodeOf(e); if (!n) return;
        e.preventDefault();
        if (!current || HRM.int(current.FolderHierarchyId) !== HRM.int(n.getAttribute('data-id'))) select(n);
        showMenu(e.clientX, e.clientY);
    });
    var pressT = null;                                                                   // long-press = right click on touch screens
    tv.addEventListener('touchstart', function (e) {
        var n = nodeOf(e); if (!n) return;
        var t = e.touches[0];
        pressT = setTimeout(function () { select(n); showMenu(t.clientX, t.clientY); }, 600);
    }, { passive: true });
    ['touchend', 'touchmove', 'touchcancel'].forEach(function (ev) { tv.addEventListener(ev, function () { clearTimeout(pressT); }, { passive: true }); });

    P.reload = function (btnEl) {
        return HRM.busy(btnEl || 'btnReloadTree', function () {
            return HRM.get(API + '/tree').then(function (t) { setTree(t); }).catch(HRM.fail);
        });
    };

    HRM.footer(null);                                                                    // the form has no History
    HRM.keys({ 'esc': function () { if (!menu) HRM.close(); }, 'ctrl+e': HRM.close, enterTab: false });
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {                              // CreateFolderStructure_Load
        tree = (d && d.tree) || [];
        render();
    }).catch(HRM.fail);
})();
