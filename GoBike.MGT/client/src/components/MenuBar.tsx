import React, { Component } from "react";
import { Dispatch } from "redux";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import { History } from "history";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { onAgentLogout } from "../actions/Action";
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
    return (
      <Navbar expand="lg" variant="dark" style={menuBar}>
        <Nav className="mr-auto">
          <Nav.Link href="#Home" className="menuLink">
            首頁
          </Nav.Link>
          <Nav.Link href="#Account" className="menuLink">
            帳號管理
          </Nav.Link>
          <Nav.Link href="#Member" className="menuLink">
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
 * 繫結 Redux Action
 * @param {Dispatch} dispatch
 */
function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onAgentLogout: () => dispatch(onAgentLogout())
  };
}

export default withRouter(connect(null, mapDispatchToProps)(MenuBar));
