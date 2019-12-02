import React, { Component } from "react";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Image from "react-bootstrap/Image";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBiking } from "@fortawesome/free-solid-svg-icons";
import LoginInfo from "../components/LoginInfo";
import "../css/LoginPage.css";

export default class LoginPage extends Component {
  render() {
    const logo = require("../assets/img/logo.png");
    return (
      <Container fluid className="LoginPageContainer">
        <Row className="justify-content-md-center">
          <Col md="auto">
            <FontAwesomeIcon icon={faBiking} size="5x" color="#fff" />
          </Col>
        </Row>
        <Row className="justify-content-md-center LoginPageRow">
          <Col md="auto">
            <Image src={logo} className="LoginPageLogo"></Image>
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
