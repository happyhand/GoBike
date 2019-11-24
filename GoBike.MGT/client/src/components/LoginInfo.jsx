import React, { Component } from "react";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import { onLoginValid, onLoginLoading, onAgentLogin, onAgentLogout } from "../actions/Action";
import Form from "react-bootstrap/Form";
import InputGroup from "react-bootstrap/InputGroup";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Button from "react-bootstrap/Button";
import "../css/LoginInfo.css";
import { faUser, faLock } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

class LoginInfo extends Component {
  constructor(props) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  /**
   * 發送表單
   * @param {Event} evt
   */
  handleSubmit(evt) {
    evt.preventDefault();
    const { onLoginValid, onLoginLoading, onAgentLogin, onAgentLogout, isLoading } = this.props;
    if (isLoading) {
      return;
    }

    const form = evt.currentTarget;
    if (form.checkValidity() === false) {
      onLoginValid(false);
      return;
    }

    onLoginValid(true);
    onLoginLoading(true);
    fetch("http://saboteur.hopto.org:18593/api/Agent/Login", {
      method: "POST",
      body: JSON.stringify({
        account: form.formAccount.value,
        password: form.formPassword.value
      }),
      headers: {
        Accept: "application/json",
        "Content-type": "application/json; charset=UTF-8"
      }
    })
      .then(response => {
        onLoginLoading(false);
        if (response.ok) {
          onAgentLogin();
          this.redirectPage();
        } else {
          onAgentLogout();
        }

        return response.json();
      })
      .then(json => {
        alert(json);
      })
      .catch(err => {
        console.log("Login Error:", err);
      });
  }

  /**
   * 轉導頁至 Home Page
   */
  redirectPage() {
    this.props.history.push("/Home");
  }

  render() {
    const { isValid } = this.props;
    const isLogin = localStorage.getItem("isLogin");
    if (isLogin === true) {
      this.redirectPage();
    }

    return (
      <Form noValidate validated={!isValid} onSubmit={this.handleSubmit}>
        <Container className="LoginInfoBox">
          <Row>
            <Col>
              <Form.Group role="form" controlId="formAccount">
                <InputGroup>
                  <InputGroup.Prepend>
                    <InputGroup.Text id="inputGroup-login-account" className="LoginInput">
                      <FontAwesomeIcon icon={faUser} size="2x" color="#999" />
                    </InputGroup.Text>
                  </InputGroup.Prepend>
                  <Form.Control
                    className="LoginInput"
                    size="lg"
                    type="text"
                    placeholder="Account"
                    aria-describedby="inputGroup-login-account"
                    required
                  />
                  <Form.Control.Feedback type="invalid">Please enter account.</Form.Control.Feedback>
                </InputGroup>
              </Form.Group>
            </Col>
          </Row>
          <Row>
            <Col>
              <Form.Group role="form" controlId="formPassword">
                <InputGroup>
                  <InputGroup.Prepend>
                    <InputGroup.Text id="inputGroup-login-password" className="LoginInput">
                      <FontAwesomeIcon icon={faLock} size="2x" color="#999" />
                    </InputGroup.Text>
                  </InputGroup.Prepend>
                  <Form.Control
                    className="LoginInput"
                    size="lg"
                    type="password"
                    placeholder="Password"
                    aria-describedby="inputGroup-login-password"
                    required
                  />
                  <Form.Control.Feedback type="invalid">Please enter password.</Form.Control.Feedback>
                </InputGroup>
              </Form.Group>
            </Col>
          </Row>
          <Row>
            <Col className="LoginButtonContent">
              <Button variant="login" type="submit" size="lg">
                Submit
              </Button>
            </Col>
          </Row>
        </Container>
      </Form>
    );
  }
}

/**
 * 繫結 Redux State
 * @param {object} state
 */
function mapStateToProps(state) {
  return { isValid: state.isValid, isLoading: state.isLoading };
}

/**
 * 繫結 Redux Action
 * @param {function} dispatch
 */
function mapDispatchToProps(dispatch) {
  return {
    onLoginValid: value => dispatch(onLoginValid(value)),
    onLoginLoading: value => dispatch(onLoginLoading(value)),
    onAgentLogin: () => dispatch(onAgentLogin()),
    onAgentLogout: () => dispatch(onAgentLogout())
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(LoginInfo));
