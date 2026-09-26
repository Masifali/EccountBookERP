package com.mst.services;

import com.mst.repositories.*;
import com.mst.security.CurrentUserContext;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** InvfrmPurchaseInvoice.LoadInGridDetail and its six companion loaders, lines 5890-6306. */
@Service
public class PurchaseInvoiceGrnService {
    private final GrnLoaderService selection;private final PurchaseInvoiceGrnRepository repository;private final PurchaseInvoiceLookupRepository lookups;
    private final JdbcTemplate jdbc;private final CurrentUserContext context;
    public PurchaseInvoiceGrnService(GrnLoaderService selection,PurchaseInvoiceGrnRepository repository,PurchaseInvoiceLookupRepository lookups,JdbcTemplate jdbc,CurrentUserContext context){this.selection=selection;this.repository=repository;this.lookups=lookups;this.jdbc=jdbc;this.context=context;}
    private static Map<String,Object> row(Object... pairs){var r=copy(null);for(int i=0;i<pairs.length;i+=2)r.put(pairs[i].toString(),pairs[i+1]);return r;}
    private String remark(Map<String,Object> r,String party){String text=i(r,"GpNo")>0?"GpNo: "+i(r,"GpNo"):"";for(String key:List.of("VehicleNo","BiltyNo"))if(!s(r,key).isBlank())text+="   "+key+": "+s(r,key);if(!party.isBlank())text+="   PartyName: "+party;return text;}
    @SuppressWarnings("unchecked")
    public Map<String,Object> preview(List<Map<String,Object>> requested){
        var validation=selection.validateAndSelectGrns(requested,46,lookups.enabled("AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt"));
        if(!Boolean.TRUE.equals(validation.get("success")))throw new IllegalArgumentException(Objects.toString(validation.get("message")));
        var selected=(List<Map<String,Object>>)validation.get("grns");var ids=selected.stream().map(r->i(copy(r),"Id")).toList();var source=repository.read(ids);
        var first=copy(source.get("details").get(0));int invoiceType=i(copy(selected.get(0)),"GrnType"),supplier=i(first,"SupplierCustomerId"),order=i(first,"PurchaseOrderId");boolean subsidiary=lookups.subsidiary();
        var party=copy(jdbc.queryForMap("SELECT GlAccountId,CompanyName FROM dbo.SupplierCustomer WHERE Id=? AND OrganizationId=? AND CompanyId=?",supplier,context.currentOrganizationId(),context.currentCompanyId()));int gl=i(party,"GlAccountId");
        var h=row("Id",0,"InvoiceTypeId",invoiceType,"SupplierCustomerId",supplier,"CommissionAgentId",i(first,"BrokerAgentSupCustId")>0?i(first,"BrokerAgentSupCustId"):supplier,"DueDays",i(first,"OrderDueDays"),"DocDate",lookups.enabled("ValidateGrnAndInvoiceDateWithGpDate")?s(first,"DocDate").substring(0,10):LocalDate.now().toString());
        for(String key:List.of("CommissionType","CommRate","UomScheduleIdCmRate","CommAmount","CommissionRemarks","RemarksHeader","BrokerAgentId","BrokeryType","BrokeryUom","BrokeryRate","BrokeryAmount","PaymentTermsId","DeliveryTerm"))h.put(key,first.get(key));
        h.put("OtherRemarks",h.get("RemarksHeader"));h.put("DueDate",LocalDate.parse(s(h,"DocDate")).plusDays(i(h,"DueDays")).toString());h.put("WagesAmount",source.get("wages").isEmpty()?0:n(copy(source.get("wages").get(0)),"WagesAmount"));
        var details=new ArrayList<Map<String,Object>>();for(var raw:source.get("details")){
            var r=copy(raw);r.put("Id",0);r.put("GrnNo",r.get("DocNo"));r.put("RateCut",r.get("DeductionRate"));r.put("EquivalentPoRate",invoiceType==2?40:n(r,"EquivalentPoRate"));
            for(String key:List.of("RateCutAmount","FreightAmount","ExpenseAmount","CommissionAmount","JournalAmount","WagesAmount","FreightDeduction","EbPurAgainstWeightAmount"))r.put(key,0);
            r.put("ItemAmount",PurchaseDirectInvoiceCalculations.round(n(r,"ItemAmount"),lookups.amountDigits(),RoundingMode.HALF_UP));
            if(n(r,"RateCut")!=0){if(n(r,"EquivalentPoRate")<=0)throw new IllegalArgumentException("The GRN rate UOM has no equivalent");double cut=n(r,"NetBillWeight")/n(r,"EquivalentPoRate")*n(r,"RateCut");r.put("RateCutAmount",cut);r.put("ItemAmount",n(r,"NetBillWeight")/n(r,"EquivalentPoRate")*n(r,"ItemRate")-cut);}
            details.add(r);
        }
        var freight=new ArrayList<Map<String,Object>>();var journal=new ArrayList<Map<String,Object>>();var expenses=new ArrayList<Map<String,Object>>();var bags=new ArrayList<Map<String,Object>>();
        boolean load=Set.of("Load","Load & PartyWeight","Load & FactoryWeight").contains(s(h,"DeliveryTerm"));double deduction=0;
        for(var raw:source.get("freight")){
            var f=copy(raw);int transporter=subsidiary?i(f,"TransporterSupCustId"):i(f,"Transporter"),transporterGl=i(f,"Transporter");double amount=n(f,"CarriageAmount")+n(f,"TotalAdvanceByFactoryAmount");String text=remark(f,s(party,"CompanyName"));
            if(n(f,"CarriageAmount")>0){
                if(load)freight.add(row("PurchaseOrderId",order,"InvGrnId",i(f,"MainId"),"FreightId",i(f,"FreightId"),"TansporterId",transporterGl,"SupplierCustomerId",transporter,"FreightAmount",amount,"Remarks",text));
                else if(i(f,"FreightId")==0||transporter!=gl)journal.add(row("InvGrnId",i(f,"MainId"),"FreightId",i(f,"FreightId"),"ChartofAccountId",transporterGl,"SupplierCustomerId",transporter,"JvCredit",amount,"JvRemarks",text));
            }
            if(load&&n(f,"TotalAdanceByPartyAmount")>0)freight.add(row("PurchaseOrderId",order,"InvGrnId",i(f,"MainId"),"FreightId",i(f,"FreightId"),"TansporterId",gl,"SupplierCustomerId",subsidiary?supplier:gl,"FreightAmount",n(f,"TotalAdanceByPartyAmount"),"Remarks",text+" Advance Paid By Party"));
            deduction+=n(f,"TotalChargeToPartyAmount");
        }
        h.put("FreightAmount",deduction);h.put("TransportAccountId",deduction>0?gl:0);h.put("TransporterCreditPartyId",deduction>0&&subsidiary?supplier:0);
        for(var raw:source.get("rateCuts")){var r=copy(raw);if(i(r,"RateCutAmount")>0)journal.add(row("InvGrnId",i(r,"MainId"),"ChartofAccountId",i(r,"GlAccountId"),"SupplierCustomerId",i(r,"GlAccountId"),"JvQty",n(r,"Qty"),"JvRate",n(r,"RateCut"),"JvCredit",n(r,"RateCutAmount"),"JvRemarks",remark(r,""),"RowType",1));}
        double total=details.stream().mapToDouble(r->n(r,"ItemAmount")).sum(),qty=details.stream().mapToDouble(r->n(r,"ItemQty")).sum();
        for(var raw:source.get("orderExpenses")){var r=copy(raw);if(i(r,"AccountId")<=0)continue;boolean percentage=n(r,"Percentage")>0&&total>0;double amount=PurchaseDirectInvoiceCalculations.round(percentage?total*n(r,"Percentage")/100:qty*n(r,"Rate"),0,RoundingMode.HALF_EVEN);freight.add(row("PurchaseOrderId",i(r,"PurchaseOrderId"),"TansporterId",i(r,"AccountId"),"SupplierCustomerId",i(r,"AccountId"),"Percentage",n(r,"Percentage"),"FrQty",percentage?n(r,"Qty"):qty,"FrRate",n(r,"Rate"),"FreightAmount",amount,"Remarks",r.get("Remarks")));}
        for(var raw:source.get("expenses")){var r=copy(raw);if(i(r,"Rate")>0)expenses.add(row("GrnId",i(r,"GrnId"),"GrnNo",i(r,"GrnNo"),"PurchaseOrderId",i(r,"PoId"),"SupplierDispatchId",i(r,"SupplierDispatchId"),"SupplierDispatchExpenseId",i(r,"SupplierDispatchExpenseId"),"InvRevExpItemId",i(r,"OtherItemId"),"Qty",n(r,"Qty"),"Rate",n(r,"Rate"),"Amount",n(r,"Qty")*n(r,"Rate"),"CustomRemarks",s(r,"Remarks"),"Remarks",s(r,"Remarks")));}
        for(var raw:source.get("emptyBags")){var r=copy(raw);if(n(r,"PurchaseQty")<=0)continue;r.put("Id",0);r.put("Amount",n(r,"PurchaseQty")*n(r,"Rate"));r.put("CreditAccountId",0);bags.add(r);}
        PurchaseInvoiceCalculations.initialBagAmounts(details,bags);
        PurchaseInvoiceCalculations.bill(h,details,expenses,freight,journal,bags,gl,new PurchaseInvoiceCalculations.Configuration(lookups.amountDigits(),lookups.enabled("DebitAmountChargetoExpenseAcFreightGridPurchase"),lookups.enabled("WagesAmountCalculateOnQty"),lookups.enabled("ContractWagesChargetoProduct"),subsidiary));
        var terms=source.get("paymentTerms").stream().map(PurchaseInvoiceFinancialRules::copy).toList();for(var p:terms){p.put("Id",0);p.put("PaymentTermId",p.get("PaymentTermsId"));}
        h.put("rateUoms",lookups.rateUoms(details));h.put("details",details);h.put("freight",freight);h.put("journal",journal);h.put("expenses",expenses);h.put("emptyBags",bags);h.put("paymentTerms",PurchaseInvoicePaymentRules.calculate(h,details,terms,true,false));return h;
    }
}
