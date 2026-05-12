import axios from 'axios';

const base = import.meta.env.VITE_CUSTOMER_API_URL?.replace(/\/$/, '') ?? '';

export const customerClient = axios.create({
  baseURL: base,
  headers: { 'Content-Type': 'application/json' },
  timeout: 30_000,
});
