import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
  callback:any;
  company: Company;
  companyForm: FormGroup;
  modalReference: any;
  // snippet code variable

  /**
   * Constructor
   *
   * @param {NgbModal} modalService
   */
  constructor(private modalService: NgbModal, private _formBuilder: FormBuilder, private companiesService:CompaniesService) {
  }

  // Public Methods
  // -----------------------------------------------------------------------------------------------------

  openDialog( company): void {
    this.company = company;
    this.companyForm = this.createCompanyForm();
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
        
        

    });
}
  closeForm(type, groupData) {
        
  if (type == 'save')
  {
    var company = new Company(groupData.value);
    this.companiesService.updateCompany(company);
  }
  //alert('groupData:' + groupData);
  //uploadData.append('User', groupData);
  //this.userForm.FileUpload = this.fileToUpload;
  this.modalReference.close();
}
}
