#r "packages/FAKE/tools/FakeLib.dll"; open Fake 
#load "build-tools/externaltools.fsx"; open Externaltools
#load "build-tools/version.fsx"; open Version
open System.IO

let version = generateVersionNumber()
let buildDir = "./out/"
let testDir  = "./test/"

RestorePackages()

trace (sprintf "##teamcity[setParameter name='VERSION' value='%s']" version)

Target "Clean" (fun _ ->
  CleanDirs [ buildDir; testDir]
)

Target "Build" (fun _ ->
  MSBuildDebug testDir "Build" (!! "TestApi.Tests/**/*.csproj") |> Log "TestBuild-Output: "
  MSBuildRelease buildDir "Build" (!! "TestApi/**/*.csproj") |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
  !! (testDir + "/*.Tests.dll") |> NUnit (fun p -> {p with DisableShadowCopy = true; OutputFile = testDir + "TestResults.xml"; Domain = NUnitDomainModel.NoDomainModel })
)

Target "Package" (fun _ ->
    docker (sprintf "build -t linux.local/testapi_linux:%s ./out" version)
    docker (sprintf "tag -f linux.local/testapi_linux:%s linux.local/testapi_linux:latest" version)
)

Target "Publish" (fun _ ->
    docker (sprintf "push linux.local/testapi_linux:%s" version)
    docker (sprintf "push linux.local/testapi_linux:latest")
)

"Clean"
   ==> "Build"
   ==> "Test"
   ==> "Package"
   ==> "Publish"

RunTargetOrDefault "Package"
