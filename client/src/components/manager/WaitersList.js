import React from "react";

const WaitersList = ({ waiters }) => {
  return (
    <ul className="list-group">
      {waiters.map(waiter => {
        return (
          <li key={waiter.id} className="list-group-item">
            {waiter.shortName}{" "}
            {waiter.tablesServed && waiter.tablesServed.length > 0 && (
              <>(table numbers: {waiter.tablesServed.join(", ")})</>
            )}
          </li>
        );
      })}
    </ul>
  );
};

export default WaitersList;
