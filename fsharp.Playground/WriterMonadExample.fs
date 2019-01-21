// http://tomasp.net/blog/2014/update-monads/
//
// Monads (with pictures)
// http://adit.io/posts/2013-06-10-three-useful-monads.html#the-writer-monad
// http://adit.io/posts/2013-04-17-functors,_applicatives,_and_monads_in_pictures.html
//
// An example implementation of a writer monad
// http://codebetter.com/matthewpodwysocki/2010/02/02/a-kick-in-the-monads-writer-edition/
//
// API reference
// https://fsprojects.github.io/FSharpPlus/reference/fsharpplus-data-writer-2.html

module WriterMonadExample

open FSharpPlus
open FSharpPlus.Data
open WebGateway
open System

let private logNumber x =
    Writer (x, ["Got number: " + (x |> string)])

let writerWithLog a b = 
    monad {
     let! a' = logNumber a
     let! b' = logNumber b
     do! tell ["multiplied both numbers"]
     return (a'*b')
    }

// >>= 'bind'

let private logFetch x =
    //let fetchPromise = fetch (x.url |> Uri |> Some)
    let u = x.url |> Uri |> Some
    Writer (u, ["Fetching url: " + (x.url |> string)])

let asyncThing = async { return String.Empty }

let asyncWriter url = 
    let inline cont (x:Async<_>) : WriterT<Async<_>> = liftAsync x
    monad {
     let! c = logFetch url
     do! cont <| asyncThing
     //do! asyncThing
     return ()
    }