# Portable PDB #

.NET Core introduces a new symbol file (PDB) format - portable PDBs. Unlike traditional PDBs which are Windows-only, portable PDBs can be created and read on all platforms.


# Embeded Source Code into PDB #

You could embed source code into pdb following ways :

* Adding **EmbedAllSources** property into csproj file
```C#
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <EmbedAllSources>true</EmbedAllSources>
    [...]

```

* Adding Files explicitly via csproj
```C#
<ItemGroup>
      <EmbeddedFiles Include="@(Compile) " />
</ItemGroup>

```

# Extracting Source Code from PDB:

```
dotnet ExtractSourceCodeFromPortablePDB.dll -i <folder path of pdb files> -o <folder path where source code will be saved>
```
