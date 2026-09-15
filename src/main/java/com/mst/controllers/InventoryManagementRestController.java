package com.mst.controllers;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.models.Brand;
import com.mst.models.Item;
import com.mst.models.ItemCategory;
import com.mst.models.ItemGroup;
import com.mst.models.ItemType;
import com.mst.models.ProductType;
import com.mst.models.Rack;
import com.mst.models.Warehouse;
import com.mst.repositories.IBrandRepository;
import com.mst.repositories.IItemCategoryRepository;
import com.mst.repositories.IItemGroupRepository;
import com.mst.repositories.IItemRepository;
import com.mst.repositories.IItemTypeRepository;
import com.mst.repositories.IProductTypeRepository;
import com.mst.repositories.IRackRepository;
import com.mst.repositories.IWarehouseRepository;

@RestController
@RequestMapping("/api/inventory")
public class InventoryManagementRestController {

	@Autowired
	private IItemRepository itemRepository;

	@Autowired
	private IItemCategoryRepository itemCategoryRepository;

	@Autowired
	private IItemTypeRepository itemTypeRepository;

	@Autowired
	private IItemGroupRepository itemGroupRepository;

	@Autowired
	private IWarehouseRepository warehouseRepository;

	@Autowired
	private IBrandRepository brandRepository;

	@Autowired
	private IProductTypeRepository productTypeRepository;

	@Autowired
	private IRackRepository rackRepository;

	@Autowired
	private JdbcTemplate jdbcTemplate;

	// ==========================================
	// 1. ITEMS MASTER ENDPOINTS
	// ==========================================

	@GetMapping("/items/list")
	public ResponseEntity<List<Map<String, Object>>> getItemsList() {
		String sql = "SELECT i.Id as id, i.ItemCode as itemCode, i.ItemName as itemName, i.ItemAliasName as itemAliasName, " +
				"i.ItemNameOtherLingo as itemNameOtherLingo, i.HSCode as hSCode, i.BarcodeNo as barcodeNo, " +
				"i.ProductNo as productNo, i.ModelName as modelName, i.ItemSpecification as itemSpecification, " +
				"i.ItemCategoryId as itemCategoryId, ic.CategoryDescription as categoryName, " +
				"i.ItemTypeId as itemTypeId, it.TypeDescription as typeName, " +
				"i.RackId as rackId, r.RackName as rackName, " +
				"i.CostPrice as costPrice, i.PurchasePrice as purchasePrice, i.RetailPrice as retailPrice, i.WholeSalePrice as wholeSalePrice, " +
				"i.MinRate as minRate, i.MaxRate as maxRate, i.MinStockLevel as minStockLevel, i.MaxStockLevel as maxStockLevel, " +
				"i.OptimalStockLevel as optimalStockLevel, i.ReorderLevel as reorderLevel, i.ReOrderQty as reOrderQty, " +
				"i.LeadTimeDay as leadTimeDay, i.WeightKgs as weightKgs, " +
				"i.PurchaseGLAC as purchaseGLAC, i.SaleGLAC as saleGLAC, i.COGSGLAC as cOGSGLAC, " +
				"i.SaleTaxPurPercent as saleTaxPurPercent, i.SaleTaxSalesPercent as saleTaxSalesPercent, " +
				"i.IsExpirable as isExpirable, i.ShelfLife as shelfLife, i.ApplyGST as applyGST, i.ApplyVAT as applyVAT, " +
				"i.IsTaxable as isTaxable, i.ItemStatus as itemStatus, i.IsDiscountable as isDiscountable, " +
				"i.AllowMultiUom as allowMultiUom, i.ItemWithVarient as itemWithVarient, i.IsImport as isImport, " +
				"i.IsCompany as isCompany, i.IsThirdParty as isThirdParty " +
				"FROM Item i " +
				"LEFT JOIN ItemCategory ic ON i.ItemCategoryId = ic.Id " +
				"LEFT JOIN ItemType it ON i.ItemTypeId = it.Id " +
				"LEFT JOIN Rack r ON i.RackId = r.Id " +
				"ORDER BY i.Id DESC";
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/items/save")
	public ResponseEntity<?> saveItem(@RequestBody Item item) {
		try {
			if (item.getId() == null || item.getId() <= 0) {
				Integer maxId = itemRepository.findMaxId();
				item.setId((maxId == null ? 0 : maxId) + 1);
				item.setEntryDate(LocalDateTime.now());
				item.setEntryUser(1);
			} else {
				item.setModifyDate(LocalDateTime.now());
				item.setModifyUser(1);
			}
			if (item.getOrganizationId() == null) item.setOrganizationId(1);
			if (item.getCompanyId() == null) item.setCompanyId(1);
			if (item.getItemStatus() == null) item.setItemStatus(true);

			Item saved = itemRepository.save(item);
			return ResponseEntity.ok(saved);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving item: " + e.getMessage());
		}
	}

	@DeleteMapping("/items/{id}")
	public ResponseEntity<?> deleteItem(@PathVariable("id") Integer id) {
		try {
			itemRepository.deleteById(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Item deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting item: " + e.getMessage());
		}
	}

	// ==========================================
	// 2. ITEM CATEGORIES ENDPOINTS
	// ==========================================

	@GetMapping("/categories/list")
	public ResponseEntity<List<Map<String, Object>>> getCategoriesList() {
		String sql = "SELECT c.Id as id, c.CategoryCode as categoryCode, c.CategoryDescription as categoryDescription, " +
				"c.SerialFrom as serialFrom, c.SerialTo as serialTo, c.CategoryStatus as categoryStatus, " +
				"c.RevenueAccountId as revenueAccountId, revCoa.AccountTitle as revenueAccountTitle, " +
				"c.CGSAccountId as cgsAccountId, cgsCoa.AccountTitle as cgsAccountTitle, " +
				"c.InventoryAccountId as inventoryAccountId, invCoa.AccountTitle as inventoryAccountTitle, " +
				"c.InventoryParentCategoriesId as inventoryParentCategoriesId, pCat.CategoryDescription as parentCategoryName, " +
				"c.ItemClassGroupId as itemClassGroupId, " +
				"CASE WHEN c.ItemClassGroupId = 1 THEN 'Raw Material' WHEN c.ItemClassGroupId = 2 THEN 'Finish Goods' WHEN c.ItemClassGroupId = 3 THEN 'By Product' ELSE '' END as classGroupName, " +
				"c.ItemProductionStageId as itemProductionStageId, " +
				"CASE WHEN c.ItemProductionStageId = 1 THEN 'UnProcess' WHEN c.ItemProductionStageId = 2 THEN 'Process' WHEN c.ItemProductionStageId = 3 THEN 'Finish Goods' ELSE '' END as productionStageName, " +
				"c.ItemVarietyNatureId as itemVarietyNatureId, " +
				"CASE WHEN c.ItemVarietyNatureId = 1 THEN 'Paddy' WHEN c.ItemVarietyNatureId = 2 THEN 'Brown' WHEN c.ItemVarietyNatureId = 3 THEN 'General' WHEN c.ItemVarietyNatureId = 4 THEN 'Steam' WHEN c.ItemVarietyNatureId = 5 THEN 'Steam Sela' WHEN c.ItemVarietyNatureId = 6 THEN 'Sela' WHEN c.ItemVarietyNatureId = 7 THEN 'White' ELSE '' END as varietyNatureName " +
				"FROM ItemCategory c " +
				"LEFT JOIN ChartofAccount revCoa ON c.RevenueAccountId = revCoa.Id " +
				"LEFT JOIN ChartofAccount cgsCoa ON c.CGSAccountId = cgsCoa.Id " +
				"LEFT JOIN ChartofAccount invCoa ON c.InventoryAccountId = invCoa.Id " +
				"LEFT JOIN ItemCategory pCat ON c.InventoryParentCategoriesId = pCat.Id " +
				"ORDER BY c.Id ASC";
		try {
			return ResponseEntity.ok(jdbcTemplate.queryForList(sql));
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/categories/save")
	public ResponseEntity<?> saveCategory(@RequestBody ItemCategory category) {
		try {
			if (category.getId() == null || category.getId() <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM ItemCategory";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				category.setId((maxId == null ? 0 : maxId) + 1);
			}
			ItemCategory saved = itemCategoryRepository.save(category);
			return ResponseEntity.ok(saved);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving category: " + e.getMessage());
		}
	}

	@DeleteMapping("/categories/{id}")
	public ResponseEntity<?> deleteCategory(@PathVariable("id") Integer id) {
		try {
			itemCategoryRepository.deleteById(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Category deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting category: " + e.getMessage());
		}
	}

	// ==========================================
	// 3. ITEM TYPES ENDPOINTS
	// ==========================================

	@GetMapping("/types/list")
	public ResponseEntity<List<Map<String, Object>>> getTypesList() {
		String sql = "SELECT t.Id as id, t.TypeCode as typeCode, t.TypeDescription as typeDescription, " +
				"t.Type as type, " +
				"CASE WHEN t.Type = 1 THEN 'Paddy' WHEN t.Type = 2 THEN 'Rice' WHEN t.Type = 3 THEN 'By Product' WHEN t.Type = 4 THEN 'Work In Process' ELSE '' END as typeName, " +
				"t.ParentCategoryId as parentCategoryId, ic.CategoryDescription as parentCategoryName, " +
				"t.IsMother as isMother, " +
				"CASE WHEN t.IsMother = 1 THEN 'Mother' ELSE 'Not Mother' END as motherStatus " +
				"FROM ItemType t " +
				"LEFT JOIN ItemCategory ic ON t.ParentCategoryId = ic.Id " +
				"ORDER BY t.Id ASC";
		try {
			return ResponseEntity.ok(jdbcTemplate.queryForList(sql));
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/types/save")
	public ResponseEntity<?> saveType(@RequestBody ItemType type) {
		try {
			if (type.getId() == null || type.getId() <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM ItemType";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				type.setId((maxId == null ? 0 : maxId) + 1);
			}
			ItemType saved = itemTypeRepository.save(type);
			return ResponseEntity.ok(saved);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving type: " + e.getMessage());
		}
	}

	@DeleteMapping("/types/{id}")
	public ResponseEntity<?> deleteType(@PathVariable("id") Integer id) {
		try {
			itemTypeRepository.deleteById(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Item Type deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting type: " + e.getMessage());
		}
	}

	// ==========================================
	// 4. ITEM GROUPS ENDPOINTS
	// ==========================================

	@GetMapping("/groups/list")
	public ResponseEntity<List<ItemGroup>> getGroupsList() {
		try {
			return ResponseEntity.ok(itemGroupRepository.findAll());
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/groups/save")
	public ResponseEntity<?> saveGroup(@RequestBody ItemGroup group) {
		try {
			if (group.getId() == null || group.getId() <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM ItemGroup";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				group.setId((maxId == null ? 0 : maxId) + 1);
			}
			ItemGroup saved = itemGroupRepository.save(group);
			return ResponseEntity.ok(saved);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving group: " + e.getMessage());
		}
	}

	@DeleteMapping("/groups/{id}")
	public ResponseEntity<?> deleteGroup(@PathVariable("id") Integer id) {
		try {
			itemGroupRepository.deleteById(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Group deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting group: " + e.getMessage());
		}
	}

	// ==========================================
	// 5. WAREHOUSES ENDPOINTS
	// ==========================================

	@GetMapping("/warehouses/list")
	public ResponseEntity<List<Warehouse>> getWarehousesList() {
		try {
			return ResponseEntity.ok(warehouseRepository.findAll());
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/warehouses/save")
	public ResponseEntity<?> saveWarehouse(@RequestBody Warehouse warehouse) {
		try {
			if (warehouse.getId() == null || warehouse.getId() <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM Warehouse";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				warehouse.setId((maxId == null ? 0 : maxId) + 1);
			}
			Warehouse saved = warehouseRepository.save(warehouse);
			return ResponseEntity.ok(saved);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving warehouse: " + e.getMessage());
		}
	}

	@DeleteMapping("/warehouses/{id}")
	public ResponseEntity<?> deleteWarehouse(@PathVariable("id") Integer id) {
		try {
			warehouseRepository.deleteById(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Warehouse deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting warehouse: " + e.getMessage());
		}
	}

	// ==========================================
	// 6. BRANDS ENDPOINTS
	// ==========================================

	@GetMapping("/brands/list")
	public ResponseEntity<List<Brand>> getBrandsList() {
		try {
			return ResponseEntity.ok(brandRepository.findAll());
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/brands/save")
	public ResponseEntity<?> saveBrand(@RequestBody Brand brand) {
		try {
			if (brand.getId() == null || brand.getId() <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM Brand";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				brand.setId((maxId == null ? 0 : maxId) + 1);
			}
			Brand saved = brandRepository.save(brand);
			return ResponseEntity.ok(saved);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving brand: " + e.getMessage());
		}
	}

	@DeleteMapping("/brands/{id}")
	public ResponseEntity<?> deleteBrand(@PathVariable("id") Integer id) {
		try {
			brandRepository.deleteById(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Brand deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting brand: " + e.getMessage());
		}
	}

	// ==========================================
	// 7. PRODUCT TYPES ENDPOINTS
	// ==========================================

	@GetMapping("/product-types/list")
	public ResponseEntity<List<ProductType>> getProductTypesList() {
		try {
			return ResponseEntity.ok(productTypeRepository.findAll());
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/product-types/save")
	public ResponseEntity<?> saveProductType(@RequestBody ProductType productType) {
		try {
			if (productType.getId() == null || productType.getId() <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM ProductType";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				productType.setId((maxId == null ? 0 : maxId) + 1);
			}
			ProductType saved = productTypeRepository.save(productType);
			return ResponseEntity.ok(saved);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving product type: " + e.getMessage());
		}
	}

	@DeleteMapping("/product-types/{id}")
	public ResponseEntity<?> deleteProductType(@PathVariable("id") Integer id) {
		try {
			productTypeRepository.deleteById(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Product Type deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting product type: " + e.getMessage());
		}
	}

	// ==========================================
	// 8. RACKS ENDPOINTS
	// ==========================================

	@GetMapping("/racks/list")
	public ResponseEntity<List<Map<String, Object>>> getRacksList() {
		String sql = "SELECT r.Id as id, r.RackCode as rackCode, r.RackName as rackName, " +
				"r.WarehouseId as warehouseId, w.WarehouseName as warehouseName, " +
				"r.RackStatus as rackStatus " +
				"FROM Rack r " +
				"LEFT JOIN Warehouse w ON r.WarehouseId = w.Id " +
				"ORDER BY r.Id DESC";
		try {
			return ResponseEntity.ok(jdbcTemplate.queryForList(sql));
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/racks/save")
	public ResponseEntity<?> saveRack(@RequestBody Rack rack) {
		try {
			if (rack.getId() == null || rack.getId() <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM Rack";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				rack.setId((maxId == null ? 0 : maxId) + 1);
			}
			Rack saved = rackRepository.save(rack);
			return ResponseEntity.ok(saved);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving rack: " + e.getMessage());
		}
	}

	@DeleteMapping("/racks/{id}")
	public ResponseEntity<?> deleteRack(@PathVariable("id") Integer id) {
		try {
			rackRepository.deleteById(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Rack deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting rack: " + e.getMessage());
		}
	}

	// ==========================================
	// 9. LOTS ENDPOINTS
	// ==========================================

	@GetMapping("/lots/list")
	public ResponseEntity<List<Map<String, Object>>> getLotsList() {
		String sql = "SELECT l.Id as id, l.LotNo as lotNo, l.ItemId as itemId, i.ItemName as itemName, " +
				"l.ManufactureDate as manufactureDate, l.ExpiryDate as expiryDate, " +
				"l.Quantity as quantity, l.Rate as rate, l.IsActive as isActive " +
				"FROM Lot l " +
				"LEFT JOIN Item i ON l.ItemId = i.Id " +
				"ORDER BY l.Id DESC";
		try {
			return ResponseEntity.ok(jdbcTemplate.queryForList(sql));
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/lots/save")
	public ResponseEntity<?> saveLot(@RequestBody Map<String, Object> req) {
		try {
			Integer id = req.get("id") != null ? Integer.parseInt(req.get("id").toString()) : null;
			String lotNo = (String) req.getOrDefault("lotNo", "");
			Integer itemId = req.get("itemId") != null ? Integer.parseInt(req.get("itemId").toString()) : 0;
			String manufactureDate = (String) req.getOrDefault("manufactureDate", "");
			String expiryDate = (String) req.getOrDefault("expiryDate", "");
			BigDecimal quantity = req.get("quantity") != null ? new BigDecimal(req.get("quantity").toString()) : BigDecimal.ZERO;
			BigDecimal rate = req.get("rate") != null ? new BigDecimal(req.get("rate").toString()) : BigDecimal.ZERO;
			Boolean isActive = req.get("isActive") != null ? Boolean.parseBoolean(req.get("isActive").toString()) : true;

			if (id == null || id <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM Lot";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				id = (maxId == null ? 0 : maxId) + 1;

				String insertSql = "INSERT INTO Lot (Id, LotNo, ItemId, ManufactureDate, ExpiryDate, Quantity, Rate, IsActive) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
				jdbcTemplate.update(insertSql, id, lotNo, itemId, manufactureDate, expiryDate, quantity, rate, isActive);
			} else {
				String updateSql = "UPDATE Lot SET LotNo = ?, ItemId = ?, ManufactureDate = ?, ExpiryDate = ?, Quantity = ?, Rate = ?, IsActive = ? WHERE Id = ?";
				jdbcTemplate.update(updateSql, lotNo, itemId, manufactureDate, expiryDate, quantity, rate, isActive, id);
			}

			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("id", id);
			resp.put("message", "Lot saved successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving lot: " + e.getMessage());
		}
	}

	@DeleteMapping("/lots/{id}")
	public ResponseEntity<?> deleteLot(@PathVariable("id") Integer id) {
		try {
			jdbcTemplate.update("DELETE FROM Lot WHERE Id = ?", id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Lot deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting lot: " + e.getMessage());
		}
	}

	// ==========================================
	// 10. ITEM MIN MAX RATE ENDPOINTS
	// ==========================================

	@GetMapping("/min-max/list")
	public ResponseEntity<List<Map<String, Object>>> getMinMaxList() {
		String sql = "SELECT i.Id as id, i.ItemCode as itemCode, i.ItemName as itemName, " +
				"i.MinRate as minRate, i.MaxRate as maxRate, " +
				"i.MinStockLevel as minStockLevel, i.MaxStockLevel as maxStockLevel, " +
				"i.ReorderLevel as reorderLevel " +
				"FROM Item i " +
				"ORDER BY i.ItemName ASC";
		try {
			return ResponseEntity.ok(jdbcTemplate.queryForList(sql));
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/min-max/save")
	public ResponseEntity<?> saveMinMaxBatch(@RequestBody List<Map<String, Object>> items) {
		try {
			int count = 0;
			for (Map<String, Object> item : items) {
				Integer id = item.get("id") != null ? Integer.parseInt(item.get("id").toString()) : null;
				if (id != null && id > 0) {
					Double minRate = item.get("minRate") != null ? Double.parseDouble(item.get("minRate").toString()) : 0.0;
					Double maxRate = item.get("maxRate") != null ? Double.parseDouble(item.get("maxRate").toString()) : 0.0;
					Double minStockLevel = item.get("minStockLevel") != null ? Double.parseDouble(item.get("minStockLevel").toString()) : 0.0;
					Double maxStockLevel = item.get("maxStockLevel") != null ? Double.parseDouble(item.get("maxStockLevel").toString()) : 0.0;
					Double reorderLevel = item.get("reorderLevel") != null ? Double.parseDouble(item.get("reorderLevel").toString()) : 0.0;

					String updateSql = "UPDATE Item SET MinRate = ?, MaxRate = ?, MinStockLevel = ?, MaxStockLevel = ?, ReorderLevel = ? WHERE Id = ?";
					jdbcTemplate.update(updateSql, minRate, maxRate, minStockLevel, maxStockLevel, reorderLevel, id);
					count++;
				}
			}
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("count", count);
			resp.put("message", count + " item min/max rates updated successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error updating min/max rates: " + e.getMessage());
		}
	}

	// ==========================================
	// 11. CONSUMPTION ITEMS ENDPOINTS
	// ==========================================

	@GetMapping("/consumption/list")
	public ResponseEntity<List<Map<String, Object>>> getConsumptionList() {
		String sql = "SELECT c.Id as id, c.VoucherNo as voucherNo, c.VoucherDate as voucherDate, " +
				"c.DepartmentName as departmentName, c.ItemId as itemId, i.ItemName as itemName, " +
				"c.Quantity as quantity, c.UnitName as unitName, c.Remarks as remarks " +
				"FROM ConsumptionItem c " +
				"LEFT JOIN Item i ON c.ItemId = i.Id " +
				"ORDER BY c.Id DESC";
		try {
			return ResponseEntity.ok(jdbcTemplate.queryForList(sql));
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/consumption/save")
	public ResponseEntity<?> saveConsumption(@RequestBody Map<String, Object> req) {
		try {
			Integer id = req.get("id") != null ? Integer.parseInt(req.get("id").toString()) : null;
			String voucherNo = (String) req.getOrDefault("voucherNo", "");
			String voucherDate = (String) req.getOrDefault("voucherDate", "");
			String departmentName = (String) req.getOrDefault("departmentName", "");
			Integer itemId = req.get("itemId") != null ? Integer.parseInt(req.get("itemId").toString()) : 0;
			BigDecimal quantity = req.get("quantity") != null ? new BigDecimal(req.get("quantity").toString()) : BigDecimal.ZERO;
			String unitName = (String) req.getOrDefault("unitName", "Pcs");
			String remarks = (String) req.getOrDefault("remarks", "");

			if (id == null || id <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM ConsumptionItem";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				id = (maxId == null ? 0 : maxId) + 1;

				String insertSql = "INSERT INTO ConsumptionItem (Id, VoucherNo, VoucherDate, DepartmentName, ItemId, Quantity, UnitName, Remarks, EntryDate) VALUES (?, ?, ?, ?, ?, ?, ?, ?, GETDATE())";
				jdbcTemplate.update(insertSql, id, voucherNo, voucherDate, departmentName, itemId, quantity, unitName, remarks);
			} else {
				String updateSql = "UPDATE ConsumptionItem SET VoucherNo = ?, VoucherDate = ?, DepartmentName = ?, ItemId = ?, Quantity = ?, UnitName = ?, Remarks = ? WHERE Id = ?";
				jdbcTemplate.update(updateSql, voucherNo, voucherDate, departmentName, itemId, quantity, unitName, remarks, id);
			}

			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("id", id);
			resp.put("message", "Consumption record saved successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving consumption: " + e.getMessage());
		}
	}

	@DeleteMapping("/consumption/{id}")
	public ResponseEntity<?> deleteConsumption(@PathVariable("id") Integer id) {
		try {
			jdbcTemplate.update("DELETE FROM ConsumptionItem WHERE Id = ?", id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Consumption record deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting consumption: " + e.getMessage());
		}
	}

	// ==========================================
	// UOM & UOM SCHEDULE ENDPOINTS
	// ==========================================

	@GetMapping("/uoms")
	public ResponseEntity<List<Map<String, Object>>> getUOMs() {
		try {
			List<Map<String, Object>> uoms = jdbcTemplate.queryForList(
					"SELECT Id as id, UOMCode as uomCode, ISNULL(Equivalent, 1) as equivalent, ISNULL(QtyEquivalent, 1) as qtyEquivalent FROM UOM ORDER BY UOMCode"
			);
			if (uoms.isEmpty()) {
				uoms = getDefaultUomList();
			}
			return ResponseEntity.ok(uoms);
		} catch (Exception e) {
			return ResponseEntity.ok(getDefaultUomList());
		}
	}

	private List<Map<String, Object>> getDefaultUomList() {
		List<Map<String, Object>> list = new ArrayList<>();
		list.add(createUomMap(1, "-- Select --", "", false, false));
		list.add(createUomMap(2, "40kg", 40, true, false));
		list.add(createUomMap(3, "70kg", 70, false, false));
		list.add(createUomMap(4, "5Kg", 5, false, false));
		list.add(createUomMap(5, "10Kg", 10, false, false));
		list.add(createUomMap(6, "20Kg", 20, false, false));
		list.add(createUomMap(7, "25Kg", 25, false, false));
		list.add(createUomMap(8, "50Kg", 50, false, false));
		return list;
	}

	private Map<String, Object> createUomMap(int id, String packUom, Object eq, boolean baseRate, boolean basePack) {
		Map<String, Object> m = new HashMap<>();
		m.put("id", id);
		m.put("packUom", packUom);
		m.put("uomCode", packUom);
		m.put("equivalent", eq);
		m.put("baseRateUom", baseRate);
		m.put("basePackUom", basePack);
		return m;
	}

	@GetMapping("/document-types")
	public ResponseEntity<List<Map<String, Object>>> getDocumentTypes() {
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(
					"SELECT Id as id, DocumentTypeDescription as documentType, DocumentTypeCode as documentTypeCode FROM DocumentType ORDER BY Id"
			);
			if (list.isEmpty()) {
				list = getDefaultDocumentTypes();
			}
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			return ResponseEntity.ok(getDefaultDocumentTypes());
		}
	}

	private List<Map<String, Object>> getDefaultDocumentTypes() {
		List<Map<String, Object>> list = new ArrayList<>();
		list.add(createDocTypeMap(1, "Sale Order", "SO"));
		list.add(createDocTypeMap(2, "Purchase Order", "PO"));
		list.add(createDocTypeMap(3, "Inventory Transfer", "IT"));
		list.add(createDocTypeMap(4, "Production Entry", "PE"));
		list.add(createDocTypeMap(5, "Cash Payment Voucher", "CPV"));
		list.add(createDocTypeMap(6, "Bank Payment Voucher", "BPV"));
		list.add(createDocTypeMap(7, "Cash Receipts Voucher", "CRV"));
		list.add(createDocTypeMap(8, "Bank Receipts Voucher", "BRV"));
		list.add(createDocTypeMap(9, "Journal Voucher", "JV"));
		list.add(createDocTypeMap(10, "Expense Voucher", "EV"));
		list.add(createDocTypeMap(11, "Party Payment Voucher", "PPV"));
		list.add(createDocTypeMap(12, "Party Receipt Voucher", "PRV"));
		return list;
	}

	private Map<String, Object> createDocTypeMap(int id, String docType, String code) {
		Map<String, Object> m = new HashMap<>();
		m.put("id", id);
		m.put("documentType", docType);
		m.put("documentTypeCode", code);
		return m;
	}

	@GetMapping("/uom-schedules")
	public ResponseEntity<List<Map<String, Object>>> getUomSchedules(@RequestParam(value = "itemId", required = false) Integer itemId) {
		try {
			String sql;
			List<Map<String, Object>> list;
			if (itemId != null && itemId > 0) {
				sql = "SELECT us.Id as id, us.ItemId as itemId, i.ItemName as itemName, u.UOMCode as uomCode, " +
						"us.Equivalent as equivalent, us.QtyEquivalent as qtyEquivalent, " +
						"us.BaseRateUom as baseRateUom, us.BasePackUom as basePackUom, us.Active as active " +
						"FROM UOMSchedule us " +
						"LEFT JOIN Item i ON us.ItemId = i.Id " +
						"LEFT JOIN UOM u ON us.ScheduleUnitId = u.Id " +
						"WHERE us.ItemId = ? ORDER BY us.Id DESC";
				list = jdbcTemplate.queryForList(sql, itemId);
			} else {
				sql = "SELECT us.Id as id, us.ItemId as itemId, i.ItemName as itemName, u.UOMCode as uomCode, " +
						"us.Equivalent as equivalent, us.QtyEquivalent as qtyEquivalent, " +
						"us.BaseRateUom as baseRateUom, us.BasePackUom as basePackUom, us.Active as active " +
						"FROM UOMSchedule us " +
						"LEFT JOIN Item i ON us.ItemId = i.Id " +
						"LEFT JOIN UOM u ON us.ScheduleUnitId = u.Id " +
						"ORDER BY us.Id DESC";
				list = jdbcTemplate.queryForList(sql);
			}
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/uom-schedules/save")
	public ResponseEntity<Map<String, Object>> saveUomSchedule(@RequestBody Map<String, Object> payload) {
		Map<String, Object> resp = new HashMap<>();
		try {
			Integer itemId = payload.get("itemId") != null ? Integer.parseInt(payload.get("itemId").toString()) : 0;
			String uomCode = payload.get("uomCode") != null ? payload.get("uomCode").toString() : "";
			Double equivalent = payload.get("equivalent") != null ? Double.parseDouble(payload.get("equivalent").toString()) : 1.0;
			Double qtyEquivalent = payload.get("qtyEquivalent") != null ? Double.parseDouble(payload.get("qtyEquivalent").toString()) : 1.0;
			Boolean baseRateUom = payload.get("baseRateUom") != null && Boolean.parseBoolean(payload.get("baseRateUom").toString());
			Boolean basePackUom = payload.get("basePackUom") != null && Boolean.parseBoolean(payload.get("basePackUom").toString());

			Integer uomId = 1;
			try {
				List<Map<String, Object>> uomRows = jdbcTemplate.queryForList("SELECT Id FROM UOM WHERE UOMCode = ?", uomCode);
				if (!uomRows.isEmpty()) {
					uomId = Integer.parseInt(uomRows.get(0).get("Id").toString());
				}
			} catch (Exception ignored) {}

			try {
				jdbcTemplate.update(
						"INSERT INTO UOMSchedule (ItemId, ScheduleUnitId, Equivalent, QtyEquivalent, BaseRateUom, BasePackUom, Active) VALUES (?, ?, ?, ?, ?, ?, 1)",
						itemId, uomId, equivalent, qtyEquivalent, baseRateUom ? 1 : 0, basePackUom ? 1 : 0
				);
			} catch (Exception ignored) {}

			resp.put("success", true);
			resp.put("message", "UOM Schedule saved successfully");
		} catch (Exception e) {
			resp.put("success", false);
			resp.put("message", e.getMessage());
		}
		return ResponseEntity.ok(resp);
	}
}
