import React, { useState } from "react";
import LoginForm from "./LoginForm";
import Errors from "../common/Errors";
import PropTypes from "prop-types";
import * as authActions from "../../redux/actions/authActions";
import { connect } from "react-redux";

const LoginPage = ({ login, history }) => {
  const [credentials, setCredentials] = useState({ email: "", password: "" });
  const [isLoggingIn, setLoggingIn] = useState(false);
  const [errors, setErrors] = useState([]);

  const handleSubmit = async event => {
    event.preventDefault();
    setLoggingIn(true);

    login(credentials)
      .then(() => {
        history.push("/");
      })
      .catch(() => {
        setLoggingIn(false);
        setErrors(["Invalid credentials."]);
      });
  };

  const handleChange = event => {
    const { name, value } = event.target;
    setCredentials(prevCredentials => {
      return { ...prevCredentials, [name]: value };
    });
  };

  return (
    <div>
      {errors.length > 0 && <Errors messages={errors} />}

      <LoginForm
        credentials={credentials}
        onSubmit={handleSubmit}
        onChange={handleChange}
        isLoggingIn={isLoggingIn}
      />
    </div>
  );
};

LoginPage.propTypes = {
  login: PropTypes.func.isRequired,
  history: PropTypes.object.isRequired
};

const mapDispatchToProps = {
  login: authActions.login
};

export default connect(
  null,
  mapDispatchToProps
)(LoginPage);
