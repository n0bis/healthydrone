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
    const { fetchDrones, setDroneStatus } = this.props.droneStore;
    const { setDroneLocation } = this.props.mapStore;
    await fetchDrones();

    /*setTimeout(function() {
      setDroneStatus("f7785735-5700-4e1c-a766-5ae7cbb4a4e3", "LANDED");
    }, 5000);*/
    // Initiate the connection to the server
    try {
      const token =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0cmFja2luZ0FsdGl0dWRlRmlsdGVyIjo0MDAwLjAsImF1ZCI6WyJ1c2VyTWFuYWdlbWVudFNlcnZpY2UiXSwiY2hhbm5lbHMiOlsiXCJvcGVyYXRvcjozOGRjM2M3YS05MTVlLTQ0MDktYjZkOS1hMGRjNDA5ZDg4MDhcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwODoqXCI6W1wic3Vic2NyaWJlXCJdIiwiXCJvcGVyYXRvcjozOGRjM2M3YS05MTVlLTQ0MDktYjZkOS1hMGRjNDA5ZDg4MDg6dWFzOipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImZsaWdodFwiOltcInN1YnNjcmliZVwiXSIsIlwibmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2I6bmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2JcIjpbXCJzdWJzY3JpYmVcIl0iXSwidXNlcl9uYW1lIjoiSk9MVU4xOEBTVFVERU5ULlNEVS5ESyIsInNjb3BlIjpbInJlYWQiXSwiZXhwIjoxNTc1MzUxMDYzLCJqdGkiOiI5NDhkNTZlMi02ZDljLTRmYTEtOWYzNy00YjhhNDEzYjE1MTMiLCJjbGllbnRfaWQiOiJzZHVIZWFsdGhEcm9uZUNvbm5lY3QiLCJ1c2lkIjoiN2NmMTZjZDgtYmFmMi00OGExLWEzNzgtOGRiOWUwYjgyNGQ4In0.8ZVaFPOfvYegbh26B9jL4JGoasdCfz5Juhu4_UD0Q4s";
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
          if (msg.data[0].source == "simulator") {
            setDroneStatus(msg.data[0].uas, "IN_FLIGHT");
            setDroneLocation(
              msg.data[0].UASOPERATION,
              msg.data[0].xy.longitude,
              msg.data[0].xy.latitude
            );
          }
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
