import { UserRole } from '../features/shared/types/role.enum';

export type TUser = {
  email: string;
  id: string;
  userRole: UserRole;
  tokenValidity: Date;
  token: string;
  userName: string;
};

export enum EAuthStatus {
  Loading = 'loading',
  Pending = 'pending',
  Error = 'error',
}

export type TAuth = {
  user: TUser | null;
  status: EAuthStatus;
  message: string;
};

export const AuthKey: string = 'auth';
