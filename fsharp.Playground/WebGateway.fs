module WebGateway    
    open Microsoft.FSharp.Control.WebExtensions
    open System
    open System.Net
    open System.Net.Sockets
    open System.Net.Http

    type httpClient() = inherit HttpClient()

    type WebPage = {
        url : string
        content : string Option }
    let fromUrl u = { url=u; content=None}

    let pages = [
        { url="https://www.google.co.uk"; content=None }
        { url="https://news.bbc.co.uk"; content=None }
        { url="https://www.guardian.co.uk"; content=None }
        { url="https://www.theregister.co.uk"; content=None }]

    // possibly use 'using' keyword to pass in IDisposable httpClient
    // timeout results in TaskCancelled exception which is not handled correctly
    let fetch (client:HttpClient) (url:Uri Option) =
        match url with
        | None -> async { return String.Empty }
        | Some u -> 
        async {
            let! response = 
                client.GetAsync(u) 
                |> Async.AwaitTask
            response.EnsureSuccessStatusCode |> ignore
            return! response.Content.ReadAsStringAsync() |> Async.AwaitTask
        } 
        //|> Async.Catch

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
        match Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute) with
        | true -> Some <| Uri uri
        | false -> None
    let isValidRemoteHost uri =
        dnsLookup uri