import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./MainPage.css";
import { getFetcher } from "../axios/AxiosInstance";
import Ports from "../constants/Ports";
import { useNavigate } from "react-router-dom";

const fetcher = getFetcher(Ports.WebApi);
const LIMIT = 4;

const GameTable = () => {
  const navigate = useNavigate();
  const [count, setCount] = useState(1);
  const [games, setGames] = useState([]);

  useEffect(() => {
    try {
      fetchData(count);
    } catch {
      navigate("/authorize");
    }
  }, [games]);

  const fetchData = (page) => {
    const authToken = localStorage.getItem("access-token") ?? "";
    fetcher
      .get("games", { params: { limit: LIMIT * page, page: 1 } })
      .then((res) => {
        console.log(res);
        setGames(res.data);
      })
      .catch((err) => {
        if (err.response && err.response.status === 401) {
          navigate("/authorize");
        } 
        else navigate("/notFound");
      });
  };

  const handleShowMorePosts = () => {
    const newCount = count + 1;
    setCount(newCount);
    fetchData(newCount);
  };

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

        {games.map((val, key) => {
          const path = "/games/" + val.id;
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
      <button onClick={handleShowMorePosts}>Load more</button>
    </div>
  );
};

export default GameTable;
