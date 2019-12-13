import React, { Component } from "react";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Image from "react-bootstrap/Image";
import LoginInfo from "../components/LoginInfo";
import "../css/LoginPage.css";

//#region Css
const logo = {
  verticalAlign: "-webkit-baseline-middle",
  fontSize: "72px",
  color: "#fff"
};

//#endregion

export default class LoginPage extends Component {
  render() {
    const logoUrl = require("../assets/img/logo.png");
    return (
      <Container fluid className="LoginPageContainer">
        <Row className="justify-content-md-center LoginPageRow">
          <Col md="auto">
            <Image src={logoUrl} />
            <span style={logo}>加樂設計</span>
          </Col>
        </Row>
        <Row className="justify-content-md-center LoginPageRow">
          <Col md="auto">
            <LoginInfo />
          </Col>
        </Row>
      </Container>
    );
  }
}
