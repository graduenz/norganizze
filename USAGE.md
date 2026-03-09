## NOrganizze usage guide

### 1. Authentication

Authenticate with your Organizze email and API key using HTTP Basic auth. All examples below assume an `NOrganizzeClient` instance configured with valid credentials.

### 2. Creating an `NOrganizzeClient`

The main entry point is `NOrganizzeClient` in the `NOrganizze` namespace:

- **Using an existing `HttpClient` and credentials provider** (recommended in ASP.NET Core):

```csharp
var client = new NOrganizzeClient(
    httpClient,
    () => new Credentials("you@example.com", "your-api-key"));
```

- **Using an existing `HttpClient` with email and API key directly**:

```csharp
var client = new NOrganizzeClient(httpClient, "you@example.com", "your-api-key");
```

- **Letting `NOrganizzeClient` create its own `HttpClient`**:

```csharp
using var client = new NOrganizzeClient("you@example.com", "your-api-key");
```

All constructors accept an optional `baseUrl` parameter if you need to override the default Organizze REST v2 URL.

### 3. Per-request overrides with `RequestOptions`

Most service methods accept an optional `RequestOptions` parameter to override settings for a single call:

- **`BaseUrl`**: override the base Organizze API URL.
- **`CredentialsProvider`**: use different credentials for this call.
- **`UserAgent`**: override the default user agent header.

Example:

```csharp
var requestOptions = new RequestOptions
{
    BaseUrl = "https://api.organizze.com.br/rest/v2",
    UserAgent = "MyApp/1.0 (NOrganizze)",
};

var transactions = client.Transactions.List(
    new TransactionListOptions
    {
        StartDate = new DateTime(2025, 3, 1),
        EndDate = new DateTime(2025, 3, 31),
    },
    requestOptions);
```

### 4. Services overview

`NOrganizzeClient` exposes the following services:

- `client.Users` – access user information.
- `client.Accounts` – list, create, update, and delete bank accounts.
- `client.Categories` – manage categories.
- `client.CreditCards` – manage credit cards.
- `client.Invoices` – list and inspect credit card invoices and their payment transaction.
- `client.Transactions` – list, create, update, and delete transactions (expenses, incomes, etc.).
- `client.Transfers` – manage transfers between accounts.
- `client.Budgets` – list budgets by year or month.

Each service has synchronous and asynchronous methods following the same naming pattern.

### 5. Transactions (`client.Transactions`)

#### 5.1 Listing transactions with filters

Use `TransactionService.List` / `ListAsync` with `TransactionListOptions`:

- `StartDate` (`DateTime?`) – inclusive start date (sent as `yyyy-MM-dd`).
- `EndDate` (`DateTime?`) – inclusive end date (sent as `yyyy-MM-dd`).
- `AccountId` (`long?`) – filter by account id.

Example: list all transactions for a specific account in March 2025:

```csharp
var options = new TransactionListOptions
{
    StartDate = new DateTime(2025, 3, 1),
    EndDate = new DateTime(2025, 3, 31),
    AccountId = 12345,
};

var transactions = client.Transactions.List(options);
```

If `options` is `null`, all transactions are returned according to Organizze defaults.

#### 5.2 Getting a single transaction

```csharp
var transaction = client.Transactions.Get(123456);
```

#### 5.3 Creating a transaction

Use `TransactionCreateOptions` with the fields supported by the API:

- `Description` (`string`) – transaction description.
- `Date` (`DateTime`) – transaction date; serialized as date-only.
- `Notes` (`string`) – optional notes.
- `AmountCents` (`int?`) – amount in cents.
- `CategoryId` (`long?`) – category id.
- `AccountId` (`long?`) – account id.
- `Paid` (`bool?`) – whether the transaction is marked as paid.
- `Tags` (`List<Tag>`) – tags to associate.
- `RecurrenceAttributes` (`RecurrenceAttributes`) – recurrence configuration.
- `InstallmentsAttributes` (`InstallmentsAttributes`) – installments configuration.

##### Recurrence and installment periodicity

When using `RecurrenceAttributes` or `InstallmentsAttributes`, the `Periodicity` property should use one of the constants in `NOrganizze.Transactions.Periodicity` instead of raw string literals:

- `Periodicity.Daily` = `"daily"` (Diário)
- `Periodicity.Weekly` = `"weekly"` (Semanal)
- `Periodicity.Biweekly` = `"biweekly"` (Quinzenal)
- `Periodicity.Monthly` = `"monthly"` (Mensal)
- `Periodicity.Bimonthly` = `"bimonthly"` (Bimestral)
- `Periodicity.Trimonthly` = `"trimonthly"` (Trimestral)
- `Periodicity.Sixmonthly` = `"sixmonthly"` (Semestral)
- `Periodicity.Yearly` = `"yearly"` (Anual)

Example – monthly recurring transaction:

```csharp
var created = client.Transactions.Create(new TransactionCreateOptions
{
    Description = "Streaming subscription",
    Date = new DateTime(2025, 4, 1),
    AmountCents = 2990,
    AccountId = 12345,
    RecurrenceAttributes = new RecurrenceAttributes
    {
        Periodicity = Periodicity.Monthly,
    },
});
```

Example:

```csharp
var created = client.Transactions.Create(new TransactionCreateOptions
{
    Description = "Coffee",
    Date = new DateTime(2025, 3, 15),
    AmountCents = 1500,
    AccountId = 12345,
    Paid = true,
});
```

#### 5.4 Updating and deleting transactions

- **Update** – `TransactionUpdateOptions` mirrors the updatable fields of a transaction:

```csharp
var updated = client.Transactions.Update(123456, new TransactionUpdateOptions
{
    Description = "Coffee (updated)",
});
```

- **Delete** – use `TransactionDeleteOptions` when the API supports extra delete parameters, otherwise pass `null`:

```csharp
var deleted = client.Transactions.Delete(123456);
```

### 6. Invoices and credit cards

#### 6.1 Credit cards (`client.CreditCards`)

- `List` / `ListAsync` – list all credit cards.
- `Get` / `GetAsync` – get a single credit card by id.
- `Create` / `CreateAsync` – create a new credit card using `CreditCardCreateOptions`.
- `Update` / `UpdateAsync` – update an existing credit card with `CreditCardUpdateOptions`.
- `Delete` / `DeleteAsync` – delete a credit card by id.

#### 6.2 Invoices (`client.Invoices`)

To list invoices for a credit card, use `InvoiceListOptions`:

- `StartDate` (`DateTime?`) – inclusive start date (sent as `yyyy-MM-dd`).
- `EndDate` (`DateTime?`) – inclusive end date (sent as `yyyy-MM-dd`).

Example:

```csharp
var invoices = client.Invoices.List(
    creditCardId: 98765,
    options: new InvoiceListOptions
    {
        StartDate = new DateTime(2025, 1, 1),
        EndDate = new DateTime(2025, 12, 31),
    });
```

Other key methods:

- `Get` / `GetAsync` – get an `InvoiceDetail` for a specific invoice.
- `GetPayment` / `GetPaymentAsync` – get the `Transaction` that represents the invoice payment.

### 7. Accounts, categories, transfers, and budgets

#### 7.1 Accounts (`client.Accounts`)

- `List` / `ListAsync` – list all accounts.
- `Get` / `GetAsync` – get an account by id.
- `Create` / `CreateAsync` – create an account with `AccountCreateOptions`.
- `Update` / `UpdateAsync` – update an account with `AccountUpdateOptions`.
- `Delete` / `DeleteAsync` – delete an account by id.

##### Account types

The `Type` property in `AccountCreateOptions` and `Account.Type` should use one of the constants in `NOrganizze.Accounts.AccountType`:

- `AccountType.Checking` = `"checking"`
- `AccountType.Savings` = `"savings"`
- `AccountType.Other` = `"other"`

Example:

```csharp
var account = client.Accounts.Create(new AccountCreateOptions
{
    Name = "Main checking account",
    Type = AccountType.Checking,
});
```

#### 7.2 Categories (`client.Categories`)

- `List` / `ListAsync` – list all categories.
- `Get` / `GetAsync` – get a category by id.
- `Create` / `CreateAsync` – create a category with `CategoryCreateOptions`.
- `Update` / `UpdateAsync` – update a category with `CategoryUpdateOptions`.
- `Delete` / `DeleteAsync` – delete a category using `CategoryDeleteOptions` if the API requires additional parameters.

#### 7.3 Transfers (`client.Transfers`)

- `List` / `ListAsync` – list transfers.
- `Get` / `GetAsync` – get a transfer by id.
- `Create` / `CreateAsync` – create a transfer with `TransferCreateOptions`.
- `Update` / `UpdateAsync` – update a transfer with `TransferUpdateOptions`.
- `Delete` / `DeleteAsync` – delete a transfer by id.

#### 7.4 Budgets (`client.Budgets`)

- `List` / `ListAsync` – list all budgets.
- `ListByYear` / `ListByYearAsync` – list budgets for a specific year.
- `ListByMonth` / `ListByMonthAsync` – list budgets for a specific year and month.

The `Budget.ActivityType` property is an integer that mirrors Organizze’s own internal activity type classification. This client passes the value through from the API; the authoritative meaning for each value comes from the Organizze application and documentation.

### 8. Options types quick reference

| Type                         | Used by                                      | Key properties                                      |
|------------------------------|----------------------------------------------|-----------------------------------------------------|
| `TransactionListOptions`     | `Transactions.List` / `ListAsync`           | `StartDate`, `EndDate`, `AccountId`                |
| `TransactionCreateOptions`   | `Transactions.Create` / `CreateAsync`       | `Description`, `Date`, `AmountCents`, `AccountId`  |
| `TransactionUpdateOptions`   | `Transactions.Update` / `UpdateAsync`       | Updatable transaction fields                        |
| `TransactionDeleteOptions`   | `Transactions.Delete` / `DeleteAsync`       | Extra delete options (if required by API)          |
| `InvoiceListOptions`         | `Invoices.List` / `ListAsync`               | `StartDate`, `EndDate`                             |
| `AccountCreateOptions`       | `Accounts.Create` / `CreateAsync`           | Account creation fields                             |
| `AccountUpdateOptions`       | `Accounts.Update` / `UpdateAsync`           | Updatable account fields                            |
| `CategoryCreateOptions`      | `Categories.Create` / `CreateAsync`         | Category creation fields                            |
| `CategoryUpdateOptions`      | `Categories.Update` / `UpdateAsync`         | Updatable category fields                           |
| `CategoryDeleteOptions`      | `Categories.Delete` / `DeleteAsync`         | Extra delete options (if required by API)          |
| `CreditCardCreateOptions`    | `CreditCards.Create` / `CreateAsync`        | Credit card creation fields                         |
| `CreditCardUpdateOptions`    | `CreditCards.Update` / `UpdateAsync`        | Updatable credit card fields                        |
| `TransferCreateOptions`      | `Transfers.Create` / `CreateAsync`          | Transfer creation fields                            |
| `TransferUpdateOptions`      | `Transfers.Update` / `UpdateAsync`          | Updatable transfer fields                           |
| `RequestOptions`             | All service methods with `RequestOptions`   | `BaseUrl`, `CredentialsProvider`, `UserAgent`      |

This guide is designed so both humans and AI agents can quickly see **which options type to pass to which method** and **which properties to set** when integrating with Organizze via this library.

### 9. Catalog values reference

This section summarizes catalog-style values exposed by the client so you can confirm they match the Organizze application.

#### 9.1 Periodicity (`NOrganizze.Transactions.Periodicity`)

| Constant                    | Value        |
|----------------------------|--------------|
| `Periodicity.Daily`        | `"daily"` (Diário)       |
| `Periodicity.Weekly`       | `"weekly"` (Semanal)     |
| `Periodicity.Biweekly`     | `"biweekly"` (Quinzenal) |
| `Periodicity.Monthly`      | `"monthly"` (Mensal)      |
| `Periodicity.Bimonthly`    | `"bimonthly"` (Bimestral)|
| `Periodicity.Trimonthly`   | `"trimonthly"` (Trimestral) |
| `Periodicity.Sixmonthly`   | `"sixmonthly"` (Semestral)  |
| `Periodicity.Yearly`       | `"yearly"` (Anual)        |

Used by `RecurrenceAttributes.Periodicity` and `InstallmentsAttributes.Periodicity` on transactions.

#### 9.2 Account types (`NOrganizze.Accounts.AccountType`)

| Constant                 | Value        |
|--------------------------|--------------|
| `AccountType.Checking`   | `"checking"` |
| `AccountType.Savings`    | `"savings"`  |
| `AccountType.Other`      | `"other"`    |

Used by `AccountCreateOptions.Type` when creating accounts and `Account.Type` when reading them.

#### 9.3 Budget activity type

`Budget.ActivityType` is an `int` that represents an Organizze-defined activity type. This library does not define named constants for these values; they should be interpreted according to the Organizze application and official documentation.

