import { IsNotEmpty, MinLength, MaxLength } from 'class-validator';

export class UserForRegister {
  @IsNotEmpty()
  public username: string;

  @IsNotEmpty()
  @MinLength(4)
  @MaxLength(8)
  public password: string;

  constructor(username?: string, password?: string) {
    this.username = username || '';
    this.password = password || '';
  }
}
