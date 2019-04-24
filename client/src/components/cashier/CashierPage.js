import React, { useEffect, useState } from "react";
import ToGoOrderForm from "./ToGoOrderForm";
import OrdersList from "../common/OrdersList";
import { toast } from "react-toastify";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import * as orderActions from "../../redux/actions/orderActions";
import * as menuItemActions from "../../redux/actions/menuItemActions";

const CashierPage = ({
  loadOrders,
  loadMenuItems,
  issueToGoOrder,
  confirmToGoOrder,
  menuItems,
  pendingOrders
}) => {
  useEffect(() => {
    loadOrders();
    loadMenuItems();
  }, []);

  const [selectedItems, setSelectedItems] = useState([]);
  const [paidPrices, setPaidPrices] = useState({});

  const handleMenuItemSelected = itemNumbers => {
    setSelectedItems(itemNumbers);
  };

  const handleIssueOrder = selectedItems => {
    issueToGoOrder(selectedItems);
  };

  const handlePricePaidChange = (orderId, event) => {
    const value = parseFloat(event.target.value);
    setPaidPrices({ ...paidPrices, [orderId]: value });
  };

  const handleOrderConfirmation = orderId => {
    const pricePaid = paidPrices[orderId];
    const order = pendingOrders.find(o => o.id == orderId);

    if (pricePaid && order && pricePaid >= order.price) {
      confirmToGoOrder(pricePaid, orderId);
      // Optimism at its finest :)
      toast.success(`Successfully confirmed order ${order.id}!`);
    } else {
      toast.error("Tried to pay less than owed.");
    }
  };

  return (
    <div>
      <h2>Cashier</h2>
      <ToGoOrderForm
        menuItems={menuItems}
        selectedItems={selectedItems}
        onSelectedItemsChanged={handleMenuItemSelected}
        onSubmit={handleIssueOrder}
      />
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
          {pendingOrders
            .sort(function(a, b) {
              return new Date(b.date) - new Date(a.date);
            })
            .map(order => (
              <tr key={order.id}>
                <td>{order.id}</td>
                <td>{order.orderedItems.map(i => i.description).join(", ")}</td>
                <td>
                  {order.orderedItems
                    .map(i => i.price)
                    .reduce((x, y) => x + y, 0)
                    .toFixed(2)}
                </td>
                <td>
                  <input
                    type="number"
                    className="form-control"
                    placeholder="Paid..."
                    onChange={event => handlePricePaidChange(order.id, event)}
                  />
                </td>
                <td>
                  <button
                    onClick={() => handleOrderConfirmation(order.id)}
                    className="btn btn-success"
                  >
                    Confirm
                  </button>
                </td>
              </tr>
            ))}
        </tbody>
      </table>
    </div>
  );
};

CashierPage.propTypes = {
  loadOrders: PropTypes.func.isRequired,
  loadMenuItems: PropTypes.func.isRequired,
  issueToGoOrder: PropTypes.func.isRequired,
  menuItems: PropTypes.array.isRequired,
  pendingOrders: PropTypes.array.isRequired
};

function mapStateToProps(state) {
  return {
    menuItems: state.menuItems,
    pendingOrders: state.orders.pending
  };
}

const mapDispatchToProps = {
  loadOrders: orderActions.loadOrders,
  loadMenuItems: menuItemActions.loadMenuItems,
  issueToGoOrder: orderActions.issueToGoOrder,
  confirmToGoOrder: orderActions.confirmToGoOrder
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(CashierPage);
