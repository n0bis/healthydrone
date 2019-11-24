import React, { Component } from "react";
import { inject, observer } from "mobx-react";

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

  render() {
    const { landingSpots, startFligth, setLanding } = this.props.mapStore;
    const { showDroneOptions, closeDroneOptions } = this.props.droneStore;
    const display = showDroneOptions ? "block" : "none";
    const drone = getDrone();
    return (
      <div className="drone-options fadeIn" style={{ display: display }}>
        {drone}
        <div className="title">
          <p>Drone ID: #31231345</p>
          <div className="close-options" onClick={closeDroneOptions}>
            Luk
          </div>
        </div>
        <div className="description">
          <p>
            <b>Bemærk</b> når du genere en rute kan den overlappe en anden rute.
            Det kan derfor være en god ide at ændre rutens forløb.
          </p>
        </div>
        <div className="actions">
          <p>
            <b>Fra</b>
          </p>
          <p>Svendbord</p>
          <p>
            <b>Til</b>
          </p>
          <select onChange={e => setLanding(e)}>
            <option default>Vælg lokation</option>
            {landingSpots.map(item => (
              <option value={item.id}>{item.name}</option>
            ))}
          </select>
          <br />
          <button onClick={this.createFligth}>Create fligth</button>
          <br />
          <button onClick={startFligth}>Start fligth</button>
        </div>
      </div>
    );
  }
}

export default DroneOptions;
