package com.mst;

public class JasperFontCheck {
    public static void main(String[] args) {
        try {
            System.out.println("Reading numbers from extract.py...");
            
            // Run the python script to extract numbers
            Process p = Runtime.getRuntime().exec("python extract.py");
            
            // Read standard error to capture Python errors
            java.io.BufferedReader stdError = new java.io.BufferedReader(new java.io.InputStreamReader(p.getErrorStream()));
            String s = null;
            boolean hasPythonError = false;
            while ((s = stdError.readLine()) != null) {
                if (!hasPythonError) {
                    System.out.println("Python Extraction Error:");
                    hasPythonError = true;
                }
                System.out.println(s);
            }
            p.waitFor();
            
            if (hasPythonError) {
                System.out.println("\nFailed to extract numbers from Excel. Make sure 'pip install pandas openpyxl' has finished.");
                return;
            }
            
            java.io.File file = new java.io.File("numbers.txt");
            if (!file.exists()) {
                System.out.println("Could not find numbers.txt. Ensure extract.py runs successfully.");
                return;
            }

            java.util.Scanner scanner = new java.util.Scanner(file);
            java.util.List<String> numbers = new java.util.ArrayList<>();
            while (scanner.hasNextLine()) {
                String line = scanner.nextLine().trim();
                if (!line.isEmpty()) {
                    numbers.add(line);
                }
            }
            scanner.close();

            if (numbers.isEmpty()) {
                System.out.println("No numbers found in the Excel file.");
                return;
            }

            System.out.println("Found " + numbers.size() + " numbers. Starting WhatsApp automation...");
            System.out.println("==========================================================================");
            System.out.println("PLEASE DO NOT TOUCH YOUR MOUSE OR KEYBOARD WHILE THIS SCRIPT IS RUNNING!");
            System.out.println("==========================================================================");

            String msg = "📢 *Steel Business Software – FBR Digital Invoicing Ready!*\n\n"
                    + "Complete software for Steel Traders & Businesses:\n\n"
                    + "✅ FBR Digital Invoice Integration\n"
                    + "✅ Sales & Purchase Management\n"
                    + "✅ Stock & Weight Management\n"
                    + "✅ Customer & Supplier Ledger\n"
                    + "✅ Accounts & Reports\n"
                    + "✅ Professional Invoices\n\n"
                    + "💻 Make your steel business digital and FBR-ready.\n\n"
                    + "📞 *Call/WhatsApp: 0321-7332860*\n\n"
                    + "Contact us for a *Free Demo*.";
                    
            System.out.println("Initializing Playwright...");
            com.microsoft.playwright.Playwright playwright = com.microsoft.playwright.Playwright.create();
            com.microsoft.playwright.Browser browser = playwright.chromium().launch(new com.microsoft.playwright.BrowserType.LaunchOptions().setHeadless(false).setChannel("chrome"));
            com.microsoft.playwright.BrowserContext context = browser.newContext();
            com.microsoft.playwright.Page page = context.newPage();
            
            int count = 1;
            for (String number : numbers) {
                System.out.println("Sending message to: " + number + " (" + count + "/" + numbers.size() + ")");
                
                try {
                    // Convert 923... to 03... as requested by user
                    String formattedNumber = number;
                    if (formattedNumber.startsWith("92") && formattedNumber.length() > 10) {
                        formattedNumber =   formattedNumber.substring(2);
                    }
                    
                    // This calls the complete logic from your WhatsappService, 
                    // which searches the number and creates a new contact if it doesn't exist!
                    com.mst.services.WhatsappService.controlWhatsapp(page, formattedNumber, formattedNumber, msg);
                } catch (Exception e) {
                    System.out.println("Failed to send message to " + number + ": " + e.getMessage());
                }
                
                count++;
            }
            
            System.out.println("Successfully sent all messages!");
            
            browser.close();
            playwright.close();
            
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
