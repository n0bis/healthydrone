import React, { Component } from "react";
import ChevronRight from "@material-ui/icons/ChevronRight";

const drones = [
  {
    group_name: "Aktive",
    group_status: "active",
    drones: [
      {
        name: "test"
      },
      {
        name: "test"
      }
    ]
  },
  {
    group_name: "Parkeret",
    group_status: "parked",
    drones: [
      {
        name: "test"
      },
      {
        name: "test"
      }
    ]
  }
];

class DronesList extends Component {
  onClick = event => {
    console.log(event.target.offsetTop);
  };

  render() {
    return (
      <div className="drones">
        {drones.map(group => (
          <div className="group">
            <p className="group-name">{group.group_name}</p>
            <div className="group">
              {group.drones.map(drone => (
                <div className="drone" onClick={this.onClick}>
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
