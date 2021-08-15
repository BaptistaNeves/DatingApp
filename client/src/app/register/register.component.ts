import { Component, Input, OnInit, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private accountService: AccountService, 
              private toastr: ToastrService,
              private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = new FormGroup({
      gender: new FormControl('male'),
      username: new FormControl('', Validators.required),
      knownAs: new FormControl('', Validators.required),
      dateOfBirth: new FormControl('', Validators.required),
      city: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, 
                                    Validators.minLength(4),
                                    Validators.maxLength(8)]),
      confirmPassword: new FormControl('', Validators.required)
    });
  }

  get f() : { [key: string]: AbstractControl } {
    return this.registerForm.controls
  }


  register(){
     this.accountService.register(this.registerForm.value).subscribe(response =>{
        this.router.navigateByUrl('/members');
     }, error =>{
        this.validationErrors = error;
     });
  }

    cancel(){
      this.cancelRegister.emit(false)
    }

}
