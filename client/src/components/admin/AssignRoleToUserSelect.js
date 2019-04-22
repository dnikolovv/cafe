import React, { useState } from "react";
import SelectInput from "../common/SelectInput";
import PropTypes from "prop-types";

const AssignRoleToUserSelect = ({
  value,
  options,
  name,
  label,
  onChange,
  onAssign
}) => {
  const [roleId, setRoleId] = useState(value);

  const handleChange = event => {
    const value = event.target.value;
    setRoleId(value);
    onChange(event);
  };

  const handleAssignment = () => {
    onAssign(roleId, label);
  };

  return (
    <div>
      <SelectInput
        name={name}
        label={label}
        value={value}
        defaultOption={`Select ${label}`}
        options={options}
        onChange={handleChange}
      />

      <button className="btn btn-success" onClick={handleAssignment}>
        Assign {label}
      </button>
    </div>
  );
};

AssignRoleToUserSelect.propTypes = {
  value: PropTypes.string.isRequired,
  options: PropTypes.array.isRequired,
  name: PropTypes.string.isRequired,
  label: PropTypes.string.isRequired,
  onChange: PropTypes.func.isRequired,
  onAssign: PropTypes.func.isRequired
};

export default AssignRoleToUserSelect;
