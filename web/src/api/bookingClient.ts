import axios from 'axios';

const base = import.meta.env.VITE_BOOKING_API_URL?.replace(/\/$/, '') ?? '';

export const bookingClient = axios.create({
  baseURL: base,
  headers: { 'Content-Type': 'application/json' },
  timeout: 30_000,
});
