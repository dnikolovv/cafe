import React, { useEffect, useState } from "react";
import * as userActions from "../../redux/actions/userActions";
import * as waiterActions from "../../redux/actions/waiterActions";
import * as baristaActions from "../../redux/actions/baristaActions";
import * as cashierActions from "../../redux/actions/cashierActions";
import * as managerActions from "../../redux/actions/managerActions";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import UserForm from "./UserForm";
import AssignRoleToUserSelect from "./AssignRoleToUserSelect";
import * as usersApi from "../../api/userService";
import { toast } from "react-toastify";

const ManageUserPage = ({
  loadWaiters,
  loadBaristas,
  loadManagers,
  loadCashiers,
  loadUsers,
  waiters,
  baristas,
  cashiers,
  managers,
  users,
  history,
  ...props
}) => {
  const [user, setUser] = useState({ ...props.user });

  useEffect(() => {
    if (waiters.length === 0) {
      loadWaiters();
    }

    if (baristas.length === 0) {
      loadBaristas();
    }

    if (cashiers.length === 0) {
      loadCashiers();
    }

    if (managers.length === 0) {
      loadManagers();
    }

    if (users.length === 0) {
      loadUsers();
    } else {
      setUser({ ...props.user });
    }
  }, [props.user]);

  const handleChange = event => {
    const { name, value } = event.target;

    setUser(prevUser => {
      return {
        ...prevUser,
        [name]: value
      };
    });
  };

  const handleRoleAssignment = (roleId, roleName) => {
    usersApi
      .assignRole(user.id, roleId, roleName)
      .then(_ => {
        toast.success(`Successfully assigned ${roleName} to ${user.email}.`);
        history.push("/admin");
      })
      .catch(error => {
        toast.error(`Failed to assign ${roleName} to ${user.email}.`);
      });
  };

  return (
    <>
      <h2>Manage user</h2>

      {user ? (
        <>
          <UserForm user={user} />
          <AssignRoleToUserSelect
            value={user.waiterId || ""}
            name="waiterId"
            label="Waiter"
            options={waiters.map(waiter => ({
              value: waiter.id,
              text: waiter.shortName
            }))}
            onChange={handleChange}
            onAssign={handleRoleAssignment}
          />
          <AssignRoleToUserSelect
            value={user.baristaId || ""}
            name="baristaId"
            label="Barista"
            options={baristas.map(barista => ({
              value: barista.id,
              text: barista.shortName
            }))}
            onChange={handleChange}
            onAssign={handleRoleAssignment}
          />
          <AssignRoleToUserSelect
            value={user.cashierId || ""}
            name="cashierId"
            label="Cashier"
            options={cashiers.map(cashier => ({
              value: cashier.id,
              text: cashier.shortName
            }))}
            onChange={handleChange}
            onAssign={handleRoleAssignment}
          />
          <AssignRoleToUserSelect
            value={user.managerId || ""}
            name="managerId"
            label="Manager"
            options={managers.map(manager => ({
              value: manager.id,
              text: manager.shortName
            }))}
            onChange={handleChange}
            onAssign={handleRoleAssignment}
          />
        </>
      ) : (
        <div>Loading...</div>
      )}
    </>
  );
};

ManageUserPage.propTypes = {
  user: PropTypes.object,
  history: PropTypes.object.isRequired,
  users: PropTypes.array.isRequired,
  waiters: PropTypes.array.isRequired,
  baristas: PropTypes.array.isRequired,
  cashiers: PropTypes.array.isRequired,
  managers: PropTypes.array.isRequired,
  loadWaiters: PropTypes.func.isRequired,
  loadManagers: PropTypes.func.isRequired,
  loadCashiers: PropTypes.func.isRequired,
  loadBaristas: PropTypes.func.isRequired,
  loadUsers: PropTypes.func.isRequired
};

function mapStateToProps(state, ownProps) {
  const userId = ownProps.match.params.userId;

  const user =
    userId && state.users.length > 0
      ? state.users.find(u => u.id === userId)
      : { id: "", firstName: "", lastName: "" };

  return {
    user,
    users: state.users,
    waiters: state.waiters,
    baristas: state.baristas,
    cashiers: state.cashiers,
    managers: state.managers
  };
}

const mapDispatchToProps = {
  loadUsers: userActions.loadUsers,
  loadWaiters: waiterActions.loadWaiters,
  loadBaristas: baristaActions.loadBaristas,
  loadManagers: managerActions.loadManagers,
  loadCashiers: cashierActions.loadCashiers
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ManageUserPage);
