import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

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
  constructor() { }

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

  }
}
