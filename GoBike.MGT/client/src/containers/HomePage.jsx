import React, { Component } from "react";
import { connect } from "react-redux";
import { Switch } from "react-router-dom";
import Container from "react-bootstrap/Container";
import PrivateRouter from "../router/PrivateRouter";
import AccountManagerPage from "./AccountManagerPage";
import MemberManagerPage from "./MemberManagerPage";
import MenuBar from "../components/MenuBar";
import TitleBar from "../components/TitleBar";

class HomePage extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div>
        <TitleBar />
        <MenuBar />
        <Container fluid>
          <Switch>
            <PrivateRouter path="/Home/Account" component={AccountManagerPage} />
            <PrivateRouter path="/Home/Member" component={MemberManagerPage} />
          </Switch>
        </Container>
      </div>
    );
  }
}

/**
 * 繫結 Redux State
 * @param {object} state
 */
function mapStateToProps(state) {
  return state;
}

export default connect(mapStateToProps)(HomePage);
