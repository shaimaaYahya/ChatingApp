import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { MemberCardComponent } from "../member-card/member-card.component";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../_services/account.service';
import { UserParams } from '../../_models/userParams';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule, ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit{
  // private accountService = inject(AccountService);
 memberService = inject(MembersService);
//  userParams = new UserParams(this.accountService.currentUser());
 genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}]
  // members: Member[] = [];
  // pageNumber = 1;
  // pageSize = 5;

  ngOnInit(): void {
    //if(this.memberService.members().length === 0)
    if(!this.memberService.paginationResult())
    this.loadMembers();
  }

  loadMembers(){
    this.memberService.getMembers();
  }

  resetFilters(){
    // this.userParams = new UserParams(this.accountService.currentUser());
    this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any){
    if(this.memberService.userParams().pageNumber !== event.page){
      this.memberService.userParams().pageNumber = event.page;
      this.loadMembers();
    }
  }

}
