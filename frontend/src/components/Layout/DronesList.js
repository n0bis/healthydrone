import React, { Component } from "react";
import ChevronRight from "@material-ui/icons/ChevronRight";
import { inject, observer } from "mobx-react";
import socketCluster from "socketcluster-client";

const DronesComponent = ({ drone, onClick }) => {
  return (
    <div className="drone" onClick={() => onClick(drone)}>
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
  );
};

@inject("droneStore", "mapStore")
@observer
class DronesList extends Component {
  async componentDidMount() {
    const {
      fetchDrones,
      setDroneStatus,
      fetchManagers
    } = this.props.droneStore;
    const { setDroneLocation } = this.props.mapStore;

    await fetchDrones();
    await fetchManagers();

    /*setTimeout(function() {
      setDroneStatus("f7785735-5700-4e1c-a766-5ae7cbb4a4e3", "LANDED");
    }, 5000);*/
    // Initiate the connection to the server
    try {
      const token = localStorage.getItem("authorization")
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
          msg.data.map(data => {
            if (data.source == "simulator") {
              setDroneStatus(data.uas, "IN_FLIGHT");
              setDroneLocation(
                data.UASOPERATION,
                data.xy.longitude,
                data.xy.latitude
              );
            }
          });
        });
      });
      socket.on("error", error => {
        console.log(error);
      });
    } catch (error) {
      console.log(error);
    }
  }

  render() {
    const { drones, onClick } = this.props.droneStore;

    const inFligth = drones.filter(drone => drone.flightStatus === "IN_FLIGHT");
    const parked = drones.filter(drone => drone.flightStatus === "LANDED");

    return (
      <div className="drones">
        <div className="group">
          <p className="group-name">IN FLIGTH</p>
          <div className="group">
            {inFligth.map(drone => (
              <DronesComponent drone={drone} onClick={onClick} />
            ))}
          </div>
        </div>
        <div className="group">
          <p className="group-name">PARKED</p>
          <div className="group">
            {parked.map(drone => (
              <DronesComponent drone={drone} onClick={onClick} />
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
