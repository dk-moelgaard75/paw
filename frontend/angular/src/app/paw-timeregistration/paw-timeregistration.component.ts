import { ITimeReg } from './../shared/ITimeReg';
import { ITaskObj } from './../shared/ITaskObj';
import { TaskobjService } from './../services/taskobj.service';
import { ICustomer } from './../shared/ICustomer';
import { CustomerService } from './../services/customer.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpResponse } from '@angular/common/http';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { JwtHelperService  } from '@auth0/angular-jwt';
import { TimeregService } from '../services/timereg.service';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';

@Component({
  selector: 'app-paw-timeregistration',
  templateUrl: './paw-timeregistration.component.html',
  styleUrls: ['./paw-timeregistration.component.css']
})
export class PawTimeregistrationComponent implements OnInit {
  timeregForm = new FormGroup({
    timeregdate: new FormControl('', Validators.required),
    timereghours: new FormControl('', [Validators.required,Validators.pattern("^[0-9]+.[0-9]+$")]),
  });

  
  private sub: any;
  private apiToken: string | null;
  private employeeGuid: string = '';
  public taskid: any;
  public customer: ICustomer | any;
  private serviceStatus: string = "N/A";
  public task: ITaskObj | any; 
  private curDate: string = '';

  private taskobjService: TaskobjService;
  private customerService: CustomerService;
  private timeregService: TimeregService;
  
  constructor( private jwtHelper: JwtHelperService,
              private router: ActivatedRoute,
              custServ: CustomerService,
              taskService: TaskobjService,
              timeReg: TimeregService) { 
   
    this.customerService = custServ;
    this.taskobjService = taskService;
    this.timeregService = timeReg;
    this.apiToken = localStorage.getItem('token');
    if (this.apiToken != null)
    {
      console.log("RAW token:" + this.apiToken);
      const tokenData = this.jwtHelper.decodeToken(this.apiToken);
      console.log("tokenData - name:" + tokenData.unique_name);
      this.employeeGuid = tokenData.nameid;
      console.log("tokenData - guid:" + this.employeeGuid);
    }

  }

  ngOnInit() {
    this.taskid = this.router.snapshot.paramMap.get('id')
    /*
    this.sub = this.router.params.subscribe(params => {
     this.taskid = params['id'];
     });
     */
     console.log("Timeregistration - ngOnInit():"  + this.taskid);
     this.task = this.taskobjService.getTask(this.taskid).subscribe(response => {
      this.task = response.body as ITaskObj; 
     });
   }

  get timereghours() {
    return this.timeregForm.get('timereghours')!; 
  }
  currentTimeregHours() {
    return Number(this.timeregForm.get('timereghours')!.value);
  }

  get timeregdate() {
    return this.timeregForm.get('timeregdate')!;
  }
  updateSelectedDate(date:NgbDate) {
    let curY = date.year;
    let curM = this.addLeadingZeros(date.month, 2);
    let curD = this.addLeadingZeros(date.day, 2);
    this.curDate = curY + "-" + curM + "-" + curD;
    console.log("curDate:" + this.curDate);
  }
  currentTimeregDate(){
    return this.curDate;    
  }
  addLeadingZeros(num: number, totalLength: number): string {
    return String(num).padStart(totalLength, '0');
  }
  getStatusLabel()
  {
    return this.serviceStatus;
  }


   createTimeRegistration()   {
    let post = {'regdate': this.currentTimeregDate(), 
                'hours': this.currentTimeregHours(),
                'taskGuid': this.taskid,
                'EmployeeGuid': this.employeeGuid} as ITimeReg
    this.timeregService.createTimereg(post).subscribe((response: HttpResponse<ITimeReg>)=> {
      console.log("createTimeRegistration - data sendt")
      console.log("->" + response);
      if (response != null && response.ok)
      {
        this.serviceStatus = "Timerne registreret";
      }
      else {
        this.serviceStatus = "Fejl opstået:";
      }
    });
   }

}


