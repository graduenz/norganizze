using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.CreditCards
{
    public class CreditCardService : Service
    {
        public CreditCardService(NOrganizzeClient client) : base(client)
        {
        }

        public List<CreditCard> List(RequestOptions requestOptions = null)
        {
            return Get<List<CreditCard>>("credit_cards", requestOptions);
        }

        public Task<List<CreditCard>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<CreditCard>>("credit_cards", requestOptions, cancellationToken);
        }

        public CreditCard Get(int id, RequestOptions requestOptions = null)
        {
            return Get<CreditCard>($"credit_cards/{id}", requestOptions);
        }

        public Task<CreditCard> GetAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<CreditCard>($"credit_cards/{id}", requestOptions, cancellationToken);
        }

        public CreditCard Create(CreditCardCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<CreditCard>("credit_cards", options, requestOptions);
        }

        public Task<CreditCard> CreateAsync(CreditCardCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<CreditCard>("credit_cards", options, requestOptions, cancellationToken);
        }

        public CreditCard Update(int id, CreditCardUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<CreditCard>($"credit_cards/{id}", options, requestOptions);
        }

        public Task<CreditCard> UpdateAsync(int id, CreditCardUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<CreditCard>($"credit_cards/{id}", options, requestOptions, cancellationToken);
        }

        public CreditCard Delete(int id, RequestOptions requestOptions = null)
        {
            return Delete<CreditCard>($"credit_cards/{id}", requestOptions);
        }

        public Task<CreditCard> DeleteAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync<CreditCard>($"credit_cards/{id}", requestOptions, cancellationToken);
        }
    }
}
