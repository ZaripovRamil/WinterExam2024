import "./GamePage.css";
import { Link, useParams } from "react-router-dom";
import React, { useEffect, useState, useRef } from "react";
import { getFetcher } from "../axios/AxiosInstance";
import Ports from "../constants/Ports";
import { useNavigate } from "react-router-dom";
import { HubConnectionBuilder, HttpTransportType } from "@microsoft/signalr";
import { JoinErrorState } from "./States/JoinErrorState";
import { WaitingForOpponentState } from "./States/WaitingForOpponentState";
import { EnableToJoinState } from "./States/EnableToJoinState";
import { PlayingTheGameState } from "./States/PlayingTheGameState";
import { WatchingTheGameState } from "./States/WatchingTheGameState";
import { WaitingForResultState } from "./States/WaitingForResultState";
import { GameFinishedState } from "./States/GameFinishedState";
import { WaitingForNewGameState } from "./States/WaitingForNewGameState";

const fetcher = getFetcher(Ports.WebApi);

export const GameWindow = () => {
  const [connection, setConnection] = useState(null);
  const navigate = useNavigate();
  const { gameId } = useParams();
  const [room, setRoom] = useState({});
  const [isJoinError, setIsJoinError] = useState(false);
  const [isWaitingForOpponent, setIsWaitingForOpponent] = useState(false);
  const [isEnableToJoin, setIsEnableToJoin] = useState(false);
  const [isPlayingTheGame, setIsPlayingTheGame] = useState(false);
  const [isWatchingTheGame, setIsWatchingTheGame] = useState(false);
  const [isWaitingForResult, setIsWaitingForResult] = useState(false);
  const [isGameFinished, setIsGameFinished] = useState(false);
  const [isWaitingForNewGame, setIsWaitingForNewGame] = useState(false);

  useEffect(() => {
    const authToken = localStorage.getItem("access-token") ?? "";
    const newConnection = new HubConnectionBuilder()
      .withUrl(`https://localhost:${Ports.WebApi}/room`, {
        transport: HttpTransportType.WebSockets,
        skipNegotiation: true,
        accessTokenFactory: () => `${authToken}`,
      })
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    // fetchData();
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then((result) => {
          console.log("Connected!");

          try {
            console.log(gameId);
            connection.send("Enter", gameId);
          } catch (e) {
            console.log(e);
          }
        })
        .catch((e) => console.log("Connection failed: ", e));

      connection.on("Receive", (roomDto) => {
        console.log(roomDto);
        if (validateStatusCode(roomDto.statusCode)) {
          changeStates(roomDto);
          setRoom(roomDto);
        }
      });
      connection.on("ReopenRoom", (roomDto) => {
        if (validateStatusCode(roomDto.statusCode)) {
          setRoom(roomDto);
          changeStates(roomDto);
        }
      });
    }
  }, [connection]);

  useEffect(() => {}, [isJoinError, room]);

  useEffect(() => {
    if (connection && connection.connectionStarted) {
      try {
        connection.send("ExitTheGame", gameId);
      } catch (e) {
        console.log(e);
      }
    }
  }, [gameId]);

  function startNewGame() {
    if (connection && connection.connectionStarted) {
      try {
        connection.send("StartNewGame", gameId);
      } catch (e) {
        console.log(e);
      }
    }
  }

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

  function validateStatusCode(statusCode) {
    if (statusCode === 401) navigate("/authorize");
    else if (statusCode === 404) navigate("/notFound");
    else if (statusCode === 400) {
      setIsJoinError(true);
      return false;
    }
    return true;
  }

  const fetchData = async () => {
    // fetcher
    //   .get(`games/{gameId}`)
    //   .then((data) => {
    //     setGame(data.data);
    //   })
    //   .catch((err) => {
    //     if (err.response.status === 401) navigate("/authorize");
    //  if (err.response.status === 404) navigate("/notFound");
    //   });
  };

  const joinToGame = async () => {
    if (connection.connectionStarted) {
      try {
        await connection.send("JoinToGame", room, gameId);
      } catch (e) {
        console.log(e);
      }
    } else {
      alert("No connection to server yet.");
    }
    // fetcher
    //   .post(`games/{gameId}`)
    //   .then((data) => {
    //     setGame(data.data);
    //   })
    //   .catch((err) => {
    //     if (err.response.status === 401) navigate("/authorize");
    //  if (err.response.status === 404) navigate("/notFound");
    //if (err.response.status === 400) setIsJoinError(true);
  };

  const sendDataToHub = async (move) => {
    if (connection.connectionStarted) {
      try {
        await connection.send("Move", move, gameId);
      } catch (e) {
        console.log(e);
      }
    } else {
      alert("No connection to server yet.");
    }
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
            startNewGame={startNewGame}
          />
        )}
        {isWaitingForNewGame && <WaitingForNewGameState />}
      </div>
    </>
  );
};
