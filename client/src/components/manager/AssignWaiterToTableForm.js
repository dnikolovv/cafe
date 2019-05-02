import React, { useState } from "react";
import SelectInput from "../common/SelectInput";

const AssignWaiterToTableForm = ({ waiters, tables, onSubmit }) => {
  const [assignment, setAssignment] = useState({
    waiterId: "",
    tableNumber: ""
  });

  const handleWaiterChange = event => {
    setAssignment({ ...assignment, waiterId: event.target.value });
  };

  const handleTableChange = event => {
    setAssignment({ ...assignment, tableNumber: event.target.value });
  };

  const handleSubmit = event => {
    event.preventDefault();
    onSubmit(assignment);
  };

  return (
    <form className="form-group" onSubmit={handleSubmit}>
      <SelectInput
        options={waiters.map(waiter => {
          return {
            value: waiter.id,
            text: waiter.shortName
          };
        })}
        label="Waiters"
        value={assignment.waiterId}
        defaultOption="Select waiter"
        onChange={handleWaiterChange}
      />
      <SelectInput
        options={tables.map(table => {
          return {
            value: table.number,
            text: table.number
          };
        })}
        label="Tables"
        value={assignment.tableNumber}
        defaultOption="Select table"
        onChange={handleTableChange}
      />
      <input type="submit" className="btn btn-success" value="Assign table" />
    </form>
  );
};

export default AssignWaiterToTableForm;
