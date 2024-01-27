import React from "react";
import "../GamePage.css";
import { useParams } from "react-router-dom";
import { Message } from "./Message";

export const ChatMsgWindow = ({ chat }) => {
  const { gameId } = useParams();
  return (
    <div className="message-block">
      {chat.map((m, id) => (
        <Message
          key={id * Math.random()}
          user={m.username}
          message={m.message}
        />
      ))}
    </div>
  );
};
