import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { validate, ValidationError } from 'class-validator';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { UserForRegister } from '../shared/models/user-for-register.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model = new UserForRegister();

  constructor(private auth: AuthService, private alertify: AlertifyService) {}

  ngOnInit() {}

  register() {
    validate(this.model).then(errors => {
      if (errors.length > 0) {
        console.log('validation failed. errors: ', errors);
        this.alertify.error(errors[0].constraints[Object.keys(errors[0].constraints)[0]]);
      } else {
        console.log('validation succeed');
        this.auth.signup(this.model).subscribe(
          () => {
            this.alertify.success('registeration successfully');
          },
          error => {
            this.alertify.error(error);
          }
        );
      }
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
