using cargo_flow_backend.Entities;
using cargo_flow_backend.Models.Requests;
using cargo_flow_backend.Models.Responses;

namespace cargo_flow_backend.Mappings
{
    public static class CompanyMapping
    {
        public static CompanyDto ToDto(this Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Cui = company.Cui,
                Address = company.Address,
                ContactPerson = new PersonDto
                {
                    Id = company.ContactPerson.Id,
                    FullName = company.ContactPerson.FullName,
                    Email = company.ContactPerson.Email,
                    Phone = company.ContactPerson.Phone,
                    Cnp = company.ContactPerson.Cnp
                }
            };
        }

        public static Company ToEntity(this CompanyCreateRequest request, Person contactPerson)
        {
            return new Company
            {
                Name = request.Name,
                Code = request.Code,
                Cui = request.Cui,
                Address = request.Address,
                ContactPerson = contactPerson
            };
        }

        public static void UpdateEntity(this CompanyUpdateRequest request, Company company, Person contactPerson)
        {
            company.Name = request.Name;
            company.Code = request.Code;
            company.Cui = request.Cui;
            company.Address = request.Address;
            company.ContactPerson = contactPerson;
        }
    }
}
