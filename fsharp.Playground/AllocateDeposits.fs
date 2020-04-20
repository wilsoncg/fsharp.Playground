module AllocateDeposits

type PaymentType =
 | ECheck = 1
 | Card = 2 
 | BankWire = 3 
 | PaymentAsia = 4
 | PayPal = 5

type NetDeposit = {
 Amount : decimal
 CurrencyId : int
 Payment : PaymentType
}

type Allocation = {
 AllocatedAmount : decimal
 AvailableAmount: decimal
 NetDeposit : decimal
}

let PerformAllocation (netDeposits:seq<NetDeposit>) =
 netDeposits
 |> Seq.sortBy (fun f -> f.Payment)  
 |> Seq.fold (fun acc element -> element)