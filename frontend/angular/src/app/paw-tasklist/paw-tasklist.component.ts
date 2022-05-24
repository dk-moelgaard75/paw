import { CustomerService } from './../services/customer.service';
import { ITaskObj } from './../shared/ITaskObj';
import { Component, OnInit } from '@angular/core';
import { TaskobjService } from '../services/taskobj.service';
import { HttpResponse } from '@angular/common/http';
import { JwtHelperService  } from '@auth0/angular-jwt';

@Component({
  selector: 'app-paw-tasklist',
  templateUrl: './paw-tasklist.component.html',
  styleUrls: ['./paw-tasklist.component.css']
})
export class PawTasklistComponent implements OnInit {

  public tasks: ITaskObj[] = [];
  private taskService: TaskobjService;
  private customerService: CustomerService;
  private apiToken: string | null;
  private empGuid: string | null;
  
  
  constructor(private jwtHelper: JwtHelperService,
                tskServ: TaskobjService,
                custServ: CustomerService) { 
    this.customerService = custServ;
    this.taskService = tskServ;
    this.empGuid = '';
    this.apiToken = localStorage.getItem('token');
    if (this.apiToken != null)
    {
      const tokenData = this.jwtHelper.decodeToken(this.apiToken);
      console.log("paw-tasklist - unique_name: " + tokenData.unique_name);
      console.log("paw-tasklist - unique_name: " + tokenData.nameid);
      this.empGuid = tokenData.nameid
    }
  }
 
  ngOnInit(): void {
    this.getTasks();
  }
  getTasks() {
    this.taskService.getTasksByEmployeeId(this.empGuid!).subscribe((response: HttpResponse<ITaskObj[]>) => {
      console.log("getTasksByEmployeeId - Ok" + response.ok);
      console.log("getTasksByEmployeeId - body" + response.body);
      this.tasks = response.body as ITaskObj[];
      console.log("getTasksByEmployeeId" + this.tasks); 
    });
  }

}
