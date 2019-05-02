import React from "react";

const TablesList = ({ tables }) => {
  return (
    <ul className="list-group">
      {tables.map(table => {
        return (
          <li key={table.number} className="list-group-item">
            Table {table.number}
          </li>
        );
      })}
    </ul>
  );
};

export default TablesList;
