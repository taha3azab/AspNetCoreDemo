import { Component, OnInit } from '@angular/core';
import { WebService } from '../services/web.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;

  constructor(private webService: WebService) {}

  async ngOnInit() {}
  registerToggle() {
    this.registerMode = true;
  }
  cancelRegisterMode() {
    this.registerMode = false;
  }
}
