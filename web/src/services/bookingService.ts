import { bookingClient } from '@/api/bookingClient';
import type {
  BookingResponse,
  ChangeBookingStatusRequest,
  SaveBookingRequest,
} from '@/types/booking';

const prefix = '/api/bookings';

export async function fetchBookings(): Promise<BookingResponse[]> {
  const { data } = await bookingClient.get<BookingResponse[]>(prefix);
  return data;
}

export async function upsertBooking(body: SaveBookingRequest): Promise<BookingResponse> {
  const { data } = await bookingClient.post<BookingResponse>(`${prefix}/upsert`, body);
  return data;
}

export async function changeBookingStatus(body: ChangeBookingStatusRequest): Promise<void> {
  await bookingClient.put(`${prefix}/changeStatus`, body);
}
