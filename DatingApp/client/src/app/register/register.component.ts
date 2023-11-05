import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  /* para comunicação pai-filho entre nível componentes */
/*   @Input() usersFromHomeComponent : any; */
  /* relação filho-pai entre nível componentes */
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  model:any = {}
  usrname: string = "";
  
  constructor( private accountService: AccountService,
     private router: Router, 
     private toast: ToastrService,
     private fb: FormBuilder ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      username:         ['', Validators.required],
      password:         ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword:  ['', [Validators.required, this.matchValues('password')]],
      bio:              ['', Validators.required]
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  matchValues(matchTo: string): ValidatorFn{
    return (control: AbstractControl) => {
      //null é o retorno padrão para caso a validação esteja ok
      return control.value === control.parent?.get(matchTo)?.value ? null : { matchingValues: true }
    }
  }

  register()
  {  
    this.accountService.register(this.model).subscribe({
      next: () =>{
        this.cancel();
        this.login();
      },
      error: err =>{
        let errorJ = JSON.parse(JSON.stringify(err));
        
        if (errorJ.error.errors.password != undefined) this.toast.error(errorJ.error.errors.password);  
        if (errorJ.error.errors.username != undefined) this.toast.error(errorJ.error.errors.username);  
        
      }
    })
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: () => {   
        this.usrname = this.accountService.getCurrentUser();
        this.router.navigateByUrl('/members');
       },
       error: error =>  this.toast.error(error.error)
    })
  }

  cancel()
  {
    this.cancelRegister.emit(false);
  }
}
