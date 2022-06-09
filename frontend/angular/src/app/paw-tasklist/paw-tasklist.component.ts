import { ICustomer } from './../shared/ICustomer';
import { CustomerService } from './../services/customer.service';
import { ITaskObj } from './../shared/ITaskObj';
import { ITaskObjShow } from './../shared/ITaskObjShow';
import { Component, OnInit } from '@angular/core';
import { TaskobjService } from '../services/taskobj.service';
import { HttpResponse } from '@angular/common/http';
import { JwtHelperService  } from '@auth0/angular-jwt';
import { DatePipe } from '@angular/common'

@Component({
  selector: 'app-paw-tasklist',
  templateUrl: './paw-tasklist.component.html',
  styleUrls: ['./paw-tasklist.component.css']
})
export class PawTasklistComponent implements OnInit {

  public tasks: ITaskObj[] = [];
  public tasksWithInfo: ITaskObjShow[] = [];
  private customer : ICustomer | any;
  private taskService: TaskobjService;
  private customerService: CustomerService;
  private apiToken: string | null;
  private empGuid: string | null;
  
  
  constructor(private jwtHelper: JwtHelperService,
                tskServ: TaskobjService,
                public datepipe: DatePipe,
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
      this.tasks = response.body as ITaskObj[];
      this.createTasksWithInfo();
    });
  }
  createTasksWithInfo()
  {
    console.log("this.tasks.length:" + this.tasks.length)
    for (let t of this.tasks) //sneaky - let x in <array> vs let x of <array> - with OF (instead of IN) the interface is recognized
    {
      this.customerService.getCustomerByGuid(t.customerGuid).subscribe(response => {
        
        var newItem:ITaskObjShow =  {taskName: t.taskName,
                                    description: t.description,
                                    startDate: t.startDate, 
                                    startHour: t.startHour, 
                                    estimatedHours: t.estimatedHours,
                                    taskGuid: t.taskGuid,
                                  customerInfo: response.firstName + " - " + response.address}
        this.tasksWithInfo.push(newItem);
  
      });
    }

  }
  getColor(startDate: any)
  { 
      var currentDate = new Date();
      var stDate = new Date(startDate);
      var time = stDate.getTime() - currentDate.getTime(); 
      var days = time / (1000 * 3600 * 24)
      if (days <= 1)
      {
        return 'red';
      }
      else if (days > 1 && days <= 3)
      {
        return 'orange';
      }
      return 'green';
  }
  getDateWithOutTime(startDate: string)
  {
    let dt = new Date(startDate);
    return this.datepipe.transform(dt, 'dd-MM-yyyy')
  }


}
