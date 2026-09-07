package com.mst;


import com.mst.repositories.ExtendedRepositoryImpl;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.web.servlet.support.SpringBootServletInitializer;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.transaction.annotation.EnableTransactionManagement;

@SpringBootApplication
@EnableTransactionManagement

@EnableJpaRepositories(repositoryBaseClass = ExtendedRepositoryImpl.class)
//@EnableJpaAuditing
//EnableJpaRepositories(repositoryBaseClass = ExtendedRepositoryImpl.class, repositoryFactoryBeanClass = EnversRevisionRepositoryImpl.class)
public class MstApplication extends SpringBootServletInitializer {

    private final Object res1 = new Object();
    private final Object res2 = new Object();

    public static void main(String[] args) {


/*        List<String> words = Arrays.asList("Apple", "Banana", "apricot", "Orange", "Avocado", "Apple");
        words.stream().distinct().filter(a -> a.startsWith("A")).forEach(s -> System.out.println(s.toUpperCase()));
        List<Integer> numbers = Arrays.asList(1, 2, 3, 4, 5);
        numbers.stream().filter(a -> a % 2 == 0).count();*/

                /*  String s = "wwww";
                  s = "asif oddd";*/
        //System.out.println("Hello Worddfkjsdkjsafklshfkljsdhfkljsfsdkjl");
        SpringApplication.run(MstApplication.class, args);
        int[] array = {1, 2, 3, 4, 2, 5, 6, 3};
        System.out.println("Hello Word");
        //new aaa().setVisible(true);
        // SerialPortDataRead.getCommunicationPort();


    }

    public void method1() {
        synchronized (res1) {
            System.out.println("Thread1 lock res1");
            try {
                Thread.sleep(50);
            } catch (Exception e) {
            }
            synchronized (res2) {
                System.out.println("Thread1 lock res2");
            }
        }
    }

    public void method2() {
        synchronized (res2) {
            System.out.println("Thread2 lock resources2");
            try {
                Thread.sleep(50);
            } catch (Exception e) {

            }
            synchronized (res1) {
                System.out.println("Thread2 locked resource1");
            }
        }
    }

   // powershell -NoProfile -Command "Get-ChildItem C:\ -File -Recurse -Force -ErrorAction SilentlyContinue | Sort-Object Length -Descending | Select-Object -First 20 @{N='SizeGB';E={[math]::Round($_.Length/1GB,2)}},FullName | Format-Table -AutoSize"
/*
    SELECT e.name ,e.Salary ,m.salary,m.name from maintable_4d4mp e JOIN maintable_4d4mp m ON e.id = m.ManagerID WHERE e.Salary>m.salary



    SELECT e.*
    FROM employees e
    WHERE (
                    SELECT COUNT(*)
    FROM employees e2
    WHERE e2.department_id = e.department_id AND e2.salary > e.salary
) < 2;


    Find Employees Joined in Last 30 Days
    SELECT *
    FROM employees
    WHERE join_date >= DATE_SUB(CURDATE(), INTERVAL 30 DAY);


    Track salary progression over time.
            SELECT

    emp_id, m.  salary,
    SUM(salary) OVER (ORDER BY hire_date) AS running_total
    FROM employees;


. Find Employees With the Same Salary

    SELECT salary, GROUP_CONCAT(name) AS Employees
    FROM employees
    GROUP BY salary
    HAVING COUNT(*) > 1;



    Show Departments With No Employees
    SELECT d.name
    FROM departments d
    LEFT JOIN employees e ON d.id = e.department_id
    WHERE e.id IS NULL;
*/



   /* @Override
    protected SpringApplicationBuilder configure(SpringApplicationBuilder application) {
        return application.sources(MstApplication.class);
    }*/
//PURCHASE_ORDER_EDIT
    //SEND_WHATSAPP_SMS ,TRANSFER_ACCOUNT,PURCHASE_ORDER_VIEW(107),SHOW_PURCHASE_ORDER_RATE(108),VIEW_COMBINE_PURCHASE(109),VIEW_COMBINE_SALE(110)
    //SALE_ORDER_VIEW(112),VIEW_SALE_ORDER_PARTY(113),VIEW_SALE_ORDER_RATE(114),SALE_VOUCHER_VIEW(115),
    //System.setProperty("rxtxComm.library", "path_to_your_native_lib\\rxtxSerial.dll");
//SEND_WEIGHT_ENTRY   very important
    //  opening rate ,qty in itemDef ,item_stock_entry,purchase_rate,sale_rate,

    //unitValue ,conversionValue  in itemDef and itemstockEntry
//MORE_DETAIL
    //DELETE FROM account WHERE code !=55111011004 and code !=44111011001 and code !=33121101001 and code !=55111011003 and code != 22121021001
    // and code != 44111011001 and code !=44111011002 and code !=55111011005 and code !=55111011006 and code !=55131021001
    // and code !=55111021001 and code !=44111021001 and code !=55111021003 and code !=55131021002
}
//889649137  purana kanta
//1705409896 new Kanta
/*SELECT
        e.Name,
        e.Salary,
        COALESCE(m.Name, 'No Manager') AS ManagerName,
        CASE
        WHEN e.Salary > m.Salary THEN 'Yes'
        ELSE 'No'
        END AS PromotionOpportunity
        FROM maintable_4D4MP e
        LEFT JOIN maintable_4D4MP m
        ON e.ManagerID = m.ID
        ORDER BY
        (e.Salary - IFNULL(m.Salary, 0)) DESC;*/







/*UPDATE item_stock_entry ise
JOIN (
                SELECT
                        id,
                @prevRate := (
                        CASE
                        WHEN transaction_type = 'IN' AND balance_closing <> 0
                        THEN CAST(((open_balance * @prevRate) + (line_rate * qty_in)) / IFNULL(balance_closing,0) AS DECIMAL(10,3))
WHEN transaction_type = 'OUT'
THEN @prevRate
ELSE @prevRate
        END
        ) AS avgRate,
        open_balance,
        balance_closing,
        line_rate
FROM (
        SELECT
                id,
        transaction_type,
        qty_in,
        qty_out,
        line_rate,
        CAST(open_balance AS DECIMAL(10,2)) AS open_balance,
CAST((open_balance + (qty_in - qty_out)) AS DECIMAL(10,2)) AS balance_closing
FROM (
        SELECT
                ise.id,
        ise.transaction_type,
        CASE WHEN ise.transaction_type = 'IN'  THEN ise.item_quantity ELSE 0 END AS qty_in,
        CASE WHEN ise.transaction_type = 'OUT' THEN ise.item_quantity ELSE 0 END AS qty_out,
        CASE
                WHEN ise.item_quantity = 0 OR ise.item_quantity IS NULL
                THEN 0
                ELSE CAST((ise.item_quantity * (ise.price + IFNULL(ise.exp, 0))) / ise.item_quantity AS DECIMAL(18,3))
END AS line_rate,
@opbalance := @opbalance + IFNULL(@prev, 0) AS open_balance,
@prev := (
CASE
WHEN ise.transaction_type = 'IN'  THEN ise.item_quantity
ELSE -ise.item_quantity
        END
                )
FROM item_stock_entry ise
JOIN (SELECT @prev := 0, @opbalance := 0) vars
WHERE ise.item_def_id = :itemDefId
ORDER BY ise.id
        ) t
    ) x
JOIN (SELECT @prevRate := 0) init
ORDER BY id
) calc
ON ise.id = calc.id
SET ise.opening_rate = CASE
WHEN calc.avgRate IS NULL THEN 0
ELSE CAST(calc.avgRate AS DECIMAL(18,3))
END;*/
