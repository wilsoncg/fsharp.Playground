module ReaderMonadExample

open System.Net.Http
open FSharpPlus
open FSharpPlus.Data

type Env() =
    let client = new HttpClient()
    member val Client = client with get

let getData<'e when 'e :> Env> (uri:string) =
    ReaderT(fun (env: 'e) -> 
        async { 
            let! response = env.Client.GetAsync uri |> Async.AwaitTask 
            response.EnsureSuccessStatusCode |> ignore
            return! response.Content.ReadAsStringAsync() |> Async.AwaitTask
        }
    )

let init (uri:string) = monad {
    return! getData uri
}

let run = 
    ReaderT.run (init "http://news.bbc.co.uk") (Env())