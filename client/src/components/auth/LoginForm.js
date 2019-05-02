import React from "react";
import Input from "../common/Input";
import PropTypes from "prop-types";

const LoginForm = ({ credentials, onSubmit, onChange, isLoggingIn }) => {
  return (
    <form onSubmit={onSubmit}>
      <Input
        value={credentials.email}
        onChange={onChange}
        name="email"
        label="Email"
        placeholder="Enter your email..."
      />
      <Input
        value={credentials.password}
        onChange={onChange}
        name="password"
        isPassword
        label="Password"
      />
      <input
        type="submit"
        className="btn btn-primary"
        disabled={isLoggingIn}
        value={isLoggingIn ? "Logging in..." : "Login"}
      />
    </form>
  );
};

LoginForm.propTypes = {
  credentials: PropTypes.object.isRequired,
  onSubmit: PropTypes.func.isRequired,
  onChange: PropTypes.func.isRequired,
  isLoggingIn: PropTypes.bool.isRequired
};

export default LoginForm;
