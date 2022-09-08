export class UserLogged {

    
    public static Id: number;
    public static Name: string;
    public static Surename: string;
    
  
    public static setValues(userlogged) {
        {
            this.Id = userlogged.Id || 0;
            this.Name = userlogged.Name || '';
            this.Surename = userlogged.Surename || '';
           
        }
    }
    public static getId():number {
        {
           return this.Id;
        
           
        }
    }
    public static logout() {
        {
            this.Id =  0;
            this.Name =  '';
            this.Surename = '';
           
        }
    }
}
export class LoginViewModel {
    Login:string;
    Password: string;
    constructor(login,password) {
        this.Login= login||'';
        this.Password = password || '';
    }
    
}

export class FacebookLoginViewModel {
    AccessToken:string;
    Expire: Date;
    UserId:string
    constructor(accessToken,expire,userID) {
        this.AccessToken= accessToken||'';
        this.Expire = expire || new Date();
        this.UserId = userID||'';
    }
    
}
