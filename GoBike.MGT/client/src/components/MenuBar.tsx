import React, { Component } from "react";
import { Dispatch } from "redux";
import { History } from "history";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { routerTag } from "../config/appconfig.json";
import { onAgentLogout } from "../actions/Action";

import "../css/Button.css";
import "../css/Nav.css";
import { isNullOrUndefined } from "util";

//#region Css
const menuBar = {
  backgroundColor: "#2b5971",
  borderRadius: "0px",
  fontSize: "16px"
};

//#endregion
interface IProp {
  menuKey: string;
  onAgentLogout: Function;
  history: History;
}

class MenuBar extends Component<IProp> {
  constructor(props: Readonly<IProp>) {
    super(props);
    this.handleLogout = this.handleLogout.bind(this);
  }

  handleLogout(evt: React.MouseEvent) {
    const { onAgentLogout } = this.props;
    const isLogout = confirm("確定要登出嗎?");
    if (isLogout) {
      onAgentLogout();
      this.props.history.push("/Login");
    }
  }

  render() {
    const { menuKey } = this.props;
    return (
      <Navbar expand="lg" variant="dark" style={menuBar}>
        <Nav className="mr-auto" activeKey={menuKey}>
          <Nav.Link href={"#" + routerTag.HomePage} className="menuLink">
            首頁
          </Nav.Link>
          <Nav.Link href={"#" + routerTag.AccountManagerPage} className="menuLink">
            帳號管理
          </Nav.Link>
          <Nav.Link href={"#" + routerTag.MemberManagerPage} className="menuLink">
            會員管理
          </Nav.Link>
        </Nav>
        <Nav>
          <Nav.Item>
            <button className="Logout" onClick={this.handleLogout}>
              Logout
            </button>
          </Nav.Item>
        </Nav>
      </Navbar>
    );
  }
}

/**
 * 繫結 Redux State
 * @param {any} state
 */
function mapStateToProps(state: any, own: any) {
  own.menuKey = isNullOrUndefined(state.menuKey) ? own.menuKey : state.menuKey;
  return state;
}

/**
 * 繫結 Redux Action
 * @param {Dispatch} dispatch
 */
function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onAgentLogout: () => dispatch(onAgentLogout())
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(MenuBar));
