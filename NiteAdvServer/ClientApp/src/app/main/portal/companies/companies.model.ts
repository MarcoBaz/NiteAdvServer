export class FilterCompany {
    IdCompany: number;
    Where: string;
    Offset: number;
    PageSize: number;
    TotalItems: number;
}

export class CompanyResponse{
     CompanyList: Company[];
     ItemsCount :number;
}
export class Company {
    constructor(company) {
        
        this.id = company.id || '';
        this.pk = company.pk || '';
        this.Url = company.Url || '';
        this.Name = company.Name || '';
        this.Street = company.Street || '';
        this.City = company.City || '';
        this.Country = company.Country || '';
        this.Latitude = company.Latitude || 0;
        this.Longitude = company.Longitude || 0;
        this.HasClaimed = company.HasClaimed || false;
        this.PlaceSearch = company.PlaceSearch || '';
        this.ImageUrl = company.ImageUrl || '';
        this.GooglePlaceId = company.GooglePlaceId || '';
        this.Type = company.Type || '';
        this.Rating = company.Rating ||0;
        this.Reviews = company.Reviews || '';
        this.Email = company.Email || '';
        this.Phone = company.Phone || '';
        this.OpeningHours = company.OpeningHours || '';
        this.GoogleUrl = company.GoogleUrl || '';
        this.WebSite = company.WebSite || '';
        this.RatingTotal = company.RatingTotal || 0;
        this.LastSyncDate = company.LastSyncDate || new Date();
        
    }
    id: string;
    pk:string;
    LastSyncDate: number;
    Url: string;
    Name: string;
    Street: string;
    City: string;
    Country: string;
    Latitude: number;
    Longitude: number;
    HasClaimed: boolean;
    ImageUrl: string;
    GooglePlaceId: string;
    PlaceSearch: string;
    Type: string;
    GoogleTypes: string;
    Rating: number;
    Reviews: string;
    GoogleUrl: string;
    WebSite: string;
    Email: string;
    Phone: string;
    OpeningHours: string;
    RatingTotal: number;
    label: string;
}