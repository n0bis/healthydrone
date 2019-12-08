import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Button from "@material-ui/core/Button";
import IconButton from "@material-ui/core/IconButton";
import { inject, observer } from "mobx-react";

const useStyles = makeStyles(theme => ({
  root: {
    flexGrow: 1
  },
  menuButton: {
    marginRight: theme.spacing(2)
  },
  title: {
    flexGrow: 1
  }
}));

@inject("loginStore")
@observer
export default function ButtonAppBar() {
  const classes = useStyles();
  const { loginStore } = this.props
  const { isLoggedInPersist, logout } = loginStore
  const isLoggedIn = isLoggedInPersist()

  return (
    <div className={classes.root}>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            edge="start"
            className={classes.menuButton}
            color="inherit"
            aria-label="menu"
          ></IconButton>
          <Typography variant="h6" className={classes.title}>
            News
          </Typography>
          {isLoggedIn && <Button color="inherit" onClick={() => logout()}>Log out</Button>}
        </Toolbar>
      </AppBar>
    </div>
  );
}
