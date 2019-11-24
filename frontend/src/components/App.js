import React, { Component } from "react";
import { Route } from "react-router-dom";
import Dashboard from "./Layout/Dashboard";
import NurseReport from "./NurseReport";

class App extends Component {
  render() {
    return (
      <>
        <Route exact path="/report" component={NurseReport} />
        <Route exact path="/" component={Dashboard} />
      </>
    );
  }
}

export default App;
