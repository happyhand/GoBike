import React, { Component } from "react";
import { Dispatch } from "redux";
import { connect } from "react-redux";
import DatePicker from "react-datepicker";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Button from "react-bootstrap/Button";
import Spinner from "react-bootstrap/Spinner";
import Calendar from "react-input-calendar";
import { isNullOrUndefined } from "util";
import { onChangeMenu, onLoadHomeTodayData, onLoadHomeChartData, onLoadHomeChartDate } from "../actions/Action";
import LineChart from "../components/LineChart";
import PieChart from "../components/PieChart";
import LineChartData from "../model/LineChartData";
import PieChartData from "../model/PieChartData";
import Utility from "../core/Utility";
import CSS from "csstype";
import "react-datepicker/dist/react-datepicker.css";

//#region Css
const textRow: CSS.Properties = {
  padding: "10px 0px"
};

const text: CSS.Properties = {
  color: "#666",
  fontSize: "16px"
};

const chart: CSS.Properties = {
  padding: "20px",
  textAlign: "center",
  color: "#666",
  fontSize: "16px"
};

const dateText: CSS.Properties = {
  margin: "0px 10px"
};

//#endregion

interface IProp {
  isLoadTodayData: boolean;
  isLoadChartData: boolean;
  todayData: number[];
  chartData: any;
  startDate: Date;
  endDate: Date;
  onChangeMenu: Function;
  onLoadHomeTodayData: Function;
  onLoadHomeChartData: Function;
  onLoadHomeChartDate: Function;
}

class HomePage extends Component<IProp> {
  static PAGE_PATH: string = "Home";
  constructor(props: Readonly<IProp>) {
    super(props);
    this.onLoadToday = this.onLoadToday.bind(this);
    this.onLoadChart = this.onLoadChart.bind(this);
    this.onHandleChartLoadStartDate = this.onHandleChartLoadStartDate.bind(this);
    this.onHandleChartLoadEndDate = this.onHandleChartLoadEndDate.bind(this);
    this.onUpdateChartLoadDate = this.onUpdateChartLoadDate.bind(this);
  }

  //#region Load Today
  /**
   * 讀取今日資料
   */
  onLoadToday() {
    const { isLoadTodayData, onLoadHomeTodayData } = this.props;
    if (isLoadTodayData) {
      return;
    }

    const loginTodayData = this.onLoadLoginTodayData();
    const createEventTodayData = this.onLoadCreateEventTodayData();
    const completeRideTodayData = this.onLoadCompleteRideTodayData();
    const numberOfRegisterTodayData = this.onLoadNumberOfRegisterTodayData();
    const createTeamTodayData = this.onLoadCreateTeamTodayData();
    const shareRoadLineTodayData = this.onLoadShareRoadLineTodayData();
    onLoadHomeTodayData(true, undefined);
    setTimeout(() => {
      onLoadHomeTodayData(false, [
        loginTodayData,
        createEventTodayData,
        completeRideTodayData,
        numberOfRegisterTodayData,
        createTeamTodayData,
        shareRoadLineTodayData
      ]);
    }, 1000);
  }

  /**
   * 讀取今日登入資料
   */
  onLoadLoginTodayData(): number {
    return Math.round(Math.random() * 1000);
  }

  /**
   * 讀取今日建立活動資料
   */
  onLoadCreateEventTodayData(): number {
    return Math.round(Math.random() * 1000);
  }

  /**
   * 讀取今日完成騎乘資料
   */
  onLoadCompleteRideTodayData(): number {
    return Math.round(Math.random() * 1000);
  }

  /**
   * 讀取今日註冊人數資料
   */
  onLoadNumberOfRegisterTodayData(): number {
    return Math.round(Math.random() * 1000);
  }

  /**
   * 讀取今日建立車隊資料
   */
  onLoadCreateTeamTodayData(): number {
    return Math.round(Math.random() * 1000);
  }

  /**
   * 讀取今日分享路線資料
   */
  onLoadShareRoadLineTodayData(): number {
    return Math.round(Math.random() * 1000);
  }
  //#endregion

  //#region Load Chart
  /**
   * 讀取圖表資料
   */
  onLoadChart(startDate: Date, endDate: Date) {
    const { isLoadChartData, onLoadHomeChartData } = this.props;
    if (isLoadChartData) {
      return;
    }

    const loginChartData = this.onLoadLoginChartData(startDate, endDate);
    const createEventChartData = this.onLoadCreateEventChartData(startDate, endDate);
    const completeRideChartData = this.onLoadCompleteRideChartData(startDate, endDate);
    const numberOfRegisterChartData = this.onLoadNumberOfRegisterChartData(startDate, endDate);
    const createTeamChartData = this.onLoadCreateTeamChartData(startDate, endDate);
    const shareRoadLineChartData = this.onLoadShareRoadLineChartData(startDate, endDate);
    const registerChartData = this.onLoadRegisterChartData();
    const teamAreaChartData = this.onLoadTeamAreaChartData();
    onLoadHomeChartData(true, {});
    setTimeout(() => {
      onLoadHomeChartData(false, [
        loginChartData,
        createEventChartData,
        completeRideChartData,
        numberOfRegisterChartData,
        createTeamChartData,
        shareRoadLineChartData,
        registerChartData,
        teamAreaChartData
      ]);
    }, 1000);
  }

  /**
   * 讀取登入圖表資料
   */
  onLoadLoginChartData(startDate: Date, endDate: Date): any {
    const groups: string[] = Utility.getDateGroups(startDate, endDate);
    const google: LineChartData = new LineChartData("Google", []);
    const fb: LineChartData = new LineChartData("FB", []);
    const local: LineChartData = new LineChartData("Local", []);
    for (let index = 0; index <= groups.length; index++) {
      google.counts.push(Math.round(Math.random() * 10) * 50);
      fb.counts.push(Math.round(Math.random() * 10) * 50);
      local.counts.push(Math.round(Math.random() * 10) * 50);
    }

    return { groups: groups, datas: [google, fb, local] };
  }

  /**
   * 讀取建立活動圖表資料
   */
  onLoadCreateEventChartData(startDate: Date, endDate: Date): any {
    const groups: string[] = Utility.getDateGroups(startDate, endDate);
    const datas: LineChartData = new LineChartData("活動數量", []);
    for (let index = 0; index <= groups.length; index++) {
      datas.counts.push(Math.round(Math.random() * 10) * 50);
    }

    return { groups: groups, datas: [datas] };
  }

  /**
   * 讀取完成騎乘圖表資料
   */
  onLoadCompleteRideChartData(startDate: Date, endDate: Date): any {
    const groups: string[] = Utility.getDateGroups(startDate, endDate);
    const datas: LineChartData = new LineChartData("騎乘數量", []);
    for (let index = 0; index <= groups.length; index++) {
      datas.counts.push(Math.round(Math.random() * 10) * 50);
    }

    return { groups: groups, datas: [datas] };
  }

  /**
   * 讀取註冊人數圖表資料
   */
  onLoadNumberOfRegisterChartData(startDate: Date, endDate: Date): any {
    const groups: string[] = Utility.getDateGroups(startDate, endDate);
    const google: LineChartData = new LineChartData("Google", []);
    const fb: LineChartData = new LineChartData("FB", []);
    const local: LineChartData = new LineChartData("Local", []);
    for (let index = 0; index <= groups.length; index++) {
      google.counts.push(Math.round(Math.random() * 10) * 50);
      fb.counts.push(Math.round(Math.random() * 10) * 50);
      local.counts.push(Math.round(Math.random() * 10) * 50);
    }

    return { groups: groups, datas: [google, fb, local] };
  }

  /**
   * 讀取建立車隊圖表資料
   */
  onLoadCreateTeamChartData(startDate: Date, endDate: Date): any {
    const groups: string[] = Utility.getDateGroups(startDate, endDate);
    const datas: LineChartData = new LineChartData("車隊數量", []);
    for (let index = 0; index <= groups.length; index++) {
      datas.counts.push(Math.round(Math.random() * 10) * 50);
    }

    return { groups: groups, datas: [datas] };
  }

  /**
   * 讀取分享路線圖表資料
   */
  onLoadShareRoadLineChartData(startDate: Date, endDate: Date): any {
    const groups: string[] = Utility.getDateGroups(startDate, endDate);
    const datas: LineChartData = new LineChartData("分享數量", []);
    for (let index = 0; index <= groups.length; index++) {
      datas.counts.push(Math.round(Math.random() * 10) * 50);
    }

    return { groups: groups, datas: [datas] };
  }

  /**
   * 讀取註冊圖表資料
   */
  onLoadRegisterChartData(): PieChartData[] {
    return [
      new PieChartData("Google", Math.round(Math.random() * 1000)),
      new PieChartData("FB", Math.round(Math.random() * 1000)),
      new PieChartData("Local", Math.round(Math.random() * 1000))
    ];
  }

  /**
   * 讀取車隊地區圖表資料
   */
  onLoadTeamAreaChartData(): PieChartData[] {
    return [
      new PieChartData("台北", Math.round(Math.random() * 1000)),
      new PieChartData("台中", Math.round(Math.random() * 1000)),
      new PieChartData("台南", Math.round(Math.random() * 1000)),
      new PieChartData("台東", Math.round(Math.random() * 1000))
    ];
  }

  /**
   * 紀錄圖表起始日期
   */
  onHandleChartLoadStartDate(date: Date) {
    const { endDate, isLoadChartData } = this.props;
    if (isLoadChartData) {
      return;
    }

    this.onUpdateChartLoadDate(date, endDate);
  }

  /**
   * 紀錄圖表結束日期
   */
  onHandleChartLoadEndDate(date: Date) {
    const { startDate, isLoadChartData } = this.props;
    if (isLoadChartData) {
      return;
    }

    this.onUpdateChartLoadDate(startDate, date);
  }

  /**
   * 更新圖表起始日期
   */
  onUpdateChartLoadDate(startDate: Date, endDate: Date) {
    const { onLoadHomeChartDate } = this.props;
    onLoadHomeChartDate(startDate, endDate);
    if (startDate && endDate) {
      this.onLoadChart(startDate, endDate);
    }
  }

  //#endregion
  /**
   * 偵測是否須更新組件
   */
  shouldComponentUpdate(nextProps: {
    isLoadChartData: boolean;
    isLoadTodayData: boolean;
    startDate: Date;
    endDate: Date;
  }) {
    return (
      !isNullOrUndefined(nextProps.isLoadChartData) ||
      !isNullOrUndefined(nextProps.isLoadTodayData) ||
      !isNullOrUndefined(nextProps.startDate) ||
      !isNullOrUndefined(nextProps.endDate)
    );
  }

  /**
   * 組件完成點
   */
  componentDidMount() {
    const { onChangeMenu } = this.props;
    const date = new Date();
    const startData = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    const endDate = new Date(date.getFullYear(), date.getMonth() + 1, 1);
    endDate.setDate(endDate.getDate() - 1);
    const self = this;
    setTimeout(() => {
      self.onUpdateChartLoadDate(startData, endDate);
      self.onLoadToday();
    }, 50);
    onChangeMenu("#" + HomePage.PAGE_PATH);
  }

  /**
   * 組件
   */
  render() {
    const { todayData, chartData, startDate, endDate } = this.props;
    return (
      <Container fluid>
        <Row>
          <Col>
            <Button className="normal" onClick={this.onLoadToday}>
              Reload
            </Button>
          </Col>
        </Row>
        <Row style={textRow}>
          <Col>
            <span style={text}>今日登入人數：{todayData ? todayData[0] : <Spinner animation="border" />}</span>
          </Col>
          <Col>
            <span style={text}>今日創建活動數：{todayData ? todayData[1] : <Spinner animation="border" />}</span>
          </Col>
          <Col>
            <span style={text}>今日完成騎乘數：{todayData ? todayData[2] : <Spinner animation="border" />}</span>
          </Col>
        </Row>
        <Row style={textRow}>
          <Col>
            <span style={text}>今日註冊人數：{todayData ? todayData[3] : <Spinner animation="border" />}</span>
          </Col>
          <Col>
            <span style={text}>今日車隊創建數：{todayData ? todayData[4] : <Spinner animation="border" />}</span>
          </Col>
          <Col>
            <span style={text}>今日分享路線數：{todayData ? todayData[5] : <Spinner animation="border" />}</span>
          </Col>
        </Row>
        <Row>
          <Col>
            <br />
            <br />
          </Col>
        </Row>
        <Row>
          <Col md={2}>
            <span style={text}>統計圖表(預設當月)</span>
          </Col>
          <Col>
            <span style={text}>選擇資料範圍：</span>
            <DatePicker
              selected={isNullOrUndefined(startDate) ? new Date() : startDate}
              selectsStart
              startDate={startDate}
              endDate={endDate}
              onChange={this.onHandleChartLoadStartDate}
            />
            <span style={dateText}>-</span>
            <DatePicker
              selected={isNullOrUndefined(endDate) ? new Date() : endDate}
              selectsEnd
              startDate={startDate}
              endDate={endDate}
              onChange={this.onHandleChartLoadEndDate}
            />
          </Col>
        </Row>
        <Row>
          <Col>
            <hr />
          </Col>
        </Row>
        <Row>
          <Col>
            <span style={text}>
              資料日期：
              {isNullOrUndefined(startDate) || isNullOrUndefined(endDate)
                ? "未輸入"
                : Utility.getDateFormat(startDate, "yyyy.MM.dd") + " ~ " + Utility.getDateFormat(endDate, "yyyy.MM.dd")}
            </span>
          </Col>
        </Row>
        <Row>
          <Col md={3}>
            <div style={chart}>
              <span>登入人數圖表</span>
              <LineChart
                datas={chartData && chartData[0] ? chartData[0].datas : undefined}
                groups={chartData && chartData[0] ? chartData[0].groups : undefined}
              />
            </div>
          </Col>
          <Col md={3}>
            <div style={chart}>
              <span>創建活動數圖表</span>
              <LineChart
                datas={chartData && chartData[1] ? chartData[1].datas : undefined}
                groups={chartData && chartData[1] ? chartData[1].groups : undefined}
              />
            </div>
          </Col>
          <Col md={3}>
            <div style={chart}>
              <span>完成騎乘數圖表</span>
              <LineChart
                datas={chartData && chartData[2] ? chartData[2].datas : undefined}
                groups={chartData && chartData[2] ? chartData[2].groups : undefined}
              />
            </div>
          </Col>
        </Row>
        <Row>
          <Col md={3}>
            <div style={chart}>
              <span>註冊人數圖表</span>
              <LineChart
                datas={chartData && chartData[3] ? chartData[3].datas : undefined}
                groups={chartData && chartData[3] ? chartData[3].groups : undefined}
              />
            </div>
          </Col>
          <Col md={3}>
            <div style={chart}>
              <span>車隊創建數圖表</span>
              <LineChart
                datas={chartData && chartData[4] ? chartData[4].datas : undefined}
                groups={chartData && chartData[4] ? chartData[4].groups : undefined}
              />
            </div>
          </Col>
          <Col md={3}>
            <div style={chart}>
              <span>分享路線數圖表</span>
              <LineChart
                datas={chartData && chartData[5] ? chartData[5].datas : undefined}
                groups={chartData && chartData[5] ? chartData[5].groups : undefined}
              />
            </div>
          </Col>
        </Row>
        <Row>
          <Col md={3}>
            <div style={chart}>
              <span>總註冊來源分布圖</span>
              <PieChart datas={chartData ? chartData[6] : undefined}></PieChart>
            </div>
          </Col>
          <Col md={3}>
            <div style={chart}>
              <span>車隊地區分布圖</span>
              <PieChart datas={chartData ? chartData[7] : undefined}></PieChart>
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
function mapStateToProps(state: any) {
  return state;
}

/**
 * 繫結 Redux Action
 * @param {Dispatch} dispatch
 */
function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onChangeMenu: (menuKey: string) => dispatch(onChangeMenu(menuKey)),
    onLoadHomeTodayData: (isLoading: boolean, data: any) => dispatch(onLoadHomeTodayData(isLoading, data)),
    onLoadHomeChartData: (isLoading: boolean, data: any) => dispatch(onLoadHomeChartData(isLoading, data)),
    onLoadHomeChartDate: (startDate: Date, endDate: Date) => dispatch(onLoadHomeChartDate(startDate, endDate))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(HomePage);
