// Learn more about F# at http://fsharp.org

open System
open System.Net

module WebGateway =
    open System.Net.Sockets

    let fetch (url:Uri) =
        async {
            use client = new System.Net.Http.HttpClient()
            let! response = client.GetAsync(url) |> Async.AwaitTask
            response.EnsureSuccessStatusCode |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return content
        }

    let dnsLookup (uri:Uri) =
        let rec isNestedSocketException (e:Exception) =
            match e with
            | :? AggregateException as ae ->
                ae.InnerException :: Seq.toList ae.InnerExceptions
                |> Seq.exists isNestedSocketException
            | :? SocketException -> true
            | _ -> false
        async {
            try
                let! _ = Dns.GetHostAddressesAsync uri.DnsSafeHost |> Async.AwaitTask
                return Some uri
            with e when isNestedSocketException e ->
                return None                
        }
        // version with Async.Catch

    let isValidUri uri =
        let kind = UriKind.RelativeOrAbsolute
        if Uri.IsWellFormedUriString(uri,kind) then 
            Some <| Uri uri
        else
            None
    let isValidRemoteHost uri =
        dnsLookup uri |> Async.RunSynchronously 

open WebGateway
[<EntryPoint>]
let main argv =
    let urls = [
        "http://www.google.co.uk" 
        "http://www.google.couk"
        "http:////www.google.couk"]
    
    let run = 
        urls |> 
        Seq.map isValidUri |> 
        Seq.choose id |>
        Seq.map isValidRemoteHost |>
        Seq.choose id |>
        Seq.map (fun f -> 
            printfn "Fetching from %s" f.OriginalString
            f) |>
        Seq.iter (fun f -> 
            let content = Async.RunSynchronously <| fetch f
            printfn "Got result: %s" content |> ignore)
    run
    //let uri = new Uri(url)
    //printfn "Fetching from %s" url    
    //let content = Async.RunSynchronously <| fetch uri 
    //printfn "Got result: %s" content
    0 // return an integer exit code
