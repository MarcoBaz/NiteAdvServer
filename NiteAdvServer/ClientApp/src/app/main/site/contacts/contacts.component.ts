import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { SiteService } from '../site.service';

@Component({
  selector: 'contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.scss']
})
export class ContactsComponent implements OnInit {
  public contactForm: UntypedFormGroup;
  public submitted = false;
  public loading = false;
  constructor(public _siteService:SiteService, private _formBuilder: UntypedFormBuilder) { }

  ngOnInit(): void {
    this.contactForm = this._formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      text: ['', Validators.required]
    });

    
  }
  get f() {
    return this.contactForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.contactForm.invalid) {
      return;
    }
    this.loading = true;
 
   
    this._siteService.SendContact(this.f.email.value,false, this.f.text.value);
      
  }
}
