import React, { useEffect, useState } from "react";
import ToGoOrderForm from "./ToGoOrderForm";
import PendingOrdersList from "./PendingOrdersList";
import { connect } from "react-redux";
import * as orderActions from "../../redux/actions/orderActions";
import * as menuItemActions from "../../redux/actions/menuItemActions";

const CashierPage = ({
  loadOrders,
  loadMenuItems,
  issueToGoOrder,
  menuItems,
  pendingOrders,
  ...props
}) => {
  useEffect(() => {
    loadOrders();
    loadMenuItems();
  }, []);

  const [selectedItems, setSelectedItems] = useState([]);

  const handleMenuItemSelected = itemNumbers => {
    setSelectedItems(itemNumbers);
  };

  const handleIssueOrder = selectedItems => {
    issueToGoOrder(selectedItems);
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
      <PendingOrdersList orders={pendingOrders} />
    </div>
  );
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
  issueToGoOrder: orderActions.issueToGoOrder
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(CashierPage);
