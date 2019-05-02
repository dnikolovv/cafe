import React from "react";

const AddMenuItemForm = ({ menuItem, onChange, onSubmit }) => {
  return (
    <form className="form-group" onSubmit={e => onSubmit(e)}>
      <input
        type="text"
        name="description"
        value={menuItem.description}
        placeholder="Item description..."
        className="form-control"
        onChange={onChange}
      />
      <input
        type="number"
        name="price"
        value={menuItem.price}
        placeholder="Price"
        className="form-control"
        onChange={onChange}
      />
      <input
        type="number"
        name="number"
        value={menuItem.number}
        placeholder="Number"
        className="form-control"
        onChange={onChange}
      />
      <input type="submit" className="btn btn-success" value="Add Item" />
    </form>
  );
};

export default AddMenuItemForm;
