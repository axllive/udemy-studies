import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit{
  baseUrl = 'https://localhost:5001/api/';
  validationErrors : string[] = [];

  constructor( private toast: ToastrService, private http: HttpClient ) {  }

  ngOnInit(): void {  }

  get404Error(){
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: response => this.toast.error(response.toString()),
      error: err => {
        const teste = JSON.parse(JSON.stringify(err));
        this.toast.error(teste.message);
        console.log(err);
      }
    })
  }

  get400Error(){
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: response => this.toast.error(response.toString()),
      error: err => {
        const teste = JSON.parse(JSON.stringify(err));
        this.toast.error(teste.message);
        console.log(err);
      }
    })
  }

  get500Error(){
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: response => this.toast.error(response.toString()),
      error: err => {
        const teste = JSON.parse(JSON.stringify(err));
        this.toast.error(teste.message);
        console.log(err);
      }
    })
  }

  get401Error(){
    this.http.get(this.baseUrl + 'buggy/auth').subscribe({
      next: response => this.toast.error(response.toString()),
      error: err => {
        /* const teste = JSON.parse(JSON.stringify(err));
        this.toast.error(teste.message); */
        console.log(err);
      }
    })
  }

  get400ValidationError(){
    this.http.post(this.baseUrl + 'account/register', {}).subscribe({
      next: response => this.toast.error(response.toString()),
      error: err => {
        const teste = JSON.parse(JSON.stringify(err));
        this.validationErrors = teste;
        for (let key = 0; key < teste.length; key++) {
          this.toast.error(teste[key]);
        }
      }
    })
  }

}
