import React from "react";
import { NavLink } from "react-router-dom";
import { connect } from "react-redux";
import PropTypes from "prop-types";

const Header = ({ isLoggedIn }) => {
  const activeStyle = { color: "#000000" };

  return (
    <nav>
      <NavLink to="/" activeStyle={activeStyle} exact>
        Home
      </NavLink>
      {" | "}
      {isLoggedIn ? (
        <>
          <NavLink to="/admin" activeStyle={activeStyle}>
            Admin
          </NavLink>
          {" | "}
          <NavLink to="/logout" activeStyle={activeStyle}>
            Logout
          </NavLink>
        </>
      ) : (
        <NavLink to="/login" activeStyle={activeStyle}>
          Login
        </NavLink>
      )}
    </nav>
  );
};

Header.propTypes = {
  isLoggedIn: PropTypes.bool.isRequired
};

function mapStateToProps(state) {
  return {
    isLoggedIn: state.auth.isLoggedIn
  };
}

export default connect(mapStateToProps)(Header);
