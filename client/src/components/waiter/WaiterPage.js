import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import * as tabActions from "../../redux/actions/tabActions";
import * as tableActions from "../../redux/actions/tableActions";
import TabsList from "./TabsList";
import OpenTabForm from "./OpenTabForm";

const WaiterPage = ({
  tables,
  tabs,
  loadAllTabs,
  loadTables,
  openTab,
  history
}) => {
  useEffect(() => {
    loadTables();
    loadAllTabs();
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
    debugger;
    openTab(tab);
  };

  return (
    <div>
      <h3>Waiter</h3>

      <div className="row">
        <div className="col-md-6">
          <OpenTabForm
            tab={newTab}
            tables={tables}
            onChange={handleNewTabChange}
            onSubmit={handleOpenTab}
          />
        </div>
        {tabs.length > 0 && (
          <div className="col-md-6">
            <h4>Open Tabs</h4>
            <TabsList tabs={tabs} onTabSelected={handleTabSelected} />
          </div>
        )}
      </div>
    </div>
  );
};

function mapStateToProps(state) {
  return {
    tables: state.tables,
    tabs: state.tabs,
    menuItems: state.menuItems
  };
}

const mapDispatchToProps = {
  loadAllTabs: tabActions.loadAllTabs,
  loadTables: tableActions.loadTables,
  openTab: tabActions.openTab
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(WaiterPage);
