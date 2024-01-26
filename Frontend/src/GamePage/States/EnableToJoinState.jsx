import React from "react";

export const EnableToJoinState = ({ joinToGame }) => {
  return (
    <>
      <button className="centerEl connectBtn" onClick={joinToGame}>
        Подключиться
      </button>
    </>
  );
};
