import { Component, OnInit } from '@angular/core';
import { UsersService } from '../_services/users.service';
import { User } from '../shared/models/user.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[];
  constructor(
    private rout: ActivatedRoute,
    private usersService: UsersService
  ) {}

  async ngOnInit() {
    this.rout.queryParamMap.subscribe(async params => {
      const page = +params.get('page');
      const size = +params.get('size');
      const pagedList = await this.usersService.getAll(page, size);
      this.users = pagedList.items;
    });
  }
}
