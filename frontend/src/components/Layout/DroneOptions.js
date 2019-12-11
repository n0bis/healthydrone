import React, { Component } from "react";
import { inject, observer } from "mobx-react";
import { toJS } from "mobx";

@inject("mapStore", "droneStore")
@observer
class DroneOptions extends Component {
  /*
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
  */

  startFligth = async () => {
    try {
      const { sendOnMission } = this.props.droneStore;
      const { data, refreshData } = this.props.mapStore;
      const coordinates = data.features[0].geometry.coordinates;

      await sendOnMission(coordinates);
      refreshData();
    } catch (err) {
      alert("Kunne ikke oprette rutes");
    }
  };

  stop = () => {
    const { stopDrone } = this.props.droneStore;
    stopDrone();
  };

  sendHome = () => {
    const { sendHome } = this.props.droneStore;
    sendHome();
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
          <h1>Kunne ikke finde drone</h1>
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
                  <div>
                    <p>Send på ny mission:</p>
                    <select onChange={this.onLocationChange}>
                      <option default>Vælg lokation</option>
                      {landingPoints.map(point => (
                        <option value={point.location}>{point.name}</option>
                      ))}
                    </select>
                    <br />
                    <button onClick={this.startFligth}>Start fligth</button>
                  </div>
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
