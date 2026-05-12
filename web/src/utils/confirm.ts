import { confirmDialog } from 'primereact/confirmdialog';

type ConfirmOptions = {
  message: string;
  header?: string;
  icon?: string;
  acceptLabel?: string;
  rejectLabel?: string;
  accept: () => void;
};

export function confirmDestructive({
  message,
  header = 'Confirm',
  icon = 'pi pi-exclamation-triangle',
  acceptLabel = 'Yes',
  rejectLabel = 'No',
  accept,
}: ConfirmOptions) {
  confirmDialog({
    message,
    header,
    icon,
    acceptLabel,
    rejectLabel,
    accept,
    defaultFocus: 'reject',
  });
}
