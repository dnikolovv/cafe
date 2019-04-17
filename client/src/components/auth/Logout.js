import React, { useEffect } from "react";
import { connect } from "react-redux";
import * as authActions from "../../redux/actions/authActions";
import PropTypes from "prop-types";

const Logout = ({ logout, history }) => {
  useEffect(() => {
    console.log(logout);
    debugger;
    logout();
    history.push("/");
  });

  return <div />;
};

Logout.propTypes = {
  logout: PropTypes.func.isRequired,
  history: PropTypes.object.isRequired
};

const mapDispatchToProps = {
  logout: authActions.logout
};

export default connect(
  null,
  mapDispatchToProps
)(Logout);
