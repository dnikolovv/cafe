import React from "react";
import PropTypes from "prop-types";
import { flattenMenuItems } from "../../utils/menuItemUtils";

const PendingOrdersList = ({
  pendingOrders,
  onPricePaidChange,
  onOrderConfirmation
}) => {
  return (
    <>
      <h3 className="mt-4">Pending Orders</h3>
      <table className="table">
        <thead>
          <tr>
            <th>Id</th>
            <th>Ordered Items</th>
            <th>Price</th>
            <th>Price paid</th>
            <th>Confirm</th>
          </tr>
        </thead>
        <tbody>
          {[...pendingOrders]
            .sort(function(a, b) {
              return new Date(b.date) - new Date(a.date);
            })
            .map(order => (
              <tr key={order.id}>
                <td>{order.id}</td>
                <td>
                  {flattenMenuItems(order.orderedItems)
                    .map(x => `${x.count} x ${x.item.description}`)
                    .join(", ")}
                </td>
                <td>{order.price.toFixed(2)}</td>
                <td>
                  <input
                    type="number"
                    className="form-control"
                    placeholder="Paid..."
                    onChange={event => onPricePaidChange(order.id, event)}
                  />
                </td>
                <td>
                  <button
                    onClick={() => onOrderConfirmation(order.id)}
                    className="btn btn-success"
                  >
                    Confirm
                  </button>
                </td>
              </tr>
            ))}
        </tbody>
      </table>
    </>
  );
};

PendingOrdersList.propTypes = {
  pendingOrders: PropTypes.array.isRequired,
  onPricePaidChange: PropTypes.func.isRequired,
  onOrderConfirmation: PropTypes.func.isRequired
};

export default PendingOrdersList;
