import React from "react";

export const WatchingTheGameState = ({ room }) => {
  return (
    <>
      <div className="centerEl">
        Игроки {room.players[0].username} и {room.players[1].username} делают
        выбор
      </div>
      ;
    </>
  );
};
