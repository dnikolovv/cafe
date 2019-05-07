import React, { useEffect, useState } from "react";
import ToGoOrderForm from "./ToGoOrderForm";
import PendingOrdersList from "./PendingOrdersList";
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

  const handleMenuItemSelected = items => {
    setSelectedItems(items);
  };

  const handleIssueOrder = selectedItems => {
    issueToGoOrder(
      selectedItems.flatMap(i => Array(i.count).fill(i.item.number))
    );
    setSelectedItems([]);
  };

  const handlePricePaidChange = (orderId, event) => {
    const value = parseFloat(event.target.value);
    setPaidPrices({ ...paidPrices, [orderId]: value });
  };

  const handleOrderConfirmation = orderId => {
    const pricePaid = paidPrices[orderId];
    const order = pendingOrders.find(o => o.id === orderId);

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
      <PendingOrdersList
        pendingOrders={pendingOrders}
        onPricePaidChange={handlePricePaidChange}
        onOrderConfirmation={handleOrderConfirmation}
      />
    </div>
  );
};

CashierPage.propTypes = {
  loadOrders: PropTypes.func.isRequired,
  loadMenuItems: PropTypes.func.isRequired,
  issueToGoOrder: PropTypes.func.isRequired,
  confirmToGoOrder: PropTypes.func.isRequired,
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
