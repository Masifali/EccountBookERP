package com.mst;



public class WhatsAppGroupScraper {

    public static void main(String[] args) throws Exception {
      /*  // Setup ChromeDriver
        WebDriverManager.chromedriver().setup();

        try {
            Runtime.getRuntime().exec("taskkill /F /IM chrome.exe");
            Thread.sleep(2000);
        } catch (Exception ignored) {
        }

        ChromeOptions options = new ChromeOptions();
        options.addArguments("--no-sandbox", "--disable-dev-shm-usage");
        options.addArguments("--user-data-dir=C:\\chrome-win64\\chrome-win64");
        options.addArguments("--profile-directory=Default");
        options.setBinary("C:\\chrome-win64\\chrome-win64\\chrome.exe");
        WebDriverManager.chromedriver().driverVersion("132.0.6834.110").setup();

        WebDriver driver = new ChromeDriver(options);
        Actions actions = new Actions(driver);
        try {
            driver.get("https://web.whatsapp.com/");
            System.out.println("Please scan QR code if not already logged in...");

            WebDriverWait wait = new WebDriverWait(driver, Duration.ofSeconds(20));
            // wait until search box available (user is logged in)
           *//* wait.until(ExpectedConditions.presenceOfElementLocated(
                    By.xpath("//div[@title='Search or start new chat']")));*//*
            //Thread.sleep(3000);
            // Click on group
        *//*    WebElement group = driver.findElement(By.xpath("//span[@title='Center Management IT Updates']"));
            group.click();
*//*
            //Thread.sleep(3000);
            WebElement groupHeader = wait.until(ExpectedConditions.elementToBeClickable(
                    By.xpath("//span[@dir='auto' and text()='Chaudhry traders multan ']"))
            );
            groupHeader.click();

// Wait for group info sidebar
            WebElement until = wait.until(ExpectedConditions.presenceOfElementLocated(
                    By.xpath("//div[@role='button']//span[@dir='auto']")));
            until.click();
// Get all members
            WebElement membersList = wait.until(ExpectedConditions.presenceOfElementLocated(
                    By.xpath("//div[@aria-label[contains(.,'Members list')]]")
            ));

// Get all member items

            List<WebElement> listItems = membersList.findElements(By.xpath(".//div[@role='listitem']"));
            System.out.println("Total members found: " + listItems.size());


            WebElement viewAllButton = wait.until(
                    ExpectedConditions.elementToBeClickable(
                            By.xpath("//div[contains(text(),'View all')]")
                    )
            );
            long lastHeight = 0;
            viewAllButton.click();

// Loop through each image and click
// Wait a little for the full members list to appear
            Thread.sleep(2000);

// Scrollable container (use role="dialog" then role="list")

            Set<String> numbers = new LinkedHashSet<>();

            JavascriptExecutor js = (JavascriptExecutor) driver;

// Scrollable container (VERY IMPORTANT)


// Wait a little for the full members list to appear
            Thread.sleep(2000);

// Scrollable container (use role="dialog" then role="list")
            //   WebElement dialog = driver.findElement(By.xpath("//div[@role='dialog']"));
            //   WebElement list = driver.findElement(By.xpath("//div[@role='dialog']//div[@role='list']"));

// 2️⃣ Locate the scrollable box inside the dialog
            // WebElement scrollBox = dialog.findElement(By.xpath(".//div[@style[contains(.,'height')]]"));

            int sameCount = 0;


// Set to store unique numbers
            ///  WebElement scrollableDiv = driver.findElement(By.xpath("//div[@role='dialog']//div[contains(@class,'x10wlt62')]"));

// Scroll down multiple times
          *//*  for (int i = 0; i < 50; i++) { // adjust number of scrolls
                ((JavascriptExecutor) driver).executeScript(
                        "arguments[0].scrollBy(0, 500);", scrollableDiv); // scroll by 500px each time
                Thread.sleep(500); // small delay so new contacts load
            }*//*

            //  WebElement scrollableDiv = driver.findElement(By.xpath("//div[@role='region']"));
            // This xpath may need adjustment based on WhatsApp updates
            WebElement dialog = driver.findElement(By.xpath("//div[@role='dialog']"));

// Inside the dialog, locate the div that has children with role="listitem"
            WebElement scrollableDiv = dialog.findElement(By.xpath(".//div[./div[@role='listitem']]"));


            Set<String> loadedContacts = new LinkedHashSet<>();


            while (sameCount < 5) {
                // Get all visible contacts
                //List<WebElement> contacts = scrollableDiv.findElements(By.xpath(".//div[@role='listitem']//span[@title]"));
                List<WebElement> contacts = scrollableDiv.findElements(By.xpath(
                        ".//div[@role='listitem']//span[starts-with(@title, '+92') and contains(@class,'x1iyjqo2')]"
                ));
                int before = loadedContacts.size();
                for (WebElement el : contacts) {
                    String title = el.getAttribute("title");
                    if (title != null && !title.isEmpty()) {
                        loadedContacts.add(title.trim());
                    }
                }
                int after = loadedContacts.size();

                if (before == after) sameCount++;
                else sameCount = 0;

                // Scroll down a bit
                js.executeScript("arguments[0].scrollTop += 1500;", scrollableDiv);
                Thread.sleep(1000);
            }

            System.out.println("Total contacts found: " + loadedContacts.size());
            loadedContacts.forEach(System.out::println);

            //driver.quit();


        } catch (Exception e) {
            e.printStackTrace();
        }*/
    }
}
