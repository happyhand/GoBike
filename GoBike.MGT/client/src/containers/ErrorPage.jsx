import React, { Component } from "react";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
export default class ErrorPage extends Component {
  render() {
    return (
      <Container fluid>
        <Row>
          <Col>This Is Error Page</Col>
        </Row>
      </Container>
    );
  }
}
