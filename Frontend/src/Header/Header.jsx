import "./Header.css";
import { Link } from "react-router-dom";

export const Header = () => {
  return (
    <header className="header">
      <div className="header-logo">
        <Link to="/games">
          <div>Камень-ножницы-бумага</div>
        </Link>
      </div>

      <div className="header-nav">
        <Link to="/games">
          <div className="text-wrapper-3">Игры</div>
        </Link>
        <Link to="/raiting">
          <div className="text-wrapper-2">Рейтинг</div>
        </Link>
        <Link
          to={localStorage.getItem("access-token") ? "/logout" : "/authorize"}
        >
          {localStorage.getItem("access-token") ? (
            <div className="div"></div>
          ) : (
            <div className="div">Вход</div>
          )}
        </Link>
      </div>
    </header>
  );
};
