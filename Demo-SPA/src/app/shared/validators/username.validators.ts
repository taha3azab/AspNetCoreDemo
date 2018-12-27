import { AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/_services/auth.service';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/observable/timer';

export class UsernameValidators {

  static cannotContaineSpace(control: AbstractControl): ValidationErrors | null {
    if ((control.value as string).indexOf(' ') >= 0) {
      return { cannotContaineSpace: true };
    }
    return null;
  }

  static shouldBeUnique(auth: AuthService) {
    return (control: AbstractControl): | Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
      const debounceTime = 500; // milliseconds
      return Observable.timer(debounceTime).switchMap(() => {
        return auth.userIsExist(control.value).map(isExist => {
          return isExist ? { shouldBeUnique: isExist } : null;
        });
    });
    };
  }
}
