package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ItemPmMapDto;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.CallableStatementCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Date;
import java.time.LocalDate;
import java.util.*;

/** Web implementation of desktop screen 497, frmItemAndPMItemMap. */
@Service
public class ItemPmMapService {

    private static final int SCREEN_ID = 497;
    private static final String SCREEN_NAME = "frmItemAndPMItemMap";

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights desktopRights;

    public Map<String, Object> lookups() {
        UserAccount user = user("View");
        Map<String, Object> out = new LinkedHashMap<>();

        // Desktop frmItemAndPMItemMap.DeliverOrder_Load binds both item combos from
        // Item.ItemsWithBaseUOMNew (BLL 0583), with @TypeIds='14'.
        List<Map<String, Object>> itemRows = itemRows(user);
        List<Map<String, Object>> items = new ArrayList<>(itemRows.size());
        for (Map<String, Object> row : itemRows) {
            Map<String, Object> item = new LinkedHashMap<>();
            item.put("Id", column(row, "Id"));
            item.put("ItemName", column(row, "ItemName"));
            item.put("ItemCode", column(row, "ItemCodeNew"));
            item.put("BaseUomId", column(row, "ItemUomId"));
            item.put("BaseUom", column(row, "UOMCode"));
            item.put("Equivalent", column(row, "Equivalent"));
            items.add(item);
        }
        out.put("mainItems", items);
        out.put("pmItems", items);

        // Desktop ComboFill uses ItemAndPMItemMap.GetDataForDropDownFromItemAndPMItemMap("ItemMain").
        out.put("historyItems", jdbcTemplate.queryForList(
                "EXEC [MRP].[USP_DropDownFillFromItemAndPMItemMap] @OrganizationId=?, @CompanyId=?, @Activity=?",
                user.getOrganizationId(), user.getCompanyId(), "ItemMain"));

        Map<String, Boolean> permissions = new LinkedHashMap<>();
        for (String action : List.of("Save", "Update", "Delete", "CanView AllRecord")) {
            permissions.put(action, hasRight(user, action));
        }
        out.put("permissions", permissions);
        return out;
    }

    public List<Map<String, Object>> uomsForItem(int itemId) {
        UserAccount user = user("View");
        if (itemId <= 0) return List.of();
        return jdbcTemplate.queryForList(
                "EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",
                user.getOrganizationId(), user.getCompanyId(), itemId, "ReadByItemID");
    }

    public List<Map<String, Object>> history(String fromDate, String toDate, Integer itemId) {
        UserAccount user = user("View");
        boolean canViewAll = hasRight(user, "CanView AllRecord");

        StringBuilder sql = new StringBuilder("EXEC [MRP].[usp_ItemAndPMItemMap_FormHistory] "
                + "@OrganizationId=?, @CompanyId=?, @CanViewAllRecord=?");
        List<Object> args = new ArrayList<>(List.of(user.getOrganizationId(), user.getCompanyId(), canViewAll));
        if (!canViewAll) {
            // The deployed desktop DDL names this filter @EntryUser (not @EntryUserId).
            sql.append(", @EntryUser=?");
            args.add(user.getId());
        }
        if (fromDate != null && !fromDate.isBlank()) {
            sql.append(", @EntryFromDate=?");
            args.add(Date.valueOf(LocalDate.parse(fromDate)));
        }
        if (toDate != null && !toDate.isBlank()) {
            sql.append(", @EntryToDate=?");
            args.add(Date.valueOf(LocalDate.parse(toDate)));
        }
        if (itemId != null && itemId > 0) {
            sql.append(", @ItemId=?");
            args.add(itemId);
        }
        return jdbcTemplate.queryForList(sql.toString(), args.toArray());
    }

    public Map<String, Object> getById(int id) {
        UserAccount user = user("View");
        if (id <= 0 || !ownsActiveRecord(user, id)) return null;

        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC [MRP].[USP_ItemAndPMItemMap_GetAllMethod] @OrganizationId=?, @CompanyId=?, @Id=?, @EntryUserId=?, @Activity=?",
                user.getOrganizationId(), user.getCompanyId(), id, user.getId(), "ReadById");
        if (rows.isEmpty()) return null;

        Map<String, Object> out = new LinkedHashMap<>(rows.get(0));
        out.put("Id", column(out, "ItemAndPMItemMapId"));
        out.put("Remarks", column(out, "RemarksHeader"));

        List<Map<String, Object>> detailRows = jdbcTemplate.queryForList(
                "EXEC [MRP].[USP_ItemAndPMItemMap_GetAllMethod] @OrganizationId=?, @CompanyId=?, @Id=?, @EntryUserId=?, @Activity=?",
                user.getOrganizationId(), user.getCompanyId(), id, user.getId(), "ReadDetailByHeaderId");
        List<Map<String, Object>> details = new ArrayList<>(detailRows.size());
        for (Map<String, Object> row : detailRows) {
            Map<String, Object> detail = new LinkedHashMap<>(row);
            detail.put("Id", column(row, "ItemAndPMItemMapDetailId"));
            detail.put("PmItem", column(row, "PmItemName"));
            detail.put("BaseUom", column(row, "BaseUomCode"));
            detail.put("Remarks", column(row, "RemarksDetail"));
            details.add(detail);
        }
        out.put("details", details);
        return out;
    }

    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> save(ItemPmMapDto dto) {
        if (dto == null || dto.getId() < 0) throw new IllegalArgumentException("Invalid map record");
        boolean insert = dto.getId() == 0;
        UserAccount user = user(insert ? "Save" : "Update");

        if (dto.getItemId() <= 0) throw new IllegalArgumentException("Item Field is Required");
        if (dto.getBaseUomId() <= 0) throw new IllegalArgumentException("BaseUom Field is Required");
        if (dto.getDetails() == null || dto.getDetails().isEmpty()) {
            throw new IllegalArgumentException("At least one PM item detail row is required");
        }
        List<Map<String, Object>> availableItems = itemRows(user);
        if (!itemExists(availableItems, dto.getItemId())) {
            throw new IllegalArgumentException("Select a valid master item");
        }
        if (!uomExists(user, dto.getItemId(), dto.getBaseUomId())) {
            throw new IllegalArgumentException("Select a valid master item Base UOM");
        }
        for (ItemPmMapDto.Detail detail : dto.getDetails()) {
            if (detail == null || detail.getPmItemId() <= 0 || detail.getBaseUomId() <= 0
                    || detail.getPmQty() <= 0 || detail.getWeightCapacity() <= 0) {
                throw new IllegalArgumentException("Complete each PM item detail row");
            }
            if (!itemExists(availableItems, detail.getPmItemId())) {
                throw new IllegalArgumentException("Select a valid PM item");
            }
            if (!uomExists(user, detail.getPmItemId(), detail.getBaseUomId())) {
                throw new IllegalArgumentException("Select a valid PM item Base UOM");
            }
        }

        int mapId = dto.getId();
        List<Map<String, Object>> previousDetails = List.of();
        if (insert) {
            mapId = insertOrUpdateHeader(user, dto, 0, 1);
        } else {
            if (!ownsActiveRecord(user, mapId)) {
                throw new IllegalArgumentException("Map record was not found");
            }
            previousDetails = jdbcTemplate.queryForList(
                    "EXEC [MRP].[USP_ItemAndPMItemMap_GetAllMethod] @OrganizationId=?, @CompanyId=?, @Id=?, @EntryUserId=?, @Activity=?",
                    user.getOrganizationId(), user.getCompanyId(), mapId, user.getId(), "ReadDetailByHeaderId");
            insertOrUpdateHeader(user, dto, mapId, 2);
            for (Map<String, Object> old : previousDetails) {
                updateDetail(user, mapId, old, 3);
            }
        }

        int sequence = 1;
        for (ItemPmMapDto.Detail detail : dto.getDetails()) {
            insertDetail(user, mapId, detail, sequence++);
        }

        Map<String, Object> result = new LinkedHashMap<>();
        result.put("success", true);
        result.put("id", mapId);
        result.put("message", insert ? "Record saved successfully!" : "Record updated successfully!");
        return result;
    }

    public Map<String, Object> delete(int id) {
        UserAccount user = user("Delete");
        if (id <= 0 || !ownsActiveRecord(user, id)) {
            return Map.of("success", false, "message", "Could not delete record.");
        }

        jdbcTemplate.execute("{call [MRP].[USP_ItemAndPMItemMap_GetAllMethod](?, ?, ?, ?, ?)}",
                (CallableStatementCallback<Void>) statement -> {
                    statement.setInt(1, user.getOrganizationId());
                    statement.setInt(2, user.getCompanyId());
                    statement.setInt(3, id);
                    statement.setInt(4, user.getId());
                    statement.setString(5, "DeleteById");
                    statement.execute();
                    return null;
                });

        return Map.of("success", true, "message", "Record deleted successfully!");
    }

    private int insertOrUpdateHeader(UserAccount user, ItemPmMapDto dto, int mapId, int actionId) {
        String sql = "EXEC [MRP].[USP_ItemAndPMItemMap_InsertAndUpdate] "
                + "@ItemAndPMItemMapId=?, @ItemUomCkId=?, @ItemId=?, @BaseUomId=?, @RemarksHeader=?, "
                + "@IsActive=?, @ActionId=?, @EntryDate=?, @EntryUserId=?, @ModifyDate=?, @ModifyUserId=?, "
                + "@OrganizationId=?, @CompanyId=?, @ScreenName=?";
        Object[] args = {actionId == 1 ? null : mapId, null, dto.getItemId(), dto.getBaseUomId(), dto.getRemarks(),
                dto.isActive(), actionId, null, user.getId(), null, user.getId(), user.getOrganizationId(),
                user.getCompanyId(), SCREEN_NAME};
        if (actionId == 1) {
            List<Map<String, Object>> result = jdbcTemplate.queryForList(sql, args);
            if (result.isEmpty() || result.get(0).isEmpty()) {
                throw new IllegalStateException("The database did not return the new map ID");
            }
            Object value = result.get(0).values().iterator().next();
            if (!(value instanceof Number)) throw new IllegalStateException("The database returned an invalid map ID");
            return ((Number) value).intValue();
        }
        jdbcTemplate.update(sql, args);
        return mapId;
    }

    private void insertDetail(UserAccount user, int mapId, ItemPmMapDto.Detail detail, int sequence) {
        Object[] args = {null, mapId, detail.getPmItemId(), detail.getBaseUomId(), detail.getWeightCapacity(),
                detail.getPmQty(), sequence, detail.getRemarks(), null, user.getId(), null, user.getId(), 1, null};
        jdbcTemplate.queryForList(
                "EXEC [MRP].[USP_ItemAndPMItemMapDetail_Insert] "
                        + "@ItemAndPMItemMapDetailId=?, @ItemAndPMItemMapId=?, @PmItemId=?, @BaseUomId=?, "
                        + "@WeightCapacity=?, @PmQty=?, @seqNo=?, @RemarksDetail=?, @EntryDate=?, @EntryUserId=?, "
                        + "@ModifyDate=?, @ModifyUserId=?, @ActionTypeId=?, @RevisionNo=?",
                args);
    }

    private void updateDetail(UserAccount user, int mapId, Map<String, Object> old, int actionId) {
        jdbcTemplate.update(
                "EXEC [MRP].[USP_ItemAndPMItemMapDetail_Insert] "
                        + "@ItemAndPMItemMapDetailId=?, @ItemAndPMItemMapId=?, @PmItemId=?, @BaseUomId=?, "
                        + "@WeightCapacity=?, @PmQty=?, @seqNo=?, @RemarksDetail=?, @EntryDate=?, @EntryUserId=?, "
                        + "@ModifyDate=?, @ModifyUserId=?, @ActionTypeId=?, @RevisionNo=?",
                column(old, "ItemAndPMItemMapDetailId"), mapId, column(old, "PmItemId"), column(old, "BaseUomId"),
                column(old, "WeightCapacity"), column(old, "PmQty"), column(old, "seqNo"),
                column(old, "RemarksDetail"), column(old, "EntryDate"), user.getId(), null, user.getId(),
                actionId, column(old, "RevisionNo"));
    }

    private boolean ownsActiveRecord(UserAccount user, int id) {
        StringBuilder sql = new StringBuilder(
                "SELECT TOP (1) ItemAndPMItemMapId FROM [MRP].[ItemAndPMItemMap] "
                        + "WHERE ItemAndPMItemMapId=? AND OrganizationId=? AND CompanyId=? AND ISNULL(ActionId, 1)<>3");
        List<Object> args = new ArrayList<>(List.of(id, user.getOrganizationId(), user.getCompanyId()));
        if (!hasRight(user, "CanView AllRecord")) {
            sql.append(" AND EntryUserId=?");
            args.add(user.getId());
        }
        return !jdbcTemplate.queryForList(sql.toString(), args.toArray()).isEmpty();
    }

    private List<Map<String, Object>> itemRows(UserAccount user) {
        return jdbcTemplate.queryForList(
                "EXEC dbo.Usp_ItemsWithBaseUOMNew @OrganizationId=?, @CompanyId=?, @TypeIds=?",
                user.getOrganizationId(), user.getCompanyId(), "14");
    }

    private List<Map<String, Object>> uomsForItem(UserAccount user, int itemId) {
        return jdbcTemplate.queryForList(
                "EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",
                user.getOrganizationId(), user.getCompanyId(), itemId, "ReadByItemID");
    }

    private static boolean itemExists(List<Map<String, Object>> items, int id) {
        return items.stream().anyMatch(row -> Objects.equals(asInt(column(row, "Id")), id));
    }

    private boolean uomExists(UserAccount user, int itemId, int uomId) {
        return uomsForItem(user, itemId).stream()
                .anyMatch(row -> Objects.equals(asInt(column(row, "Id")), uomId));
    }

    private UserAccount user(String action) {
        UserAccount user = currentUserContext.requireAccountingUser();
        desktopRights.require(user, SCREEN_ID, action);
        return user;
    }

    /** Missing CanView AllRecord grant means the form is limited to the user's own records. */
    private boolean hasRight(UserAccount user, String action) {
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "SELECT TOP (1) ur.Value FROM dbo.ScreenRights sr "
                        + "JOIN dbo.tblUserRights ur ON ur.RightId=sr.Id AND ur.UserId=? AND ur.CompanyId=? "
                        + "WHERE sr.ScreenID=? AND sr.RightName=?",
                user.getId(), user.getCompanyId(), SCREEN_ID, action);
        return !rows.isEmpty() && asBoolean(column(rows.get(0), "Value"));
    }

    private static Integer asInt(Object value) {
        return value instanceof Number ? ((Number) value).intValue() : null;
    }

    private static boolean asBoolean(Object value) {
        if (value instanceof Boolean) return (Boolean) value;
        if (value instanceof Number) return ((Number) value).intValue() != 0;
        return value != null && Boolean.parseBoolean(value.toString());
    }

    private static Object column(Map<String, Object> row, String name) {
        for (Map.Entry<String, Object> entry : row.entrySet()) {
            if (entry.getKey().equalsIgnoreCase(name)) return entry.getValue();
        }
        return null;
    }
}
