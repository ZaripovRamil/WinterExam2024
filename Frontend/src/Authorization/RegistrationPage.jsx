import React from "react";
import "./style.css";
import { RegisterForm } from "./components/RegistrationForm";

export const RegistrationPage = () => {
  return (
    <>
      <main className="main-auth">
        <RegisterForm />
      </main>
    </>
  );
};
