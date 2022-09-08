import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { User } from '../user.model';
import { UsersService } from '../users.service';

@Component({
  selector: 'user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {

  @ViewChild('modalForm') modalForm: any;
  // public
  public title:string;
  public contentMessage:string;
  public birthDateForm:NgbDate;
  showCancel:boolean;
  callback: () => void;
  user: User;
  userForm: FormGroup;
  modalReference: any;
  // snippet code variable


  constructor(private modalService: NgbModal, private _formBuilder: FormBuilder, private userService:UsersService) {
  }

  // Public Methods
  // -----------------------------------------------------------------------------------------------------

  openDialog(user, callback): void {
    this.callback =callback;
    this.user = user;
    var d = new Date(this.user.BirthDate);
    this.birthDateForm = new NgbDate(d.getFullYear(),d.getMonth(),d.getDay());
    this.userForm = this.createUserForm();
    this.modalOpen();
    
  }


  // modal Open Success
  modalOpen() {
   this.modalReference =  this.modalService.open(this.modalForm, {
      centered: true,
      windowClass: 'modal modal-success'
    });
  }

  
  ngOnInit(): void {
    // content header
    
  }

  createUserForm(): FormGroup {
    return this._formBuilder.group({
        id: [this.user.id],
        pk: [this.user.pk],
        Name : [this.user.Name],
        Surename : [this.user.Surename],
        Email : [this.user.Email],
        Password : [this.user.Password],
        BirthDate : [this.user.BirthDate],
        Disabled : [this.user.Disabled],
        UserActivated : [this.user.UserActivated],
        RegistrationDate : [this.user.RegistrationDate],
        UserImageLink : [this.user.UserImageLink],
        PhoneNumber: [this.user.PhoneNumber],
        IsCompany: [this.user.IsCompany],
        IdNation: [this.user.IdNation],
        IsAdmin: [this.user.IsAdmin],
        LastSyncDate : [this.user.LastSyncDate],


    });
}
  closeForm(type, groupData) {
        
  if (type == 'save')
  {
    var user = new User(groupData.value);
    var d = new Date(this.birthDateForm.year,this.birthDateForm.month,this.birthDateForm.day);
    user.BirthDate = d.valueOf();
    this.userService.updateUser(user);
  }
  //alert('groupData:' + groupData);
  //uploadData.append('User', groupData);
  //this.userForm.FileUpload = this.fileToUpload;
  this.callback();
  this.modalReference.close();
}
}
