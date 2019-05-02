import React, { useEffect } from "react";
import { connect } from "react-redux";
import * as tabActions from "../../redux/actions/tabActions";

const ManageTabPage = ({ tabId, tab, loadTab }) => {
  useEffect(() => {
    if (!tab.id) {
      loadTab(tabId);
    }
  }, []);

  return (
    <div>
      <h3>Manage tab {tab.id}</h3>
    </div>
  );
};

function mapStateToProps(state, ownProps) {
  const tabId = ownProps.match.params.tabId;

  return {
    tabId: tabId,
    tab: state.tabs.find(t => t.id === tabId) || {},
    tabs: state.tabs
  };
}

const mapDispatchToProps = {
  closeTab: tabActions.closeTab,
  orderMenuItems: tabActions.orderMenuItems,
  rejectMenuItems: tabActions.rejectMenuItems,
  loadTab: tabActions.loadTab
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ManageTabPage);
