using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;

namespace cargo_flow_backend.Mappings
{
    public static class InvoiceMapping
    {
        public static InvoiceDto ToDto(this Invoice invoice)
        {
            if (invoice == null)
                return null;

            return new InvoiceDto
            {
                Id = invoice.Id,
                Number = invoice.Number,
                InvoiceType = invoice.InvoiceType.ToDto(),
                Status = invoice.Status.ToDto(),
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                Company = invoice.Company.ToDto(),
                Amount = invoice.Amount,
                Currency = invoice.Currency,
                Order = invoice.Order?.ToDto(),
                Trip = invoice.Trip?.ToDto()
            };
        }

        public static Invoice ToEntity(this InvoiceCreateRequest request, DictionaryItem invoiceType, DictionaryItem status, Company company, Order? order, Trip? trip)
        {
            return new Invoice
            {
                Number = request.Number,
                InvoiceType = invoiceType,
                Status = status,
                IssueDate = request.IssueDate,
                DueDate = request.DueDate,
                Company = company,
                Amount = request.Amount,
                Currency = request.Currency,
                Order = order,
                Trip = trip
            };
        }

        public static void UpdateEntity(this InvoiceUpdateRequest request, Invoice invoice, DictionaryItem invoiceType, DictionaryItem status, Company company, Order? order, Trip? trip)
        {
            invoice.Number = request.Number;
            invoice.InvoiceType = invoiceType;
            invoice.Status = status;
            invoice.IssueDate = request.IssueDate;
            invoice.DueDate = request.DueDate;
            invoice.Company = company;
            invoice.Amount = request.Amount;
            invoice.Currency = request.Currency;
            invoice.Order = order;
            invoice.Trip = trip;
        }
    }
}
