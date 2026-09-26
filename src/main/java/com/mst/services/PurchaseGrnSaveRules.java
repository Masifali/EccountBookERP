package com.mst.services;

import com.mst.repositories.PurchaseGrnFormRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import static com.mst.repositories.PurchaseGrnFormRepository.dbl;
import static com.mst.repositories.PurchaseGrnFormRepository.flag;
import static com.mst.repositories.PurchaseGrnWriteRepository.number;

/**
 * InvFrmGRN.Insert (:3553-3940) refusals for Purchase GRN (46), repeated on the server so a
 * request that skipped the page cannot store what the desktop refuses. Confirmation prompts
 * ("Are you sure…", "You have not Apply…") stay on the page; only hard refusals are here, in the
 * desktop's words.
 *
 * Also applies Insert's two header rewrites for Market Purchase (105): OtherCharges takes the
 * carriage amount and CarriageAmount becomes Σ detail FreightAmount (:3778-3782).
 */
@Service
public class PurchaseGrnSaveRules {
    private final PurchaseGrnFormRepository form;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    public PurchaseGrnSaveRules(PurchaseGrnFormRepository form,JdbcTemplate jdbc,CurrentUserContext context) {
        this.form=form;this.jdbc=jdbc;this.context=context;
    }

    public static final class Refusal extends IllegalArgumentException { public Refusal(String m){super(m);} }

    /** C#'s double formatting in interpolated messages: 1200 not 1200.0. */
    static String f(double v) {
        if(v==Math.rint(v)&&Math.abs(v)<1e15)return Long.toString((long)v);
        String s=Double.toString(v);return s.contains("E")?java.math.BigDecimal.valueOf(v).toPlainString():s;
    }
    private static boolean differs(double a,double b){return Math.abs(a-b)>0.0001;}

    public void apply(Map<String,Object> header,List<Map<String,Object>> details,List<Map<String,Object>> emptyBags,
                      List<Map<String,Object>> breakups,Map<String,Object> incoming,int id) {
        var cfg=form.configValues();
        int gpId=number(header.get("InwardGatePassId"));
        boolean updating=id>0;
        int ref,freightId;
        java.time.LocalDate gpDate=null;
        double chargeToParty=dbl(incoming.get("ChargeToPartyAmountFV"));
        double receivedWeight=0;
        if(updating) {
            var rows=jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @Id=?,@Activity='ReadById'",id);
            var h=rows.isEmpty()?Map.<String,Object>of():rows.get(0);
            if(number(h.get("InvoiceId"))>0)throw new Refusal("This Document is referred in invoice. So you can't update this record.");
            ref=number(h.get("RefDocumentTypeId"));freightId=number(h.get("FreightId"));
            if(h.containsKey("ChargeToPartyAmountFV"))chargeToParty=dbl(h.get("ChargeToPartyAmountFV"));
            if(ref==105) {
                // BalanceWeight from the proc already subtracts this GRN's own rows; add them back so
                // an unchanged update is not refused against itself.
                var w=form.receivedWeight(gpId);
                double own=0;for(var r:jdbc.queryForList("EXEC dbo.Sp_InvGrnDetail_GetAllMethod @Id=?,@Activity='ReadByInvGrnID'",id))own+=dbl(r.get("GrossWeight"));
                receivedWeight=dbl(w.get("ReceivedWeight"))-own;
            }
        } else {
            var pending=form.pendingRow(gpId);
            if(pending==null)throw new Refusal("This gate pass is not pending for GRN.");
            if(!"Accepted".equals(Objects.toString(pending.get("Status"),"")))throw new Refusal("Status Not Accepted Please check status");
            if(!flag(cfg.get("LabCompulsoryNotCheckingOnGRN"))&&number(pending.get("LastLabId"))<=0)throw new Refusal("Lab is pending for this gate pass. Please do lab first then Load");
            if(number(pending.get("FreightSpecialApprovalStatusId"))==1)throw new Refusal("Please complete the Freight Voucher approval before proceeding further");
            ref=number(pending.get("RefDocumentTypeId"));freightId=number(pending.get("FreightId"));
            receivedWeight=dbl(pending.get("ReceivedWeight"));
            Object d=pending.get("GpDate");
            if(d instanceof java.sql.Timestamp)gpDate=((java.sql.Timestamp)d).toLocalDateTime().toLocalDate();
            else if(d instanceof java.sql.Date)gpDate=((java.sql.Date)d).toLocalDate();
            else if(d!=null&&d.toString().length()>=10)gpDate=java.time.LocalDate.parse(d.toString().substring(0,10));
        }
        boolean deductionPolicyOn=flag(cfg.get("DeductionPolicyForGrnIsOn"));
        boolean exclude=flag(cfg.get("ExcludeWeightShortageBusinessOnGrn"));
        double tolerance=dbl(cfg.get("BillWeightAndStockWeightDifferenceTolerance"));

        if(details.isEmpty())throw new Refusal("Grid Record Not Found");
        if(deductionPolicyOn&&details.size()>1)throw new Refusal("Multiple rows is not allowed while the deduction policy is active.");
        // FormValidation :1477
        if(number(header.get("DocNo"))==0)throw new Refusal("DocNo Field is Required");
        if(number(header.get("SupplierCustomerId"))==0)throw new Refusal("Supplier Field is Required");
        if(number(header.get("GpNo"))==0)throw new Refusal("Gate pass Field is Required");
        double sup=dbl(header.get("PartyWeight")),fac=dbl(header.get("FactoryWeight"));
        if(sup==0)throw new Refusal("Supplier Weight Field is Required");
        String term=Objects.toString(header.get("DeliveryTerm"),"").trim();
        if(term.isEmpty()||"0".equals(term))throw new Refusal("DeliveryTerm Field is Required");
        if(fac==0)throw new Refusal("Factory Weight Field is Required");

        if(ref==105)validationForQty(details,breakups,form.previousData(gpId,id));
        if(!updating&&flag(cfg.get("ValidateGrnAndInvoiceDateWithGpDate"))&&gpDate!=null) {
            Object dd=header.get("DocDate");
            String docDate=dd==null?"":dd.toString();
            if(docDate.length()>=10&&!java.time.LocalDate.parse(docDate.substring(0,10)).equals(gpDate))throw new Refusal("GrnDate Should be Equal to GpDate");
        }
        double advF=dbl(header.get("AdvanceByFactoryFreight")),advP=dbl(header.get("AdvanceByPartyFreight"));
        if(freightId==0&&ref==105&&(advF>0||advP>0))throw new Refusal("In Case Of Market Purchase Advance by Factory or Advance by Party can't be greater than 0");
        double carriage=dbl(header.get("CarriageAmount")),bilty=dbl(header.get("BiltyFreight")),deduction=dbl(header.get("FreightDeduction"));
        int transporter=number(header.get("TransporterId"));
        if(carriage>0) {
            if(bilty<=0)throw new Refusal("Bilty Freight field required");
            if(transporter==0)throw new Refusal("Transporter Account field required");
        }
        if(deduction>0) {
            if(transporter==0)throw new Refusal("Transporter Account field required");
            if(bilty<=0)throw new Refusal("Bilty Freight field required");
            if(deduction>bilty)throw new Refusal("Deduction Can't be greater than Bilty Freight");
            if(carriage<=0&&freightId==0)throw new Refusal("Freight Amount field required");
        }
        boolean existsFOC=emptyBags.stream().anyMatch(r->number(r.get("TypeId"))==3);
        double gross=0,bill=0,stock=0,freightFromGrid=0;
        int rowNo=0;
        for(var d:details) {
            rowNo++;
            if(number(d.get("WarehouseId"))==0)throw new Refusal("Warehouse Required in Detail Grid And row No: "+rowNo);
            if(number(d.get("ItemId"))==0)throw new Refusal("Item Required in Detail Grid And row No: "+rowNo);
            if(number(d.get("CropYearId"))==0)throw new Refusal("CropYear Required in Detail Grid And row No: "+rowNo);
            if(number(d.get("JobLotId"))==0)throw new Refusal("JobLot Required in Detail Grid And row No: "+rowNo);
            if(number(d.get("PackingTypeId"))==0)throw new Refusal("PackingType Required in Detail Grid And row No: "+rowNo);
            if(number(d.get("ItemUomId"))==0)throw new Refusal("Uom Required in Detail Grid And row No: "+rowNo);
            if(dbl(d.get("ItemQty"))==0)throw new Refusal("Qty Required in Detail Grid And row No: "+rowNo);
            if(dbl(d.get("GrossWeight"))==0)throw new Refusal("GrossWeight Required in Detail Grid And row No: "+rowNo);
            if(dbl(d.get("NetBillWeight"))==0)throw new Refusal("NetBillWeight Required in Detail Grid And row No: "+rowNo);
            if(dbl(d.get("StockWeight"))==0)throw new Refusal("StockWeight Required in Detail Grid And row No: "+rowNo);
            if(dbl(d.get("NetBillWeight"))>dbl(d.get("GrossWeight")))throw new Refusal("NetBillWeight cannot be greater than Gross Weight. Please check.");
            if(ref!=105&&existsFOC&&dbl(d.get("EBWTotal"))==0)
                throw new Refusal("EmptyBags Weight cannot be greater than 0 when Empty Bags Case is [Purchase Against Weight] in Detail Grid And row No: "+rowNo);
            gross+=dbl(d.get("GrossWeight"));bill+=dbl(d.get("NetBillWeight"));stock+=dbl(d.get("StockWeight"));freightFromGrid+=dbl(d.get("FreightAmount"));
        }
        if(ref==105) { header.put("OtherCharges",carriage); header.put("CarriageAmount",freightFromGrid); }
        weightBusinessValidations(gross,ref,freightId,term,sup,fac,tolerance,exclude,deductionPolicyOn,
                deduction+chargeToParty,sup-receivedWeight,Objects.toString(header.get("DocDate"),""));
        if(ref==105&&breakups.isEmpty())throw new Refusal("Purchase BreakUp Required When Doing Market Purchase Entry");
        for(var b:breakups)if(ref==105&&dbl(b.get("GrossWeight"))==0)throw new Refusal("GrossWeight not found in Empty Bags Weight Breakup grid.");
    }

    /** ValidationforQty :3251 — detail qty per pack size (plus other GRNs of the gate pass) against the breakup. */
    void validationForQty(List<Map<String,Object>> details,List<Map<String,Object>> breakups,List<Map<String,Object>> previous) {
        Map<Double,Double> breakupQty=new LinkedHashMap<>();
        for(var b:breakups)breakupQty.merge(dbl(b.get("NetPackSize")),dbl(b.get("Qty")),Double::sum);
        Map<Integer,Double> equivalents=new HashMap<>();
        for(var u:jdbc.queryForList("EXEC dbo.usp_getAllUomsByCompanyId @OrganizationId=?,@CompanyId=?,@Active=1",context.currentOrganizationId(),context.currentCompanyId()))
            equivalents.put(number(u.get("Id")),dbl(u.get("Equivalent")));
        Map<Double,Double> detailQty=new LinkedHashMap<>();
        for(var d:details) {
            double eq=equivalents.getOrDefault(number(d.get("ItemUomId")),dbl(d.get("UOMEquivalent")));
            detailQty.merge(eq,dbl(d.get("ItemQty")),Double::sum);
        }
        for(var e:detailQty.entrySet()) {
            double pack=e.getKey();
            if(!breakupQty.containsKey(pack))throw new Refusal("Uom "+f(pack)+" In Detail Grid ,does not exists in BreakUp Grid");
            double prev=0;for(var p:previous)if(dbl(p.get("PackUomEquivalent"))==pack)prev+=dbl(p.get("ItemQty"));
            double inBreakup=breakupQty.get(pack),total=e.getValue()+prev;
            if(total>inBreakup) {
                if(prev>0)throw new Refusal("TotalQty "+f(total)+" (Sum Of Current Detail Grid Qty "+f(e.getValue())+" and\nOther Grn (Of Same GatePass) Qty "+f(prev)+") Against PackSize "+f(pack)+",\nCan't be Greater than BreakUp Grid TotaQty "+f(inBreakup));
                throw new Refusal("TotalQty "+f(total)+" in Detail Grid Against PackSize "+f(pack)+",\nCan't be Greater than BreakUp Grid TotaQty "+f(inBreakup));
            }
        }
    }

    /** WeightBusinessValidations :3137 with IsCalculating=false, and ValidateWeights :3361. */
    void weightBusinessValidations(double gross,int ref,int freightId,String term,double sup,double fac,double tolerance,boolean exclude,
                                   boolean deductionPolicyOn,double freightDeductionAmount,double balWeight,String docDate) {
        if(deductionPolicyOn&&ref==41&&"Ponch".equals(term)) {
            int policy=0;String policyName="";
            double diff=fac-sup;
            if(diff>0) {
                var p=form.deductionPolicy(docDate,diff);
                policy=number(p.get("PolicyTypeId"));
                if(p.containsKey("ConditionDescription")&&number(p.get("PolicyTypeId"))!=0)policyName=" Policy is "+Objects.toString(p.get("ConditionDescription"),"");
            }
            double finalWt=policy==2?fac-diff/2.0:policy==3?sup:fac;
            if(gross>0&&differs(gross,finalWt))throw new Refusal("GrossWeight "+f(gross)+" Should Equal To FinalWeight "+f(finalWt)+"..."+policyName);
            return;
        }
        if(ref==105) {
            if(gross>balWeight+0.0001)throw new Refusal("GrossWeight "+f(gross)+" cannot be greater than Balance Weight "+f(balWeight));
            return;
        }
        double diff=sup-(fac+tolerance),diffRaw=sup-fac;
        boolean party="Load".equals(term)||"Load & PartyWeight".equals(term)||"Ponch & PartyWeight".equals(term);
        boolean factory="Load & FactoryWeight".equals(term)||"Ponch & FactoryWeight".equals(term);
        if(party) {
            if(diff>0&&!(freightDeductionAmount>0||exclude)) {
                if(differs(gross,fac))throw new Refusal("Shortage "+f(diffRaw)+" is Greater than Tollerance Weight "+f(tolerance)+",So Freight Deduction is Required Or You Have To Make Gross Weight Equal To Facotry Weight\nAnd DeliveryTerm is "+term);
            } else if(differs(gross,sup)) {
                String m="GrossWeight "+f(gross)+" must be equal to Supplier Weight "+f(sup)+".\n";
                if(freightDeductionAmount>0)m+="Freight deduction is applied.\n";
                throw new Refusal(m+"Delivery term is "+term+".");
            }
        } else if("Ponch".equals(term)) {
            if(sup>fac) { if(differs(gross,fac))throw new Refusal("GrossWeight "+f(gross)+" Must be Equal To Factory Weight "+f(fac)+" Because Factory Weight is Lesser\nAnd DeliveryTerm is "+term); }
            else if(differs(gross,sup))throw new Refusal("GrossWeight "+f(gross)+" Must be Equal To Supplier Weight "+f(sup)+" Because Supplier Weight is Lesser\nAnd DeliveryTerm is "+term);
        } else if(factory) {
            if(differs(gross,fac))throw new Refusal("GrossWeight "+f(gross)+" Must be Equal To Factory Weight "+f(fac)+" Because\nDeliveryTerm is "+term);
        }
    }
}
