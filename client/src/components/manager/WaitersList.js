import React from "react";

const WaitersList = ({ waiters }) => {
  return (
    <ul className="list-group">
      {waiters.map(waiter => {
        return (
          <li key={waiter.id} className="list-group-item">
            {waiter.shortName}
          </li>
        );
      })}
    </ul>
  );
};

export default WaitersList;
