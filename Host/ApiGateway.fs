module ApiGateway

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave
open Suave.Successful
open Suave.Operators
open Gateway
open Suave.RequestErrors

let JSON v = 
  let jsonSettings = new JsonSerializerSettings()
  jsonSettings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

  JsonConvert.SerializeObject(v, jsonSettings)
  |> OK
  >=> Writers.setMimeType "application/json; charset=utf-8"

let getProfile username (httpContext : HttpContext) =
  async {
    let! profile = getProfile username
    match profile with
    | Some p -> return! JSON p httpContext
    | None -> return! NOT_FOUND (sprintf "Username %s not found" username) httpContext
  }

