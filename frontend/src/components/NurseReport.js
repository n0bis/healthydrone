import React, { Component } from "react";
import { Input, Select } from "antd";
import "../styles/nurseReport.scss";

const { TextArea } = Input;
const { Option } = Select;

class NurseReport extends Component {
  render() {
    return (
      <div className="container">
        <div className="box">
          <h2>Reporter</h2>
          <h3>
            For at sende en drone, skal du udfylde nedenstående form. Herefter
            vil en operatør sende en drone afsted, når ruten er blevet godkendt.
          </h3>
          <div className="form">
            <form action="#" method="POST">
              <div className="form-item">
                <Select defaultValue="lucy">
                  <Option value="jack">Jack</Option>
                  <Option value="lucy">Lucy</Option>
                  <Option value="disabled" disabled>
                    Disabled
                  </Option>
                  <Option value="Yiminghe">yiminghe</Option>
                </Select>
              </div>
              <div className="form-item">
                <TextArea
                  placeholder="Autosize height with minimum and maximum number of lines"
                  autoSize={{ minRows: 10, maxRows: 6 }}
                />
              </div>
              <button className="button-submit">Send anmodning</button>
            </form>
          </div>
        </div>
      </div>
    );
  }
}

export default NurseReport;
