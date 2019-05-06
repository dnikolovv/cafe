import React from "react";
import { render } from "react-dom";
import { Router } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import App from "./components/App";
import configureStore from "./redux/configureStore";
import { Provider as ReduxProvider } from "react-redux";
import "./index.css";
import history from "./history";

const store = configureStore();

render(
  <ReduxProvider store={store}>
    <Router history={history}>
      <App />
    </Router>
  </ReduxProvider>,
  document.getElementById("root")
);
