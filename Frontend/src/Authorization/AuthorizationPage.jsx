import React from "react";
import AuthForm from "./components/AuthorizationForm";
import "./style.css";

export const AuthorizationPage = () => {
  return (
    <>
      <main className="main-auth">
        <AuthForm />
      </main>
    </>
  );
};
