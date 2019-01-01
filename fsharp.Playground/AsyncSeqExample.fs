module AsyncSeqExample

open FSharp.Control

// possible example using AsyncSeq<'a>
// https://fsprojects.github.io/FSharp.Control.AsyncSeq/library/AsyncSeq.html
let run' = asyncSeq {
    yield 1
}
run'