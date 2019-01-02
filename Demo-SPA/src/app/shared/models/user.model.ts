import {} from 'class-validator';

export class User {
  public id: number;

  public username: string;

  public gender: string;

  public age: number;

  public knownAs: string;

  public created: Date;

  public lastActive: Date;

  public city: string;

  public country: string;

  public photoUrl: string;

  constructor(
    id?: number,
    username?: string,
    gender?: string,
    age?: number,
    knownAs?: string,
    created?: Date,
    lastActive?: Date,
    city?: string,
    country?: string,
    photoUrl?: string,
    ) {
    this.id = id || 0;
    this.username = username || '';
    this.gender = gender || '';
    this.age = age || 0;
    this.knownAs = knownAs || '';
    this.created = created || null;
    this.lastActive = lastActive || null;
    this.city = city || '';
    this.country = country || '';
    this.photoUrl = photoUrl || '';
  }
}
