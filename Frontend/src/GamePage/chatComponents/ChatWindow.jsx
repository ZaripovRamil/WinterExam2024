import React from "react";
import "../GamePage.css";
import { useParams } from "react-router-dom";
import { ChatMsgWindow } from "./chatMsgWindow";
import { ChatInput } from "./chatInput";
import { sendMessage } from "@microsoft/signalr/dist/esm/Utils";

export const ChatWindow = ({ chat, sendMessage }) => {
  const { gameId } = useParams();
  return (
    <>
      <div className="window chatWindow">
        <ChatMsgWindow chat={chat} />
        <ChatInput sendMessage={sendMessage} />
      </div>
    </>
  );
};
