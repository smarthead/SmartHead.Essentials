using System.ComponentModel.DataAnnotations;

namespace SmartHead.Essentials.Application.Pagination
{
    public class QueryModel
    {
        [Required]
        public int Page { get; set; } = 1;
    }
}