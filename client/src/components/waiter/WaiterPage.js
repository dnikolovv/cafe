import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import * as tabActions from "../../redux/actions/tabActions";
import * as tableActions from "../../redux/actions/tableActions";
import TabsList from "./TabsList";
import OpenTabForm from "./OpenTabForm";
import PropTypes from "prop-types";

const WaiterPage = ({
  tables,
  openTabs,
  closedTabs,
  loadAllOpenTabs,
  loadPastTabHistory,
  loadTables,
  openTab,
  history
}) => {
  useEffect(() => {
    loadTables();
    loadAllOpenTabs();
    loadPastTabHistory();
  }, []);

  const [newTab, setNewTab] = useState({
    customerName: "",
    tableNumber: ""
  });

  const handleTabSelected = tabId => {
    history.push("/tab/" + tabId);
  };

  const handleNewTabChange = event => {
    setNewTab({ ...newTab, [event.target.name]: event.target.value });
  };

  const handleOpenTab = tab => {
    openTab(tab);
  };

  return (
    <>
      <div className="row">
        <div className="col-md-6">
          <OpenTabForm
            tab={newTab}
            tables={tables}
            onChange={handleNewTabChange}
            onSubmit={handleOpenTab}
          />
        </div>
        {openTabs.length > 0 && (
          <div className="col-md-6">
            <h4>Open Tabs</h4>
            <TabsList tabs={openTabs} onTabSelected={handleTabSelected} />
          </div>
        )}
      </div>
      {closedTabs.length > 0 && (
        <div>
          <h4>Tabs History</h4>
          <TabsList tabs={closedTabs} onTabSelected={handleTabSelected} />
        </div>
      )}
    </>
  );
};

WaiterPage.propTypes = {
  tables: PropTypes.array.isRequired,
  openTabs: PropTypes.array.isRequired,
  closedTabs: PropTypes.array.isRequired,
  menuItems: PropTypes.array.isRequired,
  loadAllOpenTabs: PropTypes.func.isRequired,
  loadPastTabHistory: PropTypes.func.isRequired,
  loadTables: PropTypes.func.isRequired,
  openTab: PropTypes.func.isRequired,
  history: PropTypes.object.isRequired
};

function mapStateToProps(state) {
  return {
    tables: state.tables,
    openTabs: state.tabs.open,
    closedTabs: state.tabs.closed,
    menuItems: state.menuItems
  };
}

const mapDispatchToProps = {
  loadAllOpenTabs: tabActions.loadAllOpenTabs,
  loadPastTabHistory: tabActions.loadPastTabHistory,
  loadTables: tableActions.loadTables,
  openTab: tabActions.openTab
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(WaiterPage);
