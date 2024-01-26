import React, { useEffect, useState } from "react";
import rock from "../media/rock.png";
import scissors from "../media/scissors.png";
import paper from "../media/paper.png";

const images = {
  1: rock,
  2: scissors,
  3: paper,
};

export const GameFinishedState = ({
  gameState,
  setIsWaitingForNewGame,
  setIsGameFinished,
}) => {
  const [counter, setCounter] = useState(100);

  useEffect(() => {
    if (counter === 0) {
      setIsWaitingForNewGame(true);
      setIsGameFinished(false);
    }
    counter > 0 && setTimeout(() => setCounter(counter - 1), 1000);
  }, [counter]);

  return (
    <>
      <div className="gameField">
        <div className="timer"> {counter}</div>
        {(() => {
          const moves = [];

          for (let key in gameState.moves) {
            if (gameState.moves[key] === 0)
              moves.push(
                <div className="centerEl">{`${key}`} пропустил(а) ход</div>
              );
            else {
              const imgSrc = images[gameState.moves[key]];
              console.log(gameState.moves[key]);
              const el = (
                <div className="centerEl">
                  {`${key}`} выбрал(а){" "}
                  <img src={imgSrc} className="playerMoveImg" />
                </div>
              );
              moves.push(el);
            }
          }

          return moves;
        })()}
        <div className="centerEl">Выиграл(а) {gameState.winner}</div>;
      </div>
    </>
  );
};
