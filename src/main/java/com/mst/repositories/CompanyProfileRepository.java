package com.mst.repositories;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Types;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * "Company Profile" - the procedures {@code Architecture.WinApp.Configurations.frmCompanyProfile}
 * actually calls, with the desktop's own parameters.
 *
 * ---------------------------------------------------------------------------------------------
 * THE DESKTOP CHAIN, TRACED
 * ---------------------------------------------------------------------------------------------
 *   OrganizationFill()  BLL 0072 Organization.GetByID(UserAccount.OrganizationId)
 *                       -> Sp_Organization_GetAllMethod @Id, @Activity='ReadById'
 *   LocationsBind()     BLL 0062 Company.GetAlldt(new Company{ OrgCompanyTypeId = <that org> })
 *                       -> Sp_Company_GetAllMethod @OrgCompanyTypeId, @Id, @Activity='ReadByOrganizationId'
 *   Insert()            BLL 0062 Company.Save(obj)
 *                       -> obj.Id == 0 ? DAL 0056 SetData(obj,"Proc_Company_Insert")
 *                                      : DAL 0056 SetData(obj,"Proc_Company_Update")
 *
 * DAL 0056 SetData opens its own connection, BeginTransaction, SetProc, and on any exception
 * rolls back. The {@code @Transactional} below is that transaction.
 *
 *   num = Convert.ToInt32(SetProc(trn, obj, ProcName));
 *   if (num > 0) obj.Id = num; else num = obj.Id;
 *
 * Proc_Company_Insert ends with {@code SELECT @Id}; Proc_Company_Update selects nothing, so the
 * scalar comes back null, {@code Convert.ToInt32(null)} is 0, and the existing Id is kept. Both
 * branches are reproduced exactly below.
 *
 * ---------------------------------------------------------------------------------------------
 * PARAMETERS ARE THE MODEL'S PROPERTY LIST, IN DECLARATION ORDER
 * ---------------------------------------------------------------------------------------------
 * {@code GenericProvider.SetProc} reflects over Architecture.Model.Company and sends one
 * parameter per NON-VIRTUAL property, named after the property. That model declares 35
 * properties, the last two - FirstName and LastName - {@code virtual}, so 33 parameters go to the
 * procedure. {@link #PARAMS} is that list, in that order, and every one of the 33 is declared by
 * both Proc_Company_Insert and Proc_Company_Update (checked against the schema, no parameter is
 * unaccepted and none of them lacks a default).
 *
 * Named parameters are used rather than positional ones, so a future change to either procedure's
 * parameter order cannot silently shift a value into the wrong column.
 */
@Repository
public class CompanyProfileRepository {

    private static final String P_ORG    = "Sp_Organization_GetAllMethod";
    private static final String P_GET    = "Sp_Company_GetAllMethod";
    private static final String P_INSERT = "Proc_Company_Insert";
    private static final String P_UPDATE = "Proc_Company_Update";

    /** Architecture.Model.Company, declaration order, virtual properties excluded. */
    private static final String[] PARAMS = {
            "Id", "OrgCompanyTypeId", "CompAddress", "CompBaseCurr", "CompCode",
            "CompContactPerson", "CompCountry", "CompEmailA", "CompEmailB", "CompLogo",
            "CompMobileA", "CompMobileB", "CompMobileC", "CompName", "CompReportingTitle",
            "CompState", "CompTel", "CompType", "PictureURL", "EntryUser",
            "ModifyUser", "CompLogoImage", "EntryDate", "ModifyDate", "CityName",
            "CompanyTemplateId", "CompanyId", "CompanyNameOtherLanguage", "CompanyNameOtherLing",
            "CompanyAddressOtherLanguage", "CompanyWebsite", "AllowedUserCount", "CompanyFaxNo"
    };

    private final JdbcTemplate jdbc;
    public CompanyProfileRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================================== read

    /**
     * The organization combo. The desktop builds a ONE-ROW DataTable from
     * Organization.GetByID(UserAccount.OrganizationId) - the signed-in user's own organization and
     * nothing else. There is no picker here and there never was.
     */
    public Map<String, Object> organization(int organizationId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_ORG + " @Id=?, @Activity=?", organizationId, "ReadById");
        return rows.isEmpty() ? null : rows.get(0);
    }

    /**
     * The grid. @Id is left null so the procedure returns every company in the organization; its
     * own WHERE also drops the head office ({@code ISNULL(IsHeadOffice,0) <> 1}), which is why the
     * desktop grid never lists it either.
     */
    public List<Map<String, Object>> companies(int organizationId) {
        return jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrgCompanyTypeId=?, @Id=?, @Activity=?",
                organizationId,
                new SqlParameterValue(Types.INTEGER, null),
                "ReadByOrganizationId");
    }

    /** One row of that same grid, used to re-read a record before an update. */
    public Map<String, Object> company(int organizationId, int id) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrgCompanyTypeId=?, @Id=?, @Activity=?",
                organizationId, id, "ReadByOrganizationId");
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ================================================================================== write

    /**
     * BLL 0062 Company.Save - the Id decides the procedure, exactly as on the desktop.
     *
     * Proc_Company_Insert raises its own error when the organization is at its licensed company
     * count ("You have Reached your Max Company Level..."). That message is left to reach the
     * caller unchanged rather than being pre-empted here with a count of my own.
     */
    @Transactional(propagation = Propagation.REQUIRED)
    public int save(Map<String, Object> model) {
        int id = intOf(model.get("Id"));
        Integer returned = exec(id == 0 ? P_INSERT : P_UPDATE, model);
        return (returned != null && returned > 0) ? returned : id;
    }

    private Integer exec(String proc, Map<String, Object> model) {
        StringBuilder sql = new StringBuilder("EXEC dbo.").append(proc).append(' ');
        List<Object> args = new ArrayList<>();
        for (int i = 0; i < PARAMS.length; i++) {
            if (i > 0) sql.append(", ");
            String name = PARAMS[i];
            sql.append('@').append(name).append("=?");
            args.add(typed(name, model.get(name)));
        }
        List<Map<String, Object>> rows = jdbc.queryForList(sql.toString(), args.toArray());
        if (rows.isEmpty()) return null;
        for (Object v : rows.get(0).values()) {
            if (v instanceof Number) return ((Number) v).intValue();
        }
        return null;
    }

    /**
     * An untyped null goes to the driver as an INTEGER null, and a procedure that declares the
     * parameter as varbinary or datetime then fails with "Operand type clash" before it runs. So
     * every null carries the type of the column it is headed for.
     */
    private static Object typed(String name, Object value) {
        if (value != null) return value;
        switch (name) {
            case "CompLogoImage":
                return new SqlParameterValue(Types.VARBINARY, null);
            case "EntryDate":
            case "ModifyDate":
                return new SqlParameterValue(Types.TIMESTAMP, null);
            case "Id":
            case "OrgCompanyTypeId":
            case "EntryUser":
            case "ModifyUser":
            case "CompanyTemplateId":
            case "CompanyId":
            case "AllowedUserCount":
                return new SqlParameterValue(Types.INTEGER, null);
            default:
                return new SqlParameterValue(Types.NVARCHAR, null);
        }
    }

    public static int intOf(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** The parameter list, exposed so the service can build a model without repeating it. */
    public static Map<String, Object> blankModel() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String p : PARAMS) m.put(p, null);
        return m;
    }
}
