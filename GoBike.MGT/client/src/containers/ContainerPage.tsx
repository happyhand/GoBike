import React, { Component } from "react";
import { Route, Switch } from "react-router-dom";
import { connect } from "react-redux";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import HomePage from "./HomePage";
import MenuBar from "../components/MenuBar";
import TitleBar from "../components/TitleBar";
import AccountManagerPage from "./AccountManagerPage";
import MemberManagerPage from "./MemberManagerPage";
import ErrorPage from "./ErrorPage";

//#region Css
const col = {
  padding: "0px"
};

//#endregion
interface IProp {
  menuKey: string;
}
class ContainerPage extends Component<IProp> {
  render() {
    const { menuKey } = this.props;
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
              <MenuBar menuKey={menuKey} />
            </Col>
          </Row>
          <Row>
            <Col style={col}>
              <Container fluid>
                <Switch>
                  <Route exact path={"/"} component={HomePage} />
                  <Route exact path={"/" + HomePage.PAGE_PATH} component={HomePage} />
                  <Route exact path={"/" + AccountManagerPage.PAGE_PATH} component={AccountManagerPage} />
                  <Route exact path={"/" + MemberManagerPage.PAGE_PATH} component={MemberManagerPage} />
                  <Route path={"*"} component={ErrorPage} />
                </Switch>
              </Container>
            </Col>
          </Row>
        </Container>
      </div>
    );
  }
}

/**
 * 繫結 Redux State
 * @param {any} state
 */
function mapStateToProps(state: any, own: any) {
  return state;
}

export default connect(mapStateToProps)(ContainerPage);
