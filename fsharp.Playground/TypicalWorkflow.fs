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

let failCheck a f (error:DomainErrorMessage) =
 if f then
    Ok a    
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

let validate r =
  r 
  |> Result.bind isCustomerProvided
  |> Result.bind isPaymentMethodProvided

let requestAndTakePayment (r:Request) =
 validate (Ok r)
