package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.PurchaseOrderCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.util.*;

@Repository
public class PurchaseOrderCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

    public Map<String, Object> saveOrUpdate(PurchaseOrderCmagtDto dto) {
        Map<String, Object> result = new HashMap<>();
        try {
            SimpleJdbcCall masterCall = new com.mst.repositories.support.LenientJdbcCall(jdbcTemplate)
                    .withSchemaName("cmagt")
                    .withProcedureName("USP_purchaseOrderMaster_InsertAndUpdate");

            MapSqlParameterSource masterParams = new MapSqlParameterSource();
            masterParams.addValue("purchaseOrderMasterId", dto.getPurchaseOrderMasterId() != null ? dto.getPurchaseOrderMasterId() : 0);
            masterParams.addValue("docNo", dto.getDocNo() != null ? dto.getDocNo() : 0);
            masterParams.addValue("docDate", parseDate(dto.getDocDate()));
            masterParams.addValue("deliveryStartDate", parseDate(dto.getDeliveryStartDate()));
            masterParams.addValue("ValidityDate", parseDate(dto.getValidityDate()));
            masterParams.addValue("deliveryDays", dto.getDeliveryDays() != null ? dto.getDeliveryDays() : 0);
            masterParams.addValue("supplierId", dto.getSupplierId() != null ? dto.getSupplierId() : 0);
            masterParams.addValue("commissionAgentId", dto.getCommissionAgentId() != null ? dto.getCommissionAgentId() : 0);
            masterParams.addValue("deliveryTermId", dto.getDeliveryTermId() != null ? dto.getDeliveryTermId() : 0);
            masterParams.addValue("isSupplierOtherChargesAllowed", dto.getIsSupplierOtherChargesAllowed() != null ? dto.getIsSupplierOtherChargesAllowed() : false);
            masterParams.addValue("isWhtApplied", dto.getIsWhtApplied() != null ? dto.getIsWhtApplied() : false);
            masterParams.addValue("EBWeightDeductionTermId", dto.getEbWeightDeductionTermId() != null ? dto.getEbWeightDeductionTermId() : 0);
            masterParams.addValue("remarksHeader", dto.getRemarksHeader() != null ? dto.getRemarksHeader() : "");
            masterParams.addValue("shipToAddressId", dto.getShipToAddressId() != null ? dto.getShipToAddressId() : 0);
            masterParams.addValue("DeliveryToPartyId", dto.getDeliveryToPartyId() != null ? dto.getDeliveryToPartyId() : 0);
            masterParams.addValue("ShipToAddress", dto.getShipToAddress() != null ? dto.getShipToAddress() : "");
            masterParams.addValue("paymentScheduleDescription", dto.getPaymentScheduleDescription() != null ? dto.getPaymentScheduleDescription() : "");

            /* Set by the service from the session before this runs. The ': 1'
               fallbacks that used to sit here could file a document against
               company 1, or under user 1, whenever the payload omitted them. */
            masterParams.addValue("organizationId", dto.getOrganizationId());
            masterParams.addValue("companyId", dto.getCompanyId());
            masterParams.addValue("branchId", dto.getBranchId());
            masterParams.addValue("financialYearId", dto.getFinancialYearId());
            masterParams.addValue("entryUserId", dto.getEntryUserId());
            masterParams.addValue("modifyUserId", dto.getModifyUserId());
            masterParams.addValue("documentTypeId", 1052);

            Map<String, Object> masterOut = masterCall.execute(masterParams);
            Integer masterId = extractReturnedId(masterOut);
            if (masterId == null || masterId <= 0) {
                masterId = dto.getPurchaseOrderMasterId();
            }

            // Details
            if (dto.getPurchaseOrderDetailList() != null) {
                for (PurchaseOrderCmagtDto.ItemDetailDto det : dto.getPurchaseOrderDetailList()) {
                    SimpleJdbcCall detCall = new com.mst.repositories.support.LenientJdbcCall(jdbcTemplate)
                            .withSchemaName("cmagt")
                            .withProcedureName("USP_purchaseOrderDetail_Insert");

                    MapSqlParameterSource detParams = new MapSqlParameterSource();
                    detParams.addValue("purchaseOrderDetailId", det.getPurchaseOrderDetailId() != null ? det.getPurchaseOrderDetailId() : 0);
                    detParams.addValue("purchaseOrderMasterId", masterId);
                    detParams.addValue("itemId", det.getItemId() != null ? det.getItemId() : 0);
                    detParams.addValue("cropYearId", det.getCropYearId() != null ? det.getCropYearId() : 0);
                    detParams.addValue("packTypeId", det.getPackTypeId() != null ? det.getPackTypeId() : 0);
                    detParams.addValue("weight", det.getWeight() != null ? det.getWeight() : BigDecimal.ZERO);
                    detParams.addValue("qty", det.getQty() != null ? det.getQty() : BigDecimal.ZERO);
                    detParams.addValue("packUomId", det.getPackUomId() != null ? det.getPackUomId() : 0);
                    detParams.addValue("rate", det.getRate() != null ? det.getRate() : BigDecimal.ZERO);
                    detParams.addValue("rateUomId", det.getRateUomId() != null ? det.getRateUomId() : 0);
                    detParams.addValue("amount", det.getAmount() != null ? det.getAmount() : BigDecimal.ZERO);
                    detParams.addValue("taxId", det.getTaxId() != null ? det.getTaxId() : 0);
                    detParams.addValue("taxPercent", det.getTaxPercent() != null ? det.getTaxPercent() : BigDecimal.ZERO);
                    detParams.addValue("taxAmount", det.getTaxAmount() != null ? det.getTaxAmount() : BigDecimal.ZERO);
                    detParams.addValue("totalAmount", det.getTotalAmount() != null ? det.getTotalAmount() : BigDecimal.ZERO);
                    detParams.addValue("remarksDetail", det.getRemarksDetail() != null ? det.getRemarksDetail() : "");

                    detParams.addValue("commissionTypeId", det.getCommissionTypeId() != null ? det.getCommissionTypeId() : 0);
                    detParams.addValue("commissionRate", det.getCommissionRate() != null ? det.getCommissionRate() : BigDecimal.ZERO);
                    detParams.addValue("commissionRateUomId", det.getCommissionRateUomId() != null ? det.getCommissionRateUomId() : 0);
                    detParams.addValue("commissionAmount", det.getCommissionAmount() != null ? det.getCommissionAmount() : BigDecimal.ZERO);

                    detParams.addValue("brokeryTypeId", det.getBrokeryTypeId() != null ? det.getBrokeryTypeId() : 0);
                    detParams.addValue("brokeryRate", det.getBrokeryRate() != null ? det.getBrokeryRate() : BigDecimal.ZERO);
                    detParams.addValue("brokeryRateUomId", det.getBrokeryRateUomId() != null ? det.getBrokeryRateUomId() : 0);
                    detParams.addValue("brokeryAmount", det.getBrokeryAmount() != null ? det.getBrokeryAmount() : BigDecimal.ZERO);

                    detCall.execute(detParams);
                }
            }

            result.put("status", "SUCCESS");
            result.put("message", "Purchase Order saved successfully!");
            result.put("id", masterId);
        } catch (Exception e) {
            result.put("status", "ERROR");
            result.put("message", e.getMessage());
        }
        return result;
    }

    public List<Map<String, Object>> getHistory(Integer companyId, Integer organizationId, String fromDate, String toDate) {
        SimpleJdbcCall call = new com.mst.repositories.support.LenientJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_purchaseOrderMaster_GetAllMethod");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("Activity", "ReadBySearch_purchaseOrderMaster");
        /* The controller passes the session's own values. The ': 1' fallbacks that
           used to sit here would have quietly widened a read to company 1 if one
           ever arrived null, hiding the fault instead of surfacing it. */
        params.addValue("CompanyId", companyId);
        params.addValue("OrganizationId", organizationId);
        params.addValue("DocumentTypeId", 1052);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public Map<String, Object> getById(Integer id) {
        Map<String, Object> result = new HashMap<>();

        SimpleJdbcCall headerCall = new com.mst.repositories.support.LenientJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_purchaseOrderMaster_GetAllMethod");

        MapSqlParameterSource headerParams = new MapSqlParameterSource();
        headerParams.addValue("Activity", "ReadById_purchaseOrderMaster");
        headerParams.addValue("Id", id);

        Map<String, Object> headerOut = headerCall.execute(headerParams);
        List<Map<String, Object>> headers = extractList(headerOut);

        if (headers != null && !headers.isEmpty()) {
            Map<String, Object> header = new HashMap<>(headers.get(0));

            // Details
            SimpleJdbcCall detCall = new com.mst.repositories.support.LenientJdbcCall(jdbcTemplate)
                    .withSchemaName("cmagt")
                    .withProcedureName("USP_purchaseOrderMaster_GetAllMethod");
            MapSqlParameterSource detParams = new MapSqlParameterSource();
            detParams.addValue("Activity", "ReadByHeaderId_purchaseOrderDetail");
            detParams.addValue("Id", id);
            header.put("purchaseOrderDetailList", extractList(detCall.execute(detParams)));

            result.put("status", "SUCCESS");
            result.put("data", header);
        } else {
            result.put("status", "ERROR");
            result.put("message", "Record not found");
        }
        return result;
    }

    @SuppressWarnings("unchecked")
    private List<Map<String, Object>> extractList(Map<String, Object> out) {
        for (Object val : out.values()) {
            if (val instanceof List) {
                return (List<Map<String, Object>>) val;
            }
        }
        return Collections.emptyList();
    }

    private Integer extractReturnedId(Map<String, Object> out) {
        for (Object val : out.values()) {
            if (val instanceof Integer) return (Integer) val;
            if (val instanceof Number) return ((Number) val).intValue();
        }
        return null;
    }

    private Date parseDate(String dateStr) {
        if (dateStr == null || dateStr.trim().isEmpty()) return new Date();
        try {
            return DATE_FORMAT.parse(dateStr);
        } catch (Exception e) {
            return new Date();
        }
    }

    /* ===================================================================== Load So picker
       frmLoadSaleIrderForPO (Architecture.WinApp.Cmagt). Both calls are the BLL 0489
       saleOrderMaster methods exactly as the loader makes them. */

    /** Runs "EXEC proc @A=?, @B=?" with only the parameters present - the BLL adds a parameter
     *  only when it has a value, and every omitted one has a NULL default in the procedure. */
    private List<Map<String, Object>> execProc(String proc, LinkedHashMap<String, Object> p) {
        StringBuilder sql = new StringBuilder("SET NOCOUNT ON; EXEC ").append(proc);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return jdbcTemplate.queryForList(sql.toString(), args.toArray());
    }

    /** ComboDbCall (:158-182) -> GetDataForDropDown: @OrganizationId, @CompanyId only (the
     *  loader sets DocumentTypeId/FinancialYearId/BranchesIds on the object but the BLL never
     *  sends them). Rows: Id, ReferenceName, ParentCategoryId, Activity. */
    public List<Map<String, Object>> loadSoCombos(int orgId, int companyId) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", orgId);
        p.put("CompanyId", companyId);
        return execProc("[cmagt].[USP_GetDataForDropDownFromsaleOrderMaster]", p);
    }

    /** PendingDataDbCall (:279-307) -> BLL 0489 PendingDataLoader (:675-786). Dates always
     *  (the pickers are never null); doc nos and ids only when non-zero; ship-to text only
     *  when non-empty. @DocumentTypeId is not sent by the BLL. */
    public List<Map<String, Object>> loadSoPending(int orgId, int companyId, int branchId, int fyId,
                                                   String fromDate, String toDate,
                                                   Integer fromDocNo, Integer toDocNo,
                                                   Integer commissionAgentId, Integer buyerId,
                                                   Integer itemId, Integer deliveryToPartyId,
                                                   String shipToAddress) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", orgId);
        p.put("CompanyId", companyId);
        p.put("BranchesId", branchId);
        p.put("FinancialYearId", fyId);
        Date f = parseDateOrNull(fromDate), t = parseDateOrNull(toDate);
        if (f != null) p.put("FromDate", new java.sql.Date(f.getTime()));
        if (t != null) p.put("ToDate", new java.sql.Date(t.getTime()));
        if (fromDocNo != null && fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != null && toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (commissionAgentId != null && commissionAgentId != 0) p.put("CommissionAgentId", commissionAgentId);
        if (buyerId != null && buyerId != 0) p.put("buyerId", buyerId);
        if (itemId != null && itemId != 0) p.put("ItemId", itemId);
        if (deliveryToPartyId != null && deliveryToPartyId != 0) p.put("DeliveryToPartyId", deliveryToPartyId);
        if (shipToAddress != null && !shipToAddress.trim().isEmpty()) p.put("ShipToAddress", shipToAddress);
        return execProc("[cmagt].[USP_saleOrderMaster_PendingDataLoader]", p);
    }

    /** HistoryComboDbCall (:770-788) -> BLL 0490 GetDataForDropDown with DocumentTypeIds='1052'. */
    public List<Map<String, Object>> historyCombos(int orgId, int companyId) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", orgId);
        p.put("CompanyId", companyId);
        p.put("DocumentTypeIds", "1052");
        return execProc("[cmagt].[USP_GetDataForDropDownFrompurchaseOrderMaster]", p);
    }

    /* ================================================== Ship-to "+" (SupfrmShipToAddress) */

    /** cmbcountryfill -> country.GetAll(new Country()): org/company are the model's CLR 0. */
    public List<Map<String, Object>> countries() {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", 0);
        p.put("CompanyId", 0);
        p.put("MethodType", "GetAll");
        return execProc("[dbo].[SP_Country_ReadMethod]", p);
    }

    /** cmbcountry_Leave -> City.GetAll(org, company) with @MethodType='GetAll'. */
    public List<Map<String, Object>> cities(int orgId, int companyId) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", orgId);
        p.put("CompanyId", companyId);
        p.put("MethodType", "GetAll");
        return execProc("[dbo].[SP_City_GetAllMethod]", p);
    }

    /** gridFill -> SupplierCustomerShipToAddress.FormHistory(org, company). */
    public List<Map<String, Object>> shipToHistory(int orgId, int companyId) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", orgId);
        p.put("CompanyId", companyId);
        p.put("Activity", "FormHistory");
        return execProc("[dbo].[Sp_SupplierCustomerShipToAddress_GetAllMethod]", p);
    }

    /** grdfrm_CellContentDoubleClick -> GetByID: @Id, @Activity='ReadById'. */
    public List<Map<String, Object>> shipToById(int id) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("Id", id);
        p.put("Activity", "ReadById");
        return execProc("[dbo].[Sp_SupplierCustomerShipToAddress_GetAllMethod]", p);
    }

    /** BLL 0602 Save: Id==0 -> Sp_..._Insert, else Sp_..._Update; SetProc sends every model
     *  property (19 parameters, the procedures' full list); EntryDate = ModifyDate = now. */
    public Object shipToSave(Map<String, Object> m) {
        int id = toInt(m.get("Id"));
        Timestamp now = new Timestamp(System.currentTimeMillis());
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("AddressLine1", m.get("AddressLine1"));
        p.put("AddressLine2", m.get("AddressLine2"));
        p.put("AddressLine3", m.get("AddressLine3"));
        p.put("AddressLine4", m.get("AddressLine4"));
        p.put("AddressTitle", m.get("AddressTitle"));
        p.put("ContactPerson", m.get("ContactPerson"));
        p.put("CityId", toInt(m.get("CityId")));
        p.put("CompanyId", toInt(m.get("CompanyId")));
        p.put("CountryId", toInt(m.get("CountryId")));
        p.put("EntryDate", now);
        p.put("EntryUser", toInt(m.get("EntryUser")));
        p.put("Id", id);
        p.put("MobileNo", m.get("MobileNo"));
        p.put("ModifyDate", now);
        p.put("ModifyUser", toInt(m.get("ModifyUser")));
        p.put("OrganizationId", toInt(m.get("OrganizationId")));
        p.put("PhoneNo", m.get("PhoneNo"));
        p.put("SupplierCustomerId", toInt(m.get("SupplierCustomerId")));
        p.put("WhatsAppNo", m.get("WhatsAppNo"));
        String proc = id == 0 ? "[dbo].[Sp_SupplierCustomerShipToAddress_Insert]"
                              : "[dbo].[Sp_SupplierCustomerShipToAddress_Update]";
        StringBuilder sql = new StringBuilder("SET NOCOUNT ON; EXEC ").append(proc);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        /* Insert ends with SELECT SCOPE_IDENTITY(); Update may return nothing - execute()
           handles both without "A result set was generated for update". */
        return jdbcTemplate.execute(sql.toString(), (org.springframework.jdbc.core.PreparedStatementCallback<Object>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            boolean rs = ps.execute();
            while (true) {
                if (rs) {
                    try (java.sql.ResultSet r = ps.getResultSet()) {
                        if (r.next()) return r.getObject(1);
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                rs = ps.getMoreResults();
            }
            return id;
        });
    }

    private static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return (int) Double.parseDouble(String.valueOf(v).trim()); } catch (Exception e) { return 0; }
    }

    private Date parseDateOrNull(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return new SimpleDateFormat("yyyy-MM-dd").parse(s.trim()); } catch (Exception e) { return null; }
    }
}
