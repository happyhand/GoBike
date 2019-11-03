import React from "react";
import ReactDOM from "react-dom";
import * as Sentry from "@sentry/browser";
import App from "./containers/App";
import "bootstrap/dist/css/bootstrap.min.css";

// Sentry.init({
//   dsn: "https://4a3884a74719425cba44f3675cbb62d4@sentry.io/1782844"
// });

ReactDOM.render(<App />, document.getElementById("app"));
