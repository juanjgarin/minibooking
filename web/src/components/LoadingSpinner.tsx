import { ProgressSpinner } from 'primereact/progressspinner';

type LoadingSpinnerProps = {
  label?: string;
};

export function LoadingSpinner({ label = 'Loading…' }: LoadingSpinnerProps) {
  return (
    <div className="flex flex-column align-items-center justify-content-center gap-3 py-6">
      <ProgressSpinner style={{ width: '2.5rem', height: '2.5rem' }} strokeWidth="4" />
      <span className="text-sm text-color-secondary">{label}</span>
    </div>
  );
}
