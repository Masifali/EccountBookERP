/* Shared attachment dialog for the two original weighted Purchase Invoice forms. */
const PurchaseInvoiceAttachments=(()=>{
    'use strict';
    const esc=value=>String(value??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
    function create({type,getId,canEdit,message}){
        let dialog,rows=[],files=[],removed=new Set(),changed=false,loaded=false,generation=0,pending=Promise.resolve();
        const base='/api/purchase/invoice-attachments/'+type;
        function ensure(){
            if(dialog)return;dialog=document.createElement('section');dialog.className='invoice-attachments';dialog.hidden=true;dialog.setAttribute('role','dialog');dialog.setAttribute('aria-label','Invoice attachments');
            dialog.innerHTML='<h2>Invoice Attachments<button type="button" data-close>Close</button></h2><p>Attachment changes are saved with the invoice.</p><label>Add files <input type="file" multiple aria-label="Add invoice files"></label><div class="invoice-scroll"><table><thead><tr><th>File</th><th>Status</th><th>Remove</th></tr></thead><tbody></tbody></table></div>';
            document.body.appendChild(dialog);dialog.querySelector('[data-close]').addEventListener('click',()=>{dialog.hidden=true;});
            dialog.querySelector('input').addEventListener('change',e=>{const selected=Array.from(e.target.files||[]),version=generation,input=e.target;pending=PurchaseRequest.track(async()=>{input.disabled=true;try{
                if(!canEdit())throw Error('You do not have permission to change these attachments');
                if(files.length+selected.length>10)throw Error('Select at most ten files at once');
                for(const file of selected)if(!file.size||file.size>5*1024*1024)throw Error('Each file must be between 1 byte and 5 MB');
                const additions=[];for(const file of selected){const base64=await new Promise((resolve,reject)=>{const reader=new FileReader();reader.onload=()=>resolve(String(reader.result).split(',')[1]);reader.onerror=()=>reject(Error('Could not read '+file.name));reader.readAsDataURL(file);});additions.push({name:file.name,base64});}
                if(version!==generation)return;files.push(...additions);changed=changed||additions.length>0;render();
            }catch(error){if(version===generation)message(error.message);}finally{input.value='';input.disabled=!canEdit();}});});
            dialog.querySelector('tbody').addEventListener('click',e=>{if(!canEdit())return;const button=e.target.closest('button');if(!button)return;
                if(button.dataset.existing){const id=Number(button.dataset.existing);if(removed.has(id))removed.delete(id);else removed.add(id);}
                if(button.dataset.staged!==undefined)files.splice(Number(button.dataset.staged),1);changed=true;render();
            });
        }
        function render(){
            if(!dialog)return;dialog.querySelector('input').disabled=!canEdit();
            dialog.querySelector('tbody').innerHTML=rows.map(r=>`<tr><td><a href="${base}/${getId()}/${r.Id}">${esc(r.Attachment)}</a></td><td>${removed.has(Number(r.Id))?'Remove on Save':'Saved'}</td><td><button type="button" data-existing="${r.Id}" ${canEdit()?'':'disabled'}>${removed.has(Number(r.Id))?'Undo':'Remove'}</button></td></tr>`).join('')+files.map((f,i)=>`<tr><td>${esc(f.name)}</td><td>New — save invoice</td><td><button type="button" data-staged="${i}" ${canEdit()?'':'disabled'}>Remove</button></td></tr>`).join('');
        }
        async function open(button){ensure();const version=generation;try{await PurchaseRequest.run(button,async()=>{await pending;if(!loaded&&getId()){
                const response=await fetch(base+'/'+getId(),{headers:{Accept:'application/json'}});if(response.redirected||response.status===401)throw Error('Please sign in to continue');const data=await response.json();if(!response.ok)throw Error(data.message||'Attachments could not be loaded');if(version!==generation)return;rows=data;
            }if(version!==generation)return;loaded=true;render();dialog.hidden=false;});}catch(error){message(error.message);}}
        function reset(){++generation;rows=[];files=[];removed=new Set();changed=loaded=false;if(dialog){dialog.hidden=true;render();}}
        return {open,reset,settled:()=>pending,payload:()=>changed?{files:files.slice(),removeAttachmentIds:[...removed]}:undefined};
    }
    return {create};
})();
