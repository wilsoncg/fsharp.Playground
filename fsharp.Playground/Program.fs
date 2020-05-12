// Learn more about F# at http://fsharp.org

open System
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open FSharpPlus.Data
open System.Net.Http

let loop (logger:ILogger<'a>) =
 async {
    let read = 
       let print =
        sprintf "\n" +
        sprintf "(Esc)ape, Ctrl-c or (q)uit.\n" +
        sprintf "run (s)equence example\n" +
        sprintf "run (a)sync sequence example\n" +
        sprintf "run (w)riter example\n" +
        sprintf "run async w(r)iter example\n"
       logger.LogInformation(print)
       Console.ReadKey()
    let readKeys = Seq.initInfinite(fun _ -> Console.ReadKey())
    let parse (x:ConsoleKeyInfo) =
        match x.Key with 
        | ConsoleKey.A -> 
            logger.LogInformation("(A) pressed")
            None 
        | ConsoleKey.Escape ->
            logger.LogInformation("Escape pressed")
            Some x.Key
        | _ ->
            None
    Seq.pick parse readKeys |> ignore
  }

type AsyncHostedService  
 (logger: ILogger<AsyncHostedService>, 
  app: IHostApplicationLifetime,
  httpClientFactory: IHttpClientFactory) =
 let _logger = logger
 let _app = app
 let _httpClient = httpClientFactory.CreateClient()
 let _cancellationToken = _app.ApplicationStopping

 interface IHostedService with
  member this.StartAsync(ct:CancellationToken) =
   _logger.LogInformation("Starting")
   loop _logger |> Async.StartAsTask
   Task.CompletedTask

  member this.StopAsync(ct:CancellationToken) = 
   Task.CompletedTask

type NonHostedService 
 (logger: ILogger<NonHostedService>, 
  app: IHostApplicationLifetime,
  httpClientFactory: IHttpClientFactory) =
 let _logger = logger
 let _app = app
 let _httpClient = httpClientFactory.CreateClient()
 let _cancellationToken = _app.ApplicationStopping

 member this.Start() =
  _logger.LogInformation("Starting...")
  loop _logger |> Async.StartAsTask

//  interface IHostedService with

//   member this.StartAsync(ct:CancellationToken) =
//    _app.ApplicationStarted.Register(fun _ -> _logger.LogInformation("Started...")) |> ignore
//    _app.ApplicationStopping.Register(fun _ -> _logger.LogInformation("Stopping...")) |> ignore
//    _app.ApplicationStopped.Register(fun _ -> _logger.LogInformation("Stopped...")) |> ignore
//    Task.CompletedTask 

//   member this.StopAsync(ct:CancellationToken) =
//    Task.CompletedTask

[<EntryPoint>]
let main argv =
    let host = 
     Host.CreateDefaultBuilder(argv)
      .ConfigureServices(fun (ctx:HostBuilderContext) (services:IServiceCollection) -> 
        services
         .AddHttpClient()
         .AddSingleton<NonHostedService>()
         |> ignore)
       .ConfigureLogging(fun (ctx:HostBuilderContext) (logging:ILoggingBuilder) ->
        logging
         .AddConsole()
         .AddDebug()
         |> ignore)
       .UseConsoleLifetime()
      .Build()

    let time f =
        let stopwatch = Diagnostics.Stopwatch()
        stopwatch.Restart()
        f
        stopwatch.Stop()
        printfn "elapsed %6ims" stopwatch.ElapsedMilliseconds

    let read = fun _ ->
     let print =
        sprintf "\n" +
        sprintf "(Esc)ape, Ctrl-c or (q)uit." +
        sprintf "run (s)equence example" +
        sprintf "run (a)sync sequence example" +
        sprintf "run (w)riter example" +
        sprintf "run async w(r)iter example"
     Console.ReadKey(true) // console blocked while waiting for input
    let readKeys = Seq.initInfinite(fun _ -> read())
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
            | (r, log) -> 
              printfn "result %i" r
              printfn "log"
              log |> Seq.iter (printfn "%s")
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
    //Seq.pick parse readKeys |> ignore

    let s = host.Services.GetRequiredService<NonHostedService>()
    let task = host.RunAsync() |> Async.AwaitTask 
    s.Start() |> Async.AwaitTask |> Async.RunSynchronously
    //task |> Async.RunSynchronously
    0 // return an integer exit code