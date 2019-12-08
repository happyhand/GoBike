export default class LineChartData {
  label: string;
  counts: number[];
  constructor(label: string, counts: number[]) {
    this.label = label;
    this.counts = counts;
  }
}
