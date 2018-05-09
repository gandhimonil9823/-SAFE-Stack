module Client

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.Browser
open Fable.PowerPack.Keyboard

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

// end Fable.Import.Browser test

//Some CSS
let title =
    Style [
        Padding 25
        TextAlign "center"
        unbox ("font-size","48px")
        unbox ("margin", "100px 50px")
        unbox ("font-family","Open Sans")
        unbox ("line-height","10px")
    ]

let text =
    Style [
        Margin 20
        Padding 0
        TextAlign "center"
        unbox ("font-size","20px")
        unbox ("color","#444444")
        unbox ("font-family","Open Sans")
    ]

let credit =
    Style [
        Margin 100
        Padding 10
        TextAlign "center"
        unbox ("font-size","14px")
        unbox ("color","#999999")
        unbox ("font-family","Open Sans")
    ]

let createb =
    Style [
        Height 40
        unbox ("padding","0px 40px")
        unbox ("font-size","16px")
        unbox ("background","#357ca3")
        unbox ("color","#FFFFFF")
        unbox ("font-family","Open Sans")
    ]

let init() : Model * Cmd<_> = Some, Cmd.none

let update (msg : Msg) (model : Model) : Model * Cmd<_> =
  match msg with
  | Create -> goToCreate
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

  p [credit]
    [ strong [] [ str "SAFE" ]
      str " powered by: "
      components ]

let view (model : Model) (dispatch : Msg -> unit) =
  div [ Style [ VerticalAlign "middle"; TextAlign "center" ] ]
    [ h1 [title] [ str "Collaborative Text Editor" ]
      p  [text] [ str "Create a new document:" ]

      // buttons tested for Fable.Import.Browser functions
      // only really need a button to create a page (ommitting join.html use case)
      // button [ OnClick (fun _ -> dispatch Create) ] [ str "Create" ]

      // try form rather than button to route to page
      form [ Action "./create.html" ] [ button [createb] [ str "Create" ] ]
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