namespace MinhasFinancas.Domain.ValueObjects;

public class PagedRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }

    public int Skip => (Page - 1) * PageSize;
}
