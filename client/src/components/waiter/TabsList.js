import React from "react";

const TabsList = ({ tabs, onTabSelected }) => {
  return (
    <ul className="list-group">
      {tabs.map(tab => {
        return (
          <li className="list-group-item row" key={tab.id}>
            Tab on table {tab.tableNumber}
            <button
              className="btn btn-success float-right"
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

export default TabsList;
