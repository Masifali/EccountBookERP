package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import com.mst.models.ItemType;
import com.mst.repositories.IItemTypeRepository;
import com.mst.serviceInterface.IItemTypeService;

@Service("itemTypeService")
public class ItemTypeService implements IItemTypeService {

	@Autowired
	private IItemTypeRepository itemTypeRepository;
	@Autowired
	private CurrentUserContext currentUserContext;
	@Autowired
	private JdbcTemplate jdbcTemplate;

	@Override
	public List<ItemType> getAll() {
		return itemTypeRepository.findAllByOrderByTypeDescription();
	}

	@Override
	public ItemType getById(int id) {
		return itemTypeRepository.findById(id).orElse(null);
	}

	@Override
	public ItemType addOrUpdate(ItemType itemType) {
		LocalDateTime now = LocalDateTime.now();
		if (itemType.getId() == null || itemType.getId() == 0) {
			itemType.setId(itemTypeRepository.findMaxId() + 1);
			itemType.setEntryDate(now);
			itemType.setEntryUser(currentUserContext.currentUserId());
			itemType.setOrganizationId(currentUserContext.currentOrganizationId());
			itemType.setCompanyId(currentUserContext.currentCompanyId());
			itemType.setPostState(false);
		} else {
			itemType.setModifyDate(now);
			itemType.setModifyUser(currentUserContext.currentUserId());
		}
		return itemTypeRepository.save(itemType);
	}

	@Override
	public void delete(int id) {
		itemTypeRepository.deleteById(id);
	}

	@Override
	public List<Map<String, Object>> getParentCategoriesLookup() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			return jdbcTemplate.queryForList(
				"SET NOCOUNT ON; EXEC Sp_InventoryItemsOther_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='InventoryParentCategories'",
				orgId, compId
			);
		} catch (Exception e) {
			return jdbcTemplate.queryForList(
				"SELECT Id, InvParentCateDescription FROM dbo.InventoryParentCategories WHERE Id IN (1,2,4,6,10,11) ORDER BY InvParentCateDescription"
			);
		}
	}

	@Override
	public List<Map<String, Object>> getItemTypeLookups() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			return jdbcTemplate.queryForList(
				"SET NOCOUNT ON; EXEC Sp_InvLookUp_GetAllMethod @OrganizationId=?, @CompanyId=?, @InvLookupTypeId=6, @Activity='GetLookupsByTypeIdDt'",
				orgId, compId
			);
		} catch (Exception e) {
			return jdbcTemplate.queryForList(
				"SELECT Id, LookupName FROM dbo.InvLookUp WHERE InvLookupTypeId=6 AND Id IN (15,16,18) ORDER BY LookupName"
			);
		}
	}

	@Override
	public List<Map<String, Object>> getItemTypeHistory() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			return jdbcTemplate.queryForList(
				"SET NOCOUNT ON; EXEC Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @ParentCategoryIds='1,2,4,6,10,11', @Activity='ReadByOrganizationCompanyId'",
				orgId, compId
			);
		} catch (Exception e) {
			return jdbcTemplate.queryForList(
				"SELECT t.Id, t.TypeCode, t.TypeDescription, l.LookupName AS TypeName, c.InvParentCateDescription AS ParentCategoryName, t.IsMother " +
				"FROM dbo.ItemType t " +
				"LEFT JOIN dbo.InvLookUp l ON t.Type = l.Id " +
				"LEFT JOIN dbo.InventoryParentCategories c ON t.ParentCategoryId = c.Id " +
				"WHERE t.OrganizationId=? AND t.CompanyId=? ORDER BY t.Id DESC",
				orgId, compId
			);
		}
	}

	@Override
	public String generateItemTypeCode() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(
				"SET NOCOUNT ON; EXEC Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GenerateCode'",
				orgId, compId
			);
			if (list != null && !list.isEmpty() && list.get(0).get("TypeCode") != null) {
				return list.get(0).get("TypeCode").toString();
			}
		} catch (Exception e) {
			// Fallback code generation
		}
		Integer maxId = jdbcTemplate.queryForObject("SELECT ISNULL(MAX(Id), 0) + 1 FROM dbo.ItemType", Integer.class);
		return String.format("%03d", maxId != null ? maxId : 1);
	}

	@Override
	public Map<String, Object> getByIdSp(int id) {
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(
				"SET NOCOUNT ON; EXEC Sp_ItemType_GetAllMethod @Id=?, @Activity='ReadById'",
				id
			);
			if (list != null && !list.isEmpty()) {
				return list.get(0);
			}
		} catch (Exception e) {
			List<Map<String, Object>> list = jdbcTemplate.queryForList("SELECT * FROM dbo.ItemType WHERE Id=?", id);
			if (list != null && !list.isEmpty()) {
				return list.get(0);
			}
		}
		return null;
	}

	@Override
	public Map<String, Object> saveSp(ItemType itemType) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int userId = currentUserContext.currentUserId();

		Integer parentCatId = (itemType.getParentCategoryId() != null && itemType.getParentCategoryId() > 0) ? itemType.getParentCategoryId() : null;
		Integer typeVal = (itemType.getType() != null && itemType.getType() > 0) ? itemType.getType() : null;

		if (itemType.getId() == null || itemType.getId() == 0) {
			// INSERT using Golden Master stored procedure
			String sql = "EXEC Sp_ItemType_Insert @TypeCode=?, @TypeDescription=?, @Type=?, @ParentCategoryId=?, @IsMother=?, @OrganizationId=?, @CompanyId=?, @EntryUser=?, @ModifyUser=?";
			jdbcTemplate.update(sql,
				itemType.getTypeCode(),
				itemType.getTypeDescription(),
				typeVal,
				parentCatId,
				Boolean.TRUE.equals(itemType.getIsMother()) ? 1 : 0,
				orgId,
				compId,
				userId,
				userId
			);
		} else {
			// UPDATE using Golden Master stored procedure - preserves existing ID
			String sql = "EXEC Sp_ItemType_Update @Id=?, @TypeCode=?, @TypeDescription=?, @Type=?, @ParentCategoryId=?, @IsMother=?, @OrganizationId=?, @CompanyId=?, @EntryUser=?, @ModifyUser=?, @PostState=?";
			jdbcTemplate.update(sql,
				itemType.getId(),
				itemType.getTypeCode(),
				itemType.getTypeDescription(),
				typeVal,
				parentCatId,
				Boolean.TRUE.equals(itemType.getIsMother()) ? 1 : 0,
				orgId,
				compId,
				userId,
				userId,
				0
			);
		}

		// Return saved or updated record
		List<Map<String, Object>> list = jdbcTemplate.queryForList(
			"SELECT TOP 1 * FROM dbo.ItemType WHERE TypeCode=? AND OrganizationId=? AND CompanyId=? ORDER BY Id DESC",
			itemType.getTypeCode(), orgId, compId
		);

		return (list != null && !list.isEmpty()) ? list.get(0) : Map.of("status", "success");
	}
}
