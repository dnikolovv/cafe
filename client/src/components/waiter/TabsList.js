import React from "react";
import PropTypes from "prop-types";

const TabsList = ({ tabs, onTabSelected }) => {
  return (
    <ul className="list-group">
      {tabs.map(tab => {
        return (
          <li className="list-group-item" key={tab.id}>
            Table: {tab.tableNumber}, Waiter: {tab.waiterName}{" "}
            {!tab.isOpen &&
              `(bill: $${tab.servedItemsValue}, tip: $${tab.tipValue})`}
            <button
              className="btn btn-success btn-sm float-right"
              onClick={() => onTabSelected(tab.id)}
            >
              Manage
            </button>
          </li>
        );
      })}
    </ul>
  );
};

TabsList.propTypes = {
  tabs: PropTypes.array.isRequired,
  onTabSelected: PropTypes.func.isRequired
};

export default TabsList;
