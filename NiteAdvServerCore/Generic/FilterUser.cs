using System;

namespace NiteAdvServerCore.Generic;

public class FilterUser
{
    public FilterUser()
    {
        Where = "";
    }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int Offset { get; set; }
    public string Where { get; set; }
}
