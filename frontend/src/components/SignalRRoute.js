import React, { Component } from "react";
const signalR = require("@microsoft/signalr");

class SignalR extends Component {

  async componentDidMount() {
    let connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:1339/alerts")
    .build();

    connection.on("alerts", (message) => {
      console.log(message);
    });

    connection.on("request_drone", (message) => {
      console.log(message);
    });

    connection.start().then(function () {
      console.log("connected");
    });
  }

  render() {
    return (
      <div />
    );
  }
}

export default SignalR;
