module Client

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch

open Shared



type Model = Counter option

type Msg =
| Increment
| Decrement
| Init of Result<Counter, exn>



let init () : Model * Cmd<Msg> =
  let model = None
  let cmd =
    Cmd.ofPromise 
      (fetchAs<int> "/api/init") 
      [] 
      (Ok >> Init) 
      (Error >> Init)
  model, cmd

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
  let model' =
    match model,  msg with
    | Some x, Increment -> Some (x + 1)
    | Some x, Decrement -> Some (x - 1)
    | None, Init (Ok x) -> Some x
    | _ -> None
  model', Cmd.none

let safeComponents =
  let intersperse sep ls =
    List.foldBack (fun x -> function
      | [] -> [x]
      | xs -> x::sep::xs) ls []

  let components =
    [
      "Suave", "http://suave.io"
      "Fable", "http://fable.io"
      "Elmish", "https://fable-elmish.github.io/"
    ]
    |> List.map (fun (desc,link) -> a [ Href link ] [ str desc ] )
    |> intersperse (str ", ")
    |> span [ ]

  p [ ]
    [ strong [] [ str "SAFE Template" ]
      str " powered by: "
      components ]

let show = function
| Some x -> string x
| None -> "Loading..."

let view (model : Model) (dispatch : Msg -> unit) =
  div []
    [ h1 [] [ str "Collaborative Text Editor" ]
      p  [] [ str "Create a new document:" ]
      button [ OnClick (fun _ -> dispatch Decrement) ] [ str "Create" ]
      p  [] [ str "Open an existing document:" ]
      button [ OnClick (fun _ -> dispatch Increment) ] [ str "Join" ]
      safeComponents ]

  
#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
