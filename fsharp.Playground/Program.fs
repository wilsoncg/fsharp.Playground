// Learn more about F# at http://fsharp.org

open System

module WebGateway =
    let fetch (url:Uri) =
        async {
            use client = new System.Net.Http.HttpClient()
            let! response = client.GetAsync(url) |> Async.AwaitTask
            response.EnsureSuccessStatusCode |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return content
        }

open WebGateway
[<EntryPoint>]
let main argv =
    let urls = [
        "http://www.google.co.uk" 
        "http://www.google.couk"
        "http:////www.google.couk"]
    urls |> 
        Seq.map Uri |> 
        Seq.map (fun f -> 
            printfn "Fetching from %s" f.OriginalString
            f) |>
        Seq.iter (fun f -> 
            let content = Async.RunSynchronously <| fetch f
            printfn "Got result: %s"|> ignore)
    
    //let uri = new Uri(url)
    //printfn "Fetching from %s" url    
    //let content = Async.RunSynchronously <| fetch uri 
    //printfn "Got result: %s" content
    0 // return an integer exit code
