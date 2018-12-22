import { AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/_services/auth.service';

export class UsernameValidators {

  static cannotContaineSpace(
    control: AbstractControl
  ): ValidationErrors | null {
    if ((control.value as string).indexOf(' ') >= 0) {
      return { cannotContaineSpace: true };
    }
    return null;
  }

  static shouldBeUnique(
    control: AbstractControl
  ): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    // return this.authService.userIsExist(control.value as string).
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        if (control.value === 'taha') {
          resolve({ shouldBeUnique: true });
        } else {
          resolve(null);
        }
      }, 2000);
    });
  }
}
