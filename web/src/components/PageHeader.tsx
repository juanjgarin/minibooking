import type { ReactNode } from 'react';

type PageHeaderProps = {
  title: string;
  description?: string;
  actions?: ReactNode;
};

export function PageHeader({ title, description, actions }: PageHeaderProps) {
  return (
    <div className="flex flex-column md:flex-row md:align-items-center md:justify-content-between gap-2 mb-3">
      <div>
        <h1 className="m-0 text-xl font-semibold text-color">{title}</h1>
        {description ? (
          <p className="m-0 mt-1 text-sm text-color-secondary line-height-3">{description}</p>
        ) : null}
      </div>
      {actions ? <div className="flex align-items-center gap-2 flex-wrap">{actions}</div> : null}
    </div>
  );
}
