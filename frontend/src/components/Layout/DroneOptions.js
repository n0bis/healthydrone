import React, { Component } from "react";
import { inject, observer } from "mobx-react";
import { toJS } from "mobx";

@inject("mapStore", "droneStore")
@observer
class DroneOptions extends Component {
  handleClick = event => {
    setAnchorEl(event.currentTarget);
  };

  handleClose = () => {
    //setAnchorEl(null);
  };

  createFligth = () => {
    const { createFlight } = this.props.mapStore;
    createFlight(
      [55.676098, 12.568337],
      [-122.36777130126949, 37.76914513791027]
    );
  };

  startFligth = () => {
    const { sendOnMission } = this.props.droneStore;
    const { data, refreshData } = this.props.mapStore;
    const coordinates = data.features[0].geometry.coordinates;
    sendOnMission(coordinates);
    refreshData();
  };

  stop = () => {
    const { stopDrone } = this.props.droneStore;
    stopDrone();
  };

  sendHome = () => {
    const { sendHome } = this.props.droneStore;
    sendHome();
  };

  sendOnMission = () => {
    //const { sendOnMission } = this.props.droneStore;
  };

  landAtLocation = () => {
    alert("Land at location");
  };

  onLocationChange = event => {
    const { setDraw } = this.props.mapStore;
    const { getDroneLocationCords, drone } = this.props.droneStore;
    const value = event.target.value.split(",");

    const location = getDroneLocationCords(drone.uniqueIdentifier);

    const to = [parseFloat(value[0]), parseFloat(value[1])];
    const from = [location.longitude, location.latitude];
    console.log(from, to);
    setDraw(from, to);
  };

  render() {
    const { setZoom, landingPoints } = this.props.mapStore;
    const {
      showDroneOptions,
      closeDroneOptions,
      drone,
      getDroneLocation
    } = this.props.droneStore;
    const display = showDroneOptions ? "block" : "none";

    //console.log(toJS(drone), toJS(droneManagers), toJS(droneLocations));

    return (
      <div className="drone-options fadeIn" style={{ display: display }}>
        {drone == false ? (
          <h1>asda</h1>
        ) : (
          <>
            <div className="title">
              <p>Drone ID: {drone.uniqueIdentifier}</p>
              <div className="close-options" onClick={closeDroneOptions}>
                Luk
              </div>
            </div>
            <div className="description">
              <p>
                <b>Bemærk</b> når du genere en rute kan den overlappe en anden
                rute. Det kan derfor være en god ide at ændre rutens forløb.
              </p>
            </div>
            <div className="actions">
              {drone.flightStatus === "IN_FLIGHT" ? (
                <>
                  <button onClick={this.stop}>Stop</button>
                  <button onClick={this.sendHome}>Send hjem</button>
                  <button onClick={this.sendOnMission}>Send on mission</button>
                  <select onChange={this.onLocationChange}>
                    <option>Land på lokation</option>
                    <option>a</option>
                    <option>b</option>
                    <option>c</option>
                  </select>
                </>
              ) : (
                <>
                  <p>
                    <b>Fra</b>
                  </p>
                  <p>{getDroneLocation(drone.uniqueIdentifier)}</p>
                  <p>
                    <b>Til</b>
                  </p>
                  <select onChange={this.onLocationChange}>
                    <option default>Vælg lokation</option>
                    {landingPoints.map(point => (
                      <option value={point.location}>{point.name}</option>
                    ))}
                  </select>
                  <br />
                  <button onClick={this.createFligth}>Create fligth</button>
                  <br />
                  <button onClick={this.startFligth}>Start fligth</button>
                </>
              )}
            </div>
          </>
        )}
      </div>
    );
  }
}

export default DroneOptions;
