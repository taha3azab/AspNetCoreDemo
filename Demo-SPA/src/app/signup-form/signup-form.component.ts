import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UsernameValidators } from '../shared/validators/username.validators';
import { AuthService } from '../_services/auth.service';

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
      UsernameValidators.shouldBeUnique
    ),
    password: new FormControl('', Validators.required)
  });

  constructor(private authService: AuthService) {}

  signup() {
    this.authService.signup(this.form.value).subscribe(
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

  get username() {
    return this.form.get('username');
  }
}
