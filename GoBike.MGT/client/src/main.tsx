import React from "react";
import ReactDOM from "react-dom";
import { createStore } from "redux";
import { Provider } from "react-redux";
import * as Sentry from "@sentry/browser";
import App from "./containers/App";
import reducer from "./reducers/index";
import "bootstrap/dist/css/bootstrap.min.css";

// Sentry.init({
//   dsn: "https://4a3884a74719425cba44f3675cbb62d4@sentry.io/1782844"
// });
const store = createStore(reducer);
ReactDOM.render(
  <Provider store={store}>
    <App />
  </Provider>,
  document.getElementById("app")
);
