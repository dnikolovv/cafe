import React from "react";

const HireWaiterForm = ({ waiter, onChange, onSubmit }) => {
  return (
    <form className="form-group" onSubmit={onSubmit}>
      <input
        type="text"
        className="form-control"
        value={waiter.shortName}
        name="shortName"
        onChange={onChange}
      />
      <input type="submit" className="btn btn-success" value="Hire Waiter" />
    </form>
  );
};

export default HireWaiterForm;
