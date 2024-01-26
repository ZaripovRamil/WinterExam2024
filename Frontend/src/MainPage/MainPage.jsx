import React, { useState } from "react";
import { Link } from "react-router-dom";
import "./MainPage.css";
import GameTable from "./GameTable";
import Popup from "./Popup";

export const MainPage = () => {
  const [isOpen, setIsOpen] = useState(false);

  const togglePopup = () => {
    setIsOpen(!isOpen);
  };
  return (
    <>
      <main className="main-main">
        <div className="buttons">
          <button className="createGameBtn" onClick={togglePopup}>
            Создать игру
          </button>
          {isOpen && <Popup handleClose={togglePopup} />}
          <Link to="/raiting">
            <button className="raitingBtn">Рейтинг</button>
          </Link>
        </div>
        <GameTable />
      </main>
    </>
  );
};
