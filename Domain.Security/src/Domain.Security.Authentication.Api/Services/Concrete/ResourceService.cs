using Domain.Security.Authentication.Api.Entities;
using Domain.Security.Authentication.Api.Helpers;
using Domain.Security.Authentication.Api.Services.Abstraction;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Security.Authentication.Api.Services.Concrete
{
    public class ResourceService : IResourceService
    {
        private readonly IEnumerable<Resource> _repository;

        public ResourceService(IOptions<AppSettings> appSettings)
        {
            _repository = appSettings.Value.Resources;
        }

        public IEnumerable<Resource> GetAll()
        {
            return _repository;
        }

        public Resource GetByCodeAndPassword(string code, string password)
        {
            return _repository.FirstOrDefault(x => x.Code == code && x.Password == password);
        }

        public Resource GetById(int id)
        {
            return _repository.FirstOrDefault(x => x.Id == id);
        }
    }
}
