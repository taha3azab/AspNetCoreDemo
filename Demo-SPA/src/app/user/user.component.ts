import { Component, OnInit } from '@angular/core';
import { User } from '../shared/models/user.model';
import { UsersService } from '../_services/users.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  user: User;
  constructor(private rout: ActivatedRoute, private usersService: UsersService) {}

  ngOnInit() {
    this.rout.paramMap.subscribe(async params => {
      const id = params.get('id');
      this.user = await this.usersService.getById(id);
    });
  }

}
