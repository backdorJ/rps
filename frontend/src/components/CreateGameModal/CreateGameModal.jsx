import React, { useState, useEffect } from "react";
import "./CreateGameModal.css";
import {useSignalR} from "../../contexts/signalR";
import {createGame, joinGame} from "../../http/gameHttp";
import {useNavigate} from "react-router-dom";

const CreateGameModal = ({ isOpen, onClose, onSubmit }) => {
    const [roomName, setRoomName] = useState("");
    const { connection, connected, startConnection } = useSignalR()
    const [maxRating, setMaxRating] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        if (!connected) {
            startConnection();
        }
    }, []);
    
    useEffect(() => {
        if (connection)
        {
            connection.on("JoinedGameInfo", response => {
                console.log("JoinedGameInfo")
                console.log(response)
            })
        }
    }, [connection]);

    if (!isOpen) return null;

    const handleSubmit = () => {
        const request = {
            roomName,
            maxRating
        }

        createGame(request).then((response) => {
            if (response.status === 200) {
                connection.invoke("JoinRoom", response.data.id)
                navigate("game/" + response.data.id);
            }
        })

        onClose();
    };

    return (
        <div className="modal-overlay">
            <div className="modal">
                <h2>Create Game</h2>
                <form onSubmit={handleSubmit}>
                    <label>
                        Room Name:
                        <input
                            type="text"
                            value={roomName}
                            onChange={(e) => setRoomName(e.target.value)}
                            required
                        />
                    </label>
                    <label>
                        Max Rating:
                        <input
                            type="number"
                            value={maxRating}
                            onChange={(e) => setMaxRating(e.target.value)}
                            required
                        />
                    </label>
                    <div className="modal-buttons">
                        <button onClick={handleSubmit} className="btn-create">Create</button>
                        <button type="button" className="btn-cancel" onClick={onClose}>Cancel</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default CreateGameModal;
