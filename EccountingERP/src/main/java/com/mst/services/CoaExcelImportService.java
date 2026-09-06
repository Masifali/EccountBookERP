package com.mst.services;

import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;

import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.ss.usermodel.WorkbookFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.StringUtils;
import org.springframework.web.multipart.MultipartFile;

import com.mst.models.ChartofAccount;
import com.mst.serviceInterface.IChartofAccountService;

/**
 * Backs the "Upload Excel Sheet" toolbar button on the Chart Of Account Definition
 * screen (desktop: Templates.frmExcelSheetForCoaUpload).
 *
 * NOTE (scope for this first port): the original WinForms feature is a full template
 * *generator and* importer - it exports a formatted workbook (one sheet per parent
 * account, driven by Account Type) and re-imports it with a matching column layout.
 * That's a sizeable feature of its own. This first version supports the common case
 * instead: a flat spreadsheet with one header row and the columns below, imported
 * top-to-bottom (parents must appear before their children, since a child needs its
 * parent's accountCode to already exist).
 *
 * Expected columns (by header name, case-insensitive): ParentAccountCode,
 * AccountTitle, AccountGroup (Group/Detail), AccountTypeId, OtherErpCode, ContactNo.
 */
@Service
public class CoaExcelImportService {

	@Autowired
	private IChartofAccountService chartofAccountService;

	public record ImportResult(int inserted, int failed, List<String> errors) {
	}

	public ImportResult importFile(MultipartFile file, String defaultParentAccountCode) {
		int inserted = 0;
		int failed = 0;
		List<String> errors = new ArrayList<>();

		if (file == null || file.isEmpty()) {
			errors.add("No file uploaded.");
			return new ImportResult(0, 0, errors);
		}

		try (InputStream in = file.getInputStream(); Workbook wb = WorkbookFactory.create(in)) {
			Sheet sheet = wb.getSheetAt(0);
			Row header = sheet.getRow(sheet.getFirstRowNum());
			if (header == null) {
				errors.add("Sheet has no header row.");
				return new ImportResult(0, 0, errors);
			}

			int colParent = findColumn(header, "ParentAccountCode");
			int colTitle = findColumn(header, "AccountTitle");
			int colGroup = findColumn(header, "AccountGroup");
			int colType = findColumn(header, "AccountTypeId");
			int colOtherCode = findColumn(header, "OtherErpCode");
			int colContact = findColumn(header, "ContactNo");

			if (colTitle < 0) {
				errors.add("Missing required column: AccountTitle");
				return new ImportResult(0, 0, errors);
			}

			for (int r = sheet.getFirstRowNum() + 1; r <= sheet.getLastRowNum(); r++) {
				Row row = sheet.getRow(r);
				if (row == null) {
					continue;
				}
				String title = stringValue(row.getCell(colTitle));
				if (!StringUtils.hasText(title)) {
					continue;
				}
				try {
					ChartofAccount account = new ChartofAccount();
					String parentCode = colParent >= 0 ? stringValue(row.getCell(colParent)) : null;
					account.setParentAccountCode(StringUtils.hasText(parentCode) ? parentCode : defaultParentAccountCode);
					account.setAccountTitle(title);
					account.setAccountGroup(colGroup >= 0 && StringUtils.hasText(stringValue(row.getCell(colGroup)))
							? stringValue(row.getCell(colGroup))
							: "Detail");
					account.setAccountTypeId(colType >= 0 ? intValue(row.getCell(colType)) : null);
					account.setOtherErpCode(colOtherCode >= 0 ? stringValue(row.getCell(colOtherCode)) : null);
					account.setContactNo(colContact >= 0 ? stringValue(row.getCell(colContact)) : null);
					chartofAccountService.save(account);
					inserted++;
				} catch (Exception ex) {
					failed++;
					errors.add("Row " + (r + 1) + " (" + title + "): " + ex.getMessage());
				}
			}
		} catch (Exception ex) {
			errors.add("Could not read file: " + ex.getMessage());
		}

		return new ImportResult(inserted, failed, errors);
	}

	private int findColumn(Row header, String name) {
		for (Cell cell : header) {
			if (name.equalsIgnoreCase(stringValue(cell).trim())) {
				return cell.getColumnIndex();
			}
		}
		return -1;
	}

	private String stringValue(Cell cell) {
		if (cell == null) {
			return "";
		}
		return switch (cell.getCellType()) {
			case STRING -> cell.getStringCellValue();
			case NUMERIC -> String.valueOf((long) cell.getNumericCellValue());
			case BOOLEAN -> String.valueOf(cell.getBooleanCellValue());
			default -> cell.toString();
		};
	}

	private Integer intValue(Cell cell) {
		String v = stringValue(cell).trim();
		if (!StringUtils.hasText(v)) {
			return null;
		}
		try {
			return (int) Double.parseDouble(v);
		} catch (NumberFormatException e) {
			return null;
		}
	}
}
