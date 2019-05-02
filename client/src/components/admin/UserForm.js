import React from "react";
import Input from "../common/Input";
import PropTypes from "prop-types";

const UserForm = ({ user }) => {
  return (
    <form>
      <Input
        value={user.id}
        label="Id"
        name="id"
        onChange={() => {}}
        isReadonly={true}
      />

      <Input
        value={`${user.firstName} ${user.lastName}`}
        label="Name"
        name="name"
        onChange={() => {}}
        isReadonly={true}
      />
    </form>
  );
};

UserForm.propTypes = {
  user: PropTypes.object.isRequired
};

export default UserForm;
