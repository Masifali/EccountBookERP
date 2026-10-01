package com.mst.services;

import com.mst.repositories.SaleReturnGrnLookupRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

/**
 * SaleReturnGrn.cs - Grn (Sale Return), screen 866, DocumentTypeId 143.
 *
 * Save follows SaleReturnGrn.Insert() :1228-1490 (refusals in the same order, same text) and hands a
 * whitelisted InvGrn (only the fields the form fills) to the shared InvGrn DAL chain
 * (PurchaseGrnPersistenceService: Sp_InvGrn_Insert/Update, Sp_InvGrnDetail_Insert,
 * Sp_InvGrnDetailEmptyBags_Insert, Sp_InventoryTransactions_GetALLMethod,
 * usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn - DAL 0429 SetData).
 */
@Service
public class SaleReturnGrnService {
    private static final int TYPE = 143;

    @Autowired private com.mst.repositories.GrnNumberingRepository grnNumbering;
    @Autowired private com.mst.repositories.PurchaseGrnRecordRepository records;
    @Autowired private PurchaseGrnPersistenceService persistence;
    @Autowired private com.mst.repositories.PurchaseGrnLookupRepository lookups;
    @Autowired private SaleReturnGrnLookupRepository own;

    /**
     * InitializeComponentMethod :545 + the Bind methods it calls. The shared GRN lookup supplies the lists
     * both forms bind identically; the lists SaleReturnGrn binds differently are replaced here.
     */
    public Map<String,Object> getDropdowns(int org,int company) {
        Map<String,Object> result=new LinkedHashMap<>(lookups.all(TYPE));
        // PackingTypeDtFillFromGlobalAndBind :983 - "excludedIds" {1,2,5,9} is used with Contains, so these four are KEPT.
        @SuppressWarnings("unchecked") var packing=new ArrayList<>((List<Map<String,Object>>)result.get("packingTypes"));
        packing.removeIf(row->!Set.of(1,2,5,9).contains(num(row.get("id"))));
        result.put("packingTypes",packing);
        // EmptyBagsTypeAndConditionDbCall :2649 / EmptyBagsGridComboBind :2661-2664.
        result.put("bagConditions",options(own.bagConditions(),"Id","Type"));
        result.put("emptyBagItems",options(own.bagItems(),"Id","ItemName"));
        result.put("historySuppliers",options(own.historySuppliers(),"Id","ReferenceName"));
        result.remove("deliveryTerms"); // txtDeliverTerm is a read-only text box on this form (:651, designer ReadOnly).
        result.put("configs",own.configs());
        result.put("subsidiaryAccounts",own.subsidiaryAccounts());
        boolean wages=false;
        try { wages=own.wagesActive(); } catch(RuntimeException ignored) { /* USP_GetRefDocumentsForWages absent on databases without wages */ }
        result.put("wagesActive",wages);
        Map<String,Boolean> rights=new LinkedHashMap<>();
        @SuppressWarnings("unchecked") var base=(Map<String,Boolean>)result.get("rights");
        if(base!=null)rights.putAll(base);
        rights.put("CanViewAllRecord",records.canViewAll(TYPE));
        result.put("rights",rights);
        return result;
    }

    public int generateNextDocNo(int orgId, int compId, int branchId, int yearId) {
        return grnNumbering.next(orgId,compId,branchId,yearId,TYPE);
    }

    public List<Map<String, Object>> getHistory(int orgId, int compId, int branchId, int yearId,
                                                String fromDate, String toDate, Integer supplierId,
                                                Integer fromDocNo, Integer toDocNo, String dateType) {
        return records.history(TYPE,fromDate,toDate,supplierId,fromDocNo,toDocNo,dateType);
    }

    public Map<String,Object> getById(int id) { return records.load(id,TYPE); }

    public List<Map<String,Object>> pendingGatePasses() { return own.pendingGatePasses(); }

    public List<Map<String,Object>> historySuppliers() { records.requireRight(TYPE,"View"); return options(own.historySuppliers(),"Id","ReferenceName"); }

    /** GatePassRecordFill :2955. */
    public Map<String,Object> gatePass(int id) { return own.loadGatePass(id); }

    /** frmLoadGdnForGrnSaleReturn data (see SaleReturnGrnLookupRepository.pendingGdn). */
    public List<Map<String,Object>> pendingGdn(String from,String to,Integer customer) { return own.pendingGdn(from,to,customer); }

    @Transactional
    public Map<String,Object> saveSaleReturnGrn(Map<String,Object> payload) {
        Map<String,Object> in=ci(payload);
        int id=num(in.get("Id"));
        boolean updating=id>0;
        List<Map<String,Object>> rows=rows(in.get("details"));
        // Insert() :1238
        if(rows.isEmpty())throw new IllegalArgumentException("Grid Record Not Found");
        // FormValidation :1122
        if(num(in.get("DocNo"))==0)throw new IllegalArgumentException("DocNo Field is Required");
        int supplier=num(in.get("SupplierCustomerId"));
        if(supplier==0)throw new IllegalArgumentException("Supplier Field is Required");
        int gatePass=num(in.get("InwardGatePassId"));
        if(gatePass==0)throw new IllegalArgumentException("Gatepass Field is Required");
        double partyWeight=dbl(in.get("PartyWeight")), factoryWeight=dbl(in.get("FactoryWeight"));
        if(partyWeight==0.0)throw new IllegalArgumentException("Supplier Weight Field is Required");
        if(factoryWeight==0.0)throw new IllegalArgumentException("Factory Weight Field is Required");

        Map<String,Object> header=new LinkedHashMap<>();
        if(updating)header.put("Id",id);
        // :1261-1272 - transporter/freight are sent only when the freight amount is positive.
        double carriage=dbl(in.get("CarriageAmount"));
        int transporter=0,transporterParty=0;
        if(carriage>0.0) {
            transporter=num(in.get("TransporterId"));
            if(transporter==0)throw new IllegalArgumentException("Transporter Account field required");
            transporterParty=own.subsidiaryAccounts()?num(in.get("TransporterSupCustId")):0;
        } else carriage=0.0;
        header.put("TransporterId",transporter);
        header.put("CarriageAmount",carriage);
        header.put("TransporterSupCustId",transporterParty);
        header.put("ReferencePartyId",0);
        header.put("StockPartyId",0);
        String docDate=text(in.get("DocDate"));
        if(docDate!=null && !docDate.isEmpty())header.put("DocDate",docDate); // DocDate.Value :1285 (blank -> today, as the shared DAL default)
        header.put("DocNo",num(in.get("DocNo")));
        header.put("SupplierCustomerId",supplier);
        header.put("InwardGatePassId",gatePass);
        header.put("GpNo",own.gatePassNumber(gatePass));
        header.put("DeliveryTerm",text(in.get("DeliveryTerm")));
        header.put("VehicleType",text(in.get("VehicleType")));
        header.put("VehicleNo",text(in.get("VehicleNo")));
        header.put("BiltyNo",text(in.get("BiltyNo")));
        header.put("PartyWeight",partyWeight);
        header.put("FactoryWeight",factoryWeight);
        header.put("RemarksHeader",text(in.get("RemarksHeader")));
        header.put("ActionId",updating?2:1); // BLL 0576 Save :66/:74

        // Detail rows :1309-1330 (FillGdnDetailListCommonForInsertAndDelete :1492).
        double grnQty=0,gross=0,bill=0,stock=0;
        List<Map<String,Object>> details=new ArrayList<>();
        for(int i=0;i<rows.size();i++) {
            var r=rows.get(i); var d=new LinkedHashMap<String,Object>();
            if(updating && num(r.get("Id"))>0)d.put("Id",num(r.get("Id")));
            d.put("GdnId",num(r.get("GdnId"))); d.put("GdnDetailId",num(r.get("GdnDetailId")));
            d.put("GdnDocumentTypeId",num(r.get("GdnDocumentTypeId")));
            d.put("WarehouseId",num(r.get("WarehouseId"))); d.put("ItemId",num(r.get("ItemId")));
            d.put("CropYearId",num(r.get("CropYearId"))); d.put("CropYear",text(r.get("CropYear")));
            d.put("JobLotId",num(r.get("JobLotId"))); d.put("PackingTypeId",num(r.get("PackingTypeId")));
            d.put("ItemUomId",num(r.get("ItemUomId"))); d.put("ItemQty",dbl(r.get("ItemQty")));
            d.put("GrossWeight",dbl(r.get("GrossWeight"))); d.put("EBWPerUnit",dbl(r.get("EBWPerUnit")));
            d.put("EBWTotal",dbl(r.get("EBWTotal"))); d.put("WtCut",dbl(r.get("WtCut")));
            d.put("WtCutTotal",dbl(r.get("WtCutTotal"))); d.put("AdLsWeight",dbl(r.get("AdLsWeight")));
            d.put("NetBillWeight",dbl(r.get("NetBillWeight"))); d.put("StockWeight",dbl(r.get("StockWeight")));
            d.put("LabReportRef",text(r.get("LabReportRef"))); d.put("CityId",num(r.get("CityId")));
            d.put("AreaCity",text(r.get("AreaCity")));
            // FormHelper.ValidateField, in the desktop's order :1315-1324.
            required(d.get("WarehouseId"),"Warehouse",i); required(d.get("ItemId"),"Item",i);
            required(d.get("CropYearId"),"CropYear",i); required(d.get("JobLotId"),"JobLot",i);
            required(d.get("PackingTypeId"),"PackingType",i); required(d.get("ItemUomId"),"Uom",i);
            required(d.get("ItemQty"),"Qty",i); required(d.get("GrossWeight"),"GrossWeight",i);
            required(d.get("NetBillWeight"),"NetBillWeight",i); required(d.get("StockWeight"),"StockWeight",i);
            gross+=(double)d.get("GrossWeight"); grnQty+=(double)d.get("ItemQty");
            bill+=(double)d.get("NetBillWeight"); stock+=(double)d.get("StockWeight");
            details.add(d);
        }
        // :1331 - txtGrnWeight is the gate pass factory weight on a new GRN and the loaded grid total on an edit.
        if(gross!=dbl(in.get("GrnWeight")))throw new IllegalArgumentException("GrossWeight and GrnWeight Not Match");
        String term=text(in.get("DeliveryTerm"));
        // :1338-1373 (the tolerance Yes/No at :1340 is asked in the browser before posting).
        if("Load".equals(term)||"Load & PartyWeight".equals(term)||"Ponch & PartyWeight".equals(term)) {
            if(gross!=partyWeight)throw new IllegalArgumentException("GrossWeight and Supplier Weight Not Match");
        }
        if("Load & FactoryWeight".equals(term)||"Ponch & FactoryWeight".equals(term)) {
            if(gross!=factoryWeight)throw new IllegalArgumentException("GrossWeight and Factory Weight Not Match");
        }
        if("Ponch".equals(term)) {
            if(partyWeight>factoryWeight) { if(gross!=factoryWeight)throw new IllegalArgumentException("GrossWeight and Factory Weight Not Match"); }
            else if(gross!=partyWeight)throw new IllegalArgumentException("GrossWeight and Supplier Weight Not Match");
        }

        // Empty bags :1374-1461.
        List<Map<String,Object>> bags=new ArrayList<>();
        List<Map<String,Object>> bagRows=rows(in.get("emptyBags"));
        double rec=0,pur=0;
        for(var b:bagRows) {
            if(num(b.get("TypeId"))>0 && num(b.get("ItemId"))>0) {
                if(num(b.get("BagsCondition"))==0)throw new IllegalArgumentException("Please Select Bags_Condition First");
                rec+=dbl(b.get("ReceivedQty")); pur+=dbl(b.get("PurchaseQty"));
            }
        }
        if(!bagRows.isEmpty()) {
            if(flag(own.configs().get("EmptyBagsInofrmationCompulsoryOnGRN")) && grnQty!=rec+pur)
                throw new IllegalArgumentException("Empty Bags Quantity must be equal to GrnQty");
            for(var b:bagRows) {
                int type=num(b.get("TypeId"));
                if(num(b.get("ItemId"))<=0 || type<=0)continue;
                var row=new LinkedHashMap<String,Object>();
                row.put("PurchaseOrderId",num(b.get("PurchaseOrderId"))); row.put("TypeId",type);
                row.put("ItemId",num(b.get("ItemId"))); row.put("BagsCondition",num(b.get("BagsCondition")));
                row.put("ReceivedQty",dbl(b.get("ReceivedQty"))); row.put("PurchaseQty",dbl(b.get("PurchaseQty")));
                row.put("Remarks",b.get("Remarks")==null?null:b.get("Remarks").toString());
                if(dbl(row.get("ReceivedQty"))>0 || dbl(row.get("PurchaseQty"))>0)bags.add(row);
                switch(type) {
                    case 1: if(rec==0.0 && pur==0.0)throw new IllegalArgumentException("Received Qty Or Purchase Qty Required in EmptyBags grid"); break;
                    case 2: case 3: if(pur==0.0)throw new IllegalArgumentException("Purchase Qty Required in EmptyBags grid"); break;
                    case 4: case 5: if(rec==0.0)throw new IllegalArgumentException("Received Qty Required in EmptyBags grid"); break;
                    default: break;
                }
                if(grnQty!=rec+pur)throw new IllegalArgumentException("Empty Bags Quantity must be equal to GrnQty");
            }
        }

        Map<String,Object> clean=new LinkedHashMap<>(header);
        clean.put("details",details);
        clean.put("emptyBags",bags);
        clean.put("purchaseBreakups",new ArrayList<>()); // GRN.InvGrnPurchaseBreakUplist = new List (:1299)
        if(in.get("attachments")!=null)clean.put("attachments",in.get("attachments")); // AT changes (:1462), saved with the GRN
        var saved=new LinkedHashMap<String,Object>(persistence.save(clean,TYPE));
        // :1468/:1476 - the desktop shows the number that was in txtdocno, not the one the procedure assigned.
        saved.put("message",(updating?"Record Update Successfully [":"Record Save Successfully [")+num(in.get("DocNo"))+"]");
        saved.put("grossWeight",gross);
        return saved;
    }

    /** btnDelete_Click :1556 - InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete. */
    @Transactional
    public boolean deleteSaleReturnGrn(int id) {
        if(id<=0)throw new IllegalArgumentException("Record Not Found");
        records.delete(id,TYPE);
        return true;
    }

    /** FormHelper.ValidateField - message as ported in TradeBillAgainstGdnCmagtService.required (FormHelper.cs:503). */
    private static void required(Object value,String field,int rowIndex) {
        boolean bad=value==null || (value instanceof Integer && (Integer)value==0) || (value instanceof Double && (Double)value<=0.0);
        if(bad)throw new IllegalArgumentException(field+" is required in Detail Grid at row No: "+(rowIndex+1));
    }

    private static Map<String,Object> ci(Map<String,Object> m) { var t=new TreeMap<String,Object>(String.CASE_INSENSITIVE_ORDER); if(m!=null)t.putAll(m); return t; }
    private static List<Map<String,Object>> rows(Object v) {
        List<Map<String,Object>> out=new ArrayList<>();
        if(v==null)return out;
        if(!(v instanceof List))throw new IllegalArgumentException("Invalid grid data");
        for(Object o:(List<?>)v) {
            if(!(o instanceof Map))throw new IllegalArgumentException("Invalid grid data");
            var t=new TreeMap<String,Object>(String.CASE_INSENSITIVE_ORDER);
            ((Map<?,?>)o).forEach((k,x)->t.put(String.valueOf(k),x)); out.add(t);
        }
        return out;
    }
    private static int num(Object v) {
        if(v instanceof Number)return ((Number)v).intValue();
        if(v==null||v.toString().isBlank())return 0;
        try { return (int)Double.parseDouble(v.toString().replace(",","").trim()); } catch(NumberFormatException e) { return 0; }
    }
    private static double dbl(Object v) {
        if(v instanceof Number)return ((Number)v).doubleValue();
        if(v==null||v.toString().isBlank())return 0;
        try { return Double.parseDouble(v.toString().replace(",","").trim()); } catch(NumberFormatException e) { return 0; }
    }
    private static String text(Object v) { return v==null?null:v.toString().trim(); }
    private static boolean flag(String v) { return v!=null && ("1".equals(v.trim())||"true".equalsIgnoreCase(v.trim())); }
    private static List<Map<String,Object>> options(List<Map<String,Object>> rows,String id,String caption) {
        List<Map<String,Object>> result=new ArrayList<>();
        for(var row:rows) { var m=new LinkedHashMap<String,Object>(row); m.put("id",row.get(id)); m.put("name",row.get(caption)); result.add(m); }
        return result;
    }
}
