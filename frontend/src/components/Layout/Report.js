import React, { Component } from "react";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import { observer, inject } from "mobx-react";
import { Select } from "antd";

const { Option } = Select;

function handleChange(value) {
  console.log(`selected ${value}`);
}

export var OpenModal = inject("reportStore")(
  observer(props => {
    const { onOpenModal } = props.reportStore;
    return <p onClick={onOpenModal}> REPORT </p>;
  })
);

@inject("reportStore")
@observer
class FormDialog extends Component {
  render() {
    const {
      isModalOpen,
      onCloseModal,
      onSendModal,
      handleChange
    } = this.props.reportStore;
    console.log(this.props.reportStore);
    return (
      <Dialog open={isModalOpen} aria-labelledby="form-dialog-title">
        <DialogTitle id="form-dialog-title">Rapporter fejl</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Beskriv venligst den fejl der opstod. Fejlen vil herefter blive
            gennemgået.
          </DialogContentText>
          <Select
            defaultValue="choose a Drone"
            style={{ width: 120 }}
            onChange={handleChange}
          >
            <Option value="drone1">drone1</Option>
            <Option value="drone2">drone2</Option>
            <Option value="drone3">drone3</Option>
            <Option value="luaaaacy">Lusssscy</Option>
            <Option value="luaaaqqqcy">Luaaacy</Option>
            <Option value="lubbbbcy">Luvvsdfcy</Option>
          </Select>
          <TextField
            id="standard-multiline-static"
            label="Detaljer"
            placeholder="Fortæl hvad der skete"
            multiline
            rowsMax="10"
            name="details"
            onChange={handleChange}
            fullWidth
          />
          <TextField
            id="standard-multiline-static"
            label="Skade"
            placeholder="Beskriv skaden"
            multiline
            rowsMax="10"
            onChange={handleChange}
            fullWidth
          />
          <TextField
            id="standard-multiline-static"
            label="Handling"
            placeholder="Handling"
            multiline
            rowsMax="10"
            fullWidth
            onChange={handleChange}
          />
          <TextField
            id="standard-multiline-static"
            label="Noter"
            placeholder="Noter"
            multiline
            rowsMax="10"
            onChange={handleChange}
            fullWidth
          />
        </DialogContent>
        <DialogActions>
          <Button color="primary" onClick={onCloseModal}>
            Anullere
          </Button>
          <Button color="primary" onClick={onSendModal}>
            Send
          </Button>
        </DialogActions>
      </Dialog>
    );
  }
}

export default FormDialog;
