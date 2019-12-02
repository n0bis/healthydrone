import React, {Component} from "react";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import { observer, inject } from "mobx-react";
import IconButton from "@material-ui/core/IconButton";
import ReportIcon from "@material-ui/icons/Report";
import { Descriptions } from 'antd';
import { Select } from "antd";
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';



export var OpenModal = inject("reportStore")(observer((props) => {
 const {onOpenModal} = props.reportStore;
  return <p onClick={onOpenModal}> REPORT </p>
 }));
 function SimpleMenu() {
  const [anchorEl, setAnchorEl] = React.useState(null);

  const handleClick = event => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };


@inject("reportStore")
@observer 
class FormDialog extends Component {

  render(){
    const {isModalOpen, onCloseModal, onSendModal} = this.props.reportStore;
    console.log(this.props.reportStore);
  return (
    <Dialog
      open={isModalOpen}
      aria-labelledby="form-dialog-title"
    >
      <DialogTitle id="form-dialog-title">Rapporter fejl</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Beskriv venligst den fejl der opstod. Fejlen vil herefter blive
          gennemgået.
        </DialogContentText>
        <div>
      <Button aria-controls="simple-menu" aria-haspopup="true" onClick={handleClick}>
        Open Menu
      </Button>
      <Menu
        id="simple-menu"
        anchorEl={anchorEl}
        keepMounted
        open={Boolean(anchorEl)}
        onClose={handleClose}
      >
        <MenuItem onClick={handleClose}>Profile</MenuItem>
        <MenuItem onClick={handleClose}>My account</MenuItem>
        <MenuItem onClick={handleClose}>Logout</MenuItem>
      </Menu>
    </div>
        <TextField
          id="standard-multiline-static"
          label="Detaljer"
          placeholder="Fortæl hvad der skete"
          multiline
          rowsMax="10"
          fullWidth
        />
        <TextField
          id="standard-multiline-static"
          label="Skade"
          placeholder="Beskriv skaden"
          multiline
          rowsMax="10"
          fullWidth
        />
        <TextField
          id="standard-multiline-static"
          label="Handling"
          placeholder="Handling"
          multiline
          rowsMax="10"
          fullWidth
        />
        <TextField
          id="standard-multiline-static"
          label="Noter"
          placeholder="Noter"
          multiline
          rowsMax="10"
          fullWidth
        />
      </DialogContent>
      <DialogActions>
        <Button  color="primary" onClick={onCloseModal}>
          Anullere
        </Button>
        <Button  color="primary" onClick={onSendModal}>
          Send
        </Button>
      </DialogActions>
    </Dialog>
  );
  }
}

export default FormDialog;
