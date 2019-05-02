import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import * as menuItemActions from "../../redux/actions/menuItemActions";
import MenuItemsList from "./MenuItemsList";
import AddMenuItemForm from "./AddMenuItemForm";

const ManagerPage = ({ loadMenuItems, addMenuItem, menuItems }) => {
  useEffect(() => {
    loadMenuItems();
  }, []);

  const [newMenuItem, setNewMenuItem] = useState({
    description: "",
    price: "",
    number: ""
  });

  const handleNewMenuItemChange = event => {
    setNewMenuItem({ ...newMenuItem, [event.target.name]: event.target.value });
  };

  const handleAddMenuItem = event => {
    event.preventDefault();
    addMenuItem(newMenuItem);
    setNewMenuItem({ ...newMenuItem, description: "", price: "", number: "" });
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
    menuItems: state.menuItems
  };
}

const mapDispatchToProps = {
  loadMenuItems: menuItemActions.loadMenuItems,
  addMenuItem: menuItemActions.addMenuItem
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ManagerPage);
