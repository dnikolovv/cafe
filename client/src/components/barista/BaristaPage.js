import React, { useEffect } from "react";
import OrdersList from "../common/OrdersList";
import { connect } from "react-redux";
import * as orderActions from "../../redux/actions/orderActions";

const BaristaPage = ({ loadOrders, issuedOrders }) => {
  useEffect(() => {
    loadOrders();
  }, []);

  return (
    <div>
      <h2>Barista</h2>

      <h3>Waiting Orders</h3>
      <OrdersList orders={issuedOrders} />
    </div>
  );
};

function mapStateToProps(state) {
  return {
    issuedOrders: state.orders.issued
  };
}

const mapDispatchToProps = {
  loadOrders: orderActions.loadOrders
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(BaristaPage);
