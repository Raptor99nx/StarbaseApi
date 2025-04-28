using System.ComponentModel.DataAnnotations;
using System.Net;

namespace StargateAPI.Models
{
    public class BaseApiResponse
    {
        public bool Successful { get; set; }

        public HttpStatusCode HttpStatusCode {  get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public object Result { get; set; } = null!;
    }
}
