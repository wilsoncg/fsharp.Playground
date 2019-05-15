// https://github.com/swlaschin/Railway-Oriented-Programming-Example/tree/master/src/FsRopExample

module TypicalWorkflow

open FSharpPlus

type Request = {
    CustomerId : int
    Amount : decimal
    Currency : string
    PaymentMethodId : int
}

// CustomerId > 0
// Amount > 0
// PaymentMethodId, Currency in valid list (retrieved at system startup)
// CustomerId exists
// PaymentMethodId is not restricted for customer

type DomainErrorMessage =
 | CustomerIdRequired
 | AmountRequired
 | CurrencyRequired
 | PaymentMethodIdRequired
 | CustomerNotFound
 | PaymentMethodNotFound
 | CurrencyNotFound
 | PaymentMethodRestrictedForCustomer

let failCheck request f (error:DomainErrorMessage) =
 if f then
    Ok request
 else
    Error (error)
    
let isPositiveInt i = i > 0
let isPositiveDecimal i = i > 0m
let isNotEmpty (s:string) = s.ToCharArray().Length > 0 
let currencyList = ["GBP";"USD"]
let currencyFound c = List.exists (fun e -> c = e) currencyList

let isCustomerProvided (r:Request) =
 failCheck r (r.CustomerId > 0) DomainErrorMessage.CustomerIdRequired
let isPaymentMethodProvided (r:Request) =
 failCheck r (r.PaymentMethodId > 0) DomainErrorMessage.PaymentMethodIdRequired
let isAmountProvided (r:Request) =
 failCheck r (r.Amount > 0m) DomainErrorMessage.AmountRequired
let isCurrencyProvided (r:Request) =
 failCheck r (r.Currency.Length > 0) DomainErrorMessage.CurrencyRequired
let isCurrencyFound (r:Request) =
 failCheck r (currencyFound r.Currency) DomainErrorMessage.CurrencyNotFound

let inline (>>=) x f = Result.bind f x

let validate result =
  result 
  |> Result.bind isCustomerProvided
  |> Result.bind isAmountProvided
  |> Result.bind isCurrencyProvided
  |> Result.bind isPaymentMethodProvided
  |> Result.bind isCurrencyFound

let MonadicValidation (r:Request) =
 validate (Ok r)

let ApplicativeValidation (r:Request) =
 let check r f (errors:List<DomainErrorMessage>) =
  if f then
   Ok r
  else 
   Error (errors)
 let amountPositive = r.Amount > 0m
 //Result.mapError 
 check r amountPositive [DomainErrorMessage.AmountRequired]