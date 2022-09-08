using System;

namespace NiteAdvServerCore.Generic;

public class FilterCompany
{
    public FilterCompany()
    {
        Where = "";
    }
    public int IdCompany { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int Offset { get; set; }
    public string Where { get; set; }
}
