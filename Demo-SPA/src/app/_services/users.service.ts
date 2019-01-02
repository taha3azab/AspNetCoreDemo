import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { User } from '../shared/models/user.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsersService extends DataService<User> {
  constructor(https: HttpClient) {
    super('https://localhost:5001/api/users/', https);
  }
}
