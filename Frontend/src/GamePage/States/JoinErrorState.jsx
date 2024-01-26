import React from "react";
import { Link, useParams } from "react-router-dom";

export const JoinErrorState = () => {
  return (
    <>
      <div className="centerEl">
        <div> Ваш рейтинг меньше требуемого.</div>
        <Link to="/main">Вернуться на главную страницу</Link>
      </div>
    </>
  );
};
