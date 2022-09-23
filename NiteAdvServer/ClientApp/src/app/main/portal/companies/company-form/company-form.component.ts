import { Component, EventEmitter, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileUploader } from 'ng2-file-upload';
import { Company } from '../companies.model';
import { CompaniesService } from '../companies.service';

@Component({
  selector: 'company-form',
  templateUrl: './company-form.component.html',
  styleUrls: ['./company-form.component.scss']
})
export class CompanyFormComponent implements OnInit {

  @ViewChild('modalForm') modalForm: any;
  // public
  public title:string;
  public contentMessage:string;
  showCancel:boolean;
  callback: () => void;
  company: Company;
  image:File;
  companyForm: FormGroup;
  modalReference: any;
  filename:string;
  selectedType:string='';
  public selectLabel = [
    { label: 'Concert', bullet: 'primary' },
    { label: 'Club', bullet: 'danger' },
    { label: 'Theater', bullet: 'warning' },
    { label: 'Festival', bullet: 'success' },
    // { label: 'ETC', bullet: 'info' }
  ];
  // snippet code variable
  public uploader: FileUploader = new FileUploader({
    //url: URL,
    isHTML5: true
  });
  /**
   * Constructor
   *
   * @param {NgbModal} modalService
   */
  constructor(private modalService: NgbModal, private _formBuilder: FormBuilder, private companiesService:CompaniesService) {
    this.filename ="Choose file";
  }

  // Public Methods
  // -----------------------------------------------------------------------------------------------------

  openDialog(company, callback): void {
    this.callback =callback;
    this.company = company;
    this.selectedType = company.Type;
    this.companyForm = this.createCompanyForm();
    this.modalOpen();
    
  }


  // modal Open Success
  modalOpen() {
   this.modalReference =  this.modalService.open(this.modalForm, {
      centered: true,
      size: 'lg',
      windowClass: 'modal modal-success'
    });
  }

  
  ngOnInit(): void {
    // content header
    
  }

  createCompanyForm(): FormGroup {
    return this._formBuilder.group({
        id: [this.company.id],
        pk: [this.company.pk],
        Url : [this.company.Url],
        Name : [this.company.Name],
        Street : [this.company.Street],
        City : [this.company.City],
        Country : [this.company.Country],
        Latitude : [this.company.Latitude],
        Longitude : [this.company.Longitude],
        HasClaimed : [this.company.HasClaimed],
        PlaceSearch : [this.company.PlaceSearch],
        ImageUrl : [this.company.ImageUrl],
        GooglePlaceId: [this.company.GooglePlaceId],
        GoogleTypes: [this.company.GoogleTypes],
        Type: [this.company.Type],
        Rating: [this.company.Rating],
        Reviews: [this.company.Reviews],
        Email: [this.company.Email],
        Phone: [this.company.Phone],
        OpeningHours: [this.company.OpeningHours],
        GoogleUrl: [this.company.GoogleUrl],
        WebSite: [this.company.WebSite],
        RatingTotal: [this.company.RatingTotal],
        LastSyncDate : [this.company.LastSyncDate],
        IsInBlackList: [this.company.IsInBlackList],
        Deleted: [this.company.Deleted],
        Size:[this.company.Size]

    });
}
  closeForm(type, groupData) {
        
  if (type == 'save')
  {
    var uploadData = null;
    if (this.image != null)
    {
         uploadData = new FormData();
         uploadData.append('file', this.image);
        //uploadData = new ServiceImageViewModel(this.fileToUpload,this.service.Id);
    }
    var company = new Company(groupData.value);
    this.companiesService.updateCompany(company,uploadData);
  }
  else if (type == 'delete')
  {
    var company = new Company(groupData.value);
    this.companiesService.deleteCompany(company);
  }
  //alert('groupData:' + groupData);
  //uploadData.append('User', groupData);
  //this.userForm.FileUpload = this.fileToUpload;
  this.callback();
  this.modalReference.close();
}
public onFileSelected(event: EventEmitter<File[]>) {
  const file: File = event[0];
  this.image = file;
  //this.fileToByteArray(file);
  this.filename = file.name;
}
 /*fileToByteArray(file) {
  return new Promise((resolve, reject) => {
      try {
          let reader = new FileReader();
          let fileByteArray = [];
          reader.readAsArrayBuffer(file);
          reader.onloadend = (evt) => {
              if (evt.target.readyState == FileReader.DONE) {
                  let arrayBuffer = evt.target.result,
                      array = new Uint8Array(file.size);
                  for (var byte of array) {
                      fileByteArray.push(byte);
                  }
              }
              this.image = fileByteArray;
              resolve(fileByteArray);
          }
      }
      catch (e) {
          reject(e);
      } 
  })
}*/
}


