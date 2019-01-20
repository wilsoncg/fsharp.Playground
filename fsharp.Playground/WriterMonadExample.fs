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

let logNumber x =
    Writer (x, ["Got number: " + (x |> string)])

let writerWithLog a b = 
    monad {
     let! a' = logNumber a
     let! b' = logNumber b
     do! tell ["multiplied both numbers"]
     return (a'*b')
    }