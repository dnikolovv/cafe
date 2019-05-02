import React from "react";

const MenuItemsList = ({ menuItems }) => {
  return (
    <ul className="list-group">
      {menuItems.map(item => {
        return (
          <li key={item.number} className="list-group-item">
            {item.number}. {item.description} - ${item.price}
          </li>
        );
      })}
    </ul>
  );
};

export default MenuItemsList;
