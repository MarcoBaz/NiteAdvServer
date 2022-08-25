import { Component, OnInit,Inject, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'confirm-modal',
  templateUrl: './confirm.component.html'
})
export class ConfirmComponent implements OnInit {
  @ViewChild('modalSuccess') modalSuccess: any;
  @ViewChild('modalDanger') modalDanger: any;
  // public
  public contentHeader: object;
  public title:string;
  public contentMessage:string;
  showCancel:boolean;
  callback:any;
  component: any;
  // snippet code variable

  /**
   * Constructor
   *
   * @param {NgbModal} modalService
   */
  constructor(private modalService: NgbModal) {
    var x = modalService;
  }

  // Public Methods
  // -----------------------------------------------------------------------------------------------------

  openDialog( title, contentMessage,isError,showCancel= true,onCallBack=null): void {
    this.title = title;
    this.contentMessage = contentMessage;
    this.showCancel = showCancel;
    this.callback =onCallBack
    if (isError)
    {
      this.modalOpenDanger();
    }
    else
    {
      this.modalOpenSuccess();
    }
  }


  // modal Open Success
  modalOpenSuccess() {
    this.modalService.open(this.modalSuccess, {
      centered: true,
      windowClass: 'modal modal-success'
    });
  }

  // modal Open Danger
  modalOpenDanger() {
    this.modalService.open(this.modalDanger, {
      centered: true,
      windowClass: 'modal modal-danger'
    });
  }

  
  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    // content header
   
  }
}

