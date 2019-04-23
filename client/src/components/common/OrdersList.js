import React from "react";
import PropTypes from "prop-types";

const OrdersList = ({ orders }) => {
  return (
    <>
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

OrdersList.propTypes = {
  orders: PropTypes.array.isRequired
};

export default OrdersList;
