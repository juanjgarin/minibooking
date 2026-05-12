import { useEffect, useState } from 'react';
import { Card } from 'primereact/card';
import { PageHeader } from '@/components/PageHeader';
import { LoadingSpinner } from '@/components/LoadingSpinner';
import { fetchBookings } from '@/services/bookingService';
import { fetchCustomers } from '@/services/customerService';

export function DashboardPage() {
  const [loading, setLoading] = useState(true);
  const [customerCount, setCustomerCount] = useState(0);
  const [bookingCount, setBookingCount] = useState(0);

  useEffect(() => {
    let cancelled = false;
    (async () => {
      setLoading(true);
      try {
        const [customers, bookings] = await Promise.all([fetchCustomers(), fetchBookings()]);
        if (!cancelled) {
          setCustomerCount(customers.length);
          setBookingCount(bookings.length);
        }
      } catch {
        if (!cancelled) {
          setCustomerCount(0);
          setBookingCount(0);
        }
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();
    return () => {
      cancelled = true;
    };
  }, []);

  return (
    <div>
      <PageHeader
        title="Dashboard"
        description="Overview of MiniBooking services and quick metrics."
      />
      {loading ? (
        <LoadingSpinner />
      ) : (
        <div className="grid">
          <div className="col-12 md:col-6">
            <Card title="Customers" className="shadow-1 h-full">
              <p className="m-0 text-3xl font-semibold text-primary">{customerCount}</p>
              <p className="mt-2 mb-0 text-sm text-color-secondary">Total records in CustomerService</p>
            </Card>
          </div>
          <div className="col-12 md:col-6">
            <Card title="Bookings" className="shadow-1 h-full">
              <p className="m-0 text-3xl font-semibold text-primary">{bookingCount}</p>
              <p className="mt-2 mb-0 text-sm text-color-secondary">Total records in BookingService</p>
            </Card>
          </div>
        </div>
      )}
    </div>
  );
}
