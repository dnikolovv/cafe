import React from "react";
import PropTypes from "prop-types";
import SelectableMenuDisplay from "../common/SelectableMenuDisplay";

const ToGoOrderForm = ({
  menuItems,
  selectedItems,
  onSelectedItemsChanged,
  onSubmit
}) => {
  const handleSubmit = event => {
    event.preventDefault();
    onSubmit(selectedItems);
  };

  return (
    <>
      <h3 className="mt-3">Issue To-Go Order</h3>

      <SelectableMenuDisplay
        items={menuItems}
        onSelectedItemsChanged={onSelectedItemsChanged}
      />
      <button onClick={handleSubmit} className="btn btn-success mt-2">
        Issue
      </button>
    </>
  );
};

ToGoOrderForm.propTypes = {
  menuItems: PropTypes.array.isRequired,
  selectedItems: PropTypes.array.isRequired,
  onSelectedItemsChanged: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired
};

export default ToGoOrderForm;
