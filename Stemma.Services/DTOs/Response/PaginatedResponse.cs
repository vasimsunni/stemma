namespace Stemma.Services.DTOs.Response
{
    public class PaginatedResponse<T>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public T Records { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
}
