import type { ReactNode } from 'react';

type EmptyStateProps = {
  icon?: string;
  title: string;
  description?: string;
  action?: ReactNode;
};

export function EmptyState({ icon = 'pi pi-inbox', title, description, action }: EmptyStateProps) {
  return (
    <div className="flex flex-column align-items-center justify-content-center text-center py-6 px-3 border-round surface-card border-1 surface-border">
      <i className={`${icon} text-4xl text-color-secondary mb-2`} />
      <h3 className="m-0 text-lg font-medium text-color">{title}</h3>
      {description ? (
        <p className="m-0 mt-2 mb-3 text-sm text-color-secondary max-w-30rem line-height-3">{description}</p>
      ) : null}
      {action}
    </div>
  );
}
