/// <reference types="vite/client" />

declare module '*?url' {
  const src: string;
  export default src;
}

interface ImportMetaEnv {
  readonly VITE_BOOKING_API_URL: string;
  readonly VITE_CUSTOMER_API_URL: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
