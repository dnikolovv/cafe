import React from "react";
import TextInput from "../common/TextInput";
import PropTypes from "prop-types";

const UserForm = ({ user }) => {
  return (
    <form>
      <TextInput
        value={user.id}
        label="Id"
        name="id"
        onChange={() => {}}
        isReadonly={true}
      />

      <TextInput
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
