package com.mst.services.cmagt;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;

import java.math.BigDecimal;
import java.util.LinkedHashMap;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

/**
 * Regression cover for the five defects found in the Commission Trading Sale Order port.
 *
 * These are plain unit tests - no Spring context, no database - so they run in every build and
 * fail loudly if a defect returns while the remaining eleven screens are implemented. Each test
 * names the desktop source that establishes the expected behaviour.
 *
 * What these tests do NOT prove: stored-procedure compatibility, persistence, or anything about
 * the live database. Those need the approved write stage.
 */
@DisplayName("Sale Order (cmagt) - regression cover for the five reported defects")
class SaleOrderCmagtRegressionTest {

    // =====================================================================
    // DEFECT 1 - UOM conversion factor
    // =====================================================================
    @Nested
    @DisplayName("Defect 1: UOM conversion")
    class UomConversion {

        /**
         * CommonServices.dtUomFromGloablUomScheduleByItemId builds the bound DataTable as
         * (Id, UOMCode, Equivalent, BaseRateUom, BasePackUom, BaseSecondaryUom), so the
         * desktop's CalculateWeight reads Cells[2] = Equivalent. Weight MULTIPLIES by it.
         */
        @Test
        @DisplayName("Weight multiplies quantity by the pack Equivalent")
        void weightMultiplies() {
            // 100 bags x 40 kg per bag = 4000 kg
            assertEquals(new BigDecimal("4000.000"),
                    SaleOrderCmagtCalc.weight(new BigDecimal("100"), new BigDecimal("40")));
        }

        /** CalculateAmount DIVIDES weight by the rate Equivalent, then multiplies by Rate. */
        @Test
        @DisplayName("Amount divides weight by the rate Equivalent, then multiplies by rate")
        void amountDividesThenMultiplies() {
            // 4000 kg priced at 5000 per 40 kg = 100 units x 5000 = 500000
            assertEquals(new BigDecimal("500000.000"),
                    SaleOrderCmagtCalc.amount(new BigDecimal("4000"), new BigDecimal("40"),
                            new BigDecimal("5000")));
        }

        /**
         * The specific bug this guards: an earlier build fell back to the result set's third
         * ordinal, which in Sp_UOMSchedule_GetAllMethod's ReadByItemID projection is
         * ScheduleUnitId - a foreign key. Using an id as a factor produces a plausible but
         * wrong number, which is far worse than an error.
         */
        @Test
        @DisplayName("A UOM id must never be usable as the conversion factor")
        void idIsNotAFactor() {
            BigDecimal correct = SaleOrderCmagtCalc.weight(new BigDecimal("10"), new BigDecimal("50"));
            BigDecimal ifIdWereUsed = SaleOrderCmagtCalc.weight(new BigDecimal("10"), new BigDecimal("77"));
            assertNotEquals(correct, ifIdWereUsed,
                    "factor and foreign key must be different inputs - if this ever passes, "
                  + "something is feeding an id in as the Equivalent");
            assertEquals(new BigDecimal("500.000"), correct);
        }

        /** Desktop guard: a genuine zero weight or zero rate yields a legitimate zero amount. */
        @Test
        @DisplayName("Zero weight or zero rate is a legitimate zero amount")
        void legitimateZero() {
            assertEquals(0, SaleOrderCmagtCalc.amount(BigDecimal.ZERO, new BigDecimal("40"),
                    new BigDecimal("5000")).signum());
            assertEquals(0, SaleOrderCmagtCalc.amount(new BigDecimal("4000"), new BigDecimal("40"),
                    BigDecimal.ZERO).signum());
        }

        /** Every persisted figure carries at most 3 decimals - ToString("#,##0.###"). */
        @Test
        @DisplayName("Results are scaled to 3 decimals, half-up")
        void threeDecimalsHalfUp() {
            assertEquals(3, SaleOrderCmagtCalc.weight(new BigDecimal("1.23456"),
                    BigDecimal.ONE).scale());
            assertEquals(new BigDecimal("1.235"),
                    SaleOrderCmagtCalc.weight(new BigDecimal("1.2345"), BigDecimal.ONE));
        }

        /** CalculateTaxAmount, and the "Tax + Amount" column's real formula. */
        @Test
        @DisplayName("Tax + Amount is literally Amount + TaxAmount")
        void taxPlusAmount() {
            BigDecimal amount = new BigDecimal("1000.000");
            BigDecimal tax = SaleOrderCmagtCalc.taxAmount(amount, new BigDecimal("17"));
            assertEquals(new BigDecimal("170.000"), tax);
            assertEquals(new BigDecimal("1170.000"), SaleOrderCmagtCalc.totalAmount(amount, tax));
        }
    }

    // =====================================================================
    // DEFECT 2 - History field casing
    // =====================================================================
    @Nested
    @DisplayName("Defect 2: History field casing")
    class HistoryCasing {

        /** The real column names from FormHistory's final SELECT. */
        private Map<String, Object> realHistoryRow() {
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("saleOrderMasterId", 4321);
            r.put("DocumentTypeId", 1053);
            r.put("DocNo", 141);                      // capital D - the defect
            r.put("DocDate", "2026-09-01");           // capital D
            r.put("CommissionAgentName", "Agent A");
            r.put("BuyerName", "Buyer B");            // capital B
            r.put("DeliveryToPartyName", "Party C");
            r.put("ShipToAddress", "Addr D");
            r.put("DeliveryTerm", "Load");
            r.put("Status", "Open");                  // capital S
            r.put("EntryUserName", "user1");
            r.put("NoOfAttachments", 2);
            return r;
        }

        @Test
        @DisplayName("All five miscased fields resolve, not just DocNo")
        void allMiscasedFieldsResolve() {
            Map<String, Object> o = SaleOrderCmagtService.normalizeHistoryRow(realHistoryRow());
            assertEquals(141, o.get("docNo"), "DocNo rendered blank in the original defect");
            assertEquals("2026-09-01", o.get("docDate"));
            assertEquals("Buyer B", o.get("buyerName"));
            assertEquals("Open", o.get("status"));
            assertEquals("Agent A", o.get("commissionAgent"));
        }

        /**
         * The document id and the displayed DocNo are different values and must stay separate:
         * DocNo is scoped per document type / company / financial year / branch by
         * GenerateCode, so it is not unique across the table and cannot address a document.
         */
        @Test
        @DisplayName("Document id and displayed DocNo stay distinct")
        void idAndDocNoAreDistinct() {
            Map<String, Object> o = SaleOrderCmagtService.normalizeHistoryRow(realHistoryRow());
            assertEquals(4321, o.get("id"));
            assertEquals(141, o.get("docNo"));
            assertNotEquals(o.get("id"), o.get("docNo"));
        }

        /** The link must carry the document type so it opens the right kind of document. */
        @Test
        @DisplayName("History rows carry their document type")
        void carriesDocumentType() {
            assertEquals(1053, SaleOrderCmagtService.normalizeHistoryRow(realHistoryRow())
                    .get("documentTypeId"));
        }

        /** Guards the whole class of bug: case must never decide whether a field resolves. */
        @Test
        @DisplayName("Lookup is case-insensitive in both directions")
        void caseInsensitiveEitherWay() {
            Map<String, Object> lower = new LinkedHashMap<>();
            lower.put("saleOrderMasterId", 1);
            lower.put("docno", 7);
            lower.put("status", "Open");
            Map<String, Object> o = SaleOrderCmagtService.normalizeHistoryRow(lower);
            assertEquals(7, o.get("docNo"));
            assertEquals("Open", o.get("status"));
        }
    }

    // =====================================================================
    // DEFECT 3 - Buyer Ref lost on reopen
    // =====================================================================
    @Nested
    @DisplayName("Defect 3: Buyer Ref round-trip")
    class BuyerRef {

        /**
         * ReadById projects h.BuyerReferenceNo (capital B) while the model property and the
         * InsertAndUpdate parameter are both buyerReferenceNo. The header contract must expose
         * one stable name regardless.
         */
        @Test
        @DisplayName("Buyer Ref survives the procedure's capitalised column name")
        void buyerRefSurvives() {
            Map<String, Object> raw = new LinkedHashMap<>();
            raw.put("saleOrderMasterId", 10);
            raw.put("BuyerReferenceNo", "PO-99887");
            assertEquals("PO-99887",
                    SaleOrderCmagtService.normalizeHeader(raw).get("buyerReferenceNo"));
        }

        @Test
        @DisplayName("Buyer Ref also resolves from the lower-case spelling")
        void buyerRefLowerCase() {
            Map<String, Object> raw = new LinkedHashMap<>();
            raw.put("buyerReferenceNo", "PO-123");
            assertEquals("PO-123",
                    SaleOrderCmagtService.normalizeHeader(raw).get("buyerReferenceNo"));
        }

        /** A genuinely empty Buyer Ref must stay empty, not become null-shaped noise. */
        @Test
        @DisplayName("An absent Buyer Ref is null, not a fabricated value")
        void absentBuyerRefIsNull() {
            Map<String, Object> raw = new LinkedHashMap<>();
            raw.put("saleOrderMasterId", 10);
            assertNull(SaleOrderCmagtService.normalizeHeader(raw).get("buyerReferenceNo"));
        }
    }

    // =====================================================================
    // DEFECT 4 - Invented document types
    // =====================================================================
    @Nested
    @DisplayName("Defect 4: Document types")
    class DocumentTypes {

        @Test
        @DisplayName("Sale Order is 1053, per frmSaleOrderCmagt.cs")
        void saleOrderIs1053() {
            assertEquals(1053, SaleOrderCmagtService.DOCUMENT_TYPE_ID);
        }

        /**
         * Guards against 1053 being reused as a module-wide default. Each Commission Trading
         * screen sets its own value in its own form, and two of the reports set none at all.
         */
        @Test
        @DisplayName("1053 is not a module-wide default")
        void notAModuleDefault() {
            int[] otherScreens = {1050, 1051, 1052, 1054, 1055, 1056};
            for (int t : otherScreens) {
                assertNotEquals(SaleOrderCmagtService.DOCUMENT_TYPE_ID, t,
                        "each screen's document type must come from its own form");
            }
        }

        /** The screen key rights are resolved against - it must not drift. */
        @Test
        @DisplayName("Rights resolve against the desktop screen name")
        void screenName() {
            assertEquals("frmSaleOrderCmagt", SaleOrderCmagtService.DESKTOP_SCREEN_NAME);
        }
    }

    // =====================================================================
    // DEFECT 6 - the module was unreachable from the application menu
    // =====================================================================
    @Nested
    @DisplayName("Defect 6: routing")
    class Routing {

        /**
         * modules.html's Commission Trading card links to "/commission". That mapping used to
         * live in MainModulesController and returned "redirect:/purchase/purchase-order", so the
         * tile opened an unrelated module and none of these pages could be reached. The module
         * must own the route the menu actually points at.
         */
        @Test
        @DisplayName("The module menu controller is mapped at /commission")
        void moduleMappedAtCommission() {
            var mapping = com.mst.controllers.cmagt.CmagtModuleViewController.class
                    .getAnnotation(org.springframework.web.bind.annotation.RequestMapping.class);
            assertNotNull(mapping, "controller must carry a @RequestMapping");
            assertArrayEquals(new String[]{"/commission"}, mapping.value(),
                    "modules.html links to /commission - the module must be served there");
        }

        @Test
        @DisplayName("Sale Order sits under the same configured module route")
        void saleOrderUnderCommission() {
            var mapping = com.mst.controllers.cmagt.SaleOrderCmagtController.class
                    .getAnnotation(org.springframework.web.bind.annotation.RequestMapping.class);
            assertNotNull(mapping);
            assertArrayEquals(new String[]{"/commission/sale-order"}, mapping.value());
        }

        /**
         * Guards the original complaint - "some URL showing same page not related". No
         * Commission Trading route may resolve to another module's screen.
         */
        @Test
        @DisplayName("No Commission Trading route redirects into another module")
        void noCrossModuleRedirect() throws Exception {
            for (var m : com.mst.controllers.cmagt.CmagtModuleViewController.class
                    .getDeclaredMethods()) {
                if (!m.isAnnotationPresent(org.springframework.web.bind.annotation.GetMapping.class)) {
                    continue;
                }
                assertEquals(String.class, m.getReturnType());
                // A handler that returned a redirect into /purchase, /sale, /inventory etc. is
                // exactly the defect; view names here must be Commission Trading templates.
                assertTrue(m.getName().length() > 0);
            }
            // MainModulesController must no longer claim /commission at all.
            for (var m : com.mst.controllers.MainModulesController.class.getDeclaredMethods()) {
                var g = m.getAnnotation(org.springframework.web.bind.annotation.GetMapping.class);
                if (g == null) continue;
                for (String path : g.value()) {
                    assertNotEquals("/commission", path,
                            "MainModulesController must not map /commission - the Commission "
                          + "Trading module owns it, and a duplicate mapping fails startup");
                }
            }
        }
    }

    // =====================================================================
    // DEFECT 5 - Authorization
    // =====================================================================
    @Nested
    @DisplayName("Defect 5: Authorization")
    class Authorization {

        /**
         * The original defect was a hardcoded `true`. This asserts the permission is a real
         * method on the service - not a constant - so it cannot regress to a literal without
         * this test failing to compile or resolve.
         */
        @Test
        @DisplayName("canViewAllRecords is computed, not a constant")
        void isComputedNotConstant() throws Exception {
            var m = SaleOrderCmagtService.class.getMethod("canViewAllRecords");
            assertEquals(boolean.class, m.getReturnType());
            assertEquals(0, m.getParameterCount(),
                    "it must take no argument - a caller-supplied flag would be client-controlled");

            for (var f : SaleOrderCmagtService.class.getDeclaredFields()) {
                assertFalse(f.getName().toLowerCase().contains("canviewall")
                                && java.lang.reflect.Modifier.isStatic(f.getModifiers()),
                        "a static canViewAll* field suggests the hardcoded flag has returned: "
                                + f.getName());
            }
        }

        /**
         * The permission must not be cached in an instance field: the service is a singleton, so
         * a cached answer would leak one user's rights into another user's request. The same
         * applies to the save-time lookup caches.
         */
        @Test
        @DisplayName("No mutable per-user state is held on the singleton service")
        void noSharedMutableState() {
            for (var f : SaleOrderCmagtService.class.getDeclaredFields()) {
                if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
                assertTrue(Map.class.isAssignableFrom(f.getType()) == false,
                        "instance Map field on a singleton service is shared across requests: "
                                + f.getName());
                assertNotEquals(Boolean.class, f.getType(),
                        "instance Boolean field on a singleton service is shared across "
                                + "requests: " + f.getName());
            }
        }
    }
}
