import React, { Component } from "react";
import ChevronRight from "@material-ui/icons/ChevronRight";
import { inject, observer } from "mobx-react";
import { Tabs } from "antd";
import DronesList from "./DronesList";

const { TabPane } = Tabs;

@inject("mapStore")
@observer
class Sidebar extends Component {
  onClick = (longitude, latitude) => {
    const { setLocation, setZoom } = this.props.mapStore;
    setZoom();
    setLocation(longitude, latitude);
  };

  render() {
    const { landingPoints } = this.props.mapStore;
    return (
      <Tabs defaultActiveKey="1" type="card" style={{ marginTop: 70 }}>
        <TabPane tab="Droner" key="1">
          <DronesList />
        </TabPane>
        <TabPane tab="Hospitaler" key="2">
          <div className="hospitals-list">
            {landingPoints.map(point => (
              <div
                className="item"
                onClick={() =>
                  this.onClick(point.location[0], point.location[1])
                }
              >
                <p>{point.name}</p>
              </div>
            ))}
          </div>
        </TabPane>
        <TabPane tab="Anmodninger" key="3">
          <div className="reports-list">
            <div className="report">
              <p className="location">Odense</p>
              <p className="message">Vi skal bruge en drone</p>
            </div>
            <div className="report">
              <p className="location">Svendborg</p>
              <p className="message">Vi skal bruge en drone</p>
            </div>
          </div>
        </TabPane>
      </Tabs>
    );
  }
}

export default Sidebar;
