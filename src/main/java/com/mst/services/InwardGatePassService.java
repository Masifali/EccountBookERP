package com.mst.services;

import com.mst.models.InwardGatePass;
import com.mst.models.InwardGatePassDetail;
import com.mst.models.InwardGatePassPurchaseBreakUp;
import com.mst.repositories.InwardGatePassRepository;
import com.mst.repositories.InwardGatePassRecordRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class InwardGatePassService {

    @Autowired
    private InwardGatePassRepository repository;

    @Autowired private InwardGatePassRecordRepository records;
    @Autowired private CurrentUserContext context;

    public Map<String, Object> getDropdowns(Integer orgId, Integer compId) {
        Map<String, Object> map = new HashMap<>();
        int branch=context.currentBranchId();
        List<Map<String,Object>> orderTypes=repository.getOrderTypes(orgId, compId, branch);
        Set<Integer> orderTypeIds=new HashSet<>();
        for (var row:orderTypes) if (row.get("id") instanceof Number) orderTypeIds.add(((Number)row.get("id")).intValue());
        map.put("suppliers", repository.getSuppliers(orgId, compId));
        map.put("cities", repository.getCities(orgId, compId));
        map.put("vehicleTypes", repository.getVehicleTypes(orgId, compId));
        map.put("gatePassTypes", repository.getGatePassTypes(orgId, compId));
        map.put("orderTypes", orderTypes);
        map.put("items", repository.getItems(orgId, compId));
        map.put("weighBridges", repository.getWeighBridges());
        map.put("packingTypes", repository.getPackingTypes());
        map.put("transitVehicles", List.of()); // Depends on the selected supplier/order.
        map.put("statuses", repository.getStatuses(orgId, compId));
        // OrderTypeFill: the PO information tab exists only when type 41 (or 1500) is offered; the last one wins.
        int poInfoType=0;
        for (var row:orderTypes) { int id=row.get("id") instanceof Number?((Number)row.get("id")).intValue():0; if (id==41||id==1500) poInfoType=id; }
        map.put("poInfoDocumentTypeId", poInfoType);
        Map<String,List<Map<String,Object>>> poCombos=repository.getPoInfoCombos(orgId, compId, poInfoType);
        map.put("documentTypes", poCombos.get("documentTypes"));
        map.put("poSuppliers", poCombos.get("suppliers"));
        map.put("historySuppliers", repository.getHistorySuppliers(orgId, compId, branch));
        map.put("allSupplierCustomers", orderTypeIds.contains(241)||orderTypeIds.contains(204)?repository.getAllSupplierCustomers(orgId, compId):List.of());
        map.put("saleInvoiceParties", orderTypeIds.contains(98)||orderTypeIds.contains(52)?repository.getSaleInvoiceParties(orgId, compId):List.of());
        map.put("showPurchaseBreakup", orderTypeIds.contains(105));
        map.put("canSave", records.hasRight("Save"));
        map.put("canUpdate", records.hasRight("Update"));
        map.put("canPrint", records.hasRight("Print"));
        map.put("canApprove", records.hasRight("Approve"));
        // InwardGatePass_Load: cmbcity.Value = ConfigKey of "City Area"; ChkIsApproved.Enabled uses
        // "AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt" (txtAccessWeight_TextChanged).
        String city=repository.config(orgId,compId,"City Area");
        map.put("defaultCityId", city.matches("-?\\d+")?Integer.parseInt(city):0);
        String hold=repository.config(orgId,compId,"AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt");
        map.put("holdAccessWeightForApproval", "1".equals(hold)||"true".equalsIgnoreCase(hold));
        return map;
    }

    public List<Map<String,Object>> getHistorySuppliers() {
        return repository.getHistorySuppliers(context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId());
    }

    public Map<String,Object> getPoInfoCombos(int poInfoDocumentTypeId) {
        return new HashMap<>(repository.getPoInfoCombos(context.currentOrganizationId(),context.currentCompanyId(),poInfoDocumentTypeId));
    }

    public Map<String, Object> findDriverBioByCnic(String cnic) {
        return repository.findDriverBio(context.currentOrganizationId(),context.currentCompanyId(),cnic,null);
    }

    public List<Map<String,Object>> getOpenGatePasses() {
        return repository.getOpenGatePasses(context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId());
    }

    public Map<String, Object> findDriverBioByCell(String cell) {
        return repository.findDriverBio(context.currentOrganizationId(),context.currentCompanyId(),null,cell);
    }

    public List<Map<String, Object>> getPoInfoGrid(Integer orgId, Integer compId,
            String fromDate, String toDate, Double fromDocNo, Double toDocNo,
            Integer supplierId, Integer documentTypeId, Integer expiryDays, String dateField) {
        return repository.getPoInfoGrid(orgId, compId, context.currentBranchId(), context.currentFinancialYearId(),
                fromDate, toDate, fromDocNo, toDocNo, supplierId, documentTypeId, expiryDays, dateField);
    }

    /** CmbOrderno_Leave / ReadById order lookups: 41 and 700 use SupplierByPurchaseOrderNo, 1500 the Steel procedure. */
    public List<Map<String,Object>> getOrderPartyItems(int documentTypeId,int number,String date,int gatePassId) {
        if (gatePassId>0) records.require(gatePassId);
        if (documentTypeId==1500)
            return repository.getSteelOrderPartyItems(context.currentOrganizationId(),context.currentCompanyId(),context.currentFinancialYearId(),number,date);
        if (documentTypeId!=41 && documentTypeId!=700) throw new IllegalArgumentException("Order lookup is available for Purchase Type 41, 700 and 1500 only");
        return repository.getOrderPartyItems(context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId(),documentTypeId,number,date,gatePassId);
    }

    /** LabDataGetByGpId(RecId) - called by every Edit path before ReadById. */
    public Map<String,Object> getLabData(int gatePassId) {
        records.require(gatePassId);
        return repository.getLabData(context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),gatePassId);
    }

    public Map<String, Object> generateNextNumbers(Integer orgId, Integer compId, Integer branchId, Integer yearId, Integer docTypeId, String gatepassType) {
        if (docTypeId!=51) throw new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.BAD_REQUEST,"This form uses document type 51 only");
        Map<String, Object> res = new HashMap<>();
        Integer gpSrNo = repository.generateGpCode(orgId, compId, branchId, yearId, docTypeId);
        Integer gpTypeSrNo = repository.generateGpTypeCode(orgId, compId, branchId, yearId, gatepassType);
        res.put("gpSrNo", gpSrNo);
        res.put("gpTypeSrNo", gpTypeSrNo);
        return res;
    }

    private static final java.util.regex.Pattern VEHICLE_NO=java.util.regex.Pattern.compile("^[A-Z]{1,6}-\\d{1,6}$");
    private static double num(Number v) { return v==null?0d:v.doubleValue(); }
    private static int whole(Number v) { return v==null?0:v.intValue(); }
    private static String text(String v) { return v==null?"":v.trim(); }
    /** float.Parse(text) assigned to a double field (btnsave_Click: Freight, NetPaid, SupplierWeight, DifferenceWeight). */
    private static double viaFloat(double v) { return (double)(float)v; }
    /** C# double.ToString() inside the interpolated confirmation text. */
    private static String cs(double v) { return v==Math.rint(v) && Math.abs(v)<1e15?String.valueOf((long)v):String.valueOf(v); }
    private static int rowInt(Map<String,Object> row,String key) { Object v=row==null?null:row.get(key); return v instanceof Number?((Number)v).intValue():0; }
    private static double rowDouble(Map<String,Object> row,String key) { Object v=row==null?null:row.get(key); return v instanceof Number?((Number)v).doubleValue():0d; }

    /**
     * btnsave_Click (InwardGatePass.cs:3320) and btnupdate_Click (:3612): formvalidation() (:1709), the type checks,
     * the PO access-weight rule and the model the desktop hands to GatePassInward.Save (BLL 0567 -> DAL 0421 SetData:
     * USP_DriverBiodata_InsertIfNotExists when driverBioDataId == 0, Sp_GatePassInward_Insert/Update,
     * USP_GatePassInwardPurchaseBreakUp_Insert per grid row). MessageBox confirmations that depend on server data come
     * back as {confirmKey, message}; the page asks and resends with the key in `confirmed`.
     */
    @Transactional
    public Map<String, Object> saveRecord(InwardGatePass obj, Set<String> confirmed, boolean isApprovedChecked) {
        boolean updating=obj.getId()!=null && obj.getId()>0;
        records.requireRight(updating?"Update":"Save");
        Map<String,Object> previous=updating?records.require(obj.getId()):null;
        int org=context.currentOrganizationId(), company=context.currentCompanyId(), branch=context.currentBranchId(),
                year=context.currentFinancialYearId(), user=context.currentUserId();
        boolean approveRight=records.hasRight("Approve");
        int refType=whole(obj.getRefDocumentTypeId());
        String gpType=obj.getGatepassType()==null?"":obj.getGatepassType();
        String orderTypeCaption=obj.getOtherSupCust()==null?"":obj.getOtherSupCust();

        // Factory net weight as the form shows it: GetWeighBridgeWeightAndTicketNos(RecId) on load, empty on a new record.
        double factoryWeight=0;
        if (updating) for (var row:repository.getWeighBridgeWeights(org,company,obj.getId())) factoryWeight+=rowDouble(row,"NetWbWeight");
        double supplierFirst=num(obj.getSupplierFirstWeight()), supplierSecond=num(obj.getSupplierSecondWeight());
        double supplierWeight=num(obj.getSupplierWeight());
        if (supplierFirst>0 || supplierSecond>0) supplierWeight=Math.round(Math.abs(supplierFirst-supplierSecond)*1000d)/1000d; // "#,##0.###"
        double differenceWeight=Math.abs(factoryWeight-supplierWeight);                                                // CalculateDifferenceWeights
        double freight=num(obj.getFreight()), advanceParty=num(obj.getAdvanceByParty()), advanceFactory=num(obj.getAdvanceByFactory());
        int qty=whole(obj.getNoOfPackages());
        double packUnit=num(obj.getPackUnit()), weightCompared=num(obj.getWeightComparedToPoWt());

        // ---------------- formvalidation(), in the desktop's order ----------------
        if (text(gpType).isEmpty()) throw new IllegalArgumentException("GatePassType Field Required");
        String vehicle=text(obj.getVehicleNo());
        if (vehicle.isEmpty()) throw new IllegalArgumentException("VehicleNo Field Required");
        if (!VEHICLE_NO.matcher(vehicle.toUpperCase(Locale.ROOT)).matches()) throw new IllegalArgumentException("Vehicle no is not valid. Please check!");
        if ("Import".equals(gpType) || "Import Order".equals(orderTypeCaption)) {
            String c1=obj.getContainer()==null?"":obj.getContainer(), c2=obj.getContainer1()==null?"":obj.getContainer1();
            if (c1.isEmpty() && c2.isEmpty()) throw new IllegalArgumentException("Please Select At least One Container No In Case of Import");
            if (c1.isEmpty()) throw new IllegalArgumentException("Please Select Container No 1");
            if (c1.equals(c2)) throw new IllegalArgumentException("Cannot Save With Same Container No Please Check");
        }
        if (whole(obj.getCityId())<=0) throw new IllegalArgumentException("CityName Field Required");
        if (text(obj.getVehicleType()).isEmpty()) throw new IllegalArgumentException("VehicleType Field Required");
        if (qty<=0) throw new IllegalArgumentException("ItemQty Field Required");
        if (packUnit<=0) throw new IllegalArgumentException("PackUom Field Required");
        if ((int)packUnit>110) throw new IllegalArgumentException("Pack Uom Field can't greater 110");
        if (weightCompared<=0) throw new IllegalArgumentException("Weight Field Required");
        if (whole(obj.getGpSrNo())==0) throw new IllegalArgumentException("GatePassNo Field Required");
        if (text(obj.getStatus()).isEmpty()) throw new IllegalArgumentException("Status Field Required");
        if (text(obj.getDocAttachment()).isEmpty()) throw new IllegalArgumentException("WeighBridge Status Field Required");
        String orderNo=text(obj.getSupplierContractCode());
        if (orderNo.isEmpty() || "0".equals(orderNo)) throw new IllegalArgumentException("OrderNo Field Required");
        double totalAdvance=advanceParty+advanceFactory;
        if ((freight>0 || totalAdvance>0) && totalAdvance>freight) throw new IllegalArgumentException("Advance freight cannot be greater than bilty freight...");
        if ((refType==41||refType==105||refType==106) && "Accepted".equals(text(obj.getStatus())) && factoryWeight==0)
            throw new IllegalArgumentException("Factory weight is required");
        if (refType==41 && (supplierFirst>0 || supplierSecond>0)) {
            if (supplierFirst==0) throw new IllegalArgumentException("Supplier First Weight Field Required");
            if (supplierSecond==0) throw new IllegalArgumentException("Supplier Second Weight Field Required");
        }
        if (refType!=104 && refType!=175 && whole(obj.getSupplierCustomerId())==0) throw new IllegalArgumentException("Supplier Name Field Required");
        if ((refType==204||refType==241) && whole(obj.getItemId())==0) throw new IllegalArgumentException("Please select Variety First");

        if (updating && factoryWeight>supplierWeight) {
            double diff=factoryWeight-supplierWeight, max=Math.rint(0.03*factoryWeight), min=Math.rint(0.01*factoryWeight); // Math.Round(x, 0): banker's rounding
            String remarks=text(obj.getWeightDiffComments());
            if (diff>=max) {
                if (remarks.isEmpty()) throw new IllegalArgumentException("Difference '"+cs(diff)+"' between Factory Weight '"+cs(factoryWeight)+"' and Supplier Weight '"+cs(supplierWeight)+"'\n"
                        +"is **greater than or equal to** maximum allowed 3% of factory weight ("+cs(max)+").\n\nPlease add Weight Difference Remarks if you want to proceed.");
            } else if (diff>=min && !confirmed.contains("weightDifference")) {
                return confirmation("weightDifference","Difference '"+cs(diff)+"' between Factory Weight '"+cs(factoryWeight)+"' and Supplier Weight '"+cs(supplierWeight)+"'\n"
                        +"is **greater than or equal to** the minimum allowed 1% ("+String.format(Locale.ROOT,"%.2f",min)+")\n"
                        +"but **less than** the maximum allowed 3% ("+String.format(Locale.ROOT,"%.2f",max)+").\n\nDo you want to proceed?");
            }
        }
        if (refType==105 && (advanceFactory>0 || advanceParty>0))
            throw new IllegalArgumentException("In Case Of Market Purchase Advance by Factory or Advance by Party can't be greater than 0");

        // ---------------- the model handed to GatePassInward.Save ----------------
        int gpSrNo=whole(obj.getGpSrNo());
        int purchaseOrderId=whole(obj.getPurchaseOrderId());
        if (refType==105||refType==106||refType==204||refType==241||(!updating && (refType==98||refType==52))) purchaseOrderId=gpSrNo;
        else if (updating && refType==175) purchaseOrderId=rowInt(previous,"PurchaseOrderId");
        Map<String,Object> exact=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        exact.put("Id",updating?obj.getId():0);
        exact.put("BiltyNo",text(obj.getBiltyNo()));
        exact.put("CityId",whole(obj.getCityId()));
        exact.put("CompanyId",company); exact.put("OrganizationId",org); exact.put("BranchesId",branch); exact.put("FinancialYearId",year);
        exact.put("DocumentTypeId",51);
        exact.put("Freight",updating?freight:viaFloat(freight));
        exact.put("AdvanceByParty",advanceParty); exact.put("AdvanceByFactory",advanceFactory);
        exact.put("GatepassType",gpType);
        exact.put("GpDate",obj.getGpDate()); exact.put("BiltyDate",obj.getBiltyDate());
        exact.put("Container",obj.getContainer()==null?"":obj.getContainer()); exact.put("Container1",obj.getContainer1()==null?"":obj.getContainer1());
        exact.put("GpSrNo",gpSrNo); exact.put("GpTypeSrNo",whole(obj.getGpTypeSrNo()));
        exact.put("InDateTimeStamp",updating?null:new Date());      // btnsave: DateTime.Now; btnupdate leaves it unset
        exact.put("OutDateTimeStamp",updating?new Date():null);     // btnupdate: DateTime.Now
        exact.put("OtherSupCust",orderTypeCaption);
        exact.put("NoOfPackages",qty); exact.put("PackUnit",packUnit); exact.put("WeightComparedToPoWt",weightCompared);
        exact.put("OtherRemarks",text(obj.getOtherRemarks()));
        exact.put("SupplierContractCode",orderNo);
        exact.put("VehicleNo",vehicle.toUpperCase(Locale.ROOT)); exact.put("VehicleType",text(obj.getVehicleType()));
        exact.put("SupplierCustomerId",whole(obj.getSupplierCustomerId()));
        exact.put("SupplierDispatchId",whole(obj.getSupplierDispatchId()));
        exact.put("PackingTypeId",whole(obj.getPackingTypeId()));
        exact.put("Status",updating?obj.getStatus():text(obj.getStatus()));
        exact.put("GpQRCode",updating?previous.get("GpQRCode"):null);
        exact.put("NetPaid",updating?num(obj.getNetPaid()):viaFloat(num(obj.getNetPaid())));
        exact.put("SupplierFirstWeight",supplierFirst); exact.put("SupplierSecondWeight",supplierSecond);
        exact.put("SupplierWeight",updating?supplierWeight:viaFloat(supplierWeight));
        exact.put("FactoryWeight",updating?factoryWeight:0d);                         // btnsave: FactoryWeight = 0.0
        exact.put("DifferenceWeight",updating?differenceWeight:viaFloat(differenceWeight));
        exact.put("FreightOn",1);
        exact.put("DocAttachment",obj.getDocAttachment());
        exact.put("WeighBridgeId",0); // Conversion.ToInt of the ",ticket,..." slip text, 0 when no weighbridge row
        exact.put("VarietyName",text(obj.getVarietyName())); exact.put("ItemId",whole(obj.getItemId()));
        exact.put("RefDocumentTypeId",refType); exact.put("PurchaseOrderId",purchaseOrderId);
        exact.put("EntryUser",user);
        exact.put("EntryDate",null);   // neither button sets EntryDate
        exact.put("ModifyDate",null);  // nor ModifyDate (the update procedure writes @ModifyDate as sent)
        exact.put("ModifyUser",updating?user:0);
        exact.put("WeightDiffComments",updating?text(obj.getWeightDiffComments()):null);
        exact.put("WarehouseId",updating?rowInt(previous,"WarehouseId"):0);
        exact.put("RefDocumentEntryNo",0);
        // Approval / posting: defaults of a new model unless a branch below sets them.
        exact.put("IsApproved",false); exact.put("PostState",false); exact.put("PostDate",null); exact.put("PostUser",0);
        exact.put("ActionIdForSpecialApproval",0); exact.put("AccessWeight",0d);
        if (!updating && approveRight) exact.put("IsApproved",isApprovedChecked);
        if (!updating) {
            if (refType==41) {
                Map<String,Object> access=repository.getPoAccessWeight(org,company,gpType,purchaseOrderId,refType,weightCompared,0d,0);
                if (access!=null) {
                    double accessWeight=rowDouble(access,"AccessWeight");
                    if (accessWeight>0) {
                        if (!confirmed.contains("accessWeight")) return confirmation("accessWeight","The GatePass weight exceeds the PO weight. The remaining PO weight is "+cs(rowDouble(access,"BalWeightOfPo"))+" and the access weight is "+cs(accessWeight)+".\nAre you sure you want to Proceed?");
                        exact.put("AccessWeight",accessWeight); exact.put("IsApproved",false); exact.put("PostState",false); exact.put("ActionIdForSpecialApproval",1);
                    } else { exact.put("PostDate",new Date()); exact.put("PostState",approveRight); exact.put("PostUser",user); }
                }
            } else { exact.put("PostDate",new Date()); exact.put("PostState",approveRight); exact.put("PostUser",user); }
        } else if (refType==41 && rowInt(previous,"ActionIdForSpecialApproval")!=2) {
            double compare=factoryWeight-qty*2.0; // FactoryWeight minus 2 kg empty-bag weight per bag
            Map<String,Object> access=repository.getPoAccessWeight(org,company,gpType,purchaseOrderId,refType,weightCompared,compare,obj.getId());
            if (access!=null && Math.abs(rowDouble(access,"AccessWeight"))>0) {
                double accessWeight=rowDouble(access,"AccessWeight");
                if (!confirmed.contains("accessWeight")) return confirmation("accessWeight","The GatePass weight exceeds the PO weight. The remaining PO weight is "+cs(rowDouble(access,"BalWeightOfPo"))+" and the access weight is "+cs(accessWeight)+".\nAre you sure you want to Proceed?");
                exact.put("AccessWeight",accessWeight); exact.put("IsApproved",false); exact.put("PostState",false); exact.put("ActionIdForSpecialApproval",1);
            } else if (rowInt(previous,"ActionIdForSpecialApproval")==1) {
                exact.put("IsApproved",isApprovedChecked && approveRight); exact.put("PostDate",new Date()); exact.put("PostState",approveRight);
                exact.put("PostUser",user); exact.put("ActionIdForSpecialApproval",0); exact.put("AccessWeight",0d);
            }
        }
        exact.put("driverBioDataId",whole(obj.getDriverBioDataId()));
        exact.put("DriverCNICNO",obj.getDriverCNICNO()); exact.put("DriverMobileNo",obj.getDriverMobileNo()); exact.put("DriverName",obj.getDriverName());

        // Purchase BreakUp: every grid row as it stands (the default blank row included); a loaded list is read-only.
        List<InwardGatePassPurchaseBreakUp> breakUps=new ArrayList<>();
        List<Map<String,Object>> stored=updating?repository.getPurchaseBreakUpsByHeaderId(obj.getId()):List.of();
        if (!stored.isEmpty()) {
            for (var row:stored) {
                InwardGatePassPurchaseBreakUp b=new InwardGatePassPurchaseBreakUp();
                b.setQty(rowDouble(row,"Qty")); b.setUom(rowDouble(row,"UOM")); b.setGrossWeight(rowDouble(row,"GrossWeight"));
                b.setEbWeight(rowDouble(row,"EbWeight")); b.setEbTotal(rowDouble(row,"EBTotal")); b.setNetWeight(rowDouble(row,"NetWeight"));
                breakUps.add(b);
            }
        } else if (obj.getGatePassInwardPurchaseBreakUpList()!=null) {
            for (InwardGatePassPurchaseBreakUp b:obj.getGatePassInwardPurchaseBreakUpList()) {
                double q=num(b.getQty()), u=num(b.getUom()), eb=num(b.getEbWeight());
                b.setQty(q); b.setUom(u); b.setEbWeight(eb); b.setGrossWeight(q*u); b.setEbTotal(q*eb); b.setNetWeight(q*u+q*eb); // grdPurchaseBrakup_CellUpdated
                breakUps.add(b);
            }
        }

        obj.setOrganizationId(org); obj.setCompanyId(company); obj.setBranchesId(branch); obj.setFinancialYearId(year); obj.setDocumentTypeId(51);
        obj.setEntryUser(user); obj.setEntryDate(null); obj.setModifyDate(null); obj.setPostDate((Date)exact.get("PostDate"));
        try {
            if (whole(obj.getDriverBioDataId())==0) {
                Integer bioId=repository.saveDriverBio(obj);
                exact.put("driverBioDataId",bioId==null?0:bioId);
            }
            Integer headerId=repository.saveHeader(obj,exact);
            for (InwardGatePassPurchaseBreakUp b:breakUps) { b.setInwardGatePassId(headerId); repository.savePurchaseBreakUp(b); }
            Map<String,Object> res=new HashMap<>();
            res.put("success",true); res.put("igpId",headerId);
            res.put("message",updating?"Record Update Successfully ["+gpSrNo+"]":"Record Save Successfully["+gpSrNo+"]");
            return res;
        } catch (org.springframework.dao.DataAccessException error) {
            Throwable cause=error.getMostSpecificCause();
            if (cause instanceof java.sql.SQLException && ((java.sql.SQLException)cause).getErrorCode()>=50000)
                throw new IllegalStateException(cause.getMessage(),error); // RAISERROR text, as the desktop's "Database Error" box shows it
            throw error;
        }
    }

    private static Map<String,Object> confirmation(String key,String message) {
        Map<String,Object> res=new HashMap<>();
        res.put("success",false); res.put("confirmKey",key); res.put("message",message);
        return res;
    }

    public Map<String, Object> getById(Integer id) {
        records.require(id);
        Map<String, Object> res = new HashMap<>();
        Map<String, Object> header = repository.getHeaderById(id);
        if (header != null) {
            List<Map<String, Object>> details = repository.getDetailsByHeaderId(id);
            List<Map<String, Object>> breakUps = repository.getPurchaseBreakUpsByHeaderId(id);
            res.put("header", header);
            res.put("details", details);
            res.put("purchaseBreakUps", breakUps);
            res.put("weighBridgeWeights", repository.getWeighBridgeWeights(context.currentOrganizationId(),context.currentCompanyId(),id));
            res.put("lab", repository.getLabData(context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),id));
            res.put("success", true);
        } else {
            res.put("success", false);
            res.put("message", "Record not found");
        }
        return res;
    }

    public List<Map<String,Object>> getTransitVehicles(int supplier,int order,int gatePass) {
        if (gatePass>0) records.require(gatePass);
        return repository.getTransitVehicles(context.currentOrganizationId(),context.currentCompanyId(),supplier,order,gatePass);
    }

    public Map<String, Object> deleteRecord(Integer id, Integer orgId, Integer compId) {
        // Desktop btnDelete_Click is empty and InitializeComponent hides the button.
        throw new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.METHOD_NOT_ALLOWED,"The desktop Inward Gate Pass form does not support header deletion");
    }

    public List<Map<String, Object>> getHistory(Integer orgId, Integer compId, Integer branchId, Integer yearId,
                                                Integer docTypeId, String fromDate, String toDate,
                                                Double fromDocNo, Double toDocNo, Integer supplierId) {
        return getHistory(fromDate,toDate,fromDocNo,toDocNo,supplierId,"docDate");
    }

    public List<Map<String,Object>> getHistory(String fromDate,String toDate,Double fromDocNo,Double toDocNo,Integer supplierId,String dateField) {
        return repository.getHistory(context.currentOrganizationId(), context.currentCompanyId(), context.currentBranchId(), context.currentFinancialYearId(), 51, fromDate, toDate, fromDocNo, toDocNo, supplierId, records.canViewAll(), context.currentUserId(),dateField);
    }
}
