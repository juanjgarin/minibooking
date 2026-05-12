import { customerClient } from '@/api/customerClient';
import type { CustomerResponse, SaveCustomerRequest } from '@/types/customer';

const prefix = '/api/customers';

export async function fetchCustomers(): Promise<CustomerResponse[]> {
  const { data } = await customerClient.get<CustomerResponse[]>(prefix);
  return data;
}

export async function upsertCustomer(body: SaveCustomerRequest): Promise<CustomerResponse> {
  const { data } = await customerClient.post<CustomerResponse>(`${prefix}/upsert`, body);
  return data;
}

export async function deleteCustomer(id: string): Promise<void> {
  await customerClient.delete(`${prefix}/${id}`);
}
