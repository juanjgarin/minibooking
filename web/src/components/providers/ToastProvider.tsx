import { createContext, useCallback, useContext, useMemo, useRef, type ReactNode } from 'react';
import { Toast } from 'primereact/toast';
import type { ToastMessage } from 'primereact/toast';

type ToastContextValue = {
  show: (message: Omit<ToastMessage, 'life'>, life?: number) => void;
  success: (summary: string, detail?: string) => void;
  error: (summary: string, detail?: string) => void;
  warn: (summary: string, detail?: string) => void;
};

const ToastContext = createContext<ToastContextValue | null>(null);

export function ToastProvider({ children }: { children: ReactNode }) {
  const ref = useRef<Toast>(null);

  const show = useCallback((message: Omit<ToastMessage, 'life'>, life = 3500) => {
    ref.current?.show({ ...message, life });
  }, []);

  const success = useCallback(
    (summary: string, detail?: string) => show({ severity: 'success', summary, detail }),
    [show],
  );
  const error = useCallback(
    (summary: string, detail?: string) => show({ severity: 'error', summary, detail }),
    [show],
  );
  const warn = useCallback(
    (summary: string, detail?: string) => show({ severity: 'warn', summary, detail }),
    [show],
  );

  const value = useMemo(
    () => ({
      show,
      success,
      error,
      warn,
    }),
    [show, success, error, warn],
  );

  return (
    <ToastContext.Provider value={value}>
      <Toast ref={ref} position="top-right" />
      {children}
    </ToastContext.Provider>
  );
}

export function useAppToast() {
  const ctx = useContext(ToastContext);
  if (!ctx) throw new Error('useAppToast must be used within ToastProvider');
  return ctx;
}
