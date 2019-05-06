import React from "react";
import Input from "../common/Input";

const AddTableForm = ({ table, onChange, onSubmit }) => {
  return (
    <>
      <h4>Add Table</h4>
      <form onSubmit={onSubmit}>
        <Input
          isNumber
          name="number"
          label="Table number"
          value={table.number}
          onChange={onChange}
          placeholder="Table number..."
        />
        <input type="submit" className="btn btn-success" value="Add table" />
      </form>
    </>
  );
};

export default AddTableForm;
