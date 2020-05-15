module WriterMonadExample

open FSharpPlus
open FSharpPlus.Data

let private multiply x y =
    Writer (x * y, [sprintf "multiply: %i * %i" x y])

let private writerWithLog a b = monad {
 let! result = multiply a b
 do! tell [sprintf "got result: %i" result]
 return result
}

let run = 
 let w = 
  writerWithLog 3 5 
  |> Writer.run
 match w with
  (r, log) -> 
     printfn "log"; log |> Seq.iter (printfn "%s")