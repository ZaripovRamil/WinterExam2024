import React from "react";
import { Header } from "../Header/Header";
import "./PageBuilder.css";

export const PageBuilder = ({ component }) => {
  return (
    <>
      <Header />
      <div className="content">{React.cloneElement(component)}</div>
    </>
  );
};
