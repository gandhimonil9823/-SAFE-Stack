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
      homeFolder = Some clientPath
      bindings = [ HttpBinding.create HTTP (IPAddress.Parse "0.0.0.0") port ] }

let init : WebPart = 
  Filters.path "/api/init" >=>
  fun x ->
    async {
      return! OK "" x
    }

let app : WebPart =
  choose [
    GET >=> path "/create" >=> OK "Hello World"      
    GET >=> path "/join" >=> OK "" 
    init
    Filters.path "/" >=> Files.browseFileHome "index.html"
    Files.browseHome
    RequestErrors.NOT_FOUND "Page not found"
  ]

startWebServer config app