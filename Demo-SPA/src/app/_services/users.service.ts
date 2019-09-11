import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { User } from '../shared/models/user.model';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class UsersService extends DataService<User> {
  constructor(https: HttpClient) {
    super('http://localhost:7000/demo-api/users', https);
  }
}
