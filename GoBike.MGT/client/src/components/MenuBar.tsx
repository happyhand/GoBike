import React, { Component } from "react";
import { Dispatch } from "redux";
import { History } from "history";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { onAgentLogout } from "../actions/Action";
import { isNullOrUndefined } from "util";
import HomePage from "../containers/HomePage";
import AccountManagerPage from "../containers/AccountManagerPage";
import MemberManagerPage from "../containers/MemberManagerPage";
import "../css/Button.css";
import "../css/Nav.css";

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

  shouldComponentUpdate(nextProps: { menuKey: string }) {
    return !isNullOrUndefined(nextProps.menuKey) && this.props.menuKey !== nextProps.menuKey;
  }

  render() {
    const { menuKey } = this.props;
    return (
      <Navbar expand="lg" variant="dark" style={menuBar}>
        <Nav className="mr-auto" activeKey={menuKey}>
          <Nav.Link href={"#" + HomePage.PAGE_PATH} className="menuLink">
            首頁
          </Nav.Link>
          <Nav.Link href={"#" + AccountManagerPage.PAGE_PATH} className="menuLink">
            帳號管理
          </Nav.Link>
          <Nav.Link href={"#" + MemberManagerPage.PAGE_PATH} className="menuLink">
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
