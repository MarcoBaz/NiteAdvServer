using System;
using NiteAdvServerCore.Entities;

namespace NiteAdvServerCore.Generic;

public class FilterCompany
{
    public FilterCompany()
    {
        Where = "";
    }
   
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int Offset { get; set; }
    public string Where { get; set; }
    public string City { get; set; }
    public List<Company> CheckedCompanies { get; set; }
    public Company CompanyToSave { get; set; }
}
