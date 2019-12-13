import React, { Component } from "react";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Image from "react-bootstrap/Image";

//#region Css
const bar = {
  backgroundColor: "#2b5971",
  padding: "10px 0px 0px 20px"
};

const title = {
  verticalAlign: "-webkit-baseline-middle",
  fontSize: "24px",
  color: "#fff"
};

const col = {
  paddingRight: "0px"
};
//#endregion

export default class TitleBar extends Component {
  render() {
    const logoUrl = require("../assets/img/logo_s.png");
    return (
      <Container fluid style={bar}>
        <Row>
          <Col md="auto" style={col}>
            <Image src={logoUrl} />
            <span style={title}>加樂設計 Double Happiness</span>
          </Col>
        </Row>
      </Container>
    );
  }
}
