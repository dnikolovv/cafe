import React from "react";
import Input from "../common/Input";

const AddMenuItemForm = ({ menuItem, onChange, onSubmit }) => {
  return (
    <>
      <h4>Add menu item</h4>
      <form className="form-group" onSubmit={e => onSubmit(e)}>
        <Input
          name="description"
          value={menuItem.description}
          placeholder="Item description..."
          className="form-control"
          onChange={onChange}
          label="Description"
        />
        <Input
          isNumber
          name="price"
          value={menuItem.price}
          placeholder="Price"
          className="form-control"
          onChange={onChange}
          label="Price"
        />
        <Input
          isNumber
          name="number"
          value={menuItem.number}
          placeholder="Number"
          className="form-control"
          onChange={onChange}
          label="Number"
        />
        <input type="submit" className="btn btn-success" value="Add Item" />
      </form>
    </>
  );
};

export default AddMenuItemForm;
