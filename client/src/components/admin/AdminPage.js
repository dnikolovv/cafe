import React, { useEffect } from "react";
import { connect } from "react-redux";
import * as userActions from "../../redux/actions/userActions";
import PropTypes from "prop-types";
import { Link } from "react-router-dom";

const AdminPage = ({ loadUsers, users }) => {
  useEffect(() => {
    if (!users || users.length === 0) {
      loadUsers();
    }
  }, []);

  return (
    <div>
      <h2>Admin</h2>

      <table className="table">
        <thead>
          <tr>
            <th>Id</th>
            <th>Email</th>
            <th>Manage</th>
          </tr>
        </thead>
        <tbody>
          {users.map(user => {
            return (
              <tr key={user.id}>
                <td>{user.id}</td>
                <td>{user.email}</td>
                <td>
                  <Link
                    to={"/manageUser/" + user.id}
                    className="btn btn-success"
                  >
                    Manage
                  </Link>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
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
