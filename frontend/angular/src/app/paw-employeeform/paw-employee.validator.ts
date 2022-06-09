
import { EmployeeService } from './../services/employee.service';
import { AbstractControl, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { HttpClient, HttpResponse } from '@angular/common/http';
import { IEmployee } from '../shared/IEmployee';
import { Injector, ReflectiveInjector} from '@angular/core';
import { ThisReceiver } from '@angular/compiler';
import { AppInjector } from '../app.module';
import { map, Observable } from 'rxjs';


export class PawEmployeeValidator {
    
    static emailMustBeUnique(control: AbstractControl): ValidationErrors | null {
        let emailIsUnique = false;    
        if ((control.value as string) != null && (control.value as string).length > 0)
        {  
            console.log('emailMustBeUnique - getting EmployeeService:' + (control.value as string))
            const myService = AppInjector.get(EmployeeService);            
            console.log('emailMustBeUnique - email:' + (control.value as string))
 
        
            myService.getEmployeeByEmail(control.value as string).subscribe((response: HttpResponse<IEmployee>) => {
                console.log("emailMustBeUnique -> response:" + response.ok);
                if (response.ok) {
                    emailIsUnique = false;
                }
              }); 
            
            if (emailIsUnique) 
            {
                console.log("return true");
                return {emailMustBeUnique:true};
            }    
        }
        console.log("return null");
        return null; 
    }
}