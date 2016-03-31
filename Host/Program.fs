// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Suave
open Suave.Filters

[<EntryPoint>]
let main argv = 
    let webpart = pathScan "/api/profile/%s" ApiGateway.getProfile
    startWebServer defaultConfig webpart
    0 // return an integer exit code
