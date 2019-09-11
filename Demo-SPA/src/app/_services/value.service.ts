import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { Value } from '../shared/models/value.model';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ValueService extends DataService<Value> {
  constructor(https: HttpClient) {
    super('http://localhost:7000/demo-api/values/', https);
  }
}
