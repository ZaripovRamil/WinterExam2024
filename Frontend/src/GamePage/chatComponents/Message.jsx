import React from "react";
import "../GamePage.css";
import { useParams } from "react-router-dom";

export const Message = (props) => {
  return (
    <div className={"message-block message-block-no-owner"}>
      <p>
        <strong>{props.user}</strong>
      </p>
      <div className={"message "}>
        <p className={"message-text"}>{props.message}</p>
      </div>
    </div>
  );
};
