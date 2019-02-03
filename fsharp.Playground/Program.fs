// Learn more about F# at http://fsharp.org

open System
open System.Threading
open FSharpPlus.Data

[<EntryPoint>]
let main argv =
    let exit() =
        printfn "\nExiting..."
        Thread.Sleep 1000
        Environment.Exit(0)
    Console.CancelKeyPress.Add(fun _ -> exit())

    let stopwatch = System.Diagnostics.Stopwatch()
    let time f =
        stopwatch.Restart()
        f
        stopwatch.Stop()
        printfn "elapsed %6ims" stopwatch.ElapsedMilliseconds

    let action = fun _ ->
        printfn ""
        printfn "(Esc)ape, Ctrl-c or (q)uit."
        printfn "run (s)equence example"
        printfn "run (a)sync sequence example"
        printfn "run (w)riter example"
        printfn "run async w(r)iter example"
        Console.ReadKey(true) // console blocked while waiting for input
    let readKeys = Seq.initInfinite(fun _ -> action())
    let parse (x:ConsoleKeyInfo) =
        match x.Key with 
        | ConsoleKey.S -> 
            printfn "Run (s)"
            SeqExample.run
            None
        | ConsoleKey.A -> 
            printfn "Run (a)"
            AsyncSeqExample.run |> Async.Start
            None
        | ConsoleKey.W -> 
            printfn "Run (w)"
            let w = WriterMonadExample.writerWithLog 3 5 |> Writer.run
            match w with
            | (r, log) -> printfn "result %i" r; printfn "log"; log |> Seq.iter (printfn "%s")
            None
         | ConsoleKey.R -> 
            printfn "Run (r)"
            //let w = WriterMonadExample.asyncWriter "http://news.bbc.co.uk" |> Writer.run
            //match w with
            //| (r, log) -> printfn "result %i" r; printfn "log"; log |> Seq.iter (printfn "%s")
            None
        | ConsoleKey.Escape -> Some x.Key
        | ConsoleKey.Q -> Some x.Key
        | _ -> 
            printfn ""
            None
    Seq.pick parse readKeys |> ignore
    exit()

    0 // return an integer exit code