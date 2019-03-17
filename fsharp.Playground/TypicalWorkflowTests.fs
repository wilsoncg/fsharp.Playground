module TypicalWorkflowTests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open TypicalWorkflow

[<TestClass>]
type TestClass () =

 [<TestMethod>]
 member this.Test() =
  let request = { CustomerId = -1; Amount = -1m; Currency = ""; PaymentMethodId = 1 }
  //let expected (s:Result<Request,DomainErrorMessage>) = Error request DomainErrorMessage.CustomerIdRequired
  let actual = TypicalWorkflow.requestAndTakePayment request
  Assert.AreEqual(Result.Error, actual)