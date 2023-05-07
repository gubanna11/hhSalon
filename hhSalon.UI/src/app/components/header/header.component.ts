import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { GroupsService } from 'src/app/services/groups.service';
import { SharedService } from 'src/app/services/shared.service';
import { UserStoreService } from 'src/app/services/user-store.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None 
})
export class HeaderComponent {
  isAuthorized: boolean = this.auth.isLoggedIn();
  
  subscription: Subscription | undefined;

  public fullName: string = "";
  public role!: string;
  public id!: string;

  constructor(private groupsService: GroupsService,
              private sharedService: SharedService,
              private router: Router,
              private auth: AuthService,
              private userStore: UserStoreService,
              public chatService: ChatService,
      ){}
 
  ngOnInit(): void {
    this.subscription = this.sharedService.getData().subscribe(isLoggedIn => this.isAuthorized = isLoggedIn);

    this.userStore.getFullNameFromStore().subscribe(
      name => {
        const fullNameFromToken = this.auth.getFullNameFromToken();
        this.fullName = name || fullNameFromToken;
        console.log(this.fullName);;
      })

    this.userStore.getRoleFromStore().subscribe(role => {      
      const roleFromToken = this.auth.getRoleFromToken();      
      this.role = role || roleFromToken;

      
      // this.role = (this.auth.decodedToken()).role;
    })


    this.userStore.getIdFromStore().subscribe(value => {      
      const idFromToken = this.auth.getIdFromToken();      
      const id = value || idFromToken;
      this.id = id;
      this.chatService.addUser(id).subscribe(
        () => {
          this.chatService.userId = id;
          this.chatService.createChatConnection(id);
        }              
      )
    })
    
  }

  ngOnDestroy(): void {
   this.subscription?.unsubscribe();
   //this.chatService.closePrivateChatMessage();
  }

  signOut(){
    this.isAuthorized = false;
    this.auth.signOut(); 
    
    this.userStore.setRoleForStore("Client");

    this.chatService.closePrivateChatMessage();

    this.router.navigate(['login']);
  }
}
