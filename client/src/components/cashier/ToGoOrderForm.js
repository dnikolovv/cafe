import React from "react";
import MultiSelect from "@khanacademy/react-multi-select";
import PropTypes from "prop-types";

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
      <form className="form" onSubmit={handleSubmit}>
        <MultiSelect
          options={menuItems.map(item => ({
            label: item.description,
            value: item.number
          }))}
          selected={selectedItems}
          onSelectedChanged={onSelectedItemsChanged}
        />
        <input type="submit" className="btn btn-success mt-2" value="Issue" />
      </form>
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
