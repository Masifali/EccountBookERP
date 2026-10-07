(function () {
    'use strict';
    var dialog;
    var current = null;

    function el(tag, attributes, text) {
        var node = document.createElement(tag);
        Object.keys(attributes || {}).forEach(function (key) { node.setAttribute(key, attributes[key]); });
        if (text != null) node.textContent = text;
        return node;
    }

    function ensureDialog() {
        if (dialog) return dialog;
        dialog = el('dialog', { class: 'cmagt-attachment-dialog', 'aria-labelledby': 'cmagt-attachment-title' });
        var style = el('style');
        style.textContent = '.cmagt-attachment-dialog{width:min(760px,94vw);max-height:88vh;border:1px solid #78909c;padding:0;color:#17232c;font:13px Tahoma,Arial,sans-serif;box-shadow:0 8px 28px #0005}.cmagt-attachment-dialog::backdrop{background:#0006}.cmagt-att-head{display:flex;align-items:center;justify-content:space-between;background:#00796b;color:#fff;padding:8px 12px;font-weight:bold}.cmagt-att-body{padding:12px}.cmagt-att-table-wrap{max-height:46vh;overflow:auto;border:1px solid #b5c4ce}.cmagt-att-table{width:100%;border-collapse:collapse}.cmagt-att-table th{position:sticky;top:0;background:#dbe6f2}.cmagt-att-table th,.cmagt-att-table td{border:1px solid #c4ced6;padding:5px;text-align:left}.cmagt-att-status{min-height:20px;color:#a11;padding-top:6px}.cmagt-att-actions{display:flex;justify-content:flex-end;gap:6px;padding:10px 12px;border-top:1px solid #ccd5dc}.cmagt-att-actions button,.cmagt-att-add{border:1px solid #82949d;background:#f7fafb;padding:5px 10px;cursor:pointer}.cmagt-att-actions .primary{background:#00796b;color:#fff;border-color:#00665a}.cmagt-att-file{margin:10px 0}.cmagt-att-link{color:#0059a8;text-decoration:underline;cursor:pointer;background:none;border:0;padding:0;font:inherit}';
        dialog.appendChild(style);
        var head = el('div', { class: 'cmagt-att-head' });
        head.appendChild(el('span', { id: 'cmagt-attachment-title' }, 'Attachments'));
        var close = el('button', { type: 'button', 'aria-label': 'Close' }, '✕');
        close.addEventListener('click', function () { dialog.close(); });
        head.appendChild(close);
        dialog.appendChild(head);
        var body = el('div', { class: 'cmagt-att-body' });
        var wrap = el('div', { class: 'cmagt-att-table-wrap' });
        var table = el('table', { class: 'cmagt-att-table' });
        table.innerHTML = '<thead><tr><th>Attachment</th><th>Entry Date</th><th>Entry User</th><th>Remove</th></tr></thead><tbody id="cmagtAttachmentRows"></tbody>';
        wrap.appendChild(table);
        body.appendChild(wrap);
        var fileLabel = el('label', { class: 'cmagt-att-file' }, 'Add files (maximum 5 MB each) ');
        fileLabel.appendChild(el('input', { id: 'cmagtAttachmentFiles', type: 'file', multiple: 'multiple' }));
        body.appendChild(fileLabel);
        body.appendChild(el('div', { class: 'cmagt-att-status', id: 'cmagtAttachmentStatus', role: 'status', 'aria-live': 'polite' }));
        dialog.appendChild(body);
        var footer = el('div', { class: 'cmagt-att-actions' });
        var cancel = el('button', { type: 'button' }, 'Close');
        cancel.addEventListener('click', function () { dialog.close(); });
        var save = el('button', { type: 'button', class: 'primary', id: 'cmagtAttachmentSave' }, 'Save Changes');
        save.addEventListener('click', persist);
        footer.appendChild(cancel);
        footer.appendChild(save);
        dialog.appendChild(footer);
        document.body.appendChild(dialog);
        return dialog;
    }

    function pick(row, name) {
        var key = Object.keys(row || {}).find(function (candidate) { return candidate.toLowerCase() === name.toLowerCase(); });
        return key ? row[key] : '';
    }

    function request(url, options) {
        options = options || {};
        options.headers = Object.assign({ 'Accept': 'application/json' }, options.headers || {});
        var csrf = document.querySelector('meta[name="_csrf"]');
        var csrfHeader = document.querySelector('meta[name="_csrf_header"]');
        if (csrf && csrfHeader) options.headers[csrfHeader.content] = csrf.content;
        return fetch(url, options).then(function (response) {
            if (!response.ok) throw new Error('Request failed (' + response.status + ')');
            return response.json();
        });
    }

    function render(rows) {
        var tbody = document.getElementById('cmagtAttachmentRows');
        tbody.replaceChildren();
        (rows || []).forEach(function (row) {
            var tr = document.createElement('tr');
            var name = String(pick(row, 'Attachment') || 'Attachment');
            var id = Number(pick(row, 'Id'));
            var tdName = el('td');
            var download = el('a', { class: 'cmagt-att-link', href: current.url + '/' + encodeURIComponent(id), target: '_blank', rel: 'noopener' }, name);
            tdName.appendChild(download);
            tr.appendChild(tdName);
            tr.appendChild(el('td', {}, String(pick(row, 'EntryDate') || '')));
            tr.appendChild(el('td', {}, String(pick(row, 'EntryUserName') || pick(row, 'EntryUser') || '')));
            var tdRemove = el('td');
            var checkbox = el('input', { type: 'checkbox', class: 'cmagt-att-remove', value: String(id), 'aria-label': 'Remove ' + name });
            tdRemove.appendChild(checkbox);
            tr.appendChild(tdRemove);
            tbody.appendChild(tr);
        });
        if (!rows || !rows.length) {
            var empty = el('tr'); empty.appendChild(el('td', { colspan: '4' }, 'No attachments found.')); tbody.appendChild(empty);
        }
    }

    function asUpload(file) {
        return new Promise(function (resolve, reject) {
            if (file.size > 5 * 1024 * 1024) { reject(new Error(file.name + ' exceeds 5 MB.')); return; }
            var reader = new FileReader();
            reader.onload = function () { resolve({ name: file.name, base64: String(reader.result).split(',')[1] || '' }); };
            reader.onerror = function () { reject(new Error('Could not read ' + file.name)); };
            reader.readAsDataURL(file);
        });
    }

    function persist() {
        var save = document.getElementById('cmagtAttachmentSave');
        var status = document.getElementById('cmagtAttachmentStatus');
        var files = Array.from(document.getElementById('cmagtAttachmentFiles').files || []);
        var removeAttachmentIds = Array.from(document.querySelectorAll('.cmagt-att-remove:checked')).map(function (box) { return Number(box.value); });
        if (files.length > 10) { status.textContent = 'Select at most ten files at once.'; return; }
        save.disabled = true; status.textContent = 'Saving attachments…';
        Promise.all(files.map(asUpload)).then(function (uploads) {
            return request(current.url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ files: uploads, removeAttachmentIds: removeAttachmentIds }) });
        }).then(function (rows) {
            render(rows); document.getElementById('cmagtAttachmentFiles').value = ''; status.textContent = 'Attachments saved.';
        }).catch(function (error) { status.textContent = error.message || 'Could not save attachments.'; })
            .finally(function () { save.disabled = false; });
    }

    window.CmagtAttachments = {
        open: function (screen, idField) {
            var input = document.getElementById(idField);
            var id = input ? Number(input.value) : 0;
            if (!id) { window.alert('Save this document before opening attachments.'); return; }
            current = { url: '/api/commission/attachments/' + encodeURIComponent(screen) + '/' + encodeURIComponent(id) };
            var dlg = ensureDialog();
            document.getElementById('cmagtAttachmentStatus').textContent = 'Loading…';
            document.getElementById('cmagtAttachmentFiles').value = '';
            dlg.showModal();
            request(current.url).then(function (rows) {
                render(rows); document.getElementById('cmagtAttachmentStatus').textContent = '';
            }).catch(function (error) { document.getElementById('cmagtAttachmentStatus').textContent = error.message || 'Could not load attachments.'; });
        }
    };
})();
