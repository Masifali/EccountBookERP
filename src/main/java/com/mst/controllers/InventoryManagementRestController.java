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

	// ==========================================
	// 2. ITEM CATEGORIES ENDPOINTS
	// ==========================================

	// ==========================================
	// 3. ITEM TYPES ENDPOINTS
	// ==========================================

	// ==========================================
	// 4. ITEM GROUPS ENDPOINTS
	// ==========================================

	// ==========================================
	// 5. WAREHOUSES ENDPOINTS
	// ==========================================

	// ==========================================
	// 6. BRANDS ENDPOINTS
	// ==========================================

	@GetMapping("/brands/list")
	public ResponseEntity<List<Brand>> getBrandsList() {
		return ResponseEntity.ok(desktopBrandService.getAll());
	}

    @Autowired private com.mst.services.BrandService desktopBrandService;

    @GetMapping("/brands/permissions")
    public Map<String,Boolean> brandPermissions(){return desktopBrandService.permissions();}

	@PostMapping("/brands/save")
	public ResponseEntity<?> saveBrand(@RequestBody Brand brand) {
		return ResponseEntity.ok(desktopBrandService.addOrUpdate(brand));
	}

	@DeleteMapping("/brands/{id}")
	public ResponseEntity<?> deleteBrand(@PathVariable("id") Integer id) {
		desktopBrandService.delete(id);
		return ResponseEntity.ok(Map.of("success",true,"message","Brand deleted successfully"));
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

	// JobLot endpoints are implemented by DesktopInventoryLotController.

	// ==========================================
	// 10. ITEM MIN MAX RATE ENDPOINTS
	// ==========================================

	// Rate schedule operations use DesktopInventoryMinMaxController.

	// ==========================================
	// 11. CONSUMPTION ITEMS ENDPOINTS
	// ==========================================

	// Consumption item allocations use DesktopInventoryConsumptionController.

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

}


