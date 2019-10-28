import React, { Component } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
export default class LoginInfo extends Component {
  render() {
    return (
      <Form>
        <Form.Group controlId="account">
          <Form.Label>Account</Form.Label>
          <Form.Control placeholder="Enter account" />
        </Form.Group>

        <Form.Group controlId="password">
          <Form.Label>Password</Form.Label>
          <Form.Control type="password" placeholder="Password" />
        </Form.Group>
        <Button variant="primary" type="submit">
          Submit
        </Button>
      </Form>
    );
  }
}
