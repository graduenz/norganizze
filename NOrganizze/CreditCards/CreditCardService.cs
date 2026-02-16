using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.CreditCards
{
    public class CreditCardService : Service
    {
        private const string CreditCards = "credit_cards";

        public CreditCardService(NOrganizzeClient client) : base(client)
        {
        }

        public List<CreditCard> List(RequestOptions requestOptions = null)
        {
            return Get<List<CreditCard>>(CreditCards, requestOptions);
        }

        public Task<List<CreditCard>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<CreditCard>>(CreditCards, requestOptions, cancellationToken);
        }

        public CreditCard Get(long id, RequestOptions requestOptions = null)
        {
            return Get<CreditCard>($"{CreditCards}/{id}", requestOptions);
        }

        public Task<CreditCard> GetAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<CreditCard>($"{CreditCards}/{id}", requestOptions, cancellationToken);
        }

        public CreditCard Create(CreditCardCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<CreditCard>(CreditCards, options, requestOptions);
        }

        public Task<CreditCard> CreateAsync(CreditCardCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<CreditCard>(CreditCards, options, requestOptions, cancellationToken);
        }

        public CreditCard Update(long id, CreditCardUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<CreditCard>($"{CreditCards}/{id}", options, requestOptions);
        }

        public Task<CreditCard> UpdateAsync(long id, CreditCardUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<CreditCard>($"{CreditCards}/{id}", options, requestOptions, cancellationToken);
        }

        public CreditCard Delete(long id, RequestOptions requestOptions = null)
        {
            return Delete<CreditCard>($"{CreditCards}/{id}", requestOptions);
        }

        public Task<CreditCard> DeleteAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync<CreditCard>($"{CreditCards}/{id}", requestOptions, cancellationToken);
        }
    }
}
