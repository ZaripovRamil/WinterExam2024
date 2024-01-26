import React from "react";
import "./GamePage.css";
import { useParams } from "react-router-dom";
import { GameWindow } from "./GameWindow";
import { GameWindowTest } from "./GameWindowTest";
import { ChatWindow } from "./ChatWindow";

export const GamePage = () => {
  const { gameId } = useParams();
  return (
    <>
      <main className="main-game">
        <GameWindowTest />
        <ChatWindow />
      </main>
    </>
  );
};
