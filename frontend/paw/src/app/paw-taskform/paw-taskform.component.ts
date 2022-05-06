import { CustomerService } from './../services/customer.service';
import { ICustomer } from './../shared/ICustomer';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TaskobjService } from '../services/taskobj.service';

@Component({
  selector: 'app-paw-taskform',
  templateUrl: './paw-taskform.component.html',
  styleUrls: ['./paw-taskform.component.css']
})
export class PawTaskformComponent implements OnInit {
  taskForm = new FormGroup({
    taskname: new FormControl('',Validators.required),
    description: new FormControl('', Validators.required),
    startdate: new FormControl('', Validators.required),
    estimateddays: new FormControl('', Validators.required)
  });

  private _inEditMode : boolean = false;
  private serviceStatus: string = "N/A";
  private taskService: TaskobjService;
  private customerService: CustomerService;
  private customers: ICustomer[] = [];

  constructor(tskService: TaskobjService, custService: CustomerService) { 
    this.taskService = tskService;
    this.customerService = custService;
  }

  ngOnInit(): void {
    this.getCustomers();
  }
  get taskname() {
    return this.taskForm.get('taskname')!;
  }

  get inEditMode() {
    return this._inEditMode;
  }

  getCustomers() {
    this.customerService.getCustomers().subscribe((data: ICustomer[]) => {
      
      this.customers.splice(0);
      this.customers = [... data];
      //look at: https://stackoverflow.com/questions/45239739/angular2-ngfor-does-not-update-when-array-is-updated
      console.log("getCustomers returned:" + this.customers.length)
    });
  }
  createOrUpdateTask() {
  }
}
