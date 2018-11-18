import { IsNotEmpty } from 'class-validator';

export class Value {
  public id: number;

  @IsNotEmpty()
  public name: string;

  constructor(id?: number, name?: string) {
    this.id = id || 0;
    this.name = name || '';
  }
}
