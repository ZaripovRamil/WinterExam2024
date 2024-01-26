import "./App.css";
import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthorizationPage } from "./Authorization/AuthorizationPage";
import { RegistrationPage } from "./Authorization/RegistrationPage";
import { PageBuilder } from "./PageBuilder/PageBuilder";
import { MainPage } from "./MainPage/MainPage";
import GoAwayPage from "./GoAwayPage";
import { GamePage } from "./GamePage/GamePage";
import { RaitingPage } from "./RaitingPage/RaitingPage";
import { NotFoundPage } from "./NotFoundPage/NotFoundPage";

function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route
            path="/authorize"
            element={<PageBuilder component={<AuthorizationPage />} />}
          />
          <Route
            path="/register"
            element={<PageBuilder component={<RegistrationPage />} />}
          />
          <Route
            path="/games"
            element={<PageBuilder component={<MainPage />} />}
          />
          <Route
            path="/games/:gameId"
            element={<PageBuilder component={<GamePage />} />}
          />
          <Route
            path="/raiting"
            element={<PageBuilder component={<RaitingPage />} />}
          />
          <Route
            path="/notFound"
            element={<PageBuilder component={<NotFoundPage />} />}
          />
          <Route path="*" element={<GoAwayPage />} />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
