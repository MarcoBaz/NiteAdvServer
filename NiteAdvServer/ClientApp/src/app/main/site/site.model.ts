export class ContactViewModel {
    Email:string;
    IsOwner:boolean;
    Text: string;
    constructor(email,isOwner,text) {
        this.Email= email||'';
        this.Text = text || '';
        this.IsOwner =isOwner || false;
    }
    
}