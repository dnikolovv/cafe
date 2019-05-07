import React from "react";
import PropTypes from "prop-types";

const OrderedItemsList = ({ itemPairs }) => {
  return (
    <ul className="list-group">
      {itemPairs.map(pair => {
        return (
          <li key={pair.item.number} className="list-group-item">
            {pair.count} x {pair.item.description} ($
            {pair.item.price * pair.count})
          </li>
        );
      })}
    </ul>
  );
};

OrderedItemsList.propTypes = {
  itemPairs: PropTypes.array.isRequired
};

export default OrderedItemsList;
