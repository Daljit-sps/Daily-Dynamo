
import { jwtDecode } from 'jwt-decode';
import { TUser } from '../types/user.type';


export function decryptToken(res: any): TUser | null {
    try {
        if(res.data == null)
          return null;
        const decodedToken: any = jwtDecode(res.data.token);
        const user: TUser = {
          email: decodedToken.Email,
          id: decodedToken.Id,
          userRole: decodedToken.Role,
          tokenValidity: new Date(decodedToken.exp * 1000),
          token: res.data.token,
          userName: decodedToken.Name,
        };
        console.log(decodedToken)
        console.log(user)
        return user;
      } catch (error) {
        console.error('Error decoding token:', error);
        return null;
      }
}
