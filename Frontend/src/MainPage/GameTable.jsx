import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./MainPage.css";
import { getFetcher } from "../axios/AxiosInstance";
import Ports from "../constants/Ports";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const fetcher = getFetcher(Ports.WebApi);
const LIMIT = 10;

const GameTable = () => {
  const navigate = useNavigate();
  const [page, setPage] = useState(1);
  const [games, setGames] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    fetchData(page);
  }, [page]);

  const fetchData = async (page) => {
    const authToken = localStorage.getItem("access-token") ?? "";
    fetcher
      .get("games", { params: { limit: LIMIT, page: page } })
      .then((data) => {
        setGames((prevItems) => [...prevItems, ...data.data]);
        setPage((prevPage) => prevPage + 1);
      })
      .catch((err) => {
        if (err.response.status === 401) {
          navigate("/authorize");
        }
      })
      .finally(setIsLoading(false));
  };

  const handleScroll = () => {
    if (
      window.innerHeight + document.documentElement.scrollTop !==
        document.documentElement.offsetHeight ||
      isLoading
    ) {
      return;
    }
    fetchData();
  };

  useEffect(() => {
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, [isLoading]);

  return (
    <div className="gameTable">
      <table>
        <tr>
          <th>Создатель</th>
          <th>Дата создания</th>
          <th>id игры</th>
          <th>Статус</th>
          <th></th>
        </tr>
        {console.log(games)}
        {games.map((val, key) => {
          const path = "/games/" + val.gameId;
          return (
            <tr key={key}>
              <td>{val.ownerName ? val.ownerName : "anonimous"}</td>
              <td>{val.created}</td>
              <td>{val.id.substring(0, 6)}</td>
              <td>{val.players.length == 2 ? "В разгаре" : "Ожидает"}</td>
              <td>
                <Link to={path} className="raw">
                  Войти
                </Link>
              </td>
            </tr>
          );
        })}
      </table>
    </div>
  );
};

export default GameTable;
