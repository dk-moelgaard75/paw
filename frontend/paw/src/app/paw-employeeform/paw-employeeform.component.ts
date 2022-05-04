//Alternativ look at:_ https://www.linkedin.com/pulse/3-steps-make-your-reactive-form-typesafe-angular-aart-den-braber/
import { IEmployee } from './../shared/IEmployee';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpParams, HttpHeaders,HttpResponse,HttpEvent, HttpEventType } from '@angular/common/http';
import { Observable, throwIfEmpty } from 'rxjs';
import { EmployeeService } from '../services/employee.service';
import { PawNavbarComponent} from '../paw-navbar/paw-navbar.component'



@Component({
  selector: 'app-paw-employee',
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
  private employeeService: EmployeeService;
  private serviceStatus: string = "N/A";
  private httpHeaders: HttpHeaders = new HttpHeaders;
  private httpKeys: string[] = [];
  private curEmployeeId : number = 0;
  private _inEditMode : boolean = false;

  constructor(emplService: EmployeeService) { 
    console.log("Constructor")
    
    this.employeeService = emplService;

  }

  ngOnInit(): void {
    this.getEmployeesWithHeaders();
  }
  
  //setting ! after .get('') disables the null check. It´s ok since there are validation on all fields
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
  get inEditMode() {
    return this._inEditMode;
  }

  getEmployees() {
    this.employeeService.getEmployees().subscribe((data: IEmployee[]) => {
      this.employees = data;
    });
  }
  getEmployeesWithHeaders() {
    this.employeeService.getEmployeesWithHeader().subscribe((response: HttpResponse<IEmployee[]>) => {
      console.log("header",response)
      this.employees = response.body as IEmployee[];
      
      this.httpHeaders = response.headers;

   });  
  } 
  createOrUpdateEmployee() {
    console.log('createOrUpdateEmployee kaldt - editMode:' + this._inEditMode);
    let post = {'firstName': this.currentFirstName(), 
                'lastName': this.currentLastName(), 
                'email': this.currentEmail(), 
                'phone': this.currentPhone(),
                'password': this.currentPassword()} as IEmployee
    if (!this._inEditMode) {
      this.employeeService.createEmployeeWithHeaders(post).subscribe((response: HttpResponse<IEmployee>)=> {
        if (response != null && response.ok)
        {
          this.serviceStatus = "Brugeren oprettet";
          console.log("response",response)
          console.log("OK",response.ok)
          console.log("Location", response.headers.get('Location'));
          this.employees.push(response.body as IEmployee)
        }
        else {
          this.serviceStatus = "Fejl opstået:" + response.statusText;
        }
      });
    }
    else {
      this.employeeService.updateEmployeeWithHeader(this.curEmployeeId,post).subscribe((response: HttpResponse<IEmployee>) => {
        if (response.ok) {
          this.serviceStatus = "Brugeren opdateret";
          this.employees = [];
          this.getEmployees();
        }
        else {
          this.serviceStatus = "Fejl opstået:" + response.statusText;
        }
      })
    }
    this.clearFormsData();
    this._inEditMode = false; 
  }
  deleteEmployee(id: number) {    
    this.employeeService.deleteEmployee(id).subscribe(response => {
      console.log("Delete response", response)
      this.getEmployees();
    }) 
  }
  editEmployee(id: number) {
    this.serviceStatus = "Klar til at opdatere medarbejder";
    this.employeeService.getEmployee(id).subscribe(response => {
      this._inEditMode = true;
      this.firstname.setValue(response.firstName);
      this.lastname.setValue(response.lastName);
      this.email.setValue(response.email);
      this.phone.setValue(response.phone);
      this.password.setValue(response.password);

    })
  }
  setEditModeFalse() {  
    this.clearFormsData();
    this._inEditMode = false;
  }
  clearFormsData() {
    this.firstname.setValue("");
    this.lastname.setValue("");
    this.email.setValue("");
    this.phone.setValue("");
    this.password.setValue("");
    //Ensures that the validators are not triggered by clearing thme
    this.employeeForm.markAsUntouched()

  }
}
