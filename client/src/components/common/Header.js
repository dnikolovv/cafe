import React from "react";
import { NavLink } from "react-router-dom";
import { connect } from "react-redux";
import PropTypes from "prop-types";

const Header = ({ isLoggedIn, currentUser }) => {
  const activeStyle = { color: "#000000" };

  return (
    <>
      <div className="row">
        <nav className="col-md-12 pb-0">
          <NavLink to="/" activeStyle={activeStyle} exact>
            Home
          </NavLink>
          {" | "}
          {isLoggedIn ? (
            <>
              {currentUser.isAdmin && (
                <>
                  <NavLink to="/admin" activeStyle={activeStyle}>
                    Admin
                  </NavLink>
                  {" | "}
                </>
              )}
              {(currentUser.isAdmin || currentUser.isCashier) && (
                <>
                  <NavLink to="/cashier" activeStyle={activeStyle}>
                    Cashier
                  </NavLink>
                  {" | "}
                </>
              )}
              {(currentUser.isAdmin || currentUser.isBarista) && (
                <>
                  <NavLink to="/barista" activeStyle={activeStyle}>
                    Barista
                  </NavLink>
                  {" | "}
                </>
              )}
              {(currentUser.isAdmin || currentUser.isManager) && (
                <>
                  <NavLink to="/manager" activeStyle={activeStyle}>
                    Manager
                  </NavLink>
                  {" | "}
                </>
              )}
              {(currentUser.isAdmin || currentUser.isWaiter) && (
                <>
                  <NavLink to="/waiter" activeStyle={activeStyle}>
                    Waiter
                  </NavLink>
                  {" | "}
                </>
              )}
              <NavLink to="/logout" activeStyle={activeStyle}>
                Logout
              </NavLink>
            </>
          ) : (
            <NavLink to="/login" activeStyle={activeStyle}>
              Login
            </NavLink>
          )}

          {currentUser && currentUser.firstName && (
            <div className="pull-right">
              Hi, {currentUser.firstName + " " + currentUser.lastName + "!"}
            </div>
          )}
        </nav>
        <hr className="col-md-12" />
      </div>
    </>
  );
};

Header.propTypes = {
  isLoggedIn: PropTypes.bool.isRequired,
  currentUser: PropTypes.object.isRequired
};

function mapStateToProps(state) {
  return {
    isLoggedIn: state.auth.isLoggedIn,
    currentUser: state.auth.user
  };
}

export default connect(mapStateToProps)(Header);
