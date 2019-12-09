import React, { Component } from "react";
import { Dispatch } from "redux";
import { connect } from "react-redux";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Button from "react-bootstrap/Button";
import { isNullOrUndefined } from "util";
import { routerTag } from "../config/appconfig.json";
import { onLoadHomeData, onChangeMenu } from "../actions/Action";
import LineChart from "../components/LineChart";
import PieChart from "../components/PieChart";
import LineChartData from "../model/LineChartData";
import PieChartData from "../model/PieChartData";
import CSS from "csstype";

//#region Css
const chart: CSS.Properties = {
  textAlign: "center",
  color: "#666",
  fontSize: "16px"
};

//#endregion
interface IProp {
  isLoading: boolean;
  loginData: any;
  registerData: PieChartData[];
  teamAreaData: PieChartData[];
  onChangeMenu: Function;
  onLoadHomeData: Function;
}
class HomePage extends Component<IProp> {
  constructor(props: Readonly<IProp>) {
    super(props);
    const { onChangeMenu } = this.props;
    this.onReload = this.onReload.bind(this);
    this.onReload();
    onChangeMenu("#" + routerTag.HomePage);
  }

  onLoadLoginData(): any {
    const startDay: number = Math.round(Math.random() * 14) + 1;
    const stopDay: number = Math.round(Math.random() * 15) + 16;
    const groups: number[] = [];
    const google: LineChartData = new LineChartData("Google", []);
    const fb: LineChartData = new LineChartData("FB", []);
    const local: LineChartData = new LineChartData("Local", []);
    for (let index = startDay; index <= stopDay; index++) {
      groups.push(index);
      google.counts.push(Math.round(Math.random() * 100));
      fb.counts.push(Math.round(Math.random() * 100));
      local.counts.push(Math.round(Math.random() * 100));
    }

    return { groups: groups, datas: [google, fb, local] };
  }

  onLoadRegisterData(): PieChartData[] {
    return [
      new PieChartData("Google", Math.round(Math.random() * 1000)),
      new PieChartData("FB", Math.round(Math.random() * 1000)),
      new PieChartData("Local", Math.round(Math.random() * 1000))
    ];
  }

  onLoadTeamAreaData(): PieChartData[] {
    return [
      new PieChartData("台北", Math.round(Math.random() * 1000)),
      new PieChartData("台中", Math.round(Math.random() * 1000)),
      new PieChartData("台南", Math.round(Math.random() * 1000)),
      new PieChartData("台東", Math.round(Math.random() * 1000))
    ];
  }

  onReload() {
    const { onLoadHomeData, isLoading } = this.props;
    if (isLoading) {
      return;
    }

    const loginData = this.onLoadLoginData();
    const registerData = this.onLoadRegisterData();
    const teamAreaData = this.onLoadTeamAreaData();
    onLoadHomeData(true, {});
    setTimeout(() => {
      onLoadHomeData(false, { loginData: loginData, registerData: registerData, teamAreaData: teamAreaData });
    }, 1000);
  }

  render() {
    const { loginData, registerData, teamAreaData } = this.props;

    return (
      <Container fluid>
        <Row>
          <Col>
            <Button onClick={this.onReload}>Reload</Button>
          </Col>
        </Row>
        <Row>
          <Col md={3}>
            <div style={chart}>
              <span>登入人數圖表</span>
              <LineChart
                datas={loginData ? loginData.datas : undefined}
                groups={loginData ? loginData.groups : undefined}
              />
            </div>
          </Col>
        </Row>
        <Row>
          <Col md={3}>
            <div style={chart}>
              <span>總註冊來源分布圖</span>
              <PieChart datas={registerData}></PieChart>
            </div>
          </Col>
          <Col md={3}>
            <div style={chart}>
              <span>車隊地區分布圖</span>
              <PieChart datas={teamAreaData}></PieChart>
            </div>
          </Col>
        </Row>
      </Container>
    );
  }
}

/**
 * 繫結 Redux State
 * @param {any} state
 */
function mapStateToProps(state: any, own: any) {
  own.isLoading = isNullOrUndefined(state.isLoading) ? own.isLoading : state.isLoading;
  own.loginData = isNullOrUndefined(state.data) ? own.loginData : state.data.loginData;
  own.registerData = isNullOrUndefined(state.data) ? own.registerData : state.data.registerData;
  own.teamAreaData = isNullOrUndefined(state.data) ? own.teamAreaData : state.data.teamAreaData;

  return state;
}

/**
 * 繫結 Redux Action
 * @param {Dispatch} dispatch
 */
function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onChangeMenu: (menuKey: string) => dispatch(onChangeMenu(menuKey)),
    onLoadHomeData: (isLoading: boolean, data: any) => dispatch(onLoadHomeData(isLoading, data))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(HomePage);
