import React, { Component } from "react";
import ChevronRight from "@material-ui/icons/ChevronRight";
import { inject, observer } from "mobx-react";
import { Tabs } from "antd";
import DronesList from "./DronesList";

const { TabPane } = Tabs;
@observer
class Sidebar extends Component {
  render() {
    return (
      <Tabs defaultActiveKey="1" type="card" style={{ marginTop: 70 }}>
        <TabPane tab="Droner" key="1">
          <DronesList />
        </TabPane>
        <TabPane tab="Hospitaler" key="2">
          <div className="hospitals-list">
            <div className="item">
              <p>Svendborg</p>
            </div>
            <div className="item">
              <p>Odense</p>
            </div>
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
