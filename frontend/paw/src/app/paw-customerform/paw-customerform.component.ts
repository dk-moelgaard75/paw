import { HttpClient, HttpParams, HttpHeaders,HttpResponse,HttpEvent, HttpEventType } from '@angular/common/http';
import { ICustomer } from './../shared/ICustomer';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CustomerService } from '../services/customer.service';


@Component({
  selector: 'app-paw-customerform',
  templateUrl: './paw-customerform.component.html',
  styleUrls: ['./paw-customerform.component.css']
})
export class PawCustomerformComponent implements OnInit {
  customerForm = new FormGroup({
    firstname: new FormControl('',Validators.required),
    lastname: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required),
    phone: new FormControl('', Validators.required),
    address: new FormControl('', Validators.required),
    zip: new FormControl('', Validators.required),
    country: new FormControl('', Validators.required) 
  });
  
  public customers: ICustomer[] = [];
  private editMode : boolean = false;
  private serviceStatus: string = "N/A";
  private customerService : CustomerService;
  private curCustomerId : number = 0;


  constructor(custService: CustomerService) { 
    this.customerService = custService;
  }

  ngOnInit(): void {
  }
  
  get firstname() {
    return this.customerForm.get('firstname')!;
  }
  currentFirstName(){
    return this.customerForm.get('firstname')!.value;
  }
  
  get lastname() {
    return this.customerForm.get('firstname')!;
  }
  currentLastName(){
    return this.customerForm.get('firstname')!.value;
  }

  get email() {
    return this.customerForm.get('email')!;
  }
  currentEmail() {
    return this.customerForm.get('email')!.value;
  }

  get phone() {
    return this.customerForm.get('phone')!;
  }
  currentPhone() {
    return this.customerForm.get('phone')!.value;
  }

  get address() {
    return this.customerForm.get('address')!;
  }
  currentAddress() {
    return this.customerForm.get('address')!.value;
  }
  
  get zip() {
    return this.customerForm.get('zip')!;
  }
  currentZip() {
    return this.customerForm.get('zip')!.value;
  }

  get country() {
    return this.customerForm.get('country')!;
  }
  currentCountry() {
    return this.customerForm.get('country')!.value;
  }

  createOrUpdateCustomer() {
    console.log('createOrUpdateEmployee kaldt - editMode:' + this.editMode);
    let post = {'firstName': this.currentFirstName(), 
                'lastName': this.currentLastName(), 
                'email': this.currentEmail(), 
                'phone': this.currentPhone(),
                'address': this.currentAddress(),
                'zip': this.currentAddress(),
                'country': this.currentCountry()} as ICustomer
    if (!this.editMode) {
      this.customerService.createCustomer(post).subscribe((response: HttpResponse<ICustomer>)=> {
        if (response != null && response.ok)
        {
          this.serviceStatus = "Kunden oprettet";
          console.log("response",response)
          console.log("OK",response.ok)
          console.log("Location", response.headers.get('Location'));
          this.customers.push(response.body as ICustomer)
        }
        else {
          this.serviceStatus = "Fejl opstået:" + response.statusText;
        }
      });
    }
    else {
      this.customerService.updateCustomer(this.curCustomerId,post).subscribe((response: HttpResponse<ICustomer>) => {
        if (response.ok) {
          this.serviceStatus = "Kunden opdateret";
        }
        else {
          this.serviceStatus = "Fejl opstået:" + response.statusText;
        }

      })
    }


  }
  editEmployee(id: number) {
    this.serviceStatus = "Klar til at opdatere medarbejder";
    this.customerService.getCustomer(id).subscribe(response => {
      this.editMode = true;
      this.firstname.setValue(response.firstName);
      this.lastname.setValue(response.lastName);
      this.email.setValue(response.email);
      this.phone.setValue(response.phone);
      this.address.setValue(response.address);
      this.zip.setValue(response.zip);
      this.country.setValue(response.country);

    })
  }

}
