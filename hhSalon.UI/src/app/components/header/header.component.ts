import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChild, ViewChildren, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { GroupsService } from 'src/app/services/groups.service';
import { SharedService } from 'src/app/services/shared.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { UsersService } from 'src/app/services/users.service';
import { GroupsMenuComponent } from '../groups-menu/groups-menu.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None 
})
export class HeaderComponent implements OnInit, OnDestroy, AfterViewInit {
  isAuthorized: boolean = this.auth.isLoggedIn();
  
  subscription: Subscription | undefined;
  
  public fullName: string = "";
  public role!: string;
  public id!: string;
  public user: any;


  @ViewChildren('menu__link_name') menuArrows!: QueryList<ElementRef>;
  @ViewChildren('menu__link') menu_links!: QueryList<ElementRef>;

  @ViewChild('menu_icon') menu_icon!: ElementRef;
  @ViewChildren('link') links!: QueryList<ElementRef>;
  @ViewChild(GroupsMenuComponent) group_links!: GroupsMenuComponent;

  constructor(private groupsService: GroupsService,
              private sharedService: SharedService,
              private router: Router,
              private auth: AuthService,
              private userStore: UserStoreService,
              public chatService: ChatService,
              private usersService: UsersService
      ){}

  ngAfterViewInit(): void {
    let header_menu = document.getElementsByClassName('header__menu')[0];
    this.menu_icon.nativeElement.addEventListener('click', function(){
      header_menu.classList.toggle('active');    
    })

    this.links.forEach(l =>{
      l.nativeElement.addEventListener('click', function(){
        header_menu.classList.remove('active');
      })
    })

let groups_links :QueryList<ElementRef<HTMLDivElement>>= this.group_links.menu_links;
   
      groups_links.forEach(l =>{
      l.nativeElement.addEventListener('click', function(){
        console.log('CLICK');
        
        header_menu.classList.remove('active');
      })
    })
  }
 
  ngOnInit(): void {
    this.subscription = this.sharedService.getData().subscribe(isLoggedIn => this.isAuthorized = isLoggedIn);

    this.userStore.getFullNameFromStore().subscribe(
      name => {
        const fullNameFromToken = this.auth.getFullNameFromToken();
        this.fullName = name || fullNameFromToken;
      })

    this.userStore.getRoleFromStore().subscribe(role => {      
      const roleFromToken = this.auth.getRoleFromToken();      
      this.role = role || roleFromToken;
    })


    this.userStore.getIdFromStore().subscribe(value => {      
      const idFromToken = this.auth.getIdFromToken();      
      const id = value || idFromToken;
      this.id = id;

      if(id){
        this.usersService.getUserById(this.id).subscribe(
          (user) => {
              this.user = user;
          }
        )
  
        this.chatService.addUser(id).subscribe(
          () => {
            this.chatService.userId = id;
            this.chatService.createChatConnection(id);
          }              
        )
  
      }
    })
    
  }

  ngOnDestroy(): void {
   this.subscription?.unsubscribe();
  }

  signOut(){
    this.isAuthorized = false;
     this.auth.signOut(); 
    
     this.userStore.setRoleForStore("Client");

     this.chatService.stopChatConnection();

    this.router.navigate(['/login']);
  }
}
