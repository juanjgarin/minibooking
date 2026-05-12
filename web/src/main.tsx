import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { PrimeReactProvider } from 'primereact/api';
import 'primereact/resources/primereact.min.css';
import 'primeflex/primeflex.css';
import 'primeicons/primeicons.css';
import App from '@/App';
import { ThemeProvider } from '@/theme/ThemeContext';
import { ToastProvider } from '@/components/providers/ToastProvider';
import '@/theme/app.css';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <PrimeReactProvider value={{ ripple: true }}>
      <ThemeProvider>
        <ToastProvider>
          <App />
        </ToastProvider>
      </ThemeProvider>
    </PrimeReactProvider>
  </StrictMode>,
);
