using System.Diagnostics;
using System.Text.Json;
using ApiValidator.Models;
using NOrganizze;
using NOrganizze.Accounts;
using NOrganizze.Budgets;
using NOrganizze.Categories;
using NOrganizze.CreditCards;
using NOrganizze.Invoices;
using NOrganizze.Transactions;
using NOrganizze.Transfers;
using NOrganizze.Users;

namespace ApiValidator.Services;

public class ApiTestOrchestrator
{
    private const string GroupAccounts = "Accounts";
    private const string GroupCategories = "Categories";
    private const string GroupCreditCards = "CreditCards";
    private const string GroupTransactions = "Transactions";
    private const string GroupTransfers = "Transfers";
    private const string MethodGet = "GET";
    private const string MethodPost = "POST";
    private const string MethodPut = "PUT";
    private const string MethodDelete = "DELETE";

    private readonly NOrganizzeClient _client;
    private readonly List<EndpointResult> _results = new();

    // Test data tracking for cleanup
    private long? _testAccountId;
    private long? _testAccount2Id;
    private long? _testCategoryId;
    private long? _testCreditCardId;
    private readonly List<long> _testTransactionIds = new();
    private readonly List<long> _testTransferIds = new();

    public ApiTestOrchestrator(string email, string apiToken, string userAgent)
    {
        _client = new NOrganizzeClient(email, apiToken);
    }

    public async Task<List<EndpointResult>> ExecuteAllTestsAsync()
    {
        Console.WriteLine("Starting comprehensive API validation...\n");

        try
        {
            // Phase 1: READ-only operations (safe, no data changes)
            await TestUserEndpoints();
            await TestAccountsListAndGet();
            await TestCategoriesListAndGet();
            await TestCreditCardsListAndGet();
            await TestBudgetEndpoints();

            // Phase 2: CREATE operations
            await CreateTestData();

            // Phase 3: CREATE dependent data (requires test accounts/categories)
            await TestTransactionEndpoints();
            await TestTransferEndpoints();

            // Phase 4: UPDATE operations
            await TestUpdateOperations();

            // Phase 5: Test Invoices (requires credit card with data)
            await TestInvoiceEndpoints();

            // Phase 6: DELETE cleanup
            await CleanupTestData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Fatal error during test execution: {ex.Message}");
            Console.WriteLine("Attempting cleanup...");
            await CleanupTestData();
        }

        return _results;
    }

    private async Task TestUserEndpoints()
    {
        Console.WriteLine("📋 Testing User endpoints...");

        var userService = new UserService(_client);

        // First, we need to get our user ID from an account or category
        // The API typically returns user-specific data, so we'll use ID 1 as a placeholder
        await TestEndpoint(
            "Users",
            MethodGet,
            "/users/{id}",
            async () =>
            {
                try
                {
                    var user = await userService.GetAsync(1);
                    return (user, JsonSerializer.Serialize(user));
                }
                catch
                {
                    // If ID 1 doesn't work, we can't test this without knowing the user ID
                    throw new InvalidOperationException("Cannot determine user ID - skipping user endpoint");
                }
            }
        );
    }

    private async Task TestAccountsListAndGet()
    {
        Console.WriteLine("📋 Testing Account endpoints (read)...");

        var accountService = new AccountService(_client);

        // List accounts
        List<Account>? accounts = null;
        await TestEndpoint(
            GroupAccounts,
            MethodGet,
            "/accounts",
            async () =>
            {
                accounts = await accountService.ListAsync();
                return (accounts, JsonSerializer.Serialize(accounts));
            }
        );

        // Get specific account if any exist
        if (accounts?.Count > 0)
        {
            var accountId = accounts[0].Id;
            await TestEndpoint(
                GroupAccounts,
                MethodGet,
                $"/accounts/{accountId}",
                async () =>
                {
                    var account = await accountService.GetAsync(accountId);
                    return (account, JsonSerializer.Serialize(account));
                }
            );
        }
    }

    private async Task TestCategoriesListAndGet()
    {
        Console.WriteLine("📋 Testing Category endpoints (read)...");

        var categoryService = new CategoryService(_client);

        // List categories
        List<Category>? categories = null;
        await TestEndpoint(
            GroupCategories,
            MethodGet,
            "/categories",
            async () =>
            {
                categories = await categoryService.ListAsync();
                return (categories, JsonSerializer.Serialize(categories));
            }
        );

        // Get specific category if any exist
        if (categories?.Count > 0)
        {
            var categoryId = categories[0].Id;
            await TestEndpoint(
                GroupCategories,
                MethodGet,
                $"/categories/{categoryId}",
                async () =>
                {
                    var category = await categoryService.GetAsync(categoryId);
                    return (category, JsonSerializer.Serialize(category));
                }
            );
        }
    }

    private async Task TestCreditCardsListAndGet()
    {
        Console.WriteLine("📋 Testing CreditCard endpoints (read)...");

        var creditCardService = new CreditCardService(_client);

        // List credit cards
        List<CreditCard>? creditCards = null;
        await TestEndpoint(
            GroupCreditCards,
            MethodGet,
            "/credit_cards",
            async () =>
            {
                creditCards = await creditCardService.ListAsync();
                return (creditCards, JsonSerializer.Serialize(creditCards));
            }
        );

        // Get specific credit card if any exist
        if (creditCards?.Count > 0)
        {
            var cardId = creditCards[0].Id;
            await TestEndpoint(
                GroupCreditCards,
                MethodGet,
                $"/credit_cards/{cardId}",
                async () =>
                {
                    var card = await creditCardService.GetAsync(cardId);
                    return (card, JsonSerializer.Serialize(card));
                }
            );
        }
    }

    private async Task TestBudgetEndpoints()
    {
        Console.WriteLine("📋 Testing Budget endpoints...");

        var budgetService = new BudgetService(_client);

        // List current month budgets
        await TestEndpoint(
            "Budgets",
            MethodGet,
            "/budgets",
            async () =>
            {
                var budgets = await budgetService.ListAsync();
                return (budgets, JsonSerializer.Serialize(budgets));
            }
        );

        // List budgets by year
        await TestEndpoint(
            "Budgets",
            MethodGet,
            "/budgets/2026",
            async () =>
            {
                var budgets = await budgetService.ListByYearAsync(2026);
                return (budgets, JsonSerializer.Serialize(budgets));
            }
        );

        // List budgets by month
        await TestEndpoint(
            "Budgets",
            MethodGet,
            "/budgets/2026/2",
            async () =>
            {
                var budgets = await budgetService.ListByMonthAsync(2026, 2);
                return (budgets, JsonSerializer.Serialize(budgets));
            }
        );
    }

    private async Task CreateTestData()
    {
        Console.WriteLine("\n🔨 Creating test data...");

        var accountService = new AccountService(_client);
        var categoryService = new CategoryService(_client);
        var creditCardService = new CreditCardService(_client);

        // Create test account 1
        await TestEndpoint(
            GroupAccounts,
            MethodPost,
            "/accounts",
            async () =>
            {
                var options = new AccountCreateOptions
                {
                    Name = "API Validator Test Account",
                    Type = AccountType.Checking,
                    Description = "Created by API validator - safe to delete"
                };
                var account = await accountService.CreateAsync(options);
                _testAccountId = account.Id;
                return (account, JsonSerializer.Serialize(account));
            }
        );

        // Create test account 2 (for transfers)
        await TestEndpoint(
            GroupAccounts,
            MethodPost,
            "/accounts",
            async () =>
            {
                var options = new AccountCreateOptions
                {
                    Name = "API Validator Test Account 2",
                    Type = AccountType.Savings,
                    Description = "Created by API validator - safe to delete"
                };
                var account = await accountService.CreateAsync(options);
                _testAccount2Id = account.Id;
                return (account, JsonSerializer.Serialize(account));
            }
        );

        // Create test category
        await TestEndpoint(
            GroupCategories,
            MethodPost,
            "/categories",
            async () =>
            {
                var options = new CategoryCreateOptions
                {
                    Name = "API Validator Test Category"
                };
                var category = await categoryService.CreateAsync(options);
                _testCategoryId = category.Id;
                return (category, JsonSerializer.Serialize(category));
            }
        );

        // Create test credit card
        await TestEndpoint(
            GroupCreditCards,
            MethodPost,
            "/credit_cards",
            async () =>
            {
                var options = new CreditCardCreateOptions
                {
                    Name = "API Validator Test Card",
                    DueDay = 10,
                    ClosingDay = 5,
                    CardNetwork = "visa",
                    LimitCents = 100000
                };
                var card = await creditCardService.CreateAsync(options);
                _testCreditCardId = card.Id;
                return (card, JsonSerializer.Serialize(card));
            }
        );
    }

    private async Task TestTransactionEndpoints()
    {
        Console.WriteLine("\n📋 Testing Transaction endpoints...");

        var transactionService = new TransactionService(_client);

        // List transactions
        List<Transaction>? transactions = null;
        await TestEndpoint(
            GroupTransactions,
            MethodGet,
            "/transactions",
            async () =>
            {
                var options = new TransactionListOptions
                {
                    StartDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 2, 28, 0, 0, 0, DateTimeKind.Utc)
                };
                transactions = await transactionService.ListAsync(options);
                return (transactions, JsonSerializer.Serialize(transactions));
            }
        );

        // Create simple transaction
        long? simpleTransactionId = null;
        await TestEndpoint(
            GroupTransactions,
            MethodPost,
            "/transactions",
            async () =>
            {
                var options = new TransactionCreateOptions
                {
                    Description = "API Validator Test Transaction",
                    Date = DateTime.Today,
                    AmountCents = -5000,
                    AccountId = _testAccountId,
                    CategoryId = _testCategoryId,
                    Paid = true,
                    Notes = "Test transaction"
                };
                var transaction = await transactionService.CreateAsync(options);
                simpleTransactionId = transaction.Id;
                _testTransactionIds.Add(transaction.Id);
                return (transaction, JsonSerializer.Serialize(transaction));
            }
        );

        // Get transaction
        if (simpleTransactionId.HasValue)
        {
            await TestEndpoint(
                GroupTransactions,
                MethodGet,
                $"/transactions/{simpleTransactionId}",
                async () =>
                {
                    var transaction = await transactionService.GetAsync(simpleTransactionId.Value);
                    return (transaction, JsonSerializer.Serialize(transaction));
                }
            );
        }

        // Create recurring transaction
        await TestEndpoint(
            GroupTransactions,
            MethodPost,
            "/transactions (recurring)",
            async () =>
            {
                var options = new TransactionCreateOptions
                {
                    Description = "API Validator Recurring",
                    Date = DateTime.Today,
                    AmountCents = -1000,
                    AccountId = _testAccountId,
                    CategoryId = _testCategoryId,
                    RecurrenceAttributes = new RecurrenceAttributes
                    {
                        Periodicity = Periodicity.Monthly
                    }
                };
                var transaction = await transactionService.CreateAsync(options);
                _testTransactionIds.Add(transaction.Id);
                return (transaction, JsonSerializer.Serialize(transaction));
            }
        );

        // Create installment transaction
        await TestEndpoint(
            GroupTransactions,
            MethodPost,
            "/transactions (installments)",
            async () =>
            {
                var options = new TransactionCreateOptions
                {
                    Description = "API Validator Installments",
                    Date = DateTime.Today,
                    AmountCents = -6000,
                    AccountId = _testAccountId,
                    CategoryId = _testCategoryId,
                    InstallmentsAttributes = new InstallmentsAttributes
                    {
                        Periodicity = Periodicity.Monthly,
                        Total = 3
                    }
                };
                var transaction = await transactionService.CreateAsync(options);
                _testTransactionIds.Add(transaction.Id);
                return (transaction, JsonSerializer.Serialize(transaction));
            }
        );
    }

    private async Task TestTransferEndpoints()
    {
        Console.WriteLine("\n📋 Testing Transfer endpoints...");

        var transferService = new TransferService(_client);

        // List transfers
        await TestEndpoint(
            GroupTransfers,
            MethodGet,
            "/transfers",
            async () =>
            {
                var transfers = await transferService.ListAsync();
                return (transfers, JsonSerializer.Serialize(transfers));
            }
        );

        // Create transfer
        long? transferId = null;
        if (_testAccountId.HasValue && _testAccount2Id.HasValue)
        {
            await TestEndpoint(
                GroupTransfers,
                MethodPost,
                "/transfers",
                async () =>
                {
                    var options = new TransferCreateOptions
                    {
                        CreditAccountId = _testAccountId.Value,
                        DebitAccountId = _testAccount2Id.Value,
                        AmountCents = 1000,
                        Date = DateTime.Today,
                        Paid = true
                    };
                    var transfer = await transferService.CreateAsync(options);
                    transferId = transfer.Id;
                    _testTransferIds.Add(transfer.Id);
                    return (transfer, JsonSerializer.Serialize(transfer));
                }
            );

            // Get transfer
            if (transferId.HasValue)
            {
                await TestEndpoint(
                    GroupTransfers,
                    MethodGet,
                    $"/transfers/{transferId}",
                    async () =>
                    {
                        var transfer = await transferService.GetAsync(transferId.Value);
                        return (transfer, JsonSerializer.Serialize(transfer));
                    }
                );
            }
        }
    }

    private async Task TestUpdateOperations()
    {
        Console.WriteLine("\n🔄 Testing UPDATE operations...");

        var accountService = new AccountService(_client);
        var categoryService = new CategoryService(_client);
        var creditCardService = new CreditCardService(_client);
        var transactionService = new TransactionService(_client);
        var transferService = new TransferService(_client);

        // Update account
        if (_testAccountId.HasValue)
        {
            await TestEndpoint(
                GroupAccounts,
                MethodPut,
                $"/accounts/{_testAccountId}",
                async () =>
                {
                    var options = new AccountUpdateOptions
                    {
                        Name = "API Validator Test Account (Updated)",
                        Description = "Updated by validator"
                    };
                    var account = await accountService.UpdateAsync(_testAccountId.Value, options);
                    return (account, JsonSerializer.Serialize(account));
                }
            );
        }

        // Update category
        if (_testCategoryId.HasValue)
        {
            await TestEndpoint(
                GroupCategories,
                MethodPut,
                $"/categories/{_testCategoryId}",
                async () =>
                {
                    var options = new CategoryUpdateOptions
                    {
                        Name = "API Validator Test Category (Updated)"
                    };
                    var category = await categoryService.UpdateAsync(_testCategoryId.Value, options);
                    return (category, JsonSerializer.Serialize(category));
                }
            );
        }

        // Update credit card
        if (_testCreditCardId.HasValue)
        {
            await TestEndpoint(
                GroupCreditCards,
                MethodPut,
                $"/credit_cards/{_testCreditCardId}",
                async () =>
                {
                    var options = new CreditCardUpdateOptions
                    {
                        Name = "API Validator Test Card (Updated)"
                    };
                    var card = await creditCardService.UpdateAsync(_testCreditCardId.Value, options);
                    return (card, JsonSerializer.Serialize(card));
                }
            );
        }

        // Update transaction
        if (_testTransactionIds.Count > 0)
        {
            await TestEndpoint(
                GroupTransactions,
                MethodPut,
                $"/transactions/{_testTransactionIds[0]}",
                async () =>
                {
                    var options = new TransactionUpdateOptions
                    {
                        Description = "API Validator Test Transaction (Updated)",
                        Notes = "Updated notes"
                    };
                    var transaction = await transactionService.UpdateAsync(_testTransactionIds[0], options);
                    return (transaction, JsonSerializer.Serialize(transaction));
                }
            );
        }

        // Update transfer
        if (_testTransferIds.Count > 0)
        {
            await TestEndpoint(
                GroupTransfers,
                MethodPut,
                $"/transfers/{_testTransferIds[0]}",
                async () =>
                {
                    var options = new TransferUpdateOptions
                    {
                        Description = "API Validator Test Transfer (Updated)",
                        Notes = "Updated transfer notes"
                    };
                    var transfer = await transferService.UpdateAsync(_testTransferIds[0], options);
                    return (transfer, JsonSerializer.Serialize(transfer));
                }
            );
        }
    }

    private async Task TestInvoiceEndpoints()
    {
        Console.WriteLine("\n📋 Testing Invoice endpoints...");

        if (!_testCreditCardId.HasValue)
        {
            Console.WriteLine("  ⚠️  Skipping invoice tests - no test credit card");
            return;
        }

        var invoiceService = new InvoiceService(_client);

        // List invoices
        List<Invoice>? invoices = null;
        await TestEndpoint(
            "Invoices",
            MethodGet,
            $"/credit_cards/{_testCreditCardId}/invoices",
            async () =>
            {
                var options = new InvoiceListOptions
                {
                    StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc)
                };
                invoices = await invoiceService.ListAsync(_testCreditCardId.Value, options);
                return (invoices, JsonSerializer.Serialize(invoices));
            }
        );

        // Get invoice details if any exist
        if (invoices?.Count > 0)
        {
            var invoiceId = invoices[0].Id;

            await TestEndpoint(
                "Invoices",
                MethodGet,
                $"/credit_cards/{_testCreditCardId}/invoices/{invoiceId}",
                async () =>
                {
                    var invoice = await invoiceService.GetAsync(_testCreditCardId.Value, invoiceId);
                    return (invoice, JsonSerializer.Serialize(invoice));
                }
            );

            // Try to get payment (may not exist)
            try
            {
                await TestEndpoint(
                    "Invoices",
                    MethodGet,
                    $"/credit_cards/{_testCreditCardId}/invoices/{invoiceId}/payments",
                    async () =>
                    {
                        var payment = await invoiceService.GetPaymentAsync(_testCreditCardId.Value, invoiceId);
                        return (payment, JsonSerializer.Serialize(payment));
                    }
                );
            }
            catch
            {
                Console.WriteLine("  ℹ️  Invoice payment not found (expected for unpaid invoices)");
            }
        }
    }

    private async Task CleanupTestData()
    {
        Console.WriteLine("\n🧹 Cleaning up test data...");

        var accountService = new AccountService(_client);
        var categoryService = new CategoryService(_client);
        var creditCardService = new CreditCardService(_client);
        var transactionService = new TransactionService(_client);
        var transferService = new TransferService(_client);

        // Delete transactions
        foreach (var txId in _testTransactionIds)
        {
            await TestEndpoint(
                GroupTransactions,
                MethodDelete,
                $"/transactions/{txId}",
                async () =>
                {
                    var options = new TransactionDeleteOptions { UpdateAll = true };
                    var transaction = await transactionService.DeleteAsync(txId, options);
                    return (transaction, JsonSerializer.Serialize(transaction));
                },
                isCleanup: true
            );
        }

        // Delete transfers
        foreach (var transferId in _testTransferIds)
        {
            await TestEndpoint(
                GroupTransfers,
                MethodDelete,
                $"/transfers/{transferId}",
                async () =>
                {
                    var transfer = await transferService.DeleteAsync(transferId);
                    return (transfer, JsonSerializer.Serialize(transfer));
                },
                isCleanup: true
            );
        }

        // Delete credit card
        if (_testCreditCardId.HasValue)
        {
            await TestEndpoint(
                GroupCreditCards,
                MethodDelete,
                $"/credit_cards/{_testCreditCardId}",
                async () =>
                {
                    var card = await creditCardService.DeleteAsync(_testCreditCardId.Value);
                    return (card, JsonSerializer.Serialize(card));
                },
                isCleanup: true
            );
        }

        // Delete category
        if (_testCategoryId.HasValue)
        {
            await TestEndpoint(
                GroupCategories,
                MethodDelete,
                $"/categories/{_testCategoryId}",
                async () =>
                {
                    var options = new CategoryDeleteOptions();
                    var category = await categoryService.DeleteAsync(_testCategoryId.Value, options);
                    return (category, JsonSerializer.Serialize(category));
                },
                isCleanup: true
            );
        }

        // Delete accounts
        if (_testAccountId.HasValue)
        {
            await TestEndpoint(
                GroupAccounts,
                MethodDelete,
                $"/accounts/{_testAccountId}",
                async () =>
                {
                    var account = await accountService.DeleteAsync(_testAccountId.Value);
                    return (account, JsonSerializer.Serialize(account));
                },
                isCleanup: true
            );
        }

        if (_testAccount2Id.HasValue)
        {
            await TestEndpoint(
                GroupAccounts,
                MethodDelete,
                $"/accounts/{_testAccount2Id}",
                async () =>
                {
                    var account = await accountService.DeleteAsync(_testAccount2Id.Value);
                    return (account, JsonSerializer.Serialize(account));
                },
                isCleanup: true
            );
        }
    }

    private async Task TestEndpoint<T>(
        string service,
        string method,
        string path,
        Func<Task<(T result, string json)>> action,
        bool isCleanup = false)
    {
        var sw = Stopwatch.StartNew();
        var result = new EndpointResult
        {
            Service = service,
            Method = method,
            Path = path
        };

        try
        {
            var (apiResult, attempts) = await RetryHelper.ExecuteWithRetryAndCountAsync(
                async () =>
                {
                    var (obj, json) = await action();
                    return (obj, json);
                },
                maxAttempts: 3,
                onRetry: (attempt, ex) =>
                {
                    if (!isCleanup)
                        Console.WriteLine($"  ⚠️  Attempt {attempt} failed: {ex.Message}");
                }
            );

            sw.Stop();

            result.Success = true;
            result.Attempts = attempts;
            result.StatusCode = 200; // Successful response
            result.ResponseTimeMs = sw.ElapsedMilliseconds;
            result.RawResponse = apiResult.json;

            // Validate response
            result.Validation = ResponseValidator.ValidateResponse(apiResult.json, apiResult.obj);

            if (!isCleanup)
            {
                var status = result.Validation.Passed ? "✅" : "⚠️";
                Console.WriteLine($"  {status} {method} {path} ({attempts} attempt(s), {sw.ElapsedMilliseconds}ms)");
                if (!result.Validation.Passed)
                {
                    foreach (var discrepancy in result.Validation.Discrepancies.Take(3))
                    {
                        Console.WriteLine($"      - {discrepancy.Issue}: {discrepancy.PropertyName}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"  🧹 Deleted {path}");
            }
        }
        catch (Exception ex)
        {
            sw.Stop();
            result.Success = false;
            result.Attempts = 3;
            result.ResponseTimeMs = sw.ElapsedMilliseconds;
            result.ErrorMessage = ex.Message;
            result.Validation.Passed = false;
            result.Validation.Errors.Add(ex.Message);

            if (!isCleanup)
            {
                Console.WriteLine($"  ❌ {method} {path} - FAILED: {ex.Message}");
            }
        }

        _results.Add(result);
    }

    public List<EndpointResult> GetResults() => _results;
}
