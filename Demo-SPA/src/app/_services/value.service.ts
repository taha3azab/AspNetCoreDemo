import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { Value } from '../shared/models/value.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ValueService extends DataService<Value> {
  constructor(https: HttpClient) {
    super('https://localhost:5001/api/values/', https);
  }
}
