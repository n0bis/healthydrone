import React, { Component } from "react";
import { Route, Redirect, withRouter } from "react-router-dom";
import { inject, observer } from "mobx-react";
import LazyRoute from "lazy-route";
import Dashboard from "./Layout/Dashboard";
import Login from "./Login";
import NurseReport from "./NurseReport";
import SignalR from "./SignalRRoute";
import hasAnyRole from "../utils/auth";

const Authorization = isLoggedIn => component => {
  return isLoggedIn ? component : <Redirect to="/login" />;
};

@inject("routing", "loginStore")
@observer
class App extends Component {
  render() {
    const { loginStore } = this.props;
    const { isLoggedInPersist } = loginStore;
    const isLoggedIn = isLoggedInPersist();
    const User = Authorization(isLoggedIn);

    return (
      <div>
        <Route exact path="/login" component={Login} />
        <Route
          exact
          path="/report"
          render={props =>
            User(<LazyRoute {...props} component={import("./NurseReport")} />)
          } />
        <Route 
          exact 
          path="/" 
          render={props =>
            User(<LazyRoute {...props} component={import("./Layout/Dashboard")} />)
          } />
        <Route exact path="/hello" component={SignalR} />
      </div>
    );
  }
}

export default withRouter(observer(App));
