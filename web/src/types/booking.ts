export enum BookingStatus {
  Pending = 1,
  Confirmed = 2,
  Cancelled = 3,
}

export const bookingStatusLabel: Record<BookingStatus, string> = {
  [BookingStatus.Pending]: 'Pending',
  [BookingStatus.Confirmed]: 'Confirmed',
  [BookingStatus.Cancelled]: 'Cancelled',
};

export interface BookingResponse {
  id: string;
  customerId: string;
  spaceName: string;
  startDate: string;
  endDate: string;
  status: string;
  createdAt: string;
}

export interface SaveBookingRequest {
  id?: string | null;
  customerId: string;
  spaceName: string;
  startDate: string;
  endDate: string;
  status: BookingStatus;
}

export interface ChangeBookingStatusRequest {
  id: string;
  status: BookingStatus;
}
