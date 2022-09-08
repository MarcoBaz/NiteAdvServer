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
import { CompaniesService } from './companies.service';
import { Company, FilterCompany } from './companies.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CompanyFormComponent } from './company-form/company-form.component';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CompaniesComponent implements OnInit {
 
  // Private
  private _unsubscribeAll: Subject<any>;
  private tempData = [];
  public filter:FilterCompany;
  // public
  public contentHeader: object;
  public rows: any;
  public selected = [];
  public kitchenSinkRows: any;
  //public basicSelectedOption: number = 10;
  public ColumnMode = ColumnMode;
  public expanded = {};
  public SelectionType = SelectionType;

  @ViewChild(DatatableComponent) table: DatatableComponent;
  @ViewChild('confirm') modal: ConfirmComponent;
  @ViewChild('company') companyForm: CompanyFormComponent;
  @BlockUI() blockUI: NgBlockUI;
  /**
   * Constructor
   *
   * @param {CompaniesService} _companiesService
   * @param {CoreTranslationService} _coreTranslationService
   */
   constructor(private _companiesService: CompaniesService, 
    private _coreTranslationService: CoreTranslationService,
    private modalService: NgbModal) {
    this._unsubscribeAll = new Subject();
    this.filter = new FilterCompany();
    this.filter.PageSize =10;
    this.filter.Offset = 0;
    this.filter.Where ='';
    this.rows = new Array<Company>();
    //this._coreTranslationService.translate(english, french, german, italian);
  }

  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit() {
    
    this. fullPageBlockUI();

    this._companiesService.onDatatablessChanged
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe(response => {
      if (response.CompanyList != null)
      {
        this.rows = [];
        this.rows = response.CompanyList;
        this.filter.TotalItems = response.ItemsCount;
        this.blockUI.stop();
      }
    
      /*this.tempData = this.rows;
      this.kitchenSinkRows = this.rows;
      this.exportCSVData = this.rows;*/
    });
    this._companiesService.onCompanyMessage
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe(message=>{
      
      if (message != '') {
        this.blockUI.stop();
       // var comp = new ConfirmComponent(this.modalService);
       // comp.openDialog("Messaggio", message,false);
      this.modal.openDialog("Error",message,true);
      }
     
    });
    // content header
    this.contentHeader = {
      headerTitle: 'Companies',
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
            name: 'Companies',
            isLink: false
          }
        ]
      }
    };
    this._companiesService.getDataTableRows(this.filter);
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
      // const temp = this.tempData.filter(function (d) {
      //   return d.full_name.toLowerCase().indexOf(val) !== -1 || !val;
      // });
      // update the rows
      //this.kitchenSinkRows = temp;
      this.filter.Offset =0;
      this._companiesService.getDataTableRows(this.filter);
      // Whenever the filter changes, always go back to the first page
      //this.table.offset = 0;
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
    this.companyForm.openDialog(selected[0],()=>{

      this. fullPageBlockUI();
    });
  //this.selected.splice(0, this.selected.length);
  //  this.selected.push(...selected);
  }

  onSelectedSizeChange(size)
  {
    this.filter.PageSize = size || 10;
    this._companiesService.getDataTableRows(this.filter);
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
    this._companiesService.getDataTableRows(this.filter);
  }
}
