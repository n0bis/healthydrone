import React, { Component } from "react";
import ChevronRight from "@material-ui/icons/ChevronRight";
import { inject, observer } from "mobx-react";
import socketCluster from "socketcluster-client";
import axios from "axios";

const DronesComponent = ({ drone, onClick, location }) => {
  return (
    <div className="drone" onClick={() => onClick(drone)}>
      <p>
        <b>Status:</b> <span className="text-green">Flyvende</span>
      </p>
      <p>
        <b>Batteri:</b> <span className="text-green">94%</span>
      </p>
      <p>
        <b>Lokation</b> <u>{location}</u>
      </p>
      <div className="arrow">
        <p>
          <ChevronRight />
        </p>
      </div>
    </div>
  );
};

@inject("droneStore", "mapStore")
@observer
class DronesList extends Component {
  async componentDidMount() {
    const {
      fetchDrones,
      setDroneStatus,
      fetchManagers,
      updateDroneLocation
    } = this.props.droneStore;
    const { setDroneLocation } = this.props.mapStore;

    await fetchDrones();
    await fetchManagers();

    // Initiate the connection to the server
    try {
      const token =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0cmFja2luZ0FsdGl0dWRlRmlsdGVyIjo0MDAwLjAsImF1ZCI6WyJ1c2VyTWFuYWdlbWVudFNlcnZpY2UiXSwiY2hhbm5lbHMiOlsiXCJvcGVyYXRvcjplNjMzM2ZjNC04YTcwLTQ5MDYtYWM2OC02N2YzYTFhYjdkMmZcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOmU2MzMzZmM0LThhNzAtNDkwNi1hYzY4LTY3ZjNhMWFiN2QyZjoqXCI6W1wic3Vic2NyaWJlXCJdIiwiXCJvcGVyYXRvcjplNjMzM2ZjNC04YTcwLTQ5MDYtYWM2OC02N2YzYTFhYjdkMmY6dWFzOipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImZsaWdodFwiOltcInN1YnNjcmliZVwiXSIsIlwibmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2I6bmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2JcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwOFwiOltcInN1YnNjcmliZVwiXSIsIlwib3BlcmF0b3I6MzhkYzNjN2EtOTE1ZS00NDA5LWI2ZDktYTBkYzQwOWQ4ODA4OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwODp1YXM6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiZmxpZ2h0XCI6W1wic3Vic2NyaWJlXCJdIiwiXCJuZWFyYnk6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiYWRzYjpuZWFyYnk6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiYWRzYlwiOltcInN1YnNjcmliZVwiXSJdLCJ1c2VyX25hbWUiOiJGUkhFTDE4QFNUVURFTlQuU0RVLkRLIiwic2NvcGUiOlsicmVhZCJdLCJleHAiOjE1NzU2NjQ1MDAsImp0aSI6IjJiNDEwODExLTA5YjEtNGZkZC05ZGQ3LTlhMjc5ZDViZTIzNyIsImNsaWVudF9pZCI6InNkdUhlYWx0aERyb25lQ29ubmVjdCIsInVzaWQiOiIwZjA4MmQ3Zi04ZWU1LTQ5OTEtYjM2ZC1lZWYzMjYxNGVmNDMifQ.DBwAeeaCoZ1qFZK8VycAWDXlZ2va1Wz-se_zy6Wco_s";

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

      var i = 0;

      socket.on("connect", () => {
        subscription = socket.subscribe("adsb", { waitForAuth: true });
        subscription.watch(msg => {
          msg.data.map(data => {
            if (data.source == "simulator") {
              setDroneStatus(data.uas, "IN_FLIGHT");
              setDroneLocation(
                data.UASOPERATION,
                data.xy.longitude,
                data.xy.latitude
              );

              if (i == 50) {
                updateDroneLocation(
                  data.uas,
                  data.xy.longitude,
                  data.xy.latitude
                );
                console.log(1);
              }
            }
          });
          if (i == 50) i = 0;
          i++;
        });
      });
      socket.on("error", error => {
        console.log(error);
      });
    } catch (error) {
      console.log(error);
    }
  }

  onClick = drone => {
    const { onClick, getDroneLocationCords } = this.props.droneStore;
    const { setLocation, setZoom } = this.props.mapStore;
    const location = getDroneLocationCords(drone.uniqueIdentifier);
    onClick(drone);
    setZoom();
    console.log(getDroneLocationCords(drone.uniqueIdentifier));
    setLocation(location.longitude, location.latitude);
  };

  render() {
    const { drones, getDroneLocation } = this.props.droneStore;

    const inFligth = drones.filter(drone => drone.flightStatus == "IN_FLIGHT");
    const parked = drones.filter(drone => drone.flightStatus == "LANDED");

    return (
      <div className="drones">
        <div className="group">
          <p className="group-name">IN FLIGTH</p>
          <div className="group">
            {inFligth.map((drone, key) => (
              <DronesComponent
                key={key}
                drone={drone}
                location={getDroneLocation(drone.uniqueIdentifier)}
                onClick={this.onClick}
              />
            ))}
          </div>
        </div>
        <div className="group">
          <p className="group-name">PARKED</p>
          <div className="group">
            {parked.map((drone, key) => (
              <DronesComponent
                key={key}
                drone={drone}
                location={getDroneLocation(drone.uniqueIdentifier)}
                onClick={this.onClick}
              />
            ))}
          </div>
        </div>
      </div>
    );
  }
}

export default DronesList;
/*
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
</div>;*/
