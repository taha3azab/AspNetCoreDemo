import { IsNotEmpty, IsEmail } from 'class-validator';

export class Value {
  public id: number;

  @IsNotEmpty()
  public name: string;

  @IsEmail()
  public email: string;

  constructor(id?: number, name?: string, email?: string) {
    this.id = id || 0;
    this.name = name || '';
    this.email = email || '';
  }
}
