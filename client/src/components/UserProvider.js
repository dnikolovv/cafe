import React, { useEffect } from "react";
import * as authActions from "../redux/actions/authActions";
import { connect } from "react-redux";
import { getCookie } from "../utils/cookieUtils";
import PropTypes from "prop-types";

const UserProvider = ({ loadCurrentUser }) => {
  useEffect(() => {
    if (authCookieIsSet()) {
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

function authCookieIsSet() {
  return getCookie("access_token") !== null;
}

export default connect(
  null,
  mapDispatchToProps
)(UserProvider);
