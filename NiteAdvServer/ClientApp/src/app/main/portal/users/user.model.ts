
export class FilterUser {
    Where: string;
    Offset: number;
    PageSize: number;
    TotalItems: number;
}

export class UserResponse{
     UserList: User[];
     ItemsCount :number;
}
export class User {
    constructor(user) {
        
        this.id = user.id || '';
        this.pk = user.pk || '';
        this.Name = user.Name || '';
        this.Surename = user.Surename || '';
        this.Email = user.Email || '';
        this.Password = user.Password || '';
        this.BirthDate = user.BirthDate || 0;
       // this.BirthDateDate = user.BirthDateDate || new Date();
        this.Disabled = user.Disabled || false;
        this.UserActivated = user.UserActivated || false;
        this.RegistrationDate = user.RegistrationDate || 0;
        this.UserImageLink = user.UserImageLink || '';
        this.PhoneNumber = user.PhoneNumber || '';
        this.IsCompany = user.IsCompany || false;
        this.IdNation = user.IdNation ||0;
        this.LastSyncDate = user.LastSyncDate || 0;
        
    }
    id: string;
    pk:string;
    LastSyncDate: number;
    Name: string;
    Surename: string;
    Email: string;
    Password: string;
    BirthDate: number;
    //BirthDateDate: Date;
    Disabled: boolean;
    UserActivated: boolean;
    RegistrationDate: number;
    UserImageLink: string;
    PhoneNumber: string;
    IsCompany: boolean;
    IdNation: number;
    label: string;
    IsAdmin: boolean;
}