import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit{
  users: User[] = [];
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();
  availableRoles = [
    'Admin',
    'Moderator',
    'Member'
  ];

  constructor( private adminService: AdminService, private modalService: BsModalService ) {  }

  ngOnInit(): void { this.getUsersWithRoles(); }

  getUsersWithRoles(){
    this.adminService.getUsersWithRoles().subscribe({
      next: usrs => this.users = usrs
    });
  }

  openRolesModal(usr: User){
    const config ={
      class: 'modal-dialog-centered',
      initialState: {
        username: usr.username,
        availabeRoles: this.availableRoles,
        selectedRoles: [...usr.roles]
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        const selectedRoles = this.bsModalRef.content?.selectedRoles;
        if(!this.arrayEqual(selectedRoles!, usr.roles)){
          this.adminService.updateUserRoles(usr.username, selectedRoles!.join(',')).subscribe({
            next: roles => usr.roles = roles
          })
        }
      }
    })
  }

  private arrayEqual(arr1: any[] , arr2: any[]){
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
  }

}
