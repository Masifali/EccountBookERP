package com.mst;

import net.sourceforge.tess4j.ITesseract;
import net.sourceforge.tess4j.Tesseract;
import net.sourceforge.tess4j.TesseractException;

import java.io.File;
import java.util.Arrays;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class MobileNumberExtractorService {

    public static void main(String[] args) {
        // Set TESSDATA_PREFIX correctly
        System.setProperty("TESSDATA_PREFIX", "C:\\Program Files\\Tesseract-OCR\\");
        List<String> files = Arrays.asList("A", "B", "C");
        String QRY_FETCH_ALL_PROCESSED_FILE_IF_EXIST =
                "SELECT file_name FROM settlement_files " +
                        "WHERE (status IN ('I','P','F') OR (status = 'H' AND next_retry_at > NOW())) " +
                        "AND file_name IN (%s)";
        String placeholders = files.stream().map(f -> "?").collect(Collectors.joining(","));
        String query = String.format(QRY_FETCH_ALL_PROCESSED_FILE_IF_EXIST, placeholders);
        // Path to the image file
        File imageFile = new File("E:\\chuniaMandi.jpg");

        // Set the path to the tessdata directory
        String tessDataPath = "C:\\Program Files\\Tesseract-OCR\\tessdata";

        // Debugging output to verify paths
        System.out.println("TESSDATA_PREFIX: " + System.getProperty("TESSDATA_PREFIX"));
        System.out.println("Tessdata path: " + tessDataPath);

        ITesseract instance = new Tesseract();
        instance.setLanguage("eng");  // Set the language for OCR
        instance.setDatapath(tessDataPath); // Set the tessdata directory path
        instance.setTessVariable("user_defined_dpi", "300");
        try {
            String mobileNumberPattern = "\\b(03\\d{2}-\\d{7}|03\\d{9})\\b";
            // Perform OCR on the image
            String result = instance.doOCR(imageFile);
            Pattern pattern = Pattern.compile(mobileNumberPattern);
            Matcher matcher = pattern.matcher(result);

            System.out.println("Extracted Mobile Numbers:");
            while (matcher.find()) {
                System.out.println(matcher.group());
            }
            System.out.println(result);
        } catch (TesseractException e) {
            System.err.println("Error during OCR: " + e.getMessage());
        }
    }
}
