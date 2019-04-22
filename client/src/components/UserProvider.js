import React, { useEffect } from "react";
import * as authActions from "../redux/actions/authActions";
import { connect } from "react-redux";
import PropTypes from "prop-types";

const UserProvider = ({ loadCurrentUser }) => {
  useEffect(() => {
    if (localStorage.getItem("access_token")) {
      loadCurrentUser();
    }
  }, []);

  return <div />;
};

UserProvider.propTypes = {
  loadCurrentUser: PropTypes.func.isRequired
};

const mapDispatchToProps = {
  loadCurrentUser: authActions.loadCurrentUser
};

export default connect(
  null,
  mapDispatchToProps
)(UserProvider);
