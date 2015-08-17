#r "../packages/FAKE/tools/FakeLib.dll" // include Fake lib

open Fake 
open Fake.AppVeyor
open System
open System.IO

let docker args =
    let exitCode =
        ExecProcess (fun info ->  
            info.FileName <- match tryFindFileOnPath (if isUnix then "docker" else "docker.exe") with
                                    | Some(a) -> a
                                    | None -> failwith "docker not found"
            info.Arguments <- args) TimeSpan.MaxValue

    if exitCode <> 0 then
        raise (Exception("Error running docker"))

    0 |> ignore