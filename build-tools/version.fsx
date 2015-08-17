#r "../packages/FAKE/tools/FakeLib.dll";
#r "System.Runtime.Serialization"

open Fake 
open Fake.Git
open Fake.EnvironmentHelper
open System.Runtime.Serialization
open System.Runtime.Serialization.Json
open System
open System.IO
open System.Text

[<DataContract>]
type Version = {
        [<field: DataMember(Name = "major")>]
        major:string
        [<field: DataMember(Name = "minor")>]
        minor:string
    }

type BuildVars = { branch: string; gitHash: string }

let private deserialiseVersion (s:string) =
    let json = new DataContractJsonSerializer(typeof<Version>)
    let byteArray = Encoding.UTF8.GetBytes(s)
    let stream = new MemoryStream(byteArray)
    json.ReadObject(stream) :?> Version


let private getBuildVarsLocal _ =
    let isGitRepository = Directory.Exists("./.git")
    let gitInstalled = match tryFindFileOnPath (if isUnix then "git" else "git.exe") with
                                    | Some(a) -> true
                                    | None -> false

    let gitHash = if isGitRepository && gitInstalled then Information.getCurrentSHA1(".").[0..6] else "xxxxxxx"
    let branch = if isGitRepository && gitInstalled then Information.getBranchName(".") else "unknown"

    { branch = branch; gitHash = gitHash }

let private removeBranchPrefix (branchName:string) =
    let slashIndex = branchName.LastIndexOf('/')

    if slashIndex = -1 then branchName else branchName.Substring(slashIndex + 1)

let private getBuildVarsTeamcity _ =
    if environVar "TEAMCITY_VCSROOT_BRANCH" = null then failwith "One of the required variables (TEAMCITY_VCSROOT_BRANCH) is not defined."

    let gitHash = (environVar "BUILD_VCS_NUMBER").[0..6]
    let branch = removeBranchPrefix (environVar "TEAMCITY_VCSROOT_BRANCH")

    { branch = branch; gitHash = gitHash }

let generateVersionNumber _ =
    let buildVars = if String.IsNullOrEmpty(environVar "TEAMCITY_VERSION") then getBuildVarsLocal() else getBuildVarsTeamcity()
    let buildNumber = environVarOrDefault "BUILD_NUMBER" "0"
    let version = deserialiseVersion(File.ReadAllText("./version.json"))
    let date = DateTime.Now.ToString("yyMMdd")

    [ buildVars.branch; version.major; version.minor; date; buildNumber; buildVars.gitHash] |> String.concat "."
