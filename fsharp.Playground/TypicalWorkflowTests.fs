module TypicalWorkflowTests

open System
open NUnit.Framework
open TypicalWorkflow
open FsUnit

[<TestFixture>]
type TestClass() =
 
 let shouldBeError result expected =
  match result with
  | Ok result -> failwithf "Expected Error but got Ok: %A" result
  | Error result -> 
   result |> should equal expected 

 let shouldNotBeOk result expected =
  match result with
  | Ok result -> failwith "Expected Error but got Ok: %A"
  | Error result -> 
   result |> should contain expected

 [<Test>]
 member this.``Request with CustomerId 0 is rejected``() =
  let request = { CustomerId = 0; Amount = -1m; Currency = ""; PaymentMethodId = 1 }
  let result = TypicalWorkflow.MonadicValidation request
  shouldBeError result DomainErrorMessage.CustomerIdRequired

 [<Test>]
 member this.``Request with Amount 0 is rejected``() =
  let request = { CustomerId = 1; Amount = 0m; Currency = ""; PaymentMethodId = 1 }
  let result = TypicalWorkflow.MonadicValidation request
  shouldBeError result DomainErrorMessage.AmountRequired

 [<Test>]
 member this.``Request with no Currency is rejected``() =
  let request = { CustomerId = 1; Amount = 1m; Currency = ""; PaymentMethodId = 1 }
  let result = TypicalWorkflow.MonadicValidation request
  shouldBeError result DomainErrorMessage.CurrencyRequired

 [<Test>]
 member this.``Request with Currency not in system is rejected``() =
  let request = { CustomerId = 1; Amount = 1m; Currency = "BTC"; PaymentMethodId = 1 }
  let result = TypicalWorkflow.MonadicValidation request
  shouldBeError result DomainErrorMessage.CurrencyNotFound

 [<Test>]
 member this.``All errors in Request are returned``() =
  let request = { CustomerId = 1; Amount = 0m; Currency = "BTC"; PaymentMethodId = 1 }
  let result = TypicalWorkflow.ApplicativeValidation request
  //result |> should equal Ok 
  shouldNotBeOk result DomainErrorMessage.AmountRequired