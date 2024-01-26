import "./Authorization.css";
import React, { useState } from "react";
import { Link } from "react-router-dom";
import { getFetcher } from "../../axios/AxiosInstance";
import Ports from "../../constants/Ports";
import AuthorizationErrors from "../../constants/AuthorizationErrors";
import { useNavigate } from "react-router";

const fetcher = getFetcher(Ports.WebApi);

export const RegisterForm = () => {
  const navigate = useNavigate();
  const [credentials, setCredentials] = useState({
    username: "",
    password: "",
    repeatPassword: "",
  });
  const [errors, setErrors] = useState({
    username: "",
    password: "",
  });

  const resetErrors = () => {
    errors.username = "";
    errors.password = "";
    setErrors({ ...errors });
  };

  const handleSubmitForm = (event) => {
    event.preventDefault();
    resetErrors();
    if (!validateCredentials()) {
      return;
    }
    const sendData = {
      username: credentials.username,
      password: credentials.password,
    };
    fetcher
      .post("SignUp", sendData)
      .then((res) => handleRegistrationResult(res.data))
      .catch((err) => console.log(err));
  };

  // while validating, manipulates credential errors states
  // returns false if at least one of credentials is wrong
  const validateCredentials = () => {
    let isValid = true;
    if (credentials.password !== credentials.repeatPassword) {
      isValid = false;
      errors.password = AuthorizationErrors.passwordsAreDifferent;
    }
    // email is validated by browser for now
    setErrors({ ...errors });
    return isValid;
  };

  const handleRegistrationResult = (data) => {
    if (data && data.isSuccessful) {
      navigate("/authorize");
    } else {
      errors.username = AuthorizationErrors.someError;
    }
    setErrors({ ...errors });
  };

  const updateCredentials = (name, value) => {
    credentials[name] = value.trim();
    setCredentials({ ...credentials });
  };

  return (
    <form className="register-form" onSubmit={handleSubmitForm}>
      <div className="register-header">
        <span className="register-header-main">Регистрация / </span>
        <Link to="/authorize">
          <span className="auth-header-link">Вход</span>
        </Link>
      </div>

      <div className="credentials-input">
        <div>
          <input
            required
            type="text"
            placeholder="Введите имя"
            onChange={(e) => updateCredentials("username", e.target.value)}
          />
          <div className="error-text error-name">{errors.username}</div>
        </div>

        <div>
          <input
            required
            type="password"
            placeholder="Введите пароль"
            onChange={(e) => updateCredentials("password", e.target.value)}
          />
          <div className="error-text error-password">{errors.password}</div>
        </div>
        <div>
          <input
            required
            type="password"
            placeholder="Повторите пароль"
            onChange={(e) =>
              updateCredentials("repeatPassword", e.target.value)
            }
          />
        </div>
      </div>
      <div className="submit-btn">
        <input type="submit" value="Войти" />
      </div>
    </form>
  );
};
