import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  //toastr.info(accountService.currentUser.token);
  //toastr.info(accountService.roles() ? accountService.roles()?.toString() : "");
  if(accountService.roles()?.includes("Admin") || accountService.roles()?.includes('Moderator')){
    return true;
  } else {
    toastr.error("You cannot enter this area");
    return false;
  }
};
