// Learn more about F# at http://fsharp.org

open System
open System.Net
open WebGateway

[<EntryPoint>]
let main argv =
    let urls = [
        "http://www.google.co.uk" 
        "http://www.google.couk"
        "http:////www.google.couk"]
        
    // https://github.com/fsharp/fsharp/blob/master/src/fsharp/FSharp.Core/seq.fs
    let asyncChoose (s:seq<_>) =
        async {
            use e = s.GetEnumerator()
            let r = ref None
            while Option.isNone(!r) && e.MoveNext() do
                let! x = e.Current
                r := x
            match !r with
            | Some z -> return z
            | None -> return failwith "no matching item found"
        }
    
    // match!
    // https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/computation-expressions#match
    //let chooser (u:Uri) =
    //    async {
    //        match! isValidRemoteHost u with
    //        | Some x -> x
    //        | None -> ...
    //    }
    let asyncMap f x = async {
        let! x' = x
        return f x'
    }
    // using seq<'a>
    let run = 
        urls 
        |> Seq.choose isValidUri 
        |> Seq.map isValidRemoteHost
        // Async<Uri option> -> Uri Option
        |> Seq.map (fun f -> f |> Async.RunSynchronously)        
        |> Seq.map fetch 
        |> Async.Parallel
        |> Async.RunSynchronously 
        |> Seq.iter (fun s -> printfn "Got result: %s" s)
    run

    // possible example using AsyncSeq<'a>
    // https://fsprojects.github.io/FSharp.Control.AsyncSeq/library/AsyncSeq.html

    0 // return an integer exit code
