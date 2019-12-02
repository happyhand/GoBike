import React, { Component } from "react";
import { connect } from "react-redux";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHome } from "@fortawesome/free-solid-svg-icons";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";

//#region Css
const bar = {
  backgroundColor: "#2b5971",
  padding: "10px 0px 0px 20px"
};

const title = {
  fontSize: "24px",
  color: "#fff"
};

const col = {
  paddingRight: "0px"
};
//#endregion

export default class TitleBar extends Component {
  render() {
    return (
      <Container fluid style={bar}>
        <Row>
          <Col md="auto" style={col}>
            <FontAwesomeIcon icon={faHome} size="3x" color="#fff" />
          </Col>
          <Col md="auto" style={col}>
            <span style={title}>加樂設計 Double Happiness</span>
          </Col>
        </Row>
      </Container>
    );
  }
}
