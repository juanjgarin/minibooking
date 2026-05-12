import { useEffect, useMemo, useState } from 'react';
import { Button } from 'primereact/button';
import { Calendar } from 'primereact/calendar';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';
import { Tag } from 'primereact/tag';
import { Toolbar } from 'primereact/toolbar';
import { EmptyState } from '@/components/EmptyState';
import { LoadingSpinner } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { useAppToast } from '@/hooks/useAppToast';
import { changeBookingStatus, fetchBookings, upsertBooking } from '@/services/bookingService';
import { fetchCustomers } from '@/services/customerService';
import {
  BookingStatus,
  bookingStatusLabel,
  type BookingResponse,
  type SaveBookingRequest,
} from '@/types/booking';
import type { CustomerResponse } from '@/types/customer';
import { getApiErrorMessage } from '@/utils/apiError';
import { formatDateTime } from '@/utils/formatDate';

const statusOptions = [
  { label: bookingStatusLabel[BookingStatus.Pending], value: BookingStatus.Pending },
  { label: bookingStatusLabel[BookingStatus.Confirmed], value: BookingStatus.Confirmed },
  { label: bookingStatusLabel[BookingStatus.Cancelled], value: BookingStatus.Cancelled },
];

function statusSeverity(status: string): 'warning' | 'success' | 'danger' | 'info' {
  const s = status.toLowerCase();
  if (s === 'pending') return 'warning';
  if (s === 'confirmed') return 'success';
  if (s === 'cancelled') return 'danger';
  return 'info';
}

function parseApiStatus(s: string): BookingStatus {
  const key = s.toLowerCase();
  if (key === 'confirmed') return BookingStatus.Confirmed;
  if (key === 'cancelled') return BookingStatus.Cancelled;
  return BookingStatus.Pending;
}

const emptyForm = (): Omit<SaveBookingRequest, 'id'> => ({
  customerId: '',
  spaceName: '',
  startDate: new Date().toISOString(),
  endDate: new Date(Date.now() + 3600000).toISOString(),
  status: BookingStatus.Pending,
});

export function BookingsPage() {
  const toast = useAppToast();
  const [bookings, setBookings] = useState<BookingResponse[]>([]);
  const [customers, setCustomers] = useState<CustomerResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [selected, setSelected] = useState<BookingResponse | null>(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [saving, setSaving] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);
  const [form, setForm] = useState(emptyForm);

  const loadAll = async () => {
    setLoading(true);
    try {
      const [b, c] = await Promise.all([fetchBookings(), fetchCustomers()]);
      setBookings(b);
      setCustomers(c);
    } catch (e) {
      toast.error('Failed to load data', getApiErrorMessage(e));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    void loadAll();
  }, []);

  const customerOptions = useMemo(
    () =>
      customers.map((c) => ({
        label: `${c.fullName} (${c.email})`,
        value: c.id,
      })),
    [customers],
  );

  const filtered = useMemo(() => {
    const q = search.trim().toLowerCase();
    if (!q) return bookings;
    return bookings.filter(
      (b) =>
        b.spaceName.toLowerCase().includes(q) ||
        b.status.toLowerCase().includes(q) ||
        b.customerId.toLowerCase().includes(q),
    );
  }, [bookings, search]);

  const openCreate = () => {
    setEditingId(null);
    setSelected(null);
    setForm(emptyForm());
    setDialogOpen(true);
  };

  const openEdit = (row: BookingResponse) => {
    setEditingId(row.id);
    setSelected(row);
    setForm({
      customerId: row.customerId,
      spaceName: row.spaceName,
      startDate: row.startDate,
      endDate: row.endDate,
      status: parseApiStatus(row.status),
    });
    setDialogOpen(true);
  };

  const save = async () => {
    setSaving(true);
    try {
      const body: SaveBookingRequest = {
        ...form,
        id: editingId,
        startDate: new Date(form.startDate).toISOString(),
        endDate: new Date(form.endDate).toISOString(),
      };
      await upsertBooking(body);
      toast.success('Booking saved');
      setDialogOpen(false);
      await loadAll();
    } catch (e) {
      toast.error('Save failed', getApiErrorMessage(e));
    } finally {
      setSaving(false);
    }
  };

  const applyStatus = async (status: BookingStatus) => {
    if (!selected) return;
    try {
      await changeBookingStatus({ id: selected.id, status });
      toast.success('Status updated');
      setSelected(null);
      await loadAll();
    } catch (e) {
      toast.error('Status update failed', getApiErrorMessage(e));
    }
  };

  const statusBody = (row: BookingResponse) => (
    <Tag value={row.status} severity={statusSeverity(row.status)} rounded />
  );

  const actionsBody = (row: BookingResponse) => (
    <Button type="button" icon="pi pi-pencil" text rounded severity="secondary" onClick={() => openEdit(row)} />
  );

  const toolbarStart = (
    <div className="flex flex-column sm:flex-row gap-2 align-items-stretch sm:align-items-center w-full">
      <InputText
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        placeholder="Search space, status, customer id"
        className="w-full"
        style={{ maxWidth: '20rem' }}
      />
      {selected ? (
        <div className="flex flex-wrap gap-1 align-items-center">
          <span className="text-xs text-color-secondary mr-1">Selected:</span>
          <Button
            type="button"
            label="Pending"
            size="small"
            outlined
            severity="warning"
            disabled={selected.status.toLowerCase() === 'pending'}
            onClick={() => void applyStatus(BookingStatus.Pending)}
          />
          <Button
            type="button"
            label="Confirmed"
            size="small"
            outlined
            severity="success"
            disabled={selected.status.toLowerCase() === 'confirmed'}
            onClick={() => void applyStatus(BookingStatus.Confirmed)}
          />
          <Button
            type="button"
            label="Cancelled"
            size="small"
            outlined
            severity="danger"
            disabled={selected.status.toLowerCase() === 'cancelled'}
            onClick={() => void applyStatus(BookingStatus.Cancelled)}
          />
        </div>
      ) : null}
    </div>
  );

  const toolbarEnd = (
    <Button type="button" label="New booking" icon="pi pi-plus" size="small" onClick={openCreate} />
  );

  return (
    <div>
      <PageHeader title="Bookings" description="Create and manage reservations; status must match backend rules." />
      <Toolbar className="mb-2 p-2 border-round-md surface-card border-1 surface-border" start={toolbarStart} end={toolbarEnd} />

      {loading ? (
        <LoadingSpinner />
      ) : bookings.length === 0 ? (
        <EmptyState
          title="No bookings yet"
          description="Add a booking and select an existing customer."
          action={<Button label="New booking" icon="pi pi-plus" size="small" onClick={openCreate} />}
        />
      ) : (
        <DataTable
          value={filtered}
          size="small"
          stripedRows
          responsiveLayout="scroll"
          selectionMode="single"
          selection={selected}
          onSelectionChange={(e) => setSelected(e.value as BookingResponse | null)}
          dataKey="id"
          emptyMessage="No matches"
          className="text-sm"
        >
          <Column selectionMode="single" headerStyle={{ width: '3rem' }} />
          <Column field="spaceName" header="Space" sortable style={{ minWidth: '9rem' }} />
          <Column header="Status" body={statusBody} style={{ minWidth: '8rem' }} />
          <Column header="Start" body={(r: BookingResponse) => formatDateTime(r.startDate)} style={{ minWidth: '10rem' }} />
          <Column header="End" body={(r: BookingResponse) => formatDateTime(r.endDate)} style={{ minWidth: '10rem' }} />
          <Column field="customerId" header="Customer id" style={{ minWidth: '14rem' }} />
          <Column header="Created" body={(r: BookingResponse) => formatDateTime(r.createdAt)} style={{ minWidth: '10rem' }} />
          <Column body={actionsBody} header="" style={{ width: '4rem' }} />
        </DataTable>
      )}

      <Dialog
        header={editingId ? 'Edit booking' : 'New booking'}
        visible={dialogOpen}
        onHide={() => setDialogOpen(false)}
        style={{ width: 'min(36rem, 96vw)' }}
        breakpoints={{ '768px': '96vw' }}
        draggable={false}
        blockScroll
      >
        <div className="flex flex-column gap-2">
          <label className="text-xs font-medium text-color-secondary">Customer</label>
          <Dropdown
            value={form.customerId}
            options={customerOptions}
            onChange={(e) => setForm((f) => ({ ...f, customerId: e.value as string }))}
            placeholder="Select customer"
            className="w-full"
            filter
            showClear
          />

          <label className="text-xs font-medium text-color-secondary">Space</label>
          <InputText
            value={form.spaceName}
            onChange={(e) => setForm((f) => ({ ...f, spaceName: e.target.value }))}
            className="w-full"
          />

          <div className="grid m-0">
            <div className="col-12 md:col-6 p-1">
              <label className="text-xs font-medium text-color-secondary block mb-1">Start</label>
              <Calendar
                value={new Date(form.startDate)}
                onChange={(e) => {
                  const v = e.value as Date | null;
                  if (!v) return;
                  setForm((f) => ({
                    ...f,
                    startDate: v.toISOString(),
                  }));
                }}
                showTime
                hourFormat="24"
                className="w-full"
              />
            </div>
            <div className="col-12 md:col-6 p-1">
              <label className="text-xs font-medium text-color-secondary block mb-1">End</label>
              <Calendar
                value={new Date(form.endDate)}
                onChange={(e) => {
                  const v = e.value as Date | null;
                  if (!v) return;
                  setForm((f) => ({
                    ...f,
                    endDate: v.toISOString(),
                  }));
                }}
                showTime
                hourFormat="24"
                className="w-full"
              />
            </div>
          </div>

          <label className="text-xs font-medium text-color-secondary">Status</label>
          <Dropdown
            value={form.status}
            options={statusOptions}
            onChange={(e) => setForm((f) => ({ ...f, status: e.value as BookingStatus }))}
            className="w-full"
          />

          <div className="flex justify-content-end gap-2 mt-3">
            <Button type="button" label="Cancel" text severity="secondary" onClick={() => setDialogOpen(false)} />
            <Button type="button" label="Save" icon="pi pi-check" loading={saving} onClick={() => void save()} />
          </div>
        </div>
      </Dialog>
    </div>
  );
}
