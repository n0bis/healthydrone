import React, { Component } from "react";
import { inject, observer } from "mobx-react";
import "antd/dist/antd.css";
import { Redirect } from "react-router-dom";
import { Row, Col, Form, Icon, Input, Button, Card, message } from "antd";

@inject("loginStore")
@observer
class Login extends Component {
  constructor(props) {
    super(props);
    this.authUser = (username, password) =>
      this.props.loginStore.authUser(username, password);
  }

  handleSubmit = e => {
    e.preventDefault();

    this.props.form.validateFields((err, values) => {
      if (!err) {
        this.authUser(values.userName, values.password);
      }
    });
  };

  render() {
    const { loginStore } = this.props;
    const { isLoggedInPersist, error, isLoading } = loginStore;
    const { getFieldDecorator } = this.props.form;

    if (isLoggedInPersist()) {
      return <Redirect to="/" />;
    }

    if (error) {
      message.error("Dit brugernavn eller adgangskode er ikke korrekt.");
      loginStore.setError(false);
    }

    return (
      <div
        className="login"
        style={{ background: "#ECECEC", padding: "30px", height: "100%" }}
      >
        <Row
          type="flex"
          justify="space-between"
          align="middle"
          style={{ height: "100%" }}
        >
          <Col span={6} offset={9} style={{ height: "300" }}>
            <Card title="Log ind" bordered={false} style={{ width: "100%" }}>
              <Form onSubmit={this.handleSubmit} className="login-form">
                <Form.Item label="Brugernavn">
                  {getFieldDecorator("userName", {
                    rules: [
                      { required: true, message: "Please input your username!" }
                    ]
                  })(
                    <Input
                      prefix={
                        <Icon
                          type="user"
                          style={{ color: "rgba(0,0,0,.25)" }}
                        />
                      }
                      placeholder="Username"
                    />
                  )}
                </Form.Item>
                <Form.Item label="Adgangskode">
                  {getFieldDecorator("password", {
                    rules: [
                      { required: true, message: "Please input your Password!" }
                    ]
                  })(
                    <Input
                      prefix={
                        <Icon
                          type="lock"
                          style={{ color: "rgba(0,0,0,.25)" }}
                        />
                      }
                      type="password"
                      placeholder="Password"
                    />
                  )}
                </Form.Item>
                <Form.Item>
                  <Button
                    type="primary"
                    htmlType="submit"
                    className="login-form-button"
                    loading={isLoading}
                  >
                    Log ind
                  </Button>
                </Form.Item>
              </Form>
            </Card>
          </Col>
        </Row>
      </div>
    );
  }
}

const loginForm = Form.create({ name: "normal_login" })(Login);

export default loginForm;