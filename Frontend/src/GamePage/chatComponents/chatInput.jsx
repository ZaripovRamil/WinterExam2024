import React, { useState } from "react";
import "../GamePage.css";
import { useParams } from "react-router-dom";

export const ChatInput = ({ sendMessage }) => {
  const { gameId } = useParams();
  const [message, setMessage] = useState("");

  const onSubmit = (e) => {
    e.preventDefault();
    let mes = message.trim();
    setMessage(mes);
    const isMessageProvided = mes && mes !== "";

    if (isMessageProvided) {
      sendMessage(message);
      setMessage("");
    } else {
      alert("Please insert a message.");
    }
  };

  const onMessageUpdate = (e) => {
    setMessage(e.target.value);
  };

  return (
    <form className="sentMessage-form" onSubmit={onSubmit}>
      <textarea
        id="message"
        className="sentMessage-input"
        name="message"
        value={message}
        onChange={onMessageUpdate}
        placeholder="Message to support"
      />
      <input type="submit" className="sentMessage-btn" value={"Sent"} />
    </form>
  );
};
