import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { takeUntil, first } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { AuthenticationService } from 'app/auth/service';
import { CoreConfigService } from '@core/services/config.service';
import { FacebookService, LoginResponse } from 'ngx-facebook';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmComponent } from 'app/main/portal/confirm-dialog/confirm.component';



@Component({
  selector: 'app-auth-login-v2',
  templateUrl: './auth-login-v2.component.html',
  styleUrls: ['./auth-login-v2.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AuthLoginV2Component implements OnInit {
  @ViewChild('confirm') modal: ConfirmComponent;
  //  Public
  public coreConfig: any;
  public loginForm: UntypedFormGroup;
  public loading = false;
  public submitted = false;
  public returnUrl: string;
  public error = '';
  public passwordTextType: boolean;

  // Private
  private _unsubscribeAll: Subject<any>;

  /**
   * Constructor
   *
   * @param {CoreConfigService} _coreConfigService
   */
  constructor(
    private _coreConfigService: CoreConfigService,
    private _formBuilder: UntypedFormBuilder,
    private _route: ActivatedRoute,
    private _router: Router,
    private _authService: AuthenticationService,
    private fb:FacebookService
    //private _authenticationService: AuthenticationService
  ) {
    // redirect to home if already logged in
   // if (this._authenticationService.currentUserValue) {
   //   this._router.navigate(['/']);
   // }

    this._unsubscribeAll = new Subject();

    // Configure the layout
    this._coreConfigService.config = {
      layout: {
        navbar: {
          hidden: true
        },
        menu: {
          hidden: true
        },
        footer: {
          hidden: true
        },
        customizer: false,
        enableLocalStorage: false
      }
    };
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.loginForm.controls;
  }

  /**
   * Toggle password
   */
  togglePasswordTextType() {
    this.passwordTextType = !this.passwordTextType;
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    // Login
    this.loading = true;
   
    this._authService.Login(this.f.email.value, this.f.password.value);
      
  }

  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    this.loginForm = this._formBuilder.group({
      email: ['admin@demo.com', [Validators.required, Validators.email]],
      password: ['admin', Validators.required]
    });
    
    // get return url from route parameters or default to '/'
    this.returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';

    // Subscribe to config changes
    this._coreConfigService.config.pipe(takeUntil(this._unsubscribeAll)).subscribe(config => {
      this.coreConfig = config;
    });

    this._authService.onLoginError
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe(message=>{
      
      if (message != '') {
        this.loading = false;
      this.modal.openDialog("Error",message,true);
      }
     
    });
  }

  signInWithFB(): void {
    this.fb.login()
      .then((response: LoginResponse) => {
        if (response.status=='connected')
        {
          var expireDate = new Date();
          expireDate.setSeconds(expireDate.getSeconds() + response.authResponse.expiresIn);
          var userId = response.authResponse.userID
          //var expireDate = new NgbDate(d.getFullYear(),d.getMonth(),d.getDay());
          this._authService.apiAuthenticate(response.authResponse.accessToken,expireDate,userId);
        }
      })
      .catch((error: any) => console.error(error));
  }
  
  /**
   * On destroy
   */
  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();
  }
}
