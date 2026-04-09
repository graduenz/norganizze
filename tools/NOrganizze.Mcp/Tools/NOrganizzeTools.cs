using System.ComponentModel;
using ModelContextProtocol.Server;
using NOrganizze.Accounts;
using NOrganizze.Budgets;
using NOrganizze.Categories;
using NOrganizze.CreditCards;
using NOrganizze.Invoices;
using NOrganizze.Mcp.Security;
using NOrganizze.Transactions;
using NOrganizze.Transfers;
using NOrganizze.Users;

namespace NOrganizze.Mcp.Tools;

[McpServerToolType]
public sealed class NOrganizzeTools
{
    private readonly NOrganizzeClient _client;
    private readonly ReadonlyGuard _readonlyGuard;

    public NOrganizzeTools(NOrganizzeClient client, ReadonlyGuard readonlyGuard)
    {
        _client = client;
        _readonlyGuard = readonlyGuard;
    }

    [McpServerTool, Description("Get user details by id.")]
    public User users_get([Description("User id")] long id)
    {
        return _client.Users.Get(id);
    }

    [McpServerTool, Description("List all accounts.")]
    public List<Account> accounts_list()
    {
        return _client.Accounts.List();
    }

    [McpServerTool, Description("Get account by id.")]
    public Account accounts_get([Description("Account id")] long id)
    {
        return _client.Accounts.Get(id);
    }

    [McpServerTool, Description("Create account.")]
    public Account accounts_create([Description("Create options")] AccountCreateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(accounts_create));
        return _client.Accounts.Create(options);
    }

    [McpServerTool, Description("Update account by id.")]
    public Account accounts_update(
        [Description("Account id")] long id,
        [Description("Update options")] AccountUpdateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(accounts_update));
        return _client.Accounts.Update(id, options);
    }

    [McpServerTool, Description("Delete account by id.")]
    public Account accounts_delete([Description("Account id")] long id)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(accounts_delete));
        return _client.Accounts.Delete(id);
    }

    [McpServerTool, Description("List all categories.")]
    public List<Category> categories_list()
    {
        return _client.Categories.List();
    }

    [McpServerTool, Description("Get category by id.")]
    public Category categories_get([Description("Category id")] long id)
    {
        return _client.Categories.Get(id);
    }

    [McpServerTool, Description("Create category.")]
    public Category categories_create([Description("Create options")] CategoryCreateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(categories_create));
        return _client.Categories.Create(options);
    }

    [McpServerTool, Description("Update category by id.")]
    public Category categories_update(
        [Description("Category id")] long id,
        [Description("Update options")] CategoryUpdateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(categories_update));
        return _client.Categories.Update(id, options);
    }

    [McpServerTool, Description("Delete category by id. Options are optional.")]
    public Category categories_delete(
        [Description("Category id")] long id,
        [Description("Optional delete options")] CategoryDeleteOptions options = null)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(categories_delete));
        return _client.Categories.Delete(id, options);
    }

    [McpServerTool, Description("List all credit cards.")]
    public List<CreditCard> credit_cards_list()
    {
        return _client.CreditCards.List();
    }

    [McpServerTool, Description("Get credit card by id.")]
    public CreditCard credit_cards_get([Description("Credit card id")] long id)
    {
        return _client.CreditCards.Get(id);
    }

    [McpServerTool, Description("Create credit card.")]
    public CreditCard credit_cards_create([Description("Create options")] CreditCardCreateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(credit_cards_create));
        return _client.CreditCards.Create(options);
    }

    [McpServerTool, Description("Update credit card by id.")]
    public CreditCard credit_cards_update(
        [Description("Credit card id")] long id,
        [Description("Update options")] CreditCardUpdateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(credit_cards_update));
        return _client.CreditCards.Update(id, options);
    }

    [McpServerTool, Description("Delete credit card by id.")]
    public CreditCard credit_cards_delete([Description("Credit card id")] long id)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(credit_cards_delete));
        return _client.CreditCards.Delete(id);
    }

    [McpServerTool, Description("List invoices for a credit card. Options are optional.")]
    public List<Invoice> invoices_list(
        [Description("Credit card id")] long creditCardId,
        [Description("Optional list options")] InvoiceListOptions options = null)
    {
        return _client.Invoices.List(creditCardId, options);
    }

    [McpServerTool, Description("Get invoice detail by credit card and invoice id.")]
    public InvoiceDetail invoices_get(
        [Description("Credit card id")] long creditCardId,
        [Description("Invoice id")] long invoiceId)
    {
        return _client.Invoices.Get(creditCardId, invoiceId);
    }

    [McpServerTool, Description("Get invoice payment transaction by credit card and invoice id.")]
    public Transaction invoices_get_payment(
        [Description("Credit card id")] long creditCardId,
        [Description("Invoice id")] long invoiceId)
    {
        return _client.Invoices.GetPayment(creditCardId, invoiceId);
    }

    [McpServerTool, Description("List transactions. Options are optional.")]
    public List<Transaction> transactions_list([Description("Optional list options")] TransactionListOptions options = null)
    {
        return _client.Transactions.List(options);
    }

    [McpServerTool, Description("Get transaction by id.")]
    public Transaction transactions_get([Description("Transaction id")] long id)
    {
        return _client.Transactions.Get(id);
    }

    [McpServerTool, Description("Create transaction.")]
    public Transaction transactions_create([Description("Create options")] TransactionCreateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(transactions_create));
        return _client.Transactions.Create(options);
    }

    [McpServerTool, Description("Update transaction by id.")]
    public Transaction transactions_update(
        [Description("Transaction id")] long id,
        [Description("Update options")] TransactionUpdateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(transactions_update));
        return _client.Transactions.Update(id, options);
    }

    [McpServerTool, Description("Delete transaction by id. Options are optional.")]
    public Transaction transactions_delete(
        [Description("Transaction id")] long id,
        [Description("Optional delete options")] TransactionDeleteOptions options = null)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(transactions_delete));
        return _client.Transactions.Delete(id, options);
    }

    [McpServerTool, Description("List all transfers.")]
    public List<Transfer> transfers_list()
    {
        return _client.Transfers.List();
    }

    [McpServerTool, Description("Get transfer by id.")]
    public Transfer transfers_get([Description("Transfer id")] long id)
    {
        return _client.Transfers.Get(id);
    }

    [McpServerTool, Description("Create transfer.")]
    public Transfer transfers_create([Description("Create options")] TransferCreateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(transfers_create));
        return _client.Transfers.Create(options);
    }

    [McpServerTool, Description("Update transfer by id.")]
    public Transfer transfers_update(
        [Description("Transfer id")] long id,
        [Description("Update options")] TransferUpdateOptions options)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(transfers_update));
        return _client.Transfers.Update(id, options);
    }

    [McpServerTool, Description("Delete transfer by id.")]
    public Transfer transfers_delete([Description("Transfer id")] long id)
    {
        _readonlyGuard.EnsureWriteAllowed(nameof(transfers_delete));
        return _client.Transfers.Delete(id);
    }

    [McpServerTool, Description("List all budgets.")]
    public List<Budget> budgets_list()
    {
        return _client.Budgets.List();
    }

    [McpServerTool, Description("List budgets by year.")]
    public List<Budget> budgets_list_by_year([Description("Year, e.g. 2026")] int year)
    {
        return _client.Budgets.ListByYear(year);
    }

    [McpServerTool, Description("List budgets by year and month.")]
    public List<Budget> budgets_list_by_month(
        [Description("Year, e.g. 2026")] int year,
        [Description("Month 1-12")] int month)
    {
        return _client.Budgets.ListByMonth(year, month);
    }
}
