using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using WebApiRESTv1.Repositories;

namespace WebApiRESTv1.DependencyInjection
{
    public class SimpleResolver : IDependencyResolver
    {
        private readonly IOrderDiApiRepository _orderDiApiRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public SimpleResolver(
            IOrderDiApiRepository orderDiApiRepository,
            IOrderRepository orderRepository,
            IContactRepository contactRepository,
            IPriceRepository priceRepository,
            IQuoteRepository quoteRepository,
            IInvoiceRepository invoiceRepository
            
            )
        {
            _orderDiApiRepository = orderDiApiRepository;
            _orderRepository = orderRepository;
            _contactRepository = contactRepository;
            _priceRepository = priceRepository;
            _quoteRepository = quoteRepository;
            _invoiceRepository = invoiceRepository;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(Controllers.OrderController))
            {
                return new Controllers.OrderController(_orderDiApiRepository, _orderRepository);
            }
            if (serviceType == typeof(Controllers.QuotationController))
            {
                return new Controllers.QuotationController(_orderDiApiRepository, _quoteRepository);
            }
            if (serviceType == typeof(Controllers.ContactController))
            {
                return new Controllers.ContactController(_contactRepository);
            }
            if (serviceType == typeof(Controllers.PriceController))
            {
                return new Controllers.PriceController(_priceRepository);
            }
            if (serviceType == typeof(Controllers.InvoiceController))
            {
                return new Controllers.InvoiceController(_invoiceRepository);
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose() { }
    }

}