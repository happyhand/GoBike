export default class PieChartData {
  label: string;
  count: number;
  color: string;
  constructor(label: string, count: number, color: string) {
    this.label = label;
    this.count = count;
    this.color = color;
  }
}
