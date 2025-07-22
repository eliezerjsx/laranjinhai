namespace CapitalNerd.Laranjinhai.Domain.Filters
{
    public class BaseFilter
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}