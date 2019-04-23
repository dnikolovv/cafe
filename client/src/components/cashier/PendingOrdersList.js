import React from "react";

const PendingOrdersList = ({ orders }) => {
  return (
    <>
      <h3 className="mt-4">Pending Orders</h3>
      <ul>
        {orders.map(order => (
          <li key={order.id}>
            {order.id} - {order.orderedItems.length} items ordered
          </li>
        ))}
      </ul>
    </>
  );
};

export default PendingOrdersList;
