import React from "react";
import { Link } from "react-router-dom";
import PropTypes from "prop-types";

const UsersList = ({ users }) => {
  return (
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
                <Link to={"/manageUser/" + user.id} className="btn btn-success">
                  Manage
                </Link>
              </td>
            </tr>
          );
        })}
      </tbody>
    </table>
  );
};

UsersList.propTypes = {
  users: PropTypes.array.isRequired
};

export default UsersList;
