import axios from 'axios';

export function getApiErrorMessage(err: unknown): string {
  if (axios.isAxiosError(err)) {
    const data = err.response?.data as { detail?: string; title?: string } | undefined;
    if (data?.detail) return data.detail;
    if (data?.title) return data.title;
    return err.message;
  }
  if (err instanceof Error) return err.message;
  return 'Unexpected error';
}
