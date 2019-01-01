// Learn more about F# at http://fsharp.org

open System
open System.Threading

[<EntryPoint>]
let main argv =
    let exit() =
        printfn "\nExiting..."
        Thread.Sleep 1000
        Environment.Exit(0)
    Console.CancelKeyPress.Add(fun _ -> exit())

    let action = fun _ ->
        printfn ""
        printfn "(Esc)ape, Ctrl-c or (q)uit."
        printfn "run (s)equence example"
        Console.ReadKey(true)
    let readKeys = Seq.initInfinite(fun _ -> action())
    let parse (x:ConsoleKeyInfo) =
        match x.Key with 
        | ConsoleKey.S -> 
            printfn "Run (s)"
            SeqExample.run
            None
        | ConsoleKey.Escape -> Some x.Key
        | ConsoleKey.Q -> Some x.Key
        | _ -> 
            printfn ""
            None
    Seq.pick parse readKeys |> ignore
    exit()

    0 // return an integer exit code