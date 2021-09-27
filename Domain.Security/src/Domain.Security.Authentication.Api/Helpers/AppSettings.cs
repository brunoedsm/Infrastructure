using Domain.Security.Authentication.Api.Entities;
using System.Collections.Generic;

namespace Domain.Security.Authentication.Api.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public IEnumerable<Resource> Resources { get; set; }
    }
}
