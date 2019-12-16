import React, { Component } from "react";
import ChevronRight from "@material-ui/icons/ChevronRight";
import { inject, observer } from "mobx-react";
import socketCluster from "socketcluster-client";
import axios from "axios";

const DronesComponent = ({ drone, onClick, location, status }) => {
  return (
    <div className="drone" onClick={() => onClick(drone)}>
      <p>
        <b>Status:</b> <span className="text-green">{status}</span>
      </p>
      <p className="location">
        <b>Lokation:</b> <u>{location}</u>
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
      const token = localStorage.getItem("authorization");

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
            if (data.source == "simulation") {
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

    const danger = drones.filter(drone => drone.flightStatus == "DANGER");
    const inFligth = drones.filter(drone => drone.flightStatus == "IN_FLIGHT");
    const parked = drones.filter(drone => drone.flightStatus == "LANDED");

    return (
      <div className="drones">
        {danger.length > 0 && (
          <div className="group danger">
            <p className="group-name">IN DANGER</p>
            <div className="group">
              {danger.map((drone, key) => (
                <DronesComponent
                  key={key}
                  drone={drone}
                  location={getDroneLocation(drone.uniqueIdentifier)}
                  onClick={this.onClick}
                />
              ))}
            </div>
          </div>
        )}
        {inFligth.length > 0 && (
          <div className="group in-fligth">
            <p className="group-name">IN FLIGTH</p>
            <div className="group">
              {inFligth.map((drone, key) => (
                <DronesComponent
                  key={key}
                  drone={drone}
                  status={"FLyvende"}
                  location={getDroneLocation(drone.uniqueIdentifier)}
                  onClick={this.onClick}
                />
              ))}
            </div>
          </div>
        )}
        {parked.length > 0 && (
          <div className="group parked">
            <p className="group-name">PARKED</p>
            <div className="group">
              {parked.map((drone, key) => (
                <DronesComponent
                  key={key}
                  drone={drone}
                  status={"Parkeret"}
                  location={getDroneLocation(drone.uniqueIdentifier)}
                  onClick={this.onClick}
                />
              ))}
            </div>
          </div>
        )}
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
