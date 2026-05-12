import { useCallback, useMemo, useState } from 'react';
import { NavLink, Outlet, useLocation } from 'react-router-dom';
import { Button } from 'primereact/button';
import { Menubar } from 'primereact/menubar';
import { Sidebar } from 'primereact/sidebar';
import { useThemeMode } from '@/theme/ThemeContext';
import { AppConfirmRoot } from '@/components/AppConfirmRoot';

const navCls = ({ isActive }: { isActive: boolean }) =>
  `p-menuitem-link flex align-items-center gap-2 py-2 px-2 border-round no-underline text-sm ${
    isActive ? 'bg-primary text-primary-contrast font-medium' : 'text-color hover:surface-hover'
  }`;

function NavLinks({ onNavigate }: { onNavigate?: () => void }) {
  return (
    <nav className="flex flex-column gap-1">
      <NavLink to="/dashboard" className={navCls} onClick={onNavigate}>
        <i className="pi pi-th-large" />
        <span>Dashboard</span>
      </NavLink>
      <NavLink to="/customers" className={navCls} onClick={onNavigate}>
        <i className="pi pi-users" />
        <span>Customers</span>
      </NavLink>
      <NavLink to="/bookings" className={navCls} onClick={onNavigate}>
        <i className="pi pi-calendar" />
        <span>Bookings</span>
      </NavLink>
    </nav>
  );
}

export function DashboardLayout() {
  const { mode, toggle } = useThemeMode();
  const [mobileOpen, setMobileOpen] = useState(false);
  const location = useLocation();

  const start = useMemo(
    () => (
      <div className="flex align-items-center gap-2">
        <Button
          type="button"
          icon="pi pi-bars"
          className="md:hidden"
          text
          rounded
          aria-label="Open menu"
          onClick={() => setMobileOpen(true)}
        />
        <div className="flex align-items-center gap-2">
          <div
            className="flex align-items-center justify-content-center border-circle bg-primary text-primary-contrast"
            style={{ width: '2rem', height: '2rem' }}
          >
            <i className="pi pi-bookmark text-sm" />
          </div>
          <span className="font-semibold text-lg tracking-tight">MiniBooking</span>
        </div>
      </div>
    ),
    [],
  );

  const end = useMemo(
    () => (
      <div className="flex align-items-center gap-1">
        <Button
          type="button"
          icon={mode === 'dark' ? 'pi pi-sun' : 'pi pi-moon'}
          text
          rounded
          onClick={toggle}
          aria-label="Toggle theme"
        />
      </div>
    ),
    [mode, toggle],
  );

  const menubar = useMemo(
    () => (
      <Menubar
        className="surface-card border-noround border-none border-bottom-1 surface-border mb-0 shadow-none"
        style={{ padding: '0.5rem 0.75rem' }}
        start={start}
        end={end}
      />
    ),
    [start, end],
  );

  const hideMobile = useCallback(() => setMobileOpen(false), []);

  return (
    <div className="min-h-screen surface-ground flex flex-column">
      <AppConfirmRoot />
      {menubar}
      <div className="flex flex-1 overflow-hidden" style={{ minHeight: 0 }}>
        <aside
          className="hidden md:flex surface-card border-right-1 surface-border flex-column"
          style={{ width: '15rem', minWidth: '15rem' }}
        >
          <div className="p-2 border-bottom-1 surface-border">
            <span className="text-xs font-medium text-color-secondary uppercase">Navigation</span>
          </div>
          <div className="p-2 flex-1 overflow-auto">
            <NavLinks />
          </div>
        </aside>

        <Sidebar
          visible={mobileOpen}
          onHide={() => setMobileOpen(false)}
          className="w-18rem md:hidden"
          header="Menu"
        >
          <div className="p-2">
            <NavLinks onNavigate={hideMobile} />
          </div>
        </Sidebar>

        <main className="flex-1 overflow-auto p-2 md:p-3" style={{ minWidth: 0 }}>
          <div className="mx-auto" style={{ maxWidth: '1200px' }}>
            <Outlet key={location.pathname} />
          </div>
        </main>
      </div>
    </div>
  );
}
