import React from "react";
import PropTypes from "prop-types";

const Input = ({
  name,
  label,
  onChange,
  placeholder,
  value,
  error,
  isPassword,
  isNumber,
  isReadonly
}) => {
  let wrapperClass = "form-group";
  if (error && error.length > 0) {
    wrapperClass += " has-error";
  }

  const type = isPassword ? "password" : isNumber ? "number" : "text";

  return (
    <div className={wrapperClass}>
      <label htmlFor={name}>{label}</label>
      <div className="field">
        <input
          type={type}
          name={name}
          className="form-control"
          placeholder={placeholder}
          value={value}
          onChange={onChange}
          readOnly={isReadonly}
        />
        {error && <div className="alert alert-danger">{error}</div>}
      </div>
    </div>
  );
};

Input.propTypes = {
  name: PropTypes.string.isRequired,
  label: PropTypes.string.isRequired,
  onChange: PropTypes.func.isRequired,
  placeholder: PropTypes.string,
  value: PropTypes.string,
  error: PropTypes.string,
  isPassword: PropTypes.bool.isRequired,
  isReadonly: PropTypes.bool.isRequired,
  isNumber: PropTypes.bool
};

Input.defaultProps = {
  isPassword: false,
  isReadonly: false
};

export default Input;
