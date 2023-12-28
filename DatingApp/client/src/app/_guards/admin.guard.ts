import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { map } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toast = inject(ToastrService);


  return accountService.currentUser$.pipe(
    map(usr => {
      if(!usr) return false;
      if(usr.roles.includes('Admin') || usr.roles.includes('Moderator') ){
        return true;
      }
      else{
        toast.error('You cannot access this area.');
        return false;
      }
    })
  )
};
