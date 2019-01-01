module WebGateway    
    open System
    open System.Net
    open System.Net.Sockets
    open System.Net.Http

    type httpClient() = inherit HttpClient()

    // possibly use 'using' keyword to pass in IDisposable httpClient
    // timeout results in TaskCancelled exception which is not handled correctly
    let fetch (url:Uri Option) =
        match url with
        | None -> async { return String.Empty }
        | Some u -> 
        async {
            use client = new httpClient()
            let! response = client.GetAsync(u) |> Async.AwaitTask
            response.EnsureSuccessStatusCode |> ignore
            return! response.Content.ReadAsStringAsync() |> Async.AwaitTask
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
        match Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute) with
        | true -> Some <| Uri uri
        | false -> None
    let isValidRemoteHost uri =
        dnsLookup uri