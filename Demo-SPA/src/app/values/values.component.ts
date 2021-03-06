import { Component, OnInit } from '@angular/core';
import { ValueService } from '../_services/value.service';
import { Value } from '../shared/models/value.model';

@Component({
  selector: 'app-values',
  templateUrl: './values.component.html',
  styleUrls: ['./values.component.css']
})
export class ValuesComponent implements OnInit {
  values: Value[];
  constructor(private valueService: ValueService) {}

  async ngOnInit() {
    const pagedList = await this.valueService.getAll(0, 10);
    this.values = pagedList.items;
  }
}
