import React, { useEffect } from "react";
import { connect } from "react-redux";
import * as userActions from "../../redux/actions/userActions";
import PropTypes from "prop-types";
import UsersList from "./UsersList";

const AdminPage = ({ loadUsers, users }) => {
  useEffect(() => {
    loadUsers();
  }, []);

  return (
    <div>
      <h2>Admin</h2>
      <UsersList users={users} />
    </div>
  );
};

AdminPage.propTypes = {
  loadUsers: PropTypes.func.isRequired,
  users: PropTypes.array.isRequired
};

function mapStateToProps(state) {
  return {
    users: state.users
  };
}

const mapDispatchToProps = {
  loadUsers: userActions.loadUsers
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(AdminPage);
