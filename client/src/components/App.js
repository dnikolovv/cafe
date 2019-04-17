import React from "react";
import Header from "./common/Header";
import PageNotFound from "./PageNotFound";
import HomePage from "./home/HomePage";
import LoginPage from "./auth/LoginPage";
import AdminPage from "./admin/AdminPage";
import UserProvider from "./UserProvider";
import Logout from "./auth/Logout";
import { Route, Switch } from "react-router-dom";

const App = () => {
  return (
    <div className="container-fluid">
      <Header />
      <Switch>
        <Route exact path="/" component={HomePage} />
        <Route path="/login" component={LoginPage} />
        <Route path="/logout" component={Logout} />
        <Route path="/admin" component={AdminPage} />
        <Route component={PageNotFound} />
      </Switch>
      <UserProvider />
    </div>
  );
};

export default App;
