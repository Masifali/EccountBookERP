import os
import glob

SRC_DIR = r"d:\CShapEccorErp\src"
CSPROJ_PATH = r"d:\CShapEccorErp\src\Architecture.WinApp.csproj"
SLN_PATH = r"d:\CShapEccorErp\ArchitectureERP.sln"

cs_files = glob.glob(os.path.join(SRC_DIR, "**", "*.cs"), recursive=True)
resx_files = glob.glob(os.path.join(SRC_DIR, "resx", "*.resx"))

print(f"Total C# files to include in csproj: {len(cs_files)}")
print(f"Total resx files to include in csproj: {len(resx_files)}")

csproj_content = """<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{A1B2C3D4-E5F6-47A8-9900-112233445566}</ProjectGuid>
    <OutputType>WinExe</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>Architecture.WinApp</RootNamespace>
    <AssemblyName>Architecture.WinApp</AssemblyName>
    <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\\Debug\\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\\Release\\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Speech" />
    <Reference Include="System.Configuration" />
    <Reference Include="System.Core" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="System.Data" />
    <Reference Include="System.Deployment" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Net.Http" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
"""

decomp_dlls = glob.glob(os.path.join(r"d:\CShapEccorErp\Decompressed_DLLs", "*.dll"))
for dll in decomp_dlls:
    rel_dll = os.path.relpath(dll, os.path.dirname(CSPROJ_PATH))
    dll_name = os.path.splitext(os.path.basename(dll))[0]
    csproj_content += f'    <Reference Include="{dll_name}"><HintPath>{rel_dll}</HintPath></Reference>\n'

csproj_content += """  </ItemGroup>
  <ItemGroup>
"""

for cs in cs_files:
    rel_path = os.path.relpath(cs, os.path.dirname(CSPROJ_PATH))
    csproj_content += f'    <Compile Include="{rel_path}" />\n'

csproj_content += """  </ItemGroup>
  <ItemGroup>
"""

for resx in resx_files:
    rel_path = os.path.relpath(resx, os.path.dirname(CSPROJ_PATH))
    base_name = os.path.splitext(os.path.basename(resx))[0]
    logical_name = base_name + ".resources"
    csproj_content += f'    <EmbeddedResource Include="{rel_path}">\n'
    csproj_content += f'      <LogicalName>{logical_name}</LogicalName>\n'
    csproj_content += f'    </EmbeddedResource>\n'

csproj_content += """  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\\Microsoft.CSharp.targets" />
</Project>
"""

with open(CSPROJ_PATH, "w", encoding="utf-8") as f:
    f.write(csproj_content)

print(f"Created {CSPROJ_PATH}!")

sln_content = """
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 14
VisualStudioVersion = 14.0.25420.1
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Architecture.WinApp", "src\\Architecture.WinApp.csproj", "{A1B2C3D4-E5F6-47A8-9900-112233445566}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{A1B2C3D4-E5F6-47A8-9900-112233445566}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A1B2C3D4-E5F6-47A8-9900-112233445566}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A1B2C3D4-E5F6-47A8-9900-112233445566}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A1B2C3D4-E5F6-47A8-9900-112233445566}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
"""

with open(SLN_PATH, "w", encoding="utf-8") as f:
    f.write(sln_content)

print(f"Created {SLN_PATH}!")
