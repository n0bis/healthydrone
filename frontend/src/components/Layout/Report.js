import React from "react";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";

export default function FormDialog() {
  const [open, setOpen] = React.useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <Dialog
      open={false}
      onClose={handleClose}
      aria-labelledby="form-dialog-title"
    >
      <DialogTitle id="form-dialog-title">Rapporter fejl</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Beskriv venligst den fejl der opstod. Fejlen vil herefter blive
          gennemgået.
        </DialogContentText>
        <TextField
          id="standard-multiline-static"
          label="Skriv report"
          placeholder="Fortæl hvad der skete"
          multiline
          rowsMax="10"
          fullWidth
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} color="primary">
          Anullere
        </Button>
        <Button onClick={handleClose} color="primary">
          Send
        </Button>
      </DialogActions>
    </Dialog>
  );
}
