import React, { Component } from 'react';
import ReactDom from 'react-dom';
import Board from './board';
import './style.scss';
const signalR = require("@microsoft/signalr");


class Main extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userBoard: '',
            userName: '',
            boards: '',
            mode: 'outBoard',
            playing: false,
            connection: null,
            userTurn: false,
        }
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleCreateBoard = this.handleCreateBoard.bind(this);
    };

    componentDidMount() {
        let connection = new signalR.HubConnectionBuilder().withUrl("/gamethread").build();
        connection.start()
            .then(() => {
                console.log('Connection started!');
                connection.invoke("GetBoards");
            })
            .catch(err => console.log('Error while establishing connection :( '));

        connection.on("GetBoards", boards => {
            this.setState({ boards });
            if (this.state.mode == "inBoard") {
                let userBoard = boards.find((element) => {
                    return (element.id === this.state.userBoard.id)
                })
                let playing = false;
                if (userBoard.user1Name && userBoard.user2Name) {
                    playing = true;
                }
                this.setState({ userBoard: userBoard, playing: playing });
            }
        })

        connection.on("GetBoard", board => {
            console.log(board);
            this.setState({ userBoard: board });
        })
        connection.on("JoinedBoard", (board, userTurn) => {
            this.setState({ mode: "inBoard", userBoard: board, userTurn: userTurn })
        })

        this.setState({ connection })
        this.setupBeforeUnloadListener();
    }

    setupInBoardHandle() {
        let connection = this.state.connection;
    }

    setupBeforeUnloadListener() {
        window.addEventListener("beforeunload", (ev) => {
            this.state.connection.invoke("Disconnect", this.state.userBoard.id);
        });
    };

    handleNameChange(e) {
        this.setState({
            userName: e.target.value.toUpperCase(),
        })
    }

    handleCreateBoard() {
        if (this.state.userName) {
            this.state.connection.invoke("CreateBoard", this.state.userName);
        } else {
            alert("Type your name");
        }
    }
    handleJoinBoard(boardId) {
        if (this.state.userName) {
            this.state.connection.invoke("JoinBoard", boardId, this.state.userName);
        } else {
            alert("Type your name");
        }
    }
    render() {
        if (this.state.mode == 'outBoard') {
            return (
                <>
                    <div className="row justify-content-center mb-5">
                        <div class="form-group col-3">
                            <label className="mb-2" htmlFor="name">Type your name to start</label>
                            <input type="text" value={this.state.userName} className="form-control" id="name" onChange={this.handleNameChange} required />
                        </div>
                    </div>
                    <div className="row justify-content-center">
                        <div className="col-2">
                            <button className="btn btn-primary btn-lg" onClick={this.handleCreateBoard}>New Board</button> Or
                        </div>
                        {this.state.boards &&
                            this.state.boards.map(value => {
                                // Have one player on board
                                if ((value.user1Name && !value.user2Name)
                                    || (!value.user1Name && value.user2Name))
                                    return (
                                        <div className="col-2">
                                            <div className="cell-board">
                                                <button className="btn btn-danger" onClick={this.handleJoinBoard.bind(this, value.id)}>Join</button>
                                            </div>
                                            <label>{value.user1Name}</label>
                                        </div>)
                                else return (
                                    <div className="col-2">
                                        <div className="cell-board">
                                            <strong>Playing</strong>
                                        </div>
                                        <label>{value.user1Name} vs {value.user2Name}
                                        </label>
                                    </div>
                                )
                            })}
                    </div>
                </>
            )
        }
        else if (this.state.mode == 'inBoard') {
            let color, opponentName;
            if (this.state.userBoard.user1Name == this.state.userName) {
                color = this.state.userBoard.user1Color == 0 ? 'black' : 'white';
                opponentName = this.state.userBoard.user2Name;
            } else if (this.state.userBoard.user2Name == this.state.userName) {
                color = this.state.userBoard.user2Color == 0 ? 'black' : 'white';
                opponentName = opponentName = this.state.userBoard.user1Name;
            }
            return (
                <div>
                    <Board
                        userName={this.state.userName}
                        userTurn={this.state.userTurn}
                        boardId={this.state.userBoard.id}
                        userColor={color}
                        playingMode={this.state.playing}
                        connection={this.state.connection} />
                </div>
            )
        }
    }
}


ReactDom.render(<Main />, document.getElementById('board'));