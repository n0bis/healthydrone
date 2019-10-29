import React, { Component } from "react";
import { makeStyles } from "@material-ui/core/styles";
import Popover from "@material-ui/core/Popover";
import Typography from "@material-ui/core/Typography";
import Button from "@material-ui/core/Button";

const useStyles = makeStyles(theme => ({
  typography: {
    padding: theme.spacing(2)
  }
}));

class DroneOptions extends Component {
  handleClick = event => {
    setAnchorEl(event.currentTarget);
  };

  handleClose = () => {
    //setAnchorEl(null);
  };

  render() {
    return (
      <div>
        <Popover
          id={1}
          open={false}
          anchorReference="anchorPosition"
          anchorPosition={{ top: 300, left: 340 }}
          anchorOrigin={{
            vertical: "center",
            horizontal: "right"
          }}
        >
          <div style={{ width: "350px", height: "300px" }}>
            The content of the Popover.
          </div>
        </Popover>
      </div>
    );
  }
}

export default DroneOptions;
