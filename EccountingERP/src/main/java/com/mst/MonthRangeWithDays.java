package com.mst;

/*
public class FontLoader {
    public static void main(String[] args) throws Exception {
      */
/*  try (InputStream is = new FileInputStream("E:/Trading/tradingsoftwarerepo/src/main/resources/fonts/JameelNooriNastaleeqKasheeda.ttf")) {
            Font customFont = Font.createFont(Font.TRUETYPE_FONT, is);
            GraphicsEnvironment ge = GraphicsEnvironment.getLocalGraphicsEnvironment();
            ge.registerFont(customFont);

            String[] fonts = ge.getAvailableFontFamilyNames();

            for (String font : fonts) {
                System.out.println(font);
            }
            System.out.println("Font loaded successfully!");
        } catch (Exception e) {
            e.printStackTrace();
        }*//*

 */
/* JasperReportsContext jasperReportsContext = DefaultJasperReportsContext.getInstance();
        String[] fontNames = JRFontUtil.getAvailableFontNames(jasperReportsContext);

        for (String font : fontNames) {
            System.out.println("Available Font: " + font);
        }*//*

 */
/*  String fontPath = "fonts/JameelNooriNastaleeq/Jameel Noori Nastaleeq.ttf";
        InputStream fontStream = FontFileCheck.class.getClassLoader().getResourceAsStream(fontPath);

        if (fontStream == null) {
            System.out.println("❌ Font file NOT found: " + fontPath);
        } else {
            System.out.println("✅ Font file loaded successfully!");
        }
    }

        System.out.println("Jameel Noori Nastaleeq font registered successfully!");
    *//*


        GraphicsEnvironment ge = GraphicsEnvironment.getLocalGraphicsEnvironment();
        for (String font : ge.getAvailableFontFamilyNames()) {
            System.out.println(font);
        }
    }

}
*/


import java.sql.Date;
import java.time.LocalDate;
import java.time.YearMonth;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class MonthRangeWithDays {
    public static void main(String[] args) {


        // Define start and end dates
        LocalDate startDate = LocalDate.of(2024, 6, 1);
        LocalDate endDate = LocalDate.of(2025, 6, 30);

        // Extract the YearMonth representations
        YearMonth startMonth = YearMonth.from(startDate);
        YearMonth endMonth = YearMonth.from(endDate);

        // List to store first day of each month
        List<LocalDate> monthDays = new ArrayList<>();

        // Loop through each month and store the first day
        YearMonth current = startMonth;
        while (!current.isAfter(endMonth)) {
            monthDays.add(current.atDay(1)); // 1st day of the month
            current = current.plusMonths(1);
        }

        // Print start and end dates
        System.out.println("Start Date: " + startDate);
        System.out.println("End Date: " + endDate);
        LocalDate localDate = LocalDate.of(2024, 6, 1);
        Date sqlDate = Date.valueOf(localDate);
        // Formatter including day
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("dd MMMM yyyy");

        // Print all months with days
        System.out.println("\nAll First Days of Months Between:");
        for (LocalDate date : monthDays) {
            System.out.println(date.format(formatter));
        }
    }

    public char firstNonRepaerting(String s) {
        Map<Character, Integer> map = new HashMap<>();
        for (char c : s.toCharArray()) {
            map.put(c, map.getOrDefault(c, 0) + 1);

        }
        Character key = map.entrySet().stream().filter(e -> e.getValue() == 1).findFirst().get().getKey();

        for (Map.Entry<Character, Integer> entry : map.entrySet()) {
            if (entry.getValue() > 1) {
                return entry.getKey();
            }
        }
        return s.charAt(0);
    }

}

