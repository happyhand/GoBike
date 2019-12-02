import React, { Component } from "react";
import { Route, Switch } from "react-router-dom";
import Container from "react-bootstrap/Container";
import PrivateRouter from "../router/PrivateRouter";
import MenuBar from "../components/MenuBar";
import TitleBar from "../components/TitleBar";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import HomePage from "./HomePage";
import AccountManagerPage from "./AccountManagerPage";
import MemberManagerPage from "./MemberManagerPage";
import ErrorPage from "./ErrorPage";

//#region Css
const col = {
  padding: "0px"
};

//#endregion

export default class ContainerPage extends Component {
  render() {
    return (
      <div>
        <Container fluid>
          <Row>
            <Col style={col}>
              <TitleBar />
            </Col>
          </Row>
          <Row>
            <Col style={col}>
              <MenuBar />
            </Col>
          </Row>
          <Row>
            <Col style={col}>
              <Container fluid>
                <Switch>
                  <PrivateRouter exact path="/" component={HomePage} />
                  <PrivateRouter exact path="/Home" component={HomePage} />
                  <PrivateRouter exact path="/Account" component={AccountManagerPage} />
                  <PrivateRouter exact path="/Member" component={MemberManagerPage} />
                  <Route path="*">
                    <ErrorPage />
                  </Route>
                </Switch>
              </Container>
            </Col>
          </Row>
        </Container>
      </div>
    );
  }
}
