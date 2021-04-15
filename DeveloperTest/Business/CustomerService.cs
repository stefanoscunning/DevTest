using System.Linq;
using DeveloperTest.Business.Interfaces;
using DeveloperTest.Database;
using DeveloperTest.Database.Models;
using DeveloperTest.Models;

namespace DeveloperTest.Business
{
  public class CustomerService : ICustomerService
  {
    private readonly ApplicationDbContext context;

    public CustomerService(ApplicationDbContext context)
    {
      this.context = context;
    }

    public CustomerModel[] GetCustomers()
    {
      return context.Customers.Select(x => new CustomerModel
      {
        CustomerId = x.CustomerId,
        Name = x.Name,
        Type = x.Type
      }).ToArray();
    }

    public CustomerModel GetCustomer(int customerId)
    {
      return context.Customers.Where(x => x.CustomerId == customerId).Select(x => new CustomerModel
      {
        CustomerId = x.CustomerId,
        Name = x.Name,
        Type = x.Type
      }).SingleOrDefault();
    }

    public CustomerModel CreateCustomer(BaseCustomerModel model)
    {
      var addedCustomer = context.Customers.Add(new Customer
      {
        Name = model.Name,
        Type = model.Type
      });

      context.SaveChanges();

      return new CustomerModel
      {
        CustomerId = addedCustomer.Entity.CustomerId,
        Name = addedCustomer.Entity.Name,
        Type = addedCustomer.Entity.Type
      };
    }
  }
}
