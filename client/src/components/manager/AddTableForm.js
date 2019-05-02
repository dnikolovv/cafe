import React from "react";

const AddTableForm = ({ table, onChange, onSubmit }) => {
  return (
    <form onSubmit={onSubmit}>
      <input
        type="number"
        name="number"
        value={table.number}
        onChange={onChange}
        className="form-control"
        placeholder="Table number..."
      />
      <input type="submit" className="btn btn-success" value="Add table" />
    </form>
  );
};

export default AddTableForm;
