import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import * as menuItemActions from "../../redux/actions/menuItemActions";
import * as tableActions from "../../redux/actions/tableActions";
import * as waiterActions from "../../redux/actions/waiterActions";
import MenuItemsList from "./MenuItemsList";
import AddMenuItemForm from "./AddMenuItemForm";
import TablesList from "./TablesList";
import AddTableForm from "./AddTableForm";
import WaitersList from "./WaitersList";
import HireWaiterForm from "./HireWaiterForm";

const ManagerPage = ({
  loadMenuItems,
  addMenuItem,
  loadTables,
  addTable,
  tables,
  loadWaiters,
  hireWaiter,
  waiters,
  menuItems
}) => {
  useEffect(() => {
    loadMenuItems();
    loadTables();
    loadWaiters();
  }, []);

  const [newMenuItem, setNewMenuItem] = useState({
    description: "",
    price: "",
    number: ""
  });

  const [newTable, setNewTable] = useState({ number: "" });

  const [newWaiter, setNewWaiter] = useState({});

  const handleNewMenuItemChange = event => {
    setNewMenuItem({ ...newMenuItem, [event.target.name]: event.target.value });
  };

  const handleAddMenuItem = event => {
    event.preventDefault();
    addMenuItem(newMenuItem);
    setNewMenuItem({ ...newMenuItem, description: "", price: "", number: "" });
  };

  const handleNewTableChange = event => {
    setNewTable({ ...newTable, [event.target.name]: event.target.value });
  };

  const handleAddTable = event => {
    event.preventDefault();
    addTable(newTable);
    setNewTable({ ...newTable, number: "" });
  };

  const handleNewWaiterChange = event => {
    setNewWaiter({ ...newWaiter, [event.target.name]: event.target.value });
  };

  const handleHireWaiter = event => {
    event.preventDefault();
    hireWaiter(newWaiter);
    setNewWaiter({});
  };

  return (
    <>
      <h2>Manager</h2>
      <h3>Menu</h3>
      <MenuItemsList menuItems={menuItems} />
      <AddMenuItemForm
        menuItem={newMenuItem}
        onChange={handleNewMenuItemChange}
        onSubmit={handleAddMenuItem}
      />

      <h3>Tables</h3>
      <TablesList tables={tables} />
      <AddTableForm
        table={newTable}
        onChange={handleNewTableChange}
        onSubmit={handleAddTable}
      />

      <h3>Waiters</h3>
      <WaitersList waiters={waiters} />
      <HireWaiterForm
        waiter={newWaiter}
        onChange={handleNewWaiterChange}
        onSubmit={handleHireWaiter}
      />

      <p>
        1.sees the menu contents 2.can add menu items 3.sees the tables in the
        cafe 4.can add tables 5.Sees the employed waiters 6.Can hire new waiters
        7.Can assign waiters to tables
      </p>
    </>
  );
};

ManagerPage.propTypes = {
  loadMenuItems: PropTypes.func.isRequired,
  addMenuItem: PropTypes.func.isRequired,
  menuItems: PropTypes.array.isRequired
};

function mapStateToProps(state) {
  return {
    menuItems: state.menuItems,
    tables: state.tables,
    waiters: state.waiters
  };
}

const mapDispatchToProps = {
  loadMenuItems: menuItemActions.loadMenuItems,
  addMenuItem: menuItemActions.addMenuItem,
  loadTables: tableActions.loadTables,
  addTable: tableActions.addTable,
  loadWaiters: waiterActions.loadWaiters,
  hireWaiter: waiterActions.hireWaiter
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ManagerPage);
