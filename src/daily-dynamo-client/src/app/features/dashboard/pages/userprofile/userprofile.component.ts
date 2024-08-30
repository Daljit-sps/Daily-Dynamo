import { Component, OnInit, inject } from '@angular/core';
import { UpdateUserprofileComponent } from '../../components/update-userprofile/update-userprofile.component';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.scss'],
})

export class UserprofileComponent implements OnInit{
    constructor(private route: ActivatedRoute, private router: Router) {} 
    userId: string | undefined;
    selectedOption: string = '';

    ngOnInit(): void {
      this.route.queryParams.subscribe(params => {
        const uId = params['id'];
        if (uId) {
          this.userId = uId;
          this.redirectToProfilePage(this.userId);
        }
        else{
          this.userId = undefined;
        }
      });
    }


    redirectToProfilePage(userId: string | undefined){
      this.selectedOption = 'profile';
      if(userId){
        this.router.navigate(['/dashboard/userprofile/update-userprofile'], { queryParams: { id: userId} });
      }
      else{
        this.router.navigate(['/dashboard/userprofile/update-userprofile']);
      }
      
    }

    redirectToAccountSettingsPage(userId: string | undefined){
      this.selectedOption = 'accountSettings';
      if(userId){
        this.router.navigate(['/dashboard/userprofile/user-accountsettings'], { queryParams: { id: userId } });
      }
      else{
        this.router.navigate(['/dashboard/userprofile/user-accountsettings']);
      }
      
    }
}


