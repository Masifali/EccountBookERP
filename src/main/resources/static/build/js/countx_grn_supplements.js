/* InvFrmGRN purchase breakup and empty-bag grid events. The Java endpoint performs
   breakup calculations; Save repeats them before calling the desktop procedures. */
let purchaseBreakupRows=[],emptyBagRows=[],breakupLocked=false,grnReferenceType=0;
let breakupDirty=false,emptyBagsDirty=false,breakupRequest=0,breakupPending=Promise.resolve();

function supplementRow(row) { const copy={};for(const [key,value] of Object.entries(row))copy[key.toLowerCase()]=value;return copy; }
function renderSupplements(data) {
    purchaseBreakupRows=(data.purchaseBreakups||[]).map(supplementRow);
    emptyBagRows=(data.emptyBags||[]).map(supplementRow);
    breakupLocked=!!data.breakupLocked;grnReferenceType=Number(data.referenceType??data.RefDocumentTypeId)||0;
    breakupDirty=false;emptyBagsDirty=false;breakupRequest++;breakupPending=Promise.resolve();
    drawPurchaseBreakups();drawEmptyBags();
}
function breakupEditable(){return grnReferenceType===105&&!breakupLocked;}
function gridNumber(value){return Number(value)||0;}
function numberCell(value,handler,disabled=false) {
    return '<input class="form-control text-end" type="number" step="any" value="'+gridNumber(value)+'" '+(disabled?'disabled':'')+' onchange="'+handler+'">';
}
function drawPurchaseBreakups() {
    const editable=breakupEditable();
    /* InvFrmGRN dtPurchaseBreakup column order (:683-696); Qty, UOM and EbWeight are the editable cells
       (grdPurchaseBreakUpSettings :1601). SuppShortWt/SupplierRcvdWeight show for 105 only, ScaleShortage/BillWeight
       are hidden for 106. */
    const columns=[['qty',1],['uom',1],['grossweight'],['ebweight',1],['ebtotal'],['netpacksize'],['netweight'],['suppliershortweight',0,'bk-105'],['supplierrcvdweight',0,'bk-105'],['scaleshortage',0,'bk-not106'],['billweight',0,'bk-not106']];
    $('#grdPurchaseBreakupsBody').html(purchaseBreakupRows.map((row,index)=>'<tr><td><button type="button" class="tool-btn" '+(!editable?'disabled':'')+' onclick="withButtonLoading(this,()=>deletePurchaseBreakup('+index+'))" aria-label="Delete breakup row">×</button></td><td><button type="button" class="tool-btn" '+(!editable?'disabled':'')+' onclick="withButtonLoading(this,addPurchaseBreakup)" aria-label="Add breakup row">+</button></td>'+columns.map(([key,input,cls])=>input?'<td>'+numberCell(row[key],'changePurchaseBreakup('+index+',\''+key+'\',this.value)',!editable)+'</td>':'<td'+(cls?' class="'+cls+'"':'')+' data-breakup-row="'+index+'" data-breakup-field="'+key+'">'+escapeHtml(row[key]??0)+'</td>').join('')+'</tr>').join(''));
    $('.bk-105').toggle(grnReferenceType===105);$('.bk-not106').toggle(grnReferenceType!==106);
    const button=document.getElementById('btnAddBreakup');if(button)button.disabled=!editable;
    $('#breakupState').text(breakupLocked?'Locked: this gate pass has another GRN':grnReferenceType===105?'':'Available for Market Purchase gate passes');
}
async function addPurchaseBreakup() {
    if(!breakupEditable())return;
    purchaseBreakupRows.push({id:0,qty:0,uom:0,ebweight:0});breakupDirty=true;drawPurchaseBreakups();await calculatePurchaseBreakups();
}
async function deletePurchaseBreakup(index) {
    if(!breakupEditable()||!confirm('Are you sure to Delete?'))return;
    purchaseBreakupRows.splice(index,1);if(!purchaseBreakupRows.length)purchaseBreakupRows.push({id:0,qty:0,uom:0,ebweight:0});
    breakupDirty=true;drawPurchaseBreakups();await calculatePurchaseBreakups();
}
function changePurchaseBreakup(index,key,value) {
    if(!breakupEditable()||!purchaseBreakupRows[index])return;
    purchaseBreakupRows[index][key]=gridNumber(value);breakupDirty=true;
    calculatePurchaseBreakups().catch(error=>alert(error.message));
}
function calculatePurchaseBreakups() {
    if(!breakupEditable())return Promise.resolve();
    const version=++breakupRequest;
    const payload={id:numeric('txtId'),inwardGatePassId:numeric('cmbGatePassNo'),partyWeight:numeric('txtSupplierWeight'),factoryWeight:numeric('txtFactoryWeight'),supplierShortWeightApply:$('#chkSupplierDeduct').prop('checked'),scaleShortWeightApply:$('#chkScaleDeduct').prop('checked'),purchaseBreakups:purchaseBreakupRows};
    breakupPending=api('/api/purchase/market-grn/calculate-breakups',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(payload)}).then(result=>{
        if(version!==breakupRequest)return;
        purchaseBreakupRows=result.purchaseBreakups.map(supplementRow);
        document.querySelectorAll('[data-breakup-field]').forEach(cell=>{cell.textContent=purchaseBreakupRows[Number(cell.dataset.breakupRow)]?.[cell.dataset.breakupField]??0;});
    });
    return breakupPending;
}
function recalculateBreakupHeader() {
    if(!breakupEditable())return;
    breakupDirty=true;calculatePurchaseBreakups().catch(error=>alert(error.message));
}
function bagSelect(row,index,key,templateId) {
    const template=document.getElementById(templateId);
    const options=template?template.innerHTML:'';
    return '<select data-bag-row="'+index+'" data-bag-field="'+key+'" data-dtcombo="single" class="form-control" onchange="changeEmptyBag('+index+',\''+key+'\',this.value)"><option value="0">Select</option>'+options+'</select>';
}
function drawEmptyBags() {
    const rows=emptyBagRows.length?emptyBagRows:[{}];
    if(!emptyBagRows.length)emptyBagRows=rows;
    $('#grdEmptyBagsBody').html(rows.map((row,index)=>'<tr><td><button type="button" class="tool-btn" '+(rows.length!==1?'disabled':'')+' onclick="addEmptyBagRow('+index+')" aria-label="Add empty-bag row">+</button></td>'+[['typeid','grnBagTypeOptions'],['itemid','grnBagItemOptions'],['bagscondition','grnBagConditionOptions']].map(([key,template])=>'<td>'+bagSelect(row,index,key,template)+'</td>').join('')+'<td>'+numberCell(row.receivedqty,'changeEmptyBag('+index+',\'receivedqty\',this.value)')+'</td><td>'+numberCell(row.purchaseqty,'changeEmptyBag('+index+',\'purchaseqty\',this.value)')+'</td><td><input class="form-control" value="'+escapeHtml(row.remarks||'')+'" onchange="changeEmptyBag('+index+',\'remarks\',this.value)"></td></tr>').join(''));
    document.querySelectorAll('[data-bag-field]').forEach(select=>{
        const value=emptyBagRows[Number(select.dataset.bagRow)][select.dataset.bagField]||0;
        if(value&&!Array.from(select.options).some(option=>Number(option.value)===Number(value)))select.add(new Option(String(value),value));
        select.value=String(value);
    });
}
function addEmptyBagRow(index) {
    if(emptyBagRows.length!==1)return;
    const row=emptyBagRows[index];emptyBagRows.push({purchaseorderid:row.purchaseorderid||0,typeid:row.typeid||0,bagscondition:row.bagscondition||0,itemid:0,receivedqty:0,purchaseqty:0,remarks:''});
    emptyBagsDirty=true;drawEmptyBags();
}
function changeEmptyBag(index,key,value) {
    const row=emptyBagRows[index];if(!row)return;
    row[key]=key==='remarks'?value:gridNumber(value);emptyBagsDirty=true;
    /* grdEmptyBags_CellUpdated (InvFrmGRN :2388), same checks, order and messages */
    const messages=[];
    if(row.purchaseqty<0){row.purchaseqty=0;messages.push('PurQty cannot be less than Zero');}
    if(row.receivedqty<0){row.receivedqty=0;messages.push('RecQty cannot be less than Zero');}
    if((row.typeid===4||row.typeid===5)&&key==='purchaseqty'){row.purchaseqty=0;messages.push('Retained or Returned Stock you cannot be purchase');}
    if(row.typeid===2&&key==='receivedqty'){row.receivedqty=0;messages.push('You cannot be add value RecQty because EmptyBagsType is Purchase Against Weight');}
    if(typeof onEmptyBagChanged==='function')onEmptyBagChanged(key);
    if(messages.length){drawEmptyBags();messages.forEach(m=>alert(m));}
}
async function supplementPayload() {
    if(breakupDirty)await calculatePurchaseBreakups();
    else await breakupPending;
    const payload={};
    if(breakupDirty)payload.purchaseBreakups=purchaseBreakupRows;
    if(emptyBagsDirty)payload.emptyBags=emptyBagRows.filter(row=>gridNumber(row.receivedqty)>0||gridNumber(row.purchaseqty)>0);
    return payload;
}

$(document).ready(function(){
    document.addEventListener('keydown',function(event){
        const button=event.target.closest('button');
        if(event.ctrlKey&&event.code==='Space'&&button&&!button.disabled&&button.closest('#grdEmptyBagsBody,#grdPurchaseBreakupsBody')) {
            event.preventDefault();button.click();
        }
    });
});
