module AsyncSeqExample

open System
open WebGateway
open FSharp.Control

// https://fsprojects.github.io/FSharp.Control.AsyncSeq/library/AsyncSeq.html

type WebPage = {
    url : string
    content : string Option
}

let urls = [
    { url="https://www.google.co.uk"; content=None }
    { url="https://news.bbc.co.uk"; content=None }
    { url="https://www.guardian.co.uk"; content=None }
    { url="https://www.theregister.co.uk"; content=None }]

let getContent (page:WebPage) = 
    //fetch (Some (Uri page.url))
    page.url |> Uri |> Some |> fetch

let urls' = asyncSeq { 
    for u in urls do 
     try 
      let! c = getContent u
      yield { url = u.url; content = Some c }
     with _ ->
      yield { url = u.url; content = None }
} 

// Elements are evaluate asynchronously which could take some time,
// so use async to avoid blocking
let run = 
    async {
        for u in urls' do
        match u.content with
        | Some c -> printfn "%s" c
        | None -> printfn "no content for %s" u.url
    } |> Async.Start