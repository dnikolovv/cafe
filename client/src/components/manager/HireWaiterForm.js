import React from "react";
import Input from "../common/Input";

const HireWaiterForm = ({ waiter, onChange, onSubmit }) => {
  return (
    <>
      <h4>Hire waiter</h4>
      <form className="form-group" onSubmit={onSubmit}>
        <Input
          name="shortName"
          onChange={onChange}
          label="Short Name"
          placeholder="Short name..."
          value={waiter.shortName}
        />
        <input type="submit" className="btn btn-success" value="Hire Waiter" />
      </form>
    </>
  );
};

export default HireWaiterForm;
