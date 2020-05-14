module WriterMonadTransformerExample

open FSharpPlus
open FSharpPlus.Data
open WebGateway
open System

type Observation<'a> = { 
    Time : DateTime
    Operation : string
    Input : 'a
}
let private logFetch url =
    let u = url |> Uri |> Some
    let r = "html"
    Writer(r, [{ Time = DateTime.UtcNow; Operation = "fetch"; Input = url }])

let private fetcherWriter (s:string) = monad {
        let! a = logFetch s
        return a
    }
let run =
    let w:string * Observation<_> list = 
        fetcherWriter "http://google.co.uk" 
        |> Writer.run
    match w with 
    | (r, log) -> 
        log 
        |> Seq.map (fun l -> 
            sprintf "Time: %s - Op: %s - Result: %s" 
             (l.Time.ToString("dd/MM/yyyy HH:MM")) l.Operation r)
        |> Seq.iter (fun s -> printfn "%s" s)

// log time & message
// let private log x =
//     WriterT(x, ["Got: " + (x |> string)])

// let asyncWriterWithLog a b = monad {
//  let! a' = log a
//  let! b' = log b
//  do! tell ["multiplied both numbers"]
//  return (a'*b')
// }

// >>= 'bind'

// let private logFetch x =
//     //let fetchPromise = fetch (x.url |> Uri |> Some)
//     let u = x.url |> Uri |> Some
//     WriterT(u, ["Fetching url: " + (x.url |> string)])

let asyncThing = async { return "thing" }
let otherAsyncThing s = async { return s + " other thing" }

// monad transformer
//let something : WriterT<Async<string>> = 
//    //let inline cont (x:Async<_>) : WriterT<Async<_>> = liftAsync x
//    monad {
//     let! x = liftAsync asyncThing
//     //do! asyncThing
//     return x
//    }
//let (_, log) = something |> Writer.run
