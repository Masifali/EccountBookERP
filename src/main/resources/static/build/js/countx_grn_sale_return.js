'use strict';
let lineItems=[], loadedReturn={}, editReturnLine=-1, returnLookupVersion=0;
const value=id=>document.getElementById(id)?.value||'';
const numberValue=id=>Number(value(id))||0;
const optionText=id=>document.getElementById(id)?.selectedOptions[0]?.textContent||'';
function escapeHtml(v){return String(v??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));}
async function returnApi(path,options){return PurchaseRequest.track(async()=>{const response=await fetch('/api/purchase/sale-return-grn'+path,options);const data=await response.json();if(!response.ok||data?.success===false)throw new Error(data.message||data.detail||'Request failed');return data;});}
function showError(error){alert(error.message||'The request failed.');}
function selectReturn(id,v,label){const el=document.getElementById(id);if(!el)return;if(v&&!Array.from(el.options).some(o=>o.value===String(v)))el.add(new Option(label||String(v),v));$(el).val(v??'0');}
function setWinDefaultDate(){const d=new Date();$('#txtDocDate').val(d.getFullYear()+'-'+String(d.getMonth()+1).padStart(2,'0')+'-'+String(d.getDate()).padStart(2,'0'));}
$(document).ready(()=>{setWinDefaultDate();const id=new URLSearchParams(window.location.search).get('id');if(id)loadReturnRecord(id);});
async function loadReturnItems(supplierId){try{const version=++returnLookupVersion;const rows=await returnApi('/items?supplierId='+Number(supplierId));if(version!==returnLookupVersion)return;const select=$('#cmbItem').empty().append(new Option('-- Select Item --','0'));rows.forEach(r=>select.append(new Option(r.name,r.id)));onItemChange();}catch(error){showError(error);}}
function onItemChange(){const item=numberValue('cmbItem');const select=document.getElementById('cmbUom');for(const option of select.options){const own=!option.dataset.itemId||Number(option.dataset.itemId)===item;option.hidden=!own;option.disabled=!own;}if(select.selectedOptions[0]?.disabled)select.value='0';}
function calcLineAmount(){/* Gross, bill and stock weights are separate desktop fields. Quantity is not a currency multiplier. */}
function btnAddRow_Click(){
 const itemId=numberValue('cmbItem'),warehouseId=numberValue('cmbWarehouse'),itemUomId=numberValue('cmbUom'),qty=numberValue('txtQty');
 if(!itemId||!warehouseId||!itemUomId||qty<=0){alert('Select Item, Warehouse, UOM and enter quantity.');return;}
 const grossWeight=numberValue('txtRate'),netBillWeight=numberValue('txtAmount'),stockWeight=numberValue('txtStockWeight');
 if(grossWeight<=0||netBillWeight<=0||stockWeight<=0){alert('Gross, bill and stock weights must be greater than zero.');return;}
 const row={...(editReturnLine>=0?lineItems[editReturnLine]:{}),itemId,warehouseId,itemUomId,itemQty:qty,grossWeight,netBillWeight,stockWeight,
  itemName:optionText('cmbItem'),warehouseName:optionText('cmbWarehouse'),jobLotId:numberValue('cmbJobLot'),jobLotDescription:optionText('cmbJobLot'),
  cropYearId:numberValue('cmbCropYear'),cropYear:optionText('cmbCropYear'),packingTypeId:numberValue('cmbPackingType'),cityId:numberValue('cmbCity'),commentsDetail:value('txtLineRemarks')};
 if(editReturnLine>=0)lineItems[editReturnLine]=row;else lineItems.push(row);editReturnLine=-1;renderGrid();
}
function editReturnRow(index){const row=lineItems[index];if(!row)return;editReturnLine=index;
 for(const [id,key] of Object.entries({cmbItem:'itemId',cmbWarehouse:'warehouseId',cmbJobLot:'jobLotId',cmbCropYear:'cropYearId',cmbPackingType:'packingTypeId',cmbCity:'cityId'}))selectReturn(id,row[key]);onItemChange();selectReturn('cmbUom',row.itemUomId);
 for(const [id,key] of Object.entries({txtQty:'itemQty',txtRate:'grossWeight',txtAmount:'netBillWeight',txtStockWeight:'stockWeight',txtLineRemarks:'commentsDetail'}))$('#'+id).val(row[key]??'');
}
function removeLineItem(index){if(!confirm('Delete this detail row?'))return;lineItems.splice(index,1);editReturnLine=-1;renderGrid();}
function renderGrid(){
 $('#grdDetailTbody').html(lineItems.map((row,index)=>'<tr><td>'+String(index+1)+'</td>'+['itemCode','itemName','itemQty','grossWeight','netBillWeight','warehouseName','jobLotDescription','commentsDetail'].map(key=>'<td>'+escapeHtml(row[key])+'</td>').join('')+'<td><button type="button" onclick="editReturnRow('+index+')">Edit</button><button type="button" onclick="removeLineItem('+index+')">×</button></td></tr>').join(''));
 $('#lblTotalQty').text(lineItems.reduce((s,r)=>s+(Number(r.itemQty)||0),0).toFixed(2));$('#lblNetTotal').text(lineItems.reduce((s,r)=>s+(Number(r.netBillWeight)||0),0).toFixed(2));
}
async function btnSave_Click(){try{
 if(!numberValue('cmbSupplier')||!lineItems.length){alert('Select a customer and add at least one returned item.');return;}
 const result=await returnApi('/save',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({id:numberValue('txtId'),docNo:numberValue('txtDocNo'),docDate:value('txtDocDate'),supplierCustomerId:numberValue('cmbSupplier'),referenceDocNo:value('txtRefNo'),remarksHeader:value('txtRemarks'),details:lineItems})});
 await loadReturnRecord(result.id);alert(result.message);
 }catch(error){showError(error);}}
async function btnNew_Click(){try{
 returnLookupVersion++;loadedReturn={};lineItems=[];editReturnLine=-1;$('#txtId').val(0);renderGrid();
 document.querySelectorAll('.win-form-body input').forEach(input=>{if(input.id!=='txtId')input.value='';});document.querySelectorAll('.win-form-body select').forEach(select=>{$(select).val('0');});$('#cmbItem').empty().append(new Option('-- Select Item --','0'));setWinDefaultDate();
 const next=await returnApi('/next-code');$('#txtDocNo').val(next.docNo);$('#lblVoucherDisplayHeader').text('GRN-SR-'+next.docNo);document.getElementById('returnHistory').hidden=true;
 }catch(error){showError(error);}}
function btnRefresh_Click(){return btnNew_Click();}
async function loadReturnHistory(){try{const rows=await returnApi('/history');$('#returnHistoryBody').html(rows.map(r=>'<tr><td><a href="/purchase/grn-sale-return?id='+Number(r.id)+'" onclick="event.preventDefault();loadReturnRecord('+Number(r.id)+')">'+escapeHtml(r.docNo)+'</a></td>'+['docDate','supplierName','vehicleNo','remarks'].map(k=>'<td>'+escapeHtml(r[k])+'</td>').join('')+'</tr>').join(''));document.getElementById('returnHistory').hidden=false;}catch(error){showError(error);}}
async function loadReturnRecord(id){try{
 const version=++returnLookupVersion;const data=await returnApi('/'+Number(id));if(version!==returnLookupVersion)return;loadedReturn=data;
 for(const [control,key] of Object.entries({txtId:'Id',txtDocNo:'DocNo',txtRefNo:'ReferenceDocNo',txtRemarks:'RemarksHeader'}))$('#'+control).val(data[key]??'');$('#txtDocDate').val(String(data.DocDate||'').slice(0,10));selectReturn('cmbSupplier',data.SupplierCustomerId,data.SupplierName||data.SupplierCustomer);
 lineItems=(data.details||[]).map(row=>{const mapped={};for(const [k,v] of Object.entries(row))mapped[k[0].toLowerCase()+k.slice(1)]=v;mapped.warehouseName=row.WareHouseName;return mapped;});
 for(const row of lineItems)selectReturn('cmbItem',row.itemId,row.itemName);renderGrid();$('#lblVoucherDisplayHeader').text('GRN-SR-'+data.DocNo);document.getElementById('returnHistory').hidden=true;window.history.replaceState(null,'','/purchase/grn-sale-return?id='+Number(id));
 }catch(error){showError(error);}}

function returnAction(button,action){return PurchaseRequest.run(button,action).catch(showError);}
