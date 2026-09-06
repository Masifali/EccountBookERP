import os
import glob
import uuid

MASTER_SLN_PATH = r"d:\CShapEccorErp\ECCOUNTBOOKERP_Master_Solution.sln"

projects = [
    ("ECCOUNTBOOKERP", r"d:\CShapEccorErp\EccountBook_Original_Src\ECCOUNTBOOKERP.csproj"),
    ("Architecture.BLL", r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution\architecture.bll\architecture.bll.csproj"),
    ("Architecture.DAL", r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution\architecture.dal\architecture.dal.csproj"),
    ("Architecture.Model", r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution\architecture.model\architecture.model.csproj"),
    ("Architecture.Common", r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution\architecture.common\architecture.common.csproj"),
    ("Architecture.Reports", r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution\architecture.reports\architecture.reports.csproj"),
    ("Architecture.PSDSMS", r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution\architecture.psdsms\architecture.psdsms.csproj"),
    ("Architecture.WebCamera", r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution\architecture.webcamera\architecture.webcamera.csproj"),
    ("Architecture.WebHelper", r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution\architecture.webhelper\architecture.webhelper.csproj"),
    ("Architecture.ZKTFingerPrint", r"d:\CShapEccorErp\Full_ECCOUNTBOOKERP_Solution\architecture.zktfingerprint\architecture.zktfingerprint.csproj")
]

sln_lines = []
sln_lines.append("Microsoft Visual Studio Solution File, Format Version 12.00")
sln_lines.append("# Visual Studio Version 17")
sln_lines.append("VisualStudioVersion = 17.0.31903.59")
sln_lines.append("MinimumVisualStudioVersion = 10.0.40219.1")

proj_guids = {}

for name, path in projects:
    guid = "{" + str(uuid.uuid4()).upper() + "}"
    proj_guids[name] = guid
    rel_path = os.path.relpath(path, os.path.dirname(MASTER_SLN_PATH))
    sln_lines.append(f'Project("{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}") = "{name}", "{rel_path}", "{guid}"')
    sln_lines.append("EndProject")

sln_lines.append("Global")
sln_lines.append("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution")
sln_lines.append("\t\tDebug|Any CPU = Debug|Any CPU")
sln_lines.append("\t\tRelease|Any CPU = Release|Any CPU")
sln_lines.append("\tEndGlobalSection")
sln_lines.append("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution")

for name, guid in proj_guids.items():
    sln_lines.append(f"\t\t{guid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU")
    sln_lines.append(f"\t\t{guid}.Debug|Any CPU.Build.0 = Debug|Any CPU")
    sln_lines.append(f"\t\t{guid}.Release|Any CPU.ActiveCfg = Release|Any CPU")
    sln_lines.append(f"\t\t{guid}.Release|Any CPU.Build.0 = Release|Any CPU")

sln_lines.append("\tEndGlobalSection")
sln_lines.append("\tGlobalSection(SolutionProperties) = preSolution")
sln_lines.append("\t\tHideSolutionNode = FALSE")
sln_lines.append("\tEndGlobalSection")
sln_lines.append("EndGlobal")

with open(MASTER_SLN_PATH, "w", encoding="utf-8") as f:
    f.write("\n".join(sln_lines))

print(f"Master solution generated at {MASTER_SLN_PATH}!")
