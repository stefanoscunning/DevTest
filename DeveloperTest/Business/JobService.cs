using System.Linq;
using DeveloperTest.Business.Interfaces;
using DeveloperTest.Database;
using DeveloperTest.Database.Models;
using DeveloperTest.Models;
using Microsoft.EntityFrameworkCore;

namespace DeveloperTest.Business
{
  public class JobService : IJobService
  {
    private readonly ApplicationDbContext context;

    public JobService(ApplicationDbContext context)
    {
      this.context = context;
    }

    public JobModel[] GetJobs()
    {
      return context.Jobs
    .Include(job => job.Customer)
    .Select(x => new JobModel
    {
      JobId = x.JobId,
      Engineer = x.Engineer,
      When = x.When,
      Customer = x.Customer == null ?
          new CustomerModel { CustomerId = 0, Name = "Unknown", Type = "Unknown" } :
          new CustomerModel { CustomerId = x.Customer.CustomerId, Name = x.Customer.Name, Type = x.Customer.Type }

    }).ToArray();
    }

    public JobModel GetJob(int jobId)
    {
      return context.Jobs.Where(x => x.JobId == jobId)
      .Include(job => job.Customer)
      .Select(x => new JobModel
      {
        JobId = x.JobId,
        Engineer = x.Engineer,
        When = x.When,
        Customer = x.Customer == null ?
          new CustomerModel { CustomerId = 0, Name = "Unknown", Type = "Unknown" } :
          new CustomerModel { CustomerId = x.Customer.CustomerId, Name = x.Customer.Name, Type = x.Customer.Type }

      }).SingleOrDefault();
    }

    public JobModel CreateJob(BaseJobModel model)
    {
      var addedJob = context.Jobs.Add(new Job
      {
        Engineer = model.Engineer,
        When = model.When,
        CustomerId = model.Customer.CustomerId
      });

      context.SaveChanges();

      return GetJob(addedJob.Entity.JobId);


    }
  }
}
