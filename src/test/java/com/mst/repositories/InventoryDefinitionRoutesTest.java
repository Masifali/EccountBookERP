package com.mst.repositories;
import com.mst.controllers.*;
import com.mst.services.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;
class InventoryDefinitionRoutesTest {
 @Test void itemAndRelatedFormsHaveDedicatedRoutes() throws Exception {
  var items=mock(DesktopInventoryItemService.class);
  var mvc=MockMvcBuilders.standaloneSetup(new DesktopInventoryItemViewController(items),
   new DesktopInventoryItemLookupController(mock(DesktopInventoryItemLookupService.class)),
   new DesktopInventoryItemGroupScheduleController(mock(DesktopInventoryItemGroupScheduleService.class)),
   new DesktopInventoryItemTaxScheduleController(mock(DesktopInventoryItemTaxScheduleService.class)),
   new DesktopInventoryItemWarehouseAllocationController(mock(DesktopInventoryItemWarehouseAllocationService.class)),
   new DesktopInventoryItemCompanyAllocationController(mock(DesktopInventoryItemCompanyAllocationService.class)),
   new DesktopInventoryItemLanguageController(mock(DesktopInventoryItemLanguageService.class)),
   new DesktopInventoryItemManufacturerController(mock(DesktopInventoryItemManufacturerService.class)),
   new DesktopInventoryLookupDefinitionController(mock(DesktopInventoryLookupDefinitionService.class))).build();
  for(String route:List.of("items","items/list","items/add"))mvc.perform(get("/inventory/"+route)).andExpect(status().isOk()).andExpect(view().name("inventory/general_item"));
  for(var route:Map.of("crop-years","crop_years","item-group-schedules","item_group_schedules","assign-item-group","assign_item_group","item-tax-schedules","item_tax_schedules","item-warehouse-allocations","item_warehouse_allocations","item-company-allocations","item_company_allocations","item-company-status","item_company_status","item-languages","item_languages","item-manufacturers","item_manufacturers","lookup-definitions","lookup_definitions").entrySet())mvc.perform(get("/inventory/"+route.getKey())).andExpect(status().isOk()).andExpect(view().name("inventory/"+route.getValue()));
  when(items.editRoute(23)).thenReturn("/inventory/items?id=23");
  when(items.editRoute(24)).thenReturn("/inventory/pos-define-item?id=24");
  mvc.perform(get("/inventory/items/edit/23")).andExpect(redirectedUrl("/inventory/items?id=23"));
  mvc.perform(get("/inventory/items/edit/24")).andExpect(redirectedUrl("/inventory/pos-define-item?id=24"));
 }
 @Test void distinctDefinitionRoutesAndDatabaseFailuresAreVisible() throws Exception {
  var category=mock(DesktopInventoryCategoryService.class);var type=mock(DesktopInventoryTypeService.class);
  var mvc=MockMvcBuilders.standaloneSetup(new DesktopInventoryCategoryController(category),new ItemTypeApiController(type),new InventoryModuleViewController()).build();
  for(String route:List.of("/inventory/item_categories","/inventory/item-categories"))mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name("inventory/item_categories"));
  for(String route:List.of("/inventory/item_types","/inventory/item-types"))mvc.perform(get(route)).andExpect(status().isOk()).andExpect(view().name("inventory/item_types"));
  mvc.perform(get("/inventory/warehouses")).andExpect(status().isOk()).andExpect(view().name("inventory/warehouses"));
  mvc.perform(get("/inventory/warehouses/edit/23")).andExpect(status().is3xxRedirection()).andExpect(redirectedUrl("/inventory/warehouses?id=23"));
  mvc.perform(get("/inventory/warehouse-racks")).andExpect(status().isOk()).andExpect(view().name("inventory/warehouse_racks"));
  mvc.perform(get("/inventory/warehouse-rack-items")).andExpect(status().isOk()).andExpect(view().name("inventory/warehouse_rack_items"));
  mvc.perform(get("/inventory/item-groups")).andExpect(status().isOk()).andExpect(view().name("inventory/item_uom_groups"));
  mvc.perform(get("/inventory/item_groups/edit/1")).andExpect(status().is3xxRedirection()).andExpect(redirectedUrl("/inventory/item-groups?id=1"));
  when(category.history()).thenReturn(List.of(Map.of("Id",23,"CategoryCode","02")));mvc.perform(get("/api/inventory/categories/list")).andExpect(jsonPath("$[0].id").value(23)).andExpect(jsonPath("$[0].categoryCode").value("02"));
  when(type.save(any())).thenThrow(new org.springframework.dao.DataIntegrityViolationException("Type write rejected"));mvc.perform(post("/api/inventory/item_types/save").contentType("application/json").content("{}" )).andExpect(status().isConflict()).andExpect(jsonPath("$.message").value("Type write rejected"));
  when(category.lookups()).thenThrow(new org.springframework.security.access.AccessDeniedException("View denied"));mvc.perform(get("/api/inventory/definition-categories/lookups")).andExpect(status().isForbidden());
 }
}
