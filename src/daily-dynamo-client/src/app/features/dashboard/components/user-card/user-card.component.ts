import { Component, Input } from '@angular/core';
import { TUser } from '../../types/user.type';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'dd-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.scss'],
})
export class UserCardComponent {
  @Input({ required: true }) user!: TUser;
  imgFunc(imgPath:any): string{
    if(imgPath != null)
      return environment.API_BASE_URL + '/' + imgPath;
    else
      return 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT4eMoz7DH8l_Q-iCzSc1xyu_C2iryWh2O9_FcDBpY04w&s';
    
  }
}
