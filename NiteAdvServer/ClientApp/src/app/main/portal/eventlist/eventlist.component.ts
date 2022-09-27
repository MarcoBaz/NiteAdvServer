import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ColumnMode, DatatableComponent, SelectionType } from '@swimlane/ngx-datatable';
import { CoreTranslationService } from '@core/services/translation.service';

/*import { locale as german } from 'app/main/tables/datatables/i18n/de';
import { locale as english } from 'app/main/tables/datatables/i18n/en';
import { locale as french } from 'app/main/tables/datatables/i18n/fr';
import { locale as italian } from 'app/main/tables/datatables/i18n/it';*/
import { ConfirmComponent } from '../confirm-dialog/confirm.component';
import { EventlistService } from './eventlist.service';
import { Event, FilterEvent } from '../events/events.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EventlistFormComponent } from './eventlist-form/eventlist-form.component';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-eventlist',
  templateUrl: './eventlist.component.html',
  styleUrls: ['./eventlist.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EventlistComponent implements OnInit {
 
  // Private
  private _unsubscribeAll: Subject<any>;
  private tempData = [];
  public filter:FilterEvent;
  // public
  public contentHeader: object;
  public rows: any;
  //public selected = [];
  public kitchenSinkRows: any;
  //public basicSelectedOption: number = 10;
  public ColumnMode = ColumnMode;
  public expanded = {};
  public SelectionType = SelectionType;
  public selectedEvents :Event[];
  @ViewChild(DatatableComponent) tableEvent: DatatableComponent;
  @ViewChild('confirm') modal: ConfirmComponent;
  @ViewChild('eventlist') eventlistForm: EventlistFormComponent;
  @BlockUI() blockUI: NgBlockUI;
  /**
   * Constructor
   *
   * @param {EventlistService} _eventsService
   * @param {CoreTranslationService} _coreTranslationService
   */
   constructor(private _eventsService: EventlistService, 
    private _coreTranslationService: CoreTranslationService,
    private modalService: NgbModal) {
    this._unsubscribeAll = new Subject();
    this.filter = new FilterEvent();
    this.filter.PageSize =100;
    this.filter.Offset = 0;
    this.filter.Where ='';
    this.filter.City ='World Wide';
    this.filter.CheckedEvents = new Array<Event>();
    this.filter.EventToSave = new Event('');
    this.rows = new Array<Event>();
    this.selectedEvents = new Array<Event>();
    //this._coreTranslationService.translate(english, french, german, italian);
  }

  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit() {
    
    this. fullPageBlockUI();

    this._eventsService.onDatatablessChanged
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe(response => {
      if (response.EventList != null)
      {
        this.rows = [];
        this.rows = response.EventList;
        this.filter.TotalItems = response.ItemsCount;
        this.selectedEvents =[];
        this.tableEvent.selected =[];
        
        //this.selected =[];
        this.blockUI.stop();
      }
    
      /*this.tempData = this.rows;
      this.kitchenSinkRows = this.rows;
      this.exportCSVData = this.rows;*/
    });
  
    this._eventsService.onEventMessage
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe(message=>{
      
      if (message != null && message[1]!='') {
        this.blockUI.stop();
       // var comp = new ConfirmComponent(this.modalService);
       // comp.openDialog("Messaggio", message,false);
       var title = message[0]? "Errore":"Messaggio";
      this.modal.openDialog(title,message[1],message[0]);
      }
     
    });
    // content header
    this.contentHeader = {
      headerTitle: 'Events',
      actionButton: true,
      breadcrumb: {
        type: '',
        links: [
          {
            name: 'Home',
            isLink: true,
            link: '/'
          },
          {
            name: 'Events',
            isLink: false
          }
        ]
      }
    };
    this._eventsService.getDataTableRows(this.filter);
  }
  fullPageBlockUI() {
    this.blockUI.start('Loading...'); // Start blocking

   /* setTimeout(() => {
      this.blockUI.stop(); 
    }, 2000);*/
  }

  /**
   * Search (filter)
   *
   * @param event
   */
  filterUpdate(event) {
    
    if (event.type == "search")
    {
      // filter our data
      this.filter.Offset =0;
      this._eventsService.getDataTableRows(this.filter);
    }
    
  }

  /**
   * Row Details Toggle
   *
   * @param row
   */
  

  /**
   * For ref only, log selected values
   *
   * @param selected
   */
  onSelect({ selected }) {
    //console.log('Select Event', selected, this.selected);
    this.selectedEvents =[];
    selected.forEach(element => {
      this.selectedEvents.push(element);
    });
    
  }
  Edit(event) {
    //console.log('Select Event', selected, this.selected);
    if (this.selectedEvents != null && this.selectedEvents.length >0)
    {
      var cmp = this.selectedEvents[0];
      this.eventlistForm.openDialog(cmp,()=>{

        this. fullPageBlockUI();
      });
    }
    else{ this._eventsService.onEventMessage.next([true,'Select a row before proceed']);
    }
  //this.selected.splice(0, this.selected.length);
  //  this.selected.push(...selected);
  }
  AddToBlackList(event) {
    if (this.selectedEvents != null && this.selectedEvents.length >0)
    {
      this. fullPageBlockUI();
      this.filter.CheckedEvents = this.selectedEvents;
      this._eventsService.EventsBlackList(this.filter);
    }
    else{ this._eventsService.onEventMessage.next([true,'Select a row before proceed']);}
  }
  onSelectedSizeChange(size)
  {
    this.filter.PageSize = size || 10;
    this._eventsService.getDataTableRows(this.filter);
  }
  onSelectedCityChange(city)
  {
    
    this.filter.City = city;
    this._eventsService.getDataTableRows(this.filter);
  }
  /**
   * For ref only, log activate events
   *
   * @param selected
   */
  onActivate(event) {
    // console.log('Activate Event', event);
  }

  datatablePageData(pageInfo: { count?: number, pageSize?: number, limit?: number, offset?: number }) {
    this. fullPageBlockUI();
    this.filter.Offset = pageInfo.offset || 0;
    this.filter.PageSize = pageInfo.pageSize || 10;
    this.filter.TotalItems = pageInfo.count || 10;
    this._eventsService.getDataTableRows(this.filter);
  }
  getRowClass = (row) => {    
    return {
      'row-blacklist': row.IsInBlackList,
      'row-deleted ': row.Deleted,
    };
   }
}
