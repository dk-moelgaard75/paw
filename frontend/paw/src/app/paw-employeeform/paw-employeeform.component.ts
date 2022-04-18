//Alternativ look at:_ https://www.linkedin.com/pulse/3-steps-make-your-reactive-form-typesafe-angular-aart-den-braber/
import { IEmployee } from './../shared/IEmployee';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmployeeService } from '../services/employee.service';



@Component({
  selector: 'app-paw-employeeform',
  templateUrl: './paw-employeeform.component.html',
  styleUrls: ['./paw-employeeform.component.css']
})
export class PawEmployeeComponent implements OnInit {
  employeeForm = new FormGroup({
    firstname: new FormControl('',Validators.required),
    lastname: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required),
    phone: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });

  public employees: IEmployee[] = [];
  private myHttpClient: HttpClient;
  private employeeService: EmployeeService;
  private serviceStatus: string = "N/A";
  url = "http://acme.com/api/employee";

  constructor(http: HttpClient, emplService: EmployeeService) { 
    console.log("Constructor")
    this.myHttpClient = http;
    this.employeeService = emplService;
    this.getEmployees();
  }

  ngOnInit(): void {
  }
  
  //setting ! after .get('') disables the null check. It´s ok since there is validation on all fields
  get firstname() {
    return this.employeeForm.get('firstname')!;
  }
  currentFirstName(){
    return this.employeeForm.get('firstname')!.value;
  }
  
  get lastname() {
    return this.employeeForm.get('lastname')!;
  }
  currentLastName() {
    return this.employeeForm.get('lastname')!.value;
  }

  get email() {
    return this.employeeForm.get('email')!;
  }
  currentEmail() {
    return this.employeeForm.get('email')!.value;
  }

  get phone() {
    return this.employeeForm.get('phone')!;
  }
  currentPhone() {
    return this.employeeForm.get('phone')!.value;
  }

  get password() {
    return this.employeeForm.get('password')!;
  }
  currentPassword() {
    return this.employeeForm.get('password')!.value;
  }

  getStatusLabel() {
    return this.serviceStatus;

  }

  getEmployees() {
    return this.employeeService.getEmployees().subscribe((data: IEmployee[]) => {
      this.employees = data;
    });

    
  }
  createEmployee() {
    let post = {'firstName': this.currentFirstName(), 
                'lastName': this.currentLastName(), 
                'email': this.currentEmail(), 
                'phone': this.currentPhone(),
                'password': this.currentPassword()} as IEmployee
    this.employeeService.createEmployee(post).subscribe(resp => {
      console.log('Employee created', resp)
      this.serviceStatus = "Brugeren oprettet";
      
    });

  }

}
