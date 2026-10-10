package com.mst.services.sale.pcc;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Lookups and helpers shared by the Sale Pcc forms 548 SaleOrderConcrete, 551 GoodsDispatchNotesConcrete and 552 GdnDirectConcrete
 * (Architecture.WinApp.pcc.Sale.*). Every method is one desktop call (the CommonServices / BLL method is named in the comment); organization,
 * company, user and branch always come from CurrentUserContext (through SaleEngrSupport), never from the browser.
 */
@Component
public class SalePccP2Support {
    private final SaleEngrSupport sup;
    private final SalePccLookups lk;

    public SalePccP2Support(SaleEngrSupport sup, SalePccLookups lk) { this.sup = sup; this.lk = lk; }

    public SaleEngrSupport sup() { return sup; }
    public SalePccLookups lk() { return lk; }

    /** CommonServices.GetDecimalConfiguration: places used by stringFormatsingle / DecimalRateFormate / stringFormatsingleForFcy. */
    public Map<String, Object> fmt() {
        int amt = toInt(sup.config("Default NoofDecimal Points For Amount"));
        int rate = toInt(sup.config("Default NoofDecimal Points For Rate"));
        int fcy = toInt(sup.config("DefaultNoOfDecimalPointsForFcyAmount"));
        return row("amountRound", amt, "amount", amt >= 1 && amt <= 4 ? amt : 0, "rate", rate >= 1 && rate <= 4 ? rate : (rate == 0 ? 2 : 0),
                "fcy", fcy >= 1 && fcy <= 4 ? fcy : 0, "rateRound", rate);
    }

    /** CommonServices.GetVendorsAndCustomers() (no PartyTypeId) when ERP feature 4, else SupplierCustomerGetforComboServiceBind. */
    public List<Map<String, Object>> commissionAgents() {
        if (sup.erpFeature(4)) return sup.rows("USP_GetVendorsAndCustomers", "OrganizationId", sup.org(), "CompanyId", sup.company());
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId (type 2 = Visited By, type 3 = RefSalesMan of an agent). */
    public List<Map<String, Object>> referenceParties(int typeId, int supplierCustomerId) {
        return sup.rows("Sp_ReferenceParties_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "ReferencePartyTypeId", typeId == 0 ? null : typeId, "SupplierCustomerId", supplierCustomerId == 0 ? null : supplierCustomerId,
                "Activity", "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId");
    }

    /** CommonServices.GetDueTermServiceBind = InvDueTerms.GetAll. */
    public List<Map<String, Object>> dueTerms() {
        return sup.rows("Sp_InvDueTerms_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll");
    }

    /** MultiCurrency.GetAll. */
    public List<Map<String, Object>> currencies() {
        return sup.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll");
    }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds(ids) = COAAllocation.GetAccountTitleByAccountTypeIds. */
    public List<Map<String, Object>> accountTitles(String accountTypeIds) {
        return sup.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "AppId", sup.ctx().currentAppId(),
                "AccountTypeIds", accountTypeIds, "UserId", sup.userId(), "Activity", "GetAccountTitleByAccountTypeIds");
    }

    /** SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages. */
    public List<Map<String, Object>> contractors() {
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationCompanyIdForContractorWages");
    }

    /** pcc ItemPricingSchedule.GetItemWithRatesForItemPricingSchedule(DocumentTypeId 6, EffectedDate = the document date) -> dtitem rows. */
    public List<Map<String, Object>> pricedItems(String docDate) {
        LocalDate d = SalePccLookups.parseDate(docDate);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("DocumentTypeId", 6);
        if (d != null) p.put("EffectiveDate", d);
        p.put("Activity", "GetItemWithRatesForItemPricingSchedule");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(sup.jdbc(), "[pcc].[USP_ItemPricingSchedule_GetAllMethod]", p))
            out.add(row("Id", r.get("ItemId"), "ItemName", r.get("ItemName"), "ItemCode", r.get("ItemCodeNew"), "ItemWeight", r.get("WeightKgs"),
                    "ItemRate", r.get("PreviousRate"), "ScheduleId", r.get("ScheduleId")));
        return out;
    }

    /** ContractorWagesRateSchedule.GetServiceActivityByItemId(2, itemId, date, 0, 0). */
    public List<Map<String, Object>> serviceActivities(int itemId, String date) {
        LocalDate d = SalePccLookups.parseDate(date);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("ItemId", itemId); p.put("RefDocumentTypeId", 2); p.put("EffectedDATE", d == null ? LocalDate.now() : d);
        p.put("Activity", "GetServiceActivityByItemId");
        return DesktopProc.rows(sup.jdbc(), "[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", p);
    }

    /** CommonServices.GetSupplierCustomerInfoByGLAccountId: first column of the first row (0 when none). */
    public int supplierGlId(int glAccountId) {
        List<Map<String, Object>> r = sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "GlAccountId", glAccountId, "Id", 0, "Activity", "GetSupplierCustomerInfoByGLAccountId");
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        return toInt(r.get(0).values().iterator().next());
    }

    /** VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher. */
    public List<Map<String, Object>> lastRate(int currencyId, String documentTypeIds) {
        return sup.rows("Sp_Vouchers_GetMethods", "OrganizationId", sup.org(), "CompanyId", sup.company(), "DocumentTypeIds", documentTypeIds,
                "DMultiCurrencyIds", String.valueOf(currencyId), "Activity", "GetMultiCurrencyAndLastRate");
    }

    /** Inventory SaleOrder.CHECKSUPPLIERCUSTOMERLIMITS (ActionId 2): AvailableLimit of the first row. */
    public double availableLimit(int customerId, double amount) {
        List<Map<String, Object>> r = sup.rows("SP_CHECKSUPPLIERCUSTOMERLIMITS", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "SupplierCustomerId", customerId, "Amount", BigDecimal.valueOf(amount), "ActionId", 2);
        return r.isEmpty() ? 0.0 : toDouble(r.get(0).get("AvailableLimit"));
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID / the desktop's configuration lookups used by the Load of the forms. */
    public Map<String, Object> searchConfig() {
        return row("itemByCode", sup.configBool("ItemSearchByCode"), "partyByCode", sup.configBool("SupplierCustomerDefaultFilterByPartyCode"));
    }

    /** ConfigurationDefault: City Area / Job/Lot / Base Currency / BaseCurrencyRate. */
    public Map<String, Object> defaults() {
        return row("cityId", sup.configInt("City Area"), "jobLotId", sup.configInt("Job/Lot"), "baseCurrency", sup.configInt("Base Currency"),
                "baseRate", toDouble(sup.config("BaseCurrencyRate")));
    }

    public static BigDecimal dec(double v) { return BigDecimal.valueOf(v); }

    public UserAccount user() { return sup.user(); }
}
