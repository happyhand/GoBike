import React, { Component, FormEvent } from "react";
import { Dispatch } from "redux";
import { History } from "history";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import Form from "react-bootstrap/Form";
import InputGroup from "react-bootstrap/InputGroup";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Button from "react-bootstrap/Button";
import { faUser, faLock } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { onLoginAction, onAgentLogin } from "../actions/Action";
import { isNullOrUndefined } from "util";
import CSS from "csstype";
import Spinner from "react-bootstrap/Spinner";

//#region Css
const loginInfoBox = {
  backgroundColor: "#fff",
  borderRadius: "8px",
  padding: "20px"
};

const loginInputContent = {
  borderRadius: "15px 0 0 15px"
};

const loginInputTitle = {
  borderRadius: "0 15px 15px 0"
};

const loginButtonContent: CSS.Properties = {
  textAlign: "right"
};

//#endregion
interface IProp {
  onLoginAction: Function;
  onAgentLogin: Function;
  isValid: boolean;
  isLogin: boolean;
  history: History;
}

class LoginInfo extends Component<IProp> {
  constructor(props: Readonly<IProp>) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  /**
   * 發送表單
   * @param {Event} evt
   */
  handleSubmit(evt: FormEvent<HTMLFormElement>) {
    evt.preventDefault();
    const { onLoginAction, onAgentLogin, isLogin } = this.props;
    if (isLogin) {
      return;
    }

    const form = evt.currentTarget;
    if (form.checkValidity() === false) {
      onLoginAction(false, false);
      return;
    }

    onLoginAction(true, true);
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
        onLoginAction(true, false);
        if (response.ok) {
          onAgentLogin();
          this.redirectPage();
          return undefined;
        }

        return response.json();
      })
      .then(message => {
        if (!isNullOrUndefined(message)) {
          alert(message);
        }
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
    const { isValid, isLogin } = this.props;
    return (
      <Form noValidate validated={!isValid} onSubmit={this.handleSubmit}>
        <Container style={loginInfoBox}>
          <Row>
            <Col>
              <Form.Group role="form" controlId="formAccount">
                <InputGroup>
                  <InputGroup.Prepend>
                    <InputGroup.Text id="inputGroup-login-account" style={loginInputContent}>
                      <FontAwesomeIcon icon={faUser} size="2x" color="#999" />
                    </InputGroup.Text>
                  </InputGroup.Prepend>
                  <Form.Control
                    size="lg"
                    type="text"
                    style={loginInputTitle}
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
                    <InputGroup.Text id="inputGroup-login-password" style={loginInputContent}>
                      <FontAwesomeIcon icon={faLock} size="2x" color="#999" />
                    </InputGroup.Text>
                  </InputGroup.Prepend>
                  <Form.Control
                    size="lg"
                    type="password"
                    style={loginInputTitle}
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
            <Col style={loginButtonContent}>
              {isLogin ? (
                <Spinner animation="border" />
              ) : (
                <Button className="login" type="submit" size="lg">
                  Submit
                </Button>
              )}
            </Col>
          </Row>
        </Container>
      </Form>
    );
  }
}

/**
 * 繫結 Redux State
 * @param {any} state
 */
function mapStateToProps(state: any) {
  return {
    isValid: isNullOrUndefined(state.isValid) ? true : state.isValid,
    isLogin: isNullOrUndefined(state.isLogin) ? false : state.isLogin
  };
}

/**
 * 繫結 Redux Action
 * @param {Dispatch} dispatch
 */
function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onLoginAction: (isValid: boolean, isLogin: boolean) => dispatch(onLoginAction(isValid, isLogin)),
    onAgentLogin: () => dispatch(onAgentLogin())
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(LoginInfo));
