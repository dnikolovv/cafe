import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";

const SelectableMenuDisplay = ({ items, onSelectedItemsChanged, ...props }) => {
  const [selectedItems, setSelectedItems] = useState([]);

  useEffect(() => {
    if (props.selectedItems) {
      setSelectedItems(props.selectedItems);
    }
  }, [props.selectedItems]);

  const addItem = item => {
    const existingItem = selectedItems.find(i => i.item.number === item.number);

    const newSelectedItems = existingItem
      ? [
          ...selectedItems.filter(i => i.item.number !== item.number),
          { ...existingItem, count: existingItem.count + 1 }
        ]
      : [...selectedItems, { count: 1, item }];

    setSelectedItems(newSelectedItems);

    if (onSelectedItemsChanged) {
      onSelectedItemsChanged(newSelectedItems);
    }
  };

  const removeItem = item => {
    const existingItem = selectedItems.find(i => i.item.number === item.number);

    if (existingItem) {
      let newSelectedItems = [];

      if (existingItem.count > 1) {
        newSelectedItems = [
          ...selectedItems.filter(i => i.item.number !== item.number),
          { ...existingItem, count: existingItem.count - 1 }
        ];
      } else {
        newSelectedItems = [
          ...selectedItems.filter(i => i.item.number !== item.number)
        ];
      }

      setSelectedItems(newSelectedItems);

      if (onSelectedItemsChanged) {
        onSelectedItemsChanged(newSelectedItems);
      }
    }
  };

  return (
    <>
      {items.length > 0 ? (
        <ul className="list-group">
          {items.map(item => {
            return (
              <li key={item.number} className="list-group-item">
                {item.description}
                <button
                  onClick={() => removeItem(item)}
                  disabled={
                    !selectedItems.some(si => si.item.number === item.number)
                  }
                  className="btn btn-danger btn-sm float-right"
                >
                  <i className="fa fa-minus" />
                </button>
                <button
                  onClick={() => addItem(item)}
                  className="btn btn-success btn-sm float-right mr-1"
                >
                  <i className="fa fa-plus" />
                </button>
              </li>
            );
          })}
          <li className="list-group-item list-group-item-info">
            {selectedItems.length > 0 ? (
              <>
                {selectedItems
                  .sort((a, b) => a.item.number - b.item.number)
                  .map(item => `${item.count} x ${item.item.description}`)
                  .join(", ")}
              </>
            ) : (
              <>Nothing selected</>
            )}
          </li>
        </ul>
      ) : (
        <ul className="list-group">
          <li className="list-group-item list-group-item-info">
            No items to display.
          </li>
        </ul>
      )}
    </>
  );
};

SelectableMenuDisplay.propTypes = {
  items: PropTypes.array.isRequired,
  onSelectedItemsChanged: PropTypes.func.isRequired
};

export default SelectableMenuDisplay;
