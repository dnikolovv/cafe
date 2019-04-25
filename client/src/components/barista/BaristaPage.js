import React, { useEffect } from "react";
import { connect } from "react-redux";
import * as orderActions from "../../redux/actions/orderActions";
import PropTypes from "prop-types";
import * as orderService from "../../api/orderService";

const BaristaPage = ({ completeOrder, loadOrders, issuedOrders }) => {
  useEffect(() => {
    loadOrders();
    orderService.onOrderConfirmed(() => {
      loadOrders();
    });
  }, []);

  const handleOrderCompletion = orderId => {
    completeOrder(orderId);
  };

  return (
    <div>
      <h2>Barista</h2>

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
            {[...issuedOrders]
              .sort(function(a, b) {
                return new Date(b.date) - new Date(a.date);
              })
              .map(order => (
                <tr key={order.id}>
                  <td>{order.id}</td>
                  <td>
                    {order.orderedItems.map(i => i.description).join(", ")}
                  </td>
                  <td>
                    <button
                      onClick={() => handleOrderCompletion(order.id)}
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
    </div>
  );
};

function mapStateToProps(state) {
  return {
    issuedOrders: state.orders.issued
  };
}

const mapDispatchToProps = {
  loadOrders: orderActions.loadOrders,
  completeOrder: orderActions.completeToGoOrder
};

BaristaPage.propTypes = {
  loadOrders: PropTypes.func.isRequired,
  issuedOrders: PropTypes.array.isRequired,
  completeOrder: PropTypes.func.isRequired
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(BaristaPage);
