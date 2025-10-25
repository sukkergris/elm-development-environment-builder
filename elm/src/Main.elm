module Main exposing (..)

import Browser
import Html exposing (Html, div, h1, p, text)
import Html.Attributes exposing (style)


-- MAIN


main : Program () Model Msg
main =
    Browser.sandbox { init = init, update = update, view = view }


-- MODEL


type alias Model =
    { message : String }


init : Model
init =
    { message = "Hello, World!" }


-- UPDATE


type Msg
    = NoOp


update : Msg -> Model -> Model
update msg model =
    case msg of
        NoOp ->
            model


-- VIEW


view : Model -> Html Msg
view model =
    div
        [ style "display" "flex"
        , style "flex-direction" "column"
        , style "align-items" "center"
        , style "justify-content" "center"
        , style "height" "100vh"
        , style "font-family" "Arial, sans-serif"
        , style "background-color" "#f0f8ff"
        ]
        [ h1
            [ style "color" "#60B5CC"
            , style "font-size" "3rem"
            , style "margin" "0"
            ]
            [ text model.message ]
        , p
            [ style "color" "#666"
            , style "font-size" "1.2rem"
            , style "margin-top" "1rem"
            ]
            [ text "Welcome to your Elm development environment!" ]
        ]
