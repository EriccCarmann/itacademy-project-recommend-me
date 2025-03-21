using System.ComponentModel.DataAnnotations;

namespace RecommendMe.MVC.Models
{
    public class PaginationModel
    {
        [Range(1, Int32.MaxValue)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}