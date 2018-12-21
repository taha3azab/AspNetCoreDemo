import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Value } from '../shared/models/value.model';
import { PagedList } from '../shared/interfaces/paged-list.interface';

@Injectable()
export class WebService {
  baseUrl = 'https://localhost:5001/api/values';
  constructor(private https: HttpClient) {}

  getValues() {
    return this.https.get<PagedList<Value>>(this.baseUrl).toPromise();
  }

  addValue(value: Value) {
    return this.https.post(this.baseUrl, value).toPromise();
  }
}
