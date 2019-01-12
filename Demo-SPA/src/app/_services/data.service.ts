import { BadInput } from './../common/bad-input';
import { NotFoundError } from './../common/not-found-error';
import { AppError } from './../common/app-error';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/retry';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PagedList } from '../shared/interfaces/paged-list.interface';

@Injectable()
export class DataService<T> {
  constructor(private url: string, private https: HttpClient) {}

  getAll(pageIndex: number, pageSize: number) {
    pageIndex = pageIndex || 0;
    pageSize = pageSize || 10;
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.https
      .get<PagedList<T>>(this.url, { params })
      .retry(3)
      .catch(this.handleError)
      .toPromise();
  }

  getById(id: string) {
    return this.https
      .get<T>(this.url + id)
      .catch(this.handleError)
      .toPromise();
  }

  create(resource: T) {
    return this.https
      .post<T>(this.url, resource)
      .catch(this.handleError)
      .toPromise();
  }

  update(resource: { id: string }) {
    return this.https
      .patch(this.url + '/' + resource.id, JSON.stringify({ isRead: true }))
      .map((response: Response) => response.json())
      .catch(this.handleError)
      .toPromise();
  }

  delete(id: string) {
    return this.https
      .delete<T>(this.url + '/' + id)
      .catch(this.handleError)
      .toPromise();
  }

  private handleError(error: Response) {
    if (error.status === 400) {
      return Observable.throw(new BadInput(error.json()));
    }

    if (error.status === 404) {
      return Observable.throw(new NotFoundError());
    }

    return Observable.throw(new AppError(error));
  }
}
