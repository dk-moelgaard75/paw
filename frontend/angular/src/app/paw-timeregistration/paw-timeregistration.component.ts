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

@Component({
  selector: 'app-paw-timeregistration',
  templateUrl: './paw-timeregistration.component.html',
  styleUrls: ['./paw-timeregistration.component.css']
})
export class PawTimeregistrationComponent implements OnInit {
  timeregForm = new FormGroup({
    regdate: new FormControl('', Validators.required),
    reghours: new FormControl('', [Validators.required,Validators.pattern("^[0-9]+.[0-9]+$")]),
  });

  
  private sub: any;
  private apiToken: string | null;
  private employeeGuide: string = '';
  public taskid: any;
  public customer: ICustomer | any;
  private serviceStatus: string = "N/A";
  public task: ITaskObj | any; 

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
      const tokenData = this.jwtHelper.decodeToken(this.apiToken);
      this.employeeGuide = tokenData.nameid;
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
    return this.timeregForm.get('timeregdate')!.value;
  }

  get timeregdate() {
    return this.timeregForm.get('timeregdate')!;
  }
  currentTimeregDate(){
    let curDate = this.timeregForm.get('timeregdate')!.value;
    console.log(curDate);
    let curY = curDate.year;
    let curM = this.addLeadingZeros(curDate.month, 2);
    let curD = this.addLeadingZeros(curDate.day, 2);
    let newDate = curY + "-" + curM + "-" + curD;
    return newDate;
  }
  addLeadingZeros(num: number, totalLength: number): string {
    return String(num).padStart(totalLength, '0');
  }



   createTimeRegistration()   {
    let post = {'regdate': this.currentTimeregDate(), 
                'reghours': this.currentTimeregHours(),
                'taskGuid': this.taskid,
                'employee': this.employeeGuide} as ITimeReg
    this.timeregService.createTimereg(post).subscribe((response: HttpResponse<ITimeReg>)=> {
      if (response != null && response.ok)
      {
        this.serviceStatus = "Opgave oprettet";
      }
      else {
        this.serviceStatus = "Fejl opstået:" + response.statusText;
      }
    });
   }

}


