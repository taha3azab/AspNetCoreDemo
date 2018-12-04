import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Value } from '../shared/models/value.model';

@Injectable()
export class WebService {
  constructor(private https: HttpClient) {}

  getValues() {
    return this.https.get('https://localhost:5001/api/values').toPromise();
  }

  addValue(value: Value) {
    return this.https
      .post('https://localhost:5001/api/values', value)
      .toPromise();
  }
}
