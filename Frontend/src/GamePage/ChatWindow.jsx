import React from "react";
import "./GamePage.css";
import { useParams } from "react-router-dom";

export const ChatWindow = () => {
  const { gameId } = useParams();
  return (
    <>
      <div className="window chatWindow"></div>
    </>
  );
};
