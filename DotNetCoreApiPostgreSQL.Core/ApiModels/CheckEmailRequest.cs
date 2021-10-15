using System.ComponentModel.DataAnnotations;

namespace DotNetCoreApiPostgreSQL.Core.ApiModels
{
    public class CheckEmailRequest
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
