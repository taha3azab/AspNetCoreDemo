import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Value } from '../shared/models/value.model';

@Injectable()
export class WebService {
  constructor(private https: Http) {}

  getValues() {
    return this.https.get('https://localhost:5001/api/values').toPromise();
  }

  addValue(value: Value) {
    return this.https
      .post('https://localhost:5001/api/values', value)
      .toPromise();
  }
}
