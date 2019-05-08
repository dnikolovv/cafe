import React, { useEffect, useState } from "react";
import { connect } from "react-redux";
import * as tabActions from "../../redux/actions/tabActions";
import * as menuItemActions from "../../redux/actions/menuItemActions";
import CloseTabForm from "./CloseTabForm";
import SelectableMenuDisplay from "../common/SelectableMenuDisplay";
import FiniteSelctableMenu from "../common/FiniteSelectableMenu";
import { flattenMenuItems } from "../../utils/menuItemUtils";
import OrderedItemsList from "../common/OrderedItemsList";
import PropTypes from "prop-types";
import * as tableActions from "../../redux/actions/tableActions";

const ManageTabPage = ({
  tabId,
  tab,
  loadTab,
  closeTab,
  orderMenuItems,
  menuItems,
  loadMenuItems,
  rejectMenuItems,
  serveMenuItems,
  callWaiter,
  requestBill
}) => {
  useEffect(() => {
    if (!tab) {
      loadTab(tabId);
    }

    loadMenuItems();
  }, []);

  const [selectedItemsToOrder, setSelectedItemsToOrder] = useState([]);
  const [selectedOustandingItems, setSelectedOutstandingItems] = useState([]);

  const handleSelectedItemsToOrderChanged = items => {
    setSelectedItemsToOrder(items);
  };

  const handleSelectedOutstandingItemsChanged = items => {
    setSelectedOutstandingItems(items);
  };

  const handleOrderItems = () => {
    orderMenuItems(tabId, extractItemNumbersFromPairs(selectedItemsToOrder));
    setSelectedItemsToOrder([]);
  };

  const handleRejectItems = () => {
    rejectMenuItems(
      tabId,
      extractItemNumbersFromPairs(selectedOustandingItems)
    );
    setSelectedOutstandingItems([]);
  };

  const handleServeItems = () => {
    serveMenuItems(tabId, extractItemNumbersFromPairs(selectedOustandingItems));
    setSelectedOutstandingItems([]);
  };

  const handleTabClose = amountPaid => {
    closeTab(tabId, amountPaid);
  };

  const extractItemNumbersFromPairs = pairs => {
    return pairs.flatMap(p => Array(p.count).fill(p.item.number));
  };

  return (
    <div>
      {tab && (
        <>
          <h3>Manage tab</h3>

          <div className="row">
            <div className="col-md-12">
              <p>
                Table: {tab.tableNumber}, Customer: {tab.customerName}, Waiter:{" "}
                {tab.waiterName}
              </p>
            </div>
          </div>

          {tab.isOpen && (
            <>
              <hr />
              <h4>Actions:</h4>
              <div className="row">
                <div className="col-md-10">
                  <button
                    onClick={() => callWaiter(tab.tableNumber)}
                    className="btn btn-success float-left"
                  >
                    Call Waiter
                  </button>
                  <button
                    onClick={() => requestBill(tab.tableNumber)}
                    className="btn btn-warning float-left ml-1"
                  >
                    Request Bill
                  </button>
                </div>
              </div>

              <hr />
            </>
          )}

          {tab.isOpen ? (
            <div className="row">
              <div className="col-md-6">
                <h4>Order items</h4>

                <SelectableMenuDisplay
                  items={menuItems}
                  selectedItems={selectedItemsToOrder}
                  onSelectedItemsChanged={handleSelectedItemsToOrderChanged}
                />
                <button
                  onClick={handleOrderItems}
                  className="btn btn-success mt-1"
                  disabled={selectedItemsToOrder.length === 0}
                >
                  Order items
                </button>
              </div>
              <div className="col-md-6">
                <h4>Outstanding items</h4>
                <FiniteSelctableMenu
                  itemPairs={flattenMenuItems(tab.outstandingMenuItems)}
                  selectedPairs={selectedOustandingItems}
                  onSelectedPairsChanged={handleSelectedOutstandingItemsChanged}
                />

                <button
                  onClick={handleServeItems}
                  className="btn btn-success mt-1"
                  disabled={selectedOustandingItems.length === 0}
                >
                  Serve
                </button>

                <button
                  onClick={handleRejectItems}
                  className="btn btn-danger mt-1 ml-1"
                  disabled={selectedOustandingItems.length === 0}
                >
                  Reject
                </button>
              </div>
            </div>
          ) : (
            <div className="row">
              <div className="col-md-12">
                The tab is closed. Total paid - ${tab.totalPaid}. Tip - $
                {tab.tipValue}.
              </div>
            </div>
          )}

          {tab.servedMenuItems.length > 0 ||
            (tab.rejectedMenuItems.length > 0 && (
              <>
                <hr />

                <div className="row">
                  {tab.servedMenuItems.length > 0 && (
                    <div className="col-md-6">
                      <h4>Served Items</h4>
                      <OrderedItemsList
                        itemPairs={flattenMenuItems(tab.servedMenuItems)}
                      />
                    </div>
                  )}
                  {tab.rejectedMenuItems.length > 0 && (
                    <div className="col-md-6">
                      <h4>Rejected Items</h4>
                      <OrderedItemsList
                        itemPairs={flattenMenuItems(tab.rejectedMenuItems)}
                      />
                    </div>
                  )}
                </div>
              </>
            ))}

          <hr />

          {tab.isOpen && (
            <CloseTabForm
              owedAmount={tab.servedItemsValue}
              onSubmit={handleTabClose}
            />
          )}
        </>
      )}
    </div>
  );
};

ManageTabPage.propTypes = {
  tabId: PropTypes.string.isRequired,
  tab: PropTypes.object,
  loadTab: PropTypes.func.isRequired,
  closeTab: PropTypes.func.isRequired,
  orderMenuItems: PropTypes.func.isRequired,
  menuItems: PropTypes.array.isRequired,
  loadMenuItems: PropTypes.func.isRequired,
  rejectMenuItems: PropTypes.func.isRequired,
  serveMenuItems: PropTypes.func.isRequired,
  callWaiter: PropTypes.func.isRequired,
  requestBill: PropTypes.func.isRequired
};

function mapStateToProps(state, ownProps) {
  const tabId = ownProps.match.params.tabId;

  return {
    tabId: tabId,
    tab: state.tabs.find(t => t.id === tabId),
    tabs: state.tabs,
    menuItems: state.menuItems
  };
}

const mapDispatchToProps = {
  closeTab: tabActions.closeTab,
  orderMenuItems: tabActions.orderMenuItems,
  rejectMenuItems: tabActions.rejectMenuItems,
  serveMenuItems: tabActions.serveMenuItems,
  loadMenuItems: menuItemActions.loadMenuItems,
  loadTab: tabActions.loadTab,
  callWaiter: tableActions.callWaiter,
  requestBill: tableActions.requestBill
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ManageTabPage);
