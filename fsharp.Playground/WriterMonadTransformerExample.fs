module WriterMonadTransformerExample

open FSharpPlus
open FSharpPlus.Data
open WebGateway
open System

type Observation<'a> = { 
    Time : DateTime
    Operation : string
    Input : 'a
} with
  member this.Print() =
   sprintf "Time: %s - Op: %s" 
    (this.Time.ToString("dd/MM/yyyy HH:mm")) this.Operation

let private print (log:list<Observation<'a>>) r =
   let p =
      log 
      |> Seq.map (fun l -> l.Print())
      |> Seq.fold (+) ""
   sprintf "%s \nresult: %s" p r 

let private logFetch url =
    let u = url |> Uri |> Some
    let r = "<html>"
    Writer(r, [{ Time = DateTime.UtcNow; Operation = "fetch"; Input = url }])

let private fetcherWriter (s:string) = monad {
        let! a = logFetch s
        return a
    }
let run() =
    let w:string * Observation<_> list = 
        fetcherWriter "http://google.co.uk" 
        |> Writer.run
    match w with 
    | (r, log) -> print log r |> printfn "%s"

let private asyncFetch url =
    let u =
     match Uri.TryCreate(url, UriKind.RelativeOrAbsolute) with
     | true, uriResult -> Some uriResult
     | _ -> None
    let r = async { return "<title>async fetch</title>" }
    WriterT(r, [{ Time = DateTime.UtcNow; Operation = "asyncFetch"; Input = url }])

let private asyncWriter (s:string) = monad {
        return! asyncFetch s
    }

let asyncRun() =
    let w = 
     WriterT.run (asyncWriter "")
    match w with
    | (r,log) -> r |> Async.RunSynchronously |> print log |> printfn "%s"

//let asyncThing = async { return "thing" }
//let otherAsyncThing s = async { return s + " other thing" }

// monad transformer
//let something : WriterT<Async<string>> = 
//    //let inline cont (x:Async<_>) : WriterT<Async<_>> = liftAsync x
//    monad {
//     let! x = liftAsync asyncThing
//     //do! asyncThing
//     return x
//    }
//let (_, log) = something |> Writer.run
