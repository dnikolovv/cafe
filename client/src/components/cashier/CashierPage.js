import React, { useEffect, useState } from "react";
import ToGoOrderForm from "./ToGoOrderForm";
import OrdersList from "../common/OrdersList";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import * as orderActions from "../../redux/actions/orderActions";
import * as menuItemActions from "../../redux/actions/menuItemActions";

const CashierPage = ({
  loadOrders,
  loadMenuItems,
  issueToGoOrder,
  menuItems,
  pendingOrders
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
      <h3 className="mt-4">Pending Orders</h3>
      <OrdersList orders={pendingOrders} />
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
  issueToGoOrder: orderActions.issueToGoOrder
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(CashierPage);
