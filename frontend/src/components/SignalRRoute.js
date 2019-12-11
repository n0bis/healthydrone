import React, { Component } from "react";
const signalR = require("@microsoft/signalr");

class SignalR extends Component {

  async componentDidMount() {
    let connection = new signalR.HubConnectionBuilder()
    .withUrl("http://192.168.99.185/handlealerts/alerts")
    .build();

    connection.start().then(function () {
      console.log("connected");
    });

    connection.on("alerts", (message) => {
      console.log(message);
    });

    connection.on("request_drone", (message) => {
      console.log(message);
    });
  }

  render() {
    return (
      <div />
    );
  }
}

export default SignalR;
