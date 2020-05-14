module WriterMonadExample

open FSharpPlus
open FSharpPlus.Data

let private multiply x y =
    Writer (x * y, [sprintf "multiply: %i * %i" x y])

let writerWithLog a b = monad {
 let! result = multiply a b
 do! tell [sprintf "got result: %i" result]
 return result
}