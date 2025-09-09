namespace LMS.Blazor.Client.Models;

public class BreadcrumbItem
{
    public string Text { get; set; } = "";
    public string Href { get; set; } = "";
    public bool IsActive { get; set; } = false;
}

