import React, { Component } from "react";
import ChevronRight from "@material-ui/icons/ChevronRight";
import { inject, observer } from "mobx-react";
import socketCluster from "socketcluster-client";

@inject("droneStore")
@observer
class DronesList extends Component {
  componentWillMount() {
    // Initiate the connection to the server
    try {
      const token =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0cmFja2luZ0FsdGl0dWRlRmlsdGVyIjo0MDAwLjAsImF1ZCI6WyJ1c2VyTWFuYWdlbWVudFNlcnZpY2UiXSwiY2hhbm5lbHMiOlsiXCJvcGVyYXRvcjo3MmI4YTYyYy00M2Q1LTQxODEtYWE1OS03ZjlhMWQ2YmJkZWNcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjcyYjhhNjJjLTQzZDUtNDE4MS1hYTU5LTdmOWExZDZiYmRlYzoqXCI6W1wic3Vic2NyaWJlXCJdIiwiXCJvcGVyYXRvcjo3MmI4YTYyYy00M2Q1LTQxODEtYWE1OS03ZjlhMWQ2YmJkZWM6dWFzOipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImZsaWdodFwiOltcInN1YnNjcmliZVwiXSIsIlwibmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2I6bmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2JcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwOFwiOltcInN1YnNjcmliZVwiXSIsIlwib3BlcmF0b3I6MzhkYzNjN2EtOTE1ZS00NDA5LWI2ZDktYTBkYzQwOWQ4ODA4OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwODp1YXM6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiZmxpZ2h0XCI6W1wic3Vic2NyaWJlXCJdIiwiXCJuZWFyYnk6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiYWRzYjpuZWFyYnk6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiYWRzYlwiOltcInN1YnNjcmliZVwiXSJdLCJ1c2VyX25hbWUiOiJNQUZBTDE3QFNUVURFTlQuU0RVLkRLIiwic2NvcGUiOlsicmVhZCIsIndyaXRlIl0sImV4cCI6MTU3NDA1OTI1OCwianRpIjoiMzE0ZGJkOWQtMWU3Yi00MmJhLTg0ZDktZDgzZDNkZGE1MzhlIiwiY2xpZW50X2lkIjoiYWlyZW5hV2ViUG9ydGFsIiwidXNpZCI6ImM4OTFlMWVkLTUwNmItNDYyMC04M2FjLTFmYzI2YjIxZTI5ZCJ9.Em9_NdS1nGico0UxzllF_navxO2DhDwkEBaiZ5v7y58";

      let socketClusterOptions = {
        port: 443,
        secure: true,
        hostname: "healthdrone.unifly.tech",
        path: "/socketcluster/",
        autoReconnectOptions: { initialDelay: 500, maxDelay: 2000 }
      };
      let subscription;
      let socket = socketCluster.create(socketClusterOptions);
      socket.authenticate(token, error => {
        if (error)
          console.error(`Socket cluster authentication failed : ${error}`);
      });
      socket.on("connect", () => {
        subscription = socket.subscribe("adsb", { waitForAuth: true });
        subscription.watch(msg => {
          console.info(
            `Received event: ${msg.name}with data: ${JSON.stringify(msg.data)}`
          );
        });
      });
      socket.on("error", error => {
        console.log(error);
      });
    } catch (error) {
      console.log(error);
    }
  }

  onClick = event => {
    console.log(event.target.offsetTop);
  };

  render() {
    const { drones, onClick } = this.props.droneStore;
    return (
      <div className="drones">
        {drones.map(group => (
          <div className="group">
            <p className="group-name">{group.group_name}</p>
            <div className="group">
              {group.drones.map(drone => (
                <div className="drone" onClick={onClick}>
                  <p>
                    <b>Status:</b> <span className="text-green">Flyvende</span>
                  </p>
                  <p>
                    <b>Batteri:</b> <span className="text-green">94%</span>
                  </p>
                  <p>
                    <b>Lokation</b> <u>Svendbord</u>
                  </p>
                  <div className="arrow">
                    <p>
                      <ChevronRight />
                    </p>
                  </div>
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    );
  }
}

export default DronesList;
