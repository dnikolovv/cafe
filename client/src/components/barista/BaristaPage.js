import React, { useEffect } from "react";
import { connect } from "react-redux";
import * as orderActions from "../../redux/actions/orderActions";
import PropTypes from "prop-types";
import * as orderService from "../../api/orderService";
import WaitingOrdersList from "./WaitingOrdersList";

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
    <>
      <h2>Barista</h2>
      <WaitingOrdersList
        orders={issuedOrders}
        onOrderCompleted={handleOrderCompletion}
      />
    </>
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
