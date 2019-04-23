import React from "react";

const PendingOrdersList = ({ orders }) => {
  return (
    <>
      <h3 className="mt-4">Pending Orders</h3>

      <table className="table">
        <thead>
          <tr>
            <th>Id</th>
            <th>Ordered Items</th>
            <th>Price</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          {orders.map(order => (
            <tr key={order.id}>
              <td>{order.id}</td>
              <td>{order.orderedItems.map(i => i.description).join(", ")}</td>
              <td>
                {order.orderedItems
                  .map(i => i.price)
                  .reduce((x, y) => x + y, 0)
                  .toFixed(2)}
              </td>
              <td>{order.statusText}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default PendingOrdersList;
