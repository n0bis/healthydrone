import React, { Component } from "react";
import { Input, Select } from "antd";
import { inject, observer } from "mobx-react";
import "../styles/nurseReport.scss";
import axios from "axios";

const { TextArea } = Input;
const { Option } = Select;

@inject("mapStore")
@observer
class NurseReport extends Component {
  componentDidMount = () => {
    const { fetchLandingPoints } = this.props.mapStore;
    fetchLandingPoints();
  };

  onSubmit = event => {
    event.preventDefault();
    var formData = new FormData(event.target);
    console.log(formData);
    axios.post("http://localhost:5000/RequetDrone", formData);
  };

  render() {
    const { landingPoints } = this.props.mapStore;
    return (
      <div className="container">
        <div className="box">
          <h2>Reporter</h2>
          <h3>
            For at sende en drone, skal du udfylde nedenstående form. Herefter
            vil en operatør sende en drone afsted, når ruten er blevet godkendt.
          </h3>
          <div className="form">
            <form action="#" onSubmit={this.onSubmit} method="POST">
              <div className="form-item">
                <p>Fra</p>
                <Select defaultValue="Vælg en lokation">
                  {landingPoints.map(point => (
                    <Option value={point.name}>{point.name}</Option>
                  ))}
                </Select>
              </div>
              <div className="form-item">
                <p>Til</p>
                <Select defaultValue="Vælg en lokation">
                  {landingPoints.map(point => (
                    <Option value={point.name}>{point.name}</Option>
                  ))}
                </Select>
              </div>
              <div className="form-item">
                <p>Besked</p>
                <TextArea
                  placeholder="Denne besked er en prototype og følger ikke med."
                  autoSize={{ minRows: 10, maxRows: 6 }}
                />
              </div>
              <button className="button-submit" type="submit">
                Send anmodning
              </button>
            </form>
          </div>
        </div>
      </div>
    );
  }
}

export default NurseReport;
