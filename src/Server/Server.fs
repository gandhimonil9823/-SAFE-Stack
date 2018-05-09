module Server

open System.IO
open System.Net

open Suave
open Suave.Operators
open Suave.Filters
open Suave.Successful
open Suave.ServerErrors

let clientPath = Path.Combine("..","Client") |> Path.GetFullPath 
let port = 8085us

let config =
  { defaultConfig with 
      // homeFolder = Some clientPath // testing Suave.Files paths
      bindings = [ HttpBinding.create HTTP (IPAddress.Parse "0.0.0.0") port ] }

let start : WebPart = 
  //Filters.path "/api/init" >=> // not necessary?
  fun x ->
    async {
      return! OK "" x
    }

let app : WebPart =
  choose
    // not sure if app is properly catching urls
    // currently routing via React elements
    [ GET >=> path "/create" >=> OK "Hello World"      
      GET >=> path "/" >=> SERVICE_UNAVAILABLE "" 
      RequestErrors.NOT_FOUND "Page not found"
    ]      

startWebServer config app