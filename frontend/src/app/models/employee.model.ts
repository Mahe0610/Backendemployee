export type UserRole = 'Admin' | 'User';

export interface Employee {
  id: number;
  employeeName: string;
  age: number;
  dateOfBirth: string;
  email: string;
  salary?: number | null;
  role: UserRole;
}

export interface LoginResponse {
  username: string;
  role: UserRole;
}
