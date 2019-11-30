import React, { Component } from "react";
import { connect } from "react-redux";
import { onLoadAccountData } from "../actions/Action";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import PieChart from "../components/PieChart";
import PieChartData from "../model/PieChartData";

import Button from "react-bootstrap/Button";
import { isNullOrUndefined } from "util";
class AccountManagerPage extends Component {
  constructor(props) {
    super(props);
    this.onLoadData = this.onLoadData.bind(this);
  }

  onLoadData() {
    const { onLoadAccountData } = this.props;
    const data = [
      new PieChartData("Google", Math.round(Math.random() * 1000), "#FF6384"),
      new PieChartData("FB", Math.round(Math.random() * 1000), "#36A2EB"),
      new PieChartData("Local", Math.round(Math.random() * 1000), "#FFCE56")
    ];

    onLoadAccountData(data);
  }

  render() {
    let { accountData } = this.props;
    return (
      <Container fluid>
        <Row>
          <Col>This Is Account Manager Page</Col>
          <Col>
            <Button onClick={this.onLoadData}>Load Data</Button>
          </Col>
        </Row>
        <Row>
          <Col md={2}>{isNullOrUndefined(accountData) ? <div /> : <PieChart datas={accountData}></PieChart>}</Col>
        </Row>
      </Container>
    );
  }
}

/**
 * 繫結 Redux State
 * @param {object} state
 */
function mapStateToProps(state) {
  return { accountData: state.accountData };
}

/**
 * 繫結 Redux Action
 * @param {function} dispatch
 */
function mapDispatchToProps(dispatch) {
  return {
    onLoadAccountData: value => dispatch(onLoadAccountData(value))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(AccountManagerPage);
