import { HttpClient, HttpParams, HttpHeaders,HttpResponse,HttpEvent, HttpEventType } from '@angular/common/http';
import { ICustomer } from './../shared/ICustomer';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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
  private _inEditMode : boolean = false;
  private serviceStatus: string = "N/A";
  private customerService : CustomerService;
  private curCustomerId : number = 0;


  constructor(custService: CustomerService,private changeDetection: ChangeDetectorRef) { 
    this.customerService = custService;
  }

  ngOnInit(): void {
    this.getCustomers();
  }
  
  get firstname() {
    return this.customerForm.get('firstname')!;
  }
  currentFirstName(){
    return this.customerForm.get('firstname')!.value;
  }
  
  get lastname() {
    return this.customerForm.get('lastname')!;
  }
  currentLastName() {
    return this.customerForm.get('lastname')!.value;
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
    return Number(this.customerForm.get('zip')!.value);
  }

  get country() {
    return this.customerForm.get('country')!;
  }
  currentCountry() {
    return this.customerForm.get('country')!.value;
  }
  get inEditMode() {
    return this._inEditMode;
  }

  getStatusLabel() {
    return this.serviceStatus;
  }

  getCustomers() {
    this.customerService.getCustomers().subscribe((data: ICustomer[]) => {
      this.customers.splice(0);
      this.customers = [... data];
      //Based on https://stackoverflow.com/questions/45239739/angular2-ngfor-does-not-update-when-array-is-updated
      this.changeDetection.detectChanges();
    });
  }
 
  createOrUpdateCustomer() {
    console.log('createOrUpdateEmployee kaldt - editMode:' + this._inEditMode);
    let post = {'firstName': this.currentFirstName(), 
                'lastName': this.currentLastName(), 
                'email': this.currentEmail(), 
                'phone': this.currentPhone(),
                'address': this.currentAddress(),
                'zip': this.currentZip(),
                'country': this.currentCountry()} as ICustomer
    if (!this._inEditMode) {
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
    this.getCustomers();
    
  }
  deleteCustomer(id: number) {    
    this.customerService.deleteCustomer(id).subscribe(response => {
      console.log("Delete response", response)
      this.getCustomers();
    }) 
  }

  editCustomer(id: number) {
    this.serviceStatus = "Klar til at opdatere medarbejder";
    this.customerService.getCustomer(id).subscribe(response => {
      this._inEditMode = true;
      this.firstname.setValue(response.firstName);
      this.lastname.setValue(response.lastName);
      this.email.setValue(response.email);
      this.phone.setValue(response.phone);
      this.address.setValue(response.address);
      this.zip.setValue(response.zip.toString());
      this.country.setValue(response.country);

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
    this.address.setValue("");
    this.zip.setValue("");
    this.country.setValue("");
    //Ensures that the validators are not triggered by clearing thme
    this.customerForm.markAsUntouched()

  }
}
