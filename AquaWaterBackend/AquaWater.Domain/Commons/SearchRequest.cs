namespace AquaWater.Domain.Commons
{
    public class SearchRequest<T>
    {
        public T Data { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}