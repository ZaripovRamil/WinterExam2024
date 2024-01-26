import React, { useState } from "react";
import { Link } from "react-router-dom";
import "./Authorization.css";
import Ports from "../../constants/Ports";
import AuthorizationErrors from "../../constants/AuthorizationErrors";
import { getFetcher } from "../../axios/AxiosInstance";
import { useNavigate } from "react-router";

const fetcher = getFetcher(Ports.WebApi);

const AuthForm = () => {
  const navigate = useNavigate();
  const [credentials, setCredentials] = useState({
    username: "",
    password: "",
  });
  const [loginError, setLoginError] = useState();
  const [passwordError, setPasswordError] = useState();

  const handleSubmitForm = (event) => {
    event.preventDefault();
    fetcher
      .post("signIn", credentials)
      .then((res) => handleAuthorizationInfo(res.data))
      .catch((err) => console.log(err));
  };

  const handleAuthorizationInfo = (data) => {
    if (data && data.isSuccessful) {
      localStorage.setItem("access-token", data.token);
      localStorage.setItem("userId", data.userId);
      navigate("/games");
    } else {
      setLoginError(AuthorizationErrors.wrongLoginOrPassword);
      setPasswordError(AuthorizationErrors.wrongLoginOrPassword);
    }
  };

  const updateCredentials = (name, value) => {
    credentials[name] = value;
    setCredentials({ ...credentials });
  };

  return (
    <form className="auth-form" onSubmit={handleSubmitForm}>
      <div className="auth-header">
        <Link to="/register">
          <span className="auth-header-link">Регистрация</span>
        </Link>
        <span className="auth-header-main"> / Вход</span>
      </div>

      <div className="credentials-input">
        <div>
          <div className="error-text error-login">{loginError}</div>
          <input
            type="text"
            placeholder="Введите имя"
            onChange={(e) =>
              updateCredentials("username", e.target.value.trim())
            }
          />
        </div>

        <div>
          <div className="error-text error-password">{passwordError}</div>
          <input
            type="password"
            placeholder="Введите пароль"
            onChange={(e) =>
              updateCredentials("password", e.target.value.trim())
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

export default AuthForm;
