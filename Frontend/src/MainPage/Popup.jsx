import React, { useState } from "react";
import { getFetcher } from "../axios/AxiosInstance";
import Ports from "../constants/Ports";
import { useNavigate } from "react-router";

const fetcher = getFetcher(Ports.WebApi);

const Popup = (props) => {
  const navigate = useNavigate();
  const [maxRating, setMaxRating] = useState(0);

  const handleSubmitForm = (event) => {
    event.preventDefault();
    console.log(maxRating);
    fetcher
      .post("games", { maxRating: maxRating })
      .then((res) => {
        handleResult(res.data);
      })
      .catch((err) => {
        if (err.response.status === 401) navigate("/authorize");
      });
  };

  const handleResult = (data) => {
    console.log("gameid", data.gameId);
    if (data) {
      navigate(`/games/${data.gameId}`);
    }
  };

  return (
    <div className="popup-box">
      <div className="box">
        <div className="close-icon" onClick={props.handleClose}>
          x
        </div>

        <form className="create-form" onSubmit={handleSubmitForm}>
          <div>
            <input
              type="number"
              placeholder="Max raiting"
              onChange={(e) => setMaxRating(e.target.value)}
            />
          </div>
          <div className="submit-btn">
            <input type="submit" value="Создать" />
          </div>
        </form>
      </div>
    </div>
  );
};

export default Popup;
