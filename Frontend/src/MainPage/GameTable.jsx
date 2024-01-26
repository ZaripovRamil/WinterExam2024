import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./MainPage.css";
import { getFetcher } from "../axios/AxiosInstance";
import Ports from "../constants/Ports";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const fetcher = getFetcher(Ports.WebApi);
const LIMIT = 10;

const data = [
  {
    ownerName: "Anom",
    gameCreated: "25.01.2024",
    gameId: "123456",
    gameStarted: true,
  },
  {
    ownerName: "Kamilla",
    gameCreated: "25.01.2024",
    gameId: "123456",
    gameStarted: false,
  },
  {
    ownerName: "Anom",
    gameCreated: "25.01.2024",
    gameId: "123456",
    gameStarted: true,
  },
];
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
    // await axios
    fetcher
      .get("games", { params: { limit: LIMIT, page: page } })
      // .get(`https://localhost:${Ports.WebApi}/games`, {
      //   headers: { Authorization: `Bearer ${authToken}` },
      //   params: { limit: LIMIT, page: page },
      // })
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

  // const handlePrevPage = () => {
  //   if (offset <= 0) {
  //     return;
  //   }

  //   setOffset(offset - LIMIT);
  // };

  // const handleNextPage = () => {
  //   if (offset + LIMIT >= totalCount) {
  //     return;
  //   }

  //   setOffset(offset + LIMIT);
  // };

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
              <td>{val.ownerName}</td>
              <td>{val.gameCreated}</td>
              <td>{val.gameId}</td>
              <td>{val.gameStarted ? "Ожидает" : "В разгаре"}</td>
              <td>
                <Link to={path} className="raw">
                  Войти
                </Link>
              </td>
            </tr>
          );
        })}
      </table>
      {/* Pagination controls
      <div className="paginationButtons">
        <button onClick={handlePrevPage}>Назад</button>
        <button onClick={handleNextPage}>Вперед</button>
      </div> */}
    </div>
  );
};

export default GameTable;
