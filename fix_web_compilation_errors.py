path_app = r"d:\CShapEccorErp\EccountBookERP\src\main\java\com\mst\MstApplication.java"
with open(path_app, "r", encoding="utf-8", errors="ignore") as f:
    app_code = f.read()

app_code = app_code.replace("import com.mst.kanta.SerialPortDataRead;", "// import com.mst.kanta.SerialPortDataRead;")
app_code = app_code.replace("import com.mst.repositories.ExtendedRepositoryImpl;", "// import com.mst.repositories.ExtendedRepositoryImpl;")
app_code = app_code.replace("@EnableJpaRepositories(repositoryBaseClass = ExtendedRepositoryImpl.class)", "@EnableJpaRepositories")
app_code = app_code.replace("SerialPortDataRead.getCommunicationPort();", "// SerialPortDataRead.getCommunicationPort();")

with open(path_app, "w", encoding="utf-8") as f:
    f.write(app_code)

path_cfg = r"d:\CShapEccorErp\EccountBookERP\src\main\java\com\mst\configuration\MyWebAppConfiguration.java"
with open(path_cfg, "r", encoding="utf-8", errors="ignore") as f:
    cfg_code = f.read()

cfg_code = cfg_code.replace("import com.mst.services.UserService;", "import com.mst.serviceInterface.IUserService;\nimport com.mst.services.UserServiceImpl;")
cfg_code = cfg_code.replace("public UserService userService() {\n\n        return new UserService();\n    }", "public IUserService userService() {\n\n        return new UserServiceImpl();\n    }")

with open(path_cfg, "w", encoding="utf-8") as f:
    f.write(cfg_code)

path_flt = r"d:\CShapEccorErp\EccountBookERP\src\main\java\com\mst\security\filter\ReceiptMangerAuthFilter.java"
with open(path_flt, "r", encoding="utf-8", errors="ignore") as f:
    flt_code = f.read()

flt_code = flt_code.replace("import com.mst.repositories.IUserRepository;", "import com.mst.repositories.UserRepository;")
flt_code = flt_code.replace("import com.mst.services.UserService;", "import com.mst.services.UserServiceImpl;")
flt_code = flt_code.replace("UserService userService;", "UserServiceImpl userService;")
flt_code = flt_code.replace("private IUserRepository userRepository;", "private UserRepository userRepository;")

with open(path_flt, "w", encoding="utf-8") as f:
    f.write(flt_code)

print("Fixed MstApplication.java, MyWebAppConfiguration.java, and ReceiptMangerAuthFilter.java!")
