import { CalendarService } from './../services/calendar.service';
import { ITaskObj } from './../shared/ITaskObj';
import { IStartTime } from './../shared/IStartTime';
import { EmployeeService } from './../services/employee.service';
import { IEmployee } from './../shared/IEmployee';
import { CustomerService } from './../services/customer.service';
import { ICustomer } from './../shared/ICustomer';
import { Component, OnInit,ChangeDetectorRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TaskobjService } from '../services/taskobj.service';
import { NgbDate, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbDatepicker } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient, HttpParams, HttpHeaders,HttpResponse,HttpEvent, HttpEventType } from '@angular/common/http';
import { async } from 'rxjs/internal/scheduler/async';




@Component({
  selector: 'app-paw-taskform',
  templateUrl: './paw-taskform.component.html',
  styleUrls: ['./paw-taskform.component.css']
})
export class PawTaskformComponent implements OnInit {
  taskForm = new FormGroup({
    taskname: new FormControl('',Validators.required),
    taskdescription: new FormControl('', Validators.required),
    taskstartdate: new FormControl('', Validators.required),
    taskstarttime: new FormControl('', Validators.required),
    taskestimatedhours: new FormControl('', Validators.required),
    taskcustomer: new FormControl('', Validators.required),
    taskemployee: new FormControl('', Validators.required)
  });

  private _inEditMode : boolean = false;
  private curCalendarId : string = '';
  public loading: boolean = false;
  public tasks: ITaskObj[] = [];
  private curDate: string = '';
  
  public calendarHtml: string = '';
  private serviceStatus: string = "N/A";
  private taskService: TaskobjService;
  private calenderService: CalendarService;
  private customerService: CustomerService;
  private employeeService: EmployeeService;
  public customers: ICustomer[] = [];
  public employees: IEmployee[] = [];
  public startTimeList : Array<IStartTime> =  [
                                        {id: 8, text: 'Kl. 8'},
                                        {id: 9, text: 'Kl. 9'},
                                        {id: 10, text: 'Kl. 10'},
                                        {id: 11, text: 'Kl. 11'},
                                        {id: 12, text: 'Kl. 12'},
                                        {id: 13, text: 'Kl. 13'},
                                        {id: 14, text: 'Kl. 14'},
                                        {id: 15, text: 'Kl. 15'},
                                      ]
                                    
  public estimatetHoursList: number[] = [1,2,3,4,5,6,7];
  //public employeeTypeValue = [{id:'employee', name:'Medarbejder'}, {id:'admin', name:'Administrator'}]; 

  constructor(tskService: TaskobjService, 
            custService: CustomerService,
            emplService: EmployeeService,
            calService: CalendarService,
            private changeDetection: ChangeDetectorRef) { 
    this.taskService = tskService;
    this.customerService = custService;
    this.employeeService = emplService;
    this.calenderService = calService;
  }

  ngOnInit(): void {
    this.getCustomers();
    this.getEmployees();
  }
  get taskname() {
    return this.taskForm.get('taskname')!;
  }
  currentTaskname() {
    return this.taskForm.get('taskname')!.value;
  }

  get taskdescription() {
    return this.taskForm.get('taskdescription')!;
  }
  currentTaskdescription(){
    return this.taskForm.get('taskdescription')!.value;
  }

  get taskstartdate() {
    return this.taskForm.get('taskstartdate')!;
  }
  updateSelectedDate(date:NgbDate) {
    let curY = date.year;
    let curM = this.addLeadingZeros(date.month, 2);
    let curD = this.addLeadingZeros(date.day, 2);
    this.curDate = curY + "-" + curM + "-" + curD;
    console.log("curDate:" + this.curDate);
  }

  currentTaskstartdate(){
    return this.curDate;
  }
  addLeadingZeros(num: number, totalLength: number): string {
    return String(num).padStart(totalLength, '0');
  }
  get taskstarttime() {
    return this.taskForm.get('taskstarttime')!;
  }
  currentTaskstarttime(){
    return Number(this.taskForm.get('taskstarttime')!.value);
  }
  get taskestimatedhours() {
    return this.taskForm.get('taskestimatedhours')!;
  }
  currentTaskestimatedhours(){
    return Number(this.taskForm.get('taskestimatedhours')!.value);
  }

  get taskcustomer() {
    return this.taskForm.get('taskcustomer')!;
  }
  currentTaskcustomer(){
    return this.taskForm.get('taskcustomer')!.value;
  }

  get taskemployee() { 
    return this.taskForm.get('taskemployee')!;
  }
  currentTaskemployee(){
    return this.taskForm.get('taskemployee')!.value;
  }
  
  get inEditMode() {
    return this._inEditMode;
  }

  getStatusLabel() {
    return this.serviceStatus;
  }

  getCustomers() {
    this.customerService.getCustomers().subscribe((data: ICustomer[]) => {
      this.customers = data
    });
  }
  getEmployees() {
    this.employeeService.getEmployeesWithoutHeader().subscribe((data: IEmployee[]) => {
      console.log('TaskForm - getEmployees');
      console.log(data);
      this.employees = data;
    });
  }

  createOrUpdateTask() {
    console.log('createOrUpdateTask kaldt - editMode:' + this._inEditMode);
    let post = {
                'taskName': this.currentTaskname(), 
                'description': this.currentTaskdescription(), 
                'startDate': this.currentTaskstartdate(), 
                'startHour': this.currentTaskstarttime(),
                'estimatedHours': this.currentTaskestimatedhours(),
                'endDate': '',
                'taskGuid': '',
                'customerGuid': this.currentTaskcustomer(),
                'employee': this.currentTaskemployee()} as ITaskObj;
    console.log(post); 
    console.log(JSON.stringify(post));
    if (!this._inEditMode) {
      this.taskService.createTask(post).subscribe((response: HttpResponse<ITaskObj>)=> {
        if (response != null && response.ok)
        {
          this.serviceStatus = "Opgave oprettet";
          console.log("response",response)
          console.log("OK",response.ok)
          console.log("Location", response.headers.get('Location'));
          this.tasks.push(response.body as ITaskObj)
        }
        else {
          this.serviceStatus = "Fejl opst??et:" + response.statusText;
        }
      });
    }
    /*
    TODO - missing implementation
    else {
      this.taskService.updateEmployeeWithHeader(this.curEmployeeId,post).subscribe((response: HttpResponse<IEmployee>) => {
        if (response.ok) {
          this.serviceStatus = "Brugeren opdateret";
          this.employees = [];
          this.getEmployees();
          this.changeDetection.detectChanges();
        }
        else {
          this.serviceStatus = "Fejl opst??et:" + response.statusText;
        }
      })
      
    } 
    */
  }
  handleCalendar() {
      
      let curDate = this.currentTaskstartdate();
      if (curDate != null && curDate !== 'undefined-undefined-undefined' && curDate !== 'NaN-NaN-NaN')
      {
        this.getCalenderId(curDate);
      }
      else{
        window.alert("V??lg en dato f??rst");
      }
  }
  
  

  getCalenderId(startDate: string)  {
    this.loading = true;
    console.log("getCalenderId kaldt"); 
    this.calenderService.getCalendarId(startDate).subscribe(response => {
      if (response != null && response.ok)
      {
        console.log("getCalenderId - response:",response);
        console.log("getCalenderId - response body:",response.body);
        this.curCalendarId = response.body;
        console.log("getCalenderId - OK:",response.ok);
        this.createCalendar(this.curCalendarId,this.currentTaskstartdate()!);  
      }
      else {
        this.serviceStatus = "Fejl opst??et:" + response.statusText;
        this.loading = false;
      }
    });
  }
  createCalendar(id: string, startDate: string) {
    console.log("createCalendar kaldt");
    this.calenderService.createCalendar(id,startDate).subscribe(response => {
      if (response != null && response.ok)
      {
        console.log("createCalendar - response:",response);
        console.log("createCalendar - response body:",response.body);
        console.log("createCalendar - OK:",response.ok);
        this.calendarHtml = response.body;
        this.loading = false;
      }
      else {
        this.serviceStatus = "Fejl opst??et:" + response.statusText;
        this.loading = false;
      }
      
    });
  }

}
