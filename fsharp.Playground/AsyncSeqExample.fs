module AsyncSeqExample

open System
open WebGateway
open FSharp.Control

// https://fsprojects.github.io/FSharp.Control.AsyncSeq/library/AsyncSeq.html

let getContent (page:WebPage) = 
    //fetch (Some (Uri page.url))
    page.url |> Uri |> Some |> fetch

let urls' = asyncSeq { 
    for u in urls do 
     try 
      let! c = getContent u
      let c' = if String.IsNullOrWhiteSpace(c) then None else Some c
      yield { url = u.url; content = c' }
     with _ ->
      yield { url = u.url; content = None }
}

let htmlHead (html:string) =
    let index = html.IndexOf("<head>")
    html |> Seq.skip index |> Seq.take 100 |> String.Concat

// Elements are evaluated asynchronously which could take some time,
// so use async to avoid blocking
let run = 
    async {
        for u in urls' do
        match u.content with
        | Some c -> printfn "url %s \n %s" u.url (htmlHead c)
        | None -> printfn "no content for %s" u.url
    }