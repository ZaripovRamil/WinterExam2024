import React, { useEffect, useState } from "react";

// 0 - None;
// 1 - Rock;
// 2 - Scissors;
// 3 - Paper;

export const PlayingTheGameState = ({
  sendDataToHub,
  setIsWaitingForResult,
  setIsPlayingTheGame,
}) => {
  const [counter, setCounter] = useState(10);
  const [moveNumb, setMoveNumb] = useState(0);

  useEffect(() => {
    if (counter === 0) {
      setIsWaitingForResult(true);
      setIsPlayingTheGame(false);
      sendDataToHub(moveNumb);
    }
    counter > 0 && setTimeout(() => setCounter(counter - 1), 1000);
  }, [counter]);

  return (
    <>
      <div className="gameField">
        <div className="timer"> {counter}</div>
        <div className="chooseTitle">Выберите действие</div>
        <div className="moves">
          <button className="move rock" onClick={() => setMoveNumb(1)} />
          <button className="move scissors" onClick={() => setMoveNumb(2)} />
          <button className="move paper" onClick={() => setMoveNumb(3)} />
        </div>
      </div>
    </>
  );
};
