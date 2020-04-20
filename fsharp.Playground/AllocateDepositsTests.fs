module AllocateDepositsTests

open NUnit.Framework
open FsUnit
open AllocateDeposits

[<TestFixture>]
type Tests() =
 
 [<Test>]
 member this.``Test order``() =
  let bankWire = { Amount = 100m; CurrencyId = 1; Payment = PaymentType.BankWire }
  let card = { Amount = 50m; CurrencyId = 1; Payment = PaymentType.Card }
  [ bankWire ; card ]
   |> Seq.sortBy (fun f -> f.Payment)
   |> Seq.item 0
   |> should equal card

