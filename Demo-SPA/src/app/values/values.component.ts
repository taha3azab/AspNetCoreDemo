import { Component, OnInit } from '@angular/core';
import { WebService } from '../_services/web.service';
import { Value } from '../shared/models/value.model';

@Component({
  selector: 'app-values',
  templateUrl: './values.component.html',
  styleUrls: ['./values.component.css']
})
export class ValuesComponent implements OnInit {
  values: Value[];
  constructor(private webService: WebService) {}

  async ngOnInit() {
    const response = await this.webService.getValues();
    console.log(response);
    this.values = response.items;
  }
}
