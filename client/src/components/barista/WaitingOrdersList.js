import React from "react";

const WaitingOrdersList = ({ orders, onOrderCompleted }) => {
  return (
    <>
      <h3>Waiting Orders</h3>
      <table className="table">
        <thead>
          <tr>
            <th>Id</th>
            <th>Ordered Items</th>
            <th>Complete</th>
          </tr>
        </thead>
        <tbody>
          {[...orders]
            .sort(function(a, b) {
              return new Date(b.date) - new Date(a.date);
            })
            .map(order => (
              <tr key={order.id}>
                <td>{order.id}</td>
                <td>{order.orderedItems.map(i => i.description).join(", ")}</td>
                <td>
                  <button
                    onClick={() => onOrderCompleted(order.id)}
                    className="btn btn-success"
                  >
                    Complete
                  </button>
                </td>
              </tr>
            ))}
        </tbody>
      </table>
    </>
  );
};

export default WaitingOrdersList;
