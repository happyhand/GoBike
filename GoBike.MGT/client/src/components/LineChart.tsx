import React, { Component } from "react";
import { Line } from "react-chartjs-2";
import LineChartData from "../model/LineChartData";
import { colors } from "../config/appconfig.json";
import { isNullOrUndefined } from "util";

interface IProp {
  groups: string[];
  datas: LineChartData[];
}
export default class LineChart extends Component<IProp> {
  constructor(props: Readonly<IProp>) {
    super(props);
  }

  render() {
    const { groups, datas } = this.props;
    if (isNullOrUndefined(datas)) {
      return (
        <div>
          <span>Loading</span>
        </div>
      );
    }

    let data = {
      labels: groups,
      datasets: datas.map((item, index) => {
        return {
          label: item.label, // 資料名稱
          fill: false, // 是否填滿區域 (不動)
          lineTension: 0, // 區域曲線曲度 (不動)
          // backgroundColor: "#fbb4ae", // 區域填滿顏色 (不動)
          borderColor: colors[index][0], // 區域曲線顏色
          // borderCapStyle: "butt", // 線段連接格式 (不動)
          // borderDash: [], // 格線設定 (不動)
          // borderDashOffset: 0, // 格線偏移量 (不動)
          // borderJoinStyle: "miter", // 格線樣式 (不動)
          // pointBorderColor: colors[index][1], // 區域節點顏色 (不動)
          pointBackgroundColor: colors[index][1], // 區域節點顏色
          // pointBorderWidth: 0, // 區域節點寬度 (不動)
          pointHoverRadius: 7, // 區域節點滑入寬度 (不動)
          pointHoverBackgroundColor: colors[index][1], // 區域節點滑入顏色
          // pointHoverBorderColor: colors[index][1], // 區域節點滑入邊框顏色 (不動)
          // pointHoverBorderWidth: 0, // 區域節點滑入邊框寬度 (不動)
          pointRadius: 3, // 區域節點半徑 (不動)
          pointHitRadius: 10, // 區域節點偵測滑鼠事件範圍 (不動)
          data: item.counts
        };
      })
    };

    return <Line data={data} />;
  }
}
