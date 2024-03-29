import "./GamePage.css";
import { Link, useParams } from "react-router-dom";
import React, { useEffect, useState, useRef } from "react";
import { getFetcher } from "../axios/AxiosInstance";
import Ports from "../constants/Ports";
import { useNavigate } from "react-router-dom";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { JoinErrorState } from "./States/JoinErrorState";
import { WaitingForOpponentState } from "./States/WaitingForOpponentState";
import { EnableToJoinState } from "./States/EnableToJoinState";
import { PlayingTheGameState } from "./States/PlayingTheGameState";
import { WatchingTheGameState } from "./States/WatchingTheGameState";
import { WaitingForResultState } from "./States/WaitingForResultState";
import { GameFinishedState } from "./States/GameFinishedState";
import { WaitingForNewGameState } from "./States/WaitingForNewGameState";

const fetcher = getFetcher(Ports.WebApi);

const data = {
  id: "123456",
  gameState: {
    moves: {},
    winner: null,
  },
  players: [
    { id: "1113", raiting: 2, username: "kamilla" },
    { id: "1112", raiting: 2, username: "kamilla" },
  ],
  errorCode: 0,
};

export const GameWindowTest = () => {
  const [connection, setConnection] = useState(null);
  const navigate = useNavigate();
  const { gameId } = useParams();
  const [room, setRoom] = useState(data);
  const [isJoinError, setIsJoinError] = useState(false);
  const [isWaitingForOpponent, setIsWaitingForOpponent] = useState(false);
  const [isEnableToJoin, setIsEnableToJoin] = useState(false);
  const [isPlayingTheGame, setIsPlayingTheGame] = useState(false);
  const [isWatchingTheGame, setIsWatchingTheGame] = useState(false);
  const [isWaitingForResult, setIsWaitingForResult] = useState(false);
  const [isGameFinished, setIsGameFinished] = useState(false);
  const [isWaitingForNewGame, setIsWaitingForNewGame] = useState(false);

  useEffect(() => {
    // const newConnection = new HubConnectionBuilder()
    //   .withUrl("https://localhost:5001/hubs/room")
    //   .withAutomaticReconnect()
    //   .build();

    // setConnection(newConnection);
    changeStates(room);
    // fetchData();
  }, []);

  useEffect(() => {
    // if (connection) {
    //   connection
    //     .start()
    //     .then((result) => {
    //       console.log("Connected!");
    //       try {
    //         connection.send("Enter", gameId);
    //       } catch (e) {
    //         console.log(e);
    //       }
    //       connection.on("Receive", (roomDto) => {
    //         if (validateErrorCode(roomDto.errorCode)) {
    //           setRoom(roomDto);
    //           changeStates(roomDto);
    //         }
    //       });
    //     })
    //     .catch((e) => console.log("Connection failed: ", e));
    // }
  }, [connection]);

  useEffect(() => {}, [isJoinError, isWaitingForResult, room]);

  useEffect(() => {
    //отправить сообщение на хаб, что сменила комнату
  }, [gameId]);

  useEffect(() => {
    alert("new game started");
    // if (connection.connectionStarted) {
    //   try {
    //     connection.send("StartNewGame", gameId);
    //   } catch (e) {
    //     console.log(e);
    //   }
    // } else {
    //   alert("No connection to server yet.");
    // }
  }, [isWaitingForNewGame]);

  function changeStates(roomDto) {
    const isGameStarted = roomDto.players.length === 2;
    const isPlayer = roomDto.players.find(
      (player) => player.id === localStorage.getItem("userId")
    );
    const isFinished = roomDto.gameState.winner !== null;

    //Перезапуск игры
    if (!isFinished && isWaitingForNewGame) isWaitingForNewGame(false);

    if (isFinished) {
      setIsGameFinished(true);
      setIsWaitingForResult(false);
    } else setIsGameFinished(false);

    isGameStarted && isPlayer && !isFinished
      ? setIsPlayingTheGame(true)
      : setIsPlayingTheGame(false);

    isGameStarted && !isPlayer && !isFinished
      ? setIsWatchingTheGame(true)
      : setIsWatchingTheGame(false);

    !isGameStarted && isPlayer && !isFinished
      ? setIsWaitingForOpponent(true)
      : setIsWaitingForOpponent(false);

    !isGameStarted && !isPlayer && !isFinished
      ? setIsEnableToJoin(true)
      : setIsEnableToJoin(false);
  }

  function validateErrorCode(errorCode) {
    if (errorCode === 401) navigate("/authorize");
    else if (errorCode === 404) navigate("/notFound");
    else if (errorCode === 400) {
      setIsJoinError(true);
      return false;
    }
    return true;
  }

  const joinToGame = async () => {
    setIsJoinError(true);
    // if (connection.connectionStarted) {
    //   try {
    //     await connection.send("JoinToGame", room, gameId);
    //   } catch (e) {
    //     console.log(e);
    //   }
    // } else {
    //   alert("No connection to server yet.");
    // }
  };
  const sendDataToHub = async (move) => {
    console.log(move);
  };

  return (
    <>
      <div className="window gameWindow">
        {isJoinError && <JoinErrorState />}
        {isWaitingForOpponent && <WaitingForOpponentState />}
        {isEnableToJoin && <EnableToJoinState joinToGame={joinToGame} />}
        {isPlayingTheGame && (
          <PlayingTheGameState
            sendDataToHub={sendDataToHub}
            setIsWaitingForResult={setIsWaitingForResult}
            setIsPlayingTheGame={setIsPlayingTheGame}
          />
        )}
        {isWatchingTheGame && <WatchingTheGameState room={room} />}
        {isWaitingForResult && <WaitingForResultState />}
        {isGameFinished && (
          <GameFinishedState
            gameState={room.gameState}
            setIsWaitingForNewGame={setIsWaitingForNewGame}
            setIsGameFinished={setIsGameFinished}
          />
        )}
        {isWaitingForNewGame && <WaitingForNewGameState />}
      </div>
    </>
  );
};
