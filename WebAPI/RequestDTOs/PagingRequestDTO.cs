using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDTOs
{
    public class PagingRequestDTO
    {
        public string SearchText { get; set; } = "";
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNo { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
    }
}
