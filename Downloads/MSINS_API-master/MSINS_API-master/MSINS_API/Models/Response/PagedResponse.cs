public class PagedResponse<T>
{
    public List<T> Data { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Message { get; set; }
    public int Count { get; internal set; }
    public object TotalCount { get; internal set; }
    public object PageIndex { get; internal set; }

    // Default constructor (Fix!)
    public PagedResponse() { }

    // Typed constructor (Advanced use)
    public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalRecords)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }
}
