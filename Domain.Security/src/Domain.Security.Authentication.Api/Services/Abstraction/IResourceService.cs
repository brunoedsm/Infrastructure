using Domain.Security.Authentication.Api.Entities;
using System.Collections.Generic;

namespace Domain.Security.Authentication.Api.Services.Abstraction
{
    public interface IResourceService
    {

        Resource GetById(int id);

        Resource GetByCodeAndPassword(string code, string password);

        IEnumerable<Resource> GetAll();


    }
}
