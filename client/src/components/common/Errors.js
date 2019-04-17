import React from "react";
import PropTypes from "prop-types";

const Errors = ({ messages }) => {
  return (
    <div className="alert alert-danger">
      <ul className="list-unstyled">
        {messages.map(message => {
          return <li key={message}>{message}</li>;
        })}
      </ul>
    </div>
  );
};

Errors.propTypes = {
  messages: PropTypes.array.isRequired
};

export default Errors;
