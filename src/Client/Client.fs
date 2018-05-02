module Client

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.Browser

type Model = Some
type Msg =
| Create
| Join

// testing Fable.Import.Browser for url navigation
let [<Literal>] internal NavigatedEvent = "NavigatedEvent"

let goToCreate:Model * Cmd<_> =
  // use `let =` to randomize url
  Some, [fun _ -> history.pushState((), "", "/create.html") // works with refresh, doesn't maintain URL
                  let ev = document.createEvent_CustomEvent()
                  ev.initCustomEvent (NavigatedEvent, true, true, obj())
                  window.dispatchEvent ev
                  |> ignore ]

let goToJoin : Model * Cmd<_> =
  // get url from view component(text form w/ button && POST?)
  Some, [fun _ -> history.replaceState((), "", "/join")]

// end Fable.Import.Browser test

let init() : Model * Cmd<_> = Some, Cmd.none

let update (msg : Msg) (model : Model) : Model * Cmd<_> =
  match msg with
  | Create -> goToCreate
  | Join -> goToJoin
  | _ -> Some, Cmd.none

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

let view (model : Model) (dispatch : Msg -> unit) =
  div []
    [ h1 [] [ str "Collaborative Text Editor" ]
      p  [] [ str "Create a new document:" ]

      // buttons tested for Fable.Import.Browser functions
      // place after corresponding p after uncomment
      // button [ OnClick (fun _ -> dispatch Create) ] [ str "Create" ]
      // button [ OnClick (fun _ -> dispatch Join) ] [ str "Join" ]

      // try form rather than button to route to page
      form [ Action "./create.html" ] [ button [] [ str "Create" ] ]
      p  [] [ str "Open an existing document:" ]      
      form [ Action "./join.html" ] [ button [] [ str "Join" ] ]
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