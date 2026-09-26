package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.dao.DataAccessException;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** Original type-56 loader queries from BLL 0581; every requested GRN is scoped first. */
@Repository
public class PurchaseInvoiceGrnRepository {
    private final JdbcTemplate jdbc;private final CurrentUserContext context;private final GrnLoaderRepository grns;
    public PurchaseInvoiceGrnRepository(JdbcTemplate jdbc,CurrentUserContext context,GrnLoaderRepository grns){this.jdbc=jdbc;this.context=context;this.grns=grns;}
    public Map<String,List<Map<String,Object>>> read(List<Integer> ids){
        if(ids.isEmpty())throw new IllegalArgumentException("Select a GRN");
        for(int id:ids)if(i(copy(grns.getGrnHeader(id)),"DocumentTypeId")!=46)throw new IllegalArgumentException("Select a regular purchase GRN");
        String csv=ids.stream().map(String::valueOf).collect(Collectors.joining(","));int org=context.currentOrganizationId(),company=context.currentCompanyId();
        var result=new LinkedHashMap<String,List<Map<String,Object>>>();
        try{result.put("details",jdbc.queryForList("EXEC dbo.usp_GrnLoadForPurchaseInvoice @GrnIds=?",csv));}
        catch(DataAccessException failure){Throwable cause=failure.getMostSpecificCause();if(cause instanceof java.sql.SQLException sql&&sql.getErrorCode()==50001)throw new IllegalArgumentException(sql.getMessage());throw failure;}
        if(result.get("details").isEmpty())throw new IllegalArgumentException("No GRN detail is available to invoice");
        result.put("freight",jdbc.queryForList("EXEC dbo.USP_InvGrn_GetTransporterAndFreight @GdnIds=?",csv));
        result.put("rateCuts",jdbc.queryForList("EXEC dbo.USP_GetDataRateCutInCaseOfAccessWeightReceived @OrganizationId=?,@CompanyId=?,@GdnIds=?",org,company,csv));
        result.put("expenses",jdbc.queryForList("EXEC dbo.USP_GetExpenseDetailFromPOAndPreBill @GdnIds=?",csv));
        result.put("emptyBags",jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@OrderId=0,@Activity='GetEmptyBagsFromGrn'",csv));
        result.put("wages",jdbc.queryForList("EXEC dbo.USP_GetWagesAmountByRefDocumentTypeIdandRefIds @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=46,@RefIds=?",org,company,csv));
        int order=i(copy(result.get("details").get(0)),"PurchaseOrderId");
        result.put("orderExpenses",order>0?jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='GetPurchaseOrderExpensesChargToProdut'",order):List.of());
        String orders=result.get("details").stream().map(r->i(copy(r),"PurchaseOrderId")).filter(id->id>0).distinct().map(String::valueOf).collect(Collectors.joining(","));
        result.put("paymentTerms",orders.isBlank()?List.of():jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrderIds=?,@Activity='PurchaseOrderPaymentTermDetailByPoIds'",orders));
        return result;
    }
}
