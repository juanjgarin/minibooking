import { useEffect, useMemo, useState } from 'react';
import { Button } from 'primereact/button';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { InputText } from 'primereact/inputtext';
import { Toolbar } from 'primereact/toolbar';
import { EmptyState } from '@/components/EmptyState';
import { LoadingSpinner } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { useAppToast } from '@/hooks/useAppToast';
import { deleteCustomer, fetchCustomers, upsertCustomer } from '@/services/customerService';
import type { CustomerResponse, SaveCustomerRequest } from '@/types/customer';
import { getApiErrorMessage } from '@/utils/apiError';
import { confirmDestructive } from '@/utils/confirm';
import { formatDateTime } from '@/utils/formatDate';

const emptyForm: SaveCustomerRequest = {
  fullName: '',
  email: '',
  phone: '',
};

export function CustomersPage() {
  const toast = useAppToast();
  const [rows, setRows] = useState<CustomerResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [dialogOpen, setDialogOpen] = useState(false);
  const [saving, setSaving] = useState(false);
  const [form, setForm] = useState<SaveCustomerRequest>(emptyForm);
  const [editingId, setEditingId] = useState<string | null>(null);

  const load = async () => {
    setLoading(true);
    try {
      const data = await fetchCustomers();
      setRows(data);
    } catch (e) {
      toast.error('Failed to load customers', getApiErrorMessage(e));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    void load();
  }, []);

  const filtered = useMemo(() => {
    const q = search.trim().toLowerCase();
    if (!q) return rows;
    return rows.filter(
      (r) =>
        r.fullName.toLowerCase().includes(q) ||
        r.email.toLowerCase().includes(q) ||
        r.phone.toLowerCase().includes(q),
    );
  }, [rows, search]);

  const openCreate = () => {
    setEditingId(null);
    setForm(emptyForm);
    setDialogOpen(true);
  };

  const openEdit = (row: CustomerResponse) => {
    setEditingId(row.id);
    setForm({
      id: row.id,
      fullName: row.fullName,
      email: row.email,
      phone: row.phone,
    });
    setDialogOpen(true);
  };

  const save = async () => {
    setSaving(true);
    try {
      const body: SaveCustomerRequest = {
        ...form,
        id: editingId,
      };
      await upsertCustomer(body);
      toast.success('Customer saved');
      setDialogOpen(false);
      await load();
    } catch (e) {
      toast.error('Save failed', getApiErrorMessage(e));
    } finally {
      setSaving(false);
    }
  };

  const remove = (row: CustomerResponse) => {
    confirmDestructive({
      message: `Delete customer "${row.fullName}"?`,
      header: 'Delete customer',
      accept: async () => {
        try {
          await deleteCustomer(row.id);
          toast.success('Customer deleted');
          await load();
        } catch (e) {
          toast.error('Delete failed', getApiErrorMessage(e));
        }
      },
    });
  };

  const actionsBody = (row: CustomerResponse) => (
    <div className="flex gap-1 flex-wrap">
      <Button type="button" icon="pi pi-pencil" text rounded severity="secondary" onClick={() => openEdit(row)} />
      <Button type="button" icon="pi pi-trash" text rounded severity="danger" onClick={() => remove(row)} />
    </div>
  );

  const toolbarStart = (
    <div className="flex flex-column sm:flex-row gap-2 align-items-stretch sm:align-items-center w-full">
      <InputText
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        placeholder="Search name, email, phone"
        className="w-full"
        style={{ maxWidth: '20rem' }}
      />
    </div>
  );

  const toolbarEnd = (
    <Button type="button" label="New customer" icon="pi pi-plus" size="small" onClick={openCreate} />
  );

  return (
    <div>
      <PageHeader title="Customers" description="Manage customer profiles used by bookings." />
      <Toolbar className="mb-2 p-2 border-round-md surface-card border-1 surface-border" start={toolbarStart} end={toolbarEnd} />

      {loading ? (
        <LoadingSpinner />
      ) : rows.length === 0 ? (
        <EmptyState
          title="No customers yet"
          description="Create your first customer to use when adding bookings."
          action={<Button label="New customer" icon="pi pi-plus" size="small" onClick={openCreate} />}
        />
      ) : (
        <DataTable
          value={filtered}
          size="small"
          stripedRows
          responsiveLayout="scroll"
          emptyMessage="No matches"
          className="text-sm"
        >
          <Column field="fullName" header="Name" sortable style={{ minWidth: '10rem' }} />
          <Column field="email" header="Email" sortable style={{ minWidth: '12rem' }} />
          <Column field="phone" header="Phone" style={{ minWidth: '8rem' }} />
          <Column
            header="Created"
            body={(r: CustomerResponse) => formatDateTime(r.createdAt)}
            style={{ minWidth: '10rem' }}
          />
          <Column body={actionsBody} header="" style={{ width: '6rem' }} />
        </DataTable>
      )}

      <Dialog
        header={editingId ? 'Edit customer' : 'New customer'}
        visible={dialogOpen}
        onHide={() => setDialogOpen(false)}
        style={{ width: 'min(32rem, 95vw)' }}
        breakpoints={{ '768px': '95vw' }}
        draggable={false}
        blockScroll
      >
        <div className="flex flex-column gap-2">
          <label className="text-xs font-medium text-color-secondary">Full name</label>
          <InputText
            value={form.fullName}
            onChange={(e) => setForm((f) => ({ ...f, fullName: e.target.value }))}
            className="w-full"
          />
          <label className="text-xs font-medium text-color-secondary">Email</label>
          <InputText
            value={form.email}
            onChange={(e) => setForm((f) => ({ ...f, email: e.target.value }))}
            className="w-full"
          />
          <label className="text-xs font-medium text-color-secondary">Phone</label>
          <InputText
            value={form.phone}
            onChange={(e) => setForm((f) => ({ ...f, phone: e.target.value }))}
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
