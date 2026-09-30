package com.mst.services;

import com.mst.models.dto.ItemPmMapDto;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class ItemPmMapService {

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    public Map<String, Object> lookups() {
        int compId = currentUserContext.currentCompanyId();
        int orgId = currentUserContext.currentOrganizationId();

        Map<String, Object> out = new LinkedHashMap<>();
        
        /* Main Item dropdown - ItemTypeIds 14, 17 */
        String sqlMainItems = "SELECT DISTINCT I.Id, I.ItemName, I.ItemCodeNew AS ItemCode, "
                + "I.ItemUomId AS BaseUomId, U.UOMCode AS BaseUom "
                + "FROM InventoryItemFile I "
                + "LEFT JOIN GlobalUOMSchedule U ON I.ItemUomId = U.Id "
                + "WHERE I.OrganizationId = ? AND I.CompanyId = ? AND I.IsActive = 1 "
                + "ORDER BY I.ItemName";
        out.put("mainItems", jdbcTemplate.queryForList(sqlMainItems, orgId, compId));

        /* PM Item dropdown - ItemTypeId 14 */
        String sqlPmItems = "SELECT DISTINCT I.Id, I.ItemName, I.ItemCodeNew AS ItemCode, "
                + "I.ItemUomId AS BaseUomId, U.UOMCode AS BaseUom "
                + "FROM InventoryItemFile I "
                + "LEFT JOIN GlobalUOMSchedule U ON I.ItemUomId = U.Id "
                + "WHERE I.OrganizationId = ? AND I.CompanyId = ? AND I.IsActive = 1 "
                + "ORDER BY I.ItemName";
        out.put("pmItems", jdbcTemplate.queryForList(sqlPmItems, orgId, compId));

        /* UOM list */
        String sqlUoms = "SELECT Id, UOMCode, Equivalent FROM GlobalUOMSchedule WHERE IsActive = 1 ORDER BY UOMCode";
        out.put("uoms", jdbcTemplate.queryForList(sqlUoms));

        return out;
    }

    public List<Map<String, Object>> uomsForItem(int itemId) {
        String sql = "SELECT U.Id, U.UOMCode, U.Equivalent "
                + "FROM ItemUOMSchedule I "
                + "JOIN GlobalUOMSchedule U ON I.UOMScheduleId = U.Id "
                + "WHERE I.ItemId = ? AND U.IsActive = 1 "
                + "ORDER BY U.UOMCode";
        List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, itemId);
        if (list.isEmpty()) {
            sql = "SELECT U.Id, U.UOMCode, U.Equivalent FROM InventoryItemFile I "
                + "JOIN GlobalUOMSchedule U ON I.ItemUomId = U.Id WHERE I.Id = ?";
            list = jdbcTemplate.queryForList(sql, itemId);
        }
        return list;
    }

    public List<Map<String, Object>> history(String fromDate, String toDate, Integer itemId) {
        int compId = currentUserContext.currentCompanyId();
        int orgId = currentUserContext.currentOrganizationId();
        
        try {
            return jdbcTemplate.queryForList(
                "EXEC [mrp].[usp_ItemAndPMItemMap_FormHistory] "
                + "@OrganizationId=?, @CompanyId=?, @CanViewAllRecord=1, "
                + "@EntryFromDate=?, @EntryToDate=?, @ItemId=?",
                orgId, compId, fromDate, toDate, itemId == null ? 0 : itemId
            );
        } catch (Exception e) {
            String sql = "SELECT M.Id, M.ItemId, I.ItemName, I.ItemCodeNew AS ItemCode, "
                       + "M.BaseUomId, U.UOMCode AS BaseUom, M.Remarks, M.IsActive, M.EntryDate "
                       + "FROM tblItemAndPMItemMap M "
                       + "JOIN InventoryItemFile I ON M.ItemId = I.Id "
                       + "LEFT JOIN GlobalUOMSchedule U ON M.BaseUomId = U.Id "
                       + "WHERE M.OrganizationId = ? AND M.CompanyId = ? "
                       + "ORDER BY M.Id DESC";
            return jdbcTemplate.queryForList(sql, orgId, compId);
        }
    }

    public Map<String, Object> getById(int id) {
        int compId = currentUserContext.currentCompanyId();
        int orgId = currentUserContext.currentOrganizationId();
        
        List<Map<String, Object>> main = jdbcTemplate.queryForList(
            "SELECT M.Id, M.ItemId, I.ItemName, I.ItemCodeNew AS ItemCode, "
            + "M.BaseUomId, U.UOMCode AS BaseUom, M.Remarks, M.IsActive "
            + "FROM tblItemAndPMItemMap M "
            + "JOIN InventoryItemFile I ON M.ItemId = I.Id "
            + "LEFT JOIN GlobalUOMSchedule U ON M.BaseUomId = U.Id "
            + "WHERE M.Id = ? AND M.OrganizationId = ? AND M.CompanyId = ?",
            id, orgId, compId
        );
        if (main.isEmpty()) return null;

        Map<String, Object> out = new LinkedHashMap<>(main.get(0));

        List<Map<String, Object>> details = jdbcTemplate.queryForList(
            "SELECT D.Id, D.PmItemId, P.ItemName AS PmItem, P.ItemCodeNew AS PmItemCode, "
            + "D.BaseUomId, U.UOMCode AS BaseUom, D.PmQty, D.WeightCapacity, D.Remarks "
            + "FROM tblItemAndPMItemMapDetail D "
            + "JOIN InventoryItemFile P ON D.PmItemId = P.Id "
            + "LEFT JOIN GlobalUOMSchedule U ON D.BaseUomId = U.Id "
            + "WHERE D.ItemAndPMItemMapId = ?",
            id
        );
        out.put("details", details);
        return out;
    }

    public Map<String, Object> save(ItemPmMapDto dto) {
        int compId = currentUserContext.currentCompanyId();
        int orgId = currentUserContext.currentOrganizationId();
        int userId = currentUserContext.currentUserId();

        if (dto.getItemId() <= 0) throw new IllegalArgumentException("Item Field is Required");
        if (dto.getBaseUomId() <= 0) throw new IllegalArgumentException("BaseUom Field is Required");

        int mapId = dto.getId();
        if (mapId <= 0) {
            /* Insert Master */
            jdbcTemplate.update(
                "INSERT INTO tblItemAndPMItemMap (OrganizationId, CompanyId, ItemId, BaseUomId, Remarks, IsActive, EntryUserId, EntryDate, ModifyUserId, ModifyDate) "
                + "VALUES (?, ?, ?, ?, ?, ?, ?, GETDATE(), ?, GETDATE())",
                orgId, compId, dto.getItemId(), dto.getBaseUomId(), dto.getRemarks(), dto.isActive() ? 1 : 0, userId, userId
            );
            mapId = jdbcTemplate.queryForObject("SELECT ISNULL(MAX(Id), 0) FROM tblItemAndPMItemMap", Integer.class);
        } else {
            /* Update Master */
            jdbcTemplate.update(
                "UPDATE tblItemAndPMItemMap SET ItemId=?, BaseUomId=?, Remarks=?, IsActive=?, ModifyUserId=?, ModifyDate=GETDATE() "
                + "WHERE Id=? AND OrganizationId=? AND CompanyId=?",
                dto.getItemId(), dto.getBaseUomId(), dto.getRemarks(), dto.isActive() ? 1 : 0, userId, mapId, orgId, compId
            );
            jdbcTemplate.update("DELETE FROM tblItemAndPMItemMapDetail WHERE ItemAndPMItemMapId=?", mapId);
        }

        /* Insert Details */
        if (dto.getDetails() != null) {
            for (ItemPmMapDto.Detail d : dto.getDetails()) {
                jdbcTemplate.update(
                    "INSERT INTO tblItemAndPMItemMapDetail (ItemAndPMItemMapId, PmItemId, BaseUomId, PmQty, WeightCapacity, Remarks) "
                    + "VALUES (?, ?, ?, ?, ?, ?)",
                    mapId, d.getPmItemId(), d.getBaseUomId(), d.getPmQty(), d.getWeightCapacity(), d.getRemarks()
                );
            }
        }

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", mapId);
        res.put("message", "Record saved successfully!");
        return res;
    }

    public Map<String, Object> delete(int id) {
        int compId = currentUserContext.currentCompanyId();
        int orgId = currentUserContext.currentOrganizationId();
        
        jdbcTemplate.update("DELETE FROM tblItemAndPMItemMapDetail WHERE ItemAndPMItemMapId=?", id);
        int rows = jdbcTemplate.update("DELETE FROM tblItemAndPMItemMap WHERE Id=? AND OrganizationId=? AND CompanyId=?", id, orgId, compId);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", rows > 0);
        res.put("message", rows > 0 ? "Record deleted successfully!" : "Could not delete record.");
        return res;
    }
}
