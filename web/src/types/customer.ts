export interface CustomerResponse {
  id: string;
  fullName: string;
  email: string;
  phone: string;
  createdAt: string;
}

export interface SaveCustomerRequest {
  id?: string | null;
  fullName: string;
  email: string;
  phone: string;
}
