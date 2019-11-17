import React, { Component } from "react";
import ChevronRight from "@material-ui/icons/ChevronRight";
import { inject, observer } from "mobx-react";

@inject("droneStore")
@observer
class DronesList extends Component {
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
