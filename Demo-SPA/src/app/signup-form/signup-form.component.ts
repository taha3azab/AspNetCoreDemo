import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UsernameValidators } from '../shared/validators/username.validators';
import { AuthService } from '../_services/auth.service';
import { UserForRegister } from '../shared/models/user-for-register.model';

@Component({
  selector: 'signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent {
  form = new FormGroup({
    username: new FormControl(
      '',
      [
        Validators.required,
        Validators.minLength(3),
        UsernameValidators.cannotContaineSpace
      ],
      [UsernameValidators.shouldBeUnique(this.authService)]  // async validators
    ),
    password: new FormControl('', [Validators.required])
  });

  constructor(private authService: AuthService) {}

  signup() {
    if (this.form.valid) {
      const user = this.form.value as UserForRegister;
      this.authService.signup(user).subscribe(
        () => {},
        error => {
          this.form.setErrors({
            invalidSignup: {
              errorMessage: error
            }
          });
        },
        () => {}
      );
    }
  }

  get username() {
    return this.form.get('username');
  }
}
