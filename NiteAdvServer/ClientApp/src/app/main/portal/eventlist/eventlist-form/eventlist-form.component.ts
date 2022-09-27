
import { Component, EventEmitter, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileUploader } from 'ng2-file-upload';
import { Event } from '../../events/events.model';
import { EventlistService } from '../eventlist.service';

@Component({
  selector: 'eventlist-form',
  templateUrl: './eventlist-form.component.html',
  styleUrls: ['./eventlist-form.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EventlistFormComponent implements OnInit,OnDestroy {

  @ViewChild('modalForm') modalForm: any;
  // public
  public title:string;
  public contentMessage:string;
  public descriptionEditor:string;
  showCancel:boolean;
  callback: () => void;
  event: Event;
  image:File;
  eventlistForm: FormGroup;
  modalReference: any;
  filename:string;
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
  constructor(private modalService: NgbModal, private _formBuilder: FormBuilder, private _eventlistService:EventlistService) {
    this.filename ="Choose file";
    this.descriptionEditor = '';
  }

  // Public Methods
  // -----------------------------------------------------------------------------------------------------

  openDialog(event, callback): void {
    this.callback =callback;
    this.event = event;
    this.descriptionEditor = decodeURI(event.Description);
    this.eventlistForm = this.createEventForm();
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
  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this.filename ='';
    this.descriptionEditor ='';
    this.event =null;
  }

  createEventForm(): FormGroup {
    return this._formBuilder.group({
        id: [this.event.id],
        pk: [this.event.pk],
        Url : [this.event.Url],
        Name : [this.event.Name],
        Description : [this.event.Description],
        StartTimestamp : [this.event.StartTimestamp],
        EndTimestamp : [this.event.EndTimestamp],
        Latitude : [this.event.Latitude],
        Longitude : [this.event.Longitude],
        Image : [this.event.Image],
        CategoryType : [this.event.CategoryType],
        DiscoveryCategories : [this.event.DiscoveryCategories],
        Place: [this.event.Place],
        TicketingContext: [this.event.TicketingContext],
        TicketUrl: [this.event.TicketUrl],
        UsersGoing: [this.event.UsersGoing],
        IsInBlackList: [this.event.IsInBlackList],
        Deleted: [this.event.Deleted],
        UsersInterested: [this.event.UsersInterested],
        AllDay: [this.event.AllDay],
        LastSyncDate : [this.event.LastSyncDate],
       

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
    var event = new Event(groupData.value);
    event.Description = encodeURI(this.descriptionEditor);
    this._eventlistService.updateEvent(event,uploadData);
  }
  else if (type == 'delete')
  {
    var event = new Event(groupData.value);
    this._eventlistService.deleteEvent(event);
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
 
}


