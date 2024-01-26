import "./RaitingPage.css";
import React, { useEffect, useState } from "react";
import { getFetcher } from "../axios/AxiosInstance";
import Ports from "../constants/Ports";
import { useNavigate } from "react-router-dom";

const fetcher = getFetcher(Ports.WebApi);

const data = [
  {
    username: "Anom",
    raiting: 0,
  },
  {
    username: "Anom",
    raiting: 0,
  },
  {
    username: "Anom",
    raiting: 0,
  },
];

export const RaitingPage = () => {
  const navigate = useNavigate();
  const [rating, setRating] = useState([]);

  useEffect(() => {
    fetchRating();
  }, []);

  const fetchRating = async () => {
    fetcher
      .get("rating")
      .then((data) => {
        console.log(data);
        setRating(data.data);
      })
      .catch((err) => {
        if (err.response.status === 401) navigate("/authorize");
      });
  };

  return (
    <>
      <main className="main-raiting">
        <div className="back" onClick={() => navigate(-1)}>
          Назад
        </div>
        <div className="raingHeader">Рейтинг</div>
        <div className="raitingTable">
          <table>
            <tr>
              <th>Username</th>
              <th>Рейтинг</th>
            </tr>
            {rating.map((val, key) => {
              return (
                <tr key={key}>
                  <td>{val.userName}</td>
                  <td>{val.rating}</td>
                </tr>
              );
            })}
          </table>
        </div>
      </main>
    </>
  );
};
