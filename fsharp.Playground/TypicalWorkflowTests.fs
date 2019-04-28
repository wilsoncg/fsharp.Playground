module TypicalWorkflowTests

open System
open NUnit.Framework
open TypicalWorkflow
open FsUnit

[<TestFixture>]
type TestClass() =
 
 [<Test>]
 member this.``Request with CustomerId 0 is rejected``() =
  let request = { CustomerId = 0; Amount = -1m; Currency = ""; PaymentMethodId = 1 }
  let result = TypicalWorkflow.requestAndTakePayment request
  
  match result with
   | Ok result -> failwithf "Expected Error but got Ok: %A" result
   | Error result -> 
    let expected = DomainErrorMessage.CustomerIdRequired
    result |> should equal expected 