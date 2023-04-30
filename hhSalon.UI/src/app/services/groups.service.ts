import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import { Group } from '../models/group';

@Injectable({
  providedIn: 'root'
})
export class GroupsService {
  private url = 'Groups';

  constructor(private http: HttpClient) { }

  public getGroups(): Observable<Group[]>{
    return this.http.get<Group[]>(`${environment.apiUrl}/${this.url}`);
  }

  public getGroupById(groupId: number): Observable<Group>{
    return this.http.get<Group>(`${environment.apiUrl}/${this.url}/${groupId}`);
  }

  public createGroup(group: Group): Observable<Group[]>{
    return this.http.post<Group[]>(
      `${environment.apiUrl}/${this.url}`,
      group);
  }

  public updateGroup(group: Group): Observable<Group[]>{
    return this.http.put<Group[]>(
      `${environment.apiUrl}/${this.url}`,group
      );
  }

  public deleteGroup(group: Group): Observable<Group[]>{
      return this.http.delete<Group[]>(
      `${environment.apiUrl}/${this.url}/${group.id}`
      );
  }
}
