export interface CustomerRequest {
  fullName: string;
  email: string;
  phone: string;
  address: string;
  aadharNumber: string;
  dateOfBirth: string;
}

export interface CustomerResponse {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  address: string;
  aadharNumber: string;
  dateOfBirth: string;
  joinDate: string;
  isActive: boolean;
}
