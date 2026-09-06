with open(r"d:\CShapEccorErp\EccountBookERP\pom.xml", "r", encoding="utf-8") as f:
    pom = f.read()

build_xml = """
    <build>
        <plugins>
            <plugin>
                <groupId>org.apache.maven.plugins</groupId>
                <artifactId>maven-compiler-plugin</artifactId>
                <version>3.10.1</version>
                <configuration>
                    <source>17</source>
                    <target>17</target>
                    <annotationProcessorPaths>
                        <path>
                            <groupId>org.projectlombok</groupId>
                            <artifactId>lombok</artifactId>
                            <version>1.18.30</version>
                        </path>
                    </annotationProcessorPaths>
                </configuration>
            </plugin>
            <plugin>
                <groupId>org.springframework.boot</groupId>
                <artifactId>spring-boot-maven-plugin</artifactId>
            </plugin>
        </plugins>
    </build>
</project>"""

pom = pom.replace("</project>", build_xml)

with open(r"d:\CShapEccorErp\EccountBookERP\pom.xml", "w", encoding="utf-8") as f:
    f.write(pom)

print("pom.xml updated with maven-compiler-plugin and lombok annotationProcessorPaths")
